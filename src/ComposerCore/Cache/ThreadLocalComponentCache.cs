using System;
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
        private readonly ThreadLocal<ConcurrentDictionary<IComponentRegistration, object>> _cacheContent =
            new ThreadLocal<ConcurrentDictionary<IComponentRegistration, object>>(() =>
                new ConcurrentDictionary<IComponentRegistration, object>());

        private readonly IComposer _composer;

        [CompositionConstructor]
        public ThreadLocalComponentCache(IComposer composer)
        {
            _composer = composer ?? throw new ArgumentNullException(nameof(composer));
        }

        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer scope)
        {
            return _cacheContent.Value.GetOrAdd(registration, r => r.CreateComponent(contract, scope));
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}