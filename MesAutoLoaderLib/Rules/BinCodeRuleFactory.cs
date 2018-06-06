using MesAutoLoaderLib.Interfaces.Rule;
using MesAutoLoaderLib.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Rules
{
    class BinCodeRuleFactory
    {
        //static Dictionary<string, Dictionary<string, string>> binCodeChangeMapDict = new Dictionary<string, Dictionary<string, string>>();
        //static Dictionary<string, Dictionary<string, decimal>> binCodeQtyMapDict = new Dictionary<string, Dictionary<string, decimal>>();

        public static IBinCodeChangeRule GetBinChangeRule(string moNo, string productNo, string lotNo)
        {   
            Dictionary<string, string> binCodeChangeMap = GetBinCodeChangeMap(moNo, productNo);
            Dictionary<string, decimal> binCodeQtyMap = GetBinCodeQtyMap(lotNo);

            return new BinCodeChangeRule(binCodeChangeMap, binCodeQtyMap);
        }

        public static IBinCodeChangeRule GetBinChangeForTrRule(string moNo, string productNo, string lotNo)
        {
            Dictionary<string, string> binCodeChangeMap = GetBinCodeChangeMap(moNo, productNo);
            Dictionary<string, decimal> binCodeQtyMap = GetBinCodeQtyMap(lotNo);

            return new BinCodeChangeForTrRule(binCodeChangeMap, binCodeQtyMap);
        }


        /// <summary>
        /// Query Bin 表
        /// </summary>
        /// <param name="moNo"></param>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetBinCodeChangeMap(string moNo, string productNo)
        {
            string key = "";

            if (moNo.StartsWith("T05TE"))
                key = moNo.Substring(0, 10);
            else
                key = productNo;

            //Dictionary<string, string> binCodeChangeMap;
            //if (binCodeChangeMapDict.ContainsKey(key) != true)
            //{
            //    DataTable tb = MesProdDbProxy.QueryBinChagneDefineData(key);
            //    binCodeChangeMap = tb.AsEnumerable()
            //                    .ToDictionary<DataRow, string, string>(row => row.Field<string>("BINCODE"),
            //                    row => row.Field<string>("BINNO"));
            //    binCodeChangeMapDict.Add(key, binCodeChangeMap);
            //}

            //return binCodeChangeMapDict[key];

            DataTable tb = MesProdDbProxy.QueryBinChagneDefineData(key);
            Dictionary<string, string> binCodeChangeMap = tb.AsEnumerable()
                            .ToDictionary<DataRow, string, string>(row => row.Field<string>("BINCODE"),
                            row => row.Field<string>("BINNO"));
            return binCodeChangeMap;
        }

        private static Dictionary<string, decimal> GetBinCodeQtyMap(string lotNo)
        {
            //Dictionary<string, decimal> binCodeQtyMap;
            //if (binCodeQtyMapDict.ContainsKey(lotNo) != true)
            //{
            //    DataTable binQtyTable = MesProdDbProxy.QueryBinQtyByLotNo(lotNo);

            //    binCodeQtyMap = binQtyTable.AsEnumerable()
            //        .ToDictionary<DataRow, string, decimal>(row => row.Field<string>("BINCODE"),
            //                    row => row.Field<decimal>("BINQTY"));

            //    binCodeQtyMapDict.Add(lotNo, binCodeQtyMap);
            //}


            //return binCodeQtyMapDict[lotNo];

            DataTable binQtyTable = MesProdDbProxy.QueryBinQtyByLotNo(lotNo);

            Dictionary<string, decimal> binCodeQtyMap = binQtyTable.AsEnumerable()
                .ToDictionary<DataRow, string, decimal>(row => row.Field<string>("BINCODE"),
                            row => row.Field<decimal>("BINQTY"));

            return binCodeQtyMap;
        }

    }
}
