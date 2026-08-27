using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
   // [Authorize] // optional but recommended
    public class DepartmentUserController : Controller
    {
        /* =========================
           LOAD PAGE
           ========================= */
        public ActionResult Index()
        {
            var model = new UpdateUserDetailModel
            {
                Users = EmailUserVM.GetUsersByDepartment(CurrentSession.DeptID),
                _LstUser = GetUsersDepartment()
            };

            return View(model);
        }

        public List<UserModel> GetUsersDepartment()
        {
            List<UserModel> lstusr = new List<UserModel>();
            DataSet ds = UserModelFunction.GetMCWiseUserList();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string userName = dr["UserName"].ToString();

                // ✅ FILTER (exclude punjab.gov.in)
                if (userName.ToLower().Contains("@punjab.gov.in"))
                    continue;

                UserModel fr = new UserModel();
                fr.UserName = userName;
                fr.Name = dr["name"].ToString();
                fr.Designation = dr["Designation"].ToString();
                fr.Email = dr["EmailId"].ToString();
                fr.MobileNo = dr["MobileNo"].ToString();

                lstusr.Add(fr);
            }

            return lstusr;
        }

        /* =========================
           GET USER DETAILS (OPTIONAL – AJAX)
           ========================= */
        public JsonResult GetUserDetails(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return Json(null, JsonRequestBehavior.AllowGet);

            var user = EmailUserVM.GetUserDetails(
                userId,
                CurrentSession.DeptID
            );

            return Json(user, JsonRequestBehavior.AllowGet);
        }

       

        /* =========================
           UPDATE USER (FORM POST)
           ========================= */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(UpdateUserDetailModel model)
        {
            // 🔒 Server-side validation
            if (!ModelState.IsValid)
            {
                model.Users = EmailUserVM.GetUsersByDepartment(CurrentSession.DeptID);
                model._LstUser = GetUsersDepartment();
                return View("Index", model);
            }

            try
            {
                EmailUserVM.UpdateUserProfile(
                    model,
                    CurrentSession.DeptID,
                    Convert.ToString(CurrentSession.UserID)
                );

                TempData["Success"] = "User details updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.Users = EmailUserVM.GetUsersByDepartment(CurrentSession.DeptID);
                model._LstUser = GetUsersDepartment();
                return View("Index", model);
            }
        }
    }
}
