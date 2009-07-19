using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Rensoft.Hosting.DataAccess
{
    public static class IEnumerableExtensions
    {
        public static object[] Cast(this object[] array, Type elementType)
        {
            MethodInfo castInfo = typeof(Enumerable).GetMethod("Cast");
            MethodInfo castGeneric = castInfo.MakeGenericMethod(elementType);
            object iterator = castGeneric.Invoke(null, new object[] { array });

            MethodInfo toArrayInfo = typeof(Enumerable).GetMethod("ToArray");
            MethodInfo toArrayGeneric = toArrayInfo.MakeGenericMethod(elementType);

            Type enumTypeGeneric = typeof(IEnumerable<>).MakeGenericType(elementType);
            MethodInfo dynamicCastInfo = typeof(IEnumerableExtensions).GetMethod(
                "dynamicCast", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo dynamicCastGeneric = dynamicCastInfo.MakeGenericMethod(enumTypeGeneric);

            object enumerable = dynamicCastGeneric.Invoke(null, new object[] { iterator });
            return (object[])toArrayGeneric.Invoke(null, new object[] { enumerable });
        }

        private static T dynamicCast<T>(object value)
        {
            return (T)value;
        }
    }
}
