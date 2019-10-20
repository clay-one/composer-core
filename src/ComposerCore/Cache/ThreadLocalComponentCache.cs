using System.Collections.Concurrent;
using System.Threading;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Cache
{
    [Component(nameof(ThreadLocalComponentCache)), ComponentCache(typeof(StaticComponentCache)), ConstructorResolutionPolicy(null)]
    public class ThreadLocalComponentCache : IComponentCache
    {
        private readonly ThreadLocal<ConcurrentDictionary<ComponentRegistration, object>> _cacheContent =
            new ThreadLocal<ConcurrentDictionary<ComponentRegistration, object>>(() =>
                new ConcurrentDictionary<ComponentRegistration, object>());

        [CompositionConstructor]
        public ThreadLocalComponentCache()
        {
        }

        public object GetComponent(ContractIdentity contract, ComponentRegistration registration, IComposer dependencyResolver)
        {
            return _cacheContent.Value.GetOrAdd(registration, r => r.Factory.GetComponentInstance(contract));
        }
    }
}