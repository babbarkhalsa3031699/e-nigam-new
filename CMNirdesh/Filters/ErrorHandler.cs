using Elmah;
using System;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Filters
{
     [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Method,
        Inherited = true,
        AllowMultiple = true)]
    public class ElmahHTTPErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //if (context.Exception != null)
            //{
            //    //Elmah.ErrorSignal.FromCurrentContext().Raise(context.Exception);
            //    ErrorLog.WriteToLog(context.Exception, "");
            //}
            //base.OnException(context);
            base.OnException(context);
            var e = context.Exception;
            var httpcontext = HttpContext.Current;
            string url = httpcontext.Request.Url.AbsoluteUri;
            var signal = ErrorSignal.FromContext(httpcontext);
            if (signal != null)
            {
                
                signal.Raise(e, httpcontext);

            }
        }
    }

    public class ElmahMVCErrorAttribute : System.Web.Mvc.IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(context.Exception);]
                // ErrorLog.WriteToLog(context.Exception, "");
            }
        }
    }
}