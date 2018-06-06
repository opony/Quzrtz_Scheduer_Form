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
    class ClassBinParser
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly string[] HEADER_SPLIC_FIX = new string[] { "," };
        private static readonly string[] MAP_DATA_SPLIC_FIX = new string[] { "," };

        enum ParseStep
        {
            Header,
            Summary
        }

        ParseStep currStep = ParseStep.Header;
        ClassBinInfo classBinInfo = new ClassBinInfo();

        FileInfo csvFile;
        string fileContent;

        public ClassBinParser(FileInfo csvFile)
        {
            this.csvFile = csvFile;
        }

        public ClassBinInfo Parse()
        {
            fileContent = File.ReadAllText(this.csvFile.FullName);
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
                            

                    }
                }
            }

            return classBinInfo;
        }

        private void ParseSummaryData(string line)
        {
            try
            {
                string[] tokens = line.Split(MAP_DATA_SPLIC_FIX, StringSplitOptions.None);
                if (line.StartsWith("PosX"))
                {
                    classBinInfo.SummaryTb = new DataTable();
                    foreach (string col in tokens)
                    {
                        classBinInfo.SummaryTb.Columns.Add(col.Trim());
                    }

                    return;
                }

                if (classBinInfo.SummaryTb == null)
                    throw new Exception("Can't found the summary data [column], line : [" + line + "]");


                DataRow row = classBinInfo.SummaryTb.NewRow();
                row.ItemArray = tokens;
                classBinInfo.SummaryTb.Rows.Add(row);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "ParseSummaryData error.");
                throw ex;
            }
        }

        private void ParseHeader(string line)
        {
            try
            {
                string[] tokens = line.Split(HEADER_SPLIC_FIX, StringSplitOptions.None);
                if (tokens.Length != 2)
                    throw new Exception("Header format is wrong. line [" + line + "]");

                string key = tokens[0].Trim().ToUpper();
                string value = tokens[1].Trim();

                if (key == "FILEID")
                    classBinInfo.FileID = value;
                else if (key == "TESTTIME")
                {
                    classBinInfo.TestTime = DateTime.Parse(value);
                }
                else if (key == "WO")
                {
                    classBinInfo.WO = value;
                }
                else if (key == "LOT_ID")
                {
                    classBinInfo.LOT_ID = value;
                }
                else if (key == "WAFER_ID")
                {
                    classBinInfo.WAFER_ID = value;
                }
                else if (key == "SUBSTRATE_ID")
                {
                    classBinInfo.SUBSTRATE_ID = value;
                }
                else if (key == "CST_ID")
                {
                    classBinInfo.CST_ID = value;
                }
                else if (key == "RECIPE_ID")
                {
                    classBinInfo.Recipe_ID = value;
                }
                else if (key == "RECIPE_NAME")
                {
                    classBinInfo.Recipe_Name = value;
                }
                else if (key == "OPERATOR")
                {
                    classBinInfo.OPERATOR = value;
                }
                else if (key == "TAPEID")
                {
                    classBinInfo.TapeID = value;
                }
                else if (key == "FRAMEID")
                {
                    classBinInfo.FrameID = value;
                }
                else if (key == "BIN_CODE")
                {
                    classBinInfo.Bin_CODE = value;
                }
                else if (key == "SORTERID")
                {
                    classBinInfo.SorterID = value;
                }
                else if (key == "STARTTIME")
                {
                    classBinInfo.StartTime = DateTime.Parse(value);
                }
                else if (key == "ENDTIME")
                {
                    classBinInfo.EndTime = DateTime.Parse(value);
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex, "ParseHeader error.");
                throw ex;
            }
        }
        private void StepChange(string line)
        {
            if (line.StartsWith("PosX"))
                currStep = ParseStep.Summary;
        }
    }
}
