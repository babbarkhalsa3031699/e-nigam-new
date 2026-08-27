using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Routing;

namespace CMNirdesh.Models
{
    public class CMNirdeshAuthorize : AuthorizeAttribute
    {
        private string _allow;

        public string Allow
        {
            get
            {
                return _allow ?? String.Empty;
            }
            set
            {
                _allow = value;
                if (!string.IsNullOrWhiteSpace(_allow))
                {

                }
            }
        }

        public static string UserID
        {
            get { return ((HttpContext.Current.Session["UserID"] == null) ? string.Empty : (string)HttpContext.Current.Session["UserID"]); }
            set { HttpContext.Current.Session["UserID"] = value; }
        }

        public static string RoleID
        {
            get { return ((HttpContext.Current.Session["RoleID"] == null) ? string.Empty : (string)HttpContext.Current.Session["RoleID"]); }
            set { HttpContext.Current.Session["RoleID"] = value; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {          
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            if (filterContext.ActionDescriptor.IsDefined(typeof(CMNirdeshAuthorize), true))
            {
                object[] SblAuths = filterContext.ActionDescriptor.GetCustomAttributes(typeof(CMNirdeshAuthorize), true);
                if ((null != SblAuths) && (SblAuths.Count() > 0) && (null != SblAuths[0]))
                {
                    CMNirdeshAuthorize authAttr = SblAuths[0] as CMNirdeshAuthorize;

                    if (0 == string.Compare(authAttr.Allow, "All", true))
                    {
                        return;
                    }

                    if ((0 == string.Compare(authAttr.Allow, "Authenticated", true)) && (filterContext.HttpContext.Request.LogonUserIdentity.IsAuthenticated))
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(CurrentSession.UserID))
                    {
                        if (CurrentSession.AvailableModules.Contains(authAttr.Allow.Trim().ToUpper()))
                        {
                            return;                            
                        }
                    }
                    else
                    {
                       
                       // filterContext.Result = new RedirectResult("~/Account/Login?returnUrl=" + filterContext.HttpContext.Request.Url + "&message=session");
                        return;
                    }
                }
            }
            else
                if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                {
                    return;
                }
                else
                    if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(CMNirdeshAuthorize), true))
                    {
                        object[] SblAuths = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(CMNirdeshAuthorize), true);
                        if ((null != SblAuths) && (SblAuths.Count() > 0) && (null != SblAuths[0]))
                        {
                    CMNirdeshAuthorize authAttr = SblAuths[0] as CMNirdeshAuthorize;

                            if (0 == string.Compare(authAttr.Allow, "All", true))
                            {
                                return;
                            }

                            if ((0 == string.Compare(authAttr.Allow, "Authenticated", true)) && (filterContext.HttpContext.Request.LogonUserIdentity.IsAuthenticated))
                            {
                                if (!string.IsNullOrEmpty(CurrentSession.UserID))
                                {
                                    if (CurrentSession.AvailableModules.Contains(authAttr.Allow.Trim().ToUpper()))
                                    {
                                        return;
                                    }
                                }
                                else
                                {

                                    //filterContext.Result = new JavaScriptResult() {
                                    //    Script = "location.reload();"
                                    //};
                                     filterContext.Result = new RedirectResult("~/error/SessionTimedOut");
                                    return;
                                }
                                return;
                            }

                            if (!string.IsNullOrEmpty(CurrentSession.UserID))
                            {
                                if (CurrentSession.AvailableModules.Contains(authAttr.Allow.Trim().ToUpper()))
                                {
                                    return;                                    
                                }
                        else
                        {
                            return;

                        }
                            }
                            else
                            {
                               // filterContext.Result = new RedirectResult("~/Account/Login?returnUrl=" + filterContext.HttpContext.Request.Url + "&message=session");                                
                                return;
                            }
                        }
                    }

            HandleUnauthorizedRequest(filterContext);
        }

    

 
    }
}