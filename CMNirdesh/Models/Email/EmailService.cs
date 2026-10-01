using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;

namespace CMNirdesh.Models.Email
{
    public class EmailService
    {
        #region Configuration

        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly string _smtpFrom;

        #endregion


        #region Constructor

        public EmailService()
        {
            _smtpHost =
                WebConfigurationManager.AppSettings["SmtpHost"];

            string port =
                WebConfigurationManager.AppSettings["SmtpPort"];

            _smtpUser =
                WebConfigurationManager.AppSettings["SmtpUser"];

            _smtpPassword =
                WebConfigurationManager.AppSettings["SmtpPassword"];

            _smtpFrom =
                WebConfigurationManager.AppSettings["SmtpFrom"];


            if (string.IsNullOrWhiteSpace(_smtpHost))
                throw new Exception(
                    "AppSettings 'SmtpHost' is missing.");

            if (string.IsNullOrWhiteSpace(port))
                throw new Exception(
                    "AppSettings 'SmtpPort' is missing.");

            if (!int.TryParse(
                port,
                out _smtpPort))
            {
                throw new Exception(
                    "AppSettings 'SmtpPort' is invalid.");
            }

            if (string.IsNullOrWhiteSpace(_smtpUser))
                throw new Exception(
                    "AppSettings 'SmtpUser' is missing.");

            if (string.IsNullOrWhiteSpace(_smtpPassword))
                throw new Exception(
                    "AppSettings 'SmtpPassword' is missing.");

            if (string.IsNullOrWhiteSpace(_smtpFrom))
                throw new Exception(
                    "AppSettings 'SmtpFrom' is missing.");
        }

        #endregion


        #region Send Single Email

        public EmailResult Send(
            string toEmail,
            string subject,
            string body,
            string createdBy = null,
            string ipAddress = null)
        {
            return SendEmail(
                new List<string>
                {
                    toEmail
                },
                subject,
                body,
                null,
                null,
                createdBy,
                ipAddress);
        }

        #endregion


        #region Send Email With Attachments

        public EmailResult SendEmail(
            List<string> emails,
            string subject,
            string body,
            string azureFileUrl = null,
            string coveringLetter = null,
            string createdBy = null,
            string ipAddress = null)
        {
            EmailResult result =
                new EmailResult
                {
                    TrackingId = Guid.NewGuid(),
                    Subject = subject,
                    ToEmail = emails != null
                        ? string.Join(",", emails)
                        : string.Empty
                };


            if (emails == null || !emails.Any())
            {
                result.Success = false;
                result.Status = "Failed";
                result.Message =
                    "Recipient email list is empty.";

                return result;
            }


            if (string.IsNullOrWhiteSpace(subject))
            {
                result.Success = false;
                result.Status = "Failed";
                result.Message =
                    "Email subject is required.";

                return result;
            }


            if (string.IsNullOrWhiteSpace(body))
            {
                result.Success = false;
                result.Status = "Failed";
                result.Message =
                    "Email body is required.";

                return result;
            }


            try
            {
                List<string> recipients =
                    GetValidRecipients(emails);


                if (recipients.Count == 0)
                {
                    throw new Exception(
                        "No valid recipient email address found.");
                }


                // Download all attachments once.
                List<EmailAttachment> attachments =
                    DownloadAttachments(
                        azureFileUrl,
                        coveringLetter);


                // NIC SMTP requires TLS 1.2.
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12;


                // Reuse one SMTP connection.
                using (SmtpClient smtp =
                       CreateSmtpClient())
                {
                    foreach (string recipient
                             in recipients)
                    {
                        Guid trackingId =
                            Guid.NewGuid();


                        try
                        {
                            using (MailMessage mail =
                                   CreateMailMessage(
                                       recipient,
                                       subject,
                                       body,
                                       attachments))
                            {
                                smtp.Send(mail);
                            }


                            SaveHistory(
                                trackingId,
                                recipient,
                                subject,
                                body,
                                "Success",
                                "Email sent successfully.",
                                null,
                                createdBy,
                                ipAddress);
                        }
                        catch (SmtpException ex)
                        {
                            SaveHistory(
                                trackingId,
                                recipient,
                                subject,
                                body,
                                "Failed",
                                null,
                                ex.ToString(),
                                createdBy,
                                ipAddress);
                        }
                        catch (Exception ex)
                        {
                            SaveHistory(
                                trackingId,
                                recipient,
                                subject,
                                body,
                                "Failed",
                                null,
                                ex.ToString(),
                                createdBy,
                                ipAddress);
                        }
                    }
                }


                result.Success = true;
                result.Status = "Success";
                result.Message =
                    "Email processing completed.";


                return result;
            }
            catch (SmtpException ex)
            {
                result.Success = false;
                result.Status = "Failed";
                result.Message =
                    "SMTP Error: " +
                    ex.Message;


                SaveHistory(
                    result.TrackingId,
                    result.ToEmail,
                    subject,
                    body,
                    "Failed",
                    null,
                    ex.ToString(),
                    createdBy,
                    ipAddress);


                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Status = "Failed";
                result.Message =
                    "Error sending email: " +
                    ex.Message;


                SaveHistory(
                    result.TrackingId,
                    result.ToEmail,
                    subject,
                    body,
                    "Failed",
                    null,
                    ex.ToString(),
                    createdBy,
                    ipAddress);


                return result;
            }
        }

