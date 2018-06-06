using MesAutoLoaderLib.Exceptions;
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
using System.Text;

namespace MesAutoLoaderLib.Jobs.Loader
{
    /// <summary>
    /// Prober loader
    /// </summary>
    [DisallowConcurrentExecutionAttribute]
    public class ImportBTestForWhiteChipJob : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly int KB_2 = 2048;
        private static readonly int TEST_TYPE_PROBER = 2;
        private static readonly int ADJUSTING_GLUE_FINISH = 2;//完成調膠
        ProberConfig proberConfig = null;
        string param;
        string notifyMail;
        string dirDayName = "";

        public void Execute(IJobExecutionContext context)
        {
            logger.Info("===== start =====");
            //string dirNameByDay = ""; testTime.ToString("yyyyMM");
            notifyMail = Convert.ToString(context.JobDetail.JobDataMap["Notify_Mail"]);
            param = Convert.ToString(context.JobDetail.JobDataMap["Param"]);

            try
            {
                logger.Info("Load Job Param.");
                proberConfig = new ProberConfig();
                proberConfig.Load(param);
                logger.Info("Search csv file form : {0}", proberConfig.SourcePath);
                FileInfo[] fileInfos = new DirectoryInfo(proberConfig.SourcePath).GetFiles("*.csv", SearchOption.TopDirectoryOnly);
                if (fileInfos.Length == 0)
                {
                    logger.Info("No found any csv file. then end the job.");
                    logger.Info("===== end =====");
                    return;
                }

                //logger.Info("Move files from {0} to {1}", proberConfig.SourcePath, proberConfig.QueuePath);
                //MoveToFolder(proberConfig.QueuePath, fileInfos);

                
                foreach (FileInfo csvFileInfo in fileInfos)
                {
                    
                    logger.Info("{0} move to {1}", csvFileInfo.FullName, proberConfig.QueuePath);
                    FileSystemUtil.MoveFile(proberConfig.QueuePath, csvFileInfo);
                    logger.Info("Start import : {0}", csvFileInfo.Name);
                    ImportFile(csvFileInfo);
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex, "ImportBTestForWhiteChipJob error.");
                throw ex;
            }
            logger.Info("===== end =====");
        }

