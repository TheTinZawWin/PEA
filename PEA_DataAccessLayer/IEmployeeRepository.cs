using PEA_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_DataAccessLayer
{
  public  interface IEmployeeRepository
    {
        bool SaveEmployee(TB_Employee task, ref PEAEntities db, ref string returnMessage);

        bool TaskFinished(List<TB_Employee> taskList, ref PEAEntities db, ref string returnMessage);
        bool EmployeeUpdate(TB_Employee employee, ref PEAEntities db, ref string returnMessage);
        bool EmployeeDelete(List<TB_Employee> taskList, ref PEAEntities db, ref string returnMessage);
    }
}
