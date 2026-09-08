using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.MCHouse
{
    public class TenderRecord
    {

        public List<TenderRecord> _LstTenderRecord;

        public string fileacessingUrl{ get; set; }
        public int Id { get; set; }

        public string TenderName { get; set; }

        public string TenderFileName { get; set; }

        public int MCid { get; set; }

        public string TenderName_local { get; set; }

        public bool Active { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }


        public string Status { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string StateName { get; set; }

        public int StateId { get; set; }

        public int StatusID { get; set; }

        public static List<TenderRecord> GetList()
        {
            List<TenderRecord> lst = new List<TenderRecord>();
            try
                {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "TenderRecordList");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TenderRecord items = new TenderRecord();
                    items.Id = Convert.ToInt32(dr["Id"]);
                    items.TenderName = Convert.ToString(dr["TenderName"]);
                    if (dr["TenderFileName"] != System.DBNull.Value)
                    {
                        items.TenderFileName = Convert.ToString(dr["TenderFileName"]);
                    }
                    items.TenderName_local = Convert.ToString(dr["TenderName_local"]);
                    items.Active = Convert.ToBoolean(dr["Active"]);
                    items.Status = Convert.ToString(dr["StatusName"]);
                    items.StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("dd/MM/yyyy") : string.Empty;
                    items.EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]).ToString("dd/MM/yyyy") : string.Empty;
                    items.StateName = Convert.ToString(dr["MCName"]);
                    lst.Add(items);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }

        }
        public static int SaveRecord(TenderRecord model)
        {
            try
            {
                string path = "/" + model.StateName + "/Tender/";
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                con.Open();
                SqlCommand cmd = new SqlCommand("TenderRecordSave", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@mcid", model.MCId);
                cmd.Parameters.AddWithValue("@TenderName", model.TenderName);
                cmd.Parameters.AddWithValue("@TenderName_local", model.TenderName_local);
                cmd.Parameters.AddWithValue("@TenderFileName", SqlDbType.VarChar).Value = (model.TenderFileName == null) ? string.Empty : path+"/"+model.TenderFileName;
                cmd.Parameters.AddWithValue("@MCId", model.StateId);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@Status", model.StatusID);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                //cmd.Parameters.Add("@IsActive", mDepartment.IsActive);
                SqlParameter parmOUT = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parmOUT.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parmOUT);
                cmd.ExecuteNonQuery();
                int errorCode = (int)cmd.Parameters["@ErrorCode"].Value;
                if (errorCode == 0)
                {
                    return 0; // Insertion Successful
                }
                else
                {
                    return -1; // Error occurred during insertion
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return -1; // Exception occurred
            }
        }
        public static List<TenderRecord> GetRowById(int id)
        {
            List<TenderRecord> lst = new List<TenderRecord>();
            try
            {
            SqlConnection con = ClsConnection.GetConnection();
            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
            con.Open();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@Id", id);
            con.Close();
            foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "TenderRecordGetById", param).Tables[0].Rows)
            {
                TenderRecord item = new TenderRecord();
                {
                    item.TenderName = Convert.ToString(dr["TenderName"]);
                    item.TenderName_local = Convert.ToString(dr["TenderName_local"]);
                    item.TenderFileName = Convert.ToString(dr["TenderFileName"]);
                    item.StatusID = Convert.ToInt32(dr["Status"]);
                    item.StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("yyyy-MM-dd") : string.Empty;
                    item.EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]).ToString("yyyy-MM-dd") : string.Empty;
                    item.StateId = Convert.ToInt32(dr["MCId"]);
                    item.Id = Convert.ToInt32(dr["Id"]);
                };
                lst.Add(item);
            }
            return lst;
            }
            catch (Exception ex)
            {
            return lst;
            }
        }
        public static int UpdateRecord(TenderRecord model)
        {
           try
               {
                string path = "/" + model.StateName + "/Tender/";
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("TenderRecordUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@TenderName", model.TenderName);
                cmd.Parameters.AddWithValue("@TenderName_local", model.TenderName_local);
                cmd.Parameters.AddWithValue("@Status", model.StatusID);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@TenderFileName", SqlDbType.VarChar).Value = (model.TenderFileName == null) ? string.Empty : path + "/" + model.TenderFileName;
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                cmd.Parameters.AddWithValue("@MCid", model.StateId);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                SqlParameter parmOUT = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parmOUT.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parmOUT);
                cmd.ExecuteNonQuery();
                int errorCode = (int)cmd.Parameters["@ErrorCode"].Value;
                if (errorCode == 0)
                {
                    return 0; // Insertion Successful
                }

                else
                {
                    return -1; // Error occurred during insertion
                }
           }
                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                    return -1; // Exception occurred
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
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[TenderRecordDeleteRecord]", parameterValues));
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