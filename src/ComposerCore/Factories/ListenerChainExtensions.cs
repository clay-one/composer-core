using System.Collections.Generic;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Factories
{
    internal static class ListenerChainExtensions
    {
        public static object NotifyCreated(this IEnumerable<ICompositionListener> listenerChain, 
            object originalComponentInstance, ContractIdentity contract, LocalComponentFactoryBase factory)
        {
            var componentInstance = originalComponentInstance;

            foreach (var compositionListener in listenerChain)
            {
                compositionListener.OnComponentCreated(contract, factory, factory.TargetType, ref componentInstance, originalComponentInstance);
            }

            return componentInstance;
        }

        public static void NotifyComposed(this IEnumerable<ICompositionListener> listenerChain, 
            object componentInstance, object originalComponentInstance,
            List<object> initializationPointResults, ContractIdentity contract, IEnumerable<InitializationPointSpecification> initializationPoints, LocalComponentFactoryBase factory)
        {
            foreach (var compositionListener in listenerChain)
            {
                compositionListener.OnComponentComposed(contract, initializationPoints, initializationPointResults, factory.TargetType,
                    componentInstance, originalComponentInstance);
            }
        }

        public static object NotifyRetrieved(this IEnumerable<ICompositionListener> listenerChain, 
            object componentInstance, object originalComponentInstance, ContractIdentity contract, LocalComponentFactoryBase factory)
        {
            // The component is ready to be delivered.
            // Inform composition listeners about the retrieval.

            var result = componentInstance;

            foreach (var compositionListener in listenerChain)
            {
                compositionListener.OnComponentRetrieved(contract, factory, factory.TargetType, ref result, originalComponentInstance);
            }

            return result;
        }
    }
}