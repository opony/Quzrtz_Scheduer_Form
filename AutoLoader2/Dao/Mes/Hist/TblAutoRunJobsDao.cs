using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NLog;
using Oracle.ManagedDataAccess.Client;
using static AutoLoader2.Enums.SchedulerEnum;
using DatabaseLib.Interfaces;
using AutoLoader2.Model;

namespace AutoLoader2.Dao.Mes.Hist
{
    class TblAutoRunJobsDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TblAutoRunJobsDao(IDatabase database) : base(database)
        {
        }

        public DataTable QueryAllJob(string hostIp, string autoRunID)
        {
            string sql = "SELECT * FROM PKGRPT.TBL_AUTORUN_JOB where JOB_HOST ='" + hostIp + "' AND JOB_AUTO_RUN_ID = '" + autoRunID + "'";
            logger.Debug("SQL : {0}", sql);
            
            return this.ExecuteQuery(sql);
        }

        //public List<AutoRunJob> QueryAllJob(string hostIp, string autoRunID)
        //{
        //    string sql = "SELECT * FROM PKGRPT.TBL_AUTORUN_JOB where JOB_HOST ='" + hostIp + "' AND JOB_AUTO_RUN_ID = '" + autoRunID + "'";
        //    logger.Debug("SQL : {0}", sql);
        //    DataTable tb = this.ExecuteQuery(sql);
        //    List<AutoRunJob> jobList = new List<AutoRunJob>();
        //    AutoRunJob runJob = null;
        //    foreach (DataRow row in tb.Rows)
        //    {
        //        runJob = new AutoRunJob();
        //        runJob.JobID = row.Field<string>("JOB_ID");
        //        runJob.JobFunc = row.Field<string>("JOB_FUNC");
        //        runJob.JobHost = row.Field<string>("JOB_HOST");
        //        runJob.JobAutoRunID = row.Field<string>("JOB_AUTO_RUN_ID");
        //        runJob.Enabled = row.Field<decimal>("ENABLED") == 1;
        //        runJob.Status = row.Field<string>("STATUS");
        //        runJob.FreqType = (FreqType)Enum.Parse(typeof(FreqType),row.Field<string>("FREQ_TYPE"));
        //        runJob.Freq = row.Field<string>("FREQ");
        //        runJob.StartTime = row.Field<DateTime>("START_TIME");
        //        if(row["LAST_RUN_TIME"] != DBNull.Value)
        //            runJob.LastRunTime = row.Field<DateTime>("LAST_RUN_TIME");
        //        runJob.ExecTime = row.Field<string>("EXEC_TIME");
        //        runJob.Msg = row.Field<string>("MSG");
        //        runJob.Param = row.Field<string>("PARAM");
        //        runJob.Description = row.Field<string>("DESCRIPTION");
        //        runJob.CreatedTime = row.Field<DateTime>("CREATED_TIME");
        //        runJob.CreatedUser = row.Field<string>("CREATED_USER");
        //        runJob.ModifiedTime = row.Field<DateTime>("MODIFIED_TIME");
        //        runJob.ModifiedUser = row.Field<string>("MODIFIED_USER");
        //        runJob.NotifyMail = row.Field<string>("NOTIFY_MAIL");

        //        jobList.Add(runJob);

        //    }

            
        //    return jobList;
        //}

        public void Insert(AutoRunJob runJob)
        {
            string sql = "INSERT INTO PKGRPT.TBL_AUTORUN_JOB (JOB_ID, JOB_FUNC, JOB_HOST, JOB_AUTO_RUN_ID,  ENABLED, STATUS, FREQ_TYPE, FREQ, START_TIME, PARAM, DESCRIPTION, CREATED_TIME, CREATED_USER, MODIFIED_TIME, MODIFIED_USER, NOTIFY_MAIL, UPDATE_TIME, NEXT_RUN_TIME) " +
                "VALUES (:JobID, :JobFunc, :JobHost, :JobAutoRunID, :Enabled, 'Initial', :FreqType, :Freq, :StartTime, :Param, :Des, sysdate, :CreatedUser, sysdate, :ModifiedUser, :NotifyMail, sysdate, :NextRunTime )";

            logger.Debug("Insert JobID : {0}, JobFunc : {1}, JobHost: {2}, JobAutoRunID : {3}", runJob.JobID, runJob.JobFunc, runJob.JobHost, runJob.JobAutoRunID);
            OracleCommand cmd = new OracleCommand(sql);
            cmd.BindByName = true;
            cmd.Parameters.Add("JobID", runJob.JobID);
            cmd.Parameters.Add("JobFunc", runJob.JobFunc);
            cmd.Parameters.Add("JobHost", runJob.JobHost);
            cmd.Parameters.Add("JobAutoRunID", runJob.JobAutoRunID);
            cmd.Parameters.Add("Enabled", runJob.Enabled ? 1:0);
            cmd.Parameters.Add("FreqType", runJob.FreqType.ToString());
            cmd.Parameters.Add("Freq", runJob.Freq);
            cmd.Parameters.Add("StartTime", runJob.StartTime);
            cmd.Parameters.Add("Param", runJob.Param);
            cmd.Parameters.Add("Des", runJob.Description);
            cmd.Parameters.Add("CreatedUser", runJob.CreatedUser);
            cmd.Parameters.Add("ModifiedUser", runJob.ModifiedUser);
            cmd.Parameters.Add("NotifyMail", runJob.NotifyMail);
            cmd.Parameters.Add("NextRunTime", runJob.NextRunTime);


            this.ExecuteNonQuery(cmd);

        }

