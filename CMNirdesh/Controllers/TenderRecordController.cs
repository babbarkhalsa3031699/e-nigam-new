using CMNirdesh.Models;
using CMNirdesh.Models.MCHouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class TenderRecordController : Controller
    {
        // GET: TenderRecord
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                TenderRecord model = new TenderRecord();
                var data = TenderRecord.GetList();
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                model._LstTenderRecord = data;
                return View(model);
            }
        }

        public ActionResult Create()
        {
            return View(new TenderRecord());
        }

        // Create: New  Record

        [HttpPost]
        public JsonResult Create(TenderRecord model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {

                    var folderpath = "MC/" + CurrentSession.MCName + "/Tender/";

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var ext = System.IO.Path.GetExtension(files.FileName);
                        var fileName = string.Format(@"{0}" + ext, Guid.NewGuid());
                         BlobStorage.Savefile(files, folderpath, fileName);
                        model.TenderFileName = Path.Combine(folderpath, fileName);
                        var data = TenderRecord.SaveRecord(model);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var data = TenderRecord.SaveRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        //GET:  List By Id
        public JsonResult Edit(int Id)
        {
            TenderRecord model = new TenderRecord();
            var data = TenderRecord.GetRowById(Id);
            model._LstTenderRecord = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        // Update: List By Selected Id   
        [HttpPost]
        public JsonResult Update(TenderRecord model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var root = "~/SecureFileStructure/" + model.StateName+"/Tender";
                    bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                    if (!folderpath)
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
                    }
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string FileName = "Tender" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                        files.SaveAs(path);
                        model.TenderFileName = FileName;
                        var data = TenderRecord.UpdateRecord(model);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var data = TenderRecord.UpdateRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult StateDropDown()
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetStatesByMCid]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["MCName"].ToString());
                    item.Value = Convert.ToString(row["Id"].ToString());
                    items.Add(item);
                }
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult StatusDropDown()
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetStatusName]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["StatusName"].ToString());
                    item.Value = Convert.ToString(row["StatusID"].ToString());

                    items.Add(item);
                }
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Delete(int Id)
        {
            var data = TenderRecord.DeleteRecord(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
       
        
        
        
        
        //Lg Tender part this is common part for uploading Tender for Local Govorment
        
        public ActionResult LGTender()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                DataViewModelTenderList model = new DataViewModelTenderList();
                var data = DataViewModelTender.GetList();
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                model.lgTenders = data;
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult SaveLGTender(LgTender model1)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                try
                {
                    if (Request.Files.Count > 0)
                    {
                        var folderpath = "MC/LG/Tender/";
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            var files = Request.Files[i];
                            var ext = System.IO.Path.GetExtension(files.FileName);
                            var fileName = string.Format(@"{0}" + ext, Guid.NewGuid());
                            BlobStorage.Savefile(files, folderpath, fileName);
                            model1.TenderDocumentPath = Path.Combine(folderpath, fileName);
                            var data = DataViewModelTender.SaveTender(model1);
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var data = DataViewModelTender.SaveTender(model1);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EditLGTender(int Id)
        {
            DataViewModelTenderList model = new DataViewModelTenderList();
            var data = DataViewModelTender.GetRowById(Id);
            model.lgTenders = data;
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateLGTender(LgTender model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                
                    if (Request.Files.Count > 0)
                    {
                        var folderpath = "MC/LG/Tender/";
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            var files = Request.Files[i];
                            var ext = System.IO.Path.GetExtension(files.FileName);
                            var fileName = string.Format(@"{0}" + ext, Guid.NewGuid());
                            BlobStorage.Savefile(files, folderpath, fileName);
                            model.TenderDocumentPath = Path.Combine(folderpath, fileName);
                            var data = DataViewModelTender.UpdateTender(model);
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    var data = DataViewModelTender.UpdateTender(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteLGTender(int Id)
        {
            var data = DataViewModelTender.DeleteRecord(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}