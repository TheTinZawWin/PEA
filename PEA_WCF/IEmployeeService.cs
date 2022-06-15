using PEA_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PEA_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeeService" in both code and config file together.
    [ServiceContract]
    public interface IEmployeeService
    {
        [OperationContract]
        bool EmployeeCreate(EMPModel empModel, ref string ret);
        [OperationContract]
        bool GetEmployeeList(ref List<EMPModel> empList, ref string ret);

        [OperationContract]
        bool EmployeeUpdate(EMPModel empModel, ref string returnMessage);

        [OperationContract]
        bool GetEmployeeById(Guid id, ref UpdateEMPTModel empt, ref string returnMessage);

        [OperationContract]
        bool EmployeeDelete(int userId,List<string> EMT_ID_LIST, ref string returnMessage);

        [OperationContract]
        bool GetAllDataCount(ref int activeCount, ref int inactiveCount, ref string returnMessage);
    }
}
