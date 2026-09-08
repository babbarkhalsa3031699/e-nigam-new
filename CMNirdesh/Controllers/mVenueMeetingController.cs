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
    public class mVenueMeetingController : Controller
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
                MeetingVenueModel model = new MeetingVenueModel();
                var data = MeetingVenueModel.GetMeetingVenue();
                model._MeetingVenueList = data;
                return View(model);

            }
        }

        public ActionResult Create()
        {
            return View(new MeetingVenueModel());
        }

        // Create: New Menu Record

        [HttpPost]

        public JsonResult Create(MeetingVenueModel model)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var data = MeetingVenueModel.SaveMeetingVenueRecord(model);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = MeetingVenueModel.SaveMeetingVenueRecord(model);
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


            MeetingVenueModel model = new MeetingVenueModel();
            var data = MeetingVenueModel.GetMeetingVenueModelRowById(id);
            model._MeetingVenueList = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // Update: Menu List By Selected Id   

        [HttpPost]

        public JsonResult Update(MeetingVenueModel model)
        {
            int val;
            
            try
            {
                val = 2;
                var data = MeetingVenueModel.UpdateMeetingVenueRecordById(model, Convert.ToString(val));
                return Json(data, JsonRequestBehavior.AllowGet);
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
            var data = MeetingVenueModel.DeleteMeetingVenueRowById(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