        private string ImportFile(FileInfo csvFileInfo)
        {
            
            try
            {
                dirDayName = DateTime.Now.ToString("yyyyMM");
                logger.Info("##########################");
                logger.Info("Parse file : {0}", csvFileInfo.FullName);

                ProberCsvParser parser = new ProberCsvParser(csvFileInfo);
                ProberTestResult probResult = null;
                try
                {
                    probResult = parser.Parse();
                }
                catch (FileFormatException ex)
                {
                    logger.Warn("Csv File format is wrong!.");
                    SendCsvWrongFormatAlarmMail(csvFileInfo, ex);
                    logger.Warn("Return fail.");
                    return "fail.";
                }
                
                dirDayName = probResult.TestTime.ToString("yyyyMM");
                logger.Info("SubFolder : {0}", dirDayName);
                logger.Info("Parse completed~!");
                string errMsg = "";
                if (VerifyFileContent(probResult, out errMsg) == false)
                {
                    throw new Exception(errMsg);
                }

                logger.Info("Query Lot Info by componentNo : {0}", probResult.ComponentNo);
                LotState lotInfo = MesProdDbProxy.QueryLotInofByComponentNo(probResult.ComponentNo);
                if (lotInfo == null)
                {
                    logger.Warn("無法取得歸屬的生產批號，此生產批必須在WIP!! component no : {0}", probResult.ComponentNo);

                    string failFullFolder = Path.Combine(proberConfig.FailPath, dirDayName);
                    logger.Info("{0} move to {1}", csvFileInfo.FullName, failFullFolder);
                    FileSystemUtil.MoveFile(failFullFolder, csvFileInfo);
                    logger.Info("Return fail");
                    return "fail";
                }

                logger.Info("Check lot is pilot? , Query pkgmdp.DSP_TASK by lot : {0}", lotInfo.LotNo);

                //status '0:待調膠 1:調膠中 2:調膠完成 -1:放棄調膠 -2:真的刪除 -9:試比例刪除';
                //Run_sequence  '0:放量 1:試1 2:試2 3:試3 4:試4';
                //Status >= 0 表示有在做「試程式」，status 必需要是 2 (調膠完成)。
                //query 列印的調膠單
                DataTable piLotTb = MesProdDbProxy.QueryPiLotDataByLot(lotInfo.LotNo);
                if (piLotTb.Rows.Count > 0)
                {
                    string[] rids = (from row in piLotTb.AsEnumerable()
                                     select row.Field<string>("RID")).ToArray();

                    DataTable pgmTb = MesProdDbProxy.QueryPGMTaskDataByRunID(rids);

                    //有印調膠單，卻沒有調膠記錄
                    if (pgmTb.Rows.Count == 0)
                    {
                        SendNoFinishAdjustingGlueMail(lotInfo.LotNo, rids, csvFileInfo.FullName);

                        logger.Info("File move to Fail folder. {0}", proberConfig.FailPath);
                        string failFullFolder = Path.Combine(proberConfig.FailPath, dirDayName);
                        logger.Info("{0} move to {1}", csvFileInfo.FullName, failFullFolder);
                        FileSystemUtil.MoveFile(failFullFolder, csvFileInfo);

                        logger.Info("Return fail");
                        return "fail";
                    }

                    var selPgmRows = (from row in pgmTb.AsEnumerable()
                                      where Convert.ToInt32(row["STATUS"]) == ADJUSTING_GLUE_FINISH
                                      select row).ToArray();
                    
                    //如有完成調膠
                    if (selPgmRows.Length > 0)
                    {
                        logger.Info("Export PiLotRun csv file to {0}", proberConfig.PiLotPath);
                        ExportPiLotRunCsvFile(lotInfo, probResult);
                    }
                    else
                    {
                        SendNoFinishAdjustingGlueMail(lotInfo.LotNo, rids, csvFileInfo.FullName);

                        logger.Info("File move to Fail folder. {0}", proberConfig.FailPath);
                        string failFullFolder = Path.Combine(proberConfig.FailPath, dirDayName);
                        logger.Info("{0} move to {1}", csvFileInfo.FullName, failFullFolder);
                        FileSystemUtil.MoveFile(failFullFolder, csvFileInfo);

                        logger.Info("Return fail");
                        return "fail";
                    }
                }


                logger.Info("Delete LedWipCompTestSummary");
                MesProdDbProxy.DeleteLedWipCompTestSummary(lotInfo.LotNo, probResult.ComponentNo, TEST_TYPE_PROBER, probResult.TestTime);

                logger.Info("update exists componentNo to non active = 0 by ComponentNo: {0}", probResult.ComponentNo);
                MesProdDbProxy.UpdateTbl_LedWipCompTestSummaryActiveStatus(probResult.ComponentNo, TEST_TYPE_PROBER, 0);

                //Query Bin 表
                logger.Info("Get Bin Fix table");
                Dictionary<string, string> binCodeChgMap = BinCodeRuleFactory.GetBinCodeChangeMap(lotInfo.MoNo, lotInfo.ProductNo);
                if (binCodeChgMap.Count <= 0)
                {
                    logger.Warn("ProductNo {0} no define bin fix table", lotInfo.ProductNo);
                    
                    StringBuilder mailBody = new StringBuilder();
                    mailBody.AppendLine("請確認料號[" + lotInfo.ProductNo + "] 是否有設定 Bin 表。<br>")
                        .AppendLine("Lot ID : " + lotInfo.LotNo);

                    logger.Warn("Send alarm mail to MAIL_MES_IT");
                    DataTable userDataTb = MesProdDbProxy.QueryUserBasisByGroupNo("MAIL_MES_IT");
                    string[] mailTos = (from row in userDataTb.AsEnumerable()
                                        select row.Field<string>("EMAILADDRESS")).ToArray();

                    string mailSubj = lotInfo.LotNo + " Prober 下機，" + lotInfo.ProductNo + " 未設定 Bin 表 error, File : " + csvFileInfo.Name;
                    SendMailUtil.SendMail(mailTos, mailSubj, mailBody.ToString(), new string[] { csvFileInfo.FullName });

                    logger.Warn("Send alarm mail to MAIL_PKG2_MFG, NPF01@lextar.com, NPF02@lextar.com");
                    userDataTb = MesProdDbProxy.QueryUserBasisByGroupNo("MAIL_PKG2_MFG");
                    mailTos = (from row in userDataTb.AsEnumerable()
                               select row.Field<string>("EMAILADDRESS")).ToArray();
                    List<string> mailToList = new List<string>();
                    mailToList.AddRange(mailTos);
                    mailToList.Add("NPF01@lextar.com");
                    mailToList.Add("NPF02@lextar.com");
                    SendMailUtil.SendMail(mailToList.ToArray(), mailSubj, mailBody.ToString(), new string[] { csvFileInfo.FullName });

                    logger.Info("File move to Fail folder. {0}", proberConfig.FailPath);
                    string failFullFolder = Path.Combine(proberConfig.FailPath, dirDayName);
                    logger.Info("{0} move to {1}", csvFileInfo.FullName, failFullFolder);
                    FileSystemUtil.MoveFile(failFullFolder, csvFileInfo);

                    logger.Info("Return fail");
                    return "fail";
                }

                logger.Debug("Summary Map data for insert db.");
                List<LedWipCompTestBinSummary> compTestBinSumList = GetCompTestBinSummaryData(probResult, lotInfo.LotNo, binCodeChgMap);

                logger.Info("Update WipComponentState GoodQty = : {0}", probResult.MapData.Rows.Count);
                MesProdDbProxy.UpdateCompomentStateGoodQty(lotInfo.LotNo, probResult.ComponentNo, probResult.MapData.Rows.Count);

                //更新報癈數量
                MesProdDbProxy.UpdateWipTemp_ComponentAttribScrapQty(lotInfo.LotNo, probResult.ComponentNo, probResult.MapData.Rows.Count);

                //更新Good 數量
                MesProdDbProxy.UpdateWipTemp_ComponentAttrib(lotInfo.LotNo, probResult.ComponentNo, "CGoodQty", probResult.MapData.Rows.Count.ToString());

                LedWipCompTestSummary compTestSum = ConverToLedWipComTestSummary(lotInfo, probResult);
                MesProdDbProxy.UpdateWipTemp_ComponentAttrib(compTestSum.LotNo, probResult.ComponentNo, "CYield", compTestSum.TotalYield.ToString() + "%");
                MesProdDbProxy.UpdateWipCont_ComponentAttrib(compTestSum.LotNo, lotInfo.LotSerial, probResult.ComponentNo, "CYield", compTestSum.TotalYield.ToString() + "%");

                logger.Info("Insert WipCompTestSummary and WipCompTestBinSummary table");
                MesProdDbProxy.InsertWipCompTestSummayData(probResult, compTestSum, compTestBinSumList);

                logger.Info("Inset RTP sorter Raw Data table");
                InsertRptSorterRawDataTable(probResult, lotInfo.MoNo, lotInfo.LotNo);

                string fullDestFolder = Path.Combine(proberConfig.DestinationPath, dirDayName);
                logger.Info("Move file to destination path. {0}", proberConfig.DestinationPath);
                FileSystemUtil.MoveFile(fullDestFolder, csvFileInfo);



            }
            catch (Exception ex)
            {
                logger.Error(ex , "ImportFile Error.");
                string failFullFolder = Path.Combine(proberConfig.FailPath, dirDayName);
                logger.Info("{0} move to {1}", csvFileInfo.FullName, failFullFolder);
                FileSystemUtil.MoveFile(failFullFolder, csvFileInfo);

                if (csvFileInfo.Length < KB_2)
                {
                    logger.Warn("File size : {0} < 2KB , then the file is non critical alarm, don't send alarm mail.");
                    return "fail";
                }

                string[] mailTos = notifyMail.Split(';');

                
                SendMailUtil.SendMail(mailTos, "ImportBTestForWhiteChipJob error. File : " + csvFileInfo.Name, ex.ToString(), new string[] { csvFileInfo.FullName });
                
                throw ex;
            }
            logger.Info("Return success.");
            return "success";
        }

