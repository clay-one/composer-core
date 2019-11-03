using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Component(nameof(ContractAgnosticComponentCache)), Transient, ConstructorResolutionPolicy(null)]
    public class ScopedComponentCache : IComponentCache
    {
        [CompositionConstructor]
        public ScopedComponentCache()
        {
        }

        public object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer dependencyResolver)
        {
            throw new System.NotImplementedException();
        }
    }
}