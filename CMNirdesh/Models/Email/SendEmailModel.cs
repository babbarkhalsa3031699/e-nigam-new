using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CMNirdesh.Models.Email
{
    public class SendEmailModel
    {
        public string Sender { get; set; }
        public string RecipientEmail { get; set; }
        public string CC { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }


   public class EmailHelper
    {
        public static bool SendEmail(SendEmailModel model)
        {
            bool isEmailSent = false;

            try
            {
                string sender = string.IsNullOrEmpty(model.Sender)
                    ? ConfigurationManager.AppSettings["SMTP_OTP:DefaultSender"]
                    : model.Sender;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = ConfigurationManager.AppSettings["SMTP_OTP:Host"];
                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_OTP:Port"]);
                    smtp.EnableSsl = false; // IMPORTANT for NIC
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(
                        ConfigurationManager.AppSettings["SMTP_OTP:Username"],
                        ConfigurationManager.AppSettings["SMTP_OTP:Password"]
                    );

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(sender);
                        mail.Subject = model.Subject;
                        mail.Body = model.Body;
                        mail.IsBodyHtml = true;

                        mail.To.Add(model.RecipientEmail);

                        if (!string.IsNullOrEmpty(model.CC))
                            mail.CC.Add(model.CC);

                        if (!string.IsNullOrEmpty(model.Bcc))
                            mail.Bcc.Add(model.Bcc);

                        smtp.Send(mail);
                        isEmailSent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Replace with your logging
                System.IO.File.AppendAllText(
                    System.Web.Hosting.HostingEnvironment.MapPath("~/ErrorLog.txt"),
                    DateTime.Now + " : " + ex.ToString() + Environment.NewLine
                );
            }

            return isEmailSent;
        }
    }

}