        public void UpdateJob(AutoRunJob editJob)
        {
            logger.Debug("UpdateJob , JobID : {0}, JobHost: {1}, JobAutoRunID : {2}", editJob.JobID, editJob.JobHost, editJob.JobAutoRunID);
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE PKGRPT.TBL_AUTORUN_JOB SET JOB_FUNC = :JobFunc, FREQ_TYPE = :FreqType, FREQ = :Freq, START_TIME = :StartTime, PARAM = :Param, DESCRIPTION = :Descrption, MODIFIED_TIME = sysdate, MODIFIED_USER = :UserID, NOTIFY_MAIL = :NotifyMail, NEXT_RUN_TIME = :NextRunTime, UPDATE_TIME = sysdate")
                .AppendLine("WHERE JOB_ID = :JobID")
                .AppendLine("AND JOB_HOST = :JobHost")
                .AppendLine("AND JOB_AUTO_RUN_ID = :JobAutoRunID");

            OracleCommand cmd = new OracleCommand(sql.ToString());
            cmd.BindByName = true;
            cmd.Parameters.Add("JobFunc", editJob.JobFunc);
            cmd.Parameters.Add("FreqType", editJob.FreqType.ToString());
            cmd.Parameters.Add("Freq", editJob.Freq);
            cmd.Parameters.Add("StartTime", editJob.StartTime);
            cmd.Parameters.Add("Param", editJob.Param);
            cmd.Parameters.Add("Descrption", editJob.Description);
            cmd.Parameters.Add("UserID", editJob.ModifiedUser);
            cmd.Parameters.Add("NotifyMail", editJob.NotifyMail);
            cmd.Parameters.Add("JobID", editJob.JobID);
            cmd.Parameters.Add("JobHost", editJob.JobHost);
            cmd.Parameters.Add("JobAutoRunID", editJob.JobAutoRunID);
            cmd.Parameters.Add("NextRunTime", editJob.NextRunTime);

            this.ExecuteNonQuery(cmd);
        }

        //public void UpdateLastRunStatus(string jobID, string hostIp, string autoRunID, string status, DateTime lastRunTime, DateTime nextRunTime)
        //{
        //    logger.Debug("Update Last Run status , JobID: {0}, HostIP: {1}, AutoRunID : {2}, Status: {3}, LastRunTime: {4}, NextRunTime: {5}", jobID, hostIp, autoRunID, status, lastRunTime.ToString("yyyy-MM-dd HH:mm:ss"), nextRunTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    StringBuilder sql = new StringBuilder();
        //    sql.AppendLine("UPDATE PKGRPT.TBL_AUTORUN_JOB SET STATUS = :Status,LAST_RUN_TIME = :LastRunTime, NEXT_RUN_TIME =:NextRunTime, UPDATE_TIME = sysdate")
        //        .AppendLine("WHERE JOB_ID = :JobID")
        //        .AppendLine("AND JOB_HOST = :JobHost")
        //        .AppendLine("AND JOB_AUTO_RUN_ID = :JobAutoRunID");

        //    OracleCommand cmd = new OracleCommand(sql.ToString());
        //    cmd.BindByName = true;
        //    cmd.Parameters.Add("Status", status);
        //    cmd.Parameters.Add("LastRunTime", lastRunTime);
        //    cmd.Parameters.Add("JobID", jobID);
        //    cmd.Parameters.Add("JobHost", hostIp);
        //    cmd.Parameters.Add("JobAutoRunID", autoRunID);
        //    cmd.Parameters.Add("NextRunTime", nextRunTime);

        //    this.ExecuteNonQuery(cmd);
        //}

        public void UpdateRunningStatus(string jobID, string hostIp, string autoRunID, string status, DateTime lastRunTime)
        {
            logger.Debug("UpdateRunningStatus, JobID: {0}, HostIP: {1}, AutoRunID : {2}, Status: {3}, LastRunTime : {4}", jobID, hostIp, autoRunID, status, lastRunTime.ToString("yyyy-MM-dd HH:mm:ss"));
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE PKGRPT.TBL_AUTORUN_JOB SET STATUS = :Status, UPDATE_TIME = sysdate, LAST_RUN_TIME = :LastRunTime, MSG = ''")
                .AppendLine("WHERE JOB_ID = :JobID")
                .AppendLine("AND JOB_HOST = :JobHost")
                .AppendLine("AND JOB_AUTO_RUN_ID = :JobAutoRunID");

            OracleCommand cmd = new OracleCommand(sql.ToString());
            cmd.BindByName = true;
            cmd.Parameters.Add("Status", status);
            cmd.Parameters.Add("LastRunTime", lastRunTime);
            cmd.Parameters.Add("JobID", jobID);
            cmd.Parameters.Add("JobHost", hostIp);
            cmd.Parameters.Add("JobAutoRunID", autoRunID);

            this.ExecuteNonQuery(cmd);
        }

