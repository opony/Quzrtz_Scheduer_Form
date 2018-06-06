using DatabaseLib.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class TblPrdLedBinProgramBinDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TblPrdLedBinProgramBinDao(IDatabase database) : base(database)
        {
        }

        public DataTable QueryBinChangeDefineData(string programNo)
        {
            try
            {
                string sql = "select binno,bincode from TBLPRDLEDBINPROGRAMBIN where programno ='" + programNo + "'";
                logger.Debug("SQL : {0}", sql);

                return this.ExecuteQuery(sql);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "QueryBinChangeDefineData error.");
                throw ex;
            }
        }
    }
}
