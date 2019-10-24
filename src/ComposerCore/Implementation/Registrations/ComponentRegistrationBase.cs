using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation
{
    public abstract class ComponentRegistrationBase : IComponentRegistration
    {
        // ReSharper disable once InconsistentNaming
        protected readonly List<ContractIdentity> _contracts;

        public Type TargetType { get; }

        public ICompositionalQuery CacheQuery { get; private set; }
        public IComponentCache Cache { get; protected set; }
        public string DefaultContractName { get; private set; }
        public IComponentContext RegistrationContext { get; private set; }

        public IEnumerable<ContractIdentity> Contracts => _contracts;
        
        protected ComponentRegistrationBase(Type targetType)
        {
            TargetType = targetType;
            RegistrationContext = null;
            CacheQuery = null;
            Cache = null;
            DefaultContractName = ComponentContextUtils.GetComponentDefaultName(targetType);

            _contracts = new List<ContractIdentity>();
        }

        public void SetCache(string cacheComponentName)
        {
            CacheQuery = new ComponentQuery(typeof(IComponentCache), cacheComponentName ?? nameof(NoComponentCache));
            Cache = null;
        }

        public void SetCache(IComponentCache cache)
        {
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public void SetDefaultContractName(string defaultContractName)
        {
            EnsureNotInitialized();
            DefaultContractName = defaultContractName;
        }

        public void AddContractType(Type contractType)
        {
            EnsureNotInitialized();
            AddContract(new ContractIdentity(contractType, DefaultContractName));
        }

        public virtual void AddContract(ContractIdentity contract)
        {
            EnsureNotInitialized();

            if (TargetType != null && !contract.Type.IsAssignableFrom(TargetType))
                throw new CompositionException("This component type / factory cannot be registered with the contract " +
                                               $"{contract.Type.FullName}. The component type is not assignable to " +
                                               $"the contract or the factory logic prevents such registration.");
            
            _contracts.Add(contract);
        }


        public virtual void SetAsRegistered(IComponentContext registrationContext)
        {
            EnsureNotInitialized();
            EnsureContracts();
            if (!registrationContext.Configuration.DisableAttributeChecking)
            {
                EnsureComponentAttribute();
                EnsureContractAttribute();
            }

            RegistrationContext = registrationContext;
        }

        public virtual bool IsResolvable(Type contractType)
        {
            return Contracts != null && Contracts.Any(c => contractType.IsAssignableFrom(c.Type));
        }

        public abstract object GetComponent(ContractIdentity contract, IComposer dependencyResolver);
        public abstract object CreateComponent(ContractIdentity contract, IComposer dependencyResolver);

        protected virtual void ReadContractsFromTarget()
        {
            if (TargetType == null)
                return;
            
            foreach (var contractType in ComponentContextUtils.FindContracts(TargetType) ?? Enumerable.Empty<Type>())
            {
                AddContractType(contractType);
            }
        }

        protected virtual void EnsureComponentAttribute()
        {
            if (!ComponentContextUtils.HasComponentAttribute(TargetType))
                throw new CompositionException("The type '" + TargetType +
                                               "' is not a component, but it is being registered as one. Only classes marked with [Component] attribute can be registered.");
        }
        
        protected void EnsureNotInitialized()
        {
            if (RegistrationContext != null)
                throw new InvalidOperationException("ComponentRegistration cannot be changed after being registered " +
                                                    "in a ComponentContext.");
        }

        protected void EnsureContracts()
        {
            if (_contracts.Any())
                return;

            ReadContractsFromTarget();

            if (!_contracts.Any())
                throw new CompositionException("No contracts found for the component registration " + this);
        }

        protected void EnsureContractAttribute()
        {
            foreach (var contract in _contracts)
            {
                ComponentContextUtils.ThrowIfNotContract(contract.Type);
            }
        }
    }
}