using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

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
        
        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer dependencyResolver)
        {
            return registration.CreateComponent(contract, dependencyResolver);
        }
    }
}