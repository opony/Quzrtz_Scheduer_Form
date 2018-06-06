using DatabaseLib.Interfaces;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class TblWipTemp_CompomentReSortDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public TblWipTemp_CompomentReSortDao(IDatabase database) : base(database)
        {
        }

        public void UpdateResultStatus(int status, string result, DateTime endTime, string lotNo, string compoNo, string lotSerial)
        {
            try
            {
                logger.Debug("Status : {0}", status);
                logger.Debug("result : {0}", result);
                logger.Debug("endTime : {0}", endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                logger.Debug("lotNo : {0}", lotNo);
                logger.Debug("ComponentNo : {0}", compoNo);
                logger.Debug("LotSerial : {0}", lotSerial);

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UPdate tblWIPTemp_ComponentReSort Set Status = :Status, Result = :Result, EndTime = :EndTime ")
                    .AppendLine("WHERE LotNo = :LotNo")
                    .AppendLine("AND ComponentNo = :ComponentNo")
                    .AppendLine("And LotSerial = :LotSerial");

                logger.Debug("SQL : {0}", sql);

                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                cmd.Parameters.Add("Status", status);
                cmd.Parameters.Add("Result", result);
                cmd.Parameters.Add("EndTime", endTime);
                cmd.Parameters.Add("LotNo", lotNo);
                cmd.Parameters.Add("ComponentNo", compoNo);
                cmd.Parameters.Add("LotSerial", lotSerial);

                this.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "UpdateResultStatus error.");
                throw ex;
            }
        }

        public void UpdateStartTime(int status, string fileName, DateTime stTime, string lotNo, string compoNo, string lotSerial)
        {
            try
            {
                logger.Debug("Status : {0}, FileName : {1}, LotNo : {2}, Start Time : {3}, Lot No : {4}, compoment no : {5}, Lot Serial : {6}", status, fileName, stTime.ToString("yyyy-MM-dd HH:mm:ss"), lotNo, compoNo, lotSerial);
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UPdate tblWIPTemp_ComponentReSort Set Status = :Status, FileName = :FileName, StartTime = :StartTime ")
                    .AppendLine("WHERE LotNo = :LotNo")
                    .AppendLine("AND ComponentNo = :CompomentNo")
                    .AppendLine("AND LotSerial = :LotSerial");

                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                cmd.Parameters.Add("Status", status);
                cmd.Parameters.Add("FileName", fileName.ToUpper());
                cmd.Parameters.Add("StartTime", stTime);
                cmd.Parameters.Add("LotNo", lotNo);
                cmd.Parameters.Add("CompomentNo", compoNo);
                cmd.Parameters.Add("LotSerial", lotSerial);

                logger.Debug("SQL : {0}");
                this.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "UpdateFileNameStartTime error.");
                throw ex;
            }
        }
    }
}
