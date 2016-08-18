using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.SqlClient
{
    class CommandArgs : Args
    {
        private const string ARG_SERVER = "-s";
        private const string ARG_USER = "-u";
        private const string ARG_PWD = "-p";
        private const string ARG_DATABASE = "-t";

        public const string DATABASE_ORACLE = "oracle";
        public const string DATABASE_SQLSERVER = "sqlserver";
        public const string DATABASE_MYSQL = "mysql";

        public string Server
        {
            get
            {
                return GetArg(ARG_SERVER);
            }
        }
        public string User
        {
            get
            {
                return GetArg(ARG_USER);
            }
        }
        public string Password
        {
            get
            {
                return GetArg(ARG_PWD);
            }
        }
        public string DataBase
        {
            get
            {
                var value = GetArg(ARG_DATABASE);

                if (value == null)
                    SetArg(ARG_DATABASE, value = DATABASE_ORACLE);

                return value;
            }
        }
        
        public CommandArgs(params string[] args) 
            : base(args) { }

        protected override string GetArgsHelpInfoInternal()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(ArgUsage(ARG_SERVER, "the database server addr"));
            sb.AppendLine(ArgUsage(ARG_USER, "the login username"));
            sb.AppendLine(ArgUsage(ARG_PWD, "the login password"));
            sb.AppendLine(ArgUsageWithDefault(ARG_DATABASE, "type of database", DATABASE_ORACLE));

            return sb.ToString();
        }

        public override string Validate()
        {
            if (Server == null)
                return ArgRequired(ARG_SERVER);
            if (User == null)
                return ArgRequired(ARG_USER);
            if (Password == null)
                return ArgRequired(ARG_PWD);
            
            return base.Validate();
        }
    }
}
