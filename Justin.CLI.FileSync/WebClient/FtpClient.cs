using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.FileSync
{
    internal class FtpClient : WebClient
    {
        public FtpClient(string userId, string password) 
            : base (userId, password) { }
        
        protected override WebRequest RequestForDownload(WebRequest req)
        {
            FtpWebRequest reqFtp = req as FtpWebRequest;
            reqFtp.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFtp.UseBinary = true;

            return reqFtp;
        }
    }
}
