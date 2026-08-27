using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models.MCHouse
{
    [Serializable]
    [Table("mMCHomeMenu")]
    public class MMCHomeMenu
    {
        [Key]
        public int Id { get; set; }
        public int PageId { get; set; }
        public int MCId { get; set; }
        public int Type { get; set; }
        public int MenuType { get; set; }
        public int MenuOfcType { get; set; }
        public string MCName { get; set; }
        public string MenuName { get; set; }
        public string MenuName_local { get; set; }
        public int SrNo { get; set; }
        public string icon { get; set; }
        public int islink { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; }

        private List<MMCHomeMenu> lstMenu;
        public List<MMCHomeMenu> _LstMenu { get => lstMenu; set => lstMenu = value; }
        public static List<MMCHomeMenu> GetMCMenuList(int MCId)
        {

            List<MMCHomeMenu> lstitem = new List<MMCHomeMenu>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Id", MCId);
                param[1] = new SqlParameter("@roleId", Convert.ToInt32(CurrentSession.RoleID));
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mMCHomeMenuList",param);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MMCHomeMenu items = new MMCHomeMenu();
                    items.MCName = Convert.ToString(dr["MCName"]);
                    items.MenuName = Convert.ToString(dr["MenuName"]);
                    items.MenuName_local = Convert.ToString(dr["MenuName_local"]);
                    items.icon = Convert.ToString(dr["icon"]);
                    items.Url = Convert.ToString(dr["Url"]);
                   
                    items.Id = Convert.ToInt32(dr["Id"]);
                    items.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    items.SrNo = Convert.ToInt32(dr["MenuOrder"]);
                    lstitem.Add(items);
                }
                return lstitem;
            }
            catch
            {
                return lstitem;
            }

        }

        public static string SaveMCRecord(MMCHomeMenu model)
        {
            try
            {
                string mcid = "1";
                if (model.MenuOfcType.ToString() == "2")
                {
                    mcid = Convert.ToString(CurrentSession.StateId);
                    if (CurrentSession.RoleID == "18")
                    {
                        mcid = model.MCId.ToString();
                    }
                }
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@MenuName",Convert.ToString(model.MenuName)),
                new SqlParameter("@MenuName_local",Convert.ToString(model.MenuName_local)),
                new SqlParameter("@Url", (model.Url == null) ? string.Empty :Convert.ToString(model.Url)),
                new SqlParameter("@icon",Convert.ToString(model.icon)),
                new SqlParameter("@MCId",Convert.ToString(mcid)),
                new SqlParameter("@CreatedBy", Convert.ToString(CurrentSession.UserID)),
                new SqlParameter("@SrNo", Convert.ToInt32(model.SrNo)),
                new SqlParameter("@Type", Convert.ToInt32(CurrentSession.officeType)),
                new SqlParameter("@MenuOfcType", Convert.ToInt32(model.MenuOfcType)),
                new SqlParameter("@MenuType", Convert.ToInt32(model.MenuType)),
                new SqlParameter("@isLink", Convert.ToInt32(model.islink))
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(con, "[mcHomeMenuInsert]", parameterValues));
                con.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, "Error at MChomemenu");
                return "Some error occurs, Please try again";
            }
        }
        public static List<MMCHomeMenu> GetMCRecordById(int id)
        {
            List<MMCHomeMenu> lstItem = new List<MMCHomeMenu>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mcMenuRecordGetByid", param).Tables[0].Rows)
                {
                    MMCHomeMenu item = new MMCHomeMenu();
                    {
                        item.MenuName = Convert.ToString(dr["MenuName"]);
                        item.MenuName_local = Convert.ToString(dr["MenuName_local"]);
                        item.Type = Convert.ToInt32(dr["Type"]);
                        item.Url = Convert.ToString(dr["Url"]);
                        item.icon = Convert.ToString(dr["icon"]);
                        item.MCId = Convert.ToInt32(dr["MCId"]);
                        if (System.DBNull.Value != dr["PageId"])
                        {
                            item.PageId = Convert.ToInt32(dr["PageId"]);
                        }
                        item.SrNo = Convert.ToInt32(dr["SrNo"]);
                        item.Id = Convert.ToInt32(dr["Id"]);
                        if (dr["islink"] != System.DBNull.Value)
                        {
                            item.islink = Convert.ToInt32(dr["islink"]);
                        }
                        item.MenuType = 0;
                        item.MenuOfcType = 0;
                        if (dr["menuType"] != System.DBNull.Value)
                        {
                            item.MenuType = Convert.ToInt16(dr["menuType"]);
                        }
                        if (dr["MenuOfcType"] != System.DBNull.Value)
                        {
                            item.MenuOfcType = Convert.ToInt16(dr["MenuOfcType"]);
                        }
                    };
                    lstItem.Add(item);
                }

                return lstItem;
            }
            catch (Exception ex)
            {
                return lstItem;

            }
        }


        public static int UpdateMCRecord(MMCHomeMenu model)
        {
            try
            {
                string mcid = "1";
                if (model.MenuOfcType.ToString() == "2")
                {
                    mcid = Convert.ToString(CurrentSession.StateId);
                    if (CurrentSession.RoleID == "18")
                    {
                        mcid = model.MCId.ToString();
                    }
                }
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mMCHomeMenuUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuName", model.MenuName);
                cmd.Parameters.AddWithValue("@MenuName_local", model.MenuName_local);
                cmd.Parameters.AddWithValue("@Type", SqlDbType.VarChar).Value = CurrentSession.officeType;
                cmd.Parameters.AddWithValue("@Url", SqlDbType.VarChar).Value = model.Url ?? string.Empty;
                cmd.Parameters.AddWithValue("@icon", SqlDbType.VarChar).Value = (model.icon == null) ? string.Empty : model.icon;
                cmd.Parameters.AddWithValue("@MCId", SqlDbType.VarChar).Value = mcid;
                cmd.Parameters.AddWithValue("@PageId", SqlDbType.VarChar).Value = model.PageId;
                cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.VarChar).Value = (CurrentSession.UserID == null) ? string.Empty : CurrentSession.UserID;
                cmd.Parameters.AddWithValue("@SrNo", model.SrNo);
                cmd.Parameters.AddWithValue("@MenuOfcType", Convert.ToInt16(model.MenuOfcType));
                cmd.Parameters.AddWithValue("@MenuType", Convert.ToInt16(model.MenuType));
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@isLink", Convert.ToInt16(model.islink));
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int DeleteMCRecordById(int Id)
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
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mMCHomeMenuDeleteRowById]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }
        public static List<MMCHomeMenu> Max(int Id)
        {

            List<MMCHomeMenu> lstdept = new List<MMCHomeMenu>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@MCId", Id);



                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetMCmenuMaxSrNo", param).Tables[0].Rows)
                {
                    MMCHomeMenu item = new MMCHomeMenu();
                    {
                        item.SrNo = 1;
                        if (System.DBNull.Value != dr["SrNo"])
                        {
                            item.SrNo = Convert.ToInt32(dr["SrNo"]) + 1;
                        }

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

    }
    public class LstMCHomeMenuModel
        {
            public List<MMCHomeMenu> _ListMCHomeMenuModel { get; set; }
            public string fileacessingUrl { get; set; }
    }

    }
