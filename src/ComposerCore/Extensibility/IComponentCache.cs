using ComposerCore.Attributes;
using ComposerCore.Implementation;

namespace ComposerCore.Extensibility
{
    [Contract]
    public interface IComponentCache
    {
        object GetComponent(ContractIdentity contract, ComponentRegistration registration, IComposer dependencyResolver);
    }
}