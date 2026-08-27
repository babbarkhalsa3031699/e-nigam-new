using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CMNirdesh.Models.MCHouse;

namespace CMNirdesh.Models
{
    public class HouseMeetingsDocuments
    {
        public int MeetingID { get; set; }
       
        public int HouseNo { get; set; }
        public int HouseID { get; set; }

        public int MCID { get; set; }

        [Display(Name = "Meeting Date")]
        [DataType(DataType.Date)]
        public string MeetingDate { get; set; }

        [Display(Name = "Title")]
        [StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "Meeting Type")]
        [StringLength(50)]
        public string MeetingType { get; set; }

        [Display(Name = "Document Name")]
        [StringLength(200)]
        public string DocumentName { get; set; }

        public string Mode { get; set; }


        // This property will hold the file content

        public string FilePath { get; set; }


        public string FullPath { get; set; }

        public List<SelectListItem> MeetingTypesList { get; set; }

        public string fileacessingUrl { get; set; }

        public SelectList HouseList { get; set; }

        public List<HouseMeetingsDocuments> _houseDocuments { get; set; }

        public List<HouseMeetingsDocuments> _houseDocumentsbyId { get; set; }

        public static string SaveNewMeeting(HouseMeetingsDocuments model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                 SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@HouseId", model.HouseNo),
                    new SqlParameter("@MeetingDate",model.MeetingDate),
                    new SqlParameter("@Subject", model.Title),
                    new SqlParameter("@MeetingType", model.MeetingType),
                    new SqlParameter("@DocumentName", "Proceeding"),
                    new SqlParameter("@DocumentPath", model.FilePath),
                    new SqlParameter("@mcid", model.MCID),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertNewProceeding]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static List<HouseMeetingsDocuments> GetMeetingsDocumentsList(int MCID)
        {

            List<HouseMeetingsDocuments> lst = new List<HouseMeetingsDocuments>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] parameterValues = new SqlParameter[] {
                   
                    new SqlParameter("@mcid", MCID),
                };
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetProceeding", parameterValues);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HouseMeetingsDocuments items = new HouseMeetingsDocuments();
                    items.MeetingID = Convert.ToInt32(dr["MeetingID"]);
                    items.Title = Convert.ToString(dr["Title"]);
                    items.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                    items.FilePath = Convert.ToString(dr["pdfpath"]);
                    items.DocumentName = Convert.ToString(dr["DocumentType"]);
                    lst.Add(items);
                }
                return lst;
            }
            catch
            {
                return lst;
            }

        }

        public static string UpdateNewMeeting(HouseMeetingsDocuments model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@MeetingId", model.MeetingID),
                    new SqlParameter("@HouseId", model.HouseNo),
                    new SqlParameter("@MeetingDate",model.MeetingDate),
                    new SqlParameter("@Subject", model.Title),
                    new SqlParameter("@MeetingType", model.MeetingType),
                    new SqlParameter("@DocumentName", "Proceeding"),
                    new SqlParameter("@DocumentPath", model.FilePath),
                    new SqlParameter("@mcid", model.MCID),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "UpdateMeetingDocument", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }




        public static List<HouseMeetingsDocuments> GetMeetingsDocumentById(int MeetingID)
        {
            List<HouseMeetingsDocuments> lstdept = new List<HouseMeetingsDocuments>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@MeetingID", MeetingID);
                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetMettingDocument", param).Tables[0].Rows)
                {
                    HouseMeetingsDocuments doc = new HouseMeetingsDocuments();
                    {
                        doc.MeetingID = Convert.ToInt32(dr["MeetingID"]);
                        doc.HouseNo = Convert.ToInt32(dr["HouseID"]);
                        doc.Title = Convert.ToString(dr["Title"]);
                        //doc.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        doc.MeetingType = Convert.ToString(dr["MeetingType"]);
                        doc.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]).ToString("yyyy/MM/dd");
                        doc.FilePath = Convert.ToString(dr["DocumentPath"]);
                        doc.FullPath = BlobStorage.GetStorageAcessingPath() + "/" + Convert.ToString(dr["DocumentPath"]);
                        doc.MCID = Convert.ToInt32(dr["mcid"]);
                    };

                    lstdept.Add(doc);
                }

                return lstdept;
            }
            catch (Exception ex)
            {
                return lstdept;

            }
        }


        public static string DeleteMettingDocument(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@Id", Id),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DeleteMettingDocument]", parameterValues));

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