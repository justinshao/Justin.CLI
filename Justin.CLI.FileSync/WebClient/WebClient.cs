using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.FileSync
{
    abstract class WebClient
    {
        private string m_userId;
        private string m_password;

        public WebClient(string userId, string password)
        {
            m_userId = userId;
            m_password = password;
        }

        public void Download(string srcFileName, string dest, Action<long, long> onProgress)
        {
            using (Stream fs = File.OpenWrite(dest))
            {
                Download(srcFileName, fs, onProgress);
            }
        }

        public void Download(string srcFileName, string dest)
        {
            using (Stream fs = File.OpenWrite(dest))
            {
                Download(srcFileName, fs);
            }
        }

        public void Download(string srcFileName, Stream dest)
        {
            Download(new Uri(srcFileName), dest, null);
        }

        public void Download(string srcFileName, Stream dest, Action<long, long> onProgress)
        {
            Download(new Uri(srcFileName), dest, onProgress);
        }

        public void Download(Uri uri, Stream dest, Action<long, long> onProgress)
        {
            WebRequest req = WebRequest.Create(uri);
            req = RequestForDownload(req);
            req.Credentials = new NetworkCredential(m_userId, m_password);

            using (WebResponse response = req.GetResponse())
            {
                using (Stream ftpStream = response.GetResponseStream())
                {
                    byte[] buf = new byte[0x1000];
                    int l = 0;
                    long total = response.ContentLength;
                    long read = 0;

                    while ((l = ftpStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        dest.Write(buf, 0, l);

                        if (onProgress != null)
                            onProgress(read += l, total);
                    }
                }
            }
        }

        protected virtual WebRequest RequestForDownload(WebRequest req) { return req; }
    }
}
