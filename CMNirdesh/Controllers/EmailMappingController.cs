using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CMNirdesh.Controllers
{
    public class EmailMappingController : Controller
    {
        // GET: EmailMapping
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                EmailUserMappingModel usermapping = new EmailUserMappingModel();
                string DeptId = CurrentSession.DeptID;
                usermapping.Users = EmailUserVM.GetUsersByDepartment(DeptId);
                return View(usermapping);
            }
        }

        [HttpPost]
        public JsonResult MapUser(string emailId, string userId, string Name, string Mobile)
        {

            if (string.IsNullOrWhiteSpace(emailId) || string.IsNullOrWhiteSpace(userId))
            {
                return Json(new { success = false, message = "Invalid data" });
            }
            try
            {
                bool isAlreadyMapped = EmailUserVM.IsUserAlreadyMapped(userId);
                if (isAlreadyMapped)
                {
                    return Json(new
                    {
                        success = false,
                        code = "ALREADY_MAPPED",
                        message = "This user is already mapped. Please unmap it first, then try again."
                    });
                }

                EmailUserVM.MapUser(emailId, userId, Name, Mobile);
                return Json(new
                {
                    success = true,
                    message = "User mapped successfully."
                });
            }
            catch (Exception ex)
            {
                // log exception if required
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetMappedUsers()
        {
            var data = EmailUserVM.GetMappedUsers();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ToggleMapping(int mappingId, bool isActive)
        {
            try
            {
                EmailUserVM.ToggleMapping(mappingId, isActive);
                return Json(new { success = true, message = "Update Sucessfully" });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Unable to update mapping"
                });
            }
        }
    }
}