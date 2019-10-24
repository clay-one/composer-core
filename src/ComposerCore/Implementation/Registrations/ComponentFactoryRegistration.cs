using System;
using System.Linq;
using ComposerCore.Cache;
using ComposerCore.Extensibility;
using ComposerCore.Utility;

namespace ComposerCore.Implementation
{
    public class ComponentFactoryRegistration : ComponentBuilderRegistration
    {
        public IComponentFactory Factory { get; }
        
        public ComponentFactoryRegistration(IComponentFactory factory) : base(factory?.TargetType)
        {
            Factory = factory;
            if (TargetType.IsOpenGenericType())
                throw new InvalidOperationException("This class is not designed for open generic types. Use " +
                                                    "GenericComponentRegistration instead.");
        }

        public ComponentFactoryRegistration(IComponentFactory factory, string componentCacheName)
            : this(factory)
        {
            SetCache(componentCacheName);
        }
        
        public ComponentFactoryRegistration(IComponentFactory factory, IComponentCache cache)
            : this(factory)
        {
            SetCache(cache);
        }

        public override void SetAsRegistered(IComponentContext registrationContext)
        {
            base.SetAsRegistered(registrationContext);
            Factory.Initialize(registrationContext);
        }

        public override object GetComponent(ContractIdentity identity, IComposer dependencyResolver)
        {
            FillCache(dependencyResolver);
            return Cache.GetComponent(identity, this, dependencyResolver);
        }

        public override object CreateComponent(ContractIdentity contract, IComposer dependencyResolver)
        {
            return Factory.GetComponentInstance(contract);
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