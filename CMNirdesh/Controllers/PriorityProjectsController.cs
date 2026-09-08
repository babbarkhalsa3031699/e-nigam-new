using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ErrorLog = CMNirdesh.Error.ErrorLog;

namespace CMNirdesh.Controllers
{
    public class PriorityProjectsController : Controller
    {
        public ActionResult Index(string Id)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            string res = "";
            string[] split = null;
            if (string.IsNullOrEmpty(Id))
            {
                res = Id;

            }
            else
            {
                split = Id.Split('-');
            }
            CurrentSession.workid = "0";
            PriorityProject model = new PriorityProject();
            if (CurrentSession.serviceName == "MC")
            {
                model.mOfficeList = PriorityProject.GetOfficebyDepartmentId(CurrentSession.DeptID);
            }
            else
            {
                model.mOfficeList = DiaryViewModel.getDepartment();
            }
            model.mDepartmentList = DiaryViewModel.getDepartment(); //PriorityProject.getUserDepartment(CurrentSession.UserID);
            model.PromiseTypeList = PriorityProject.GetPromise("All");
            model.ProjectStatusTypeList = PriorityProject.GetProjectStatus();
            model.mProjectConstituencyList = PriorityProject.GetConstituencyList(model.DeptId);
            DataSet ds = new DataSet();
            ds = UserModelFunction.GetPermission(CurrentSession.UserID);
            model.projectUserActions = "";
            model.projectedityn = "N";
            model.deptedityn = "N";
            model.workedityn = "N";
            if (ds.Tables[10].Rows.Count > 0)
            {
                model.projectUserActions = ds.Tables[10].Rows[0][1].ToString();
            }
            if (model.projectUserActions != string.Empty && model.projectUserActions != null)
            {
                string[] actions = model.projectUserActions.Split(',');
                foreach (string str in actions)
                {
                    if (str == "1")
                    {
                        model.projectedityn = "Y";
                    }
                    if (str == "2")
                    {
                        model.deptedityn = "Y";
                    }
                    if (str == "3")
                    {
                        model.workedityn = "Y";
                    }
                }
            }

