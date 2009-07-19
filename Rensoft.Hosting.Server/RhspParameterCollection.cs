using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace Rensoft.Hosting.Server
{
    [DataContract]
    [DebuggerStepThrough]
    public class RhspParameterCollection
    {
        private Dictionary<string, RhspParameter> table;

        [DataMember]
        public Dictionary<string, RhspParameter> Table
        {
            get { return table; }
            set { table = value; }
        }

        public int Count
        {
            get { return table.Count; }
        }

        public RhspParameterCollection()
        {
            this.table = new Dictionary<string, RhspParameter>();
        }

        public RhspParameter this[string name]
        {
            get
            {
                if (!Exists(name))
                {
                    throw new RhspException(
                        "No parameter exists with name '" + name + "'.");
                }
                return table[name];
            }
        }

        public bool Exists(string name)
        {
            return table.ContainsKey(name);
        }

        public RhspParameter[] ToArray()
        {
            List<RhspParameter> list = new List<RhspParameter>();
            foreach (KeyValuePair<string, RhspParameter> kvp in table)
            {
                list.Add(kvp.Value);
            }
            return list.ToArray();
        }
    }
}
