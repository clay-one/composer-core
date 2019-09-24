using System;

namespace ComposerCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ConstructorResolutionPolicyAttribyte : Attribute
    {
        public ConstructorResolutionPolicyAttribyte(ConstructorResolutionPolicy policy)
        {
            this.ConstructorResolutionPolicy = policy;
        }

        public ConstructorResolutionPolicy ConstructorResolutionPolicy { get; }
    }
}