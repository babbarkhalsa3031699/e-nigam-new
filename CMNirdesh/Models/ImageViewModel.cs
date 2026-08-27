using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using CMNirdesh.Error;

namespace CMNirdesh.Models
{
 

    public class ImageViewModel
    {
        public Guid CreatedBy { get; set; }

        public Guid ModifiedBy { get; set; }
        [Required(ErrorMessage = "Please select project")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Image Title is Required")]
        public string ImageTitle { get; set; }
        public string path { get; set; }

        public string ProjectName { get; set; }


        


      
        public string ImageFileName { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string Mode { get; set; }

        public SelectList ProjectList { get; set; }
        public SelectList StatusTypeList { get; set; }
        

        public int ProgrammeImageId { get; set; }
        public string ImageLocation
        {

            get;
            set;
        }
        [AllowHtml]
        public string ImageDescription { get; set; }

        public static SelectList GetStatusTypes()
        {
            var result = new List<SelectListItem>();
            // result.Add(new SelectListItem() { Text = "Select Status Type", Value = "0" });
            result.Add(new SelectListItem() { Text = "Published", Value = "1" });
            result.Add(new SelectListItem() { Text = "Unpublished ", Value = "0" });
            //  result.Add(new SelectListItem() { Text = "Archived", Value = "3" });
            //  result.Add(new SelectListItem() { Text = "Trashed", Value = "4" });


            return new SelectList(result, "Value", "Text");
        }

        public static SelectList getProjectByDeptId(string deptId)
        {

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem
            {
                Text = "--Select Project--",
                Value = "",
                Selected = true,
            });

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@deptId", deptId);
                connection.Close();
                foreach (DataRow row in CMNirdesh.Models.SqlHelper.ExecuteDataset(connection, "[getProjectByDeptId]", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ProjectName"].ToString());
                    item.Value = Convert.ToString(row["ProjectId"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            };
        }

        public static int SaveImage(ImageViewModel model)
        {
            try
            {



                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@programmeImageId", model.ProgrammeImageId);
                param[1] = new SqlParameter("@imageTitle", model.ImageTitle);
                param[2] = new SqlParameter("@imageTitleLocal", model.ImageTitle);
                param[3] = new SqlParameter("@imageDescription", model.ImageDescription);
                param[4] = new SqlParameter("@imageLocation", model.ImageLocation);
                param[5] = new SqlParameter("@isActive", model.IsActive);
                param[6] = new SqlParameter("@programmeId", model.ProjectId);
                param[7] = new SqlParameter("@createdBy", model.CreatedBy.ToString());

                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "sp_SaveUpdateImage", param);
                con.Close();
                return 1;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return 0;

            }
        }




        public static List<ImageViewModel> GetImageList()
        {
           
            List<ImageViewModel> lstImage = new List<ImageViewModel>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "getImageList");
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    ImageViewModel image = new ImageViewModel();
                    image.ProgrammeImageId = Convert.ToInt32(row["programmeImageId"].ToString());
                    image.ImageTitle = Convert.ToString(row["imageTitle"].ToString());
                 
                    image.ImageDescription = Convert.ToString(row["imageDescription"].ToString());
                  
                    image.ImageLocation = Convert.ToString(row["imageLocation"].ToString());
                    image.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());
                    image.IsActive = Convert.ToInt32(row["isActive"]);
                    image.ProjectId = Convert.ToInt32(row["programmeId"].ToString());
                    image.ProjectName = Convert.ToString(row["ProgrammeTitle"].ToString());
                   
                    image.path = Convert.ToString(Convert.ToString(row["path"]));
                    lstImage.Add(image);

                }
                return lstImage;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return lstImage;

            }
        }


        public static ImageViewModel GetImageListById(int Id)
        {
            ImageViewModel image = new ImageViewModel();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@programmeImageId", Id);


                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "[getImageListById]", param);
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    
                    image.ProgrammeImageId = Convert.ToInt32(row["programmeImageId"].ToString());
                    image.ImageTitle = Convert.ToString(row["imageTitle"].ToString());
                   
                    image.ImageDescription = Convert.ToString(row["imageDescription"].ToString());
                   
