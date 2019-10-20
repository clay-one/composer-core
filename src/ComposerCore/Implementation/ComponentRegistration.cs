using System;
using System.Collections.Generic;
using System.Linq;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation
{
    public class ComponentRegistration
    {
        private readonly List<ContractIdentity> _contracts;

        public IComponentFactory Factory { get; }
        public ICompositionalQuery CacheQuery { get; protected set; }
        public IComponentCache Cache { get; protected set; }
        public string DefaultContractName { get; protected set; }
        public IComponentContext RegistrationContext { get; protected set; }
        
        public IEnumerable<ContractIdentity> Contracts => _contracts;

        public ComponentRegistration(IComponentFactory factory, string cacheComponentName = null)
        {
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
            SetCache(cacheComponentName);
            DefaultContractName = ComponentContextUtils.GetComponentDefaultName(factory.TargetType);
            
            _contracts = new List<ContractIdentity>();
        }
        
        public ComponentRegistration(IComponentFactory factory, IComponentCache cache)
            : this(factory)
        {
            SetCache(cache);
        }
        
        internal void SetAsRegistered(IComponentContext context)
        {
            EnsureNotInitialized();
            EnsureContracts();
            if (!context.Configuration.DisableAttributeChecking)
                EnsureContractAttribute();

            RegistrationContext = context;
            Factory.Initialize(context);
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
            AddContract(new ContractIdentity(contractType, DefaultContractName));
        }
        
        public void AddContract(ContractIdentity contract)
        {
            EnsureNotInitialized();

            if (!Factory.ValidateContractType(contract.Type))
                throw new CompositionException("This component type / factory cannot be registered with the contract " +
                                               $"{contract.Type.FullName}. The component type is not assignable to " +
                                               $"the contract or the factory logic prevents such registration.");
            
            _contracts.Add(contract);
        }
        
        public object GetComponent(ContractIdentity identity, IComposer dependencyResolver)
        {
            FillCache(dependencyResolver);
            return Cache.GetComponent(identity, this, dependencyResolver);
        }

        private void EnsureNotInitialized()
        {
            if (RegistrationContext != null)
                throw new InvalidOperationException("ComponentRegistration cannot be changed after being registered " +
                                                    "in a ComponentContext.");
        }

        private void EnsureContracts()
        {
            if (_contracts.Any())
                return;

            foreach (var contractType in Factory.GetContractTypes() ?? Enumerable.Empty<Type>())
            {
                AddContractType(contractType);
            }

            if (!_contracts.Any())
                throw new CompositionException("No contracts found for the component factory " + Factory);
        }

        private void EnsureContractAttribute()
        {
            foreach (var contract in _contracts)
            {
                ComponentContextUtils.ThrowIfNotContract(contract.Type);
            }
        }
        
        private void FillCacheQuery()
        {
            if (CacheQuery != null)
                return;
            
            // For backward compatibility, the default cache type is set to DefaultComponentCache when there
            // is no [ComponentCache] attribute present on the component.

            var attribute = ComponentContextUtils.GetComponentCacheAttribute(Factory.TargetType);
            var name = attribute == null ? nameof(DefaultComponentCache) : attribute.ComponentCacheName;
            
            SetCache(name);
        }

        private void FillCache(IComposer dependencyResolver)
        {
            if (Cache != null)
                return;
            
            FillCacheQuery();
            Cache ??= CacheQuery.Query(dependencyResolver) as IComponentCache
                      ?? throw new CompositionException("Could not resolve cache component for factory " +
                                                        $"{Factory}.");
        }
    }
}