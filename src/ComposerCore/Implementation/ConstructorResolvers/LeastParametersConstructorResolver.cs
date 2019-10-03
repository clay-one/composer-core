using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComposerCore.Attributes;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.LeastParameters)), Singleton]
    [ConstructorResolutionPolicy(ConstructorResolutionPolicy.DefaultConstructor)]
    public class LeastParametersConstructorResolver : ExplicitConstructorResolver
    {
        protected override ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors)
        {
            return base.Resolve(targetType, candidateConstructors) ??
                   FindLeastParametersConstructor(candidateConstructors);
        }
        
        protected ConstructorInfo FindLeastParametersConstructor(IEnumerable<ConstructorInfo> candidateConstructors)
        {
            var constructors = candidateConstructors.OrderBy(c => c.GetParameters().Length).ToArray();

            if (constructors.Length > 1 &&
                constructors[0].GetParameters().Length == constructors[1].GetParameters().Length)
            {
                throw new CompositionException("Multiple constructors are matched with the LeastParameters policy");
            }
            
            return constructors.FirstOrDefault();
        }
    }
}