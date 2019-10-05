using System;
using System.Collections.Generic;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;


namespace ComposerCore.Factories
{
	public class LocalComponentFactory : LocalComponentFactoryBase
	{
		private IComponentCache _componentCache;

		private readonly LocalComponentBuilder _builder;
		private readonly LocalComponentInitializer _initializer;

		#region Constructors

		public LocalComponentFactory(Type targetType) : base(targetType)
		{
			_componentCache = null;
			
			_builder = new LocalComponentBuilder(targetType);
			_initializer = new LocalComponentInitializer();
		}

		#endregion

		#region IComponentFactory Members

		public override bool ValidateContractType(Type contract)
		{
			return contract.IsAssignableFrom(TargetType);
		}

		public override void Initialize(IComposer composer)
		{
			base.Initialize(composer);

			_builder.Initialize(composer);
			
			try
			{
				LoadInitializationPoints();
				LoadComponentCacheQuery();
				LoadComponentCache();
				LoadCompositionNotificationMethods();
			}
			catch(Exception e)
			{
				throw new CompositionException(
					$"Could not initialize LocalComponentFactory for type '{TargetType.FullName}'", e);
			}
		}

		public override IEnumerable<Type> GetContractTypes()
		{
			return ComponentContextUtils.FindContracts(TargetType);
		}

		public override object GetComponentInstance(ContractIdentity contract, IEnumerable<ICompositionListener> listenerChain)
		{
			if (!Initialized)
				throw new InvalidOperationException(
					"LocalComponentFactory should be initialized before calling GetComponentInstance method.");

			if (_componentCache == null)
			{
				// If the component is not cached at all, then unique instances should
				// be created for each call, then locking does not help and just
				// degrades performance. So, create without locking.

				var newComponent = CreateComponent(contract, listenerChain);
				return listenerChain.NotifyRetrieved(
					newComponent.ComponentInstance, newComponent.OriginalComponentInstance, contract, this);
			}

			// Check if the component is cached, and ready to deliver

			var componentCacheEntry = _componentCache.GetFromCache(contract);
			if (componentCacheEntry != null)
			{
				return listenerChain.NotifyRetrieved(
					componentCacheEntry.ComponentInstance, componentCacheEntry.OriginalComponentInstance, contract, this);
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
					return listenerChain.NotifyRetrieved(
						componentCacheEntry.ComponentInstance, componentCacheEntry.OriginalComponentInstance, contract, this);
				}

				componentCacheEntry = CreateComponent(contract, listenerChain);
				return listenerChain.NotifyRetrieved(
					componentCacheEntry.ComponentInstance, componentCacheEntry.OriginalComponentInstance, contract, this);
			}
		}

		#endregion
		
		#region Private helper methods

        private void LoadComponentCache()
		{
			if (_componentCacheQuery == null || _componentCacheQuery is NullQuery)
			{
				_componentCache = null;
				return;
			}

			var result = _componentCacheQuery.Query(Composer);
			if (result == null)
				throw new CompositionException($"Can not register component type {TargetType.FullName} because " +
				                               $"the specified ComponentCache contract ({_componentCache}) could not be queried from Composer.");

			_componentCache = result as IComponentCache
			                  ?? throw new CompositionException(
				                  $"Component cache type {result.GetType().FullName} that is specified " +
				                  $"as component cache handler on component {TargetType.FullName} does not implement " +
				                  "IComponentCache interface.");
		}

		private ComponentCacheEntry CreateComponent(ContractIdentity contract, IEnumerable<ICompositionListener> listenerChain)
		{
			// Save the original component instance reference, so that
			// we can apply initialization points to it later, as the
			// composition listeners may change the reference to a
			// wrapped one.

			object originalComponentInstance = _builder.Build();

			// After constructing the component object, first process
			// all composition listeners so that if the reference should
			// change, it changes before setting it to the cache.
			// Otherwise, in circular dependency scenarios, dependent
			// components may get unwrapped component where the component
			// is wrapped by composition listeners.

			object componentInstance = listenerChain.NotifyCreated(originalComponentInstance, contract, this);

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

			listenerChain.NotifyComposed(
				componentInstance, originalComponentInstance, initializationPointResults, contract, _initializationPoints, this);

			// The composition is now finished for the component instance.
			// See if an [OnCompositionComplete] method is specified, call it.
			// This should be called on the original component instance
			// for the same reason stated above.

			InvokeCompositionNotifications(componentInstance);
			return result;
		}
		
		private List<object> ApplyInitializationPoints(object originalComponentInstance)
		{
			var initializationPointResults = new List<object>();

			foreach (var initializationPoint in _initializationPoints)
			{
				if (initializationPoint.Query == null)
					throw new CompositionException(
					        $"Query is null for initialization point '{initializationPoint.Name}' on component instance of type '{TargetType.FullName}'");

				if (initializationPoint.Query.IsResolvable(Composer))
				{
					var initializationPointResult = initializationPoint.Query.Query(Composer);

					initializationPointResults.Add(initializationPointResult);
					ComponentContextUtils.ApplyInitializationPoint(originalComponentInstance,
						initializationPoint.Name,
						initializationPoint.MemberType,
						initializationPointResult);
				}
				else
				{
					// Check if the required initialization points get a value.
					if (initializationPoint.Required.GetValueOrDefault(Composer.Configuration.ComponentPlugRequiredByDefault))
						throw new CompositionException(
							$"Could not fill initialization point '{initializationPoint.Name}' of type '{TargetType.FullName}'.");
					
					initializationPointResults.Add(null);
				}
				
			}

			return initializationPointResults;
		}

		#endregion
	}
}