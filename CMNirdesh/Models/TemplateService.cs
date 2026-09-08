using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class TemplateService
    {
        private readonly string _entityId =
            ConfigurationManager.AppSettings["DltEntityId"];

        public string BuildMessage(string templateId, string[] variables)
        {
            string msg = GetTemplateText(templateId);

            foreach (string v in variables)
            {
                int idx = msg.IndexOf("{#var#}");
                if (idx < 0) break;
                msg = msg.Remove(idx, 7).Insert(idx, v);
            }

            // 🔑 DLT REQUIRED FORMAT
            return $"{msg}|{_entityId}|{templateId}";
        }

        private string GetTemplateText(string templateId)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            {
                con.Open();

                SqlParameter[] p =
                {
                new SqlParameter("@TemplateID", templateId)
            };

                DataSet ds = SqlHelper.ExecuteDataset(
                    con,
                    CommandType.StoredProcedure,
                    "GetTemplateText",
                    p
                );

                return ds.Tables[0].Rows[0][0].ToString();
            }
        }

        
    }

}