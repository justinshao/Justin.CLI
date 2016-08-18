using Justin.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Justin.CLI.SqlClient
{
    class SqlExecutor : IDisposable
    {
        private const string PREFIX_SELECT = "SELECT";
        private const string PREFIX_INSERT = "INSERT";
        private const string PREFIX_DELETE = "DELETE";
        private const string PREFIX_UPDATE = "UPDATE";
        private const string COMMIT = "COMMIT";
        private const string ROLLBACK = "ROLLBACK";

        private Justin.Database.Database m_db;
        private SqlResultOut m_output;

        public SqlExecutor(Justin.Database.Database db, SqlResultOut output)
        {
            m_db = db;
            m_output = output;
        }

        public void Prepare()
        {
            m_db.Open();
        }

        public void Execute(string sql)
        {
            try
            {   
                if (sql.StartsWith(PREFIX_SELECT, StringComparison.OrdinalIgnoreCase))
                {
                    using (var result = ExecuteSelect(sql))
                    {
                        m_output.OutputResultSet(result);
                    }
                }
                else if (
                    sql.StartsWith(PREFIX_INSERT, StringComparison.OrdinalIgnoreCase) ||
                    sql.StartsWith(PREFIX_DELETE, StringComparison.OrdinalIgnoreCase) ||
                    sql.StartsWith(PREFIX_UPDATE, StringComparison.OrdinalIgnoreCase)
                    )
                {
                    ExecuteBeginTrans();
                    var result = ExcuteNonQuery(sql);

                    m_output.OutputAffected(result);
                }
                else if (sql.Equals(COMMIT, StringComparison.OrdinalIgnoreCase))
                {
                    ExecuteCommit();

                    m_output.OutputOk();
                }
                else if (sql.Equals(ROLLBACK, StringComparison.OrdinalIgnoreCase))
                {
                    ExecuteRollback();

                    m_output.OutputOk();
                }
                else
                {
                    var result = ExcuteNonQuery(sql);

                    m_output.OutputAffected(result);
                }
            }
            catch (Exception ex)
            {
                m_output.OutputError(ex);
            }
        }

        private DataTable ExecuteSelect(string sql)
        {
            return m_db.QueryTable(sql);
        }
        private int ExcuteNonQuery(string sql)
        {
            return m_db.ExcuteNonQuery(sql);
        }
        private void ExecuteBeginTrans()
        {
            if (!m_db.HasTransaction)
            {
                m_db.BeginTransaction();
            }
        }
        private void ExecuteCommit()
        {
            if (m_db.HasTransaction)
            {
                m_db.CommitTransaction();
            }
        }
        private void ExecuteRollback()
        {
            if(m_db.HasTransaction)
            {
                m_db.RollbackTransaction();
            }
        }

        public void Dispose()
        {
            try
            {
                if (m_db != null)
                {
                    m_db.Dispose();
                }
                if (m_output != null)
                {
                    m_output.Dispose();
                }
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
