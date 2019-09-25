using System;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.Explicit)), Singleton]
    public class ExplicitConstructorResolver : IConstructorResolver
    {
        public ConstructorInfo Resolve(Type targetType)
        {
            throw new NotImplementedException();
        }
    }
}