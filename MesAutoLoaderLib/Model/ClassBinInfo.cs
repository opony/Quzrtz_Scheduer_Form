using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Model
{
    /// <summary>
    /// 分類機檔案 parse 的內容
    /// </summary>
    class ClassBinInfo
    {
        private string tapeID;

        public string FileID
        { get; set; }

        public DateTime TestTime
        { get; set; }

        public string WO
        { get; set; }

        public string LOT_ID
        { get; set; }

        public string WAFER_ID
        { get; set; }

        public string SUBSTRATE_ID
        { get; set; }

        public string CST_ID
        { get; set; }

        public string Recipe_ID
        { get; set; }

        public string Recipe_Name
        { get; set; }

        public string OPERATOR
        { get; set; }

        public string TapeID
        {
            get
            {
                return tapeID;
            }

            set
            {
                tapeID = value;
                if (tapeID.Length > 30)
                {
                    ComponentNo = tapeID.Substring(5,5) + tapeID.Substring(21);
                }
                else
                {
                    ComponentNo = tapeID;
                    
                }
            }
        }

        public string ComponentNo
        {
            get;set;
        }

        public string FrameID
        { get; set; }

        public string Bin_CODE
        { get; set; }

        public string SorterID
        { get; set; }

        public DateTime StartTime
        { get; set; }

        public DateTime EndTime
        { get; set; }

        public int BinQty
        {
            get
            {
                return SummaryTb.Rows.Count;
            }
        }

        public DataTable SummaryTb;
    }
}
