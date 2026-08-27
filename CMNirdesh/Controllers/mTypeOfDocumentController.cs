using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMNirdesh.Models.Diaries;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CMNirdesh.Controllers
{
    public class mTypeOfDocumentController : Controller
    {
        // GET: TypeOfDocument List
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstTypeOfDocumentModel model = new LstTypeOfDocumentModel();
                var data = mTypeOfDocument.GetTypeOfDocumentList();
                model.mDepartmentList = DiaryViewModel.getDepartment();
                model.OfficeList = DiaryViewModel.GetOfficeByDept(model.DeptId, CurrentSession.OfficeId);
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                model._ListTypeOfDocumentModel = data;
                return View(model);

            }
        }

        public ActionResult Create()
        {
            return View(new mTypeOfDocument());
        }

        //Post: Insert New TypeOfDocument Record

        [HttpPost]
        public JsonResult Create(mTypeOfDocument mTypeOfDocument)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = UserModelFunction.GetDeptDtlsbyDeptId(mTypeOfDocument.DeptId);

                string MCName = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MCName = ds.Tables[0].Rows[0]["MCName"].ToString();
                }
                var folderPath = "MC/" + MCName + "/ReferenceIcon/";
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        //var fileName = System.IO.Path.GetFileName(files.FileName);
                        var ext = System.IO.Path.GetExtension(files.FileName);
                        string FileName = "ReferenceIcon" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) +  ext;
                      // BlobStorage.Savefile(files, folderPath, FileName);
                        BlobStorage.Savefile(files, folderPath, FileName);
                        mTypeOfDocument.AppIcon = "MC/" + MCName + "/ReferenceIcon/" + FileName;
                        var data = mTypeOfDocument.SaveTypeOfDocumentRecord(mTypeOfDocument);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var data = mTypeOfDocument.SaveTypeOfDocumentRecord(mTypeOfDocument);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);

        }


        // GET: Fetch Record Row by selected ID
        public JsonResult Edit(int id)
        {

            LstTypeOfDocumentModel model = new LstTypeOfDocumentModel();
            var data = mTypeOfDocument.GetTypeOfDocumentRecordById(id);
            model._ListTypeOfDocumentModel = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // POST: Update selected row after alteration
        [HttpPost]
        
        public JsonResult Update(mTypeOfDocument mTypeOfDocument)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = UserModelFunction.GetDeptDtlsbyDeptId(mTypeOfDocument.DeptId);

                string MCName = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MCName = ds.Tables[0].Rows[0]["MCName"].ToString();
                }
                int val = 1;
                var folderPath = "MC/" + MCName + "/ReferenceIcon/";
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {

                        var files = Request.Files[i];
                        //var fileName = System.IO.Path.GetFileName(files.FileName);
                        var ext = System.IO.Path.GetExtension(files.FileName);
                        string FileName = "ReferenceIcon" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + ext;
                       BlobStorage.Savefile(files, folderPath, FileName);
                        mTypeOfDocument.AppIcon = "MC/" + MCName + "/ReferenceIcon/" + FileName;
                        var data = mTypeOfDocument.UpdateTypeOfDocumentRecordById(mTypeOfDocument,Convert.ToString(val));
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    val = 2;
                    var data = mTypeOfDocument.UpdateTypeOfDocumentRecordById(mTypeOfDocument,Convert.ToString(val));
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

       //Post: Delete Selected Row Id
        public JsonResult Delete(int id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                {

                    SqlCommand cmd = new SqlCommand("mTypeOfDocumentDelete", connection);
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DocumentTypeId", id);
                        connection.Open();
                        cmd.ExecuteScalar();
                        connection.Close();
                    }
                }
                return Json(JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(JsonRequestBehavior.AllowGet);

            }
        }



    }
}
