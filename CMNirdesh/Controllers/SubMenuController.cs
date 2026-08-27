using CMNirdesh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CMNirdesh.Models.SubMenuModel2;

namespace CMNirdesh.Controllers
{
    public class SubMenuController : Controller
    {
        // GET: SubMenu List
        public ActionResult Index()
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstSubMenu model = new LstSubMenu();
                var data = SubMenuModel2.GetSubMenuList();
                model._LstSubMenu2 = data;
                return View(model);
            }
        }

           
        public JsonResult Create()
        {
            return Json(new SubMenuModel2());
        }

        // POST: Insert SubMenu record

        [HttpPost]
        public JsonResult Create(SubMenuModel2 SubMenuModel)
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
                    for (int i = 0; i<Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string FileName = "UserPhoto" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                        files.SaveAs(path);
                        SubMenuModel.icon = "SecureFileStructure/menu_icon/" + FileName;
                       
                    }
                    var data = SubMenuModel2.SaveSubmenuRecord(SubMenuModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                var data = SubMenuModel2.SaveSubmenuRecord(SubMenuModel);
                return Json(data, JsonRequestBehavior.AllowGet);
                }
            }


          catch (Exception ex)
                {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Selected Record By Id
        public JsonResult Edit(int id)
        {
            LstSubMenu model = new LstSubMenu();
            var data = SubMenuModel2.GetSubMenuRecordById(id);
            model._LstSubMenu2 = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }


        //Post: Alter selected record with value

        [HttpPost]

        public JsonResult Update(SubMenuModel2 SubMenuModel)
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
                        SubMenuModel.icon = "SecureFileStructure/menu_icon/" + FileName;

                    }
                    var data = SubMenuModel2.UpdateSubMenuRecordById(SubMenuModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = SubMenuModel2.UpdateSubMenuRecordById(SubMenuModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }


            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Max(int Id)
        {
            LstSubMenu model = new LstSubMenu();
            var data = SubMenuModel2.Max(Id);
            model._LstSubMenu2 = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        // POST: Delete record By Id
        public JsonResult Delete(int id)
        {
            var data = SubMenuModel2.DeleteSubMenuRecordById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Menu Drop Down
      
        public JsonResult MenuDropDown()
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetMenu]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["MenuName"].ToString());
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
