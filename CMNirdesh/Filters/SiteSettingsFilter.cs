

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Filters
{
    public class SiteSettingsFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
          //  var siteSettings = (SiteSettings)Helper.ExecuteService("SiteSetting", "GetAllSiteSettings", new SiteSettings() { });

            filterContext.Controller.ViewBag.CurrentSessionName = "5<sup>th</sup> Session (Feb 2014)";
            filterContext.Controller.ViewBag.CurrentAssemblyName = "12<sup>th</sup> Legislative Assembly";
            filterContext.Controller.ViewBag.CurrentSessionId = "";
            filterContext.Controller.ViewBag.CurrenAssemblyId = "";

           
        } 
    
    }
}