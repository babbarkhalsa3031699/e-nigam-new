using CMNirdesh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class AuthorityTypeController : Controller
    {
        // GET: AuthorityType
        public ActionResult Index()
        {
            AuthorityTypeList model = new AuthorityTypeList();
 model._LstAuthorityType = AuthorityTypeModel.AuthorityTypeList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View(new AuthorityType());
        }

        [HttpPost]
        public JsonResult Create(AuthorityType model)
        {
            try
            {
                model.CreatedBy = CurrentSession.UserID;
                model.CreatedDate = DateTime.Now;
                model.IsActive = true;

                var result = AuthorityTypeModel.SaveAuthorityType(model);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Edit(int id)
        {
            var model = AuthorityTypeModel.GetAuthorityTypeById(id);

            string json = JsonConvert.SerializeObject(model, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(AuthorityType model)
        {
            try
            {
                model.ModifiedBy = CurrentSession.UserID;
                model.ModifiedDate = DateTime.Now;

                var result = AuthorityTypeModel.UpdateAuthorityType(model);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(int id)
        {
            var result = AuthorityTypeModel.DeleteAuthorityType(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}