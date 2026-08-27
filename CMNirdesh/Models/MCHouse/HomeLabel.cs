using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.MCHouse
{
    [Serializable]
    [Table("mMembers")]
    public class HomeLabel
    {
        [Key]
        public int Id { get; set; }

        public string GovtTitle { get; set; }

        public string GovtTitle_local { get; set; }

        public string SiteTitle { get; set; }

        public string SiteTitle_local { get; set; }

        public string SubTitle { get; set; }

        public string SubTitle_local { get; set; }

        public string MCTitle { get; set; }

        public string MCTitle_local { get; set; }


        public string CounsilTitle { get; set; }

        public string CounsilTitle_local { get; set; }


        public string LoginTitle { get; set; }

        public string LoginTitle_local { get; set; }

        public string SitemapTitle { get; set; }

        public string SitemapTitle_local { get; set; }

        public string NoticeTItle { get; set; }

        public string NoticeTitle_local { get; set; }

        public string HousesTitle { get; set; }

        public string HousesTitle_local { get; set; }

        public string Councilors { get; set; }

        public string Councilors_local { get; set; }

        public string Meetings { get; set; }

        public string Meetings_local { get; set; }

        public string Departments { get; set; }

        public string Departments_local { get; set; }

        public string FCC { get; set; }

        public string FCC_local { get; set; }

        public string Employees { get; set; }

        public string Employees_local { get; set; }

        public string StatusPanel { get; set; }

        public string StatusPanel_local { get; set; }


        public string MemberListTitle { get; set; }

        public string MemberListTitle_local { get; set; }

        public string MemberListDescription { get; set; }

        public string MemberListDescription_local { get; set; }


        public string NewsTitle { get; set; }

        public string NewsTitle_local { get; set; }

        public string NewsDescription { get; set; }

        public string NewsDescription_local { get; set; }

        public string GalleryTitle { get; set; }

        public string GalleryTitle_local { get; set; }

        public string HouseBusiness { get; set; }


        public string HouseBusiness_local { get; set; }

        public string HouseResolutions { get; set; }

        public string HouseResolutions_local { get; set; }

        public string FCCResolutions { get; set; }

        public string FCCResolutions_local { get; set; }

        public string alert { get; set; }

        public string alert_local { get; set; }

        public string StateSubTitle { get; set; }

        public string StateSubTitle_local { get; set; }

        public bool IsActive { get; set; }





        public static List<HomeLabel> GetList()
        {

            List<HomeLabel> lstItems = new List<HomeLabel>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "HomeLabelList");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HomeLabel item = new HomeLabel();
                 
                    item.alert = Convert.ToString(dr["alert"]);
                    item.alert_local = Convert.ToString(dr["alert_local"]);
                    item.Councilors = Convert.ToString(dr["Councilors"]);
                    item.Councilors_local = Convert.ToString(dr["Councilors_local"]);
                    item.CounsilTitle = Convert.ToString(dr["CounsilTitle"]);
                    item.CounsilTitle_local = Convert.ToString(dr["CounsilTitle_local"]);
                    item.Departments = Convert.ToString(dr["Departments"]);
                    item.Departments_local = Convert.ToString(dr["Departments_local"]);
                    item.Employees = Convert.ToString(dr["Employees"]);
                    item.Employees_local = Convert.ToString(dr["Employees_local"]);
                    item.FCC = Convert.ToString(dr["FCC"]);
                    item.FCCResolutions = Convert.ToString(dr["FCCResolutions"]);
                    item.FCCResolutions_local = Convert.ToString(dr["FCCResolutions_local"]);
                    item.FCC_local = Convert.ToString(dr["FCC_local"]);
                    item.GalleryTitle = Convert.ToString(dr["GalleryTitle"]);
                    item.GalleryTitle_local = Convert.ToString(dr["GalleryTitle_local"]);
                    item.GovtTitle = Convert.ToString(dr["GovtTitle"]);
                    item.GovtTitle_local = Convert.ToString(dr["GovtTitle_local"]);
                    item.HouseBusiness = Convert.ToString(dr["HouseBusiness"]);
                    item.HouseBusiness_local = Convert.ToString(dr["HouseBusiness_local"]);
                    item.HouseResolutions = Convert.ToString(dr["HouseResolutions"]);
                    item.HouseResolutions_local = Convert.ToString(dr["HouseResolutions_local"]);
                    item.HousesTitle = Convert.ToString(dr["HousesTitle"]);
                    item.HousesTitle_local = Convert.ToString(dr["HousesTitle_local"]);
                    item.LoginTitle = Convert.ToString(dr["LoginTitle"]);
                    item.LoginTitle_local = Convert.ToString(dr["LoginTitle_local"]);
                    item.MCTitle = Convert.ToString(dr["MCTitle"]);
                    item.MCTitle_local = Convert.ToString(dr["MCTitle_local"]);
                    item.Meetings = Convert.ToString(dr["Meetings"]);
                    item.Meetings_local = Convert.ToString(dr["Meetings_local"]);
                    item.MemberListDescription = Convert.ToString(dr["MemberListDescription"]);
                    item.MemberListDescription_local = Convert.ToString(dr["MemberListDescription_local"]);
                    item.MemberListTitle = Convert.ToString(dr["MemberListTitle"]);
                    item.MemberListTitle_local = Convert.ToString(dr["MemberListTitle_local"]);
                    item.NewsDescription = Convert.ToString(dr["NewsDescription"]);
                    item.NewsDescription_local = Convert.ToString(dr["NewsDescription_local"]);
                    item.NewsTitle = Convert.ToString(dr["NewsTitle"]);
                    item.NewsTitle_local = Convert.ToString(dr["NewsTitle_local"]);
                    item.NoticeTItle = Convert.ToString(dr["NoticeTitle"]);
                    item.NoticeTitle_local = Convert.ToString(dr["NoticeTitle_local"]);
                    item.SitemapTitle = Convert.ToString(dr["SitemapTitle"]);
                    item.SitemapTitle_local = Convert.ToString(dr["SitemapTitle_local"]);
                    item.SiteTitle = Convert.ToString(dr["SiteTitle"]);
                    item.SiteTitle_local = Convert.ToString(dr["SiteTitle_local"]);
                    item.StateSubTitle = Convert.ToString(dr["StateSubTitle"]);
                    item.StateSubTitle_local = Convert.ToString(dr["StateSubTitle_local"]);
                    item.StatusPanel = Convert.ToString(dr["StatusPanel"]);
                    item.StatusPanel_local = Convert.ToString(dr["StatusPanel_local"]);
                    item.SubTitle = Convert.ToString(dr["SubTitle"]);
                    item.SubTitle_local = Convert.ToString(dr["SubTitle_local"]);
                    item.Id = Convert.ToInt32(dr["Id"]);
                    item.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    lstItems.Add(item);
                }
                return lstItems;
            }
            catch
            {
                return lstItems;
            }

        }

        public static int SaveRecords(HomeLabel model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("HomeLabelInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GovtTitle", model.GovtTitle);
                cmd.Parameters.AddWithValue("@GovtTitle_local", model.GovtTitle_local);
                cmd.Parameters.AddWithValue("@SiteTitle", model.SiteTitle);
                cmd.Parameters.AddWithValue("@SiteTitle_local", model.SiteTitle_local);
                cmd.Parameters.AddWithValue("@SubTitle", SqlDbType.VarChar).Value = (model.SubTitle == null) ? string.Empty : model.SubTitle;
                cmd.Parameters.AddWithValue("@SubTitle_local", SqlDbType.VarChar).Value = (model.SubTitle_local == null) ? string.Empty : model.SubTitle_local;

                cmd.Parameters.AddWithValue("@MCTitle", SqlDbType.VarChar).Value = (model.MCTitle == null) ? string.Empty : model.MCTitle;

                cmd.Parameters.AddWithValue("@MCTitle_local", SqlDbType.VarChar).Value = (model.MCTitle_local == null) ? string.Empty : model.MCTitle_local;
                cmd.Parameters.AddWithValue("@CounsilTitle", SqlDbType.VarChar).Value = (model.CounsilTitle == null) ? string.Empty : model.CounsilTitle;

                cmd.Parameters.AddWithValue("@CounsilTitle_local", SqlDbType.VarChar).Value = (model.CounsilTitle_local == null) ? string.Empty : model.CounsilTitle_local;
                cmd.Parameters.AddWithValue("@LoginTitle", SqlDbType.VarChar).Value = (model.LoginTitle == null) ? string.Empty : model.LoginTitle;

                cmd.Parameters.AddWithValue("@LoginTitle_local", SqlDbType.VarChar).Value = (model.LoginTitle_local == null) ? string.Empty : model.LoginTitle_local;

                cmd.Parameters.AddWithValue("@SitemapTitle", SqlDbType.VarChar).Value = (model.SitemapTitle == null) ? string.Empty : model.SitemapTitle;
                cmd.Parameters.AddWithValue("@SitemapTitle_local", SqlDbType.VarChar).Value = (model.SitemapTitle_local == null) ? string.Empty : model.SitemapTitle_local;
                cmd.Parameters.AddWithValue("@NoticeTitle", SqlDbType.VarChar).Value = (model.NoticeTItle == null) ? string.Empty : model.NoticeTItle;
                cmd.Parameters.AddWithValue("@NoticeTitle_local", SqlDbType.VarChar).Value = (model.NoticeTitle_local == null) ? string.Empty : model.NoticeTitle_local;

                cmd.Parameters.AddWithValue("@HousesTitle", SqlDbType.VarChar).Value = (model.HousesTitle == null) ? string.Empty : model.HousesTitle;

                cmd.Parameters.AddWithValue("@HousesTitle_local", SqlDbType.VarChar).Value = (model.HousesTitle_local == null) ? string.Empty : model.HousesTitle_local;
                cmd.Parameters.AddWithValue("@Councilors", SqlDbType.VarChar).Value = (model.Councilors == null) ? string.Empty : model.Councilors;
                cmd.Parameters.AddWithValue("@Councilors_local", SqlDbType.VarChar).Value = (model.Councilors_local == null) ? string.Empty : model.Councilors_local;
                cmd.Parameters.AddWithValue("@Meetings", SqlDbType.VarChar).Value = (model.Meetings == null) ? string.Empty : model.Meetings;

                cmd.Parameters.AddWithValue("@Meetings_local", SqlDbType.VarChar).Value = (model.Meetings_local == null) ? string.Empty : model.Meetings_local;

                cmd.Parameters.AddWithValue("@Departments", SqlDbType.VarChar).Value = (model.Departments == null) ? string.Empty : model.Departments;
                cmd.Parameters.AddWithValue("@Departments_local", SqlDbType.VarChar).Value = (model.Departments_local == null) ? string.Empty : model.Departments_local;
                cmd.Parameters.AddWithValue("@FCC", SqlDbType.VarChar).Value = (model.FCC == null) ? string.Empty : model.FCC;
                cmd.Parameters.AddWithValue("@FCC_local", SqlDbType.VarChar).Value = (model.FCC_local == null) ? string.Empty : model.FCC_local;

                cmd.Parameters.AddWithValue("@Employees", SqlDbType.VarChar).Value = (model.Employees == null) ? string.Empty : model.Employees;

                cmd.Parameters.AddWithValue("@Employees_local", SqlDbType.VarChar).Value = (model.Employees_local == null) ? string.Empty : model.Employees_local;
                cmd.Parameters.AddWithValue("@StatusPanel", SqlDbType.VarChar).Value = (model.StatusPanel == null) ? string.Empty : model.StatusPanel;


                cmd.Parameters.AddWithValue("@StatusPanel_local", SqlDbType.VarChar).Value = (model.StatusPanel_local == null) ? string.Empty : model.StatusPanel_local;

                cmd.Parameters.AddWithValue("@MemberListTitle", SqlDbType.VarChar).Value = (model.MemberListTitle == null) ? string.Empty : model.MemberListTitle;
                cmd.Parameters.AddWithValue("@MemberListTitle_local", SqlDbType.VarChar).Value = (model.MemberListTitle_local == null) ? string.Empty : model.MemberListTitle_local;
                cmd.Parameters.AddWithValue("@MemberListDescription", SqlDbType.VarChar).Value = (model.MemberListDescription == null) ? string.Empty : model.MemberListDescription;
                cmd.Parameters.AddWithValue("@MemberListDescription_local", SqlDbType.VarChar).Value = (model.MemberListDescription_local == null) ? string.Empty : model.MemberListDescription_local;

                cmd.Parameters.AddWithValue("@NewsTitle", SqlDbType.VarChar).Value = (model.NewsTitle == null) ? string.Empty : model.NewsTitle;

                cmd.Parameters.AddWithValue("@NewsTitle_local", SqlDbType.VarChar).Value = (model.NewsTitle_local == null) ? string.Empty : model.NewsTitle_local;
                cmd.Parameters.AddWithValue("@NewsDescription", SqlDbType.VarChar).Value = (model.NewsDescription == null) ? string.Empty : model.NewsDescription;
                cmd.Parameters.AddWithValue("@NewsDescription_local", SqlDbType.VarChar).Value = (model.NewsDescription_local == null) ? string.Empty : model.NewsDescription_local;

                cmd.Parameters.AddWithValue("@GalleryTitle", SqlDbType.VarChar).Value = (model.GalleryTitle == null) ? string.Empty : model.GalleryTitle;
                cmd.Parameters.AddWithValue("@GalleryTitle_local", SqlDbType.VarChar).Value = (model.GalleryTitle_local == null) ? string.Empty : model.GalleryTitle_local;
                cmd.Parameters.AddWithValue("@HouseBusiness", SqlDbType.VarChar).Value = (model.HouseBusiness == null) ? string.Empty : model.HouseBusiness;
                cmd.Parameters.AddWithValue("@HouseBusiness_local", SqlDbType.VarChar).Value = (model.HouseBusiness_local == null) ? string.Empty : model.HouseBusiness_local;

                cmd.Parameters.AddWithValue("@HouseResolutions", SqlDbType.VarChar).Value = (model.HouseResolutions == null) ? string.Empty : model.HouseResolutions;

                cmd.Parameters.AddWithValue("@HouseResolutions_local", SqlDbType.VarChar).Value = (model.HouseResolutions_local == null) ? string.Empty : model.HouseResolutions_local;
                cmd.Parameters.AddWithValue("@FCCResolutions", SqlDbType.VarChar).Value = (model.FCCResolutions == null) ? string.Empty : model.FCCResolutions;

                cmd.Parameters.AddWithValue("@FCCResolutions_local", SqlDbType.VarChar).Value = (model.FCCResolutions_local == null) ? string.Empty : model.FCCResolutions_local;

                cmd.Parameters.AddWithValue("@alert", SqlDbType.VarChar).Value = (model.alert == null) ? string.Empty : model.alert;
                cmd.Parameters.AddWithValue("@alert_local", SqlDbType.VarChar).Value = (model.alert_local == null) ? string.Empty : model.alert_local;
                cmd.Parameters.AddWithValue("@StateSubTitle", SqlDbType.VarChar).Value = (model.StateSubTitle == null) ? string.Empty : model.StateSubTitle;
                cmd.Parameters.AddWithValue("@StateSubTitle_local", SqlDbType.VarChar).Value = (model.StateSubTitle_local == null) ? string.Empty : model.StateSubTitle_local;

                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<HomeLabel> GetRecordsById(int id)
        {
            List<HomeLabel> lstItems = new List<HomeLabel>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "HomeLabelGetID", param).Tables[0].Rows)
                {
                    HomeLabel item = new HomeLabel();
                    {
                        item.Id = Convert.ToInt32(dr["Id"]);
                        item.alert = Convert.ToString(dr["alert"]);
                        item.alert_local = Convert.ToString(dr["alert_local"]);
                        item.Councilors = Convert.ToString(dr["Councilors"]);
                        item.Councilors_local = Convert.ToString(dr["Councilors_local"]);
                        item.CounsilTitle = Convert.ToString(dr["CounsilTitle"]);
                        item.CounsilTitle_local = Convert.ToString(dr["CounsilTitle_local"]);
                        item.Departments = Convert.ToString(dr["Departments"]);
                        item.Departments_local = Convert.ToString(dr["Departments_local"]);
                        item.Employees = Convert.ToString(dr["Employees"]);
                        item.Employees_local = Convert.ToString(dr["Employees_local"]);
                        item.FCC = Convert.ToString(dr["FCC"]);
                        item.FCCResolutions = Convert.ToString(dr["FCCResolutions"]);
                        item.FCCResolutions_local = Convert.ToString(dr["FCCResolutions_local"]);
                        item.FCC_local = Convert.ToString(dr["FCC_local"]);
                        item.GalleryTitle = Convert.ToString(dr["GalleryTitle"]);
                        item.GalleryTitle_local = Convert.ToString(dr["GalleryTitle_local"]);
                        item.GovtTitle = Convert.ToString(dr["GovtTitle"]);
                        item.GovtTitle_local = Convert.ToString(dr["GovtTitle_local"]);
                        item.HouseBusiness = Convert.ToString(dr["HouseBusiness"]);
                        item.HouseBusiness_local = Convert.ToString(dr["HouseBusiness_local"]);
                        item.HouseResolutions = Convert.ToString(dr["HouseResolutions"]);
                        item.HouseResolutions_local = Convert.ToString(dr["HouseResolutions_local"]);
                        item.HousesTitle = Convert.ToString(dr["HousesTitle"]);
                        item.HousesTitle_local = Convert.ToString(dr["HousesTitle_local"]);
                        item.LoginTitle = Convert.ToString(dr["LoginTitle"]);
                        item.LoginTitle_local = Convert.ToString(dr["LoginTitle_local"]);
                        item.MCTitle = Convert.ToString(dr["MCTitle"]);
                        item.MCTitle_local = Convert.ToString(dr["MCTitle_local"]);
                        item.Meetings = Convert.ToString(dr["Meetings"]);
                        item.Meetings_local = Convert.ToString(dr["Meetings_local"]);
                        item.MemberListDescription = Convert.ToString(dr["MemberListDescription"]);
                        item.MemberListDescription_local = Convert.ToString(dr["MemberListDescription_local"]);
                        item.MemberListTitle = Convert.ToString(dr["MemberListTitle"]);
                        item.MemberListTitle_local = Convert.ToString(dr["MemberListTitle_local"]);
                        item.NewsDescription = Convert.ToString(dr["NewsDescription"]);
                        item.NewsDescription_local = Convert.ToString(dr["NewsDescription_local"]);
                        item.NewsTitle = Convert.ToString(dr["NewsTitle"]);
                        item.NewsTitle_local = Convert.ToString(dr["NewsTitle_local"]);
                        item.NoticeTItle = Convert.ToString(dr["NoticeTitle"]);
                        item.NoticeTitle_local = Convert.ToString(dr["NoticeTitle_local"]);
                        item.SitemapTitle = Convert.ToString(dr["SitemapTitle"]);
                        item.SitemapTitle_local = Convert.ToString(dr["SitemapTitle_local"]);
                        item.SiteTitle = Convert.ToString(dr["SiteTitle"]);
                        item.SiteTitle_local = Convert.ToString(dr["SiteTitle_local"]);
                        item.StateSubTitle = Convert.ToString(dr["StateSubTitle"]);
                        item.StateSubTitle_local = Convert.ToString(dr["StateSubTitle_local"]);
                        item.StatusPanel = Convert.ToString(dr["StatusPanel"]);
                        item.StatusPanel_local = Convert.ToString(dr["StatusPanel_local"]);
                        item.SubTitle = Convert.ToString(dr["SubTitle"]);
                        item.SubTitle_local = Convert.ToString(dr["SubTitle_local"]);

                    };
                    lstItems.Add(item);
                }

                return lstItems;
            }
            catch (Exception ex)
            {
                return lstItems;

            }
        }
        public static int UpdateRecordsById(HomeLabel model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("HomeLabelUpdateById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);

                cmd.Parameters.AddWithValue("@GovtTitle", model.GovtTitle);

                cmd.Parameters.AddWithValue("@GovtTitle_local", model.GovtTitle_local);
                cmd.Parameters.AddWithValue("@SiteTitle", model.SiteTitle);
                cmd.Parameters.AddWithValue("@SiteTitle_local", model.SiteTitle_local);
                cmd.Parameters.AddWithValue("@SubTitle", SqlDbType.VarChar).Value = (model.SubTitle == null) ? string.Empty : model.SubTitle;
                cmd.Parameters.AddWithValue("@SubTitle_local", SqlDbType.VarChar).Value = (model.SubTitle_local == null) ? string.Empty : model.SubTitle_local;

                cmd.Parameters.AddWithValue("@MCTitle", SqlDbType.VarChar).Value = (model.MCTitle == null) ? string.Empty : model.MCTitle;

                cmd.Parameters.AddWithValue("@MCTitle_local", SqlDbType.VarChar).Value = (model.MCTitle_local == null) ? string.Empty : model.MCTitle_local;
                cmd.Parameters.AddWithValue("@CounsilTitle", SqlDbType.VarChar).Value = (model.CounsilTitle == null) ? string.Empty : model.CounsilTitle;

                cmd.Parameters.AddWithValue("@CounsilTitle_local", SqlDbType.VarChar).Value = (model.CounsilTitle_local == null) ? string.Empty : model.CounsilTitle_local;
                cmd.Parameters.AddWithValue("@LoginTitle", SqlDbType.VarChar).Value = (model.LoginTitle == null) ? string.Empty : model.LoginTitle;

                cmd.Parameters.AddWithValue("@LoginTitle_local", SqlDbType.VarChar).Value = (model.LoginTitle_local == null) ? string.Empty : model.LoginTitle_local;

                cmd.Parameters.AddWithValue("@SitemapTitle", SqlDbType.VarChar).Value = (model.SitemapTitle == null) ? string.Empty : model.SitemapTitle;
                cmd.Parameters.AddWithValue("@SitemapTitle_local", SqlDbType.VarChar).Value = (model.SitemapTitle_local == null) ? string.Empty : model.SitemapTitle_local;
                cmd.Parameters.AddWithValue("@NoticeTitle", SqlDbType.VarChar).Value = (model.NoticeTItle == null) ? string.Empty : model.NoticeTItle;
                cmd.Parameters.AddWithValue("@NoticeTitle_local", SqlDbType.VarChar).Value = (model.NoticeTitle_local == null) ? string.Empty : model.NoticeTitle_local;

                cmd.Parameters.AddWithValue("@HousesTitle", SqlDbType.VarChar).Value = (model.HousesTitle == null) ? string.Empty : model.HousesTitle;

                cmd.Parameters.AddWithValue("@HousesTitle_local", SqlDbType.VarChar).Value = (model.HousesTitle_local == null) ? string.Empty : model.HousesTitle_local;
                cmd.Parameters.AddWithValue("@Councilors", SqlDbType.VarChar).Value = (model.Councilors == null) ? string.Empty : model.Councilors;
                cmd.Parameters.AddWithValue("@Councilors_local", SqlDbType.VarChar).Value = (model.Councilors_local == null) ? string.Empty : model.Councilors_local;
                cmd.Parameters.AddWithValue("@Meetings", SqlDbType.VarChar).Value = (model.Meetings == null) ? string.Empty : model.Meetings;

                cmd.Parameters.AddWithValue("@Meetings_local", SqlDbType.VarChar).Value = (model.Meetings_local == null) ? string.Empty : model.Meetings_local;

                cmd.Parameters.AddWithValue("@Departments", SqlDbType.VarChar).Value = (model.Departments == null) ? string.Empty : model.Departments;
                cmd.Parameters.AddWithValue("@Departments_local", SqlDbType.VarChar).Value = (model.Departments_local == null) ? string.Empty : model.Departments_local;
                cmd.Parameters.AddWithValue("@FCC", SqlDbType.VarChar).Value = (model.FCC == null) ? string.Empty : model.FCC;
                cmd.Parameters.AddWithValue("@FCC_local", SqlDbType.VarChar).Value = (model.FCC_local == null) ? string.Empty : model.FCC_local;

                cmd.Parameters.AddWithValue("@Employees", SqlDbType.VarChar).Value = (model.Employees == null) ? string.Empty : model.Employees;

                cmd.Parameters.AddWithValue("@Employees_local", SqlDbType.VarChar).Value = (model.Employees_local == null) ? string.Empty : model.Employees_local;
                cmd.Parameters.AddWithValue("@StatusPanel", SqlDbType.VarChar).Value = (model.StatusPanel == null) ? string.Empty : model.StatusPanel;


                cmd.Parameters.AddWithValue("@StatusPanel_local", SqlDbType.VarChar).Value = (model.StatusPanel_local == null) ? string.Empty : model.StatusPanel_local;

                cmd.Parameters.AddWithValue("@MemberListTitle", SqlDbType.VarChar).Value = (model.MemberListTitle == null) ? string.Empty : model.MemberListTitle;
                cmd.Parameters.AddWithValue("@MemberListTitle_local", SqlDbType.VarChar).Value = (model.MemberListTitle_local == null) ? string.Empty : model.MemberListTitle_local;
                cmd.Parameters.AddWithValue("@MemberListDescription", SqlDbType.VarChar).Value = (model.MemberListDescription == null) ? string.Empty : model.MemberListDescription;
                cmd.Parameters.AddWithValue("@MemberListDescription_local", SqlDbType.VarChar).Value = (model.MemberListDescription_local == null) ? string.Empty : model.MemberListDescription_local;

                cmd.Parameters.AddWithValue("@NewsTitle", SqlDbType.VarChar).Value = (model.NewsTitle == null) ? string.Empty : model.NewsTitle;

                cmd.Parameters.AddWithValue("@NewsTitle_local", SqlDbType.VarChar).Value = (model.NewsTitle_local == null) ? string.Empty : model.NewsTitle_local;
                cmd.Parameters.AddWithValue("@NewsDescription", SqlDbType.VarChar).Value = (model.NewsDescription == null) ? string.Empty : model.NewsDescription;
                cmd.Parameters.AddWithValue("@NewsDescription_local", SqlDbType.VarChar).Value = (model.NewsDescription_local == null) ? string.Empty : model.NewsDescription_local;

                cmd.Parameters.AddWithValue("@GalleryTitle", SqlDbType.VarChar).Value = (model.GalleryTitle == null) ? string.Empty : model.GalleryTitle;
                cmd.Parameters.AddWithValue("@GalleryTitle_local", SqlDbType.VarChar).Value = (model.GalleryTitle_local == null) ? string.Empty : model.GalleryTitle_local;
                cmd.Parameters.AddWithValue("@HouseBusiness", SqlDbType.VarChar).Value = (model.HouseBusiness == null) ? string.Empty : model.HouseBusiness;
                cmd.Parameters.AddWithValue("@HouseBusiness_local", SqlDbType.VarChar).Value = (model.HouseBusiness_local == null) ? string.Empty : model.HouseBusiness_local;

                cmd.Parameters.AddWithValue("@HouseResolutions", SqlDbType.VarChar).Value = (model.HouseResolutions == null) ? string.Empty : model.HouseResolutions;

                cmd.Parameters.AddWithValue("@HouseResolutions_local", SqlDbType.VarChar).Value = (model.HouseResolutions_local == null) ? string.Empty : model.HouseResolutions_local;
                cmd.Parameters.AddWithValue("@FCCResolutions", SqlDbType.VarChar).Value = (model.FCCResolutions == null) ? string.Empty : model.FCCResolutions;

                cmd.Parameters.AddWithValue("@FCCResolutions_local", SqlDbType.VarChar).Value = (model.FCCResolutions_local == null) ? string.Empty : model.FCCResolutions_local;

                cmd.Parameters.AddWithValue("@alert", SqlDbType.VarChar).Value = (model.alert == null) ? string.Empty : model.alert;
                cmd.Parameters.AddWithValue("@alert_local", SqlDbType.VarChar).Value = (model.alert_local == null) ? string.Empty : model.alert_local;
                cmd.Parameters.AddWithValue("@StateSubTitle", SqlDbType.VarChar).Value = (model.StateSubTitle == null) ? string.Empty : model.StateSubTitle;
                cmd.Parameters.AddWithValue("@StateSubTitle_local", SqlDbType.VarChar).Value = (model.StateSubTitle_local == null) ? string.Empty : model.StateSubTitle_local;

                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string DeleteRecordById(string Id)
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
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[HomeLabelDeleteById]", parameterValues));
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
public class LstHomeLabelModel
    {
        public List<HomeLabel> _LstHomeLabel { get; set; }
    }
}