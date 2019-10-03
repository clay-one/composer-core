using System;
using System.Linq;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.Explicit)), Singleton, ConstructorResolutionPolicy(null)]
    public class ExplicitConstructorResolver : IConstructorResolver
    {
        [CompositionConstructor]
        public ExplicitConstructorResolver()
        {
        }
        
        public ConstructorInfo Resolve(Type targetType)
        {
            var candidateConstructors = targetType.GetConstructors();
            return Resolve(targetType, candidateConstructors);
        }
        
        protected virtual ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors)
        {
            return FindMarkedConstructor(targetType, candidateConstructors);
        }

        protected ConstructorInfo FindMarkedConstructor(Type targetType, ConstructorInfo[] candidateConstructors)
        {
            var markedConstructors =
                candidateConstructors.Where(ComponentContextUtils.HasCompositionConstructorAttribute).ToArray();

            if (markedConstructors.Length == 0)
                return null;

            if (markedConstructors.Length > 1)
                throw new CompositionException("The type '" + targetType.FullName +
                                               "' has more than one constructor marked with [CompositionConstructor] attribute.");

            return markedConstructors[0];
        }
    }
}