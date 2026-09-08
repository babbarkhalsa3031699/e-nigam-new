using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using CMNirdesh.Error;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf.parser;

namespace CMNirdesh.Models
{

    [Serializable]
    public class PriorityProjectList : System.Collections.ObjectModel.ObservableCollection<PriorityProject> { }
    [Serializable]
    public class PriorityProject
    {
        public Int64 ProjectId
        {

            get;
            set;
        }
        [Required(ErrorMessage = "Please select promise")]
        public Int64 PromiseId
        {

            get;
            set;
        }
        [NotMapped]
        public string PromiseName
        {

            get;
            set;
        }
        public string RefId
        {

            get;
            set;
        }
        public string PromiseRefId
        {

            get;
            set;
        }
        public string DeptId
        {

            get;
            set;
        }
        public string OfficeId
        {

            get;
            set;
        }

        [NotMapped]
        public string DeptName
        {

            get;
            set;
        }
        public string OfficeName
        {

            get;
            set;
        }
        public int ResolutioNo { get; set; }
        public int ResolutionOfcType { get; set; }
        public string Resolution { get; set; }
        public int implementedProject { get; set; }
        public int PartiallyImplemented { get; set; }
        public int NotImplemented { get; set; }
        public string isapex { get; set; }
        public int? noStatusProject { get; set; }
        public int totalProject { get; set; }
        public int totalOffices { get; set; }
        public int totalWorks { get; set; }
        public int totalDepartment { get; set; }
        public string Msg { get; set; }
        public string isParent { get; set; }
        [Required(ErrorMessage = "Please enter project name")]
        public string ProjectName
        {

            get;
            set;
        }
        public string FirmName
        {

            get;
            set;
        }
        [Required(ErrorMessage = "Please select category")]
        public int CategoryId
        {

            get;
            set;
        }
        [NotMapped]
        public string CategoryName
        {

            get;
            set;
        }
        public Int64 ProgrammeDocsId { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentTitleLocal { get; set; }
        public string DocumentFileName { get; set; }
        public string DocumentLocation
        {
            get;
            set;
        }
        public string projectedityn { get; set; }
        public string deptedityn { get; set; }
        public string workedityn { get; set; }
        public string EstimatedAmount
        {
            get;
            set;
        }
        public string RevisedEstimatedAmount
        {

            get;
            set;
        }
        public string FundReleased
        {

            get;
            set;
        }
        public string Expenditure
        {

            get;
            set;
        }
        public string PhysicalProgress
        {

            get;
            set;
        }

        [Required(ErrorMessage = "Please select Start Date")]
        //[DataType(DataType.Date)]
        public string SStartDate
        {

            get;
            set;
        }
        public string SEndDate
        {

            get;
            set;
        }
        public DateTime? StartDate
        {

            get;
            set;
        }
        [Required(ErrorMessage = "Please select End Date")]
        //[DataType(DataType.Date)]
        public DateTime? EndDate
        {

            get;
            set;
        }
        [Required(ErrorMessage = "Please select project status")]
        public int ProjectStatusId
        {

            get;
            set;
        }
        [NotMapped]
        public string ProjectStatusName
        {

            get;
            set;
        }
        [AllowHtml]
        public string Remark
        {

            get;
            set;
        }
        public string useractiontype
        {
            get;
            set;
        }
        public string projectUserActions
        {
            get;
            set;
        }
        public string projectRadioActions
        {
            get;
            set;
        }
        public string projectServiceName
        {
            get;
            set;
        }
        public string mode
        {
            get;
            set;
        }
        public bool IsImage
        {
            get; set;
        }
        public bool IsDocument
        {
            get; set;
        }
        public bool IsConstituency
        {
            get; set;
        }
        public bool IsWork
        {
            get; set;
        }
        public bool isActive
        {

            get;
            set;
        }
        [NotMapped]
        public bool isDelete
        {

            get;
            set;
        }

        [NotMapped]
        public string createdBy
        {
            get;
            set;
        }
        public Guid ModifiedBy { get; set; }
        public Int64 ProjectMappedDeptId { get; set; }
        public string mdeptid { get; set; }
        public SelectList StatusTypeList { get; set; }
        public SelectList PromiseTypeList { get; set; }
        public SelectList CategoryTypeList { get; set; }
        public SelectList ResolutionOfcTypeList { get; set; }
        public SelectList ResolutionList { get; set; }
        public SelectList ProjectStatusTypeList { get; set; }
        public SelectList ProjectListItems { get; set; }
        public List<PriorityProject> projectslist { get; set; }
        public List<PriorityProject> workimageslist { get; set; }
        public List<PriorityProject> workdocumentslist { get; set; }
        public PriorityProject worksdatalist { get; set; }
        public PriorityProject projectactions { get; set; }
        public PriorityProject workimagedetails { get; set; }
        public List<string> projectDepartmentsList { get; set; }
        public SelectList mDepartmentList { get; set; }
        public SelectList mOfficeList { get; set; }
        public SelectList mProjectDepartmentList { get; set; }
        public Int64 ConstituencyId { get; set; }
        public SelectList mProjectConstituencyList { get; set; }
        public SelectList mWorkConstituencyList { get; set; }
        [Required(ErrorMessage = "Image Title is Required")]
        public string ImageTitle { get; set; }
        public string ImageTitle2 { get; set; }
        public string ImageTitle3 { get; set; }
        public string path { get; set; }
        public string ImageFileName { get; set; }
        public string ImageFileName2 { get; set; }
        public string ImageFileName3 { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Int64 ProjectMappedConstituencyId { get; set; }
        public string cdid { get; set; }
        public string ConstituencyName { get; set; }
        public int ProgrammeImageId { get; set; }
        public string ImageThumbLocation { get; set; }
        public string ImageLocation { get; set; }
        public string ImageLocation2 { get; set; }
        public string ImageLocation3 { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Publish_date { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PublishEnd_date { get; set; }
        public string UTubeLink { get; set; }
        public List<PriorityProject> imageslist { get; set; }
        [AllowHtml]
        public string ImageDescription { get; set; }
        public static SelectList GetPromise(string flag)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@deptId", CurrentSession.DeptID);
                param[1] = new SqlParameter("@OfficeId", CurrentSession.OfficeId);
                param[2] = new SqlParameter("@isapex", CurrentSession.isApex);
                param[3] = new SqlParameter("@flag", flag);
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getPromise]", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["PromiseName"].ToString());
                    item.Value = Convert.ToString(row["PromiseId"].ToString());
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
        public static SelectList GetCategory()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Clear();
            items.Add(new SelectListItem
            {
                Text = "--Select Category--",
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
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getProjectCategory]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["CategoryName"].ToString());
                    item.Value = Convert.ToString(row["CategoryId"].ToString());
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
        public static SelectList GetResolutionNo(int ResolutionType)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Clear();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@deptId", CurrentSession.DeptID);
                param[1] = new SqlParameter("@ResolutionType", ResolutionType);
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetResolutionNoList]", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["Resolution"].ToString());
                    item.Value = Convert.ToString(row["ResolutionNo"].ToString());
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
        public static SelectList GetResolutionOfcType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Clear();
            items.Add(new SelectListItem
            {
                Text = "Select",
                Value = "0",
                Selected = true,
            });
            items.Add(new SelectListItem
            {
                Text = "House",
                Value = "1",
            });
            items.Add(new SelectListItem
            {
                Text = "F&CC",
                Value = "5",
            });
            return new SelectList(items, "Value", "Text");
        }
        public static SelectList GetProjectDepartments(Int64 projectid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Clear();
            //items.Add(new SelectListItem
            //{
            //    Text = "--Select Department--",
            //    Value = "",
            //    Selected = true,
            //});
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                string flag = "Dept";
                if (CurrentSession.serviceName == "MC")
                {
                    flag = "office";
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@projectid", projectid);
                param[1] = new SqlParameter("@flag", flag);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetProjectDepartments]", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["deptname"].ToString());
                    item.Value = Convert.ToString(row["DeptId"].ToString());
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
        public static SelectList GetProjectDepartmentsforDelete(Int64 projectid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Clear();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                string flag = "Dept";
                if (CurrentSession.serviceName == "MC")
                {
                    flag = "office";
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@projectid", projectid);
                param[1] = new SqlParameter("@flag", flag);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetProjectDepartments]", param).Tables[1].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["deptname"].ToString());
                    item.Value = Convert.ToString(row["DeptId"].ToString());
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
        public static SelectList GetProjectStatus()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Clear();
            items.Add(new SelectListItem
            {
                Text = "--Select Project Status--",
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
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getProjectstatus]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ProjectStatusName"].ToString());
                    item.Value = Convert.ToString(row["ProjectStatusId"].ToString());
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
        public static PriorityProjectList GetProjectList(string DeptId)
        {
            PriorityProjectList list = new PriorityProjectList();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@DeptId", DeptId);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getPriorityProject]", param).Tables[0].Rows)
                {
                    PriorityProject item = new PriorityProject
                    {
                        PromiseId = Convert.ToInt64(row["PromiseId"].ToString()),
                        ProjectId = Convert.ToInt64(row["ProjectId"]),
                        CategoryId = Convert.ToInt32(row["CategoryId"].ToString()),
                        CategoryName = Convert.ToString(row["CategoryName"].ToString()),
                        DeptId = Convert.ToString(row["DeptId"].ToString()),
                        DeptName = Convert.ToString(row["DeptName"].ToString()),
                        SStartDate = Convert.ToString(row["StartDate"].ToString()),
                        SEndDate = Convert.ToString(row["EndDate"].ToString()),
                        EstimatedAmount = Convert.ToString(row["EstimatedAmount"].ToString()),
                        Expenditure = Convert.ToString(row["Expenditure"].ToString()),
                        FirmName = Convert.ToString(row["FirmName"].ToString()),
                        FundReleased = Convert.ToString(row["FundReleased"].ToString()),
                        PhysicalProgress = Convert.ToString(row["FundReleased"].ToString()),
                        ProjectName = Convert.ToString(row["ProjectName"].ToString()),
                        ProjectStatusName = Convert.ToString(row["ProjectStatusName"].ToString()),
                        Remark = Convert.ToString(row["Remark"].ToString()),
                        ProjectStatusId = Convert.ToInt32(row["ProjectStatusId"].ToString()),
                        PromiseName = Convert.ToString(row["PromiseName"].ToString()),
                        isActive = Convert.ToBoolean(row["isActive"].ToString()),

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
        public static List<CMNirdesh.Models.PriorityProject> getCMProjectsList(int projectId, string deptId, int constituencyId, int? statusId, string currdeptId, int currOfficeId, string isapex)// Method to all all projects by ritika
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@PromiseId", projectId);
                param[1] = new SqlParameter("@deptId", deptId);
                param[2] = new SqlParameter("@constituencyid", constituencyId);
                param[3] = new SqlParameter("@status", statusId);
                param[4] = new SqlParameter("@currdeptId", currdeptId);
                param[5] = new SqlParameter("@currOfficeId", currOfficeId);
                param[6] = new SqlParameter("@isapex", Convert.ToInt16(isapex));
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getcmpromisesdata]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.PromiseId = Convert.ToInt64(row["PromiseId"].ToString());
                    item.PromiseRefId = row["PromiseRefId"].ToString();
                    item.PromiseName = Convert.ToString(row["PromiseName"].ToString());
                    if (row["ProjectId"] != System.DBNull.Value)
                    {
                        item.ProjectId = Convert.ToInt64(row["ProjectId"]);
                    }
                    item.SStartDate = Convert.ToString(row["StartDate"].ToString());
                    item.SEndDate = Convert.ToString(row["EndDate"].ToString());
                    //if (row["EndDate"] != System.DBNull.Value)
                    //{
                    //    item.EndDate = DateTime.ParseExact(row["EndDate"].ToString(), "dd/MM/yyyy", null);
                    //}
                    //if (row["StartDate"] != System.DBNull.Value)
                    //{
                    //    item.StartDate = DateTime.ParseExact(row["StartDate"].ToString(), "dd/MM/yyyy", null);
                    //}
                    if (row["CategoryId"] != System.DBNull.Value)
                    {
                        item.CategoryId = Convert.ToInt32(row["CategoryId"].ToString());
                    }
                    if (row["ProjectStatusId"] != System.DBNull.Value)
                    {
                        item.ProjectStatusId = Convert.ToInt32(row["ProjectStatusId"].ToString());
                    }
                    item.CategoryName = Convert.ToString(row["CategoryName"].ToString());
                    item.ConstituencyName = Convert.ToString(row["ConstituencyName"].ToString());
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["DeptName"].ToString());
                    item.EstimatedAmount = Convert.ToString(row["EstimatedAmount"].ToString());
                    item.Expenditure = Convert.ToString(row["Expenditure"].ToString());
                    item.FirmName = Convert.ToString(row["FirmName"].ToString());
                    item.FundReleased = Convert.ToString(row["FundReleased"].ToString());
                    item.PhysicalProgress = Convert.ToString(row["PhysicalProgress"].ToString());
                    item.ProjectName = Convert.ToString(row["ProjectName"].ToString());
                    item.Remark = Convert.ToString(row["Remark"].ToString());
                    if (row["modifieddate"] != System.DBNull.Value)
                    {
                        item.ProjectStatusName = Convert.ToString(row["ProjectStatusName"].ToString()) + "<br>" + Convert.ToString(row["modifieddate"].ToString()) + "<br>" + Convert.ToString(row["modifiedtime"].ToString());
                    }
                    else
                    {
                        item.ProjectStatusName = Convert.ToString(row["ProjectStatusName"].ToString());

                    }
                    item.path = Convert.ToString(row["path"].ToString());
                    item.isParent = Convert.ToString(row["isParent"].ToString());

                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                // Console.Write(ex.Message);
                // CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getprojectdeptlist(int projectId, string deptId, int constituencyId, int? statusId, string flag)// Method to all all projects by ritika
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@PromiseId", projectId);
                param[1] = new SqlParameter("@currdeptId", CurrentSession.DeptID);
                param[2] = new SqlParameter("@currofcId", CurrentSession.OfficeId);
                param[3] = new SqlParameter("@deptId", deptId);
                param[4] = new SqlParameter("@constituencyid", constituencyId);
                param[5] = new SqlParameter("@status", statusId);
                param[6] = new SqlParameter("@flag", flag);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getprojectdeptlist]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.PromiseId = Convert.ToInt64(row["PromiseId"].ToString());
                    item.PromiseRefId = row["PromiseRefId"].ToString();
                    item.PromiseName = Convert.ToString(row["PromiseName"].ToString());
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["DeptName"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                // Console.Write(ex.Message);
                // CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getDeptProjectsList(string Id, string deptId)// Method to all all projects by ritika
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@PromiseId", Convert.ToInt64(Id));
                param[1] = new SqlParameter("@deptId", deptId);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getdeptprojectdata]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.PromiseId = Convert.ToInt64(row["PromiseId"].ToString());
                    item.PromiseRefId = row["PromiseRefId"].ToString();
                    item.PromiseName = Convert.ToString(row["PromiseName"].ToString());

                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["DeptName"].ToString());
                    item.OfficeName = Convert.ToString(row["department"].ToString());

                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                // Console.Write(ex.Message);
                // CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return items;
            }
        }
        public static PriorityProject GetProjectById(Int64 ProjectId)
        {
            PriorityProject item = new PriorityProject();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ProjectId", ProjectId);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetProjectById]", param).Tables[0].Rows)
                {

                    item.PromiseId = Convert.ToInt64(row["PromiseId"].ToString());
                    item.ProjectId = Convert.ToInt64(row["ProjectId"]);
                    item.CategoryId = Convert.ToInt32(row["CategoryId"].ToString());
                    item.CategoryName = Convert.ToString(row["CategoryName"].ToString());
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["DeptName"].ToString());
                    item.SStartDate = Convert.ToString(row["StartDate"].ToString());
                    item.SEndDate = Convert.ToString(row["EndDate"].ToString());
                    item.EstimatedAmount = Convert.ToString(row["EstimatedAmount"].ToString());
                    item.Expenditure = Convert.ToString(row["Expenditure"].ToString());
                    item.FirmName = Convert.ToString(row["FirmName"].ToString());
                    item.FundReleased = Convert.ToString(row["FundReleased"].ToString());
                    item.PhysicalProgress = Convert.ToString(row["FundReleased"].ToString());
                    item.ProjectName = Convert.ToString(row["ProjectName"].ToString());
                    item.Remark = Convert.ToString(row["Remark"].ToString());
                    item.ProjectStatusId = Convert.ToInt32(row["ProjectStatusId"].ToString());
                    item.PromiseName = Convert.ToString(row["PromiseName"].ToString());
                    item.isActive = Convert.ToBoolean(row["isActive"].ToString());
                    if (row["modifieddate"] != System.DBNull.Value)
                    {
                        item.ProjectStatusName = Convert.ToString(row["ProjectStatusName"].ToString()) + "<br>" + Convert.ToString(row["modifieddate"].ToString());
                    }
                    else
                    {
                        item.ProjectStatusName = Convert.ToString(row["ProjectStatusName"].ToString());

                    }

                }
                return item;
            }
            catch (Exception)
            {
                return item;
            }
        }
        public static int SaveProjectWork(PriorityProject model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[21];
                param[0] = new SqlParameter("@ProjectId", model.ProjectId);
                param[1] = new SqlParameter("@PromiseId", model.PromiseId);
                param[2] = new SqlParameter("@DeptId", model.DeptId);
                param[3] = new SqlParameter("@ProjectName", model.worksdatalist.ProjectName);
                param[4] = new SqlParameter("@FirmName", model.worksdatalist.FirmName);
                param[5] = new SqlParameter("@CategoryId", model.worksdatalist.CategoryId);
                param[6] = new SqlParameter("@EstimatedAmount", model.worksdatalist.EstimatedAmount);
                param[7] = new SqlParameter("@RevisedEstimatedAmount", model.worksdatalist.RevisedEstimatedAmount);
                param[8] = new SqlParameter("@FundReleased", model.worksdatalist.FundReleased);
                param[9] = new SqlParameter("@Expenditure", model.worksdatalist.Expenditure);
                param[10] = new SqlParameter("@PhysicalProgress", model.worksdatalist.PhysicalProgress);
                param[11] = new SqlParameter("@StartDate", model.worksdatalist.StartDate);
                param[12] = new SqlParameter("@EndDate", model.worksdatalist.EndDate);
                param[13] = new SqlParameter("@ProjectStatusId", model.worksdatalist.ProjectStatusId);
                param[14] = new SqlParameter("@Remark", model.worksdatalist.Remark);
                param[15] = new SqlParameter("@createdBy", model.createdBy);
                param[16] = new SqlParameter("@isActive", model.isActive);
                param[17] = new SqlParameter("@constituency", model.cdid);
                param[18] = new SqlParameter("@OfficeId", model.OfficeId);
                param[19] = new SqlParameter("@ResolutionNo", model.worksdatalist.ResolutioNo);
                param[20] = new SqlParameter("@ResolutionOfcType", model.worksdatalist.ResolutionOfcType);
                SqlHelper.ExecuteNonQuery(con, "SaveProject", param);
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
        public static SelectList GetOfficebyDepartmentId(string deptid)
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
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@deptid", deptid);
                param[1] = new SqlParameter("@officeid", CurrentSession.OfficeId);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetOfficeListByDeptID]", param).Tables[0].Rows)
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
        public static SelectList GetDeptOfficeListforProject(string deptid, string officeid, string flag, int apex, int projectid)
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
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@deptid", deptid);
                param[1] = new SqlParameter("@officeid", officeid);
                param[2] = new SqlParameter("@flag", flag);
                param[3] = new SqlParameter("@apex", apex);
                param[4] = new SqlParameter("@projectid", projectid);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "GetDeptOfficeListforProject", param).Tables[0].Rows)
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
        public static SelectList GetDeptsListforProject(string deptid, string officeid, string flag, int apex, int projectid)
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
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@deptid", deptid);
                param[1] = new SqlParameter("@officeid", officeid);
                param[2] = new SqlParameter("@flag", flag);
                param[3] = new SqlParameter("@apex", apex);
                param[4] = new SqlParameter("@projectid", projectid);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "GetDeptsListforProject", param).Tables[0].Rows)
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
        public static void DeleteProjectById(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@ProjectId", Id) };
                SqlHelper.ExecuteNonQuery(connection, "[DeleteProjectById]", parameterValues);
                connection.Close();
            }
            catch (Exception)
            {
            }
        }
        public DataTable GetProjectConstituency(int projectId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@ProjectId", projectId));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetProjectConstituency", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public List<Dictionary<string, object>> GetTableRows(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }
        public List<PriorityProject> lstPriorityProject
        {

            get;
            set;
        }
        public string GenerateProjectNameUrl()
        {
            string phrase = string.Format("{0}-{1}", PromiseId, PromiseName);

            string str = RemoveAccent(phrase).ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            str = Regex.Replace(str, @"\s+", " ").Trim();

            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }
        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
        public Boolean isDeleted
        {

            get;
            set;
        }
        public static PromiseList GetPromiseList(string deptId)
        {
            PromiseList list = new PromiseList();
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getPromise]", param).Tables[0].Rows)
                {
                    Promise item = new Promise
                    {
                        PromiseId = Convert.ToInt64(row["PromiseId"].ToString()),
                        isActive = Convert.ToBoolean(row["isActive"]),
                        PromiseName = Convert.ToString(row["PromiseName"].ToString()),
                        DeptId = Convert.ToString(row["DeptId"].ToString()),
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
        public static Promise GetPromiseListById(int Id)
        {
            Promise promise = new Promise();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@promiseId", Id);


                DataSet ds = SqlHelper.ExecuteDataset(con, "[getPromiseListbyId]", param);
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    promise.PromiseId = Convert.ToInt64(row["PromiseId"].ToString());
                    promise.isActive = Convert.ToBoolean(row["isActive"]);
                    promise.PromiseName = Convert.ToString(row["PromiseName"].ToString());
                    promise.DeptId = Convert.ToString(row["DeptId"].ToString());




                }
                return promise;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return promise;

            }
        }
        public static int SavePromise(PriorityProject model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@CreatedBy", model.createdBy.ToString());
                param[1] = new SqlParameter("@Active", model.isActive);
                param[2] = new SqlParameter("@IsDeleted", model.isDeleted);
                param[3] = new SqlParameter("@PromiseId", model.PromiseId);
                param[4] = new SqlParameter("@PromiseName", model.PromiseName);
                param[5] = new SqlParameter("@deptid", model.DeptId);
                SqlHelper.ExecuteNonQuery(con, "[SavePromise]", param);
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
        public static SelectList GetStatusTypes()
        {
            var result = new List<SelectListItem>();
            // result.Add(new SelectListItem() { Text = "Select Status Type", Value = "0" });
            result.Add(new SelectListItem() { Text = "Published", Value = "True" });
            result.Add(new SelectListItem() { Text = "Unpublished ", Value = "False" });
            //  result.Add(new SelectListItem() { Text = "Archived", Value = "3" });
            //  result.Add(new SelectListItem() { Text = "Trashed", Value = "4" });


            return new SelectList(result, "Value", "Text");
        }
        public static void DeletePromiseById(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@PromiseId", Id) };
                SqlHelper.ExecuteNonQuery(connection, "[DeletePromiseById]", parameterValues);
                connection.Close();
            }
            catch (Exception)
            {
            }
        }
        public static SelectList getDepartment()
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDepartment]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["deptname"].ToString());
                    item.Value = Convert.ToString(row["deptid"].ToString());
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
        public static SelectList getUserDepartment(string userId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@userId", userId);
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDepartmentsbyUser]", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["deptname"].ToString());
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
        public static String SaveDepartmentProjectMapping(PriorityProject model, string DeptId, int officeId, string action, int isApex)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@ProjectId", model.PromiseId);
                param[1] = new SqlParameter("@DeptId", DeptId);
                param[2] = new SqlParameter("@officeId", officeId);
                param[3] = new SqlParameter("@isActive", model.isActive);
                param[4] = new SqlParameter("@createdBy", model.createdBy.ToString());
                param[5] = new SqlParameter("@flag", action);
                param[6] = new SqlParameter("@isapex", isApex);
                foreach (DataRow row in SqlHelper.ExecuteDataset(con, "[SaveDeptProjectMapping]", param).Tables[0].Rows)
                {
                    return row["RESULT"].ToString();
                }
                con.Close();
                return "N";
            }
            catch (Exception ex)
            {
                return "N";

            }
        }
        public static int SaveConstituency(PriorityProject model, string action)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@ProjectId", model.ProjectId);
                param[1] = new SqlParameter("@ConstituencyId", model.cdid);
                param[2] = new SqlParameter("@isActive", model.isActive);
                param[3] = new SqlParameter("@CreatedBy", model.createdBy);
                param[4] = new SqlParameter("@flag", action);
                SqlHelper.ExecuteNonQuery(con, "[SaveConstituencyProjectMapping]", param);
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }
        }
        public static PriorityProject GetWorkData(string promiseId, string deptID, int workID)
        {
            PriorityProject workDetail = new PriorityProject();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@promiseId", promiseId);
                param[1] = new SqlParameter("@deptId", deptID);
                param[2] = new SqlParameter("@workID", workID);


                DataSet ds = SqlHelper.ExecuteDataset(con, "[getprojectworkdata]", param);
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    workDetail.ProjectId = Convert.ToInt64(row["ProjectId"]);
                    workDetail.EstimatedAmount = Convert.ToString(row["EstimatedAmount"].ToString());
                    workDetail.RevisedEstimatedAmount = Convert.ToString(row["RevisedEstimatedAmount"].ToString());
                    workDetail.Expenditure = Convert.ToString(row["Expenditure"].ToString());
                    workDetail.FirmName = Convert.ToString(row["FirmName"].ToString());
                    workDetail.FundReleased = Convert.ToString(row["FundReleased"].ToString());
                    workDetail.ProjectName = Convert.ToString(row["ProjectName"].ToString());
                    workDetail.Remark = Convert.ToString(row["Remark"].ToString());
                    workDetail.CategoryName = Convert.ToString(row["CategoryName"].ToString());
                    workDetail.CategoryId = Convert.ToInt16(row["CategoryId"].ToString());
                    workDetail.PromiseId = Convert.ToInt64(row["PromiseId"].ToString());
                    workDetail.PromiseName = Convert.ToString(row["PromiseName"].ToString());
                    workDetail.DeptId = Convert.ToString(row["deptId"].ToString());
                    workDetail.DeptName = Convert.ToString(row["deptname"].ToString());
                    workDetail.SStartDate = row["StartDate"].ToString();
                    workDetail.SEndDate = row["EndDate"].ToString();

                    workDetail.PhysicalProgress = Convert.ToString(row["PhysicalProgress"].ToString());
                    workDetail.ProjectStatusId = Convert.ToInt16(row["ProjectStatusId"].ToString());
                    workDetail.ProjectStatusName = Convert.ToString(row["ProjectStatusName"].ToString());
                    workDetail.ResolutioNo = Convert.ToInt32(row["resolutionNo"].ToString());
                    workDetail.ResolutionOfcType = Convert.ToInt32(row["ResolutionOfcType"].ToString());
                    workDetail.Resolution = row["resolution"].ToString();

                }
                return workDetail;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return workDetail;

            }



        }

        //image model

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
        public static int SaveImage(PriorityProject model)
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
                param[5] = new SqlParameter("@isActive", model.isActive);
                param[6] = new SqlParameter("@programmeId", model.ProjectId);
                param[7] = new SqlParameter("@createdBy", model.createdBy.ToString());

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
        public static List<PriorityProject> GetImageList()
        {

            List<PriorityProject> lstImage = new List<PriorityProject>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "getImageList");
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    PriorityProject image = new PriorityProject();
                    image.ProgrammeImageId = Convert.ToInt32(row["programmeImageId"].ToString());
                    image.ImageTitle = Convert.ToString(row["imageTitle"].ToString());

                    image.ImageDescription = Convert.ToString(row["imageDescription"].ToString());

                    image.ImageLocation = Convert.ToString(row["imageLocation"].ToString());
                    image.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());
                    image.isActive = Convert.ToBoolean(row["isActive"]);
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
        public static PriorityProject GetImageListById(int Id)
        {
            PriorityProject image = new PriorityProject();
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

                    image.ImageLocation = Convert.ToString(row["imageLocation"].ToString());
                    image.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());
                    image.isActive = Convert.ToBoolean(row["isActive"]);
                    image.ProjectId = Convert.ToInt32(row["programmeId"].ToString());
                    image.ProjectName = Convert.ToString(row["ProgrammeTitle"].ToString());

                    image.ModifiedDate = string.IsNullOrWhiteSpace(Convert.ToString(row["modifiedDate"])) ? (DateTime?)null : DateTime.Parse(Convert.ToString(row["modifiedDate"]));

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
        public static int DeleteDeptImageById(int Id)
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
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static PriorityProject GetDeptImageListById(int Id)
        {
            PriorityProject model = new PriorityProject();
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
                    model.ImageThumbLocation = Convert.ToString(row["imageThumbnailLocation"].ToString());
                    model.CreatedDate = Convert.ToDateTime(row["createdDate"].ToString());
                    model.isActive = Convert.ToBoolean(row["isActive"]);
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
        public static int SaveDeptImage(PriorityProject model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@programmeImageId", model.ProgrammeImageId),
                    new SqlParameter("@imageTitle", model.ImageTitle),
                    new SqlParameter("@imageDescriptionLocal", model.ImageTitle),
                    new SqlParameter("@imageDescription", model.ImageDescription),
                    new SqlParameter("@imageLocation", model.ImageLocation),
                    new SqlParameter("@imageThumbLocation", model.ImageThumbLocation),
                    new SqlParameter("@isActive", model.isActive),
                    new SqlParameter("@departmentdetailsid",model.ProjectId),
                    new SqlParameter("@createdBy", model.createdBy.ToString()) };
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(connection, "[sp_SaveUpdateDeptImage]", parameterValues);
                connection.Close();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> GetProjectConstituencyList(string deptId)
        {
            List<CMNirdesh.Models.PriorityProject> list = new List<CMNirdesh.Models.PriorityProject>();
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getPromiseMappedConstituecyList]", param).Tables[0].Rows)
                {
                    PriorityProject item = new PriorityProject
                    {
                        cdid = Convert.ToString(row["ConstituencyId"].ToString()),
                        ConstituencyName = Convert.ToString(row["ProjectName"].ToString()),
                        ProjectName = Convert.ToString(row["ConstituencyName"].ToString()),
                        isActive = Convert.ToBoolean(row["isActive"]),
                        ProjectId = Convert.ToInt64(row["ProjectId"]),
                        ProjectMappedConstituencyId = Convert.ToInt64(row["ProjectMappedConstituencyId"]),
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
        public static List<PriorityProject> GetDeptImageList(string deptId)
        {
            List<PriorityProject> list = new List<PriorityProject>();
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
                    PriorityProject item = new PriorityProject
                    {
                        ProgrammeImageId = Convert.ToInt32(row["programmeImageId"].ToString()),
                        ImageTitle = Convert.ToString(row["imageTitle"].ToString()),

                        ImageDescription = Convert.ToString(row["imageDescription"].ToString()),

                        ImageLocation = Convert.ToString(row["imageLocation"].ToString()),
                        ImageThumbLocation = Convert.ToString(row["imageThumbnailLocation"].ToString()),
                        CreatedDate = Convert.ToDateTime(row["createdDate"].ToString()),
                        isActive = Convert.ToBoolean(row["isActive"]),
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
        public List<CMNirdesh.Models.PriorityProject> getWorkImage(Int64 ProjectId)
        {
            List<CMNirdesh.Models.PriorityProject> list = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ProjectId", ProjectId);
                connection.Close();
                foreach (DataRow row in CMNirdesh.Models.SqlHelper.ExecuteDataset(connection, "GetProjectDocImage", param).Tables[0].Rows)
                {
                    PriorityProject item = new PriorityProject
                    {
                        ProgrammeImageId = Convert.ToInt32(row["programmeImageId"].ToString()),
                        ImageTitle = row["imageTitle"].ToString(),

                        ImageDescription = row["ImageDescription"].ToString() + "<br>" + row["publishDate"].ToString() + "<br>" + row["publishtime"].ToString(),

                        ImageLocation = Convert.ToString(row["imageLocation"].ToString()),
                        ImageThumbLocation = Convert.ToString(row["imageThumbnailLocation"].ToString()),
                        CreatedDate = DateTime.ParseExact(row["publishDate"].ToString(), "dd/MM/yyyy", null),//  Convert.ToDateTime(row["publishDate"].ToString()),
                        path = Convert.ToString(row["path"]),
                    };

                    list.Add(item);
                }
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }

        }
        public List<CMNirdesh.Models.PriorityProject> getWorkDocuments(Int64 ProjectId)
        {
            List<CMNirdesh.Models.PriorityProject> list = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ProjectId", ProjectId);
                connection.Close();
                foreach (DataRow row in CMNirdesh.Models.SqlHelper.ExecuteDataset(connection, "GetProjectDocImage", param).Tables[1].Rows)
                {
                    PriorityProject item = new PriorityProject
                    {
                        CreatedDate =   Convert.ToDateTime(row["publishDate"].ToString()),
                        DocumentLocation = row["documentLocation"].ToString(),
                        DocumentTitle = row["DocumentTitle"].ToString(),
                        ProgrammeDocsId = Convert.ToInt64(row["ProgrammeDocsId"]),


                        DocumentTitleLocal = Convert.ToString(row["documentTitleLocal"].ToString()),
                        isActive = Convert.ToBoolean(row["isActive"]),
                        ProjectId = Convert.ToInt32(row["ProjectId"].ToString()),
                        ProjectName = Convert.ToString(row["projectname"].ToString()),

                        path = Convert.ToString(row["path"]),
                    };

                    list.Add(item);
                }
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }

        }
        public static int DeleteDocumentById(int Id)
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
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int SaveDocumentDepartment(PriorityProject model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@programmeDocsId", model.ProgrammeDocsId),
                    new SqlParameter("@documentTitle", model.DocumentTitle),
                    new SqlParameter("@documentTitle", model.DocumentTitleLocal),
                    new SqlParameter("@documentLocation", model.DocumentLocation),
                    new SqlParameter("@isActive", model.isActive),
                    new SqlParameter("@departmentDetailsId", model.ProjectId),
                    new SqlParameter("@createdBy", model.createdBy.ToString()),
                    new SqlParameter("@StartPublish", model.Publish_date),
                    new SqlParameter("@StopPublish", model.PublishEnd_date),
                    new SqlParameter("@UTubeLink",model.UTubeLink)


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
        public static SelectList getWorkConstituency(Int64 ProjectId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Clear();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ProjectId", ProjectId);
                connection.Close();
                foreach (DataRow row in CMNirdesh.Models.SqlHelper.ExecuteDataset(connection, "GetProjectConstituency", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ConstituencyName"].ToString());
                    item.Value = Convert.ToString(row["ConstituencyId"].ToString());
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
        public static SelectList GetConstituencyList(string deptId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Clear();
            //items.Add(new SelectListItem
            //{
            //    Text = "--Select Department--",
            //    Value = "",
            //    Selected = true,
            //});
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetConstituency]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ConstituencyName"].ToString());
                    item.Value = Convert.ToString(row["ConstituencyCode"].ToString());
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

        //-------------Document View/Upload
        public static List<PriorityProject> GetDeptDocumentList(string DeptId)
        {
            List<PriorityProject> list = new List<PriorityProject>();
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
                    PriorityProject item = new PriorityProject
                    {
                        ProgrammeDocsId = Convert.ToInt32(row["programmeDocsId"].ToString()),
                        DocumentTitle = Convert.ToString(row["documentTitle"].ToString()),
                        DocumentTitleLocal = Convert.ToString(row["DocumentTitleLocal"].ToString()),
                        DocumentLocation = Convert.ToString(row["documentLocation"].ToString()),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString()),
                        isActive = Convert.ToBoolean(row["isActive"]),
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
        //Works list for dashboard
        public static List<CMNirdesh.Models.PriorityProject> getProjectsListForDashboard(string currDeptId, int officeid, int projectId, string deptId, int constituencyId, int? statusId)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@promiseid", projectId);
                param[1] = new SqlParameter("@deptId", deptId);
                param[2] = new SqlParameter("@constituencyid", constituencyId);
                param[3] = new SqlParameter("@status", statusId);
                param[4] = new SqlParameter("@currDeptId", currDeptId);
                param[5] = new SqlParameter("@officeid", officeid);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getprojectsfordashboard]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.PromiseId = Convert.ToInt64(row["PromiseId"].ToString());
                    item.PromiseRefId = row["PromiseRefId"].ToString();
                    item.PromiseName = Convert.ToString(row["PromiseName"].ToString());
                    if (row["ProjectId"] != System.DBNull.Value)
                    {
                        item.ProjectId = Convert.ToInt64(row["ProjectId"]);
                    }
                    item.SStartDate = row["StartDate"].ToString();
                    item.SEndDate = row["EndDate"].ToString();
                    if (row["CategoryId"] != System.DBNull.Value)
                    {
                        item.CategoryId = Convert.ToInt32(row["CategoryId"].ToString());
                    }
                    if (row["ProjectStatusId"] != System.DBNull.Value)
                    {
                        item.ProjectStatusId = Convert.ToInt32(row["ProjectStatusId"].ToString());
                    }
                    item.CategoryName = Convert.ToString(row["CategoryName"].ToString());
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["DeptName"].ToString());
                    item.EstimatedAmount = Convert.ToString(row["EstimatedAmount"].ToString());
                    item.Expenditure = Convert.ToString(row["Expenditure"].ToString());
                    item.FirmName = Convert.ToString(row["FirmName"].ToString());
                    item.FundReleased = Convert.ToString(row["FundReleased"].ToString());
                    item.PhysicalProgress = Convert.ToString(row["PhysicalProgress"].ToString());
                    item.ProjectName = Convert.ToString(row["ProjectName"].ToString());
                    item.Remark = Convert.ToString(row["Remark"].ToString());
                    if (row["modifieddate"] != System.DBNull.Value)
                    {
                        item.ProjectStatusName = Convert.ToString(row["ProjectStatusName"].ToString()) + "<br>" + Convert.ToString(row["modifieddate"].ToString()) + "<br>" + Convert.ToString(row["modifiedtime"].ToString());
                    }
                    else
                    {
                        item.ProjectStatusName = Convert.ToString(row["ProjectStatusName"].ToString());

                    }
                    item.ConstituencyName = Convert.ToString(row["ConstituencyName"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getDeptWiseWorkStatus(string flag)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@currdeptId", CurrentSession.DeptID);
                param[1] = new SqlParameter("@currOfficeId", CurrentSession.OfficeId);
                param[2] = new SqlParameter("@flag", flag);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getdepartmentwiseprojectdata]", param).Tables[0].Rows)
                {
                    PriorityProject item = new PriorityProject();
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["DeptName"].ToString());
                    item.totalProject = Convert.ToInt32(row["totalProjects"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getDeptWiseProjectSummary(int projectId, string deptId, int constituencyId, int statusId, string currdeptId, int currofcId, string flag)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@promiseid", projectId);
                param[1] = new SqlParameter("@deptId", deptId);
                param[2] = new SqlParameter("@currdeptId", currdeptId);
                param[3] = new SqlParameter("@currofcId", currofcId);
                param[4] = new SqlParameter("@userId", CurrentSession.UserID);
                param[5] = new SqlParameter("@flag", flag);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getDeptWiseProjectCount]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.OfficeId = Convert.ToString(row["OfficeId"].ToString());
                    item.DeptName = Convert.ToString(row["DeptName"].ToString());
                    item.totalProject = Convert.ToInt32(row["cnt"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getProjectWiseDeptSummary(int projectId, string deptId, int constituencyId, int statusId, string currdeptid, string flag)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@promiseid", projectId);
                param[1] = new SqlParameter("@deptId", deptId);
                param[2] = new SqlParameter("@constituencyid", constituencyId);
                param[3] = new SqlParameter("@status", statusId);
                param[4] = new SqlParameter("@currdeptid", currdeptid);
                param[5] = new SqlParameter("@OfficeId", CurrentSession.OfficeId);
                param[6] = new SqlParameter("@isapex", CurrentSession.isApex);
                param[7] = new SqlParameter("@flag", flag);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getProjectWiseDeptCount]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.PromiseId = Convert.ToInt32(row["promiseid"].ToString());
                    item.PromiseRefId = Convert.ToString(row["PromiseRefId"].ToString());
                    item.DeptId = Convert.ToString(row["deptid"].ToString());
                    item.PromiseName = Convert.ToString(row["PromiseName"].ToString());
                    item.totalDepartment = Convert.ToInt32(row["NoofDept"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getDeptWorkStatusWiseSummary(int projectId, string deptId, int constituencyId, int statusId, string currdeptId)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@promiseid", projectId);
                param[1] = new SqlParameter("@deptId", deptId);
                param[2] = new SqlParameter("@constituencyid", constituencyId);
                param[3] = new SqlParameter("@status", statusId);
                param[4] = new SqlParameter("@currDeptId", currdeptId);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getdeptworkstatus]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["officename"].ToString());
                    item.totalProject = Convert.ToInt32(row["work"].ToString());
                    item.ProjectStatusName = row["status"].ToString();
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getDeptProjectStatusWiseSummary(int projectId, string deptId, int constituencyId, int statusId, string currdeptId)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@promiseid", projectId);
                param[1] = new SqlParameter("@deptId", deptId);
                param[2] = new SqlParameter("@constituencyid", constituencyId);
                param[3] = new SqlParameter("@status", statusId);
                param[4] = new SqlParameter("@currDeptId", currdeptId);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getdeptprojectstatus]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["officename"].ToString());
                    item.totalProject = Convert.ToInt32(row["project"].ToString());
                    item.ProjectStatusName = row["status"].ToString();
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getProjectWiseWorkStatus(string deptId, string officeId, int projectId, string sdeptId, int constituencyId, int statusId)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@deptId", deptId);
                param[1] = new SqlParameter("@officeId", officeId);
                param[2] = new SqlParameter("@promiseid", projectId);
                param[3] = new SqlParameter("@sdeptId", sdeptId);
                param[4] = new SqlParameter("@constituencyid", constituencyId);
                param[5] = new SqlParameter("@status", statusId);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getprojectwiseworksummary]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.PromiseId = Convert.ToInt32(row["promiseid"].ToString());
                    item.PromiseRefId = Convert.ToString(row["promiserefid"].ToString());
                    item.PromiseName = Convert.ToString(row["promisename"].ToString());
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["department"].ToString());
                    item.OfficeName = Convert.ToString(row["officename"].ToString());
                    item.totalWorks = Convert.ToInt32(row["totalWorks"].ToString());
                    item.implementedProject = Convert.ToInt32(row["implemented"].ToString());
                    item.PartiallyImplemented = Convert.ToInt32(row["PartiallyImplemented"].ToString());
                    item.NotImplemented = Convert.ToInt32(row["NotImplemented"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getProjectDeptWiseWorkStatus(int projectId, string deptId, int constituencyId, int statusId)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                string flag = "Dept";
                if (CurrentSession.serviceName == "MC")
                {
                    flag = "office";
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@promiseid", projectId);
                param[1] = new SqlParameter("@deptId", deptId);
                param[2] = new SqlParameter("@constituencyid", constituencyId);
                param[3] = new SqlParameter("@status", statusId);
                param[4] = new SqlParameter("@currOfficeId", CurrentSession.OfficeId);
                param[5] = new SqlParameter("@currdeptId", CurrentSession.DeptID);
                param[6] = new SqlParameter("@userId", CurrentSession.UserID);
                param[7] = new SqlParameter("@flag", flag);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getprojectdeptworksummary]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.PromiseId = Convert.ToInt32(row["promiseid"].ToString());
                    item.PromiseRefId = Convert.ToString(row["promiserefid"].ToString());
                    item.PromiseName = Convert.ToString(row["promisename"].ToString());
                    item.totalDepartment = Convert.ToInt32(row["totalDepartment"].ToString());
                    item.totalWorks = Convert.ToInt32(row["totalWorks"].ToString());
                    item.implementedProject = Convert.ToInt32(row["implemented"].ToString());
                    item.PartiallyImplemented = Convert.ToInt32(row["PartiallyImplemented"].ToString());
                    item.NotImplemented = Convert.ToInt32(row["NotImplemented"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getProjectWiseDeptWorkStatus(int projectID)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@PromiseId", projectID);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getdeptwiseworksummary]", param).Tables[0].Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.PromiseName = Convert.ToString(row["promisename"].ToString());
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["officename"].ToString());
                    item.totalWorks = Convert.ToInt32(row["totalWorks"].ToString());
                    item.PromiseId = Convert.ToInt32(row["promiseid"].ToString());
                    item.implementedProject = Convert.ToInt32(row["implemented"].ToString());
                    item.PartiallyImplemented = Convert.ToInt32(row["PartiallyImplemented"].ToString());
                    item.NotImplemented = Convert.ToInt32(row["NotImplemented"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getDeptWiseProjectWorkStatus(int promiseid, int spromiseid, string deptId, int constituencyId, int statusId)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@promiseid", promiseid);
                param[1] = new SqlParameter("@deptId", deptId);
                param[2] = new SqlParameter("@constituencyId", constituencyId);
                param[3] = new SqlParameter("@statusId", statusId);
                param[4] = new SqlParameter("@userId", CurrentSession.UserID);
                param[5] = new SqlParameter("@apex", CurrentSession.isApex);
                param[6] = new SqlParameter("@spromiseid", spromiseid);
                DataSet dataSet = SqlHelper.ExecuteDataset(connection, "[getDeptwiseprojects]", param);
                DataTable dataTable = dataSet.Tables[0];
                DataTable dataTable2 = dataSet.Tables[1];
                foreach (DataRow row in dataTable.Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["DeptName"].ToString());
                    item.totalProject = Convert.ToInt32(row["totalProjects"].ToString());
                    item.totalWorks = Convert.ToInt32(row["totalWorks"].ToString());
                    item.implementedProject = Convert.ToInt32(row["implemented"].ToString());
                    item.PartiallyImplemented = Convert.ToInt32(row["PartiallyImplemented"].ToString());
                    item.NotImplemented = Convert.ToInt32(row["NotImplemented"].ToString());
                    if (dataTable2.Rows.Count > 0)
                    {
                        item.PromiseName = dataTable2.Rows[0]["promisename"].ToString();
                    }
                    items.Add(item);
                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getdeptofcprojectdata(int promiseId, string deptId, string flag, int spromiseId, string sdeptId, int constituencyId, int statusId)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@promiseId", promiseId);
                param[1] = new SqlParameter("@deptId", deptId);
                param[2] = new SqlParameter("@flag", flag);
                param[3] = new SqlParameter("@spromiseId", spromiseId);
                param[4] = new SqlParameter("@sdeptId", sdeptId);
                param[5] = new SqlParameter("@constituencyId", constituencyId);
                param[6] = new SqlParameter("@statusId", statusId);
                DataSet dataSet = SqlHelper.ExecuteDataset(connection, "[getdeptofcprojectdata]", param);
                DataTable dataTable = dataSet.Tables[0];
                DataTable dataTable2 = dataSet.Tables[1];
                foreach (DataRow row in dataTable.Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["DeptName"].ToString());
                    item.OfficeId = Convert.ToString(row["OfficeId"].ToString());
                    item.OfficeName = Convert.ToString(row["officename"].ToString());
                    item.implementedProject = Convert.ToInt32(row["implemented"].ToString());
                    item.PartiallyImplemented = Convert.ToInt32(row["PartiallyImplemented"].ToString());
                    item.NotImplemented = Convert.ToInt32(row["NotImplemented"].ToString());
                    item.totalProject = Convert.ToInt32(row["totalProjects"].ToString());
                    item.totalWorks = Convert.ToInt32(row["totalWorks"].ToString());
                    if (dataTable2.Rows.Count > 0)
                    {
                        item.PromiseName = dataTable2.Rows[0]["promisename"].ToString();
                    }
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getConstituencyWiseProjectCount(string currdeptId, int currofcId, int isapex)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@currdeptId", currdeptId);
                param[1] = new SqlParameter("@currofcId", currofcId);
                param[2] = new SqlParameter("@isapex", isapex);
                DataSet dataSet = SqlHelper.ExecuteDataset(connection, "[getConstituencyWiseProjectCount]", param);
                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow row in dataTable.Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.ConstituencyId = Convert.ToInt32(row["ConstituencyId"].ToString());
                    item.ConstituencyName = Convert.ToString(row["constituencyname"].ToString());
                    item.totalProject = Convert.ToInt32(row["cnt"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getAgendaDetailByResolutionNo(int ResolutionNo)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@resolutioNo", ResolutionNo);
                DataSet dataSet = SqlHelper.ExecuteDataset(connection, "[GetAgendaDetailsByReslnNo]", param);
                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow row in dataTable.Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.ProjectName = Convert.ToString(row["Subject"].ToString());
                    item.Remark = Convert.ToString(row["SubjectDetails"].ToString());
                    item.RefId = Convert.ToString(row["RefId"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getStatusWiseProjectCount(string currdeptId, int currofcId, int isapex)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@currdeptId", currdeptId);
                param[1] = new SqlParameter("@currofcId", currofcId);
                param[2] = new SqlParameter("@isapex", isapex);
                DataSet dataSet = SqlHelper.ExecuteDataset(connection, "[getStatusWiseProjectCount]", param);
                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow row in dataTable.Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.ProjectStatusId = Convert.ToInt32(row["StatusId"].ToString());
                    item.ProjectStatusName = Convert.ToString(row["ProjectStatusName"].ToString());
                    item.totalProject = Convert.ToInt32(row["cnt"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getProjectWiseworkCount(string currdeptId, int currofcId, int isapex)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@currdeptId", currdeptId);
                param[1] = new SqlParameter("@currofcId", currofcId);
                param[2] = new SqlParameter("@isapex", isapex);
                DataSet dataSet = SqlHelper.ExecuteDataset(connection, "[getProjectWiseworkCount]", param);
                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow row in dataTable.Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.PromiseId = Convert.ToInt32(row["PromiseId"].ToString());
                    item.PromiseName = Convert.ToString(row["PromiseName"].ToString());
                    item.totalWorks = Convert.ToInt32(row["cnt"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
        public static List<CMNirdesh.Models.PriorityProject> getDeptOfficeWiseProjectCount(string currdeptId, int currofcId, int isapex)
        {
            List<CMNirdesh.Models.PriorityProject> items = new List<CMNirdesh.Models.PriorityProject>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@currdeptId", currdeptId);
                param[1] = new SqlParameter("@currofcId", currofcId);
                param[2] = new SqlParameter("@isapex", isapex);
                DataSet dataSet = SqlHelper.ExecuteDataset(connection, "[getDeptOfficeWiseProjectCount]", param);
                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow row in dataTable.Rows)
                {
                    CMNirdesh.Models.PriorityProject item = new CMNirdesh.Models.PriorityProject();
                    item.OfficeId = Convert.ToString(row["OfficeId"].ToString());
                    item.DeptId = Convert.ToString(row["DeptId"].ToString());
                    item.DeptName = Convert.ToString(row["officename"].ToString());
                    item.totalProject = Convert.ToInt32(row["cnt"].ToString());
                    items.Add(item);

                }
                return items;
            }
            catch (Exception ex)
            {
                return items;
            }
        }
    }
}