using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Model
{
    class LedWipCompTestSummary
    {
        //LotNo, ComponentNo, TestType, Active, FileID, TestTime, MONo, EquipmentNo
        public string LotNo
        { get; set; }

        public string ComponentNo
        { get; set; }

        public int TestType
        { get; set; }

        public int Active
        { get; set; }

        public string FileID
        { get; set; }

        public DateTime TestTime
        { get; set; }

        public string MoNo
        { get; set; }

        public string EquipmentNo
        { get; set; }

        //CassetteNo, Recipe_ID, Recipe_Name, Operator, Product, SpecNo, TestQty, GoodQty, TotalYield, BinQty
        public string CassetteNo
        { get; set; }

        public string Recipe_ID
        { get; set; }

        public string Recipe_Name
        { get; set; }

        public string Operator
        { get; set; }

        public string Product
        { get; set; }

        public int SpecNo
        { get; set; }

        public int TestQty
        { get; set; }

        public int GoodQty
        { get; set; }

        public double TotalYield
        {
            get
            {
                return Math.Round((GoodQty / (double)TestQty) * 100, 2);
            }


        }

        public int BinQty
        { get; set; }

        public DateTime StartTime
        { get; set; }

        public DateTime EndTime
        { get; set; }




    }
}
