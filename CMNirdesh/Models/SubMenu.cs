using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    [Table("mSubMenu")]
    [Serializable]
    public class SubMenuModel2
    {
        [Key]
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public int Type { get; set; }

        public string MenuName { get; set; }
        public int MenuId { get; set; }

        public string icon { get; set; }
        public int SrNo { get; set; }

        public bool IsDeleted { get; set; }

        public static List<SubMenuModel2> GetSubMenuList()
        {
            List<SubMenuModel2> lstdept = new List<SubMenuModel2>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mSubMenuIndex");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SubMenuModel2 item = new SubMenuModel2();
                    item.SubMenuId = Convert.ToInt32(dr["SubMenuId"]);
                    item.Type = Convert.ToInt32(dr["Type"]);
                    item.SrNo = Convert.ToInt32(dr["SrNo"]);
                    item.SubMenuName = Convert.ToString(dr["SubMenuName"]);
                    item.ControllerName = Convert.ToString(dr["ControllerName"]);
                    item.ActionName = Convert.ToString(dr["ActionName"]);
                    item.MenuName = Convert.ToString(dr["MenuName"]);
                    item.icon = Convert.ToString(dr["icon"]);
                    item.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    lstdept.Add(item);
                }
                return lstdept;
            }
            catch
            {
                return lstdept;
            }

        }

        public static int SaveSubmenuRecord(SubMenuModel2 subMenuModel2)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mSubMenuInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuId", subMenuModel2.MenuId);
                cmd.Parameters.AddWithValue("@SubMenuName", subMenuModel2.SubMenuName);
                cmd.Parameters.AddWithValue("@ControllerName", subMenuModel2.ControllerName);
                cmd.Parameters.AddWithValue("@ActionName", subMenuModel2.ActionName);
                cmd.Parameters.AddWithValue("@Type", subMenuModel2.Type);
                cmd.Parameters.AddWithValue("@SrNo", subMenuModel2.SrNo);
                //cmd.Parameters.Add("@IsActive", mDepartment.IsActive);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@icon", SqlDbType.VarChar).Value = (subMenuModel2.icon == null) ? string.Empty : subMenuModel2.icon;
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<SubMenuModel2> GetSubMenuRecordById(int id)
        {
            List<SubMenuModel2> lstdept = new List<SubMenuModel2>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@SubMenuId", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mSubMenuGetID", param).Tables[0].Rows)
                {
                    SubMenuModel2 item = new SubMenuModel2();
                    {

                        item.SubMenuId = Convert.ToInt32(dr["SubMenuId"]);
                        item.SubMenuName = Convert.ToString(dr["SubMenuName"]);
                        item.ControllerName = Convert.ToString(dr["ControllerName"]);
                        item.ActionName = Convert.ToString(dr["ActionName"]);
                        item.MenuId = Convert.ToInt32(dr["MenuId"]);
                        item.MenuName = Convert.ToString(dr["MenuName"]);
                        item.icon = Convert.ToString(dr["icon"]);
                        item.Type = Convert.ToInt32(dr["Type"]);
                        item.SrNo = Convert.ToInt32(dr["SrNo"]);

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
        public static int UpdateSubMenuRecordById(SubMenuModel2 model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();


                SqlCommand cmd = new SqlCommand("mSubMenuUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubMenuId", model.SubMenuId);
                cmd.Parameters.AddWithValue("@SubMenuName", model.SubMenuName);
                cmd.Parameters.AddWithValue("@ControllerName", model.ControllerName);
                cmd.Parameters.AddWithValue("@ActionName", model.ActionName);
                cmd.Parameters.AddWithValue("@MenuId", model.MenuId);
                //cmd.Parameters.Add("@IsActive", mDepartment.IsActive);
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@icon", SqlDbType.VarChar).Value = (model.icon == null) ? string.Empty : model.icon;
                cmd.Parameters.AddWithValue("Type", model.Type);
                cmd.Parameters.AddWithValue("SrNo", model.SrNo);


                cmd.ExecuteNonQuery().ToString();



                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int DeleteSubMenuRecordById(int Id)
        {


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@SubMenuId", Id),


                };
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mSubMenuDelete]", parameterValues));
                connection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }
        public static List<SubMenuModel2> Max(int Id)
        {

            List<SubMenuModel2> lstdept = new List<SubMenuModel2>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@SubMenuId", Id);


              
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetMaxSrNo", param).Tables[0].Rows)
                {
                    SubMenuModel2 item = new SubMenuModel2();
                    {
                    
                        item.SrNo = Convert.ToInt32(dr["SrNo"]) +1;

                    };
                    lstdept.Add(item);
                }
                con.Close();
                return lstdept;
            }
            catch (Exception ex)
            {
                return lstdept;

            }

        }







        public class LstSubMenu
        {
            private List<SubMenuModel2> lstSubMenu2;

            public List<SubMenuModel2> _LstSubMenu2 { get => lstSubMenu2; set => lstSubMenu2 = value; }
        }


    }
}
