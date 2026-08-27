using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.MCHouse
{
    public class TenderMenu
    {
        public List<TenderMenu> _LstTenderMenu;
        public string SubMenuName { get; set; }

        public string SubMenu_local { get; set; }

        public int Type { get; set; }
        public string TypeName { get; set; }

        public int MenuId { get; set; }

        public int SubMenuId { get; set; }
        public bool IsDeleted { get; set; }
        public int Order { get; set; }
        public static List<TenderMenu> GetList()
        {

            List<TenderMenu> lst = new List<TenderMenu>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "TenderMenuList");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TenderMenu items = new TenderMenu();
                    items.TypeName = Convert.ToString(dr["unitname"]);
                    items.SubMenuName = Convert.ToString(dr["SubMenuName"]);
                    items.SubMenu_local = Convert.ToString(dr["SubMenuName_local"]);
                    items.Order = Convert.ToInt32(dr["SrNo"]);
                    items.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    items.SubMenuId = Convert.ToInt32(dr["SubMenuId"]);
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

        public static int SaveRecord(TenderMenu model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("InsertTenderMenuRecord", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@mcid", model.MCId);
                cmd.Parameters.AddWithValue("@SubMenuName", model.SubMenuName);
                cmd.Parameters.AddWithValue("@SubMenuName_local", model.SubMenu_local);
                cmd.Parameters.AddWithValue("@SrNo", model.Order);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@Type", model.Type);
              
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
        public static List<TenderMenu> GetRowById(int id)
        {
            List<TenderMenu> lst = new List<TenderMenu>();
                try
                {
                    SqlConnection con = ClsConnection.GetConnection();
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Id", id);
                    con.Close();
                    foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetTenderMenuRecord", param).Tables[0].Rows)
                    {
                        TenderMenu item = new TenderMenu();
                        {
                            item.SubMenuName = Convert.ToString(dr["SubMenuName"]);
                            item.SubMenu_local = Convert.ToString(dr["SubMenuName_local"]);
                            item.Order = Convert.ToInt32(dr["SrNo"]);
                            item.Type = Convert.ToInt32(dr["Type"]);
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
            public static int UpdateRecord(TenderMenu model)
            {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("TenderMenuUpdateRecord", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
               
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@SubMenuName", model.SubMenuName);
                cmd.Parameters.AddWithValue("@SubMenuName_local", model.SubMenu_local);
                cmd.Parameters.AddWithValue("@SrNo", model.Order);
                cmd.Parameters.AddWithValue("@Type", model.Type);
                cmd.Parameters.AddWithValue("@Id", model.SubMenuId);
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
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[TenderMenuDeleteRecord]", parameterValues));
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