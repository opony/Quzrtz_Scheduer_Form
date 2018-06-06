using DatabaseLib.Interfaces;
using MesAutoLoaderLib.Model;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class Tbl_LedWipCompTestSummaryDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Tbl_LedWipCompTestSummaryDao(IDatabase database) : base(database)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="testType">Prober : 2, 套 Bin : 4</param>
        /// <param name="active">active : 1, non active : 0</param>
        /// <returns></returns>
        //public void UpdateActiveByLotNoAndTestType(string lotNo, int testType, int active)
        //{
        //    try
        //    {
        //        StringBuilder sql = new StringBuilder();
        //        sql.AppendLine("UpDate tbl_LEDWIPCompTestSummary Set Active = " + active)
        //            .AppendLine("Where LotNo = '" + lotNo + "'")
        //            .AppendLine("And TestType = " + testType);
        //        logger.Debug("SQL : {0}", sql);

        //        this.ExecuteNonQuery(sql.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex, "tbl_LEDWIPCompTestSummary UpdateActiveByLotNoAndTestType error.");
        //        throw new Exception("Update Tbl_LedWipCompTestSummary Active error.", ex);
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="testType">Prober : 2, 套 Bin : 4</param>
        /// <param name="active">active : 1, non active : 0</param>
        /// <returns></returns>
        public void UpdateActiveByComponentNoAndTestType(string componentNo, int testType, int active)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UpDate tbl_LEDWIPCompTestSummary Set Active = " + active)
                    .AppendLine("Where ComponentNo = '" + componentNo + "'")
                    .AppendLine("And TestType = " + testType);
                logger.Debug("SQL : {0}", sql);

                this.ExecuteNonQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "tbl_LEDWIPCompTestSummary UpdateActiveByLotNoAndTestType error.");
                throw new Exception("Update Tbl_LedWipCompTestSummary Active error.", ex);
            }
        }


        public void Delete(string lotNo, string componentNo, int testType, DateTime testTime)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Delete From tbl_LEDWIPCompTestSummary ")
                    .AppendLine("Where LotNo = :LotNo")
                    .AppendLine("AND ComponentNo = :ComponentNo")
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
                logger.Error(ex, "tbl_LEDWIPCompTestSummary Delete error.");
                throw new Exception("tbl_LEDWIPCompTestSummary Delete error.", ex);
            }
        }

        public void Insert(LedWipCompTestSummary wipCompTestSum)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Insert into tbl_LEDWIPCompTestSummary(LotNo, ComponentNo, TestType, Active, FileID, TestTime, MONo, EquipmentNo,")
                    .AppendLine("CassetteNo, Recipe_ID, Recipe_Name, Operator, Product, SpecNo, TestQty, GoodQty, TotalYield, BinQty, StartTime, EndTime, INSERT_TIME)")
                    .AppendLine("VALUES(:LotNo, :ComponentNo, :TestType, :Active, :FileID, :TestTime, :MoNo, :EquipmentNo, :CassetteNo, :Recipe_ID, :Recipe_Name, :Operator,")
                    .AppendLine(":Product, :SpecNo, :TestQty, :GoodQty, :TotalYield, :BinQty, :StartTime, :EndTime, sysdate)");

                OracleCommand cmd = new OracleCommand(sql.ToString());
                cmd.BindByName = true;
                cmd.Parameters.Add("LotNo", wipCompTestSum.LotNo);
                cmd.Parameters.Add("ComponentNo", wipCompTestSum.ComponentNo);
                cmd.Parameters.Add("TestType", wipCompTestSum.TestType);
                cmd.Parameters.Add("Active", wipCompTestSum.Active);
                cmd.Parameters.Add("FileID", wipCompTestSum.FileID);
                cmd.Parameters.Add("TestTime", wipCompTestSum.TestTime);
                cmd.Parameters.Add("MoNo", wipCompTestSum.MoNo);
                cmd.Parameters.Add("EquipmentNo", wipCompTestSum.EquipmentNo);
                cmd.Parameters.Add("CassetteNo", wipCompTestSum.CassetteNo);
                cmd.Parameters.Add("Recipe_ID", wipCompTestSum.Recipe_ID);
                cmd.Parameters.Add("Recipe_Name", wipCompTestSum.Recipe_Name);
                cmd.Parameters.Add("Operator", wipCompTestSum.Operator);
                cmd.Parameters.Add("Product", wipCompTestSum.Product);
                cmd.Parameters.Add("SpecNo", wipCompTestSum.SpecNo);
                cmd.Parameters.Add("TestQty", wipCompTestSum.TestQty);
                cmd.Parameters.Add("GoodQty", wipCompTestSum.GoodQty);
                cmd.Parameters.Add("TotalYield", wipCompTestSum.TotalYield);
                cmd.Parameters.Add("BinQty", wipCompTestSum.BinQty);
                cmd.Parameters.Add("StartTime", wipCompTestSum.StartTime);
                cmd.Parameters.Add("EndTime", wipCompTestSum.EndTime);

                this.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                logger.Error(ex,"Insert Error");
                throw new Exception("tbl_LEDWIPCompTestSummary Insert error.", ex) ;
            }
        }
    }
}
