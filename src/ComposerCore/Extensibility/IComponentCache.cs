using System;
using ComposerCore.Attributes;
using ComposerCore.Implementation;

namespace ComposerCore.Extensibility
{
    [Contract]
    public interface IComponentCache : IDisposable
    {
        object GetComponent(ContractIdentity contract, IComponentRegistration registration, IComposer scope);
    }
}