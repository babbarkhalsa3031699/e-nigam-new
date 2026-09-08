using CMNirdesh.Models.MCHouse;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class LgTender
    {
        public int TenderId { get; set; }

        public string TenderTitle { get; set; }

        public string TenderTitle_Punjabi { get; set; }

        public string Description { get; set; }

        public string Description_Punjabi { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TenderDocumentPath { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
    }

    public class DataViewModelTender
    {
        public static int SaveTender(LgTender model)
        {
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertLGTenderRecord", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TenderTitle", model.TenderTitle);
                        cmd.Parameters.AddWithValue("@TenderTitle_Punjabi", model.TenderTitle_Punjabi ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Description", model.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Description_Punjabi", model.Description_Punjabi ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                        cmd.Parameters.AddWithValue("@TenderDocumentPath", model.TenderDocumentPath ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy ?? CurrentSession.UserID);
                        cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                        SqlParameter parmOUT = new SqlParameter("@ErrorCode", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(parmOUT);

                        cmd.ExecuteNonQuery();
                        int errorCode = (int)cmd.Parameters["@ErrorCode"].Value;

                        return errorCode == 0 ? 0 : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return -1;
            }
        }
        public static List<LgTender> GetList()
        {
            List<LgTender> list = new List<LgTender>();

            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();

                    using (SqlCommand cmd = new SqlCommand("LGTenderList", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                LgTender item = new LgTender
                                {
                                    TenderId = Convert.ToInt32(dr["TenderId"]),
                                    TenderTitle = Convert.ToString(dr["TenderTitle"]),
                                    TenderTitle_Punjabi = dr["TenderTitle_Punjabi"] != DBNull.Value ? Convert.ToString(dr["TenderTitle_Punjabi"]) : null,
                                    Description = dr["Description"] != DBNull.Value ? Convert.ToString(dr["Description"]) : null,
                                    Description_Punjabi = dr["Description_Punjabi"] != DBNull.Value ? Convert.ToString(dr["Description_Punjabi"]) : null,
                                    StartDate = (DateTime)(dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : (DateTime?)null),
                                    EndDate = (DateTime)(dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : (DateTime?)null),
                                    TenderDocumentPath = dr["TenderDocumentPath"] != DBNull.Value ? Convert.ToString(dr["TenderDocumentPath"]) : null,
                                    CreatedOn = (DateTime)(dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]) : (DateTime?)null),
                                    CreatedBy = dr["CreatedBy"] != DBNull.Value ? Convert.ToString(dr["CreatedBy"]) : null,
                                    ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : (DateTime?)null,
                                    ModifiedBy = dr["ModifiedBy"] != DBNull.Value ? Convert.ToString(dr["ModifiedBy"]) : null,
                                    IsDeleted = Convert.ToBoolean(dr["IsDeleted"]),
                                    IsActive = Convert.ToBoolean(dr["IsActive"])
                                };

                                list.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }

            return list;
        }

        public static List<LgTender> GetRowById(int id)
        {
            List<LgTender> lst = new List<LgTender>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);
                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "LgGetById", param).Tables[0].Rows)
                {
                    LgTender item = new LgTender();
                    {
                        item.TenderTitle = Convert.ToString(dr["TenderTitle"]);
                        item.TenderTitle_Punjabi = Convert.ToString(dr["TenderTitle_Punjabi"]);
                        item.TenderDocumentPath = Convert.ToString(dr["TenderDocumentPath"]);
                        item.IsActive = Convert.ToBoolean(dr["IsActive"]);
                        item.StartDate = (DateTime)(dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : (DateTime?)null);
                        item.EndDate = (DateTime)(dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : (DateTime?)null);
                        item.TenderId = Convert.ToInt32(dr["TenderId"]);
                       
                    }
                    ;
                    lst.Add(item);
                }
                return lst;
            }
            catch (Exception ex)
            {
                return lst;
            }
        }

        public static bool UpdateTender(LgTender tender)
        {
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();

                    SqlParameter[] param = new SqlParameter[]
                    {
                        new SqlParameter("@TenderId", tender.TenderId),
                        new SqlParameter("@TenderTitle", tender.TenderTitle ?? (object)DBNull.Value),
                        new SqlParameter("@TenderTitle_Punjabi", tender.TenderTitle_Punjabi ?? (object)DBNull.Value),
                        new SqlParameter("@TenderDocumentPath", tender.TenderDocumentPath ?? (object)DBNull.Value),
                        new SqlParameter("@IsActive", tender.IsActive),
                        new SqlParameter("@StartDate", tender.StartDate),
                        new SqlParameter("@EndDate", tender.EndDate),
                    };
                    int result = CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "LgTender_UpdateById", param);
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
              return false;
            }
        }


        public static string DeleteRecord(int Id)
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
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[TenderLGDeleteRecord]", parameterValues));
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
    public class DataViewModelTenderList
    {
        public List<LgTender> lgTenders { get; set; }
        public string fileacessingUrl { get; set; }

    }
}