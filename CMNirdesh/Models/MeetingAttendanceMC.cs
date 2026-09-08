using BotDetect;
using CMNirdesh.Models.MCHouse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class MeetingAttendanceMC
    {
        public int Id { get; set; }
        public string MCCode { get; set; }
        public string MemberCode { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Status { get; set; }
        public string AttendanceStatus { get; set; }
        public string WardCode { get; set; }
        public string HouseCode { get; set; }
        public List<MCMembers> MembersList { get; set; }
        public List<FNCCMember> FNCCMembersList { get; set; }
        public int AgendaId { get; set; }
        public List<AgendaNote> AddedFNCCMembers { get; set; }
        public SelectList AgendaList { get; set; }

    }

    public class AttendanceDocument
    {
        public int Id { get; set; }
        public int AgendaId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }
    }

    public class UploadAttendanceViewModel
    {
        public string fileacessingUrl { get; set; }
        public int MeetingId { get; set; }
        public List<AttendanceDocument> Documents { get; set; }
    }

    public class AgendaNote
    {
        public int NoteId { get; set; }
        public int MeetingId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string NoteTitle { get; set; }
        public string NoteText { get; set; }
    }

    public class NoteCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }


    public class AgendaNoteViewModel
    {
        public int MeetingId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<AgendaNote> AgendaNoteList  { get; set; }
        public List<NoteCategory> Categories { get; set; }
    }



    public class AttendenceList
    {
        public static SelectList GetMeetingListforAttendance(string OfficeId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@OfficeId", OfficeId) };
                ds = SqlHelper.ExecuteDataset(connection, "getMeetingListforAttendance", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToDateTime(row["AgendaDate"]).ToString("dd/MM/yyyy") + ": " + Convert.ToString(row["AgendaName"].ToString() + " [" + Convert.ToString(row["AgendaType"].ToString()) + "]");
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }
        public static List<MCMembers> GetMembersList(int Id)
        {
            List<MCMembers> model = new List<MCMembers>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
               

                SqlParameter[] parameterValues = new SqlParameter[] {
                     new SqlParameter("@MeetingId", Id),
                };
                DataSet ds = SqlHelper.ExecuteDataset(con, "[GetMinisterData]", parameterValues);
                con.Close();
           
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MCMembers item = new MCMembers();
                        item.MemberCode = Convert.ToInt32(dr["MemberCode"]);
                        item.Name = Convert.ToString(dr["Name"]);
                        item.WardName = Convert.ToString(dr["WardId"]);
                        item.Designation = Convert.ToString(dr["Designation"]);
                        item.MemberID= Convert.ToInt16(dr["Status"]);
                        item.IsAlive = Convert.ToString(dr["AttendanceStatus"]);
                        item.ModifiedDate = Convert.ToDateTime(dr["MeetingDate"]);
                        model.Add(item);
                    };

                }
                return model;

            }
            catch (Exception ex)
            {
                return model;

            }
        }
        public static List<MCMembers> GetMCMembersList(int Id,int MCid)
        {
            List<MCMembers> model = new List<MCMembers>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();


                SqlParameter[] parameterValues = new SqlParameter[] {
                     new SqlParameter("@MCID", MCid),
                       new SqlParameter("@MeetingId", Id),
                };
                DataSet ds = SqlHelper.ExecuteDataset(con, "[GetMembersData]", parameterValues);
                con.Close();

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MCMembers item = new MCMembers();
                        item.MemberCode = Convert.ToInt32(dr["MemberCode"]);
                        item.Name = Convert.ToString(dr["Name"]);
                        item.WardName = Convert.ToString(dr["WardId"]);
                        item.Designation = Convert.ToString(dr["Designation"]);
                        item.MemberID = Convert.ToInt16(dr["Status"]);
                        item.IsAlive = Convert.ToString(dr["AttendanceStatus"]);
                        item.ModifiedDate = Convert.ToDateTime(dr["MeetingDate"]);
                        model.Add(item);
                    };

                }
                return model;

            }
            catch (Exception ex)
            {
                return model;

            }
        }

        public static List<FNCCMember> GetFNCCMembersList(int Id, int MCid)
        {
            List<FNCCMember> model = new List<FNCCMember>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();


                SqlParameter[] parameterValues = new SqlParameter[] {
                     new SqlParameter("@MCID", MCid),
                       new SqlParameter("@MeetingId", Id),
                };
                DataSet ds = SqlHelper.ExecuteDataset(con, "[GetFNCCMembersData]", parameterValues);
                con.Close();

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FNCCMember item = new FNCCMember();
                        item.UserId = Convert.ToString(dr["UserId"]);
                        item.UserName = Convert.ToString(dr["Name"]);
                        item.Designation = Convert.ToString(dr["Designation"]);
                        item.Status = Convert.ToString(dr["AttendanceStatus"]);
                        item.IsPresent = Convert.ToInt32(dr["Status"]);
                        
                        model.Add(item);
                    };

                }
                return model;

            }
            catch (Exception ex)
            {
                return model;

            }
        }
        public static int AddAttendenceAll(int Id, int MCid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                     new SqlParameter("@MCCode", MCid),
                    new SqlParameter("@MeetingId", Id)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "Markattendenceall", parameterValues));
                connection.Close();
                return Convert.ToInt16(refid);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }
        public static int ClearAttendenceAll(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@MeetingId", Id)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "Clearattendenceall", parameterValues));
                connection.Close();
                return Convert.ToInt16(refid);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

        public static int AddAttendenceMember(int Id, int MemberCode,int MCid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                     new SqlParameter("@MCCode", MCid),
                    new SqlParameter("@MeetingId", Id),
                    new SqlParameter("@MemberCode", MemberCode)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "MarkattendenceMember", parameterValues));
                connection.Close();
                return Convert.ToInt16(refid);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

        public static int ClearAttendenceMember(int mcid, int Id, int MemberCode)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@MCCode", mcid),
                    new SqlParameter("@MeetingId", Id),
                    new SqlParameter("@MemberCode", MemberCode)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "ClearattendenceMember", parameterValues));
                connection.Close();
                return Convert.ToInt16(refid);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

        public static int AddAttendenceFNCC(int Id, string UserId, int MCid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                     new SqlParameter("@MCCode", MCid),
                    new SqlParameter("@MeetingId", Id),
                    new SqlParameter("@UserId", UserId)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "MarkattendenceFNCC", parameterValues));
                connection.Close();
                return Convert.ToInt16(refid);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

        public static int ClearAttendenceFNCC(int Id, string UserId, int MCid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@MCCode", MCid),
                    new SqlParameter("@MeetingId", Id),
                    new SqlParameter("@UserId", UserId)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "ClearAttendenceFNCC", parameterValues));
                connection.Close();
                return Convert.ToInt16(refid);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

        public static void InsertAttendanceDocument(AttendanceDocument doc)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
             new SqlParameter("@MeetingId", doc.AgendaId),
             new SqlParameter("@FileName", doc.FileName),
             new SqlParameter("@FilePath", doc.FilePath),
             new SqlParameter("@UploadedBy", CurrentSession.UserID)
            };

            SqlHelper.ExecuteNonQuery(ClsConnection.GetConnection(), "InsertMeetingAttendanceDocument", parameters);
        }
        public static List<AttendanceDocument> GetAttendanceDocuments(int AgendaId)
        {
            List<AttendanceDocument> list = new List<AttendanceDocument>();
            SqlParameter[] parameters = { new SqlParameter("@MeetingId", AgendaId) };

            using (DataSet ds = SqlHelper.ExecuteDataset(ClsConnection.GetConnection(), "GetMeetingAttendanceDocuments", parameters))
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        list.Add(new AttendanceDocument
                        {
                            AgendaId = Convert.ToInt32(row["MeetingId"]),
                            FileName = Convert.ToString(row["FileName"]),
                            FilePath = Convert.ToString(row["FilePath"]),
                            UploadedBy = Convert.ToString(row["UploadedBy"]),
                            UploadedOn = Convert.ToDateTime(row["UploadedOn"])
                        });
                    }
                }
            }

            return list;
        }

        public static List<AgendaNote> GetNotes(int agendaId)
        {
            var list = new List<AgendaNote>();
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetAgendaNotes", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AgendaId", agendaId);
                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new AgendaNote
                        {
                            NoteId = Convert.ToInt32(rdr["NoteId"]),
                            MeetingId = Convert.ToInt32(rdr["AgendaId"]),
                            CategoryId = Convert.ToInt32(rdr["CategoryId"]),
                            CategoryName = rdr["CategoryName"].ToString(),
                            NoteTitle = rdr["NoteTitle"].ToString(),
                            NoteText = rdr["NoteText"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public static bool IsBackLogAgenda(int agendaId)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("IsBackLogAgenda", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@agendaid", agendaId);

                con.Open();

                object result = cmd.ExecuteScalar();

                return result != null && Convert.ToInt32(result) == 1;
            }
        }

        public static List<NoteCategory> GetNoteCategories()
        {
            var list = new List<NoteCategory>();
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetNoteCategories", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new NoteCategory
                        {
                            CategoryId = Convert.ToInt32(rdr["CategoryId"]),
                            CategoryName = rdr["CategoryName"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public static void InsertNote(AgendaNote model, string userId)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_InsertAgendaNote", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NoteId", model.NoteId == 0 ? 0 : model.NoteId);
                cmd.Parameters.AddWithValue("@AgendaId", model.MeetingId);
                cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("@NoteTitle", "");
                cmd.Parameters.AddWithValue("@NoteText", model.NoteText);
                cmd.Parameters.AddWithValue("@CreatedBy", userId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public  static void DeleteNote(int noteId, int agendaId, string username)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_DeleteAgendaNote", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NoteId", noteId);
                cmd.Parameters.AddWithValue("@AgendaId", agendaId);
                cmd.Parameters.AddWithValue("@DeletedBy", username);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
