using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
	public class FactoryMethodComponentFactory<TComponent> : IComponentFactory where TComponent : class
	{
		private IComposer _composer;

		private readonly Func<IComposer, TComponent> _factoryMethod;
		private IComponentCache _componentCache;

		private readonly List<InitializationPointSpecification> _initializationPoints;
		private List<Action<IComposer, object>> _compositionNotificationMethods;
	    private ICompositionalQuery _componentCacheQuery;

		#region Constructors

		public FactoryMethodComponentFactory(Func<IComposer, TComponent> factoryMethod)
		{
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));

			_composer = null;
			_componentCache = null;
            _componentCacheQuery = null;
			_initializationPoints = new List<InitializationPointSpecification>();
			_compositionNotificationMethods = null;
		}

		#endregion

		#region IComponentFactory Members

		public bool ValidateContractType(Type contract)
		{
			return contract.IsAssignableFrom(typeof(TComponent));
		}

		public void Initialize(IComposer composer)
		{
			if (_composer != null)
				return;

			if (_factoryMethod == null)
				throw new InvalidOperationException("FactoryMethod is not specified.");

			if (!composer.Configuration.DisableAttributeChecking && !ComponentContextUtils.HasComponentAttribute(typeof(TComponent)))
				throw new CompositionException("The type '" + typeof(TComponent) +
				                               "' is not a component, but it is being registered as one. Only classes marked with [Component] attribute can be registered.");

			_composer = composer;
			CompleteConfiguration();
		}

		public IEnumerable<Type> GetContractTypes()
		{
			return ComponentContextUtils.FindContracts(typeof(TComponent));
		}

		public object GetComponentInstance(ContractIdentity contract, IEnumerable<ICompositionListener> listenerChain)
		{
			// Check if the factory is initialized

			if (_composer == null)
				throw new InvalidOperationException(
					"DelegateComponentFactory should be initialized before calling GetComponentInstance method.");

			if (_componentCache == null)
			{
				// If the component is not cached at all, then unique instances should
				// be created for each call, then locking does not help and just
				// degrades performance. So, create without locking.

				var newComponent = CreateComponent(contract, listenerChain);
				return NotifyRetrieved(newComponent.ComponentInstance,
				                       newComponent.OriginalComponentInstance,
				                       contract, listenerChain);
			}

			// Check if the component is cached, and ready to deliver

			var componentCacheEntry = _componentCache.GetFromCache(contract);
			if (componentCacheEntry != null)
			{
				return NotifyRetrieved(componentCacheEntry.ComponentInstance,
				                       componentCacheEntry.OriginalComponentInstance,
				                       contract, listenerChain);
			}

			// If the component is cached, then lock the component instance
			// to avoid creation of more than one cached components per config 
			// when called in concurrent manner

			lock (_componentCache.SynchronizationObject)
			{
				// Double-check the initialization to avoid rendezvouz

				componentCacheEntry = _componentCache.GetFromCache(contract);
				if (componentCacheEntry != null)
				{
					return NotifyRetrieved(componentCacheEntry.ComponentInstance,
					                       componentCacheEntry.OriginalComponentInstance,
					                       contract, listenerChain);
				}

				componentCacheEntry = CreateComponent(contract, listenerChain);
				return NotifyRetrieved(componentCacheEntry.ComponentInstance,
				                       componentCacheEntry.OriginalComponentInstance,
				                       contract, listenerChain);
			}
		}

		#endregion

		#region Override methods

		public override string ToString()
		{
			return _factoryMethod?.ToString() ?? base.ToString();
		}

		#endregion

		#region Public properties

		public Func<IComposer, TComponent> FactoryMethod => _factoryMethod;

		public List<InitializationPointSpecification> InitializationPoints
		{
			get
			{
				if (_composer != null)
					throw new InvalidOperationException("Cannot access InitializationPoints when the factory is initialized.");

				return _initializationPoints;
			}
		}

	    public List<Action<IComposer, object>> CompositionNotificationMethods
	    {
	        get
	        {
	            if (_composer != null)
	                throw new InvalidOperationException("Cannot access CompositionNotificationMethods when the factory is initialized.");

	            return _compositionNotificationMethods ?? (_compositionNotificationMethods = new List<Action<IComposer, object>>());
	        }
        }

	    public ICompositionalQuery ComponentCacheQuery
	    {
	        get
	        {
	            if (_composer != null)
	                throw new InvalidOperationException("Cannot access ComponentCacheQuery when the factory is initialized.");

	            return _componentCacheQuery;
	        }
	        set
	        {
	            if (_composer != null)
	                throw new InvalidOperationException("Cannot access ComponentCacheQuery when the factory is initialized.");

	            _componentCacheQuery = value;
	        }
	    }

	    #endregion

		#region Private helper methods

		private void CompleteConfiguration()
		{
		    try
		    {
		        LoadInitializationPoints();
		        LoadComponentCacheQuery();
		        LoadComponentCache();
		        LoadCompositionNotificationMethods();
		    }
		    catch(Exception e)
		    {
		        throw new CompositionException("Could not initialize DelegateComponentFactory", e);
		    }
		}

		private void LoadInitializationPoints()
		{
			// Check two categories of members for being an initialization point:
			//   1. Public fields
			//   2. Public properties
			// Check and add them to the list of initialization points if they
			// are not already registered.

			foreach (var fieldInfo in typeof(TComponent).GetFields())
			{
				ComponentContextUtils.CheckAndAddInitializationPoint(_composer, _initializationPoints, fieldInfo);
			}

			foreach (var fieldInfo in typeof(TComponent).GetProperties())
			{
				ComponentContextUtils.CheckAndAddInitializationPoint(_composer, _initializationPoints, fieldInfo);
			}
		}

	    private void LoadComponentCacheQuery()
	    {
	        if (_componentCacheQuery != null)
                return;

            var attribute = ComponentContextUtils.GetComponentCacheAttribute(typeof(TComponent));
	        if (attribute == null)
	        {
	            _componentCacheQuery = new ComponentQuery(typeof(DefaultComponentCache), null);
                return;
	        }

	        if (attribute.ComponentCacheType == null)
	        {
	            _componentCacheQuery = null;
                return;
	        }

            _componentCacheQuery = new ComponentQuery(attribute.ComponentCacheType, attribute.ComponentCacheName);
        }

        private void LoadComponentCache()
		{
			if (_componentCacheQuery == null || _componentCacheQuery is NullQuery)
			{
				_componentCache = null;
				return;
			}

			var result = _componentCacheQuery.Query(_composer);
			if (result == null)
				throw new CompositionException($"Can not register delegate component factory because " +
				                               $"the specified ComponentCache contract ({_componentCache}) could not be queried from Composer.");

			if (!(result is IComponentCache))
				throw new CompositionException($"Component cache type {result.GetType().FullName} that is specified " +
				                               $"as component cache handler on component does not implement " +
				                               "IComponentCache interface.");

			_componentCache = (IComponentCache) result;
		}

		private void LoadCompositionNotificationMethods()
		{
		    var methodsFound = ComponentContextUtils.FindCompositionNotificationMethods(typeof(TComponent)).ToList();
		    _compositionNotificationMethods = _compositionNotificationMethods?.Concat(methodsFound).ToList() ?? methodsFound;
		}

		private void InvokeCompositionNotifications(object componentInstance)
		{
			if (_compositionNotificationMethods == null)
				return;

			foreach (var method in _compositionNotificationMethods)
			{
			    method(_composer, componentInstance);
			}
		}

		private ComponentCacheEntry CreateComponent(ContractIdentity contract, IEnumerable<ICompositionListener> listenerChain)
		{
			// Save the original component instance reference, so that
			// we can apply initialization points to it later, as the
			// composition listeners may change the reference to a
			// wrapped one.

			object originalComponentInstance = _factoryMethod(_composer);

			// After constructing the component object, first process
			// all composition listeners so that if the reference should
			// change, it changes before setting it to the cache.
			// Otherwise, in circular dependency scenarios, dependent
			// components may get unwrapped component where the component
			// is wrapped by composition listeners.

			object componentInstance = NotifyCreated(originalComponentInstance, contract, listenerChain);

			// Store the cache, so that if there is a circular dependency,
			// applying initialization points is not blocked and chained
			// recursively.

			var result = new ComponentCacheEntry
			             	{
			             		ComponentInstance = componentInstance,
			             		OriginalComponentInstance = originalComponentInstance
			             	};

		    _componentCache?.PutInCache(contract, result);

		    // Complete the object initialization by applying the initialization
			// points. They should be applied to the original component instance,
			// as the reference may have been changed by composition listeners to
			// an instance that does not have the original configuration points.

			var initializationPointResults = ApplyInitializationPoints(originalComponentInstance);

			// Inform all composition listeners of the newly composed
			// component instance by calling OnComponentComposed method.

			NotifyComposed(componentInstance, originalComponentInstance, initializationPointResults, contract, listenerChain);

			// The composition is now finished for the component instance.
			// See if an [OnCompositionComplete] method is specified, call it.
			// This should be called on the original component instance
			// for the same reason stated above.

			InvokeCompositionNotifications(componentInstance);
			return result;
		}
		
		private object NotifyCreated(object originalComponentInstance, ContractIdentity contract,
		                             IEnumerable<ICompositionListener> listenerChain)
		{
			var componentInstance = originalComponentInstance;

			foreach (var compositionListener in listenerChain)
			{
				compositionListener.OnComponentCreated(contract, this, typeof(TComponent), ref componentInstance, originalComponentInstance);
			}

			return componentInstance;
		}

		private void NotifyComposed(object componentInstance, object originalComponentInstance,
		                            List<object> initializationPointResults, ContractIdentity contract,
		                            IEnumerable<ICompositionListener> listenerChain)
		{
			foreach (var compositionListener in listenerChain)
			{
				compositionListener.OnComponentComposed(contract, _initializationPoints, initializationPointResults, typeof(TComponent),
				                                        componentInstance, originalComponentInstance);
			}
		}

		private object NotifyRetrieved(object componentInstance, object originalComponentInstance, ContractIdentity contract,
		                               IEnumerable<ICompositionListener> listenerChain)
		{
			// The component is ready to be delivered.
			// Inform composition listeners about the retrieval.

			var result = componentInstance;

			foreach (var compositionListener in listenerChain)
			{
				compositionListener.OnComponentRetrieved(contract, this, typeof(TComponent), ref result, originalComponentInstance);
			}

			return result;
		}

		private List<object> ApplyInitializationPoints(object originalComponentInstance)
		{
			var initializationPointResults = new List<object>();

			foreach (var initializationPoint in _initializationPoints)
			{
				if (initializationPoint.Query == null)
					throw new CompositionException(
					        $"Query is null for initialization point '{initializationPoint.Name}' on component instance of type '{originalComponentInstance.GetType().FullName}'");

				var initializationPointResult = initializationPoint.Query.Query(_composer);

				// Check if the required initialization points get a value.
				if (initializationPoint.Required.GetValueOrDefault(_composer.Configuration.InitializationPointsRequiredByDefault) && initializationPointResult == null)
					throw new CompositionException(
					        $"Could not fill initialization point '{initializationPoint.Name}' of type '{originalComponentInstance.GetType().FullName}'.");

				initializationPointResults.Add(initializationPointResult);
				ComponentContextUtils.ApplyInitializationPoint(originalComponentInstance,
				                                               initializationPoint.Name,
				                                               initializationPoint.MemberType,
				                                               initializationPointResult);
			}

			return initializationPointResults;
		}


		#endregion
	}
}