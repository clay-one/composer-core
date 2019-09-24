using System;

namespace ComposerCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ConstructorResolutionPolicyAttribute : Attribute
    {
        public ConstructorResolutionPolicyAttribute(ConstructorResolutionPolicy policy)
        {
            this.ConstructorResolutionPolicy = policy;
        }

        public ConstructorResolutionPolicy ConstructorResolutionPolicy { get; }
    }
}