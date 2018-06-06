
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml;

namespace AutoLoader2.Util
{
    class AppConfigFactory
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static string IP = GetLocalIPAddress();

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    logger.Info("Get ap IP : {0}", ip.ToString());
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static string MesProdConnStr
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MesProd"].ConnectionString;
            }

        }


        public static string MesHistConnStr
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MesHist"].ConnectionString;
            }

        }

        public static string NasDbConnStr
        {
            get {
                return ConfigurationManager.ConnectionStrings["NasDb"].ConnectionString;
            }
        }

        public static string StmpServer
        {
            get {
                return ConfigurationManager.AppSettings["SMTP_Server"];
            }
        }

        public static int StmpPort
        {
            get {
                string value = ConfigurationManager.AppSettings["SMTP_Port"];
                return int.Parse(value);
            }
        }

        public static string AutoRunID
        {
            get {
                return ConfigurationManager.AppSettings["AutoRunID"];
            }
        }

        public static int ThreadCount
        {
            get {
                string value = ConfigurationManager.AppSettings["ThreadCount"];
                return int.Parse(value);
            }
        }

        //public static void LoadAppConfig()
        //{
        //    string value = ConfigurationManager.AppSettings["ThreadCount"];
        //    ThreadCount = int.Parse(value);

        //    value = ConfigurationManager.AppSettings["AutoRunID"];
        //    AutoRunID = value;
        //}

        public static void UpdateAppConfig()
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            foreach (XmlElement element in xmlDoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {
                        if (node.Attributes["key"].Value.Equals("ThreadCount"))
                        {
                            node.Attributes["value"].Value = Convert.ToString(ThreadCount);
                        }
                    }
                }
            }

            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            ConfigurationManager.RefreshSection("appSettings");

        }
    }
}
