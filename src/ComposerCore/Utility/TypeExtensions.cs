using System;
using System.Collections.Generic;

namespace ComposerCore.Utility
{
    public static class TypeExtensions
    {
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        public static bool IsOpenGenericType(this Type type)
        {
            return type.ContainsGenericParameters && type.IsGenericType;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type, bool includeSelf = false)
        {
            if (!includeSelf)
                type = type.BaseType;

            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}