using DatabaseLib.Interfaces;
using MesAutoLoaderLib.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class TblWipLotStateDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TblWipLotStateDao(IDatabase database) : base(database)
        {
        }

        public LotState QueryLotInfoByComponentNo(string componNo)
        {
            LotState lotInfo = null;
            try
            {
                StringBuilder sql = new StringBuilder();
                //sql.AppendLine("Select A.LotNo, A.LotSerial, A.LogGroupSerial, B.FeedBackState, B.Substrate_ID,C.mono,C.Productno From tblWIPLotState A,tblWIPComponentState B,TBLWIPLOTBASIS C ")
                //    .AppendLine("Where A.LotNo = B.LotNo And A.Status In (0,1,2,3,4,5,11)")
                //    .AppendLine(" And A.baselotno = C.baselotno ")
                //    .AppendLine("   And B.ComponentNo = '" + componNo + "'");

                sql.AppendLine("Select A.LotNo, A.LotSerial, A.LogGroupSerial, B.FeedBackState, B.Substrate_ID,C.mono,C.Productno From tblWIPLotState A,tblWIPComponentState B,TBLWIPLOTBASIS C ")
                    .AppendLine("Where A.LotNo = B.LotNo ")
                    .AppendLine(" And A.baselotno = C.baselotno ")
                    .AppendLine("   And B.ComponentNo = '" + componNo + "'");

                logger.Debug("SQL : {0}", sql);

                DataTable tb = this.ExecuteQuery(sql.ToString());

                if (tb.Rows.Count == 0)
                    return null;
                DataRow row = tb.Rows[0];
                lotInfo = new LotState();
                lotInfo.LotNo = row.Field<string>("LotNo");
                lotInfo.LotSerial = row.Field<string>("LotSerial");
                lotInfo.LogGroupSerial = row.Field<string>("LogGroupSerial");
                lotInfo.FeedBackState = Convert.ToInt32(row["FeedBackState"]);
                lotInfo.Substrate_ID = row.Field<string>("Substrate_ID");
                lotInfo.MoNo = row.Field<string>("mono");
                lotInfo.ProductNo = row.Field<string>("Productno");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "QueryLotInfoByComponentNo error.");
                throw ex;
            }

            return lotInfo;

        }
    }
}
