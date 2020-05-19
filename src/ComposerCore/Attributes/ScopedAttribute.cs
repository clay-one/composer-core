using System;
using ComposerCore.Cache;

namespace ComposerCore.Attributes
{
    /// <summary>
    /// Syntactic sugar for setting the component cache to ScopedComponentCache.
    /// Causes the decorated component to be instantiated once per scope (instance of IComposer created for scoping) in the container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScopedAttribute : ComponentCacheAttribute
    {
        public ScopedAttribute() : base(typeof(ScopedComponentCache))
        {
        }
    }
}