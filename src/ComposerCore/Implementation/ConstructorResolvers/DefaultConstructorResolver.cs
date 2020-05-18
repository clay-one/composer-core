using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComposerCore.Attributes;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.DefaultConstructor)), Singleton, ConstructorResolutionPolicy(null)]
    public class DefaultConstructorResolver : ExplicitConstructorResolver
    {
        [CompositionConstructor]
        public DefaultConstructorResolver()
        {
        }
        
        public override ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors, ConstructorArgSpecification[] preConfiguredArgs)
        {
            return base.Resolve(targetType, candidateConstructors, preConfiguredArgs) ??
                   FindDefaultConstructor(candidateConstructors);
        }
        
        protected ConstructorInfo FindDefaultConstructor(IEnumerable<ConstructorInfo> candidateConstructors)
        {
            return candidateConstructors.FirstOrDefault(c => c.GetParameters().Length == 0);
        }
    }
}