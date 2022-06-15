using PEA_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PEA_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILoginService" in both code and config file together.
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        bool LoginUser(string hash,ref string returnMessage, ref SessionUserModel user);

        [OperationContract]
        bool SaveOperationLog(OperationModel operationModel, ref string returnMessage);
    }
}
