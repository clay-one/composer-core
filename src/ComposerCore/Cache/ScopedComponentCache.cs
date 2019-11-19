using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Component(nameof(ScopedComponentCache)), Transient, ConstructorResolutionPolicy(null)]
    public class ScopedComponentCache : IComponentCache
    {
        [CompositionConstructor]
        public ScopedComponentCache()
        {
        }

        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer dependencyResolver)
        {
            var store = dependencyResolver.GetComponent<ScopedComponentCacheStore>();
            return store.GetOrCreate(contract, registration, dependencyResolver);
        }
    }
}