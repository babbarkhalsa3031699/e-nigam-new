using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBL.eLegistrator.HouseController.Web.App_Code
{
    public class Upload : IHttpHandler {

   

    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/plain";

        context.Response.Expires = -1;

        try

        {

            HttpPostedFile postedFile = context.Request.Files["Filedata"];

            string url = "~/TempData";
            string directory = Server.MapPath(url);
if (Directory.Exists(directory))
                {
                    System.IO.Directory.Delete(directory, true);
                }
           

          

            string filename = postedFile.FileName;

            if (!Directory.Exists(directory))

                Directory.CreateDirectory(directory);

 

            postedFile.SaveAs(directory + @"\" + filename);

            context.Response.Write(directory + "/" + filename);

            context.Response.StatusCode = 200;

        }

        catch (Exception ex)

        {

            context.Response.Write("Error: " + ex.Message);

        }

    }

 

    public bool IsReusable {

        get {

            return false;

        }

    }

}
}
