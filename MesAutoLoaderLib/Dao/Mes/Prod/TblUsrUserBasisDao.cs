using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseLib.Interfaces;
using NLog;
using System.Data;

namespace MesAutoLoaderLib.Dao.Mes.Prod
{
    class TblUsrUserBasisDao : BaseDao
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TblUsrUserBasisDao(IDatabase database) : base(database)
        {
        }

        public DataTable QueryUserBasisByGroupNo(string groupNo)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select a.*, b.GROUPNO")
                    .AppendLine("from TBLUSRUSERBASIS a, TBLUSRUSERGROUP b")
                    .AppendLine("where a.USERNO = b.USERNO")
                    .AppendLine("and b.GROUPNO = '" + groupNo + "'");

                return this.ExecuteQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex, "QueryUserBasisByGroupNo error.");
                throw ex;
            }
        }
    }
}
