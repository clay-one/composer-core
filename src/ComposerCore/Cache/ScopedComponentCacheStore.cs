using System.Collections.Concurrent;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Contract, Component, Singleton]
    public class ScopedComponentCacheStore
    {
        private readonly ConcurrentDictionary<IComponentRegistration, object> _store;
        
        [CompositionConstructor]
        public ScopedComponentCacheStore()
        {
            _store = new ConcurrentDictionary<IComponentRegistration, object>();
        }

        public object GetOrCreate(ContractIdentity contractIdentity, IComponentRegistration registration, 
            IComposer dependencyResolver)
        {
            var result = _store.GetOrAdd(registration, r => r.CreateComponent(contractIdentity, dependencyResolver));
            return result;
        }
    }
}