using System;

namespace ComposerCore.Extensibility
{
    public interface IComponentRegistration
    {
        bool IsResolvable(Type contractType);
        object GetComponent(ContractIdentity identity, IComposer dependencyResolver);
    }
}