        #endregion


        #region Bulk Email

        public BulkEmailResult SendBulk(
            IEnumerable<string> recipients,
            string subject,
            string body,
            string azureFileUrl = null,
            string coveringLetter = null,
            string createdBy = null,
            string ipAddress = null)
        {
            BulkEmailResult result =
                new BulkEmailResult
                {
                    BatchTrackingId =
                        Guid.NewGuid()
                };


            if (recipients == null)
            {
                result.Success = false;
                result.Message =
                    "Recipient list is empty.";

                return result;
            }


            List<string> emailList =
                GetValidRecipients(
                    recipients);


            if (emailList.Count == 0)
            {
                result.Success = false;
                result.Message =
                    "No valid recipient email address found.";

                return result;
            }


            if (string.IsNullOrWhiteSpace(subject))
            {
                result.Success = false;
                result.Message =
                    "Email subject is required.";

                return result;
            }


            if (string.IsNullOrWhiteSpace(body))
            {
                result.Success = false;
                result.Message =
                    "Email body is required.";

                return result;
            }


            result.Total =
                emailList.Count;


            try
            {
                // Download attachments only once.
                List<EmailAttachment> attachments =
                    DownloadAttachments(
                        azureFileUrl,
                        coveringLetter);


                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12;


                // One SMTP connection for the entire batch.
                using (SmtpClient smtp =
                       CreateSmtpClient())
                {
                    foreach (string recipient
                             in emailList)
                    {
                        Guid trackingId =
                            Guid.NewGuid();


                        try
                        {
                            using (MailMessage mail =
                                   CreateMailMessage(
                                       recipient,
                                       subject,
                                       body,
                                       attachments))
                            {
                                smtp.Send(mail);
                            }


                            result.SuccessCount++;


                            SaveHistory(
                                trackingId,
                                recipient,
                                subject,
                                body,
                                "Success",
                                "Email sent successfully.",
                                null,
                                createdBy,
                                ipAddress);
                        }
                        catch (SmtpException ex)
                        {
                            result.FailedCount++;


                            SaveHistory(
                                trackingId,
                                recipient,
                                subject,
                                body,
                                "Failed",
                                null,
                                ex.ToString(),
                                createdBy,
                                ipAddress);
                        }
                        catch (Exception ex)
                        {
                            result.FailedCount++;


                            SaveHistory(
                                trackingId,
                                recipient,
                                subject,
                                body,
                                "Failed",
                                null,
                                ex.ToString(),
                                createdBy,
                                ipAddress);
                        }
                    }
                }


                result.Success =
                    result.FailedCount == 0;


                result.Message =
                    string.Format(
                        "Bulk email completed. Total: {0}, Success: {1}, Failed: {2}.",
                        result.Total,
                        result.SuccessCount,
                        result.FailedCount);


                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;

                result.Message =
                    "Bulk email error: " +
                    ex.Message;


                return result;
            }
        }

        #endregion


        #region SMTP

        private SmtpClient CreateSmtpClient()
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12;


            SmtpClient smtp =
                new SmtpClient(
                    _smtpHost,
                    _smtpPort);


            smtp.UseDefaultCredentials =
                false;


            smtp.Credentials =
                new NetworkCredential(
                    _smtpUser,
                    _smtpPassword);


            smtp.EnableSsl = true;


            smtp.DeliveryMethod =
                SmtpDeliveryMethod.Network;


            smtp.Timeout =
                60000;


            return smtp;
        }

        #endregion


        #region Mail Message

