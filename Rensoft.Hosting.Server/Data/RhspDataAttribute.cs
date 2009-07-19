using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.Server.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public class RhspDataAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Parent { get; private set; }
        public string ID { get; private set; }
        public int SchemaVersion { get; set; }

        public RhspDataAttribute(string name, string parent, string id)
        {
            this.Name = name;
            this.Parent = parent;
            this.ID = id;
        }
    }
}
