using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class ContactUs
    {
        public string PageName { get; set; }
        public string HtmlContent { get; set; }
    }

    public class LGContactUsDataViewModel
    {
        public ContactUs GetContent(string pageName)
        {
            ContactUs content = null;
            using (SqlConnection con = ClsConnection.GetConnection())
            using (var cmd = new SqlCommand("GetCMSContentByPageName", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@PageName", pageName);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        content = new ContactUs
                        {
                            PageName = reader["PageName"].ToString(),
                            HtmlContent = reader["HtmlContent"].ToString()
                        };
                    }
                }
            }
            return content;
        }

        public void SaveOrUpdateContent(ContactUs model)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (var cmd = new SqlCommand("UpsertCMSContent", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageName", model.PageName);
                cmd.Parameters.AddWithValue("@HtmlContent", model.HtmlContent);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }


    public class ContactUsList
    {
        public  ContactUs ContactUs { get; set; }
        public string fileacessingUrl { get; set; }
        public string Message { get; set; }

    }

}
