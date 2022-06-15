using PEA_Common;
using ProjectEmployeeAssignment.LoginService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectEmployeeAssignment.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(string userId, string password)
        {
            string ret = String.Empty;
            SessionUserModel user = new SessionUserModel();

            
            LoginServiceClient loginService = new LoginServiceClient();
            var time = new TimeSpan(0, 5, 0);
            loginService.Endpoint.Binding.CloseTimeout = time;
            loginService.Endpoint.Binding.OpenTimeout = time;
            loginService.Endpoint.Binding.ReceiveTimeout = time;
            loginService.Endpoint.Binding.SendTimeout = time;
            string hash = CommonUtility.GenerateHash(userId, password);
            if (loginService.LoginUser(hash, ref ret, ref user))
            {
               
                SaveLoginInfo(user.id,user.UserName);
                ret = "success";
            }
            else
            {
                ret = "fail";
            }
            return Json(new { result = ret }, JsonRequestBehavior.AllowGet);

        }
        private void SaveLoginInfo(string id, string userName)
        {
            // actually you need to get user info from table
            LoginInfo.UserId = id.ToString() ;//default assign
            LoginInfo.UserName = userName;

            // Cookie
            LoginInfo.CookieUserId = id.ToString();
        }

        [HttpPost]

        public ActionResult ClearSSO()
        {
            string result = "0";
            string message = "";
            try
            {

                Session.Clear();
                Session.Abandon();
                result = "1";
            }
            catch (Exception ex)
            {
                result = "0";
                message = "Session Expired";
               
            }

            return Json(new { result = result, message = message });
        }
    }
}