using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MesAutoLoaderLib.Model.Config
{
    public class SortBinConfig
    {
        public string DestinationPath
        { get; set; }

        public string EqPath
        { get; set; }

        public string FailPath
        { get; set; }

        public string SourcePath
        { get; set; }

        public string QueuePath
        { get; set; }
        


        /// <summary>
        /// <config>
        ///     <SourcePath>D:\EQPFS\PKG\P5WC1\RAW_BAK\Destination\</SourcePath>
        ///     <DestinationPath>D:\EQPFS\PKG\P5WC2\RAW_BAK\Destination\</DestinationPath>
        ///     <QueuePath>D:\EQPFS\PKG\P5WC2\RAW_BAK\BinQueue\</QueuePath>
        ///     <FailPath>D:\EQPFS\PKG\P5WC2\RAW_BAK\BinFail\</FailPath>
        ///     <EQ_Path>D:\EQPFS\PKG\P5WC2\EQ\</EQ_Path>
        /// </config>
        /// </summary>
        /// <param name="configXml"></param>
        public void Load(string configXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(configXml);
            DestinationPath = doc.SelectSingleNode("/config/DestinationPath").InnerText;
            EqPath = doc.SelectSingleNode("/config/EQ_Path").InnerText;
            FailPath = doc.SelectSingleNode("/config/FailPath").InnerText;
            SourcePath = doc.SelectSingleNode("/config/SourcePath").InnerText;
            QueuePath = doc.SelectSingleNode("/config/QueuePath").InnerText;
        }
    }
}
