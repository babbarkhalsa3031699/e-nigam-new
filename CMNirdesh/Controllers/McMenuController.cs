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
using System.Web.UI.WebControls;


namespace CMNirdesh.Controllers
{
    public class McMenuController : Controller
    {
        // GET: McMenu
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstMCHomeMenuModel model = new LstMCHomeMenuModel();
                var data = MMCHomeMenu.GetMCMenuList(Convert.ToInt32(CurrentSession.StateId));
                model._ListMCHomeMenuModel = data;
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                return View(model);

            }
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[mMCMenuDropDown]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
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

                    //item.Text = Convert.ToString(row["MCName"].ToString());
                    //item.Value = Convert.ToString(row["Id"].ToString());
                    //items.Add(item);

                }
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult Create()
        {
            return View(new MMCHomeMenu());
        }

        // Post: Insert New McMenu Record

        [HttpPost]

        public JsonResult Create(MMCHomeMenu model)
        {
            try
            {
                //if (Request.Files.Count > 0)
                //{
                //    var folderpath = "MC/" + CurrentSession.MCName + "/MCMenu/";
                //    for (int i = 0; i < Request.Files.Count; i++)
                //    {
                //        var files = Request.Files[i];
                //        var fileName = System.IO.Path.GetFileName(files.FileName);
                //        string FileName = "MCMenu" + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "" + DateTime.Now.Year + "" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;

                //        model.icon = folderpath + FileName;
                //        System.Threading.Tasks.Task task = BlobStorage.Savefile(files, folderpath, FileName);
                //    }
                //    var data = MMCHomeMenu.SaveMCRecord(model);
                //    return Json(data, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                    var data = MMCHomeMenu.SaveMCRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                //}

            }

            catch (Exception ex)
            {
                //ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult Create(MMCHomeMenu model)
        //{
        //    string error = "";



        //        try
        //        {
        //            if (Request.Files.Count > 0)
        //            {
        //                var root = "~/SecureFileStructure/MCMenu";
        //                bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
        //                if (!folderpath)
        //                {
        //                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
        //                }
        //                for (int i = 0; i < Request.Files.Count; i++)
        //                {
        //                    var files = Request.Files[i];
        //                    var fileName = System.IO.Path.GetFileName(files.FileName);
        //                    string FileName = "MCMenu" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
        //                    var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);

        //                    files.SaveAs(path);
        //                    model.icon = "SecureFileStructure/MCMenu/" + FileName;

        //                }
        //                var data = MMCHomeMenu.SaveMCRecord(model);
        //                return Json(data, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                var data = MMCHomeMenu.SaveMCRecord(model);
        //                return Json(data, JsonRequestBehavior.AllowGet);
        //            }

        //        }

        //        catch (Exception ex)
        //        {

        //            return Json(0, JsonRequestBehavior.AllowGet);
        //        }
        //    }



        //}



        [HttpGet]
        //GET: Fetch Record Row by selected ID
        public JsonResult Edit(int id)
        {

            LstMCHomeMenuModel model = new LstMCHomeMenuModel();
            var data = MMCHomeMenu.GetMCRecordById(id);
            model._ListMCHomeMenuModel = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        //POST: Update selected row after alteration
        [HttpPost]

        public JsonResult Update(MMCHomeMenu model)
        {
            try
            {
                //if (Request.Files.Count > 0)
                //{
                //    var root = "MC/" + CurrentSession.MCName + "/MCMenu/";
                //    bool folderpath = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                //    if (!folderpath)
                //    {
                //        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
                //    }
                //    for (int i = 0; i < Request.Files.Count; i++)
                //    {
                //        var files = Request.Files[i];
                //        var fileName = System.IO.Path.GetFileName(files.FileName);
                //        string FileName = "MCMenu" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "." + fileName;
                //        var path = System.IO.Path.Combine(HttpContext.Server.MapPath(root), FileName);
                //        //files.SaveAs(path);
                //        //System.Threading.Tasks.Task task = BlobStorage.Savefile(files, root, FileName);
                //        model.icon = "MC/" + CurrentSession.MCName + "/MCMenu/" + FileName;
                //    }
                //    var data = MMCHomeMenu.UpdateMCRecord(model);
                //    return Json(data, JsonRequestBehavior.AllowGet);


                //}
                //else
                //{
                    var data = MMCHomeMenu.UpdateMCRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);

                //}

            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Post: Delete Selected Row Id

        public JsonResult Delete(int Id)
        {
            var data = MMCHomeMenu.DeleteMCRecordById(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Max(int Id)
        {
            MMCHomeMenu model = new MMCHomeMenu();
            var data = MMCHomeMenu.Max(Id);
            model._LstMenu = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
    }

}