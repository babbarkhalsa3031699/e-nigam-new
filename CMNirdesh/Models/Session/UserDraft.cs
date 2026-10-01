using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models.Session
{
    public class UserDraft
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string DraftKey { get; set; }

        [AllowHtml]
        public string DraftContent { get; set; }

       

        public bool SaveDraft()
        {
            if(string.IsNullOrWhiteSpace(DraftContent))
            {
                return false;
            }
            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SaveDraft", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@DraftKey", DraftKey);
                    cmd.Parameters.AddWithValue("@DraftContent",
                    string.IsNullOrWhiteSpace(DraftContent) ? "" : DraftContent);

                    con.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public string GetDraft(string userId, string draftKey)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetDraft", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@DraftKey", draftKey);

                con.Open();

                object obj = cmd.ExecuteScalar();

                return obj == null ? "" : obj.ToString();
            }
        }

        public bool DeleteDraft(string userId, string draftKey)
        {


            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_DeleteDraft", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@DraftKey", draftKey);

                con.Open();

                cmd.ExecuteNonQuery();

                return true;
            }
        }

        public bool DeleteDraftDirect(string userId, string draftKey)
        {


            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_DeleteDraftDirect", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@DraftKey", draftKey);

                con.Open();

                cmd.ExecuteNonQuery();

                return true;
            }
        }

        public bool UnReadDeleteDraft(string userId, string draftKey)
        {
         
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_DeleteUnreadDrafts", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@RefIds", draftKey);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }


    }


}