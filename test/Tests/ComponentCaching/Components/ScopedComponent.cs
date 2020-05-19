using System;
using ComposerCore.Attributes;

namespace ComposerCore.Tests.ComponentCaching.Components
{
    [Contract, Component, Scoped]
    public class ScopedComponent : ISomeContract, IAnotherContract, IDisposable
    {
        public bool Disposed { get; private set; }
        public void Dispose()
        {
            // Check to make sure Dispose is not called multiple times
            if (Disposed)
                throw new InvalidOperationException("Already disposed");
            
            Disposed = true;
        }
    }

    [Contract, Component, Scoped]
    public class ScopedComponentWithPlugs
    {
        [ComponentPlug]
        public IComposer Scope { get; set; }
        
        [ComponentPlug]
        public ScopedComponent ScopedComponent { get; set; }
        
        [ComponentPlug]
        public ContractAgnosticComponent ContractAgnosticComponent { get; set; }

        [ComponentPlug]
        public DefaultCacheComponent DefaultCacheComponent { get; set; }

        [ComponentPlug]
        public UncachedComponent UncachedComponent { get; set; }
    }
}