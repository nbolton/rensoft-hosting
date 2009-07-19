using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Rensoft.Hosting.Server
{
    public static class Extensions
    {
        public static TValue GetValue<TValue>(this SqlDataReader reader, string column)
        {
            return GetValue<TValue>(reader, column, false);
        }

        public static TValue GetValue<TValue>(this SqlDataReader reader, string column, bool allowDefault)
        {
            if (reader[column] is DBNull)
            {
                if (allowDefault)
                {
                    return default(TValue);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Reader column value was null.");
                }
            }
            else
            {
                return (TValue)reader[column];
            }
        }

        public static TValue GetElementValue<TValue>(this XElement element, string name, bool allowDefault)
        {
            if ((element.Element(name) != null)
                && !string.IsNullOrEmpty(element.Element(name).Value))
            {
                if (typeof(TValue).IsEnum)
                {
                    return (TValue)Enum.Parse(typeof(TValue), element.Element(name).Value);
                }
                else if (typeof(TValue) == typeof(RhspDataID))
                {
                    return (TValue)(object)new RhspDataID(element.Element(name).Value);
                }
                else
                {
                    return (TValue)Convert.ChangeType(element.Element(name).Value, typeof(TValue));
                }
            }
            else
            {
                if (allowDefault)
                {
                    return default(TValue);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Value for element with name '" + name + "' does not exist.");
                }
            }
        }

        public static TValue GetAttributeValue<TValue>(this XElement element, string name, bool allowDefault)
        {
            if ((element.Attribute(name) != null) 
                && !string.IsNullOrEmpty(element.Attribute(name).Value))
            {
                return (TValue)Convert.ChangeType(element.Attribute(name).Value, typeof(TValue));
            }
            else
            {
                if (allowDefault)
                {
                    return default(TValue);
                }
                else
                {
                    throw new InvalidOperationException(
                        "Value for attribute with name '" + name + "' does not exist.");
                }
            }
        }
    }
}
