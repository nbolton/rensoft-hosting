using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class RhspCommandResponse
    {
        public bool ErrorOccured { get; protected set; }

        public static RhspCommandResponse Error
        {
            get
            {
                RhspCommandResponse r = new RhspCommandResponse();
                r.ErrorOccured = true;
                return r;
            }
        }

        public RhspCommandResponse() { }

        public T GetResult<T>()
        {
            if (typeof(T).IsArray)
            {
                if (ErrorOccured)
                {
                    // Return an empty array (instead of null).
                    return getEmptyArray<T>();
                }
                else
                {
                    return getArray<T>();
                }
            }
            else
            {
                if (ErrorOccured)
                {
                    return default(T);
                }
                else
                {
                    return getSingle<T>();
                }
            }
        }

        private T getEmptyArray<T>()
        {
            return (T)(object)(new object[0]).Cast(typeof(T).GetElementType());
        }

        private T getSingle<T>()
        {
            if (DataMode != RhspDataMode.Single)
            {
                throw new RhspException(
                    "Cannot get single data when mode is " + DataMode.ToString() + ".");
            }

            if (RawData[0] != null)
            {
                // When not null, OK to cast.
                return (T)RawData[0];
            }
            else
            {
                // Cannot cast null to T, so return default instead.
                return default(T);
            }
        }

        private T getArray<T>()
        {
            if (DataMode != RhspDataMode.Array)
            {
                throw new RhspException(
                    "Cannot get array data when mode is " + DataMode.ToString() + ".");
            }

            if (RawData != null)
            {
                return (T)(object)RawData.Cast(typeof(T).GetElementType());

                //return (T)(object)createArray(typeof(T).GetElementType(), RawData);

                //List<T> list = new List<T>();
                //foreach (object o in RawData)
                //{
                //    list.Add((T)o);
                //}
                //return list.ToArray();
            }
            else
            {
                return (T)(object)(new T[0]);
            }
        }

        //private object[] createArray(Type elementType, object[] elements)
        //{
        //    //Type listType = typeof(List<>).MakeGenericType(elementType);
        //    //object list = Activator.CreateInstance(listType);
        //    //MethodInfo addMethod = listType.GetMethod("Add").MakeGenericMethod(elementType);

        //    //MethodInfo castMethod = GetType().GetMethod(
        //    //    "cast", BindingFlags.NonPublic | BindingFlags.Static);

        //    //foreach (object o in elements)
        //    //{
        //    //    addMethod.Invoke(
        //    //        list,
        //    //        new object[] {
        //    //            castMethod.Invoke(null, new object[] { o })
        //    //        }
        //    //    );
        //    //}
        //    ////elements.ToList().ForEach(
        //    ////    o =>
        //    ////    {
        //    ////        addMethod.Invoke(list,
        //    ////            new object[] { castMethod.Invoke(null, new object[] { o }) });
        //    ////    }
        //    ////);

        //    //MethodInfo toArrayInfo = typeof(Enumerable).GetMethod("ToArray");
        //    //MethodInfo toArrayGeneric = toArrayInfo.MakeGenericMethod(elementType);

        //    //return (object[])toArrayGeneric.Invoke(null, new object[] { list });
        //}

        //private static T cast<T>(object value)
        //{
        //    return (T)value;
        //}

        //[Obsolete("Use GetResult<T> instead, with T is an array type.")]
        //public T[] GetArray<T>()
        //{
        //    if (DataMode != RhspDataMode.Array)
        //    {
        //        throw new RhspException(
        //            "Cannot get array data when mode is " + DataMode.ToString() + ".");
        //    }

        //    List<T> list = new List<T>();
        //    if (RawData == null)
        //    {
        //        // Where results empty, return empty list.
        //        return list.ToArray();
        //    }

        //    foreach (object o in RawData)
        //    {
        //        list.Add((T)o);
        //    }
        //    return list.ToArray();
        //}
    }
}
