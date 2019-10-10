using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.Extensibility;
using ComposerCore.Factories;
using ComposerCore.Implementation.ConstructorResolvers;
using ComposerCore.Utility;


namespace ComposerCore.Implementation
{
	[Contract]
	[Component]
	[ComponentCache(null)]
	public class ComponentContext : IComponentContext
	{
        #region Private Data

		private readonly Repository _repository;
		private readonly Dictionary<string, object> _variables;

		#endregion

		#region Constructors

		public ComponentContext() : this(true)
		{
		}

		public ComponentContext(bool registerBuiltInComponents)
		{
            Configuration = new ComposerConfiguration();

			_repository = new Repository();
			_variables = new Dictionary<string, object>();

			RegisterObject(this);

			if (registerBuiltInComponents)
				RegisterBuiltInComponents();
		}

		private void RegisterBuiltInComponents()
		{
			RegisterObject(new CompositionListenerChain());
			
		    InternalRegister(typeof (DefaultComponentCache), null,
		        ComponentContextUtils.CreateLocalFactory(typeof (DefaultComponentCache)), false);
            InternalRegister(typeof(ContractAgnosticComponentCache), null,
                ComponentContextUtils.CreateLocalFactory(typeof(ContractAgnosticComponentCache)), false);

            RegisterObject((string)null, new ExplicitConstructorResolver());
            Register(typeof(ExplicitConstructorResolver));
            Register(typeof(DefaultConstructorResolver));
            Register(typeof(SingleOrDefaultConstructorResolver));
            Register(typeof(MostParametersConstructorResolver));
            Register(typeof(LeastParametersConstructorResolver));
            Register(typeof(MostResolvableConstructorResolver));
            Register(typeof(PresetConstructorStore));
            Register(typeof(PresetConstructorResolver));
            
            InternalRegister(typeof(StaticComponentCache), null,
                ComponentContextUtils.CreateLocalFactory(typeof(StaticComponentCache)), false);
            InternalRegister(typeof(ThreadLocalComponentCache), null,
                ComponentContextUtils.CreateLocalFactory(typeof(ThreadLocalComponentCache)), false);
		}

		#endregion

		#region IComponentContext implementation

		public virtual void Register(Type contract, Type component)
		{
			Register(contract, ComponentContextUtils.GetComponentDefaultName(component), component);
		}

        public virtual void Register(Type component)
		{
			if (component == null)
				throw new ArgumentNullException(nameof(component));

			Register(
				ComponentContextUtils.GetComponentDefaultName(component), 
				ComponentContextUtils.CreateLocalFactory(component));
		}

        public virtual void Register(Type contract, string name, Type component)
		{
			if (contract == null)
				throw new ArgumentNullException(nameof(contract));
			if (component == null)
				throw new ArgumentNullException(nameof(component));

			ComponentContextUtils.ThrowIfNotSubTypeOf(contract, component);

			InternalRegister(contract, name, ComponentContextUtils.CreateLocalFactory(component), true);
		}

        public virtual void Register(IComponentFactory componentFactory)
		{
			Register((string) null, componentFactory);
		}

        public virtual void Register(string name, IComponentFactory componentFactory)
		{
			if (componentFactory == null)
				throw new ArgumentNullException(nameof(componentFactory));

			var contracts = componentFactory.GetContractTypes().ToArray();

			if (!contracts.Any())
				throw new CompositionException("No contracts found for the component factory " + componentFactory);

			foreach (var contract in contracts)
			{
				InternalRegister(contract, name, componentFactory, false);
			}
		}

        public virtual void Register(string name, Type componentType)
		{
			if (componentType == null)
				throw new ArgumentNullException(nameof(componentType));

			Register(name, ComponentContextUtils.CreateLocalFactory(componentType));
		}

        public virtual void Register(Type contract, IComponentFactory factory)
		{
			Register(contract, null, factory);
		}

        public virtual void Register(Type contract, string name, IComponentFactory factory)
		{
			if (contract == null)
				throw new ArgumentNullException(nameof(contract));
			if (factory == null)
				throw new ArgumentNullException(nameof(factory));

			InternalRegister(contract, name, factory, true);
		}

