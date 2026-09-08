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
    public class SendMailerController : Controller
    {
        // GET: SendMailer
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Index(CMNirdesh.Models.Mail _objModelMail, HttpPostedFileBase fileUploader)
        {
            if (ModelState.IsValid)
            {
              
               string Enpath1 = System.Web.Configuration.WebConfigurationManager.AppSettings["Site"];
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient();
                smtpServer.Port = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["emailport"]);
                smtpServer.Host = System.Web.Configuration.WebConfigurationManager.AppSettings["emailhost"];
                smtpServer.Credentials = new System.Net.NetworkCredential(System.Web.Configuration.WebConfigurationManager.AppSettings["emailuser"], System.Web.Configuration.WebConfigurationManager.AppSettings["emailpwd"]);
                mail.From = new MailAddress("no-reply.pmidc@punjab.gov.in");
                mail.To.Add(_objModelMail.To);
                mail.Subject = _objModelMail.Subject;
                mail.IsBodyHtml = false;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                smtpServer.EnableSsl = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["enablessl"]);
                string Body = _objModelMail.Body;
                if (fileUploader != null)
                {
                    string fileName = Path.GetFileName(fileUploader.FileName);
                    mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                }

                mail.Body = Body;
                smtpServer.Send(mail);
                ViewBag.Message = "Sent";
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }





    }
}