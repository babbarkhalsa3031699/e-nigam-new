using CMNirdesh.Models;
using CMNirdesh.Models.MCHouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class HomeLabelController : Controller
    {
        // GET: HomeLabel List

        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstHomeLabelModel model = new LstHomeLabelModel();
                var data = HomeLabel.GetList();
                model._LstHomeLabel = data;
                return View(model);
            }
        }
    

    public ActionResult Create()
    {
        return View(new HomeLabel());
    }

    // Create: New  Record

    [HttpPost]
    public JsonResult Create(HomeLabel model)
    {
        var data = HomeLabel.SaveRecords(model);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

    // GET: Record List By Id
    public JsonResult Edit(int id)
    {
        LstHomeLabelModel model = new LstHomeLabelModel();
        var data = HomeLabel.GetRecordsById(id);
        model._LstHomeLabel = data;
        string value = string.Empty;

        value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        return Json(value, JsonRequestBehavior.AllowGet);
    }

    // Update: Record List By Selected Id   

    [HttpPost]

    public JsonResult Update(HomeLabel model)
    {
        var data = HomeLabel.UpdateRecordsById(model);
        return Json(data, JsonRequestBehavior.AllowGet);

    }

    // Delete: Record List By Selected Id
    public JsonResult Delete(string Id)
    {
        var data = HomeLabel.DeleteRecordById(Id);
        return Json(data, JsonRequestBehavior.AllowGet);
    }

}
}

