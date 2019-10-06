using System;
using System.Collections.Generic;
using System.Reflection;
using ComposerCore.Attributes;
using ComposerCore.Implementation;

namespace ComposerCore.Extensibility
{
    [Contract]
    public interface IConstructorResolver
    {
        ConstructorInfo Resolve(Type targetType, ConstructorInfo[] candidateConstructors, ConstructorArgSpecification[] preConfiguredArgs);
    }
}