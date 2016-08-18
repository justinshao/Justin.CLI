using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.SqlClient
{
    abstract class DatabaseFactory
    {
        protected string m_server;
        protected string m_user;
        protected string m_password;

        public DatabaseFactory(string server, string user, string password)
        {
            m_server = server;
            m_user = user;
            m_password = password;
        }

        public abstract Justin.Database.Database CreateDatabase();
        public abstract string ConnectionString { get; }
    }
}
