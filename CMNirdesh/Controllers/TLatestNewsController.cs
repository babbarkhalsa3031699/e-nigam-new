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
    public class TLatestNewsController : Controller
    {
        // GET: tFlashNews
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                TLatestNews model = new TLatestNews();
                var data = TLatestNews.GetNewsList();
                model._LstTLatestNews = data;
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                return View(model);
            }
        }

        public ActionResult Create()
        {
            return View(new TLatestNews());
        }

        // Create: New Department Record

        [HttpPost]
        public JsonResult Create(TLatestNews model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var folderpath = "MC/" + CurrentSession.MCName + "/LatestNews/";
                    
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var ext = System.IO.Path.GetExtension(files.FileName);
                        var fileName = string.Format(@"{0}" + ext, Guid.NewGuid());
                        BlobStorage.Savefile(files, folderpath, fileName);
                        model.FileName = Path.Combine(folderpath, fileName);
                      
                    }

                }
                var data = TLatestNews.InsertNewsRecord(model);
                return Json(data, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

          
        }

        // GET: Department List By Id
        public JsonResult Edit(int id)
        {
            TLatestNews model = new TLatestNews();
            var data = TLatestNews.GetNewsRecord(id);
            model._LstTLatestNews = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // Update: Department List By Selected Id   

        [HttpPost]

        public JsonResult Update(TLatestNews model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var root = "~/SecureFileStructure/News/";
                    bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                    if (!folderpath)
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
                    }
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        //   string FileName = "UserPhoto" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), fileName);
                        model.FileName = fileName;
                        files.SaveAs(path);


                    }

                }
                var data = TLatestNews.UpdateNewsRecord(model);
                return Json(data, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Delete(int Id)
        {
            var data = TLatestNews.DeleteNewsRecord(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StatesDropDown()
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
        

    }
}
