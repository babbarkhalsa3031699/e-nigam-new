using CMNirdesh.Models;
using CMNirdesh.Models.MCHouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Record List
        public ActionResult Index()
        {
            string MCName = CurrentSession.MCName;
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstGalleryModel model = new LstGalleryModel();
                var data = HomeGallery.GetGalleryList()
                      .Where(g => g.MCName == Convert.ToString(CurrentSession.MCName))
                      .ToList();
                model._LstHomeGallery = data;
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                return View(model);
            }
        }
        public ActionResult Create()
        {
            return View(new HomeGallery());
        }

        // Create: New Record

        [HttpPost]
        public JsonResult Create(HomeGallery model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                   
                    var root = "MC/" + CurrentSession.MCName + "/Gallery/";

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string FileName = "Gallery" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        //var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                        BlobStorage.Savefile(files, root, FileName);
                        model.ImagePath= root + FileName;
                    }

                }
                var data = HomeGallery.SaveGalleryDetails(model);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        //GET: Fetch Record Row by selected ID
        public JsonResult Edit(int id)
        {

            LstGalleryModel model = new LstGalleryModel();
            var data = HomeGallery.GetGalleryRecordById(id);
            model._LstHomeGallery = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        //POST: Update selected row after alteration
        [HttpPost]

        public JsonResult Update(HomeGallery model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var root = "MC/" + CurrentSession.MCName + "/Gallery/";
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string FileName = "Gallery" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                        //files.SaveAs(path);
                        BlobStorage.Savefile(files, root, FileName);
                        model.ImagePath = FileName;
                    }
                    var data = HomeGallery.UpdateImageGalleryRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var data = HomeGallery.UpdateImageGalleryRecord(model);
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
            var data = HomeGallery.DeleteGalleryRecordById(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }

}
