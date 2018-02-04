using ComposerCore.Definitions;
using ComposerCore.Definitions.Cache;

namespace ComposerCore.Tests.ComponentCaching.Components
{
    [Contract]
    [Component]
    [ComponentCache(typeof(ThreadLocalComponentCache))]
    public class ThreadLocalComponent : ISomeContract, IAnotherContract
    {
        
    }
}