        private MailMessage CreateMailMessage(
            string recipient,
            string subject,
            string body,
            List<EmailAttachment> attachments)
        {
            MailMessage mail =
                new MailMessage();


            mail.From =
                new MailAddress(
                    _smtpFrom,
                    "Digital MC House");


            mail.To.Add(
                new MailAddress(
                    recipient));


            mail.Subject =
                subject;


            mail.SubjectEncoding =
                Encoding.UTF8;


            mail.BodyEncoding =
                Encoding.UTF8;


            mail.IsBodyHtml =
                true;


            mail.Body =
                BuildProfessionalHtmlBody(
                    body);


            // Add attachments.
            if (attachments != null)
            {
                foreach (EmailAttachment attachment
                         in attachments)
                {
                    if (attachment == null)
                        continue;


                    if (attachment.FileBytes == null)
                        continue;


                    if (attachment.FileBytes.Length == 0)
                        continue;


                    MemoryStream stream =
                        new MemoryStream(
                            attachment.FileBytes);


                    mail.Attachments.Add(
                        new Attachment(
                            stream,
                            attachment.FileName));
                }
            }


            return mail;
        }

        #endregion


        #region Download Attachments

        private List<EmailAttachment> DownloadAttachments(
            string azureFileUrl,
            string coveringLetter)
        {
            List<EmailAttachment> attachments =
                new List<EmailAttachment>();


            // ------------------------------------------------------
            // Main Azure file
            // ------------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                azureFileUrl))
            {
                EmailAttachment attachment =
                    DownloadFile(
                        azureFileUrl);


                if (attachment != null)
                {
                    attachments.Add(
                        attachment);
                }
            }


            // ------------------------------------------------------
            // Covering letters
            // ------------------------------------------------------

            if (!string.IsNullOrWhiteSpace(
                coveringLetter))
            {
                string[] letters =
                    coveringLetter.Split(
                        new[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);


                foreach (string letter
                         in letters)
                {
                    string url =
                        letter.Trim();


                    if (string.IsNullOrWhiteSpace(url))
                        continue;


                    EmailAttachment attachment =
                        DownloadFile(url);


                    if (attachment != null)
                    {
                        attachments.Add(
                            attachment);
                    }
                }
            }


            return attachments;
        }


        private EmailAttachment DownloadFile(
            string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;


            try
            {
                using (WebClient client =
                       new WebClient())
                {
                    byte[] fileBytes =
                        client.DownloadData(
                            url);


                    Uri uri =
                        new Uri(url);


                    string fileName =
                        Path.GetFileName(
                            uri.LocalPath);


                    if (string.IsNullOrWhiteSpace(
                        fileName))
                    {
                        fileName =
                            "Attachment";
                    }


                    return new EmailAttachment
                    {
                        FileName = fileName,
                        FileBytes = fileBytes
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Unable to download email attachment: " +
                    url +
                    ". " +
                    ex.Message,
                    ex);
            }
        }

        #endregion


        #region Recipients

        private List<string> GetValidRecipients(
            IEnumerable<string> emails)
        {
            List<string> result =
                new List<string>();


            if (emails == null)
                return result;


            foreach (string email
                     in emails)
            {
                if (string.IsNullOrWhiteSpace(email))
                    continue;


                string value =
                    email.Trim();


                if (!IsValidEmail(value))
                    continue;


                if (!result.Contains(
                    value,
                    StringComparer.OrdinalIgnoreCase))
                {
                    result.Add(value);
                }
            }


            return result;
        }


        private bool IsValidEmail(
            string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;


            try
            {
                MailAddress address =
                    new MailAddress(email);


                return string.Equals(
                    address.Address,
                    email,
                    StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        #endregion


        #region Professional HTML

        private string BuildProfessionalHtmlBody(
            string body)
        {
            StringBuilder html =
                new StringBuilder();


            html.Append(@"
<!DOCTYPE html>
<html>
<head>
<meta charset='UTF-8'>
<meta name='viewport'
      content='width=device-width,initial-scale=1.0'>
<title>Digital MC House</title>
</head>

<body style='margin:0;
padding:0;
background:#f4f6f8;
font-family:Arial,Helvetica,sans-serif;'>

<table width='100%'
       cellpadding='0'
       cellspacing='0'
       border='0'
       style='background:#f4f6f8;
              padding:30px 0;'>

<tr>
<td align='center'>

<table width='650'
       cellpadding='0'
       cellspacing='0'
       border='0'
       style='max-width:650px;
              width:100%;
              background:#ffffff;
              border:1px solid #e1e5e9;'>

<!-- HEADER -->

<tr>
<td style='padding:22px 30px;
           border-bottom:1px solid #eeeeee;'>

<table width='100%'
       cellpadding='0'
       cellspacing='0'
       border='0'>

<tr>

<td style='font-size:20px;
           font-weight:bold;
           color:#222222;'>

Digital MC House

</td>

<td align='right'
    style='font-size:10px;
           color:#888888;
           letter-spacing:1px;'>

SYSTEM GENERATED

</td>

</tr>

</table>

</td>
</tr>


<!-- BODY -->

<tr>

<td style='padding:35px 30px;
           font-size:15px;
           line-height:1.7;
           color:#333333;'>

");


            html.Append(body);


            html.Append(@"

</td>

</tr>


<!-- FOOTER -->

<tr>

<td style='padding:20px 30px;
           background:#f8f9fa;
           border-top:1px solid #eeeeee;
           font-size:12px;
           line-height:1.6;
           color:#777777;'>

<strong>Note:</strong>
This is a system-generated email.
Please do not reply to this message.

<br><br>

Regards,<br>
<strong>Digital MC House</strong>

</td>

</tr>

</table>

</td>
</tr>

</table>

</body>
</html>
");


            return html.ToString();
        }

        #endregion


        #region Save History

        private void SaveHistory(
            Guid trackingId,
            string toEmail,
            string subject,
            string body,
            string status,
            string smtpResponse,
            string errorMessage,
            string createdBy,
            string ipAddress)
        {
            try
            {
                const string sql = @"
INSERT INTO dbo.EmailHistory
(
    TrackingId,
    FromEmail,
    FromName,
    ToEmail,
    Subject,
    Body,
    Status,
    SmtpResponse,
    ErrorMessage,
    SentDate,
    CreatedDate,
    CreatedBy,
    IPAddress
)
VALUES
(
    @TrackingId,
    @FromEmail,
    @FromName,
    @ToEmail,
    @Subject,
    @Body,
    @Status,
    @SmtpResponse,
    @ErrorMessage,
    @SentDate,
    GETDATE(),
    @CreatedBy,
    @IPAddress
);";


                using (SqlConnection connection =
                       ClsConnection.GetConnection())
                using (SqlCommand command =
                       new SqlCommand(
                           sql,
                           connection))
                {
                    command.Parameters.Add(
                        "@TrackingId",
                        SqlDbType.UniqueIdentifier)
                        .Value =
                            trackingId;


                    command.Parameters.Add(
                        "@FromEmail",
                        SqlDbType.NVarChar,
                        320)
                        .Value =
                            _smtpFrom;


                    command.Parameters.Add(
                        "@FromName",
                        SqlDbType.NVarChar,
                        200)
                        .Value =
                            "Digital MC House";


                    command.Parameters.Add(
                        "@ToEmail",
                        SqlDbType.NVarChar,
                        320)
                        .Value =
                            toEmail;


                    command.Parameters.Add(
                        "@Subject",
                        SqlDbType.NVarChar,
                        500)
                        .Value =
                            subject;


                    command.Parameters.Add(
                        "@Body",
                        SqlDbType.NVarChar)
                        .Value =
                            (object)body ??
                            DBNull.Value;


                    command.Parameters.Add(
                        "@Status",
                        SqlDbType.NVarChar,
                        20)
                        .Value =
                            status;


                    command.Parameters.Add(
                        "@SmtpResponse",
                        SqlDbType.NVarChar,
                        2000)
                        .Value =
                            (object)smtpResponse ??
                            DBNull.Value;


                    command.Parameters.Add(
                        "@ErrorMessage",
                        SqlDbType.NVarChar,
                        4000)
                        .Value =
                            (object)Truncate(
                                errorMessage,
                                4000)
                            ?? DBNull.Value;


                    command.Parameters.Add(
                        "@SentDate",
                        SqlDbType.DateTime)
                        .Value =
                            status == "Success"
                                ? (object)DateTime.Now
                                : DBNull.Value;


                    command.Parameters.Add(
                        "@CreatedBy",
                        SqlDbType.NVarChar,
                        200)
                        .Value =
                            (object)createdBy ??
                            DBNull.Value;


                    command.Parameters.Add(
                        "@IPAddress",
                        SqlDbType.NVarChar,
                        50)
                        .Value =
                            (object)ipAddress ??
                            DBNull.Value;


                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                // Email history failure must not
                // cause the email itself to fail.
            }
        }

        #endregion


        #region Utility

        private string Truncate(
            string value,
            int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;


            if (value.Length <= maxLength)
                return value;


            return value.Substring(
                0,
                maxLength);
        }

        #endregion
    }


    // ================================================================
    // EMAIL ATTACHMENT
    // ================================================================

    public class EmailAttachment
    {
        public string FileName { get; set; }

        public byte[] FileBytes { get; set; }
    }


    // ================================================================
    // EMAIL RESULT
    // ================================================================

    public class EmailResult
    {
        public bool Success { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }

        public Guid TrackingId { get; set; }

        public string ToEmail { get; set; }

        public string Subject { get; set; }
    }


    // ================================================================
    // BULK RESULT
    // ================================================================

    public class BulkEmailResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public Guid BatchTrackingId { get; set; }

        public int Total { get; set; }

        public int SuccessCount { get; set; }

        public int FailedCount { get; set; }
    }
}