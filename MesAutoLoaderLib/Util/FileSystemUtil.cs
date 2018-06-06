using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Util
{
    class FileSystemUtil
    {
        /// <summary>
        /// 取得資料夾下最新的檔案
        /// </summary>
        /// <param name="dirPath">Folder</param>
        /// <param name="searchPattern">Search file pattern . ex : *.txt</param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(string dirPath, string searchPattern, SearchOption searchOption)
        {
            FileInfo[] files = new DirectoryInfo(dirPath).GetFiles(searchPattern, searchOption);

            return files;
        }


        public static FileInfo[] GetFiles(string dirPath, string searchPattern, SearchOption searchOption, DateTime ModifiedDayBefore)
        {

            FileInfo[] files = new DirectoryInfo(dirPath).GetFiles(searchPattern, searchOption);
            var selFiles = (from row in files
                            where row.LastWriteTime < ModifiedDayBefore
                            select row).ToArray();

            return selFiles;

        }
        public static DirectoryInfo[] GetDirectorys(string dirPath)
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);
            return di.GetDirectories();
        }

        public static void CreateFolderIfNoExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void WriteTxtFile(string filePath, string content, bool isOverWrite)
        {
            if (File.Exists(filePath))
            {
                if (isOverWrite)
                    File.Delete(filePath);
                else
                    throw new Exception("Can't Write file , File is exists : [" + filePath + "]");
            }

            File.WriteAllText(filePath, content);
        }

        public static void MoveFile(string newFolder, FileInfo fileInfo)
        {
            CreateFolderIfNoExists(newFolder);

            string newFullPath = Path.Combine(newFolder, fileInfo.Name);

            File.Delete(newFullPath);
            fileInfo.MoveTo(newFullPath);
        }


    }
}
