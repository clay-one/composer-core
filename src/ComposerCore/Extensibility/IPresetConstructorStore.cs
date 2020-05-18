using System;
using System.Reflection;
using ComposerCore.Attributes;

namespace ComposerCore.Extensibility
{
    [Contract]
    public interface IPresetConstructorStore
    {
        ConstructorInfo GetConstructor(Type targetType);
        void SetConstructor(Type targetType, ConstructorInfo constructorInfo);
    }
}