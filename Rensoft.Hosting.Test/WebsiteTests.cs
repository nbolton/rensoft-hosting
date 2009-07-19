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
    public class WebsiteTests : TestBase
    {
        private WebsiteAdapter adapter;

        public WebsiteTests()
        {
            this.adapter = LocalContext.Default.CreateAdapter<WebsiteAdapter>();
        }

        [TestMethod]
        public void CrudTest()
        {
            Website w = new Website(RhspDataID.Generate())
            {
                HostArray = new WebsiteHost[]
                {
                    new WebsiteHost(RhspDataID.Generate()) { Name = "www.test.com", Port = WebsiteHost.DefaultHttpPort },
                    new WebsiteHost(RhspDataID.Generate()) { Name = "test.com", Port = WebsiteHost.DefaultHttpPort },
                }
            };

            adapter.Create(w);

            Website w2 = adapter.Get(w.DataID);

            w2.HostArray = new WebsiteHost[] 
            { 
                new WebsiteHost(RhspDataID.Generate()) { Name = "foobar", Port = WebsiteHost.DefaultHttpPort } 
            };

            adapter.Update(w2);

            Website w3 = adapter.Get(w.DataID);
            Assert.AreEqual(w2.HostArray.Length, w3.HostArray.Length);
            Assert.AreEqual(w2.HostArray[0].Name, w3.HostArray[0].Name);

            adapter.Delete(w.DataID);
        }
    }
}
