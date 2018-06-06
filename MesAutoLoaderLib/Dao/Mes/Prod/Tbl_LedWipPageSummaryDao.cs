using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseLib.Interfaces;
using NLog;
using MesAutoLoaderLib.Model;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class Tbl_LedWipPageSummaryDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Tbl_LedWipPageSummaryDao(IDatabase database) : base(database)
        {
        }

        public void DeleteByComponentNo(string lotNo, string componentNo, int createLot)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("DELETE FROM tbl_LEDWIPPageSummary")
                    .AppendLine("Where CreateLot = " + createLot)
                    .AppendLine("And ComponentNo = '" + componentNo + "'")
                    .AppendLine("And LotNo = '" + lotNo + "'");
                logger.Debug("SQL : {0}", sql);
                this.ExecuteNonQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "DeleteByComponentNo error.");
                throw ex;
            }
        }
        public DataTable Query(string lotNo, string componentNo, int createLot)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT * From tbl_LEDWIPPageSummary ")
                    .AppendLine("Where CreateLot = " + createLot)
                    .AppendLine("And ComponentNo = '" + componentNo + "'")
                    .AppendLine("And LotNo = '" + lotNo + "'");
                logger.Debug("SQL : {0}", sql);
                return this.ExecuteQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Query error.");
                throw ex;
            }
        }

        public void Insert(LedWipPageSummary pageSum)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO tbl_LEDWIPPageSummary ( LOTNO, COMPONENTNO, FILEID, TESTTIME ,MONO, RECIPE_ID, RECIPE_NAME, WAFER_ID, FRAME_ID, BINCODE, BINQTY, SORTERID, STARTTIME, ENDTIME, STATUS )")
                    .AppendLine("VALUES(:LotNo, :ComponentNo, :FileID, :TestTime, :MoNo, :RecipeID, :RecipeName, :WaferID, :FrameID, :BinCode, :BinQty, :SorterID, :StartTime, :EndTime, :Status)");

                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                cmd.Parameters.Add("LotNo", pageSum.LotNo);
                cmd.Parameters.Add("ComponentNo", pageSum.ComponentNo);
                cmd.Parameters.Add("FileID", pageSum.FileID);
                cmd.Parameters.Add("TestTime", pageSum.TestTime);
                cmd.Parameters.Add("MoNo", pageSum.MoNo);
                cmd.Parameters.Add("RecipeID", pageSum.RecipeID);
                cmd.Parameters.Add("RecipeName", pageSum.RecipeName);
                cmd.Parameters.Add("WaferID", pageSum.WaferID);
                cmd.Parameters.Add("FrameID", pageSum.FrameID);
                cmd.Parameters.Add("BinCode", pageSum.BinCode);
                cmd.Parameters.Add("BinQty", pageSum.BinQty);
                cmd.Parameters.Add("SorterID", pageSum.SorterID);
                cmd.Parameters.Add("StartTime", pageSum.StartTime);
                cmd.Parameters.Add("EndTime", pageSum.EndTime);
                cmd.Parameters.Add("Status", pageSum.Status);

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
