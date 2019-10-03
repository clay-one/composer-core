using System;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.SingleOrDefault)), Singleton]
    [ConstructorResolutionPolicy(ConstructorResolutionPolicy.DefaultConstructor)]
    public class SingleOrDefaultConstructorResolver : DefaultConstructorResolver
    {
        protected override ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors)
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