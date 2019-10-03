using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ComposerCore.Attributes;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.MostResolvable)), Singleton]
    [ConstructorResolutionPolicy(ConstructorResolutionPolicy.DefaultConstructor)]
    public class MostResolvableConstructorResolver : ExplicitConstructorResolver
    {
        [ComponentPlug]
        public IComposer Composer { get; set; }
        
        protected override ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors)
        {
            return base.Resolve(targetType, candidateConstructors) ??
                   FindMostResolvableConstructor(candidateConstructors);
        }
        
        protected ConstructorInfo FindMostResolvableConstructor(IEnumerable<ConstructorInfo> candidateConstructors)
        {
            return candidateConstructors.OrderByDescending(c => c.GetParameters().Length).FirstOrDefault(IsResolvable);
        }

        private bool IsResolvable(ConstructorInfo constructorInfo)
        {
            var specs = ConstructorArgSpecification.BuildFrom(constructorInfo);
            return specs.All(s => s.IsResolvable(Composer));
        }
    }
}