using Justin.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.SqlClient
{
    class SqlResultOut : IDisposable
    {
        private TextWriter m_writer;

        public SqlResultOut(TextWriter writer)
        {
            m_writer = writer;
        }

        public void OutputResultSet(DataTable table)
        {
            using (var printer = new ResultSetPrinter(table, m_writer))
            {
                printer.Print();
            }
        }
        public void OutputAffected(int num)
        {
            m_writer.WriteLine($"{num} rows affected");
        }
        public void OutputOk()
        {
            m_writer.WriteLine("query ok");
        }
        public void OutputError(Exception ex)
        {
            m_writer.WriteLine(ex.Message);
        }

        public void Dispose()
        {
            try
            {
                m_writer.Dispose();
            }
            catch
            {
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}
