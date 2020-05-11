using System;
using System.Collections.Concurrent;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Component(nameof(PerContractComponentCache)), Transient, ConstructorResolutionPolicy(null)]
    public class PerContractComponentCache : IComponentCache
    {
        private readonly IComposer _composer;
        
        private readonly ConcurrentDictionary<ContractIdentity, object> _cacheContent =
            new ConcurrentDictionary<ContractIdentity, object>();

        [CompositionConstructor]
        public PerContractComponentCache(IComposer composer)
        {
            _composer = composer ?? throw new ArgumentNullException(nameof(composer));
        }
        
        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer scope)
        {
            return _cacheContent.GetOrAdd(contract, c => registration.CreateComponent(c, scope));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}