using System;
using System.Collections.Generic;

namespace ComposerCore.Extensibility
{
    public interface IComponentRegistration
    {
        Type TargetType { get; }
        IEnumerable<ContractIdentity> Contracts { get; }

        void SetCache(string cacheComponentName);
        void SetCache(IComponentCache cache);
        void SetDefaultContractName(string defaultContractName);

        void AddContractType(Type contractType);
        void AddContract(ContractIdentity contract);
        void SetAsRegistered(IComponentContext registrationContext);
        
        bool IsResolvable(Type contractType);
        object GetComponent(ContractIdentity contract, IComposer scope);
        object CreateComponent(ContractIdentity contract, IComposer scope);
    }
}