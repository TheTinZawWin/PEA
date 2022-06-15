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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DepartmentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DepartmentService.svc or DepartmentService.svc.cs at the Solution Explorer and start debugging.
    public class DepartmentService : IDepartmentService
    {
        public bool GetDepartmentList(ref List<DepartmentModel> departmentList, ref string returnMessage)
        {
            bool result = false;
            try
            {
                if (DEPTBLL.GetDepartmentList(ref departmentList, ref returnMessage))
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
