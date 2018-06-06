using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseLib.Interfaces;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class TblWipTemp_ComponentAttribDao : BaseDao
    {
        public TblWipTemp_ComponentAttribDao(IDatabase database) : base(database)
        {
        }

        public void UpdateAttribValue(string lotNo,  string componentNo,string attNo,  string attValue )
        {
            try
            {
                //string sql = "UPDate tblWIPTemp_ComponentAttrib Set AttribValue = '" + attValue + "' Where LotNo = '"+ lotNo + "' And ComponentNo = '" + componentNo+"' And AttribNo = 'CYield'";
                string sql = "UPDate tblWIPTemp_ComponentAttrib Set AttribValue = '" + attValue + "' Where LotNo = '" + lotNo + "' And ComponentNo = '" + componentNo + "' And AttribNo = '" + attNo + "'";

                this.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public void UpdateAttribScrapQty(string lotNo, string componentNo, int dataMapCount)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UPDate tblWIPTemp_ComponentAttrib Set AttribValue = ")
                    .AppendLine(" (select AttribValue - " + dataMapCount + " from tblWIPTemp_ComponentAttrib Where LotNo = '" + lotNo + "' And ComponentNo = '" + componentNo + "' And AttribNo = 'CInputQty' )")
                    .AppendLine("Where LotNo = '" + lotNo + "' And ComponentNo = '" + componentNo + "' And AttribNo = 'CScrapQty'");

                this.ExecuteNonQuery(sql.ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
