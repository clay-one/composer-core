using ComposerCore.Attributes;

namespace ComposerCore.Extensibility
{
    [Contract]
    public interface IComponentCache
    {
        object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer scope);
    }
}