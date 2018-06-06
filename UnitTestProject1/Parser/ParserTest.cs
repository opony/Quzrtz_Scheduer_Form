using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;
using System.Text;
using MesAutoLoaderLib.Parsers;
using MesAutoLoaderLib.Model;
using MesAutoLoaderLib.Model.Config;

namespace UnitTestProject1.Parser
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void ProberParserTest()
        {
            FileInfo fileInfo = new FileInfo("Parser/T20P58300100002.csv");
            ProberCsvParser parser = new ProberCsvParser(fileInfo);
            ProberTestResult probResult = parser.Parse();
            Assert.AreEqual("T20P58300100002", probResult.LOT_ID, "LOT_ID compare fail.");
            Assert.AreEqual("T20P58300100002", probResult.ComponentNo, "ComponentNo compare fail.");
            Assert.AreEqual(256, probResult.MaximumBin, "MaximumBin compare fail.");
            Assert.AreEqual("2018/03/12 00:40", probResult.TestTime.ToString("yyyy/MM/dd HH:mm"), "TestTime compare fail.");

            Assert.AreEqual(20, probResult.SummaryData.Columns.Count, "Summary data column count is wrong");
            Assert.AreEqual(6, probResult.SummaryData.Rows.Count, "Summary data row count is wrong");

            Assert.AreEqual(22, probResult.MapData.Columns.Count, "Map data column count is wrong");
            Assert.AreEqual(1080, probResult.MapData.Rows.Count, "Map data row count is wrong");


            Trace.WriteLine(fileInfo.FullName);
        }

        [TestMethod]
        public void SortBinConfigTest()
        {
            StringBuilder configXml = new StringBuilder();
            configXml.AppendLine("<config>")
                .AppendLine(@"<SourcePath>D:\EQPFS\PKG\P5WC1\RAW_BAK\Destination\</SourcePath>")
                .AppendLine(@"<DestinationPath>D:\EQPFS\PKG\P5WC2\RAW_BAK\Destination\</DestinationPath>")
                .AppendLine(@"<QueuePath>D:\EQPFS\PKG\P5WC2\RAW_BAK\BinQueue\</QueuePath>")
                .AppendLine(@"<FailPath>D:\EQPFS\PKG\P5WC2\RAW_BAK\BinFail\</FailPath>")
                .AppendLine(@"<EQ_Path>D:\EQPFS\PKG\P5WC2\EQ\</EQ_Path>")
                .AppendLine("</config>");
            SortBinConfig sortBinConfig = new SortBinConfig();
            sortBinConfig.Load(configXml.ToString());

            Assert.AreEqual(@"D:\EQPFS\PKG\P5WC1\RAW_BAK\Destination\", sortBinConfig.SourcePath);
            Assert.AreEqual(@"D:\EQPFS\PKG\P5WC2\RAW_BAK\Destination\", sortBinConfig.DestinationPath);
            Assert.AreEqual(@"D:\EQPFS\PKG\P5WC2\RAW_BAK\BinQueue\", sortBinConfig.QueuePath);
            Assert.AreEqual(@"D:\EQPFS\PKG\P5WC2\RAW_BAK\BinFail\", sortBinConfig.FailPath);
            Assert.AreEqual(@"D:\EQPFS\PKG\P5WC2\EQ\", sortBinConfig.EqPath);

        }
    }
}
