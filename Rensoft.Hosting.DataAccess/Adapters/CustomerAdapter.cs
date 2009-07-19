using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    [RhspAdapterUsage("Customer")]
    public class CustomerAdapter : RhspAdapter
    {
        public void Create(Customer customer)
        {
            Run("Create", new { customer });
        }

        public void Update(Customer customer)
        {
            Run("Update", new { customer });
        }

        public CustomerDeleteResult Delete(RhspDataID dataID)
        {
            return Run<CustomerDeleteResult>("Delete", new { dataID });
        }

        public bool ExistsWithCode(Customer checkCustomer)
        {
            return Run<bool>("ExistsWithCode", new { checkCustomer });
        }

        public Customer Get(RhspDataID dataID)
        {
            return Run<Customer>("Get", new { dataID });
        }

        public Customer[] GetAll()
        {
            return Run<Customer[]>("GetAll");
        }

        public bool HasWebsites(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
