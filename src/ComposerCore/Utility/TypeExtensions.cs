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

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
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
        
        public static Type GetEnumerableTypeArgument(this Type type)
        {
            if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];
            
            foreach (Type interfaceType in type.GetInterfaces()) 
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) 
                {
                    return interfaceType.GetGenericArguments()[0];
                }
            }
            return null;
        }
    }
}