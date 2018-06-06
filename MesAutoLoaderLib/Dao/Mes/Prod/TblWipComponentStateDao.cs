using DatabaseLib.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class TblWipComponentStateDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TblWipComponentStateDao(IDatabase database) : base(database)
        {
        }

        public DataTable QueryCompomentsJoinWipCompTestSummary(string lotNo)
        {
            DataTable tb = null;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Select A.ComponentNo")
                    .AppendLine(",(Select B.FileID From tbl_LEDWIPCompTestSummary B ")
                    .AppendLine("   Where B.ComponentNo = A.ComponentNo And B.Active = 1 ")
                    .AppendLine("     And B.TestType = (Select Max(C.TestType) From tbl_LEDWIPCompTestSummary C Where C.TestType In (2,3) And C.Active = 1 And C.ComponentNo = A.ComponentNo)) As FileName ")
                    .AppendLine(",(Select B.TestTime From tbl_LEDWIPCompTestSummary B ")
                    .AppendLine("   Where B.ComponentNo = A.ComponentNo And B.Active = 1 ")
                    .AppendLine("     And B.TestType = (Select Max(C.TestType) From tbl_LEDWIPCompTestSummary C Where C.TestType In (2,3) And C.Active = 1 And C.ComponentNo = A.ComponentNo)) As TestTime ")
                    .AppendLine("  From tblWIPComponentState A ")
                    .AppendLine(" Where A.LotNo = '" + lotNo + "'");

                logger.Debug("SQL : {0}", sql);
                tb = this.ExecuteQuery(sql.ToString());


            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error.");
                throw new Exception("QueryCompoments error.", ex);
            }

            return tb;
        }

        public DataTable QueryBinQtyByLotNo(string lotNo)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select b.bincode,sum(b.binqty) as BINQTY from ")
                    .AppendLine("TBLWIPCOMPONENTSTATE a, TBL_LEDWIPCOMPTESTBINSUMMARY b")
                    .AppendLine("where(a.componentno = b.componentno) and b.TestTYpe = 2")
                    .AppendLine("and upper(bincode) not like 'NOTG%' ")
                    .AppendLine("and upper(bincode) not like 'SIDE%' ")
                    .AppendLine("and a.lotno='" + lotNo + "' ")
                    .AppendLine(" group by b.bincode ");

                logger.Debug("SQL : {0}", sql.ToString());
                return this.ExecuteQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "QueryBinQtyByLotNo error.");
                throw ex;
            }
        }

        public void UpdateGoodQty(string lotNo, string componentNo, int goodQty)
        {
            try
            {
                string sql = "Update tblWIPComponentState Set GoodQty = " + goodQty +" Where LotNo = '" + lotNo + "' And ComponentNo = '" + componentNo + "'";
                logger.Debug("SQL : {0}",sql);
                this.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "UpdateGoodQty error.");
                throw ex;
            }
        }
    }
}
