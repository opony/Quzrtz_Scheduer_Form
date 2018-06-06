using DatabaseLib.Database;
using DatabaseLib.Interfaces;
using MesAutoLoaderLib.Dao.Mes.MDP;
using MesAutoLoaderLib.Dao.Mes.Prod;
using MesAutoLoaderLib.Model;
using MesAutoLoaderLib.Util;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Proxy
{
    class MesProdDbProxy
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //public static void UpdateTbl_LedWipCompTestSummaryActiveStatus(string lotNo, int testType, int active)
        //{
        //    IDatabase db = GetDb();
        //    Tbl_LedWipCompTestSummaryDao wipCompTestSumDao = new Tbl_LedWipCompTestSummaryDao(db);
        //    wipCompTestSumDao.UpdateActiveByLotNoAndTestType(lotNo, testType, active);
        //}

        public static DataTable QueryLedWipPageSummary(string lotNo, string componentNo, int createLot)
        {
            IDatabase db = GetDb();
            Tbl_LedWipPageSummaryDao pageSumDao = new Tbl_LedWipPageSummaryDao(db);
            return pageSumDao.Query(lotNo, componentNo, createLot);
        }

        public static void InsertLedWipPageBinSummary(List<LedWipPageBinSummary> pageBinSumList)
        {
            IDatabase db = GetDb();
            db.GetConnection();
            db.BeginTrans();
            try
            {
                Tbl_LedWipPageBinSummaryDao pageBinSumDao = new Tbl_LedWipPageBinSummaryDao(db);
                foreach (LedWipPageBinSummary pageBinSum in pageBinSumList)
                {
                    pageBinSumDao.Delete(pageBinSum);
                    pageBinSumDao.Insert(pageBinSum);
                }

                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                throw ex;
            }
            finally
            {
                db.Close();
            }
        }

        
        public static void InsertLedWipPageSummary(LedWipPageSummary pageSummary)
        {
            IDatabase db = GetDb();
            db.GetConnection();
            db.BeginTrans();
            try
            {
                Tbl_LedWipPageSummaryDao pageSumDao = new Tbl_LedWipPageSummaryDao(db);
                pageSumDao.DeleteByComponentNo(pageSummary.LotNo, pageSummary.ComponentNo, 0);
                pageSumDao.Insert(pageSummary);

                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                throw ex;
            }
            finally
            {
                db.Close();
            }
            
        }


        /// <summary>
        /// Query 列印調膠單記錄
        /// </summary>
        /// <param name="lotNo"></param>
        /// <returns></returns>
        public static DataTable QueryPiLotDataByLot(string lotNo)
        {
            IDatabase db = GetDb();
            Dsp_TaskDao dspTaskDao = new Dsp_TaskDao(db);
            return dspTaskDao.QueryPiLotDataByLot(lotNo);
        }

        /// <summary>
        /// Query 調膠程式的記錄
        /// </summary>
        /// <param name="rids">Run ID</param>
        /// <returns></returns>
        public static DataTable QueryPGMTaskDataByRunID(string[] rids)
        {
            IDatabase db = GetDb();
            PGM_TaskDao pkgTaskDao = new PGM_TaskDao(db);
            return pkgTaskDao.QueryPGMTaskData(rids);
        }

        public static DataTable QueryUserBasisByGroupNo(string groupNo)
        {
            IDatabase db = GetDb();
            TblUsrUserBasisDao userBasisDao = new TblUsrUserBasisDao(db);
            return userBasisDao.QueryUserBasisByGroupNo(groupNo);
        }

        public static void UpdateTbl_LedWipCompTestSummaryActiveStatus(string componentNo, int testType, int active)
        {
            IDatabase db = GetDb();
            Tbl_LedWipCompTestSummaryDao wipCompTestSumDao = new Tbl_LedWipCompTestSummaryDao(db);
            wipCompTestSumDao.UpdateActiveByComponentNoAndTestType(componentNo, testType, active);
        }


        public static DataTable QueryWipTempReSortData(int status, string hostName)
        {
            IDatabase db = GetDb();
            TblWipTemp_ReSortDao wipTempReSortDao = new TblWipTemp_ReSortDao(db);
            return wipTempReSortDao.QueryByStatus(status, hostName);
        }

        public static void UpdateWipTemp_ReSortStartStatus(int status, string hostName, DateTime stTime, string lotSerial)
        {
            IDatabase db = GetDb();
            TblWipTemp_ReSortDao wipTempReSortDao = new TblWipTemp_ReSortDao(db);
            wipTempReSortDao.UpdateStartTimeStatus(status, hostName, stTime, lotSerial);
        }

        public static void UpdateWipTemp_ReSortEndStatus(int status, DateTime endTime, string lotSerial)
        {
            IDatabase db = GetDb();
            TblWipTemp_ReSortDao wipTempReSortDao = new TblWipTemp_ReSortDao(db);
            wipTempReSortDao.UpdateEndTimeStatus(status, endTime, lotSerial);
        }

        public static DataTable QueryCompoments(string lotNo)
        {
            IDatabase db = GetDb();
            TblWipComponentStateDao wipCompStateDao = new TblWipComponentStateDao(db);
            return wipCompStateDao.QueryCompomentsJoinWipCompTestSummary(lotNo);
        }

        public static void UpdateTempCompomentReSortStart(int status, string fileName, DateTime stTime, string lotNo, string compomentNo, string lotSerial)
        {
            IDatabase db = GetDb();
            TblWipTemp_CompomentReSortDao compoReSortDao = new TblWipTemp_CompomentReSortDao(db);
            compoReSortDao.UpdateStartTime(status, fileName, stTime, lotNo, compomentNo, lotSerial);
        }

        public static void UpdateTempComponentReSortEndStatus(int status, string result, DateTime endTime, string lotNo, string compoNo, string lotSerial)
        {
            IDatabase db = GetDb();
            TblWipTemp_CompomentReSortDao compoReSortDao = new TblWipTemp_CompomentReSortDao(db);
            compoReSortDao.UpdateResultStatus(status, result, endTime, lotNo, compoNo, lotSerial);
        }

        public static LotState QueryLotInofByComponentNo(string compoNo)
        {
            IDatabase db = GetDb();
            TblWipLotStateDao lotStateDao = new TblWipLotStateDao(db);
            return lotStateDao.QueryLotInfoByComponentNo(compoNo);
        }

        public static DataTable QueryBinQtyByLotNo(string lotNo)
        {
            IDatabase db = GetDb();
            TblWipComponentStateDao componentStateDao = new TblWipComponentStateDao(db);

            return componentStateDao.QueryBinQtyByLotNo(lotNo);
        }


        public static DataTable QueryBinChagneDefineData(string programNo)
        {
            IDatabase db = GetDb();
            TblPrdLedBinProgramBinDao binProgBinDao = new TblPrdLedBinProgramBinDao(db);
            return binProgBinDao.QueryBinChangeDefineData(programNo);
        }

        public static void DeleteLedWipCompTestSummary(string lotNo, string componentNo, int testType, DateTime testTime)
        {
            IDatabase db = GetDb();
            Tbl_LedWipCompTestSummaryDao wipCompTestSumDao = new Tbl_LedWipCompTestSummaryDao(db);
            wipCompTestSumDao.Delete(lotNo, componentNo, testType, testTime);
        }
        

        public static void UpdateWipTemp_ComponentAttribScrapQty(string lotNo, string componentNo, int dataMapCount)
        {
            IDatabase db = GetDb();
            TblWipTemp_ComponentAttribDao compAttribDao = new TblWipTemp_ComponentAttribDao(db);
            compAttribDao.UpdateAttribScrapQty(lotNo, componentNo, dataMapCount);
        }

        public static void UpdateWipTemp_ComponentAttrib(string lotNo, string componentNo, string attNo, string attValue)
        {
            IDatabase db = GetDb();
            TblWipTemp_ComponentAttribDao compAttribDao = new TblWipTemp_ComponentAttribDao(db);
            compAttribDao.UpdateAttribValue(lotNo, componentNo, attNo, attValue);
        }

        public static void UpdateWipCont_ComponentAttrib(string lotNo, string lotSerial, string componentNo, string attNo, string attValue)
        {
            IDatabase db = GetDb();
            TblWipCont_ComponentAttribDao wipContAttribDao = new TblWipCont_ComponentAttribDao(db);
            wipContAttribDao.UpdateAttribValue(lotNo, lotSerial, componentNo, attNo, attValue);
        }

        public static void UpdateCompomentStateGoodQty(string lotNo, string componentNo, int goodQty)
        {
            IDatabase db = GetDb();
            TblWipComponentStateDao compoStateDao = new TblWipComponentStateDao(db);
            compoStateDao.UpdateGoodQty(lotNo, componentNo, goodQty);
        }

        public static void InsertWipCompTestSummayData(ProberTestResult probResult, LedWipCompTestSummary compTestSum, List<LedWipCompTestBinSummary> compTestBinSumList)
        {
            IDatabase db = GetDb();
            try
            {
                db.GetConnection();
                db.BeginTrans();
                logger.Debug("BeginTrans");
                Tbl_LedWipCompTestSummaryDao wipCompTestSumDao = new Tbl_LedWipCompTestSummaryDao(db);
                logger.Debug("Delete WipCompTestSummary data");
                wipCompTestSumDao.Delete(compTestSum.LotNo, probResult.ComponentNo, compTestSum.TestType, probResult.TestTime);

                //LedWipCompTestSummary compTestSum = new LedWipCompTestSummary();
                //compTestSum.LotNo = lotNo;
                //compTestSum.ComponentNo = probResult.ComponentNo;
                //compTestSum.TestType = testType;
                //compTestSum.Active = 1;
                //compTestSum.FileID = probResult.FileName;
                //compTestSum.TestTime = probResult.TestTime;
                //compTestSum.MoNo = moNo;
                //compTestSum.EquipmentNo = probResult.EQP_ID;
                //compTestSum.TestQty = probResult.QTY;
                //compTestSum.GoodQty = probResult.QTY;
                ////compTestSum.TotalYield = 0;
                //compTestSum.BinQty = 0;
                //compTestSum.StartTime = probResult.TestTime;
                //compTestSum.EndTime = DateTime.Now;

                logger.Debug("Insert WipCompTestSummary data");
                wipCompTestSumDao.Insert(compTestSum);

                //logger.Info("Update WipComponent Attribute CYield value");
                //TblWipTemp_ComponentAttribDao compAttribDao = new TblWipTemp_ComponentAttribDao(db);
                //compAttribDao.UpdateAttribValue(compTestSum.LotNo, probResult.ComponentNo, "CYield", compTestSum.TotalYield.ToString() + "%");

                //TblWipCont_ComponentAttribDao wipContAttribDao = new TblWipCont_ComponentAttribDao(db);
                //wipContAttribDao.UpdateAttribValue(compTestSum.LotNo, lotSerial, probResult.ComponentNo, "CYield", compTestSum.TotalYield.ToString() + "%");

                Tbl_LedWipCompTestBinSummaryDao testBinSumDao = new Tbl_LedWipCompTestBinSummaryDao(db);
                logger.Debug("Delete WipCompTestBinSummary data");
                testBinSumDao.Delete(compTestSum.LotNo, probResult.ComponentNo, compTestSum.TestType, probResult.TestTime);

                logger.Debug("Insert WipCompTestBinSummary data");
                testBinSumDao.Insert(compTestBinSumList);
                logger.Debug("Commit.");
                db.Commit();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Insert WipCompTestSummay and WipCompTestBinSummary error");
                logger.Debug("Rollback.");
                db.Rollback();
                throw;
            }
            finally
            {
                db.Close();
            }
            

        }

        
        private static IDatabase GetDb()
        {
            return new OraDatabase(AppConfigFactory.MesProdConnStr);
        }
    }
}
