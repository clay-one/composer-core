using System;
using System.Linq;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Tests.CompositionByConstructor.Resolvers
{
    [Component(nameof(ConstructorResolutionPolicy.Custom))]
    [ConstructorResolutionPolicy(ConstructorResolutionPolicy.DefaultConstructor)]
    public class AnyConstructorWithMostParamsResolver : IConstructorResolver
    {
        public ConstructorInfo Resolve(Type targetType)
        {
            return targetType.GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
        }
    }
}