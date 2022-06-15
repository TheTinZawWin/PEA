using PEA_Common;
using PEA_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAE_BusinessLayer
{
    public class LOGINBLL
    {
        static ICommonRepository commonRepository;
        static LOGINBLL()
        {
            commonRepository = new CommonRepository();

        }
        public static bool SaveOperationLog(OperationModel operationModel, ref string returnMessage)
        {
            bool result = false;

            PEAEntities db = new PEAEntities();
            TB_OperationLog operation = new TB_OperationLog();
            operation.OperationDatetime = DateTime.Now;
            operation.RemoteAddress = operationModel.RemoteAddress;
            operation.RemoteHost = operationModel.RemoteHost;
            operation.Url = operationModel.Url;
            operation.UserId = operationModel.UserId;
            operation.ActionName = operationModel.ActionName;
            operation.ActionParameter = operationModel.ActionParameter;
            operation.CookieUserId = operationModel.CookieUserId;
            operation.CreatedAt = DateTime.Now;
            operation.CreatedBy = operation.UserId;
            if (commonRepository.isConnected(ref db, ref returnMessage))
            {
                try
                {

                    if (commonRepository.SaveOperationLog(operation, ref db, ref returnMessage))
                    {
                        //commit to database
                        if (commonRepository.dbCommit(db, ref returnMessage))
                        {

                            result = true;
                        }
                    }


                    else
                    {
                        commonRepository.dbRollback(ref returnMessage);
                    }


                }
                catch (Exception ex)
                {

                    returnMessage = ex.Message;
                }
                finally
                {
                    commonRepository.dbClose(db, ref returnMessage);
                }
            }
            return result;
        }

        public static bool LoginUser(string hash, ref string returnMessage, ref SessionUserModel user)
        {
            bool result = false;

            PEAEntities db = new PEAEntities();

            if (commonRepository.isConnected(ref db, ref returnMessage))
            {
                try
                {
                    var selectuser = (from u in db.TB_Employee
                                      where  u.Password == hash

                                      select u).FirstOrDefault();
                    if (selectuser != null && !String.IsNullOrEmpty(selectuser.Name))
                    {
                        user.id = selectuser.EmployeeID;
                        user.UserName = selectuser.Name.ToString();

                        result = true;
                    }
                }
                catch (Exception ex)
                {

                    returnMessage = ex.Message;
                }

            }
            return result;



        }
    }
}
