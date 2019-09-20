using ComposerCore.Attributes;
using ComposerCore.Cache;

namespace ComposerCore.Tests.ComponentCaching.Components
{
    [Contract]
    [Component]
    [ComponentCache(typeof(ThreadLocalComponentCache))]
    public class ThreadLocalComponent : ISomeContract, IAnotherContract
    {
        
    }
}