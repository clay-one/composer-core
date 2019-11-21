using ComposerCore.Attributes;

namespace ComposerCore.Tests.Features.Components
{
    [Contract, Component, Singleton]
    public class NonDisposableSingletonComponent
    {
        public bool Disposed { get; private set; }
        
        public void Dispose()
        {
            Disposed = true;
        }
    }
}