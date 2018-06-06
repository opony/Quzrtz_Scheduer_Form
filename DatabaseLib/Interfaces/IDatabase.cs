using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DatabaseLib.Interfaces
{
    public interface IDatabase
    {
        void GetConnection();

        void Close();

        void BeginTrans();

        void Rollback();

        void Commit();

        DataTable ExecuteQuery(IDbCommand cmd);

        DataTable ExecuteQuery(string sql);

        void ExecuteNonQuery(IDbCommand cmd);

        void ExecuteNonQuery(string sql);
    }
}
