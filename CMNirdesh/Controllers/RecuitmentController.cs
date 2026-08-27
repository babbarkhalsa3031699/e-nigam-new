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
    public class RecuitmentController : Controller
    {
        // GET: Recuitment
        public ActionResult Index()
        {
            RecruitmentNotification Recruitment = new   RecruitmentNotification();
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            Recruitment.RecruitmentNotificationList = RecruitmentNotificationModel.GetRecuitmentList();
            return View(Recruitment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecruitmentNotification model, HttpPostedFileBase NotificationFile, HttpPostedFileBase ApplyOfflineFile)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false });

            try
            {
                // Folder path where files will be saved
                var folderpath = "MC/"+ CurrentSession.MCName + "/Recruitment/";

                // Handle NotificationFile upload
                if (NotificationFile != null && NotificationFile.ContentLength > 0)
                {
                    var ext = Path.GetExtension(NotificationFile.FileName);
                    var fileName = string.Format("{0}{1}", Guid.NewGuid(), ext);
                    BlobStorage.Savefile(NotificationFile, folderpath, fileName);
                    model.NotificationFile = Path.Combine(folderpath, fileName);
                }

                // Handle ApplyOfflineFile upload
                if (ApplyOfflineFile != null && ApplyOfflineFile.ContentLength > 0)
                {
                    var ext1 = Path.GetExtension(ApplyOfflineFile.FileName);
                    var fileName1 = string.Format("{0}{1}", Guid.NewGuid(), ext1);
                    BlobStorage.Savefile(ApplyOfflineFile, folderpath, fileName1);
                    model.ApplyOfflineFile = Path.Combine(folderpath, fileName1);
                }
                int result = 0;
                // Set CreatedBy or ModifiedBy
                if (model.Mode == "Add")
                {
                    model.CreatedBy = CurrentSession.UserID;
                    model.IsActive = true;
                    result = RecruitmentNotificationModel.SaveRecruitment(model);
                }
                else
                {
                    model.ModifiedBy = CurrentSession.UserID;
                    //model.ModifiedOn = DateTime.Now; // optional
                    result = RecruitmentNotificationModel.SaveRecruitment(model);
                }

                return RedirectToAction("Index", "Recuitment", new { @area = "" });

            }
            catch (Exception ex)
            {
                // You can log ex.Message here if needed
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult EditRecutmentNotification(int id)
        {
            RecruitmentNotification model = new RecruitmentNotification();

            string value = string.Empty;
            if (ModelState.IsValid)
            {
                var data = RecruitmentNotificationModel.GetRecruitmentNotificationByCode(id);
                model.RecruitmentNotificationList = data;
                value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStatusRecruitment(int Id)
        {

            if (ModelState.IsValid)
            {
                RecruitmentNotificationModel.UpdateStatusRecuitment(Id);
            }
            return Json(new { success = true });
        }

    }
}