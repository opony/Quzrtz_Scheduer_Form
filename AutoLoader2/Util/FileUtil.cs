using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoLoader2.Util
{
    class FileUtil
    {
        /// <summary>
        /// 取得資料夾下最新的檔案
        /// </summary>
        /// <param name="dirPath">Folder</param>
        /// <param name="searchPattern">Search file pattern . ex : *.txt</param>
        /// <returns></returns>
        public static FileInfo GetFiles(string dirPath, string searchPattern, SearchOption searchOption)
        {
            FileInfo[] files = new DirectoryInfo(dirPath).GetFiles(searchPattern, searchOption);

            if (files.Length <= 0)
                return null;
            
            return files[0];
        }
    }
}
