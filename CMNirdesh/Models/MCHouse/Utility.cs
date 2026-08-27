using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.MCHouse
{
    public class Utility
    {
        public List<Utility> _LstUtilityMenu;
        public int UtilityId { get; set; }

        public string fileacessingUrl { get; set; }

        public int MicroProcessID { get; set; }

        public string UtilityName { get; set; }

        public string UtilityNameLocal { get; set; }

        public string UtilityFileName { get; set; }

        public string UtilityController { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsActive { get; set; }

        public static List<Utility> GetList()
        {

            List<Utility> lst = new List<Utility>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "UtilityMenuList");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Utility items = new Utility();
                    items.UtilityId = Convert.ToInt32(dr["UtilityId"]);
                    items.UtilityName = Convert.ToString(dr["UtilityName"]);
                    if (dr["UtilityName_Local"] != System.DBNull.Value)
                    {
                        items.UtilityNameLocal = Convert.ToString(dr["UtilityName_Local"]);
                    }
                    items.UtilityFileName = Convert.ToString(dr["UtilityIcon"]);
                    items.IsActive = Convert.ToBoolean(dr["IsActive"]);
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
        public static int SaveRecord(Utility model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("InsertUtilityRecord", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@mcid", model.MCId);
                cmd.Parameters.AddWithValue("@UtilityName", model.UtilityName);
                cmd.Parameters.AddWithValue("@UtilityNameLocal", model.UtilityNameLocal);
                cmd.Parameters.AddWithValue("@UtilityIcon", model.UtilityFileName);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
        //        cmd.Parameters.AddWithValue("@UtilityController", model.UtilityController);
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
        public static List<Utility> GetRowById(int id)
        {
            List<Utility> lst = new List<Utility>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);
                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetUtilityRecord", param).Tables[0].Rows)
                {
                    Utility item = new Utility();
                    {
                        item.UtilityId = Convert.ToInt32(dr["UtilityId"]);
                        item.UtilityName = Convert.ToString(dr["UtilityName"]);
                        item.UtilityNameLocal = Convert.ToString(dr["UtilityName_local"]);
                        item.UtilityFileName = Convert.ToString(dr["UtilityIcon"]);
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
        public static int UpdateRecord(Utility model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("UpdateUtilityRecord", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UpdatedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@UtilityId", model.UtilityId);
                cmd.Parameters.AddWithValue("@UtilityName", model.UtilityName);
                cmd.Parameters.AddWithValue("@UtilityNameLocal", model.UtilityNameLocal);
                cmd.Parameters.AddWithValue("@UtilityIcon", model.UtilityFileName ?? string.Empty);


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
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UtilityMenuDeleteRecord]", parameterValues));
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
