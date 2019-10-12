using System.Collections.Generic;
using System.Threading;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Cache
{
    [Contract, Component, ComponentCache(typeof(StaticComponentCache)), ConstructorResolutionPolicy(null)]
    public class ThreadLocalComponentCache
    {
        [CompositionConstructor]
        public ThreadLocalComponentCache()
        {
        }
    }
}