        public void RegisterObject(object componentInstance)
        {
	        if (componentInstance == null)
		        throw new ArgumentNullException(nameof(componentInstance));

	        var componentType = componentInstance.GetType();
	        var name = ComponentContextUtils.GetComponentDefaultName(componentType);
	        
	        RegisterObject(name, componentInstance);
        }

        public void RegisterObject(Type contract, object componentInstance)
        {
	        if (componentInstance == null)
		        throw new ArgumentNullException(nameof(componentInstance));

	        var componentType = componentInstance.GetType();
	        var name = ComponentContextUtils.GetComponentDefaultName(componentType);
	        
	        RegisterObject(contract, name, componentInstance);
        }

        public void RegisterObject(string name, object componentInstance)
        {
	        if (componentInstance == null)
		        throw new ArgumentNullException(nameof(componentInstance));
	        
	        var factory = new PreInitializedComponentFactory(componentInstance);
	        Register(name, factory);
        }

        public void RegisterObject(Type contract, string name, object componentInstance)
        {
	        if (contract == null)
		        throw new ArgumentNullException(nameof(contract));
	        if (componentInstance == null)
		        throw new ArgumentNullException(nameof(componentInstance));

	        ComponentContextUtils.ThrowIfNotSubTypeOf(contract, componentInstance.GetType());

	        var factory = new PreInitializedComponentFactory(componentInstance);
	        Register(contract, name, factory);
        }

        public virtual void Unregister(ContractIdentity identity)
		{
			if (identity == null)
				throw new ArgumentNullException();

			_repository.Remove(identity);
		}

        public virtual void UnregisterFamily(Type type)
		{
			if (type == null)
				throw new ArgumentNullException();

			_repository.RemoveAll(type);
		}

        public virtual void SetVariableValue(string name, object value)
		{
			if (value == null)
				RemoveVariable(name);
			else
				SetVariable(name, new Lazy<object>(() => value));
		}

        public virtual void SetVariable(string name, Lazy<object> value)
		{
			RemoveVariable(name);

			if (value == null)
				return;

			_variables.Add(name, value);
		}

        public virtual void RemoveVariable(string name)
		{
			if (name == null)
				throw new ArgumentNullException();

			_variables.Remove(name);
		}

        public virtual void RegisterCompositionListener(string name, ICompositionListener listener)
		{
			GetComponent<CompositionListenerChain>().RegisterCompositionListener(name, listener);
		}

        public virtual void UnregisterCompositionListener(string name)
		{
			GetComponent<CompositionListenerChain>().UnregisterCompositionListener(name);
		}

		#endregion

		#region IComposer implementation

	    public ComposerConfiguration Configuration { get; }

	    public bool IsResolvable<TContract>(string name = null) where TContract : class
	    {
		    return IsResolvable(typeof(TContract), name);
	    }

	    public bool IsResolvable(Type contract, string name = null)
	    {
		    if (contract.ContainsGenericParameters)
			    throw new CompositionException("Requested contract type " + contract.Name +
			                                   " contains open generic parameters. Can not construct a concrete type.");

		    if (contract.IsInterface && contract.IsGenericType)
		    {
			    var enumerableElementType = contract.GetEnumerableElementType();
			    if (enumerableElementType != null)
				    return true;
		    }

		    var identity = new ContractIdentity(contract, name);
		    return _repository.FindFactories(identity)?.Any() ?? false;
	    }

	    public virtual TContract GetComponent<TContract>(string name = null)
			where TContract : class
		{
			return GetComponent(typeof (TContract), name) as TContract;
		}

