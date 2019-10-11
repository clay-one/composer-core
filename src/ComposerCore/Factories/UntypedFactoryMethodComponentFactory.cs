using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
    public class UntypedFactoryMethodComponentFactory : IComponentFactory
    {
		private IComposer _composer;

		private readonly Func<IComposer, object> _factoryMethod;

		private IComponentCache _componentCache;
	    private ICompositionalQuery _componentCacheQuery;
	    private List<Type> _contractTypes;

		#region Constructors

		public UntypedFactoryMethodComponentFactory(Func<IComposer, object> factoryMethod, IEnumerable<Type> contractTypes = null)
		{
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));

			_composer = null;
			_componentCache = null;
            _componentCacheQuery = null;
            _contractTypes = contractTypes?.ToList();
		}

		#endregion

		#region IComponentFactory Members

		public bool ValidateContractType(Type contract)
		{
			return true;
		}

		public void Initialize(IComposer composer)
		{
			if (_composer != null)
				return;

			if (_factoryMethod == null)
				throw new InvalidOperationException("FactoryMethod is not specified.");

			_composer = composer;
			CompleteConfiguration();
		}

		public IEnumerable<Type> GetContractTypes()
		{
			if (_contractTypes == null || _contractTypes.Count == 0)
				throw new CompositionException("Contracts are not specified. For an UntypedFactoryMethod, there is " +
				                               "no way of discovering contracts before actual invocation of the " +
				                               "method and instantiation of the component. Contract types should " +
				                               "be provided to the factory using ContractTypes property before " +
				                               "registering it in the ComponentContext.");
			
			return _contractTypes;
		}

		public bool IsResolvable(Type contractType)
		{
			if (_contractTypes == null || _contractTypes.Count == 0)
				return false;

			return _contractTypes.Any(contractType.IsAssignableFrom);
		}

		public object GetComponentInstance(ContractIdentity contract)
		{
			// Check if the factory is initialized

			if (_composer == null)
				throw new InvalidOperationException(
					"DelegateComponentFactory should be initialized before calling GetComponentInstance method.");

			var listenerChain = _composer.GetComponent<ICompositionListenerChain>();
			
			if (_componentCache == null)
			{
				// If the component is not cached at all, then unique instances should
				// be created for each call, then locking does not help and just
				// degrades performance. So, create without locking.

				var newComponent = CreateComponent(contract, listenerChain);
				return listenerChain.NotifyRetrieved(newComponent.ComponentInstance, newComponent.OriginalComponentInstance,
					contract, this, newComponent.OriginalComponentInstance.GetType());
			}

			// Check if the component is cached, and ready to deliver

			var componentCacheEntry = _componentCache.GetFromCache(contract);
			if (componentCacheEntry != null)
			{
				return listenerChain.NotifyRetrieved(componentCacheEntry.ComponentInstance,
					componentCacheEntry.OriginalComponentInstance, contract, this,
					componentCacheEntry.OriginalComponentInstance.GetType());
			}

			// If the component is cached, then lock the component instance
			// to avoid creation of more than one cached components per config 
			// when called in concurrent manner

			lock (_componentCache.SynchronizationObject)
			{
				// Double-check the initialization to avoid rendezvouz

				componentCacheEntry = _componentCache.GetFromCache(contract) ?? CreateComponent(contract, listenerChain);
				return listenerChain.NotifyRetrieved(componentCacheEntry.ComponentInstance,
					componentCacheEntry.OriginalComponentInstance, contract, this,
					componentCacheEntry.OriginalComponentInstance.GetType());
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

		public Func<IComposer, object> FactoryMethod => _factoryMethod;

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

	    public List<Type> ContractTypes
	    {
	        get
	        {
	            if (_composer != null)
	                throw new InvalidOperationException("Cannot access ComponentCacheQuery when the factory is initialized.");

	            return _contractTypes;
	        }
	        set
	        {
	            if (_composer != null)
	                throw new InvalidOperationException("Cannot access ComponentCacheQuery when the factory is initialized.");

	            _contractTypes = value;
	        }
	    }

	    #endregion

		#region Private helper methods

		private void CompleteConfiguration()
		{
		    try
		    {
		        LoadComponentCache();
		    }
		    catch(Exception e)
		    {
		        throw new CompositionException("Could not initialize DelegateComponentFactory", e);
		    }
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

			_componentCache = result as IComponentCache ??
			                  throw new CompositionException(
				                  $"Component cache type {result.GetType().FullName} that is specified " +
				                  $"as component cache handler on component does not implement " +
				                  "IComponentCache interface.");
		}
        
        private ComponentCacheEntry CreateComponent(ContractIdentity contract, ICompositionListenerChain listenerChain)
		{
			// Save the original component instance reference, so that
			// we can apply initialization points to it later, as the
			// composition listeners may change the reference to a
			// wrapped one.

			var originalComponentInstance = _factoryMethod(_composer);

			// After constructing the component object, first process
			// all composition listeners so that if the reference should
			// change, it changes before setting it to the cache.
			// Otherwise, in circular dependency scenarios, dependent
			// components may get unwrapped component where the component
			// is wrapped by composition listeners.

			var componentInstance = listenerChain.NotifyCreated(originalComponentInstance, contract, this, originalComponentInstance.GetType());

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

			_composer.InitializePlugs(originalComponentInstance, originalComponentInstance.GetType());
			
			// We do not call "OnComponentComposed" on the listener chain here, because they are already called
			// with the "InitializePlugs" call

			return result;
		}
        
        #endregion
        
    }
}