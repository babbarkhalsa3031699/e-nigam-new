using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.MCHouse
{
    public class HomeGallery
    {
        public int Id { get; set; }
        public int MCId { get; set; }
        public string MCName { get; set; }
        public string Title { get; set; }
        public string TitlePunjabi { get; set; }
        public string Description { get; set; }
        public string DescriptionPunjabi { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public static List<HomeGallery> GetGalleryList()
        {
            List<HomeGallery> lstitems = new List<HomeGallery>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetGalleryList");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HomeGallery items = new HomeGallery();
                    items.Id = Convert.ToInt32(dr["Id"]);
                    items.Title = Convert.ToString(dr["imageTitle"]);
                    items.TitlePunjabi = Convert.ToString(dr["imageTitleLocal"]);
                    items.Description = Convert.ToString(dr["imageDescription"]);
                    items.DescriptionPunjabi = Convert.ToString(dr["imageDescriptionLocal"]);
                    items.MCId = Convert.ToInt32(dr["MCId"]);
                    items.MCName = Convert.ToString(dr["MCName"]);
                    items.ImagePath = Convert.ToString(dr["imageLocation"]);
                    items.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    lstitems.Add(items);
                }
                return lstitems;
            }
            catch
            {
                return lstitems;
            }
        }
        public static int SaveGalleryDetails(HomeGallery model)
        {
            try
            {
               // string path = "SecureFileStructure/" + model.MCName + "/Gallery";
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("SaveGalleryDetails", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@TitlePunjabi", model.TitlePunjabi);
                cmd.Parameters.AddWithValue("@Description", SqlDbType.VarChar).Value = (model.Description == null) ? string.Empty : model.Description;
                cmd.Parameters.AddWithValue("@DescriptionPunjabi", SqlDbType.VarChar).Value = (model.DescriptionPunjabi == null) ? string.Empty : model.DescriptionPunjabi;
                cmd.Parameters.AddWithValue("@MCID", model.MCId);
                cmd.Parameters.AddWithValue("@ImagePath", SqlDbType.VarChar).Value =model.ImagePath;
                cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = (CurrentSession.UserID == null) ? string.Empty : CurrentSession.UserID;
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static List<HomeGallery> GetGalleryRecordById(int id)
        {
            List<HomeGallery> lstItems = new List<HomeGallery>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);


                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetGalleryRecordById", param).Tables[0].Rows)
                {
                    HomeGallery item = new HomeGallery();
                    {
                        item.Id = Convert.ToInt32(dr["Id"]);
                        item.Title = Convert.ToString(dr["imageTitle"]);
                        item.TitlePunjabi = Convert.ToString(dr["imageTitleLocal"]);
                        item.Description = Convert.ToString(dr["imageDescription"]);
                        item.DescriptionPunjabi = Convert.ToString(dr["imageDescriptionLocal"]);
                        if (dr["MCId"] != System.DBNull.Value)
                        {
                            item.MCId = Convert.ToInt32(dr["MCId"]);
                        }
                        item.MCName = Convert.ToString(dr["MCName"]);
                        item.ImagePath = Convert.ToString(dr["imageLocation"]);

                    };
                    lstItems.Add(item);
                    con.Close();
                }
                return lstItems;

            }
            catch (Exception ex)
            {
                return lstItems;

            }

        }
        public static int UpdateImageGalleryRecord(HomeGallery model)
        {
            try
            {
                string path = "MC/" + model.MCName + "/Gallery";
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("UpdateImageGalleryRecord", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@TitlePunjabi", model.TitlePunjabi);
                cmd.Parameters.AddWithValue("@Description", SqlDbType.VarChar).Value = (model.Description == null) ? string.Empty : model.Description;
                cmd.Parameters.AddWithValue("@DescriptionPunjabi", SqlDbType.VarChar).Value = (model.DescriptionPunjabi == null) ? string.Empty : model.DescriptionPunjabi;
                cmd.Parameters.AddWithValue("@ImagePath", SqlDbType.VarChar).Value = (model.ImagePath == null) ? string.Empty : path + "/" + model.ImagePath;
                cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.VarChar).Value = (CurrentSession.UserID == null) ? string.Empty : CurrentSession.UserID;
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int DeleteGalleryRecordById(int Id)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", Id);

                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "DeleteGalleryRecordById", param);
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

    }
    public class LstGalleryModel
    {
        public List<HomeGallery> _LstHomeGallery { get; set; }
        public string fileacessingUrl { get; set; }
    }
}
