using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoLoader2.Util
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

        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
