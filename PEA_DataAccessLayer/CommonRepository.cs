using PEA_Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_DataAccessLayer
{
   public class CommonRepository:ICommonRepository
    {
        DbContextTransaction dbContextTransaction;
        public bool isConnected(ref PEAEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
                //RBS_dbEntities dbContext = new RBS_dbEntities();
                if (db.Database.Exists())
                {
                    dbContextTransaction = db.Database.BeginTransaction();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;

        }
        public bool dbCommit(PEAEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
                db.SaveChanges();
                dbContextTransaction.Commit();
                //db.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            finally
            {
                db.Database.Connection.Close();
                db.Dispose();
            }
            return result;

        }
        public bool dbClose(PEAEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
                db.Database.Connection.Close();
                db.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;

        }
        public bool dbRollback(ref string returnMessage)
        {
            bool result = false;
            try
            {
                dbContextTransaction.Rollback();
                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;

        }

        public bool SaveOperationLog(TB_OperationLog task, ref PEAEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {

                db.TB_OperationLog.Add(task);
                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;
        }

    }
}
