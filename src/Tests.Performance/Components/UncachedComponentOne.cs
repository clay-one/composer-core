using ComposerCore.Attributes;

namespace ComposerCore.Tests.Performance.Components
{
    [Component]
    [ComponentCache(null)]
    public class UncachedComponentOne : IUncachedComponentOne
    {
        
    }
}