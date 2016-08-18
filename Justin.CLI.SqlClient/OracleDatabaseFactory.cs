using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justin.Database;

namespace Justin.CLI.SqlClient
{
    class OracleDatabaseFactory : DatabaseFactory
    {
        public OracleDatabaseFactory(string server, string user, string password) 
            : base(server, user, password)
        {
        }

        public override string ConnectionString
        {
            get
            {
                return $"Data Source=//{m_server}; User ID={m_user}; Password={m_password}";
            }
        }

        public override Database.Database CreateDatabase()
        {
            return new OracleDatabase(ConnectionString);
        }
    }
}
