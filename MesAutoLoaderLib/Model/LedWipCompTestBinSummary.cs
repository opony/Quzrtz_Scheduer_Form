using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Model
{
    class LedWipCompTestBinSummary
    {
        public string LotNo
        { get; set; }

        public string ComponentNo
        { get; set; }

        public int TestType
        { get; set; }

        public DateTime TestTime
        { get; set; }

        public string BinNo
        { get; set; }

        public int BinQty
        { get; set; }

        public int Percentage
        { get; set; }

        public int BinGrade
        { get; set; }

        public string BinCode
        { get; set; }

        public int BinMinQty
        { get; set; }

        public int TapeMinQty
        { get; set; }

        public int BinFlag
        { get; set; }

        public string InventoryNo
        { get; set; }

        public int MaxBinFlag
        { get; set; }


    }
}
