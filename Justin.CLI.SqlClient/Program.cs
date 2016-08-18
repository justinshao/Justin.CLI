using Justin.CLI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Justin.CLI.SqlClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // load embed assemblies.
            AppDomain.CurrentDomain.AssemblyResolve += 
                new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            LoadDependencyAssemblies();

            try
            {
                Process(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"unexpected error: {ex.Message}");

                // wait for exit.
                Console.ReadLine();
            }
        }

        private static void Process(string[] args)
        {
            new CommandProcessor<CommandArgs>(new CommandArgs(args))
                .Process(cmdArgs =>
                {
                    var sqlReader = new SqlReader(
                        cmdArgs.DataBase,
                        Console.In,
                        Console.Out);
                    var db = GetDatabaseFactory(
                        cmdArgs.DataBase,
                        cmdArgs.Server,
                        cmdArgs.User,
                        cmdArgs.Password).CreateDatabase();
                    var output = new SqlResultOut(Console.Out);

                    using (var sqlExecutor = new SqlExecutor(db, output))
                    {
                        sqlExecutor.Prepare();

                        Console.Title = $"sqlclient -{cmdArgs.DataBase} {cmdArgs.User}@{cmdArgs.Server}";

                        OutputDescription();

                        while (true)
                        {
                            var sql = sqlReader.Read();

                            if (SqlReader.EXIT.Equals(sql))
                            {
                                break;
                            }

                            if (!string.IsNullOrEmpty(sql))
                            {
                                Console.WriteLine();
                                sqlExecutor.Execute(sql);
                                Console.WriteLine();
                            }
                        }
                    }
                });
        }

        private static void OutputDescription()
        {
            Console.WriteLine();
            Console.WriteLine("This tool can be used to interact with databases. It supports Oracle, SqlServer and MySql.");
            Console.WriteLine("Legal sql shall end with ';'. ");
            Console.WriteLine("Type 'Enter' start a new line.");
            Console.WriteLine("Type 'q;' to cancel current input and start a new sql input.");
            Console.WriteLine("Type 'exit;' to close the connection and exit form promotion.");
            Console.WriteLine();
        }

        private static DatabaseFactory GetDatabaseFactory(string database, string server, string user, string password)
        {
            switch (database)
            {
                case CommandArgs.DATABASE_ORACLE:
                    return new OracleDatabaseFactory(server, user, password);
                case CommandArgs.DATABASE_SQLSERVER:
                    return new SqlDatabaseFactory(server, user, password);
                case CommandArgs.DATABASE_MYSQL:
                    return new MySqlDatabaseFactory(server, user, password);

                default:
                    throw new NotSupportedException($"database `{database}` not supported");
            }
        }

        private static IDictionary<string, Assembly> dependencyAssemblies 
            = new Dictionary<string, Assembly>();
        private static void LoadDependencyAssemblies()
        {
            var assemblies = new string[]
            {
                "Justin.CLI.SqlClient.lib.cli.dll",
                "Justin.CLI.SqlClient.lib.Justin.Database.dll",
                "Justin.CLI.SqlClient.lib.Oracle.ManagedDataAccess.dll",
                "Justin.CLI.SqlClient.lib.MySql.Data.dll",
                "Justin.CLI.SqlClient.lib.Google.ProtocolBuffers.dll",
                "Justin.CLI.SqlClient.lib.Google.ProtocolBuffers.Serialization.dll",
            };

            var mainAss = Assembly.GetExecutingAssembly();
            foreach (var item in assemblies)
            {
                using (var stream = mainAss.GetManifestResourceStream(item))
                {
                    var buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);

                    var ass = AppDomain.CurrentDomain.Load(buffer);
                    dependencyAssemblies.Add(ass.FullName, ass);
                }
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (dependencyAssemblies.ContainsKey(args.Name) == true)
            {
                return dependencyAssemblies[args.Name];
            }

            return null;
        }
        
    }
}
