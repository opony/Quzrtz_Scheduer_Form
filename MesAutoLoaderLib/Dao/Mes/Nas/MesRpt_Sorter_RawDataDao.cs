using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseLib.Interfaces;
using NLog;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MesAutoLoaderLib.Dao.Mes.Nas
{
    class MesRpt_Sorter_RawDataDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public MesRpt_Sorter_RawDataDao(IDatabase database) : base(database)
        {
        }

        public void Insert(List<Dictionary<string, object>> rawDataList)
        {
            if (rawDataList.Count == 0)
                return;

            try
            {

                var colNames = rawDataList[0].Keys.ToArray();

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO pkgrpt.MESRPT_SORTER_RAWDATA (" + string.Join(", ", colNames) + ")");
                sql.AppendLine("VALUES (:" + string.Join(", :", colNames) + ")");
                logger.Debug("SQL : {0}", sql);
                using (OracleCommand cmd = new OracleCommand(sql.ToString()))
                {
                    cmd.BindByName = true;
                    foreach (Dictionary<string, object> rowDataMap in rawDataList)
                    {
                        foreach (var keyValue in rowDataMap)
                        {
                            cmd.Parameters.Add(keyValue.Key, keyValue.Value);
                        }
                        this.ExecuteNonQuery(cmd);
                        cmd.Parameters.Clear();
                    }
                }
                
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Insert fail");
                throw ex;
            }
        }

        public void DeleteByFileName(string fileName)
        {
            try
            {
                string sql = "DELETE pkgrpt.MESRPT_SORTER_RAWDATA Where FILENAME = '" + fileName + "'";
                logger.Debug("SQL : {0}", sql);
                this.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "DeleteByFile error.");
                throw ex;
            }
        }

        public DataTable QueryEmptyTb()
        {
            try
            {
                string sql = "Select * From pkgrpt.MESRPT_SORTER_RAWDATA Where ROWNUM <= 1";
                logger.Debug("SQL : {0}", sql);

                return this.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "QueryEmptyTb error.");
                throw ex;
            }
        }
    }
}
