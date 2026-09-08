using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMNirdesh.Error;
using CMNirdesh.Extensions;
using CMNirdesh.Models;

namespace SBL.eLegistrator.HouseController.Web.Areas.Grievances.Controllers
{
    public class PromiseController : Controller
    {
        //
        // GET: /Grievances/Promise/

   
        public ActionResult PromiseDetails()
        {

            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                else
                {
                    return ((ActionResult)base.View(Promise.GetPromiseList(CurrentSession.DeptID)));
                }
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                ViewBag.ErrorMessage = "Their is a Error while getting  Promises Details";
                return RedirectToAction("Login", "Account", new { @area = "" });
            }

           
        }


        public ActionResult EditPromise(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                var model = Promise.GetPromiseListById(Convert.ToInt32(EncryptionUtility.Decrypt(Id.ToString())));
                model.mode = "Edit";
                model.StatusTypeList = Promise.GetStatusTypes();
                model.DeptId = CurrentSession.DeptID;
                return View("CreatePromise", model);
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                ViewBag.ErrorMessage = "Their is a Error While Getting all Promise Details";
                return View("AdminErrorPage");
            }
        }


        public ActionResult CreatePromise()
        {

            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }

                var model = new Promise()
                {
                    mode = "Add",
                    StatusTypeList = Promise.GetStatusTypes(),
                    isActive = true,
                    DeptId = CurrentSession.DeptID,
                    isDeleted=false,

                };
                return View(model);
            }
            catch (Exception ex)
            {

                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                ViewBag.ErrorMessage = "Their is a Error While Creating Project Details";
                return View("AdminErrorPage");
            }


        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SavePromise(Promise model)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }

                if (model.mode == "Add")
                {
                    model.createdBy = CurrentSession.UserID;
                    model.DeptId = CurrentSession.DeptID;
                    model.isDeleted = false;
                    Promise.SavePromise(model);
                }
                else
                {
                    model.createdBy = CurrentSession.UserID;
                    Promise.SavePromise(model);
                }
                return RedirectToAction("PromiseDetails");
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                ViewBag.ErrorMessage = "Their is a Error While Saving Promise Details";
                return View("AdminErrorPage");
            }
        }


        public ActionResult DeletePromise(string Id)
        {

            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                else
                {
                    Promise.DeletePromiseById(Convert.ToInt32(EncryptionUtility.Decrypt(Id.ToString())));
                    return RedirectToAction("PromiseDetails");
                }
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                ViewBag.ErrorMessage = "Their is a Error While Deleting Promise Details";
                return View("AdminErrorPage");
            }
        }

    }
}
