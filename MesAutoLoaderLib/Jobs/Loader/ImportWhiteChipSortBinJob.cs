using MesAutoLoaderLib.Interfaces.Rule;
using MesAutoLoaderLib.Model;
using MesAutoLoaderLib.Model.Config;
using MesAutoLoaderLib.Parsers;
using MesAutoLoaderLib.Proxy;
using MesAutoLoaderLib.Rules;
using MesAutoLoaderLib.Util;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MesAutoLoaderLib.Jobs.Loader
{

    /// <summary>
    /// Initial 2018-03-13 by pony
    /// Mes 套 Bin loader
    /// </summary>
    /// 
    [DisallowConcurrentExecutionAttribute]
    public class ImportWhiteChipSortBinJob : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly int RE_SORT_QUEUE = 0;
        private static readonly int RE_SORT_START = 1;
        private static readonly int RE_SORT_END = 2;
        private static readonly int RE_SORT_ERROR = 3;

        private static readonly int TEST_TYPE_BIN_COLLECTION = 4;
        
        SortBinConfig sortBinConfig = new SortBinConfig();

        String hostName = Dns.GetHostName().ToUpper();

        //Dictionary<string, decimal> binCodeQtyMap;


        string param;
        string notifyMail;

        public void Execute(IJobExecutionContext context)
        {

            logger.Info("===== start =====");
            notifyMail = Convert.ToString(context.JobDetail.JobDataMap["Notify_Mail"]);
            param = Convert.ToString(context.JobDetail.JobDataMap["Param"]);

            try
            {
                logger.Debug("Input :");
                logger.Debug("param : {0}", param);

                sortBinConfig.Load(param);

                //FileSystemUtil.CreateFolderIfNoExists(sortBinConfig.DestinationPath);
                FileSystemUtil.CreateFolderIfNoExists(sortBinConfig.EqPath);
                FileSystemUtil.CreateFolderIfNoExists(sortBinConfig.QueuePath);
                


                DataTable tempReSortTb = MesProdDbProxy.QueryWipTempReSortData(RE_SORT_QUEUE, hostName);


                string lotSerial;
                string lotNo;
                string pageIDGroupNo;
                string impResult;
                foreach (DataRow reSortRow in tempReSortTb.Rows)
                {
                    lotSerial = Convert.ToString(reSortRow["LOTSERIAL"]);
                    MesProdDbProxy.UpdateWipTemp_ReSortStartStatus(RE_SORT_START, hostName, DateTime.Now, lotSerial);

                    lotNo = Convert.ToString(reSortRow["LOTNO"]);
                    //MesProdDbProxy.UpdateTbl_LedWipCompTestSummaryActiveStatus(lotNo, TEST_TYPE_BIN_COLLECTION, 0);



                    pageIDGroupNo = lotNo.Substring(0, 5) + DateTime.Now.ToString("yyyy").Substring(3, 1) + "S00001";
                    logger.Info("Page ID Group No : {0}", pageIDGroupNo);

                    impResult = ImportByLot(lotNo, lotSerial, pageIDGroupNo);

                    if (impResult == "success")
                        MesProdDbProxy.UpdateWipTemp_ReSortEndStatus(RE_SORT_END, DateTime.Now, lotSerial);
                    else
                        MesProdDbProxy.UpdateWipTemp_ReSortEndStatus(RE_SORT_ERROR, DateTime.Now, lotSerial);
                }



            }
            catch (Exception ex)
            {
                logger.Error(ex, "ImportWhiteChipSortBin error.");
                throw ex;
            }
            logger.Info("===== end =====");
        }

        private string ImportByLot(string lotNo, string lotSerial, string pageIDGroupNo)
        {
            logger.Info("<<<<< ImportByLot >>>>>");
            logger.Info("ImportByLot : {0}", lotNo);
            try
            {
                logger.Info("Query Compoment No list by Lot");
                string fileName;
                string componentNo;
                DateTime testTime;
                DataTable compomTb = MesProdDbProxy.QueryCompoments(lotNo);
                string result;

                foreach (DataRow compoRow in compomTb.Rows)
                {
                    fileName = Convert.ToString(compoRow["FILENAME"]);
                    componentNo = Convert.ToString(compoRow["COMPONENTNO"]);
                    testTime = compoRow.Field<DateTime>("TESTTIME");
                    logger.Info("Call ImportByComponent");
                    result = ImportByComponent(lotNo, lotSerial, pageIDGroupNo, componentNo, fileName, testTime);
                    logger.Info("ImportByComponent result : {0}", result);
                    if (result != "success")
                        return result;
                }



            }
            catch (Exception ex)
            {
                logger.Error(ex, "ImportByLot error.");
                logger.Info("Return : {0}", "Error." + ex.ToString());
                return "Error." + ex.ToString();
            }
            return "success";
        }

        public string ImportByComponent(string lotNo, string lotSerial, string pageIDGroupNo, string componentNo, string fileName, DateTime testTime)
        {
            logger.Info("<<<<< ImportByComponent >>>>>");
            FileInfo fileInfo = null;
            string dirNameByDay = "";
            try
            {
                MesProdDbProxy.UpdateTempCompomentReSortStart(1, fileName, DateTime.Now, lotNo, componentNo, lotSerial);
                dirNameByDay = testTime.ToString("yyyyMM");

                string filePath = Path.Combine(sortBinConfig.SourcePath, dirNameByDay);

                logger.Info("Get file {0}", Path.Combine(filePath, fileName));
                FileInfo[] files = FileSystemUtil.GetFiles(filePath, fileName, SearchOption.TopDirectoryOnly);
                if (files.Length == 0)
                {
                    MesProdDbProxy.UpdateTempComponentReSortEndStatus(3, "檔案不存在", DateTime.Now, lotNo, componentNo, lotSerial);
                    logger.Warn("Compoment no " + componentNo + " : " + filePath + "/" + fileName + " , file not exists.");
                    return "Compoment no " + componentNo + " : " + filePath + "/" + fileName + " , file not exists.";
                }

                fileInfo = files[0];
                string queFullFileName = Path.Combine(sortBinConfig.QueuePath, fileInfo.Name);
                if (File.Exists(queFullFileName))
                {
                    logger.Info("Queue path have be exists file, and delete file : {0}", queFullFileName);
                    File.Delete(queFullFileName);
                }

                logger.Info("Move {0} to {1}", fileInfo.FullName, queFullFileName);
                fileInfo.MoveTo(queFullFileName);
                //File.Move(fileInfo.FullName, queFullFileName);

                //fileInfo = new FileInfo(queFullFileName);
                logger.Info("Parse [{0}]", fileInfo.FullName);
                ProberCsvParser parser = new ProberCsvParser(fileInfo);
                ProberTestResult probResult = parser.Parse();
                probResult.PageIDGroupNo = pageIDGroupNo;

                logger.Info("Parse completed~!");

                LotState lotInfo = MesProdDbProxy.QueryLotInofByComponentNo(componentNo);
                if (lotInfo == null)
                {
                    logger.Warn("無法取得歸屬的生產批號，此生產批必須在WIP!! component no : {0}", componentNo);
                    return "Error : 無法取得歸屬的生產批號，此生產批必須在WIP!!, component no : " + componentNo;
                }

                //Dictionary<string, string> binNoChangeMap = GetBinNoChangeMap(lotInfo);
                //if (binNoChangeMap.Keys.Count == 0)
                //{
                //    throw new Exception("Can't query the Bin Chagne Define Data ");
                //}

                probResult.WO = lotInfo.MoNo;
                logger.Info("Mono : {0}", lotInfo.MoNo);
                IBinCodeChangeRule binChgRule = null;
                if (lotInfo.MoNo.StartsWith("T05TM") || lotInfo.MoNo.StartsWith("T05TE") || lotInfo.MoNo.StartsWith("T05TQ"))
                {
                    logger.Info("Change bin code rule for [T05TM, T05TE, T05TQ]");
                    binChgRule = BinCodeRuleFactory.GetBinChangeRule(lotInfo.MoNo, lotInfo.ProductNo, lotInfo.LotNo);
                }
                else if (lotInfo.MoNo.StartsWith("T05TR"))//T05TR
                {
                    logger.Info("Change bin code rule for [T05TR]");
                    binChgRule = BinCodeRuleFactory.GetBinChangeForTrRule(lotInfo.MoNo, lotInfo.ProductNo, lotInfo.LotNo);
                }

                logger.Debug("Starting change bin code.");
                binChgRule.ChangeBinCode(probResult.MapData);
                logger.Debug("bin code change completed!");

                logger.Info("Update Temp ComponentReSort End status");
                MesProdDbProxy.UpdateTempComponentReSortEndStatus(1, "", DateTime.Now, lotNo, componentNo, lotSerial);

                if (probResult.Recipe_Name.ToUpper().Contains("PULSE"))
                {
                    probResult.MapData.Columns.Remove("VF6");
                    probResult.MapData.Columns.Remove("VF7");
                    probResult.MapData.Columns.Remove("VF8");
                    probResult.MapData.Columns.Remove("VF9");
                    probResult.MapData.Columns.Remove("VF10");
                    probResult.MapData.Columns.Remove("VF11");
                    probResult.MapData.Columns.Remove("VF12");
                    probResult.MapData.Columns.Remove("VF13");
                    probResult.MapData.Columns.Remove("VF14");

                }

                logger.Info("Export EQ csv file.");
                ExportEqCsvFile(sortBinConfig.EqPath, probResult, lotInfo.MoNo, lotInfo.LotNo);

                

                //logger.Info("Delete tbl_LEDWIPCompTestSummary");
                //MesProdDbProxy.DeleteLedWipCompTestSummary(lotNo, componentNo, TEST_TYPE_BIN_COLLECTION, probResult.TestTime);

                //MesProdDbProxy.InsertLedWipCompTestSummary(probResult, TEST_TYPE_BIN_COLLECTION, lotInfo.MoNo);

                logger.Debug("Summary Map data for insert db.");
                List<LedWipCompTestBinSummary> compTestBinSumList = GetCompTestBinSummaryData(probResult, lotInfo.LotNo, binChgRule.GetBinCodeChangeMap());

                LedWipCompTestSummary compTestSum = ConverToLedWipComTestSummary(lotInfo, probResult);

                logger.Info("Insert WipCompTestSummary and WipCompTestBinSummary table");
                MesProdDbProxy.InsertWipCompTestSummayData(probResult, compTestSum, compTestBinSumList);

                //string destFullFilePath = Path.Combine(sortBinConfig.DestinationPath, dirNameByDay, fileInfo.Name);


                //FileSystemUtil.CreateFolderIfNoExists(Path.Combine(sortBinConfig.DestinationPath, dirNameByDay));

                //logger.Info("{0} Copy to Destination : {1}", fileInfo.Name, destFullFilePath);
                //MoveFileToFolder(sortBinConfig.DestinationPath, dirNameByDay, fileInfo);
                //fileInfo.CopyTo(destFullFilePath, true);
                string destFolder = Path.Combine(sortBinConfig.DestinationPath, dirNameByDay);
                logger.Info("{0} move to Destination : {1}", fileInfo.Name, destFolder);
                FileSystemUtil.MoveFile(destFolder, fileInfo);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "ImportByComponent error.");
                string errMsg = "ImportByComponent error." + ex.ToString();
                string failFolder = Path.Combine(sortBinConfig.FailPath, dirNameByDay);
                logger.Info("{0} move to fail path : {1}", fileInfo.Name, failFolder);
                //MoveFileToFolder(sortBinConfig.FailPath, dirNameByDay, fileInfo);
                FileSystemUtil.MoveFile(failFolder, fileInfo);
                logger.Info("Send error mail.");
                SendErrorMail(fileInfo, errMsg);

                logger.Info("Return : fail");
                return "fail";
            }


            logger.Info("Return : success");
            return "success";
        }

        private LedWipCompTestSummary ConverToLedWipComTestSummary(LotState lotInfo, ProberTestResult probResult)
        {
            LedWipCompTestSummary compTestSum = new LedWipCompTestSummary();
            compTestSum.LotNo = lotInfo.LotNo;
            compTestSum.ComponentNo = probResult.ComponentNo;
            compTestSum.TestType = TEST_TYPE_BIN_COLLECTION;
            compTestSum.Active = 1;
            compTestSum.FileID = probResult.FileName;
            compTestSum.TestTime = probResult.TestTime;
            compTestSum.MoNo = lotInfo.MoNo;
            compTestSum.EquipmentNo = probResult.EQP_ID;
            compTestSum.TestQty = probResult.QTY;
            compTestSum.GoodQty = probResult.QTY;
            //compTestSum.TotalYield = 0;
            compTestSum.BinQty = 0;
            compTestSum.StartTime = probResult.TestTime;
            compTestSum.EndTime = DateTime.Now;

            return compTestSum;
        }
        private void SendErrorMail(FileInfo errFile, string errMsg)
        {
            try
            {
                string mailSubj = "MES 套 Bin loader (ImportWhiteChipSortBin) error. File : " + errFile.Name;
                List<String> mailToList = new List<string>();
                string[] notifyMails = notifyMail.Split(';');
                foreach (string notiMail in notifyMails)
                {
                    if(notifyMail.Contains("@"))
                        mailToList.Add(notifyMail);
                }

                if (notifyMails.Length > 0)
                {
                    logger.Debug("Mail to : {0}", string.Join(", ", mailToList));
                    SendMailUtil.SendMail(mailToList.ToArray(), mailSubj, errMsg, new string[] { errFile.FullName });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "send mail error.");
                
            }
        }

        private List<LedWipCompTestBinSummary> GetCompTestBinSummaryData(ProberTestResult probResult,string lotNo,  Dictionary<string, string> binCodeChangeMap)
        {
            Dictionary<string, LedWipCompTestBinSummary> binTestSumMap = new Dictionary<string, LedWipCompTestBinSummary>();
            
            LedWipCompTestBinSummary testBinSum = null;
            string binNo;
            foreach (DataRow row in probResult.MapData.Rows)
            {
                binNo = row.Field<string>("Bin");
                if (binTestSumMap.ContainsKey(binNo) == false)
                {
                    testBinSum = new LedWipCompTestBinSummary();
                    testBinSum.LotNo = lotNo;
                    testBinSum.ComponentNo = probResult.ComponentNo;
                    testBinSum.BinNo = binNo;
                    testBinSum.BinQty = 1;
                    testBinSum.BinMinQty = 100;
                    testBinSum.TapeMinQty = 100;
                    testBinSum.BinGrade = 0;
                    testBinSum.BinCode = row.Field<string>("BIN_CODE");
                    testBinSum.BinFlag = 1;
                    testBinSum.MaxBinFlag = 1;
                    testBinSum.Percentage = 1;
                    testBinSum.TestTime = probResult.TestTime;
                    testBinSum.TestType = TEST_TYPE_BIN_COLLECTION;
                    //aRow("PMSpec") = "Y"
                    if (binCodeChangeMap.ContainsKey(testBinSum.BinCode))
                    {
                        testBinSum.InventoryNo = "F/G";
                    }
                    else
                    {
                        testBinSum.InventoryNo = "N/A";
                    }
                    binTestSumMap.Add(binNo, testBinSum);
                }
                else
                {
                    binTestSumMap[binNo].BinQty++;

                }
            } //foreach (DataRow row in probResult.MapData.Rows)

            return binTestSumMap.Values.ToList();
        }
        
        //private void MoveFileToFolder(string rootPath, string dirNameByDay, FileInfo fileInfo)
        //{
        //    string fullPath = Path.Combine(rootPath, dirNameByDay);
        //    FileSystemUtil.CreateFolderIfNoExists(fullPath);
        //    string fullFilePath = Path.Combine(fullPath, fileInfo.Name);
        //    logger.Debug("Delete : {0}", fullFilePath);
        //    File.Delete(fullFilePath);
        //    logger.Debug("{0} to {1}", fileInfo.FullName, fullFilePath);
        //    fileInfo.MoveTo(fullFilePath);
        //}
        

        private void ExportEqCsvFile(string eqFolder, ProberTestResult probTest, string moNo, string lotNo)
        {
            string fileName = probTest.ComponentNo + ".csv";
            string eqCsvFileName = Path.Combine(eqFolder, probTest.ComponentNo + ".csv");
            
            logger.Debug("Write file : {0}", eqCsvFileName);
            StringBuilder content = new StringBuilder();
            content.AppendLine(probTest.ToCsvHeader(fileName, moNo, lotNo));
            content.AppendLine("");
            content.AppendLine(probTest.ToCsvSummary());
            content.AppendLine("");
            content.AppendLine("map data");
            content.Append(probTest.ToCsvMapData());

            FileSystemUtil.WriteTxtFile(eqCsvFileName, content.ToString().Trim(), true);

        }

        //private void ChangeBinCodeAndBinNo(Dictionary<string, string> binNoChangeMap, ProberTestResult probResult)
        //{
        //    string binCode;
        //    foreach (DataRow binRow in probResult.MapData.Rows)
        //    {
        //        binCode = binRow.Field<string>("BIN_CODE");
        //        if (binCode.ToUpper().StartsWith("SIDE"))
        //            binRow["Bin"] = "99";
        //        else if (binCode.ToUpper().StartsWith("NOTG"))
        //            binRow["Bin"] = "100";                
        //        else
        //        {

        //            if (binCodeQtyMap.ContainsKey(binCode))
        //            {
        //                if (binCodeQtyMap[binCode] < 5)
        //                {
        //                    binRow["Bin"] = "98";
        //                    binRow["BIN_CODE"] = "ReSorter";
        //                }
        //            }
        //            else if (binNoChangeMap.ContainsKey(binCode))
        //            {
        //                binRow["Bin"] = binNoChangeMap[binCode];
        //            }
        //            else
        //            {
        //                //不存在 bin 表裡的全轉成 98 , ReSorter
        //                binRow["Bin"] = "98";
        //                binRow["BIN_CODE"] = "ReSorter";
        //            }

        //        }
        //    }
        //}

        //private Dictionary<string, string> GetBinNoChangeMap(LotState lotInfo)
        //{
        //    DataTable binChangeTb = null;
        //    logger.Info("Query Bin change define table");
        //    if (lotInfo.MoNo.StartsWith("T05TE"))
        //    {
        //        logger.Debug("QueryBinChagneDefineData by [{0}]", lotInfo.MoNo.Substring(0, 10));
        //        binChangeTb = MesProdDbProxy.QueryBinChagneDefineData(lotInfo.MoNo.Substring(0, 10));
        //    }
        //    else
        //    {
        //        logger.Debug("QueryBinChagneDefineData by [{0}]", lotInfo.ProductNo);
        //        binChangeTb = MesProdDbProxy.QueryBinChagneDefineData(lotInfo.ProductNo);
        //    }

        //    logger.Debug("Bin change table count : {0}", binChangeTb.Rows.Count);

        //    return binChangeTb.AsEnumerable()
        //            .ToDictionary<DataRow, string, string>(row => row.Field<string>("BINCODE"),
        //                        row => row.Field<string>("BINNO"));
        //}
    }
}
