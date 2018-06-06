using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseLib.Interfaces;
using NLog;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MesAutoLoaderLib.Dao.Mes.Hist
{
    class MesRpt_Sorter_RawHeaderDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MesRpt_Sorter_RawHeaderDao(IDatabase database) : base(database)
        {
        }

        public void DeleteByFileName(string fileName)
        {
            try
            {
                string sql = "Delete From pkgrpt.MESRPT_SORTER_RAWHEADER where FILENAME = '" + fileName + "'";
                logger.Debug("SQL : {0}", sql);

                this.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "DeleteByFileName error.");
                throw ex;
            }
        }

        public void InsertByDataMap(Dictionary<string, object> headerMap)
        {
            try
            {
                List<string> keyList = new List<string>(headerMap.Keys);

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO pkgrpt.MESRPT_SORTER_RAWHEADER(" + string.Join(", ", keyList) + ")");
                sql.AppendLine("VALUES(:" + string.Join(", :", keyList) + ")");

                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                foreach (var keyValue in headerMap)
                {
                    cmd.Parameters.Add(keyValue.Key, keyValue.Value);
                }

                this.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "InsertByDataMap error.");
                throw ex;
            }
        }

        public DataTable QueryEmptyTable()
        {
            try
            {
                string sql = "Select * From pkgrpt.MESRPT_SORTER_RAWHEADER Where ROWNUM <= 1";
                logger.Debug("SQL : {0}", sql);

                return this.ExecuteQuery(sql);

            }
            catch (Exception ex)
            {
                logger.Error(ex,"QueryEmptyTable");
                throw ex;
            }
        }
    }
}
