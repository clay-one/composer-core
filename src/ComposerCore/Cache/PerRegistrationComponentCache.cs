using System.Threading;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Component(nameof(PerRegistrationComponentCache)), Transient, ConstructorResolutionPolicy(null)]
    public class PerRegistrationComponentCache : IComponentCache
    {
        private readonly object _syncKey = new object();
        private object _componentInstance;

        [CompositionConstructor]
        public PerRegistrationComponentCache()
        {
        }

        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer dependencyResolver)
        {
            if (_componentInstance != null)
                return _componentInstance;

            lock (_syncKey)
            {
                if (_componentInstance != null)
                    return _componentInstance;

                var result = registration.CreateComponent(contract, dependencyResolver);

                Thread.MemoryBarrier();
                _componentInstance = result;
                return result;
            }
        }
    }
}