        private void SendBinCodeMapEmptyAlarmMail()
        {

        }

        private void SendNoFinishAdjustingGlueMail(string lotNo, string[] rids, string csvFileFullName)
        {
            logger.Warn("Lot status is not 2 (調膠完成)");
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendLine("請確認 [" + lotNo + "] 是否在調膠機完成調膠。<br>")
                .AppendLine("Run ID : " + string.Join(", ", rids));


            logger.Warn("Send alarm mail to MAIL_MES_IT");
            DataTable userDataTb = MesProdDbProxy.QueryUserBasisByGroupNo("MAIL_MES_IT");
            string[] mailTos = (from row in userDataTb.AsEnumerable()
                                select row.Field<string>("EMAILADDRESS")).ToArray();
            string fileName = Path.GetFileName(csvFileFullName);
            SendMailUtil.SendMail(mailTos, lotNo + " Prober 下機，調謬未完成 error, File : " + fileName, mailBody.ToString(), new string[] { csvFileFullName });

            logger.Warn("Send alarm mail to MAIL_PKG2_MFG");
            userDataTb = MesProdDbProxy.QueryUserBasisByGroupNo("MAIL_PKG2_MFG");
            mailTos = (from row in userDataTb.AsEnumerable()
                       select row.Field<string>("EMAILADDRESS")).ToArray();

            SendMailUtil.SendMail(mailTos, lotNo + " Prober 下機，調謬未完成 error, File : " + fileName, mailBody.ToString(), new string[] { csvFileFullName });

            

            logger.Info("Return fail");
        }

