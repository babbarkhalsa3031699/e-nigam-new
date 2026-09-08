using CMNirdesh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class AssemblyController : Controller
    {
        // GET: Assembly
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                mAssembly Assembly = new mAssembly();
                Assembly.Houselist = Assemblylist.GetHouselistDetail(Convert.ToInt32(CurrentSession.StateId));
                return View(Assembly);
            }
        }

        [HttpPost]
        public ActionResult Create(mAssembly model)
        {
            if (ModelState.IsValid)
            {
                if (model.Mode != "Edit")
                {
                    Assemblylist.SaveHouse(model);
                }
                else
                {
                    Assemblylist.UpdateHouse(model);
                }
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, errors = errors });
            }
        }
         public ActionResult EditHouse(int AssemblyCode)
         {
            mAssembly model = new mAssembly();
        
            string value = string.Empty;
            if (ModelState.IsValid)
            {
                var data = Assemblylist.GetHouseByCode(AssemblyCode, CurrentSession.StateId);
                model.Houselist = data;
                value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateStatusHouse(int AssemblyCode) {

            if (ModelState.IsValid)
            {
                Assemblylist.UpdateStatusHouse(AssemblyCode, CurrentSession.StateId);
            }
            return Json(new { success = true });
        }
    }
}