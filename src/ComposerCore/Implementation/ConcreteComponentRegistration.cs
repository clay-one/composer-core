using System;
using System.Linq;
using ComposerCore.Cache;
using ComposerCore.Extensibility;
using ComposerCore.Utility;

namespace ComposerCore.Implementation
{
    public class ConcreteComponentRegistration : ComponentBuilderRegistration
    {
        public IComponentFactory Factory { get; }
        
        public ConcreteComponentRegistration(IComponentFactory factory) : base(factory?.TargetType)
        {
            Factory = factory;
            if (TargetType.IsOpenGenericType())
                throw new InvalidOperationException("This class is not designed for open generic types. Use " +
                                                    "GenericComponentRegistration instead.");
        }

        public ConcreteComponentRegistration(IComponentFactory factory, string componentCacheName)
            : this(factory)
        {
            SetCache(componentCacheName);
        }
        
        public ConcreteComponentRegistration(IComponentFactory factory, IComponentCache cache)
            : this(factory)
        {
            SetCache(cache);
        }

        public override void SetAsRegistered(IComponentContext registrationContext)
        {
            base.SetAsRegistered(registrationContext);
            Factory.Initialize(registrationContext);
        }

        public override void AddContract(ContractIdentity contract)
        {
            EnsureNotInitialized();

            if (TargetType != null && !contract.Type.IsAssignableFrom(TargetType))
                throw new CompositionException("This component type / factory cannot be registered with the contract " +
                                               $"{contract.Type.FullName}. The component type is not assignable to " +
                                               $"the contract or the factory logic prevents such registration.");
            
            _contracts.Add(contract);
        }
        

        public override bool IsResolvable(Type contractType)
        {
            return Contracts != null && Contracts.Any(c => contractType.IsAssignableFrom(c.Type));
        }

        public override object GetComponent(ContractIdentity identity, IComposer dependencyResolver)
        {
            FillCache(dependencyResolver);
            return Cache.GetComponent(identity, this, dependencyResolver);
        }

        protected override void ReadContractsFromTarget()
        {
            foreach (var contractType in Factory.GetContractTypes() ?? Enumerable.Empty<Type>())
            {
                AddContractType(contractType);
            }
        }

        protected override void EnsureComponentAttribute()
        {
            if (!ComponentContextUtils.HasComponentAttribute(Factory.TargetType))
                throw new CompositionException("The type '" + Factory.TargetType +
                                               "' is not a component, but it is being registered as one. Only classes marked with [Component] attribute can be registered.");
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