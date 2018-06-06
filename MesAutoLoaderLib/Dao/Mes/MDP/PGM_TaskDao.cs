using DatabaseLib.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Dao.Mes.MDP
{
    class PGM_TaskDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public PGM_TaskDao(IDatabase database) : base(database)
        {
        }

        public DataTable QueryPGMTaskData(string[] rids)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.rid, b.STATUS ")
                    .AppendLine("from pkgmdp.PGM_TASK_MERGE a, pkgmdp.PGM_TASK b")
                    .AppendLine("where a.potid = b.potid ")
                    .AppendLine("and a.rid in ('" + string.Join("','", rids) + "')");

                return this.ExecuteQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "QueryPGMTaskData error.");
                throw ex;
            }
        }

    }
}
