using MesAutoLoaderLib.Interfaces.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MesAutoLoaderLib.Rules
{
    class BinCodeChangeRule : IBinCodeChangeRule
    {
        Dictionary<string, string> binNoChangeMap;
        Dictionary<string, decimal> binCodeQtyMap;

        public BinCodeChangeRule(Dictionary<string, string> binNoChangeMap, Dictionary<string, decimal> binCodeQtyMap)
        {
            this.binNoChangeMap = binNoChangeMap;
            this.binCodeQtyMap = binCodeQtyMap;
        }

        public Dictionary<string, string> GetBinCodeChangeMap()
        {
            return this.binNoChangeMap;
        }

        public void ChangeBinCode(DataTable mapDataTb)
        {
            string binCode;
            foreach (DataRow binRow in mapDataTb.Rows)
            {
                binCode = binRow.Field<string>("BIN_CODE");
                if (binCode.ToUpper().Contains("SIDE"))
                    binRow["Bin"] = "99";
                else if (binCode.ToUpper().StartsWith("NOTG"))
                    binRow["Bin"] = "100";
                else
                {
                    if (binNoChangeMap.ContainsKey(binCode))
                    {
                        //在Bin 表裡的，就照 Bin 表轉換
                        binRow["Bin"] = binNoChangeMap[binCode];
                    }
                    else
                    {
                        //不存在 bin 表裡的全轉成 98 , ReSorter
                        binRow["Bin"] = "98";
                        binRow["BIN_CODE"] = "ReSorter";
                    }
                }

                if (binCodeQtyMap.ContainsKey(binCode))
                {
                    
                    if (binCodeQtyMap[binCode] < 5)
                    {
                        binRow["Bin"] = "98";
                        binRow["BIN_CODE"] = "ReSorter";
                    }
                }
            } //foreach (DataRow binRow in mapDataTb.Rows)
        }
    }
}
