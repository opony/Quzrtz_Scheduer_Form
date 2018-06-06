using MesAutoLoaderLib.Exceptions;
using MesAutoLoaderLib.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Parsers
{
    public class ProberCsvParser
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly string[] HEADER_SPLIC_FIX = new string[] { ",," };
        private static readonly string[] MAP_DATA_SPLIC_FIX = new string[] { "," };
        enum ParseStep
        {
            Header,
            Summary,
            MapData
        }

        ParseStep currStep = ParseStep.Header;
        ProberTestResult probResult = new ProberTestResult();

        FileInfo csvFile;
        string fileContent;
        public ProberCsvParser(FileInfo csvFile)
        {
            this.csvFile = csvFile;
        }

        public ProberTestResult Parse()
        {
            fileContent = File.ReadAllText(this.csvFile.FullName);

            string errMsg;
            if (VerifyFileFormat(out errMsg) == false)
            {
                throw new FileFormatException(csvFile.Name + " format is wrong. " + errMsg);
            }

            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;
                    StepChange(line);
                    switch (currStep)
                    {
                        case ParseStep.Header:
                            ParseHeader(line);
                            break;
                        case ParseStep.Summary:
                            ParseSummaryData(line);
                            break;

                        case ParseStep.MapData:
                            ParseMapData(line);
                            break;

                    }
                }
            }

            return probResult;
        }

        private void ParseMapData(string line)
        {
            string[] tokens = line.Split(MAP_DATA_SPLIC_FIX, StringSplitOptions.None);
            if (line.StartsWith("PosX"))
            {
                probResult.MapData = new DataTable();
                foreach (string col in tokens)
                {
                    probResult.MapData.Columns.Add(col);
                }
                return;
            }

            if (probResult.MapData == null)
                throw new Exception("Can't found the map data [column], line : [" + line + "]");


            DataRow row = probResult.MapData.NewRow();
            for (int i=0; i< tokens.Length; i++)
            {
                //欄位內容有可能是空白的
                if (string.IsNullOrEmpty(tokens[i].Trim()))
                    tokens[i] = "0";

            }
            row.ItemArray = tokens;
            probResult.MapData.Rows.Add(row);
        }

        private void ParseSummaryData(string line)
        {
            if (line.StartsWith("map data"))
                return;

            string[] tokens = line.Split(MAP_DATA_SPLIC_FIX, StringSplitOptions.None);
            //DataTable add column
            if (line.StartsWith("SUMITEM"))
            {
                probResult.SummaryData = new DataTable();
                foreach (string col in tokens)
                {
                    probResult.SummaryData.Columns.Add(col.Trim());
                }

                return;
            }

            if (probResult.SummaryData == null)
                throw new Exception("Can't found the summary data [column], line : [" + line + "]");

            DataRow row = probResult.SummaryData.NewRow();
            row.ItemArray = tokens;
            probResult.SummaryData.Rows.Add(row);

        }



        public void ParseHeader(string line)
        {

            try
            {
                string[] tokens = line.Split(HEADER_SPLIC_FIX, StringSplitOptions.None);
                if (tokens.Length < 2)
                    throw new Exception("Header format is wrong. line [" + line + "]");

                string key = tokens[0].Trim();
                string value = tokens[1].Trim();

                if (key == "FileID")
                    probResult.FileID = value;
                else if (key == "TestTime")
                {
                    //2018/03/02 17:16
                    value = value + ":00";
                    probResult.TestTime = DateTime.Parse(value);
                }
                else if (key == "WO")
                {
                    probResult.WO = value;
                }
                else if (key == "Operator")
                {
                    probResult.Operator = value;
                }
                else if (key == "MaximumBin")
                {
                    probResult.MaximumBin = int.Parse(value);
                }
                else if (key == "Specification")
                {
                    probResult.Specification = value;
                }
                else if (key == "SpecRemark")
                {
                    probResult.SpecRemark = value;
                }
                else if (key == "FileName")
                {
                    probResult.FileName = value;
                }
                else if (key == "ProductName")
                {
                    probResult.ProductName = value;
                }
                else if (key == "DeviceNumber")
                {
                    probResult.DeviceNumber = value;
                }
                else if (key == "LOT_ID")
                {
                    probResult.LOT_ID = value;
                }
                else if (key == "ComponentNo")
                {
                    probResult.ComponentNo = value;
                }
                else if (key == "Customer")
                {
                    probResult.Customer = value;
                }
                else if (key == "Class")
                {
                    probResult.Class = value;
                }
                else if (key == "OrderNumber")
                {
                    probResult.OrderNumber = value;
                }
                else if (key == "Temperature")
                {
                    probResult.Temperature = value;
                }
                else if (key == "Humidity")
                {
                    probResult.Humidity = value;
                }
                else if (key == "Remark1")
                {
                    probResult.Remark1 = value;
                }
                else if (key == "Remark2")
                {
                    probResult.Remark2 = value;
                }
                else if (key == "Remark3")
                {
                    probResult.Remark3 = value;
                }
                else if (key == "Remark4")
                {
                    probResult.Remark4 = value;
                }
                else if (key == "EQP_ID")
                {
                    probResult.EQP_ID = value;
                }
                else if (key == "Recipe_Name")
                {
                    probResult.Recipe_Name = value;
                }
                else if (key == "QTY")
                {
                    
                    probResult.QTY = int.Parse(value);
                }
                else if (key == "SideBin")
                {
                    probResult.SideBin = int.Parse(value);
                }
                else if (key == "NGBin")
                {
                    probResult.NGBin = int.Parse(value);
                }
                else if (key == "ResortBin")
                {
                    probResult.ResortBin = int.Parse(value);
                }
                else if (key == "TryLot")
                {
                    probResult.TryLot = value;
                }
                else if (key == "CORREL1")
                {
                    probResult.CORREL1 = int.Parse(value);
                }
                else if (key == "CORREL2")
                {
                    probResult.CORREL2 = int.Parse(value);
                }
                else if (key == "CORREL2")
                {
                    probResult.CORREL2 = int.Parse(value);
                }



            }
            catch (Exception ex)
            {
                logger.Error(ex, "Parse Header Error.");
                throw new FileFormatException("Parse Header 錯誤. 錯誤 line : [" + line + "] ", ex);
            }
        }

        private void StepChange(string line)
        {
            if (line.StartsWith("SUMITEM"))
                currStep = ParseStep.Summary;
            else if (line.StartsWith("PosX"))
                currStep = ParseStep.MapData;
        }
        private bool VerifyFileFormat(out string errMsg)
        {
            errMsg = "";
            using (StringReader reader = new StringReader(fileContent))
            {
                bool foundMapDataHeader = false;
                string headCol = "";
                string data = "";
                int headCnt = 0;
                int dataCnt = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    
                    if (line.StartsWith("PosX"))
                    {
                        foundMapDataHeader = true;
                        headCol = line;
                        continue;
                    }
                    data = line;
                    if (foundMapDataHeader)
                    {
                        headCnt = headCol.Split(MAP_DATA_SPLIC_FIX, StringSplitOptions.None).Length;
                        dataCnt = data.Split(MAP_DATA_SPLIC_FIX, StringSplitOptions.None).Length;
                        if (headCnt != dataCnt)
                        {
                            errMsg = "Map data column and data count no match.";
                            return false;
                        }
                    }

                }

                if (foundMapDataHeader == false)
                {
                    errMsg = "Can't found the [map data].";
                    return false;
                }
            }

            return true;
        }
    }
}
