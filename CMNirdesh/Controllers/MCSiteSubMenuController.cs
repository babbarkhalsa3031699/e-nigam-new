using CMNirdesh.Models;
using CMNirdesh.Models.MCHouse;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CMNirdesh.Models.MCHouse.MCSiteSubMenu;
using Newtonsoft.Json;
using System.IO;

namespace CMNirdesh.Controllers
{
    public class MCSiteSubMenuController : Controller
    {
        // GET: MCSiteSubMenu
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                MCSiteSubMenu model = new MCSiteSubMenu();
                var data = MCSiteSubMenu.GetSubMenuList();
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                model._LstSubMenu = data;
                return View(model);
            }
        }
        public JsonResult Create()
        {
            return Json(new MCSiteSubMenu());
        }

        // POST: Insert SubMenu record

        [HttpPost]
        public JsonResult Create(MCSiteSubMenu SubMenuModel)
        {
            try
            {
                if (Request.Files.Count > 0)
                {                    
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var root = "MC/" + SubMenuModel.MCName + "/MCSubMenu/pdf/";
                        bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string extension = Path.GetExtension(fileName);
                        string FileName = "UserPhoto" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        if (extension==".pdf" || extension == ".PDF")
                        {
                            FileName = "linkpdf" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                            SubMenuModel.pdf = "MC/" + SubMenuModel.MCName + "/MCSubMenu/pdf/" + FileName;
                        }
                        else
                        {
                            SubMenuModel.icon = "MC/" + SubMenuModel.MCName + "/MCSubMenu/pdf/" + FileName;
                        }
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                        //files.SaveAs(path);
                        BlobStorage.Savefile(files, root, FileName);

                    }
                    var data = MCSiteSubMenu.SaveSubmenuRecord(SubMenuModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = MCSiteSubMenu.SaveSubmenuRecord(SubMenuModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }


            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Edit(int id)
        {
            MCSiteSubMenu model = new MCSiteSubMenu();
            var data = MCSiteSubMenu.GetSubMenuRecordById(id);
            model._LstSubMenu = data;
            model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);

        }

        public JsonResult MCDropDown()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                items.Clear();
                items.Insert(0, new SelectListItem { Text = "Please Select", Value = "0", Selected = true, });
                if (CurrentSession.RoleID == "18")
                {
                    items.Add(new SelectListItem { Text = "LG", Value = "2008", });
                    
                }
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
        //Post: Alter selected record with value

        [HttpPost]

        public JsonResult Update(MCSiteSubMenu SubMenuModel)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var root = "MC/" + SubMenuModel.MCName + "/MCSubMenu/pdf/";
                        bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                        if (!folderpath)
                        {
                            System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
                        }
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        string extension = Path.GetExtension(fileName);
                        string FileName = "UserPhoto" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                        if (extension == ".pdf")
                        {                         
                            FileName = "linkpdf" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                            SubMenuModel.pdf = "MC/" + SubMenuModel.MCName + "/MCSubMenu/pdf/" + FileName;
                        }
                        else
                        {
                            SubMenuModel.icon = "MC/" + SubMenuModel.MCName + "/MCSubMenu/pdf/" + FileName;
                        }
                        //System.Threading.Tasks.Task task = BlobStorage.Savefile(files, root, FileName);
                        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                        //files.SaveAs(path);
                        BlobStorage.Savefile(files, root, FileName);
                    }
                    var data = MCSiteSubMenu.UpdateSubMenuRecordById(SubMenuModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = MCSiteSubMenu.UpdateSubMenuRecordById(SubMenuModel);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }


            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, "Error at File Upload in MCSiteSubMenu");
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Max(int Id)
        {
            MCSiteSubMenu model = new MCSiteSubMenu();
            var data = MCSiteSubMenu.Max(Id);
            model._LstSubMenu = data;
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
            var data = MCSiteSubMenu.DeleteSubMenuRecordById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MenuDropDown(string mcid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                items.Clear();
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@MCId", mcid);

                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetMCHomeMenuforSubmenu]", param).Tables[0].Rows)
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