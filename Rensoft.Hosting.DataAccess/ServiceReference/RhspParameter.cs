using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class RhspParameter
    {
        public void SetValue(object value)
        {
            if (value != null)
            {
                // Remember type which will be lost in serialization.
                DataTypeName = value.GetType().FullName;

                if (value is object[])
                {
                    RawValue = (object[])value;
                    DataMode = RhspDataMode.Array;
                }
                else
                {
                    RawValue = new object[] { value };
                    DataMode = RhspDataMode.Single;
                }
            }
        }
    }
}
