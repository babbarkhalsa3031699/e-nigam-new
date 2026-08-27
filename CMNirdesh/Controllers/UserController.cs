
using CMNirdesh.Models;
using CMNirdesh.Models.Session;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using System.IO;
using CMNirdesh.Filters;
using CMNirdesh.Error;
using ErrorLog = CMNirdesh.Error.ErrorLog;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Web.Routing;
using System.Web;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Web.Security;
using System.Drawing.Imaging;
using System.Drawing;
using Azure;
using CMNirdesh.Models.Diaries;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace CMNirdesh.Controllers
{
    [Audit]
    [NoCache]
    [CMNirdeshAuthorize(Allow = "Authenticated")]
    public class UserController : Controller
    {

        // [MenuAuthorization("User")]
        public ActionResult CreateUser()
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            UserModel model = new UserModel();

            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                model.mDepartmentList = DiaryViewModel.getRoleWiseDepartment();
                model.mDepartmentList = DiaryViewModel.getRoleWiseDepartment();
                model.RoleList = DiaryViewModel.getuserRoles();
                model.OfficeList = DiaryViewModel.GetOfficeByDept(model.DeptId, CurrentSession.OfficeId);
                model._LstUser = GetUsers();
                ViewBag.OfficeName = CurrentSession.OfficeName;
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }

        // GET: Fetch Record Row by selected ID
        public JsonResult Edit(string id)
        {

            UserModel model = new UserModel();
            model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
            var data = UserModelFunction.GetUserDetailRowById(id);
            model._LstUser = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveUser(UserModel mdl)
        {
            mdl.fileacessingUrl = BlobStorage.GetStorageAcessingPath();

            HttpPostedFileBase filephoto = Request.Files["filephoto"];
            HttpPostedFileBase filesignature = Request.Files["filesignature"];


            string photopath = "MC/" + CurrentSession.MCName + "/Users/Pic/";
            string signpath = "MC/" + CurrentSession.MCName + "/Users/Sign/";
            //Photo
            if (mdl.PhotoFileName != null)
            {
                if (FileValidation.IsValidImageUpload(filephoto))
                {
                    HttpPostedFileBase photoFile = Request.Files["filephoto"];
                    if (photoFile != null && photoFile.ContentLength > 0)
                    {
                        string photoFileName = "UserPhoto" + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(photoFile.FileName);
                        BlobStorage.Savefile(photoFile, photopath, photoFileName);
                        mdl.PhotoLocation = photopath+ photoFileName;
                    }
                }
                else
                {

                    UserModel model1 = new UserModel();
                    ModelState.AddModelError("file", "Invalid Photo file Name, format or content. Please Try Again");
                    mdl.mDepartmentList = DiaryViewModel.getDepartment();
                    mdl.OfficeList = DiaryViewModel.GetOfficeByDept(Convert.ToString(model1.DeptId), CurrentSession.OfficeId);
                    mdl._LstUser = GetUsers();
                    model1.RoleList = DiaryViewModel.getuserRoles();
                    ViewBag.OfficeName = CurrentSession.OfficeName;
                    ViewBag.Error = "Invalid file Name, format or content. Please Try Again";
                    return View("CreateUser", mdl);
                }
            }
            else
            {
                mdl.PhotoLocation = "";
            }
            //Sign
            if (mdl.SignFileName != null)
            {
                if (FileValidation.IsValidImageUpload(filesignature))
                {
                    HttpPostedFileBase signFile = Request.Files["filesignature"];
                    if (signFile != null && signFile.ContentLength > 0)
                    {
                        string signFileName = "UserSign" + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(signFile.FileName);
                        string signFilePath = Path.Combine(signpath, signFileName);
                        //signFile.SaveAs(signFilePath);
                        BlobStorage.Savefile(signFile, signpath, signFileName);
                        mdl.SignLocation = signpath+signFileName;
                    }
                }
                else
                {

                    UserModel model1 = new UserModel();
                    ModelState.AddModelError("file", "Invalid Sign file Name, format or content. Please Try Again");
                    mdl.mDepartmentList = DiaryViewModel.getDepartment();
                    mdl.OfficeList = DiaryViewModel.GetOfficeByDept(Convert.ToString(model1.DeptId), CurrentSession.OfficeId);
                    mdl._LstUser = GetUsers();
                    model1.RoleList = DiaryViewModel.getuserRoles();
                    ViewBag.OfficeName = CurrentSession.OfficeName;
                    ViewBag.Error = "Invalid file Name, format or content. Please Try Again";
                    return View("CreateUser", mdl);
                }

            }
            else
            {
                mdl.SignLocation = "";
            }

            UserModel model = new UserModel();
            model.DeptId = mdl.DeptId;
            model.OfficeId = mdl.OfficeId;
            model.Name = mdl.Name;
            model.UserName = mdl.UserName;
            var pwd = mdl.Password.Substring(0, 64);

            model.Password = pwd;
            model.Designation = mdl.Designation;
            model.Email = mdl.Email;
            model.MobileNo = mdl.MobileNo;
            model.IsActive = 1;
            model.Apex = mdl.Apex;
            model.RoleId = mdl.RoleId;
            model.Photo = mdl.PhotoLocation;
            model.Signature = mdl.SignLocation;
            //bool isValidMail = mdl.IsValidEmail(mdl.UserName);
            bool isValidMail = true;
            bool isOnlyLower = mdl.UserName.Any(c => Char.IsUpper(c));

            if (isOnlyLower)
            {
                model.UserName = null;
                ViewBag.Error = "User Name should be Lower Case";

            }
            if (!isValidMail)
            {
                ViewBag.Error = "The user name should be the valid Email Address, such as user@punjab.gov.in.";
            }
            else
            {
                string msg = UserModelFunction.SaveUser(model);
                if (msg == "1")
                {
                    Session["userExist"] = "User Already Exist";
                    Session["userAdded"] = "";
                }
                else
                {
                    Session["userAdded"] = "User Added Successfully.";
                    Session["userExist"] = "";
                    ModelState.Clear();
                }
                ViewBag.Error = "";
            }
            mdl.mDepartmentList = DiaryViewModel.getDepartment();
            mdl.OfficeList = DiaryViewModel.GetOfficeByDept(model.DeptId, CurrentSession.OfficeId);
            mdl.RoleList = DiaryViewModel.getuserRoles();
            mdl._LstUser = GetUsers();
            ViewBag.OfficeName = CurrentSession.OfficeName;
            return View("CreateUser", mdl);

        }
        [HttpPost]
        public ActionResult UpdateUser(UserModel mdl)
        {
            try
            {

                //HttpPostedFileBase filephoto1 = Request.Files["file"];
                //HttpPostedFileBase filesignature1 = Request.Files["file2"];


                HttpPostedFileBase filephoto = Request.Files["file"];
                HttpPostedFileBase filesignature = Request.Files["file2"];


              


                if (true)
                {
                    var fileAcessingSettings = CMNirdesh.Models.FileSetting.GetDISFileSetting();
                    //fileValidation.IsImageFileExtensionValid(mdl.PhotoFileName) && fileValidation.IsImageFileTypeValid(filephoto) &&
                  //fileValidation.HasImageDoubleFileExtension(mdl.PhotoFileName) && fileValidation.IsImageFileExtensionValid(mdl.SignFileName) && fileValidation.IsImageFileTypeValid(filesignature) &&
                 // fileValidation.HasImageDoubleFileExtension(mdl.SignFileName) && fileValidation.IsInvalidName(filephoto.FileName) && fileValidation.IsInvalidName(filesignature.FileName)



                    string photopath = "MC/" + CurrentSession.MCName + "/Users/Pic/";
                    string signpath = "MC/" + CurrentSession.MCName + "/Users/Sign/";

                    if (mdl.PhotoFileName != null)
                    {
                        HttpPostedFileBase photoFile = Request.Files["file"];
                        if (photoFile != null && photoFile.ContentLength > 0)
                        {
                            string photoFileName = "UserPhoto" + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(photoFile.FileName);
                            //string photoFilePath = Path.Combine(photopath, photoFileName);
                            // photoFile.SaveAs(photoFilePath);
                            BlobStorage.Savefile(photoFile, photopath, photoFileName);
                            mdl.PhotoLocation = photopath + photoFileName;//photopath+ photoFileName
                        }
                        else
                        {
                            mdl.PhotoLocation = "";
                        }
                    }

                    // Sign
                    if (mdl.SignFileName != null)
                    {
                        HttpPostedFileBase signFile = Request.Files["file2"];
                        if (signFile != null && signFile.ContentLength > 0)
                        {
                            string signFileName = "UserSign" + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(signFile.FileName);
                            string signFilePath = Path.Combine(signpath, signFileName);
                            //signFile.SaveAs(signFilePath);
                            BlobStorage.Savefile(signFile, signpath, signFileName);
                            mdl.SignLocation = signpath+ signFileName;
                        }
                        else
                        {
                            mdl.SignLocation = "";
                        }
                    }

                    //mdl.Password = CMNirdesh.Models.Login.ComputeHash(mdl.Password, "SHA1", null);
                    Session["userExist"] = "";
                    Session["userAdded"] = "";
                    var data = UserModelFunction.UpdateUserById(mdl);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = "Invalid file Name, format or content. Please Try Again";
                    //return View("CreateUser", mdl);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here or log the error
                // For example:
                Console.WriteLine("An error occurred during user update: " + ex.Message);
                return Json(new { value = 0, message = "An error occurred during user update." }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Delete(string UserId)
        {
            var data = UserModelFunction.DeleteUserRecordById(UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public List<UserModel> GetUsers()
        {
            List<UserModel> lstusr = new List<UserModel>();
            DataSet ds = new DataSet();
            ds = UserModelFunction.GetMCWiseUserList();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                UserModel fr = new UserModel();

                fr.UserId = dr["userId"].ToString();
                fr.UserName = dr["UserName"].ToString();
                fr.DepartmentName = Convert.ToString(dr["deptname"]);
                fr.OfficeName = dr["officename"].ToString();
                fr.Name = dr["name"].ToString();
                fr.Designation = dr["Designation"].ToString();
                fr.Email = dr["EmailId"].ToString();
                fr.MobileNo = dr["MobileNo"].ToString();
                fr.RoleName = dr["Role"].ToString();
                fr.RoleId = 1;
                fr.Photo = dr["Photo"].ToString();
                fr.Signature = dr["SignaturePath"].ToString();
                fr.Apex = false;
                if (dr["Apex"] != System.DBNull.Value)
                {
                    fr.Apex = Convert.ToBoolean(dr["Apex"]);
                }
                lstusr.Add(fr);
            }
            return lstusr;

        }

        private static void ConvertBase64StringToBytes(string base64String, ref byte[] bytes1, ref string Type)
        {
            string[] split = base64String.Split(',');
            bytes1 = System.Convert.FromBase64String(base64String.Substring(base64String.IndexOf(',') + 1));
            if (base64String != "")
            {
                split = base64String.Split(',');
                Type = split[0];

            }
        }

        [HttpPost]
        public JsonResult AddOfficeDesignation(string userid, string officeid, string designation)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = UserModelFunction.AddOfficeDesignation(userid, Convert.ToInt32(officeid), designation);
                //var dtt = dvm.GetTableRows(data);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetOfficebyDepartmentId(string deptid)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetOfficebyDepartmentId(deptid);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetUserbyOfficeId(string deptid, string officeid = "0")
        {
            try
            {
                if (officeid == "")
                {
                    officeid = "0";
                }
                DiaryViewModel dvm = new DiaryViewModel();
                var data = UserModelFunction.GetUserbyOfficeId(deptid, officeid);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetUserbyOfficeIdForSend(string deptid, string officeid="0")
        {
            try
            {
                if (officeid == "")
                {
                    officeid = "0";
                }
                DiaryViewModel dvm = new DiaryViewModel();
                var data = UserModelFunction.GetUserbyOfficeIdForSend(deptid, officeid);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetUserOffices(string UserId)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = UserModelFunction.GetUserOffices(UserId);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetAllUser(string act)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = UserModelFunction.GetMCWiseUserList();
                var dtt = dvm.GetTableRows(data.Tables[0]);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult Permission()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }

            PermissionModel model = new PermissionModel();
            model.mDepartmentList = DiaryViewModel.getRoleWiseDepartment();
            model.OfficeList = UserModelFunction.getOfficeSelectList(model.DeptId);
            model.DesOfficeList = UserModelFunction.getOfficeSelectListbyUser(model.UserId);
            model.UserList = UserModelFunction.getUserbyOfficeSelectList(model.DeptId, model.OfficeId.ToString());
            return View(model);
        }


        public ActionResult GetPermissionList(PermissionModel mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            PermissionModel model = new PermissionModel();
            try
            {
                string userId = mdl.UserId;
                DataSet ds = new DataSet();
                ds = UserModelFunction.GetUserbyUserId(userId);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (mdl.DeptId == null)
                    {
                        mdl.DeptId = ds.Tables[0].Rows[0]["deptId"].ToString();
                        mdl.OfficeId = Convert.ToInt32(ds.Tables[0].Rows[0]["OfficeId"].ToString());

                    }
                    model.DepartmentName = ds.Tables[0].Rows[0]["deptname"].ToString();
                    model.OfficeName = ds.Tables[0].Rows[0]["officename"].ToString();
                    //model.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["ApprovalAuthority"].ToString());
                    model.IsActive = Convert.ToInt32(ds.Tables[0].Rows[0]["ApprovalAuthority"]) == 1;
                    model.IsOpen = true;
                }

                model.DeptId = mdl.DeptId;
                model.OfficeId = mdl.OfficeId;
                model.mDepartmentList = DiaryViewModel.getDepartment();
                model.OfficeList = UserModelFunction.getOfficeSelectList(mdl.DeptId);
                model.DesOfficeList = UserModelFunction.getOfficeSelectListbyUser(mdl.UserId);
                model.MultiOfficeList = UserModelFunction.getOfficeSelectList(mdl.DeptId);
                model.UserList = UserModelFunction.getUserbyOfficeSelectList(mdl.DeptId, mdl.OfficeId.ToString());

                model.MenuList = PopulateMenu();
                model.References = UserModelFunction.getReferences(model.DeptId, model.OfficeId.ToString());
                model.FundReferences = UserModelFunction.getFundReferences(model.DeptId, model.OfficeId.ToString());


                model.DispatchReferences = UserModelFunction.getReferencesDispatch();


                model.Actions = UserModelFunction.getActions();
                model.ActionFiles = UserModelFunction.getActionsFile();

                model.ActionsDispatch = UserModelFunction.getActionsDisptach();
                model.ActionsFileDispatch = UserModelFunction.getActionsFileDisptach();



                model.addupdateList = UserModelFunction.PopulateButtonMenu(userId);
                model.DiaryButton = UserModelFunction.getDiaryUpdateList(userId);
                model.DispatchButton = UserModelFunction.getDispatchUpdateList(userId);
                model.ProjectActions = UserModelFunction.getProjectActions(userId);
                //model.FundButton = UserModelFunction.getFundButtons();
                //model.mDepartmentList = DiaryViewModel.getDepartment();
                //model.RoleList = UserModelFunction.getRoles();
                //model.UserList = UserModelFunction.getUserSelectList();
                //model.OfficeList = DiaryViewModel.GetOfficeByDept(model.DeptId, CurrentSession.OfficeId);
                ViewBag.OfficeName = CurrentSession.OfficeName;
                DataSet pds = new DataSet();
                pds = UserModelFunction.GetPermission(userId);
                if (pds.Tables[0].Rows.Count > 0)
                {
                    model.menuids = pds.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    model.menuids = "";
                }

                if (pds.Tables[1].Rows.Count > 0)
                {
                    model.refids = pds.Tables[1].Rows[0][1].ToString();
                }
                else
                {
                    model.refids = "";
                }

                if (pds.Tables[2].Rows.Count > 0)
                {
                    model.actionids = pds.Tables[2].Rows[0][1].ToString();
                }
                else
                {
                    model.actionids = "";
                }

                if (pds.Tables[3].Rows.Count > 0)
                {
                    model.addupdateids = pds.Tables[3].Rows[0][1].ToString();
                }
                else
                {
                    model.addupdateids = "";
                }

                if (pds.Tables[4].Rows.Count > 0)
                {
                    model.depids = pds.Tables[4].Rows[0][0].ToString();
                }
                else
                {
                    model.depids = "";
                }

                if (pds.Tables[5].Rows.Count > 0)
                {
                    model.officeids = pds.Tables[5].Rows[0][1].ToString();
                }
                else
                {
                    model.officeids = "";
                }


                if (pds.Tables[6].Rows.Count > 0)
                {
                    model.DispatchRefids = pds.Tables[6].Rows[0][1].ToString();
                }
                else
                {
                    model.DispatchRefids = "";
                }

                if (pds.Tables[7].Rows.Count > 0)
                {
                    model.Dispatchactionids = pds.Tables[7].Rows[0][1].ToString();
                }
                else
                {
                    model.Dispatchactionids = "";
                }

                if (pds.Tables[8].Rows.Count > 0)
                {
                    model.Diaryaddupdateids = pds.Tables[8].Rows[0][1].ToString();
                }
                else
                {
                    model.Diaryaddupdateids = "";
                }

                if (pds.Tables[9].Rows.Count > 0)
                {
                    model.Dispatchaddupdateids = pds.Tables[9].Rows[0][1].ToString();
                }
                else
                {
                    model.Dispatchaddupdateids = "";
                }
                if (pds.Tables[10].Rows.Count > 0)
                {
                    model.Projectaddupdateids = pds.Tables[10].Rows[0][1].ToString();
                }
                else
                {
                    model.Projectaddupdateids = "";
                }


                return View("Permission", model);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View("Permission", model);
            }

        }



        public ActionResult AddMenu(string UserId, int MenuId)
        {
            UserModelFunction.AddMenu(UserId, MenuId);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveMenu(string UserId, int MenuId)
        {
            UserModelFunction.RemoveMenu(UserId, MenuId);
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddDocument(string UserId, int DocTypeId, string type)
        {
            UserModelFunction.AddUserDocument(UserId, DocTypeId, type);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveDocument(string UserId, int DocTypeId, string type)
        {
            UserModelFunction.RemoveUserDocument(UserId, DocTypeId, type);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddAction(string UserId, int ActionId, string type)
        {
            UserModelFunction.AddUserAction(UserId, ActionId, type);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveAction(string UserId, int ActionId, string type)
        {
            UserModelFunction.RemoveUserAction(UserId, ActionId, type);
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddButton(string UserId, int ButtonId)
        {
            UserModelFunction.AddUserButton(UserId, ButtonId);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveButton(string UserId, int ButtonId)
        {
            UserModelFunction.RemoveUserButton(UserId, ButtonId);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddStatusButton(string UserId, int ButtonId, string type)
        {
            UserModelFunction.AddUserStatusButton(UserId, ButtonId, type);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveStatusButton(string UserId, int ButtonId, string type)
        {
            UserModelFunction.RemoveUserStatusButton(UserId, ButtonId, type);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Active(string UserId, int act)
        {
            UserModelFunction.ActiveUser(UserId, act);
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddOffice(string UserId, int OfficeId, string DeptId)
        {
            UserModelFunction.AddOffice(UserId, OfficeId, DeptId);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveOffice(string UserId, int OfficeId, string DeptId)
        {
            UserModelFunction.RemoveOffice(UserId, OfficeId);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        private static List<MenuModel> PopulateMenu()
        {
            List<MenuModel> menu = new List<MenuModel>();
            DataSet ds = new DataSet();
            string userid = CurrentSession.UserID;
            ds = UserModelFunction.GetMenuList();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                MenuModel fr = new MenuModel();

                fr.MenuId = Convert.ToInt32(dr["Id"].ToString());
                fr.MenuName = dr["MenuName"].ToString();
                fr._LstSubmenu = PopulateSubMenu(fr.MenuId);
                menu.Add(fr);

            }
            //menu.Add(new MenuModel
            //{
            //    //MenuName = sdr["FruitName"].ToString(),
            //    //MenuId = Convert.ToInt32(sdr["FruitId"])
            //});

            return menu;
        }

        private static List<SubMenuModel> PopulateSubMenu(int menuId)
        {
            List<SubMenuModel> submenu = new List<SubMenuModel>();
            DataSet ds = new DataSet();
            ds = UserModelFunction.GetSubMenuList(menuId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                SubMenuModel fr = new SubMenuModel();

                fr.SubMenuId = Convert.ToInt32(dr["SubMenuId"].ToString());
                fr.SubMenuName = dr["SubMenuName"].ToString();
                fr.ControllerName = dr["ControllerName"].ToString();
                fr.ActionName = dr["ActionName"].ToString();
                fr.Type = Convert.ToInt32(dr["Type"].ToString());
                submenu.Add(fr);

            }
            //menu.Add(new MenuModel
            //{
            //    //MenuName = sdr["FruitName"].ToString(),
            //    //MenuId = Convert.ToInt32(sdr["FruitId"])
            //});

            return submenu;
        }


        private static List<MenuModel> PopulateMenuByUser()
        {
            List<MenuModel> menu = new List<MenuModel>();
            DataSet ds = new DataSet();
            string userid = CurrentSession.UserID;
            ds = UserModelFunction.GetMenuList(userid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    MenuModel fr = new MenuModel();

                    fr.MenuId = Convert.ToInt32(dr["Id"].ToString());
                    fr.MenuName = dr["MenuName"].ToString();
                    fr._LstSubmenu = PopulateSubMenubyUser(fr.MenuId, userid);
                    foreach (var item in fr._LstSubmenu)
                    {
                        if(item.SubMenuId==5101 || item.SubMenuId == 5102 || item.SubMenuId == 5104)
                        {
                            item.Count=0;
                        }
                        else
                        {
                            fr.MenuCount = fr.MenuCount + item.Count;
                        }
                        
                    }
                    fr.icon = dr["icon"].ToString();
                    
                    menu.Add(fr);

                }
            }

            //menu.Add(new MenuModel
            //{
            //    //MenuName = sdr["FruitName"].ToString(),
            //    //MenuId = Convert.ToInt32(sdr["FruitId"])
            //});

            return menu;
        }

        private static List<SubMenuModel> PopulateSubMenubyUser(int menuId, string userid)
        {
            List<SubMenuModel> submenu = new List<SubMenuModel>();
            DataSet ds = new DataSet();
            ds = UserModelFunction.GetSubMenuListByUser(menuId, userid, Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                SubMenuModel fr = new SubMenuModel();

                fr.SubMenuId = Convert.ToInt32(dr["SubMenuId"].ToString());
                fr.SubMenuName = dr["SubMenuName"].ToString();
                fr.ControllerName = dr["ControllerName"].ToString();
                fr.ActionName = dr["ActionName"].ToString();
                fr.Type = Convert.ToInt32(dr["Type"].ToString());
                fr.Count= Convert.ToInt32(dr["TotalCount"].ToString());
                if (fr.Type > 0 || menuId == 21) {
                    ////DiaryViewModel dvm = new DiaryViewModel();
                    ////DataTable dtinboxcount = new DataTable();
                    ////dtinboxcount = dvm.SubmenuInboxCount(Convert.ToInt32(fr.Type),Convert.ToString(CurrentSession.UserID),Convert.ToInt32(CurrentSession.MapId));
                    ////if(dtinboxcount.Rows.Count > 0)
                    ////{
                    ////    int RefCount = Convert.ToInt32(dtinboxcount.Rows[0]["InboxRef"].ToString());
                    ////    int FileCount = Convert.ToInt32(dtinboxcount.Rows[0]["InboxFile"].ToString());
                    ////    fr.Count = RefCount+ FileCount;
                    ////}
                    ////else
                    ////{
                    ////    fr.Count = 0;
                    ////}

                    //DataSet menuds = UserModelFunction.Getmenutype(Convert.ToInt32(fr.Type));
                    //int FileCount = 0;
                    //if (menuds.Tables[0].Rows.Count > 0)
                    //{

                    //DataTable dtinbox = new DataTable();
                    //DataTable dtinboxfile = new DataTable();
                    //int RefMenutype = Convert.ToInt32(menuds.Tables[0].Rows[0]["DocumentTypeId"].ToString());
                    //dtinbox = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.MapId), RefMenutype.ToString());
                    //if (menuds.Tables[1].Rows.Count > 0)
                    //{
                    //    int FileMenuType = Convert.ToInt32(menuds.Tables[1].Rows[0]["DocumentTypeId"].ToString());
                    //    dtinboxfile = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.MapId), FileMenuType.ToString());
                    //    FileCount= dtinboxfile.Rows.Count;
                    //}
                    //else
                    //{
                    //    FileCount = 0;
                    //}

                    //fr.Count = dtinbox.Rows.Count + FileCount;
                    //}

                    //else
                    //{
                    //    fr.Count = 0;
                    //}
                }
                else
                {
                    fr.Count = 0;
                }
                submenu.Add(fr);

            }


            return submenu;
        }


        public PartialViewResult Menu()
        {
            var model = new UserMenu();
            model.MenuList = PopulateMenuByUser();
            return PartialView(model);
        }

        public PartialViewResult MenuMobile()
        {
            var model = new UserMenu();
            model.MenuList = PopulateMenuByUser();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult UrlCheck(string type = "0")
        {
            var request = HttpContext.Request;
            string userid = CurrentSession.UserID;
            string controller = request.RequestContext.RouteData.Values["controller"].ToString();
            string action = request.RequestContext.RouteData.Values["action"].ToString();
            if (action == "GetPermissionList")
            {
                action = "Permission";
            }
            if (action == "SaveFundReceipt")
            {
                action = "FundReceipt";
            }
            if (action == "SaveAgenda")
            {
                action = "CreateAgenda";
            }
            if (action == "PromiseDetails")
            {
                action = "PromiseDetails";
            }


            string hasPermission = UserModelFunction.CheckPermission(controller, action, userid, type);
            if (hasPermission == "0")
            {
                //return View("Error");
                return PartialView();
            }
            else
            {
                return PartialView();
            }

        }



        private static List<AlertModel> GetMessages()
        {
            List<AlertModel> alerts = new List<AlertModel>();
            DataSet ds = new DataSet();
            string deptId = CurrentSession.DeptID;
            string OfficeId = CurrentSession.OfficeId;
            ds = UserModelFunction.GetAlerts(deptId, Convert.ToInt32(OfficeId), CurrentSession.UserID, 0);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    AlertModel fr = new AlertModel();
                    fr.NotificationId = Convert.ToInt32(dr["NotificationId"].ToString());
                    fr.refId = Convert.ToInt64(dr["RefId"].ToString());
                    fr.alert = dr["alert"].ToString().Replace("<.*?>", String.Empty);
                    fr.alertType = dr["alerttype"].ToString();
                    fr.DocType = dr["DocumentTypeName"].ToString();
                    fr.createdDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd/MM/yyyy hh:mm tt");
                    fr.UserName = dr["UserName"].ToString();
                    fr.NotificationType = Convert.ToInt32(dr["NotificationType"].ToString());
                    fr.IsLetter = dr["isLetter"].ToString();
                    fr.NotificationIcon = dr["icon"].ToString();
                    alerts.Add(fr);
                }
            }

            return alerts;
        }
        private static List<AlertModel> GetAlerts()
        {
            List<AlertModel> alerts = new List<AlertModel>();
            DataSet ds = new DataSet();
            string deptId = CurrentSession.DeptID;
            string OfficeId = CurrentSession.OfficeId;
            ds = UserModelFunction.GetAlerts(deptId, Convert.ToInt32(OfficeId), CurrentSession.UserID,1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    AlertModel fr = new AlertModel();
                    fr.NotificationId = Convert.ToInt32(dr["NotificationId"].ToString());
                    fr.refId = Convert.ToInt64(dr["RefId"].ToString());
                    fr.alert = dr["alert"].ToString().Replace("<.*?>", String.Empty);
                    fr.alertType = dr["alerttype"].ToString();
                    fr.DocType = dr["DocumentTypeName"].ToString();
                    fr.createdDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd/MM/yyyy hh:mm tt");
                    fr.UserName = dr["UserName"].ToString();
                    fr.NotificationType = Convert.ToInt32(dr["NotificationType"].ToString());
                    fr.IsLetter = dr["isLetter"].ToString();
                    fr.NotificationIcon = dr["icon"].ToString();
                    alerts.Add(fr);
                }
            }

            return alerts;
        }

        public PartialViewResult Alerts()
        {
            var model = new Alerts();
            model.MessageList = GetMessages();
            model.MessageCount = model.MessageList.Count().ToString();

            model.alertList = GetAlerts();
            model.alertCount = model.alertList.Count().ToString();
            return PartialView(model);
        }


        public ActionResult ClearAlerts(string clearalert)
        {
            string deptId = CurrentSession.DeptID;
            string OfficeId = CurrentSession.OfficeId;
            UserModelFunction.ClearAlerts(deptId, Convert.ToInt32(OfficeId), CurrentSession.UserID, clearalert);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClearAlertsbyId(string NotiId)
        {
            UserModelFunction.ClearAlertsbyId(Convert.ToInt32(NotiId));
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult getUserProfileDetail()
        {
            var dtt = UserModelFunction.GetUserDetailRowById(CurrentSession.UserID);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        //////////////////
        public ActionResult ChangePassword(LoginModel model)
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            model.UserName = CurrentSession.UserName;

            return View("ChangePassword", model);

        }
        [HttpPost]
        public ActionResult SaveUserNewPassword(LoginModel model, string CaptchaText)
        {
            try
            {
                if (Session["CaptchaImageText"] != null)
                {
                    if (this.Session["CaptchaImageText"].ToString() != CaptchaText)
                    {
                        model.ErrorMessage = "Captcha Validation Failed!";
                        return View("ChangePassword", model);
                    }
                }
                if (model.ActualPassword == null)
                {

                    model.ErrorMessage = "Please Enter Old Password";
                    return View("ChangePassword", model);

                }
                if (model.ActualPassword == model.Password)
                {
                    model.ErrorMessage = "New Password can not be Old Password, Please try password";
                }
                if (model.Password == null)
                {

                    model.ErrorMessage = "Please Enter New Password";
                    return View("ChangePassword", model);

                }
                if (model.confirmPassword == "")
                {

                    model.ErrorMessage = "Confirm Password first";
                    return View("ChangePassword", model);

                }
                bool s = LoginModel.IsPreviousUser(model.Password, CurrentSession.UserName);
                if (s)
                {
                    model.ErrorMessage = "This Password used previously by the User, Please Try another password";
                    return View("ChangePassword", model);

                }

                var oldpwd = model.ActualPassword.Substring(0, 64);


                mUsers user = new mUsers();
                user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(CurrentSession.UserName);
                if (user.mUserList != null && user.mUserList.Count > 0)
                {
                    Console.WriteLine(user.mUserList);
                    foreach (var list in user.mUserList)
                    {

                        if (CMNirdesh.Models.Login.VerifyHash(oldpwd, "SHA256", list.Password) == true)
                        {
                            string username = CurrentSession.UserName;
                            string newPassword = model.Password.Substring(0, 64);

                            //newPassword = CMNirdesh.Models.Login.ComputeHash(newPassword, "SHA1", null);

                            string res = CMNirdesh.Models.Login.changeUserLoginPwd(username, newPassword);
                            if (res == "Y")
                            {
                                string[] credentials = { username, model.Password };
                                SMSMessage _smsService = new SMSMessage();
                                //Task<string> task = _smsService.SendLoginMessageMultipleVariables(list.MobileNo, credentials, "1407170659322347791");


                                //SMSMessage _smsService = new SMSMessage();
                                //Task<string> asyncTask = _smsService.SendUserPasswordChangeMsg(list.MobileNo,username, model.Password);
                                //task.Wait();

                                TempData["ChngPwdSucessMsg"] = "Password changed successfully.Please login again with new credentials";
                                return RedirectToAction("Login", "Account");
                            }
                            model.ErrorMessage = "Invalid Username!";
                        }
                    }
                }
                model.ErrorMessage = "There is some error!";
                return View("ChangePassword", model);
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "There is some error!";
                return View("ChangePassword", model);
            }
        }


        public FileResult GetCaptchaImage()
        {
            CaptchaRandomImage CI = new CaptchaRandomImage();
            this.Session["CaptchaImageText"] = GetCaptchaText();
            CI.GenerateImage(this.Session["CaptchaImageText"].ToString(), 250, 50, Color.Black, Color.White);
            MemoryStream stream = new MemoryStream();
            CI.Image.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "image/png");
        }

        public string GetCaptchaText()
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        //Role Wise Permission
        public ActionResult RoleWisePermission()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }

            PermissionModel model = new PermissionModel();
            model.UserList = UserModelFunction.getUserTypes();
            return View(model);
        }
        public ActionResult GetRoleWisePermissionList(PermissionModel mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            PermissionModel model = new PermissionModel();
            try
            {
                int UserTypeId = mdl.UserTypeId;
                model.UserTypeId = mdl.UserTypeId;
                model.UserList = UserModelFunction.getUserTypes();
                model.IsActive = true;
                model.IsOpen = true;
                model.MenuList = PopulateMenu();
                model.References = UserModelFunction.getReferences(model.DeptId, model.OfficeId.ToString());
                model.FundReferences = UserModelFunction.getFundReferences(model.DeptId, model.OfficeId.ToString());
                model.DispatchReferences = UserModelFunction.getReferencesDispatch();
               


                model.Actions = UserModelFunction.getActions();
                model.ActionFiles = UserModelFunction.getActionsFile();

                model.ActionsDispatch = UserModelFunction.getActionsDisptach();
                model.ActionsFileDispatch = UserModelFunction.getActionsFileDisptach();

                model.addupdateList = UserModelFunction.PopulateUserTypeButtonMenu(UserTypeId);
                model.DiaryButton = UserModelFunction.getUserTypeDiaryUpdateList(UserTypeId);
                model.DispatchButton = UserModelFunction.getUserTypeDispatchUpdateList(UserTypeId);
                model.ProjectActions = UserModelFunction.getUserTypeProjectActions(UserTypeId);

               

                ViewBag.OfficeName = CurrentSession.OfficeName;
                DataSet pds = new DataSet();
                pds = UserModelFunction.GetUserTypePermission(UserTypeId);
                if (pds.Tables[0].Rows.Count > 0)
                {
                    model.menuids = pds.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    model.menuids = "";
                }

                if (pds.Tables[1].Rows.Count > 0)
                {
                    model.Diaryactionids = pds.Tables[1].Rows[0][1].ToString();
                }
                else
                {
                    model.Diaryactionids = "";
                }

                if (pds.Tables[2].Rows.Count > 0)
                {
                    model.Dispatchactionids = pds.Tables[2].Rows[0][1].ToString();
                }
                else
                {
                    model.Dispatchactionids = "";
                }
                if (pds.Tables[3].Rows.Count > 0)
                {
                    model.Diaryaddupdateids = pds.Tables[3].Rows[0][1].ToString();
                }
                else
                {
                    model.Diaryaddupdateids = "";
                }

                if (pds.Tables[4].Rows.Count > 0)
                {
                    model.Dispatchaddupdateids = pds.Tables[4].Rows[0][1].ToString();
                }
                if (pds.Tables[5].Rows.Count > 0)
                {
                    model.refids = pds.Tables[5].Rows[0][1].ToString();
                }
                else
                {
                    model.refids = "";
                }

                if (pds.Tables[6].Rows.Count > 0)
                {
                    model.DispatchRefids = pds.Tables[6].Rows[0][1].ToString();
                }
                else
                {
                    model.DispatchRefids = "";
                }

                if (pds.Tables[7].Rows.Count > 0)
                {
                    model.Projectaddupdateids = pds.Tables[7].Rows[0][1].ToString();
                }
                else
                {
                    model.Projectaddupdateids = "";
                }

                return View("RoleWisePermission", model);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View("RoleWisePermission", model);
            }

        }

        public ActionResult AddUserTypeMenu(string UserTypeId, int MenuId)
        {
            UserModelFunction.UpdateUserTypeMenu(UserTypeId, MenuId, "S");
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveUserTypeMenu(string UserTypeId, int MenuId)
        {
            UserModelFunction.UpdateUserTypeMenu(UserTypeId, MenuId, "D");
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddUserTypeRefAction(string UserTypeId, int DocTypeId, string type)
        {
            if (type == "Diary")
            {
                UserModelFunction.UpdateUserTypeDiaryRefAction(UserTypeId, DocTypeId, "S");
            }
            else if (type == "Dispatch")
            {
                UserModelFunction.UpdateUserTypeDispatchRefAction(UserTypeId, DocTypeId, "S");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveUserTypeRefAction(string UserTypeId, int DocTypeId, string type)
        {
            if (type == "Diary")
            {
                UserModelFunction.UpdateUserTypeDiaryRefAction(UserTypeId, DocTypeId, "D");
            }
            else if (type == "Dispatch")
            {
                UserModelFunction.UpdateUserTypeDispatchRefAction(UserTypeId, DocTypeId, "D");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddUserTypeAction(string UserTypeId, int ActionId, string type)
        {
            if (type == "Diary")
            {
                UserModelFunction.UpdateUserTypeDiaryAction(UserTypeId, ActionId, "S");
            }
            else if (type == "Dispatch")
            {
                UserModelFunction.UpdateUserTypeDispatchAction(UserTypeId, ActionId, "S");
            }
            else if (type == "Project")
            {
                UserModelFunction.UpdateUserTypeProjectAction(UserTypeId, ActionId, "S");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveUserTypeAction(string UserTypeId, int ActionId, string type)
        {
            if (type == "Diary")
            {
                UserModelFunction.UpdateUserTypeDiaryAction(UserTypeId, ActionId, "D");
            }
            else if (type == "Dispatch")
            {
                UserModelFunction.UpdateUserTypeDispatchAction(UserTypeId, ActionId, "D");
            }
            else if (type == "Project")
            {
                UserModelFunction.UpdateUserTypeProjectAction(UserTypeId, ActionId, "D");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddUserTypeAddUpdateAction(string UserTypeId, int ActionId, string type)
        {
            if (type == "Diary")
            {
                UserModelFunction.UpdateUserTypeDiaryAddUpdateAction(UserTypeId, ActionId, "S");
            }
            else if (type == "Dispatch")
            {
                UserModelFunction.UpdateUserTypeDispatchAddUpdateAction(UserTypeId, ActionId, "S");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult RemoveUserTypeAddUpdateAction(string UserTypeId, int ActionId, string type)
        {
            if (type == "Diary")
            {
                UserModelFunction.UpdateUserTypeDiaryAddUpdateAction(UserTypeId, ActionId, "D");
            }
            else if (type == "Dispatch")
            {
                UserModelFunction.UpdateUserTypeDispatchAddUpdateAction(UserTypeId, ActionId, "D");
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }



        public ActionResult UserTransfer()
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            PermissionModel model = new PermissionModel();

            try
            {
              
                model.UserList = UserModelFunction.getAllUserSelectList();
                ViewBag.OfficeName = CurrentSession.OfficeName;
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }


        public ActionResult GetUserInbox(PermissionModel mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            PermissionModel model = new PermissionModel();
            try
            {
                string userId = mdl.UserId;
                DataSet ds = new DataSet();
                DataTable dtinboxfile = new DataTable();
                model.UserList = UserModelFunction.getAllUserSelectList();
                if (mdl.refids != "")
                {
                    List<SendInboxJson> indexTable = new List<SendInboxJson>();
                    indexTable = new JavaScriptSerializer().Deserialize<List<SendInboxJson>>(mdl.refids);
                    int i = 1;
                    foreach (SendInboxJson index in indexTable)
                    {
                        int order = i;
                        int MoveId = Convert.ToInt32(index.MoveId);
                        Int64 refid = Convert.ToInt64(index.RefId);
                        string NewUserId = mdl.NewUserId;
                        string OldUserId = mdl.UserId;
                        DiaryViewModel.MovementTransfer(refid, MoveId,OldUserId,NewUserId, CurrentSession.UserID);
                        i++;
                    }
                }

                return View("UserTransfer", model);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View("UserTransfer", model);
            }

        }



    }
}
