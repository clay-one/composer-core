using System;
using System.Collections.Concurrent;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Component(nameof(PerContractComponentCache)), Transient, ConstructorResolutionPolicy(null)]
    public class PerContractComponentCache : IComponentCache
    {
        private readonly ConcurrentDictionary<ContractIdentity, object> _cacheContent =
            new ConcurrentDictionary<ContractIdentity, object>();

        [CompositionConstructor]
        public PerContractComponentCache()
        {
        }
        
        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer scope)
        {
            return _cacheContent.GetOrAdd(contract, c =>
            {
                var component = registration.CreateComponent(c, scope);
                if (component is IDisposable disposable)
                    registration.RegistrationContext.TrackDisposable(disposable);

                return component;
            });
        }
    }
}