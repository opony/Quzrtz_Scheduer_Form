using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MesAutoLoaderLib.Model.Config
{
    class ProberConfig
    {
        public string SourcePath
        { get; set; }

        public string QueuePath
        { get; set; }

        public string FailPath
        { get; set; }

        public string DestinationPath
        { get; set; }

        public string PiLotPath
        { get; set; }

        /// <summary>
        /// <config>
        ///     <SourcePath>D:\EQPFS\PKG\P5WC1\RAW_BAK\Destination\</SourcePath>
        ///     <DestinationPath>D:\EQPFS\PKG\P5WC2\RAW_BAK\Destination\</DestinationPath>
        ///     <QueuePath>D:\EQPFS\PKG\P5WC2\RAW_BAK\BinQueue\</QueuePath>
        ///     <FailPath>D:\EQPFS\PKG\P5WC2\RAW_BAK\BinFail\</FailPath>
        ///     <PiLotPath>D:\EQPFS\PKG\P5WC2\RAW_BAK\BinFail\</PiLotPath>
        /// </config>
        /// </summary>
        /// <param name="configXml"></param>
        public void Load(string configXml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(configXml);
            DestinationPath = doc.SelectSingleNode("/config/DestinationPath").InnerText;
            
            FailPath = doc.SelectSingleNode("/config/FailPath").InnerText;
            SourcePath = doc.SelectSingleNode("/config/SourcePath").InnerText;
            QueuePath = doc.SelectSingleNode("/config/QueuePath").InnerText;
            PiLotPath = doc.SelectSingleNode("/config/PiLotPath").InnerText;
        }
    }
}
