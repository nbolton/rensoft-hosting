using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Collections;

namespace Rensoft.Hosting.Server
{
    [DataContract]
    [DebuggerStepThrough]
    public class RhspCommandResponse
    {
        [DataMember]
        public object[] RawData { get; set; }

        [DataMember]
        public RhspDataMode DataMode { get; set; }

        public void SetData(object data)
        {
            if (data is Array)
            {
                RawData = (object[])data;
                DataMode = RhspDataMode.Array;
            }
            else if (data is IList)
            {
                throw new NotSupportedException(
                    "Lists are not supported. Consider " +
                    "converting the list to an array.");
            }
            else
            {
                RawData = new object[] { data };
                DataMode = RhspDataMode.Single;
            }
        }
    }
}
