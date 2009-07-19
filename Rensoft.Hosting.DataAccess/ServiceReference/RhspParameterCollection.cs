using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class RhspParameterCollection
    {
        public RhspParameterCollection()
        {
            this.Table = new Dictionary<string, RhspParameter>();
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
                return Table[name];
            }
        }

        public void Add(RhspParameter parameter)
        {
            Table[parameter.Name] = parameter;
        }

        public RhspParameter Add(string name, object value)
        {
            RhspParameter parameter = new RhspParameter();
            parameter.Name = name;
            parameter.SetValue(value);
            Add(parameter);
            return parameter;
        }

        public bool Exists(string name)
        {
            return Table.ContainsKey(name);
        }
    }
}
