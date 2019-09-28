using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComposerCore.CompositionalQueries;
using ComposerCore.Cache;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;


namespace ComposerCore.Factories
{
	public class LocalComponentFactory : LocalComponentFactoryBase
	{
		private IComponentCache _componentCache;

		private ConstructorInfo _targetConstructor;

		#region Constructors

		public LocalComponentFactory(Type targetType) : base(targetType)
		{
			_componentCache = null;
			_targetConstructor = null;
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
			try
			{
				LoadInitializationPoints();
				LoadTargetConstructor();
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

		#region Public properties

		public ConstructorInfo TargetConstructor
		{
			get => _targetConstructor;
	        set
			{
				EnsureNotInitialized("change TargetConstructor");
				_targetConstructor = value;
			}
		}

	    #endregion

		#region Private helper methods

		private void LoadTargetConstructor()
		{
			// If the constructor arguments are specified by the creator of the factory,
			// ignore finding the constructor. The constructor to be used will be bound
			// when creating the component.

			if (_constructorArgs != null)
				return;

			// Ignore finding the constructor if the creator of the factory has specified one.
			// Just extract the constructor args.

			if (_targetConstructor == null)
			{
				// Find the appropriate constructor for instantiating the component.
				// Order of precedence:
				//     1. The one marked with [CompositionConstructor]
				//     2. If there's a single public constructor, use it.
				//     3. If there's the default constructor, use it.
				// And it is an exception if none of the above is found.

				var candidateConstructors = TargetType.GetConstructors();
				_targetConstructor = FindMarkedConstructor(TargetType, candidateConstructors) ??
				                     FindSingleConstructor(candidateConstructors) ??
				                     FindDefaultConstructor(candidateConstructors);

				if (_targetConstructor == null)
					throw new CompositionException(
						"There's no appropriate constructor identified as the composition constructor for type '" + TargetType.FullName +
						"'" +
						"You can fix this by using [CompositionConstructor] attribute on the constructor that you intend to be used by Composer.");
			}

			_constructorArgs = new List<ConstructorArgSpecification>();
			string[] queryNames = null;

			if (ComponentContextUtils.HasCompositionConstructorAttribute(_targetConstructor))
				queryNames = ComponentContextUtils.GetCompositionConstructorAttribute(_targetConstructor).Names;

			foreach (var parameterInfo in _targetConstructor.GetParameters())
			{
				if (!Composer.Configuration.DisableAttributeChecking && !ComponentContextUtils.HasContractAttribute(parameterInfo.ParameterType))
					throw new CompositionException(
					        $"Parameter '{parameterInfo.Name}' of the constructor of type '{TargetType.FullName}' is not of a Contract type. " +
					        "All parameters of the composition constructor must be of Contract types, so that Composer can query for a component and pass it to them.");

				if ((queryNames != null) && (queryNames.Length > parameterInfo.Position))
					_constructorArgs.Add(new ConstructorArgSpecification(true,
					                                                     new ComponentQuery(parameterInfo.ParameterType,
					                                                                        queryNames[parameterInfo.Position])));
				else
					_constructorArgs.Add(new ConstructorArgSpecification(true, new ComponentQuery(parameterInfo.ParameterType, null)));
			}

			if ((queryNames != null) && (queryNames.Length > _constructorArgs.Count))
				throw new CompositionException("Extra names are specified for the constructor of type '" +
				                               TargetType.FullName + "'");
		}

		private void LoadInitializationPoints()
		{
			// Check two categories of members for being an initialization point:
			//   1. Public fields
			//   2. Public properties
			// Check and add them to the list of initialization points if they
			// are not already registered.

			_initializationPoints = _initializationPoints ?? new List<InitializationPointSpecification>();
			
			foreach (var fieldInfo in TargetType.GetFields())
			{
				ComponentContextUtils.CheckAndAddInitializationPoint(Composer, _initializationPoints, fieldInfo);
			}

			foreach (var fieldInfo in TargetType.GetProperties())
			{
				ComponentContextUtils.CheckAndAddInitializationPoint(Composer, _initializationPoints, fieldInfo);
			}
		}

	    private void LoadComponentCacheQuery()
	    {
	        if (_componentCacheQuery != null)
                return;

            var attribute = ComponentContextUtils.GetComponentCacheAttribute(TargetType);
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

		private void LoadCompositionNotificationMethods()
		{
		    var methodsFound = ComponentContextUtils.FindCompositionNotificationMethods(TargetType).ToList();
		    _compositionNotificationMethods = _compositionNotificationMethods?.Concat(methodsFound).ToList() ?? methodsFound;
		}

		private static ConstructorInfo FindMarkedConstructor(Type targetType,
		                                                     IEnumerable<ConstructorInfo> candidateConstructors)
		{
			var markedConstructors =
				candidateConstructors.Where(ComponentContextUtils.HasCompositionConstructorAttribute).ToArray();

			if (markedConstructors.Length == 0)
				return null;

			if (markedConstructors.Length > 1)
				throw new CompositionException("The type '" + targetType.FullName +
				                               "' has more than one constructor marked with [CompositionConstructor] attribute.");

			return markedConstructors[0];
		}

		private static ConstructorInfo FindSingleConstructor(IEnumerable<ConstructorInfo> candidateConstructors)
		{
			return candidateConstructors.Count() == 1 ? candidateConstructors.First() : null;
		}

		private static ConstructorInfo FindDefaultConstructor(IEnumerable<ConstructorInfo> candidateConstructors)
		{
			return candidateConstructors.FirstOrDefault(c => c.GetParameters().Length == 0);
		}

		private ComponentCacheEntry CreateComponent(ContractIdentity contract, IEnumerable<ICompositionListener> listenerChain)
		{
			// Prepare the constructor information for instantiating the object.

			var constructorArguments = PrepareConstructorArguments();
			var targetConstructor = FindTargetConstructor(constructorArguments);

			// Save the original component instance reference, so that
			// we can apply initialization points to it later, as the
			// composition listeners may change the reference to a
			// wrapped one.

			object originalComponentInstance = targetConstructor.Invoke(constructorArguments.ToArray());

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

		private List<object> PrepareConstructorArguments()
		{
			var constructorArguments = new List<object>();

            if (_constructorArgs == null)
                throw new CompositionException("LocalComponentFactory is not property initialized. _constructorArgs is null.");

			foreach (var cas in _constructorArgs)
			{
				if (cas.Query == null)
					throw new CompositionException("Query is null for a constructor argument, for the type '" +
					                               TargetType.FullName + "'");

				object argumentValue = cas.Query.Query(Composer);

				if ((argumentValue == null) && (cas.Required))
					throw new CompositionException("Required constructor argument can not be queried for type '" +
					                               TargetType.FullName + "'");

				constructorArguments.Add(argumentValue);
			}
			return constructorArguments;
		}

		private object NotifyCreated(object originalComponentInstance, ContractIdentity contract,
		                             IEnumerable<ICompositionListener> listenerChain)
		{
			var componentInstance = originalComponentInstance;

			foreach (var compositionListener in listenerChain)
			{
				compositionListener.OnComponentCreated(contract, this, TargetType, ref componentInstance, originalComponentInstance);
			}

			return componentInstance;
		}

		private void NotifyComposed(object componentInstance, object originalComponentInstance,
		                            List<object> initializationPointResults, ContractIdentity contract,
		                            IEnumerable<ICompositionListener> listenerChain)
		{
			foreach (var compositionListener in listenerChain)
			{
				compositionListener.OnComponentComposed(contract, _initializationPoints, initializationPointResults, TargetType,
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
				compositionListener.OnComponentRetrieved(contract, this, TargetType, ref result, originalComponentInstance);
			}

			return result;
		}

		private ConstructorInfo FindTargetConstructor(List<object> constructorArguments)
		{
			ConstructorInfo targetConstructor = _targetConstructor;

			if (targetConstructor == null)
			{
                if (constructorArguments.Any(arg => arg == null))
                    throw new CompositionException($"Canntot find an appropriate constructor to initialize type {TargetType.FullName} " +
                                                   "because some of the constructor arguments are null. You can specify the constructor " +
                                                   "to use to avoid this problem when passing null values.");

				var constructorArgTypes = constructorArguments.Select(arg => arg.GetType()).ToArray();
				targetConstructor = TargetType.GetConstructor(constructorArgTypes.ToArray());
			}

			if (targetConstructor == null)
				throw new CompositionException($"No constructor found for the component type '{TargetType.FullName}'");

			return targetConstructor;
		}

		private List<object> ApplyInitializationPoints(object originalComponentInstance)
		{
			var initializationPointResults = new List<object>();

			foreach (var initializationPoint in _initializationPoints)
			{
				if (initializationPoint.Query == null)
					throw new CompositionException(
					        $"Query is null for initialization point '{initializationPoint.Name}' on component instance of type '{TargetType.FullName}'");

				var initializationPointResult = initializationPoint.Query.Query(Composer);

				// Check if the required initialization points get a value.
				if (initializationPoint.Required && initializationPointResult == null)
					throw new CompositionException(
					        $"Could not fill initialization point '{initializationPoint.Name}' of type '{TargetType.FullName}'.");

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