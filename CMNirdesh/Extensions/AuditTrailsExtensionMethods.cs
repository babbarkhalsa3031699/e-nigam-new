
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMNirdesh.Models;
using CMNirdesh.Models.AuditTrail;

namespace CMNirdesh.Extensions
{
    public static class AuditTrailsExtensionMethods
    {
        #region Audit Trail Helper Methods (Login Audit Extensions).
        public static bool FillAuditTrail()
        {
            var request = HttpContext.Current.Request;
           Audit audit = new Audit()
            {
                UserName = (request.IsAuthenticated) ? HttpContext.Current.User.Identity.Name : "Anonymous",
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                LoginDateTime = CurrentSession.LoginTime,
                LoginStatus = CurrentSession.LoginStatus,
                //LogoutDateTime = Convert.ToDateTime("1753-01-01 00:00:00"),
                ModuleName = "Login",
                ActionName = "User Login",
                ActionType = HttpContext.Current.Request.HttpMethod,
                URL = request.RawUrl,
                ActionDate = Convert.ToDateTime(DateTime.Now)
            };

            //Stores the Audit in the Database
            Helper.ExecuteService("AuditTrail", "AddAudit", audit);
            return true;
        }

        //Audit Trail Logout
        public static bool FillAuditTrailLogOut()
        {
            var request = HttpContext.Current.Request;
           Audit audit = new Audit()
            {
                UserName = (request.IsAuthenticated) ? HttpContext.Current.User.Identity.Name : "Anonymous",
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                LoginDateTime = CurrentSession.LoginTime,
                LoginStatus = CurrentSession.LoginStatus,
                //LogoutDateTime = DateTime.Now,
                ModuleName = "LogOff",
                ActionName = "User LogOff",
                ActionType = HttpContext.Current.Request.HttpMethod,
                URL = request.RawUrl,
                ActionDate = Convert.ToDateTime(DateTime.Now)
            };

            //Stores the Audit in the Database
            Helper.ExecuteService("AuditTrail", "AddAudit", audit);
            return true;
        }

        //Audit Trail Logout after change password
        public static bool FillAuditTrailChangePasswordLogOut()
        {
            var request = HttpContext.Current.Request;
            Audit audit = new Audit()
            {
                UserName = (request.IsAuthenticated) ? HttpContext.Current.User.Identity.Name : "Anonymous",
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                LoginDateTime = CurrentSession.LoginTime,
                LoginStatus = CurrentSession.LoginStatus,
                //LogoutDateTime = DateTime.Now,
                ModuleName = "Change Password Log Off",
                ActionName = "Log Off On Change Password",
                ActionType = HttpContext.Current.Request.HttpMethod,
                URL = request.RawUrl,
                ActionDate = Convert.ToDateTime(DateTime.Now)
            };

            //Stores the Audit in the Database
            Helper.ExecuteService("AuditTrail", "AddAudit", audit);
            return true;
        }


        //Audit Trail after change password
        public static bool FillAuditTrailChangePassword()
        {
            var request = HttpContext.Current.Request;
           Audit audit = new Audit()
            {
                UserName = (request.IsAuthenticated) ? HttpContext.Current.User.Identity.Name : "Anonymous",
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                LoginDateTime = CurrentSession.LoginTime,
                LoginStatus = CurrentSession.LoginStatus,
                //LogoutDateTime = Convert.ToDateTime("1753-01-01 00:00:00"),
                ModuleName = "Change Password",
                ActionName = "Change Password",
                ActionType = HttpContext.Current.Request.HttpMethod,
                URL = request.RawUrl,
                ActionDate = Convert.ToDateTime(DateTime.Now)
            };

            //Stores the Audit in the Database
            Helper.ExecuteService("AuditTrail", "AddAudit", audit);
            return true;
        }
        #endregion
    }
}