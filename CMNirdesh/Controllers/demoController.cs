using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class demoController : Controller
    {
        // GET: demo
        public async Task<ActionResult> SignPdfNoStampingV3(string AgendaId)
        {
            AgendaDetailModel agendaDetailModel = new AgendaDetailModel();
            string Blobpath = AgendaModelFunction.GetAgendaPdf(Convert.ToInt32(AgendaId));
            byte[] fileBytes = await BlobStorage.GetFileFromAzureAsync(Blobpath);
            string fileBase64 = Convert.ToBase64String(fileBytes);
            agendaDetailModel.AgendaId = Convert.ToInt32(AgendaId);
            agendaDetailModel.agendadetails = fileBase64;
            return View(agendaDetailModel);
        }
        public async Task<ActionResult> SignApprovals(string Blobpath, string AgendaId, int ApprovalId)
        {
            AgendaDetailModel agendaDetailModel = new AgendaDetailModel();
            string Blobpath1 = AgendaModelFunction.GetAgendaPdf(Convert.ToInt32(AgendaId));
            byte[] fileBytes = await BlobStorage.GetFileFromAzureAsync(Blobpath);
            string fileBase64 = Convert.ToBase64String(fileBytes);
            agendaDetailModel.AgendaId = Convert.ToInt32(AgendaId);
            agendaDetailModel.ApprovalId = ApprovalId;
            agendaDetailModel.agendadetails = fileBase64;
            agendaDetailModel.attachfile= Blobpath;
            Session["ApprovalId"] = ApprovalId;
            Session["AgendaId"] = AgendaId;
            return View("SignApprovals", agendaDetailModel);
        }
      public async Task<ActionResult> SignPdfCommissioner(string AgendaId)
      {
            AgendaDetailModel agendaDetailModel = new AgendaDetailModel();
            string Blobpath = AgendaModelFunction.GetCommissionerPdf(Convert.ToInt32(AgendaId));
            byte[] fileBytes = await BlobStorage.GetFileFromAzureAsync(Blobpath);
            string fileBase64 = Convert.ToBase64String(fileBytes);
            agendaDetailModel.AgendaId = Convert.ToInt32(AgendaId);
            agendaDetailModel.agendadetails = fileBase64;
            return View(agendaDetailModel);
        }
      }
   }
