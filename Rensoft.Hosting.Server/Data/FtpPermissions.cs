using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.Server.Data
{
    public class FtpPermissions
    {
        public bool FileRead { get; set; }
        public bool FileWrite { get; set; }
        public bool FileDelete { get; set; }
        public bool FileAppend { get; set; }
        public bool DirectoryCreate { get; set; }
        public bool DirectoryDelete { get; set; }
        public bool DirectoryList { get; set; }
        public bool DirectoryOpen { get; set; }

        public static FtpPermissions AllowAll
        {
            get
            {
                FtpPermissions p = new FtpPermissions();
                p.FileRead = true;
                p.FileWrite = true;
                p.FileAppend = true;
                p.FileDelete = true;
                p.DirectoryCreate = true;
                p.DirectoryDelete = true;
                p.DirectoryOpen = true;
                p.DirectoryList = true;
                return p;
            }
        }
    }
}
