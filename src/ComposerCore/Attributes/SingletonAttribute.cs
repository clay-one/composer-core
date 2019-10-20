using System;
using ComposerCore.Cache;

namespace ComposerCore.Attributes
{
    /// <summary>
    /// Syntactic sugar for setting the component cache to ContractAgnosticComponentCache.
    /// Causes the decorated component to be instantiated only once (per registration) in the container.
    /// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SingletonAttribute : ComponentCacheAttribute
    {
        public SingletonAttribute() : base(typeof(PerRegistrationComponentCache))
        {
        }
    }
}