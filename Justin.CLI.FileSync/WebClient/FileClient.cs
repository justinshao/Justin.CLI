using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.FileSync
{
    class FileClient : WebClient
    {
        public FileClient(string userId, string password) 
            : base (userId, password) { }

        protected override WebRequest RequestForDownload(WebRequest req)
        {
            req.Method = WebRequestMethods.File.DownloadFile;

            return req;
        }
    }
}
