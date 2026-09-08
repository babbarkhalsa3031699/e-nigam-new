using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Filters
{
    //public class CheckSessionAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        HttpContext ctx = HttpContext.Current;

    //        // check if session is supported
    //        if (ctx.Session != null)
    //        {
    //            // check if a new session id was generated
    //            if (ctx.Session.IsNewSession)
    //            {
    //                // If it says it is a new session, but an existing cookie exists, then it must
    //                // have timed out
    //                string sessionCookie = ctx.Request.Headers["Cookie"];
    //                if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
    //                {
    //                    if (!string.IsNullOrEmpty(ctx.Request.RawUrl) && ctx.Request.RawUrl != "/Account/Login")  
    //                    {
    //                        ctx.Response.Write("Session Time Out");
    //                        UrlHelper helper = new UrlHelper(filterContext.RequestContext);
    //                        String url = helper.Action("Index", "Home", new { area = "" });
    //                        //String url = ConfigurationManager.AppSettings["publicHost"].ToString();
    //                        ctx.Response.Redirect(url);
    //                        return;  
    //                    }
    //                }
    //            }
    //        }

    //        base.OnActionExecuting(filterContext);
    //    }
    //}
}