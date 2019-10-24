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
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
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

        public abstract void AddContract(ContractIdentity contract);
        public abstract bool IsResolvable(Type contractType);
        public abstract object GetComponent(ContractIdentity contract, IComposer dependencyResolver);

        protected abstract void ReadContractsFromTarget();
        protected abstract void EnsureComponentAttribute();
        
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