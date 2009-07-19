using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;

namespace Rensoft.Hosting.Server
{
    [DataContract]
    [DebuggerStepThrough]
    public class RhspParameter
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public object[] RawValue { get; set; }

        [DataMember]
        public RhspDataMode DataMode { get; set; }

        [DataMember]
        public string DataTypeName { get; set; }

        public object Value
        {
            get
            {
                object result = null;
                switch (DataMode)
                {
                    case RhspDataMode.Single:
                        if ((RawValue != null) && (RawValue.Length != 0))
                        {
                            result = RawValue[0];
                        }
                        break;

                    case RhspDataMode.Array:
                        {
                            // Get the type of array.
                            Type arrayType = Type.GetType(DataTypeName);

                            // If type was not found, then assume non-system type.
                            if (arrayType == null)
                            {
                                throw new RhspException(
                                    "Cannot handle arrays of non-system type. Consider " +
                                    "wrapping non-system elements in a system type array " +
                                    "such as object[].");
                            }

                            // Ensure that array type was specified.
                            if (!arrayType.HasElementType)
                            {
                                throw new RhspException(
                                    "A non-array type was specified, which " +
                                    "is not allowed for array values.");
                            }

                            Type elementType = arrayType.GetElementType();
                            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                            MethodInfo method = GetType().GetMethod("convertArray", flags);
                            MethodInfo generic = method.MakeGenericMethod(elementType);
                            result = generic.Invoke(this, new object[] { RawValue });
                        }
                        break;
                }
                return result;
            }
        }

        private T[] convertArray<T>(object[] array)
        {
            List<T> list = new List<T>();
            foreach (object o in array)
            {
                list.Add((T)o);
            }
            return list.ToArray();
        }
    }
}
