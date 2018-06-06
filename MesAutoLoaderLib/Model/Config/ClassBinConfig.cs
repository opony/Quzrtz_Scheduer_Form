using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MesAutoLoaderLib.Model.Config
{
    class ClassBinConfig
    {
        public string SourcePath
        { get; set; }

        public string QueuePath
        { get; set; }

        public string FailPath
        { get; set; }

        public string DestinationPath
        { get; set; }

        /// <summary>
        /// <config>
        ///     <SourcePath>D:\EQPFS\PKG\P5WC2\RAW\</SourcePath>
        ///     <DestinationPath>D:\EQPFS\PKG\P5WC2\RAW_BAK\Destination\</DestinationPath>
        ///     <QueuePath>D:\EQPFS\PKG\P5WC2\RAW_BAK\Queue1\</QueuePath>
        ///     <FailPath>D:\EQPFS\PKG\P5WC2\RAW_BAK\Fail\</FailPath>
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
            
        }

    }
}
