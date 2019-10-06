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
        
        public override ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors, ConstructorArgSpecification[] preConfiguredArgs)
        {
            return base.Resolve(targetType, candidateConstructors, preConfiguredArgs) ??
                   FindMostResolvableConstructor(candidateConstructors, preConfiguredArgs);
        }
        
        protected ConstructorInfo FindMostResolvableConstructor(IEnumerable<ConstructorInfo> candidateConstructors, ConstructorArgSpecification[] preConfiguredArgs)
        {
            return candidateConstructors
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault(c => IsResolvable(c, preConfiguredArgs));
        }

        private bool IsResolvable(ConstructorInfo constructorInfo, ConstructorArgSpecification[] preConfiguredArgs)
        {
            var specs = ConstructorArgSpecification.BuildFrom(constructorInfo);
            var skipCount = preConfiguredArgs?.Length ?? 0;
            return specs.Skip(skipCount).All(s => s.IsResolvable(Composer));
        }
    }
}