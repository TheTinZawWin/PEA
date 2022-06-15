using Newtonsoft.Json;
using PEA_Common;
using ProjectEmployeeAssignment.EmployeeService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectEmployeeAssignment.Controllers
{
    public class EmployeeController : CustomController
    {
       
       
        /// <summary>
        /// create employee 
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="departmentId"></param>
        /// <param name="joindate"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createEmployee(string empId, string name,string email,string phone,string departmentId,string joindate,string password)
        {
            string ret = String.Empty;
            EMPModel empModel = new EMPModel();
            string userName = LoginInfo.UserName;
            empModel.EmployeeID = empId;
            empModel.Name = name;
            empModel.Email = email;
            empModel.Phone = phone;
            empModel.Department = departmentId;
            empModel.JointDate = DateTime.Parse(joindate);
            empModel.Password =  CommonUtility.GenerateHash(empId, password);
            empModel.CreatedBy = LoginInfo.UserId.ToString();
            empModel.Active = true;
            EmployeeServiceClient employeeService = new EmployeeServiceClient();
            var time = new TimeSpan(0, 5, 0);
            employeeService.Endpoint.Binding.CloseTimeout = time;
            employeeService.Endpoint.Binding.OpenTimeout = time;
            employeeService.Endpoint.Binding.ReceiveTimeout = time;
            employeeService.Endpoint.Binding.SendTimeout = time;


            if (employeeService.EmployeeCreate(empModel, ref ret))
            {
                ret = "success";
            }
            else
            {
                ret = "fail";
            }
            return Json(new { result = ret }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// update employee
        /// </summary>
        /// <param name="id"></param>
        /// <param name="empId"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="departmentId"></param>
        /// <param name="joindate"></param>
        /// <param name="password"></param>
        /// <param name="checkActive"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult updateEmployee(Guid id,string empId, string name, string email, string phone, string departmentId, string joindate, string password,bool checkActive)
        {
            string ret = String.Empty;
            EMPModel empModel = new EMPModel();
            empModel.ID = id;
            empModel.EmployeeID = empId;
            empModel.Name = name;
            empModel.Email = email;
            empModel.Phone = phone;
            empModel.Department = departmentId;
            empModel.JointDate = DateTime.Parse(joindate);
            empModel.Password = LoginInfo.GetPasswordHash(empId, password);
            empModel.UpdatedBy= LoginInfo.UserId.ToString();
            empModel.Active = checkActive;
            EmployeeServiceClient employeeService = new EmployeeServiceClient();
            var time = new TimeSpan(0, 5, 0);
            employeeService.Endpoint.Binding.CloseTimeout = time;
            employeeService.Endpoint.Binding.OpenTimeout = time;
            employeeService.Endpoint.Binding.ReceiveTimeout = time;
            employeeService.Endpoint.Binding.SendTimeout = time;
            if (employeeService.EmployeeUpdate(empModel, ref ret))
            {
                ret = "success";
            }
            else
            {
                ret = "fail";
            }
            return Json(new { result = ret }, JsonRequestBehavior.AllowGet);
        }
      
        /// <summary>
        /// update employee to inactive
        /// </summary>
        /// <param name="EMT_ID_LIST"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeDelete(List<string> EMT_ID_LIST)
        {
            string ret = String.Empty;
            EmployeeServiceClient employeeService = new EmployeeServiceClient();
            var time = new TimeSpan(0, 5, 0);
            employeeService.Endpoint.Binding.CloseTimeout = time;
            employeeService.Endpoint.Binding.OpenTimeout = time;
            employeeService.Endpoint.Binding.ReceiveTimeout = time;
            employeeService.Endpoint.Binding.SendTimeout = time;
            if (LoginInfo.UserId == "001")
            {
                if (employeeService.EmployeeDelete(Convert.ToInt32(LoginInfo.UserId), EMT_ID_LIST.ToArray(), ref ret))
                {
                    ret = "success";
                }
            }
               
          
            return Json(new { result = ret }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get employee list
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string GetEmployeeList()
        {
            string ret = string.Empty;
            EMPModel[] empList = null;
            EmployeeServiceClient employeeService = new EmployeeServiceClient();
            var time = new TimeSpan(0, 5, 0);
            employeeService.Endpoint.Binding.CloseTimeout = time;
            employeeService.Endpoint.Binding.OpenTimeout = time;
            employeeService.Endpoint.Binding.ReceiveTimeout = time;
            employeeService.Endpoint.Binding.SendTimeout = time;
            try
            {
                if (employeeService.GetEmployeeList(ref empList, ref ret))
                {

                    DataTable dt;
                   
                        var collection = from emp in empList.AsEnumerable()
                                         select new
                                         {
                                             ID = emp.ID,
                                             EmployeeID = emp.EmployeeID,
                                             Name = emp.Name,
                                             Email = emp.Email,
                                             Phone = emp.Phone,
                                             Department = emp.Department,
                                             JointDate =emp.JointDate,
                                             Active = emp.Active,
                                             CreatedBy = emp.CreatedBy,
                                             UpdatedBy = emp.UpdatedBy
                                         };
                        dt = CommonFunction.ConvertToDataTable(collection.ToList());
                    
                  




                    List<string[]> p = CommonFunction.ConvertTable(dt);

                    Dictionary<string, List<string[]>> d = new Dictionary<string, List<string[]>>();
                    d.Add("data", p);

                    ret = JsonConvert.SerializeObject(d);
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message.ToString();
            }
            return ret;
        }

        /// <summary>
        /// get only one employee with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEmployeeById(Guid id)
        {
            string ret = String.Empty;
            UpdateEMPTModel empt = new UpdateEMPTModel();
            EmployeeServiceClient employeeService = new EmployeeServiceClient();
            var time = new TimeSpan(0, 5, 0);
            employeeService.Endpoint.Binding.CloseTimeout = time;
            employeeService.Endpoint.Binding.OpenTimeout = time;
            employeeService.Endpoint.Binding.ReceiveTimeout = time;
            employeeService.Endpoint.Binding.SendTimeout = time;
            if (employeeService.GetEmployeeById(id, ref empt, ref ret))
            {
                if (empt.EmployeeID == LoginInfo.UserId || LoginInfo.UserId=="001")
                {
                    ret = "success";
                }

                else
                {
                    ret = "unauthorized";
                }
               
            }
            else
            {
                ret = "fail";
            }
            return Json(new { result = ret, empt }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// employee data count for active and inactive
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetAllDataCount()
        {
            string ret = String.Empty;
            int activeCount = 0;
            int inactiveCount = 0;
            EmployeeServiceClient employeeService = new EmployeeServiceClient();
            var time = new TimeSpan(0, 5, 0);
            employeeService.Endpoint.Binding.CloseTimeout = time;
            employeeService.Endpoint.Binding.OpenTimeout = time;
            employeeService.Endpoint.Binding.ReceiveTimeout = time;
            employeeService.Endpoint.Binding.SendTimeout = time;
            if (employeeService.GetAllDataCount(ref activeCount, ref inactiveCount,  ref ret))
            {
                ret = "success";
            }
            else
            {
                ret = "fail";
            }
            return Json(new { result = ret, activeCount,inactiveCount }, JsonRequestBehavior.AllowGet);
        }
    }
}