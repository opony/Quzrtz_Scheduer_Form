using AutoLoader2.Dao.Mes.Hist;
using AutoLoader2.Model;
using AutoLoader2.Util;
using DatabaseLib.Database;
using DatabaseLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AutoLoader2.Proxy
{
    class MesHistDataProxy
    {

        public static DataTable QueryAllAutoRunJobs(string ip, string autoRunID)
        {
            IDatabase db = GetHistDb();
            TblAutoRunJobsDao jobsDao = new TblAutoRunJobsDao(db);
            
            return jobsDao.QueryAllJob(ip, autoRunID);

        }

        private static IDatabase GetHistDb()
        {
            return new OraDatabase(AppConfigFactory.MesHistConnStr);
        }

        public static void Insert(AutoRunJob runJob)
        {
            IDatabase db = GetHistDb();
            TblAutoRunJobsDao jobsDao = new TblAutoRunJobsDao(db);
            jobsDao.Insert(runJob);
        }

        public static void UpdateExecutedStatus(string jobID,string hostIP,  string autoRunID, string status, string msg, string executeTime, DateTime lastRunTime, DateTime? nextRunTime)
        {
            IDatabase db = GetHistDb();
            TblAutoRunJobsDao jobsDao = new TblAutoRunJobsDao(db);
            
            jobsDao.UpdateExecutedStatus(jobID, hostIP, autoRunID,  status, msg, executeTime, lastRunTime, nextRunTime);
        }

        public static void UpdateRunningStatus(string jobID, string hostIP, string autoRunID, string status, DateTime lastRunTime)
        {
            IDatabase db = GetHistDb();
            TblAutoRunJobsDao jobsDao = new TblAutoRunJobsDao(db);

            jobsDao.UpdateRunningStatus(jobID, hostIP, autoRunID, status, lastRunTime);
        }

        //public static void UpdateLastRunTime(string jobID, string hostIP, string autoRunID ,string status, DateTime lastRunTime)
        //{
        //    IDatabase db = GetHistDb();
        //    TblAutoRunJobsDao jobsDao = new TblAutoRunJobsDao(db);

        //    jobsDao.UpdateLastRunStatus(jobID, hostIP, autoRunID, status, lastRunTime);
        //}


        public static void UpdateEnbableStatus(string jobID, string hostIP, string autoRunID, int enabled, string status, DateTime? nextRunTime)
        {
            IDatabase db = GetHistDb();
            TblAutoRunJobsDao jobsDao = new TblAutoRunJobsDao(db);
            jobsDao.UpdateEnabledStatus(jobID, hostIP, autoRunID, enabled, status, nextRunTime);
        }

        public static void UpdateJob(AutoRunJob editJob)
        {
            IDatabase db = GetHistDb();
            TblAutoRunJobsDao jobsDao = new TblAutoRunJobsDao(db);
            jobsDao.UpdateJob(editJob);
        }

        public static void DeleteJob(string jobID, string hostIP, string autoRunID)
        {
            IDatabase db = GetHistDb();
            TblAutoRunJobsDao jobsDao = new TblAutoRunJobsDao(db);
            jobsDao.DeleteJob(jobID, hostIP, autoRunID);
        }
    }
}
