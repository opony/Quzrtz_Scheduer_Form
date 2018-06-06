using DatabaseLib.Interfaces;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class TblWipTemp_ReSortDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public TblWipTemp_ReSortDao(IDatabase database) : base(database)
        {
        }

        public DataTable QueryByStatus(int status, string hostName)
        {
            DataTable tb = null;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Select LotNo,LotSerial,Count From tblWIPTemp_ReSort")
                    .AppendLine("Where Status = " + status)
                    .AppendLine("And (HostName = '*' Or HostName = '" + hostName + "')");
                logger.Debug("SQL : {0}", sql);

                tb = this.ExecuteQuery(sql.ToString());

            }
            catch (Exception ex)
            {
                logger.Error(ex, "QueryByHostName error.");
                throw new Exception("QueryByHostName error.", ex);
            }

            return tb;
        }

        public void UpdateStartTimeStatus(int status, string hostName, DateTime stTime, string lotSerial)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Update tblWIPTemp_ReSort Set Status = :Status, HostName = :HostName, StartTime = :StartTime")
                    .AppendLine("Where LotSerial = :LotSerial");

                logger.Debug("SQL : {0}", sql);

                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                cmd.Parameters.Add("Status", status);
                cmd.Parameters.Add("HostName", hostName);
                cmd.Parameters.Add("StartTime", stTime);
                cmd.Parameters.Add("LotSerial", lotSerial);

                this.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "UpdateStatus start time status error.");
                throw ex;
            }
        }

        public void UpdateEndTimeStatus(int status, DateTime endTime, string lotSerial)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Update tblWIPTemp_ReSort Set Status = :Status, EndTime = :EndTime")
                    .AppendLine("Where LotSerial = :LotSerial");

                logger.Debug("SQL : {0}", sql);
                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                cmd.Parameters.Add("Status", status);
                cmd.Parameters.Add("EndTime", endTime);
                cmd.Parameters.Add("LotSerial", lotSerial);

                this.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "UpdateStatus End Time status error.");
                throw ex;
            }
        }
    }
}
