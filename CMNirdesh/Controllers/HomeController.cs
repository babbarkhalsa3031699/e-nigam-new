using CMNirdesh.DTO;
using CMNirdesh.Models;
using System.IO;

using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web.Security;


namespace CMNirdesh.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            Session.Abandon();
            CurrentSession.ClearSessions();
            return View();
        }

        [HttpGet]
        public ActionResult Index(String encrString, string status)
        {
            Console.WriteLine(encrString);

            encrString = Request.QueryString["string"];

            Console.WriteLine(encrString);

            if ( encrString == null) 
            {

            FormsAuthentication.SignOut();
            Session.Abandon();
            CurrentSession.ClearSessions();
            Session["LoggedIn"] = "no";
            Session["HomeButton"] = "no";
                return View();
            }
            else
            {
                Session["LoggedIn"] = "no";

                var statuss = hsFunc(encrString);
                var newstatus = decrptFunc(statuss);
               
                string email = newstatus.data.signature.email;
                CurrentSession.UserID = newstatus.data.signature.user_id;
                CurrentSession.UserName = newstatus.data.signature.fullName;
                CurrentSession.ServiceSessionID = newstatus.data.signature.sessionId;
                CurrentSession.browserId = newstatus.data.signature.browserId;
                CurrentSession.localTokenId = newstatus.data.signature.localTokenId;
               
                if (email!="")
                {

                    mUsers user = new mUsers();
                    user.AadarId = email;
                    CurrentSession.AadharId = user.AadarId;
                    user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(email);

                    if (user.mUserList != null && user.mUserList.Count > 0)
                    {
                        foreach (var list in user.mUserList)
                        {
                            
                            email = list.UserName;
                            FormsAuthentication.SetAuthCookie(list.UserName, false);
                            CurrentSession.UserID = list.UserId.ToString();
                            CurrentSession.UserName = list.UserName;
                            CurrentSession.SubUserTypeID = list.UserType.ToString();
                            CurrentSession.IsMember = list.IsMember;
                            CurrentSession.OfficeId = list.OfficeId.ToString();
                            CurrentSession.OfficeLevel = list.OfficeLevel;
                            CurrentSession.OfficeId = list.OfficeId.ToString();
                            CurrentSession.isApex = list.isApex.ToString();
                        }
                    }
                    else
                    {
                        FormsAuthentication.SignOut();
                        CurrentSession.ClearSessions();
                    }



                    AccountController accountController = new AccountController();
                    accountController.SetSessions();
                    Session["LoggedIn"] = "yes";

                    return RedirectToAction("Index", "References");
                }
                else
                {
                    return View(newstatus);
                }

                
                
            }

          
            
        }



            public ActionResult Login()
        {
            return View();
        }


        public ActionResult Logout()
        {
            long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string Service = "eNigam";
            string userName = CurrentSession.UserName;
            string sessionId = CurrentSession.ServiceSessionID;
            string browserId = CurrentSession.browserId;
            string localTokenId = CurrentSession.localTokenId;
            string hmacString = "Parichay" + timeStamp + "https://parichay.nic.in/pnv1/salt/api/client/logout" + userName + Service + sessionId;
            string requestBody = "{\"HmacString\":\"" + hmacString + "\"}";
            string HMAC_URL = "http://localhost:8085/hmac";

            using (var client = new HttpClient())
            {
                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseTask = client.PostAsync(HMAC_URL, content).Result;

                if (responseTask.StatusCode == HttpStatusCode.OK)
                {
                    ParichayClass resposedata = JsonConvert.DeserializeObject<ParichayClass>(responseTask.Content.ReadAsStringAsync().Result);

                    if (resposedata?.data?.signature != null)
                    {

                       

                        String logOutURL1 = "https://parichay.nic.in/logoutAll?userName=" + userName + "&service=" + Service + "&sessionId=" + sessionId + "&browserId=" + browserId + "&localTokenId=" + localTokenId;
                        FormsAuthentication.SignOut();
                        Session.Abandon();
                        CurrentSession.ClearSessions();
                        HttpCookie reqCookies = Request.Cookies["Login"];
                        if (reqCookies != null)
                        {
                            HttpCookie nameCookie = Request.Cookies["Login"];
                            nameCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(nameCookie);
                        }


                        return Redirect(logOutURL1);
                    }
                }

                return View();
            }


    

        }

        //public ActionResult parichaylogout()
        //{
           

        //    long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //    string Service = "eNigam";
        //    string userName = CurrentSession.UserName;
        //    string sessionId = CurrentSession.ServiceSessionID;

        //    sessionTimeout();


        //    string hmacString ="eNigam"+timeStamp+"https://parichay.nic.in/pnv1/salt/api/client/logout"+userName+Service+sessionId;
        //    string requestBody = "{\"HmacString\":\"" + hmacString + "\"}";
        //    string HMAC_URL = "http://localhost:8083/hmac";

        //    using (var client = new HttpClient())
        //     {
        //        var content = new StringContent(requestBody);
        //        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var responseTask = client.PostAsync(HMAC_URL, content).Result;

        //        if (responseTask.StatusCode == HttpStatusCode.OK)
        //        {
        //            ParichayClass resposedata = JsonConvert.DeserializeObject<ParichayClass>(responseTask.Content.ReadAsStringAsync().Result);

        //            if (resposedata?.data?.signature != null)
        //            {


        //                String logOutURL = "https://parichay.nic.in/pnv1/salt/api/client/logout?userName=" + userName + "&service=" + Service + "&sessionId=" + sessionId + "&tid=" + timeStamp + "&cs=" + resposedata.data.signature;
        //                FormsAuthentication.SignOut();
        //                Session.Abandon();
        //                CurrentSession.ClearSessions();
        //                HttpCookie reqCookies = Request.Cookies["Login"];
        //                if (reqCookies != null)
        //                {
        //                    HttpCookie nameCookie = Request.Cookies["Login"];
        //                    nameCookie.Expires = DateTime.Now.AddDays(-1);
        //                    Response.Cookies.Add(nameCookie);
        //                }

                      
        //                return Redirect(logOutURL);
        //            }
        //        }

        //        return View();
        //    }

          

        //  }

        public ActionResult parichaylogout()
        {
            long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string Service = "eNigam";
            string userName = CurrentSession.UserName;
            string sessionId = CurrentSession.ServiceSessionID;

            sessionTimeout();


            // string hmacString = "eNigam" + timeStamp + "https://parichay.nic.in/pnv1/salt/api/client/logout" +"/"+ userName + "/" + Service + "/" + sessionId;

            string hmacString = "Parichay" + timeStamp + "https://parichay.nic.in/pnv1/salt/api/client/logout"  + userName  + Service  + sessionId;
            string requestBody = "{\"HmacString\":\"" + hmacString + "\"}";
            string HMAC_URL = "http://localhost:8085/hmac";

            using (var client = new HttpClient())
            {
                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseTask = client.PostAsync(HMAC_URL, content).Result;

                if (responseTask.StatusCode == HttpStatusCode.OK)
                {
                    ParichayClass resposedata = JsonConvert.DeserializeObject<ParichayClass>(responseTask.Content.ReadAsStringAsync().Result);

                    if (resposedata?.data?.signature != null)
                    {
                        string logOutURL = $"https://parichay.nic.in/pnv1/salt/api/client/logout?&userName={userName}&service={Service}&sessionId={sessionId}&tid={timeStamp}&cs={resposedata.data.signature}";
                        FormsAuthentication.SignOut();
                        Session.Abandon();
                        CurrentSession.ClearSessions();
                        HttpCookie reqCookies = Request.Cookies["Login"];
                        if (reqCookies != null)
                        {
                            HttpCookie nameCookie = Request.Cookies["Login"];
                            nameCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(nameCookie);
                        }


                        return Redirect(logOutURL);
                    }
                }

                return View();
            }



        }
        public ActionResult GetProjectByFilter(string DeptId, string promiseId)
        {
            Models.PriorityProjectList plist = new Models.PriorityProjectList();
            plist = GetProjectData.GetPrjectDataByFilter(DeptId, Convert.ToInt64(promiseId));
            return PartialView("/Views/PriorityProjects/_ProjectByFilter.cshtml", plist);
        }

        public ActionResult BindocImage(Int64 ProjectId, string ProjectName, string Remark)
        {
            Session["DocumentListalert"] = "";
            Session["ImageListalert"] = "";
            Session["ProjectId"] = "";
            Session["ProjectName"] = "";
            Session["ProjectId"] = ProjectId;
            Session["ProjectName"] = ProjectName;
            GetProjectData doc = new GetProjectData();
            PromiseDocument model = new PromiseDocument();
            model = doc.getDocImage(ProjectId, ProjectName, Remark);
            model.IsImage = true;
            if (model.DocumentList.Count < 1)
            {
                Session["DocumentListalert"] = "Documents Not Attached with the Project.";
            }
            else {
                Session["DocumentListalert"] = null;
            }
            if (model.ImageList.Count < 1)
            {
                model.IsImage = false;
                Session["ImageListalert"] = "Images Not Attached with the Project.";
            }
            else {
                Session["ImageListalert"] = null;
            }
            return PartialView("/Views/PriorityProjects/_ProjectDocImage.cshtml", model);

        }

       


        [Obsolete]
        public void GetPrint()
        {

            DiaryDispatchModel mdl = new DiaryDispatchModel();
            DataTable ds = mdl.PromisesPrintDataN();
            StringBuilder body = new StringBuilder();
            PromiseNameDetail fr = new PromiseNameDetail();
            body = body.Append("<div class='row col-md-12' style='text-align:center;'><h2> Priority Projects</h2> </div>");
            if (ds.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr1 in ds.Rows)
                {
                    fr.PromiseName = dr1["PromiseName"].ToString();
                    fr.Dept_Name = dr1["deptname"].ToString();
                    if (i == 0)
                        fr.Dept_Id = Convert.ToString(dr1["DeptId"]);
                    if (fr.Dept_Id == Convert.ToString(dr1["DeptId"]) || fr.PromiseId != Convert.ToInt32(dr1["PromiseId"]))
                        body = body.Append("<br />&nbsp; &nbsp; &nbsp;<h4> Promise:" + fr.PromiseName + " </h4> &nbsp; &nbsp; &nbsp; ");
                    fr.Dept_Id = Convert.ToString(dr1["DeptId"]);
                    fr.PromiseId = Convert.ToInt32(dr1["PromiseId"]);
                    DataTable ds1 = mdl.PromisesPrintData(fr.PromiseId, fr.Dept_Id);
                    body = body.Append("<div class='row col-md-12' style='padding-left:20px;'><h4> Department:" + fr.Dept_Name + "</h4></div>");
                    body = body.Append("<div class='row col-md-12'>" +
                       "   <div >" +
                       "     <table style ='border:2px solid'>" +
                       "          <tr style ='background:red; border: 1px solid #000;'>" +
                       "                    <th style='width:8%;color:blue; border: 1px solid #000;'> Project ID</th>" +
                       "                    <th style='width:8%; background:green;'>Project Name</th>" +
                       "                    <th style='width:8%'>Name of Firm(s)</th>" +
                       "                    <th style='width:8%'>Category</th>" +
                       "                    <th style='width:8%'>Estimated Amount<br /> (In Lakhs)</th>" +
                       "                    <th style='width:10%'>Funds Released <br />Till Date <br />(In Lakhs)</th>" +
                       "                    <th style='width:10%'>Expenditure<br /> Till Date<br />(In Lakhs)</th>" +
                       "                    <th style='width:8%'>Physical Progress (%)</th>" +
                       "                    <th style='width:8%'>Start Date</th>" +
                       "                    <th style='width:8%'>End Date</th>" +
                       "                    <th style='width:8%'>Status</th>" +
                       "                    <th style='width:8%'>Details/ Remark</th>" +
                       "                </tr>");
                    foreach (DataRow dr in ds1.Rows)
                    {
                        PromiseReportModel fr1 = new PromiseReportModel();
                        fr1.EndDate = dr["EndDate"].ToString();
                        fr1.EstimatedAmount = dr["EstimatedAmount"].ToString();
                        fr1.Expenditure = dr["Expenditure"].ToString();
                        fr1.FirmName = dr["FirmName"].ToString();
                        fr1.FundReleased = dr["FundReleased"].ToString();
                        fr1.PhysicalProgress = dr["PhysicalProgress"].ToString();
                        fr1.ProjectName = dr["ProjectName"].ToString();
                        fr1.Remark = dr["Remark"].ToString();
                        fr1.StartDate = dr["StartDate"].ToString();
                        fr1.CategoryName = dr["CategoryName"].ToString();
                        fr1.ProjectStatusName = dr["ProjectStatusName"].ToString();
                        fr1.PromiseId = dr["PromiseId"].ToString();
                        fr1.PromiseName = dr["PromiseName"].ToString();
                        fr1.deptname = dr["deptname"].ToString();
                        fr1.ProjectId = dr["ProjectId"].ToString();
                        // promiseReport.Add(fr1);

                        body = body.Append("<tr style='border: 1px solid #000;'>" +
                       "                    <td style='border: 1px solid #000; width:3%'>" + fr1.ProjectId + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.ProjectName + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.FirmName + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.CategoryName + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.EstimatedAmount + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.FundReleased + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.Expenditure + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.PhysicalProgress + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.StartDate + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.EndDate + "</td>" +
                       "                    <td style='border: 1px solid #000; width:8%'>" + fr1.ProjectStatusName + "</td>" +
                       "                    <td style='border: 1px solid #000; width:15%'>" + fr1.Remark + "</td>" +
                       "        </tr>");

                    }
                    body = body.Append("  </table> </div>  </div>");
                    i++;
                }
            }


            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Promise.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            var gv = new GridView();

            gv.DataSource = "";
            gv.DataBind();
            gv.RenderControl(hw);
            var pagenumber = gv.PageCount;
            StringReader sr = new StringReader(body.ToString());

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();

        }



        public ActionResult ReferencesDisposal()
        {


            return View();
        }

       

        public JsonResult KeepSessionAlive()
        {

            return null;
        }
        public ActionResult KeepSessionAliveTest()
        {
            return Content("Session Hit");

        }

        [HttpPost]
        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            string langCulture = "en";
            if (lang == "43177cd4-b51d-497a-a3bc-e81f23b07450")
            {
                langCulture = "hi-IN";
            }
            var langCookie = new HttpCookie("lang", langCulture)
            {
                HttpOnly = true
            };
            langCookie.Expires = DateTime.Now.AddDays(-1);

            Response.Cookies.Add(langCookie);
            CurrentSession.LanguageCulture = langCulture;
            if (lang != null)
            {
                CurrentSession.LanguageID = lang;
            }
            else
            {
                CurrentSession.LanguageID = "7C4F28F6-02FC-4627-993D-E255037531AD";
            }
            CurrentSession.UpdateMenuandModules();

            return Redirect(Request.UrlReferrer.ToString());
        }

        public JsonResult GetDataByAssemblySession(string AssemblyId, string SessionId)
        {
            CurrentSession.AssemblyId = AssemblyId;
            CurrentSession.SessionId = SessionId;

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult BranchInSession(string BranchId, string BranchName)
        {
            CurrentSession.BranchId = BranchId;
            CurrentSession.BranchName = BranchName;
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        //Capture ModuleId For Dynamically Active the module or Menu in LeftNavigation for onlineMember

        public JsonResult CaptureModuleIdActionId(string ModuleId, string ActionIds)
        {
            CurrentSession.MenuId = ModuleId;
            CurrentSession.ActionIds = ActionIds;
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /////////Parichay after  loginnnnnnnn
        ///
        private static string hsFunc(string encrString)
        {
            if (encrString == null)
            {
                return encrString;
            }

            else
            {
                string urlHs = "http://localhost:8083/handshake" + "/" + encrString + "/" + "CMNirdesh";
                using (var client = new HttpClient())
                {
                    var responseTask = client.GetAsync(urlHs).Result;
                    if (responseTask.StatusCode == HttpStatusCode.OK)
                    {
                        try
                        {
                            var resposedata = responseTask.Content.ReadAsByteArrayAsync().Result;
                            var response_value = "";
                            Task task = responseTask.Content.ReadAsStreamAsync().ContinueWith(t =>
                            {
                                var stream = t.Result;
                                using (var reader = new StreamReader(stream))
                                {
                                    response_value = reader.ReadToEnd();
                                }
                            }); task.Wait();
                            return response_value;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + " statuscode is :" + responseTask.StatusCode);
                            return responseTask.StatusCode.ToString() + "message" + e.Message;
                        }
                    }
                    else
                    {
                        Console.WriteLine("else part statuscode is :" + responseTask.StatusCode);
                    }
                    return responseTask.StatusCode.ToString();
                }
            }
           

           

        }

        private static Getdata_api decrptFunc(String hs_resp)
        {
            string requestBody = "{\"EncryptedString\":\"" + hs_resp + "\"}";
            string DEC_URL = "http://localhost:8083/decryption";
            using (var client = new HttpClient())
            {
                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseTask = client.PostAsync(DEC_URL, content).Result;

                if (responseTask.StatusCode == HttpStatusCode.OK)
                {
                    Getdata_api resposedata = JsonConvert.DeserializeObject<Getdata_api>(responseTask.Content.ReadAsStringAsync().Result);
                 
                   
                    return resposedata;
                }
            }
            return null;
        }

       
        private string sessionTimeout()
        {
            long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            string Service = "eNigam";
            string ClientSignature = hmacTimeOut(timeStamp, Service);
            string SesstimeOut = "https://parichay.nic.in/pnv1/salt/api/client/timeout?service=" + Service + "&tid=" + timeStamp + "&cs=" + ClientSignature;
            return SesstimeOut;

        }
        private string hmacFunc_logout(long timeStamp, string service, string parichayUrl, string userName, string sessionId)
        {
            string hmacString = "Parichay" + timeStamp + "https://parichay.nic.in/pnv1/salt/api/client/logout" + userName + service + sessionId;
            string requestBody = "{\"HmacString\":\"" + hmacString + "\"}";
            string HMAC_URL = "http://localhost:8085/hmac";

            using (var client = new HttpClient())
            {
                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseTask = client.PostAsync(HMAC_URL, content).Result;

                if (responseTask.StatusCode == HttpStatusCode.OK)
                {
                    ParichayClass resposedata = JsonConvert.DeserializeObject<ParichayClass>(responseTask.Content.ReadAsStringAsync().Result);

                    if (resposedata?.data?.signature != null)
                    {
                        
                        return resposedata.data.signature;
                    }
                }

                return null;
            }
        }
        private string hmacTimeOut(long timeStamp, string Service)
        {
            string hmacString = "Parichay" + timeStamp + "https://parichay.nic.in/pnv1/salt/api/client/timeout" + Service;
            string requestBody = "{\"HmacString\":\"" + hmacString + "\"}";
            string HMAC_URL = "http://localhost:8085/hmac";
            using (var client = new HttpClient())
            {
                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseTask = client.PostAsync(HMAC_URL, content).Result;
                if (responseTask.StatusCode == HttpStatusCode.OK)
                {
                    ParichayClass resposedata = JsonConvert.DeserializeObject<ParichayClass>(responseTask.Content.ReadAsStringAsync().Result);
                    if (resposedata?.data?.signature != null)
                    {
                        return resposedata.data.signature;
                    }
                }
                return null;
            }
        }
        public JsonResult ButtonsLoad()
        {
            LstHomeModel model = new LstHomeModel();
            var data = HomeButtons.GetButtonsList();
            model.ListHomeModel = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ButtonsLoad2()
        {
            LstHomeModel model = new LstHomeModel();
            var data = HomeButtons2.GetButtonsList2();
            model.ListHomeModel2 = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TestOtp()
        {
            SmsOrchestrationService smsService = new SmsOrchestrationService(); ;
            Task<string> asyncTask = smsService.SendForgetPasswordOtp("7018026001", "1234", "");
            asyncTask.Wait();
            string result = asyncTask.Result;
            return View();
        }

    }

}
    
