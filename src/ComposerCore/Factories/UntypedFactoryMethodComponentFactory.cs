using System;
using System.Collections.Generic;
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

		public UntypedFactoryMethodComponentFactory(Func<IComposer, object> factoryMethod)
		{
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException(nameof(factoryMethod));

			_composer = null;
			_componentCache = null;
            _componentCacheQuery = null;
            _contractTypes = null;
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
			return _contractTypes;
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

			if (!(result is IComponentCache))
				throw new CompositionException($"Component cache type {result.GetType().FullName} that is specified " +
				                               $"as component cache handler on component does not implement " +
				                               "IComponentCache interface.");

			_componentCache = (IComponentCache) result;
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

			_composer.InitializePlugs(originalComponentInstance, originalComponentInstance.GetType());
			
			// We do not call "OnComponentComposed" on the listener chain here, because they are already called
			// with the "InitializePlugs" call

			return result;
		}
		
		private object NotifyCreated(object originalComponentInstance, ContractIdentity contract,
		                             IEnumerable<ICompositionListener> listenerChain)
		{
			var componentInstance = originalComponentInstance;

			foreach (var compositionListener in listenerChain)
			{
				compositionListener.OnComponentCreated(contract, this, originalComponentInstance.GetType(), ref componentInstance, originalComponentInstance);
			}

			return componentInstance;
		}
		
		private object NotifyRetrieved(object componentInstance, object originalComponentInstance, ContractIdentity contract,
		                               IEnumerable<ICompositionListener> listenerChain)
		{
			// The component is ready to be delivered.
			// Inform composition listeners about the retrieval.

			var result = componentInstance;

			foreach (var compositionListener in listenerChain)
			{
				compositionListener.OnComponentRetrieved(contract, this, originalComponentInstance.GetType(), ref result, originalComponentInstance);
			}

			return result;
		}

		#endregion
        
    }
}