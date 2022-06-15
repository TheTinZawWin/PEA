using PAE_BusinessLayer;
using PEA_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PEA_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LoginService.svc or LoginService.svc.cs at the Solution Explorer and start debugging.
    public class LoginService : ILoginService
    {
        public bool SaveOperationLog(OperationModel operationModel, ref string returnMessage)
        {
            bool result = false;
            try
            {
                if (LOGINBLL.SaveOperationLog(operationModel, ref returnMessage))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;
        }

        public bool LoginUser(string hash, ref string returnMessage, ref SessionUserModel user)
        {
            bool result = false;
            try
            {
                if (LOGINBLL.LoginUser(hash,  ref returnMessage, ref user))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;
        }
    }
}
