using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.Configuration;
using System;
using System.Linq;
using System.Data.SqlClient;
using CMNirdesh.Models;
using System.Data;

public  class EmailHelper
{
    public  void SendActionNotification(string refIds, string commonAction)
    {
        var data = GetRefWiseEmails(refIds);

        if (data == null || !data.Any())
            return;

        foreach (var item in data)
        {
            try
            {
                string subject = "LG Observation Raised";

                string body = $@"
                <p><b>LG Observation Raised</b></p>
                <p><b>RefId:</b> {item.RefId}</p>
                <p><b>Action:</b> {item.ActionDescription ?? commonAction ?? "N/A"}</p>
                ";

                SendEmailSingle(item.Email, subject, body);
            }
            catch (Exception ex)
            {
                // TODO: log per RefId
            }
        }
    }

    // 🔹 Get RefId-wise emails from DB
    private static List<RefEmailModel> GetRefWiseEmails(string refIds)
    {
        List<RefEmailModel> list = new List<RefEmailModel>();

        SqlConnection connection = ClsConnection.GetConnection();

        try
        {
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                connection.Open();

            using (SqlCommand cmd = new SqlCommand("GetRefWiseEmails", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RefIds", refIds);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new RefEmailModel
                        {
                            RefId = reader["RefId"].ToString(),
                            Email = reader["Email"].ToString(),
                            ActionDescription = reader["ActionDescription"]?.ToString()
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // TODO: log error
        }
        finally
        {
            connection.Close();
        }

        return list;
    }

    // 🔹 Send single email
    private static void SendEmailSingle(string email, string subject, string body)
    {
        string emailUser = WebConfigurationManager.AppSettings["emailuser"];
        string emailPwd = WebConfigurationManager.AppSettings["emailpwd"];
        string emailHost = WebConfigurationManager.AppSettings["emailhost"];
        int emailPort = Convert.ToInt32(WebConfigurationManager.AppSettings["emailport"]);
        bool enableSsl = Convert.ToBoolean(WebConfigurationManager.AppSettings["enablessl"]);

        using (MailMessage mail = new MailMessage())
        {
            mail.From = new MailAddress(emailUser);
            mail.To.Add(email);

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (SmtpClient smtp = new SmtpClient(emailHost, emailPort))
            {
                smtp.Credentials = new NetworkCredential(emailUser, emailPwd);
                smtp.EnableSsl = enableSsl;

                smtp.Send(mail);
            }
        }
    }
}


public class RefEmailModel
{
    public string RefId { get; set; }
    public string Email { get; set; }
    public string ActionDescription { get; set; }
}