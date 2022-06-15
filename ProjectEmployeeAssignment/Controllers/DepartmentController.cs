using Newtonsoft.Json;
using PEA_Common;
using ProjectEmployeeAssignment.DepartmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectEmployeeAssignment.Controllers
{
    public class DepartmentController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string GetDepartmentList()
        {
            string ret = string.Empty;

            DepartmentServiceClient departmentServiceClient = new DepartmentServiceClient();
            DepartmentModel[] list = null;

            try
            {
               
                if (departmentServiceClient.GetDepartmentList( ref list, ref ret))
                {
                    ret = JsonConvert.SerializeObject(list.OrderBy(d => d.ID));
                }
            }
            catch (Exception ex)
            {
                ret = ex.Message.ToString();
            }

            
            return ret;
        }
    }
}