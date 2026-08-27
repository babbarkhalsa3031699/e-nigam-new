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

using static CMNirdesh.Models.MCHouse.mCouncilCorporation;

namespace CMNirdesh.Controllers
{
    public class mCouncilCorporationController : Controller
    {
        // GET: mCouncilCorporation
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstMCModel model = new LstMCModel();
                var data = GetMCModelList();
                model._ListMCModel = data;
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                ViewBag.MCDropDown = MCDropDownSelectList();
                var UserID = CurrentSession.UserID;
                return View(model);
            }

        }
        public ActionResult Create()
        {
            return View(new mCouncilCorporation());
        }


        [HttpPost]
        public JsonResult Create(mCouncilCorporation model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid model received" }, JsonRequestBehavior.AllowGet);

                var mcName = string.IsNullOrWhiteSpace(model.MCName) ? "MC" : model.MCName.Trim();
                var folderPath = "MC/" + mcName + "/";

                // Create unique timestamp for all files uploaded in this transaction
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // ------------------------------
                // MC Logo
                // ------------------------------
                var mcLogo = Request.Files["MCLogoFile"];
                if (mcLogo != null && mcLogo.ContentLength > 0)
                {
                    string ext = Path.GetExtension(mcLogo.FileName);
                    string newFileName = $"{timestamp}_MCLogo{ext}";

                    BlobStorage.Savefile(mcLogo, folderPath, newFileName);
                    model.MClogo = newFileName;
                }

                // ------------------------------
                // Head Photo
                // ------------------------------
                var headPhoto = Request.Files["HeadPhotoFile"];
                if (headPhoto != null && headPhoto.ContentLength > 0)
                {
                    string ext = Path.GetExtension(headPhoto.FileName);
                    string newFileName = $"{timestamp}_Head{ext}";

                    BlobStorage.Savefile(headPhoto, folderPath, newFileName);
                    model.HeadPhoto = newFileName;
                }

                // ------------------------------
                // Sub Head Photo
                // ------------------------------
                var subHeadPhoto = Request.Files["SubHeadingPhotoFile"];
                if (subHeadPhoto != null && subHeadPhoto.ContentLength > 0)
                {
                    string ext = Path.GetExtension(subHeadPhoto.FileName);
                    string newFileName = $"{timestamp}_SubHead{ext}";

                    BlobStorage.Savefile(subHeadPhoto, folderPath, newFileName);
                    model.SubHeadPhoto = newFileName;
                }

                // ------------------------------
                // Save Database Record
                // ------------------------------
                var data = SaveMCRecord(model, folderPath);

                // Maintain User Session
                Session[CurrentSession.UserID] = CurrentSession.UserID;

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        //GET: Fetch Record Row by selected ID
        public JsonResult Edit(int id)
        {

            LstMCModel model = new LstMCModel();
            var data = mCouncilCorporation.GetMCRecordById(id);
            model._ListMCModel = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore                                   
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // POST: Update selected row after alteration
        [HttpPost]
        public JsonResult Update(mCouncilCorporation model)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid data received" }, JsonRequestBehavior.AllowGet);

                var mcName = string.IsNullOrWhiteSpace(model.MCName) ? "MC" : model.MCName.Trim();
                var folderPath = "MC/" + mcName + "/";

                // Timestamp for unique naming
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // ------------------------------
                // MC LOGO
                // ------------------------------
                var mcLogo = Request.Files["MCLogoFile"];
                if (mcLogo != null && mcLogo.ContentLength > 0)
                {
                    string ext = Path.GetExtension(mcLogo.FileName);

                    // Completely NEW file name
                    string newFileName = $"{timestamp}_MCLogo{ext}";

                    BlobStorage.Savefile(mcLogo, folderPath, newFileName);
                    model.MClogo = newFileName;
                }

                // ------------------------------
                // HEAD PHOTO
                // ------------------------------
                var headPhoto = Request.Files["HeadPhotoFile"];
                if (headPhoto != null && headPhoto.ContentLength > 0)
                {
                    string ext = Path.GetExtension(headPhoto.FileName);
                    string newFileName = $"{timestamp}_Head{ext}";

                    BlobStorage.Savefile(headPhoto, folderPath, newFileName);
                    model.HeadPhoto = newFileName;
                }

                // ------------------------------
                // SUB HEAD PHOTO
                // ------------------------------
                var subHeadPhoto = Request.Files["SubHeadingPhotoFile"];
                if (subHeadPhoto != null && subHeadPhoto.ContentLength > 0)
                {
                    string ext = Path.GetExtension(subHeadPhoto.FileName);
                    string newFileName = $"{timestamp}_SubHead{ext}";

                    BlobStorage.Savefile(subHeadPhoto, folderPath, newFileName);
                    model.SubHeadPhoto = newFileName;
                }

                // ------------------------------
                // UPDATE DB
                // ------------------------------
                var result = mCouncilCorporation.UpdateMCRecordById(model, folderPath);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        //Post: Delete Selected Row Id

        public JsonResult Delete(int Id)
        {
            var data = mCouncilCorporation.DeleteMCRecordById(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MCDropDown()
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[mMCDropDown]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["unitname"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
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

        public List<SelectListItem> MCDropDownSelectList()
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



                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[mMCDropDown]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["unitname"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return items;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return items;
            }

        }
    }
}