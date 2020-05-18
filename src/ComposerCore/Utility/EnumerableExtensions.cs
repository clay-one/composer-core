using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ComposerCore.Utility
{
    public static class EnumerableExtensions
    {
        private static readonly MethodInfo CastMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast));
        
        public static IEnumerable<object> CastToRuntimeType(this IEnumerable<object> enumerable, Type elementType)
        {
            return CastMethod.MakeGenericMethod(elementType).Invoke(null, new object[] {enumerable}) 
                as IEnumerable<object>;
        }
    }
}