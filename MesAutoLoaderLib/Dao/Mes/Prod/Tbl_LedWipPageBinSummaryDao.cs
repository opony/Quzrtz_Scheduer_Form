using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseLib.Interfaces;
using MesAutoLoaderLib.Model;
using NLog;
using Oracle.ManagedDataAccess.Client;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class Tbl_LedWipPageBinSummaryDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Tbl_LedWipPageBinSummaryDao(IDatabase database) : base(database)
        {
        }

        public void Delete(LedWipPageBinSummary pageBinSum)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("DELETE FROM tbl_LEDWIPPageBinSummary")
                    .AppendLine("WHERE LOTNO = '" + pageBinSum.LotNo + "'")
                    .AppendLine("AND COMPONENTNO = '" + pageBinSum.ComponentNo + "'")
                    .AppendLine("AND WAFERNO = '" + pageBinSum.WaferNo + "'");

                logger.Debug("SQL : {0}",sql);

                this.ExecuteNonQuery(sql.ToString());

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Delete error.");
                throw ex;
            }
        }

        public void Insert(LedWipPageBinSummary pageBinSum)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO tbl_LEDWIPPageBinSummary (LOTNO, COMPONENTNO, WAFERNO, TESTTIME, BINQTY )")
                    .AppendLine("VALUES(:LotNo, :ComonentNo, :WaferNo, :TestTime, :BinQty)");

                logger.Debug("SQL : {0}",sql );
                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                cmd.Parameters.Add("LotNo", pageBinSum.LotNo);
                cmd.Parameters.Add("ComonentNo", pageBinSum.ComponentNo);
                cmd.Parameters.Add("WaferNo", pageBinSum.WaferNo);
                cmd.Parameters.Add("TestTime", pageBinSum.TestTime);
                cmd.Parameters.Add("BinQty", pageBinSum.BinQty);

                this.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Insert Error.");
                throw ex;
            }
        }
    }
}
