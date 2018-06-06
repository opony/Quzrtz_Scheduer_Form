using DatabaseLib.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DatabaseLib.Database
{
    public class OraDatabase : IDatabase
    {
        private OracleConnection conn;
        private OracleTransaction trans;
        string connStr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStr">Oracle connect string</param>
        public OraDatabase(string connStr)
        {
            this.connStr = connStr;
        }

        /// <summary>
        /// Begin Transaction
        /// </summary>
        public void BeginTrans()
        {
            if (conn == null)
                throw new Exception("OraDatabase must get connection beofre invoke BeginTrans() method.");
            if (trans == null)
                trans = conn.BeginTransaction();
        }

        /// <summary>
        /// Close connection
        /// </summary>
        public void Close()
        {
            if (trans != null)
                return;

            if (conn != null && conn.State != ConnectionState.Closed)
                conn.Close();

            conn = null;
        }

        /// <summary>
        /// Transaction commit
        /// </summary>
        public void Commit()
        {
            if (trans == null)
                throw new Exception("OraDatabase don't BeginTrans beofre invoke Commit() method.");

            trans.Commit();
            trans = null;
        }






        /// <summary>
        /// Get db connection before operate sql command
        /// </summary>
        public void GetConnection()
        {
            if (conn == null)
            {
                conn = new OracleConnection(connStr);
                conn.Open();

            }

        }

        /// <summary>
        /// Transaction Rollback
        /// </summary>
        public void Rollback()
        {
            if (trans == null)
                throw new Exception("OraDatabase don't BeginTrans beofre invoke Rollback() method.");

            trans.Rollback();
            trans = null;
        }

        public DataTable ExecuteQuery(IDbCommand cmd)
        {
            if (conn == null)
                throw new Exception("OraDatabase must get connection beofre invoke ExecuteQuery(cmd) method.");

            cmd.Connection = conn;
            DataTable tb = new DataTable();
            using (OracleDataAdapter adapter = new OracleDataAdapter((OracleCommand)cmd))
            {
                adapter.Fill(tb);
            }

            return tb;
        }

        public DataTable ExecuteQuery(string sql)
        {

            return ExecuteQuery(new OracleCommand(sql));
        }

        public void ExecuteNonQuery(string sql)
        {
            ExecuteNonQuery(new OracleCommand(sql));
        }
        public void ExecuteNonQuery(IDbCommand cmd)
        {
            if (conn == null)
                throw new Exception("OraDatabase must get connection beofre invoke ExecuteNonQuery(cmd) method.");

            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }
    }
}
