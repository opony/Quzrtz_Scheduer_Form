using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseLib.Interfaces;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using MesAutoLoaderLib.Model;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class Tbl_LedWipCompTestBinSummaryDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Tbl_LedWipCompTestBinSummaryDao(IDatabase database) : base(database)
        {
        }

        public void Delete(string lotNo, string componentNo, int testType, DateTime testTime)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Delete FROM tbl_LEDWIPCompTestBinSummary ")
                    .AppendLine("Where LotNo = :LotNo")
                    .AppendLine("and ComponentNo = :ComponentNo")
                    .AppendLine("And TestType = :TestType")
                    .AppendLine("And TestTime = :TestTime");

                logger.Debug("SQL : {0}", sql);

                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                cmd.Parameters.Add("LotNo", lotNo);
                cmd.Parameters.Add("ComponentNo", componentNo);
                cmd.Parameters.Add("TestType", testType);
                cmd.Parameters.Add("TestTime", testTime);

                this.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Delete error.");
                throw new Exception("Delete Tbl_LedWipCompTestBinSummary error.", ex) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tb">map data table</param>
        /// <param name="binCodeChgKeys">定義在 Bin 表的 Bin code</param>
        public void Insert(List<LedWipCompTestBinSummary> compTestBinSumList)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Insert Into tbl_LEDWIPCompTestBinSummary (LotNo, ComponentNo,TestType,TestTime, BinNo,BinQty,Percentage,BinGrade,BinCode,BinMinQty,TapeMinQty,BinFlag,InventoryNo,MaxBinFlag, INSERT_TIME) ")
                    .AppendLine("VALUES(:LotNo, :ComponentNo, :TestType, :TestTime, :BinNo, :BinQty, :Percentage, :BinGrade, :BinCode, :BinMinQty, :TapeMinQty, :BinFlag, :InventoryNo, :MaxBinFlag, sysdate)");

                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                foreach (LedWipCompTestBinSummary testBinSum in compTestBinSumList)
                {
                    cmd.Parameters.Add("LotNo", testBinSum.LotNo);
                    cmd.Parameters.Add("ComponentNo", testBinSum.ComponentNo);
                    cmd.Parameters.Add("TestType", testBinSum.TestType);
                    cmd.Parameters.Add("TestTime", testBinSum.TestTime);
                    cmd.Parameters.Add("BinNo", testBinSum.BinNo);
                    cmd.Parameters.Add("BinQty", testBinSum.BinQty);
                    cmd.Parameters.Add("Percentage", testBinSum.Percentage);
                    cmd.Parameters.Add("BinGrade", testBinSum.BinGrade);
                    cmd.Parameters.Add("BinCode", testBinSum.BinCode);
                    cmd.Parameters.Add("BinMinQty", testBinSum.BinMinQty);
                    cmd.Parameters.Add("TapeMinQty", testBinSum.TapeMinQty);
                    cmd.Parameters.Add("BinFlag", testBinSum.BinFlag);
                    cmd.Parameters.Add("InventoryNo", testBinSum.InventoryNo);
                    cmd.Parameters.Add("MaxBinFlag", testBinSum.MaxBinFlag);

                    this.ExecuteNonQuery(cmd);
                    cmd.Parameters.Clear();
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Tbl_LedWipCompTestBinSummary insert error.");
                throw ex;
            }
        }
    }
}
