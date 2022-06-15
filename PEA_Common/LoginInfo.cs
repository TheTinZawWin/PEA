//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;

//namespace PEA_Common
//{
//   public class LoginInfo
//    {
//        public static string UserId
//        {
//            get
//            {
//                return (string)HttpContext.Current.Session[KeyName.UserId];
//            }

//            set
//            {
//                HttpContext.Current.Session[LoginInfo.KeyName.UserId] = value;
//            }
//        }


//        public static string UserName
//        {
//            get
//            {
//                return (string)HttpContext.Current.Session[KeyName.UserName];
//            }

//            set
//            {
//                HttpContext.Current.Session[LoginInfo.KeyName.UserName] = value;
//            }
//        }


//        public static string CookieUserId
//        {
//            get
//            {
               
//                var cookieUserInfo = HttpContext.Current.Request.Cookies.Get(LoginInfo.KeyName.LoginInfo);
//                string cookieDecriptedUserId = string.Empty;
//                if (cookieUserInfo != null)
//                {
//                    string cookieEncryptedUserId = cookieUserInfo[LoginInfo.KeyName.UserId];
//                    try
//                    {
//                        cookieDecriptedUserId = CommonUtility.DecryptString(cookieEncryptedUserId, "GetSiteAccessIP");
//                    }
//                    catch (Exception ex)
//                    {
                       
//                        System.Diagnostics.Debug.WriteLine(ex.ToString());
//                    }
//                }

//                return cookieDecriptedUserId;
//            }

//            set
//            {
               
//                var encryptUserId = CommonUtility.EncryptString(value, "GetSiteAccessIP");
//                var cookie = new HttpCookie(LoginInfo.KeyName.LoginInfo);
//                cookie.Values[LoginInfo.KeyName.UserId] = encryptUserId;
//                HttpContext.Current.Response.Cookies.Add(cookie);
//            }
//        }


//        public class KeyName
//        {

//            public const string LoginInfo = "LoginInfo";

//            public const string UserId = "UserId";


//            public const string UserName = "UserName";


//        }

//    }
//}
