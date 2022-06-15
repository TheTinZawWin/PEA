using PEA_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_DataAccessLayer
{
    public class EmployeeRepository:IEmployeeRepository
    {
        public bool SaveEmployee(TB_Employee task, ref PEAEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {

                db.TB_Employee.Add(task);
                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;
        }

        public bool TaskFinished(List<TB_Employee> taskList, ref PEAEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
                foreach (var item in taskList)
                {
                    db.TB_Employee.Attach(item);
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }

                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;
        }
        public bool EmployeeUpdate(TB_Employee employee, ref PEAEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
                  db.TB_Employee.Attach(employee);
                    db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
               

                result = true;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return result;
        }
        public bool EmployeeDelete(List<TB_Employee> taskList, ref PEAEntities db, ref string returnMessage)
        {
            bool result = false;
            try
            {
                foreach (var item in taskList)
                {
                    db.TB_Employee.Attach(item);
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }

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
