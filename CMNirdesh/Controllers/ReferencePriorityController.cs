using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMNirdesh.Models;
using Newtonsoft.Json;


namespace CMNirdesh.Controllers
{
    public class ReferencePriorityController : Controller
    {
        // GET: Priority Page List
        public ActionResult Index()
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstReferencePriority model = new LstReferencePriority();
                var data = ReferencePriority.GetReferencePriorityList();
                model._LstReferencePriority = data;
                return View(model);
            }
        }
        
        public ActionResult Create()
        {
            return View(new ReferencePriority());
        }

        // Create: New ReferencePriority Record

        [HttpPost]

        public JsonResult Create(ReferencePriority referencePriority)
        {
            var data = ReferencePriority.SaveReferencePriorityRecord(referencePriority);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        // GET: ReferencePriortity Row By Id
        public JsonResult Edit(int Id)
        {
            LstReferencePriority model = new LstReferencePriority();
            var data = ReferencePriority.GetReferencePriorityById(Id);
            model._LstReferencePriority = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // Update: Reference Priority Row By Selected Id   

        [HttpPost]

        public JsonResult Update(ReferencePriority model)
        {
            var data = ReferencePriority.UpdateReferencePriorityRecordById(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        // Delete: Reference Row By Selected Id
        public JsonResult Delete(int id)
        {
            var data = ReferencePriority.DeleteReferencePriorityRecordById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
