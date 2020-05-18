using System;
using System.Collections.Generic;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation
{
    [Contract, Singleton]
    public class CompositionListenerChain : ICompositionListenerChain
    {
        private readonly Dictionary<string, ICompositionListener> _compositionListeners;

        public CompositionListenerChain()
        {
            _compositionListeners = new Dictionary<string, ICompositionListener>();
        }

        public void RegisterCompositionListener(string name, ICompositionListener listener)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if ((_compositionListeners.ContainsKey(name)) && (listener != null))
                throw new ArgumentException($"Another composition listener with the name '{name}' is already registered.");

            if (listener == null)
                _compositionListeners.Remove(name);
            else
                _compositionListeners[name] = listener;
        }

        public void UnregisterCompositionListener(string name)
        {
            RegisterCompositionListener(name, null);
        }

        public object NotifyCreated(object originalComponentInstance, ContractIdentity contract, 
            IComponentFactory factory, Type targetType)
        {
            var componentInstance = originalComponentInstance;

            foreach (var compositionListener in _compositionListeners.Values)
            {
                compositionListener.OnComponentCreated(contract, factory, targetType, ref componentInstance, originalComponentInstance);
            }

            return componentInstance;
        }

        public void NotifyComposed(object componentInstance, object originalComponentInstance, List<object> initializationPointResults,
            ContractIdentity contract, IEnumerable<InitializationPointSpecification> initializationPoints, Type targetType)
        {
            foreach (var compositionListener in _compositionListeners.Values)
            {
                compositionListener.OnComponentComposed(contract, initializationPoints, initializationPointResults, targetType,
                    componentInstance, originalComponentInstance);
            }
        }

        public object NotifyRetrieved(object componentInstance, object originalComponentInstance, ContractIdentity contract,
            IComponentFactory factory, Type targetType)
        {
            // The component is ready to be delivered.
            // Inform composition listeners about the retrieval.

            var result = componentInstance;

            foreach (var compositionListener in _compositionListeners.Values)
            {
                compositionListener.OnComponentRetrieved(contract, factory, targetType, ref result, originalComponentInstance);
            }

            return result;
        }
    }
}