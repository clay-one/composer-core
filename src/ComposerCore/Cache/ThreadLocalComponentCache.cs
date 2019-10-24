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
        private readonly ThreadLocal<ConcurrentDictionary<ConcreteComponentRegistration, object>> _cacheContent =
            new ThreadLocal<ConcurrentDictionary<ConcreteComponentRegistration, object>>(() =>
                new ConcurrentDictionary<ConcreteComponentRegistration, object>());

        [CompositionConstructor]
        public ThreadLocalComponentCache()
        {
        }

        public object GetComponent(ContractIdentity contract, ConcreteComponentRegistration registration, IComposer dependencyResolver)
        {
            return _cacheContent.Value.GetOrAdd(registration, r => r.Factory.GetComponentInstance(contract));
        }
    }
}