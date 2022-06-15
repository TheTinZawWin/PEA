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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select EmployeeService.svc or EmployeeService.svc.cs at the Solution Explorer and start debugging.
    public class EmployeeService : IEmployeeService
    {
        public bool EmployeeCreate(EMPModel empModel, ref string ret)
        {
            bool result = false;
            try
            {
                if (EMPBLL.EmployeeCreate(empModel, ref ret))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            return result;
        }

        public bool GetEmployeeList( ref List<EMPModel> empList, ref string ret)
        {
            bool result = false;
            try
            {
                if (EMPBLL.GetEmployeeList(ref empList, ref ret))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message;
            }
            return result;
        }

        public bool EmployeeUpdate(EMPModel empModel, ref string returnMessage)
        {
            bool result = false;
            try
            {
                if (EMPBLL.EmployeeUpdate(empModel, ref returnMessage))
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

        public bool GetEmployeeById(Guid id, ref UpdateEMPTModel empt, ref string returnMessage)
        {
            bool result = false;
            try
            {
                if (EMPBLL.GetEmployeeById(id,ref empt, ref returnMessage))
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

        public bool EmployeeDelete(int userId,List<string> EMT_ID_LIST, ref string returnMessage)
        {
            bool result = false;
            try
            {
                if (EMPBLL.EmployeeDelete(userId,EMT_ID_LIST, ref returnMessage))
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

        public bool GetAllDataCount(ref int activeCount, ref int inactiveCount, ref string returnMessage)
        {
            bool result = false;
            try
            {
                if (EMPBLL.GetAllDataCount(ref activeCount, ref inactiveCount, ref returnMessage))
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

