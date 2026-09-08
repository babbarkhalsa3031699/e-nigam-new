
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using CMNirdesh.Utility;
//using CMNirdesh.Helpers;

namespace CMNirdesh.Models
{
    public class AuditAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ////Stores the Request in an Accessible object
            var request = filterContext.HttpContext.Request;


            //Generate an audit
          
            //Stores the Audit in the Database
           
            //Finishes executing the Action as normal 
            base.OnActionExecuting(filterContext);
        }
    }
}