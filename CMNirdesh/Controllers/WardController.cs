using CMNirdesh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CMNirdesh.Controllers
{
    public class WardController : Controller
    {

        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                mConstituency mConstituency = new mConstituency();
                mConstituency.mcid = Convert.ToInt32(CurrentSession.StateId);
                mConstituency.Houselist = Constituencylist.GetHouselist(Convert.ToInt32(mConstituency.mcid));
                mConstituency.WardList = Constituencylist.GetWardList(Convert.ToInt32(mConstituency.mcid));
                mConstituency.Zonelist = Constituencylist.GetZonelist(Convert.ToInt32(mConstituency.mcid));
                return View(mConstituency);
            }
        }


        [HttpPost]
        public ActionResult Create(mConstituency model)
        {
            if (ModelState.IsValid)
            {
                if (model.Mode != "Edit")
                {
                    Constituencylist.SaveConstituency(model);
                    return Json(new { success = true });
                }
                else
                {
                    Constituencylist.UpdateConstituency(model);
                    return Json(new { success = true });
                }
                    
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, errors = errors });
            }
        }


        public ActionResult EditWard(int ConstituencyCode)
        {
            mConstituency model = new mConstituency();

            string value = string.Empty;
            if (ModelState.IsValid)
            {
                var data = Constituencylist.GetConstituencyByCode(ConstituencyCode, CurrentSession.StateId);
                model.WardList = data;
                value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateStatusConstituency(int ConstituencyCode, int AssemblyCode)
        {

            if (ModelState.IsValid)
            {
                Constituencylist.UpdateStatusConstituency(ConstituencyCode, CurrentSession.StateId, AssemblyCode);
            }
            return Json(new { success = true });
        }
    }
}
