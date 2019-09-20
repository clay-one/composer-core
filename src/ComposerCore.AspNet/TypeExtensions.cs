using System;
using System.Collections.Generic;

namespace ComposerCore.Utility
{
    public static class TypeExtensions
    {
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