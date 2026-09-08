using CMNirdesh.DTO;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMNirdesh.Models
{

    public class GetProjectData
    {

        public ProjectDList GetProjectName(int IsLocal, string UnitURL)
        {
            try
            {
                ProjectDList projectList = null;
                ProjectD project = new ProjectD();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);

                    var responseTask = client.GetAsync("GetProjectName?IsLocal=" + IsLocal);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ProjectDList>();
                        readTask.Wait();
                        projectList = readTask.Result;
                    }
                    foreach (var s in projectList)
                    {
                        project.ProgramId = s.ProgramId;

                        project.ProgrammeTitle = s.ProgrammeTitle;

                        project.IconImage = s.IconImage;

                    }
                }
                return projectList;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectName");
                throw new Exception(ex.StackTrace.ToString());
            }
        }

        public ProjectDList GetPData(string Id)
        {
            ProjectDList getprojectlist = new ProjectDList();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@IsLocal", "0"), new SqlParameter("@Id", Id) };
                DataSet dsProject = SqlHelper.ExecuteDataset(connection, "[GetProject]", parameterValues);




                // ContactDList getcontactlist = new DTO.ContactDList();
                foreach (DataRow row in dsProject.Tables[0].Rows)
                {
                    List<ImageD> lstImage = new List<ImageD>();
                    List<DocumentD> lstDoc = new List<DocumentD>();
                    ProjectD project = new ProjectD();

                    project.ProgramId = Convert.ToInt32(row["ProgramId"].ToString());
                    project.ProgrammeTitle = Convert.ToString(row["ProgrammeTitle"]);
                    project.ProgrammeName = Convert.ToString(row["ProgrammeName"]);
                    project.ProgrammeDescription = Convert.ToString(row["ProgrammeDescription"]);
                    project.IconImage = Convert.ToString(row["IconImage"]);
                    project.CreatedDate = string.IsNullOrWhiteSpace(Convert.ToString(row["CreatedDate"]))
   ? (DateTime?)null
   : DateTime.Parse(Convert.ToString(row["CreatedDate"]));
                    var datarowImage = dsProject.Tables[1].AsEnumerable().Where(a => a.Field<int>("programmeId") == Convert.ToInt32(row["ProgramId"]));
                    var datarowDoc = dsProject.Tables[2].AsEnumerable().Where(a => a.Field<int>("programmeId") == Convert.ToInt32(row["ProgramId"]));
                    foreach (DataRow dr in datarowImage)
                    {
                        ImageD image = new ImageD();
                        image.CreatedDate = string.IsNullOrWhiteSpace(Convert.ToString(dr.Field<DateTime>("publishDate")))

   ? (DateTime?)null
   : DateTime.Parse(Convert.ToString(dr.Field<DateTime>("publishDate")));
                        image.ImageDescription = Convert.ToString(dr.Field<string>("ImageDescription"));
                        image.ImageLocation = Convert.ToString(dr.Field<string>("ImageLocation"));
                        image.ImageTitle = Convert.ToString(dr.Field<string>("ImageTitle"));
                        image.ProgrammeImageId = dr.Field<Int64>("ProgrammeImageId");
                        lstImage.Add(image);
                    }

                    foreach (DataRow dr in datarowDoc)
                    {
                        DocumentD doc = new DocumentD();
                        doc.CreatedDate = string.IsNullOrWhiteSpace(Convert.ToString(dr.Field<DateTime>("publishDate")))

   ? (DateTime?)null
   : DateTime.Parse(Convert.ToString(dr.Field<DateTime>("publishDate")));
                        doc.DocumentLocation = Convert.ToString(dr.Field<string>("documentLocation"));
                        doc.DocumentTitle = Convert.ToString(dr.Field<string>("DocumentTitle"));
                        doc.ProgrammeDocsId = dr.Field<Int64>("ProgrammeDocsId");
                        lstDoc.Add(doc);
                    }
                    project.DocumentList = lstDoc;
                    project.ImageList = lstImage;





                    //members.MemberID = Convert.ToInt32(row["PermanentAddress"]);
                    getprojectlist.Add(project);
                }

            }
            catch (Exception ex)
            {
                //ErrorLog.WriteToLog(ex, " MethodName:GetProjectData");
                //throw new Exception(ex.StackTrace.ToString());
            }
            return getprojectlist;
        }

        public PromiseList GetPromiseData(string Id)
        {
            PromiseList promiseList = new PromiseList();
            bool bind = false;
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@PromiseId", Id) };
                DataSet dsProject = SqlHelper.ExecuteDataset(connection, "[getreportpromisesdata]", parameterValues);

                int i = 0;


                // ContactDList getcontactlist = new DTO.ContactDList();
                foreach (DataRow row in dsProject.Tables[1].Rows)
                {
                    Promise promise = new Promise();
                    if (i == 0)
                    {

                       promise.mDepartmentList = getDepartment(dsProject.Tables[0]);


                    }

                    promise.PromiseId = Convert.ToInt32(row["PromiseId"].ToString());
                    promise.PromiseName = Convert.ToString(row["PromiseName"]);

                    //var PromiseData = dsProject.Tables[0].AsEnumerable().Where(a => a.Field<Int64>("PromiseId") == Convert.ToInt64(row["PromiseId"]));
                    List<PriorityProject> lstPrzt = new List<PriorityProject>();
                    if (i == 0)
                    {



                        foreach (DataRow dr in dsProject.Tables[0].Rows)
                        {

                            PriorityProject przt = new PriorityProject();

                            przt.CategoryName = Convert.ToString(dr.Field<string>("CategoryName"));
                            przt.DeptId = Convert.ToString(dr.Field<string>("DeptId"));
                            przt.DeptName = Convert.ToString(dr.Field<string>("DeptName"));
                            przt.EndDate = Convert.ToDateTime(dr.Field<string>("EndDate"));
                            przt.EstimatedAmount = Convert.ToString(dr.Field<decimal>("EstimatedAmount"));
                            przt.Expenditure = Convert.ToString(dr.Field<decimal>("Expenditure"));
                            przt.FirmName = Convert.ToString(dr.Field<string>("FirmName"));
                            przt.FundReleased = Convert.ToString(dr.Field<decimal>("FundReleased"));
                            przt.PhysicalProgress = Convert.ToString(dr.Field<decimal>("PhysicalProgress"));
                            przt.ProjectId = Convert.ToInt32(dr.Field<Int64>("ProjectId"));
                            przt.ProjectName = Convert.ToString(dr.Field<string>("ProjectName"));
                            przt.ProjectStatusName = Convert.ToString(dr.Field<string>("ProjectStatusName"));
                            przt.Remark = Convert.ToString(dr.Field<string>("Remark"));
                            przt.StartDate = Convert.ToDateTime(dr.Field<string>("StartDate"));
                            przt.PromiseName = Convert.ToString(dr.Field<string>("PromiseName"));
                            lstPrzt.Add(przt);

                        }
                    }
                    promise.lstPriorityProject = lstPrzt;

                    promiseList.Add(promise);
                    i++;
                }



            }
            catch (Exception ex)
            {
                //ErrorLog.WriteToLog(ex, " MethodName:GetProjectData");
                //throw new Exception(ex.StackTrace.ToString());
            }
            return promiseList;

        }

        public static PriorityProjectList GetPrjectDataByFilter(string DeptId, Int64 PromiseId)
        {
            PriorityProjectList priorityList = new PriorityProjectList();
            bool bind = false;
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@PromiseId", PromiseId), new SqlParameter("@DeptId", DeptId) }

                    ;
                DataSet dsProject = SqlHelper.ExecuteDataset(connection, "[getProjectByfilter]", parameterValues);




                // ContactDList getcontactlist = new DTO.ContactDList();
                foreach (DataRow dr in dsProject.Tables[0].Rows)
                {


                    PriorityProject przt = new PriorityProject();

                    przt.CategoryName = Convert.ToString(dr.Field<string>("CategoryName"));
                    przt.DeptId = Convert.ToString(dr.Field<string>("DeptId"));
                    przt.DeptName = Convert.ToString(dr.Field<string>("DeptName"));
                    przt.EndDate = Convert.ToDateTime(dr.Field<string>("EndDate"));
                    przt.EstimatedAmount = Convert.ToString(dr.Field<decimal>("EstimatedAmount"));
                    przt.Expenditure = Convert.ToString(dr.Field<decimal>("Expenditure"));
                    przt.FirmName = Convert.ToString(dr.Field<string>("FirmName"));
                    przt.FundReleased = Convert.ToString(dr.Field<decimal>("FundReleased"));
                    przt.PhysicalProgress = Convert.ToString(dr.Field<decimal>("PhysicalProgress"));
                    przt.ProjectId = Convert.ToInt32(dr.Field<Int64>("ProjectId"));
                    przt.ProjectName = Convert.ToString(dr.Field<string>("ProjectName"));
                    przt.ProjectStatusName = Convert.ToString(dr.Field<string>("ProjectStatusName"));
                    przt.Remark = Convert.ToString(dr.Field<string>("Remark"));
                    przt.StartDate = Convert.ToDateTime(dr.Field<string>("StartDate"));
                    priorityList.Add(przt);
                }
                return priorityList;
            }
            catch (Exception ex)
            {

            }
            return priorityList;

        }
        public static SelectList getDepartment(DataTable dt)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable distinctDT = (from rows in dt.AsEnumerable()
                                            group rows by new { deptname = rows["deptname"], deptId = rows["deptId"] } into grp
                                            select grp.First()).CopyToDataTable();
                    foreach (DataRow row in distinctDT.Rows)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = Convert.ToString(row["deptname"].ToString());
                        item.Value = Convert.ToString(row["deptid"].ToString());
                        items.Add(item);
                    }
                    return new SelectList(items, "Value", "Text");
                }
                else
                {
                    return new SelectList(items, "Value", "Text");
                }
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public PromiseDocument getDocImage(Int64 ProjectId, string ProjectName, string Remark)
        {
            PromiseDocument Pdoc = new PromiseDocument();
            Pdoc.ProjectName = ProjectName;
            Pdoc.ProjectDescription = Remark;
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@ProjectId", ProjectId) };
                DataSet dsProject = SqlHelper.ExecuteDataset(connection, "GetProjectDocImage", parameterValues);
                List<ImageD> lstImage = new List<ImageD>();
                List<DocumentD> lstDoc = new List<DocumentD>();
                // get Document;
                foreach (DataRow dr in dsProject.Tables[0].Rows)
                {
                    ImageD image = new ImageD();
                    image.CreatedDate = string.IsNullOrWhiteSpace(Convert.ToString(dr.Field<DateTime>("publishDate")))? (DateTime?)null
                       : DateTime.Parse(Convert.ToString(dr.Field<DateTime>("publishDate")));
                    image.ImageDescription = Convert.ToString(dr.Field<string>("ImageDescription"));
                    image.ImageLocation = Convert.ToString(dr.Field<string>("ImageLocation"));
                    image.ImageTitle = Convert.ToString(dr.Field<string>("ImageTitle"));
                    image.ProgrammeImageId = dr.Field<Int64>("ProgrammeImageId");
                    lstImage.Add(image);
                }

                foreach (DataRow dr in dsProject.Tables[1].Rows)
                {
                    DocumentD doc = new DocumentD();
                    doc.CreatedDate = string.IsNullOrWhiteSpace(Convert.ToString(dr.Field<DateTime>("publishDate")))

? (DateTime?)null
: DateTime.Parse(Convert.ToString(dr.Field<DateTime>("publishDate")));
                    doc.DocumentLocation = Convert.ToString(dr.Field<string>("documentLocation"));
                    doc.DocumentTitle = Convert.ToString(dr.Field<string>("DocumentTitle"));
                    doc.ProgrammeDocsId = dr.Field<Int64>("ProgrammeDocsId");
                    lstDoc.Add(doc);
                }
                Pdoc.DocumentList = lstDoc;
                Pdoc.ImageList = lstImage;



            }
            catch (Exception ex)
            {
                //ErrorLog.WriteToLog(ex, " MethodName:GetProjectData");
                //throw new Exception(ex.StackTrace.ToString());
            }
            return Pdoc;

        }


        public ProjectList GetAllProject(int IsLocal, string UnitURL)
        {
            try
            {
                ProjectList projectList = new ProjectList();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);

                    var responseTask = client.GetAsync("GetAllProject?IsLocal=" + IsLocal);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ProjectList>();
                        readTask.Wait();
                        projectList = readTask.Result;
                    }

                }
                return projectList;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetAllProject");
                throw new Exception(ex.StackTrace.ToString());
            }
        }
        public ProjectDList GetDepartmentList(int IsLocal, string UnitURL, string Id)
        {
            ProjectDList list2;
            try
            {
                ProjectDList list = null;
                ProjectD td = new ProjectD();
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);
                    Task<HttpResponseMessage> async = client.GetAsync("GetDepartmentDetails?IsLocal=" + IsLocal.ToString() + "&Id=" + Id);
                    async.Wait();
                    HttpResponseMessage result = async.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Task<ProjectDList> task2 = result.Content.ReadAsAsync<ProjectDList>();
                        task2.Wait();
                        list = task2.Result;
                    }
                    foreach (ProjectD td2 in list)
                    {
                        td.ProgramId = td2.ProgramId;
                        td.ImageList = td2.ImageList;
                        td.ProgrammeDescription = td2.ProgrammeDescription;
                        td.ProgrammeName = td2.ProgrammeName;
                        td.ProgrammeTitle = td2.ProgrammeTitle;
                        td.DocumentList = td2.DocumentList;
                        td.CreatedDate = td2.CreatedDate;
                        td.IconImage = td2.IconImage;
                    }
                }
                list2 = list;
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectList");
                throw new Exception(ex.StackTrace.ToString());
            }
            return list2;
        }

        public ProjectDList GetRTIList(int IsLocal, string UnitURL, string Id)
        {
            ProjectDList list2;
            try
            {
                ProjectDList list = null;
                ProjectD td = new ProjectD();
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);
                    Task<HttpResponseMessage> async = client.GetAsync("GetRTIDetails?IsLocal=" + IsLocal.ToString() + "&Id=" + Id);
                    async.Wait();
                    HttpResponseMessage result = async.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Task<ProjectDList> task2 = result.Content.ReadAsAsync<ProjectDList>();
                        task2.Wait();
                        list = task2.Result;
                    }
                    foreach (ProjectD td2 in list)
                    {
                        td.ProgramId = td2.ProgramId;
                        td.ImageList = td2.ImageList;
                        td.ProgrammeDescription = td2.ProgrammeDescription;
                        td.ProgrammeName = td2.ProgrammeName;
                        td.ProgrammeTitle = td2.ProgrammeTitle;
                        td.DocumentList = td2.DocumentList;
                        td.CreatedDate = td2.CreatedDate;
                        td.IconImage = td2.IconImage;
                    }
                }
                list2 = list;
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectList");
                throw new Exception(ex.StackTrace.ToString());
            }
            return list2;
        }




        public ProjectDList getCitizencharter(int IsLocal, string UnitURL, string Id)
        {
            ProjectDList list2;
            try
            {
                ProjectDList list = null;
                ProjectD td = new ProjectD();
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);
                    Task<HttpResponseMessage> async = client.GetAsync("getCitizencharter?IsLocal=" + IsLocal.ToString() + "&Id=" + Id);
                    async.Wait();
                    HttpResponseMessage result = async.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Task<ProjectDList> task2 = result.Content.ReadAsAsync<ProjectDList>();
                        task2.Wait();
                        list = task2.Result;
                    }
                    foreach (ProjectD td2 in list)
                    {
                        td.ProgramId = td2.ProgramId;
                        td.ImageList = td2.ImageList;
                        td.ProgrammeDescription = td2.ProgrammeDescription;
                        td.ProgrammeName = td2.ProgrammeName;
                        td.ProgrammeTitle = td2.ProgrammeTitle;
                        td.DocumentList = td2.DocumentList;
                        td.CreatedDate = td2.CreatedDate;
                        td.IconImage = td2.IconImage;
                    }
                }
                list2 = list;
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectList");
                throw new Exception(ex.StackTrace.ToString());
            }
            return list2;
        }
        public ProjectDList getActsRules(int IsLocal, string UnitURL, string Id)
        {
            ProjectDList list2;
            try
            {
                ProjectDList list = null;
                ProjectD td = new ProjectD();
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);
                    Task<HttpResponseMessage> async = client.GetAsync("getActsRules?IsLocal=" + IsLocal.ToString() + "&Id=" + Id);
                    async.Wait();
                    HttpResponseMessage result = async.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Task<ProjectDList> task2 = result.Content.ReadAsAsync<ProjectDList>();
                        task2.Wait();
                        list = task2.Result;
                    }
                    foreach (ProjectD td2 in list)
                    {
                        td.ProgramId = td2.ProgramId;
                        td.ImageList = td2.ImageList;
                        td.ProgrammeDescription = td2.ProgrammeDescription;
                        td.ProgrammeName = td2.ProgrammeName;
                        td.ProgrammeTitle = td2.ProgrammeTitle;
                        td.DocumentList = td2.DocumentList;
                        td.CreatedDate = td2.CreatedDate;
                        td.IconImage = td2.IconImage;
                    }
                }
                list2 = list;
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectList");
                throw new Exception(ex.StackTrace.ToString());
            }
            return list2;
        }


        public ProjectDList getCommittee(int IsLocal, string UnitURL, string Id)
        {
            ProjectDList list2;
            try
            {
                ProjectDList list = null;
                ProjectD td = new ProjectD();
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);
                    Task<HttpResponseMessage> async = client.GetAsync("getCommittee?IsLocal=" + IsLocal.ToString() + "&Id=" + Id);
                    async.Wait();
                    HttpResponseMessage result = async.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Task<ProjectDList> task2 = result.Content.ReadAsAsync<ProjectDList>();
                        task2.Wait();
                        list = task2.Result;
                    }
                    foreach (ProjectD td2 in list)
                    {
                        td.ProgramId = td2.ProgramId;
                        td.ImageList = td2.ImageList;
                        td.ProgrammeDescription = td2.ProgrammeDescription;
                        td.ProgrammeName = td2.ProgrammeName;
                        td.ProgrammeTitle = td2.ProgrammeTitle;
                        td.DocumentList = td2.DocumentList;
                        td.CreatedDate = td2.CreatedDate;
                        td.IconImage = td2.IconImage;
                    }
                }
                list2 = list;
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectList");
                throw new Exception(ex.StackTrace.ToString());
            }
            return list2;
        }


        public ProjectDList getTender(int IsLocal, string UnitURL, string Id)
        {
            ProjectDList list2;
            try
            {
                ProjectDList list = null;
                ProjectD td = new ProjectD();
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);
                    Task<HttpResponseMessage> async = client.GetAsync("getTender?IsLocal=" + IsLocal.ToString() + "&Id=" + Id);
                    async.Wait();
                    HttpResponseMessage result = async.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Task<ProjectDList> task2 = result.Content.ReadAsAsync<ProjectDList>();
                        task2.Wait();
                        list = task2.Result;
                    }
                    foreach (ProjectD td2 in list)
                    {
                        td.ProgramId = td2.ProgramId;
                        td.ImageList = td2.ImageList;
                        td.ProgrammeDescription = td2.ProgrammeDescription;
                        td.ProgrammeName = td2.ProgrammeName;
                        td.ProgrammeTitle = td2.ProgrammeTitle;
                        td.DocumentList = td2.DocumentList;
                        td.CreatedDate = td2.CreatedDate;
                        td.IconImage = td2.IconImage;
                        td.mDepartmentDetailsOrder = td2.mDepartmentDetailsOrder;
                    }
                }
                list2 = list;
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectList");
                throw new Exception(ex.StackTrace.ToString());
            }
            return list2;
        }

        public ProjectDList getDisaster(int IsLocal, string UnitURL, string Id)
        {
            ProjectDList list2;
            try
            {
                ProjectDList list = null;
                ProjectD td = new ProjectD();
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);
                    Task<HttpResponseMessage> async = client.GetAsync("getDisaster?IsLocal=" + IsLocal.ToString() + "&Id=" + Id);
                    async.Wait();
                    HttpResponseMessage result = async.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Task<ProjectDList> task2 = result.Content.ReadAsAsync<ProjectDList>();
                        task2.Wait();
                        list = task2.Result;
                    }
                    foreach (ProjectD td2 in list)
                    {
                        td.ProgramId = td2.ProgramId;
                        td.ImageList = td2.ImageList;
                        td.ProgrammeDescription = td2.ProgrammeDescription;
                        td.ProgrammeName = td2.ProgrammeName;
                        td.ProgrammeTitle = td2.ProgrammeTitle;
                        td.DocumentList = td2.DocumentList;
                        td.CreatedDate = td2.CreatedDate;
                        td.IconImage = td2.IconImage;
                        td.mDepartmentDetailsOrder = td2.mDepartmentDetailsOrder;
                    }
                }
                list2 = list;
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectList");
                throw new Exception(ex.StackTrace.ToString());
            }
            return list2;
        }

        public ProjectDList getArchTender(int IsLocal, string UnitURL, string Id)
        {
            ProjectDList list2;
            try
            {
                ProjectDList list = null;
                ProjectD td = new ProjectD();
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);
                    Task<HttpResponseMessage> async = client.GetAsync("getArchTender?IsLocal=" + IsLocal.ToString() + "&Id=" + Id);
                    async.Wait();
                    HttpResponseMessage result = async.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        Task<ProjectDList> task2 = result.Content.ReadAsAsync<ProjectDList>();
                        task2.Wait();
                        list = task2.Result;
                    }
                    foreach (ProjectD td2 in list)
                    {
                        td.ProgramId = td2.ProgramId;
                        td.ImageList = td2.ImageList;
                        td.ProgrammeDescription = td2.ProgrammeDescription;
                        td.ProgrammeName = td2.ProgrammeName;
                        td.ProgrammeTitle = td2.ProgrammeTitle;
                        td.DocumentList = td2.DocumentList;
                        td.CreatedDate = td2.CreatedDate;
                        td.IconImage = td2.IconImage;
                        td.mDepartmentDetailsOrder = td2.mDepartmentDetailsOrder;
                    }
                }
                list2 = list;
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectList");
                throw new Exception(ex.StackTrace.ToString());
            }
            return list2;
        }

        public ProjectDList GetAward(int IsLocal, string UnitURL, string Id)
        {
            try
            {
                ProjectDList projectList = null;
                ProjectD project = new ProjectD();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UnitURL);

                    var responseTask = client.GetAsync("GetAwardDetails?IsLocal=" + IsLocal + "&Id=" + Id);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ProjectDList>();
                        readTask.Wait();
                        projectList = readTask.Result;
                    }
                    foreach (var s in projectList)
                    {
                        project.ProgramId = s.ProgramId;
                        project.ImageList = s.ImageList;
                        project.ProgrammeDescription = s.ProgrammeDescription;
                        project.ProgrammeName = s.ProgrammeName;
                        project.ProgrammeTitle = s.ProgrammeTitle;
                        project.DocumentList = s.DocumentList;
                        project.CreatedDate = s.CreatedDate;
                        project.IconImage = s.IconImage;


                    }
                }
                return projectList;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " MethodName:GetProjectList");
                throw new Exception(ex.StackTrace.ToString());
            }
        }


    }
}
