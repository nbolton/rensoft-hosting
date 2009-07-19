using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.Test
{
    [TestClass]
    public class CustomerTests : TestBase
    {
        private CustomerAdapter adapter;

        public CustomerTests()
        {
            this.adapter = LocalContext.Default.CreateAdapter<CustomerAdapter>();
        }

        [TestMethod]
        public void CrudTest()
        {
            Customer c = new Customer(RhspDataID.Generate())
            {
                Code = "TEST",
                Name = "Test Company"
            };

            adapter.Create(c);

            Customer c2 = adapter.Get(c.DataID);

            c2.Name += " TEST";
            adapter.Update(c2);

            Customer c3 = adapter.Get(c.DataID);
            Assert.AreEqual(c2.Name, c3.Name);

            adapter.Delete(c.DataID);
        }
    }
}
