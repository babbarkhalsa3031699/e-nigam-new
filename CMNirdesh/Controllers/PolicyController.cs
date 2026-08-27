using CMNirdesh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class PolicyController : Controller
    {
      
        public ActionResult LGPolicy()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            else
            {
                DataViewModelPolicyList model = new DataViewModelPolicyList();
                var data = DataViewModelPolicy.GetList();
                model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
                model.lgPolicies = data;
                return View(model);
            }
        }

        
        [HttpPost]
        public ActionResult SaveLGPolicy(LgPolicy model1, HttpPostedFileBase PolicyDocument)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            try
            {
                var folderpath = "MC/LG/Policy/";

                if (PolicyDocument != null && PolicyDocument.ContentLength > 0)
                {
                    var ext = Path.GetExtension(PolicyDocument.FileName);
                    var fileName = $"{Guid.NewGuid()}{ext}";

                    BlobStorage.Savefile(PolicyDocument, folderpath, fileName);
                    model1.PolicyDocumentPath = Path.Combine(folderpath, fileName).Replace("\\", "/");
                }

                var data = DataViewModelPolicy.SavePolicy(model1);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult EditLGPolicy(int Id)
        {
            DataViewModelPolicyList model = new DataViewModelPolicyList();
            var data = DataViewModelPolicy.GetRowByIdList(Id);
            model.lgPolicies = data;
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

       
        public JsonResult UpdateLGPolicy(LgPolicy model, HttpPostedFileBase PolicyDocument)
        {
            try
            {
                   var folderpath = "MC/LG/Policy/";
                    if (PolicyDocument != null && PolicyDocument.ContentLength > 0)
                    {
                        var ext = Path.GetExtension(PolicyDocument.FileName);
                        var fileName = $"{Guid.NewGuid()}{ext}";

                        BlobStorage.Savefile(PolicyDocument, folderpath, fileName);
                        model.PolicyDocumentPath = Path.Combine(folderpath, fileName).Replace("\\", "/");
                    }
                    var data = DataViewModelPolicy.UpdatePolicy(model);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(JsonRequestBehavior.AllowGet);
        }

      
        public JsonResult DeleteLGPolicy(int Id)
        {
            var data = DataViewModelPolicy.DeleteRecord(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPolicyCategories()
        {
            DataViewModelPolicy obj = new DataViewModelPolicy();
            var list = obj.GetPolicyCategories();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }

}