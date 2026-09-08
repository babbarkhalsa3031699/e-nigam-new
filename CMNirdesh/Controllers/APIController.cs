using CMNirdesh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using System.Security.Policy;

namespace CMNirdesh.Controllers
{
    public class APIController : Controller
    {
        // GET: API

        private static readonly HttpClient client = new HttpClient();
        public async Task<ActionResult> Index(string ConsId="88", int Type=3,string SelectedLang="2")
        {
             if (string.IsNullOrEmpty(ConsId))
            {
            }
            else
            {

                APIConstituency model = new APIConstituency();
                model.ConstituencyId = Convert.ToInt32(ConsId);
                model.Constituencylist = APIlist.GetDistrictConstituency(ConsId);
                if (model.ConstituencyId != 0)
                {
                    var apiUrl = "https://eapi.punjab.gov.in/hapi/nic/pswc-office-constituency-wise";
                    var requestData = new PswcRequest
                    {
                        StateId = 3,
                        ConstituencyID = model.ConstituencyId
                    };

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Server-Key", "IuJAhPm4yWhE7PNVOIu9eoNnPIBDIbctweC9gaHsCZFPMtn1zx3kZXM2n4CUq44f");

                    var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var pswcResponse = JsonConvert.DeserializeObject<PswcResponse>(result);
                        model.OfficeData = pswcResponse.Data;
                        List<PswcOffice> flagoffice= APIlist.GetFlagedOffices(ConsId);
                        var officeDict = model.OfficeData.ToDictionary(x => x.OfficeID, x => x);
                        // Loop through flagoffice
                        foreach (var office in flagoffice)
                        {
                            if (officeDict.ContainsKey(office.OfficeID))
                            {
                                // Update the existing office with values from flagoffice
                                var existing = officeDict[office.OfficeID];
                                existing.Flag = "1";
                            }
                            else
                            {
                                // Add new office from flagoffice
                                model.OfficeData.Add(office);
                            }
                        }

                        
                    }
                }
                model.OfficeData = model.OfficeData.Where(o => o.Flag == "1").ToList();
                model.OfficeData = model.OfficeData.OrderBy(x => x.DepartmentName).ToList();
                model.IsOpen = true;
                model.Type = Type;
                return View("Index",model);
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<ActionResult> Index(APIConstituency mdl)
        {
            APIConstituency model = new APIConstituency();
            model.ConstituencyId = Convert.ToInt32(mdl.ConstituencyId);
            if (model.ConstituencyId != 0)
            {
                var apiUrl = "https://eapi.punjab.gov.in/hapi/nic/pswc-office-constituency-wise";
                var requestData = new PswcRequest
                {
                    StateId = 3,
                    ConstituencyID = model.ConstituencyId
                };

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Server-Key", "IuJAhPm4yWhE7PNVOIu9eoNnPIBDIbctweC9gaHsCZFPMtn1zx3kZXM2n4CUq44f");

                var content = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var pswcResponse = JsonConvert.DeserializeObject<PswcResponse>(result);
                    model.OfficeData = pswcResponse.Data;
                }
            }
            List<PswcOffice> flagoffice = APIlist.GetFlagedOffices(mdl.ConstituencyId.ToString());
            var officeDict = model.OfficeData.ToDictionary(x => x.OfficeID, x => x);
            // Loop through flagoffice
            foreach (var office in flagoffice)
            {
                if (officeDict.ContainsKey(office.OfficeID))
                {
                    // Update the existing office with values from flagoffice
                    var existing = officeDict[office.OfficeID];
                    existing.Flag = "1";
                }
                else
                {
                    // Add new office from flagoffice
                    model.OfficeData.Add(office);
                }
            }


            if(mdl.typechkbox== "Flaged")
            {
                model.OfficeData = model.OfficeData.Where(o => o.Flag == "1").ToList();
            }
            model.OfficeData = model.OfficeData.OrderBy(x => x.DepartmentName).ToList();


            model.Constituencylist = APIlist.GetDistrictConstituency(mdl.ConstituencyId.ToString());
            model.IsOpen = true;
            model.EmpOpen = false;
            model.Type = mdl.Type;
            if (model.Type == 3 && mdl.PostDeptID != null )
            {
                var url = "https://eapi.punjab.gov.in/hapi/nic/fetch-pswc-employee";
                var serverKey = "IuJAhPm4yWhE7PNVOIu9eoNnPIBDIbctweC9gaHsCZFPMtn1zx3kZXM2n4CUq44f";

                var requestData = new
                {
                    StateId = 3,
                    PostDeptID = mdl.PostDeptID,
                    PostOfficeID = mdl.PostOfficeID
                };

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Server-Key", serverKey);

                    var json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Optional: deserialize response if needed
                    var result = JsonConvert.DeserializeObject<EmployeeApiResponse>(responseContent);
                    model.EmployeeData = result.data;
                    model.EmployeeData = model.EmployeeData.OrderBy(x => x.Designation).ToList();

                }

                if (mdl.Clicked == 1)
                {
                    model.EmpOpen = true;
                    model.OfficeDetial = model.OfficeData.SingleOrDefault(x => x.OfficeID == mdl.PostOfficeID.ToString());
                }
                else
                {
                    model.EmpOpen = false;
                }
                    
                   
            }



