using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models.MCHouse
{
    public class MCSiteSubMenu
    {
        [Key]
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public string SubMenuNameLocal { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public int Type { get; set; }

        public string MCName { get; set; }
        public string MCId { get; set; }
        public string MenuName { get; set; }
        public int MenuId { get; set; }

        public string icon { get; set; }
        public int linktype { get; set; }
        public int isLink { get; set; }
        public string linkurl { get; set; }
        public string pdf { get; set; }
        public string pdfYN { get; set; }
        public int SrNo { get; set; }
        public int SubmenuOrderNo { get; set; }

        public bool IsDeleted { get; set; }
        private List<MCSiteSubMenu> lstSubMenu;
        public string fileacessingUrl { get; set; }
        public List<MCSiteSubMenu> _LstSubMenu { get => lstSubMenu; set => lstSubMenu = value; }
        public static List<MCSiteSubMenu> GetSubMenuList()
        {
            List<MCSiteSubMenu> lstdept = new List<MCSiteSubMenu>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@MCId", CurrentSession.StateId);
                param[1] = new SqlParameter("@RoleId", CurrentSession.RoleID);

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mMCSiteSubMenu",param);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MCSiteSubMenu item = new MCSiteSubMenu();
                    item.SubMenuId = Convert.ToInt32(dr["SubMenuId"]);
                    item.Type = Convert.ToInt32(dr["Type"]);
                    if(System.DBNull.Value != dr["SrNo"])
                    {
                        item.SrNo = Convert.ToInt32(dr["SrNo"]);
                    }
                    item.MCName = Convert.ToString(dr["MCName"]);
                    item.SubMenuName = Convert.ToString(dr["SubMenuName"]);
                    item.SubMenuNameLocal = Convert.ToString(dr["SubMenuName_local"]);
                    item.ControllerName = Convert.ToString(dr["ControllerName"]);
                    item.ActionName = Convert.ToString(dr["ActionName"]);
                    item.MenuName = Convert.ToString(dr["MenuName"]);
                    item.icon = Convert.ToString(dr["icon"]);
                    item.linkurl = Convert.ToString(dr["FilePath"]);
                   // item.isLink = Convert.ToInt16(dr["isLink"]);
                   
                    item.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    lstdept.Add(item);
                }
                return lstdept;
            }
            catch(Exception ex)
            {
                return lstdept;
            }

        }

        public static int SaveSubmenuRecord(MCSiteSubMenu MCSiteSubMenu)
        {



            if(MCSiteSubMenu.MCName=="LG")
            {
                try
                {
                    string controllerName = "";
                    string actionName = "";
                    if (MCSiteSubMenu.linktype.ToString() == "1")
                    {
                        MCSiteSubMenu.isLink = 2;
                    }
                    else if (MCSiteSubMenu.linktype.ToString() == "2")
                    {
                        MCSiteSubMenu.isLink = 0;
                        string[] array = MCSiteSubMenu.linkurl.Split('/');
                        if (array.Length > 1)
                        {
                            controllerName = array[0].ToString();
                            actionName = array[1].ToString();
                        }
                    }
                    else if (MCSiteSubMenu.linktype.ToString() == "3")
                    {
                        MCSiteSubMenu.isLink = 1;
                    }

                    SqlConnection con = ClsConnection.GetConnection();
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("mLGSiteSubMenuInsert", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MenuId", MCSiteSubMenu.MenuId);
                    cmd.Parameters.AddWithValue("@SubMenuName", MCSiteSubMenu.SubMenuName);
                    cmd.Parameters.AddWithValue("@SubMenuNameLocal", MCSiteSubMenu.SubMenuNameLocal);
                    cmd.Parameters.AddWithValue("@SrNo", MCSiteSubMenu.SrNo);
                    cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                    cmd.Parameters.AddWithValue("@icon", SqlDbType.VarChar).Value = (MCSiteSubMenu.icon == null) ? string.Empty : MCSiteSubMenu.icon;
                    cmd.Parameters.AddWithValue("@linktype", MCSiteSubMenu.linktype);
                    cmd.Parameters.AddWithValue("@isLink", MCSiteSubMenu.isLink);
                    if (MCSiteSubMenu.linktype.ToString() == "3")
                    {
                        cmd.Parameters.AddWithValue("@linkurl", SqlDbType.VarChar).Value = (MCSiteSubMenu.pdf == null) ? string.Empty : MCSiteSubMenu.pdf;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@linkurl", SqlDbType.VarChar).Value = (MCSiteSubMenu.linkurl == null) ? string.Empty : MCSiteSubMenu.linkurl;
                    }
                    cmd.Parameters.AddWithValue("@controllerName", SqlDbType.VarChar).Value = (controllerName == null) ? string.Empty : controllerName;
                    cmd.Parameters.AddWithValue("@actionName", SqlDbType.VarChar).Value = (actionName == null) ? string.Empty : actionName;
                    cmd.ExecuteNonQuery().ToString();
                    con.Close();
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            else
            {
                try
                {
                    string controllerName = "";
                    string actionName = "";
                    if (MCSiteSubMenu.linktype.ToString() == "1")
                    {
                        MCSiteSubMenu.isLink = 2;
                    }
                    else if (MCSiteSubMenu.linktype.ToString() == "2")
                    {
                        MCSiteSubMenu.isLink = 0;
                        string[] array = MCSiteSubMenu.linkurl.Split('/');
                        if (array.Length > 1)
                        {
                            controllerName = array[0].ToString();
                            actionName = array[1].ToString();
                        }
                    }
                    else if (MCSiteSubMenu.linktype.ToString() == "3")
                    {
                        MCSiteSubMenu.isLink = 1;
                    }

                    SqlConnection con = ClsConnection.GetConnection();
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("mMCSiteSubMenuInsert", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MenuId", MCSiteSubMenu.MenuId);
                    cmd.Parameters.AddWithValue("@SubMenuName", MCSiteSubMenu.SubMenuName);
                    cmd.Parameters.AddWithValue("@SubMenuNameLocal", MCSiteSubMenu.SubMenuNameLocal);
                    cmd.Parameters.AddWithValue("@SrNo", MCSiteSubMenu.SrNo);
                    cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                    cmd.Parameters.AddWithValue("@icon", SqlDbType.VarChar).Value = (MCSiteSubMenu.icon == null) ? string.Empty : MCSiteSubMenu.icon;
                    cmd.Parameters.AddWithValue("@linktype", MCSiteSubMenu.linktype);
                    cmd.Parameters.AddWithValue("@isLink", MCSiteSubMenu.isLink);
                    if (MCSiteSubMenu.linktype.ToString() == "3")
                    {
                        cmd.Parameters.AddWithValue("@linkurl", SqlDbType.VarChar).Value = (MCSiteSubMenu.pdf == null) ? string.Empty : MCSiteSubMenu.pdf;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@linkurl", SqlDbType.VarChar).Value = (MCSiteSubMenu.linkurl == null) ? string.Empty : MCSiteSubMenu.linkurl;
                    }
                    cmd.Parameters.AddWithValue("@controllerName", SqlDbType.VarChar).Value = (controllerName == null) ? string.Empty : controllerName;
                    cmd.Parameters.AddWithValue("@actionName", SqlDbType.VarChar).Value = (actionName == null) ? string.Empty : actionName;
                    cmd.ExecuteNonQuery().ToString();
                    con.Close();
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
          
        }

        public static List<MCSiteSubMenu> GetSubMenuRecordById(int id)
        {
            List<MCSiteSubMenu> lstdept = new List<MCSiteSubMenu>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@SubMenuId", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mMCSiteSubMenuGetID", param).Tables[0].Rows)
                {
                    MCSiteSubMenu item = new MCSiteSubMenu();
                    {

                        item.SubMenuId = Convert.ToInt32(dr["SubMenuId"]);
                        item.MCId = Convert.ToString(dr["MCId"]);
                        item.SubMenuName = Convert.ToString(dr["SubMenuName"]);
                        item.SubMenuNameLocal = Convert.ToString(dr["SubMenuName_local"]);
                        item.ControllerName = Convert.ToString(dr["ControllerName"]);
                        item.ActionName = Convert.ToString(dr["ActionName"]);
                        item.MenuId = Convert.ToInt32(dr["MenuId"]);
                        item.MenuName = Convert.ToString(dr["MenuName"]);
                        item.icon = Convert.ToString(dr["icon"]);
                        item.linkurl = Convert.ToString(dr["FilePath"]);
                        item.pdfYN = "N";
                        
                        item.Type = Convert.ToInt32(dr["Type"]);
                        if (System.DBNull.Value != dr["SrNo"])
                        {
                            item.SrNo = Convert.ToInt32(dr["SrNo"]);
                        }
                        if (System.DBNull.Value != dr["linktype"])
                        {
                            item.linktype = Convert.ToInt32(dr["linktype"]);
                            if(item.linktype.ToString()=="3")
                            {
                                item.pdfYN = "Y";
                                //item.pdf = BlobStorage.GetStorageAcessingPath() + "/" + Convert.ToString(dr["FilePath"]);
                                item.pdf =   Convert.ToString(dr["FilePath"]);
                            }
                        }
                        //item.linkurl = Convert.ToString(dr["linkurl"]);

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
        public static int UpdateSubMenuRecordById(MCSiteSubMenu model)
        {
            try
            {
                string controllerName = "";
                string actionName = "";
                if (model.linktype.ToString() == "1")
                {
                    model.isLink = 2;
                }
                else if (model.linktype.ToString() == "2")
                {
                    model.isLink = 0;
                    string[] array = model.linkurl.Split('/');
                    if (array.Length > 1)
                    {
                        controllerName = array[0].ToString();
                        actionName = array[1].ToString();
                    }
                }
                else if (model.linktype.ToString() == "3")
                {
                    model.isLink = 1;
                }

                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();


                SqlCommand cmd = new SqlCommand("mMCSiteSubMenuUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubMenuId", model.SubMenuId);
                cmd.Parameters.AddWithValue("@SubMenuName", model.SubMenuName);
                cmd.Parameters.AddWithValue("@SubMenuNameLocal", model.SubMenuNameLocal);
                cmd.Parameters.AddWithValue("@MenuId", model.MenuId);
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@icon", SqlDbType.VarChar).Value = (model.icon == null) ? string.Empty : model.icon;
                cmd.Parameters.AddWithValue("@SrNo", model.SrNo);
                cmd.Parameters.AddWithValue("@linktype", model.linktype);
                cmd.Parameters.AddWithValue("@isLink", model.isLink);
                if (model.linktype.ToString() == "3")
                {
                    cmd.Parameters.AddWithValue("@linkurl", SqlDbType.VarChar).Value = (model.pdf == null) ? string.Empty : model.pdf;
                }
                else
                {
                    cmd.Parameters.AddWithValue("@linkurl", SqlDbType.VarChar).Value = (model.linkurl == null) ? string.Empty : model.linkurl;
                }
                cmd.Parameters.AddWithValue("@controllerName", SqlDbType.VarChar).Value = (controllerName == null) ? string.Empty : controllerName;
                cmd.Parameters.AddWithValue("@actionName", SqlDbType.VarChar).Value = (actionName == null) ? string.Empty : actionName;

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
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mMCSiteSubMenuDelete]", parameterValues));
                connection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }
        public static List<MCSiteSubMenu> Max(int Id)
        {

            List<MCSiteSubMenu> lstdept = new List<MCSiteSubMenu>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@SubMenuId", Id);



                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetMCSiteSubmenuMaxSrNo", param).Tables[0].Rows)
                {
                    MCSiteSubMenu item = new MCSiteSubMenu();
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
}