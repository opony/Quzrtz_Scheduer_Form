using MesAutoLoaderLib.Model;
using MesAutoLoaderLib.Model.Config;
using MesAutoLoaderLib.Parsers;
using MesAutoLoaderLib.Proxy;
using MesAutoLoaderLib.Util;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Jobs.Loader
{
    /// <summary>
    /// 分類機 loader
    /// </summary>
    [DisallowConcurrentExecutionAttribute]
    public class ImportPageForWhiteChipJob : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        ClassBinConfig classConfig = new ClassBinConfig();

        string param;
        string notifyMail;
        
        public void Execute(IJobExecutionContext context)
        {
            logger.Info("===== start =====");
            
            notifyMail = Convert.ToString(context.JobDetail.JobDataMap["Notify_Mail"]);
            param = Convert.ToString(context.JobDetail.JobDataMap["Param"]);
            
            try
            {
                logger.Info("Load Job Param.");
                classConfig.Load(param);

                logger.Info("Search csv file form : {0}", classConfig.SourcePath);

                FileInfo[] fileInfos = new DirectoryInfo(classConfig.SourcePath).GetFiles("*.csv", SearchOption.TopDirectoryOnly);
                if (fileInfos.Length == 0)
                {
                    logger.Info("No found any csv file. then end the job.");
                    logger.Info("===== end =====");
                    return;
                }

                //logger.Info("Move files from {0} to {1}", classConfig.SourcePath, classConfig.QueuePath);
                foreach (FileInfo csvFileInfo in fileInfos)
                {
                    
                    logger.Info("{0} move to {1}", csvFileInfo.FullName, classConfig.QueuePath);
                    logger.Info("Start : {0}", csvFileInfo.Name);
                    
                    FileSystemUtil.MoveFile(classConfig.QueuePath, csvFileInfo);

                    ImportFile(csvFileInfo);
                }
                
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ImportPageForWhiteChipJob error.");
                throw ex;
            }

            logger.Info("===== end =====");
        }

        private string ImportFile(FileInfo csvFileInfo)
        {
            string result = "success";
            string dirDayName = null;
            try
            {
                logger.Info("##########################");
                logger.Info("Parse file : {0}", csvFileInfo.FullName);
                ClassBinParser parser = new ClassBinParser(csvFileInfo);
                ClassBinInfo classInfo = parser.Parse();
                logger.Info("Parse completed~!");
                dirDayName = classInfo.TestTime.ToString("yyyyMM");

                logger.Info("SubFolder : {0}", dirDayName);

                logger.Info("Query Lot Info by WAFER_ID : {0}", classInfo.WAFER_ID);
                LotState lotInfo = MesProdDbProxy.QueryLotInofByComponentNo(classInfo.WAFER_ID);
                
                if (lotInfo == null)
                {
                    logger.Warn("無法取得歸屬的生產批號，此生產批必須在WIP!! component no : {0}", classInfo.WAFER_ID);

                    string failFullFolder = Path.Combine(classConfig.FailPath, dirDayName);
                    logger.Info("{0} move to {1}", csvFileInfo.FullName, failFullFolder);
                    FileSystemUtil.MoveFile(failFullFolder, csvFileInfo);
                    logger.Info("Return fail");
                    return "fail";
                }
                classInfo.LOT_ID = lotInfo.LotNo;
                logger.Info("check componentNo [{0}] is exists in LedWipPageSummary ?", classInfo.ComponentNo);
                DataTable pageSumTb = MesProdDbProxy.QueryLedWipPageSummary(lotInfo.LotNo, classInfo.ComponentNo, 1);
                bool isUpdate = pageSumTb.Rows.Count > 0;
                logger.Info("Exists : {0}", isUpdate);
                if (!isUpdate)
                {
                    logger.Info("No exists , then insert LedWipPageSummary data.");
                    LedWipPageSummary pageSum = ConvertToPageSummary(classInfo);
                    MesProdDbProxy.InsertLedWipPageSummary(pageSum);
                }
                logger.Info("Insert LedWipPageBinSummary data");
                List<LedWipPageBinSummary> pageBinSumList = ConvertToPageBinSummaryList(classInfo);
                MesProdDbProxy.InsertLedWipPageBinSummary(pageBinSumList);

                string fullDestFolder = Path.Combine(classConfig.DestinationPath, dirDayName);
                logger.Info("{0} move to folder : {1}", csvFileInfo.FullName, fullDestFolder);
                FileSystemUtil.MoveFile(fullDestFolder, csvFileInfo);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "ImportFile error.");
                string failFullFolder = Path.Combine(classConfig.FailPath, dirDayName);
                logger.Info("{0} move to {1}", csvFileInfo.FullName, failFullFolder);
                FileSystemUtil.MoveFile(failFullFolder, csvFileInfo);

                string[] mailTos = notifyMail.Split(';');

                SendMailUtil.SendMail(mailTos, "ImportPageForWhiteChipJob error. File : " + csvFileInfo.Name, ex.ToString(), new string[] { csvFileInfo.FullName });
                logger.Info("Return fail");
                
            }

            return result;
        }

        private List<LedWipPageBinSummary> ConvertToPageBinSummaryList(ClassBinInfo classInfo)
        {
            Dictionary<string, int> waferCountMap = new Dictionary<string, int>();
            //string waferId;
            //foreach (DataRow row in classInfo.SummaryTb.Rows)
            //{
            //    waferId = row.Field<string>("WaferFrom");
            //    if (waferCountMap.ContainsKey(waferId) == false)
            //    {
            //        waferCountMap.Add(waferId, 0);
            //    }

            //    waferCountMap[waferId]++;
            //}

            waferCountMap = classInfo.SummaryTb.AsEnumerable()
                .Select(row => new { key = row.Field<string>("WaferFrom"), val = row })
                .GroupBy(g => g.key)
                .ToDictionary(a => a.Key, a => a.Count());
            List<LedWipPageBinSummary> pageBinSumList = new List<LedWipPageBinSummary>();
            LedWipPageBinSummary pageBinSum = null;
            foreach (var keyValue in waferCountMap)
            {
                pageBinSum = new LedWipPageBinSummary();
                pageBinSum.LotNo = classInfo.LOT_ID;
                pageBinSum.WaferNo = keyValue.Key;
                pageBinSum.ComponentNo = classInfo.ComponentNo;
                pageBinSum.BinQty = keyValue.Value;
                pageBinSum.TestTime = classInfo.TestTime;

                pageBinSumList.Add(pageBinSum);

            }

            return pageBinSumList;
        }

        private LedWipPageSummary ConvertToPageSummary(ClassBinInfo classInfo)
        {
            LedWipPageSummary pageSum = new LedWipPageSummary();
            pageSum.LotNo = classInfo.LOT_ID;
            pageSum.ComponentNo = classInfo.ComponentNo;
            pageSum.FileID = classInfo.FileID;
            pageSum.TestTime = classInfo.TestTime;
            pageSum.MoNo = classInfo.WO;
            pageSum.CassetteNo = classInfo.CST_ID;
            pageSum.RecipeID = classInfo.Recipe_ID;
            pageSum.RecipeName = classInfo.Recipe_Name;
            pageSum.Operator = classInfo.OPERATOR;
            pageSum.WaferID = classInfo.WAFER_ID;
            pageSum.SubStrateID = classInfo.SUBSTRATE_ID;
            pageSum.FrameID = classInfo.FrameID;
            pageSum.BinCode = classInfo.Bin_CODE;
            pageSum.BinQty = classInfo.BinQty;
            pageSum.SorterID = classInfo.SorterID;
            pageSum.StartTime = classInfo.StartTime;
            pageSum.EndTime = classInfo.EndTime;
            pageSum.CreateLot = 0;

            return pageSum;
        }
        //private Dictionary<string, object> CalculateWipPageSummaryData(ClassBinInfo classInfo)
        //{
        //    Dictionary<string, object> pageSummaryMap = new Dictionary<string, object>();

        //    pageSummaryMap.Add("LOTNO", classInfo.LOT_ID);
        //    pageSummaryMap.Add("COMPONENTNO", classInfo.ComponentNo);
        //    pageSummaryMap.Add("FILEID", classInfo.FileID);
        //    pageSummaryMap.Add("TESTTIME", classInfo.TestTime);
        //    pageSummaryMap.Add("MONO", classInfo.WO);
        //    pageSummaryMap.Add("CASSETTE", classInfo.CST_ID);
        //    pageSummaryMap.Add("RECIPE_ID", classInfo.Recipe_ID);
        //    pageSummaryMap.Add("RECIPE_NAME", classInfo.Recipe_Name);
        //    pageSummaryMap.Add("OPERATOR", classInfo.OPERATOR);
        //    pageSummaryMap.Add("WAFER_ID", classInfo.WAFER_ID);
        //    pageSummaryMap.Add("SUBSTRATE_ID", classInfo.SUBSTRATE_ID);
        //    pageSummaryMap.Add("FRAME_ID", classInfo.FrameID);
        //    pageSummaryMap.Add("BINCODE", classInfo.Bin_CODE);
        //    pageSummaryMap.Add("BINQTY", classInfo.BinQty);
        //    pageSummaryMap.Add("SORTERID", classInfo.SorterID);
        //    pageSummaryMap.Add("STARTTIME", classInfo.StartTime);
        //    pageSummaryMap.Add("ENDTIME", classInfo.EndTime);
        //    pageSummaryMap.Add("STATUS", 0);

        //    //statistic number column's Max, Min , Std, Avg
        //    //var enumRows = classInfo.SummaryTb.AsEnumerable();
        //    //string colName;
        //    //foreach (DataColumn col in classInfo.SummaryTb.Columns)
        //    //{
        //    //    colName = col.ColumnName.ToUpper();
        //    //    if (colName == "WAFERFROM" || colName == "BIN")
        //    //        continue;
        //    //    else
        //    //    {
        //    //        var values = (from row in enumRows
        //    //                     select Convert.ToDouble(row[col])).ToArray();

        //    //        double average = values.Average();
        //    //        double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
        //    //        double std = Math.Sqrt(sumOfSquaresOfDifferences / values.Length);

        //    //        pageSummaryMap.Add(colName + "_MAX", values.Max());
        //    //        pageSummaryMap.Add(colName + "_MIN", values.Max());
        //    //        pageSummaryMap.Add(colName + "_AVG", average);
        //    //        pageSummaryMap.Add(colName + "_STD", std);

        //    //    }
        //    //}

            
        //    return pageSummaryMap;
        //}

        //private void MoveToFolder(string newFolder, FileInfo fileInfo)
        //{

        //    string newFullPath = Path.Combine(newFolder, fileInfo.Name);

        //    File.Delete(newFullPath);
        //    logger.Debug("{0} move to {1}", fileInfo.FullName, newFullPath);
        //    fileInfo.MoveTo(newFullPath);
        //}
    }
}
