using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Rensoft.Hosting.Server.Managers.Config;
using Rensoft.Hosting.Server.Managers;
using System.Xml.Linq;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    public abstract class RhspData
    {
        /// <summary>
        /// The first version of an object structure.
        /// </summary>
        public const int FirstSchemaVersion = 1;

        [DataMember]
        public RhspDataID DataID { get; set; }

        protected int GetSchemaVersion(HostingConfigManager manager)
        {
            return manager.GetDataSchemaVersion(DataID, GetType());
        }

        protected RhspDataAttribute GetRuntimeHda()
        {
            return GetRuntimeHda(GetType());
        }

        public static RhspDataAttribute GetRuntimeHda(Type type)
        {
            var query = from attribute in type.GetCustomAttributes(true)
                        where attribute is RhspDataAttribute
                        select attribute;

            if (query.Count() != 0)
            {
                // Single should be OK as multiple not allowed for attribute.
                return (RhspDataAttribute)query.Single();
            }
            else
            {
                throw new NotSupportedException(
                    "The data type '" + type.Name + "' does not use the " +
                    "required attribute, " + typeof(RhspDataAttribute) + ".");
            }
        }

        public static bool IsDeleteOrDiscard(IRhspDataChild dataChild)
        {
            return (dataChild.PendingAction == ChildPendingAction.Delete)
                || (dataChild.PendingAction == ChildPendingAction.Discard);
        }
    }
}
