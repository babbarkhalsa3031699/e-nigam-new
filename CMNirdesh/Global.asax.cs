using CMNirdesh.App_Start;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CMNirdesh
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Task.Run(async () =>
            {
                try
                {
                    PuppeteerLauncher.BrowserInstance = (Browser)await Puppeteer.LaunchAsync(new LaunchOptions
                    {
                        Headless = true,
                        ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                        Args = new[] { "--no-sandbox", "--disable-gpu" }
                    });

                    System.Diagnostics.Debug.WriteLine("Chrome started successfully!");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Chrome failed: " + ex.Message);
                }

            }).GetAwaiter().GetResult();  // <-- use this instead of .Wait()
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["NoOfVisitors"] = 0;
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            Application.Lock();
            Application["NoOfVisitors"] = (int)Application["NoOfVisitors"] + 1;
            Application.UnLock();
        }

        protected void Application_BeginRequest()
        {
            Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
        }


        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }



    }
}
