using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rensoft.Hosting.Server.Managers;
using System.IO;
using System.Net;
using Rensoft.Net;
using System.Threading;
using Rensoft.Hosting.Server;
using Rensoft.Hosting.Server.Data;

namespace Rensoft.Hosting.Test
{
    [TestClass]
    public class FileZillaTests : TestBase
    {
        [TestMethod]
        public void CrudGroupAndUser()
        {
            FileZillaManager manager = LocalContext.Default.CreateManager<FileZillaManager>();
            string homePath = Path.Combine(@"C:\Websites", RhspDataID.Generate().ToString());
            string group = "test-group-" + Guid.NewGuid().ToString();
            string userName = "test-user-" + Guid.NewGuid().ToString();
            string password = "test";
            string uploadText = "Hello world!";
            string ftpServer = "192.168.10.10"; // FileZilla doesn't like localhost or 127.0.0.1
            string uploadFileName = "Test.txt";
            string downloadText = string.Empty;
            string websiteDirectory = "test-website.com";
            string websitePath = Path.Combine(homePath, websiteDirectory);

            manager.CreateGroup(group, new DirectoryInfo(homePath));
            manager.CreateUser(userName, password, true, group);

            // Ensure that FTP test has something to write to.
            Directory.CreateDirectory(homePath);
            Directory.CreateDirectory(websitePath);

            // Don't allow users to create anything in base directory.
            FtpPermissions basePerms = new FtpPermissions();
            basePerms.DirectoryOpen = true;
            basePerms.DirectoryList = true;
            manager.SetGroupPermissions(group, new DirectoryInfo(homePath), basePerms);

            // Allow user to do anything in the website directory.
            manager.SetGroupPermissions(group, new DirectoryInfo(websitePath), FtpPermissions.AllowAll);

            bool ftpFailed = false;
            string ftpError = null;

            try
            {
                FtpClient client = new FtpClient(ftpServer, userName, password);
                client.Open();

                // Upload to mock website directory as no access is granted on root.
                client.ChangeWorkingDirectory(websiteDirectory);

                MemoryStream uploadStream = new MemoryStream();
                uploadStream.Write(Encoding.UTF8.GetBytes(uploadText), 0, uploadText.Length);
                client.Upload(uploadStream, uploadFileName, false);

                MemoryStream downloadStream = new MemoryStream();
                client.Download(uploadFileName, downloadStream);
                downloadText = Encoding.UTF8.GetString(downloadStream.ToArray());

                client.DeleteFile(uploadFileName);
                client.Close();
            }
            catch (Exception ex)
            {
                ftpFailed = true;
                ftpError = ex.Message;
            }

            // Clean up first before asserting.
            manager.DeleteGroup(group, true);

            // Delete now we're finished.
            Directory.Delete(homePath, true);

            if (ftpFailed)
            {
                Assert.Fail("FTP test failed: " + ftpError);
            }
            else
            {
                // Ensure that user can write to home directory.
                Assert.AreEqual(uploadText, downloadText);
            }
        }
    }
}
