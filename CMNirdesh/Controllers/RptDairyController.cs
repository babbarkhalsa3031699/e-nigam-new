using CMNirdesh.Filters;
using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMNirdesh.Models.PaperLaid;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Drawing;
using CMNirdesh.Models.Diaries;
using FundReportModel = CMNirdesh.Models.Diaries.FundReportModel;
using System.Data;
using System.Text;
using CMNirdesh.Error;
using ErrorLog = CMNirdesh.Error.ErrorLog;

using SelectPdf;

using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web.Security;
using System.Data.SqlClient;

namespace CMNirdesh.Controllers
{
    [Audit]
    [NoCache]
    [CMNirdeshAuthorize(Allow = "Authenticated")]
    public class RptDairyController : Controller
    {
        // GET: Fetch  of Records
        public ActionResult Index()
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                else
                {
                    return ((ActionResult)base.View(RptDiary.GetUserWiseStatusDetails(CurrentSession.UserID,"Dispatch")));
                }
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                ViewBag.ErrorMessage = "Their is a Error while getting  User Wise Status  Details";
                return RedirectToAction("Login", "Account", new { @area = "" });
            }

        }   



    }
}
