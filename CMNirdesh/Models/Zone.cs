using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using PoliteCaptcha;
using System.ComponentModel.DataAnnotations;

namespace CMNirdesh.Models
{
    public class Zone
    {
        public int ZoneID { get; set; }


        [Required(ErrorMessage = "Zone Name is required")]
        public string ZoneName { get; set; }

        [Required(ErrorMessage = "Zone Name (Punjabi) is required")]
        public string ZoneNameLocal { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Active { get; set; }

        public string Mode { get; set; }
        public List<Zone> Zoneslist { get; set; }
    }
    public class Zonelist
    {
        public static List<Zone> GetZonelistDetail(string mcid)
        {
            List<Zone> model = new List<Zone>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
             
                con.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@mcid", mcid)  };
                DataSet ds = SqlHelper.ExecuteDataset(con, "GetZoneList", parameterValues);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Zone item = new Zone();
                        item.ZoneID = Convert.ToInt32(dr["ZoneID"]);
                        item.ZoneName = Convert.ToString(dr["Zone"]);
                        item.ZoneNameLocal = Convert.ToString(dr["ZoneLocal"]);
                        item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        item.CreatedBy = Convert.ToString(dr["CreatedBy"]);
                        item.Active = Convert.ToBoolean(dr["Active"]);
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

        public static string SaveZone(Zone model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@ZoneName", model.ZoneName),
                new SqlParameter("@ZoneNameLocal", model.ZoneNameLocal),
                new SqlParameter("@CreatedBy", CurrentSession.UserID),
                 new SqlParameter("@mcid", CurrentSession.StateId),

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertZone]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string UpdateZone(Zone model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@ZoneID", model.ZoneID),
                new SqlParameter("@ZoneName", model.ZoneName),
                new SqlParameter("@ZoneNameLocal", model.ZoneNameLocal),
                new SqlParameter("@CreatedBy", CurrentSession.UserID),
                new SqlParameter("@mcid", CurrentSession.StateId),

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateZone]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static List<Zone> GetZoneCode (int ZoneID, string MCID)
        {
            List<Zone> lstdept = new List<Zone>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@ZoneID", ZoneID),
                    new SqlParameter("@mcid", MCID),
                };

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetZoneByCode", parameterValues);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Zone item = new Zone();
                    {
                        item.ZoneID = Convert.ToInt32(dr["ZoneID"]);
                        item.ZoneName = Convert.ToString(dr["Zone"]);
                        item.ZoneNameLocal = Convert.ToString(dr["ZoneLocal"]);
                        item.Active = Convert.ToBoolean(dr["Active"]);
                      
                     };
                    lstdept.Add(item);
                }

                return lstdept;
            }
            catch (Exception ex)
            {
                return lstdept;

            }
        }

        public static string UpdateStatusZone (int ZoneID, string MCID, int Active)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@ZoneID", ZoneID),
                new SqlParameter("@mcid", MCID),
                new SqlParameter("@IsActive", Active),

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[ChangeStatusZone]", parameterValues));
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