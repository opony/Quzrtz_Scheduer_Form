using MesAutoLoaderLib.Jobs.Loader;
using MesAutoLoaderLib.Jobs.Purge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoLoader2.Schedulers
{
    class JobFactory
    {
        public static Dictionary<string, Type> GetJobMap()
        {
            Dictionary<string, Type> jobMap = new Dictionary<string, Type>();
            
            //File purge
            jobMap.Add("FilePargeByDayJob", typeof(FilePargeByDayJob));

            //ImportWhiteChipSortBin : 套 Bin loader
            jobMap.Add("ImportWhiteChipSortBin", typeof(ImportWhiteChipSortBinJob));

            //ImportBTestForWhiteChipJob : Prober loader
            jobMap.Add("ImportBTestForWhiteChipJob", typeof(ImportBTestForWhiteChipJob));

            //ImportBTestForWhiteChipJob : Prober loader
            jobMap.Add("ImportPageForWhiteChipJob", typeof(ImportPageForWhiteChipJob));

            return jobMap;
        }
    }
}
