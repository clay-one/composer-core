using System;
using System.Collections.Generic;
using ComposerCore.Attributes;
using ComposerCore.Implementation;

namespace ComposerCore.Extensibility
{
    [Contract]
    public interface ICompositionListenerChain
    {
        void RegisterCompositionListener(string name, ICompositionListener listener);
        void UnregisterCompositionListener(string name);

        object NotifyCreated(object originalComponentInstance, ContractIdentity contract, IComponentFactory factory, 
            Type targetType);

        void NotifyComposed(object componentInstance, object originalComponentInstance,
            List<object> initializationPointResults, ContractIdentity contract,
            IEnumerable<InitializationPointSpecification> initializationPoints, Type targetType);

        object NotifyRetrieved(object componentInstance, object originalComponentInstance, ContractIdentity contract,
            IComponentFactory factory, Type targetType);
    }
}