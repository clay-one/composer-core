using System;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Component(nameof(NoComponentCache)), Singleton, ConstructorResolutionPolicy(null)]
    public class NoComponentCache : IComponentCache
    {
        public static readonly NoComponentCache Instance = new NoComponentCache();
        
        [CompositionConstructor]
        public NoComponentCache()
        {
        }
        
        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer scope)
        {
            return registration.CreateComponent(contract, scope);
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}