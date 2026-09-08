using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMNirdesh.Models
{

    public class MCHomeLabel
    {
        public string GovtTittle { get; set; }
        public string GovtTittle_local { get; set; }
        public string SiteTittle { get; set; }
        public string StateSubTittle { get; set; }
        public string SubTittle { get; set; }
        public string MCTittle { get; set; }
        public string CounsilTittle { get; set; }
        public string LoginTittle { get; set; }
        public string SitemapTittle { get; set; }
        public string NoticeTittle { get; set; }
        public string HousesTittle { get; set; }
        public string Councilors { get; set; }
        public string Meetings { get; set; }
        public string Departments { get; set; }
        public string FCC { get; set; }
        public string Employees { get; set; }
        public string StatusPanel { get; set; }
        public string MemberListTittle { get; set; }
        public string MemberListDescription { get; set; }
        public string NewsTittle { get; set; }
        public string NewsDescription { get; set; }
        public string GallaryTittle { get; set; }

        public string Logo { get; set; }

        public string SiteUrl { get; set; }

        public string HouseBusiness { get; set; }
        public string HouseResolutions { get; set; }
        public string FCCResolutions { get; set; }
        public string alert { get; set; }
        

    }

    public class MCHomeModel
    {
        public string HeaderName { get; set; }
        public string MCName{ get; set; }
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
        public int IsMC   { get; set; }

        public string Language { get; set; }
        public string HeadName { get; set; }

        public string SubHeadName { get; set; }

        public string HeadDesignation { get; set; }
        public string SubHeadDesignation { get; set; }
        public string HeadPhoto { get; set; }
        public string SubHeadPhoto { get; set; }

        public string HeadMessage { get; set; }

        public string SubHeadMessage { get; set; }
        public string About { get; set; }

        public string MCCount { get; set; }
        public List<MC> _MCList { get; set; }

        public string ConsCount { get; set; }
        public List<MC> _ConsList { get; set; }
        public List<Member> _Members { get; set; }

        public List<News> _News { get; set; }

        public List<News> _FlashNews { get; set; }
        public List<Gallary> _Gallary { get; set; }

        public List<Tender> _Tenders { get; set; }

        public List<Member> _TodayBirthdays { get; set; }
        public List<Member> _BirthdaysList { get; set; }

        public List<MCHomeLabel>  _MCHomeLabelList { get; set; }
        public MCHomeLabel _MCHomelabel { get; set; }

        public List<HomeMenu> MenuList { get; set; }

        public List<HouseDashboard> HouseBusiness { get; set; }

        public List<HouseDashboard> HouseResolutions { get; set; }

        public List<HouseDashboard> FCCResolutions { get; set; }

    }



    public class MC
    {
        public String MCId { get; set; }
        public String MCName { get; set; }
        public String MCName_local { get; set; }
        public String Address { get; set; }
        public String Icon { get; set; }
    }

    public class Member
    {
        public String MemberPreFix { get; set; }
        public String MemberName { get; set; }
        public String MemberPreFix_local { get; set; }
        public String MemberName_local { get; set; }
        public String Address { get; set; }
        public String Gender { get; set; }
        public String MFGender { get; set; }
        public String MobileNo { get; set; }
        public String Designation { get; set; }
        public String ImagePath { get; set; }

        public string BirthDate { get; set; }
        public string BirthYear { get; set; }
    }

    public class News
    {
        public int NewsId { get; set; }
        public string NewsName { get; set; }
        public string NewsDate  { get; set; }
        public string ImagePath { get; set; }
    }

    public class Gallary
    {
        public int Id { get; set; }
        public string ImageTittle { get; set; }
        public string ImageDate { get; set; }
        public string ImagePath { get; set; }
    }


    public class Tender
    {
        public int TenderId { get; set; }
        public String TenderName { get; set; }
        public String FileName { get; set; }
        public String CreatedDate { get; set; }

    }


    public class HomeMenu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string icon { get; set; }
        public string MenuType { get; set; }
        public string IsLink { get; set; }
        public string Url { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public List<HomeSubMenu> _LstSubmenu { get; set; }


    }

    public class HomeSubMenu
    {
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public int Type { get; set; }
        public string FilePath { get; set; }
        public string MenuName { get; set; }
        public int MenuId { get; set; }
        public string qs { get; set; }

        public string islink { get; set; }


    }


    public class HouseDashboard
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public string icon { get; set; }
    }

    public class MCHomeFunctionModel
    {

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

                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCid", MCId)};
                ds = SqlHelper.ExecuteDataset(connection, "[GetHomeDashboard]", parameterValues);
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }
            return ds;
        }

        public static SelectList GetMCSelectList()
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
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@Agendatype", AgendaType) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMC]", null);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["MCName"].ToString());
                    item.Value = Convert.ToString(row["Id"].ToString());
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



        public static List<Member> GetMembers(int McId, string lang)
        {
            List<Member> lstMem = new List<Member>();
            DataSet ds = new DataSet();
            ds = GetMembersData(McId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Member fr = new Member();
                fr.MemberName = dr["Name"+lang].ToString();
                fr.MemberPreFix = dr["PreFix"+ lang].ToString();
                fr.Address = dr["PermanentAddress"].ToString();
                fr.Gender = dr["Sex"].ToString();
                if (dr["memdob"].ToString() != "")
                {
                    fr.BirthDate = Convert.ToDateTime(dr["memdob"]).ToString("dd/MM/yyyy");
                }
                
                if (fr.Gender == "MALE")
                {
                    fr.MFGender = "M";
                }
                else
                {
                    fr.MFGender = "F";
                }
                fr.MobileNo = dr["Mobile"].ToString();
                fr.Designation = dr["Designation"].ToString();
                fr.ImagePath = dr["FilePath"].ToString() + dr["FileName"].ToString();
                lstMem.Add(fr);
            }
            return lstMem;

        }

        public static DataSet GetMembersData(int MCId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCId", MCId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMembers]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }

        public static List<MC> GetMCList()
        {
            List<MC> lstMem = new List<MC>();
            DataSet ds = new DataSet();
            ds = GetMCListData();

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                MC fr = new MC();

                fr.MCId = dr["Id"].ToString();
                fr.MCName = dr["MCName"].ToString();
                fr.MCName_local = dr["MCName_local"].ToString();
                fr.Address = dr["Address"].ToString();
                fr.Icon = dr["MCLogo"].ToString();
                lstMem.Add(fr);
            }
            return lstMem;

        }

        public static List<MC> GetConsList()
        {
            List<MC> lstMem = new List<MC>();
            DataSet ds = new DataSet();
            ds = GetMCListData();

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                MC fr = new MC();

                fr.MCId = dr["Id"].ToString();
                fr.MCName = dr["MCName"].ToString();
                fr.MCName_local = dr["MCName_local"].ToString();
                fr.Address = dr["Address"].ToString();
                fr.Icon = dr["MCLogo"].ToString();
                lstMem.Add(fr);
            }
            return lstMem;

        }

        public static DataSet GetMCListData()
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
                ds = SqlHelper.ExecuteDataset(connection, "[GetMC]", null);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }


        public static List<News> GetNews(int MCId,string lang)
        {
            List<News> lstnews = new List<News>();
            DataSet ds = new DataSet();
            ds = GetNewsData(MCId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                News fr = new News();
                fr.NewsId = Convert.ToInt32(dr["LatestNewsID"].ToString());
                fr.NewsName = dr["NewsTitle"+ lang].ToString();

                fr.NewsDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd/MM/yyyy");  
                fr.ImagePath = dr["Images"].ToString() + dr["FileName"].ToString();
                lstnews.Add(fr);
            }
            return lstnews;

        }

        public static DataSet GetNewsData( int MCId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCId", MCId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetLatestNews]", parameterValues);

                

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }


        public static List<News> GetFlashNews(int MCId, string lang)
        {
            List<News> lstnews = new List<News>();
            DataSet ds = new DataSet();
            ds = GetFlashNewsData(MCId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                News fr = new News();
                fr.NewsId = Convert.ToInt32(dr["FlashNewsID"].ToString());
                fr.NewsName = dr["FlashNewsText"+ lang].ToString();
                fr.NewsDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd/MM/yyyy"); 
                fr.ImagePath = dr["FlashNewsFile"].ToString();
                lstnews.Add(fr);
            }
            return lstnews;

        }

        public static DataSet GetFlashNewsData(int MCId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCId", MCId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetFlashNews]", parameterValues);



            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }


        public static List<Gallary> GetMCGallary(int MCId)
        {
            List<Gallary> lstgal = new List<Gallary>();
            DataSet ds = new DataSet();
            ds = GetMCGallaryData(MCId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Gallary fr = new Gallary();
                fr.Id = Convert.ToInt32(dr["Id"].ToString());
                fr.ImageTittle = dr["imageTitle"].ToString();
                fr.ImageDate = Convert.ToDateTime(dr["createdDate"]).ToString("dd/MM/yyyy");
                fr.ImagePath = dr["imageLocation"].ToString();
                lstgal.Add(fr);
            }
            return lstgal;

        }

        public static DataSet GetMCGallaryData(int MCId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCId", MCId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMCGallary]", parameterValues);



            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }


        public static List<Tender> GetMCTender(int MCId, string lang)
        {
            List<Tender> lstTender = new List<Tender>();
            DataSet ds = new DataSet();
            ds = GetMCTenderData(MCId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Tender fr = new Tender();
                fr.TenderId = Convert.ToInt32(dr["Id"].ToString());
                fr.TenderName = dr["TenderName"+ lang].ToString();
                fr.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd/MM/yyyy");
                fr.FileName = dr["TenderFileName"].ToString();
                lstTender.Add(fr);
            }
            return lstTender;

        }

        public static DataSet GetMCTenderData(int MCId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCId", MCId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetTenders]", parameterValues);



            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }





        public static List<Member> GetTodayBirthdays(int McId)
        {
            List<Member> lstMem = new List<Member>();
            DataSet ds = new DataSet();
            ds = GetBirthdayData(McId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Member fr = new Member();

                fr.MemberName = dr["Name"].ToString();
                fr.MemberPreFix = dr["PreFix"].ToString();
                fr.Address = dr["PermanentAddress"].ToString();
                fr.Gender = dr["Sex"].ToString();
                fr.BirthDate = Convert.ToDateTime(dr["memdob"]).ToString("dd/MMMM");
                fr.BirthYear = Convert.ToDateTime(dr["memdob"]).ToString("dd/MM/yyyy");
                if (fr.Gender == "MALE")
                {
                    fr.MFGender = "M";
                }
                else
                {
                    fr.MFGender = "F";
                }
                fr.MobileNo = dr["Mobile"].ToString();
                fr.Designation = dr["Designation"].ToString();
                fr.ImagePath = dr["FilePath"].ToString() + dr["FileName"].ToString();
                lstMem.Add(fr);
            }
            return lstMem;

        }
        public static List<Member> GetBirthday(int McId)
        {
            List<Member> lstMem = new List<Member>();
            DataSet ds = new DataSet();
            ds = GetBirthdayData(McId);

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                Member fr = new Member();

                fr.MemberName = dr["Name"].ToString();
                fr.MemberPreFix = dr["PreFix"].ToString();

               
                fr.Address = dr["PermanentAddress"].ToString();
                fr.Gender = dr["Sex"].ToString();
                fr.BirthDate = Convert.ToDateTime(dr["memdob"]).ToString("dd/MMMM");
                fr.BirthYear = Convert.ToDateTime(dr["memdob"]).ToString("dd/MM/yyyy");
                if (fr.Gender == "MALE")
                {
                    fr.MFGender = "M";
                }
                else
                {
                    fr.MFGender = "F";
                }
                fr.MobileNo = dr["Mobile"].ToString();
                fr.Designation = dr["Designation"].ToString();
                fr.ImagePath = dr["FilePath"].ToString() + dr["FileName"].ToString();
                lstMem.Add(fr);
            }
            return lstMem;

        }

        public static DataSet GetBirthdayData(int MCId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCId", MCId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetBithday]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

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
                ds = SqlHelper.ExecuteDataset(connection, "[GetMCHomeMenu]", null);

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
                ds = SqlHelper.ExecuteDataset(connection, "[GetMCHomeSubMenu]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }



        public static List<HomeMenu> PopulateMenu(string lang)
        {
            List<HomeMenu> menu = new List<HomeMenu>();
            DataSet ds = new DataSet();
            ds = GetMenuList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    HomeMenu fr = new HomeMenu();

                    fr.MenuId = Convert.ToInt32(dr["Id"].ToString());
                    fr.MenuName = dr["MenuName"+ lang].ToString();
                    fr.IsLink = dr["islink"].ToString();
                    fr.Url = dr["Url"].ToString();
                    if (fr.IsLink != "0")
                    {
                        string[] url = fr.Url.Split(new char[] { '/', '\\', }, System.StringSplitOptions.RemoveEmptyEntries);
                        if (url.Length > 0)
                        {
                            fr.ControllerName = url[0];

                            if (url.Length > 1)
                            {
                                fr.ActionName = url[1];
                            }
                                
                        }
                    }
                    fr._LstSubmenu = PopulateSubMenubyUser(fr.MenuId, lang);
                    fr.icon = dr["icon"].ToString();
                    menu.Add(fr);

                }
            }


            return menu;
        }

        public static List<HomeSubMenu> PopulateSubMenubyUser(int menuId, string lang)
        {
            List<HomeSubMenu> submenu = new List<HomeSubMenu>();
            DataSet ds = new DataSet();
            ds = GetSubMenuList(menuId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                HomeSubMenu fr = new HomeSubMenu();

                fr.SubMenuId = Convert.ToInt32(dr["SubMenuId"].ToString());
                fr.SubMenuName = dr["SubMenuName"+ lang].ToString();
                fr.ControllerName = dr["ControllerName"].ToString();
                fr.ActionName = dr["ActionName"].ToString();
                fr.qs = dr["qs"].ToString();
                fr.islink = dr["islink"].ToString();
                fr.Type = Convert.ToInt32(dr["Type"].ToString());
                fr.FilePath = dr["FilePath"].ToString();

                submenu.Add(fr);

            }


            return submenu;
        }



        public static List<HouseDashboard> GetHouseBusiness(string lang)
        {
            List<HouseDashboard> lstlbl = new List<HouseDashboard>();
            DataSet ds = new DataSet();
            ds = GetHouseDashboardData();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                HouseDashboard fr = new HouseDashboard();

                fr.id = Convert.ToInt32(dr["Id"].ToString());
                fr.Name = dr["Name"+ lang].ToString();
                fr.icon = dr["icon"].ToString();
                Random rnd = new Random();
                fr.Count = rnd.Next(1, 50); 
                lstlbl.Add(fr);
            }
            return lstlbl;

        }
        public static List<HouseDashboard> GetHouseResolutions(string lang)
        {
            List<HouseDashboard> lstlbl = new List<HouseDashboard>();
            DataSet ds = new DataSet();
            ds = GetHouseDashboardData();

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                HouseDashboard fr = new HouseDashboard();

                fr.id = Convert.ToInt32(dr["Id"].ToString());
                fr.Name = dr["Name" + lang].ToString();
                fr.icon = dr["icon"].ToString();
                Random rnd = new Random();
                fr.Count = rnd.Next(1, 50);
                lstlbl.Add(fr);
            }
            return lstlbl;

        }

        public static List<HouseDashboard> GetFCCResolutions(string lang)
        {
            List<HouseDashboard> lstlbl = new List<HouseDashboard>();
            DataSet ds = new DataSet();
            ds = GetHouseDashboardData();

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                HouseDashboard fr = new HouseDashboard();

                fr.id = Convert.ToInt32(dr["Id"].ToString());
                fr.Name = dr["Name" + lang].ToString();
                fr.icon = dr["icon"].ToString();
                Random rnd = new Random();
                fr.Count = rnd.Next(1, 50);
                lstlbl.Add(fr);
            }
            return lstlbl;

        }

        public static DataSet GetHouseDashboardData()
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
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCId", MCId) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetHouseDashboardLabel]", null);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }

    }
}
