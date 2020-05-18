using System;
using System.Collections.Generic;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Extensibility;

namespace ComposerCore.Implementation.ConstructorResolvers
{
    [Component, Singleton]
    public class PresetConstructorStore : IPresetConstructorStore
    {
        private Dictionary<Type, ConstructorInfo> _constructors;
        
        [CompositionConstructor]
        public PresetConstructorStore()
        {
            _constructors = new Dictionary<Type, ConstructorInfo>();
        }

        public ConstructorInfo GetConstructor(Type targetType)
        {
            if (!_constructors.ContainsKey(targetType))
                return null;

            return _constructors[targetType];
        }

        public void SetConstructor(Type targetType, ConstructorInfo constructorInfo)
        {
            _constructors[targetType] = constructorInfo;
        }
    }
}