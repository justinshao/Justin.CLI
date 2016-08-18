﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.FileSync
{
    class HttpClient : WebClient
    {
        public HttpClient(string userId, string password) 
            : base(userId, password)
        { }
    }
}
