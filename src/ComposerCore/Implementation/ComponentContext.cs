using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.Extensibility;
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

		public ComponentContext()
		{
			Configuration = new ComposerConfiguration();

			_repository = new Repository();
			_variables = new Dictionary<string, object>();

			this.RegisterObject(this);
			RegisterBuiltInComponents();
		}
		
		private void RegisterBuiltInComponents()
		{
			this.RegisterObject(new CompositionListenerChain());
			
		    this.Register(typeof(DefaultComponentCache));
            this.Register(typeof(ContractAgnosticComponentCache));
            this.Register(typeof(StaticComponentCache));
            this.Register(typeof(ThreadLocalComponentCache));

            this.RegisterObject((string)null, new ExplicitConstructorResolver());
            this.Register(typeof(ExplicitConstructorResolver));
            this.Register(typeof(DefaultConstructorResolver));
            this.Register(typeof(SingleOrDefaultConstructorResolver));
            this.Register(typeof(MostParametersConstructorResolver));
            this.Register(typeof(LeastParametersConstructorResolver));
            this.Register(typeof(MostResolvableConstructorResolver));
            this.Register(typeof(PresetConstructorStore));
            this.Register(typeof(PresetConstructorResolver));
		}

		#endregion

		#region IComponentContext implementation


        public virtual void Register(Type contract, string name, IComponentFactory factory)
		{
			if (contract == null)
				throw new ArgumentNullException(nameof(contract));
			if (factory == null)
				throw new ArgumentNullException(nameof(factory));

			if (!Configuration.DisableAttributeChecking)
				ComponentContextUtils.ThrowIfNotContract(contract);

			if (!factory.ValidateContractType(contract))
				throw new CompositionException("This component type / factory cannot be registered with the contract " +
				                               $"{contract.FullName}. The component type is not assignable to the " +
				                               "contract or the factory logic prevents such registration.");
			
			factory.Initialize(this);
			_repository.Add(new ContractIdentity(contract, name), factory);
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

		#endregion

		#region IComposer implementation

	    public ComposerConfiguration Configuration { get; }

	    public bool IsResolvable(Type contract, string name = null)
	    {
		    if (contract.ContainsGenericParameters)
			    throw new CompositionException("Requested contract type " + contract.Name +
			                                   " contains open generic parameters. Can not construct a concrete type.");

		    // IEnumerable<T> is always resolvable, since it is equal to calling GetAllComponents
		    var enumerableElementType = contract.GetEnumerableElementType();
		    if (enumerableElementType != null)
			    return Configuration.DisableAttributeChecking ||
			           ComponentContextUtils.HasContractAttribute(enumerableElementType);

		    if (!Configuration.DisableAttributeChecking && !ComponentContextUtils.HasContractAttribute(contract))
			    return false;
		    
		    var identity = new ContractIdentity(contract, name);
		    var factories = _repository.FindFactories(identity);
		    
		    using (var enumerator = factories?.GetEnumerator())
		    {
			    if (enumerator == null)
				    return false;

			    while (enumerator.MoveNext())
			    {
				    var current = enumerator.Current;
				    if (current != null && current.IsResolvable(identity.Type))
					    return true;
			    }
		    }

		    return false;
	    }

        public virtual object GetComponent(Type contract, string name = null)
		{
			if (contract.ContainsGenericParameters)
				throw new CompositionException("Requested contract type " + contract.Name +
				                               " contains open generic parameters. Can not construct a concrete type.");

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

		    var enumerableElementType = contract.GetEnumerableElementType();
		    if (enumerableElementType != null)
			    return GetAllComponents(enumerableElementType, name);

		    return null;
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
				if (initializationPointResult == null &&
				    initializationPoint.Required.GetValueOrDefault(Configuration.InitializationPointsRequiredByDefault))
				{
					throw new CompositionException($"Could not resolve required initialization point '{initializationPoint.Name}' on type '{componentType.FullName}'");
				}
				
				initializationPointResults.Add(initializationPointResult);

				ComponentContextUtils.ApplyInitializationPoint(componentInstance,
				                                               initializationPoint.Name,
				                                               initializationPoint.MemberType,
				                                               initializationPointResult);
			}

			this.GetComponent<ICompositionListenerChain>().NotifyComposed(
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
	}
}