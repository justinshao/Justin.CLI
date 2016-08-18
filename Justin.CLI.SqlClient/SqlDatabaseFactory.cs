using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Justin.Database;

namespace Justin.CLI.SqlClient
{
    class SqlDatabaseFactory : DatabaseFactory
    {
        public SqlDatabaseFactory(string server, string user, string password) 
            : base(server, user, password)
        {
        }

        public override string ConnectionString
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Database.Database CreateDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
