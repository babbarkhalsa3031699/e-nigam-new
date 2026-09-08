using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

using CMNirdesh.Models;
using CMNirdesh.Error;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace CMNirdesh.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string Photo { get; set; }
        public string Signature { get; set; }

        public string webtype { get; set; }

        public string DeptId { get; set; }

        public string DepartmentName { get; set; }
        public string OfficeName { get; set; }

        public int OfficeId { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int IsActive { get; set; }
        public bool Apex { get; set; }
        public int IsApex { get; set; }
        public SelectList RoleList { get; set; }
        public SelectList mDepartmentList { get; set; }

        public SelectList OfficeList { get; set; }

        public SelectList UserList { get; set; }

        public List<UserModel> _LstUser { get; set; }

        public string PhotoLocation { get; set; }

        public string PhotoFileName { get; set; }

        public string SignLocation { get; set; }

        public string SignFileName { get; set; }
        public string ModifiedBy { get; set; }

        public TimeSpan LoginTime { get; set; }
        public TimeSpan LogOutTime { get; set; }

        public string fileacessingUrl { get; set; }
        public void AddLoginUser(string userid2)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("LoginUserInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userid2);





                cmd.ExecuteNonQuery().ToString();
                con.Close();

            }
            catch (Exception ex)
            {

            }

        }
        public void AddLogoutUser(string UserId)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("LoginUserUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", UserId);
                DateTime currentDateTime = DateTime.Now;

                // Get only the time portion
                TimeSpan currentTime = currentDateTime.TimeOfDay;



                cmd.Parameters.AddWithValue("@LogoutTime", currentTime);

                cmd.ExecuteNonQuery().ToString();
                con.Close();

            }
            catch (Exception ex)
            {

            }

        }


        public static List<UserModel> GetUserDetails(UserModel model)
        {
            List<UserModel> lstitems = new List<UserModel>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetUserDetails");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    UserModel item = new UserModel();
                    item.Name = Convert.ToString(dr["Name"]);
                    item.Email = Convert.ToString(dr["Email"]);
                    item.MobileNo = Convert.ToString(dr["Mobile"]);
                    DateTime loginDateTime = Convert.ToDateTime(dr["LoginTime"]);
                    TimeSpan loginTime = new TimeSpan(loginDateTime.Hour, loginDateTime.Minute, 0);
                    item.LoginTime = loginTime;
                    DateTime logoutDateTime = Convert.ToDateTime(dr["LogOutTime"]);
                    TimeSpan logoutTime = new TimeSpan(logoutDateTime.Hour, logoutDateTime.Minute, 0);
                    item.LogOutTime = logoutTime;
                    lstitems.Add(item);
                }
                return lstitems;
            }
            catch
            {
                return lstitems;
            }

        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }


    public class PermissionModel
    {
        public string UserId { get; set; }
        public string NewUserId { get; set; }
        public int UserTypeId { get; set; }
        public SelectList UserList { get; set; }
        public SelectList RoleList { get; set; }
        public SelectList mDepartmentList { get; set; }
        public SelectList OfficeList { get; set; }
        public SelectList DesOfficeList { get; set; }
        public SelectList MultiOfficeList { get; set; }
        public int OfficeIdMulti { get; set; }
        //public SelectList MenuList { get; set; }
        public string DeptId { get; set; }
        public string DepartmentName { get; set; }
        public string OfficeName { get; set; }
        public int OfficeId { get; set; }
        public int ActionId { get; set; }

        public bool IsActive { get; set; }
        public bool IsOpen { get; set; }
        public List<MenuModel> MenuList { get; set; }

        public SelectList References { get; set; }
        public SelectList FundReferences { get; set; }

        public SelectList DispatchReferences { get; set; }
        public SelectList Actions { get; set; }
        public SelectList ActionFiles { get; set; }
        public SelectList ActionsDispatch { get; set; }

        public SelectList ActionsFileDispatch { get; set; }
        

        public List<MenuModel> addupdateList { get; set; }

        public SelectList DiaryButton { get; set; }
        public SelectList DispatchButton { get; set; }
        public SelectList ProjectActions { get; set; }
        public SelectList FundButton { get; set; }

        public int addupdate { get; set; }

        public string menuids { get; set; }

        public string refids { get; set; }

        public string actionids { get; set; }

        public string DispatchRefids { get; set; }

        public string Diaryactionids { get; set; }

        public string Dispatchactionids { get; set; }

        public string addupdateids { get; set; }

        public string Diaryaddupdateids { get; set; }
        public string Dispatchaddupdateids { get; set; }
        public string Projectaddupdateids { get; set; }


        public string depids { get; set; }
        public string officeids { get; set; }

    }



    public class Alerts
    {
        public List<AlertModel> MessageList { get; set; }
        public string MessageCount { get; set; }
        public List<AlertModel> alertList { get; set; }
        public string alertCount { get; set; }
    }

    public class AlertModel
    {
        public int NotificationId { get; set; }
        public Int64 refId { get; set; }
        public string createdDate { get; set; }
        public string alert { get; set; }
        public string alertType { get; set; }

        public string DocType { get; set; }

        public string IsLetter { get; set; }
        public string UserName { get; set; }
        public int NotificationType { get; set; }
        public string NotificationIcon { get; set; }

    }

    public class UserMenu
    {
        public List<MenuModel> MenuList { get; set; }
    }

    public class LstMenu
    {
        private List<MenuModel2> lstMenu;

        public List<MenuModel2> _LstMenu { get => lstMenu; set => lstMenu = value; }
    }



    public class MeetingVenueMenu
    {
        private List<MeetingVenueModel> lstVenueMenu;

        public List<MeetingVenueModel> _LstVenueMenu { get => lstVenueMenu; set => lstVenueMenu = value; }

      
    }



    public class MenuModel
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }

        public int MenuCount { get; set; }
        public string icon { get; set; }
        public List<SubMenuModel> _LstSubmenu { get; set; }

        

    }
    public class MenuModel2
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string icon { get; set; }
        public string UserType { get; set; }

        public bool IsDeleted { get; set; }



        

           




        public static List<MenuModel2> GetMenuList()
        {

            List<MenuModel2> lstdept = new List<MenuModel2>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mMenuIndex");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MenuModel2 item = new MenuModel2();
                    item.MenuId = Convert.ToInt32(dr["Id"]);
                    item.MenuName = Convert.ToString(dr["MenuName"]);
                    item.UserType = Convert.ToString(dr["UserType"]);
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



        public static int SaveMenuRecord(MenuModel2 model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mMenuInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuName", model.MenuName);
                cmd.Parameters.AddWithValue("@icon", SqlDbType.VarChar).Value =
       (model.icon == null) ? string.Empty : model.icon;
                //cmd.Parameters.Add("@IsActive", mDepartment.IsActive);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<MenuModel2> GetMenuModelRowById(int id)
        {
            List<MenuModel2> lstdept = new List<MenuModel2>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mMenuGetID", param).Tables[0].Rows)
                {
                    MenuModel2 item = new MenuModel2();
                    {

                        item.MenuId = Convert.ToInt32(dr["Id"]);
                        item.MenuName = Convert.ToString(dr["MenuName"]);
                        item.icon = Convert.ToString(dr["icon"]);

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
        public static int UpdateMenuRecordById(MenuModel2 menuModel2, string val)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mMenuUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@op", val);
                cmd.Parameters.AddWithValue("@Id", menuModel2.MenuId);
                cmd.Parameters.AddWithValue("@MenuName", menuModel2.MenuName);
                cmd.Parameters.AddWithValue("@icon", SqlDbType.VarChar).Value = (menuModel2.icon == null) ? string.Empty : menuModel2.icon;
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


        public static int DeleteMenuRowById(int Id)
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
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mMenuDelete]", parameterValues));
                connection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }



    }
    public class SubMenuModel
    {
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public int Type { get; set; }

        public string MenuName { get; set; }
        public int MenuId { get; set; }

        public int Count { get; set; }

    }



    public class MeetingVenueModel
    {
        public int MeetingVenueId { get; set; }
        public string MeetingVenueName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public bool IsDeleted { get; set; }

        public string MeetingVenueNameLocal { get; set; }
      

        public int Count { get; set; }

        public List<MeetingVenueModel> _MeetingVenueList { get; set; }

        public static List<MeetingVenueModel> GetMeetingVenue()
        {

            List<MeetingVenueModel> lstdept = new List<MeetingVenueModel>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetMeetingVenue");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MeetingVenueModel item = new MeetingVenueModel();
                    item.MeetingVenueId = Convert.ToInt32(dr["VenueId"]);
                    item.MeetingVenueName = Convert.ToString(dr["VenueName"]);
                    item.MeetingVenueNameLocal=Convert.ToString(dr["VenueName_local"]);
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




        public static int SaveMeetingVenueRecord(MeetingVenueModel model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mMeetingVenueInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VenueId", model.MeetingVenueId);
                cmd.Parameters.AddWithValue("@VenueName", model.MeetingVenueName);
                cmd.Parameters.AddWithValue("@VenueNameLocal", model.MeetingVenueNameLocal);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int UpdateMeetingVenueRecordById(MeetingVenueModel meetingModel2, string val)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mMeetingVenueUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@VenueId", meetingModel2.MeetingVenueId);
                cmd.Parameters.AddWithValue("@VenueName", meetingModel2.MeetingVenueName);
                cmd.Parameters.AddWithValue("@VenueNameLocal", meetingModel2.MeetingVenueNameLocal);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



        public static List<MeetingVenueModel> GetMeetingVenueModelRowById(int id)
        {
            List<MeetingVenueModel> lstdept = new List<MeetingVenueModel>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mMeetingVenueGetID", param).Tables[0].Rows)
                {
                    MeetingVenueModel item = new MeetingVenueModel();
                    {

                        item.MeetingVenueId = Convert.ToInt32(dr["VenueId"]);
                        item.MeetingVenueName = Convert.ToString(dr["VenueName"]);
                        item.MeetingVenueNameLocal = Convert.ToString(dr["VenueName_local"]);

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



        public static int DeleteMeetingVenueRowById(int Id)
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
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mMeetingVenueDelete]", parameterValues));
                connection.Close();
                return 1;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }


    }


    public class UserModelFunction
    {

        public static SelectList getRoles()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetRoles]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["role"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static string SaveUser(UserModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserName", model.UserName),
                    new SqlParameter("@Password", model.Password),
                    new SqlParameter("@Name", model.Name),
                    new SqlParameter("@Designation", model.Designation),
                    new SqlParameter("@MobileNo", model.MobileNo),
                     new SqlParameter("@Email", model.Email),
                    new SqlParameter("@DeptId", model.DeptId),
                    new SqlParameter("@OfficeId", model.OfficeId),
                     new SqlParameter("@Photo", model.Photo),
                      new SqlParameter("@Signature", model.Signature),
                      new SqlParameter("@RoleId", model.RoleId),
                       new SqlParameter("@IsActive", model.IsActive),
                       new SqlParameter("@IsApex", model.Apex),
                       new SqlParameter("@CreatedBy", CurrentSession.UserID)


                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[Insertuser_New]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static DataSet GetUserList()
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@FinYear", finyear) };
                ds = SqlHelper.ExecuteDataset(connection, "[getUserList]", null);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }
        public static DataSet GetMCWiseUserList()
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DeptID", CurrentSession.DeptID), new SqlParameter("@RoleID", Convert.ToInt32(CurrentSession.RoleID)) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMCWiseUserList]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }
        public static DataSet GetUserbyUserId(string userId)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserId", userId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetUserByUserId]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }
        public static DataSet GetDeptDtlsbyDeptId(string DeptId)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DeptId", DeptId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetDeptDtlsbyDeptId]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static SelectList getUserSelectList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getUserList]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["UserName"].ToString());
                    item.Value = Convert.ToString(row["userId"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList getUserTypeProjectActions(int UserTypeId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserTypeId", UserTypeId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetUserTypePriorityProjectActions]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionId"].ToString());
                    items.Add(item);

                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static DataSet GetUserTypePermission(int UserTypeId)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserTypeID", UserTypeId) };
                ds = SqlHelper.ExecuteDataset(connection, "[getUserTypePermission]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static SelectList getUserTypeDispatchUpdateList(int UserTypeId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserTypeId", UserTypeId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDispatchUserTypeActionApex]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionId"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList getAllUserSelectList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptid", deptid), new SqlParameter("@officeid", officeid) };
                ds = SqlHelper.ExecuteDataset(connection, "[getUserList]", null);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["UserName"].ToString());
                    item.Value = Convert.ToString(row["UserId"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

      

        public static SelectList getUserbyOfficeSelectList(string deptid, string officeid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptid", deptid), new SqlParameter("@officeid", officeid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetUserByOffice]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["name"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList getUserTypeDiaryUpdateList(int UserTypeId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserTypeId", UserTypeId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryUserTypeActionApex]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionId"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static List<MenuModel> PopulateUserTypeButtonMenu(int UserTypeId)
        {
            List<MenuModel> menu = new List<MenuModel>();

            DataSet ds = new DataSet();
            List<SelectListItem> items = new List<SelectListItem>();

            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserTypeId", UserTypeId) };
            ds = SqlHelper.ExecuteDataset(connection, "[GetUserTypeButtonsMenu]", parameterValues);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                MenuModel fr = new MenuModel();

                fr.MenuId = Convert.ToInt32(dr["MenuId"].ToString());
                fr.MenuName = dr["MenuName"].ToString();
                fr._LstSubmenu = GetUserTypeButtonSubMenu(fr.MenuId, UserTypeId);
                menu.Add(fr);

            }

            return menu;
        }




        private static List<SubMenuModel> GetUserTypeButtonSubMenu(int menuId, int UserTypeId)
        {
            List<SubMenuModel> submenu = new List<SubMenuModel>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MenuId", menuId), new SqlParameter("@UserTypeId", UserTypeId) };
            ds = SqlHelper.ExecuteDataset(connection, "[GetUserTypeButtons]", parameterValues);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                SubMenuModel fr = new SubMenuModel();

                fr.SubMenuId = Convert.ToInt32(dr["SubMenuId"].ToString());
                fr.SubMenuName = dr["SubMenuName"].ToString();
                submenu.Add(fr);

            }
            //menu.Add(new MenuModel
            //{
            //    //MenuName = sdr["FruitName"].ToString(),
            //    //MenuId = Convert.ToInt32(sdr["FruitId"])
            //});

            return submenu;
        }

        public static SelectList getUserTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(connection, "[GetUserType]", new object[0]);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["UserTypeName"].ToString());
                    item.Value = Convert.ToString(row["UserTypeID"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList getOfficeSelectListbyUser(string userid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid) };
                ds = SqlHelper.ExecuteDataset(connection, "[UserOffice]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["name"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }
        public static SelectList getOfficeSelectList(string deptid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptid", deptid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetOfficeByDepartments]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["name"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public DataTable GetOfficebyDepartmentId(string deptid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetOfficeByDepartments", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public static DataTable GetUserOffices(string userId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userId", userId));
            var dtt = SqlHelper.ExecuteDataset(connection, "[GetUserOffices]", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public static DataTable GetUserbyOfficeId(string deptid, string officeid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@officeid", officeid));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetUserByOffice", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public static DataTable GetUserbyOfficeIdForSend(string deptid, string officeid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@officeid", officeid));
            var dtt = SqlHelper.ExecuteDataset(connection, "[GetUserByOfficeForSend]", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public static DataTable AddOfficeDesignation(string userid, int officeid, string designation)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", userid));
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@designation", designation));
            var dtt = SqlHelper.ExecuteDataset(connection, "AddOfficeDesignation", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public static DataSet GetMenuList()
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                // SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetAllMenuList]", null);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static DataSet Getmenutype(int type)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@type", type) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMenutype]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        

             public static DataSet HomeDashboard(string deptid, string isDeptApex)
              {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DeptId", deptid), new SqlParameter("@isDeptApex", isDeptApex) };
                ds = SqlHelper.ExecuteDataset(connection, "[HouseDashboard]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }
        public static DataSet Getmenutype_dashboard(int type, int usertype)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@type", type), new SqlParameter("@usertype", usertype) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMenutype_new]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        /*  public static DataSet Getdashbaordmenutype(int type)
          {
              DataSet ds = new DataSet();


              try
              {
                  SqlConnection connection = ClsConnection.GetConnection();
                  if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                  {
                      connection.Open();
                  }
                  connection.Close();
                  SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@type", type) };
                  ds = SqlHelper.ExecuteDataset(connection, "[GetMenutype]", parameterValues);

              }
              catch (Exception ex)
              {
                  ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

              }
              return ds;
          }*/


        public static DataSet GetMenuList(string userid)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMenuList]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        public static DataSet GetSubMenuList(int menuId)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MenuId", menuId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetSubMenuList]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        public static DataSet GetSubMenuListByUser(int menuId, string userid, int AssignedOfficeId, int OfficeId)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MenuId", menuId), new SqlParameter("@userid", userid), new SqlParameter("@AssignedOfficeId", AssignedOfficeId), new SqlParameter("@SessToOfficeId", OfficeId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetSubMenuListbyUser]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static SelectList getReferences(string Deptid, string OfficeId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[2];
                parameterValues[0] = new SqlParameter("@DeptID", Deptid);
                parameterValues[1] = new SqlParameter("@OfficeId", OfficeId);

                ds = SqlHelper.ExecuteDataset(connection, "[GetDocumentsbyFund]", parameterValues);
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["DocumentType"].ToString());
                    item.Value = Convert.ToString(row["DocumentId"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList getFundReferences(string Deptid, string OfficeId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DeptID", Deptid), new SqlParameter("@OfficeId", OfficeId) };

                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDocumentsbyFund]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["DocumentType"].ToString());
                    item.Value = Convert.ToString(row["DocumentId"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList getReferencesDispatch()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(connection, "[GetDocumentsbyDispatch]", new object[0]);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["DocumentType"].ToString());
                    item.Value = Convert.ToString(row["DocumentId"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        

           public static SelectList getActionsFile()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetActionFileApex]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionCode"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList getActions()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetActionApex]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionCode"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        

            

            public static SelectList getActionsFileDisptach()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetActionApexFilesDispatch]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionCode"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList getActionsDisptach()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetActionApexDispatch]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionCode"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList getDiaryUpdateList(string UserId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserId", UserId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryUserActionApex]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionId"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList getDispatchUpdateList(string UserId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserId", UserId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDispatchUserActionApex]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionId"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList getButtons(string userid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserId", userid) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetButtons]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["SubmenuName"].ToString());
                    item.Value = Convert.ToString(row["SubMenuId"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }



        public static List<MenuModel> PopulateButtonMenu(string userid)
        {
            List<MenuModel> menu = new List<MenuModel>();
            DataSet ds = new DataSet();
            ds = UserModelFunction.getButtonsWithMenu(userid);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                MenuModel fr = new MenuModel();

                fr.MenuId = Convert.ToInt32(dr["MenuId"].ToString());
                fr.MenuName = dr["MenuName"].ToString();
                fr._LstSubmenu = GetButtonSubMenu(fr.MenuId, userid);
                menu.Add(fr);

            }

            return menu;
        }

        private static List<SubMenuModel> GetButtonSubMenu(int menuId, string userId)
        {
            List<SubMenuModel> submenu = new List<SubMenuModel>();
            DataSet ds = new DataSet();
            ds = UserModelFunction.getButtonsSubMenu(menuId, userId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                SubMenuModel fr = new SubMenuModel();

                fr.SubMenuId = Convert.ToInt32(dr["SubMenuId"].ToString());
                fr.SubMenuName = dr["SubMenuName"].ToString();
                submenu.Add(fr);

            }
            //menu.Add(new MenuModel
            //{
            //    //MenuName = sdr["FruitName"].ToString(),
            //    //MenuId = Convert.ToInt32(sdr["FruitId"])
            //});

            return submenu;
        }

        public static DataSet getButtonsWithMenu(string userid)
        {
            DataSet ds = new DataSet();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserId", userid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetButtonsMenu]", parameterValues);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }

            return ds;
        }


        public static DataSet getButtonsSubMenu(int menuId, string userid)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MenuId", menuId), new SqlParameter("@UserId", userid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetButtons]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        public static SelectList getDispatchButtons()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetButtons]", new object[0]).Tables[1].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ButtonName"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList getFundButtons()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetButtons]", new object[0]).Tables[2].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ButtonName"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }



        public static string AddMenu(string UserId, int MenuId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@MenuId", MenuId)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertUserMenu]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string RemoveMenu(string UserId, int MenuId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@MenuId", MenuId)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DeleteUserMenu]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string AddOffice(string UserId, int OfficeId, string DeptId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@OfficeId", OfficeId),
                     new SqlParameter("@DeptId", DeptId)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertUserOffice]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string RemoveOffice(string UserId, int OfficeId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@OfficeId", OfficeId)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DeleteUserOffice]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string AddUserDocument(string UserId, int DocTypeId, string type)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@DocTypeId", DocTypeId),
                     new SqlParameter("@Type", type)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertUserDocument]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string RemoveUserDocument(string UserId, int DocTypeId, string type)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@DocTypeId", DocTypeId),
                     new SqlParameter("@Type", type)
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DeleteUserDocument]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string AddUserAction(string UserId, int ActionId, string type)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@ActionId", ActionId),
                     new SqlParameter("@Type", type)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertUserAction]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string RemoveUserAction(string UserId, int ActionId, string type)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@ActionId", ActionId),
                     new SqlParameter("@Type", type)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DeleteUserAction]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }



        public static string AddUserButton(string UserId, int buttonId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@ButtonId", buttonId)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InserUserButtons]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string RemoveUserButton(string UserId, int buttonId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@ButtonId", buttonId)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DeleteUserButton]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string AddUserStatusButton(string UserId, int buttonId, string type)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@ButtonId", buttonId),
                      new SqlParameter("@Type", type)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertUserStatusButton]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string RemoveUserStatusButton(string UserId, int buttonId, string type)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@ButtonId", buttonId),
                     new SqlParameter("@Type", type)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DeleteUserStatusButton]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string ActiveUser(string UserId, int act)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),
                    new SqlParameter("@act", act)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UserActive]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static int DeleteUserRecordById(string UserId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserId", UserId),


                };
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[DeleteUserRecordById]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

        public static DataSet GetPermission(string UserId)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserId", UserId) };
                ds = SqlHelper.ExecuteDataset(connection, "[getPermission]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static string CheckPermission(string cont, string action, string userid, string type)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@cont", cont),
                    new SqlParameter("@action", action),
                    new SqlParameter("@userid",userid),
                     new SqlParameter("@type",type)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[CheckPermission]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static DataSet GetAlerts(string deptId, int Officeid, string userid, int isAlert)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DeptId", deptId), new SqlParameter("@OfficeId", Officeid), new SqlParameter("@UserId", userid), new SqlParameter("@isAlert", isAlert) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetNotifications]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static DataSet ClearAlerts(string deptId, int Officeid, string userid, string isalert)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DeptId", deptId), new SqlParameter("@OfficeId", Officeid), new SqlParameter("@UserId", userid), new SqlParameter("@isalert", isalert) };
                ds = SqlHelper.ExecuteDataset(connection, "[ClearNotifications]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        public static DataSet ClearAlertsbyId(int NotifId)
        {
            DataSet ds = new DataSet();


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@NotifId", NotifId) };
                ds = SqlHelper.ExecuteDataset(connection, "[ClearNotificationById]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }
        public static List<UserModel> GetUserDetailRowById(string id)
        {
            List<UserModel> lstitem = new List<UserModel>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetuserById", param).Tables[0].Rows)
                {
                    UserModel item = new UserModel();
                    {
                        item.UserName = Convert.ToString(dr["UserName"]);
                        string pwd = Convert.ToString(dr["Password"]);
                        byte[] data2 = System.Convert.FromBase64String(pwd);
                        item.Password = System.Text.ASCIIEncoding.ASCII.GetString(data2);

                        item.MobileNo = Convert.ToString(dr["MobileNo"]);
                        item.Email = Convert.ToString(dr["EmailId"]);
                        item.Name = Convert.ToString(dr["Name"]);
                        item.Designation = Convert.ToString(dr["Designation"]);
                        item.DeptId = Convert.ToString(dr["DeptId"]);
                        item.DepartmentName = Convert.ToString(dr["deptname"]);
                        item.OfficeId = Convert.ToInt32(dr["OfficeId"]);
                        item.RoleId = 0;
                        if (dr["RoleID"] != System.DBNull.Value)
                        {
                            item.RoleId = Convert.ToInt32(dr["RoleID"]);
                        }
                        item.OfficeName = Convert.ToString(dr["officename"]);
                        item.PhotoLocation = Convert.ToString(dr["Photo"]);
                        item.SignLocation = Convert.ToString(dr["SignaturePath"]);
                        if (dr["Apex"] != System.DBNull.Value)
                        {
                            item.Apex = Convert.ToBoolean(dr["Apex"]);
                        }

                    };
                    lstitem.Add(item);
                    con.Close();
                }

                return lstitem;
            }
            catch (Exception ex)
            {
                return lstitem;

            }
        }

        public static int UpdateUserById(UserModel model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("Updateuser", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", model.UserName);
                // cmd.Parameters.AddWithValue("@Password", model.Password);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Designation", model.Designation);
                cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@DeptId", model.DeptId);
                cmd.Parameters.AddWithValue("@OfficeId", model.OfficeId);
                cmd.Parameters.AddWithValue("@RoleId", model.RoleId);
                cmd.Parameters.AddWithValue("@Photo", SqlDbType.VarChar).Value = (model.PhotoLocation == null) ? string.Empty : model.PhotoLocation;
                cmd.Parameters.AddWithValue("@Signature", SqlDbType.VarChar).Value = (model.SignLocation == null) ? string.Empty : model.SignLocation;
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@IsApex", Convert.ToInt32(model.Apex));


                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static SelectList getProjectActions(string UserId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserId", UserId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetPriorityProjectActions]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString());
                    item.Value = Convert.ToString(row["ActionId"].ToString());
                    items.Add(item);

                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static bool GetMenuPermissionUser(string Menuname, string userid)
        {
            int s = 0;
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid), new SqlParameter("@MenuName", Menuname) };

                s = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[CheckPermission]", parameterValues));
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return Convert.ToBoolean(s);

        }

        public static string UpdateUserTypeMenu(string UserTypeId, int MenuId, string Flag)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserTypeId", UserTypeId),
                    new SqlParameter("@MenuId", MenuId),
                    new SqlParameter("@Flag", Flag)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateUserTypeMenu]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string UpdateUserTypeDiaryRefAction(string UserTypeId, int DocTypeId, string Flag)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserTypeId", UserTypeId),
                    new SqlParameter("@DocTypeId", DocTypeId),
                    new SqlParameter("@Flag", Flag)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateUserTypeDiaryRefAction]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string UpdateUserTypeDispatchRefAction(string UserTypeId, int DocTypeId, string Flag)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserTypeId", UserTypeId),
                    new SqlParameter("@DocTypeId", DocTypeId),
                    new SqlParameter("@Flag", Flag)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateUserTypeDispatchRefAction]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string UpdateUserTypeDiaryAction(string UserTypeId, int ActionId, string Flag)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserTypeId", UserTypeId),
                    new SqlParameter("@ActionId", ActionId),
                    new SqlParameter("@Flag", Flag)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateUserTypeDiaryAction]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string UpdateUserTypeProjectAction(string UserTypeId, int ActionId, string Flag)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserTypeId", UserTypeId),
                    new SqlParameter("@ActionId", ActionId),
                    new SqlParameter("@Flag", Flag)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateUserTypeProjectAction]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string UpdateUserTypeDispatchAction(string UserTypeId, int ActionId, string Flag)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserTypeId", UserTypeId),
                    new SqlParameter("@ActionId", ActionId),
                    new SqlParameter("@Flag", Flag)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateUserTypeDispatchAction]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string UpdateUserTypeDiaryAddUpdateAction(string UserTypeId, int ActionId, string Flag)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserTypeId", UserTypeId),
                    new SqlParameter("@ActionId", ActionId),
                    new SqlParameter("@Flag", Flag)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateUserTypeDiaryAddUpdateAction]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string UpdateUserTypeDispatchAddUpdateAction(string UserTypeId, int ActionId, string Flag)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@UserTypeId", UserTypeId),
                    new SqlParameter("@ActionId", ActionId),
                    new SqlParameter("@Flag", Flag)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateUserTypeDispatchAddUpdateAction]", parameterValues));
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