        private void SendCsvWrongFormatAlarmMail(FileInfo csvFileInfo, Exception ex)
        {
            

            if (csvFileInfo.Length < KB_2)
            {
                logger.Warn("File size : {0} < 2KB , then the file is non critical alarm, don't send alarm mail.");
            }
            else
            {
                string[] mailTos = notifyMail.Split(';');

                string mailSubj = "Prober 下機 csv 檔案格式錯誤 : " + csvFileInfo.Name;
                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendLine("請確認 Prober 測試程式設定是否正確 <br>")
                    .AppendLine(ex.ToString());

                SendMailUtil.SendMail(mailTos, mailSubj, mailBody.ToString(), new string[] { csvFileInfo.FullName });

                logger.Warn("Send alarm mail to MAIL_PKG2_MFG, NPF01@lextar.com, NPF02@lextar.com");
                DataTable userDataTb = MesProdDbProxy.QueryUserBasisByGroupNo("MAIL_PKG2_MFG");
                mailTos = (from row in userDataTb.AsEnumerable()
                           select row.Field<string>("EMAILADDRESS")).ToArray();
                List<string> mailToList = new List<string>();
                mailToList.AddRange(mailTos);
                mailToList.Add("NPF01@lextar.com");
                mailToList.Add("NPF02@lextar.com");


                SendMailUtil.SendMail(mailToList.ToArray(), mailSubj, mailBody.ToString(), new string[] { csvFileInfo.FullName });
            }

            string failFullFolder = Path.Combine(proberConfig.FailPath, dirDayName);
            logger.Info("{0} move to {1}", csvFileInfo.FullName, failFullFolder);
            FileSystemUtil.MoveFile(failFullFolder, csvFileInfo);

        }

