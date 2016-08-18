using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.FileSync
{
    class CommandArgs : Args
    {
        private const string ARG_LOCAL = "-l";
        private const string ARG_SERVER = "-s";
        private const string ARG_USER = "-u";
        private const string ARG_PWD = "-p";
        private const string ARG_DIR = "-d";
        private const string ARG_MAP_FILE = "-m";
        private const string ARG_PROTOCOL = "-t";

        public const string PROTOCOL_FILE = "file";
        public const string PROTOCOL_HTTP = "http";
        public const string PROTOCOL_FTP = "ftp";

        public CommandArgs(params string[] args)
            : base(args)
        { }

        public string Local
        {
            get
            {
                var value = GetArg(ARG_LOCAL);

                if (value == null)
                    SetArg(ARG_LOCAL, value = Environment.CurrentDirectory);

                return value;
            }
        }
        public string Server
        {
            get { return GetArg(ARG_SERVER); }
        }
        public string User
        {
            get { return GetArg(ARG_USER); }
        }
        public string Pwd
        {
            get { return GetArg(ARG_PWD); }
        }
        public string Dir
        {
            get
            {
                var value = GetArg(ARG_DIR);

                if (value == null)
                    SetArg(ARG_DIR, string.Empty);

                return value;
            }
        }
        public string MapFile
        {
            get
            {
                var value = GetArg(ARG_MAP_FILE);

                if (value == null)
                    SetArg(ARG_MAP_FILE, value = "map.txt");

                return value;
            }
        }
        public string Protocol
        {
            get
            {
                var value = GetArg(ARG_PROTOCOL);

                if (value == null)
                    SetArg(ARG_PROTOCOL, value = PROTOCOL_FTP);

                return value;
            }
        }

        public override string Validate()
        {
            if (Protocol != PROTOCOL_FTP && Protocol != PROTOCOL_HTTP && Protocol != PROTOCOL_FILE)
                return ArgOptions(ARG_PROTOCOL, PROTOCOL_FTP, PROTOCOL_HTTP, PROTOCOL_FILE);

            if (Protocol == PROTOCOL_FTP || Protocol == PROTOCOL_FILE)
            {
                if (Server == null)
                    return ArgRequired(ARG_SERVER);
                if (User == null)
                    return ArgRequired(ARG_USER);
                if (Pwd == null)
                    return ArgRequired(ARG_PWD);
            }
            else // http
            {
                if (Server == null)
                    return ArgRequired(ARG_SERVER);
            }

            return base.Validate();
        }

        protected override string GetArgsHelpInfoInternal()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(ArgUsageWithDefault(ARG_LOCAL, "the local directory", "current dir"));
            sb.AppendLine(ArgUsage(ARG_SERVER, "the ftp server address", required: true));
            sb.AppendLine(ArgUsage(ARG_USER, "the login name. only used in ftp and file", required: false));
            sb.AppendLine(ArgUsage(ARG_PWD, "the login password. used only in ftp and file", required: false));
            sb.AppendLine(ArgUsageWithDefault(ARG_DIR, "the default directory for login", "root"));
            sb.AppendLine(ArgUsageWithDefault(ARG_MAP_FILE, "the file mapping the local directories from server", "map.txt"));
            sb.AppendLine(ArgUsageWithDefault(ARG_MAP_FILE, "type of protocol. ftp, http or file", PROTOCOL_FTP));

            return sb.ToString();
        }
    }
}
