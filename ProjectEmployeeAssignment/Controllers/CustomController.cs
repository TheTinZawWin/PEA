using PEA_Common;
using ProjectEmployeeAssignment.LoginService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static PEA_Common.CommonUtility;

namespace ProjectEmployeeAssignment.Controllers
{
    public class CustomController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //set site default culture
            this.SetCurrentCulture();
            //Remove cache            
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.AddHeader("Pragma", "no-cache");



            base.OnActionExecuting(filterContext);

            //saveoperationlog
            this.WriteOperationLog(filterContext);
            // Check the session if it is not the login screen
            if (!filterContext.ActionDescriptor.ActionName.Equals("Login"))
            {
                //If the check is caught, the login screen is displayed.
                if (!this.ValidateSessionUser())
                {
                    string rawUrl = this.Request.RawUrl;
                    string toUrl = Url.Content("~/Login/Login");
                    filterContext.Result = this.Redirect(toUrl);
                }
            }
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            filterContext.Controller.ViewData[ViewDataParam.ErrorMessage] = string.Empty;
            var model = filterContext.Controller.ViewData.Model;
            if (model != null)
            {
                var modelState = filterContext.Controller.ViewData.ModelState;
                if (!modelState.IsValid)
                {
                    StringBuilder sb = new StringBuilder();
                    if (modelState[string.Empty] != null)
                    {
                        foreach (var mes in modelState[string.Empty].Errors)
                        {
                            if (sb.Length > 0)
                            {
                                sb.AppendLine();
                            }

                            sb.Append(mes.ErrorMessage);
                        }
                    }

                    var modelErrors = modelState.Where(x => !string.IsNullOrWhiteSpace(x.Key)).Select(x => x.Value);
                    var contextErrorItems = modelErrors.Where(x => x.Errors.Count > 0).ToList();
                    foreach (var c in contextErrorItems)
                    {
                        for (int i = 0; i < c.Errors.Count; i++)
                        {
                            if (sb.Length > 0)
                            {
                                sb.AppendLine();
                            }

                            sb.Append(string.Format("ModelError:[{0}]", c.Errors[i].ErrorMessage));
                        }
                    }

                    filterContext.Controller.ViewData[ViewDataParam.ErrorMessage] = sb.ToString();
                }
            }

