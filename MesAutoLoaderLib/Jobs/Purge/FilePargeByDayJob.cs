using MesAutoLoaderLib.Config;
using MesAutoLoaderLib.Util;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Jobs.Purge
{
    [DisallowConcurrentExecutionAttribute]
    public class FilePargeByDayJob : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        string param;
        string notifyMail;
        public void Execute(IJobExecutionContext context)
        {
            logger.Info("===== start =====");
            try
            {
                notifyMail = Convert.ToString(context.JobDetail.JobDataMap["Notify_Mail"]);
                param = Convert.ToString(context.JobDetail.JobDataMap["Param"]);
                Dictionary<string, string> paramMap = JobConfig.ParserString(param, '=');
                logger.Info("Input : {0}", param);
                logger.Info("Notify Mail : {0}", notifyMail);

                int keepDay = int.Parse(paramMap["KeepDay"]);
                string purgeFolder = paramMap["PurgeFolder"];
                string searchPatten = paramMap["SearchPatten"];

                logger.Info("File keep day : {0}", keepDay);
                logger.Info("Purge Folder : {0}", purgeFolder);
                logger.Info("Search Patten : {0}", searchPatten);

                DateTime beforeTime = DateTime.Now.AddDays(-keepDay);

                DeleteFile(purgeFolder, beforeTime, searchPatten);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "FilePargeByDayJob error");

            }


            logger.Info("===== end =====");
        }

        private void DeleteFile(string path, DateTime beforeTime, string searchPatten)
        {
            logger.Info("Current path : [{0}]", path);
            logger.Info("Get File with last write date < {0}", beforeTime.ToString("yyyy-MM-dd HH:mm:ss"));
            FileInfo[] files = FileSystemUtil.GetFiles(path, searchPatten, System.IO.SearchOption.TopDirectoryOnly, beforeTime);
            logger.Info("Start Delete File count : {0}", files.Length);
            int idx = 0;
            foreach (FileInfo file in files)
            {
                idx++;
                if ((idx % 100) == 0 || idx == files.Length)
                    logger.Info("Deleted File counter : {0}", idx);
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {

                    throw new Exception("Delete File : [" + file.FullName + "] error.", ex);
                }

            }


            DirectoryInfo[] subFolders = FileSystemUtil.GetDirectorys(path);

            logger.Info("Get sub folders : {0}", subFolders);
            logger.Info("Start delete sub folder file.");
            foreach (DirectoryInfo subDirInfo in subFolders)
            {
                DeleteFile(subDirInfo.FullName, beforeTime, searchPatten);
            }

            logger.Info("Delete complted : {0}", path);
        }
    }
}
