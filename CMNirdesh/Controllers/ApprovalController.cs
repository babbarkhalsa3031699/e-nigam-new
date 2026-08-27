using CMNirdesh.Models;
using CMNirdesh.Models.Diaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class ApprovalController : Controller
    {
        // GET: Approval
        [HttpPost]
        [ValidateInput(false)]
       public ActionResult SaveEndorsement(EndorsementApprovalVM model)
        {
            try
            {
                if (CurrentSession.UserID == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Your session has expired. Please login again."
                    });
                }


                bool result = mTypeOfDocument.SaveEndorsement(
                    model,
                    CurrentSession.UserID,
                    CurrentSession.DeptID,
                    CurrentSession.OfficeId);

                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Saved successfully."
                    });
                }

                return Json(new
                {
                    success = false,
                    message = "Unable to save endorsement."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        public ActionResult GetEndorsementList(int agendaId)
        {
            var list = mTypeOfDocument.GetEndorsementList(agendaId);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEndorsementById(int id)
        {
            var data = mTypeOfDocument.GetEndorsementById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveCreateApproval(AgendaApprovalStatusModel model)
        {
            model.UserId = CurrentSession.UserID;
            bool result = mTypeOfDocument.SaveCreateApproval(model);
            return Json(new
            {
                Status = result
            });
        }

        [HttpGet]
        public JsonResult GetCreateApprovalStatus(int AgendaId)
        {
            AgendaApprovalStatusModel model = mTypeOfDocument.GetCreateApprovalStatus(AgendaId);
            return Json(new
            {
                Status = true,
                IsCreateApproval = model.IsCreateApproval
            }, JsonRequestBehavior.AllowGet);
        }
    }
}