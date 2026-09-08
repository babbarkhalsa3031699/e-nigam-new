using CMNirdesh.Models;
using CMNirdesh.Models.MCHouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class UtilityController : Controller
    {
        // GET: Utility
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Utility model = new Utility();
                var data = Utility.GetList();
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                model._LstUtilityMenu = data;
                return View(model);
            }
        }
        public ActionResult Create()
        {
            return View(new Utility());
        }
        // Create: New Department Record
        [HttpPost]
        public JsonResult Create(Utility model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {

                    var folderpath = "MC/" + CurrentSession.MCName + "/Utility/";
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var ext = System.IO.Path.GetExtension(files.FileName);
                        var fileName = string.Format(@"{0}" + ext, Guid.NewGuid());
                        BlobStorage.Savefile(files, folderpath, fileName);

                        model.UtilityFileName = Path.Combine(folderpath, folderpath);
                        var data = Utility.SaveRecord(model);
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    var data = Utility.SaveRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);

        }


        // GET: Department List By Id
        public JsonResult Edit(int id)
        {
            Utility model = new Utility();
            var data = Utility.GetRowById(id);
            model._LstUtilityMenu = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        // Update: Department List By Selected Id   
        [HttpPost]
        public JsonResult Update(Utility model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var root = "~/SecureFileStructure/Utility/";
                    bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                    if (!folderpath)
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
                    }
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string FileName = "Utility" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                        files.SaveAs(path);
                        model.UtilityFileName = "SecureFileStructure/Utility/" + FileName;
                        var data = Utility.UpdateRecord(model);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var data = Utility.UpdateRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);


        }
        // Delete: Department List By Selected Id
        public JsonResult Delete(int Id)
        {
            var data = Utility.DeleteRecord(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