        public virtual object GetComponent(Type contract, string name = null)
		{
			if (contract.ContainsGenericParameters)
				throw new CompositionException("Requested contract type " + contract.Name +
				                               " contains open generic parameters. Can not construct a concrete type.");

			if (contract.IsInterface && contract.IsGenericType)
			{
				var enumerableElementType = contract.GetEnumerableElementType();
				if (enumerableElementType != null)
					return GetAllComponents(enumerableElementType, name);
			}
			
			var identity = new ContractIdentity(contract, name);
			var factories = _repository.FindFactories(identity);

		    using (var enumerator = factories?.GetEnumerator())
		    {
		        if (enumerator == null)
		            return null;

		        while (enumerator.MoveNext())
		        {
                    var result = enumerator.Current?.GetComponentInstance(identity);
		            if (result != null)
		                return result;
		        }
            }

		    return null;
		}

        public virtual IEnumerable<TContract> GetAllComponents<TContract>(string name = null)
			where TContract : class
		{
			return GetAllComponents(typeof (TContract), name).Cast<TContract>();
		}

        public virtual IEnumerable<object> GetAllComponents(Type contract, string name = null)
		{
			var identity = new ContractIdentity(contract, name);
			var factories = _repository.FindFactories(identity);

			return factories
				.Select(f => f.GetComponentInstance(identity))
				.Where(result => result != null)
				.CastToRuntimeType(contract);
		}

        public virtual IEnumerable<TContract> GetComponentFamily<TContract>()
		{
			return GetComponentFamily(typeof (TContract)).Cast<TContract>();
		}

        public virtual IEnumerable<object> GetComponentFamily(Type contract)
		{
			var identities = _repository.GetContractIdentityFamily(contract);

			return identities.SelectMany(identity => _repository.FindFactories(identity),
				(identity, factory) =>
					factory.GetComponentInstance(identity))
			.CastToRuntimeType(contract);
		}

        public virtual bool HasVariable(string name)
        {
	        if (name == null)
		        throw new ArgumentNullException(nameof(name));

	        return _variables.ContainsKey(name);
        }
        
        public virtual object GetVariable(string name)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			var variableValue = _variables.ContainsKey(name) ? _variables[name] : null;

			if (variableValue is Lazy<object>)
				variableValue = ((Lazy<object>) variableValue).Value;

			return variableValue;
		}

        public virtual void InitializePlugs<T>(T componentInstance)
		{
			InitializePlugs(componentInstance, typeof (T));
		}

        public virtual void InitializePlugs(object componentInstance, Type componentType)
		{
			var initializationPoints = ComponentContextUtils.ExtractInitializationPoints(this, componentType);

			var initializationPointResults = new List<object>();

			foreach (var initializationPoint in initializationPoints)
			{
				if (initializationPoint.Query == null)
					throw new CompositionException(
					        $"Query is null for initialization point '{initializationPoint.Name}' on component instance of type '{componentType.FullName}'");

				var initializationPointResult = initializationPoint.Query.Query(this);
				initializationPointResults.Add(initializationPointResult);

				ComponentContextUtils.ApplyInitializationPoint(componentInstance,
				                                               initializationPoint.Name,
				                                               initializationPoint.MemberType,
				                                               initializationPointResult);
			}

			GetComponent<ICompositionListenerChain>().NotifyComposed(
				componentInstance, componentInstance, initializationPointResults, 
				null, initializationPoints, componentType);

			var compositionNotificationMethods = ComponentContextUtils.FindCompositionNotificationMethods(componentType);
			if (compositionNotificationMethods != null)
			{
				foreach (var method in compositionNotificationMethods)
				{
				    method(this, componentInstance);
				}
			}
		}

		#endregion

		#region Internal and Private helper methods

		private void InternalRegister(Type contract, string name, IComponentFactory factory,
									  bool performChecking)
		{
			if (contract == null)
				throw new ArgumentNullException(nameof(contract));
			if (factory == null)
				throw new ArgumentNullException(nameof(factory));

			if (performChecking && !Configuration.DisableAttributeChecking)
				ComponentContextUtils.ThrowIfNotContract(contract);

			if (!factory.ValidateContractType(contract))
				throw new CompositionException("This component type / factory cannot be registered with the contract " +
				                               $"{contract.FullName}. The component type is not assignable to the contract " +
				                               "or the factory logic prevents such registration.");
			
			factory.Initialize(this);

			_repository.Add(new ContractIdentity(contract, name), factory);
		}

		#endregion
	}
}