using DatabaseLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Dao
{
    abstract class BaseDao
    {
        IDatabase database;
        public BaseDao(IDatabase database)
        {
            this.database = database;
        }

        protected DataTable ExecuteQuery(string sql)
        {
            try
            {
                this.database.GetConnection();
                return this.database.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.database.Close();
            }
        }

        protected DataTable ExecuteQuery(IDbCommand cmd)
        {
            try
            {
                this.database.GetConnection();
                return this.database.ExecuteQuery(cmd);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.database.Close();
            }
        }


        protected void ExecuteNonQuery(string sql)
        {
            try
            {
                this.database.GetConnection();
                this.database.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.database.Close();
            }
        }


        protected void ExecuteNonQuery(IDbCommand cmd)
        {
            try
            {
                this.database.GetConnection();
                this.database.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.database.Close();
            }
        }
    }
}
