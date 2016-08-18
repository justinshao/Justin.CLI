using Justin.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Justin.CLI.SqlClient
{
    class ResultSetPrinter : TablePrinter<DataRow>
    {
        private const int MAX_COL_WIDTH = 40;
        private IDictionary<string, int> m_colLengths = new Dictionary<string, int>();

        private Tuple<string, Type>[] m_columns;

        public ResultSetPrinter(DataTable table, TextWriter target) 
            : base(table.AsEnumerable(), target)
        {
            m_columns = GetColumnsInfo(table);
            m_colLengths = GetColMaxLengths(table, target.Encoding);
        }

        private static Tuple<string, Type>[] GetColumnsInfo(DataTable table)
        {
            var columns = new Tuple<string, Type>[table.Columns.Count];

            for (int i = 0; i < columns.Length; i++)
            {
                var c = table.Columns[i];
                columns[i] = Tuple.Create(c.ColumnName, c.DataType);
            }

            return columns;
        }
        private static IDictionary<string, int> GetColMaxLengths(DataTable table, Encoding encoding)
        {
            IDictionary<string, int> dic = new Dictionary<string, int>();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                var col = table.Columns[i].Caption;
                var l = table.Rows.Count == 0 ? 0 : 
                    (from r in table.AsEnumerable()
                         select encoding.GetByteCount(r[i].ToString())).Max();

                dic[col] = Math.Min(Math.Max(encoding.GetByteCount(col), l), MAX_COL_WIDTH);
            }

            return dic;
        }

        protected override void PrintHeader(TextWriter writer)
        {
            PrintSplitLine(writer);
            PrintHeaderTitle(writer);
            PrintSplitLine(writer);
        }

        protected override void PrintLine(DataRow line, TextWriter writer)
        {
            writer.Write('|');

            foreach (var col in m_columns)
            {
                writer.Write(' ');
                PrintValue(writer, line[col.Item1], col.Item1, col.Item2);
                writer.Write(' ');
                writer.Write('|');
            }

            writer.WriteLine();
        }

        protected override void PrintFooter(TextWriter writer, int rows)
        {
            PrintSplitLine(writer);
            writer.WriteLine($"{rows} rows in set");
        }

        private void PrintSplitLine(TextWriter writer)
        {
            writer.Write('+');
            foreach (var e in m_colLengths)
            {
                writer.Write("".PadLeft(e.Value + 2, '-'));
                writer.Write('+');
            }
            writer.WriteLine();
        }
        private void PrintHeaderTitle(TextWriter writer)
        {
            var encoding = writer.Encoding;
            writer.Write('|');
            foreach (var e in m_colLengths)
            {
                writer.Write(' ');

                var l = m_colLengths[e.Key];
                var cl = encoding.GetByteCount(e.Key);

                writer.Write(e.Key + "".PadRight(Math.Max(0, l - cl)));
                
                writer.Write(' ');
                writer.Write('|');
            }
            writer.WriteLine();
        }

        private void PrintValue(TextWriter writer, object value, string col, Type type = null)
        {
            var encoding = writer.Encoding;
            string v_str = value.ToString();
            int vl = encoding.GetByteCount(v_str);
            int len = m_colLengths[col];
            type = type ?? typeof(string);

            if (IsNumberic(type))
            {
                writer.Write("".PadRight(Math.Max(0, len - vl)) + v_str.ToString());
            }
            else
            {
                writer.Write(v_str.ToString() + "".PadRight(Math.Max(0, len - vl)));
            }
        }
        private static bool IsNumberic(Type type)
        {
            return type == typeof(byte) ||
                type == typeof(sbyte) ||
                type == typeof(short) ||
                type == typeof(ushort) ||
                type == typeof(int) ||
                type == typeof(uint) ||
                type == typeof(long) ||
                type == typeof(ulong) ||
                type == typeof(decimal) ||
                type == typeof(double) ||
                type == typeof(float);
        }
        
    }
}