        public void UpdateExecutedStatus(string jobID, string hostIp, string autoRunID, string status, string msg, string executeTime, DateTime lastRunTime, DateTime? nextRunTime)
        {
            
            
            StringBuilder sql = new StringBuilder();
            if (nextRunTime.HasValue)
            {
                logger.Debug("UpdateExecutedStatus, JobID: {0}, HostIP: {1}, AutoRunID : {2}, Status: {3}, Msg: {4}, ExecuteTime: {5}, LastRunTime: {6}, NextRunTime : {7}", jobID, hostIp, autoRunID, status, msg, executeTime, lastRunTime.ToString("yyyy-MM-dd HH:mm:ss"), nextRunTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                
                sql.AppendLine("UPDATE PKGRPT.TBL_AUTORUN_JOB SET STATUS = :Status, MSG = :Msg, EXEC_TIME = :ExecTime, LAST_RUN_TIME = :LastRunTime, NEXT_RUN_TIME = :NextRunTime, UPDATE_TIME = sysdate");
            }
            else
            {
                logger.Debug("UpdateExecutedStatus, JobID: {0}, HostIP: {1}, AutoRunID : {2}, Status: {3}, Msg: {4}, ExecuteTime: {5}, LastRunTime: {6}", jobID, hostIp, autoRunID, status, msg, executeTime, lastRunTime.ToString("yyyy-MM-dd HH:mm:ss"));
                sql.AppendLine("UPDATE PKGRPT.TBL_AUTORUN_JOB SET STATUS = :Status, MSG = :Msg, EXEC_TIME = :ExecTime, LAST_RUN_TIME = :LastRunTime, UPDATE_TIME = sysdate");
            }
            
            sql.AppendLine("WHERE JOB_ID = :JobID")
                .AppendLine("AND JOB_HOST = :JobHost")
                .AppendLine("AND JOB_AUTO_RUN_ID = :JobAutoRunID");

            OracleCommand cmd = new OracleCommand(sql.ToString());
            cmd.BindByName = true;
            cmd.Parameters.Add("Status", status);
            cmd.Parameters.Add("Msg", msg);
            cmd.Parameters.Add("ExecTime", executeTime);
            cmd.Parameters.Add("JobID", jobID);
            cmd.Parameters.Add("JobHost", hostIp);
            cmd.Parameters.Add("JobAutoRunID", autoRunID);
            cmd.Parameters.Add("LastRunTime", lastRunTime);
            if (nextRunTime.HasValue)
                cmd.Parameters.Add("NextRunTime", nextRunTime.Value);

            this.ExecuteNonQuery(cmd);
        }



        public void UpdateEnabledStatus(string jobID, string hostIp, string autoRunID, int enabled, string status, DateTime? nextRunTime)
        {
            logger.Debug("Update Enabled Status, JobID : {0}, HostIP: {1}, AutoRunID : {2}, Enabled: {3}, Status: {4} , NextRunTime: {5}", jobID, hostIp, autoRunID, enabled, status, nextRunTime.HasValue ? nextRunTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "null");

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE PKGRPT.TBL_AUTORUN_JOB SET ENABLED = :Enabled, STATUS = :Status, UPDATE_TIME = sysdate, NEXT_RUN_TIME = :NextRunTime")
                .AppendLine("WHERE JOB_ID = :JobID")
                .AppendLine("AND JOB_HOST = :JobHost")
                .AppendLine("AND JOB_AUTO_RUN_ID = :JobAutoRunID");

            OracleCommand cmd = new OracleCommand(sql.ToString());
            cmd.BindByName = true;
            cmd.Parameters.Add("Enabled", enabled);
            cmd.Parameters.Add("JobID", jobID);
            cmd.Parameters.Add("JobHost", hostIp);
            cmd.Parameters.Add("JobAutoRunID", autoRunID);
            cmd.Parameters.Add("Status", status);
            cmd.Parameters.Add("NextRunTime", nextRunTime);

            this.ExecuteNonQuery(cmd);

        }

        public void DeleteJob(string jobID, string hostIp, string autoRunID)
        {
            logger.Debug("DeleteJob, JobID : {0}, HostIP: {1}, AutoRunID : {2}", jobID, hostIp, autoRunID);
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("DELETE FROM PKGRPT.TBL_AUTORUN_JOB")
                .AppendLine("WHERE JOB_ID = :JobID")
                .AppendLine("AND JOB_HOST = :JobHost")
                .AppendLine("AND JOB_AUTO_RUN_ID = :JobAutoRunID");


            OracleCommand cmd = new OracleCommand(sql.ToString());
            cmd.BindByName = true;
            cmd.Parameters.Add("JobID", jobID);
            cmd.Parameters.Add("JobHost", hostIp);
            cmd.Parameters.Add("JobAutoRunID", autoRunID);

            this.ExecuteNonQuery(cmd);
        }
    }
}
