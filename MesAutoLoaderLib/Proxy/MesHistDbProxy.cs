using DatabaseLib.Database;
using DatabaseLib.Interfaces;
using MesAutoLoaderLib.Dao.Mes.Hist;
using MesAutoLoaderLib.Dao.Mes.Nas;
using MesAutoLoaderLib.Util;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Proxy
{
    class MesHistDbProxy
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static DataTable QueryMesRptSorterRawHeaderEmptyTable()
        {
            IDatabase db = GetDb();
            MesRpt_Sorter_RawHeaderDao sorterRawHeaderDao = new MesRpt_Sorter_RawHeaderDao(db);
            return sorterRawHeaderDao.QueryEmptyTable();
        }

        

        public static void DeleteMesRptSorterRawHeaderByFileName(string fileName)
        {
            IDatabase db = GetDb();
            MesRpt_Sorter_RawHeaderDao sorterRawHeaderDao = new MesRpt_Sorter_RawHeaderDao(db);
            sorterRawHeaderDao.DeleteByFileName(fileName);
        }

        public static DataTable QueryRptSorterRawEmptyTable()
        {
            IDatabase nasDb = GetNasDb();
            MesRpt_Sorter_RawDataDao sorterRawDataDao = new MesRpt_Sorter_RawDataDao(nasDb);
            return sorterRawDataDao.QueryEmptyTb();
        }

        

        public static void insertRptSorterRawData(Dictionary<string, object> rawHeaderDataMap, List<Dictionary<string, object>> rawDataList)
        {
            IDatabase db = GetDb();
            IDatabase nasDb = GetNasDb();
            logger.Debug("Get connection.");
            nasDb.GetConnection();
            db.GetConnection();
            logger.Debug("Begin transaction.");
            nasDb.BeginTrans();
            db.BeginTrans();
            try
            {
                MesRpt_Sorter_RawHeaderDao sorterRawHeaderDao = new MesRpt_Sorter_RawHeaderDao(db);
                MesRpt_Sorter_RawHeaderDao nasSorterRawHeaderDao = new MesRpt_Sorter_RawHeaderDao(nasDb);
                MesRpt_Sorter_RawDataDao sorterRawDataDao = new MesRpt_Sorter_RawDataDao(nasDb);

                string fileName = (string)rawHeaderDataMap["FILENAME"];
                sorterRawHeaderDao.DeleteByFileName(fileName);
                nasSorterRawHeaderDao.DeleteByFileName(fileName);
                sorterRawDataDao.DeleteByFileName(fileName);

                sorterRawHeaderDao.InsertByDataMap(rawHeaderDataMap);
                nasSorterRawHeaderDao.InsertByDataMap(rawHeaderDataMap);

                sorterRawDataDao.Insert(rawDataList);

                nasDb.Commit();
                db.Commit();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "InsertRptSorterRawData error.");
                logger.Debug("Rollback.");
                nasDb.Rollback();
                db.Rollback();
                throw ex;
            }
            finally
            {
                db.Close();
                nasDb.Close();
            }
            


        }


        private static IDatabase GetDb()
        {
            return new OraDatabase(AppConfigFactory.MesHistConnStr);
        }

        private static IDatabase GetNasDb()
        {
            return new OraDatabase(AppConfigFactory.NasDbConnStr);
        }
    }
}