            return View("Index", model);
        }


        public ActionResult UpdateFlag(string ConstituencyId, string OfficeId, string IsFlag)
        {
            if (OfficeId != "")
            {
                APIlist APC = new APIlist();
                APC.UpdateFlag(Convert.ToInt32(ConstituencyId), Convert.ToInt32(OfficeId), Convert.ToInt32(IsFlag));
                
               
                
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> AllVisitors(int status = 1)
        {
            ViewBag.Status = status;

            List<VisitorModel> list = new List<VisitorModel>();
            string url;
            if (status == 0)
            {
                 url = "https://punjabassembly.gov.in/PunjabAPI/api/Visitor/GetPendingVisitorDetails?userId=65182cda-5a4f-4bf6-b340-7d646a638823&sessionDateId=1929&status=0&SessionNumber=07&assemblyCode=16";
            }
            else
            {
                url = "https://punjabassembly.gov.in/PunjabAPI/api/Visitor/GetAllSubmenuIDVisitors?menuid=3&Status=" + status + "&UserId=5e70495d-c2b2-49af-a8f4-95ed7f6ef890";
            
            }

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                (message, cert, chain, errors) => true;

            using (HttpClient client = new HttpClient(handler))
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    // if API returns direct array
                    list = JsonConvert.DeserializeObject<List<VisitorModel>>(json);
                    if (status == 0)
                    {
                        list = list
                            .Where(x => x.status != null && x.status.ToLower() == "forward")
                            .ToList();
                    }

                    list = list.OrderBy(x => x.sessionDate).OrderBy(x => x.mediaHouse).ToList();
                    // if wrapped response, use:
                    // var result = JsonConvert.DeserializeObject<Root>(json);
                    // list = result.data;
                }
            }

            return View(list);
        }

