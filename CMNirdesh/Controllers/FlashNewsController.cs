using CMNirdesh.Models;
using CMNirdesh.Models.MCHouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CMNirdesh.Models.MCHouse.FlashNews;

namespace CMNirdesh.Controllers
{
    public class FlashNewsController : Controller
    {
        // GET: FlashNews
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstFlashNewsModel model = new LstFlashNewsModel();
                var data = GetFlashNewsListbyMCId();
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                model._LstFlashNews = data;
                return View(model);

            }
        }

        public ActionResult Create()
        {
            return View(new FlashNews());
        }

        // Create: New FLashNews Record

        [HttpPost]
        public JsonResult Create(FlashNews model)
        {
            model.MCId =  Convert.ToInt16(CurrentSession.StateId);
            try
            {
                if (Request.Files.Count > 0)
                {
                    var root = "MC/" + model.MCName + "/FlashNews/";
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string FileName = "FlashNews" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                         BlobStorage.Savefile(files, root, FileName);
                        model.FlashNewsFile = root + FileName;
                    }

                }
                var data = FlashNews.SaveFlashNewsRecord(model);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult MCDropDown()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string StateID = Convert.ToString(CurrentSession.StateId);

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SelectListItem item = new SelectListItem();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[mMCMenuDropDown]", new object[0]).Tables[0].Rows)
                {
                   
                    if (CurrentSession.RoleID == "18")
                    {
                        item.Text = Convert.ToString(row["MCName"].ToString());
                        item.Value = Convert.ToString(row["id"].ToString());
                        items.Add(item);
                    }
                    
                   else
                    {
                        if (CurrentSession.StateId == row["id"].ToString())
                        {
                            item.Text = Convert.ToString(row["MCName"].ToString());
                            item.Value = Convert.ToString(row["id"].ToString());
                            items.Add(item);
                        }
                    }

                  
                }
               if (CurrentSession.DeptID == "LG00001")
                {
                    item.Text = Convert.ToString(CurrentSession.MCName);
                    item.Value = Convert.ToString(CurrentSession.StateId);
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

        [HttpGet]
        //GET: Fetch Record Row by selected ID
        public JsonResult Edit(int id)
        {

            LstFlashNewsModel model = new LstFlashNewsModel();
            var data = FlashNews.GetFlashNewsRecordById(id);
            model._LstFlashNews = data;
            model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        //POST: Update selected row after alteration
        [HttpPost]

        public JsonResult Update(FlashNews model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var root = "MC/" + model.MCName + "/FlashNews/";
                   
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string FileName = "FlashNews" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        BlobStorage.Savefile(files, root, FileName);
                        model.FlashNewsFile = root + FileName;
                    }
                    var data = FlashNews.UpdateFlashNewsRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = FlashNews.UpdateFlashNewsRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);

                }

            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Post: Delete Selected Row Id

        public JsonResult Delete(int Id)
        {
            var data = FlashNews.DeleteFlashNewsRecordById(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


      

    }

}
