using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.IO;
using System.Net;



namespace CMNirdesh.Controllers
{
    public class UploadController : Controller
    {
        // GET: SendMailer
        public ActionResult Index()
        {
            return View();
        }




        public ActionResult Fileupload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = "Z:\\myfileshare" + "//" + file.FileName;
                    //string path = "D:\\myfileshare" + "//" + file.FileName;
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }


    }
}