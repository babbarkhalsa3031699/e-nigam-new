using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;

namespace CMNirdesh.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username required")]

        [Display(Name = "User name")]

        [RegularExpression(@"^[\w_]+$", ErrorMessage = "Please Enter Valid User Name")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string confirmPassword { get; set; }
        [Required]
        [StringLength(5)]
        public string userOTP { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string returnUrl { get; set; }

        public string LoginDepartmentName { get; set; }

        public string CaptchaMessage { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string ActualPassword { get; set; }

        public string ErrorMessage { get; set; }
        public int UserType { get; set; }



        public string HeaderName { get; set; }
        public string MCName { get; set; }
        public string DisplayMCName { get; set; }
        public string DisplayConName { get; set; }
        public String Address { get; set; }
        public string Houses { get; set; }
        public string Councilors { get; set; }
        public string HouseMeetings { get; set; }
        public string Departments { get; set; }
        public string FCCMeetings { get; set; }
        public string Employees { get; set; }
        public SelectList MCList { get; set; }
        public int MCId { get; set; }


        public string MName { get; set; }

        public string MCName_Local { get; set; }

        public string MCLogo { get; set; }

        public string SiteUrl { get; set; }
        public string IsRegister { get; set; }

        public static List<MCHomeLabel> GetSiteLabel(string lang)
        {
            List<MCHomeLabel> Mem = new List<MCHomeLabel>();
            DataSet ds = new DataSet();
            ds = GetSiteLabelData();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                MCHomeLabel lstMem = new MCHomeLabel();
                lstMem.GovtTittle = dr["GovtTitle"].ToString();
                lstMem.GovtTittle_local = dr["GovtTitle_local"].ToString();
                lstMem.SiteTittle = dr["SiteTitle" + lang].ToString();
                lstMem.StateSubTittle = dr["StateSubTitle" + lang].ToString();
                lstMem.SubTittle = dr["SubTitle" + lang].ToString();
                lstMem.MCTittle = dr["MCTitle" + lang].ToString();
                lstMem.CounsilTittle = dr["CounsilTitle" + lang].ToString();
                lstMem.LoginTittle = dr["LoginTitle" + lang].ToString();
                lstMem.SitemapTittle = dr["SitemapTitle" + lang].ToString();
                lstMem.NoticeTittle = dr["NoticeTitle" + lang].ToString();
                lstMem.HousesTittle = dr["HousesTitle" + lang].ToString();
                lstMem.Councilors = dr["Councilors" + lang].ToString();
                lstMem.Meetings = dr["Meetings" + lang].ToString();
                lstMem.Departments = dr["Departments" + lang].ToString();
                lstMem.FCC = dr["FCC" + lang].ToString();
                lstMem.Employees = dr["Employees" + lang].ToString();
                lstMem.StatusPanel = dr["StatusPanel" + lang].ToString();
                lstMem.MemberListTittle = dr["MemberListTitle" + lang].ToString();
                lstMem.MemberListDescription = dr["MemberListDescription" + lang].ToString();
                lstMem.NewsTittle = dr["NewsTitle" + lang].ToString();
                lstMem.NewsDescription = dr["NewsDescription" + lang].ToString();
                lstMem.GallaryTittle = dr["GalleryTitle" + lang].ToString();
                lstMem.HouseResolutions = dr["HouseResolutions" + lang].ToString();
                lstMem.HouseBusiness = dr["HouseBusiness" + lang].ToString();
                lstMem.FCCResolutions = dr["FCCResolutions" + lang].ToString();
                lstMem.alert = dr["alert" + lang].ToString();
                Mem.Add(lstMem);
            }
            return Mem;

        }

        public static DataSet GetSiteLabelData()
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
                // SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCId", MCId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMCHomelabel]", null);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }




        public static DataSet GetDashboardData(int MCId)
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

                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCid", MCId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetHomeDashboard]", parameterValues);
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }
            return ds;
        }





        public static LoginModel GetMCId(string url)
        {

            DataSet ds = new DataSet();
            LoginModel fr = new LoginModel();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@url", url) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetSiteId]", parameterValues);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    
              
                    fr.MCName = Convert.ToString(ds.Tables[0].Rows[0]["MCName"]);
                    fr.MCName_Local = Convert.ToString(ds.Tables[0].Rows[0]["MCName_Local"]);
                    fr.MCLogo = Convert.ToString(ds.Tables[0].Rows[0]["MClogo"]);
                    fr.SiteUrl = Convert.ToString(ds.Tables[0].Rows[0]["SiteUrl"]);


                }

               
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return fr;

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


        public static bool IsPreviousUser(string password, string UserName)
        {

            password = password.Substring(0, 64);
            DataSet ds = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@Username", UserName), new SqlParameter("@Password", password) };
                ds = SqlHelper.ExecuteDataset(connection, "GetPreviousPasswordHistory", parameterValues);
                DataSet st = new DataSet();
                if (ds != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return false;
            }
        }

        static bool IsPasswordValid(string password)
        {
            // Check length
            if (password.Length < 8)
            {
                return false;
            }

            // Check for at least one digit
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            // Check for at least one lowercase letter
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            // Check for at least one uppercase letter
            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            // Check for at least one special character
            string specialCharacters = "@$!%*?&";
            if (!password.Any(c => specialCharacters.Contains(c)))
            {
                return false;
            }

            return true;
        }

    }



}