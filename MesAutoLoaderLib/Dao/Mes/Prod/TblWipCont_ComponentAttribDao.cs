using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseLib.Interfaces;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class TblWipCont_ComponentAttribDao : BaseDao
    {
        public TblWipCont_ComponentAttribDao(IDatabase database) : base(database)
        {
        }

        public void UpdateAttribValue(string lotNo, string lotSerial,string componentNo, string attNo, string attValue)
        {
            try
            {
                string sql = "UPDate tblWIPCont_ComponentAttrib Set AttribValue = '" + attValue + "' Where LotSerial = '" + lotSerial + "' And ComponentNo = '"+ componentNo + "' And AttribNo = '" + attNo + "'";

                this.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