        private LedWipCompTestSummary ConverToLedWipComTestSummary(LotState lotInfo, ProberTestResult probResult)
        {
            LedWipCompTestSummary compTestSum = new LedWipCompTestSummary();
            compTestSum.LotNo = lotInfo.LotNo;
            compTestSum.ComponentNo = probResult.ComponentNo;
            compTestSum.TestType = TEST_TYPE_PROBER;
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

        private void InsertRptSorterRawDataTable(ProberTestResult probResult, string moNo, string lotNo)
        {
            DateTime nowTime = DateTime.Now;
            DataTable emptyRawHeaderTb = MesHistDbProxy.QueryMesRptSorterRawHeaderEmptyTable();
            Dictionary<string, object> rawHeaderDataMap = GetRawHeaderMap(probResult, emptyRawHeaderTb, nowTime, moNo, lotNo);


            DataTable emptyRawTb = MesHistDbProxy.QueryRptSorterRawEmptyTable();
            List<Dictionary<string, object>> rawDataList = GetRawDataMap(probResult, emptyRawTb, nowTime);

            MesHistDbProxy.insertRptSorterRawData(rawHeaderDataMap, rawDataList);

        }

        private List<Dictionary<string, object>> GetRawDataMap(ProberTestResult probResult, DataTable emptyRawTb, DateTime recTime)
        {
            Dictionary<string, Type> rowTbColumnTypeMap = (from dc in emptyRawTb.Columns.Cast<DataColumn>()
                                                                                   select dc).ToDictionary<DataColumn, string, Type>(row => row.ColumnName, row => row.DataType);


            List<Dictionary<string, object>> rawDataList = new List<Dictionary<string, object>>();
            Dictionary<string, object> rawDataMap = null;
            
            foreach (DataRow row in probResult.MapData.Rows)
            {
                rawDataMap = new Dictionary<string, object>();
                rawDataMap.Add("FILENAME", probResult.FileName.ToUpper());
                rawDataMap.Add("RECORD_TIME", recTime);

                foreach (DataColumn col in probResult.MapData.Columns)
                {
                    string colName = col.ColumnName.ToUpper();
                    colName = ChangeColumnName(colName);
                    if (rowTbColumnTypeMap.ContainsKey(colName) == false)
                        continue;

                    rawDataMap.Add(colName, Convert.ChangeType(row[col.ColumnName], rowTbColumnTypeMap[colName]));
                }

                rawDataList.Add(rawDataMap);
            }

            return rawDataList;
        }

        private Dictionary<string, object> GetRawHeaderMap(ProberTestResult probResult, DataTable emptyRawHeaderTb, DateTime recTime, string moNo, string lotNo)
        {
            Dictionary<string, object> rawHeaderMap = new Dictionary<string, object>();
            rawHeaderMap.Add("FILENAME", probResult.FileName.ToUpper());
            rawHeaderMap.Add("TESTTIME", probResult.TestTime);
            rawHeaderMap.Add("RECORD_TIME", recTime);
            
            rawHeaderMap.Add("WO", moNo);
            rawHeaderMap.Add("EQP_ID", probResult.EQP_ID);
            rawHeaderMap.Add("RECIPE_NAME", probResult.Recipe_Name);
            rawHeaderMap.Add("IS_VALID", "Y");
            rawHeaderMap.Add("LOT_ID", lotNo);
            rawHeaderMap.Add("QTY", probResult.QTY);
            rawHeaderMap.Add("SIDEBIN", probResult.SideBin);
            rawHeaderMap.Add("NGBIN", probResult.NGBin);
            rawHeaderMap.Add("RESORTBIN", probResult.ResortBin);
            rawHeaderMap.Add("TRYLOT", probResult.TryLot);

            //string[] rawDbColumns = (from dc in emptyRawHeaderTb.Columns.Cast<DataColumn>()
            //                         select dc.ColumnName).ToArray();

            Dictionary<string, Type> rowTbColumnTypeMap = (from dc in emptyRawHeaderTb.Columns.Cast<DataColumn>()
                                                           select dc).ToDictionary<DataColumn, string, Type>(row => row.ColumnName, row => row.DataType);


            string colName;
            string rawColName;
            string value;
            foreach (DataRow sumRow in probResult.SummaryData.Rows)
            {
                value = sumRow.Field<string>("SUMITEM");
                if (value != "MAX" && value != "MIN" && value != "AVG" && value != "STD")
                    continue;
                
                for (int colIdx =1; colIdx < probResult.SummaryData.Columns.Count; colIdx++ )
                {
                    colName = probResult.SummaryData.Columns[colIdx].ColumnName.ToUpper();
                    //colName = ChangeColumnName(colName);
                    rawColName = ChangeColumnName(colName) + "_" + value;
                    if (rowTbColumnTypeMap.Keys.Contains(rawColName))
                        rawHeaderMap.Add(rawColName, Convert.ChangeType(sumRow[colName], rowTbColumnTypeMap[rawColName]));
                        //rawHeaderMap.Add(rawColName, Convert.ToDouble(sumRow[colName]));
                }
            }

            
            
            return rawHeaderMap;
        }

        private string ChangeColumnName(string orgColName)
        {
            string col = orgColName.ToUpper();
            //If strtemp.Split(",")(j).Trim.ToUpper = "OPT-CIEXY1-X" Then
            //                        strtemp.Split(",")(j) = "SPEC1_X"
            //                    ElseIf strtemp.Split(",")(j).Trim.ToUpper = "OPT-CIEXY1-Y" Then
            //                        strtemp.Split(",")(j) = "SPEC1_Y"
            //                    ElseIf strtemp.Split(",")(j).Trim.ToUpper = "OPT-WLP1" Then
            //                        strtemp.Split(",")(j) = "SPEC1_WLP"
            //                    ElseIf strtemp.Split(",")(j).Trim.ToUpper = "OPT-FLUX1" Then
            //                        strtemp.Split(",")(j) = "LOP1"
            //                    End If

            
            if (col == "OPT-CIEXY1-X")
                return "SPEC1_X";

            if (col == "OPT-CIEXY1-Y")
                return "SPEC1_Y";

            if (col == "OPT-WLP1")
                return "SPEC1_WLP";

            if (col == "OPT-FLUX1")
                return "LOP1";

            return orgColName;
        }
        private void ExportPiLotRunCsvFile(LotState lotInfo, ProberTestResult probResult)
        {
            string fileName = lotInfo.MoNo + "_" + lotInfo.LotNo + "_" + probResult.TestTime.ToString("yyyyMMddHHmmss") + "_" + probResult.EQP_ID + "_" + lotInfo.ProductNo + ".csv";
            string fullFileName = Path.Combine(proberConfig.PiLotPath, fileName);
            logger.Info("Lot {0} is pilot , then export bin data to [{1}]", lotInfo.LotNo, fullFileName);

            StringBuilder content = new StringBuilder();
            content.Append(probResult.ToCsvMapData());
            FileSystemUtil.WriteTxtFile(fullFileName, content.ToString().Trim(), true);
            logger.Info("Export completed~!");
        }

        private List<LedWipCompTestBinSummary> GetCompTestBinSummaryData(ProberTestResult probResult, string lotNo, Dictionary<string, string> binCodeChangeMap)
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
                    testBinSum.TestType = TEST_TYPE_PROBER;
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


        private bool VerifyFileContent(ProberTestResult probResult, out string errMsg)
        {
            errMsg = "";
            if (string.IsNullOrEmpty(probResult.ComponentNo))
            {
                errMsg = "WAFER_ID 不可空白!!";
                return false;
            }

            if (string.IsNullOrEmpty(probResult.EQP_ID))
            {
                errMsg = "EQP_ID 不可空白!!";
                return false;
            }

            if (probResult.MapData.Rows.Count == 0)
            {
                errMsg = "map data is empty!!";
                return false;
            }
            
            return true;
        }

        //private void MoveToFolder(string newFolder, FileInfo fileInfo)
        //{
            
        //    string newFullPath = Path.Combine(newFolder, fileInfo.Name);
            
        //    File.Delete(newFullPath);
        //    logger.Debug("{0} move to {1}", fileInfo.FullName, newFullPath);
        //    fileInfo.MoveTo(newFullPath);
        //}
    }
}
