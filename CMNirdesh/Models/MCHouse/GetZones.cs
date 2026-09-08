using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Data;

namespace CMNirdesh.Models.MCHouse
{

    public class GetZones
    {
        [Key]

        public int id { get; set; }

        public string WardName { get; set; }

        public string WardName_local { get; set; }

        public int DistrictCode { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public int Zones { get; set; }

        public bool? Active { get; set; }

        public string DistrictName { get; set; }

        public string ZoneName { get; set; }

        public List<GetZones> _ListZonesModel { get; set; }

        public static List<GetZones> GetList()
        {
            List<GetZones> lstdept = new List<GetZones>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "ListOfZones");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    GetZones item = new GetZones();
                    item.id = Convert.ToInt32(dr["ConstituencyID"]);
                    item.WardName = Convert.ToString(dr["ConstituencyName"]);
                    item.WardName_local = Convert.ToString(dr["ConstituencyName_Local"]);
                    item.DistrictName = Convert.ToString(dr["DistrictName"]);
                    item.ZoneName = Convert.ToString(dr["Zone"]);
                    item.Active = Convert.ToBoolean(dr["Active"]);
                    lstdept.Add(item);
                }
                return lstdept;
            }
            catch(Exception ex)
            {
                return lstdept;
            }
        }
        public static int Save(GetZones modal)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("InsertZoneById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ConstituencyName", modal.WardName);
                cmd.Parameters.AddWithValue("@ConstituencyName_Local", modal.WardName_local);
                cmd.Parameters.AddWithValue("@DistrictCode", modal.DistrictCode);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@Id", modal.Zones);
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<GetZones> GetById(int id)
        {
            List<GetZones> lstdept = new List<GetZones>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);
                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetZoneById", param).Tables[0].Rows)
                {
                    GetZones items = new GetZones();
                    {
                        items.WardName = Convert.ToString(dr["ConstituencyName"]);
                        items.WardName_local = Convert.ToString(dr["ConstituencyName_Local"]);
                        items.Zones = Convert.ToInt32(dr["ZoneId"]);
                        items.DistrictCode = Convert.ToInt32(dr["DistrictCode"]);
                    };
                    lstdept.Add(items);
                }
                return lstdept;
            }
            catch (Exception ex)
            {
                return lstdept;
            }
        }
        public static int UpdateById(GetZones modal)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("UpdateZoneById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", modal.id);
                cmd.Parameters.AddWithValue("@ConstituencyName", modal.WardName);
                cmd.Parameters.AddWithValue("@ConstituencyName_Local", modal.WardName_local);
                cmd.Parameters.AddWithValue("@ZoneId", modal.Zones);
                cmd.Parameters.AddWithValue("@DistrictCode", modal.DistrictCode);
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int DeleteById(int Id)
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
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[DeleteZoneById]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }
        }
    }
}

