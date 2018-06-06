using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Model
{
    public class ProberTestResult
    {
        public string FileID
        { get; set; }

        public DateTime TestTime
        { get; set; }

        public string WO
        { get; set; }

        public string Operator
        { get; set; }

        public int MaximumBin
        { get; set; }

        public string Specification
        { get; set; }

        public string SpecRemark
        { get; set; }

        public string FileName
        { get; set; }

        public string ProductName
        { get; set; }

        public string DeviceNumber
        { get; set; }

        
        /// <summary>
        /// 注意 csv file 的 LOT_ID 是存放 component id, 操作 DB 時，不可使用
        /// 只用於 export EQ csv
        /// </summary>
        public string LOT_ID
        { get; set; }

        public string ComponentNo
        { get; set; }

        public string Customer
        { get; set; }

        public string Class
        { get; set; }

        public string OrderNumber
        { get; set; }

        public string Temperature
        { get; set; }

        public string Humidity
        { get; set; }

        public string Remark1
        { get; set; }

        public string Remark2
        { get; set; }

        public string Remark3
        { get; set; }

        public string Remark4
        { get; set; }

        public string EQP_ID
        { get; set; }

        public string Recipe_Name
        { get; set; }

        public int QTY
        { get; set; }

        public int SideBin
        { get; set; }

        public int NGBin
        { get; set; }

        public int ResortBin
        { get; set; }

        public string TryLot
        { get; set; }

        public int CORREL1
        { get; set; }

        public int CORREL2
        { get; set; }

        public string PageIDGroupNo
        { get; set; }

        public DataTable SummaryData;

        public DataTable MapData;

        public string ToCsvHeader(string fileName, string moNo, string lotNo )
        {
            StringBuilder headerStr = new StringBuilder();
            headerStr.AppendLine("FileID," + fileName);
            headerStr.AppendLine("TestTime," + DateTime.Now.ToString("yyyy/MM/dd H:mm:ss"));
            headerStr.AppendLine("WO," + moNo);
            headerStr.AppendLine("EQP_ID," + EQP_ID);
            headerStr.AppendLine("LOT_ID," + lotNo);
            headerStr.AppendLine("WAFER_ID," + ComponentNo);
            headerStr.AppendLine("SUBSTRATE_ID,");
            headerStr.AppendLine("CST_ID,");
            headerStr.AppendLine("Recipe_ID," + Recipe_Name);
            headerStr.AppendLine("Recipe_Name," + Recipe_Name);
            headerStr.AppendLine("OPERATOR," + Operator);
            headerStr.AppendLine("TapeIDGroup," + PageIDGroupNo);
            headerStr.Append("Program,");

            return headerStr.ToString();
        }

        public string ToCsvSummary()
        {
            //舊版的 code ，export 的邏輯
            return "BIN,Bin_CODE,TotalCount";
        }

        public string ToCsvMapData()
        {
            if (MapData == null)
                return "";
            StringBuilder str = new StringBuilder();
            string[] columnNames = (from dc in MapData.Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();
            str.AppendLine(string.Join(",", columnNames));

            foreach (DataRow dataRow in MapData.Rows)
            {
                str.AppendLine(string.Join(",", dataRow.ItemArray));
            }

            return str.ToString();
        }
    }
}
