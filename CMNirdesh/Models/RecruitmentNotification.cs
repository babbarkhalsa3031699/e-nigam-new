using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class RecruitmentNotification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Title_Local { get; set; }
        public string Details { get; set; }
        public string Details_Local { get; set; }
        public string NotificationFile { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ApplyOnlineURL { get; set; }
        public string ApplyOfflineFile { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public string Mode { get; set; }
        public bool IsActive { get; set; }
        public List<RecruitmentNotification> RecruitmentNotificationList { get; set; }

    }
    public class RecruitmentNotificationModel
    {
        [HttpPost]
        public static int SaveRecruitment(RecruitmentNotification model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                SqlCommand cmd = new SqlCommand("sp_InsertOrUpdateRecruitmentNotification", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@Title_Local", model.Title_Local);
                cmd.Parameters.AddWithValue("@Details", model.Details);
                cmd.Parameters.AddWithValue("@Details_Local", model.Details_Local);
                cmd.Parameters.AddWithValue("@NotificationFile", model.NotificationFile ?? "");
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ApplyOnlineURL", model.ApplyOnlineURL ?? "");
                cmd.Parameters.AddWithValue("@ApplyOfflineFile", model.ApplyOfflineFile ?? "");
                cmd.Parameters.AddWithValue("@CreatedBy", "admin");
                cmd.Parameters.AddWithValue("@ModifiedBy", "admin");
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<RecruitmentNotification> GetRecuitmentList()
        {
            List<RecruitmentNotification> model = new List<RecruitmentNotification>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                //var p = new List<SqlParameter>();
                //p.Add(new SqlParameter("@mcid", mcid));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "sp_GetAllNotifications", null);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RecruitmentNotification item = new RecruitmentNotification();
                        item.Id = Convert.ToInt32(dr["Id"]);
                        item.Title = Convert.ToString(dr["Title"]);
                        item.Title_Local = Convert.ToString(dr["Title_Local"]);
                        item.Details = Convert.ToString(dr["Details"]);
                        item.Details_Local = Convert.ToString(dr["Details_Local"]);
                        item.IsActive = Convert.ToBoolean(dr["IsActive"]);
                        item.ApplyOfflineFile = Convert.ToString(dr["ApplyOfflineDocumentPath"]);
                        item.NotificationFile = Convert.ToString(dr["NotificationDocumentPath"]);
                        item.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        item.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        item.ApplyOnlineURL = Convert.ToString(dr["ApplyOnlineUrl"]);
                        model.Add(item);
                    }
                    ;

                }
                return model;

            }
            catch (Exception ex)
            {
                return model;

            }
        }

        public static List<RecruitmentNotification> GetRecruitmentNotificationByCode(int Id)
        {
            List<RecruitmentNotification> lstdept = new List<RecruitmentNotification>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@id", Id),
                  
                };

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "sp_GetAllNotificationsByID", parameterValues);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RecruitmentNotification item = new RecruitmentNotification();
                    {
                        item.Id = Convert.ToInt32(dr["Id"]);
                        item.Title = Convert.ToString(dr["Title"]);
                        item.Title_Local = Convert.ToString(dr["Title_Local"]);
                        item.Details = Convert.ToString(dr["Details"]);
                        item.Details_Local = Convert.ToString(dr["Details_Local"]);
                        item.IsActive = Convert.ToBoolean(dr["IsActive"]);
                        item.ApplyOfflineFile = Convert.ToString(dr["ApplyOfflineDocumentPath"]);
                        item.NotificationFile = Convert.ToString(dr["NotificationDocumentPath"]);
                        item.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        item.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        item.ApplyOnlineURL = Convert.ToString(dr["ApplyOnlineUrl"]);
                      }
                    ;
                    lstdept.Add(item);
                }

                return lstdept;
            }
            catch (Exception ex)
            {
                return lstdept;

            }
        }

        public static string UpdateStatusRecuitment(int ID)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@Id", ID),
                new SqlParameter("@ModifiedBy", CurrentSession.UserID),

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[ChangeStatusRecruitment]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

    }

}