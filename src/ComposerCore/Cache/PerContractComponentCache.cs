using System.Collections.Concurrent;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

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
        
        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer dependencyResolver)
        {
            return _cacheContent.GetOrAdd(contract, (c) => registration.CreateComponent(c, dependencyResolver));
        }
    }
}