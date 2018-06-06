using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Model
{
    class LedWipPageBinSummary
    {
        public string LotNo
        { get; set; }

        public string ComponentNo
        { get; set; }

        public string WaferNo
        { get; set; }

        public DateTime TestTime
        { get; set; }

        public int BinQty
        { get; set; }

    }
}
