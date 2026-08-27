using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class mAssembly
    {
        public int AssemblyID { get; set; }
        public int AssemblyCode { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyNameLocal { get; set; }
        public DateTime? AssemblyStartDate { get; set; }
        public DateTime? AssemblyEndDate { get; set; }
        public string Period { get; set; }
        public string Rem1 { get; set; }
        public string Rem2 { get; set; }
        public string Rem3 { get; set; }
        public string Rem4 { get; set; }
        public string Rem5 { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsAssemblyData { get; set; }
        public bool? Active { get; set; }
        public bool? IsDeleted { get; set; }
        public string DeptId { get; set; }

        public string Mode { get; set; }
        public List<mAssembly> Houselist { get; set; }
    }

    public class Assemblylist
    {
        public static List<mAssembly> GetHouselistDetail(int mcid)
        {
            List<mAssembly> model = new List<mAssembly>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@mcid", mcid));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con,CommandType.StoredProcedure, "GetAssemblyList", p.ToArray());
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        mAssembly item = new mAssembly();
                        item.AssemblyID = Convert.ToInt32(dr["AssemblyCode"]);
                        item.AssemblyName = Convert.ToString(dr["AssemblyName"]);
                        item.AssemblyNameLocal = Convert.ToString(dr["AssemblyNameLocal"]);
                        item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        item.Active = Convert.ToBoolean(dr["Active"]);
                        if (dr["AssemblyStartDate"] != DBNull.Value && dr["AssemblyStartDate"] != null)
                        {
                            item.AssemblyStartDate = Convert.ToDateTime(dr["AssemblyStartDate"]);
                        }
                        if (dr["AssemblyEndDate"] != DBNull.Value && dr["AssemblyEndDate"] != null)
                        {
                            item.AssemblyEndDate = Convert.ToDateTime(dr["AssemblyEndDate"]);
                        }
                        item.Period = Convert.ToString(dr["Period"]);
                        item.DeptId = Convert.ToString(dr["DeptId"]);
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

        public static string SaveHouse(mAssembly model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@AssemblyName", model.AssemblyName),
                new SqlParameter("@AssemblyName_Local", model.AssemblyNameLocal),
                new SqlParameter("@CreatedBy", CurrentSession.UserID),
                new SqlParameter("@mcid", CurrentSession.StateId),
                new SqlParameter("@StartDate", model.AssemblyStartDate),
                new SqlParameter("@EndDate", model.AssemblyEndDate),
                new SqlParameter("@Period", model.Period),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertAssembly]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string UpdateHouse(mAssembly model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@AssemblyCode", model.AssemblyCode),
                new SqlParameter("@AssemblyName", model.AssemblyName),
                new SqlParameter("@AssemblyName_Local", model.AssemblyNameLocal),
                new SqlParameter("@CreatedBy", CurrentSession.UserID),
                new SqlParameter("@mcid", CurrentSession.StateId),
                new SqlParameter("@StartDate", model.AssemblyStartDate),
                new SqlParameter("@EndDate", model.AssemblyEndDate),
                new SqlParameter("@Period", model.Period),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateAssembly]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static List<mAssembly> GetHouseByCode(int AssemblyCode, string MCID)
        {
            List<mAssembly> lstdept = new List<mAssembly>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
              
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@AssemblyCode", AssemblyCode),
                    new SqlParameter("@MCId", MCID),
                };

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetAssemblyByCode", parameterValues);
                con.Close();    
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mAssembly doc = new mAssembly();
                    {
                        doc.DeptId = Convert.ToString(dr["DeptId"]);
                        doc.AssemblyCode = Convert.ToInt32(dr["AssemblyCode"]);
                        doc.AssemblyName = Convert.ToString(dr["AssemblyName"]);
                        doc.AssemblyNameLocal = Convert.ToString(dr["AssemblyNameLocal"]);
                        doc.AssemblyStartDate = Convert.ToDateTime(dr["AssemblyStartDate"]);
                        doc.AssemblyEndDate = Convert.ToDateTime(dr["AssemblyEndDate"]);
                        doc.Period = Convert.ToString(dr["Period"]);
                        
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

        public static string UpdateStatusHouse(int AssemblyCode, string MCID)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@AssemblyCode", AssemblyCode),
                new SqlParameter("@MCId", MCID),
               
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[ChangeStatusAssembly]", parameterValues));
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