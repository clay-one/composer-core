using System;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component(nameof(ConstructorResolutionPolicy.Preset)), Contract, Singleton, ConstructorResolutionPolicy(null)]
    public class PresetConstructorResolver : IConstructorResolver
    {
        private readonly IComposer _composer;
        
        [CompositionConstructor]
        public PresetConstructorResolver(IComposer composer)
        {
            _composer = composer;
        }

        public ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors,
            ConstructorArgSpecification[] preConfiguredArgs)
        {
            return _composer.GetComponent<IPresetConstructorStore>().GetConstructor(targetType);
        }
    }
}