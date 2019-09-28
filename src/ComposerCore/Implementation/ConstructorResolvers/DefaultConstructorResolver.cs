using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.DefaultConstructor)), Singleton]
    public class DefaultConstructorResolver : ExplicitConstructorResolver, IConstructorResolver
    {
        protected override ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors)
        {
            return base.Resolve(targetType, candidateConstructors) ??
                   FindDefaultConstructor(candidateConstructors);
        }
        
        protected ConstructorInfo FindDefaultConstructor(IEnumerable<ConstructorInfo> candidateConstructors)
        {
            return candidateConstructors.FirstOrDefault(c => c.GetParameters().Length == 0);
        }
    }
}