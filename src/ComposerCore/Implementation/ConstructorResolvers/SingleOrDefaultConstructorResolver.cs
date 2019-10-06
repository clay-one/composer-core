using System;
using System.Reflection;
using ComposerCore.Attributes;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.SingleOrDefault)), Singleton]
    [ConstructorResolutionPolicy(ConstructorResolutionPolicy.DefaultConstructor)]
    public class SingleOrDefaultConstructorResolver : DefaultConstructorResolver
    {
        public override ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors, ConstructorArgSpecification[] preConfiguredArgs)
        {
            return FindMarkedConstructor(targetType, candidateConstructors) ??
                   FindSingleConstructor(candidateConstructors) ??
                   FindDefaultConstructor(candidateConstructors);
        }
        
        private static ConstructorInfo FindSingleConstructor(ConstructorInfo[] candidateConstructors)
        {
            return candidateConstructors.Length == 1 ? candidateConstructors[0] : null;
        }
    }
}