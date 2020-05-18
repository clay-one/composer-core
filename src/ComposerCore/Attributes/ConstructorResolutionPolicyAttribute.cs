using System;

namespace ComposerCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ConstructorResolutionPolicyAttribute : Attribute
    {
        public ConstructorResolutionPolicyAttribute(ConstructorResolutionPolicy policy)
        {
            ConstructorResolutionPolicy = policy.ToString();
        }
        
        public ConstructorResolutionPolicyAttribute(string policy)
        {
            ConstructorResolutionPolicy = policy;
        }

        public string ConstructorResolutionPolicy { get; }
    }
}