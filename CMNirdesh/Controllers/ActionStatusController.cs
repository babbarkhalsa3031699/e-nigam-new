using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMNirdesh.Models.Diaries;
using Newtonsoft.Json;

namespace CMNirdesh.Controllers
{
    public class ActionStatusController : Controller
    {
        // GET: ActionStatusList
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstActionStatus model = new LstActionStatus();
                var data = mTypeOfAction.GetActionsList();
                model._LstActionStatus = data;
                return View(model);
            }
        }

        // GET: ActionStatus/Create
        public ActionResult Create()
        {
            return View(new mTypeOfAction());
        }

        // POST: Insert Action Status Record

        [HttpPost]
        public ActionResult Create(mTypeOfAction mTypeOfAction)
        {
            var data = mTypeOfAction.SaveActionRecord(mTypeOfAction);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Action Record By Id
        public JsonResult Edit(int id)
        {
            LstActionStatus model = new LstActionStatus();
            var data = mTypeOfAction.GetActionRecordById(id);
            model._LstActionStatus = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }


        // POST: Update ActionRecord By ID
        [HttpPost]
        public ActionResult Update(mTypeOfAction mTypeOfAction)
        {
            var data = mTypeOfAction.UpdateActionRecordById(mTypeOfAction);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // POST: Delete Action Record By Id

        public JsonResult Delete(int id)
        {
            var data = mTypeOfAction.DeleteActionRecordById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
