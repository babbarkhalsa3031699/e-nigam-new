using CMNirdesh.Models.Session;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
     public class mConstituency
    {
        public int ConstituencyID { get; set; }

        [Required(ErrorMessage = "Select House")]
        public int AssemblyID { get; set; }

        public string AssemblyName { get; set; }

        public int ConstituencyCode { get; set; }

        [Required(ErrorMessage = "Ward Name is required")]
        [MaxLength(150)]
        public string ConstituencyName { get; set; }

        [Required(ErrorMessage = "WardName (Punjabi) is required")]
        [MaxLength(150)]
        public string ConstituencyName_Local { get; set; }

        public int? DistrictCode { get; set; }

        [MaxLength]
        public string LGDistrictCode { get; set; }

        [MaxLength(2)]
        public string ConstituencyCode_CmRefnic { get; set; }

        public DateTime? ConstituencyCreatedDate { get; set; }

        public bool? Active { get; set; }

        [MaxLength]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [MaxLength]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsDeleted { get; set; }

        public int? ReservedCategory { get; set; }

        public int? ZoneId { get; set; }
        public string ZoneName { get; set; }

        public int? mcid { get; set; }

        public string Mode { get; set; }
        public List<mConstituency> WardList { get; set; }
        public List<assembly> Houselist { get; set; }

        public List<Zone> Zonelist{ get; set; }
    }
       
        public class assembly
        {
            public int AssemblyID { get; set; }
            public string AssemblyName { get; set; }

        }


        public class Constituencylist
        {
        public static List<mConstituency> GetWardList(int mcid)
        {
            List<mConstituency> model = new List<mConstituency>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@mcid", mcid));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "GetWardList", p.ToArray());
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        mConstituency item = new mConstituency();
                        item.ConstituencyCode= Convert.ToInt32(dr["ConstituencyCode"]);
                        item.AssemblyName = Convert.ToString(dr["AssemblyName"]);
                        item.ZoneName = Convert.ToString(dr["Zone"]);
                        item.AssemblyID = Convert.ToInt32(dr["AssemblyID"]);
                        item.mcid = Convert.ToInt32(dr["mcid"]);
                        item.Active = Convert.ToBoolean(dr["Active"]);
                        item.ConstituencyName = Convert.ToString(dr["ConstituencyName"]);
                        item.ConstituencyName_Local = Convert.ToString(dr["ConstituencyName_Local"]);
                        item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
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

        public static List<assembly> GetHouselist(int mcid)
            {
            List<assembly> model = new List<assembly>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@mcid", mcid));
                p.Add(new SqlParameter("@IsActiveRequired", true));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con,CommandType.StoredProcedure, "GetAssemblyList", p.ToArray());
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        assembly item = new assembly();
                        item.AssemblyID = Convert.ToInt32(dr["AssemblyCode"]);
                        item.AssemblyName = Convert.ToString(dr["AssemblyName"]);
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


        public static List<Zone> GetZonelist(int mcid)
        {
            List<Zone> model = new List<Zone>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@mcid", mcid));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "GetZoneList", p.ToArray());
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Zone item = new Zone();
                        item.ZoneID = Convert.ToInt32(dr["ZoneID"]);
                        item.ZoneName = Convert.ToString(dr["Zone"]);
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

        public static string SaveConstituency(mConstituency model)
        {
            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    SqlParameter[] parameterValues = new SqlParameter[]
                    {
                        new SqlParameter("@AssemblyID", SqlDbType.Int) { Value = model.AssemblyID },
                        new SqlParameter("@ZoneID", SqlDbType.Int) { Value = model.ZoneId },
                        new SqlParameter("@ConstituencyName", SqlDbType.NVarChar, 500) { Value = model.ConstituencyName },
                        new SqlParameter("@ConstituencyName_Local", SqlDbType.NVarChar, 500) { Value = model.ConstituencyName_Local }, // ✅ Unicode-safe
                        new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 500) { Value = CurrentSession.UserID },
                        new SqlParameter("@mcid", SqlDbType.Int) { Value = CurrentSession.StateId }
                    };

                    string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertWard]", parameterValues));
                    return refid;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return "0";
            }
        }

        public static string UpdateConstituency(mConstituency model)
        {
            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    SqlParameter[] parameterValues = new SqlParameter[]
                    {
                new SqlParameter("@ConstituencyCode", SqlDbType.Int) { Value = model.ConstituencyCode },
                new SqlParameter("@AssemblyID", SqlDbType.Int) { Value = model.AssemblyID },
                new SqlParameter("@ZoneID", SqlDbType.Int) { Value = model.ZoneId },
                new SqlParameter("@ConstituencyName", SqlDbType.NVarChar, 500) { Value = model.ConstituencyName },
                new SqlParameter("@ConstituencyName_Local", SqlDbType.NVarChar, 500) { Value = model.ConstituencyName_Local }, // ✅ Unicode-safe
                new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 500) { Value = CurrentSession.UserID },
                new SqlParameter("@mcid", SqlDbType.Int) { Value = CurrentSession.StateId }
                    };

                    string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateWard]", parameterValues));
                    return refid;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return "0";
            }
        }

        public static List<mConstituency> GetConstituencyByCode(int ConstituencyCode, string MCID)
        {
            List<mConstituency> lstdept = new List<mConstituency>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@ConstituencyCode", ConstituencyCode),
                    new SqlParameter("@MCId", MCID),
                };

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetConstituencyByCode", parameterValues);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mConstituency item = new mConstituency();
                    {
                        item.AssemblyID = Convert.ToInt32(dr["AssemblyID"]);
                        item.ZoneId = Convert.ToInt32(dr["ZoneID"]);
                        item.mcid = Convert.ToInt32(dr["mcid"]);
                        item.ConstituencyCode = Convert.ToInt32(dr["ConstituencyCode"]);
                        item.ConstituencyName = Convert.ToString(dr["ConstituencyName"]);
                        item.ConstituencyName_Local = Convert.ToString(dr["ConstituencyName_Local"]);
                        item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                      

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

        public static string UpdateStatusConstituency(int ConstituencyCode, string MCID, int AssemblyCode)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@ConstituencyCode", ConstituencyCode),
                new SqlParameter("@MCId", MCID),
                new SqlParameter("@AssemblyCode", AssemblyCode),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[ChangeStatusConstituencyCode]", parameterValues));
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