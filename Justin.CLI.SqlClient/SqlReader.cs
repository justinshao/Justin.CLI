using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.SqlClient
{
    class SqlReader
    {
        private const string PROMOT = "> ";
        private const string END = ";";
        private const string QUIT = "q";

        public const string EXIT = "exit";

        private static char[] EmptyChars = new char[] { ' ', '\t', };

        private readonly string Database;
        private StringBuilder m_sqlBuilder = new StringBuilder();
        private TextReader m_reader;
        private TextWriter m_writer;
        
        public SqlReader(string database, TextReader reader, TextWriter writer)
        {
            Database = database;
            m_reader = reader;
            m_writer = writer;
        }

        public string Read()
        {
            m_sqlBuilder.Clear();

            var startPromot = Database + PROMOT;
            m_writer.Write(startPromot);
            
            string line = null;

            while ((line = m_reader.ReadLine()) != null)
            {
                line = line.Trim(EmptyChars);

                if(!line.EndsWith(END) && !string.IsNullOrEmpty(line))
                {
                    m_sqlBuilder.Append(line).AppendLine();
                }
                else
                {
                    var input = line.TrimEnd(END.ToArray()).Trim(EmptyChars);
                    if (QUIT.Equals(input, StringComparison.OrdinalIgnoreCase))
                    {
                        return string.Empty;
                    }
                    else if (EXIT.Equals(input, StringComparison.OrdinalIgnoreCase))
                    {
                        return EXIT;
                    }
                    else
                    {
                        return m_sqlBuilder.Append(input).ToString();
                    }
                }

                m_writer.Write(("-" + PROMOT).PadLeft(startPromot.Length));
            }

            return EXIT; // end of file.            
        }
    }
}
