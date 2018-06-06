using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseLib.Interfaces;
using NLog;
using System.Data;

namespace MesAutoLoaderLib.Dao.Mes.MDP
{
    class Dsp_TaskDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Dsp_TaskDao(IDatabase database) : base(database)
        {
        }

        public DataTable QueryPiLotDataByLot(string lotNo)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select mono, Run_sequence,lot_list , status, RID ")
                    .AppendLine("from pkgmdp.DSP_TASK ")
                    .AppendLine("where lot_list like '%" + lotNo + "%'")
                    .AppendLine("and Run_sequence <> 0");
                    //.AppendLine("where status >= 0 and Run_sequence <> 0 ")
                    //.AppendLine("and lot_list like '%" + lotNo + "%'");

                return this.ExecuteQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "QueryPlotDataByLot");
                throw ex;
            }
        }

        

    }
}
