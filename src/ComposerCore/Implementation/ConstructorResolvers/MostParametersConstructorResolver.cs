using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComposerCore.Attributes;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.MostParameters)), Singleton]
    [ConstructorResolutionPolicy(ConstructorResolutionPolicy.DefaultConstructor)]
    public class MostParametersConstructorResolver : ExplicitConstructorResolver
    {
        public override ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors, ConstructorArgSpecification[] preConfiguredArgs)
        {
            return base.Resolve(targetType, candidateConstructors, preConfiguredArgs) ??
                   FindMostParametersConstructor(candidateConstructors);
        }
        
        protected ConstructorInfo FindMostParametersConstructor(IEnumerable<ConstructorInfo> candidateConstructors)
        {
            var constructors = candidateConstructors.OrderByDescending(c => c.GetParameters().Length).ToArray();

            if (constructors.Length > 1 &&
                constructors[0].GetParameters().Length == constructors[1].GetParameters().Length)
            {
                throw new CompositionException("Multiple constructors are matched with the MostParameters policy");
            }
            
            return constructors.FirstOrDefault();
        }
    }
}