            TempData["apex"] = CurrentSession.isApex;
            TempData["parentYN"] = CurrentSession.parentYN;
            // TempData["useractiontype"] = "";
            model.useractiontype = "All";
            model.projectServiceName = CurrentSession.serviceName;
            model.isapex = CurrentSession.isApex;
            return View(model);
        }
        public ActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            PriorityProject model = new PriorityProject();
            if (CurrentSession.serviceName == "MC")
            {
                model.mDepartmentList = PriorityProject.GetOfficebyDepartmentId(CurrentSession.DeptID);
            }
            else
            {
                model.mDepartmentList = DiaryViewModel.getDepartment();
            }
            model.PromiseTypeList = PriorityProject.GetPromise("All");
            model.ProjectStatusTypeList = PriorityProject.GetProjectStatus();
            model.mProjectConstituencyList = PriorityProject.GetConstituencyList(model.DeptId);
            model.useractiontype = "All";
            model.projectServiceName = CurrentSession.serviceName;
            model.isapex = CurrentSession.isApex;
            model.workedityn = "N";
            return View(model);
        }
        public ActionResult Createprojectanddepartment()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            CurrentSession.workid = "0";
            PriorityProject model = new PriorityProject();
            if (CurrentSession.serviceName == "MC" && CurrentSession.isApex == "0")
            {
                model.mOfficeList = PriorityProject.GetOfficebyDepartmentId(CurrentSession.DeptID);
                model.mDepartmentList = PriorityProject.GetOfficebyDepartmentId(CurrentSession.DeptID);
            }
            else
            {
                model.mOfficeList = PriorityProject.GetOfficebyDepartmentId(CurrentSession.DeptID);
                model.mDepartmentList = DiaryViewModel.getDepartment();
            }
            model.PromiseTypeList = PriorityProject.GetPromise("All");
            model.ProjectStatusTypeList = PriorityProject.GetProjectStatus();
            model.mProjectConstituencyList = PriorityProject.GetConstituencyList(model.DeptId);
            DataSet ds = new DataSet();
            ds = UserModelFunction.GetPermission(CurrentSession.UserID);
            model.projectUserActions = "";
            model.projectedityn = "N";
            model.deptedityn = "N";
            model.workedityn = "N";
            if (ds.Tables[10].Rows.Count > 0)
            {
                model.projectUserActions = ds.Tables[10].Rows[0][1].ToString();
            }
            if (model.projectUserActions != string.Empty && model.projectUserActions != null)
            {
                string[] actions = model.projectUserActions.Split(',');
                foreach (string str in actions)
                {
                    if (str == "1")
                    {
                        model.projectedityn = "Y";
                    }
                    if (str == "2")
                    {
                        model.deptedityn = "Y";
                    }
                    if (str == "3")
                    {
                        model.workedityn = "Y";
                    }
                }
            }
            TempData["apex"] = CurrentSession.isApex;
            TempData["parentYN"] = CurrentSession.parentYN;
            TempData["useractiontype"] = "Edit";
            model.useractiontype = "Edit";
            model.projectServiceName = CurrentSession.serviceName;
            model.isapex = CurrentSession.isApex;
            return View("Index", model);
        }
        //----------Common method to call all model data------------------------
        public PriorityProject loadData(string promiseid, string deptid, int projectid)
        {
            PriorityProject model = new PriorityProject();
            model.PromiseId = Convert.ToInt64(promiseid);
            model.projectslist = PriorityProject.getDeptProjectsList(promiseid, CurrentSession.DeptID);
            model.PromiseName = "";
            if (model.projectslist.Count > 0)
            {
                model.PromiseName = "(" + model.projectslist[0].PromiseRefId.ToString() + ") " + model.projectslist[0].PromiseName.ToString();
            }
            TempData["PromiseName"] = model.PromiseName;
            model.StatusTypeList = Promise.GetStatusTypes();
            if (CurrentSession.serviceName == "MC")
            {
                model.mDepartmentList = PriorityProject.GetOfficebyDepartmentId(CurrentSession.DeptID);
            }
            else
            {
                model.mDepartmentList = DiaryViewModel.getDepartment();
            }
            model.PromiseTypeList = PriorityProject.GetPromise("All");
            model.mProjectDepartmentList = PriorityProject.GetProjectDepartments(model.PromiseId);
            model.PromiseId = Convert.ToInt64(promiseid);
            model.DeptId = deptid;
            model.ProjectId = projectid;

            foreach (var str in model.mProjectDepartmentList)
            {
                if (deptid == str.Value)
                {
                    model.DeptName = str.Text;
                }
            }

            model.ProjectStatusTypeList = PriorityProject.GetProjectStatus();
            model.worksdatalist = PriorityProject.GetWorkData(promiseid, deptid, projectid);
            model.CategoryTypeList = PriorityProject.GetCategory();
            //model.ResolutionList = PriorityProject.GetResolutionNo();
            model.ResolutionOfcTypeList = PriorityProject.GetResolutionOfcType();
            model.mProjectConstituencyList = PriorityProject.GetConstituencyList(model.DeptId);

            PriorityProject pd = new PriorityProject();
            model.workimageslist = pd.getWorkImage(projectid);
            model.workdocumentslist = pd.getWorkDocuments(projectid);
            model.mWorkConstituencyList = PriorityProject.getWorkConstituency(projectid);

            model.IsImage = true;
            model.IsDocument = true;
            model.IsConstituency = true;
            if (model.workimageslist.Count < 1)
            {
                model.IsImage = false;
            }
            if (model.workdocumentslist.Count < 1)
            {
                model.IsDocument = false;
            }
            if (model.mWorkConstituencyList.Count() < 1)
            {
                model.IsConstituency = false;
            }
            return model;
        }
        #region "Project"
        //-----------------------Project-----------------------------
        public ActionResult AddProject()
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                PriorityProject model = new PriorityProject();
                model.DeptId = CurrentSession.DeptID;
                model = loadData("0", model.DeptId, 0);
                model.mode = "Add";
                model.PromiseName = "";
                return View("Index", model);
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                TempData["Message"] = "Their is a Error While showing Project Details";
                return RedirectToAction("Index");
            }
        }
        public ActionResult ProjectDetails()
        {
            try
            {
                return (!string.IsNullOrEmpty(CurrentSession.UserID) ? ((ActionResult)base.View(Promise.GetPromiseList(CurrentSession.DeptID))) : ((ActionResult)base.RedirectToAction("Login", "Account", new { area = "" })));
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Their is a Error While Getting All Promise";
                return RedirectToAction("Index");
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SaveProject(PriorityProject model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.PromiseName))
                {
                    TempData["ErrMsgPromise"] = "Y";
                    TempData["Message"] = "Enter Project Name";
                    return RedirectToAction("Index");
                }
                model.DeptId = CurrentSession.DeptID;
                int result = 0;
                string mode = model.mode;
                if (model.mode == "Add")
                {
                    model.mode = "Add";
                    model.createdBy = CurrentSession.UserID;
                    model.DeptId = CurrentSession.DeptID;
                    model.isDeleted = false;
                    model.isActive = true;
                    result = PriorityProject.SavePromise(model);
                }
                else
                {
                    //string promiseid = TempData["promiseId"].ToString();
                    //model.PromiseId = Convert.ToInt64(promiseid);
                    model.mode = "Edit";
                    model.isDeleted = false;
                    model.isActive = true;
                    model.createdBy = CurrentSession.UserID;
                    result = PriorityProject.SavePromise(model);
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Their is a Error While Saving Project Details";
                return RedirectToAction("Index");
            }
        }
        public ActionResult EditProjectDetails(string Id)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                string promiseid = EncryptionUtility.Decrypt(Id.ToString());
                model.PromiseId = Convert.ToInt64(promiseid);
                model.DeptId = CurrentSession.DeptID;
                model = loadData(promiseid, model.DeptId, 0);
                model.mode = "Edit";
                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Their is a Error While Creating Project Details";
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region "Departments"
        //---------------------Department Mapping-------------
        public ActionResult SaveDepartment(string deptId, string PromiseId,string officeId)
        {
            try
            {
                PriorityProject model = new PriorityProject();

                model.isActive = true;
                model.createdBy = CurrentSession.UserID;
                model.ModifiedBy = Guid.Parse(CurrentSession.UserID);
                model.DeptId = deptId;
                model.PromiseId = Convert.ToInt64(PromiseId);
                //Save function call
                string result = "";
                if (CurrentSession.serviceName == "MC")
                {
                    result = PriorityProject.SaveDepartmentProjectMapping(model, deptId, Convert.ToInt32(officeId), "S", Convert.ToInt16(CurrentSession.isApex));
                }
                else
                {
                    result = PriorityProject.SaveDepartmentProjectMapping(model, model.DeptId, 0, "S", Convert.ToInt16(CurrentSession.isApex));
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Their is a Error While Saving Department";
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteProjectDepartment(string Id, string PromiseId)
        {
            try
            {
                PriorityProject model = new PriorityProject();

                string[] idArray = Id.Split('-');
                string officeid = "0";
                model.DeptId = Id;
                if (idArray.Length>1) {
                    officeid = idArray[0].ToString();
                    model.DeptId = idArray[1].ToString(); ;
                }
                model.isActive = true;
                model.createdBy = CurrentSession.UserID;
                model.ModifiedBy = Guid.Parse(CurrentSession.UserID);
                model.PromiseId = Convert.ToInt64(PromiseId);
                //Delete function call
                string result = "";
                if (CurrentSession.serviceName == "MC")
                {
                    result = PriorityProject.SaveDepartmentProjectMapping(model, model.DeptId, Convert.ToInt32(officeid), "D", Convert.ToInt16(CurrentSession.isApex));
                }
                else
                {
                    result = PriorityProject.SaveDepartmentProjectMapping(model, model.DeptId, 0, "D", Convert.ToInt16(CurrentSession.isApex));
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Their is a Error While Deleting Department";
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region "Works"
        //-----------------------Priority Project Works-------------------------------                
        public ActionResult SavePriorityProjectWork(PriorityProject model)
        {
            try
            {
                string promiseid = model.PromiseId.ToString();
                string officeid = model.DeptId.ToString();
                model.PromiseId = Convert.ToInt64(promiseid);
                model.ProjectId = model.worksdatalist.ProjectId;
                if (CurrentSession.CurrentAction == "AddWork")
                {
                    model.ProjectId = 0;
                }
                model.createdBy = CurrentSession.UserID;
                model.DeptId = CurrentSession.DeptID;
                model.OfficeId = officeid;
                model.isActive = true;
                string format = "dd/MM/yyyy";

                if (model.worksdatalist.SStartDate != null && model.worksdatalist.SStartDate != "")
                {
                    model.worksdatalist.StartDate = DateTime.ParseExact(model.worksdatalist.SStartDate, format, CultureInfo.InvariantCulture);
                }
                if (model.worksdatalist.SEndDate != null && model.worksdatalist.SEndDate != "")
                {
                    model.worksdatalist.EndDate = DateTime.ParseExact(model.worksdatalist.SEndDate, format, CultureInfo.InvariantCulture);
                }
                int result = 0;
                result = PriorityProject.SaveProjectWork(model);
                CurrentSession.CurrentAction = "";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                TempData["Message"] = "Their is an Error While Saving Work Details";
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region "Documents"
        //----------------------Document-------------------------------
        public ActionResult DocumentList()
        {
            try
            {
                return ((ActionResult)base.View(DocumentViewModel.GetDeptDocumentList(CurrentSession.DeptID)));
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                TempData["Message"] = "Their is a Error While getting  Document Details";
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
        }
        public ActionResult DeleteDocument(string ProjectId, string deptID, string workID, string docId)
        {

            try
            {
                int result = 0;
                result = PriorityProject.DeleteDocumentById(Convert.ToInt32(docId));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                TempData["Message"] = "Their is a Error While Deleting DepartMent Document Deails";
                return RedirectToAction("Index");
            }


        }
        public ActionResult SaveDocument(PriorityProject model)
        {
            var fileAcessingSettings = CMNirdesh.Models.FileSetting.GetDISFileSetting();
            string url = "~/MlaDiary/DepartmentDetails/Doc/";
            string path = Server.MapPath(url);
            try
            {
                int result = 0;
                string promiseid = model.PromiseId.ToString();
                string deptID = model.DeptId.ToString();
                string workid = CurrentSession.workid.ToString();
                model.PromiseId = Convert.ToInt64(promiseid);
                model.DeptId = deptID;
                model.ProjectId = Convert.ToInt64(workid);
                model.isActive = true;
                model.createdBy = CurrentSession.UserID;
                model.ModifiedBy = Guid.Parse(CurrentSession.UserID);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                byte[] bytes1 = null;
                string type = "";
                if (model.DocumentFileName != null)
                {

                    string[] filenamearr = model.DocumentFileName.Split('.');
                    string FileName = filenamearr[0] + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + filenamearr[1];
                    ConvertBase64StringToBytes(model.DocumentLocation, ref bytes1, ref type);
                    System.IO.File.WriteAllBytes(path + "/" + FileName, bytes1);
                    model.DocumentLocation = "DepartmentDetails/Doc/" + FileName;
                    result = PriorityProject.SaveDocumentDepartment(model);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                TempData["Message"] = "Their is a Error While saving the department Document";
                return RedirectToAction("Index");
            }

            // return RedirectToAction("GalleryCategoryIndex");
        }
        #endregion
        #region "Constituencies"
        //-----------------------Priority Project Work Constituency-------------------------------       

        public ActionResult SaveProjectConstituency(string ProjectId, string deptID, string workID, string contituencyId)
        {
            try
            {
                PriorityProject model = new PriorityProject();

                model.PromiseId = Convert.ToInt64(ProjectId);
                model.DeptId = deptID;
                model.ProjectId = Convert.ToInt64(workID);
                model.cdid = contituencyId;
                model.ProjectMappedConstituencyId = 0;
                model.isActive = true;
                model.createdBy = CurrentSession.UserID;
                CurrentSession.workid = model.ProjectId.ToString();
                //-------Save Details-----------//
                int result = 0;
                result = PriorityProject.SaveConstituency(model, "S");

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                //ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                TempData["Message"] = "Their is a Error While Creating Project Details";
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteProjectConstituency(string ProjectId, string deptID, string workID, string contituencyId)
        {
            try
            {
                PriorityProject model = new PriorityProject();

                model.PromiseId = Convert.ToInt64(ProjectId);
                model.DeptId = deptID;
                model.ProjectId = Convert.ToInt64(workID);
                model.cdid = contituencyId;
                model.ProjectMappedConstituencyId = 0;
                model.isActive = true;
                model.createdBy = CurrentSession.UserID;
                CurrentSession.workid = model.ProjectId.ToString();
                //-------Save Details-----------//
                int result = 0;
                result = PriorityProject.SaveConstituency(model, "D");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                //ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                TempData["Message"] = "Their is a Error While Creating Project Details";
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region "Images"
        //---------------Images---------------------------
        public ActionResult DeleteImage(string imageId)
        {

            try
            {
                PriorityProject model = new PriorityProject();
                //Delete image from directory
                model.workimagedetails = PriorityProject.GetDeptImageListById(Convert.ToInt32(imageId));
                string path = Server.MapPath("/MlaDiary/DepartmentDetails/Image/");
                string FileName = model.workimagedetails.ImageLocation;
                string[] filenamearr = FileName.Split('/');
                FileName = filenamearr[2].ToString();
                FileName = Path.Combine(path, FileName);
                FileInfo file = new FileInfo(FileName);
                if (file.Exists)//check file exist or not  
                {
                    file.Delete();
                }
                string thumbFileName = model.workimagedetails.ImageThumbLocation;
                filenamearr = thumbFileName.Split('/');
                thumbFileName = filenamearr[2].ToString();
                thumbFileName = Path.Combine(path, thumbFileName);
                file = new FileInfo(thumbFileName);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
                //// code ends
                int result = 0;
                result = PriorityProject.DeleteDeptImageById(Convert.ToInt32(imageId));

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                TempData["Message"] = "Their is a Error While Deleting DepartMent Image Deails";
                return RedirectToAction("Index");
            }
        }
        public ActionResult SaveImage(PriorityProject model)
        {
            //string path = $@"\\{ConfigurationManager.AppSettings["filePath"]}";
            //string path = ConfigurationManager.AppSettings["filePath"];
            string url = "~/MlaDiary/DepartmentDetails/Image/";
            string path = Server.MapPath(url);
            try
            {
                string promiseid = model.PromiseId.ToString();
                string deptID = model.DeptId.ToString();
                string workid = CurrentSession.workid.ToString();
                model.PromiseId = Convert.ToInt64(promiseid);
                model.DeptId = deptID;
                model.ProjectId = Convert.ToInt64(workid);

                model.isActive = true;
                model.createdBy = CurrentSession.UserID;
                model.ModifiedBy = Guid.Parse(CurrentSession.UserID);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                byte[] bytes1 = null;
                string type = "";

                int result = 0;
                if (!string.IsNullOrEmpty(model.ImageFileName))
                {
                    string[] filenamearr = model.ImageFileName.Split('.');
                    string FileName = filenamearr[0].Replace("-", "") + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + filenamearr[1];
                    string thumbFileName = filenamearr[0].Replace("-", "") + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "_thumb." + filenamearr[1];
                    ConvertBase64StringToBytes(model.ImageLocation, ref bytes1, ref type);
                    System.IO.File.WriteAllBytes(path + "/" + FileName, bytes1);
                    //model.ImageLocation = FileName;
                    //model.ImageThumbLocation = thumbFileName;
                    model.ImageLocation = "DepartmentDetails/Image/" + FileName;
                    model.ImageThumbLocation = "DepartmentDetails/Image/" + thumbFileName;
                    model.ImageTitle = model.ImageTitle;
                    model.ImageDescription = model.ImageDescription;
                    result = PriorityProject.SaveDeptImage(model);


                    //Code to create image thumbnail
                    FileName = Path.Combine(path, FileName);
                    //return Json(FileName, JsonRequestBehavior.AllowGet);
                    thumbFileName = Path.Combine(path, thumbFileName);
                    createThumbnail(FileName, thumbFileName);//function to create and save thumbnail image

                }
                if (!string.IsNullOrEmpty(model.ImageFileName2))
                {
                    string[] filenamearr = model.ImageFileName2.Split('.');
                    string FileName = filenamearr[0].Replace("-", "") + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + filenamearr[1];
                    string thumbFileName = filenamearr[0].Replace("-", "") + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "_thumb." + filenamearr[1];
                    ConvertBase64StringToBytes(model.ImageLocation2, ref bytes1, ref type);
                    System.IO.File.WriteAllBytes(path + "/" + FileName, bytes1);
                    model.ImageLocation = "DepartmentDetails/Image/" + FileName;
                    model.ImageThumbLocation = "DepartmentDetails/Image/" + thumbFileName;
                    model.ImageTitle = model.ImageTitle2;
                    model.ImageDescription = model.ImageDescription;
                    result = PriorityProject.SaveDeptImage(model);

                    //Code to create image thumbnail
                    FileName = Path.Combine(path, FileName);
                    thumbFileName = Path.Combine(path, thumbFileName);
                    createThumbnail(FileName, thumbFileName);//function to create and save thumbnail image
                }
                if (!string.IsNullOrEmpty(model.ImageFileName3))
                {
                    string[] filenamearr = model.ImageFileName3.Split('.');
                    string FileName = filenamearr[0].Replace("-", "") + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + filenamearr[1];
                    string thumbFileName = filenamearr[0].Replace("-", "") + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "_thumb." + filenamearr[1];
                    ConvertBase64StringToBytes(model.ImageLocation3, ref bytes1, ref type);
                    System.IO.File.WriteAllBytes(path + "/" + FileName, bytes1);
                    model.ImageLocation = "DepartmentDetails/Image/" + FileName;
                    model.ImageThumbLocation = "DepartmentDetails/Image/" + thumbFileName;
                    model.ImageTitle = model.ImageTitle3;
                    model.ImageDescription = model.ImageDescription;
                    result = PriorityProject.SaveDeptImage(model);

                    //Code to create image thumbnail
                    FileName = Path.Combine(path, FileName);
                    thumbFileName = Path.Combine(path, thumbFileName);

                    createThumbnail(FileName, thumbFileName);//function to create and save thumbnail image
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public static void createThumbnail(string FileName, string thumbFileName)
        {
            Image img = Image.FromFile(FileName, true);
            int imgHeight = 150;
            int imgWidth = 200;
            //if (img.Width < img.Height)
            //{
            //    //portrait image  
            //    imgHeight = 200;
            //    var imgRatio = (float)imgHeight / (float)img.Height;
            //    imgWidth = Convert.ToInt32(img.Height * imgRatio);
            //}
            //else if (img.Height < img.Width)
            //{
            //    //landscape image  
            //    imgWidth = 200;
            //    var imgRatio = (float)imgWidth / (float)img.Width;
            //    imgHeight = Convert.ToInt32(img.Height * imgRatio);
            //}
            Image thumb = img.GetThumbnailImage(imgWidth, imgHeight, () => false, IntPtr.Zero);
            thumb.Save(thumbFileName);
        }
        private static void ConvertBase64StringToBytes(string base64String, ref byte[] bytes1, ref string Type)
        {
            string[] split = base64String.Split(',');
            bytes1 = System.Convert.FromBase64String(base64String.Substring(base64String.IndexOf(',') + 1));
            if (base64String != "")
            {
                split = base64String.Split(',');
                Type = split[0];

            }
        }
        #endregion

        #region "Partial View Call"
        public ActionResult openImageView(string ProjectId, string deptID, string workID, string path)
        {
            try
            {
                CurrentSession.workid = workID;
                PriorityProject model = new PriorityProject();
                string promiseid = ProjectId.ToString();
                deptID = deptID.ToString();
                model.PromiseId = Convert.ToInt64(promiseid);
                model.DeptId = deptID;

                PriorityProject pd = new PriorityProject();
                model.workimageslist = pd.getWorkImage(Convert.ToInt64(workID));
                model.IsImage = true;
                if (model.workimageslist.Count < 1)
                {
                    model.IsImage = false;
                }
                model.path = path;
                model.ProjectId = Convert.ToInt64(workID);
                return PartialView("_Images", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult ViewWorkDetails(string promiseid, string deptID, string workID)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                model.PromiseId = Convert.ToInt64(promiseid);
                //if (workID == "0" && CurrentSession.workid != "")
                //{
                //    workID = CurrentSession.workid.ToString();
                //}
                CurrentSession.workid = workID;
                model.DeptId = deptID;
                model.ProjectId = Convert.ToInt32(workID);

                model.PromiseId = Convert.ToInt64(promiseid);
                model.projectslist = PriorityProject.getDeptProjectsList(promiseid, CurrentSession.DeptID);
                model.PromiseName = "";
                if (model.projectslist.Count > 0)
                {
                    model.PromiseName = "(" + model.projectslist[0].PromiseRefId.ToString() + ") " + model.projectslist[0].PromiseName.ToString();
                }
                TempData["PromiseName"] = model.PromiseName;
                model.StatusTypeList = Promise.GetStatusTypes();
                model.mProjectDepartmentList = PriorityProject.GetProjectDepartments(model.PromiseId);
                model.PromiseId = Convert.ToInt64(promiseid);
                model.DeptId = deptID;
                model.ProjectId = Convert.ToInt64(workID);
                foreach (var str in model.mProjectDepartmentList)
                {
                    if (deptID == str.Value)
                    {
                        model.DeptName = str.Text;
                    }
                }
                model.projectServiceName = CurrentSession.serviceName;
                model.isapex = CurrentSession.isApex;
                model.ProjectStatusTypeList = PriorityProject.GetProjectStatus();
                model.worksdatalist = PriorityProject.GetWorkData(promiseid, deptID, Convert.ToInt32(workID));
                model.CategoryTypeList = PriorityProject.GetCategory();
                //model.ResolutionList = PriorityProject.GetResolutionNo();
                model.ResolutionOfcTypeList = PriorityProject.GetResolutionOfcType();

                DataSet ds = new DataSet();
                ds = UserModelFunction.GetPermission(CurrentSession.UserID);
                model.projectUserActions = "";
                model.workedityn = "N";
                if (ds.Tables[10].Rows.Count > 0)
                {
                    model.projectUserActions = ds.Tables[10].Rows[0][1].ToString();
                }
                if (model.projectUserActions != string.Empty && model.projectUserActions != null)
                {
                    string[] actions = model.projectUserActions.Split(',');
                    foreach (string str in actions)
                    {
                        if (str == "3")
                        {
                            model.workedityn = "Y";
                        }
                    }
                }
                CurrentSession.CurrentAction = "EditWork";
                if (deptID == null || deptID == "")
                {
                    model.mode = "";
                    TempData["Message"] = "Please add Department before enter work details.";
                }
                PriorityProject pd = new PriorityProject();
                model.workimageslist = pd.getWorkImage(Convert.ToInt32(workID));
                model.IsImage = true;
                if (model.workimageslist.Count < 1)
                {
                    model.IsImage = false;
                }
                return PartialView("_WorkDetails", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult AddWorkDetails(string promiseid, string deptID)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                model.PromiseId = Convert.ToInt64(promiseid);
                CurrentSession.workid = "0";
                model.DeptId = deptID;
                model.ProjectId = 0;

                model.PromiseId = Convert.ToInt64(promiseid);
                model.projectslist = PriorityProject.getDeptProjectsList(promiseid, CurrentSession.DeptID);
                model.PromiseName = "";
                if (model.projectslist.Count > 0)
                {
                    model.PromiseName = "(" + model.projectslist[0].PromiseRefId.ToString() + ") " + model.projectslist[0].PromiseName.ToString();
                }
                TempData["PromiseName"] = model.PromiseName;
                model.StatusTypeList = Promise.GetStatusTypes();
                model.mProjectDepartmentList = PriorityProject.GetProjectDepartments(model.PromiseId);
                model.PromiseId = Convert.ToInt64(promiseid);
                model.DeptId = deptID;
                model.ProjectId = 0;
                foreach (var str in model.mProjectDepartmentList)
                {
                    if (deptID == str.Value)
                    {
                        model.DeptName = str.Text;
                    }
                }
                model.projectServiceName = CurrentSession.serviceName;
                model.isapex = CurrentSession.isApex;
                model.ProjectStatusTypeList = PriorityProject.GetProjectStatus();
                model.worksdatalist = PriorityProject.GetWorkData(promiseid, deptID, 0);
                model.worksdatalist.ProjectId = 0;
                model.CategoryTypeList = PriorityProject.GetCategory();
                //model.ResolutionList = PriorityProject.GetResolutionNo();
                model.ResolutionOfcTypeList = PriorityProject.GetResolutionOfcType();

                DataSet ds = new DataSet();
                ds = UserModelFunction.GetPermission(CurrentSession.UserID);
                model.projectUserActions = "";
                model.workedityn = "N";
                if (ds.Tables[10].Rows.Count > 0)
                {
                    model.projectUserActions = ds.Tables[10].Rows[0][1].ToString();
                }
                if (model.projectUserActions != string.Empty && model.projectUserActions != null)
                {
                    string[] actions = model.projectUserActions.Split(',');
                    foreach (string str in actions)
                    {
                        if (str == "3")
                        {
                            model.workedityn = "Y";
                        }
                    }
                }
                CurrentSession.CurrentModule = "Add";
                CurrentSession.CurrentAction = "AddWork";
                if (deptID == null || deptID == "")
                {
                    model.mode = "";
                    TempData["Message"] = "Please add Department before enter work details.";
                }
                return PartialView("_WorkDetails", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult ViewProjects(string ProjectId)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                model.PromiseId = Convert.ToInt64(ProjectId);
                model.projectslist = PriorityProject.getDeptProjectsList(ProjectId, CurrentSession.DeptID);
                if (model.projectslist.Count > 0)
                {
                    model.PromiseName = model.projectslist[0].PromiseName.ToString();
                    model.PromiseRefId = model.projectslist[0].PromiseRefId.ToString();
                }
                model.PromiseTypeList = PriorityProject.GetPromise("All");
                if (ProjectId == "0")
                {
                    model.PromiseName = "";
                    model.mode = "Add";
                }
                return PartialView("_Project", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult ViewDepartments(string ProjectId)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                string flag = "Dept";
                if (CurrentSession.serviceName == "MC")
                {
                    flag = "office";
                }
                model.mDepartmentList = PriorityProject.GetDeptOfficeListforProject(CurrentSession.DeptID, CurrentSession.OfficeId, flag, Convert.ToInt32(CurrentSession.isApex),Convert.ToInt32(ProjectId));
                model.mProjectDepartmentList = PriorityProject.GetProjectDepartments(Convert.ToInt64(ProjectId));
                //code to remove already added items from list
                List<string> myStringList = model.mProjectDepartmentList.AsEnumerable().Select(x => x.Value).ToList();
                IEnumerable<SelectListItem> filteredList = model.mDepartmentList.Where(x => !myStringList.Contains(x.Value));
                model.mDepartmentList = new SelectList(filteredList, "Value", "Text");
                model.mProjectDepartmentList = PriorityProject.GetDeptsListforProject(CurrentSession.DeptID, CurrentSession.OfficeId, flag, Convert.ToInt32(CurrentSession.isApex), Convert.ToInt32(ProjectId));
                model.projectServiceName = CurrentSession.serviceName;
                model.isapex = CurrentSession.isApex;
                model.projectslist = PriorityProject.getDeptProjectsList(ProjectId, CurrentSession.DeptID);
                model.PromiseName = "";
                if (model.projectslist.Count > 0)
                {
                    model.PromiseName = "(" + model.projectslist[0].PromiseRefId.ToString() + ") " + model.projectslist[0].PromiseName.ToString();
                }
                model.PromiseId = Convert.ToInt64(ProjectId);
                model.DeptId = CurrentSession.DeptID;
                TempData["apex"] = Convert.ToInt16(CurrentSession.isApex);
                return PartialView("_Departments", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult ViewDocuments(string ProjectId, string deptID, string workID)
        {
            try
            {
                CurrentSession.workid = workID;
                PriorityProject model = new PriorityProject();
                model.worksdatalist = PriorityProject.GetWorkData(ProjectId, deptID, Convert.ToInt32(workID));
                return PartialView("_Documents", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult ViewConstituencies(string ProjectId, string deptID, string workID)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                model.worksdatalist = PriorityProject.GetWorkData(ProjectId, deptID, Convert.ToInt32(workID));
                model.mProjectConstituencyList = PriorityProject.GetConstituencyList(model.DeptId);

                model.mWorkConstituencyList = PriorityProject.getWorkConstituency(Convert.ToInt32(workID));

                List<string> myStringList = model.mWorkConstituencyList.AsEnumerable().Select(x => x.Value).ToList();
                IEnumerable<SelectListItem> filteredList = model.mProjectConstituencyList.Where(x => !myStringList.Contains(x.Value));
                model.mProjectConstituencyList = new SelectList(filteredList, "Value", "Text");

                model.PromiseId = Convert.ToInt64(ProjectId);
                model.DeptId = deptID;
                model.ProjectId = Convert.ToInt64(workID);
                model.ProjectMappedConstituencyId = 0;
                model.isActive = true;
                model.createdBy = CurrentSession.UserID;
                CurrentSession.workid = workID;

                model.IsConstituency = true;

                if (model.mWorkConstituencyList.Count() < 1)
                {
                    model.IsConstituency = false;
                }
                return PartialView("_Constituency", model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "WebMethods"
        [HttpGet]
        public JsonResult getProjectsList()
        {
            try
            {
                var dtt = PriorityProject.GetPromise("0");
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public JsonResult GetProjectDepartmentList(Int64 ID)
        {
            var dtt = PriorityProject.GetProjectDepartmentsforDelete(ID);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult GetResolutionOfcTypeList(int ResolutionOfcType)
        {
            var dtt = PriorityProject.GetResolutionNo(ResolutionOfcType);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getProjectDetails(string projectid)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                model.mode = "Edit";
                var dtt = PriorityProject.getDeptProjectsList(projectid, CurrentSession.DeptID);// PriorityProject.getCMProjectsList(projectid, CurrentSession.DeptID, CurrentSession.isApex);
                TempData["promiseId"] = projectid;
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public JsonResult getResolutionDetails(int resolutionNo)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                var dtt = PriorityProject.getAgendaDetailByResolutionNo(resolutionNo);// PriorityProject.getCMProjectsList(projectid, CurrentSession.DeptID, CurrentSession.isApex);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public JsonResult getWorkImages(string workId)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                var dtt = model.getWorkImage(Convert.ToInt64(workId));
                CurrentSession.workid = workId;
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public JsonResult getWorkDocuments(string workId)
        {
            try
            {
                PriorityProject model = new PriorityProject();
                var dtt = model.getWorkDocuments(Convert.ToInt64(workId));
                CurrentSession.workid = workId;
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public JsonResult getProjectConstituency(string workId)
        {
            try
            {
                var dtt = PriorityProject.getWorkConstituency(Convert.ToInt64(workId));
                CurrentSession.workid = workId;
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public JsonResult getProjects(int projectId, string deptId, int constituencyId, int? statusId, string useraction)
        {
            CurrentSession.CurrentModule = "";
            CurrentSession.workid = "0";
            string currdeptId = CurrentSession.DeptID;
            int currOfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            var dtt = PriorityProject.getCMProjectsList(projectId, deptId, constituencyId, statusId, currdeptId, currOfficeId, CurrentSession.isApex);
            string flag = "Dept";
            if(CurrentSession.serviceName=="MC")
            {
                flag = "office";
            }
            if (useraction != null && useraction.ToString() == "Edit")
            {
                dtt = PriorityProject.getprojectdeptlist(projectId, deptId, constituencyId, statusId, flag);
            }

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getProjectsforDasboard(string currDeptId, int officeid, int projectId, string deptId, int constituencyId, int? statusId)
        {
            CurrentSession.CurrentModule = "View";
            var dtt = PriorityProject.getProjectsListForDashboard(currDeptId, officeid, projectId, deptId, constituencyId, statusId);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getDeptWiseWorkStatus()
        {
            string flag = "Dept";
            if (CurrentSession.serviceName == "MC")
            {
                flag = "office";
            }
            var dtt = PriorityProject.getDeptWiseWorkStatus(flag);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getDeptWiseProjectSummary(int projectId, string deptId, int constituencyId, int statusId)
        {
            string currdeptId = CurrentSession.DeptID;
            int currofcId = Convert.ToInt32(CurrentSession.OfficeId);
            string flag = "Dept";
            if (CurrentSession.serviceName == "MC" && CurrentSession.isApex == "0")
            {
                flag = "office";
            }
            var dtt = PriorityProject.getDeptWiseProjectSummary(projectId, deptId, constituencyId, statusId, currdeptId, currofcId, flag);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getProjectWiseDeptSummary(int projectId, string deptId, int constituencyId, int statusId)
        {
            string flag = "Dept";
            if (CurrentSession.serviceName == "MC" && CurrentSession.isApex=="0")
            {
                flag = "office";
            }
            var dtt = PriorityProject.getProjectWiseDeptSummary(projectId, deptId, constituencyId, statusId, CurrentSession.DeptID, flag);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        public JsonResult getDeptWorkStatusWiseSummary(int projectId, string deptId, int constituencyId, int statusId)
        {
            string currdeptid = CurrentSession.OfficeId;
            var dtt = PriorityProject.getDeptWorkStatusWiseSummary(projectId, deptId, constituencyId, statusId, currdeptid);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        public JsonResult getDeptProjectStatusWiseSummary(int projectId, string deptId, int constituencyId, int statusId)
        {
            string currdeptid = CurrentSession.OfficeId;
            var dtt = PriorityProject.getDeptProjectStatusWiseSummary(projectId, deptId, constituencyId, statusId, currdeptid);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getProjectWiseWorkSummary(string deptId, string officeId, int projectId, string sdeptId, int constituencyId, int statusId)
        {
            var dtt = PriorityProject.getProjectWiseWorkStatus(deptId, officeId, projectId, sdeptId, constituencyId, statusId);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getProjectDeptWiseWorkStatus(int projectId, string deptId, int constituencyId, int statusId)
        {
            var dtt = PriorityProject.getProjectDeptWiseWorkStatus(projectId, deptId, constituencyId, statusId);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getProjectWiseDeptWorkSummary(int projectID)
        {
            var dtt = PriorityProject.getProjectWiseDeptWorkStatus(projectID);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getDeptWiseProjectWorkStatus(int promiseid, int spromiseid, string deptId, int constituencyId, int statusId)
        {
            var dtt = PriorityProject.getDeptWiseProjectWorkStatus(promiseid, spromiseid, deptId, constituencyId, statusId);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getprojectdeptstatusdata(int promiseId, string deptId, int spromiseId, string sdeptId, int constituencyId, int statusId)
        {
            string flag = "Dept";
            if (CurrentSession.serviceName == "MC" && CurrentSession.isApex == "0")
            {
                flag = "office";
            }
            if (deptId=="0" && CurrentSession.isApex == "0")
            {
                deptId = CurrentSession.DeptID;
            }
            var dtt = PriorityProject.getdeptofcprojectdata(promiseId, deptId, flag, spromiseId, sdeptId, constituencyId, statusId);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getdeptofcprojectdata(int promiseId, string deptId, int spromiseId, string sdeptId, int constituencyId, int statusId)
        {
            string flag = "Dept";
            if (CurrentSession.serviceName == "MC")
            {
                flag = "office";
            }
            if (deptId == "0" && CurrentSession.isApex == "0")
            {
                deptId = CurrentSession.DeptID;
            }
            var dtt = PriorityProject.getdeptofcprojectdata(promiseId, deptId, flag, spromiseId, sdeptId, constituencyId, statusId);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getdeptwiseofclist(string deptId,int ProjectId)
        {
            PriorityProject model = new PriorityProject();
            if (CurrentSession.serviceName == "MC")
            {
                model.mDepartmentList = PriorityProject.GetOfficebyDepartmentId(deptId);
            }
            else
            {
                model.mDepartmentList = DiaryViewModel.getDepartment();
            }
            
            model.mProjectDepartmentList = PriorityProject.GetProjectDepartments(ProjectId);
            List<string> myStringList = model.mProjectDepartmentList.AsEnumerable().Select(x => x.Value).ToList();

            IEnumerable<SelectListItem> filteredList = model.mDepartmentList.Where(x => !myStringList.Contains(x.Value));
            model.mDepartmentList = new SelectList(filteredList, "Value", "Text");
            var dtt = model.mDepartmentList;

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getConstituencyWiseProjectCount()
        {
            string currdeptId = CurrentSession.DeptID;
            int currofcId = Convert.ToInt32(CurrentSession.OfficeId);
            int isapex = Convert.ToInt32(CurrentSession.isApex);
            var dtt = PriorityProject.getConstituencyWiseProjectCount(currdeptId, currofcId, isapex);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getStatusWiseProjectCount()
        {
            string currdeptId = CurrentSession.DeptID;
            int currofcId = Convert.ToInt32(CurrentSession.OfficeId);
            int isapex = Convert.ToInt32(CurrentSession.isApex);
            var dtt = PriorityProject.getStatusWiseProjectCount(currdeptId, currofcId, isapex);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getProjectWiseworkCount()
        {
            string currdeptId = CurrentSession.DeptID;
            int currofcId = Convert.ToInt32(CurrentSession.OfficeId);
            int isapex = Convert.ToInt32(CurrentSession.isApex);
            var dtt = PriorityProject.getProjectWiseworkCount(currdeptId, currofcId, isapex);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult getDeptOfficeWiseProjectCount()
        {
            string currdeptId = CurrentSession.DeptID;
            int currofcId = Convert.ToInt32(CurrentSession.OfficeId);
            int isapex = Convert.ToInt32(CurrentSession.isApex);
            if (CurrentSession.serviceName == "MC" && CurrentSession.isApex == "0")
            {
                currofcId = 0;
            }
            var dtt = PriorityProject.getDeptOfficeWiseProjectCount(currdeptId, currofcId, isapex);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion
    }
}