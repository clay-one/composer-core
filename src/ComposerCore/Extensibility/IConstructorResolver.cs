using System;
using System.Reflection;
using ComposerCore.Attributes;

namespace ComposerCore.Extensibility
{
    [Contract]
    public interface IConstructorResolver
    {
        ConstructorInfo Resolve(Type targetType);
    }
}