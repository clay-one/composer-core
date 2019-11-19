using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.Extensibility;
using ComposerCore.Implementation.ConstructorResolvers;
using ComposerCore.Utility;

namespace ComposerCore.Implementation
{
	[Contract, Component, ComponentCache(typeof(NoComponentCache))]
	public class ComponentContext : IComponentContext
	{
        #region Private Data

		private readonly ComponentRepository _repository;
		private readonly Dictionary<string, object> _variables;

		#endregion

		#region Constructors

		public ComponentContext()
			: this(true)
		{
		}

		protected ComponentContext(bool registerComponents)
		{
			Configuration = new ComposerConfiguration();

			_repository = new ComponentRepository();
			_variables = new Dictionary<string, object>();

			this.RegisterObject(this);
			
			if (registerComponents)
				RegisterBuiltInComponents();
		}
		
		private void RegisterBuiltInComponents()
		{
			Register(new ConcreteTypeRegistration(typeof(PerRegistrationComponentCache), NoComponentCache.Instance));
			
			this.RegisterObject(NoComponentCache.Instance);
			this.RegisterObject(new CompositionListenerChain());
			
			this.Register(typeof(PerContractComponentCache));
			this.Register(typeof(DefaultComponentCache));
            this.Register(typeof(ContractAgnosticComponentCache));
            this.Register(typeof(SingletonComponentCache));
            this.Register(typeof(TransientComponentCache));
            this.Register(typeof(ScopedComponentCacheStore));
            this.Register(typeof(ScopedComponentCache));
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
		
        public void Register(IComponentRegistration registration)
        {
	        registration.SetAsRegistered(this);
	        _repository.Add(registration);
        }

        public void Unregister(ContractIdentity identity)
		{
			if (identity == null)
				throw new ArgumentNullException();

			_repository.Remove(identity);
		}

        public void UnregisterFamily(Type type)
		{
			if (type == null)
				throw new ArgumentNullException();

			_repository.RemoveAll(type);
		}

        public void SetVariable(string name, Lazy<object> value)
		{
			RemoveVariable(name);

			if (value == null)
				return;

			_variables.Add(name, value);
		}

        public void RemoveVariable(string name)
		{
			if (name == null)
				throw new ArgumentNullException();

			_variables.Remove(name);
		}

		#endregion

		#region IComposer implementation

	    public ComposerConfiguration Configuration { get; }

	    public virtual bool IsResolvable(Type contract, string name = null)
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
		    var registrations = _repository.Find(identity);
		    
		    using (var enumerator = registrations?.GetEnumerator())
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

        public object GetComponent(Type contract, string name = null)
        {
	        return GetComponent(contract, name, this);
		}

        public IEnumerable<object> GetAllComponents(Type contract, string name = null)
        {
	        return GetAllComponents(contract, name, this);
		}

        public IEnumerable<object> GetComponentFamily(Type contract)
        {
	        return GetComponentFamily(contract, this);
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

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "IEnumerable is over an in-memory at runtime array and doesn't impose any cost to enumerate it multiple times.'")]
        public void InitializePlugs(object componentInstance, Type componentType)
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

        public IComposer CreateScope()
        {
	        var result = new ChildComponentContext(this);
	        result.Register(typeof(ScopedComponentCacheStore));
            
	        return result;
        }
        
		#endregion
		
		#region IDisposable implementation

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion
		
		#region Protected component query methods
		
		protected internal virtual object GetComponent(Type contract, string name, IComposer dependencyResolver)
		{
			if (contract.ContainsGenericParameters)
				throw new CompositionException("Requested contract type " + contract.Name +
				                               " contains open generic parameters. Can not construct a concrete type.");

			var identity = new ContractIdentity(contract, name);
			var registrations = _repository.Find(identity);

			using (var enumerator = registrations?.GetEnumerator())
			{
				if (enumerator == null)
					return null;

				while (enumerator.MoveNext())
				{
					var result = enumerator.Current?.GetComponent(identity, dependencyResolver);
					if (result != null)
						return result;
				}
			}

			var enumerableElementType = contract.GetEnumerableElementType();
			if (enumerableElementType != null)
				return GetAllComponents(enumerableElementType, name);

			return null;
		}

		protected internal virtual IEnumerable<object> GetAllComponents(Type contract, string name, IComposer dependencyResolver)
		{
			var identity = new ContractIdentity(contract, name);
			var registrations = _repository.Find(identity);

			return registrations
				.Select(f => f.GetComponent(identity, dependencyResolver))
				.Where(result => result != null)
				.CastToRuntimeType(contract);
		}

		protected internal virtual IEnumerable<object> GetComponentFamily(Type contract, IComposer dependencyResolver)
		{
			var identities = _repository.GetContractIdentityFamily(contract);

			return identities.SelectMany(identity => _repository.Find(identity),
					(identity, registration) => registration.GetComponent(identity, dependencyResolver))
				.CastToRuntimeType(contract);
		}

		
		#endregion
	}
}