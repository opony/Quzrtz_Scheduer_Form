using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Model
{
    class LedWipPageSummary
    {
        public string LotNo
        { get; set; }

        public string ComponentNo
        { get; set; }

        public string FileID
        { get; set; }

        public DateTime TestTime
        { get; set; }

        public string MoNo
        { get; set; }

        public string EquipmentNo
        { get; set; }

        public string CassetteNo
        { get; set; }

        public string RecipeID
        { get; set; }

        public string RecipeName
        { get; set; }

        public string Operator
        { get; set; }

        public string WaferID
        { get; set; }

        public string SubStrateID
        { get; set; }

        public string FrameID
        { get; set; }

        public string BinCode
        { get; set; }

        public int BinQty
        { get; set; }

        public string SorterID
        { get; set; }

        public DateTime StartTime
        { get; set; }

        public DateTime EndTime
        { get; set; }

        public int CreateLot
        { get; set; }

        public int Status
        { get; set; }
    }
}
