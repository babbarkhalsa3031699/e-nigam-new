using CMNirdesh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class mMenuController : Controller
    {
        // GET: Menu List Index
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstMenu model = new LstMenu();
                var data = MenuModel2.GetMenuList();
                model._LstMenu = data;
                return View(model);

            }
        }

        public ActionResult Create()
        {
            return View(new MenuModel2());
        }

        // Create: New Menu Record

        [HttpPost]

        public JsonResult Create(MenuModel2 model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var root = "~/SecureFileStructure/menu_icon";
                    bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                    if (!folderpath)
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
                    }
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string FileName = "UserPhoto" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                        files.SaveAs(path);
                        model.icon = "SecureFileStructure/menu_icon/" + FileName;
                        var data = MenuModel2.SaveMenuRecord(model);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var data = MenuModel2.SaveMenuRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);

        }


        // GET: Menu List By Id
        public JsonResult Edit(int id)
        {
            LstMenu model = new LstMenu();
            var data = MenuModel2.GetMenuModelRowById(id);
            model._LstMenu = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // Update: Menu List By Selected Id   

        [HttpPost]

        public JsonResult Update(MenuModel2 model)
        {
            int val;
            
            try
            {
                if (Request.Files.Count > 0)
                {
                    val = 1;
                    var root = "~/SecureFileStructure/menu_icon";
                    bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                    if (!folderpath)
                    {
                        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
                    }
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string FileName = "UserPhoto" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                        files.SaveAs(path);
                        model.icon = "SecureFileStructure/menu_icon/" + FileName;
                        var data = MenuModel2.UpdateMenuRecordById(model,Convert.ToString(val));
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    val = 2;
                    var data = MenuModel2.UpdateMenuRecordById(model,Convert.ToString(val));
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(JsonRequestBehavior.AllowGet);

        }

        // Delete: Menu List By Selected Id
        public JsonResult Delete(int Id)
        {
            var data = MenuModel2.DeleteMenuRowById(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
