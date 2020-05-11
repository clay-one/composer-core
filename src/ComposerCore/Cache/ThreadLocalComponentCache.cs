using System;
using System.Collections.Concurrent;
using System.Threading;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Component(nameof(ThreadLocalComponentCache)), ComponentCache(typeof(StaticComponentCache)), ConstructorResolutionPolicy(null)]
    public class ThreadLocalComponentCache : IComponentCache
    {
        private readonly ThreadLocal<ConcurrentDictionary<IComponentRegistration, object>> _cacheContent =
            new ThreadLocal<ConcurrentDictionary<IComponentRegistration, object>>(() =>
                new ConcurrentDictionary<IComponentRegistration, object>());

        [CompositionConstructor]
        public ThreadLocalComponentCache()
        {
        }

        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer scope)
        {
            return _cacheContent.Value.GetOrAdd(registration, r =>
            {
                var component = r.CreateComponent(contract, scope);
                if (component is IDisposable disposable)
                    registration.RegistrationContext.TrackDisposable(disposable);

                return component;
            });
        }
    }
}