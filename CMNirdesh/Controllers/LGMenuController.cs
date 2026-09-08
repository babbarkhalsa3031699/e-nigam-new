using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class LGMenuController : Controller
    {
       
        [HttpGet]
        public ActionResult CreateMenu()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            LGMenuDataList model = new LGMenuDataList
            {
                _LstLGMenu = LGMenuDataViewModel.GetAllMenus()
            };
            return View(model);
        }


        [HttpPost]
        public JsonResult CreateMenu(LGMenuViewModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.MenuTitle) || string.IsNullOrWhiteSpace(model.MenuType))
                {
                    return Json(new { success = false, message = "Menu Title and Type are required." });
                }

                // If internal menu, ensure Controller and Action are set
                if (model.MenuType == "internal")
                {
                    if (string.IsNullOrWhiteSpace(model.ControllerName) || string.IsNullOrWhiteSpace(model.ActionName))
                    {
                        return Json(new { success = false, message = "Controller and Action are required for internal menus." });
                    }

                    model.ExternalUrl = null;
                }
                else if (model.MenuType == "external")
                {
                    if (string.IsNullOrWhiteSpace(model.ExternalUrl))
                    {
                        return Json(new { success = false, message = "External URL is required for external menus." });
                    }

                    model.ControllerName = null;
                    model.ActionName = null;
                }

             
                model.CreatedDate = DateTime.Now;

                bool result = LGMenuDataViewModel.InsertMenu(model);

                if (result)
                {
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Failed to save menu." });
            }
            catch (Exception ex)
            {
                // Log ex as needed
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }


        [HttpPost]
        public JsonResult UpdateMenu(LGMenuViewModel model)
        {
            try
            {
                if (model.MenuId <= 0 || string.IsNullOrWhiteSpace(model.MenuTitle) || string.IsNullOrWhiteSpace(model.MenuType))
                    return Json(new { success = false, message = "Invalid data." });

                if (model.MenuType == "internal")
                {
                    if (string.IsNullOrWhiteSpace(model.ControllerName) || string.IsNullOrWhiteSpace(model.ActionName))
                        return Json(new { success = false, message = "Controller and Action are required." });

                    model.ExternalUrl = null;
                }
                else if (model.MenuType == "external")
                {
                    if (string.IsNullOrWhiteSpace(model.ExternalUrl))
                        return Json(new { success = false, message = "External URL is required." });

                    model.ControllerName = null;
                    model.ActionName = null;
                }

                bool result = LGMenuDataViewModel.UpdateMenu(model);

                return Json(new { success = result, message = result ? "" : "Failed to update." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        public JsonResult DeleteMenu(int id)
        {
            try
            {
              
                bool result = LGMenuDataViewModel.DeleteMenu(id);
                if (result)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Delete failed." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
