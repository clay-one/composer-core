using System;
using ComposerCore.Cache;

namespace ComposerCore.Attributes
{
    /// <summary>
    /// Syntactic sugar for setting the component cache to null.
    /// Causes the decorated component to be instantiated every time that it is queried from the container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TransientAttribute : ComponentCacheAttribute
    {
        public TransientAttribute() : base(typeof(NoComponentCache))
        {
        }
    }
}