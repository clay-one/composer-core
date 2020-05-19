using System;
using System.Collections.Concurrent;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Contract, Component, Singleton]
    public class ScopedComponentCacheStore
    {
        private readonly ConcurrentDictionary<IComponentRegistration, object> _store;
        private readonly IComponentContext _componentContext;
        
        [CompositionConstructor]
        public ScopedComponentCacheStore(IComponentContext componentContext)
        {
            _store = new ConcurrentDictionary<IComponentRegistration, object>();
            _componentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));
        }

        public object GetOrCreate(ContractIdentity contractIdentity, IComponentRegistration registration, IComposer scope)
        {
            var result = _store.GetOrAdd(registration, r =>
            {
                var component = r.CreateComponent(contractIdentity, scope);
                if (component is IDisposable disposable)
                    _componentContext.TrackDisposable(disposable);

                return component;
            });
            
            return result;
        }
    }
}