                    image.ImageLocation =  Convert.ToString(row["imageLocation"].ToString());
                    image.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());
                    image.IsActive = Convert.ToInt32(row["isActive"]);
                    image.ProjectId = Convert.ToInt32(row["programmeId"].ToString());
                    image.ProjectName = Convert.ToString(row["ProgrammeTitle"].ToString());
                   
                    image.ModifiedDate = string.IsNullOrWhiteSpace(Convert.ToString(row["modifiedDate"])) ? (DateTime?)null  : DateTime.Parse(Convert.ToString(row["modifiedDate"]));

                    image.path = Convert.ToString(Convert.ToString(row["path"]));







                }
                return image;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return image;

            }
        }


        public static void DeleteImageById(int Id)
        {

            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@programmeImageId", Id);


                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "[DeleteImageById]", param);
                con.Close();

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }

        public static void DeleteDeptImageById(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@programmeImageId", Id) };
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(connection, "[DeleteDeptImageById]", parameterValues);
                connection.Close();
            }
            catch (Exception)
            {
            }
        }

        public static List<ImageViewModel> GetDeptImageList(string deptId)
        {
            List<ImageViewModel> list = new List<ImageViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@deptId", deptId);
                connection.Close();
                foreach (DataRow row in CMNirdesh.Models.SqlHelper.ExecuteDataset(connection, "getDeptImageList", param).Tables[0].Rows)
                {
                    ImageViewModel item = new ImageViewModel
                    {
                        ProgrammeImageId = Convert.ToInt32(row["programmeImageId"].ToString()),
                        ImageTitle = Convert.ToString(row["imageTitle"].ToString()),
                       
                        ImageDescription = Convert.ToString(row["imageDescription"].ToString()),
                      
                        ImageLocation = Convert.ToString(row["imageLocation"].ToString()),
                        CreatedDate = Convert.ToDateTime(row["createdDate"].ToString()),
                        IsActive = Convert.ToInt32(row["isActive"]),
                        ProjectId = Convert.ToInt32(row["ProjectId"].ToString()),
                        ProjectName = Convert.ToString(row["projectname"].ToString()),
                       
                        path = Convert.ToString(Convert.ToString(row["path"])),
                       
                    };
                    list.Add(item);
                }
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        public static ImageViewModel GetDeptImageListById(int Id)
        {
            ImageViewModel model = new ImageViewModel();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@programmeImageId", Id) };
                connection.Close();
                foreach (DataRow row in CMNirdesh.Models.SqlHelper.ExecuteDataset(connection, "[getdeptImageListById]", parameterValues).Tables[0].Rows)
                {
                    DateTime? nullable1;
                    model.ProgrammeImageId = Convert.ToInt32(row["programmeImageId"].ToString());
                    model.ImageTitle = Convert.ToString(row["imageTitle"].ToString());
                  
                    model.ImageDescription = Convert.ToString(row["imageDescription"].ToString());
                  
                    model.ImageLocation = Convert.ToString(row["imageLocation"].ToString());
                    model.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());
                    model.IsActive = Convert.ToInt32(row["isActive"]);
                    model.ProjectId = Convert.ToInt32(row["ProjectId"].ToString());
                  
                    model.ProjectName = Convert.ToString(row["ProjectName"].ToString());
                    
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(row["modifiedDate"])))
                    {
                        nullable1 = new DateTime?(DateTime.Parse(Convert.ToString(row["modifiedDate"])));
                    }
                    else
                    {
                        nullable1 = null;
                    }
                    model.ModifiedDate = nullable1;
                    model.path = Convert.ToString(Convert.ToString(row["path"]));
                }
                return model;
            }
            catch (Exception)
            {
                return model;
            }
        }


        public static int SaveDeptImage(ImageViewModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@programmeImageId", model.ProgrammeImageId), new SqlParameter("@imageTitle", model.ImageTitle), new SqlParameter("@imageDescriptionLocal", model.ImageTitle), new SqlParameter("@imageDescription", model.ImageDescription),  new SqlParameter("@imageLocation", model.ImageLocation), new SqlParameter("@isActive", model.IsActive), new SqlParameter("@departmentdetailsid", model.ProjectId), new SqlParameter("@createdBy", model.CreatedBy.ToString()) };
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(connection, "[sp_SaveUpdateDeptImage]", parameterValues);
                connection.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }







    }

    public class ImageGallery
    {
        public ImageGallery()
        {
            ImageList = new List<string>();
        }
       
       
        public List<string> ImageList { get; set; }
    }
}