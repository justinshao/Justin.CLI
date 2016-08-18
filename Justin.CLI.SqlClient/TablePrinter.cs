using Justin.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.SqlClient
{
    class TablePrinter<T> : IDisposable
    {
        private IEnumerable<T> m_table;
        protected TextWriter m_writer;

        public TablePrinter(IEnumerable<T> table, TextWriter target)
        {
            m_table = table;
            m_writer = target;
        }

        public void Print()
        {
            PrintHeader(m_writer);
            
            int rows = 0;
            foreach (var line in m_table)
            {
                PrintLine(line, m_writer);
                rows++;
            }

            PrintFooter(m_writer, rows);
        }

        protected virtual void PrintHeader(TextWriter writer)
        {

        }
        protected virtual void PrintLine(T line, TextWriter writer)
        {

        }
        protected virtual void PrintFooter(TextWriter writer, int rows)
        {

        }

        public void Dispose()
        {
            m_writer.Flush();
        }
    }
}
