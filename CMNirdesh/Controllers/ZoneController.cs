using CMNirdesh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class ZoneController : Controller
    {
        // GET: Zone
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Zone zn = new Zone();
                zn.Zoneslist = Zonelist.GetZonelistDetail(CurrentSession.StateId);
                return View(zn);
            }
        }

        [HttpPost]
        public ActionResult Create(Zone model)
        {
            if (ModelState.IsValid)
            {
                if (model.Mode != "Edit")
                {
                    Zonelist.SaveZone(model);
                }
                else
                {
                    Zonelist.UpdateZone(model);
                }
                return Json(new { success = true });
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, errors = errors });
            }
        }

        public ActionResult EditZone(int ZoneID)
        {
            Zone model = new Zone();

            string value = string.Empty;
            if (ModelState.IsValid)
            {
                var data = Zonelist.GetZoneCode(ZoneID, CurrentSession.StateId);
                model.Zoneslist = data;
                value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateStatusZone(int ZoneId, int Active)
        {

            if (ModelState.IsValid)
            {
                Zonelist.UpdateStatusZone(ZoneId, CurrentSession.StateId, Active);
            }
            return Json(new { success = true });
        }


    }
}