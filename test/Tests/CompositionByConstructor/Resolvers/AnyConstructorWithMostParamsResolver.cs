using System;
using System.Linq;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;
using ComposerCore.Implementation;

namespace ComposerCore.Tests.CompositionByConstructor.Resolvers
{
    [Component(nameof(ConstructorResolutionPolicy.Custom))]
    [ConstructorResolutionPolicy(ConstructorResolutionPolicy.DefaultConstructor)]
    public class AnyConstructorWithMostParamsResolver : IConstructorResolver
    {
        public ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors, ConstructorArgSpecification[] preConfiguredArgs)
        {
            return candidateConstructors.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
        }
    }
}