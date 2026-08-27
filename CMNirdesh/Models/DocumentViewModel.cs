using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CMNirdesh.Models
{


    public class DocumentViewModel
    {
        public Guid CreatedBy { get; set; }

        public Guid ModifiedBy { get; set; }

        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Document Title is Required")]


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Publish_date { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PublishEnd_date { get; set; }
        public string DocumentTitle { get; set; }
        public string UTubeLink { get; set; }
        public string DocumentFileName { get; set; }
        public string ProjectName { get; set; }

        public string path { get; set; }
       


        public string DocumentTitleLocal { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ? ModifiedDate { get; set; }

        public int IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public string Mode { get; set; }

        public SelectList ProjectList { get; set; }
        public SelectList StatusTypeList { get; set; }

        public int ProgrammeDocsId { get; set; }
       
        public string DocumentLocation
        {

            get;
            set;
        }

        public static int SaveDocument(DocumentViewModel model)
        {
            try
            {



                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@programmeDocsId", model.ProgrammeDocsId);
                param[1] = new SqlParameter("@documentTitle", model.DocumentTitle);
                param[2] = new SqlParameter("@documentTitleLocal", model.DocumentTitleLocal);
                param[3] = new SqlParameter("@documentLocation", model.DocumentLocation);
                param[4] = new SqlParameter("@isActive", model.IsActive);
                param[5] = new SqlParameter("@programmeId", model.ProjectId);
                param[6] = new SqlParameter("@createdBy", model.CreatedBy.ToString());
                param[7] = new SqlParameter("@StartPublish", model.Publish_date);
                param[8] = new SqlParameter("@StopPublish", model.PublishEnd_date);
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "sp_SaveUpdateDocument", param);
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

     

        public static List<DocumentViewModel> GetDocumentList()
        {
            
            List<DocumentViewModel> lstDocument = new List<DocumentViewModel>();
            try
            {
               SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "getDocumentList");
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    DocumentViewModel doc = new DocumentViewModel();
                    doc.ProgrammeDocsId = Convert.ToInt32(row["programmeDocsId"].ToString());
                    doc.DocumentTitle = Convert.ToString(row["documentTitle"].ToString());
                    doc.DocumentTitleLocal = Convert.ToString(row["DocumentTitleLocal"].ToString());
                    doc.DocumentLocation = Convert.ToString(row["documentLocation"].ToString());
                    doc.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                    doc.IsActive = Convert.ToInt32(row["isActive"]);
                    doc.ProjectId = Convert.ToInt32(row["ProjectId"].ToString());
                    doc.ProjectName = Convert.ToString(row["ProjectName"].ToString());
                    
                    doc.path = Convert.ToString(Convert.ToString(row["path"]));
                    lstDocument.Add(doc);

                }
                return lstDocument;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return lstDocument;

            }
        }


        public static DocumentViewModel GetDocumentListById(int Id)
        {
            DocumentViewModel doc = new DocumentViewModel();
            try
            {
              SqlConnection con = ClsConnection.GetConnection();

                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@programId", Id);


                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "[getDocumentListById]", param);
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    doc.ProgrammeDocsId = Convert.ToInt32(row["programmeDocsId"].ToString());
                    doc.DocumentTitle = Convert.ToString(row["documentTitle"].ToString());
                    doc.DocumentTitleLocal = Convert.ToString(row["DocumentTitleLocal"].ToString());
                    doc.DocumentLocation =  Convert.ToString(row["documentLocation"].ToString());
                    doc.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                    doc.ModifiedDate = string.IsNullOrWhiteSpace(Convert.ToString(row["modifiedDate"]))
    ? (DateTime?)null
    : DateTime.Parse(Convert.ToString(row["modifiedDate"]));

                 
                    doc.IsActive = Convert.ToInt32(row["isActive"]);
                    doc.ProjectId = Convert.ToInt32(row["ProjectId"].ToString());
                    doc.ProjectName = Convert.ToString(row["ProjectName"].ToString());
                   
                    doc.path = Convert.ToString(Convert.ToString(row["path"]));
                    doc.Publish_date = Convert.ToDateTime(Convert.ToString(row["StartPublish"]));
                    doc.PublishEnd_date = Convert.ToDateTime(Convert.ToString(row["StopPublish"]));



                }
                return doc;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return doc;

            }
        }

        public static List<DocumentViewModel> GetDeptDocumentList(string DeptId)
        {
            List<DocumentViewModel> list = new List<DocumentViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@DeptId", DeptId);
                connection.Close();
                foreach (DataRow row in CMNirdesh.Models.SqlHelper.ExecuteDataset(connection, "getDeptDocumentList", param).Tables[0].Rows)
                {
                    DocumentViewModel item = new DocumentViewModel
                    {
                        ProgrammeDocsId = Convert.ToInt32(row["programmeDocsId"].ToString()),
                        DocumentTitle = Convert.ToString(row["documentTitle"].ToString()),
                        DocumentTitleLocal = Convert.ToString(row["DocumentTitleLocal"].ToString()),
                        DocumentLocation = Convert.ToString(row["documentLocation"].ToString()),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                        IsActive = Convert.ToInt32(row["isActive"]),
                        ProjectId = Convert.ToInt32(row["ProjectId"].ToString()),
                        ProjectName = Convert.ToString(row["ProjectName"].ToString()),
                        
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
        public static List<DocumentViewModel> GetDeptDocumentListByDept(string DeptId)
        {
            List<DocumentViewModel> list = new List<DocumentViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptid", DeptId) };
                connection.Close();
                foreach (DataRow row in CMNirdesh.Models.SqlHelper.ExecuteDataset(connection, "GetDeptDocumentListByDept", parameterValues).Tables[0].Rows)
                {
                    DocumentViewModel item = new DocumentViewModel
                    {
                        ProgrammeDocsId = Convert.ToInt32(row["programmeDocsId"].ToString()),
                        DocumentTitle = Convert.ToString(row["documentTitle"].ToString()),
                        DocumentTitleLocal = Convert.ToString(row["DocumentTitleLocal"].ToString()),
                        DocumentLocation = Convert.ToString(row["documentLocation"].ToString()),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                        IsActive = Convert.ToInt32(row["isActive"]),
                        ProjectId = Convert.ToInt32(row["ProjectId"].ToString()),
                        ProjectName = Convert.ToString(row["ProjectName"].ToString()),
                     
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

        public static DocumentViewModel GetDeptDocumentListById(int Id)
        {
            DocumentViewModel model = new DocumentViewModel();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@programId", Id) };
                connection.Close();
                foreach (DataRow row in CMNirdesh.Models.SqlHelper.ExecuteDataset(connection, "[getDeptDocumentListById]", parameterValues).Tables[0].Rows)
                {
                    DateTime? nullable1;
                    model.ProgrammeDocsId = Convert.ToInt32(row["programmeDocsId"].ToString());
                    model.DocumentTitle = Convert.ToString(row["documentTitle"].ToString());
                   
                    model.DocumentLocation = Convert.ToString(row["documentLocation"].ToString());
                    model.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(row["modifiedDate"])))
                    {
                        nullable1 = new DateTime?(DateTime.Parse(Convert.ToString(row["modifiedDate"])));
                    }
                    else
                    {
                        nullable1 = null;
                    }
                    model.ModifiedDate = nullable1;
                    model.IsActive = Convert.ToInt32(row["isActive"]);
                    model.ProjectId = Convert.ToInt32(row["ProjectId"].ToString());
                    model.ProjectName = Convert.ToString(row["ProjectName"].ToString());
                   
                  
                    model.path = Convert.ToString(Convert.ToString(row["path"]));
                    model.Publish_date = Convert.ToDateTime(Convert.ToString(row["StartPublish"]));
                    model.PublishEnd_date = Convert.ToDateTime(Convert.ToString(row["StopPublish"]));
                    model.UTubeLink= Convert.ToString(Convert.ToString(row["UTubeLink"])); 
                }
                return model;
            }
            catch (Exception)
            {
                return model;
            }
        }


        public static int SaveDocumentDepartment(DocumentViewModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@programmeDocsId", model.ProgrammeDocsId), new SqlParameter("@documentTitle", model.DocumentTitle), new SqlParameter("@documentTitle", model.DocumentTitleLocal), new SqlParameter("@documentLocation", model.DocumentLocation), new SqlParameter("@isActive", model.IsActive), new SqlParameter("@departmentDetailsId", model.ProjectId), new SqlParameter("@createdBy", model.CreatedBy.ToString()),
           new SqlParameter("@StartPublish", model.Publish_date), new SqlParameter("@StopPublish", model.PublishEnd_date),new SqlParameter("@UTubeLink",model.UTubeLink)


            };
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(connection, "sp_SaveUpdateDepartmentDetailsDocument", parameterValues);
                connection.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }



        public static void DeleteDocumentById(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@programmeDocsId", Id) };
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(connection, "[DeleteDeptDocumentById]", parameterValues);
                connection.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