            // サーバロジックの処理終了としてログを出力する
            //  BusinessLogger.Default.Write(string.Format("OnActionExecuted [{0}]", filterContext.ActionDescriptor.ActionName));
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            string message = filterContext.Exception.Message;
            var cEx = filterContext.Exception as CustomException;
            if (cEx != null)
            {
                switch (cEx.ErrorType)
                {
                    case CustomException.CustomExceptionType.SeesionTimeOut:
                        message = "Session Timeout";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                message = filterContext.Exception.ToString();
            }

            this.Session.Add("ErrorMessage", message);

            // BusinessLogger.Default.Write("OnException [{0}]", filterContext.Exception.ToString());
            this.WriteOperationLog(filterContext);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                base.OnException(filterContext);
            }
            else
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = this.RedirectToAction("SystemError", "Error");
            }
        }
        private void SetCurrentCulture()
        {
            string cultureInfo = "en-US";
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(cultureInfo);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureInfo);

        }

        private void WriteOperationLog(ControllerContext controllerContext)
        {
            string calledAction = string.Empty;
            string actionParameter = string.Empty;
            string calledScreen = string.Empty;
            string user = LoginInfo.UserId;
            string cookieUser = LoginInfo.CookieUserId;
            string remoteIPAddress = controllerContext.RequestContext.HttpContext.Request.UserHostAddress;
            string remoteHostName = "Unknown";

            try
            {
                IPHostEntry iphe = Dns.GetHostEntry(remoteIPAddress);
                remoteHostName = iphe.HostName;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.ToString());
            }

          
            var actionExecutingContext = controllerContext as ActionExecutingContext;
            var exceptionContext = controllerContext as ExceptionContext;

            if (actionExecutingContext != null)
            {
                calledAction = actionExecutingContext.ActionDescriptor.ActionName;
                foreach (var keyValuePair in actionExecutingContext.ActionParameters)
                {
                    if (!string.IsNullOrWhiteSpace(actionParameter))
                    {
                        actionParameter += ", ";
                    }

                    string value = string.Empty;
                    if (keyValuePair.Value == null)
                    {
                        value = "null";
                    }
                    else
                    {
                        value = keyValuePair.Value.ToString();
                    }

                    actionParameter += string.Format("{0}={1}", keyValuePair.Key, value);
                }
            }
            else if (exceptionContext != null)
            {
                calledAction = exceptionContext.HttpContext.Request.CurrentExecutionFilePath;
                foreach (string key in exceptionContext.HttpContext.Request.QueryString.Keys)
                {
                    if (!string.IsNullOrWhiteSpace(actionParameter))
                    {
                        actionParameter += ", ";
                    }

                    string value = string.Empty;
                    if (exceptionContext.HttpContext.Request.QueryString[key] == null)
                    {
                        value = "null";
                    }
                    else
                    {
                        value = exceptionContext.HttpContext.Request.QueryString[key].ToString();
                    }

                    actionParameter += string.Format("{0}={1}", key, value);
                }

                actionParameter += Environment.NewLine;
                actionParameter += exceptionContext.Exception.ToString();
            }

            System.Diagnostics.Debug.WriteLine(
                string.Format(
                    "UserId:[{0}] UserIdCookie:[{1}] RemoteHostIP:[{2}] RemoteHostName:[{3}] ScreenPath:[{4}] ActionName:[{5}] Parameter:[{6}]",
                    user,
                    cookieUser,
                    remoteIPAddress,
                    remoteHostName,
                    calledScreen,
                    calledAction,
                    actionParameter));
            LoginServiceClient loginService = new LoginServiceClient();
            var time = new TimeSpan(0, 5, 0);
            loginService.Endpoint.Binding.CloseTimeout = time;
            loginService.Endpoint.Binding.OpenTimeout = time;
            loginService.Endpoint.Binding.ReceiveTimeout = time;
            loginService.Endpoint.Binding.SendTimeout = time;
            OperationModel operation = new OperationModel();
            operation.OperationDatetime = DateTime.Now;
            operation.UserId = user;
            operation.CookieUserId = cookieUser;
            operation.Url = calledScreen;
            operation.ActionName = calledAction;
            operation.ActionParameter = actionParameter;
            operation.RemoteAddress = remoteIPAddress;
            operation.RemoteHost = remoteHostName;

            operation.CreatedAt = DateTime.Now;
            operation.CreatedBy = LoginInfo.UserId;
            string ret = string.Empty;
            loginService.SaveOperationLog(operation, ref ret);
           
            // you need to add add in table or log file

            //var m = new BaseTB_OperationLog();
            //m.OperationDatetime = DateTime.Now;
            //m.UserId = user;
            //m.CookieUserId = cookieUser;
            //m.Url = calledScreen;
            //m.ActionName = calledAction;
            //m.ActionParameter = actionParameter;
            //m.RemoteAddress = remoteIPAddress;
            //m.RemoteHost = remoteHostName;

            //m.CreatedAt = DateTime.Now;
            //m.CreatedBy = LoginInfo.UserId;
            //m.CreatedByFunction = "OperationLog";
            //using (var con = DataBase.GetConnection())
            //using (var tran = DataBase.GetLocalTransaction(con))
            //{
            //    m.DataInsert(con, tran);
            //    tran.Commit();
            //}
        }
        protected bool ValidateSessionUser()
        {
            string sessionUserId = LoginInfo.UserId;
            string cookieUserId = LoginInfo.CookieUserId;

            if ((string.IsNullOrWhiteSpace(sessionUserId) && string.IsNullOrWhiteSpace(cookieUserId)) ||
                sessionUserId != cookieUserId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}