        public async Task<ActionResult> ExportPdf(int status = 1)
        {
            List<VisitorModel> list = new List<VisitorModel>();
            string url;
            if (status == 0)
            {
                url = "https://punjabassembly.gov.in/PunjabAPI/api/Visitor/GetPendingVisitorDetails?userId=65182cda-5a4f-4bf6-b340-7d646a638823&sessionDateId=1929&status=0&SessionNumber=07&assemblyCode=16";
            }
            else
            {
                url = "https://punjabassembly.gov.in/PunjabAPI/api/Visitor/GetAllSubmenuIDVisitors?menuid=3&Status=" + status + "&UserId=5e70495d-c2b2-49af-a8f4-95ed7f6ef890";

            }

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                (message, cert, chain, errors) => true;

            using (HttpClient client = new HttpClient(handler))
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VisitorModel>>(json);
                }
            }

            if (status == 0)
            {
                list = list
                    .Where(x => x.status != null && x.status.ToLower() == "forward")
                    .ToList();
            }
            ViewBag.Status = status;
            list= list.OrderBy(x => x.sessionDate).OrderBy(x => x.mediaHouse).ToList();
            return new ViewAsPdf("PdfView", list)
            {
                FileName = status == 1 ? "Approved.pdf" : status == 0 ? "Pending.pdf" : "Rejected.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Landscape
            };
        }


        [HttpGet]
        public async Task<HttpResponseMessage> ExportPdfApi(int status = 1)
        {
            List<VisitorModel> list = new List<VisitorModel>();
            string url;

            if (status == 0)
            {
                url = "https://punjabassembly.gov.in/PunjabAPI/api/Visitor/GetPendingVisitorDetails?userId=65182cda-5a4f-4bf6-b340-7d646a638823&sessionDateId=1929&status=0&SessionNumber=07&assemblyCode=16";
            }
            else
            {
                url = "https://punjabassembly.gov.in/PunjabAPI/api/Visitor/GetAllSubmenuIDVisitors?menuid=3&Status=" + status + "&UserId=5e70495d-c2b2-49af-a8f4-95ed7f6ef890";
            }

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                (message, cert, chain, errors) => true;

            using (HttpClient client = new HttpClient(handler))
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VisitorModel>>(json);
                }
            }

            // ✅ Filter pending → forward only
            if (status == 0)
            {
                list = list
                    .Where(x => x.status != null && x.status.ToLower() == "forward")
                    .ToList();
            }

            // ✅ Sorting FIX (your code had bug)
            list = list
                .OrderBy(x => x.mediaHouse)
                .ThenBy(x => x.sessionDate)
                .ToList();

            // ✅ Generate PDF using Rotativa
            var pdf = new Rotativa.ViewAsPdf("PdfView", list)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Landscape
            };

            var pdfBytes = pdf.BuildPdf(ControllerContext);

            // ✅ Return as file to mobile
            HttpResponseMessage result = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            result.Content = new ByteArrayContent(pdfBytes);
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = status == 1 ? "Approved.pdf" :
                               status == 0 ? "Pending.pdf" : "Rejected.pdf"
                };

            result.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            return result;
        }

        public async Task<ActionResult> ExportPdfMobile(int status = 1)
        {
            List<VisitorModel> list = new List<VisitorModel>();
            string url;

            if (status == 0)
            {
                url = "https://punjabassembly.gov.in/PunjabAPI/api/Visitor/GetPendingVisitorDetails?userId=65182cda-5a4f-4bf6-b340-7d646a638823&sessionDateId=1929&status=0&SessionNumber=07&assemblyCode=16";
            }
            else
            {
                url = "https://punjabassembly.gov.in/PunjabAPI/api/Visitor/GetAllSubmenuIDVisitors?menuid=3&Status=" + status + "&UserId=5e70495d-c2b2-49af-a8f4-95ed7f6ef890";
            }

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                (message, cert, chain, errors) => true;

            using (HttpClient client = new HttpClient(handler))
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VisitorModel>>(json);
                }
            }

            if (status == 0)
            {
                list = list
                    .Where(x => x.status != null && x.status.ToLower() == "forward")
                    .ToList();
            }

            list = list.OrderBy(x => x.mediaHouse)
                       .ThenBy(x => x.sessionDate)
                       .ToList();

            ViewBag.Status = status;

            var pdf = new Rotativa.ViewAsPdf("PdfView", list)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Landscape
            };

            byte[] pdfBytes = pdf.BuildPdf(ControllerContext);

            return File(pdfBytes, "application/pdf",
                status == 1 ? "Approved.pdf" :
                status == 0 ? "Pending.pdf" : "Rejected.pdf");
        }
    }
}