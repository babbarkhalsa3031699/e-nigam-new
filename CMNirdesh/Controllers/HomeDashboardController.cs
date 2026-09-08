using CMNirdesh.Models;
using CMNirdesh.Models.MCHouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class HomeDashBoardController : Controller
    {
        // GET: tFlashNews
        public ActionResult Index()
        {
            //if (string.IsNullOrEmpty(CurrentSession.UserID))
            //{
            //    return RedirectToAction("Login", "Account", new { @area = "" });
            //}
            //else
            //{
            HomeDashBoard model = new HomeDashBoard();
            var data = HomeDashBoard.GetList();
            model._LstHomeDashBoard = data;
            return View(model);
            //}
        }

        public ActionResult Create()
        {
            return View(new HomeDashBoard());
        }

        // Create: New Department Record

        [HttpPost]
        public JsonResult Create(HomeDashBoard model)
        {
            var data = HomeDashBoard.InsertRecord(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Department List By Id
      

        // Update: Department List By Selected Id   

        [HttpPost]

        public JsonResult Update(HomeDashBoard model)
        {

            var data = HomeDashBoard.UpdateRecord(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        // Delete: Department List By Selected Id
        public JsonResult Delete(int Id)
        {
            var data = HomeDashBoard.DeleteRecord(Id);
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





        public ActionResult UploadMettingDocument()
        {
            HouseMeetingsDocuments model = new HouseMeetingsDocuments();
            var meetingTypes = new List<SelectListItem>
            {
               new SelectListItem { Value = "1", Text = "House Meeting" },
               new SelectListItem { Value = "2", Text = "F&CC" },
             };
            model.Mode = "Add";
            model.MeetingTypesList = meetingTypes;
            model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
            model.HouseList = AgendaModelFunction.GetAssemblyList(Convert.ToString(CurrentSession.StateId));
            model._houseDocuments = HouseMeetingsDocuments.GetMeetingsDocumentsList(Convert.ToInt16(CurrentSession.StateId));
            return View(model);
        }
    
        [HttpPost]

        public ActionResult UploadDocument(HouseMeetingsDocuments model, HttpPostedFileBase file)
        {
            string Savefilepath = "";
            string dateString = model.MeetingDate;
            string datefolder = "";
            string NewDate = "";
            string msg = "";
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                int day = parsedDate.Day;
                int month = parsedDate.Month;
                int year = parsedDate.Year;
                datefolder = Convert.ToString(Convert.ToString(day) + Convert.ToString(month) + Convert.ToString(year));
                NewDate = Convert.ToString(Convert.ToString(day) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year));
            }

            var SateID = CurrentSession.MCName;
         
            if (file != null && file.ContentLength > 0)
            {
                string subfolder;
                if (model.MeetingType == "1")
                {
                    subfolder = "House";
                }
                else
                {
                    subfolder = "FNCC";
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filepath = "MC/" + SateID + "/HouseProceeding/" + subfolder + "/" + Convert.ToInt16(model.HouseNo) + "/" + datefolder +"/";
                Savefilepath = Path.Combine(filepath, fileName);
                BlobStorage.Savefile(file, filepath, fileName);
            }
            else
            {
                Savefilepath = model.FilePath;
            }

            model.FilePath = Savefilepath;
            model.MeetingDate = NewDate;
            model.MCID = Convert.ToInt16(CurrentSession.StateId);
           
            if (model.Mode == "Add")
            {
                msg = HouseMeetingsDocuments.SaveNewMeeting(model);
            }
            if (model.Mode == "Edit")
            {
                msg = HouseMeetingsDocuments.UpdateNewMeeting(model);
            }
            if (msg == "0")
            {
                Session["userAdded"] = "New Proceeding Added Successfully.";
                Session["userExist"] = "";

            }
            else if (msg == "00")
            {
                Session["userAdded"] = "Proceeding Updated Successfully.";
                Session["userExist"] = "";

            }
            else
            {
                Session["userExist"] = "Proceeding not added";
                Session["userAdded"] = "";

            }
            return RedirectToAction("UploadMettingDocument");
        }


        public ActionResult EditMettingDocument(int MeetingId)
        {
            HouseMeetingsDocuments model = new HouseMeetingsDocuments();
            model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
            string value = string.Empty;
            if (ModelState.IsValid)
            {
                model.Mode = "Edit";
                var data = HouseMeetingsDocuments.GetMeetingsDocumentById(MeetingId);
                model._houseDocumentsbyId = data;

                value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(value, JsonRequestBehavior.AllowGet);
            }

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteMettingDocument(int Id)
        {
            if (ModelState.IsValid)
            {
                HouseMeetingsDocuments.DeleteMettingDocument(Id);
            }
            return Json(new { success = true });
        }





    }
}

