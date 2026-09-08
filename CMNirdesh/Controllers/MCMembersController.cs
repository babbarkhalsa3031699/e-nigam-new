using CMNirdesh.Models;
using CMNirdesh.Models.MCHouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CMNirdesh.Controllers
{
    public class MCMembersController : Controller
    {
        //GET: MCMembers List
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstMembers model = new LstMembers();
                var data = MCMembers.MembersListMaster();
                model._LstMembers = data;
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                model._CommitteeType = MCMembers.GetCommitteeTypes();
                model._LstDesignation = MCMembers.GetMemberDesignation("E");
                model._LstDesignationlocal = MCMembers.GetMemberDesignation("P");
                model.Houses = MCMembers.FetchHouses();

                model.MemberCodeOld = 0;
                if (data.Count > 0)
                {
                    model.MemberCodeOld = data[0].MemberCodeOld;
                }
                return View(model);
            }
        }
        public ActionResult Create()
        {
            LstMembers model = new LstMembers();
            var data = MCMembers.MemberRecordGetById(0);
            model._LstMembers = data;
            model._CommitteeType = MCMembers.GetCommitteeTypes();
            return View(new MCMembers());
        }
        // Post: Insert New MCMembers Record

        [HttpPost]

        public JsonResult Create(MCMembers model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var folderpath = "MC/" + model.MCName + "/Members/";
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var ext = System.IO.Path.GetExtension(files.FileName);
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        fileName = "MemberPhoto" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "_" + fileName;
                        BlobStorage.Savefile(files, folderpath, fileName);
                        model.FileName = fileName;
                        model.FilePath = folderpath;

                    }
                    var data = MCMembers.MemberRecordSave(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = MCMembers.MemberRecordSave(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {

                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        //GET: Fetch Record Row by selected ID
        public JsonResult Edit(int id)
        {

            LstMembers model = new LstMembers();
            var data = MCMembers.MemberRecordGetById(id);
            model._LstMembers = data;
            model._CommitteeType = MCMembers.GetCommitteeTypes();
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        //POST: Update selected row after alteration
        [HttpPost]

        public JsonResult Update(MCMembers model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var folderpath = "MC/" + model.MCName + "/Members/";
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var files = Request.Files[i];
                        var fileName = System.IO.Path.GetFileName(files.FileName);
                        fileName = "MemberPhoto" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.Second + "_" + Convert.ToString(DateTime.Now.Millisecond) + "_" + fileName;
                        BlobStorage.Savefile(files, folderpath, fileName);
                        model.FileName = fileName;
                        model.FilePath = folderpath;
                    }
                    var data =MCMembers.MemberRecordUpdate(model);
                    return Json(data, JsonRequestBehavior.AllowGet);


                }
                else
                {
                    var data = MCMembers.MemberRecordUpdate(model);
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
            var data = MCMembers.MemberRecordDeleteById(Id);
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetMC]", new object[0]).Tables[1].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    if (CurrentSession.RoleID=="18")
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
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult WardDropDown(int mcid)
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
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@MCId", mcid);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetConstituencybyMC]", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ConstituencyName"].ToString());
                    item.Value = Convert.ToString(row["ConstituencyCode"].ToString());
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
        public JsonResult PartyDropDown()
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetParty]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["PartyName"].ToString());
                    item.Value = Convert.ToString(row["partyid"].ToString());
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
        public JsonResult StateDropDown()
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
                int islocal = 0;
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@IsLocal", islocal);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetmStates]", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["StateName"].ToString());
                    item.Value = Convert.ToString(row["StateCode"].ToString());
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
        public JsonResult DistrictDropDown(int statecode)
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
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@statecode", statecode);
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetStateWiseDistricts]", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["DistrictName"].ToString());
                    item.Value = Convert.ToString(row["DistrictCode"].ToString());
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

        [HttpPost]
        public JsonResult CallSearchedMCMembers(int assemblyId = 0)
        {
            List<MCMembers> data = MCMembers.MembersListMaster(true, assemblyId);
            //if (assemblyId.HasValue && assemblyId.Value > 0)
            //{
            //    var wards = Constituencylist.GetWardList(Convert.ToInt32(CurrentSession.StateId));
            //    var validWardCodes = new HashSet<int>(wards.Where(w => w.AssemblyID == assemblyId.Value).Select(w => w.ConstituencyCode));
            //    data = data.Where(m => validWardCodes.Contains(m.Ward)).ToList();
            //}
            var response = new
            {
                members = data,
                fileacessingUrl = BlobStorage.GetStorageAcessingPath()
            };
            return Json(response, "Value", JsonRequestBehavior.AllowGet);
        }
    }
}
    


        