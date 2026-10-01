
using Azure.Storage.Blobs;
using BotDetect;
using BotDetect.C5;
using CMNirdesh.App_Start;
using CMNirdesh.Filters;
using CMNirdesh.Models;
using CMNirdesh.Models.Diaries;
using CMNirdesh.Models.Email;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.html.head;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Tsp;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using Rotativa;
using Rotativa.Options;
using SelectPdf;
using Serilog;
using Syncfusion.DocIO.DLS;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.PeerToPeer;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer;
using System.Web.Script.Serialization;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static iTextSharp.tool.xml.html.HTML;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using ErrorLog = CMNirdesh.Error.ErrorLog;




namespace CMNirdesh.Controllers
{
    [Audit]
    [NoCache]
    [CMNirdeshAuthorize(Allow = "Authenticated")]
    public class AgendaController : Controller
    {
        private static IBrowser _browser;

        private static async Task<IBrowser> GetBrowserAsync()
        {
            if (_browser == null || !_browser.IsConnected)
            {
                _browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                    Headless = true,
                    Args = new[]
                    {
                    "--disable-gpu",
                    "--no-sandbox",
                    "--disable-dev-shm-usage",
                    "--disable-extensions",
                    "--disable-software-rasterizer"
                }
                });
            }
            return _browser;
        }
        public ActionResult CreateDiaryAgenda()
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            AgendaModel model = new AgendaModel();

            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                ViewBag.OfficeName = CurrentSession.OfficeName;
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                model._LstAgenda = AgendaModelFunction.GetAgenda(OfficeId, "Diary");
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }


        [HttpPost]
        public ActionResult SaveDiaryAgenda(AgendaModel mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }

            AgendaModel model = new AgendaModel();
            model.AgendaName = mdl.AgendaName;
            model.AgendaDate = mdl.AgendaDate;
            model.Remarks = mdl.Remarks;
            model.UserId = CurrentSession.UserID;
            model.DeptId = CurrentSession.DeptID;
            model.OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            model.Agendatype = "Diary";
            string msg = AgendaModelFunction.SaveAgenda(model);
            if (msg == "1")
            {
                Session["userExist"] = "Agenda Already Exist";
                Session["userAdded"] = "";
            }
            else
            {
                Session["userAdded"] = "Agenda Added Successfully.";
                Session["userExist"] = "";
                ModelState.Clear();
            }
            ViewBag.OfficeName = CurrentSession.OfficeName;
            int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            mdl._LstAgenda = AgendaModelFunction.GetAgenda(OfficeId, "Diary");
            return View("CreateDiaryAgenda", mdl);
        }

        public ActionResult CreateDispatchAgenda()
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            AgendaModel model = new AgendaModel();

            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                ViewBag.OfficeName = CurrentSession.OfficeName;
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                model._LstAgenda = AgendaModelFunction.GetAgenda(OfficeId, "Dispatch");
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }


        public ActionResult SaveDispatchAgenda(AgendaModel mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }

            AgendaModel model = new AgendaModel();
            model.AgendaName = mdl.AgendaName;
            model.AgendaDate = mdl.AgendaDate;
            model.Remarks = mdl.Remarks;
            model.UserId = CurrentSession.UserID;
            model.DeptId = CurrentSession.DeptID;
            model.OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            model.Agendatype = mdl.Agendatype;
            DateTime dateTime = DateTime.Parse(Convert.ToString(mdl.StartTime), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);

            // Get the time in the format "01:15:00.0000000".
            string formattedTime = dateTime.ToString("HH:mm:ss", CultureInfo.GetCultureInfo("en-US"));

            model.StartTime = formattedTime;


            model.Header = mdl.Header;
            model.Footer = mdl.Footer;

            model.Footer = model.Footer.Replace("\r\n", "<br>");
            string msg = AgendaModelFunction.SaveAgenda(model);

            if (msg == "Success")
            {
                TempData["AlertMessage"] = "Meeting Added Successfully";
                ModelState.Clear();

            }
            else if (msg == "HouseId is null")
            {
                TempData["AlertMessageDanger"] = "House ID Does Not Exist";
                ModelState.Clear();

            }
            else if (msg == "Meeting Already Exists")
            {
                TempData["AlertMessageDanger"] = "Meeting Already Exists";
                ModelState.Clear();

            }
            else
            {
                TempData["AlertMessageDanger"] = "Something went Wrong";
                ModelState.Clear();

            }
            ViewBag.OfficeName = CurrentSession.OfficeName;
            int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            mdl._LstAgenda = AgendaModelFunction.GetAgenda(OfficeId, "Dispatch");
            return RedirectToAction("CreateDispatchAgenda");
        }




        [HttpGet]
        public ActionResult DiaryAgendaDetail()
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            AgendaDetailModel model = new AgendaDetailModel();

            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                ViewBag.OfficeName = CurrentSession.OfficeName;
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                string DeptId = CurrentSession.DeptID;
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }

        [HttpPost]

        public ActionResult DiaryAgendaDetail(AgendaDetailModel mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            Session["userExist"] = "";
            Session["userAdded"] = "";
            int agendaid = Convert.ToInt32(mdl.AgendaId);
            mdl._LstReferences = AgendaModelFunction.GetDiaryReferences(agendaid);
            int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            string DeptId = CurrentSession.DeptID;
            mdl.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
            mdl.IsOpen = true;
            mdl.AgendaType = "Diary";
            mdl.Count = mdl._LstReferences.Count.ToString();
            if (mdl._LstReferences.Count > 0)
            {
                mdl.freezeButton = true;
            }
            else
            {
                mdl.freezeButton = false;
            }
            return View("DiaryAgendaDetail", mdl);
        }



        [HttpGet]
        public ActionResult DispatchAgendaDetail()
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            AgendaDetailModel model = new AgendaDetailModel();

            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                ViewBag.OfficeName = CurrentSession.OfficeName;
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                string DeptId = CurrentSession.DeptID;
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
                // model.FileList = DiaryViewModel.GetFileList(CurrentSession.UserID, CurrentSession.OfficeId,"0","");
                model.FileList = DiaryViewModel.GetMeetingFileList(CurrentSession.UserID.ToString(), CurrentSession.DeptID.ToString(), Convert.ToInt32(CurrentSession.OfficeId), 0, 0);
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }


        [HttpPost]

        public ActionResult DispatchAgendaDetail(AgendaDetailModel mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            Session["userExist"] = "";
            Session["userAdded"] = "";
            int agendaid = Convert.ToInt32(mdl.AgendaId);


            //Aman
            if (Request.Form["getagenda"] != null)
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
            }
            else if (Request.Form["getdown2pdf"] != null)
            {

                // read parameters from the webpage
                string htmlString = GetJalpdfhtml(agendaid);
                //string baseUrl = TxtBaseUrl.Text;

                //string pdf_page_size = DdlPageSize.SelectedValue;

                //PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),"A4", true);            

                PdfPageOrientation pdfOrientation =
                    (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                    "Portrait", true);

                //int webPageWidth = 1024;


                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                //   converter.Options.WebPageWidth = webPageWidth;
                //  converter.Options.WebPageHeight = 100;

                // create a new pdf document converting an url
                SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);

                string finalpdfpath = "Agenda/" + agendaid.ToString() + "_Agenda123.pdf";
                string folderpath = "~/MLADIARY/" + finalpdfpath;
                string agendapath = Server.MapPath(folderpath);
                pdfDoc.Save(agendapath);
                SavePDf2(agendaid.ToString(), finalpdfpath, "");


                //var ms = new MemoryStream();
                //pdfDoc.Save(ms);

                return File(folderpath, "application/pdf", "ExportData.pdf");
            }




            //else if (Request.Form["getpdfapprove"] != null)
            //{

            //    // read parameters from the webpage
            //    //string htmlString = GetNewapprovepdfhtml(agendaid,doctype);


            //    PdfPageOrientation pdfOrientation =
            //        (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
            //        "Portrait", true);



            //    // instantiate a html to pdf converter object
            //    HtmlToPdf converter = new HtmlToPdf();

            //    // set converter options
            //    converter.Options.PdfPageSize = PdfPageSize.A4;


            //    converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            //    //   converter.Options.WebPageWidth = webPageWidth;
            //    //  converter.Options.WebPageHeight = 100;
            //    converter.Options.MarginTop = 20;
            //    converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            //    converter.Options.MarginLeft = 20;
            //    converter.Options.MarginRight = 20;

            //    // create a new pdf document converting an url
            //    SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);


            //    string finalpdfpath = "Agenda/" + agendaid.ToString() + "_Agenda.pdf";
            //    string folderpath = "~/MLADIARY/" + finalpdfpath;
            //    string agendapath = Server.MapPath(folderpath);
            //    pdfDoc.Save(agendapath);
            //    SavePDf(agendaid.ToString(), finalpdfpath);
            //    //var ms = new MemoryStream();
            //    //pdfDoc.Save(ms);

            //    return File(folderpath, "application/pdf", "ExportData.pdf");
            //}


            else if (Request.Form["getpdf"] != null)
            {

                // read parameters from the webpage
                string htmlString = GetNewpdfhtml(agendaid);






                PdfPageOrientation pdfOrientation =
                    (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                    "Portrait", true);



                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                //   converter.Options.WebPageWidth = webPageWidth;
                //  converter.Options.WebPageHeight = 100;

                // create a new pdf document converting an url
                SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);

                string finalpdfpath = "Agenda/" + agendaid.ToString() + "_Agenda.pdf";
                string folderpath = "~/MLADIARY/" + finalpdfpath;
                string agendapath = Server.MapPath(folderpath);
                pdfDoc.Save(agendapath);
                SavePDf(agendaid.ToString(), finalpdfpath);
                //var ms = new MemoryStream();
                //pdfDoc.Save(ms);

                return File(folderpath, "application/pdf", "ExportData.pdf");
            }

            else if (Request.Form["getdownpdf"] != null)
            {




                string htmlString = Getpdfhtml(agendaid);




                PdfPageOrientation pdfOrientation =
                    (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                    "Portrait", true);


                HtmlToPdf converter = new HtmlToPdf();

                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                //   converter.Options.WebPageWidth = webPageWidth;
                //  converter.Options.WebPageHeight = 100;

                // create a new pdf document converting an url
                SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);


                var ms = new MemoryStream();
                string finalpdfpath = "~/MLADIARY/Agenda/" + agendaid.ToString() + "_Agenda123.pdf";
                string agendapath = Server.MapPath(finalpdfpath);
                pdfDoc.Save(agendapath);

                //pdfDoc.Save(ms);

                return File(agendapath, "application/pdf", "ExportData.pdf");
            }



            if (mdl.refresh == "1")
            {
                List<AdminLOBJson> indexTable = new List<AdminLOBJson>();
                indexTable = new JavaScriptSerializer().Deserialize<List<AdminLOBJson>>(mdl.agendadetails);
                int i = 1;
                foreach (AdminLOBJson index in indexTable)
                {
                    int order = i;
                    int agendaRecordId = Convert.ToInt32(index.AgendaRecordId);
                    int indexcount;
                    if (index.Index.ToString() != "")
                    {
                        indexcount = Convert.ToInt32(index.Index);
                    }
                    else
                    {
                        indexcount = 0;
                    }
                    int indexcountTwo;
                    if (index.IndexTwo.ToString() != "")
                    {
                        indexcountTwo = Convert.ToInt32(index.IndexTwo);
                    }
                    else
                    {
                        indexcountTwo = 0;
                    }
                    AgendaModelFunction.FreezeRecord(agendaRecordId, indexcount, indexcountTwo, order);
                    i++;
                }
                mdl.refresh = "0";
                mdl.agendadetails = "";
                //Session["userAdded"] = "Agenda Freezed Successfully.";
            }

            //if (mdl.freezed == "1")
            //{
            //    List<AdminLOBJson> indexTable = new List<AdminLOBJson>();
            //    indexTable = new JavaScriptSerializer().Deserialize<List<AdminLOBJson>>(mdl.agendadetails);
            //    int i = 1;
            //    foreach (AdminLOBJson index in indexTable)
            //    {
            //        int order = i;
            //        int agendaRecordId = Convert.ToInt32(index.AgendaRecordId);
            //        int indexcount;
            //        if (index.Index.ToString() != "")
            //        {
            //            indexcount = Convert.ToInt32(index.Index);
            //        }
            //        else
            //        {
            //            indexcount = 0;
            //        }
            //        AgendaModelFunction.FreezeRecord(agendaRecordId, indexcount, order);
            //        i++;
            //    }
            //    AgendaModelFunction.FreezeUnfreezeAgenda(Convert.ToInt32(mdl.AgendaId), 1);
            //    mdl.freezed = "0";
            //    mdl.agendadetails = "";
            //    Session["userAdded"] = "Agenda Freezed Successfully.";
            //}

            if (mdl.freezed == "1")
            {
                List<AdminLOBJson> indexTable = new List<AdminLOBJson>();
                indexTable = new JavaScriptSerializer().Deserialize<List<AdminLOBJson>>(mdl.agendadetails);
                int i = 1;
                foreach (AdminLOBJson index in indexTable)
                {
                    int order = i;
                    int agendaRecordId = Convert.ToInt32(index.AgendaRecordId);
                    int indexcount;
                    if (index.Index.ToString() != "")
                    {
                        indexcount = Convert.ToInt32(index.Index);
                    }
                    else
                    {
                        indexcount = 0;
                    }
                    int indexcountTwo;
                    if (index.IndexTwo.ToString() != "")
                    {
                        indexcountTwo = Convert.ToInt32(index.IndexTwo);
                    }
                    else
                    {
                        indexcountTwo = 0;
                    }
                    AgendaModelFunction.FreezeRecord(agendaRecordId, indexcount, indexcountTwo, order);
                    i++;
                }
                AgendaModelFunction.FreezeUnfreezeAgenda(Convert.ToInt32(mdl.AgendaId), 1);
                //AgendaModelFunction.ApproveAgenda(Convert.ToInt32(mdl.AgendaId), 1);
                mdl.freezed = "0";
                mdl.agendadetails = "";
                Session["userAdded"] = "Agenda Saved Successfully.";
            }


            mdl._LstReferences = AgendaModelFunction.GetDishpatchReferences(agendaid);
            int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            string DeptId = CurrentSession.DeptID;
            // mdl.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
            mdl.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
            mdl.Count = mdl._LstReferences.Count.ToString();

            if (mdl._LstReferences.Count > 0)
            {
                string isfreeze = mdl._LstReferences.FirstOrDefault().AgendaApprove.ToString();
                if (isfreeze == "1")
                {
                    mdl.freezeButton = false;
                }
                else
                {
                    mdl.freezeButton = true;
                }

                mdl.FileRefId = mdl._LstReferences.Select(m => m.FileRefId).FirstOrDefault();
                mdl.FileList = DiaryViewModel.GetMeetingFileList(CurrentSession.UserID.ToString(), CurrentSession.DeptID.ToString(), Convert.ToInt32(CurrentSession.OfficeId), 0, Convert.ToInt64(mdl.FileRefId));
            }
            else
            {
                mdl.FileList = DiaryViewModel.GetMeetingFileList(CurrentSession.UserID.ToString(), CurrentSession.DeptID.ToString(), Convert.ToInt32(CurrentSession.OfficeId), 0, 0);
                mdl.freezeButton = true;
            }
            mdl.IsOpen = true;
            mdl.AgendaType = "Dispatch";

            return View("DispatchAgendaDetail", mdl);
        }



        [HttpGet]
        public ActionResult ApproveAgendaDetail()
        {

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            AgendaDetailModel model = new AgendaDetailModel();

            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                ViewBag.OfficeName = CurrentSession.OfficeName;
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                string DeptId = CurrentSession.DeptID;
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "1");
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }


        [HttpPost]

        public ActionResult ApproveAgendaDetail(AgendaDetailModel mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            Session["userExist"] = "";
            Session["userAdded"] = "";
            if (mdl.AgendaId != 0)
            {
                int agendaid = Convert.ToInt32(mdl.AgendaId);
                string mcname = CurrentSession.DeptName;

                //Aman
                if (Request.Form["getagenda"] != null)
                {
                    Session["userExist"] = "";
                    Session["userAdded"] = "";
                }
                else if (Request.Form["getdown2pdf"] != null)
                {

                    // read parameters from the webpage
                    string htmlString = GetJalpdfhtml(agendaid);
                    //string baseUrl = TxtBaseUrl.Text;

                    //string pdf_page_size = DdlPageSize.SelectedValue;

                    //PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),"A4", true);            

                    PdfPageOrientation pdfOrientation =
                        (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                        "Portrait", true);

                    //int webPageWidth = 1024;


                    // instantiate a html to pdf converter object
                    HtmlToPdf converter = new HtmlToPdf();

                    // set converter options
                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                    //   converter.Options.WebPageWidth = webPageWidth;
                    //  converter.Options.WebPageHeight = 100;

                    // create a new pdf document converting an url
                    SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);

                    string finalpdfpath = "Agenda/" + agendaid.ToString() + "_Agenda123.pdf";
                    string folderpath = "~/MLADIARY/" + finalpdfpath;
                    string agendapath = Server.MapPath(folderpath);
                    pdfDoc.Save(agendapath);
                    SavePDf2(agendaid.ToString(), finalpdfpath, "");


                    //var ms = new MemoryStream();
                    //pdfDoc.Save(ms);

                    return File(folderpath, "application/pdf", "ExportData.pdf");
                }


                ////Agenda PDF 1
                //else if (Request.Form["getpdfapprove"] != null)
                //{

                //    // read parameters from the webpage
                //   // string htmlString = GetNewapprovepdfhtml(agendaid);
                //    //string htmlString = GetMuncipalpdfhtml(agendaid);

                //    // instantiate a html to pdf converter object
                //    HtmlToPdf converter = new HtmlToPdf();

                //    converter.Options.PdfPageSize = PdfPageSize.A4;
                //    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                //    //converter.Options.WebPageWidth = webPageWidth;
                //    //converter.Options.WebPageHeight = 100;
                //    //converter.Options.MarginLeft = 40;
                //    //converter.Options.MarginRight =40;
                //    converter.Options.MarginTop = 20;
                //    //converter.Options.MarginBottom = 30;

                //    converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                //    converter.Options.MarginLeft = 20;
                //    converter.Options.MarginRight = 20;




                //    // page numbers can be added using a PdfTextSection object
                //    // footer settings
                //    converter.Options.DisplayFooter = true;
                //    converter.Footer.DisplayOnFirstPage = true;
                //    converter.Footer.DisplayOnOddPages = true;
                //    converter.Footer.DisplayOnEvenPages = true;
                //    converter.Footer.Height = 30;


                //    PdfTextSection text1 = new PdfTextSection(0, 10, "Digital MC House " + mcname + " .Generated on " + DateTime.Now, new System.Drawing.Font("Arial", 8));
                //    text1.HorizontalAlign = PdfTextHorizontalAlign.Left;
                //    converter.Footer.Add(text1);

                //    PdfTextSection text = new PdfTextSection(0, 10, "(NIC) - Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                //    text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                //    converter.Footer.Add(text);

                //    // create a new pdf document converting an url
                //    SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);

                //    string finalpdfpath = "Agenda/" + agendaid.ToString() + "_Agenda.pdf";
                //    string folderpath = "~/MLADIARY/" + finalpdfpath;
                //    string agendapath = Server.MapPath(folderpath);
                //    pdfDoc.Save(agendapath);
                //    SavePDf(agendaid.ToString(), finalpdfpath);
                //    //var ms = new MemoryStream();
                //    //pdfDoc.Save(ms);
                //    // Set the content type to HTML
                //    Response.ContentType = "text/html";

                //    // Get the byte array representation of the HTML string
                //    byte[] htmlBytes = Encoding.UTF8.GetBytes(htmlString);

                //    // Write the HTML bytes to the response stream
                //    Response.OutputStream.Write(htmlBytes, 0, htmlBytes.Length);
                //    Response.End();
                //    return File(htmlString, "text/html");
                //    ///return File(folderpath, "application/pdf", "ExportData.pdf");
                //}

                else if (Request.Form["getpdf"] != null)
                {

                    // read parameters from the webpage
                    string htmlString = GetNewpdfhtml(agendaid);
                    //string baseUrl = TxtBaseUrl.Text;

                    //string pdf_page_size = DdlPageSize.SelectedValue;

                    //PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),"A4", true);            

                    PdfPageOrientation pdfOrientation =
                        (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                        "Portrait", true);

                    //int webPageWidth = 1024;


                    // instantiate a html to pdf converter object
                    HtmlToPdf converter = new HtmlToPdf();

                    // set converter options
                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                    //   converter.Options.WebPageWidth = webPageWidth;
                    //  converter.Options.WebPageHeight = 100;

                    // create a new pdf document converting an url
                    SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);


                    //Replace Patial with database value
                    PdfTextSection text1 = new PdfTextSection(0, 10, "Digital MC House " + mcname + " .Generated on " + DateTime.Now, new System.Drawing.Font("Arial", 8));
                    text1.HorizontalAlign = PdfTextHorizontalAlign.Left;
                    converter.Footer.Add(text1);

                    PdfTextSection text = new PdfTextSection(0, 10, "(NIC) - Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                    text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                    converter.Footer.Add(text);

                    string finalpdfpath = "Agenda/" + agendaid.ToString() + "_Agenda.pdf";
                    string folderpath = "~/MLADIARY/" + finalpdfpath;
                    string agendapath = Server.MapPath(folderpath);
                    pdfDoc.Save(agendapath);
                    SavePDf(agendaid.ToString(), finalpdfpath);
                    //var ms = new MemoryStream();
                    //pdfDoc.Save(ms);

                    return File(folderpath, "application/pdf", "ExportData.pdf");
                }

                else if (Request.Form["getdownpdf"] != null)
                {

                    string htmlString = Getpdfhtml(agendaid);


                    PdfPageOrientation pdfOrientation =
                        (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                        "Portrait", true);


                    HtmlToPdf converter = new HtmlToPdf();

                    // set converter options
                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                    //   converter.Options.WebPageWidth = webPageWidth;
                    //  converter.Options.WebPageHeight = 100;

                    // create a new pdf document converting an url
                    SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);



                    //Replace Patial with database value
                    PdfTextSection text1 = new PdfTextSection(0, 10, "Digital MC House " + mcname + " .Generated on " + DateTime.Now, new System.Drawing.Font("Arial", 8));
                    text1.HorizontalAlign = PdfTextHorizontalAlign.Left;
                    converter.Footer.Add(text1);

                    PdfTextSection text = new PdfTextSection(0, 10, "(NIC) - Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                    text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                    converter.Footer.Add(text);


                    var ms = new MemoryStream();
                    string finalpdfpath = "~/MLADIARY/Agenda/" + agendaid.ToString() + "_Agenda123.pdf";
                    string agendapath = Server.MapPath(finalpdfpath);
                    pdfDoc.Save(agendapath);

                    //pdfDoc.Save(ms);

                    return File(agendapath, "application/pdf", "ExportData.pdf");
                }





                if (mdl.freezed == "1")
                {
                    List<AdminLOBJson> indexTable = new List<AdminLOBJson>();
                    indexTable = new JavaScriptSerializer().Deserialize<List<AdminLOBJson>>(mdl.agendadetails);
                    int i = 1;
                    foreach (AdminLOBJson index in indexTable)
                    {
                        int order = i;
                        int agendaRecordId = Convert.ToInt32(index.AgendaRecordId);
                        int indexcount;
                        if (index.Index.ToString() != "")
                        {
                            indexcount = Convert.ToInt32(index.Index);
                        }
                        else
                        {
                            indexcount = 0;
                        }

                        int indexcountTwo;
                        if (index.IndexTwo.ToString() != "")
                        {
                            indexcountTwo = Convert.ToInt32(index.IndexTwo);
                        }
                        else
                        {
                            indexcountTwo = 0;
                        }
                        AgendaModelFunction.FreezeRecord(agendaRecordId, indexcount, indexcountTwo, order);
                        i++;
                    }
                    AgendaModelFunction.ApproveAgenda(Convert.ToInt32(mdl.AgendaId), 1);
                    mdl.freezed = "0";
                    mdl.agendadetails = "";
                    Session["userAdded"] = "Agenda Approved Successfully.";
                }

                mdl._LstReferences = AgendaModelFunction.GetDishpatchReferences(agendaid);
                mdl.IsOpen = true;


                if (mdl.unfreezed == "1")
                {
                    AgendaModelFunction.FreezeUnfreezeAgenda(Convert.ToInt32(mdl.AgendaId), Convert.ToInt32(0));
                    mdl.unfreezed = "0";
                    mdl._LstReferences = AgendaModelFunction.GetDishpatchReferences(0);
                    mdl.IsOpen = false;
                    Session["userAdded"] = "Agenda Unfreezed Successfully.";
                }



                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);


                mdl.ButtonPermission = AgendaModelFunction.GetMenuButtonPermission(CurrentSession.UserID.ToString(), "4044");
                mdl.Count = mdl._LstReferences.Count.ToString();
                if (mdl._LstReferences.Count > 0)
                {
                    string isappove = mdl._LstReferences.FirstOrDefault().AgendaApprove.ToString();
                    if (isappove == "1")
                    {
                        mdl.freezeButton = false;
                        DataSet ds = AgendaModelFunction.CheckAgendaStatus(Convert.ToInt32(mdl.AgendaId));
                        if (ds.Tables.Count > 0)
                        {
                            int AgendaCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            int PendingCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                            if (ds.Tables[2].Rows[0][0].ToString() != "")
                            {
                                DateTime EndTime = Convert.ToDateTime(ds.Tables[2].Rows[0][0].ToString());
                                DateTime Today = DateTime.Now;
                                if (Today > EndTime)
                                {
                                    mdl.MeeingEnd = true;
                                }
                                else
                                {
                                    mdl.MeeingEnd = false;
                                }
                            }
                            else
                            {
                                mdl.MeeingEnd = false;
                            }


                            //if (AgendaCount > 0  & PendingCount == 0)
                            //{
                            //    mdl.PassButton = true;
                            //}
                            //else
                            //{
                            //    mdl.PassButton = false;
                            //}

                        }
                    }
                    else
                    {
                        mdl.freezeButton = true;
                        // mdl.PassButton = false;
                    }

                }
                else
                {
                    mdl.freezeButton = true;
                    // mdl.PassButton = false;
                }
                mdl.AgendaType = "Approve";

            }

            string DeptId = CurrentSession.DeptID;
            mdl.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, mdl.ActId.ToString());
            return View("ApproveAgendaDetail", mdl);
        }


        [HttpGet]
        public ActionResult ApproveProceeding()
        {



            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            AgendaDetailModel model = new AgendaDetailModel();

            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                ViewBag.OfficeName = CurrentSession.OfficeName;
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                string DeptId = CurrentSession.DeptID;
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "1");
                model.ActList = AgendaModelFunction.GetActList();
                model.ActionList = DiaryViewModel.GetAllActionTypeList(0, CurrentSession.UserID.ToString(), "Diary", 0);
                model.CurrentDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                model.FileList = DiaryViewModel.GetMeetingFileList(CurrentSession.UserID.ToString(), CurrentSession.DeptID.ToString(), Convert.ToInt32(CurrentSession.OfficeId), 0, 0);
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }


        [HttpPost]

        public ActionResult ApproveProceeding(AgendaDetailModel mdl)
        {

            if (mdl.AgendaId != 0)
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                Session["userExist"] = "";
                Session["userAdded"] = "";
                int agendaid = Convert.ToInt32(mdl.AgendaId);
                string mcname = CurrentSession.DeptName;



                //Aman
                if (Request.Form["getagenda"] != null)
                {
                    Session["userExist"] = "";
                    Session["userAdded"] = "";
                }

                if (mdl.freezed == "1")
                {
                    List<AdminLOBJson> indexTable = new List<AdminLOBJson>();
                    indexTable = new JavaScriptSerializer().Deserialize<List<AdminLOBJson>>(mdl.agendadetails);
                    int i = 1;
                    string emptyindex = "", ResolutionExist = "";
                    Boolean validate;
                    int SesOfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                    foreach (AdminLOBJson index in indexTable)
                    {
                        //
                        string[] result = index.Index.Split('.');
                        int indexcount; int index1; int index2;

                        if (result[0] != "")
                        {
                            indexcount = result.Length;
                        }
                        else
                        {
                            indexcount = 0;
                        }

                        switch (indexcount)
                        {
                            case 1:
                                index1 = Convert.ToInt32(result[0]);
                                index2 = 0;
                                break;
                            case 2:
                                index1 = Convert.ToInt32(result[0]);
                                index2 = Convert.ToInt32(result[1]);
                                break;
                            default:
                                index1 = 0;
                                index2 = 0;
                                break;
                        }
                        //
                        if (index2 == 0)
                        {
                            var uc = indexTable.Where(x => x.Index == index1.ToString());
                            int ac = uc.Count();
                            if (ac > 1)
                            {
                                ResolutionExist = ResolutionExist + index1.ToString() + ",";
                            }
                        }
                        else
                        {
                            var uc = indexTable.Where(x => x.Index.ToString().Contains("." + index2.ToString()));
                            int ac = uc.Count();
                            if (ac > 1)
                            {
                                ResolutionExist = ResolutionExist + index1.ToString() + "." + index2.ToString() + ",";
                            }
                        }
                        int order = i;
                        int agendaRecordId = Convert.ToInt32(index.AgendaRecordId);

                        if (index1.ToString() != "" && index1.ToString() != "0")
                        {
                            indexcount = Convert.ToInt32(index1);
                            DataSet ds = AgendaModelFunction.CheckResolutionNo(Convert.ToInt32(index1), Convert.ToInt32(index2), SesOfficeId);
                            if (ds.Tables.Count > 0)
                            {
                                int ResolutionCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                                if (ResolutionCount > 0)
                                {
                                    ResolutionExist = ResolutionExist + index1.ToString() + ",";

                                }
                            }
                        }
                        else
                        {

                            emptyindex = emptyindex + index.AgendaRecordId.ToString() + ",";
                        }
                        i++;
                    }

                    if (emptyindex == "" && ResolutionExist == "")
                    {
                        validate = true;
                    }
                    else
                    {
                        validate = false;
                    }
                    //validate = true;
                    if (validate)
                    {
                        foreach (AdminLOBJson index in indexTable)
                        {
                            //
                            string[] result = index.Index.Split('.');
                            int indexcount; int index1; int index2;

                            if (result[0] != "")
                            {
                                indexcount = result.Length;
                            }
                            else
                            {
                                indexcount = 0;
                            }

                            switch (indexcount)
                            {
                                case 1:
                                    index1 = Convert.ToInt32(result[0]);
                                    index2 = 0;
                                    break;
                                case 2:
                                    index1 = Convert.ToInt32(result[0]);
                                    index2 = Convert.ToInt32(result[1]); ;
                                    break;
                                default:
                                    index1 = 0;
                                    index2 = 0;
                                    break;
                            }
                            //

                            int order = i;
                            int agendaRecordId = Convert.ToInt32(index.AgendaRecordId);

                            if (index1.ToString() != "")
                            {
                                indexcount = Convert.ToInt32(index1);
                            }
                            else
                            {
                                indexcount = 0;
                            }
                            AgendaModelFunction.FreezeProceeding(agendaRecordId, indexcount, index2, order);
                            i++;
                        }
                        AgendaModelFunction.FreezeProceedings(Convert.ToInt32(mdl.AgendaId), 1);
                        Session["userAdded"] = "Proceedings Saved Successfully.";
                    }
                    else
                    {
                        Session["userAdded"] = "Resolution No. is Empty Or Resolution No. " + ResolutionExist + " Already Assign.";
                    }

                    mdl.freezed = "0";
                    mdl.agendadetails = "";

                }


                mdl._LstReferences = AgendaModelFunction.GetApproveReferences(agendaid);
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);



                mdl.ActList = AgendaModelFunction.GetActList();
                mdl.ActionList = DiaryViewModel.GetAllActionTypeList(0, CurrentSession.UserID.ToString(), "Diary", 0);
                mdl.ButtonPermission = AgendaModelFunction.GetMenuButtonPermission(CurrentSession.UserID.ToString(), "5049");
                mdl.Count = mdl._LstReferences.Count.ToString();
                if (mdl._LstReferences.Count > 0)
                {
                    string FreezePro = mdl._LstReferences.FirstOrDefault().ProceedingFreeze.ToString();
                    string isappove = mdl._LstReferences.FirstOrDefault().AgendaApprove.ToString();
                    string AgendaPassed = mdl._LstReferences.FirstOrDefault().AgendaPassed.ToString();
                    mdl.FileRefId = mdl._LstReferences.FirstOrDefault().FileRefId.ToString();
                    if (mdl.FileRefId == "")
                    {
                        mdl.FileRefId = "0";
                    }
                    if (isappove == "1" && AgendaPassed == "0")
                    {
                        mdl.freezeButton = false;
                        mdl.PdfButtons = false;
                        DataSet ds = AgendaModelFunction.CheckAgendaStatus(Convert.ToInt32(mdl.AgendaId));
                        if (ds.Tables.Count > 0)
                        {
                            int AgendaCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            int PendingCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                            string EndTime = ds.Tables[2].Rows[0][0].ToString();
                            int AttandanceCount = Convert.ToInt32(ds.Tables[3].Rows[0][0]);
                            if (EndTime == "" || AttandanceCount == 0)
                            {
                                mdl.MeeingEnd = false;
                                mdl.PassButton = false;
                            }
                            else
                            {
                                mdl.MeeingEnd = true;
                                mdl.PassButton = true;
                            }




                            if (FreezePro == "1")
                            {
                                mdl.PdfButtons = false;
                            }
                            else
                            {
                                mdl.PdfButtons = true;
                            }


                            //string pdf1 = ds.Tables[2].Rows[0][0].ToString();
                            //string pdf2 = ds.Tables[2].Rows[0][1].ToString();
                            //if (AgendaCount > 0 & PendingCount == 0 && EndTime != "")
                            //{
                            //    mdl.PassButton = true;
                            //}
                            //else
                            //{
                            //    mdl.PassButton = false;
                            //}
                            //mdl.PassButton = true;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        mdl.PdfButtons = false;
                    }




                }
                else
                {
                    mdl.freezeButton = true;
                    mdl.PassButton = false;
                }
                mdl.IsOpen = true;
                mdl.AgendaType = "ApproveProceeding";

            }
            string DeptId = CurrentSession.DeptID;
            mdl.ActionList = DiaryViewModel.GetAllActionTypeList(0, CurrentSession.UserID.ToString(), "Diary", 0);
            mdl.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "1");
            mdl.FileList = DiaryViewModel.GetMeetingFileList(CurrentSession.UserID.ToString(), CurrentSession.DeptID.ToString(), Convert.ToInt32(CurrentSession.OfficeId), 0, Convert.ToInt64(mdl.FileRefId));
            return View("ApproveProceeding", mdl);
        }




        [HttpPost]

        public ActionResult SaveAct(string ActId, string refId)
        {
            string response = "";
            try
            {
                if (refId != "")
                {
                    AgendaModelFunction.SaveAct(Convert.ToInt64(refId), ActId);
                }

            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewRef(AgendaDetailModel mdl)
        {
            //if (Session["userAdded"].ToString() != "")
            //{
            //    return RedirectToAction("DiaryAgendaDetail", "Agenda");
            //}

            Session["userExist"] = "";
            Session["userAdded"] = "";
            AgendaDetailModel model = new AgendaDetailModel();

            //upload action file 
            if (model.IsFile == "Y")
            {
                model.attachfile = model.attachfile;
            }
            else
            {
                DataSet ds = new DataSet();
                string agendaDate = "";
                ds = AgendaModelFunction.GetAgendabyId(mdl.HdnAgendaId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    agendaDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["AgendaDate"]).ToString("ddmmyyyy");
                }
                else
                {
                    agendaDate = "";
                }

                string urlA = "/Agenda/" + agendaDate + "/";
                string directoryA = "~/MlaDiary/Agenda/" + agendaDate;
                directoryA = Server.MapPath(directoryA);
                if (!System.IO.Directory.Exists(directoryA))
                {
                    System.IO.Directory.CreateDirectory(directoryA);
                }


                string tempurlA1 = "~/MlaDiary/TempFile";
                string tempdirectoryA1 = Server.MapPath(tempurlA1);
                //Save ActionTaken File
                if (!System.IO.Directory.Exists(tempdirectoryA1))
                {
                    System.IO.Directory.CreateDirectory(tempdirectoryA1);
                }
                if (Directory.Exists(directoryA))// && (model.ActiontakenDate != null ))
                {
                    string[] savedFileName = Directory.GetFiles(Server.MapPath(tempurlA1));
                    if (savedFileName.Length > 0)// && (model.ActiontakenDate != null ))   //&& model.ActiontakenDate != null && model.ActiontakenDate != ""
                    {
                        string SourceFile = savedFileName[0];
                        foreach (string page in savedFileName)
                        {

                            // string path1 = System.IO.Path.Combine(directory, FileName1 + "_" + nametwo.Replace(" ", ""));

                            string ext = Path.GetExtension(SourceFile);
                            string name = Path.GetFileName(page);
                            // string path1 = System.IO.Path.Combine(directoryA, model.RecordNumber.ToString() + ext);
                            var fname = Guid.NewGuid().ToString();
                            string path1 = System.IO.Path.Combine(directoryA, fname + ext);
                            if (!string.IsNullOrEmpty(name))
                            {
                                //  System.IO.File.Delete(System.IO.Path.Combine(directoryA, model.ActiontakenFile));
                                System.IO.File.Copy(SourceFile, path1, true);
                                model.attachfile = urlA + fname + ext;
                                mdl.attachfile = urlA + fname + ext;
                            }
                        }


                    }
                    else
                    {
                        model.attachfile = null;
                        //   return Json("Please select Action Taken file", JsonRequestBehavior.AllowGet);
                        //  TempData["Msg"] = "Please select Action Taken file";
                        // return RedirectToAction("Index");
                    }
                }
                else
                {
                    model.attachfile = null;
                }
                if (Directory.Exists(tempdirectoryA1))   //tempurlA1
                {
                    string[] filePaths = Directory.GetFiles(Server.MapPath(tempurlA1));
                    foreach (string filePath in filePaths)
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

            }


            model.NewSubject = mdl.NewSubject;
            model.Createdby = CurrentSession.UserID;
            model.attachfile = mdl.attachfile;
            model.AgendaId = mdl.HdnAgendaId;
            model.NewSubjectDetails = mdl.NewSubjectDetails;
            model.NewLetterNo = mdl.NewLetterNo;
            model.HdnAgendaId = 0;
            model.AgendaType = mdl.AgendaType;
            model.IsOpen = true;
            model.freezeButton = true;
            string msg = AgendaModelFunction.SaveNewRefAgenda(model);
            if (msg == "0")
            {
                Session["userAdded"] = "New Agenda Added Successfully.";
                Session["userExist"] = "";

            }
            else
            {
                Session["userExist"] = "Agenda not added";
                Session["userAdded"] = "";

            }

            ViewBag.OfficeName = CurrentSession.OfficeName;
            int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            string DeptId = CurrentSession.DeptID;
            model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
            model._LstReferences = AgendaModelFunction.GetDishpatchReferences(model.AgendaId);
            model.Count = model._LstReferences.Count.ToString();
            if (model.AgendaType == "Diary")
            {
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
                return View("DiaryAgendaDetail", model);
            }
            else if (model.AgendaType == "Dispatch")
            {
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
                return View("DispatchAgendaDetail", model);
            }
            else if (model.AgendaType == "Approve")
            {
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "1");
                return View("ApproveAgendaDetail", model);
            }
            else if (model.AgendaType == "ApproveProceeding")
            {
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "1");
                return View("ApproveProceeding", model);
            }
            else
            {
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
                return View("DispatchAgendaDetail", model);
            }

        }


        public ActionResult EditNewRef(AgendaDetailModel mdl)
        {
            //if (Session["userAdded"].ToString() != "")
            //{
            //    return RedirectToAction("DiaryAgendaDetail", "Agenda");
            //}

            Session["userExist"] = "";
            Session["userAdded"] = "";
            AgendaDetailModel model = new AgendaDetailModel();

            //upload action file 
            if (model.IsFile == "Y")
            {
                model.attachfile = model.attachfile;
            }
            else
            {
                DataSet ds = new DataSet();
                string agendaDate = "";
                ds = AgendaModelFunction.GetAgendabyId(mdl.HdnAgendaId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    agendaDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["AgendaDate"]).ToString("ddmmyyyy");
                }
                else
                {
                    agendaDate = "";
                }

                string urlA = "/Agenda/" + agendaDate + "/";
                string directoryA = "~/MlaDiary/Agenda/" + agendaDate;
                directoryA = Server.MapPath(directoryA);
                if (!System.IO.Directory.Exists(directoryA))
                {
                    System.IO.Directory.CreateDirectory(directoryA);
                }


                string tempurlA1 = "~/MlaDiary/TempFile";
                string tempdirectoryA1 = Server.MapPath(tempurlA1);
                //Save ActionTaken File
                if (!System.IO.Directory.Exists(tempdirectoryA1))
                {
                    System.IO.Directory.CreateDirectory(tempdirectoryA1);
                }
                if (Directory.Exists(directoryA))// && (model.ActiontakenDate != null ))
                {
                    string[] savedFileName = Directory.GetFiles(Server.MapPath(tempurlA1));
                    if (savedFileName.Length > 0)// && (model.ActiontakenDate != null ))   //&& model.ActiontakenDate != null && model.ActiontakenDate != ""
                    {
                        string SourceFile = savedFileName[0];
                        foreach (string page in savedFileName)
                        {

                            // string path1 = System.IO.Path.Combine(directory, FileName1 + "_" + nametwo.Replace(" ", ""));

                            string ext = Path.GetExtension(SourceFile);
                            string name = Path.GetFileName(page);
                            // string path1 = System.IO.Path.Combine(directoryA, model.RecordNumber.ToString() + ext);
                            var fname = Guid.NewGuid().ToString();
                            string path1 = System.IO.Path.Combine(directoryA, fname + ext);
                            if (!string.IsNullOrEmpty(name))
                            {
                                //  System.IO.File.Delete(System.IO.Path.Combine(directoryA, model.ActiontakenFile));
                                System.IO.File.Copy(SourceFile, path1, true);
                                model.attachfile = urlA + fname + ext;
                                mdl.attachfile = urlA + fname + ext;
                            }
                        }


                    }
                    else
                    {
                        model.attachfile = null;
                        //   return Json("Please select Action Taken file", JsonRequestBehavior.AllowGet);
                        //  TempData["Msg"] = "Please select Action Taken file";
                        // return RedirectToAction("Index");
                    }
                }
                else
                {
                    model.attachfile = null;
                }
                if (Directory.Exists(tempdirectoryA1))   //tempurlA1
                {
                    string[] filePaths = Directory.GetFiles(Server.MapPath(tempurlA1));
                    foreach (string filePath in filePaths)
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

            }

            model.AgendaDetailId = mdl.AgendaDetailId;
            model.NewSubject = mdl.NewSubject;
            model.Createdby = CurrentSession.UserID;
            model.attachfile = mdl.attachfile;
            model.AgendaId = mdl.HdnAgendaId;
            model.NewSubjectDetails = mdl.EditSubjectDetails;
            model.NewLetterNo = mdl.NewLetterNo;
            model.HdnAgendaId = 0;
            model.IsOpen = true;
            model.freezeButton = true;
            model.AgendaType = mdl.AgendaType;
            string msg = AgendaModelFunction.EditAgendaDetails(model);

            if (msg == "0")
            {
                Session["userAdded"] = "Agenda Updated Successfully.";
                Session["userExist"] = "";

            }
            else
            {
                Session["userExist"] = "Agenda not Updated";
                Session["userAdded"] = "";

            }

            ViewBag.OfficeName = CurrentSession.OfficeName;
            int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            string DeptId = CurrentSession.DeptID;
            model.AgendaId = mdl.HdnAgendaId;
            model._LstReferences = AgendaModelFunction.GetDishpatchReferences(model.AgendaId);
            model.Count = model._LstReferences.Count.ToString();
            if (model.AgendaType == "Diary")
            {
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
                return View("DiaryAgendaDetail", model);
            }
            else if (model.AgendaType == "Dispatch")
            {
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
                return View("DispatchAgendaDetail", model);
            }
            else if (model.AgendaType == "Approve")
            {
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "1");
                return View("ApproveAgendaDetail", model);
            }
            else
            {
                model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "0");
                return View("DispatchAgendaDetail", model);
            }
        }


        public static void DecisionSavePDf(string Deptid, string AgendaId, string filepath, int adid)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveDeptDecisionPdf(Convert.ToInt32(Deptid), Convert.ToInt32(AgendaId), filepath, adid);
            }

        }

        public static void SavePDf(string AgendaId, string filepath)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveAgendaPdf(Convert.ToInt32(AgendaId), filepath);
            }

        }

        public static void SavePDfCircular(string AgendaId, string filepath)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveAgendaPdfCircular(Convert.ToInt32(AgendaId), filepath);
            }

        }

        public static void SavePDfProceedingCircular(string AgendaId, string filepath)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveProceedingPdfCircular(Convert.ToInt32(AgendaId), filepath);
            }

        }
        public static void SavePDf2(string AgendaId, string filepath, string ReportType)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveAgendaPdf2(Convert.ToInt32(AgendaId), filepath, ReportType);
            }

        }
        public static void SavApprovalHistory(string AgendaId, string filepath, string ReportType, string UserId)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveAgendaPdf2(Convert.ToInt32(AgendaId), filepath, ReportType);
            }

        }


        public ActionResult FreezeAgenda(int AID, string RowOrder)
        {
            List<AdminLOBJson> indexTable = new List<AdminLOBJson>();
            indexTable = new JavaScriptSerializer().Deserialize<List<AdminLOBJson>>(RowOrder);


            AdminLOB lob = new AdminLOB();
            int i = 1;
            foreach (AdminLOBJson index in indexTable)
            {
                string[] result = index.Index.Split('.');
                int agendaRecordId = Convert.ToInt32(index.AgendaRecordId);
                DataSet ds = new DataSet();
                ds = AgendaModelFunction.GetAgendaRecord(agendaRecordId);
                lob.AgendaId = Convert.ToInt32(ds.Tables[0].Rows[0]["AgendaId"].ToString());
                lob.AgendaRecordId = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"].ToString());

                int indexcount;
                if (result[0] != "")
                {
                    indexcount = result.Length;
                }
                else
                {
                    indexcount = 0;
                }

                switch (indexcount)
                {
                    case 1:
                        lob.SrNo1 = Convert.ToInt32(result[0]);
                        lob.SrNo2 = 0;
                        lob.SrNo3 = 0;
                        break;
                    case 2:
                        lob.SrNo1 = Convert.ToInt32(result[0]);
                        lob.SrNo2 = Convert.ToInt32(result[1]); ;
                        lob.SrNo3 = 0;
                        break;
                    case 3:
                        lob.SrNo1 = Convert.ToInt32(result[0]);
                        lob.SrNo2 = Convert.ToInt32(result[1]);
                        lob.SrNo3 = Convert.ToInt32(result[2]);
                        break;
                    default:
                        lob.SrNo1 = 0;
                        lob.SrNo2 = 0;
                        lob.SrNo3 = 0;
                        break;
                }


                lob.SessionDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["AgendaDate"].ToString());
                lob.TextLOB = ds.Tables[0].Rows[0]["Subject"].ToString();
                lob.pdflocation = ds.Tables[0].Rows[0]["attachedfile"].ToString();
                lob.islaid = 1;
                lob.isdeleted = 0;
                lob.CreatedBy = CurrentSession.UserID.ToString();
                AgendaModelFunction.SaveAdminLOB(lob);
                i++;
            }

            return Json("0", JsonRequestBehavior.AllowGet);

        }


        public ActionResult DeleteAgenda(string AgendaId)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.DeleteAgenda(Convert.ToInt32(AgendaId));
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteAgendaDetail(string AgendaDetailId)
        {
            if (AgendaDetailId != "")
            {
                AgendaModelFunction.DeleteAgendaDetail(Convert.ToInt32(AgendaDetailId), 0);
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteUpdateAgendaDetail(string AgendaDetailId)
        {
            if (AgendaDetailId != "")
            {
                AgendaModelFunction.DeleteAgendaDetail(Convert.ToInt32(AgendaDetailId), 2);
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }

        public ActionResult LockUnlockAgenda(string AgendaId, string freeze)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.FreezeUnfreezeAgenda(Convert.ToInt32(AgendaId), Convert.ToInt32(freeze));
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }

        public ActionResult VotingAgenda(string AgdId, string IsVote)
        {
            if (AgdId != "")
            {
                AgendaModelFunction.votingAgenda(Convert.ToInt32(AgdId), Convert.ToInt32(IsVote));
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public FileResult DownloadPdf(int AgendaId)
        {
            string fileName = Guid.NewGuid() + ".pdf";
            MemoryStream workStream = new MemoryStream();
            iTextSharp.text.Document doc = new Document(iTextSharp.text.PageSize.A4, 3, 3, 3, 3);
            // Create paragraph for show in PDF file header
            Paragraph p = new Paragraph("Agenda List");
            p.Alignment = Element.ALIGN_CENTER;

            //PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;


            //Create table here for write database data
            PdfPTable pdfTab = new PdfPTable(5); // here 4 is no of column
            pdfTab.HorizontalAlignment = 1; // 0- Left, 1- Center, 2- right
            pdfTab.SpacingBefore = 20f;
            pdfTab.SpacingAfter = 20f;
            SqlConnection con = ClsConnection.GetConnection();
            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                con.Open();


            SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@AgendaId", AgendaId),
            };





            var font5 = FontFactory.GetFont(FontFactory.HELVETICA, 15, BaseColor.BLACK);



            //  PdfPCell theCell = new PdfPCell(new Paragraph("Serial", font5));
            PdfPCell theCell1 = new PdfPCell(new Paragraph("Subject", font5));
            PdfPCell theCell2 = new PdfPCell(new Paragraph("Release Date", font5));
            PdfPCell theCell3 = new PdfPCell(new Paragraph("Department", font5));
            PdfPCell theCell4 = new PdfPCell(new Paragraph("Brief Of Last Action", font5));
            PdfPCell theCell5 = new PdfPCell(new Paragraph("Status", font5));
            //  theCell.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            theCell1.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            theCell2.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            theCell3.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            theCell4.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            theCell5.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            // pdfTab.AddCell(theCell);
            pdfTab.AddCell(theCell1);
            pdfTab.AddCell(theCell2);
            pdfTab.AddCell(theCell3);
            pdfTab.AddCell(theCell4);
            pdfTab.AddCell(theCell5);
            foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mladiaryListIndex", parameterValues).Tables[0].Rows)
            {
                pdfTab.AddCell(Convert.ToString(dr["Subject"]));
                pdfTab.AddCell(Convert.ToString(dr["LetterDate"]));

                pdfTab.AddCell(Convert.ToString(dr["deptname"]));
                pdfTab.AddCell(Convert.ToString(dr["ActionDescription"]).Replace("<p>", "").Replace("</p>", "").Replace("<br>", ""));
                pdfTab.AddCell(Convert.ToString(dr["ActionNameStatus"]));
            }
            doc.Open();
            doc.Add(p);
            doc.Add(pdfTab);
            doc.Close();
            con.Close();
            //get the temp folder and file path in server
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);


            workStream.Position = 0;


            return File(workStream, "application/pdf", "AgendaList");

        }


        //AMAN Code
        public string Getpdfhtml1(int agendaid)
        {
            // AdminLOB adminLOB = new AdminLOB();

            // CultureInfo provider = CultureInfo.InvariantCulture;
            // provider = new CultureInfo("fr-FR");
            // var Date = DateTime.ParseExact(SessionDate, "d", provider);

            // DateTime sessDate = Convert.ToDateTime(Date);
            // adminLOB.SessionDate = sessDate;
            // LOBID = 0;
            LOBModel lob = new LOBModel();
            lob.ListAdminLOB = (List<AdminLOBModel>)AgendaModelFunction.GetAdminlobData(agendaid);
            string outXml = "";
            string LOBName = "";
            string SrNo1 = "";
            string SrNo2 = "";
            string SrNo3 = "";
            int SrNo1Count = 1;
            int SrNo2Count = 1;
            int SrNo3Count = 1;
            // DateTime SessionDate1 = new DateTime();

            // SiteSettings siteSettingMod = new SiteSettings();
            // siteSettingMod = (SiteSettings)Helper.ExecuteService("SiteSetting", "GetAllSiteSettings", siteSettingMod);

            //     SessionId = Convert.ToString(siteSettingMod.SessionCode); //Convert.ToInt32(siteSettingMod.SessionCode);
            //   AssemblyId = Convert.ToString(siteSettingMod.AssemblyCode);// Convert.ToInt32(siteSettingMod.AssemblyCode);
            // string CurrentSecretery = siteSettingMod.CurrentSecretery;
            /// string SubittedDate = "";
            // string SubittedTime = "";
            if (lob.ListAdminLOB != null && lob.ListAdminLOB.Count > 0)
            {
                outXml = outXml + @"<html>                           
								 <body><div style='width: 100%;'><table style='width: 100%;   border: 1px solid black; border-collapse: collapse;'>";
                for (int i = 0; i < lob.ListAdminLOB.Count; i++)
                {
                    if (i == 0)
                    {

                        // LOBID = lob.ListAdminLOB[i].LOBId;
                        // sessiondate = Convert.ToString(SummaryDataSet.Tables[0].Rows[i]["SessionDate"]);

                        //SessionDate1 = Convert.ToDateTime(lob.ListAdminLOB[i].SessionDate);
                        //string date = ConvertSQLDate(SessionDate1);
                        //date = date.Replace("/", "");
                        //date = date.Replace("-", "");
                        //LOBName = date + "_" + "LOB" + "_" + Convert.ToString(lob.ListAdminLOB[i].LOBId);


                        outXml += @"<tr>
													<td style='text-align: center; font-size: 30px; font-weight: bold;'>               
														ਪੰਜਾਬ ਰਾਜ &nbsp;" + Convert.ToString(lob.ListAdminLOB[i].AssemblyNameLocal) + @"    
													</td>
												</tr>
												<tr>
													<td style='text-align: center; font-size: 30px; font-weight: bold;text-decoration: underline;'>
																 कार्यसूची    &nbsp;                         
													</td>
												</tr>

												<tr>
													<td style='text-align: center; font-size: 30px; font-weight: bold'>
															" + Convert.ToString(lob.ListAdminLOB[i].SessionNameLocal) + @"               
													</td>
												</tr>

												<tr>
													<td style='text-align: center; font-size: 28px; font-weight: bold;'>               
														   " + Convert.ToString(lob.ListAdminLOB[i].SessionDateLocal) + @"              
													</td>
												</tr>

												<tr>
													<td style='text-align: center; font-size: 28px; font-weight: bold;text-decoration: underline;'>               
															 " + Convert.ToString(lob.ListAdminLOB[i].SessionTimeLocal) + @"              
													</td>
												</tr>";
                    }

                    if (lob.ListAdminLOB[i].SrNo1.ToString() != "0" && lob.ListAdminLOB[i].SrNo2.ToString() == "0" && lob.ListAdminLOB[i].SrNo3.ToString() == "0")
                    {

                        SrNo1 = Convert.ToString(SrNo1Count) + ".";
                        outXml += @"<tr><td style='text-align: left; font-size: 24px;'>
									 <div style='padding-left:100px;text-align:justify;'>
								  <b>  " + SrNo1 + @" </b> &nbsp;&nbsp;&nbsp;";
                        // <b> " + SrNo1 + @" </b> &nbsp; &nbsp; &nbsp; < div style = 'padding-left:20px; margin: -23px 0 0 2%;text-align:justify;' > ";
                        SrNo1Count = SrNo1Count + 1;
                        SrNo2Count = 1;


                    }
                    else if (lob.ListAdminLOB[i].SrNo1 != "0" && lob.ListAdminLOB[i].SrNo2 != "0" && lob.ListAdminLOB[i].SrNo3 == "0")
                    {

                        SrNo2 = "&nbsp;&nbsp;&nbsp;" + "(" + Convert.ToString(SrNo2Count) + ")";
                        outXml += @"<tr><td style='text-align: left; font-size: 22px;'><div style='padding-left:120px;text-align:justify;'>
	                                  <b>" + SrNo2 + @"</b>&nbsp;&nbsp;&nbsp;";
                        //<b>" + SrNo2 + @"</b>&nbsp;&nbsp;&nbsp;<div style='padding-left:15px; margin: -20px 0 0 4.5%;text-align:justify;'>";
                        SrNo2Count = SrNo2Count + 1;
                        SrNo3Count = 1;

                    }
                    else if (lob.ListAdminLOB[i].SrNo1 != "0" && lob.ListAdminLOB[i].SrNo2 != "0" && lob.ListAdminLOB[i].SrNo3 != "0")
                    {

                        SrNo3 = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "(" + Convert.ToString(ToRoman(SrNo3Count)) + ")";
                        outXml += @"<tr><td style='text-align: left; font-size: 22px;'><div style='padding-left:120px;text-align:justify;'>
	                                  <b>" + SrNo3 + @"</b>&nbsp;&nbsp;&nbsp;";
                        //<b>" + SrNo3 + @"</b>&nbsp;&nbsp;&nbsp; <div style='padding-left:15px; margin: -24px 0 0 6%;text-align:justify;'>";
                        SrNo3Count = SrNo3Count + 1;

                    }

                    string LOBText = Convert.ToString(lob.ListAdminLOB[i].TextLOB);
                    if (LOBText != "")
                    {
                        LOBText = LOBText.Replace("<p>", "");
                        LOBText = LOBText.Replace("</p>", "");
                    }


                    outXml += @"" + LOBText + "<br/>";

                    //if (lob.ListAdminLOB[i].PDFLocation != null && lob.ListAdminLOB[i].PDFLocation != "")
                    //{
                    //    string pdfpath = lob.ListAdminLOB[i].PDFLocation;
                    //    pdfpath = pdfpath.Substring(1);
                    //    pdfpath = pdfpath.Replace(" ", "%20");
                    //    outXml += @" </td> </tr> <tr><td style='text-align: right;font-size:16px;font-weight:bold'>Paper :  <a href=" + pdfpath + " target='_blank' > <img style='width: 25px; height: 25px' title='PDF' src='/Images/Common/PDF.png'  />  </a>";
                    //    outXml += @"</td></tr>";
                    //}
                    //else
                    //{
                    //outXml += @"<br/> </div></div></td></tr>";
                    outXml += @"<br/> </div></td></tr>";
                    // }

                    //    SubittedDate = Convert.ToDateTime(lob.ListAdminLOB[i].ApprovedDate).ToString("D", new CultureInfo("hi"));
                    //if (Convert.ToString(lob.ListAdminLOB[i].ApprovedTime) != "")
                    //  {
                    //      TimeSpan interval = TimeSpan.Parse(Convert.ToString(lob.ListAdminLOB[i].ApprovedTime));
                    //      DateTime time = DateTime.Today.Add(interval);
                    //      string displayTime = time.ToString("hh:mm tt");

                    //      SubittedTime = displayTime;
                    //  }
                    //  else
                    //  {
                    //      SubittedTime = "";
                    //  }

                    if (i == lob.ListAdminLOB.Count - 1)
                    {
                        //            outXml += @" <tr><td colspan='2'><br /><br /></td></tr>

                        //<tr><td>
                        //<table style='width:100%;'>
                        //<tr>
                        //<td style='width:70%;float:left;font-size: 24px;'>
                        //   <b>शिमला-171004</b>
                        //</td>
                        //<td style='width:30%;float:left;font-size: 24px;'>
                        //  <b> " + CurrentSecretery + @",  </b>
                        //</td>
                        //</tr>
                        //<br/>
                        //<tr>
                        //  <td style='width:70%;float:left;font-size: 24px;'>
                        // <b> दिनांक: " + SubittedDate + @"</b>
                        //  <b> &nbsp;&nbsp;" + SubittedTime + @"</b>
                        //</td>
                        //<td style='width:30%;float:right;text-align:right;padding-right:85px;font-size: 24px;'>
                        //  <b>  सचिव।</b>
                        //</td>
                        //</tr>
                        //</table>           
                        //	</td>
                        //	</tr>";

                        outXml += "</table></div></body> </html>";
                        // outXml += "</table></body> </html>";
                    }

                }

            }
            // lob.sessDate = sessDate;

            //lob.outXml = outXml;

            return outXml;
        }

        public string Getpdfhtml(int agendaid)
        {

            LOBModel lob = new LOBModel();
            lob.ListAdminLOB = (List<AdminLOBModel>)AgendaModelFunction.GetAdminlobData(agendaid);
            string outXml = "";
            string LOBName = "";
            string SrNo1 = "";
            string SrNo2 = "";
            string SrNo3 = "";
            int SrNo1Count = 1;
            int SrNo2Count = 1;
            int SrNo3Count = 1;
            // DateTime SessionDate1 = new DateTime();

            // SiteSettings siteSettingMod = new SiteSettings();
            // siteSettingMod = (SiteSettings)Helper.ExecuteService("SiteSetting", "GetAllSiteSettings", siteSettingMod);

            //     SessionId = Convert.ToString(siteSettingMod.SessionCode); //Convert.ToInt32(siteSettingMod.SessionCode);
            //   AssemblyId = Convert.ToString(siteSettingMod.AssemblyCode);// Convert.ToInt32(siteSettingMod.AssemblyCode);
            // string CurrentSecretery = siteSettingMod.CurrentSecretery;
            /// string SubittedDate = "";
            // string SubittedTime = "";
            if (lob.ListAdminLOB != null && lob.ListAdminLOB.Count > 0)
            {
                outXml = outXml + @"<html>                           
								 <body><div style='width: 100%;'><table style='width: 100%;'>";
                for (int i = 0; i < lob.ListAdminLOB.Count; i++)
                {
                    if (i == 0)
                    {

                        // LOBID = lob.ListAdminLOB[i].LOBId;
                        // sessiondate = Convert.ToString(SummaryDataSet.Tables[0].Rows[i]["SessionDate"]);

                        //SessionDate1 = Convert.ToDateTime(lob.ListAdminLOB[i].SessionDate);
                        //string date = ConvertSQLDate(SessionDate1);
                        //date = date.Replace("/", "");
                        //date = date.Replace("-", "");
                        //LOBName = date + "_" + "LOB" + "_" + Convert.ToString(lob.ListAdminLOB[i].LOBId);


                        outXml += @"<tr>
													<td style='text-align: center; font-size: 30px; font-weight: bold;'>               
														ਪੰਜਾਬ ਰਾਜ &nbsp;" + Convert.ToString(lob.ListAdminLOB[i].AssemblyNameLocal) + @"    
													</td>
												</tr>
												<tr>
													<td style='text-align: center; font-size: 30px; font-weight: bold;text-decoration: underline;'>
																 कार्यसूची    &nbsp;                      
													</td>
												</tr>

												<tr>
													<td style='text-align: center; font-size: 30px; font-weight: bold'>
															" + Convert.ToString(lob.ListAdminLOB[i].SessionNameLocal) + @"               
													</td>
												</tr>

												<tr>
													<td style='text-align: center; font-size: 28px; font-weight: bold;'>               
														   " + Convert.ToString(lob.ListAdminLOB[i].SessionDateLocal) + @"              
													</td>
												</tr>

												<tr>
													<td style='text-align: center; font-size: 28px; font-weight: bold;text-decoration: underline;'>               
															 " + Convert.ToString(lob.ListAdminLOB[i].SessionTimeLocal) + @"              
													</td>
												</tr>";
                    }

                    if (lob.ListAdminLOB[i].SrNo1.ToString() != "0" && lob.ListAdminLOB[i].SrNo2.ToString() == "0" && lob.ListAdminLOB[i].SrNo3.ToString() == "0")
                    {

                        SrNo1 = Convert.ToString(SrNo1Count) + ".";
                        outXml += @"<tr><td style='text-align: left; font-size: 24px;'>
									 <div style='padding-left:100px;text-align:justify;'>

                                  <div style='float:left;width:5%'><b>" + SrNo1 + @"</b></div><div style='float:left;width:90%;'>";

                        SrNo1Count = SrNo1Count + 1;
                        SrNo2Count = 1;


                    }
                    else if (lob.ListAdminLOB[i].SrNo1 != "0" && lob.ListAdminLOB[i].SrNo2 != "0" && lob.ListAdminLOB[i].SrNo3 == "0")
                    {

                        SrNo2 = "&nbsp;&nbsp;&nbsp;" + "(" + Convert.ToString(SrNo2Count) + ")";
                        outXml += @"<tr><td style='text-align: left; font-size: 22px;'>
                                   <div style='padding-left:120px;text-align:justify;'>
                                  <div style='float:left;width:10%'><b>&nbsp;&nbsp;&nbsp;" + SrNo2 + @"</b></div><div style='float:left;width:80%;padding-left:5px;'>";
                        //<b>" + SrNo2 + @"</b>&nbsp;&nbsp;&nbsp;";
                        //<b>" + SrNo2 + @"</b>&nbsp;&nbsp;&nbsp;<div style='padding-left:15px; margin: -20px 0 0 4.5%;text-align:justify;'>";
                        SrNo2Count = SrNo2Count + 1;
                        SrNo3Count = 1;

                    }
                    else if (lob.ListAdminLOB[i].SrNo1 != "0" && lob.ListAdminLOB[i].SrNo2 != "0" && lob.ListAdminLOB[i].SrNo3 != "0")
                    {

                        SrNo3 = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "(" + Convert.ToString(ToRoman(SrNo3Count)) + ")";
                        outXml += @"<tr><td style='text-align: left; font-size: 22px;'>
                                  <div style='padding-left:140px;text-align:justify;'>
                        <div style='float:left;width:15%'><b>&nbsp;&nbsp;&nbsp;" + SrNo3 + @"</b></div><div style='float:left;width:70%;'>";
                        //<b>" + SrNo3 + @"</b>&nbsp;&nbsp;&nbsp;";
                        //<b>" + SrNo3 + @"</b>&nbsp;&nbsp;&nbsp; <div style='padding-left:15px; margin: -24px 0 0 6%;text-align:justify;'>";
                        SrNo3Count = SrNo3Count + 1;

                    }

                    string LOBText = Convert.ToString(lob.ListAdminLOB[i].TextLOB);
                    if (LOBText != "")
                    {
                        LOBText = LOBText.Replace("<p>", "");
                        LOBText = LOBText.Replace("</p>", "");
                    }


                    outXml += @"" + LOBText + "<br/>";


                    outXml += @"<br/></div></div></td></tr>";


                    if (i == lob.ListAdminLOB.Count - 1)
                    {


                        outXml += "</table></div></body> </html>";

                    }

                }

            }
            // lob.sessDate = sessDate;

            //lob.outXml = outXml;

            return outXml;
        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "m" + ToRoman(number - 1000);
            if (number >= 900) return "cm" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "d" + ToRoman(number - 500);
            if (number >= 400) return "cd" + ToRoman(number - 400);
            if (number >= 100) return "c" + ToRoman(number - 100);
            if (number >= 90) return "xc" + ToRoman(number - 90);
            if (number >= 50) return "l" + ToRoman(number - 50);
            if (number >= 40) return "xl" + ToRoman(number - 40);
            if (number >= 10) return "x" + ToRoman(number - 10);
            if (number >= 9) return "ix" + ToRoman(number - 9);
            if (number >= 5) return "v" + ToRoman(number - 5);
            if (number >= 4) return "iv" + ToRoman(number - 4);
            if (number >= 1) return "i" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }



        public string GetDepDecisionNewpdfhtml(int agendaid, int deptid)
        {

            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();



            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetDeptlobDataForProcedingPDF(agendaid, deptid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var path1 = BlobStorage.GetStorageAcessingPath();
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            string signature = path1 + "/Signature/" + CurrentSession.SignaturePath;

            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>NextGen e-Cabinet: ApprovedAgendaReport</title>                                   
                                        <style>
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            table{
                                            width:100%;
                                            }
                                       #bloc1
                       {
                        float:left;
						width: 160px;
                       }
					   #bloc2
					   {
					   text-align: center;
					   }
                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");
            if (HeaderlistofAgenda != null)
            {
                outXml += @"<div style='margin: 5px 10px;page-break-before: always;'>
                  <div id='bloc1'> 
                  ਕੈਬਨਿਟ ਮੀਟਿੰਗ<br>        	                 	    	                                                                
                 ਸਪਲੀਮੈਂਟਰੀ ਅਜੰਡਾ-1<br>	         		 	 			                                                 
                 ਗੁਪਤ<br>             
                 </div>  
                 <div id='bloc2'>
	             <span>ਪੰਜਾਬ ਸਰਕਾਰ </span><br/>
	             <span>ਆਮ ਰਾਜ ਪ੍ਰਬੰਧ ਵਿਭਾਗ </span><br/>
	             <span>(ਮੰਤਰੀ ਮੰਡਲ ਮਾਮਲੇ ਸ਼ਾਖਾ)</span><br/>
	             </div>";
                outXml += @"<p style='font-size: 12pt; text-indent: 42px;'>"
                 + HeaderlistofAgenda.Header +
" </p>";
            }
            /*   "ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਮਿਤੀ ... ਦਿਨ …ਵਾਰ ਨੂੰ ਸਵੇਰੇ ..:00 ਵਜੇ, ਸਥਾਨ......... ਵਿਖੇ ਹੋਣ ਵਾਲੀ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖੀਆਂ ਆਈਟਮਾਂ ਤੇ ਵਿਚਾਰ ਕੀਤਾ ਜਾਵੇਗਾ:-*/
            outXml += @"<table style='font-size: 12pt; border: 1px solid black;border-collapse: collapse;'>";
            outXml += @"<tr style = 'padding:5px;'>
             <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse; width:5%;'> ਮੀਟਿੰਗ ਦਾ ਆਈਟਮ ਨੰ.</th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;  width:10%;'> ਸਾਲਾਨਾ ਆਈਟਮ ਨੰ.</th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;'> ਵਿਸ਼ਾ </th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;width:15%;'> ਸਬੰਧਤ ਮੰਤਰੀ / ਸਕੱਤਰ </th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;width:15%;' > ਵਿਭਾਗ ਜਿਨ੍ਹਾਂ ਦੀ ਸਲਾਹ ਲਈ ਗਈ </th>";
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + mdl._LstReferences[i].SrNo + @"    
										</td>
	                                     <td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + mdl._LstReferences[i].SrNo + @"    
										</td>
                                       
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>                                       
                     <td style = 'padding:10px; border: 1px solid black;border - collapse: collapse;'>   
                        " + mdl._LstReferences[i].MinisterName + @"  
                                        </td>
                        
                     <td style = 'padding:10px; border: 1px solid black;border - collapse: collapse;'>                                      
                                        </td></tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {
                        outXml += "</table>";
                        outXml += "<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += "ਕੇ ਏ ਪੀ  ਸਿਨਹਾ";
                        outXml += "<br/>";
                        outXml += "ਮੁੱਖ ਸਕੱਤਰ, ਪੰਜਾਬ ਸਰਕਾਰ";
                        outXml += "</div>";

                        //if (HeaderlistofAgenda != null)
                        //{
                        //    outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                        //                 " + HeaderlistofAgenda.Footer + "</p>";
                        //}
                        outXml += "<div class='maindiv'>";
                        outXml += "<div style='float:left;width: 70%;'> ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>";
                        outXml += "<div style ='float:right;width: 30%;text-align: end;'> ਮਿਤੀ:" + gt.ToString() + @"</div>";
                        outXml += "</div>";

                        outXml += "<p style='font-size: 12pt; text-indent: 42px;'>";
                        outXml += @"ਉਤਾਰਾ ਹੇਠ ਲਿਖਿਆਂ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਸ਼ਾਮਲ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ:-</p>
	  <p style = 'font-size: 12pt; text-indent: 50px;'>
       <ol>
       <li>
       <span style='padding-left: 50px;'>ਮੁੱਖ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	   </li>
	   <li>
	   <span style = 'padding-left: 50px;' > ਸਮੂਹ ਕੈਬਨਿਟ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	   </li>
	   <li>
	   <span style = 'padding-left: 50px;' > ਵਿਸ਼ੇਸ਼ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ/ਮੁੱਖ ਮੰਤਰੀ</span>
	   </li></ol>
	   </p>
	   <div style='text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt= 'Signature' style= 'height:50px;'><br/>";
                        outXml += @" ਸੁਪਰਡੰਟ
            </div>
            <div class='maindiv'>
			<div style ='float:left;width: 70%;'> ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>
			<div style ='float:right;width: 30%;text-align: end;'> ਮਿਤੀ:" + gt.ToString() + @"</div>			
			</div>			
 <p style = 'font-size: 12pt; text-indent: 42px;'>
 ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ। 
      </p>	  
	   <div style = 'text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += @" ਸੁਪਰਡੰਟ
            </div>
			<div class='maindiv'>
			<div style = 'float:left;width: 70%;' > ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>
			<div style = 'float:right;width: 30%;text-align: end;' > ਮਿਤੀ:" + gt.ToString() + @"</div>			
			</div>			
 <p style='font-size: 12pt; text-indent: 42px;'>
     ਉਤਾਰਾ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਵਿਸ਼ੇਸ਼ ਸਕੱਤਰ/ਮੁੱਖ ਸਕੱਤਰ ਨੂੰ ਮੁੱਖ ਸਕੱਤਰ ਪੰਜਾਬ ਜੀ ਦੀ ਸੂਚਨਾ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।	
      </p>
	   <div style ='text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += @" ਸੁਪਰਡੰਟ</div>
			<div class='maindiv'>
			<div style = 'float:left;width: 70%;' > ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>
			<div style = 'float:right;width: 30%;text-align: end;' > ਮਿਤੀ: " + gt.ToString() + @"</div>			
			</div>			
 <p style = 'font-size: 12pt; text-indent: 42px;'>
     ਉਤਾਰਾ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਵਧੀਕ ਮੁੱਖ ਸਕੱਤਰ/ਰਾਜਪਾਲ ਨੂੰ ਰਾਜਪਾਲ ਪੰਜਾਬ ਜੀ ਦੀ ਸੂਚਨਾ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।		
      </p>
  <div style ='text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += " ਸੁਪਰਡੰਟ</div>";
                    }
                }
                outXml += "</div></div></body> </html>";
            }
            return outXml;


        }

        public string GetCircularhtmlPatiala(int agendaid, ref string meetingDate)
        {

            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();

            //councillorlis start
            Councillorslist cl = new Councillorslist();
            string deptid = string.Empty;
            Int32 mcid = 0;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            if (CurrentSession.StateId != null)
            {
                mcid = Convert.ToInt32(CurrentSession.StateId);
            }

            cl.Councillor_list = AgendaModelFunction.getPresentCouncillirslist(mcid, agendaid);
            //end

            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);

            Authoritylist cl1 = new Authoritylist();
            cl1.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);


            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForProcedingPDF(agendaid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            var path1 = BlobStorage.GetStorageAcessingPath();

            string logo = "";

            var dptobj = AgendaModelFunction.GetDeptData(deptid);
            var deptname = CurrentSession.DeptName;

            meetingDate = HeaderlistofAgenda.AgendaDate.ToString();

            if (dptobj != null)
            {
                if (dptobj.Deptname.Contains("Local"))
                {
                    logo = path1 + "/LG/" + CurrentSession.MCName + "/" + dptobj.logo;
                }
                else
                {
                    logo = path1 + "/MC/" + dptobj.Name + "/" + dptobj.logo;
                }
            }


            string signature = path1 + "/" + CurrentSession.SignaturePath;
            string outXml = "";
            // if(mdl.)


            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: ProceedingReport</title>                                    
                                        <style>


                                        table.maintable {
                                           width: 100%;
                                         border-collapse: collapse;      
                                         border-bottom: 1px solid black;
                                         border-left: 1px solid black;
                                         border-right: 1px solid black;
                                         }

tfoot {
  display: table-footer-group; /* forces this row to appear at bottom of each page */
}


table.maintable tr1,table.maintable tr td1{
 page-break-inside: avoid;
}



                                        body {
                                          page-break-inside: avoid;
                                          overflow: visible;
                                          word-wrap: break-word;
                                        }
                                    .MsoNormalTable tr,.MsoNormalTable td,.MsoNormalTable th,.MsoTableGrid tr,.MsoTableGrid td,.MsoTableGrid th,#tblfooter tr{
                                  page-break-inside: avoid !important;                                                                          
                                    }  
                                    .pdf-class .MsoNormal{
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            table{
                                            width:100%;
                                            }
                                          ol li {
                                          padding: 5px;
                                              }

                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";
            var firstResolutionNo = "";
            var lastResolutionNo = "";
            if (mdl._LstReferences.Count > 1)
            {

                firstResolutionNo = mdl._LstReferences[0].SerialNo.ToString();
                lastResolutionNo = mdl._LstReferences[mdl._LstReferences.Count - 1].SerialNo.ToString();
            }

            if (officetype == "FNCC")
            {
                outXml += @"<div style='font-size: 14pt;font-weight: bold;width: 100%;'>
                <div style='float:left;width: 100px;'>
                <img src='" + logo + @"' height='50' width='55' /></div>
                <div style='text-align: center;'><span style='border-bottom: 1px solid;'>ਦਫਤਰ  " + mdl._LstReferences[0].FromDeptLocal + @" </span><br>
                     <span style='border-top: 1px solid;'>OFFICE OF " + mdl._LstReferences[0].FromDept.ToUpper() + @"</span></div></div>
                               
                            <br> <br>  
                            <p style='display: inline; font-size: 12pt;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p>

                   <div style='margin-bottom:10px;'><ol style='margin: 14px 65px; font-size:12pt;'>";
                outXml += @"<li> ਮਾਨਯੋਗ ਮੇਅਰ ਸਾਹਿਬ </li>";

                for (int j = 0; j < cl1.AuthorityId_list.Count; j++)
                {
                    outXml += @"<li>" + cl1.AuthorityId_list[j].name + ", </li>";
                }
                outXml += "<span>&nbsp;&nbsp;</span>" + mdl._LstReferences[0].FromDeptLocal + "।";
                outXml += @"</ol>";

                outXml += @"<p style='font-size: 12pt;'> <b>ਨੰਬਰ:.................</b></p>
                            <p style='font-size: 12pt;'> <b>ਮਿਤੀ:.................</b></p>";
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ " + HeaderlistofAgenda.StartTime + @"ਵਜੇ ਹੋਈ ਐਫ ਐਂਡ ਸੀ.ਸੀ.ਦੀ ਮੀਟਿੰਗ ਦੀ ਕਾਰਵਾਈ ਭੇਜਣ ਸਬੰਧੀ।                              
                                </span></p>
                                <p style='text-align: justify; font-size: 12pt;line-height:1.4'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                                ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਬੇਨਤੀ ਹੈ ਕਿ ਨਗਰ ਨਿਗਮ ਪਟਿਆਲਾ ਦੀ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + @" ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime
                                + @" ਵਜੇ ਹੋਈ ਐਫ ਐਂਡ ਸੀ.ਸੀ. ਦੀ ਮੀਟਿੰਗ ਦੀ ਕਾਰਵਾਈ ਦੀ ਕਾਪੀ (ਮਤਾ ਨੰ: " + firstResolutionNo + "-" + lastResolutionNo + @") ਆਪ ਜੀ ਨੂੰ ਸੂਚਨਾ ਅਤੇ ਅਗਲੇਰੀ ਯੋਗ ਕਾਰਵਾਈ ਹਿੱਤ ਘੱਲੀ ਜਾਂਦੀ ਹੈ ਜੀ।
                        <br>
                        <br>                          
                                </p>
                                <p style='margin: auto; font-size: 12pt;text-align:right;'> 
                       ਸਕੱਤਰ ,<br> 
                                  ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।
                                </p>";
                outXml += @"<div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style='font-size: 12pt;float: left;width: 50%;'> <b>ਪਿੱਠ ਅੰਕਣ ਨੰ :.................</b></p>
                                <p style='font-size: 12pt;float: right;width: 50%;text-align: right;'> <b>ਮਿਤੀ:.................</b></p> 
                            </div>";
                outXml += @"<p style='text-align: justify; font-size: 12pt;line-height:1.4'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                                ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਮਾਨਯੋਗ ਸੰਯੁਕਤ ਕਮਿਸ਼ਨਰਜ਼/ਨਿਗਰਾਨ ਇੰਜੀਨੀਅਰ ਅਰਬਨ ਅਤੇ ਰੂਰਲ/ਐਮ.ਟੀ.ਪੀ./ਸਹਾਇਕ ਕਮਿਸ਼ਨਰਜ਼/ਹੈਲਥ ਅਫਸਰ/ਡੀ.ਸੀ.ਐਫ.ਏ./ਏ.ਡੀ.ਐਫ.ਓ./ਸਬੰਧਤ ਸ਼ਾਖਾਵਾਂ ਦੇ ਸੁਪਰਡੰਟਾਂ ਨੂੰ 
                                ਸੂਚਨਾ ਅਤੇ ਅਗਲੇਰੀ ਯੋਗ ਕਾਰਵਾਈ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।
                        
                        <br> </p>
                                <p style='margin: auto; font-size: 12pt;text-align:right;page-break-after: always;'> 
                       ਸਕੱਤਰ ,<br> 
                                  ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।
                                </p>";
            }



            //this is normal parts as used in preview and approved
            if (HeaderlistofAgenda != null && mdl._LstReferences.Count > 0 && cl1.AuthorityId_list.Count > 0)
            {
                outXml += @"<div style='font-size: 14pt;font-weight: bold;width: 100%;'>
                <div style='float:left;width: 100px;'>
                <img src='" + logo + @"' height='50' width='55' /></div>
                <div style='text-align: center;'><span style='border-bottom: 1px solid;'>ਦਫਤਰ  " + mdl._LstReferences[0].FromDeptLocal + @" </span><br>
                     <span style='border-top: 1px solid;'>OFFICE OF " + mdl._LstReferences[0].FromDept.ToUpper() + @"</span></div></div>";

                outXml += @" <div>";
                outXml += @"<p style='font-size: 16px;margin-top: 30px;line-height:1.7'>";
                if (officetype == "FNCC")
                    outXml += "ਨਗਰ ਨਿਗਮ ਪਟਿਆਲਾ ਦੀ ਫਾਇਨਾਂਸ ਅਤੇ ਕੰਟਰੈਕਟ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਜੋ ਕਿ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ ਮਾਨਯੋਗ ਮੇਅਰ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਜੀ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਦੇ ਦਫਤਰ ਵਿਖੇ ਹੋਈ, ਜਿਸ ਵਿੱਚ ਉਨ੍ਹਾਂ ਤੋਂ ਇਲਾਵਾਂ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਹਿਬਾਨ ਨੇ ਹਿੱਸਾ ਲਿਆ:-";
                // outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ। ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—  </p></div>";
                else
                    outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ। ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—   </p></div>";

            }

            //      if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
            //   {
            // if (cl.Councillor_list[0].meetingType == "FNCC")
            if (officetype == "FNCC")
            {
                outXml += @"<ol>";
                if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
                {
                    for (int j = 0; j < cl.Councillor_list.Count; j++)
                    {
                        if (cl.Councillor_list[j].Designation.ToString().Trim() != "ਮੇਅਰ")
                        {
                            outXml += @"<li>" + cl.Councillor_list[j].councillorname + ",<br> ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                        }
                    }
                }
                else
                {
                    outXml += @"<li> ਸ਼੍ਰੀ ਪਰਮਵੀਰ ਸਿੰਘ, ਆਈ ਏ ਐਸ., <br>
                                    ਕਮਿਸ਼ਨਰ, ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ। </li>";
                    outXml += @"<li> ਸ਼੍ਰੀ ਹਰਿੰਦਰ ਕੋਹਲੀ, ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ, <br>
                                    ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                    outXml += @"<li> ਸ਼੍ਰੀ ਜਗਦੀਪ ਸਿੰਘ ਰਾਏ, ਡਿਪਟੀ ਮੇਅਰ, <br>
                                    ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                }

                outXml += @"</ol>";

                outXml += @"<p> ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖਿਆਂ ਮੱਦਾਂ ਦੇ ਵਿਚਾਰ ਕਰਨ ਉਪਰੰਤ ਫੈਂਸਲੇ ਕੀਤੇ ਗਏ:— </p><br>";
            }
            else
            {
                outXml += @" <table id='maintable' style='width:100%;font-size: 12pt; border: 1px solid black; border-collapse: collapse;'>";
                outXml += @"<tr>                    
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                    </tr>";
                for (int j = 0; j < cl.Councillor_list.Count; j++)
                {
                    //first list of mayor that donot under mayor so skip first record
                    if (j == 0)
                    {
                        j = j + 1;
                    }
                    outXml += @"<tr>
                      <td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                    outXml += @"" + cl.Councillor_list[j].wardid;
                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'> "
                    + cl.Councillor_list[j].councillorname + "</td>";
                    j = j + 1;
                    if (cl.Councillor_list.Count > j)
                    {
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";


                        outXml += @"" + cl.Councillor_list[j].wardid;

                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; ' > " + cl.Councillor_list[j].councillorname + "</td>";
                    }
                    else
                    {
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += "</tr>";
                        break;
                    }
                    j = j + 1;
                    if (cl.Councillor_list.Count > j)
                    {
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                        outXml += @"" + cl.Councillor_list[j].wardid;
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'>"
                       + cl.Councillor_list[j].councillorname + "</td>";
                    }
                    else
                    {
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += "</tr>";
                        break;
                    }
                    outXml += "</tr>";
                }
                outXml += "</table><br>";
            }
            // }
            if (officetype == "FNCC")
            {
                outXml = outXml + @"<div>";
            }
            else
            {
                outXml = outXml + @"<div style='page-break-before: always;'>";
            }
            //outXml = outXml + @"<div>";
            if (mdl._LstExtraReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstExtraReferences.Count; i++)
                {
                    outXml += @"<p>" + Convert.ToString(mdl._LstExtraReferences[i].SubjectDetails) + "</p><br>";
                }
            }
            outXml += @"</div> <table class='maintable' style='width:100%;font-size: 12pt;'>";
            outXml += @"<thead><tr> <td colspan='5'></td> </tr></thead>";
            outXml += @"<tfoot> <tr><td colspan='5'' style='border-bottom: 1px solid black;'></td> </tr></tfoot>";

            outXml = outXml + @"<tbody><tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Sr. No</th>
                  <th style ='padding:10px;border: 1px solid black;border - collapse: collapse;'>Resolution No.</th> 
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Date of Meeting</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Resolution Description </th>";
            if (officetype == "FNCC")
            {
                outXml += @"<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Orders of FNCC</th>";
            }
            else
            {
                outXml += @"<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;width: 15%;'>Orders of Competent Authority</th>";
            }
            outXml += @"</tr>";
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {

                    if (mdl._LstReferences[i].TableItem == "0")
                    {
                        if (j == 0)
                        {
                            outXml += @"<tr>
										<td colspan='5' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
												Table Items</td></tr>";
                        }
                        j = j + 1;
                    }
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + Convert.ToString(i + 1) + @"    
										</td>
										<td style='padding: 10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(mdl._LstReferences[i].SerialNo) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    // Replace searchText with a new line character
                    //LOBText = LOBText.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "</br>");

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>
                                       " + Convert.ToString(mdl._LstReferences[i].ActionDescription) + @"   
                                        </td>                                        
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</tbody></table>";
                        outXml += @"<br/><br/>";
                        //outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                        //if (HeaderlistofAgenda != null)
                        //{
                        var mayorsign = "";
                        var comsign = "";
                        var srdeptmayorsign = "";
                        var depmayor = "";

                        if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
                        {
                            for (int m = 0; m < cl.Councillor_list.Count; m++)
                            {
                                if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਮੇਅਰ")
                                {
                                    mayorsign = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                                }
                                else if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਕਮਿਸ਼ਨਰ")
                                {
                                    comsign = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                                }
                                else if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ")
                                {
                                    srdeptmayorsign = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                                }
                                else if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਡਿਪਟੀ ਮੇਅਰ")
                                {
                                    depmayor = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                                }
                            }
                        }



                        outXml += @" <table id='tblfooter' style = 'width:100%;font-size: 14px;margin-top: 40px;' > ";
                        outXml += @"<tr><td style='padding:10px;'> <div style='padding-top:5px'>";
                        if (mayorsign != "")
                            outXml += @"<img src='" + mayorsign + @"'alt='Signature' style='height:50px;'>";
                        else
                            outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                        outXml += @"</div> ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                                        </td>
										<td style='padding: 10px;'>
<div style='padding-top:5px'>";
                        if (comsign != "")
                            outXml += @"<img src='" + comsign + @"'alt='Signature' style='height:50px;'>";
                        else
                            outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                        outXml += @"</div>  ਕਮਿਸ਼ਨਰ  <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                                        </td>
                                        <td style ='padding:10px;'>
<div style='padding-top:5px'>";
                        if (srdeptmayorsign != "")
                            outXml += @"<img src='" + srdeptmayorsign + @"'alt='Signature' style='height:50px;'>";
                        else
                            outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                        outXml += @"</div>
                                       ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                                        </td>
                                        <td style ='padding:10px;width:15%'>
<div style='padding-top:5px'>";
                        if (depmayor != "")
                            outXml += @"<img src='" + depmayor + @"'alt='Signature' style='height:50px;'>";
                        else
                            outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                        outXml += @"</div>
                                         ਡਿਪਟੀ ਮੇਅਰ <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                                       </td></tr></table>";

                        //outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                        //                 ਮੇਅਰ,<br>" + mdl._LstReferences[0].FromOfficeLocal + "</p>";
                        //}

                    }
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }

        public string GetCircularhtmlMohali(int agendaid, ref string meetingDate)
        {

            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();

            //councillorlis start
            Councillorslist cl = new Councillorslist();
            string deptid = string.Empty;
            Int32 mcid = 0;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            if (CurrentSession.StateId != null)
            {
                mcid = Convert.ToInt32(CurrentSession.StateId);
            }

            cl.Councillor_list = AgendaModelFunction.getPresentCouncillirslist(mcid, agendaid);
            //end

            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);

            Authoritylist cl1 = new Authoritylist();
            cl1.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);


            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForProcedingPDF(agendaid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            var path1 = BlobStorage.GetStorageAcessingPath();

            string logo = "";

            var dptobj = AgendaModelFunction.GetDeptData(deptid);
            var deptname = CurrentSession.DeptName;

            meetingDate = HeaderlistofAgenda.AgendaDate.ToString();

            if (dptobj != null)
            {
                if (dptobj.Deptname.Contains("Local"))
                {
                    logo = path1 + "/LG/" + CurrentSession.MCName + "/" + dptobj.logo;
                }
                else
                {
                    logo = path1 + "/MC/" + dptobj.Name + "/" + dptobj.logo;
                }
            }


            string signature = path1 + "/" + CurrentSession.SignaturePath;
            string outXml = "";
            // if(mdl.)


            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: ProceedingReport</title>                                    
                                        <style>


                                        table.maintable {
                                           width: 100%;
                                         border-collapse: collapse;      
                                         border-bottom: 1px solid black;
                                         border-left: 1px solid black;
                                         border-right: 1px solid black;
                                         }

tfoot {
  display: table-footer-group; /* forces this row to appear at bottom of each page */
}


table.maintable tr1,table.maintable tr td1{
 page-break-inside: avoid;
}



                                        body {
                                          page-break-inside: avoid;
                                          overflow: visible;
                                          word-wrap: break-word;
                                        }
                                    .MsoNormalTable tr,.MsoNormalTable td,.MsoNormalTable th,.MsoTableGrid tr,.MsoTableGrid td,.MsoTableGrid th,#tblfooter tr{
                                  page-break-inside: avoid !important;                                                                          
                                    }  
                                    .pdf-class .MsoNormal{
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            table{
                                            width:100%;
                                            }
                                          ol li {
                                          padding: 5px;
                                              }

                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";
            var firstResolutionNo = "";
            var lastResolutionNo = "";
            if (mdl._LstReferences.Count > 1)
            {

                firstResolutionNo = mdl._LstReferences[0].SerialNo.ToString();
                lastResolutionNo = mdl._LstReferences[mdl._LstReferences.Count - 1].SerialNo.ToString();
            }

            if (officetype == "FNCC")
            {
                outXml += @"<div style='font-size: 14pt;font-weight: bold;width: 100%;'>
                <div style='float:left;width: 100px;'>
                <img src='" + logo + @"' height='50' width='55' /></div>
                <div style='text-align: center;'><span style='border-bottom: 1px solid;'>ਦਫਤਰ  " + mdl._LstReferences[0].FromDeptLocal + @" </span><br>
                     <span style='border-top: 1px solid;'>OFFICE OF " + mdl._LstReferences[0].FromDept.ToUpper() + @"</span></div></div>
                               
                            <br> <br>  
                            <p style='display: inline; font-size: 12pt;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p>

                   <div style='margin-bottom:10px;'><ol style='margin: 14px 65px; font-size:12pt;'>";
                //outXml += @"<li> ਮਾਨਯੋਗ ਮੇਅਰ ਸਾਹਿਬ </li>";

                for (int j = 0; j < cl1.AuthorityId_list.Count; j++)
                {
                    outXml += @"<li>" + cl1.AuthorityId_list[j].name + ", </li>";
                }
                outXml += "<span>&nbsp;&nbsp;</span>" + mdl._LstReferences[0].FromDeptLocal + "।";
                outXml += @"</ol>";

                outXml += @"<p style='font-size: 12pt;'> <b>ਨੰਬਰ:.................</b></p>
                            <p style='font-size: 12pt;'> <b>ਮਿਤੀ:.................</b></p>";
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ " + HeaderlistofAgenda.StartTime + @"ਵਜੇ ਹੋਈ ਐਫ ਐਂਡ ਸੀ.ਸੀ.ਦੀ ਮੀਟਿੰਗ ਦੀ ਕਾਰਵਾਈ ਭੇਜਣ ਸਬੰਧੀ।                              
                                </span></p>
                                <p style='text-align: justify; font-size: 12pt;line-height:1.4'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                                ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਬੇਨਤੀ ਹੈ ਕਿ ਨਗਰ ਨਿਗਮ ਪਟਿਆਲਾ ਦੀ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + @" ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime
                                + @" ਵਜੇ ਹੋਈ ਐਫ ਐਂਡ ਸੀ.ਸੀ. ਦੀ ਮੀਟਿੰਗ ਦੀ ਕਾਰਵਾਈ ਦੀ ਕਾਪੀ (ਮਤਾ ਨੰ: " + firstResolutionNo + "-" + lastResolutionNo + @") ਆਪ ਜੀ ਨੂੰ ਸੂਚਨਾ ਅਤੇ ਅਗਲੇਰੀ ਯੋਗ ਕਾਰਵਾਈ ਹਿੱਤ ਘੱਲੀ ਜਾਂਦੀ ਹੈ ਜੀ।
                        <br>
                        <br>                          
                                </p>
                                <p style='margin: auto; font-size: 12pt;text-align:right;'> 
                       ਸਕੱਤਰ ,<br> 
                                  ਨਗਰ ਨਿਗਮ, ਮੋਹਾਲੀ।
                                </p>";
                outXml += @"<div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style='font-size: 12pt;float: left;width: 50%;'> <b>ਪਿੱਠ ਅੰਕਣ ਨੰ :.................</b></p>
                                <p style='font-size: 12pt;float: right;width: 50%;text-align: right;'> <b>ਮਿਤੀ:.................</b></p> 
                            </div>";
                outXml += @"<p style='text-align: justify; font-size: 12pt;line-height:1.4'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                                ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਮਾਨਯੋਗ ਸੰਯੁਕਤ ਕਮਿਸ਼ਨਰਜ਼/ਨਿਗਰਾਨ ਇੰਜੀਨੀਅਰ ਅਰਬਨ ਅਤੇ ਰੂਰਲ/ਐਮ.ਟੀ.ਪੀ./ਸਹਾਇਕ ਕਮਿਸ਼ਨਰਜ਼/ਹੈਲਥ ਅਫਸਰ/ਡੀ.ਸੀ.ਐਫ.ਏ./ਏ.ਡੀ.ਐਫ.ਓ./ਸਬੰਧਤ ਸ਼ਾਖਾਵਾਂ ਦੇ ਸੁਪਰਡੰਟਾਂ ਨੂੰ 
                                ਸੂਚਨਾ ਅਤੇ ਅਗਲੇਰੀ ਯੋਗ ਕਾਰਵਾਈ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।
                        
                        <br> </p>
                                <p style='margin: auto; font-size: 12pt;text-align:right;page-break-after: always;'> 
                       ਸਕੱਤਰ ,<br> 
                                  ਨਗਰ ਨਿਗਮ, ਮੋਹਾਲੀ।
                                </p>";
            }



            //this is normal parts as used in preview and approved
            if (HeaderlistofAgenda != null && mdl._LstReferences.Count > 0 && cl1.AuthorityId_list.Count > 0)
            {
                outXml += @"<div style='font-size: 14pt;font-weight: bold;width: 100%;'>
                <div style='float:left;width: 100px;'>
                <img src='" + logo + @"' height='50' width='55' /></div>
                <div style='text-align: center;'><span style='border-bottom: 1px solid;'>ਦਫਤਰ  " + mdl._LstReferences[0].FromDeptLocal + @" </span><br>
                     <span style='border-top: 1px solid;'>OFFICE OF " + mdl._LstReferences[0].FromDept.ToUpper() + @"</span></div></div>";

                outXml += @" <div>";
                outXml += @"<p style='font-size: 16px;margin-top: 30px;line-height:1.7'>";
                if (officetype == "FNCC")
                    outXml += "ਨਗਰ ਨਿਗਮ ਮੋਹਾਲੀ ਦੀ ਫਾਇਨਾਂਸ ਅਤੇ ਕੰਟਰੈਕਟ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਜੋ ਕਿ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ ਮਾਨਯੋਗ ਮੇਅਰ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਜੀ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਦੇ ਦਫਤਰ ਵਿਖੇ ਹੋਈ, ਜਿਸ ਵਿੱਚ ਉਨ੍ਹਾਂ ਤੋਂ ਇਲਾਵਾਂ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਹਿਬਾਨ ਨੇ ਹਿੱਸਾ ਲਿਆ:-";
                else
                    outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ। ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—   </p></div>";

            }
            if (officetype == "FNCC")
            {
                outXml += @"<ol>";
                if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
                {
                    for (int j = 0; j < cl.Councillor_list.Count; j++)
                    {
                        if (cl.Councillor_list[j].Designation.ToString().Trim() != "ਮੇਅਰ")
                        {
                            outXml += @"<li>" + cl.Councillor_list[j].councillorname + ",<br> ਨਗਰ ਨਿਗਮ, ਮੋਹਾਲੀ।</li>";
                        }
                    }
                }
                else
                {
                    outXml += @"<li> ਸ਼੍ਰੀ ਪਰਮਵੀਰ ਸਿੰਘ, ਆਈ ਏ ਐਸ., <br>
                                    ਕਮਿਸ਼ਨਰ, ਨਗਰ ਨਿਗਮ, ਮੋਹਾਲੀ। </li>";
                    outXml += @"<li> ਸ਼੍ਰੀ ਹਰਿੰਦਰ ਕੋਹਲੀ, ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ, <br>
                                    ਨਗਰ ਨਿਗਮ, ਮੋਹਾਲੀ।</li>";
                    outXml += @"<li> ਸ਼੍ਰੀ ਜਗਦੀਪ ਸਿੰਘ ਰਾਏ, ਡਿਪਟੀ ਮੇਅਰ, <br>
                                    ਨਗਰ ਨਿਗਮ, ਮੋਹਾਲੀ।</li>";
                }

                outXml += @"</ol>";
                outXml += @"<p> ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖਿਆਂ ਮੱਦਾਂ ਦੇ ਵਿਚਾਰ ਕਰਨ ਉਪਰੰਤ ਫੈਂਸਲੇ ਕੀਤੇ ਗਏ:— </p><br>";
            }
            else
            {
                outXml += @" <table id='maintable' style='width:100%;font-size: 12pt; border: 1px solid black; border-collapse: collapse;'>";
                outXml += @"<tr>                    
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                    </tr>";
                for (int j = 0; j < cl.Councillor_list.Count; j++)
                {
                    //first list of mayor that donot under mayor so skip first record
                    if (j == 0)
                    {
                        j = j + 1;
                    }
                    outXml += @"<tr>
                      <td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                    outXml += @"" + cl.Councillor_list[j].wardid;
                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'> "
                    + cl.Councillor_list[j].councillorname + "</td>";
                    j = j + 1;
                    if (cl.Councillor_list.Count > j)
                    {
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";


                        outXml += @"" + cl.Councillor_list[j].wardid;

                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; ' > " + cl.Councillor_list[j].councillorname + "</td>";
                    }
                    else
                    {
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += "</tr>";
                        break;
                    }
                    j = j + 1;
                    if (cl.Councillor_list.Count > j)
                    {
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                        outXml += @"" + cl.Councillor_list[j].wardid;
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'>"
                       + cl.Councillor_list[j].councillorname + "</td>";
                    }
                    else
                    {
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += "</tr>";
                        break;
                    }
                    outXml += "</tr>";
                }
                outXml += "</table><br>";
            }
            // }
            if (officetype == "FNCC")
            {
                outXml = outXml + @"<div>";
            }
            else
            {
                outXml = outXml + @"<div style='page-break-before: always;'>";
            }
            //outXml = outXml + @"<div>";
            if (mdl._LstExtraReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstExtraReferences.Count; i++)
                {
                    outXml += @"<p>" + Convert.ToString(mdl._LstExtraReferences[i].SubjectDetails) + "</p><br>";
                }
            }
            outXml += @"</div> <table class='maintable' style='width:100%;font-size: 12pt;'>";
            outXml += @"<thead><tr> <td colspan='5'></td> </tr></thead>";
            outXml += @"<tfoot> <tr><td colspan='5'' style='border-bottom: 1px solid black;'></td> </tr></tfoot>";

            outXml = outXml + @"<tbody><tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Sr. No</th>
                  <th style ='padding:10px;border: 1px solid black;border - collapse: collapse;'>Resolution No.</th> 
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Date of Meeting</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Resolution Description </th>";
            if (officetype == "FNCC")
            {
                outXml += @"<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Orders of FNCC</th>";
            }
            else
            {
                outXml += @"<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;width: 15%;'>Orders of Competent Authority</th>";
            }
            outXml += @"</tr>";
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {

                    if (mdl._LstReferences[i].TableItem == "0")
                    {
                        if (j == 0)
                        {
                            outXml += @"<tr>
										<td colspan='5' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
												Table Items</td></tr>";
                        }
                        j = j + 1;
                    }
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + Convert.ToString(i + 1) + @"    
										</td>
										<td style='padding: 10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(mdl._LstReferences[i].SerialNo) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    // Replace searchText with a new line character
                    //LOBText = LOBText.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "</br>");

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>
                                       " + Convert.ToString(mdl._LstReferences[i].ActionDescription) + @"   
                                        </td>                                        
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</tbody></table>";
                        outXml += @"<br/><br/>";
                        outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                        //if (HeaderlistofAgenda != null)
                        //{
                        //var mayorsign = "";
                        //var comsign = "";
                        //var srdeptmayorsign = "";
                        //var depmayor = "";

                        //if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
                        //{
                        //    for (int m = 0; m < cl.Councillor_list.Count; m++)
                        //    {
                        //        if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਮੇਅਰ")
                        //        {
                        //            mayorsign = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                        //        }
                        //else if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਕਮਿਸ਼ਨਰ")
                        //{
                        //    comsign = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                        //}
                        //else if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ")
                        //{
                        //    srdeptmayorsign = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                        //}
                        //else if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਡਿਪਟੀ ਮੇਅਰ")
                        //{
                        //    depmayor = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                        //}
                        //    }
                        //}



                        //                        outXml += @" <table id='tblfooter' style = 'width:100%;font-size: 14px;margin-top: 40px;' > ";
                        //                        outXml += @"<tr><td style='padding:10px;'> <div style='padding-top:5px'>";
                        //                        if (mayorsign != "")
                        //                            outXml += @"<img src='" + mayorsign + @"'alt='Signature' style='height:50px;'>";
                        //                        else
                        //                            outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                        //                        outXml += @"</div> ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                                        </td>
                        //										<td style='padding: 10px;'>
                        //<div style='padding-top:5px'>";
                        //                        if (comsign != "")
                        //                            outXml += @"<img src='" + comsign + @"'alt='Signature' style='height:50px;'>";
                        //                        else
                        //                            outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                        //                        outXml += @"</div>  ਕਮਿਸ਼ਨਰ  <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                                        </td>
                        //                                        <td style ='padding:10px;'>
                        //<div style='padding-top:5px'>";
                        //                        if (srdeptmayorsign != "")
                        //                            outXml += @"<img src='" + srdeptmayorsign + @"'alt='Signature' style='height:50px;'>";
                        //                        else
                        //                            outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                        //                        outXml += @"</div>
                        //                                       ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                                        </td>
                        //                                        <td style ='padding:10px;width:15%'>
                        //<div style='padding-top:5px'>";
                        //                        if (depmayor != "")
                        //                            outXml += @"<img src='" + depmayor + @"'alt='Signature' style='height:50px;'>";
                        //                        else
                        //                            outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                        //                        outXml += @"</div>
                        //                                         ਡਿਪਟੀ ਮੇਅਰ <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                                       </td></tr></table>";

                        outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                                         ਮੇਅਰ,<br>" + mdl._LstReferences[0].FromOfficeLocal + "</p>";
                        // }

                    }
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }


        public string GetCircularPatiala(int agendaid, string doctype, int IsCirculate = 0)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();

            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);
            int ofid = 0;
            if (IsCirculate == 1)
            {
                if (officetype == "FNCC")
                {
                    ofid = 2;
                }
                else
                {
                    ofid = 1;
                }
            }
            Authoritylist cl = new Authoritylist();
            cl.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);
            string maintitle = string.Empty;

            var path1 = BlobStorage.GetStorageAcessingPath();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);

            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            string value = ConfigurationManager.AppSettings["StorageType"];

            string signature;
            string logo = "";

            var dptobj = AgendaModelFunction.GetDeptData(deptid);
            var deptname = CurrentSession.DeptName;

            if (dptobj != null)
            {
                if (dptobj.Deptname.Contains("Local"))
                {
                    logo = path1 + "/LG/" + CurrentSession.MCName + "/" + dptobj.logo;
                }
                else
                {
                    logo = path1 + "/MC/" + dptobj.Name + "/" + dptobj.logo;
                }
            }

            if (value == "Blob")
            {
                signature = path1 + "/" + CurrentSession.SignaturePath;
            }
            else
            {
                signature = "https://enigambackuprestore.blob.core.windows.net/securefilestructure" + "/" + CurrentSession.SignaturePath;

            }





            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: ApprovedAgendaReport</title>
                                    <style>
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    .pdf-table td {
                                        vertical-align: top;
                                       
                                    }
                                     ol li {
                                          padding: 5px;
                                              }
                                    ol li {
                                         margin: 5px 0px;
                                          }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            .pdf-table{
                                            width:100%;
                                            }
                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");
            //covering letter start
            if (ofid == 2) //FNCC
            {
                outXml += @"<div style='font-size: 14pt;font-weight: bold;width: 100%;'>
                <div style='float:left;width: 100px;'>
                <img src='" + logo + @"' height='50' width='55' /></div>
                <div><span style='border-bottom: 1px solid;'>ਦਫਤਰ  " + mdl._LstReferences[0].FromDeptLocal + @" </span><br>
                     <span style='border-top: 1px solid;'>OFFICE OF " + mdl._LstReferences[0].FromDept.ToUpper() + @"<span></div></div>
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style='font-size: 12pt;float: left;width: 50%;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style='font-size: 12pt;float: right;width: 30%;text-align: right;'> <b>ਮਿਤੀ:.................</b></p> 
                            </div>
                            <br> <br> <br> <br> 
                            <p style='display: inline; font-size: 12pt;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p>

                   <div style='margin-bottom:10px;'><ol style='margin: 20px 65px; font-size:12pt;'>";
                for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                {
                    outXml += @"<li>" + cl.AuthorityId_list[j].name + ", </li>";
                }
                outXml += mdl._LstReferences[0].FromDeptLocal + "।";
                outXml += @"</ol>";
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + @" ਨੂੰ ਰੱਖੀ ਗਈ ਐਫ ਐਂਡ ਸੀ.ਸੀ.ਦੀ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                             
                                </span></p>

                                <p style='text-align: justify; font-size: 12pt; line-height:1.7;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                                 ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਬੇਨਤੀ ਹੈ ਕਿ ਮਾਨਯੋਗ ਮੇਅਰ ਸਾਹਿਬ ਦੇ ਜੁਬਾਨੀ ਹੁਕਮਾਂ ਦੀ ਪਾਲਣਾ ਹਿੱਤ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + @" ਨੂੰ ਸਮਾਂ "
                                 + HeaderlistofAgenda.StartTime + @" ਵਜੇ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਦੇ ਦਫਤਰ ਵਿਖੇ ਐਫ ਐਂਡ ਸੀ.ਸੀ. ਦੀ ਮੀਟਿੰਗ ਰੱਖੀ ਗਈ ਹੈ ਜਿਸ ਦਾ ਏਜੰਡਾ ਨਾਲ ਨੱਥੀ ਕਰਕੇ ਆਪ ਜੀ ਨੂੰ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ। 
                                   ਕਿਰਪਾ ਕਰਕੇ ਮੀਟਿੰਗ ਵਿੱਚ ਸਮੇਂ ਸਿਰ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਖੇਚਲ ਕਰਨਾ ਜੀ।
                        <br>
                        <br>                          
                                </p>
                                <p style='margin: auto; font-size: 12pt;text-align:right;'> 
                       ਸਕੱਤਰ ,<br> 
                                  ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।
                                </p>";
                outXml += @"<p style='text-align: justify; font-size: 14pt; display: flex;' ><b style='margin-right: 20px;'> ਉਤਾਰਾ:-</b></p>";
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;margin-right: 30px'> ਮਾਨਯੋਗ ਮੇਅਰ, ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਜੀ ਨੂੰ ਸੂਚਨਾ ਹਿੱਤ।
                         <br>
                        <br> </p></p>
                                <p style='margin: auto; font-size: 12pt;text-align:right;page-break-after: always;'> 
                       ਸਕੱਤਰ ,<br> 
                                  ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।
                                </p>";
            }
            else if (ofid != 0)
            {
                outXml += @" < div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align: left;float:left;margin: 0px;'> <b>Website : " + mdl._LstReferences[0].DeptWebsite + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align: right;float:right;margin: 0px;'> <b>" + mdl._LstReferences[0].MayorContactNo + @" (Mayor)</b></p> 
                             </div>
                            <div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align:left;float:left;margin: 0px;'> <b>E-Mail : " + mdl._LstReferences[0].DeptEmail + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align:right;float:right;margin: 0px;'> <b>" + mdl._LstReferences[0].CommContactNo + @" (Commissioner)</b></p>
                            </div>
                            <div style='text-align: center;font-size: 30px;font-weight: bold;'>ਨਗਰ ਨਿਗਮ</div>    
                            <div style='text-align: center;'> 
                                <h3>" + mdl._LstReferences[0].DeptAddress_Local + @"</h3>
                            </div>
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style=' font-size: 16px;float: left;width: 50%;margin: 0px;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style=' font-size: 16px;float: right;width: 30%;text-align: right;margin: 0px;'> <b>ਮਿਤੀ  ..... " + gt + @" .</b></p> 
                            </div>
                            <br> 
            <div style='display:flex;'>
                            <p style='width: 100%;margin: 0px;float: left; font-size: 16px;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p></div>
            <div class='clearfix'></div>
                   <div class='text-align:center;margin-bottom:10px;'>";
                for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                {
                    outXml += @"<div style='margin: 15px 65px; font-size:12pt;'>" + (j + 1).ToString() + ") " + cl.AuthorityId_list[j].name + "," + cl.AuthorityId_list[j].Address + "</div>";
                }
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> " + mdl._LstReferences[0].FromDeptLocal + @" ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                                      
                                </span> 
                            </p>

                            <p style='text-align: justify; font-size: 16px;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ਉਕਤ ਵਿਸ਼ੇ ਸਬੰਦੀ ਬੇਨਤੀ ਹੈ ਕੀ , " + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ  ਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ : " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"
                              ਨੂ ਸਮਾਂ " + Convert.ToString(HeaderlistofAgenda.StartTime) + @" ਵਜੇ ਮਿਉਂਸਪਲ  ਭਵਨ ," + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ " + cl.AuthorityId_list[0].name + @" ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਵਿਖੇ ਹੋਵੇਗੀ |                           
                    <br>
                    <br>
                    ਨਥੀ : ਏਜੇਂਡੇ ਦੀ ਕਾਪੀ | <br>  <br>  

                            </p>        

                            <p style='margin: auto; font-size: 16px;text-align:right;page-break-after: always;'> 
                   ਸਕੱਤਰ ,<br>
                                          " + mdl._LstReferences[0].FromDeptLocal + @"             
                            </p></div>";
            }
            //covering letter end

            if (ofid == 0)//none circular with
            {
                if (HeaderlistofAgenda != null && cl.AuthorityId_list.Count > 0 && mdl._LstReferences.Count > 0)
                {
                    outXml += @"<div style='margin: 35px 10px;page-break-before: always;'>";
                    //<h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    //+ HeaderlistofAgenda.AgendaName + "</h1>";
                    //+ HeaderlistofAgenda.Header +
                    outXml += @"<p style='font-size: 12pt; text-align: justify'>";
                    if (officetype != "FNCC")
                        outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                    else
                        outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                }
                else
                {
                    outXml += @"<div style='margin: 35px 10px;page-break-before: always;'>";
                    //<h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    //+ HeaderlistofAgenda.AgendaName + "</h1>";
                    //+ HeaderlistofAgenda.Header +
                    outXml += @"<p style='font-size: 12pt; text-align: justify'>
                 
                 ਮਿਉਂਸਪਲ ਕਾਰਪੋਰੇਸ਼ਨ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: 24/03/2023 ਨੂੰ ਸਮਾਂ ਸਵੇਰੇ 11:30 ਵਜੇ ਸ੍ਰੀ ਅਮਰਜੀਤ ਸਿੰਘ ਸਿੱਧੂ, ਮੇਅਰ, ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਦਫ਼ਤਰ ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                }
            }
            outXml += @"<table class='pdf-table' style='font-size: 12pt; border: 1px solid black;border-collapse: collapse;'>";
            outXml += @"<tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse; width:10%;'>Sr. No</th>";
            //<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;  width:15%;'>Date of Meeting</th>
            outXml += @" <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'> " + doctype + " Details </th></tr>";

            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + mdl._LstReferences[i].SrNo + @"    
										</td>";
                    //<td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                    //" + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                    //</td>
                    outXml += @"  <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</table>";

                        outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";

                        // // if (HeaderlistofAgenda != null)
                        // {
                        //outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                        //             " + HeaderlistofAgenda.Footer + "</p>";
                        outXml += @"<p style ='text-align: right; font-size: 12pt;'> 
                                         ਸਕੱਤਰ,<br>" + mdl._LstReferences[0].FromOfficeLocal + "</p>";
                        // }

                    }
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }

     
        public string GetCircularPatiala(int agendaid)
        {

            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();

            //councillorlis start
            Councillorslist cl = new Councillorslist();
            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }

            cl.Councillor_list = AgendaModelFunction.getCouncillirslist(deptid, agendaid);
            //end

            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);

            Authoritylist cl1 = new Authoritylist();
            cl1.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);


            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForProcedingPDF(agendaid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            var path1 = BlobStorage.GetStorageAcessingPath();
            string signature = path1 + "/" + CurrentSession.SignaturePath;
            string outXml = "";
            // if(mdl.)


            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: ProceedingReport</title>                                    
                                        <style>
                                   .MsoNormalTable tr,.MsoNormalTable td,.MsoNormalTable th,.MsoTableGrid tr,.MsoTableGrid td,.MsoTableGrid th,#tblfooter tr{
                                   page-break-inside: avoid !important;                                                                          
                                    }                                  
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;                                       
                                        white-space: pre-wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{      
                                        max-width: 300px !important;
                                        padding: 1px !important;
                                        word-wrap:break-word;

                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class .MsoTableGrid td{                                       
                                        text-wrap:wrap;
                                        white-space: pre-wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;                                        
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            table{
                                            width:100%;                                           
                                            }
                                          ol li {
                                          padding: 10px;
                                              }


                                        </style>
                                    </head> 
         <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            if (HeaderlistofAgenda != null && mdl._LstReferences.Count > 0 && cl1.AuthorityId_list.Count > 0)
            {
                outXml += @"<div>
                        <h1 style='font-size: 26px; font-weight: 700; text-align: center; border-bottom: 3px solid #000;'>"
                    + mdl._LstReferences[0].FromDeptLocal + "</h1>";
                outXml += @"<p style='font-size: 16px; text-align: center'>";
                if (officetype != "FNCC")
                    outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ। ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—   </p></div>";
                else
                    outXml += "ਨਗਰ ਨਿਗਮ ਪਟਿਆਲਾ ਦੀ ਫਾਇਨਾਂਸ ਅਤੇ ਕੰਟਰੈਕਟ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਜੋ ਕਿ ਮਿਤੀ 29.05.2025 ਨੂੰ ਬਾਅਦ ਦੁਪਹਿਰ 4.00 ਵਜੇ ਮਾਨਯੋਗ ਮੇਅਰ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਜੀ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਦੇ ਦਫਤਰ ਵਿਖੇ ਹੋਈ, ਜਿਸ ਵਿੱਚ ਉਨ੍ਹਾਂ ਤੋਂ ਇਲਾਵਾਂ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਹਿਬਾਨ ਨੇ ਹਿੱਸਾ ਲਿਆ:-";
                // outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ। ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—  </p></div>";

            }

            //      if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
            //   {
            //if (cl.Councillor_list[0].meetingType == "FNCC")
            if (officetype == "FNCC")
            {
                outXml += @"<ol>";
                //for (int j = 0; j < cl.Councillor_list.Count; j++)
                //{
                //    outXml += @"<li>" + cl.Councillor_list[j].councillorname + "</li>";
                //}

                outXml += @"<li> ਸ਼੍ਰੀ ਪਰਮਵੀਰ ਸਿੰਘ, ਆਈ ਏ ਐਸ., <br>
                                     ਕਮਿਸ਼ਨਰ, ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ। </li>";
                outXml += @"<li> ਸ਼੍ਰੀ ਹਰਿੰਦਰ ਕੋਹਲੀ, ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ, <br>
                                     ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                outXml += @"<li> ਸ਼੍ਰੀ ਜਗਦੀਪ ਸਿੰਘ ਰਾਏ, ਡਿਪਟੀ ਮੇਅਰ, <br>
                                     ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                outXml += @"</ol>";

                outXml += @"<p> ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖਿਆਂ ਮੱਦਾਂ ਦੇ ਵਿਚਾਰ ਕਰਨ ਉਪਰੰਤ ਫੈਂਸਲੇ ਕੀਤੇ ਗਏ:— </p><br>";
            }
            else
            {
                outXml += @" <table id='maintable' style='width:100%;font-size: 12pt; border: 1px solid black; border-collapse: collapse;'>";
                outXml += @"<tr>                    
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                    </tr>";
                for (int j = 0; j < cl.Councillor_list.Count; j++)
                {
                    //first list of mayor that donot under mayor so skip first record
                    if (j == 0)
                    {
                        j = j + 1;
                    }
                    outXml += @"<tr>
                      <td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                    outXml += @"" + cl.Councillor_list[j].wardid;
                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'> "
                    + cl.Councillor_list[j].councillorname + "</td>";
                    j = j + 1;
                    if (cl.Councillor_list.Count > j)
                    {
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";


                        outXml += @"" + cl.Councillor_list[j].wardid;

                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; ' > " + cl.Councillor_list[j].councillorname + "</td>";
                    }
                    else
                    {
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += "</tr>";
                        break;
                    }
                    j = j + 1;
                    if (cl.Councillor_list.Count > j)
                    {
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                        outXml += @"" + cl.Councillor_list[j].wardid;
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'>"
                       + cl.Councillor_list[j].councillorname + "</td>";
                    }
                    else
                    {
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += "</tr>";
                        break;
                    }
                    outXml += "</tr>";
                }
                outXml += "</table><br>";
            }
            // }
            if (officetype == "FNCC")
            {
                outXml = outXml + @"<div>";
            }
            else
            {
                outXml = outXml + @"<div style='page-break-before: always;'>";
            }
            //outXml = outXml + @"<div>";
            if (mdl._LstExtraReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstExtraReferences.Count; i++)
                {
                    outXml += @"<p>" + Convert.ToString(mdl._LstExtraReferences[i].SubjectDetails) + "</p><br>";
                }
            }
            outXml += @"</div> <table style='width:100%;font-size: 12pt; border: 1px solid black;border-collapse: collapse;'>";
            outXml = outXml + @"<tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Sr. No</th>
                  <th style ='padding:10px;border: 1px solid black;border - collapse: collapse;'>Resolution No.</th> 
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Date of Meeting</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Resolution Description </th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;width: 15%;'>Orders of Competent Authority</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;display:none'>Act</th></tr>";

            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {

                    if (mdl._LstReferences[i].SerialNo == null || mdl._LstReferences[i].SerialNo == "")
                    {
                        if (j == 0)
                        {
                            outXml += @"<tr>
        		<td colspan='5' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
        				Table Items</td></tr>";
                        }
                        j = j + 1;
                    }
                    outXml += @"<tr>
        		<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
        				" + Convert.ToString(i + 1) + @"    
        		</td>
        		<td style='padding: 10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(mdl._LstReferences[i].SerialNo) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    // Replace searchText with a new line character
                    //LOBText = LOBText.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "</br>");

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>
                                       " + Convert.ToString(mdl._LstReferences[i].ActionDescription) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;display:none'>
                                       " + Convert.ToString(mdl._LstReferences[i].Act) + @"
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</table>";
                        outXml += @"<br/><br/>";
                        //outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                        //if (HeaderlistofAgenda != null)
                        //{
                        outXml += @" <table id='tblfooter' style='width:100%;font-size: 14px;margin-top: 40px;' > ";
                        outXml += @"<tr>
        		<td style='padding:10px;'>               
        				ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                                        </td>
        		<td style='padding: 10px;'>
                                        ਕਮਿਸ਼ਨਰ  <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                                        </td>
                                        <td style ='padding:10px;'>
                                       ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                                        </td>
                                        <td style ='padding:10px;'>
                                         ਡਿਪਟੀ ਮੇਅਰ <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                                       </td></tr></table>";

                        //outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                        //                 ਮੇਅਰ,<br>" + mdl._LstReferences[0].FromOfficeLocal + "</p>";
                        //}

                    }
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }

        public string  GetNewpdfhtml(int agendaid)
        {

            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();

            //councillorlis start
            Councillorslist cl = new Councillorslist();
            string deptid = string.Empty;
            bool isHoseExist = AgendaModelFunction.CheckHouseExists(CurrentSession.StateId);

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }

            cl.Councillor_list = AgendaModelFunction.getCouncillirslist(deptid, agendaid);
            var note = AttendenceList.GetNotes(agendaid);

            bool isBacklog = AttendenceList.IsBackLogAgenda(agendaid);

            //if (isBacklog)
            //{
            //    // Backlog agenda logic
            //}

            if ((cl.Councillor_list == null || !cl.Councillor_list.Any()) && isHoseExist == true && note?.Any(n => n.CategoryName == "FNCC Members") == false && isBacklog == false)
            {
                return "<h3 style='color:red;'>⚠ Attendance has not been marked yet. Cannot generate Proceedings PDF.</h3>";
            }

            if (isBacklog == false)
            {
                if (note?.Any(n => n.CategoryName == "Administration Members") == false)
                {
                    bool isHouseExists = AgendaModelFunction.CheckHouseExists(CurrentSession.StateId);

                    if (!isHouseExists)
                    {
                        return @"
                     <div style='color:#c0392b; font-weight:600;'>
                     ⚠ As the House stands dissolved, Administration-cum-Committee members have not been added.
                     <br/>
                     Kindly add the required members from the <b>“Add Note”</b> section.
                    </div>";
                    }
                }
            }


            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);

            Authoritylist cl1 = new Authoritylist();
            cl1.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);


            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForProcedingPDF(agendaid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            var Chairperson = HeaderlistofAgenda.Chairperson.ToString();
            var ChairpersonDesignation = HeaderlistofAgenda.ChairpersonDesignation.ToString();
            var Header = HeaderlistofAgenda.Header.ToString();
            var MeetingTypeName = HeaderlistofAgenda.MeetingTypeName.ToString();

            var path1 = BlobStorage.GetStorageAcessingPath();
            string signature = path1 + "/" + CurrentSession.SignaturePath;
            string outXml = "";
            
            outXml += @"<style>
                        /* Main table styling */
                        table.maintable {
                            width: 100% !important;
                            border-collapse: collapse !important;
                            table-layout: fixed !important; /* enforce column widths */
                            word-break: break-word;
                            overflow-wrap: break-word;
                        }

                        /* Table cells */
                        table.maintable th,
                        table.maintable td {
                            padding: 10px !important;
                            border: 1px solid black !important;
                            white-space: normal !important; /* allow text to wrap */
                            word-wrap: break-word !important;
                            overflow-wrap: break-word !important;
                        }

                        /* Column widths */
                        table.maintable th:nth-child(1),
                        table.maintable td:nth-child(1) { width: 4%; }
                        table.maintable th:nth-child(2),
                        table.maintable td:nth-child(2) { width: 10%; }
                        table.maintable th:nth-child(3),
                        table.maintable td:nth-child(3) { width: 10%; }
                        table.maintable th:nth-child(4),
                        table.maintable td:nth-child(4) { width: 60%; }
                        table.maintable th:nth-child(5),
                        table.maintable td:nth-child(5) { width: 17%; }

                        /* Footer row stays at bottom of each page */
                        tfoot {
                            display: table-footer-group !important;
                        }

                        /* Allow rows to split across pages */
                        table.maintable tr {
                            page-break-inside: auto; /* allow row content to split */
                        }

                        /* Cells allow content to split across pages */
                        table.maintable td {
                            page-break-inside: auto;
                        }

                        /* Remove restrictive max-widths from legacy classes */
                        .pdf-class .MsoNormalTable,
                        .pdf-class .MsoTableGrid {
                            max-width: none !important;
                            margin-left: 0 !important;
                        }

                        .pdf-class .MsoNormalTable td,
                        .pdf-class .MsoTableGrid td {
                            max-width: none !important;
                            white-space: normal !important;
                            word-wrap: break-word !important;
                            overflow-wrap: break-word !important;
                            padding: 1px !important;
                        }

                        /* Additional body rules */
                        body {
                            page-break-inside: auto;
                            overflow: visible;
                            word-wrap: break-word;
                        }

                        /* Vertical align */
                        td {
                            vertical-align: top;
                        }

                        /* Hide headers when printing */
                        @media print {
                            .header {
                                display: none;
                            }
                        }

                        /* Misc table adjustments */
                        table {
                            width: 100%;
                        }

                        ol li {
                            padding: 5px;
                        }
                        @page {
                            margin: 20mm 10mm;
                            @top-center {
                                content: "";
                                border-bottom: 1px solid black;
                                display: block;
                                margin-bottom: 5mm;
                            }
                        }
                        </style>";

            string endingText = "";

            if (!isBacklog)
            {
                endingText = " ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—";
            }

            if (HeaderlistofAgenda != null && mdl._LstReferences.Count > 0 && cl1.AuthorityId_list.Count > 0)
            {
                outXml += @"<div>
                        <h1 style='font-size: 26px; font-weight: 700; text-align: center; border-bottom: 3px solid #000;'>"
                    + mdl._LstReferences[0].FromDeptLocal + "</h1>";


                outXml += @"<p style='font-size: 16px; text-align: center'>";
                if (officetype != "FNCC")
                {
                    //outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ। ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—   </p></div>";
                    if (!string.IsNullOrWhiteSpace(Chairperson))
                    {
                        outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ " + MeetingTypeName + ": " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + Chairperson + "," + ChairpersonDesignation + " " + mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ।"  + endingText + "</p></div>";
                    }
                    else
                    {
                        outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " " + mdl._LstReferences[0].FromDeptLocal + "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ।"  + endingText + "</p></div>";
                    }
                }
                else
                {
                    //outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ। ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—  </p></div>";
                    if (Chairperson != null)
                    {
                        outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + Chairperson + "," + ChairpersonDesignation + "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ।"  + endingText + "</p></div>";
                    }
                    else
                    {
                        outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ।"  + endingText + "</p></div>";
                    }
                }

            }

           

            if (note != null && note.Any())
            {
                var ExOfficio = note?.Where(n => n.CategoryName == "ExOfficio").ToList();
                //var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

                foreach (var note1 in ExOfficio)
                {
                    outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                }
            }
            if (note != null && note.Any())
            {
                var AdministrationMembers = note?.Where(n => n.CategoryName == "Administration Members").ToList();
                //var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

                foreach (var note1 in AdministrationMembers)
                {
                    outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                }
            }


            if (isBacklog != true)
            {
                if (cl.Councillor_list == null || cl.Councillor_list.Count == 0)
                {
                    if (officetype == "FNCC")
                    {
                        outXml += @"<ol>";

                        var FNCCMembers = note?.Where(n => n.CategoryName == "FNCC Members").ToList();
                        foreach (var note1 in FNCCMembers)
                        {
                            outXml += @"<P>" + Convert.ToString(note1.NoteText) + "</p>";
                        }
                    }
                }
            }
            if (isBacklog != true)
            {
                if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
                {
                    if (cl.Councillor_list[0].meetingType != "")
                    {
                        if (officetype == "FNCC")
                        {
                            outXml += @"<ol>";
                            //for (int j = 0; j < cl.Councillor_list.Count; j++)
                            //{
                            //    outXml += @"<li>" + cl.Councillor_list[j].councillorname + "</li>";
                            //}

                            var FNCCMembers = note?.Where(n => n.CategoryName == "FNCC Members").ToList();
                            //var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

                            foreach (var note1 in FNCCMembers)
                            {
                                outXml += @"<li>" + Convert.ToString(note1.NoteText) + "</li>";
                            }

                            //outXml += @"<li> ਸ਼੍ਰੀ ਪਰਮਵੀਰ ਸਿੰਘ, ਆਈ ਏ ਐਸ., <br>
                            //                 ਕਮਿਸ਼ਨਰ, ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ। </li>";
                            //outXml += @"<li> ਸ਼੍ਰੀ ਹਰਿੰਦਰ ਕੋਹਲੀ, ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ, <br>
                            //                 ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                            //outXml += @"<li> ਸ਼੍ਰੀ ਜਗਦੀਪ ਸਿੰਘ ਰਾਏ, ਡਿਪਟੀ ਮੇਅਰ, <br>
                            //                 ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";                  
                            //outXml += @"</ol>";

                            //outXml += @"<p> ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖਿਆਂ ਮੱਦਾਂ ਦੇ ਵਿਚਾਰ ਕਰਨ ਉਪਰੰਤ ਫੈਂਸਲੇ ਕੀਤੇ ਗਏ:— </p><br>";
                        }
                        else
                        {

                            outXml += @"<table style='width:100%;font-size: 11pt; border: 1px solid black; border-collapse: collapse;'>";
                            outXml += @"<tr>                    
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                </tr>";

                            int totalCols = 3; // number of Ward+Name pairs
                            int rows = (int)Math.Ceiling((cl.Councillor_list.Count - 1) / (double)totalCols);
                            // -1 because skipping first record

                            for (int i = 1; i <= rows; i++) // start from 1, skipping first councillor
                            {
                                outXml += "<tr>";

                                for (int col = 0; col < totalCols; col++)
                                {
                                    int index = i + col * rows; // spread into columns
                                    if (index < cl.Councillor_list.Count)
                                    {
                                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>" +
                                                  cl.Councillor_list[index].wardid + "</td>";
                                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'>" +
                                                  cl.Councillor_list[index].councillorname + "</td>";
                                    }
                                    else
                                    {
                                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'></td>
                        <td style='border: 1px solid black; border-collapse: collapse;'></td>";
                                    }
                                }

                                outXml += "</tr>";
                            }

                            outXml += "</table><br>";

                        }

                    }
                    else
                    {
                        //outXml += @" <table style='width:100%;font-size: 12pt; border: 1px solid black; border-collapse: collapse;'>";
                        //outXml += @"<tr>                    
                        //      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                        //      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                        //      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                        //      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                        //      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                        //      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                        //    </tr>";
                        //for (int j = 0; j < cl.Councillor_list.Count; j++)
                        //{
                        //    //first list of mayor that donot under mayor so skip first record
                        //    if (j == 0)
                        //    {
                        //        j = j + 1;
                        //    }
                        //    outXml += @"<tr>
                        //      <td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                        //    outXml += @"" + cl.Councillor_list[j].wardid;
                        //    outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'> "
                        //    + cl.Councillor_list[j].councillorname + "</td>";
                        //    j = j + 1;
                        //    if (cl.Councillor_list.Count > j)
                        //    {
                        //        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";


                        //        outXml += @"" + cl.Councillor_list[j].wardid;

                        //        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; ' > " + cl.Councillor_list[j].councillorname + "</td>";
                        //    }
                        //    else
                        //    {
                        //        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                        //                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        //        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                        //                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        //        outXml += "</tr>";
                        //        break;
                        //    }
                        //    j = j + 1;
                        //    if (cl.Councillor_list.Count > j)
                        //    {
                        //        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                        //        outXml += @"" + cl.Councillor_list[j].wardid;
                        //        outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'>"
                        //       + cl.Councillor_list[j].councillorname + "</td>";
                        //    }
                        //    else
                        //    {
                        //        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                        //                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        //        outXml += "</tr>";
                        //        break;
                        //    }
                        //    outXml += "</tr>";
                        //}
                        //outXml += "</table><br>";

                        outXml += @"<table style='width:100%;font-size: 11pt; border: 1px solid black; border-collapse: collapse;'>";
                        outXml += @"<tr>                    
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                </tr>";

                        int totalCols = 3; // number of Ward+Name pairs
                        int rows = (int)Math.Ceiling((cl.Councillor_list.Count - 1) / (double)totalCols);
                        // -1 because skipping first record

                        for (int i = 1; i <= rows; i++) // start from 1, skipping first councillor
                        {
                            outXml += "<tr>";

                            for (int col = 0; col < totalCols; col++)
                            {
                                int index = i + col * rows; // spread into columns
                                if (index < cl.Councillor_list.Count)
                                {
                                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>" +
                                              cl.Councillor_list[index].wardid + "</td>";
                                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'>" +
                                              cl.Councillor_list[index].councillorname + "</td>";
                                }
                                else
                                {
                                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'></td>
                        <td style='border: 1px solid black; border-collapse: collapse;'></td>";
                                }
                            }

                            outXml += "</tr>";
                        }

                        outXml += "</table><br>";
                    }


                    if (officetype == "FNCC")
                    {
                        outXml = outXml + @"<div>";
                    }
                    else
                    {
                        outXml = outXml + @"<div style='page-break-before: always;'>";
                    }
                }
            }
           
            if (note != null && note.Any())
            {
                var obituaryNotes = note?.Where(n => n.CategoryName == "Obituary").ToList();
                //var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

                foreach (var note1 in obituaryNotes)
                {
                    outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                }
            }


            //outXml = outXml + @"<div>";
            if (mdl._LstExtraReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstExtraReferences.Count; i++)
                {
                    outXml += @"<p>" + Convert.ToString(mdl._LstExtraReferences[i].SubjectDetails) + "</p><br>";
                }
            }
            outXml += @"<div style='width:100%;'>
            <table class='maintable' style='border-collapse:collapse;font-size:12pt;'>";
            outXml += @"<tfoot>
            <tr>
                <td colspan='5' style='border-top: 1px solid black !important; border-left: none !important; border-right: none !important; border-bottom: none !important; padding:0;'></td>
            </tr>
            </tfoot>";

            outXml += @"<tbody>";

            outXml += @"<thead><tr>
                        <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'> </td>
                        <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>
                        <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>
                        <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>
                        <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>
                        <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>
          </tr></thead>";
            outXml = outXml + @"<tr style = 'padding:5px;'>
            <th style='padding:10px; border:1px solid black;'>Sr. No</th>
            <th style='padding:10px; border:1px solid black;'>Resolution No.</th>
            <th style='padding:10px; border:1px solid black;'>Date of Meeting</th>
            <th style='padding:10px; border:1px solid black;word-wrap:break-word;'>Resolution Description</th>
            <th style='padding:10px; border:1px solid black;word-wrap:break-word;'>Orders of Competent Authority</th>
                  </tr>";

          

            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {

                    if (mdl._LstReferences[i].TableItem == "0" || mdl._LstReferences[i].TableItem == "")
                    {
                        if (j == 0)
                        {
                            outXml += @"<tr>
										<td colspan='5' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
												Table Items</td></tr>";
                        }
                        j = j + 1;
                    }
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + Convert.ToString(i + 1) + @"    
										</td>
										<td style='padding: 10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(mdl._LstReferences[i].SerialNo) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    if (!string.IsNullOrEmpty(LOBText) && LOBText.Contains("<table"))
                    {
                        // Force all <table> to expand
                        LOBText = Regex.Replace(LOBText, "<table[^>]*>",
                            "<table style='width:100%; border-collapse:collapse; border:1px solid black;'>",
                            RegexOptions.IgnoreCase);

                        // Remove inline width attributes from <td>, <th>, <tr>
                        LOBText = Regex.Replace(LOBText, @"<(td|th|tr)([^>]*)\swidth\s*=\s*['""][^'""]*['""]", "<$1$2", RegexOptions.IgnoreCase);

                        // Ensure all <td> allow wrapping
                        LOBText = Regex.Replace(LOBText, "<td([^>]*)>", "<td$1 style='word-wrap:break-word; padding:5px;'>", RegexOptions.IgnoreCase);
                    }

                    // Replace searchText with a new line character
                    //LOBText = LOBText.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "</br>");

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }
                 
                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    
                    outXml += @"<td style='padding:10px; border:1px solid black; text-align:justify;font-size:12px;text-align:justify;'>
                    " + mdl._LstReferences[i].ActionDescription + @"
                    </td>";


                    if (i == mdl._LstReferences.Count - 1)
                    {

                         outXml += "</tbody>";
                         outXml += "</table>";
                         outXml += @"<br/><br/>";
                       
                        if (note != null && note.Any())
                        {
                            //var obituaryNotes = note?.Where(n => n.CategoryName == "Obituary").ToList();
                            var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();
                            foreach (var note1 in EndorsementNotes)
                            {
                                outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                            }
                        }
                        //if (signature != null || signature != "")
                        //{
                        //outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                        //}
                        //if (HeaderlistofAgenda != null)
                        //{
                        //              outXml += @" <table style = 'width:100%;font-size: 14px;margin-top: 40px;' > ";
                        //              outXml += @"<tr>
                        //<td style='padding:10px;'>               
                        //		ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                              </td>
                        //<td style='padding: 10px;'>
                        //                              ਕਮਿਸ਼ਨਰ  <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                              </td>
                        //                              <td style ='padding:10px;'>
                        //                             ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                              </td>
                        //                              <td style ='padding:10px;'>
                        //                               ਡਿਪਟੀ ਮੇਅਰ <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                             </td></tr></table>";
                        if (!string.IsNullOrWhiteSpace(Chairperson))
                        {
                            outXml += @"<p style ='text-align: right; font-size: 18px;'>" + ChairpersonDesignation + @"<br>" + mdl._LstReferences[0].FromDeptLocal + "</p>";
                        }
                        else
                        {
                            outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                                         ਮੇਅਰ,<br>" + mdl._LstReferences[0].FromDeptLocal
                           + "</p>";
                        }
                     
                    }

                }

                outXml += "</div></body> </html>";
            }
            return outXml;
        }

        public string GetNewpdfhtmlCounsil(int agendaid)
        {

            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();

            //councillorlis start
            Councillorslist cl = new Councillorslist();
            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            
            bool isHoseExist = AgendaModelFunction.CheckHouseExists(CurrentSession.StateId);
            cl.Councillor_list = AgendaModelFunction.getCouncillirslist(deptid, agendaid);
            var note = AttendenceList.GetNotes(agendaid);
            bool isBacklog = AttendenceList.IsBackLogAgenda(agendaid);

            if ((cl.Councillor_list == null || !cl.Councillor_list.Any()) && isHoseExist == true && note?.Any(n => n.CategoryName == "FNCC Members") == false && isBacklog == false)
            {
               return "<h3 style='color:red;'>⚠ Attendance has not been marked yet. Cannot generate Proceedings PDF.</h3>";
            }
          
           
            if (isBacklog == false)
            {
                if (note?.Any(n => n.CategoryName == "Administration Members") == false)
                {
                    bool isHouseExists = AgendaModelFunction.CheckHouseExists(CurrentSession.StateId);

                    if (!isHouseExists)
                    {
                        return @"
                    <div style='color:#c0392b; font-weight:600;'>
                    ⚠ As the House stands dissolved, Administration-cum-Committee Members have not been added.
                    <br/>
                    Kindly add the required Members from the <b>“Add Note”</b> section.
                   </div>";
                    }
                }
            }


            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);

            Authoritylist cl1 = new Authoritylist();
            cl1.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);


            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForProcedingPDF(agendaid);

            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
           
            var Chairperson = HeaderlistofAgenda.Chairperson.ToString();
            var ChairpersonDesignation = HeaderlistofAgenda.ChairpersonDesignation.ToString();
            var Header = HeaderlistofAgenda.Header.ToString();
            var MeetingTypeName = HeaderlistofAgenda.MeetingTypeName.ToString();
            
            var path1 = BlobStorage.GetStorageAcessingPath();
            string signature = path1 + " / " + CurrentSession.SignaturePath;
            string outXml = "";

            outXml += @"<style>
                        /* Main table styling */
                        table.maintable {
                            width: 100% !important;
                            border-collapse: collapse !important;
                            table-layout: fixed !important; /* enforce column widths */
                            word-break: break-word;
                            overflow-wrap: break-word;
                        }

                        /* Table cells */
                        table.maintable th,
                        table.maintable td {
                            padding: 10px !important;
                            border: 1px solid black !important;
                            white-space: normal !important; /* allow text to wrap */
                            word-wrap: break-word !important;
                            overflow-wrap: break-word !important;
                        }

                       
                      

                        /* Footer row stays at bottom of each page */
                        tfoot {
                            display: table-footer-group !important;
                        }

                        /* Allow rows to split across pages */
                        table.maintable tr {
                            page-break-inside: auto; /* allow row content to split */
                        }

                        /* Cells allow content to split across pages */
                        table.maintable td {
                            page-break-inside: auto;
                        }

                        /* Remove restrictive max-widths from legacy classes */
                        .pdf-class .MsoNormalTable,
                        .pdf-class .MsoTableGrid {
                            max-width: none !important;
                            margin-left: 0 !important;
                        }

                        .pdf-class .MsoNormalTable td,
                        .pdf-class .MsoTableGrid td {
                            max-width: none !important;
                            white-space: normal !important;
                            word-wrap: break-word !important;
                            overflow-wrap: break-word !important;
                            padding: 1px !important;
                        }

                        /* Additional body rules */
                        body {
                            page-break-inside: auto;
                            overflow: visible;
                            word-wrap: break-word;
                        }

                        /* Vertical align */
                        td {
                            vertical-align: top;
                        }

                        /* Hide headers when printing */
                        @media print {
                            .header {
                                display: none;
                            }
                        }

                        /* Misc table adjustments */
                        table {
                            width: 100%;
                        }

                        ol li {
                            padding: 5px;
                        }
                        @page {
                            margin: 20mm 10mm;
                            @top-center {
                                content: "";
                                border-bottom: 1px solid black;
                                display: block;
                                margin-bottom: 5mm;
                            }
                        }
                        </style>";
           
            
            string endingText = "";

            if (!isBacklog)
            {
                endingText = " ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—";
            }

            if (HeaderlistofAgenda != null && mdl._LstReferences.Count > 0 && cl1.AuthorityId_list.Count > 0)
            {
                outXml += @"<div>
                        <h1 style='font-size: 26px; font-weight: 700; text-align: center; border-bottom: 3px solid #000;'>"
                    + mdl._LstReferences[0].FromDeptLocal + "</h1>";
                outXml += @"<p style='font-size: 16px; text-align: center'>";
                if (officetype != "FNCC")
                {
                    if (!string.IsNullOrWhiteSpace(Chairperson) && isHoseExist == true)
                    {
                        outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ " + MeetingTypeName + ": " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + Chairperson + "," + ChairpersonDesignation + " " + mdl._LstReferences[0].FromDeptLocal + "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ।"+ endingText + "</p></div>";
                    }
                    else if (!string.IsNullOrWhiteSpace(Chairperson) && isHoseExist == false)
                    {
                        outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ " + MeetingTypeName + ": " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + Chairperson + "," + ChairpersonDesignation + " " + mdl._LstReferences[0].FromDeptLocal + "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ। </p></div>";

                    }
                    else
                    {
                        string s = "";

                        if (note != null && note.Any())
                        {
                            s = note
                                .Where(n => n.CategoryName == "Administration Members")
                                .Select(n => Convert.ToString(n.NoteText).Trim())
                                .FirstOrDefault() ?? "";
                            s = Regex.Replace(s, "<.*?>", "").Trim();
                        }

                        string dept = mdl._LstReferences[0].FromDeptLocal.ToString().Trim();
                        string date = HeaderlistofAgenda.AgendaDate.ToString().Trim();
                        string time = HeaderlistofAgenda.StartTime.ToString().Trim();
                        string meetingType = MeetingTypeName.Trim();
                        string chairPerson = cl1?.AuthorityId_list?[0]?.name?.ToString().Trim();

                        outXml += "<div><p>" + dept + " ਦੀ ";

                        if (!isHoseExist)
                        {
                            outXml += meetingType + ": " + date + " ਨੂੰ ਸਮਾਂ " + time +
                                      " ਵਜੇ " + (string.IsNullOrEmpty(s.Trim()) ? "" : s + ", ") +
                                      "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ।";
                        }
                        else
                        {
                            outXml += "ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: " + date + " ਨੂੰ ਸਮਾਂ " + time +
                                      " ਵਜੇ " + chairPerson + " " + dept +
                                      " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ।"+ endingText + "";
                        }

                        
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(Chairperson))
                    {

                        outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + Chairperson + "," + ChairpersonDesignation + "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ।"+ endingText + "</p></div>";
                    }
                    else
                    {
                        outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ।"+ endingText + "</p></div>";
                    }
                }
            }


            if (note != null && note.Any())
            {
                var ExOfficio = note?.Where(n => n.CategoryName == "ExOfficio").ToList();
                //var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

                foreach (var note1 in ExOfficio)
                {
                    outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                }
            }
            if (note != null && note.Any())
            {
                var AdministrationMembers = note?.Where(n => n.CategoryName == "Administration Members").ToList();
                //var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

                foreach (var note1 in AdministrationMembers)
                {
                    outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                }
            }

            if (isBacklog != true )
            {
                if (cl.Councillor_list.Count > 0)
                {

                    if (cl.Councillor_list[0].meetingType != "")
                    {
                        if (officetype == "FNCC")
                        {
                            outXml += @"<ol>";
                            //for (int j = 0; j < cl.Councillor_list.Count; j++)
                            //{
                            //    outXml += @"<li>" + cl.Councillor_list[j].councillorname + "</li>";
                            //}
                            var FNCCMembers = note?.Where(n => n.CategoryName == "FNCC Members").ToList();
                            //var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

                            foreach (var note1 in FNCCMembers)
                            {
                                outXml += @"<li>" + Convert.ToString(note1.NoteText) + "</li>";
                            }



                            //outXml += @"<li> ਸ਼੍ਰੀ ਪਰਮਵੀਰ ਸਿੰਘ, ਆਈ ਏ ਐਸ., <br>
                            //                 ਕਮਿਸ਼ਨਰ, ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ। </li>";
                            //outXml += @"<li> ਸ਼੍ਰੀ ਹਰਿੰਦਰ ਕੋਹਲੀ, ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ, <br>
                            //                 ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                            //outXml += @"<li> ਸ਼੍ਰੀ ਜਗਦੀਪ ਸਿੰਘ ਰਾਏ, ਡਿਪਟੀ ਮੇਅਰ, <br>
                            //                 ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";                  
                            //outXml += @"</ol>";

                            //outXml += @"<p> ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖਿਆਂ ਮੱਦਾਂ ਦੇ ਵਿਚਾਰ ਕਰਨ ਉਪਰੰਤ ਫੈਂਸਲੇ ਕੀਤੇ ਗਏ:— </p><br>";
                        }
                        else
                        {

                            if (isHoseExist)
                            {
                                outXml += @"<table style='width:100%;font-size: 11pt; border: 1px solid black; border-collapse: collapse;'>";
                                outXml += @"<tr>                    
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                </tr>";

                                int totalCols = 3; // number of Ward+Name pairs
                                int rows = (int)Math.Ceiling((cl.Councillor_list.Count - 1) / (double)totalCols);
                                // -1 because skipping first record

                                for (int i = 1; i <= rows; i++) // start from 1, skipping first councillor
                                {
                                    outXml += "<tr>";

                                    for (int col = 0; col < totalCols; col++)
                                    {
                                        int index = i + col * rows; // spread into columns
                                        if (index < cl.Councillor_list.Count)
                                        {
                                            outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>" +
                                                      cl.Councillor_list[index].wardid + "</td>";
                                            outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'>" +
                                                      cl.Councillor_list[index].councillorname + "</td>";
                                        }
                                        else
                                        {
                                            outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'></td>
                                        <td style='border: 1px solid black; border-collapse: collapse;'></td>";
                                        }
                                    }

                                    outXml += "</tr>";
                                }

                                outXml += "</table><br>";
                            }
                            else
                            {
                                //outXml += "<div style='text-align:center; margin-top:100px;'><p>ਹਾਊਸ ਦੀ ਮਿਆਦ ਖਤਮ ਹੋਣ ਦੇ ਮੱਦੇਨਜ਼ਰ, ਇਸ ਭਾਗ ਵਿੱਚ ਮੈਂਬਰਾਂ ਦੀ ਹਾਜ਼ਰੀ ਦਰਜ ਨਹੀਂ ਕੀਤੀ ਜਾਂਦੀ ।</p></div>";
                            }

                        }

                    }
                    else
                    {
                        outXml += @"<table style='width:100%;font-size: 11pt; border: 1px solid black; border-collapse: collapse;'>";
                        outXml += @"<tr>                    
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 120px;'>ਵਾਰਡ ਨੰ:</th>
                                  <th style='border: 1px solid black; border-collapse: collapse; width: 350px;'>ਕੌਂਸਲਰ ਦਾ ਨਾਮ</th> 
                                </tr>";

                        int totalCols = 3; // number of Ward+Name pairs
                        int rows = (int)Math.Ceiling((cl.Councillor_list.Count - 1) / (double)totalCols);
                        // -1 because skipping first record

                        for (int i = 1; i <= rows; i++) // start from 1, skipping first councillor
                        {
                            outXml += "<tr>";

                            for (int col = 0; col < totalCols; col++)
                            {
                                int index = i + col * rows; // spread into columns
                                if (index < cl.Councillor_list.Count)
                                {
                                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>" +
                                              cl.Councillor_list[index].wardid + "</td>";
                                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'>" +
                                              cl.Councillor_list[index].councillorname + "</td>";
                                }
                                else
                                {
                                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'></td>
                               <td style='border: 1px solid black; border-collapse: collapse;'></td>";
                                }
                            }

                            outXml += "</tr>";
                        }

                        outXml += "</table><br>";
                    }
                }

                if (officetype == "FNCC")
                {
                    outXml = outXml + @"<div>";
                }
                else
                {
                    outXml = outXml + @"<div style='page-break-before: always;'>";
                }
            }

            if (note != null && note.Any())
            {
                var obituaryNotes = note?.Where(n => n.CategoryName == "Obituary").ToList();
                //var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

                foreach (var note1 in obituaryNotes)
                {
                    outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                }
            }


            //outXml = outXml + @"<div>";
            if (mdl._LstExtraReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstExtraReferences.Count; i++)
                {
                    outXml += @"<p>" + Convert.ToString(mdl._LstExtraReferences[i].SubjectDetails) + "</p><br>";
                    
                }
            }
            outXml += @"<div style='width:100%;'>
            <table class='maintable' id='agendaTable' style='border-collapse:collapse;font-size:12pt;'>
            <colgroup>
                    <col style = 'width:4%'>
                    <col style = 'width:20%'>
                    <col style = 'width:77%'>
                   
                   </colgroup> ";
            outXml += @"<tfoot>
            <tr>
                <td colspan='4' style='border-top: 1px solid black !important; border-left: none !important; border-right: none !important; border-bottom: none !important; padding:0;'></td>
            </tr>
            </tfoot>";

            outXml += @"<tbody>";

            outXml += @"<thead><tr>
                        <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'> </td>
                        <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>
                        <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>
                      
                        
          </tr></thead>";
            outXml = outXml + @"<tr style = 'padding:5px;'>
            <th style='padding:10px; border:1px solid black;'>Sr. No</th>
            <th style='padding:10px; border:1px solid black;'>Resolution No.</br>Date of Meeting</th>
            <th style='padding:10px; border:1px solid black;word-wrap:break-word;'>Resolution Description</th>
           
                  </tr>";



            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {

                    if (mdl._LstReferences[i].TableItem == "0" || mdl._LstReferences[i].TableItem == "")
                    {
                        if (j == 0)
                        {
                            outXml += @"<tr>
										<td colspan='5' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
												Table Items</td></tr>";
                        }
                        j = j + 1;
                    }
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + Convert.ToString(i + 1) + @"    
										</td>
										<td style='padding: 10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(mdl._LstReferences[i].SerialNo) +"</br>" + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"     
                                        </td>
                                      
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    if (!string.IsNullOrEmpty(LOBText) && LOBText.Contains("<table"))
                    {
                        // Force all <table> to expand
                        LOBText = Regex.Replace(LOBText, "<table[^>]*>",
                            "<table style='width:100%; border-collapse:collapse; border:1px solid black;'>",
                            RegexOptions.IgnoreCase);

                        // Remove inline width attributes from <td>, <th>, <tr>
                        LOBText = Regex.Replace(LOBText, @"<(td|th|tr)([^>]*)\swidth\s*=\s*['""][^'""]*['""]", "<$1$2", RegexOptions.IgnoreCase);

                        // Ensure all <td> allow wrapping
                        LOBText = Regex.Replace(LOBText, "<td([^>]*)>", "<td$1 style='word-wrap:break-word; padding:5px;'>", RegexOptions.IgnoreCase);
                    }

                    // Replace searchText with a new line character
                    //LOBText = LOBText.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "</br>");

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }

                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"<b>Orders of Competent Authority :- </b>" + mdl._LstReferences[i].ActionDescription + "<br/>";

                    //outXml += @" < td style='padding:10px; border:1px solid black; text-align:justify;font-size:12px;text-align:justify;'>
                    //" + mdl._LstReferences[i].ActionDescription + @"
                    //</td>";


                    if (i == mdl._LstReferences.Count - 1)
                    {

                        outXml += "</tbody>";
                        outXml += "</table>";
                        outXml += @"<br/><br/>";

                        if (note != null && note.Any())
                        {
                            //var obituaryNotes = note?.Where(n => n.CategoryName == "Obituary").ToList();
                            var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();
                            foreach (var note1 in EndorsementNotes)
                            {
                                outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                            }
                        }
                        //if (signature != null || signature != "")
                        //{
                        //    outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                        //}
                        //if (HeaderlistofAgenda != null)
                        //{
                        //              outXml += @" <table style = 'width:100%;font-size: 14px;margin-top: 40px;' > ";
                        //              outXml += @"<tr>
                        //<td style='padding:10px;'>               
                        //		ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                              </td>
                        //<td style='padding: 10px;'>
                        //                              ਕਮਿਸ਼ਨਰ  <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                              </td>
                        //                              <td style ='padding:10px;'>
                        //                             ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //                              </td>
                        //                              <td style ='padding:10px;'>
                        //                               ਡਿਪਟੀ ਮੇਅਰ <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                        //
                        // </td></tr></table>";
                        //if (!isHoseExist && string.IsNullOrWhiteSpace(ChairpersonDesignation))
                        //{
                            outXml += @"<p style ='text-align: right; font-size: 18px;'>" 
                                         + ChairpersonDesignation + ",<br>" + mdl._LstReferences[0].FromDeptLocal
                          + "</p>";
                      //  }
                        //else if (!string.IsNullOrWhiteSpace(ChairpersonDesignation))
                        //{
                        //    outXml += @"<p style ='text-align: right; font-size: 18px;'>" + ChairpersonDesignation + @"<br>" + mdl._LstReferences[0].FromDeptLocal + "</p>";
                        //}
                        //else
                        //{
                        //    outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                        //                 ਪ੍ਰਧਾਨ,<br>" + mdl._LstReferences[0].FromDeptLocal
                        //   + "</p>";
                        //}
                    }

                }

                outXml += "</div></body> </html>";
            }
            return outXml;
        }


        //public string GetNewpdfhtmlImprovementTrust(int agendaid)
        //{

        //    Proceeding PDF first
        //    AgendaDetailModel mdl = new AgendaDetailModel();

        //    councillorlis start
        //    Councillorslist cl = new Councillorslist();
        //    string deptid = string.Empty;

        //    if (CurrentSession.DeptID != null)
        //    {
        //        deptid = CurrentSession.DeptID.ToString();
        //    }



        //    var note = AttendenceList.GetNotes(agendaid);
        //    bool isBacklog = AttendenceList.IsBackLogAgenda(agendaid);




        //    if (isBacklog == false)
        //    {
        //        if (note?.Any(n => n.CategoryName == "Administration Members") == false)
        //        {

        //            return @"
        //            <div style='color:#c0392b; font-weight:600;'>
        //            Administration-cum-Committee Members have not been added. Kindly add the required Members from the <b>“Add Note”</b> section.
        //           </div>";

        //        }
        //    }


        //    string officetype = string.Empty;
        //    officetype = AgendaModelFunction.GetOfficeType(agendaid);

        //    Authoritylist cl1 = new Authoritylist();
        //    cl1.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);


        //    string maintitle = string.Empty;
        //    mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForProcedingPDF(agendaid);
        //    mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
        //    maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
        //    var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

        //    var Chairperson = HeaderlistofAgenda.Chairperson.ToString();
        //    var ChairpersonDesignation = HeaderlistofAgenda.ChairpersonDesignation.ToString();
        //    var Header = HeaderlistofAgenda.Header.ToString();
        //    var MeetingTypeName = HeaderlistofAgenda.MeetingTypeName.ToString();

        //    var path1 = BlobStorage.GetStorageAcessingPath();
        //    string signature = path1 + " / " + CurrentSession.SignaturePath;
        //    string outXml = "";

        //    outXml += @"<style>
        //                /* Main table styling */
        //                table.maintable {
        //                    width: 100% !important;
        //                    border-collapse: collapse !important;
        //                    table-layout: fixed !important; /* enforce column widths */
        //                    word-break: break-word;
        //                    overflow-wrap: break-word;
        //                }

        //                /* Table cells */
        //                table.maintable th,
        //                table.maintable td {
        //                    padding: 10px !important;
        //                    border: 1px solid black !important;
        //                    white-space: normal !important; /* allow text to wrap */
        //                    word-wrap: break-word !important;
        //                    overflow-wrap: break-word !important;
        //                }




        //                /* Footer row stays at bottom of each page */
        //                tfoot {
        //                    display: table-footer-group !important;
        //                }

        //                /* Allow rows to split across pages */
        //                table.maintable tr {
        //                    page-break-inside: auto; /* allow row content to split */
        //                }

        //                /* Cells allow content to split across pages */
        //                table.maintable td {
        //                    page-break-inside: auto;
        //                }

        //                /* Remove restrictive max-widths from legacy classes */
        //                .pdf-class .MsoNormalTable,
        //                .pdf-class .MsoTableGrid {
        //                    max-width: none !important;
        //                    margin-left: 0 !important;
        //                }

        //                .pdf-class .MsoNormalTable td,
        //                .pdf-class .MsoTableGrid td {
        //                    max-width: none !important;
        //                    white-space: normal !important;
        //                    word-wrap: break-word !important;
        //                    overflow-wrap: break-word !important;
        //                    padding: 1px !important;
        //                }

        //                /* Additional body rules */
        //                body {
        //                    page-break-inside: auto;
        //                    overflow: visible;
        //                    word-wrap: break-word;
        //                }

        //                /* Vertical align */
        //                td {
        //                    vertical-align: top;
        //                }

        //                /* Hide headers when printing */
        //                @media print {
        //                    .header {
        //                        display: none;
        //                    }
        //                }

        //                /* Misc table adjustments */
        //                table {
        //                    width: 100%;
        //                }

        //                ol li {
        //                    padding: 5px;
        //                }
        //                @page {
        //                    margin: 20mm 10mm;
        //                    @top-center {
        //                        content: "";
        //                        border-bottom: 1px solid black;
        //                        display: block;
        //                        margin-bottom: 5mm;
        //                    }
        //                }
        //                </style>";


        //    string endingText = "";

        //    if (!isBacklog)
        //    {
        //        endingText = " ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—";
        //    }

        //    if (HeaderlistofAgenda != null && mdl._LstReferences.Count > 0 && cl1.AuthorityId_list.Count > 0)
        //    {
        //        outXml += @"<div>
        //                <h1 style='font-size: 26px; font-weight: 700; text-align: center; border-bottom: 3px solid #000;'>"
        //            + mdl._LstReferences[0].FromDeptLocal + "</h1>";
        //        outXml += @"<p style='font-size: 16px; text-align: center'>";

        //        if (!string.IsNullOrWhiteSpace(Chairperson) && !isBacklog)
        //        {
        //            outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ " + MeetingTypeName + ": " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + Chairperson + "," + ChairpersonDesignation + " " + mdl._LstReferences[0].FromDeptLocal + "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ।" + endingText + "</p></div>";
        //        }
        //        else if (!string.IsNullOrWhiteSpace(Chairperson) && isBacklog)
        //        {
        //            outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ " + MeetingTypeName + ": " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + Chairperson + "," + ChairpersonDesignation + " " + mdl._LstReferences[0].FromDeptLocal + "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ। </p></div>";
        //        }
        //        else
        //        {
        //            outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ।" + endingText + "</p></div>";
        //        }

        //    }


        //    if (note != null && note.Any())
        //    {
        //        var ExOfficio = note?.Where(n => n.CategoryName == "ExOfficio").ToList();
        //        var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

        //        foreach (var note1 in ExOfficio)
        //        {
        //            outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
        //        }
        //    }
        //    if (note != null && note.Any())
        //    {
        //        var AdministrationMembers = note?.Where(n => n.CategoryName == "Administration Members").ToList();
        //        var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

        //        foreach (var note1 in AdministrationMembers)
        //        {
        //            outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
        //        }
        //    }



        //    if (note != null && note.Any())
        //    {
        //        var obituaryNotes = note?.Where(n => n.CategoryName == "Obituary").ToList();
        //        var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

        //        foreach (var note1 in obituaryNotes)
        //        {
        //            outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
        //        }
        //    }


        //    outXml = outXml + @"<div>";
        //    if (mdl._LstExtraReferences.Count > 0)
        //    {
        //        for (int i = 0; i < mdl._LstExtraReferences.Count; i++)
        //        {
        //            outXml += @"<p>" + Convert.ToString(mdl._LstExtraReferences[i].SubjectDetails) + "</p><br>";

        //        }
        //    }
        //    outXml += @"<div style='width:100%;'>
        //    <table class='maintable' id='agendaTable' style='border-collapse:collapse;font-size:12pt;'>
        //    <colgroup>
        //            <col style = 'width:4%'>
        //            <col style = 'width:10%'>
        //            <col style = 'width:67%'>
        //            <col style = 'width:20%'>
        //           </colgroup> ";
        //    outXml += @"<tfoot>
        //    <tr>
        //        <td colspan='4' style='border-top: 1px solid black !important; border-left: none !important; border-right: none !important; border-bottom: none !important; padding:0;'></td>
        //    </tr>
        //    </tfoot>";

        //    outXml += @"<tbody>";

        //    outXml += @"<thead><tr>
        //                <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'> </td>
        //                <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>
        //                <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>
        //                <td style='border-top: none !important; border-left: none !important; border-right: none !important; border-bottom: 1px solid black !important ; padding:0;'></td>


        //  </tr></thead>";
        //    outXml = outXml + @"<tr style = 'padding:5px;'>
        //    <th style='padding:10px; border:1px solid black;'>Sr. No</th>
        //    <th style='padding:10px; border:1px solid black;'>Resolution No.</br>Date of Meeting</th>
        //    <th style='padding:10px; border:1px solid black;word-wrap:break-word;'>Resolution Description</th>
        //    <th style='padding:10px; border:1px solid black;word-wrap:break-word;'>Orders of Competent Authority</th>
        //          </tr>";



        //    if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
        //    {
        //        int j = 0;
        //        for (int i = 0; i < mdl._LstReferences.Count; i++)
        //        {

        //            if (mdl._LstReferences[i].TableItem == "0" || mdl._LstReferences[i].TableItem == "")
        //            {
        //                if (j == 0)
        //                {
        //                    outXml += @"<tr>
        //		<td colspan='5' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
        //				Table Items</td></tr>";
        //                }
        //                j = j + 1;
        //            }
        //            outXml += @"<tr>
        //		<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
        //				" + Convert.ToString(i + 1) + @"    
        //		</td>
        //		<td style='padding: 10px; border: 1px solid black;border - collapse: collapse; '>
        //                                " + Convert.ToString(mdl._LstReferences[i].SerialNo) + "</br>" + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"     
        //                                </td>

        //                                <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
        //            string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

        //            if (!string.IsNullOrEmpty(LOBText) && LOBText.Contains("<table"))
        //            {
        //                Force all<table> to expand

        //               LOBText = Regex.Replace(LOBText, "<table[^>]*>",
        //                   "<table style='width:100%; border-collapse:collapse; border:1px solid black;'>",
        //                   RegexOptions.IgnoreCase);

        //                Remove inline width attributes from<td>, < th >, < tr >
        //               LOBText = Regex.Replace(LOBText, @"<(td|th|tr)([^>]*)\swidth\s*=\s*['""][^'""]*['""]", "<$1$2", RegexOptions.IgnoreCase);

        //                Ensure all<td> allow wrapping

        //               LOBText = Regex.Replace(LOBText, "<td([^>]*)>", "<td$1 style='word-wrap:break-word; padding:5px;'>", RegexOptions.IgnoreCase);
        //            }

        //            Replace searchText with a new line character
        //           LOBText = LOBText.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "</br>");

        //            string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
        //            if (_subject != "")
        //            {

        //                _subject = "<b>" + _subject + " :- </b>";
        //            }

        //            outXml += @"" + _subject + " " + LOBText + "<br/>";

        //            outXml += @"<td style='padding:10px; border:1px solid black; text-align:justify;font-size:12px;text-align:justify;'>
        //            " + mdl._LstReferences[i].ActionDescription + @"
        //            </td>";


        //            if (i == mdl._LstReferences.Count - 1)
        //            {

        //                outXml += "</tbody>";
        //                outXml += "</table>";
        //                outXml += @"<br/><br/>";

        //                if (note != null && note.Any())
        //                {
        //                    var obituaryNotes = note?.Where(n => n.CategoryName == "Obituary").ToList();
        //                    var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();
        //                    foreach (var note1 in EndorsementNotes)
        //                    {
        //                        outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
        //                    }
        //                }

        //                if (!string.IsNullOrWhiteSpace(Chairperson))
        //                {
        //                    outXml += @"<p style ='text-align: right; font-size: 18px;'>" + ChairpersonDesignation + @"<br>" + mdl._LstReferences[0].FromDeptLocal + "</p>";
        //                }
        //                else
        //                {
        //                    outXml += @"<p style ='text-align: right; font-size: 18px;'> 
        //                                 ਚੇਅਰਮੈਨ,<br>" + mdl._LstReferences[0].FromDeptLocal
        //                   + "</p>";
        //                }
        //            }

        //        }

        //        outXml += "</div></body> </html>";
        //    }
        //    return outXml;
        //}

        public string GetNewpdfhtmlImprovementTrust(int agendaid)
        {
            // Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();

            // councillorlis start
            Councillorslist cl = new Councillorslist();
            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }

            var note = AttendenceList.GetNotes(agendaid);
            bool isBacklog = AttendenceList.IsBackLogAgenda(agendaid);

            if (isBacklog == false)
            {
                if (note?.Any(n => n.CategoryName == "Administration Members") == false)
                {
                    return @"
                <div style='color:#c0392b; font-weight:600;'>
                    Administration-cum-Committee Members have not been added.
                    Kindly add the required Members from the <b>“Add Note”</b> section.
                </div>";
                }
            }

            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);

            Authoritylist cl1 = new Authoritylist();
            cl1.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);

            string maintitle = string.Empty;

            mdl._LstReferences =
                AgendaModelFunction.GetAdminlobDataForProcedingPDF(agendaid);

            mdl._LstExtraReferences =
                AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);

            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);

            var HeaderlistofAgenda =
                AgendaModelFunction.GetAgendaHeader(agendaid);

            var Chairperson = HeaderlistofAgenda.Chairperson.ToString();
            var ChairpersonDesignation =
                HeaderlistofAgenda.ChairpersonDesignation.ToString();

            var Header = HeaderlistofAgenda.Header.ToString();
            var MeetingTypeName =
                HeaderlistofAgenda.MeetingTypeName.ToString();

            var path1 = BlobStorage.GetStorageAcessingPath();

            string signature =
                path1 + " / " + CurrentSession.SignaturePath;

            string outXml = "";

            // ============================================================
            // CSS
            // ============================================================

            outXml += @"
<style>

    /* ============================================================
       MAIN TABLE
       ============================================================ */

    table.maintable {
        width: 100% !important;
        max-width: 100% !important;
        border-collapse: collapse !important;
        table-layout: fixed !important;
        word-break: break-word !important;
        overflow-wrap: anywhere !important;
    }

    table.maintable th,
    table.maintable td {
        padding: 10px !important;
        border: 1px solid black !important;
        white-space: normal !important;
        word-wrap: break-word !important;
        word-break: break-word !important;
        overflow-wrap: anywhere !important;
        vertical-align: top !important;
        box-sizing: border-box !important;
        min-width: 0 !important;
    }

    /* ============================================================
       RESOLUTION DESCRIPTION
       ============================================================ */

    .resolution-content {
        width: 100% !important;
        max-width: 100% !important;
        min-width: 0 !important;
        overflow: hidden !important;
        word-wrap: break-word !important;
        word-break: break-word !important;
        overflow-wrap: anywhere !important;
        white-space: normal !important;
        box-sizing: border-box !important;
    }

    /* Any table inside Resolution Description */
    .resolution-content table {
        width: 100% !important;
        max-width: 100% !important;
        min-width: 0 !important;
        table-layout: fixed !important;
        border-collapse: collapse !important;
        box-sizing: border-box !important;
    }

    /* Inner table cells */
    .resolution-content table td,
    .resolution-content table th {
        max-width: 100% !important;
        min-width: 0 !important;
        white-space: normal !important;
        word-wrap: break-word !important;
        word-break: break-word !important;
        overflow-wrap: anywhere !important;
        vertical-align: top !important;
        box-sizing: border-box !important;
        overflow: hidden !important;
    }

    /* Images inside Resolution Description */
    .resolution-content img {
        max-width: 100% !important;
        width: auto !important;
        height: auto !important;
        box-sizing: border-box !important;
    }

    /* Paragraphs / spans / anchors */
    .resolution-content p,
    .resolution-content span,
    .resolution-content a,
    .resolution-content div {
        max-width: 100% !important;
        white-space: normal !important;
        word-wrap: break-word !important;
        word-break: break-word !important;
        overflow-wrap: anywhere !important;
        box-sizing: border-box !important;
    }

    /* Long URLs */
    .resolution-content a {
        overflow-wrap: anywhere !important;
        word-break: break-all !important;
    }

    /* ============================================================
       MS WORD TABLES
       ============================================================ */

    .pdf-class .MsoNormalTable,
    .pdf-class .MsoTableGrid {
        max-width: 100% !important;
        width: 100% !important;
        margin-left: 0 !important;
        table-layout: fixed !important;
    }

    .pdf-class .MsoNormalTable td,
    .pdf-class .MsoTableGrid td {
        max-width: 100% !important;
        min-width: 0 !important;
        white-space: normal !important;
        word-wrap: break-word !important;
        word-break: break-word !important;
        overflow-wrap: anywhere !important;
        padding: 1px !important;
        box-sizing: border-box !important;
    }

    /* ============================================================
       FOOTER
       ============================================================ */

    tfoot {
        display: table-footer-group !important;
    }

    /* ============================================================
       ALLOW ROWS TO SPLIT ACROSS PAGES
       ============================================================ */

    table.maintable tr {
        page-break-inside: auto !important;
    }

    table.maintable td {
        page-break-inside: auto !important;
    }

    body {
        page-break-inside: auto;
        overflow: visible;
        word-wrap: break-word;
        overflow-wrap: anywhere;
    }

    td {
        vertical-align: top;
    }

    /* ============================================================
       PRINT
       ============================================================ */

    @media print {
        .header {
            display: none;
        }
    }

    /* ============================================================
       LIST
       ============================================================ */

    ol li {
        padding: 5px;
    }

    /* ============================================================
       PAGE
       ============================================================ */

    @page {
        margin: 20mm 10mm;

        @top-center {
            content: """";
            border-bottom: 1px solid black;
            display: block;
            margin-bottom: 5mm;
        }
    }

</style>";

            string endingText = "";

            if (!isBacklog)
            {
                endingText =
                    " ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—";
            }

            // ============================================================
            // HEADER
            // ============================================================

            if (HeaderlistofAgenda != null &&
                mdl._LstReferences.Count > 0 &&
                cl1.AuthorityId_list.Count > 0)
            {
                outXml += @"
<div style='width:100%;'>

    <h1 style='
        font-size:26px;
        font-weight:700;
        text-align:center;
        border-bottom:3px solid #000;
        word-wrap:break-word;
        overflow-wrap:anywhere;
    '>"
                + mdl._LstReferences[0].FromDeptLocal +
                @"</h1>";

                outXml += @"
    <p style='
        font-size:16px;
        text-align:center;
        word-wrap:break-word;
        overflow-wrap:anywhere;
    '>";

                if (!string.IsNullOrWhiteSpace(Chairperson) && !isBacklog)
                {
                    outXml +=
                        mdl._LstReferences[0].FromDeptLocal.ToString()
                        + " ਦੀ "
                        + MeetingTypeName
                        + ": "
                        + HeaderlistofAgenda.AgendaDate.ToString()
                        + " ਨੂੰ ਸਮਾਂ "
                        + HeaderlistofAgenda.StartTime
                        + " ਵਜੇ "
                        + Chairperson
                        + ","
                        + ChairpersonDesignation
                        + " "
                        + mdl._LstReferences[0].FromDeptLocal
                        + "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ।"
                        + endingText
                        + "</p></div>";
                }
                else if (!string.IsNullOrWhiteSpace(Chairperson) && isBacklog)
                {
                    outXml +=
                        mdl._LstReferences[0].FromDeptLocal.ToString()
                        + " ਦੀ "
                        + MeetingTypeName
                        + ": "
                        + HeaderlistofAgenda.AgendaDate.ToString()
                        + " ਨੂੰ ਸਮਾਂ "
                        + HeaderlistofAgenda.StartTime
                        + " ਵਜੇ "
                        + Chairperson
                        + ","
                        + ChairpersonDesignation
                        + " "
                        + mdl._LstReferences[0].FromDeptLocal
                        + "ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਹੋਈ। </p></div>";
                }
                else
                {
                    outXml +=
                        mdl._LstReferences[0].FromDeptLocal
                        + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: "
                        + HeaderlistofAgenda.AgendaDate.ToString()
                        + " ਨੂੰ ਸਮਾਂ "
                        + HeaderlistofAgenda.StartTime
                        + " ਵਜੇ "
                        + cl1.AuthorityId_list[0].name
                        + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  "
                        + mdl._LstReferences[0].FromDeptLocal
                        + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ."
                        + endingText
                        + "</p></div>";
                }
            }

            // ============================================================
            // EX-OFFICIO NOTES
            // ============================================================

            if (note != null && note.Any())
            {
                var ExOfficio =
                    note?.Where(n => n.CategoryName == "ExOfficio").ToList();

                foreach (var note1 in ExOfficio)
                {
                    outXml +=
                        @"<p style='word-wrap:break-word;overflow-wrap:anywhere;'>"
                        + Convert.ToString(note1.NoteText)
                        + "</p><br>";
                }
            }

            // ============================================================
            // ADMINISTRATION MEMBERS
            // ============================================================

            if (note != null && note.Any())
            {
                var AdministrationMembers =
                    note?.Where(n =>
                        n.CategoryName == "Administration Members").ToList();

                foreach (var note1 in AdministrationMembers)
                {
                    outXml +=
                        @"<p style='word-wrap:break-word;overflow-wrap:anywhere;'>"
                        + Convert.ToString(note1.NoteText)
                        + "</p><br>";
                }
            }

            // ============================================================
            // OBITUARY
            // ============================================================

            if (note != null && note.Any())
            {
                var obituaryNotes =
                    note?.Where(n => n.CategoryName == "Obituary").ToList();

                foreach (var note1 in obituaryNotes)
                {
                    outXml +=
                        @"<p style='word-wrap:break-word;overflow-wrap:anywhere;'>"
                        + Convert.ToString(note1.NoteText)
                        + "</p><br>";
                }
            }

            // ============================================================
            // EXTRA REFERENCES
            // ============================================================

            if (mdl._LstExtraReferences.Count > 0)
            {
                for (int i = 0;
                     i < mdl._LstExtraReferences.Count;
                     i++)
                {
                    outXml +=
                        @"<p style='word-wrap:break-word;overflow-wrap:anywhere;'>"
                        + Convert.ToString(
                            mdl._LstExtraReferences[i].SubjectDetails)
                        + "</p><br>";
                }
            }

            // ============================================================
            // MAIN TABLE
            // ============================================================

            outXml += @"
<div style='width:100%; max-width:100%; overflow:hidden;'>

<table class='maintable'
       id='agendaTable'
       style='
            border-collapse:collapse;
            font-size:12pt;
            width:100%;
            max-width:100%;
            table-layout:fixed;
       '>

<colgroup>
    <col style='width:4%;'>
    <col style='width:10%;'>
    <col style='width:66%;'>
    <col style='width:20%;'>
</colgroup>";

            // ============================================================
            // FOOTER
            // ============================================================

            outXml += @"
<tfoot>
    <tr>
        <td colspan='4'
            style='
                border-top:1px solid black !important;
                border-left:none !important;
                border-right:none !important;
                border-bottom:none !important;
                padding:0;
            '>
        </td>
    </tr>
</tfoot>";

            outXml += @"<tbody>";

            // ============================================================
            // HEADER PLACEHOLDER ROW
            // ============================================================

            outXml += @"
<thead>
<tr>
    <td style='
        border-top:none !important;
        border-left:none !important;
        border-right:none !important;
        border-bottom:1px solid black !important;
        padding:0;
    '></td>

    <td style='
        border-top:none !important;
        border-left:none !important;
        border-right:none !important;
        border-bottom:1px solid black !important;
        padding:0;
    '></td>

    <td style='
        border-top:none !important;
        border-left:none !important;
        border-right:none !important;
        border-bottom:1px solid black !important;
        padding:0;
    '></td>

    <td style='
        border-top:none !important;
        border-left:none !important;
        border-right:none !important;
        border-bottom:1px solid black !important;
        padding:0;
    '></td>
</tr>
</thead>";

            // ============================================================
            // COLUMN HEADER
            // ============================================================

            outXml += @"
<tr style='padding:5px;'>

    <th style='
        padding:10px;
        border:1px solid black;
        width:4%;
        word-wrap:break-word;
    '>
        Sr. No
    </th>

    <th style='
        padding:10px;
        border:1px solid black;
        width:10%;
        word-wrap:break-word;
    '>
        Resolution No.</br>
        Date of Meeting
    </th>

    <th style='
        padding:10px;
        border:1px solid black;
        width:66%;
        word-wrap:break-word;
        overflow-wrap:anywhere;
    '>
        Resolution Description
    </th>

    <th style='
        padding:10px;
        border:1px solid black;
        width:20%;
        word-wrap:break-word;
        overflow-wrap:anywhere;
    '>
        Orders of Competent Authority
    </th>

</tr>";

            // ============================================================
            // DATA
            // ============================================================

            if (mdl._LstReferences != null &&
                mdl._LstReferences.Count > 0)
            {
                int j = 0;

                for (int i = 0;
                     i < mdl._LstReferences.Count;
                     i++)
                {
                    // ====================================================
                    // TABLE ITEMS
                    // ====================================================

                    if (mdl._LstReferences[i].TableItem == "0" ||
                        mdl._LstReferences[i].TableItem == "")
                    {
                        if (j == 0)
                        {
                            outXml += @"
<tr>
    <td colspan='4'
        style='
            padding:10px;
            border:1px solid black;
            border-collapse:collapse;
            font-weight:bold;
            word-wrap:break-word;
            overflow-wrap:anywhere;
        '>
        Table Items
    </td>
</tr>";
                        }

                        j = j + 1;
                    }

                    // ====================================================
                    // DATA ROW
                    // ====================================================

                    outXml += @"
<tr>

    <td style='
        padding:10px;
        border:1px solid black;
        border-collapse:collapse;
        width:4%;
        vertical-align:top;
        word-wrap:break-word;
        overflow-wrap:anywhere;
    '>"
                        + Convert.ToString(i + 1) +
                        @"</td>

    <td style='
        padding:10px;
        border:1px solid black;
        border-collapse:collapse;
        width:10%;
        vertical-align:top;
        word-wrap:break-word;
        overflow-wrap:anywhere;
    '>"
                        + Convert.ToString(
                            mdl._LstReferences[i].SerialNo)
                        + "</br>"
                        + Convert.ToString(
                            HeaderlistofAgenda.AgendaDate)
                        + @"</td>";

                    // ====================================================
                    // RESOLUTION DESCRIPTION TD
                    // ====================================================

                    outXml += @"
<td class='resolution-description'
    style='
        padding:10px;
        border:1px solid black;
        width:66%;
        max-width:66%;
        min-width:0;
        vertical-align:top;
        text-align:justify;
        overflow:hidden;
        word-wrap:break-word;
        word-break:break-word;
        overflow-wrap:anywhere;
        white-space:normal;
        box-sizing:border-box;
    '>";

                    string LOBText =
                        Convert.ToString(
                            mdl._LstReferences[i].SubjectDetails);

                    // ====================================================
                    // NESTED TABLE FIX
                    // ====================================================

                    if (!string.IsNullOrEmpty(LOBText) &&
                        LOBText.IndexOf(
                            "<table",
                            StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Wrap entire resolution description
                        LOBText =
                            "<div class='resolution-content'>" +
                            LOBText +
                            "</div>";

                        // ------------------------------------------------
                        // Force inner tables to remain within parent TD
                        // ------------------------------------------------

                        LOBText = Regex.Replace(
                            LOBText,
                            "<table([^>]*)>",
                            "<table$1 style='width:100% !important; max-width:100% !important; min-width:0 !important; table-layout:fixed !important; border-collapse:collapse !important; box-sizing:border-box !important;'>",
                            RegexOptions.IgnoreCase);

                        // ------------------------------------------------
                        // Remove width attributes from inner elements
                        // ------------------------------------------------

                        LOBText = Regex.Replace(
                            LOBText,
                            @"(<(?:td|th|tr))([^>]*?)\s+width\s*=\s*(['""][^'""]*['""])",
                            "$1$2",
                            RegexOptions.IgnoreCase);

                        // ------------------------------------------------
                        // Remove existing style from TD/TH
                        // Prevent duplicate style attributes
                        // ------------------------------------------------

                        LOBText = Regex.Replace(
                            LOBText,
                            @"<(td|th)([^>]*)\s+style\s*=\s*(['""])(.*?)\3([^>]*)>",
                            "<$1$2$5>",
                            RegexOptions.IgnoreCase);

                        // ------------------------------------------------
                        // Force inner cells to wrap
                        // ------------------------------------------------

                        LOBText = Regex.Replace(
                            LOBText,
                            @"<(td|th)([^>]*)>",
                            "<$1$2 style='width:auto; max-width:100%; min-width:0; padding:5px; white-space:normal; word-wrap:break-word; word-break:break-word; overflow-wrap:anywhere; vertical-align:top; box-sizing:border-box;'>",
                            RegexOptions.IgnoreCase);

                        // ------------------------------------------------
                        // Images cannot exceed Resolution Description
                        // ------------------------------------------------

                        LOBText = Regex.Replace(
                            LOBText,
                            "<img([^>]*)>",
                            "<img$1 style='max-width:100% !important; width:auto !important; height:auto !important; box-sizing:border-box;'>",
                            RegexOptions.IgnoreCase);
                    }

                    // ====================================================
                    // SUBJECT
                    // ====================================================

                    string _subject =
                        Convert.ToString(
                            mdl._LstReferences[i].Subject);

                    if (_subject != "")
                    {
                        _subject = "<b>" + _subject + " :- </b>";
                    }

                    // ====================================================
                    // DESCRIPTION
                    // ====================================================

                    outXml +=
                        _subject
                        + LOBText
                        + "<br/>";

                    // ====================================================
                    // ACTION DESCRIPTION
                    // ====================================================

                    outXml += @"
<td style='
    padding:10px;
    border:1px solid black;
    width:20%;
    max-width:20%;
    min-width:0;
    vertical-align:top;
    text-align:justify;
    font-size:12px;
    white-space:normal;
    word-wrap:break-word;
    word-break:break-word;
    overflow-wrap:anywhere;
    overflow:hidden;
    box-sizing:border-box;
'>"
                        + Convert.ToString(
                            mdl._LstReferences[i].ActionDescription)
                        + @"</td>

</tr>";

                    // ====================================================
                    // LAST ROW
                    // ====================================================

                    if (i == mdl._LstReferences.Count - 1)
                    {
                        outXml += "</tbody>";
                        outXml += "</table>";
                        outXml += @"<br/><br/>";

                        // ================================================
                        // ENDORSEMENT NOTES
                        // ================================================

                        if (note != null && note.Any())
                        {
                            var EndorsementNotes =
                                note?.Where(n =>
                                    n.CategoryName == "Endorsement")
                                .ToList();

                            foreach (var note1 in EndorsementNotes)
                            {
                                outXml +=
                                    @"<p style='
                                word-wrap:break-word;
                                word-break:break-word;
                                overflow-wrap:anywhere;
                            '>"
                                    + Convert.ToString(note1.NoteText)
                                    + "</p><br>";
                            }
                        }

                        // ================================================
                        // SIGNATURE
                        // ================================================

                        outXml += @"
<p style='
    text-align:right;
    font-size:18px;
    word-wrap:break-word;
    overflow-wrap:anywhere;
'>"
                            + ChairpersonDesignation
                            + @"<br>"
                            + mdl._LstReferences[0].FromDeptLocal
                            + @"</p>";
                    }
                }
            }

            outXml += "</div></body></html>";

            return outXml;
        }


        public static string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string output = input;

            // 1️⃣ Decode HTML entities (&nbsp;, &amp;, etc.)
            output = HttpUtility.HtmlDecode(output);

            // 2️⃣ Normalize Unicode (NFC form) — fixes composed/decomposed characters
            output = output.Normalize(NormalizationForm.FormC);

            // 3️⃣ Replace ALL kinds of non-breaking and invisible spaces
            // (includes U+00A0, U+2000–U+200D, U+202F, U+FEFF, etc.)
            output = Regex.Replace(output, @"[\u00A0\u2000-\u200D\u202F\uFEFF]", " ");

            // 4️⃣ Remove zero-width and control characters except line breaks
            output = Regex.Replace(output, @"[\p{Cc}-[ \r\n\t]]", "");

            // 5️⃣ Replace multiple spaces/tabs/newlines with single space
            output = Regex.Replace(output, @"[ \t\r\n]+", " ");

            // 6️⃣ Trim final result
            output = output.Trim();

            return output;
        }

        //patial proceeding aman
        public string GetNewpdfhtmlPatiala(int agendaid, ref string meetingDate)
        {

            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();

            //councillorlis start
            Councillorslist cl = new Councillorslist();
            string deptid = string.Empty;
            Int32 mcid = 0;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            if (CurrentSession.StateId != null)
            {
                mcid = Convert.ToInt32(CurrentSession.StateId);
            }

            cl.Councillor_list = AgendaModelFunction.getPresentCouncillirslist(mcid, agendaid);
            var note = AttendenceList.GetNotes(agendaid);

            if (cl.Councillor_list == null || !cl.Councillor_list.Any())
            {
                // No attendance → Block PDF generation
                return "<h3 style='color:red;'>⚠ Attendance has not been marked yet. Cannot generate Proceedings PDF.</h3>";
            }

            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);

            Authoritylist cl1 = new Authoritylist();
            cl1.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);


            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForProcedingPDF(agendaid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            var path1 = BlobStorage.GetStorageAcessingPath();

            string logo = "";
            var dptobj = AgendaModelFunction.GetDeptData(deptid);
            var deptname = CurrentSession.DeptName;
            meetingDate = HeaderlistofAgenda.AgendaDate.ToString();
            if (dptobj != null)
            {
                if (dptobj.Deptname.Contains("Local"))
                {
                    logo = path1 + "/LG/" + CurrentSession.MCName + "/" + dptobj.logo;
                }
                else
                {
                    logo = path1 + "/MC/" + dptobj.Name + "/" + dptobj.logo;
                }
            }


            string signature = path1 + "/" + CurrentSession.SignaturePath;
            string outXml = "";
            // if(mdl.)


            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: ProceedingReport</title>                                    
                                        <style>
                                        table.maintable {
                                           width: 100%;
                                         border-collapse: collapse;      
                                         border-bottom: 1px solid black;
                                         border-left: 1px solid black;
                                         border-right: 1px solid black;
                                         }

tfoot {
  display: table-footer-group; /* forces this row to appear at bottom of each page */
}

table.maintable tr1,table.maintable td1{
 page-break-inside: avoid;
}

                                        body {
                                          page-break-inside: avoid;
                                          overflow: visible;
                                          word-wrap: break-word;
                                        }

                                    .MsoNormalTable tr,.MsoNormalTable td,.MsoNormalTable th,.MsoTableGrid tr,.MsoTableGrid td,.MsoTableGrid th,#tblfooter tr{
                                  page-break-inside: avoid !important;                                                                          
                                    }  
                                    .pdf-class .MsoNormal{
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            table{
                                            width:100%;
                                            }
                                          ol li {
                                          padding: 10px;
                                              }

                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";
            if (HeaderlistofAgenda != null && mdl._LstReferences.Count > 0 && cl1.AuthorityId_list.Count > 0)
            {
                outXml += @"<div style='font-size: 14pt;font-weight: bold;width: 100%;'>
                <div style='float:left;width: 100px;'>
                <img src='" + logo + @"' height='50' width='55' /></div>
                <div style='text-align: center;'><span style='border-bottom: 1px solid;'>ਦਫਤਰ  " + mdl._LstReferences[0].FromDeptLocal + @" </span><br>
                     <span style='border-top: 1px solid;'>OFFICE OF " + mdl._LstReferences[0].FromDept.ToUpper() + @"<span></div></div>";

                outXml += @" <div>";
                outXml += @"<p style='font-size: 16px;line-height:1.7;margin-top: 30px;'>";
                if (officetype == "FNCC")
                    outXml += "ਨਗਰ ਨਿਗਮ ਪਟਿਆਲਾ ਦੀ ਫਾਇਨਾਂਸ ਅਤੇ ਕੰਟਰੈਕਟ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਜੋ ਕਿ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ ਮਾਨਯੋਗ ਮੇਅਰ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਜੀ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਦੇ ਦਫਤਰ ਵਿਖੇ ਹੋਈ, ਜਿਸ ਵਿੱਚ ਉਨ੍ਹਾਂ ਤੋਂ ਇਲਾਵਾਂ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਹਿਬਾਨ ਨੇ ਹਿੱਸਾ ਲਿਆ:-";
                // outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ। ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—  </p></div>";
                else
                    outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl1.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਈ। ਜਿਸ ਵਿੱਚ ਹੇਠ ਲਿਖੇ ਮੈਂਬਰ ਸਾਹਿਬਾਨ ਹਾਜ਼ਰ ਆਏ:—   </p></div>";

            }


            // if (cl.Councillor_list[0].meetingType == "FNCC")
            if (officetype == "FNCC")
            {
                outXml += @"<ol>";
                if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
                {
                    for (int j = 0; j < cl.Councillor_list.Count; j++)
                    {
                        if (cl.Councillor_list[j].Designation.ToString().Trim() != "ਮੇਅਰ")
                        {
                            outXml += @"<li>" + cl.Councillor_list[j].councillorname + ",<br> ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                        }
                    }
                }
                else
                {
                    outXml += @"<li> ਸ਼੍ਰੀ ਪਰਮਵੀਰ ਸਿੰਘ, ਆਈ ਏ ਐਸ., <br>
                                    ਕਮਿਸ਼ਨਰ, ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ। </li>";
                    outXml += @"<li> ਸ਼੍ਰੀ ਹਰਿੰਦਰ ਕੋਹਲੀ, ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ, <br>
                                    ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                    outXml += @"<li> ਸ਼੍ਰੀ ਜਗਦੀਪ ਸਿੰਘ ਰਾਏ, ਡਿਪਟੀ ਮੇਅਰ, <br>
                                    ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।</li>";
                }
                outXml += @"</ol>";

                outXml += @"<p> ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖਿਆਂ ਮੱਦਾਂ ਦੇ ਵਿਚਾਰ ਕਰਨ ਉਪਰੰਤ ਫੈਂਸਲੇ ਕੀਤੇ ਗਏ:— </p><br>";
            }
            else
            {
                outXml += @" <table style='width:100%;font-size: 12pt; border: 1px solid black; border-collapse: collapse;'>";
                outXml += @"<tr>                    
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 120px;"">ਵਾਰਡ  ਨੰ: </th>
                      <th style=""border: 1px solid black; border-collapse: collapse; width: 350px;"">ਕੋਸਲਰ  ਦਾ  ਨਾਮ </th> 
                    </tr>";
                for (int j = 0; j < cl.Councillor_list.Count; j++)
                {
                    //first list of mayor that donot under mayor so skip first record
                    if (j == 0)
                    {
                        j = j + 1;
                    }
                    outXml += @"<tr>
                      <td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                    outXml += @"" + cl.Councillor_list[j].wardid;
                    outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'> "
                    + cl.Councillor_list[j].councillorname + "</td>";
                    j = j + 1;
                    if (cl.Councillor_list.Count > j)
                    {
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";


                        outXml += @"" + cl.Councillor_list[j].wardid;

                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; ' > " + cl.Councillor_list[j].councillorname + "</td>";
                    }
                    else
                    {
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += "</tr>";
                        break;
                    }
                    j = j + 1;
                    if (cl.Councillor_list.Count > j)
                    {
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse; text-align: center;'>";
                        outXml += @"" + cl.Councillor_list[j].wardid;
                        outXml += @"<td style='border: 1px solid black; border-collapse: collapse;'>"
                       + cl.Councillor_list[j].councillorname + "</td>";
                    }
                    else
                    {
                        outXml += @" <td style=""border: 1px solid black; border-collapse: collapse; text-align: center;""></td>
                                         <td style=""border: 1px solid black; border-collapse: collapse;""></td>";
                        outXml += "</tr>";
                        break;
                    }
                    outXml += "</tr>";
                }
                outXml += "</table><br>";
            }
            // }
            if (officetype == "FNCC")
            {
                outXml = outXml + @"<div>";
            }
            else
            {
                outXml = outXml + @"<div style='page-break-before: always;'>";
            }
            if (note != null && note.Any())
            {
                var obituaryNotes = note?.Where(n => n.CategoryName == "Obituary").ToList();
               
                foreach (var note1 in obituaryNotes)
                {
                    outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                }
            }
            //outXml = outXml + @"<div>";
            if (mdl._LstExtraReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstExtraReferences.Count; i++)
                {
                    outXml += @"<p>" + Convert.ToString(mdl._LstExtraReferences[i].SubjectDetails) + "</p><br>";
                }
            }
            outXml += @"</div> <table class='maintable' style='width:100%;font-size: 12pt;'>";
            outXml += @"<thead><tr> <td colspan='5'></td> </tr></thead>";
            outXml += @"<tfoot> <tr><td colspan='5'' style='border-bottom: 1px solid black;'></td> </tr></tfoot>";

            outXml = outXml + @"<tbody><tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Sr. No</th>
                  <th style ='padding:10px;border: 1px solid black;border - collapse: collapse;'>Resolution No.</th> 
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Date of Meeting</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Resolution Description </th>";
            if (officetype == "FNCC")
            {
                outXml += @"<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Orders of F&CC</th>";
            }
            else
            {
                outXml += @"<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;width: 15%;'>Orders of Competent Authority</th>";
            }
            outXml += @"<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;display:none'>Act</th></tr>";

            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {

                    if (mdl._LstReferences[i].TableItem == "0")
                    {
                        if (j == 0)
                        {
                            outXml += @"<tr>
										<td colspan='5' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
												Table Items</td></tr>";
                        }
                        j = j + 1;
                    }
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + Convert.ToString(i + 1) + @"    
										</td>
										<td style='padding: 10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(mdl._LstReferences[i].SerialNo) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    // Replace searchText with a new line character
                    //LOBText = LOBText.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "</br>");

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>
                                       " + Convert.ToString(mdl._LstReferences[i].ActionDescription) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;display:none'>
                                       " + Convert.ToString(mdl._LstReferences[i].Act) + @"
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</tbody></table>";
                        outXml += @"<br/><br/>";

                        if (note != null && note.Any())
                        {
                            var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();
                            foreach (var note1 in EndorsementNotes)
                            {
                                outXml += @"<p>" + Convert.ToString(note1.NoteText) + "</p><br>";
                            }
                        }
                        //outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                        //if (HeaderlistofAgenda != null)
                        //{
                        var mayorsign = "";
                        var comsign = "";
                        var srdeptmayorsign = "";
                        var depmayor = "";


                        if (officetype == "FNCC")
                        {


                            if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
                            {
                                for (int m = 0; m < cl.Councillor_list.Count; m++)
                                {
                                    if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਮੇਅਰ")
                                    {
                                        mayorsign = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                                    }
                                    else if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਕਮਿਸ਼ਨਰ")
                                    {
                                        comsign = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                                    }
                                    else if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ")
                                    {
                                        srdeptmayorsign = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                                    }
                                    else if (cl.Councillor_list[m].Designation.ToString().Trim() == "ਡਿਪਟੀ ਮੇਅਰ")
                                    {
                                        depmayor = path1 + "/" + cl.Councillor_list[m].signature.ToString();
                                    }
                                }
                            }



                            outXml += @" <table id='tblfooter' style = 'width:100%;font-size: 14px;margin-top: 40px;' > ";
                            outXml += @"<tr><td style='padding:10px;'> <div style='padding-top:5px'>";
                            if (mayorsign != "")
                                outXml += @"<img src='" + mayorsign + @"'alt='Signature' style='height:50px;'>";
                            else
                                outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                            outXml += @"</div> ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                                        </td>
										<td style='padding: 10px;'>
<div style='padding-top:5px'>";
                            if (comsign != "")
                                outXml += @"<img src='" + comsign + @"'alt='Signature' style='height:50px;'>";
                            else
                                outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                            outXml += @"</div>  ਕਮਿਸ਼ਨਰ  <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                                        </td>
                                        <td style ='padding:10px;'>
<div style='padding-top:5px'>";
                            if (srdeptmayorsign != "")
                                outXml += @"<img src='" + srdeptmayorsign + @"'alt='Signature' style='height:50px;'>";
                            else
                                outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                            outXml += @"</div>
                                       ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ <br > " + mdl._LstReferences[0].FromOfficeLocal + @"
                                        </td>
                                        <td style ='padding:10px;width:15%'>
<div style='padding-top:5px'>";
                            if (depmayor != "")
                                outXml += @"<img src='" + depmayor + @"'alt='Signature' style='height:50px;'>";
                            else
                                outXml += @"<img src='/./.:0' alt='' style='height:50px;'>";
                            outXml += @"</div>
                                         ਡਿਪਟੀ ਮੇਅਰ <br> " + mdl._LstReferences[0].FromOfficeLocal + @"
                                       </td></tr></table>";

                            //outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                            //                 ਮੇਅਰ,<br>" + mdl._LstReferences[0].FromOfficeLocal + "</p>";
                            //}

                        }
                        else
                        {
                            outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                                         ਮੇਅਰ,<br>" + mdl._LstReferences[0].FromOfficeLocal + "</p>";

                        }
                    }
                   
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }


      
        public string GeteCabinetAgendaProceddinghtml(int agendaid)
        {
            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();
            List<AgendaReferences> agendaReferenceslst = new List<AgendaReferences>();

            //councillorlis start
            Councillorslist cl = new Councillorslist();
            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }

            cl.Councillor_list = AgendaModelFunction.getCouncillirslist(deptid, agendaid);
            //end

            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForProcedingPDF(agendaid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            agendaReferenceslst = AgendaModelFunction.GetAgendaHeaderForPDF(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            var path1 = BlobStorage.GetStorageAcessingPath();

            string signature = path1 + "/Signature/" + CurrentSession.SignaturePath;
            string outXml = "";

            // string signature = path1 + "/Signature/" + CurrentSession.SignaturePath;

            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>NextGen e-Cabinet: ApprovedAgendaReport</title>                                  
                                        <style>
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            table{
                                            width:100%;
                                            }
                                       #bloc1
                       {
                        float:left;
						width: 160px;
                       }
					   #bloc2
					   {
					   text-align: center;
					   }
                         ul {
                          list-style-type: none;
                          margin: 0;
                          padding: 0;
                        }
                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");
            if (agendaReferenceslst.Count != 0 && agendaReferenceslst.Count > 0)
            {
                outXml += @"<div style='margin: 35px 70px;page-break-after: always;'>";
                outXml += @"<p style='font-size: 15pt; text-indent: 42px;'>          
                  ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਮਿਤੀ " + Convert.ToString(agendaReferenceslst[0].AgendaDate) + " ਦਿਨ " + Convert.ToString(Convert.ToDateTime(agendaReferenceslst[0].AgendaDate).DayOfWeek) + @" ਨੂੰ ਸਵੇਰੇ/ " + agendaReferenceslst[0].StartTime + @" ਵਜੇ, ਸਥਾਨ..........ਵਿਖੇ ਹੋਈ ਮੀਟਿੰਗ ਦੀ ਕਾਰਵਾਈ ਮੁੱਖ ਮੰਤਰੀ ਜੀ ਦੀ ਪ੍ਰਵਾਨਗੀ ਅਤੇ ਹਸਤਾਖਰਾਂ ਹਿੱਤ ਹੇਠ ਰੱਖੀ ਜਾਂਦੀ ਹੈ।  
</p>";
            }
            else
            {
                outXml += @"<div style='margin: 35px 70px;page-break-after: always;'>";
                outXml += @"<p style='font-size: 15pt; text-indent: 42px;'>          
                  ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਮਿਤੀ" + Convert.ToString(agendaReferenceslst[0].AgendaDate) + " ਦਿਨ " + Convert.ToString(Convert.ToDateTime(agendaReferenceslst[0].AgendaDate).DayOfWeek) + @" ਨੂੰ ਸਵੇਰੇ/ " + agendaReferenceslst[0].StartTime + @" ਵਜੇ, ਸਥਾਨ..........ਵਿਖੇ ਹੋਈ ਮੀਟਿੰਗ ਦੀ ਕਾਰਵਾਈ ਮੁੱਖ ਮੰਤਰੀ ਜੀ ਦੀ ਪ੍ਰਵਾਨਗੀ ਅਤੇ ਹਸਤਾਖਰਾਂ ਹਿੱਤ ਹੇਠ ਰੱਖੀ ਜਾਂਦੀ ਹੈ।  
</p>";
            }
            outXml += @" <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + @"' alt='Signature' style='height:50px;'><br>
			<span style='font-size: 15pt;'>ਮੁੱਖ ਸਕੱਤਰ</span> 			
			</div>
	<p style='float:left;font-size: 15pt;'>ਮਾਨਯੋਗ ਮੁੱਖ ਮੰਤਰੀ ਜੀ।</p>
</div>";
            //Second page
            outXml += @"<div style='margin: 35px 70px;'>  
 <p style='font-size: 15pt; text-indent: 5px;'>
 ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਮਿਤੀ " + Convert.ToString(agendaReferenceslst[0].AgendaDate) + " ਦਿਨ " + Convert.ToString(Convert.ToDateTime(agendaReferenceslst[0].AgendaDate).DayOfWeek) + @" ਨੂੰ ਸਵੇਰੇ/ " + agendaReferenceslst[0].StartTime + @" ਵਜੇ, ............ ਵਿਖੇ ਹੋਈ ਮੀਟਿੰਗ ਦੀ ਕਾਰਵਾਈ।
      </p>
	  <p style='font-size: 15pt;'>
	  <u>ਹਾਜ਼ਰ:-</u>
	  </p>
	   
	   <ol>
	   <li style='font-size: 20px; text-indent: 5px;'>
	   <span style='padding-left: 5px;'>ਮੁੱਖ ਮੰਤਰੀ| </span>
	   </li>
       </ol>	   
	 
	    <p style='font-size: 20px; text-indent: 10px;text-decoration:underline'>
		ਹਾਜ਼ਰ ਕੈਬਨਿਟ ਮੰਤਰੀ ਸਾਹਿਬਾਨਾਂ ਦੇ ਨਾਮ……..
		</p>
		
		  <p style='font-size: 15pt;'>
	  <u>ਹਾਜ਼ਰ:-</u>
	  </p>
	   
	   <ol>
	   <li style='font-size: 20px; text-indent: 15px;'>
	  ਸ੍ਰੀ ਕੇ ਏ ਪੀ ਸਿਨਹਾ , ਮੁੱਖ ਸਕੱਤਰ
	   </li>	 
	   <li style='font-size: 20px; text-indent: 15px;'>
	  	ਸ੍ਰੀ ........, ਵਿਸ਼ੇਸ਼ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ/ਮੁੱਖ ਮੰਤਰੀ
	   </li>	
	   <li style='font-size: 20px; text-indent: 15px;'>
	  	ਸ੍ਰੀ ..........., ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ 
	   </li>	
	   </ol>
	  
		<p style='font-size:15pt;text-decoration:underline'>
		ਮੀਟਿੰਗ ਦੀ ਆਈਟਮ ਵਾਰ ਕਾਰਵਾਈਃ 
		</p>
		<p  style='font-size:15pt;'>
		<ul style='padding-left:0px'>";
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<li>
		<p style='font-size:15pt;'>ਲੜੀ ਨੰ: " + Convert.ToString(i + 1) + @"     (ਸਾਲਾਨਾ ਆਈਟਮ ਨੰ: " + Convert.ToString(mdl._LstReferences[i].yearno) + @")</p>
		<table style='font-size: 15pt; border: 1px solid black;border-collapse: collapse;'>
		<tr style = 'padding:5px;'>";
                    outXml += @"<td style = 'padding:10px; border: 1px solid black;border - collapse: collapse; width:5%;'>
                     
                                             ਵਿਸ਼ਾ:-
                        
                        </td>";
                    outXml += @"<td style='padding:10px; border: 1px solid black;border - collapse: collapse;'>";

                    //string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }
                    outXml += @"" + _subject + "<br/>";
                    outXml += @"</td>
		</tr>";
                    outXml += @"</table>
                  <p style='font-size:15pt;text-indent:45pt'>" + mdl._LstReferences[i].FromDeptLocal + @" ਦੇ ਯਾਦ-ਪੱਤਰ ਮਿਤੀ " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @" ਤੇ ਵਿਚਾਰ ਵਟਾਂਦਰੇ ਉਪਰੰਤ " + mdl._LstReferences[i].ActionDescription + @"</p>
                  </li>";
                }
            }
            outXml += @"</ul></p>            		  
			<div class='maindiv'>
			<div style='float:left;width: 70%;font-size:15pt;'>(" + CurrentSession.Name + @")<br>" + CurrentSession.Designation + @"			
			</div>
			<div style='float:right;width: 30%;text-align: end;font-size:15pt;'>(ਭਗਵੰਤ ਮਾਨ)<br>
			ਮੁੱਖ ਮੰਤਰੀ ਪੰਜਾਬ
			</div></div>";
            outXml += " </div></div></body> </html>";
            return outXml;
        }

        public string GetJalpdfhtml(int agendaid)
        {
            //LOBModel lob = new LOBModel();
            //lob.ListAdminLOB = (List<AdminLOBModel>)AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            AgendaDetailModel mdl = new AgendaDetailModel();

            //councillorlis start
            Councillorslist cl = new Councillorslist();


            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }

            cl.Councillor_list = AgendaModelFunction.getCouncillirslist(deptid, agendaid);
            //end

            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            var hd = AgendaModelFunction.GetHouseDeatilsList(agendaid, deptid);

            string outXml = "";
            if ((mdl._LstReferences != null && mdl._LstReferences.Count > 0) && hd != null)
            {

                outXml = outXml + @"<html>                           
								 <body><div style='max-width:1170px; margin: 20px 30px'>
                                 <h1 style='text-align: center;color:red;'>" + hd.MCName_Local + @"</h1>          
                <div style = 'text-align: center; '>";
                var base64string = "";
                try
                {
                    var imgpath = Server.MapPath("/mlaDiary/" + hd.MCLogo);
                    base64string = AgendaModelFunction.ImageToBase64(imgpath);
                }
                catch
                {
                    base64string = "data:image/jpg;base64,";
                }
                outXml = outXml + @"<img height = '300px' src ='" + base64string + @"'> </div>
                <div style='text-align: center;'> 
                <h1 style='page-break-after: always;'>ਨਗਰ ਨਿਗਮ ਹਾਊਸ ਦੀ ਮੀਟਿੰਗ <br> ਤਾਰੀਖ਼ (" + Convert.ToString(hd.AgendaDate) + @")<br> ਦੀ ਕਾਰਵਾਈ <br> (ਮਤਾ ਨੰ. : 
                  " + 1.ToString() + "-" + Convert.ToString(mdl._LstReferences.Count - 1) + @")</h1></div>";
                //" + Convert.ToString(mdl._LstReferences[0].SrNo1) + " -" + Convert.ToString(mdl._LstReferences[mdl._LstReferences.Count - 1].SrNo1) + @")</h1></div>";
                outXml += @"<div style='page-break-after: always;'>
                          <p style='font-size: 15px; text-align: justify; font-weight: 600;'>
                          <u><b>" + hd.AgendaName + @"</b></u></p>
                          <p  style='font-size: 15px; text-align: justify'> 
                           " + hd.Header + "</p>";
                if (cl.Councillor_list != null && cl.Councillor_list.Count > 0)
                {
                    if (cl.Councillor_list[0].meetingType == "FNCC")
                    {
                        outXml += @"<ol>";
                        for (int j = 0; j < cl.Councillor_list.Count; j++)
                        {
                            outXml += @"<li>" + cl.Councillor_list[j].councillorname + "</li>";
                        }
                        outXml += @"</ol>";
                    }
                    else
                    {
                        outXml += @"<table style='border: 1px solid black; border-collapse: collapse; margin:30px 0 0 30px;'>
                            <tr><th style='border: 1px solid black; border-collapse: collapse; width: 120px; padding:5px;'> ਲੜੀ ਨੰ: </th>
                            <th style='border: 1px solid black; border-collapse: collapse; width: 150px; padding:5px;'> ਵਾਰਡ ਨੰ: </th>
                            <th style='border: 1px solid black; border-collapse: collapse; width: 350px; padding:5px;'> ਨਾਮ </th>
                            </tr>";
                        for (int j = 0; j < cl.Councillor_list.Count; j++)
                        {
                            outXml += @"<tr>
                            <td style='border: 1px solid black; border-collapse: collapse; text-align: center; padding:5px;'>"
                                 + (j + 1).ToString() + @"
                            </td>
                            <td style='border: 1px solid black; border-collapse: collapse; padding:5px;'>";
                            outXml += cl.Councillor_list[j].wardid;
                            outXml += @"</td>
                            <td style='border: 1px solid black; border-collapse: collapse; padding:5px;'>"
                                  + cl.Councillor_list[j].councillorname + @"</td>
                            </tr>";
                        }
                        outXml += @"</table>";
                    }
                    outXml += @"<p style='margin-top: 30px;font-weight: bold;'> ਕੁਲ ਹਾਜ਼ਰ ਮੈਬਰ :: " + (cl.Councillor_list.Count).ToString() + "</p>" +
                            "<p style='margin-top: 30px; text-align: center;'>" + hd.Footer + "<br>" + Convert.ToString(hd.AgendaDate) + "</p></div>";
                }
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<p style='font-size: 12pt;'><b> ਮਤਾ ਨੰਬਰ : " + Convert.ToString(i + 1) + @" </b></p>
                       <p></p>
                      
                       <p style='float:left;width:10%;font-size: 12pt;'><b>ਵਿਸ਼ਾ:-</b></p>               
                       <p style='float:left;width:90%;text-align: justify;font-size: 12pt;'> " + Convert.ToString(mdl._LstReferences[i].Subject) + @"</p>";
                    outXml += @"<p style='font-size: 30px;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);
                    outXml += "     " + LOBText + @"<br/></p>";
                    outXml += @"<p style='max-width: 90%; margin: auto; width:90%; font-size: 12pt; text-align:center;'> 
                ਫੈਸਲਾ: <br>" + Convert.ToString(mdl._LstReferences[i].ActionDescription) + @"
                    <br>" + hd.Footer + "<br>" + Convert.ToString(hd.AgendaDate) +
                    "</p>";
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }

        public ActionResult GetDataForDecisionDashboardStatus(string agendaid)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;

            AgendaModel model = new AgendaModel
            {


                Agendadecionlist = AgendaModelFunction.GetDataForDecisionList(agendaid)
            };

            return View("_Deptdecision", model);
            // return View(model);
        }


        public ActionResult DeptDecision()


        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {



                AgendaModel model = new AgendaModel
                {


                    Agendadecionlist = AgendaModelFunction.GetDataForDecisionList("0")
                };

                return View("DeptDecision", model);



            }
        }


        public ActionResult MOMDispatch()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                List<Dashboard> lstDashboard = new List<Dashboard>();
                LstDashboard model = new LstDashboard();
                try
                {
                    DataSet ds = new DataSet();
                    ds = DiaryViewModel.GetDataByDocTypeDispatch(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID), "Letter");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Dashboard db = new Dashboard();
                        db.Count = Convert.ToInt32(dr["cnt"]);
                        db.DocOrder = Convert.ToInt32(dr["DocOrder"]);
                        db.Count = Convert.ToInt32(dr["cnt"]);
                        db.DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]);
                        db.DocumentTypeName = Convert.ToString(dr["DocumentTypeName"]);
                        db.CssClass = Convert.ToString(dr["DocumentIcon"]);
                        db.documentId = Convert.ToString(dr["documentId"]);
                        lstDashboard.Add(db);
                    }
                    model._LstDashboard = lstDashboard;
                    return View("MOMDispatch", model);
                }

                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("MOMDispatch", model);
                }
            }
        }

        public ActionResult GetDataForDispatchDashboardStatus(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            if (deptid != "" || DocumentType != "" || status != "10" || term != "" || priority != "")
            {

                dt = AgendaModelFunction.GetDataForDispatchDashboardStatusFilter(CurrentSession.OfficeId, officeid, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, since, deptid, term, priority, out dtstatus);
            }
            else
            {
                dt = AgendaModelFunction.GetDataForDispatchDashboardStatus(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus);
            }
            if (dt.Rows.Count == 0 || dt.Rows.Count > 0)
            {
                sb = sb.Append("<thead><tr role='row' style='color: white; font-size:16px; font-weight:bold;'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'>Meeting Date</th>");
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'>Meeting Title</th>");
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'>Department</th>");
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'>Office</th>");
                sb = sb.Append("<th  class='center' style=''>Memorandums</th>");


                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<th class='center sts' style=' ' >" + result1[0].ItemArray[1].ToString().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</th>");
                    }
                }



                sb = sb.Append("<th  class='pdf' id='tdFrom'>Agenda/Proceedings</th></tr>");

                // Second Column Row
                sb = sb.Append("<tr role='row' class='' style='color: white; font-size:16px; font-weight:bold;'>");
                sb = sb.Append("<th  class='center blank' style=' ' id='tdFrom'></th>");
                sb = sb.Append("<th  class='center blank' style=' ' id='tdFrom'></th>");
                sb = sb.Append("<th  class='center blank' style=' ' id='tdFrom'></th>");
                sb = sb.Append("<th  class='center blank' style=' ' id='tdFrom'></th>");
                //sb = sb.Append("<th  class='center blank' style=''></th>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {

                        sb = sb.Append("<th class='center sts' style=' ' >" + result1[0].ItemArray[1].ToString().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</th>");
                    }
                }
                sb = sb.Append("<th class='center blank' style=''></th>");
                sb = sb.Append("</tr></thead>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            string Enpath1 = System.Web.Configuration.WebConfigurationManager.AppSettings["URL"];
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["AgendaId"] + "'>");

                // sb = sb.Append("<td data-sort=" + Convert.ToDateTime(drDyn["AgendaDate"]).ToString("yyyyMMdd") + " class='center'>" + Convert.ToDateTime(drDyn["AgendaDate"]).ToString("yyyyMMdd").Trim() + "</td>");
                sb = sb.Append("<td data-sort=" + Convert.ToDateTime(drDyn["AgendaDate"]).ToString("yyyyMMdd") + " class='center'>" + drDyn["AgendaDate"].ToString().Trim() + "</td>");
                sb = sb.Append("<td class='Subject'>" + drDyn["AgendaName"].ToString().Trim() + "</td>");

                sb = sb.Append("<td class='dept'>" + drDyn["deptname"].ToString().Trim() + "</td>");
                sb = sb.Append("<td class='office'>" + drDyn["officename"].ToString().Trim() + "</td>");





                //For Total
                int Total = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }

                sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["AgendaId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");
                //For Total

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center sts'><a onclick='GetDispatchDasdhboardDataStatusType(this)' DeptId='" + drDyn["AgendaId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='pdf'>");
                if (drDyn["filepath"].ToString() != "")
                {
                    sb = sb.Append("<a href='" + Enpath1 + "/" + drDyn["filepath"].ToString().Trim() + "' target='_blank'><img src = '/Assets/Home_Images/rpdf.png' alt = 'Download' title = 'Agenda PDF' width = '24' height = '24' /> </ a >");
                }
                if (drDyn["filepath2"].ToString() != "")
                {
                    sb = sb.Append("<a href='" + Enpath1 + "/" + drDyn["filepath2"].ToString().Trim() + "' target='_blank'><img src = '/Assets/Home_Images/gpdf.png' alt = 'Download' title = 'Proceeding PDF' width = '24' height = '24' /> </ a >");
                }

                sb = sb.Append("</td>");
                //sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["AgendaId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            //Grand Total
            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                    }
                }
                //Grand Total

                sb = sb.Append(" <tfoot><tr id='trtd0' class='sts'>");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                //sb = sb.Append("<td class='center'>Total</td> ");
                sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td>");
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                        sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='center'></td> </tr> </tfoot> ");
            }
            return Content(sb.ToString());
        }

        [HttpGet]
        public JsonResult GetAgendaDataForDispatchCount(string agendaid, string status, string since, string deptid, string officeid, string term, string priority, string documentype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = AgendaModelFunction.GetAgendaDataForDispatchCount(agendaid, officeid, CurrentSession.OfficeId, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority, documentype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDashboardDataForDispatchStatusTotalCount(string agendaid, string deptid, string officeid, string documentype, string status, string term, string priority, string since)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = AgendaModelFunction.GetDashboardDataForDispatchStatusTotalCount(agendaid, deptid, CurrentSession.DeptID, officeid, CurrentSession.OfficeId, documentype, status, term, priority, since, Convert.ToString(CurrentSession.UserID));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult MOMDiary()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                List<Dashboard> lstDashboard = new List<Dashboard>();
                LstDashboard model = new LstDashboard();
                try
                {
                    DataSet ds = new DataSet();
                    ds = DiaryViewModel.GetDataByDocTypeDispatch(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID), "Letter");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Dashboard db = new Dashboard();
                        db.Count = Convert.ToInt32(dr["cnt"]);
                        db.DocOrder = Convert.ToInt32(dr["DocOrder"]);
                        db.Count = Convert.ToInt32(dr["cnt"]);
                        db.DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]);
                        db.DocumentTypeName = Convert.ToString(dr["DocumentTypeName"]);
                        db.CssClass = Convert.ToString(dr["DocumentIcon"]);
                        db.documentId = Convert.ToString(dr["documentId"]);
                        lstDashboard.Add(db);
                    }
                    model._LstDashboard = lstDashboard;
                    return View("MOMDiary", model);
                }

                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("MOMDiary", model);
                }
            }
        }

        public ActionResult MOMProceedings()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                List<Dashboard> lstDashboard = new List<Dashboard>();
                LstDashboard model = new LstDashboard();
                try
                {
                    DataSet ds = new DataSet();
                    ds = DiaryViewModel.GetDataByDocTypeDispatch(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID), "Letter");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Dashboard db = new Dashboard();
                        db.Count = Convert.ToInt32(dr["cnt"]);
                        db.DocOrder = Convert.ToInt32(dr["DocOrder"]);
                        db.Count = Convert.ToInt32(dr["cnt"]);
                        db.DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]);
                        db.DocumentTypeName = Convert.ToString(dr["DocumentTypeName"]);
                        db.CssClass = Convert.ToString(dr["DocumentIcon"]);
                        db.documentId = Convert.ToString(dr["documentId"]);
                        lstDashboard.Add(db);
                    }
                    model._LstDashboard = lstDashboard;
                    return View("MOMProceedings", model);
                }

                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("MOMProceedings", model);
                }
            }
        }




        public ActionResult LGProceddings(Int64 RefId)
        {
            DataSet ds = AgendaModelFunction.LGPdfDiaried(Convert.ToString(RefId));

            string outXml = "";
            outXml = @"<style>
             body {
             margin-left: 100px;
             margin-right: 100px;
             padding: 20px;
             width: 210mm; /* A4 width in millimeters */
             height: 297mm; /* A4 height in millimeters */
             font-family: Raavi (Body CS);
            }
             h1 {
             margin-bottom: 10px;
             }
             h2 {
             margin-top: 10px;
             margin-bottom: 5px;
            }
             p {
             margin-bottom: 10px;
            }
            .tab {
            margin-left: 100px;
            }
            .tabtd {
            margin-left: 55px;
             }
            table {
            border-collapse: collapse;
            width: 100%;
            }
            th, td {
            border: 1px solid #ccc;
            padding: 8px;
            text-align: left;
            }
            th {
            font-weight: bold;
            }";

            outXml = outXml + @" </style> ";

            outXml = outXml + @"<html>                           
								 <body><div style='width: 100%;'><table style='width: 100%;'>";

            //   outXml = outXml + @"<p><label style='float: left'>No: hps-klm-2023/ 29489 </label> <label style='float: right' >Ph No. 0172- 2619145</label> </p>";
            //   outXml = outXml + @"<p></br><label style='float: left'>ਤਾਰੀਖ਼...........</label><label style='float: right' >Email: digresolutioncorp@gmail.com</label> </p>";

            //   outXml = outXml + @" <html>                           `
            //<body>";
            //                           //<div style='max-width:1170px; margin: 0 auto;'>";

            //        outXml = outXml + @"<div style='text-align: center;'> ਡਾਇਰੈਕਟੋਰੇਟ, ਸਥਾਨਕ ਸਰਕਾਰ ਵਿਭਾਗ, ਪੰਜਾਬ,</br> ਪਲਾਟ ਨੰ: 3, ਮਿਉਂਸਪਲ ਭਵਨ, ਸੈਕਟਰ-35 ਏ, ਚੰਡੀਗੜ੍ਹ </br> ਮਿਉਨਿਸਪਲ ਸੇਵਾ ਸੈੱਲ</div>";
            //       outXml += @"   <p>ਵੱਲੋ,</p>
            //       <p class='tab'>ਸਕੱਤਰ, ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ.ਨਗਰ।</p>
            //       <p class='tab'>ਪੱਤਰ ਨੰ: 23</p>
            //       <p class='tab'>ਮਿਤੀ: 11.01.2022</p>
            //       <p>ਵਿਸਾ: <label style='margin-left: 55px;' >ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ. ਨਗਰ ਦੇ ਮਤਾ ਨੰ. 101 ਤੋਂ 108 ਮਿਤੀ 10.01.2022 ਸਬੰਧੀ।<label></p>
            //       <p  style= 'font-weight: bold;' class='tab'>..........</p>
            //       <p class='tab'>ਵਿ.ਅੱ.ਪੱਤਰ ਵਾਚਣ ਦੀ ਖੇਚਲ ਕੀਤੀ ਜਾਵੇ।</p>
            //       <p>2<label style='margin-left: 55px;' >ਵਿ.ਅ.ਪੱਤਰ ਰਾਹੀਂ<label></p>

            //       <p>ਸਕੱਤਰ, ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ.ਨਗਰ, ਮੋਹਾਲੀ ਵੱਲੋਂ ਮਤਾ ਨੰ: 101 ਤੋਂ 108 ਮਿਤੀ 10.01.2022 ਦੀ ਕਾਪੀ ਅਗਲੇਰੀ ਕਾਰਵਾਈ ਹਿੱਤ ਇਸ ਦਫਤਰ ਨੂੰ ਭੇਜੀ ਹੈ ਜੋ ਕਿ ਇਸ ਦਫਤਰ ਵਿਖੇ ਈ-ਮੇਲ ਰਾਹੀਂ
            //                       12.01.2022 ਨੂੰ ਪ੍ਰਾਪਤ ਹੋਈ ਹੈ। ਜਿਸ ਸਬੰਧੀ ਦਫਤਰ ਦਾ ਸੁਝਾਉ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ ਜੀ:-
            //       </p>";

            outXml += @"<table>
        <thead>
            <tr>
                <th>ਮਤਾ ਨੰ:</th>
                <th>ਵੇਰਵਾ</th>
            </tr>
        </thead>
        <tbody>";
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    outXml += "<tr>";
                    outXml += @"<td style= 'width:25%'>" + Convert.ToString(dr["ResolutionNos"].ToString()) + "</td>";
                    outXml += @"<td>" + Convert.ToString(dr["ActionDescription"].ToString()) + "</td>";
                    outXml += "</tr>";
                }
            }

            outXml += @"</tbody></table>";
            outXml = outXml + @"</html>";
            string htmlString = outXml;
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), "Portrait", true);
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);
            string finalpdfpath = "lg/" + "Agenda.pdf";
            string folderpath = "~/MLADIARY/" + finalpdfpath;
            string agendapath = Server.MapPath(folderpath);
            pdfDoc.Save(agendapath);
            SavePDf("1", finalpdfpath);
            return File(folderpath, "application/pdf", "LGProceddings.pdf");

        }







        public ActionResult GetDataForDiaryDashboardStatusorder(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();



            if (deptid != "")
            {
                dt = AgendaModelFunction.GetDataForDiaryDashboardStatusorder(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), deptid, out dtstatus);

            }
            else
            {
                dt = AgendaModelFunction.GetDataForDiaryDashboardStatusorder(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus);

            }

            //if (deptid != "" || DocumentType != "" || status != "10" || term != "" || priority != "")
            //{

            //    dt = AgendaModelFunction.GetDataForDiaryDashboardStatusFilter(CurrentSession.OfficeId, officeid, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, since, deptid, term, priority, out dtstatus);
            //}
            //else
            //{
            //    dt = AgendaModelFunction.GetDataForDiaryDashboardStatus(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus);
            //}
            if (dt.Rows.Count == 0 || dt.Rows.Count > 0)
            {

                sb = sb.Append("<thead><tr role='row' style='color: white; font-size:16px; font-weight:bold;'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'>Department</th>");
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'>Office</th>");

                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'>Meeting Date</th>");
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'>Meeting Title</th>");

                sb = sb.Append("<th  class='center' style=''>Memorandums</th>");


                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<th class='center sts' style=' ' >" + result1[0].ItemArray[1].ToString().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</th>");
                    }
                }



                sb = sb.Append("<th  class='pdf' id='tdFrom'>Order Proceedings</th>");
                //sb = sb.Append("<th  class='pdf' id='tdFrom'>Proceedings Covering Letter</th></tr>");

                // Second Column Row
                sb = sb.Append("<tr role='row' style='color: white; font-size:16px; font-weight:bold;'>");
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'></th>");
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'></th>");
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'></th>");
                sb = sb.Append("<th  class='center' style=' ' id='tdFrom'></th>");
                //sb = sb.Append("<th  class='center' style=''></th>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {

                        sb = sb.Append("<th class='center sts' style=' ' >" + result1[0].ItemArray[1].ToString().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</th>");
                    }
                }
                sb = sb.Append("<th class='center' style=''></th>");
                sb = sb.Append("<th class='center' style=''></th>");
                sb = sb.Append("</tr></thead>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            string Enpath1 = System.Web.Configuration.WebConfigurationManager.AppSettings["URL"];
            string BlobPath = System.Web.Configuration.WebConfigurationManager.AppSettings["URL"];
            //string BlobPath = BlobStorage.GetStorageAcessingPath() + "/";
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["AgendaId"] + "'>");

                // sb = sb.Append("<td data-sort=" + Convert.ToDateTime(drDyn["AgendaDate"]).ToString("yyyyMMdd") + " class='center'>" + Convert.ToDateTime(drDyn["AgendaDate"]).ToString("yyyyMMdd").Trim() + "</td>");

                sb = sb.Append("<td class='dept'>" + drDyn["deptname"].ToString().Trim() + "</td>");
                sb = sb.Append("<td class='dept'>" + drDyn["officename"].ToString().Trim() + "</td>");

                sb = sb.Append("<td data-sort=" + drDyn["AgendaId"] + " class='center'>" + drDyn["AgendaDate"].ToString().Trim() + "</td>");
                sb = sb.Append("<td class='Subject'>" + drDyn["AgendaName"].ToString().Trim() + "</td>");







                //For Total
                int Total = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }

                sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["AgendaId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");
                //For Total

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center sts'><a onclick='GetDispatchDasdhboardDataStatusType(this)' DeptId='" + drDyn["AgendaId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='pdf'>");
                if (drDyn["DecisionPath"].ToString() != "")
                {
                    sb = sb.Append("<a href='" + BlobPath + drDyn["DecisionPath"].ToString().Trim() + "' target='_blank'><img src = '/Assets/Home_Images/gpdf.png' alt = 'Download' title = 'Agenda PDF' width = '24' height = '24' /> </ a >");
                }


                sb = sb.Append("</td>");
                //sb = sb.Append("<td class='pdf'>");
                int agendaid = Convert.ToInt32(drDyn["AgendaId"]);

                //sb.Append("<a href='" + Enpath1 + "/Agenda/GetProceedingCoveringLetter?AgendaId=" + agendaid + "&LetterType=1' target='_blank'><img src='/Assets/Home_Images/rpdf.png' alt='Download' title='Proceeding Covering Letter to Divisional Commissioner, Rupnagar Mandal' width='24' height='24' /></a>");
                ////sb.Append("<a href='" + Enpath1 + "/Agenda/GetProceedingCoveringLetter?AgendaId=" + agendaid + "&LetterType=2' target='_blank'><img src='/Assets/Home_Images/rpdf.png' alt='Download' title='Proceeding Covering Letter to Director (LG)' width='24' height='24' /></a>");
                //sb.Append("<a href='" + Enpath1 + "/Agenda/GetProceedingCoveringLetter?AgendaId=" + agendaid + "&LetterType=3' target='_blank'><img src='/Assets/Home_Images/rpdf.png' alt='Download' title='Proceeding Covering Letter to All Members' width='24' height='24' /></a>");
                //sb.Append("<a href='" + Enpath1 + "/Agenda/GetProceedingCoveringLetter?AgendaId=" + agendaid + "&LetterType=4' target='_blank'><img src='/Assets/Home_Images/rpdf.png' alt='Download' title='Proceeding Covering Letter to Joint Comr, Assistant Comr, All Branches' width='24' height='24' /></a>");

                sb = sb.Append("</td>");
                //sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["AgendaId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            //Grand Total
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                if (result1.Length > 0)
                {
                    int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                    int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                    //int cnt = 0;
                    GTotal = GTotal + cnt;
                }
            }
            //Grand Total
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append(" <tfoot><tr id='trtd0' class='sts'>");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                //sb = sb.Append("<td class='center'>Total</td> ");
                sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td>");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                        sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }

                sb = sb.Append("<td class='center'></td> </tr> </tfoot> ");
            }

            return Content(sb.ToString());
        }



        public ActionResult GetDataForDiaryDashboardProStatus(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();



            if (deptid != "")
            {
                dt = AgendaModelFunction.GetDataForDiaryDashboardStatus(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), deptid, out dtstatus);

            }
            else
            {
                dt = AgendaModelFunction.GetDataForDiaryDashboardStatus(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus);

            }

          
            if (dt.Rows.Count == 0 || dt.Rows.Count > 0)
            {
                sb = sb.Append("<thead><tr role='row' class='table-header-agenda'>");
                sb = sb.Append("<th class='center th-dept'>Department</th>");
                sb = sb.Append("<th class='center th-office'>Office</th>");
                sb = sb.Append("<th class='center th-date'>Meeting Date</th>");
                sb = sb.Append("<th class='center th-title'>Meeting Title</th>");
                sb = sb.Append("<th class='center th-proposals'>Resolutions</th>");
                sb = sb.Append("<th class='center th-agenda'>Proceedings</th>");
                sb = sb.Append("<th class='center th-covering'>Covering Letters</th>");
                sb = sb.Append("<th class='center th-circulation'>Online Circulation</th></tr></thead>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            string Enpath1 = System.Web.Configuration.WebConfigurationManager.AppSettings["URL"];
            string BlobPath = BlobStorage.GetStorageAcessingPath() + "/";
            foreach (DataRow drDyn in dt.Rows)
            {
                int agenda = Convert.ToInt32(drDyn["AgendaId"]);
                var CoveringLetters = PDFCoverList.GetCoveringLetters(CurrentSession.DeptID, agenda, "Resolutions");
                sb = sb.Append("<tr id='trtd" + drDyn["AgendaId"] + "'>");

                sb = sb.Append("<td class='dept-cell'><span class='dept-name'>" + drDyn["deptname"].ToString().Trim() + "</span></td>");
                sb = sb.Append("<td class='center office-cell'><span class='office-badge'>" + drDyn["officename"].ToString().Trim() + "</span></td>");

                sb = sb.Append("<td data-sort='" + drDyn["AgendaId"] + "' class='center date-cell'><div class='meeting-date-box'><i class='fa fa-calendar me-1 text-primary'></i>" + drDyn["AgendaDate"].ToString().Trim() + "</div></td>");
                sb = sb.Append("<td class='Subject title-cell'><div class='meeting-title-text'>" + drDyn["AgendaName"].ToString().Trim() + "</div></td>");

                sb = sb.Append("<td class='center proposals-cell'><a class='proposals-pill-btn' onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["AgendaId"] + "' DocTypeId='10' title='View Resolutions'><i class='fa fa-list-ol me-1'></i>" + drDyn["cnt"].ToString().Trim() + "</a></td>");

                sb = sb.Append("<td class='center agenda-cell'>");
                if (drDyn["filepath2"].ToString() != "")
                {
                    sb = sb.Append("<a href='" + BlobStorage.GetStorageAcessingPath() + "/" + drDyn["filepath2"].ToString().Trim() + "' target='_blank' class='btn-proceedings-pdf' title='View Proceedings PDF'><img src='/Assets/Home_Images/rpdf.png' alt='PDF' width='16' height='16' class='me-1' /><span>PDF</span></a>");
                }
                else
                {
                    sb = sb.Append("<span class='text-muted small'>-</span>");
                }
                sb = sb.Append("</td>");

                sb.Append("<td class='covering-cell center'>");
                if (CoveringLetters != null && CoveringLetters.Count > 0)
                {
                    int index = 0;
                    foreach (var letter in CoveringLetters)
                    {
                        if (!string.IsNullOrEmpty(letter.PdfPath))
                        {
                            sb.Append("<label class='covering-chip' title='Covering Letter " + (index + 1) + "'>");
                            sb.AppendFormat("<input type='checkbox' name='SelectedPdfPaths' onclick='UpdateCoveringLetterStatus(this)' PdfPath='{0}' value='{0}' class='form-check-input me-1' />",
                                System.Net.WebUtility.HtmlEncode(letter.PdfPath));
                            sb.AppendFormat("<a href='{0}' target='_blank' class='covering-link'><i class='fa fa-file-text-o text-danger me-1'></i>{1}</a>",
                                System.Net.WebUtility.HtmlEncode(letter.PdfPath), (index + 1));
                            sb.Append("</label> ");
                            index++;
                        }
                    }
                }
                else
                {
                    sb.Append("<span class='text-muted small'>-</span>");
                }
                sb.Append("</td>");

                sb.Append("<td class='OnlineCir center' style='white-space: nowrap;'>");
                bool hasDoc = false;
                string targetPath = "";
                if (drDyn["filepath2"].ToString() != "")
                {
                    targetPath = BlobStorage.GetStorageAcessingPath() + "/" + drDyn["filepath2"].ToString().Trim();
                    hasDoc = true;
                }
                else if (drDyn["filepath"].ToString() != "")
                {
                    targetPath = BlobStorage.GetStorageAcessingPath() + "/" + drDyn["filepath"].ToString().Trim();
                    hasDoc = true;
                }

                if (hasDoc)
                {
                    string agendaId = drDyn["AgendaId"].ToString();

                    // SMS Button
                    sb.Append("<button type='button' class='btn-cir-action btn-cir-sms me-2' ");
                    sb.Append("onclick='GetContactList(this)' ");
                    sb.Append("DeptId='" + agendaId + "' ");
                    sb.Append("agendaPath='" + targetPath + "' ");
                    sb.Append("Department='" + CurrentSession.DeptID + "' title='Circulate via SMS'>");
                    sb.Append("<i class='fa fa-commenting-o me-1'></i><span>SMS</span></button>");

                    // Email Button
                    sb.Append("<button type='button' class='btn-cir-action btn-cir-email' ");
                    sb.Append("onclick='GetContactListEmail(this)' ");
                    sb.Append("DeptId='" + agendaId + "' ");
                    sb.Append("agendaPath='" + targetPath + "' ");
                    sb.Append("Department='" + CurrentSession.DeptID + "' title='Circulate via Email'>");
                    sb.Append("<i class='fa fa-envelope-o me-1'></i><span>Email</span></button>");
                }
                else
                {
                    sb.Append("<span class='text-muted small'>-</span>");
                }
                sb.Append("</td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            //Grand Total
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                if (result1.Length > 0)
                {
                    int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                    int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                    //int cnt = 0;
                    GTotal = GTotal + cnt;
                }
            }
            //Grand Total
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append(" <tfoot><tr id='trtd0' class='sts'>");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                //sb = sb.Append("<td class='center'>Total</td> ");
                sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td>");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                        sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }

                sb = sb.Append("<td class='center'></td> </tr> </tfoot> ");
            }

            return Content(sb.ToString());
        }


        public ActionResult GetDataForDiaryDashboardStatus(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            if (deptid != "")
            {
                dt = AgendaModelFunction.GetDataForDiaryDashboardStatus(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), deptid, out dtstatus);

            }
            else
            {
                dt = AgendaModelFunction.GetDataForDiaryDashboardStatus(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus);

            }


            if (dt.Rows.Count == 0 || dt.Rows.Count > 0)
            {
                sb = sb.Append("<thead><tr role='row' class='table-header-agenda'>");
                sb = sb.Append("<th class='center th-dept'>Department</th>");
                sb = sb.Append("<th class='center th-office'>Office</th>");
                sb = sb.Append("<th class='center th-date'>Meeting Date</th>");
                sb = sb.Append("<th class='center th-title'>Meeting Title</th>");
                sb = sb.Append("<th class='center th-proposals'>Proposals</th>");
                sb = sb.Append("<th class='center th-agenda'>Agenda</th>");
                sb = sb.Append("<th class='center th-covering'>Covering Letters</th>");
                sb = sb.Append("<th class='center th-circulation'>Online Circulation</th></tr></thead>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            string Enpath1 = System.Web.Configuration.WebConfigurationManager.AppSettings["URL"];
            string BlobPath = System.Web.Configuration.WebConfigurationManager.AppSettings["URL"];
            //string BlobPath = BlobStorage.GetStorageAcessingPath() + "/";
            foreach (DataRow drDyn in dt.Rows)
            {
                int agenda = Convert.ToInt32(drDyn["AgendaId"]);
                var CoveringLetters = PDFCoverList.GetCoveringLetters(CurrentSession.DeptID, agenda, "Agenda");

                sb = sb.Append("<tr id='trtd" + drDyn["AgendaId"] + "'>");

                sb = sb.Append("<td class='dept-cell'><span class='dept-name'>" + drDyn["deptname"].ToString().Trim() + "</span></td>");
                sb = sb.Append("<td class='center office-cell'><span class='office-badge'>" + drDyn["officename"].ToString().Trim() + "</span></td>");

                sb = sb.Append("<td data-sort='" + drDyn["AgendaId"] + "' class='center date-cell'><div class='meeting-date-box'><i class='fa fa-calendar me-1 text-primary'></i>" + drDyn["AgendaDate"].ToString().Trim() + "</div></td>");
                sb = sb.Append("<td class='Subject title-cell'><div class='meeting-title-text'>" + drDyn["AgendaName"].ToString().Trim() + "</div></td>");
                sb = sb.Append("<td class='center proposals-cell'><a class='proposals-pill-btn' onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["AgendaId"] + "' DocTypeId='10' title='View Proposals'><i class='fa fa-list-ol me-1'></i>" + drDyn["cnt"].ToString().Trim() + "</a></td>");

                sb = sb.Append("<td class='center agenda-cell'>");
                if (drDyn["filepath"].ToString() != "")
                {
                    sb = sb.Append("<a href='" + BlobStorage.GetStorageAcessingPath() + "/" + drDyn["filepath"].ToString().Trim() + "' target='_blank' class='btn-agenda-pdf' title='View Agenda PDF'><img src='/Assets/Home_Images/gpdf.png' alt='PDF' width='16' height='16' class='me-1' /><span>PDF</span></a>");
                }
                else
                {
                    sb = sb.Append("<span class='text-muted small'>-</span>");
                }
                sb = sb.Append("</td>");

                sb.Append("<td class='covering-cell center'>");
                if (CoveringLetters != null && CoveringLetters.Count > 0)
                {
                    int index = 0;
                    foreach (var letter in CoveringLetters)
                    {
                        if (!string.IsNullOrEmpty(letter.PdfPath))
                        {
                            sb.Append("<label class='covering-chip' title='Covering Letter " + (index + 1) + "'>");
                            sb.AppendFormat("<input type='checkbox' name='SelectedPdfPaths' onclick='UpdateCoveringLetterStatus(this)' PdfPath='{0}' value='{0}' class='form-check-input me-1' />",
                                System.Net.WebUtility.HtmlEncode(letter.PdfPath));
                            sb.AppendFormat("<a href='{0}' target='_blank' class='covering-link'><i class='fa fa-file-text-o text-danger me-1'></i>{1}</a>",
                                System.Net.WebUtility.HtmlEncode(letter.PdfPath), (index + 1));
                            sb.Append("</label> ");
                            index++;
                        }
                    }
                }
                else
                {
                    sb.Append("<span class='text-muted small'>-</span>");
                }
                sb.Append("</td>");

                sb.Append("<td class='OnlineCir center' style='white-space: nowrap;'>");
                if (drDyn["filepath"].ToString() != "")
                {
                    string agendaId = drDyn["AgendaId"].ToString();
                    string agendaPath = BlobStorage.GetStorageAcessingPath() + "/" + drDyn["filepath"].ToString().Trim();

                    // SMS Button
                    sb.Append("<button type='button' class='btn-cir-action btn-cir-sms me-2' ");
                    sb.Append("onclick='GetContactList(this)' ");
                    sb.Append("DeptId='" + agendaId + "' ");
                    sb.Append("agendaPath='" + agendaPath + "' ");
                    sb.Append("Department='" + CurrentSession.DeptID + "' title='Circulate via SMS'>");
                    sb.Append("<i class='fa fa-commenting-o me-1'></i><span>SMS</span></button>");

                    // Email Button
                    sb.Append("<button type='button' class='btn-cir-action btn-cir-email' ");
                    sb.Append("onclick='GetContactListEmail(this)' ");
                    sb.Append("DeptId='" + agendaId + "' ");
                    sb.Append("agendaPath='" + agendaPath + "' ");
                    sb.Append("Department='" + CurrentSession.DeptID + "' title='Circulate via Email'>");
                    sb.Append("<i class='fa fa-envelope-o me-1'></i><span>Email</span></button>");
                }
                else
                {
                    sb.Append("<span class='text-muted small'>-</span>");
                }
                sb.Append("</td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            //Grand Total
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                if (result1.Length > 0)
                {
                    int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                    int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                    //int cnt = 0;
                    GTotal = GTotal + cnt;
                }
            }
            //Grand Total
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append(" <tfoot><tr id='trtd0' class='sts'>");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center'></td> ");
                //sb = sb.Append("<td class='center'>Total</td> ");
                sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td>");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                        sb = sb.Append("<td class='center'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }

                sb = sb.Append("<td class='center'></td> </tr> </tfoot> ");
            }

            return Content(sb.ToString());
        }

        [HttpGet]
        public JsonResult GetAgendaDataForDiaryCount(string agendaid, string status, string since, string deptid, string officeid, string term, string priority, string documentype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = AgendaModelFunction.GetAgendaDataForDiaryCount(agendaid, officeid, CurrentSession.OfficeId, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority, documentype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDashboardDataForDiaryStatusTotalCount(string agendaid, string deptid, string officeid, string documentype, string status, string term, string priority, string since)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = AgendaModelFunction.GetDashboardDataForDiaryStatusTotalCount(agendaid, deptid, CurrentSession.DeptID, officeid, CurrentSession.OfficeId, documentype, status, term, priority, since, Convert.ToString(CurrentSession.UserID));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // GET: Menu List By Id
        public JsonResult Edit(int id)
        {
            List<AgendaModel> model = new List<AgendaModel>();
            var data = AgendaEdit.GetRowById(id);

            model.AddRange(data);
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public JsonResult Update(AgendaModel model)
        {
            var data = AgendaEdit.UpdateRowById(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        public JsonResult UpdateMeetingNotice(AgendaModel model)
        {
            var data = AgendaEdit.UpdateMeetingRowById(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult EditAgendaMeeting(int id)
        {
            List<AgendaModel> model = new List<AgendaModel>();
            var data = AgendaEdit.GetRowById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult NewMlaDispatchEntryForm()
        {
            MlaDiaryData model = new MlaDiaryData();
            string isapex = CurrentSession.isApex;
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            try
            {
                model.mDepartmentListPbi = DiaryViewModel.getDepartmentPbi();
                if (SiteName == "CMNirdesh")
                {
                    if (isapex == "0")
                    {
                        //model.mDepartmentListPbi = model.mDepartmentListPbi.Where(x => x.Value == "HPD0001").First();
                        List<DepartmentSelectListItem> _list = model.mDepartmentListPbi.Where(x => x.Value == "HPD0001").ToList();
                        model.mDepartmentListPbi = _list;
                    }
                }
                else if (SiteName == "MC")
                {


                    List<DepartmentSelectListItem> _list = model.mDepartmentListPbi.Where(x => x.Value == "LG00001").ToList();
                    model.mDepartmentListPbi = _list;

                }

                model.TemplateList = DiaryViewModel.getTeamplateList(Convert.ToInt32(CurrentSession.OfficeId));
                model.ReferencePriorityTypeList = DiaryViewModel.GetPriorityTypeList();
                model.OfficeId = Convert.ToInt32(Session["officeid"]);
                model.officename = CurrentSession.OfficeName;
                model.deptname = CurrentSession.DeptName;
                var a = CurrentSession.SPMinId;
                model.DeptId = CurrentSession.DeptID;
                model.FromDeptId = CurrentSession.DeptID;
                model.OfficeId = Convert.ToInt32(Session["officeid"]);
                model.FromOfficeId = Convert.ToInt32(Session["officeid"]);
                model.DocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "R");
                // model.ActionTypeList = DiaryViewModel.GetAllActionTypeList();
                model.PaperEntryType = "NewEntry";
                model.BtnCaption = "Save";
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
            }

            return PartialView("_FileEntry", model);

        }

        public static string CleanHtmlBehaviour(string value)
        {
            value = Regex.Replace(value, "(<style.+?</style>)|(<script.+?</script>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return value;
        }

        //Proceeding preview



        public async Task<ActionResult> ApproveProceedingpreview(int AgendaId)
        {
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;
            var officeType = CurrentSession.officeType;
            //officetype = AgendaModelFunction.GetOfficeType(AgendaId);

            if (mcid == "1001" && officeType == "1")
                htmlString = GetNewpdfhtml(AgendaId);
            else if (mcid == "1002" && officeType == "1")
                htmlString = GetNewpdfhtml(AgendaId);
            //htmlString = GetNewpdfhtmlPatiala(AgendaId, ref meetingdate);
            else if (mcid != "1002" && mcid != "1001" && officeType == "1")
                htmlString = GetNewpdfhtml(AgendaId);

            else if (officeType == "4" || officeType == "5")
                htmlString = GetNewpdfhtmlImprovementTrust(AgendaId);

            else
                htmlString = GetNewpdfhtmlCounsil(AgendaId);




            //if (mcid == "1001") // Mohali
            //{
            //    htmlString = GetNewpdfhtml(AgendaId);
            //}
            //else if (mcid == "1002") // Patiala
            //{
            //    htmlString = GetNewpdfhtmlPatiala(AgendaId, ref meetingdate);
            //}
            //else
            //{
            //    htmlString = GetNewpdfhtml(AgendaId);
            //}

            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;

            var pdfOptions = new PdfOptions
            {
                Format = PuppeteerSharp.Media.PaperFormat.A4,
                Landscape = true,
                MarginOptions = new MarginOptions
                {
                    Top = "5mm",     // small margin so header line touches content
                    Bottom = "15mm",
                    Left = "10mm",
                    Right = "10mm"
                },
                PrintBackground = true,
                DisplayHeaderFooter = true,
                
                HeaderTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; text-align: center;'></div>",
                FooterTemplate = $@"
                <div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; padding-right:15px; display: flex; justify-content: space-between; align-items: center;'>
                    <div style='flex: 1;'></div>
                    <div style='flex: 1; text-align: center;'>{MCHeader}</div>
                    <div style='flex: 1; text-align: right;'>
                        Page <span class='pageNumber'></span> of <span class='totalPages'></span>
                    </div>
                </div>"

            };

            string watermarkCss = @"
        <style>
            .watermark {
                position: fixed;
                top: 50%;
                left: 50%;
                transform: translate(-50%, -50%) rotate(-45deg);
                font-size: 100px;
                color: rgba(180, 180, 180, 0.2);
                z-index: 9999;
                pointer-events: none;
                font-family: Arial, sans-serif;
                white-space: nowrap;
                text-align: center;
            }
        </style>
        <div class='watermark'>PREVIEW</div>";

            var browser = await GetBrowserAsync();
            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(watermarkCss + htmlString, new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.DOMContentLoaded }
                });

                var pdfBytes = await page.PdfDataAsync(pdfOptions);

                Response.Headers["Content-Disposition"] = "inline; filename=\"Proceeding.pdf\"";
                return File(pdfBytes, "application/pdf");
            }
        }

        public ActionResult ApproveProceedingpreviewDOCX1(int AgendaId)
        {
            // Get HTML string
            string htmlString = GetNewpdfhtml(AgendaId);

            // Ensure UTF-8 meta is included for Punjabi text
            if (!htmlString.Contains("<meta charset"))
            {
                htmlString = htmlString.Replace("<head>", "<head><meta charset=\"UTF-8\">");
            }

            string fileName = $"Proceeding_{AgendaId}.docx";

            // Ensure TempFiles folder exists
            string tempFolder = Server.MapPath("~/TempFiles");
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            // Temp HTML file
            string tempHtml = Path.Combine(tempFolder, "temp.html");
            // Output DOCX file
            string outputDocx = Path.Combine(tempFolder, Path.GetFileNameWithoutExtension(tempHtml) + ".docx");

            // Write HTML to temp file with UTF-8 encoding
            System.IO.File.WriteAllText(tempHtml, htmlString, System.Text.Encoding.UTF8);

            // Run LibreOffice headless to convert HTML → DOCX with export filter
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = @"E:\LibreOffice\program\soffice.exe"; // full path to soffice.exe
            process.StartInfo.Arguments = $"--headless --convert-to docx:\"MS Word 2007 XML\" \"{tempHtml}\" --outdir \"{tempFolder}\"";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new Exception($"LibreOffice conversion failed. StdErr: {error}");

            if (!System.IO.File.Exists(outputDocx))
                throw new Exception($"DOCX file was not created. StdErr: {error}");

            // Serve the DOCX file to the browser
            return File(outputDocx,
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                        fileName);
        }


        public ActionResult ApproveProceedingPreviewDOCX(int AgendaId)
        {
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;
            var officeType = CurrentSession.StateId;

            if (mcid == "1001" && officeType == "1")
                htmlString = GetNewpdfhtml(AgendaId);
            else if (mcid == "1002" && officeType == "1")
                htmlString = GetNewpdfhtmlPatiala(AgendaId, ref meetingdate);
            else if (mcid != "1002" && mcid != "1001" && officeType == "1")
                htmlString = GetNewpdfhtml(AgendaId);
            else
                htmlString = GetNewpdfhtmlCounsil(AgendaId);

            string formattedHtml = @"
                <html xmlns:o='urn:schemas-microsoft-com:office:office'
                      xmlns:w='urn:schemas-microsoft-com:office:word'
                      xmlns='http://www.w3.org/TR/REC-html40'>
                <head>
                    <meta charset='utf-8'>
                    <title>Agenda Document</title>

                    <!-- MS Word specific page setup -->
                    <xml>
                        <w:WordDocument>
                            <w:View>Print</w:View>
                            <w:Zoom>90</w:Zoom>
                            <w:DoNotOptimizeForBrowser/>
                        </w:WordDocument>
                    </xml>

                    <style>
                        /* Define Word Section with A4 landscape and margins */
                        @page WordSection1 {
                            size: 841.95pt 595.35pt;   /* A4 landscape */
                            mso-page-orientation: landscape;
                            margin:72pt 72pt 72pt 72pt; /* 1 inch margins (72pt = 1 inch) */
                        }
                        div.WordSection1 { page: WordSection1; }

                        body {
                            font-family: Calibri, sans-serif;
                            font-size: 12pt;
                            line-height: 1.5;
                            color: #000;
                        }

                        table {
                            border-collapse: collapse;
                            width: 100%;
                        }
                        th, td {
                            border: 1px solid #000;
                            padding: 6px;
                            vertical-align: top;
                            text-align: left;
                        }

                        h1, h2, h3 {
                            text-align: center;
                            margin: 10px 0;
                        }
                    </style>
                </head>
                <body>
                    <div class='WordSection1'>
                        " + htmlString + @"
                    </div>
                </body>
                </html>";

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/msword";
            Response.AddHeader("Content-Disposition", $"attachment;filename=Agenda_{AgendaId}.doc");
            Response.Charset = "";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write(formattedHtml);
            Response.Flush();
            Response.End();

            return new EmptyResult();
        }



        private string CleanMinimalHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return string.Empty;

            // Simple cleanup: remove scripts, styles, and comments
            html = System.Text.RegularExpressions.Regex.Replace(html, "<script.*?</script>", "", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = System.Text.RegularExpressions.Regex.Replace(html, "<style.*?</style>", "", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = System.Text.RegularExpressions.Regex.Replace(html, "<!--.*?-->", "", System.Text.RegularExpressions.RegexOptions.Singleline);

            return html.Trim();
        }


        public ActionResult ApproveProceedingpreview_old(int AgendaId)
        {
            //string htmlString = GetNewpdfhtml(AgendaId);

            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;
            if (mcid == "1001") //Mohali
            {
                htmlString = GetNewpdfhtml(AgendaId);
            }
            else if (mcid == "1002") //Patiala
            {
                // htmlString = GetNewpdfhtmlPatiala(AgendaId);
            }


            //string htmlString = GeteCabinetAgendaProceddinghtml(AgendaId);

            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"Digital MC House\" " +
                    "--footer-right \"Proceedings Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);


            // Add diagonal watermark using iTextSharp
            using (var inputPdfStream = new MemoryStream(pdfBytes))
            using (var outputPdfStream = new MemoryStream())
            {
                var reader = new iTextSharp.text.pdf.PdfReader(inputPdfStream);
                var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

                // Create the watermark
                var watermarkFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 100, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(192, 192, 192));
                var watermarkText = "Preview";

                // Loop through pages and apply watermark
                int pageCount = reader.NumberOfPages;
                for (int i = 1; i <= pageCount; i++)
                {
                    var pdfContent = stamper.GetOverContent(i);
                    iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(i);

                    // Set diagonal rotation
                    var gstate = new iTextSharp.text.pdf.PdfGState { FillOpacity = 0.5f }; // Semi-transparent
                    pdfContent.SaveState();
                    pdfContent.SetGState(gstate);

                    // Calculate position and rotation
                    pdfContent.BeginText();
                    pdfContent.SetColorFill(iTextSharp.text.BaseColor.LIGHT_GRAY);
                    pdfContent.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED), 100);

                    float x = (pageSize.Left + pageSize.Right) / 2;
                    float y = (pageSize.Top + pageSize.Bottom) / 2; // Below content
                    float angle = (float)Math.Atan2(pageSize.Top - pageSize.Bottom, pageSize.Right - pageSize.Left) * (180 / (float)Math.PI);
                    //float angle = 60; // Diagonal angle

                    pdfContent.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, watermarkText, x, y, angle);
                    pdfContent.EndText();

                    pdfContent.RestoreState();
                }

                // Close the stamper
                stamper.Close();
                reader.Close();
                // Get the final PDF with watermark
                pdfBytes = outputPdfStream.ToArray();
            }


            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";

            Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");


            return File(pdfBytes, "application/pdf");


        }


        //Approved Proceeding aman
        //public async Task<ActionResult> ApproveProceedingHtml(int AgendaId)
        //{
        //    string meetingdate = "";
        //    string htmlString = string.Empty;
        //    var mcid = CurrentSession.StateId;
        //    if (mcid == "1001") //Mohali
        //    {
        //        htmlString = GetNewpdfhtml(AgendaId);
        //    }
        //    else if (mcid == "1002") //Patiala
        //    {
        //        htmlString = GetNewpdfhtmlPatiala(AgendaId, ref meetingdate);
        //    }
        //    else
        //    {
        //        htmlString = GetNewpdfhtml(AgendaId);
        //    }

        //    var MCName = CurrentSession.MCName;
        //    var MCHeader = "Digital MC House " + MCName;

        //    //start Puppeteer
        //    string filePath = "";
        //    BrowserFetcher browserFetcher = new BrowserFetcher();
        //    await browserFetcher.DownloadAsync();


        //    var pdfOptions = new PuppeteerSharp.PdfOptions();
        //    pdfOptions.Format = PuppeteerSharp.Media.PaperFormat.A4;
        //    pdfOptions.Landscape = true;
        //    pdfOptions.MarginOptions = new MarginOptions
        //    {
        //        Top = "12mm",
        //        Left = "1cm",
        //        Right = "1cm",
        //        Bottom = "10mm",
        //    };
        //    pdfOptions.PrintBackground = true;
        //    pdfOptions.DisplayHeaderFooter = true;
        //    //pdfOptions.HeaderTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; text-align: center;'>" + MCHeader + "</div>";
        //    //pdfOptions.FooterTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px;'><div style='float: left; font-size: 13px;width:40%;padding-left:15px'><span style='font-size: 13px'> <span class='pageNumber'></span> of <span class='totalPages'></span></span></div><div style='float:left; font-size: 13px;width:20%'>Digital MC House</div><div style='float: right;text-align:right; font-size: 13px;width:35%;padding-right:15px;'>Proceeding Generated on: " + meetingdate + " </div></div>";

        //    pdfOptions.HeaderTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; text-align: center;'></div>";
        //    pdfOptions.FooterTemplate = @"
        //    <div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; padding-right:15px; display: flex; justify-content: space-between; align-items: center;'>
        //        <div style='flex: 1;'></div>
        //        <div style='flex: 1; text-align: center;'>" + MCHeader + @"</div>
        //        <div style='flex: 1; text-align: right;'>
        //            Page <span class='pageNumber'></span> of <span class='totalPages'></span>
        //        </div>
        //    </div>";




        //    using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        //    {
        //        ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
        //        Headless = true
        //        // ExecutablePath = "CHROME_BROWSER_PATH"
        //    }))

        //    using (var page = await browser.NewPageAsync())
        //    {
        //        await page.SetContentAsync(htmlString);
        //        page.DefaultNavigationTimeout = 0;
        //        // var result = await page.GetContentAsync();
        //        var pdfBytes = await page.PdfDataAsync(pdfOptions);

        //        //await page.PdfAsync(Server.MapPath("~/uploads/pdffromhtml" + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf"), pdfOptions);

        //        filePath = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";
        //        // await page.PdfAsync(filePath, pdfOptions);               ;
        //        BlobStorage.UploadPdfToBlob(pdfBytes, filePath);
        //        SavePDf2(AgendaId.ToString(), filePath, "ProceedingRpt");
        //        if (AgendaId != 0)
        //        {
        //            AgendaModelFunction.ApproveProceedings(Convert.ToInt32(AgendaId), 1);
        //        }
        //        // Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
        //        //  return File(pdfBytes, "application/pdf");
        //    }
        //    //Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
        //    //return File(pdfBytes, "application/pdf");
        //    string blobUrl = Convert.ToString(BlobStorage.GetStorageAcessingPath()) + "/" + filePath;
        //    return Redirect(blobUrl);

        //}

        public async Task<ActionResult> ApproveProceedingHtml(int AgendaId)
        {
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;

            var officeType = CurrentSession.officeType;

            if (mcid == "1001" && officeType == "1")
                htmlString = GetNewpdfhtml(AgendaId);
            else if (mcid == "1002" && officeType == "1")
                htmlString = GetNewpdfhtml(AgendaId);
            //htmlString = GetNewpdfhtmlPatiala(AgendaId, ref meetingdate);
            else if (mcid != "1002" && mcid != "1001" && officeType == "1")
                htmlString = GetNewpdfhtml(AgendaId);
            else if (officeType == "4" || officeType == "5")
                htmlString = GetNewpdfhtmlImprovementTrust(AgendaId);
            else
                htmlString = GetNewpdfhtmlCounsil(AgendaId);



            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;

            var pdfOptions = new PdfOptions
            {
                Format = PuppeteerSharp.Media.PaperFormat.A4,
                Landscape = true,
                MarginOptions = new MarginOptions
                {
                    Top = "12mm",
                    Left = "1cm",
                    Right = "1cm",
                    Bottom = "10mm"
                },
                PrintBackground = true,
                DisplayHeaderFooter = true,
                HeaderTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; text-align: center;'></div>",
                FooterTemplate = $@"
                <div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; padding-right:15px; display: flex; justify-content: space-between; align-items: center;'>
                    <div style='flex: 1;'></div>
                    <div style='flex: 1; text-align: center;'>{MCHeader}</div>
                    <div style='flex: 1; text-align: right;'>
                        Page <span class='pageNumber'></span> of <span class='totalPages'></span>
                    </div>
                </div>"
            };

            var browser = await GetBrowserAsync();
            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(htmlString, new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.DOMContentLoaded } // load faster
                });

                var pdfBytes = await page.PdfDataAsync(pdfOptions);
                Councillorslist cl = new Councillorslist();
                var deptid = "";
                if (CurrentSession.DeptID != "")
                {
                    deptid = CurrentSession.DeptID.ToString();
                }
                cl.Councillor_list = AgendaModelFunction.getCouncillirslist(deptid, AgendaId);
                //if (cl.Councillor_list == null || !cl.Councillor_list.Any())
                //{
                //    return File(pdfBytes, "application/pdf", "Procedding.pdf");
                //}
                //else
                //{
                    string filePath = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";
                    // Upload to Blob Storage
                    BlobStorage.UploadPdfToBlob(pdfBytes, filePath);
                    // Save record in DB
                    SavePDf2(AgendaId.ToString(), filePath, "ProceedingRpt");

                    if (AgendaId != 0)
                    {
                        AgendaModelFunction.ApproveProceedings(Convert.ToInt32(AgendaId), 1);
                    }


                    // Return blob link
                    string blobUrl = Convert.ToString(BlobStorage.GetStorageAcessingPath()) + "/" + filePath;
                    return Redirect(blobUrl);
                }
            }
        



        public ActionResult ApproveProceedingHtml_old(int AgendaId)
        {

            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;
            if (mcid == "1001") //Mohali
            {
                htmlString = GetNewpdfhtml(AgendaId);
            }
            else if (mcid == "1002") //Patiala
            {
                // htmlString = GetNewpdfhtmlPatiala(AgendaId);
            }


            //string htmlString = GetNewpdfhtml(AgendaId);
            //string htmlString = GeteCabinetAgendaProceddinghtml(AgendaId);

            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"Digital MC House\" " +
                    "--footer-right \"Proceedings Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            SavePDf2(AgendaId.ToString(), pdfFilename, "ProceedingRpt");
            if (AgendaId != 0)
            {
                AgendaModelFunction.ApproveProceedings(Convert.ToInt32(AgendaId), 1);
            }
            //Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
            //return File(pdfBytes, "application/pdf");
            string blobUrl = Convert.ToString(BlobStorage.GetStorageAcessingPath()) + "/" + pdfFilename;
            return Redirect(blobUrl);

        }

        //circularproceeding for patiala temporary

        public async Task<ActionResult> CircularProceedingHtmlNew(int AgendaId)
        {
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;
            if (mcid == "1001") //Mohali
            {
                htmlString = GetCircularhtmlMohali(AgendaId, ref meetingdate);
            }
            else if (mcid == "1002") //Patiala
            {
                htmlString = GetCircularhtmlPatiala(AgendaId, ref meetingdate);
            }


            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;
            var currentDt = DateTime.Now;
            //var _dt="29/05/2025";
            var _dt = meetingdate;


            //start Puppeteer
            string filePath = "";
            BrowserFetcher browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();


            var pdfOptions = new PuppeteerSharp.PdfOptions();
            pdfOptions.Format = PuppeteerSharp.Media.PaperFormat.A4;
            pdfOptions.Landscape = true;
            pdfOptions.MarginOptions = new MarginOptions
            {
                Top = "12mm",
                Left = "1cm",
                Right = "1cm",
                Bottom = "10mm",
            };
            pdfOptions.PrintBackground = true;
            pdfOptions.DisplayHeaderFooter = true;
            pdfOptions.HeaderTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; text-align: center;'></div>";
            pdfOptions.FooterTemplate = @"
            <div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; padding-right:15px; display: flex; justify-content: space-between; align-items: center;'>
                <div style='flex: 1;'></div>
                <div style='flex: 1; text-align: center;'>" + MCHeader + @"</div>
                <div style='flex: 1; text-align: right;'>
                    Page <span class='pageNumber'></span> of <span class='totalPages'></span>
                </div>
            </div>";

            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                Headless = true
                // ExecutablePath = "CHROME_BROWSER_PATH"
            }))

            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(htmlString);
                page.DefaultNavigationTimeout = 0;
                var pdfBytes = await page.PdfDataAsync(pdfOptions);

                //await page.PdfAsync(Server.MapPath("~/uploads/pdffromhtml" + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf"), pdfOptions);

                filePath = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";
                // await page.PdfAsync(filePath, pdfOptions);               ;
                BlobStorage.UploadPdfToBlob(pdfBytes, filePath);
                SavePDfProceedingCircular(AgendaId.ToString(), filePath);
                if (AgendaId != 0)
                {
                    AgendaModelFunction.ApproveProceedings(Convert.ToInt32(AgendaId), 1);
                }
                Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
                return File(pdfBytes, "application/pdf");
            }
        }
        public ActionResult CircularProceedingHtmlNew_old(int AgendaId)
        {
            string meetingdate = "";
            string htmlString = GetCircularhtmlPatiala(AgendaId, ref meetingdate);
            //string htmlString = GeteCabinetAgendaProceddinghtml(AgendaId);

            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;
            var currentDt = DateTime.Now;
            //var _dt="29/05/2025";
            var _dt = meetingdate;
            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(10, 10, 10, 10),

                // Custom switches for footer, header, and other styling options
                // CustomSwitches = "--disable-smart-shrinking"
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"Digital MC House\" " +
                    "--footer-right \"Proceedings Generated on: " + _dt + "\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            SavePDf2(AgendaId.ToString(), pdfFilename, "ProceedingRpt");
            if (AgendaId != 0)
            {
                AgendaModelFunction.ApproveProceedings(Convert.ToInt32(AgendaId), 1);
            }
            Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
            return File(pdfBytes, "application/pdf");

        }

        public ActionResult CircularProceedingHtml(int AgendaId)
        {
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;
            if (mcid == "1001") //Mohali
            {
                htmlString = GetNewpdfhtml(AgendaId);
            }
            else if (mcid == "1002") //Patiala
            {
                htmlString = GetNewpdfhtmlPatiala(AgendaId, ref meetingdate);
            }
            // string htmlString = GetNewpdfhtml(AgendaId);
            //string htmlString = GeteCabinetAgendaProceddinghtml(AgendaId);

            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"Digital MC House\" " +
                    "--footer-right \"Proceedings Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            SavePDf2(AgendaId.ToString(), pdfFilename, "ProceedingRpt");
            if (AgendaId != 0)
            {
                AgendaModelFunction.ApproveProceedings(Convert.ToInt32(AgendaId), 1);
            }
            Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
            return File(pdfBytes, "application/pdf");

        }

        //for preview draft agenda button

        public ActionResult PreviewApprovedAgendaHtmlNew(int AgendaId, string doctype)
        {

            string htmlString = GetNewapprovepdfhtmlPreview(AgendaId, doctype);
            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"Digital MC House \" " +
                    "--footer-right \"Agenda Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);

            // Add diagonal watermark using iTextSharp
            using (var inputPdfStream = new MemoryStream(pdfBytes))
            using (var outputPdfStream = new MemoryStream())
            {
                var reader = new iTextSharp.text.pdf.PdfReader(inputPdfStream);
                var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

                // Create the watermark
                var watermarkFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 100, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(192, 192, 192));
                var watermarkText = "Preview";

                // Loop through pages and apply watermark
                int pageCount = reader.NumberOfPages;
                for (int i = 1; i <= pageCount; i++)
                {
                    var pdfContent = stamper.GetOverContent(i);
                    iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(i);

                    // Set diagonal rotation
                    var gstate = new iTextSharp.text.pdf.PdfGState { FillOpacity = 0.5f }; // Semi-transparent
                    pdfContent.SaveState();
                    pdfContent.SetGState(gstate);

                    // Calculate position and rotation
                    pdfContent.BeginText();
                    pdfContent.SetColorFill(iTextSharp.text.BaseColor.LIGHT_GRAY);
                    pdfContent.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED), 100);

                    float x = (pageSize.Left + pageSize.Right) / 2;
                    float y = (pageSize.Top + pageSize.Bottom) / 2; // Below content
                    float angle = (float)Math.Atan2(pageSize.Top - pageSize.Bottom, pageSize.Right - pageSize.Left) * (180 / (float)Math.PI);
                    //float angle = 60; // Diagonal angle

                    pdfContent.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, watermarkText, x, y, angle);
                    pdfContent.EndText();

                    pdfContent.RestoreState();
                }

                // Close the stamper
                stamper.Close();
                reader.Close();
                // Get the final PDF with watermark
                pdfBytes = outputPdfStream.ToArray();
            }





            string pdfFilename = $"Agenda/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Agenda.pdf";


            Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
            return File(pdfBytes, "application/pdf");
        }

        //public async Task<ActionResult> ApprovedAgendaHtmlpreview(int AgendaId, string doctype)
        //{
        //    string meetingdate = "";
        //    string htmlString = string.Empty;
        //    var mcid = CurrentSession.StateId;


        //    if (mcid == "1001") 
        //        htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 0);
        //    else if (mcid == "1002") 
        //        htmlString = GetNewapprovepdfPatiala(AgendaId, doctype, ref meetingdate, 0);
        //    else
        //        htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 0);


        //    var MCName = CurrentSession.MCName;
        //    var MCHeader = "Digital MC House " + MCName;

        //    DateTime dt = DateTime.Now.Date;
        //    //DateTime meetingdt = DateTime.ParseExact(meetingdate, "dd/MM/yyyy", null);

        //    //if (DateTime.Compare(dt, meetingdt) < 0)
        //    //    meetingdate = dt.Date.ToString("dd/MM/yyyy");


        //    var pdfOptions = new PuppeteerSharp.PdfOptions
        //    {
        //        Format = PuppeteerSharp.Media.PaperFormat.A4,
        //        Landscape = false,
        //        PrintBackground = true,
        //        PreferCSSPageSize = true,
        //        DisplayHeaderFooter = true,
        //        MarginOptions = new MarginOptions
        //        {
        //            Top = "30px", 
        //            Bottom = "50px", 
        //            Left = "20px",
        //            Right = "20px"
        //        },
        //        HeaderTemplate = @"
        //        <div style='font-size:10px; height:10px; width:100%; text-align:center; 
        //                    color:#fff; background:#0055ff; line-height:10px;'>
        //        </div>",
        //        FooterTemplate = @"
        //            <div style='font-size:10px; height:30px; width:100%; color:#fff; 
        //                        background:#0055ff; display:flex; justify-content:space-between; 
        //                        align-items:center; padding:0 15px;'>
        //                <span>" + MCHeader + @"</span>
        //                <span>Page <span class='pageNumber'></span> of <span class='totalPages'></span></span>
        //            </div>"
        //    };


        //     string watermarkCss = @"
        //        <style>
        //          body { margin:0; padding:0; font-family: Arial, sans-serif; }
        //          .watermark {
        //            position: fixed;
        //            top: 50%;
        //            left: 50%;
        //            transform: translate(-50%, -50%) rotate(-45deg);
        //            font-size: 12vw;
        //            color: rgba(180, 180, 180, 0.15);
        //            z-index: 9999;
        //            pointer-events: none;
        //            white-space: nowrap;
        //            text-align: center;
        //          }
        //          .avoid-break {
        //            page-break-after: avoid;
        //            page-break-before: avoid;
        //            page-break-inside: avoid;
        //          }
        //        </style>";


        //    var fullHtml = @"
        //    <!DOCTYPE html>
        //    <html>
        //    <head>
        //        <meta charset='utf-8'>
        //        " + watermarkCss + @"
        //    </head>
        //    <body>
        //        <div class='watermark'>PREVIEW</div>
        //        <div class='avoid-break'>
        //            " + htmlString + @"
        //        </div>
        //    </body>
        //    </html>";


        //    using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        //    {
        //        ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
        //        Headless = true,
        //        Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
        //    }))
        //    using (var page = await browser.NewPageAsync())
        //    {
        //        await page.SetContentAsync(fullHtml, new NavigationOptions
        //        {
        //            WaitUntil = new[] { WaitUntilNavigation.Load }
        //        });


        //        var pdfBytes = await page.PdfDataAsync(pdfOptions);

        //        // Return inline preview
        //        Response.Headers.Add("Content-Disposition", "inline; filename=AgendaPreview.pdf");
        //        return File(pdfBytes, "application/pdf");
        //    }
        //}

        public async Task<ActionResult> ApprovedAgendaHtmlpreview(int AgendaId, string doctype)
        {
            string meetingdate = "";
            string htmlString = string.Empty;

            var mcid = CurrentSession.StateId;
            var officeType = CurrentSession.officeType;


            if (mcid == "1001" && officeType== "1")
                htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 0);
            else if (mcid == "1002" && officeType == "1")
                htmlString = GetNewapprovepdfPatiala(AgendaId, doctype, ref meetingdate, 0);
             else if (mcid != "1002" && mcid != "1001" && officeType == "1")
                htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 0);
            else
                htmlString = GetNewapprovepdfhtmlCouncil(AgendaId, doctype, ref meetingdate, 0);


              var MCHeader = "Digital MC House " + CurrentSession.MCName;

            /* ---------------- PDF OPTIONS (MARGINS FIXED) ---------------- */
            var pdfOptions = new PuppeteerSharp.PdfOptions
            {
                Format = PuppeteerSharp.Media.PaperFormat.A4,
                Landscape = false,
                PrintBackground = true,
                DisplayHeaderFooter = true,
                PreferCSSPageSize = false, // 🔴 CRITICAL FIX
                MarginOptions = new MarginOptions
                {
                    Top = "60px",
                    Bottom = "70px",
                    Left = "25px",
                    Right = "25px"
                },
                HeaderTemplate = @"
            <div style='width:100%; height:40px; background:#0055ff;'></div>",
                FooterTemplate = @"
            <div style='width:100%; height:50px; background:#0055ff;
                        color:#fff; font-size:10px;
                        display:flex; justify-content:space-between;
                        align-items:center; padding:0 15px;'>
                <span>" + MCHeader + @"</span>
                <span>Page <span class='pageNumber'></span> of <span class='totalPages'></span></span>
            </div>"
            };

            /* ---------------- GLOBAL CSS (TD CUTTING FIX) ---------------- */
            string globalCss = @"
<style>
body {
    margin: 0;
    padding: 0;
    font-family: Arial, sans-serif;
}

/* 🔑 FIX: stop text cutting inside TD */
th, td {
    overflow: visible !important;
    word-break: break-word !important;
    overflow-wrap: anywhere !important;
    white-space: normal !important;
}

/* Wrapper safety */
.pdf-wrapper {
    max-width: calc(100% - 10px) !important;
    margin: 0 auto;
}

/* Watermark */
.watermark {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%) rotate(-45deg);
    font-size: 10vw;
    color: rgba(180,180,180,0.12);
    pointer-events: none;
    z-index: 0;
}

.pdf-content {
    position: relative;
    z-index: 1;
}
</style>";

            /* ---------------- FINAL HTML ---------------- */
            string fullHtml = @"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
" + globalCss + @"
</head>
<body>

<div class='watermark'>PREVIEW</div>

<div class='pdf-content'>
" + htmlString + @"
</div>

</body>
</html>";

            /* ---------------- PUPPETEER ---------------- */
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                Headless = true,
                Args = new[]
                {
            "--no-sandbox",
            "--disable-setuid-sandbox",
            "--disable-dev-shm-usage"
        }
            }))
            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(fullHtml, new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Load }
                });

                var pdfBytes = await page.PdfDataAsync(pdfOptions);

                Response.Headers.Add(
                    "Content-Disposition",
                    "inline; filename=AgendaPreview.pdf"
                );

                return File(pdfBytes, "application/pdf");
            }
        }

        public ActionResult ApprovedAgendaHtmlpreview_old(int AgendaId, string doctype)
        {
            //string htmlString = GeteCabinetAgendafhtml(AgendaId, doctype);
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;
            if (mcid == "1001") //Mohali
            {
                htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 0);
            }
            else if (mcid == "1002") //Patiala
            {
                htmlString = GetNewapprovepdfPatiala(AgendaId, doctype, ref meetingdate, 0);
            }

            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                // PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"Digital MC House\" " +
                    "--footer-right \"Agenda Generated on: [date]\" " +
                    "--footer-font-size 8 --footer-spacing 2 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);

            // Add diagonal watermark using iTextSharp
            using (var inputPdfStream = new MemoryStream(pdfBytes))
            using (var outputPdfStream = new MemoryStream())
            {
                var reader = new iTextSharp.text.pdf.PdfReader(inputPdfStream);
                var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

                // Create the watermark
                var watermarkFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 100, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(192, 192, 192));
                var watermarkText = "Preview";

                // Loop through pages and apply watermark
                int pageCount = reader.NumberOfPages;
                for (int i = 1; i <= pageCount; i++)
                {
                    var pdfContent = stamper.GetOverContent(i);
                    iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(i);

                    // Set diagonal rotation
                    var gstate = new iTextSharp.text.pdf.PdfGState { FillOpacity = 0.5f }; // Semi-transparent
                    pdfContent.SaveState();
                    pdfContent.SetGState(gstate);

                    // Calculate position and rotation
                    pdfContent.BeginText();
                    pdfContent.SetColorFill(iTextSharp.text.BaseColor.LIGHT_GRAY);
                    pdfContent.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED), 100);

                    float x = (pageSize.Left + pageSize.Right) / 2;
                    float y = (pageSize.Top + pageSize.Bottom) / 2; // Below content
                    float angle = (float)Math.Atan2(pageSize.Top - pageSize.Bottom, pageSize.Right - pageSize.Left) * (180 / (float)Math.PI);
                    //float angle = 60; // Diagonal angle

                    pdfContent.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, watermarkText, x, y, angle);
                    pdfContent.EndText();

                    pdfContent.RestoreState();
                }

                // Close the stamper
                stamper.Close();
                reader.Close();
                // Get the final PDF with watermark
                pdfBytes = outputPdfStream.ToArray();
            }


            string pdfFilename = $"Agenda/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Agenda.pdf";

            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            //  SavePDf(AgendaId.ToString(), pdfFilename);



            //Aprove Agenda
            //AgendaModelFunction.ApproveAgenda(Convert.ToInt32(AgendaId.ToString()), 1);
            string folderpath = "~/SecureFileStructure/" + pdfFilename;
            string finalpdfpath = Server.MapPath(folderpath);
            if (!System.IO.File.Exists(finalpdfpath))
            {
                var fileStream = new FileStream(finalpdfpath, FileMode.Create, FileAccess.Write);
                fileStream.Write(pdfBytes, 0, pdfBytes.Length);
                fileStream.Close();
            }

            Response.Headers.Add("Content-Disposition", "inline; filename=Agenda");
            return File(pdfBytes, "application/pdf");

            //return File(pdfBytes, "application/pdf", "Agenda.pdf");
        }

        public async Task<ActionResult> ApprovedAgendaHtmlNew(int AgendaId, string doctype)
        {
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.DeptID;


            var officeType = CurrentSession.officeType;


            if (mcid == "1001" && officeType == "1")
                htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 0);
            else if (mcid == "1002" && officeType == "1")
                htmlString = GetNewapprovepdfPatiala(AgendaId, doctype, ref meetingdate, 0);
            else if (mcid != "1002" && mcid != "1001" && officeType == "1")
                htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 0);
            else
                htmlString = GetNewapprovepdfhtmlCouncil(AgendaId, doctype, ref meetingdate, 0);


            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;
            string pdfFilename = "";

            var pdfOptions = new PuppeteerSharp.PdfOptions
            {
                Format = PuppeteerSharp.Media.PaperFormat.A4,
                Landscape = false,
                PrintBackground = true,
                DisplayHeaderFooter = true,
                PreferCSSPageSize = false, // 🔴 CRITICAL FIX
                MarginOptions = new MarginOptions
                {
                    Top = "60px",
                    Bottom = "70px",
                    Left = "25px",
                    Right = "25px"
                },
                HeaderTemplate = @"
            <div style='width:100%; height:40px; background:#0055ff;'></div>",
                FooterTemplate = @"
            <div style='width:100%; height:50px; background:#0055ff;
                        color:#fff; font-size:10px;
                        display:flex; justify-content:space-between;
                        align-items:center; padding:0 15px;'>
                <span>" + MCHeader + @"</span>
                <span>Page <span class='pageNumber'></span> of <span class='totalPages'></span></span>
            </div>"
            };



            //var fullHtml = @"
            //<!DOCTYPE html>
            //<html>
            //  <body>
            //   <div class='avoid-break'>
            //        " + htmlString + @"
            //    </div>
            //</body>
            //</html>";

            /* ---------------- GLOBAL CSS (TD CUTTING FIX) ---------------- */
            string globalCss = @"
<style>
body {
    margin: 0;
    padding: 0;
    font-family: Arial, sans-serif;
}

/* 🔑 FIX: stop text cutting inside TD */
th, td {
    overflow: visible !important;
    word-break: break-word !important;
    overflow-wrap: anywhere !important;
    white-space: normal !important;
}

/* Wrapper safety */
.pdf-wrapper {
    max-width: calc(100% - 10px) !important;
    margin: 0 auto;
}

/* Watermark */
.watermark {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%) rotate(-45deg);
    font-size: 10vw;
    color: rgba(180,180,180,0.12);
    pointer-events: none;
    z-index: 0;
}

.pdf-content {
    position: relative;
    z-index: 1;
}
</style>";

            /* ---------------- FINAL HTML ---------------- */
            string fullHtml = @"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
" + globalCss + @"
</head>
<body>



<div class='pdf-content'>
" + htmlString + @"
</div>

</body>
</html>";




            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                Headless = true,
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
            }))
            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(fullHtml, new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Load }
                });


                var pdfBytes = await page.PdfDataAsync(pdfOptions);
                pdfFilename = $"Agenda/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Agenda.pdf";
                // await page.PdfAsync(filePath, pdfOptions);               ;
                BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
                SavePDf(AgendaId.ToString(), pdfFilename);
                AgendaModelFunction.ApproveAgenda(Convert.ToInt32(AgendaId.ToString()), 1);

            }
            string blobUrl = Convert.ToString(BlobStorage.GetStorageAcessingPath()) + "/" + pdfFilename;
            return Redirect(blobUrl);
        }

        public ActionResult ApprovedAgendaHtmlNew_old(int AgendaId, string doctype)
        {
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;
            if (mcid == "1001") //Mohali
            {
                htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 0);
            }
            else if (mcid == "1002") //Patiala
            {
                htmlString = GetNewapprovepdfPatiala(AgendaId, doctype, ref meetingdate, 0);
            }
            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;
            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"Digital MC House\" " +
                    "--footer-right \"Agenda Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };
            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
            string pdfFilename = $"Agenda/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Agenda.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            SavePDf(AgendaId.ToString(), pdfFilename);
            AgendaModelFunction.ApproveAgenda(Convert.ToInt32(AgendaId.ToString()), 1);
            string blobUrl = Convert.ToString(BlobStorage.GetStorageAcessingPath()) + "/" + pdfFilename;
            return Redirect(blobUrl);
        }

        // for Circulate agenda button 

        public async Task<ActionResult> CirculateAgendaHtmlNew(int AgendaId, string doctype)
        {
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;


            if (mcid == "1001") //Mohali
            {
                htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 1);
            }
            else if (mcid == "1002") //Patiala
            {
                htmlString = GetNewapprovepdfPatiala(AgendaId, doctype, ref meetingdate, 1);
            }
            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;
            DateTime dt = DateTime.Now.Date;
            DateTime meetingdt = DateTime.ParseExact(meetingdate, "dd/MM/yyyy", null);

            // if dt is less than to meeeting date then show current date mean dt
            if (DateTime.Compare(dt, meetingdt) < 0)
            {
                meetingdate = dt.Date.ToString("dd/MM/yyyy");
            }

            //start Puppeteer            
            BrowserFetcher browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();


            var pdfOptions = new PuppeteerSharp.PdfOptions();
            pdfOptions.Format = PuppeteerSharp.Media.PaperFormat.A4;
            pdfOptions.Landscape = true;
            pdfOptions.MarginOptions = new MarginOptions
            {
                Top = "12mm",
                Left = "1cm",
                Right = "1cm",
                Bottom = "10mm",
            };
            pdfOptions.PrintBackground = true;
            pdfOptions.DisplayHeaderFooter = true;
            //pdfOptions.HeaderTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; text-align: center;'>" + MCHeader + "</div>";
            //pdfOptions.FooterTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px;'><div style='float: left; font-size: 13px;width:44%;padding-left:15px'><span style='font-size: 13px'> <span class='pageNumber'></span> of <span class='totalPages'></span></span></div><div style='float:left; font-size: 13px;width:17%'>Digital MC House</div><div style='float: right;text-align:right; font-size: 13px;width:35%;padding-right:15px;'>Agenda Generated on: " + meetingdate + " </div></div>";

            pdfOptions.HeaderTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; text-align: center;'></div>";
            pdfOptions.FooterTemplate = @"
            <div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; padding-right:15px; display: flex; justify-content: space-between; align-items: center;'>
                <div style='flex: 1;'></div>
                <div style='flex: 1; text-align: center;'>" + MCHeader + @"</div>
                <div style='flex: 1; text-align: right;'>
                    Page <span class='pageNumber'></span> of <span class='totalPages'></span>
                </div>
            </div>";

            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                Headless = true
                // ExecutablePath = "CHROME_BROWSER_PATH"
            }))

            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(htmlString);
                page.DefaultNavigationTimeout = 0;
                // var result = await page.GetContentAsync();
                var pdfBytes = await page.PdfDataAsync(pdfOptions);

                //await page.PdfAsync(Server.MapPath("~/uploads/pdffromhtml" + DateTime.Now.Hour + DateTime.Now.Minute + ".pdf"), pdfOptions);

                string pdfFilename = $"Circular/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Agenda.pdf";
                BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
                SavePDfCircular(AgendaId.ToString(), pdfFilename);
                string blobUrl = Convert.ToString(BlobStorage.GetStorageAcessingPath()) + "/" + pdfFilename;
                return Redirect(blobUrl);
            }
        }
        public ActionResult CirculateAgendaHtmlNew_old(int AgendaId, string doctype)
        {
            string meetingdate = "";
            string htmlString = string.Empty;
            var mcid = CurrentSession.StateId;


            if (mcid == "1001") //Mohali
            {
                htmlString = GetNewapprovepdfhtml(AgendaId, doctype, ref meetingdate, 1);
            }
            else if (mcid == "1002") //Patiala
            {
                htmlString = GetNewapprovepdfPatiala(AgendaId, doctype, ref meetingdate, 1);
            }
            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(10, 10, 10, 10),
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"Digital MC House\" " +
                    "--footer-right \"Agenda Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
            string pdfFilename = $"Circular/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Agenda.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            SavePDfCircular(AgendaId.ToString(), pdfFilename);
            string blobUrl = Convert.ToString(BlobStorage.GetStorageAcessingPath()) + "/" + pdfFilename;
            return Redirect(blobUrl);
        }

        public ActionResult ApproveProceedingDeptDecisionHtmlNew(int AgendaId, int DeptId)
        {
            string htmlString = GeteCabinetAgendaDeptReplyhtml(AgendaId, DeptId);
            var MCName = CurrentSession.MCName;
            var MCHeader = " " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"NextGen e-Cabinet\" " +
                    "--footer-right \"Proceedings Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);



            // Add diagonal watermark using iTextSharp
            using (var inputPdfStream = new MemoryStream(pdfBytes))
            using (var outputPdfStream = new MemoryStream())
            {
                var reader = new iTextSharp.text.pdf.PdfReader(inputPdfStream);
                var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

                // Create the watermark
                var watermarkFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 100, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(192, 192, 192));
                var watermarkText = "Preview";

                // Loop through pages and apply watermark
                int pageCount = reader.NumberOfPages;
                for (int i = 1; i <= pageCount; i++)
                {
                    var pdfContent = stamper.GetOverContent(i);
                    iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(i);

                    // Set diagonal rotation
                    var gstate = new iTextSharp.text.pdf.PdfGState { FillOpacity = 0.5f }; // Semi-transparent
                    pdfContent.SaveState();
                    pdfContent.SetGState(gstate);

                    // Calculate position and rotation
                    pdfContent.BeginText();
                    pdfContent.SetColorFill(iTextSharp.text.BaseColor.LIGHT_GRAY);
                    pdfContent.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED), 100);

                    float x = (pageSize.Left + pageSize.Right) / 2;
                    float y = (pageSize.Top + pageSize.Bottom) / 2; // Below content
                    float angle = (float)Math.Atan2(pageSize.Top - pageSize.Bottom, pageSize.Right - pageSize.Left) * (180 / (float)Math.PI);
                    //float angle = 60; // Diagonal angle

                    pdfContent.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, watermarkText, x, y, angle);
                    pdfContent.EndText();

                    pdfContent.RestoreState();
                }

                // Close the stamper
                stamper.Close();
                reader.Close();

                // Get the final PDF with watermark
                pdfBytes = outputPdfStream.ToArray();
            }




            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{DeptId}_Proceeding.pdf";
            Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
            return File(pdfBytes, "application/pdf");
        }
        public ActionResult ApproveProceedingHtmlNew(int AgendaId)
        {
            string htmlString = GetNewpdfhtml(AgendaId);
            var MCName = CurrentSession.MCName;
            var MCHeader = " " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"NextGen e-Cabinet\" " +
                    "--footer-right \"Proceedings Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);



            // Add diagonal watermark using iTextSharp
            using (var inputPdfStream = new MemoryStream(pdfBytes))
            using (var outputPdfStream = new MemoryStream())
            {
                var reader = new iTextSharp.text.pdf.PdfReader(inputPdfStream);
                var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

                // Create the watermark
                var watermarkFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 100, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(192, 192, 192));
                var watermarkText = "Preview";

                // Loop through pages and apply watermark
                int pageCount = reader.NumberOfPages;
                for (int i = 1; i <= pageCount; i++)
                {
                    var pdfContent = stamper.GetOverContent(i);
                    iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(i);

                    // Set diagonal rotation
                    var gstate = new iTextSharp.text.pdf.PdfGState { FillOpacity = 0.5f }; // Semi-transparent
                    pdfContent.SaveState();
                    pdfContent.SetGState(gstate);

                    // Calculate position and rotation
                    pdfContent.BeginText();
                    pdfContent.SetColorFill(iTextSharp.text.BaseColor.LIGHT_GRAY);
                    pdfContent.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED), 100);

                    float x = (pageSize.Left + pageSize.Right) / 2;
                    float y = (pageSize.Top + pageSize.Bottom) / 2; // Below content
                    float angle = (float)Math.Atan2(pageSize.Top - pageSize.Bottom, pageSize.Right - pageSize.Left) * (180 / (float)Math.PI);
                    //float angle = 60; // Diagonal angle

                    pdfContent.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, watermarkText, x, y, angle);
                    pdfContent.EndText();

                    pdfContent.RestoreState();
                }

                // Close the stamper
                stamper.Close();
                reader.Close();

                // Get the final PDF with watermark
                pdfBytes = outputPdfStream.ToArray();
            }




            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";



            Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
            return File(pdfBytes, "application/pdf");



        }

        public ActionResult ApproveProceedingDeptDecision(int AgendaId, int DeptId, int adid)
        {
            string htmlString = GeteCabinetAgendaDeptReplyhtml(AgendaId, DeptId);

            var MCName = CurrentSession.MCName;
            var MCHeader = "NextGen e-Cabinet " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"NextGen e-Cabinet\" " +
                    "--footer-right \"Proceedings Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);

            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            //SavePDf2(AgendaId.ToString(), pdfFilename, "ProceedingRpt");

            DecisionSavePDf(DeptId.ToString(), AgendaId.ToString(), pdfFilename, adid);


            //Aprove Proceeding
            //AgendaModelFunction.ApproveAgenda(Convert.ToInt32(AgendaId.ToString()), 1);
            string folderpath = "~/SecureFileStructure/" + pdfFilename;
            string finalpdfpath = Server.MapPath(folderpath);
            if (!System.IO.File.Exists(finalpdfpath))
            {
                var fileStream = new FileStream(finalpdfpath, FileMode.Create, FileAccess.Write);
                fileStream.Write(pdfBytes, 0, pdfBytes.Length);
                fileStream.Close();
            }
            Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
            return File(pdfBytes, "application/pdf");

            // return File(pdfBytes, "application/pdf", "Proceeding.pdf");

        }

        public ActionResult ApproveProceeding(int AgendaId)
        {
            string htmlString = GetNewpdfhtml(AgendaId);
            var MCName = CurrentSession.MCName;
            var MCHeader = "NextGen e-Cabinet " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"NextGen e-Cabinet\" " +
                    "--footer-right \"Proceedings Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);

            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Proceeding.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            //SavePDf2(AgendaId.ToString(), pdfFilename, "ProceedingRpt");

            SavePDf(AgendaId.ToString(), pdfFilename);


            //Aprove Proceeding
            //AgendaModelFunction.ApproveAgenda(Convert.ToInt32(AgendaId.ToString()), 1);
            string folderpath = "~/SecureFileStructure/" + pdfFilename;
            string finalpdfpath = Server.MapPath(folderpath);
            if (!System.IO.File.Exists(finalpdfpath))
            {
                var fileStream = new FileStream(finalpdfpath, FileMode.Create, FileAccess.Write);
                fileStream.Write(pdfBytes, 0, pdfBytes.Length);
                fileStream.Close();
            }
            Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
            return File(pdfBytes, "application/pdf");

            // return File(pdfBytes, "application/pdf", "Proceeding.pdf");

        }

        public ActionResult ApproveProceedingHtmlOld(int AgendaId)
        {
            string htmlString = GetNewpdfhtml(AgendaId);
            string BlobfolderPath = "Proceedings/";

            string htmlfilename = DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + AgendaId.ToString() + "_Proceeding.html";
            string pdffilename = DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + AgendaId.ToString() + "_Proceeding.pdf";

            string htmlpath = BlobfolderPath + htmlfilename;
            string folderpath = "~/SecureFileStructure/" + htmlpath;
            string finalhtmlpath = Server.MapPath(folderpath);

            string pdfpath = BlobfolderPath + pdffilename;
            folderpath = "~/SecureFileStructure/" + pdfpath;
            string finalpdfpath = Server.MapPath(folderpath);
            var root = "~/SecureFileStructure/Proceedings";
            bool path = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
            if (!path)
            {
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
            }


            System.IO.File.WriteAllText(finalhtmlpath, htmlString);
            Response.ContentType = "text/html";

            byte[] htmlBytes = Encoding.UTF8.GetBytes(htmlString);
            //
            PDFModel.ConvertChromePdfRenderer(finalhtmlpath, finalpdfpath);
            SavePDf2(AgendaId.ToString(), BlobfolderPath + pdffilename, "ProceedingRpt");

            //Response.OutputStream.Write(htmlBytes, 0, htmlBytes.Length);
            //Response.End();
            //return File(htmlString, "text/html", AgendaId.ToString() + "_agenda.html");

            //code to save file in blob storage
            byte[] fileBytes = System.IO.File.ReadAllBytes(finalpdfpath);
            MemoryStream memoryStream = new MemoryStream(fileBytes);
            HttpPostedFileBase postedFile = new PDFModel.MockHttpPostedFileBase(memoryStream, Path.GetFileName(pdfpath));
            BlobStorage.Savefile(postedFile, BlobfolderPath, pdffilename);

            //code to remove file from project folder
            if (System.IO.File.Exists(htmlpath))
            {
                System.IO.File.Delete(htmlpath);
            }
            if (System.IO.File.Exists(pdfpath))
            {
                System.IO.File.Delete(pdfpath);
            }

            var fileStream = new FileStream(finalpdfpath, FileMode.Open, FileAccess.Read);
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            Response.AppendHeader("Content-Disposition", "inline; filename=Proceeding.pdf");
            return fsResult;
        }

        //aman

        public string GetMultiProceedingCoveringLetterpdf(int agendaid, int LetterType)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();
            //mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            string outXml = "";
            string meetingdate = "";
            string meetingtime = "";
            string relativePath = "~/";// "~/SecureFileStructure/";
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            string imagePath = baseUrl + "/" + mdl._LstReferences[0].DeptLogo;

            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);

            if (HeaderlistofAgenda != null)
            {
                meetingdate = HeaderlistofAgenda.AgendaDate;
                meetingtime = HeaderlistofAgenda.StartTime;
            }
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                outXml = outXml + @"<html>
                                    <head><meta charset='UTF-8'><title>Digital MC House: CoveringLetter Report</title>                                  
                                        <style>
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            table{
                                            width:100%;
                                            }
                                        </style>
                                    </head> 
                                    <body><div class='pdf-class' style='max-width:1050px; margin:auto;font-size: 16pt'>";

                outXml = outXml + @"<div style='page-break-after: always;'>";
                outXml = outXml + @"<img src='" + imagePath + "' height='50' width='55' style='float:left' />";
                outXml += @"<p style='text-align: center !important;text-decoration: underline;font-weight:bold;font-size:20pt;padding-top: 16px;'>" + mdl._LstReferences[0].FromDeptLocal + "</p></br>";

                outXml += @"<span style='float:left;'>ਨੰਬਰ:... </span><span style='float:right;'>" + "ਮਿਤੀ: " + DateTime.Now.ToString("dd/MM/yyyy") + "</span></br></br>";
                outXml += @"<span style='float:left;'>ਸੇਵਾ ਵਿਖੇ,</span></br>";

                if (LetterType == 1)//Proceeding Covering Letter to Divisional Commissioner, Rupnagar Mandal
                {
                    outXml += @"<div style='float:left;margin-left:80px;width:100%'><span>ਡਵੀਜਨਲ ਕਮਿਸ਼ਨਰ,</br>ਰੂਪਨਗਰ ਮੰਡਲ,</br>ਰੂਪਨਗਰ।</span></br></br></div>";
                }
                if (LetterType == 2) //Proceeding Covering Letter to Director(LG)
                {
                    outXml += @"<div style='float:left;margin-left:80px;width:100%'><span>ਡਾਇਰੈਕਟਰ,</br>ਸਥਾਨਕ ਸਰਕਾਰ ਵਿਭਾਗ,</br>ਪੰਜਾਬ।</span></br></br></br></div>";
                }
                if (LetterType == 3)//Proceeding Covering Letter to All Members
                {
                    outXml += @"<div style='float:left;margin-left:80px;width:100%'><span>1) ਮੇਅਰ, ਨਗਰ ਨਿਗਮ, " + mdl._LstReferences[0].FromDeptLocal + "।</br>";
                    outXml += @"2) ਸ਼੍ਰੀ ਕੁਲਵੰਤ ਸਿੰਘ, (ਹਲਕਾ ਵਿਧਾਇਕ)," + mdl._LstReferences[0].FromDeptLocal + "।</br>";
                    outXml += @"3) ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ/ ਡਿਪਟੀ ਮੇਅਰ, " + mdl._LstReferences[0].FromDeptLocal + "।</br>";
                    outXml += @"4) ਕਮਿਸ਼ਨਰ, ਨਗਰ ਨਿਗਮ, " + mdl._LstReferences[0].FromDeptLocal + "।</br>";
                    outXml += @"5) ਸੀਨੀਅਰ ਡਿਪਟੀ ਮੇਅਰ/ ਡਿਪਟੀ ਮੇਅਰ, " + mdl._LstReferences[0].FromDeptLocal + "।</span></br></br></br></div>";
                }
                if (LetterType == 4)//Proceeding Covering Letter to Joint Comr, Assistant Comr, All Branches
                {
                    outXml += @"<div style='float:left;margin-left:80px;width:100%'><span>1) ਸੰਯੁਕਤ ਕਮਿਸ਼ਨਰੇਸਹਾਇਕ ਕਮਿਸ਼ਨਰ</br>";
                    outXml += @"2) ਨਿਗਰਾਨ ਇੰਜੀਨੀਅਰ/ਸਮੂਹ ਨਿਗਮ ਇੰਜੀਨੀਅਰ/ਐਮ ਓ ਐਚ/ਐਮ ਟੀ ਪੀ, ਏ ਡੀ ਐਫ ਓ (ਫਾਇਰ ਬ੍ਰਿਗੇਡ), ਸਮੂਹ ਸੁਪਰਡੰਟ, </br>";
                    outXml += mdl._LstReferences[0].FromDeptLocal + "।</span></br></br></div>";
                }
                var firstResolutionNo = "";
                var lastResolutionNo = "";
                // for (int i = 0; i < mdl._LstReferences.Count; i++)
                //{
                firstResolutionNo = mdl._LstReferences[0].SrNo.ToString();
                //}
                outXml += @"<div class='clearfix'></div>";
                if (officetype == "FNCC")
                    outXml += @"</br><div><span style='float:left;width:50px'>ਵਿਸ਼ਾ:- </span>" + "<span style='text-align: justify;'> " + mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਮੀਟਿੰਗ ਮਿਤੀ: <b>" + meetingdate + "</b> ਦੇ ਮਤਾ ਨੰਬਰ <b>";
                else
                    outXml += @"</br><div><span style='float:left;width:50px'>ਵਿਸ਼ਾ:- </span>" + "<span style='text-align: justify;'> " + mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: <b>" + meetingdate + "</b> ਦੇ ਮਤਾ ਨੰਬਰ <b>";

                if (mdl._LstReferences.Count > 1)
                {
                    // foreach (var item2 in mdl._LstReferences)
                    //{
                    lastResolutionNo = mdl._LstReferences[mdl._LstReferences.Count - 1].SrNo.ToString();
                    // }
                    outXml += firstResolutionNo + "</b> ਤੋਂ <b>" + lastResolutionNo + "</b> ਤੱਕ ਦੀ ਕਾਰਵਾਈ ਭੇਜਣ ਬਾਰੇ।</br></br>";
                }
                else
                {
                    outXml += firstResolutionNo + "</b> ਦੀ ਕਾਰਵਾਈ ਭੇਜਣ ਬਾਰੇ।</br></br>";
                }
                if (officetype == "FNCC")
                    outXml += @"<span style='float:left;margin-left:50px;text-align: justify;'>ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਬੇਨਤੀ ਹੈ ਕਿ " + mdl._LstReferences[0].FromDeptLocal + " ਜੀ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਮੇਅਰ ਦੇ ਹਾਊਸ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਮੀਟਿੰਗ ਰਾਂਹੀ ਮਿਤੀ: <b>";
                else
                    outXml += @"<span style='float:left;margin-left:50px;text-align: justify;'>ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਬੇਨਤੀ ਹੈ ਕਿ " + mdl._LstReferences[0].FromDeptLocal + " ਜੀ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਮੇਅਰ ਦੇ ਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਰਾਂਹੀ ਮਿਤੀ: <b>";

                if (mdl._LstReferences.Count > 1)
                {
                    outXml += meetingdate + "</b> ਸਮਾਂ ਬਾਅਦ ਦੁਪਹਿਰ <b>" + meetingtime + " </b> ਵਜੇ ਦੇ ਮਤਾ ਨੰ: <b>" + firstResolutionNo + "</b> ਤੋਂ <b>" + lastResolutionNo + "</b> ਤੱਕ ਦੀ ਕਾਰਵਾਈ ਨੱਥੀ ਕਰਕੇ ਆਪ ਜੀ ਪਾਸ ਅਗਲੇਰੀ ਕਾਰਵਾਈ ਹਿੱਤ ਭੇਜੀ ਜਾਂਦੀ ਹੈ ਜੀ।</span></br></br></br>";
                }
                else
                {
                    outXml += meetingdate + "</b> ਸਮਾਂ ਬਾਅਦ ਦੁਪਹਿਰ <b>" + meetingtime + " </b> ਵਜੇ ਦੇ ਮਤਾ ਨੰ: <b>" + firstResolutionNo + "</b> ਦੀ ਕਾਰਵਾਈ ਨੱਥੀ ਕਰਕੇ ਆਪ ਜੀ ਪਾਸ ਅਗਲੇਰੀ ਕਾਰਵਾਈ ਹਿੱਤ ਭੇਜੀ ਜਾਂਦੀ ਹੈ ਜੀ।</span></br></br></br>";
                }

                outXml += @"</div></br></br></br><span style='float:left'>ਉਕਤ ਅਨੁਸਾਰ PDF ਫਾਈਲ ਨਾਲ ਲੱਗੀ ਹੈ।</span>";
                outXml += @"<span style='float:right;text-align: right;'>ਸਕੱਤਰ,</br>ਨਗਰ ਨਿਗਮ,</br>" + mdl._LstReferences[0].FromDeptLocal + "</span></br>";
                outXml += @"</div></br></br></br>";

                //Resolutions list start from here
                outXml += @"<table style='width:100%;font-size: 14pt; border: 1px solid black;border-collapse: collapse;'>";
                outXml = outXml + @"<tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Sr. No</th>
                  <th style ='padding:10px;border: 1px solid black;border - collapse: collapse;'>Resolution No.</th> 
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Date of Meeting</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Resolution Description </th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;width: 15%;'>Orders of Competent Authority<br> ਪ੍ਰਵਾਨਗੀ ਜਨਰਲ ਹਾਊਸ</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;display:none'>Act</th></tr>";

                if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
                {
                    for (int i = 0; i < mdl._LstReferences.Count; i++)
                    {
                        //if (i == 0)
                        //{
                        outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + Convert.ToString(i + 1) + @"    
										</td>
										<td style='padding: 10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(mdl._LstReferences[i].SrNo) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                        string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);
                        string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                        if (_subject != "")
                        {

                            _subject = "<b>" + _subject + " :- </b>";
                        }


                        outXml += @"" + _subject + " " + LOBText + "<br/>";
                        outXml += @"</td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>
                                       " + Convert.ToString(mdl._LstReferences[i].ActionDescription) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;display:none'>
                                       " + Convert.ToString(mdl._LstReferences[i].Act) + @"
                                        </tr>";
                    }
                }
                outXml += @"</table>";
                outXml += @"</div></body> </html>";
            }
            return outXml;
        }
        public ActionResult GetProceedingCoveringLetter(int AgendaId, int LetterType)
        {
            string htmlString = GetMultiProceedingCoveringLetterpdf(AgendaId, LetterType);

            string htmlpath = "Proceedings/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + AgendaId.ToString() + "_" + LetterType.ToString() + "_Proceeding.html";
            string folderpath = "~/SecureFileStructure/" + htmlpath;
            htmlpath = Server.MapPath(folderpath);

            string pdfpath = "CirculatedProcceding/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + AgendaId.ToString() + "_" + LetterType.ToString() + "_Proceeding.pdf";
            folderpath = "~/SecureFileStructure/" + pdfpath;
            string finalpdfpath = Server.MapPath(folderpath);
            var root = "~/SecureFileStructure/CirculatedProcceding";
            bool path = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
            if (!path)
            {
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
            }
            System.IO.File.WriteAllText(htmlpath, htmlString);
            Response.ContentType = "text/html";
            byte[] htmlBytes = Encoding.UTF8.GetBytes(htmlString);
            PDFModel.ConvertChromePdfRenderer(htmlpath, finalpdfpath);
            SavePDf2(AgendaId.ToString(), pdfpath, LetterType.ToString());

            //code to save file in blob storage
            byte[] fileBytes = System.IO.File.ReadAllBytes(finalpdfpath);
            MemoryStream memoryStream = new MemoryStream(fileBytes);
            HttpPostedFileBase postedFile = new PDFModel.MockHttpPostedFileBase(memoryStream, Path.GetFileName(pdfpath));
            BlobStorage.Savefile(postedFile, "", pdfpath);

            //code to remove file from project folder
            if (System.IO.File.Exists(htmlpath))
            {
                System.IO.File.Delete(htmlpath);
            }
            if (System.IO.File.Exists(pdfpath))
            {
                System.IO.File.Delete(pdfpath);
            }
            ////Write the HTML bytes to the response stream
            //Response.OutputStream.Write(htmlBytes, 0, htmlBytes.Length);
            //Response.End();
            //return File(htmlString, "text/html", AgendaId.ToString() + "_agenda.html");

            var fileStream = new FileStream(finalpdfpath, FileMode.Open, FileAccess.Read);
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            //Response.AppendHeader("Content-Disposition", "inline; filename=ProceedingCoveringLetter.pdf");
            //return fsResult;
            string blobUrl = Convert.ToString(BlobStorage.GetStorageAcessingPath()) + "/" + pdfpath;
            return Redirect(blobUrl);

        }

        //    public string GetProceedingCoveringLetterpdf(int agendaid, string refid, int IsPreview)
        //    {
        //        string outXml = "";

        //        string uniqueNumber = ""; 
        //        if (IsPreview == 1)
        //        {
        //            uniqueNumber = "XX/XXXX/XXX/XXXXX";
        //        }
        //        else
        //        {
        //            uniqueNumber = AgendaModelFunction.GetUniqueNumber(agendaid);
        //        }
        //        AgendaDetailModel mdl = new AgendaDetailModel();
        //        mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
        //        mdl._LstResolutionActions = AgendaModelFunction.GetProcedingDataForCommPDF(refid, "0");

        //        var note = AttendenceList.GetNotes(agendaid);
        //        var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

        //        string meetingdate = HeaderlistofAgenda?.AgendaDate ?? "";
        //        string meetingtime = HeaderlistofAgenda?.StartTime ?? "";
        //        string MeetingTypeName = HeaderlistofAgenda?.MeetingTypeName ?? "";

        //        var OfficeType = CurrentSession.officeType;
        //        string officeTitle = OfficeType == "1"
        //            ? "ਕਮਿਸ਼ਨਰ ਦਫ਼ਤਰ"
        //            : "ਕਾਰਜਕਾਰੀ ਅਧਿਕਾਰੀ ਦਫ਼ਤਰ";

        //        outXml += @"<html style='height: 100%;'>   
        //        <head>
        //        <meta charset='UTF-8'>
        //        <title>Digital MC House: ApprovedAgendaReport</title>

        //        <style>

        //        table.maintable {
        //         width: 100%;
        //         table-layout: fixed; 
        //         border-collapse: collapse;      
        //         border: 1px solid black !important;

        //        }

        //        table.maintable th,
        //        table.maintable td {
        //         border: 1px solid black;
        //         padding: 8px;
        //         vertical-align: top;
        //         word-break: break-word;
        //         overflow-wrap: break-word;
        //        }

        //        tfoot {
        //         display: table-footer-group;

        //        }

        //        .pdf-class, table.maintable, .MsoNormalTable, .MsoTableGrid {
        //         max-width: 100% !important;
        //        }

        //        body {
        //         page-break-inside: avoid;
        //         overflow: visible;
        //         word-wrap: break-word;
        //         margin: 0;
        //         padding: 0;
        //        }

        //        .MsoNormalTable tr,
        //        .MsoNormalTable td,
        //        .MsoNormalTable th,
        //        .MsoTableGrid tr,
        //        .MsoTableGrid td,
        //        .MsoTableGrid th,

        //         #tblfooter tr {
        //         page-break-inside: avoid !important;                                                                          
        //         }

        //        .pdf-class .MsoNormal{
        //         text-wrap: balance;
        //         white-space: pre-wrap;
        //         word-wrap: break-word;
        //        }

        //        .pdf-class .MsoNormalTable td{
        //         max-width: 65px !important;
        //         padding: 1px !important;
        //         overflow-wrap: break-word;
        //        }
        //        table.maintable td table {
        //            width: 100% !important;
        //            max-width: 100% !important;
        //            table-layout: fixed !important;
        //            border-collapse: collapse !important;
        //        }

        //        table.maintable td table td,
        //        table.maintable td table th {
        //            word-break: break-word !important;
        //            overflow-wrap: break-word !important;
        //            white-space: normal !important;
        //        }
        //        .pdf-class .MsoNormalTable {
        //         max-width: 700px !important;
        //         margin-left: 0 !important;
        //        }

        //        .pdf-class table.MsoNormalTable {
        //         max-width: 700px !important;
        //         margin-left: 0 !important;
        //        }

        //        .pdf-class .MsoTableGrid td{
        //         max-width: 65px !important;
        //         white-space: pre-wrap;
        //         word-wrap: break-word;
        //        }

        //        .pdf-class .MsoTableGrid {
        //         max-width: 500px !important;
        //         margin-left: 0 !important;
        //        }

        //        .pdf-class table.MsoTableGrid {
        //         max-width: 500px !important;
        //         margin-left: 0 !important;
        //        }

        //        p.MsoListParagraphCxSpFirst,
        //        p.MsoListParagraphCxSpMiddle,
        //        p.MsoListParagraphCxSpLast {
        //         margin-left: 20px;
        //        }

        //        td {
        //         vertical-align: top;
        //        }

        //        @media print {
        //         .header { display: none; }
        //        }

        //        ol li { padding:5px; }

        //        </style>
        //        </head>
        //        <body>
        //        <div class='pdf-class' style='width:100%; margin:0; min-height:100%;'>";


        //        if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
        //        {
        //                outXml += @"<div>
        //                <h1 style='font-size:20px;font-weight:700;text-align:center;text-transform:uppercase;'>"
        //                         + officeTitle + " " + mdl._LstReferences[0].FromDeptLocal + @"</h1>

        //                <p style='font-size:16px;text-align:center;font-weight:bold'>
        //                Telephone No. " + mdl._LstReferences[0].DeptPhoneNo + @"<br>
        //                Email: " + mdl._LstReferences[0].DeptEmail + @"</p>

        //                <table style='width:100%;font-size:15px;margin:10px 0;border-collapse:collapse;'>
        //                    <tr>
        //                          <td style='text-align:left;font-weight:bold;'>
        //                            No.: " + uniqueNumber + @"<br/>
        //                            ..............................................
        //                        </td>
        //                        <td style='text-align:right;font-weight:bold;'>
        //                            Date: " + DateTime.Now.ToString("dd-MM-yyyy") + @"
        //                        </td>
        //                    </tr>
        //                </table>

        //<p style='font-size:16px;text-align:center;font-weight:bold'>
        //ਵਿਸ਼ਾ:— " + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ " + MeetingTypeName + @" ਮਿਤੀ: "
        //         + meetingdate + @" ਨੂੰ ਸਮਾਂ " + meetingtime + @" ਵਜੇ ਦੀ ਕਾਰਵਾਈ ਸਬੰਧੀ।
        //</p>";

        //            outXml += @"<table class='pdf-table maintable' style='font-size:12pt;'>";

        //            outXml += @"<thead>
        //                            <tr>
        //                            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //                            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //                            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //                            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //                            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //                            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //                            </tr>
        //                            </thead>";


        //            outXml += @"<colgroup>
        //            <col style='width:5%;'>
        //            <col style='width:10%;'>
        //            <col style='width:60%;'>
        //            <col style='width:10%;'>
        //            <col style='width:10%;'>
        //            <col style='width:10%;'>
        //            </colgroup>";

        //                            // HEADER
        //             outXml += @"<tr>
        //            <th style='font-weight:bold;border:1px solid black;'>Sr. No</th>
        //            <th style='font-weight:bold;border:1px solid black;'>Resolution No.<br>& Date</th>
        //            <th style='font-weight:bold;border:1px solid black;'>Resolution Details</th>
        //            <th style='font-weight:bold;border:1px solid black;'>House/Administrator Decision</th>
        //            <th style='font-weight:bold;border:1px solid black;'>Whether the Resolution<br>Cover under Act/Rules/Govt. Instructions</th>
        //            <th style='font-weight:bold;border:1px solid black !important;'>Comments of Comr./ E.O.</th>
        //            </tr>";
        //            int j = 0;
        //            if (mdl._LstResolutionActions != null)
        //            {
        //                for (int i = 0; i < mdl._LstResolutionActions.Count; i++)
        //                {
        //                    if (mdl._LstResolutionActions[i].TableItem == "0")
        //                    {
        //                        if (j == 0)
        //                        {
        //                            outXml += @"<tr>
        //                         <td colspan='6' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
        //                         Table Items</td></tr>";
        //                        }
        //                        j++;
        //                    }


        //                    string subject = mdl._LstResolutionActions[i].Subject ?? "";
        //                    if (!string.IsNullOrEmpty(subject))
        //                        subject = "<b>" + subject + " :- </b>";

        //                    outXml += @"<tr>
        //                    <td style='padding:10px;border:1px solid black;'>" + (i + 1) + @"</td>
        //                    <td style='padding:10px;border:1px solid black;'>"
        //                            + mdl._LstResolutionActions[i].DisplaySrNo + "<br>"
        //                            + mdl._LstResolutionActions[i].AgendaDate + @"</td>
        //                    <td style='padding:10px;border:1px solid black;text-align: justify;'>"
        //                            + subject + mdl._LstResolutionActions[i].SubjectDetails + @"</td>
        //                    <td style='padding:10px;border:1px solid black;text-align: justify;'>"
        //                            + mdl._LstResolutionActions[i].HouseDecision + @"</td>
        //                    <td style='padding:10px;border:1px solid black;text-align: justify;'>"
        //                            + mdl._LstResolutionActions[i].Act + @"</td>
        //                    <td style='padding:10px;border:1px solid black !important;text-align: justify;'>"
        //                            + mdl._LstResolutionActions[i].commissioner + @"</td>
        //                   </tr>";
        //                }
        //            }

        //            // FOOTER BORDER FIX
        //            outXml += @"<tfoot>
        //            <tr>
        //            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //            <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
        //            </tr>
        //            </tfoot>";


        //            outXml += @"</table>";

        //            if (OfficeType == "1")
        //            {
        //                outXml += @"<p style='text-align:right;font-size:18px;padding-top:20px;'>
        //                ਕਮਿਸ਼ਨਰ,<br>" + mdl._LstReferences[0].FromDeptLocal.Trim() + "</p>";
        //            }
        //            else
        //            {
        //                outXml += @"<p style='text-align:right;font-size:18px;padding-top:20px;'>
        //                ਕਾਰਜ ਸਾਧਕ ਅਫ਼ਸਰ,<br>" + mdl._LstReferences[0].FromDeptLocal.Trim() + "</p>";
        //            }
        //        }

        //        outXml += @"</div></body></html>";

        //        return outXml;
        //    }


        public string GetProceedingCoveringLetterpdf(int agendaid, string refid, int IsPreview)
        {
            string outXml = "";

            string uniqueNumber = "";
            if (IsPreview == 1)
            {
                uniqueNumber = "XX/XXXX/XXX/XXXXX";
            }
            else
            {
                uniqueNumber = AgendaModelFunction.GetUniqueNumber(agendaid);
            }
            AgendaDetailModel mdl = new AgendaDetailModel();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            mdl._LstResolutionActions = AgendaModelFunction.GetProcedingDataForCommPDF(refid, "0");

            var note = AttendenceList.GetNotes(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            string meetingdate = HeaderlistofAgenda?.AgendaDate ?? "";
            string meetingtime = HeaderlistofAgenda?.StartTime ?? "";
            string MeetingTypeName = HeaderlistofAgenda?.MeetingTypeName ?? "";

            var OfficeType = CurrentSession.officeType;
            string officeTitle = OfficeType == "1"
                ? "ਕਮਿਸ਼ਨਰ ਦਫ਼ਤਰ"
                : "ਕਾਰਜਕਾਰੀ ਅਧਿਕਾਰੀ ਦਫ਼ਤਰ";

            outXml += @"<html style='height: 100%;'>   
            <head>
            <meta charset='UTF-8'>
            <title>Digital MC House: ApprovedAgendaReport</title>

            <style>

            table.maintable {
             width: 100%;
             table-layout: fixed; 
             border-collapse: collapse;      
             border: 1px solid black !important;

            }

            table.maintable th,
            table.maintable td {
             border: 1px solid black;
             padding: 8px;
             vertical-align: top;
             word-break: break-word;
             overflow-wrap: break-word;
            }

            tfoot {
             display: table-footer-group;

            }

            .pdf-class, table.maintable, .MsoNormalTable, .MsoTableGrid {
             max-width: 100% !important;
            }

            body {
             page-break-inside: avoid;
             overflow: visible;
             word-wrap: break-word;
             margin: 0;
             padding: 0;
            }

            .MsoNormalTable tr,
            .MsoNormalTable td,
            .MsoNormalTable th,
            .MsoTableGrid tr,
            .MsoTableGrid td,
            .MsoTableGrid th,

             #tblfooter tr {
             page-break-inside: avoid !important;                                                                          
             }

            .pdf-class .MsoNormal{
             text-wrap: balance;
             white-space: pre-wrap;
             word-wrap: break-word;
            }

            .pdf-class .MsoNormalTable td{
             max-width: 65px !important;
             padding: 1px !important;
             overflow-wrap: break-word;
            }
            table.maintable td table {
                width: 100% !important;
                max-width: 100% !important;
                table-layout: fixed !important;
                border-collapse: collapse !important;
            }

            table.maintable td table td,
            table.maintable td table th {
                word-break: break-word !important;
                overflow-wrap: break-word !important;
                white-space: normal !important;
            }
            .pdf-class .MsoNormalTable {
             max-width: 700px !important;
             margin-left: 0 !important;
            }

            .pdf-class table.MsoNormalTable {
             max-width: 700px !important;
             margin-left: 0 !important;
            }

            .pdf-class .MsoTableGrid td{
             max-width: 65px !important;
             white-space: pre-wrap;
             word-wrap: break-word;
            }

            .pdf-class .MsoTableGrid {
             max-width: 500px !important;
             margin-left: 0 !important;
            }

            .pdf-class table.MsoTableGrid {
             max-width: 500px !important;
             margin-left: 0 !important;
            }

            p.MsoListParagraphCxSpFirst,
            p.MsoListParagraphCxSpMiddle,
            p.MsoListParagraphCxSpLast {
             margin-left: 20px;
            }

            td {
             vertical-align: top;
            }

            @media print {
             .header { display: none; }
            }

            ol li { padding:5px; }

            </style>
            </head>
            <body>
            <div class='pdf-class' style='width:100%; margin:0; min-height:100%;'>";


            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                outXml += @"<div>
                    <h1 style='font-size:20px;font-weight:700;text-align:center;text-transform:uppercase;'>"
                         + officeTitle + " " + mdl._LstReferences[0].FromDeptLocal + @"</h1>

                    <p style='font-size:16px;text-align:center;font-weight:bold'>
                    Telephone No. " + mdl._LstReferences[0].DeptPhoneNo + @"<br>
                    Email: " + mdl._LstReferences[0].DeptEmail + @"</p>

                    <table style='width:100%;font-size:15px;margin:10px 0;border-collapse:collapse;'>
                        <tr>
                              <td style='text-align:left;font-weight:bold;'>
                                No.: " + uniqueNumber + @"<br/>
                                ..............................................
                            </td>
                            <td style='text-align:right;font-weight:bold;'>
                                Date: " + DateTime.Now.ToString("dd-MM-yyyy") + @"
                            </td>
                        </tr>
                    </table>

    <p style='font-size:16px;text-align:center;font-weight:bold'>
    ਵਿਸ਼ਾ:— " + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ " + MeetingTypeName + @" ਮਿਤੀ: "
         + meetingdate + @" ਨੂੰ ਸਮਾਂ " + meetingtime + @" ਵਜੇ ਦੀ ਕਾਰਵਾਈ ਸਬੰਧੀ।
    </p>";

                outXml += @"<table class='pdf-table maintable' style='font-size:12pt;'>";

                outXml += @"<thead>
                                <tr>
                                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                                
                                </tr>
                                </thead>";


                outXml += @"<colgroup>
                <col style='width:5%;'>
                <col style='width:10%;'>
                <col style='width:70%;'>
                <col style='width:15%;'>
               
             
                </colgroup>";

                // HEADER
                outXml += @"<tr>
                <th style='font-weight:bold;border:1px solid black;'>Sr. No</th>
                <th style='font-weight:bold;border:1px solid black;'>Resolution No.<br>& Date</th>
                <th style='font-weight:bold;border:1px solid black;'>Resolution Details</th>
                <th style='font-weight:bold;border:1px solid black !important;'>Comments of Comr./ E.O.</th>
                </tr>";
                int j = 0;
                if (mdl._LstResolutionActions != null)
                {
                    for (int i = 0; i < mdl._LstResolutionActions.Count; i++)
                    {
                        if (mdl._LstResolutionActions[i].TableItem == "0")
                        {
                            if (j == 0)
                            {
                                outXml += @"<tr>
                             <td colspan='6' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
                             Table Items</td></tr>";
                            }
                            j++;
                        }


                        string subject = mdl._LstResolutionActions[i].Subject ?? "";
                        if (!string.IsNullOrEmpty(subject))
                            subject = "<b>" + subject + "</b>";

                        outXml += @"<tr>
                        <td style='padding:10px;border:1px solid black;'>" + (i + 1) + @"</td>
                        <td style='padding:10px;border:1px solid black;'>"
                                + mdl._LstResolutionActions[i].DisplaySrNo + "<br>"
                                + mdl._LstResolutionActions[i].AgendaDate + @"</td>
                        <td style='padding:10px;border:1px solid black;text-align: justify;'>"

                        + subject + mdl._LstResolutionActions[i].SubjectDetails + "<br><br>"
                                + "<b>House/Administrator Decision :-</b>"
                                 + mdl._LstResolutionActions[i].HouseDecision + "<br><br>"
                                   + "<b>Cover under Act/Rules/Govt. Instructions :-</b>"

                                + mdl._LstResolutionActions[i].Act + @" </td>

                
                        <td style='padding:10px;border:1px solid black !important;text-align: justify;'>"
                                + mdl._LstResolutionActions[i].commissioner + @"</td>
                       </tr>";
                    }
                }

                // FOOTER BORDER FIX
                outXml += @"<tfoot>
                <tr>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                </tr>
                </tfoot>";


                outXml += @"</table>";

                if (OfficeType == "1")
                {
                    outXml += @"<p style='text-align:right;font-size:18px;padding-top:20px;'>
                    ਕਮਿਸ਼ਨਰ,<br>" + mdl._LstReferences[0].FromDeptLocal.Trim() + "</p>";
                }
                else
                {
                    outXml += @"<p style='text-align:right;font-size:18px;padding-top:20px;'>
                    ਕਾਰਜ ਸਾਧਕ ਅਫ਼ਸਰ,<br>" + mdl._LstReferences[0].FromDeptLocal.Trim() + "</p>";
                }
            }

            outXml += @"</div></body></html>";

            return outXml;
        }

        public string GetProceedingCoveringFormatedLetterpdf(int agendaid, string refid, int IsPreview)
        {
            string outXml = "";

            string uniqueNumber = "";
            if (IsPreview == 1)
            {
                uniqueNumber = "XX/XXXX/XXX/XXXXX";
            }
            else
            {
                uniqueNumber = AgendaModelFunction.GetUniqueNumber(agendaid);
            }
            AgendaDetailModel mdl = new AgendaDetailModel();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            mdl._LstResolutionActions = AgendaModelFunction.GetProcedingDataForCommPDF(refid, "0");

            var note = AttendenceList.GetNotes(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            string meetingdate = HeaderlistofAgenda?.AgendaDate ?? "";
            string meetingtime = HeaderlistofAgenda?.StartTime ?? "";
            string MeetingTypeName = HeaderlistofAgenda?.MeetingTypeName ?? "";

            var OfficeType = CurrentSession.officeType;
            string officeTitle = OfficeType == "1"
                ? "ਕਮਿਸ਼ਨਰ ਦਫ਼ਤਰ"
                : "ਕਾਰਜਕਾਰੀ ਅਧਿਕਾਰੀ ਦਫ਼ਤਰ";

            outXml += @"<html style='height: 100%;'>   
            <head>
            <meta charset='UTF-8'>
            <title>Digital MC House: ApprovedAgendaReport</title>

            <style>

            table.maintable {
             width: 100%;
             table-layout: fixed; 
             border-collapse: collapse;      
             border: 1px solid black !important;

            }

            table.maintable th,
            table.maintable td {
             border: 1px solid black;
             padding: 8px;
             vertical-align: top;
             word-break: break-word;
             overflow-wrap: break-word;
            }

            tfoot {
             display: table-footer-group;

            }

            .pdf-class, table.maintable, .MsoNormalTable, .MsoTableGrid {
             max-width: 100% !important;
            }

            body {
             page-break-inside: avoid;
             overflow: visible;
             word-wrap: break-word;
             margin: 0;
             padding: 0;
            }

            .MsoNormalTable tr,
            .MsoNormalTable td,
            .MsoNormalTable th,
            .MsoTableGrid tr,
            .MsoTableGrid td,
            .MsoTableGrid th,

             #tblfooter tr {
             page-break-inside: avoid !important;                                                                          
             }

            .pdf-class .MsoNormal{
             text-wrap: balance;
             white-space: pre-wrap;
             word-wrap: break-word;
            }

            .pdf-class .MsoNormalTable td{
             max-width: 65px !important;
             padding: 1px !important;
             overflow-wrap: break-word;
            }
            table.maintable td table {
                width: 100% !important;
                max-width: 100% !important;
                table-layout: fixed !important;
                border-collapse: collapse !important;
            }

            table.maintable td table td,
            table.maintable td table th {
                word-break: break-word !important;
                overflow-wrap: break-word !important;
                white-space: normal !important;
            }
            .pdf-class .MsoNormalTable {
             max-width: 700px !important;
             margin-left: 0 !important;
            }

            .pdf-class table.MsoNormalTable {
             max-width: 700px !important;
             margin-left: 0 !important;
            }

            .pdf-class .MsoTableGrid td{
             max-width: 65px !important;
             white-space: pre-wrap;
             word-wrap: break-word;
            }

            .pdf-class .MsoTableGrid {
             max-width: 500px !important;
             margin-left: 0 !important;
            }

            .pdf-class table.MsoTableGrid {
             max-width: 500px !important;
             margin-left: 0 !important;
            }

            p.MsoListParagraphCxSpFirst,
            p.MsoListParagraphCxSpMiddle,
            p.MsoListParagraphCxSpLast {
             margin-left: 20px;
            }

            td {
             vertical-align: top;
            }

            @media print {
             .header { display: none; }
            }

            ol li { padding:5px; }

            </style>
            </head>
            <body>
            <div class='pdf-class' style='width:100%; margin:0; min-height:100%;'>";


            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                outXml += @"<div>
                    <h1 style='font-size:20px;font-weight:700;text-align:center;text-transform:uppercase;'>"
                         + officeTitle + " " + mdl._LstReferences[0].FromDeptLocal + @"</h1>

                    <p style='font-size:16px;text-align:center;font-weight:bold'>
                    Telephone No. " + mdl._LstReferences[0].DeptPhoneNo + @"<br>
                    Email: " + mdl._LstReferences[0].DeptEmail + @"</p>

                    <table style='width:100%;font-size:15px;margin:10px 0;border-collapse:collapse;'>
                        <tr>
                              <td style='text-align:left;font-weight:bold;'>
                                No.: " + uniqueNumber + @"<br/>
                                ..............................................
                            </td>
                            <td style='text-align:right;font-weight:bold;'>
                                Date: " + DateTime.Now.ToString("dd-MM-yyyy") + @"
                            </td>
                        </tr>
                    </table>

    <p style='font-size:16px;text-align:center;font-weight:bold'>
    ਵਿਸ਼ਾ:— " + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ " + MeetingTypeName + @" ਮਿਤੀ: "
         + meetingdate + @" ਨੂੰ ਸਮਾਂ " + meetingtime + @" ਵਜੇ ਦੀ ਕਾਰਵਾਈ ਸਬੰਧੀ।
    </p>";

                outXml += @"<table class='pdf-table maintable' style='font-size:12pt;'>";

                outXml += @"<thead>
                                <tr>
                                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                                
                                </tr>
                                </thead>";


                outXml += @"<colgroup>
                <col style='width:5%;'>
                <col style='width:10%;'>
                <col style='width:70%;'>
                <col style='width:15%;'>
               
             
                </colgroup>";

                // HEADER
                outXml += @"<tr>
                <th style='font-weight:bold;border:1px solid black;'>Sr. No</th>
                <th style='font-weight:bold;border:1px solid black;'>Resolution No.<br>& Date</th>
                <th style='font-weight:bold;border:1px solid black;'>Resolution Details</th>
                <th style='font-weight:bold;border:1px solid black !important;'>Comments of Comr./ E.O.</th>
                </tr>";
                int j = 0;
                if (mdl._LstResolutionActions != null)
                {
                    for (int i = 0; i < mdl._LstResolutionActions.Count; i++)
                    {
                        if (mdl._LstResolutionActions[i].TableItem == "0")
                        {
                            if (j == 0)
                            {
                                outXml += @"<tr>
                             <td colspan='6' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
                             Table Items</td></tr>";
                            }
                            j++;
                        }


                        string subject = mdl._LstResolutionActions[i].Subject ?? "";
                        if (!string.IsNullOrEmpty(subject))
                            subject = "<b>" + subject + "</b>";

                        outXml += @"<tr>
                        <td style='padding:10px;border:1px solid black;'>" + (i + 1) + @"</td>
                        <td style='padding:10px;border:1px solid black;'>"
                                + mdl._LstResolutionActions[i].DisplaySrNo + "<br>"
                                + mdl._LstResolutionActions[i].AgendaDate + @"</td>
                        <td style='padding:10px;border:1px solid black;text-align: justify;'>"
                     
                        + subject + mdl._LstResolutionActions[i].SubjectDetails + "<br><br>"
                                + "<b>House/Administrator Decision :-</b>"
                                 + mdl._LstResolutionActions[i].HouseDecision + "<br><br>"
                                   + "<b>Cover under Act/Rules/Govt. Instructions :-</b>"

                                + mdl._LstResolutionActions[i].Act + @" </td>

                
                        <td style='padding:10px;border:1px solid black !important;text-align: justify;'>"
                                + mdl._LstResolutionActions[i].commissioner + @"</td>
                       </tr>";
                    }
                }

                // FOOTER BORDER FIX
                outXml += @"<tfoot>
                <tr>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                <td style='padding:0;height:0;border-top:0.6px solid #bbb;'></td>
                </tr>
                </tfoot>";


                outXml += @"</table>";

                if (OfficeType == "1")
                {
                    outXml += @"<p style='text-align:right;font-size:18px;padding-top:20px;'>
                    ਕਮਿਸ਼ਨਰ,<br>" + mdl._LstReferences[0].FromDeptLocal.Trim() + "</p>";
                }
                else
                {
                    outXml += @"<p style='text-align:right;font-size:18px;padding-top:20px;'>
                    ਕਾਰਜ ਸਾਧਕ ਅਫ਼ਸਰ,<br>" + mdl._LstReferences[0].FromDeptLocal.Trim() + "</p>";
                }
            }

            outXml += @"</div></body></html>";

            return outXml;
        }


        //public string GetProceedingCoveringFormatedLetterpdf(int agendaid, string refid)
        //{

        //    string outXml = "";
        //    AgendaDetailModel mdl = new AgendaDetailModel();
        //    mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
        //    mdl._LstResolutionActions = AgendaModelFunction.GetProcedingDataForCommPDF(refid, "0");
        //    var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
        //    string meetingdate = HeaderlistofAgenda?.AgendaDate ?? "";
        //    string meetingtime = HeaderlistofAgenda?.StartTime ?? "";
        //    string path1 = BlobStorage.GetStorageAcessingPath();
        //    string signature = path1 + "/" + CurrentSession.SignaturePath;
        //    string imagePath = path1 + "/MC/" + CurrentSession.MCName + "/mc_logo.jpg";

        //    outXml = outXml + @"<html style='height: 100%;'>   
        //                            <head><meta charset='UTF-8'><title>Digital MC House: ApprovedAgendaReport</title>
        //                             <style>
        //                                 table.maintable {
        //                                 width: 100%;
        //                                 border-collapse: collapse;      
        //                                 border-bottom: 1px solid black;
        //                                 border-left: 1px solid black;
        //                                 border-right: 1px solid black;
        //                                 }
        //                                 tfoot {
        //                                  display: table-footer-group; /* forces this row to appear at bottom of each page */
        //                                 }

        //                                 table.maintable tr1,table.maintable td1 {
        //                                 page-break-inside: avoid;
        //                                 }

        //                                 body {
        //                                  page-break-inside: avoid;
        //                                  overflow: visible;
        //                                  word-wrap: break-word;
        //                                 }
        //                                .MsoNormalTable tr,.MsoNormalTable td,.MsoNormalTable th,.MsoTableGrid tr,.MsoTableGrid td,.MsoTableGrid th,#tblfooter tr{
        //                                 page-break-inside: avoid !important;                                                                          
        //                                }  
        //                            .pdf-class .MsoNormal{
        //                                text-wrap:balance;
        //                                white-space: pre-wrap;
        //                                word-wrap: break-word;
        //                            }
        //                            .pdf-class .MsoNormalTable td{
        //                                max-width: 65px !important;
        //                                padding: 1px !important;
        //                                overflow-wrap: break-word;
        //                            }

        //                            .pdf-class .MsoNormalTable {
        //                                max-width: 700px !important;
        //                                margin-left: 0 !important;
        //                            }
        //                            .pdf-class table.MsoNormalTable {
        //                                max-width: 700px !important;
        //                                margin-left: 0 !important;
        //                            }

        //                            .pdf-class .MsoTableGrid td{
        //                                max-width: 65px !important;
        //                                text-wrap:balance;
        //                                white-space: pre-wrap;
        //                                word-wrap: break-word;
        //                            }

        //                            .pdf-class .MsoTableGrid {
        //                                max-width: 500px !important;
        //                                margin-left: 0 !important;
        //                            }

        //                            .pdf-class table.MsoTableGrid {
        //                                max-width: 500px !important;
        //                                margin-left: 0 !important;
        //                            }
        //                            p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
        //                                margin-left: 20px;
        //                            }
        //                            td {
        //                                vertical-align: top;
        //                            }
        //                                @media print {
        //                                    .header {
        //                                        display: none;
        //                                    }
        //                                }
        //                                    table{
        //                                    width:100%;
        //                                    }
        //                                  ol li {
        //                                  padding: 5px;
        //                                      }

        //                                </style>
        //                            </head> 
        // <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

        //    if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
        //    {
        //        //Resolutions list start from here
        //        outXml += @"<div><h1 style='font-size: 20px; font-weight: 700; text-align: center;margin: auto;text-transform: uppercase;'>ਕਮਿਸ਼ਨਰ ਦਾ ਦਫ਼ਤਰ "
        //                + mdl._LstReferences[0].FromDeptLocal + "</h1>";
        //        outXml += @"<p style='font-size: 16px; text-align: center;font-weight:bold'>Telephone No. " + mdl._LstReferences[0].DeptPhoneNo + "<br> Email:" + mdl._LstReferences[0].DeptEmail + "</p></div>";


        //        outXml += @"<table class='pdf-table maintable' style='font-size: 12pt;''>";
        //        outXml += @"<tfoot><tr><td colspan='5' style='border-bottom: 1px solid black;'></td></tr></tfoot>";
        //        outXml += @"<thead><tr style='border-bottom: 1px solid black;'><td></td><td></td><td></td></tr></thead>";
        //        outXml += @"<tbody>";
        //        if (mdl._LstResolutionActions != null && mdl._LstResolutionActions.Count > 0)
        //        {
        //            int j = 0;
        //            for (int i = 0; i < mdl._LstResolutionActions.Count; i++)
        //            {
        //                if (mdl._LstResolutionActions[i].SerialNo == null || mdl._LstResolutionActions[i].SerialNo == "")
        //                {
        //                    if (j == 0)
        //                    {
        //                        outXml = outXml + @"<tr style ='padding:5px;'>
        //                        <td style='padding:10px;font-weight:bold; border: 1px solid black;border-collapse: collapse;vertical-align:top'>Sr. No</th>
        //                        <td style='padding:10px;font-weight:bold; border: 1px solid black;border-collapse: collapse;vertical-align:top'>Resolution No. & Date</th> 
        //                        <td style='padding:10px;font-weight:bold;border: 1px solid black;border-collapse: collapse;vertical-align:top'>Resolution Details</th>";

        //                    }
        //                    j = j + 1;
        //                }

        //                outXml += @"<tr>
        //										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;vertical-align:top'>               
        //												" + Convert.ToString(i + 1) + @"<br>    
        //										</td>
        //										<td style='padding: 10px; border: 1px solid black;border - collapse: collapse;vertical-align:top '>
        //                                        " + Convert.ToString(mdl._LstResolutionActions[i].LetterNo) + @"<br>" + Convert.ToString(mdl._LstResolutionActions[i].AgendaDate) + @"
        //                                        </td>


        //                <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;vertical-align:top'>";
        //                string _SubjectDetails = Convert.ToString(mdl._LstResolutionActions[i].SubjectDetails);
        //                string _subject = Convert.ToString(mdl._LstResolutionActions[i].Subject);
        //                if (_subject != "")
        //                {

        //                    _subject = "<b>" + _subject + " :- </b>";
        //                }


        //                outXml += @"" + _subject + " " + _SubjectDetails;
        //                outXml += @"<br><b>House Decision:</b><br>" + Convert.ToString(mdl._LstResolutionActions[i].HouseDecision) + @"<br>";
        //                outXml += @"<b>Please specify under which point of instructions dated 10.10.2022 this resolution is covered:</b><br>" + Convert.ToString(mdl._LstResolutionActions[i].Act) + @"<br>";
        //                outXml += @"<b>Comments of Commissioner/ E.O.:</b><br>" + Convert.ToString(mdl._LstResolutionActions[i].commissioner) + @"</td> </tr>";

        //               //<td style ='padding:10px;margin-bottom: 20px; border: 1px solid black;border-collapse: collapse;vertical-align:top'>
        //               //                        " + Convert.ToString(mdl._LstResolutionActions[i].Act) + @"                                             
        //               //                         </td><td style ='padding:10px;margin-bottom: 20px; border: 1px solid black;border - collapse: collapse;vertical-align:top'>
        //               //                        " + Convert.ToString(mdl._LstResolutionActions[i].commissioner) + @"
        //               //                         </tr>";
        //            }
        //        }
        //        outXml += @"</tbody>";
        //        outXml += @"</table>";
        //        outXml += @"<br/><br/><div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";

        //        outXml += @"<p style ='text-align: right; font-size: 18px;'> 
        //                                 ਕਮਿਸ਼ਨਰ,<br>" + mdl._LstReferences[0].FromOfficeLocal.Replace("House", "").Trim()
        //        + "</p>";
        //        outXml += @"</div></body> </html>";
        //    }
        //    return outXml;
        //}

        //public async Task<ActionResult> GetPreviewCommisioner(int AgendaId, string refid)
        //{
        //    string htmlString = "";
        //    string value = ConfigurationManager.AppSettings["FormatType"];
        //    if (value == "1")
        //    {
        //       htmlString = GetProceedingCoveringLetterpdf(AgendaId, refid);
        //    }
        //    else
        //    {
        //        htmlString = GetProceedingCoveringFormatedLetterpdf(AgendaId, refid);
        //    }
        //        BrowserFetcher browserFetcher = new BrowserFetcher();
        //    await browserFetcher.DownloadAsync();


        //    var pdfOptions = new PuppeteerSharp.PdfOptions();
        //    pdfOptions.Format = PuppeteerSharp.Media.PaperFormat.A4;
        //    pdfOptions.Landscape = true;
        //    pdfOptions.MarginOptions = new MarginOptions
        //    {
        //        Top = "12mm",
        //        Left = "1cm",
        //        Right = "1cm",
        //        Bottom = "10mm",
        //    };
        //    pdfOptions.PrintBackground = true;
        //    pdfOptions.DisplayHeaderFooter = true;

        //    pdfOptions.HeaderTemplate = "<div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; text-align: center;'></div>";
        //    pdfOptions.FooterTemplate = @"
        //    <div style='width: 100%; background-color: #0055ff; color: white; font-size: 13px; padding-right:15px; display: flex; justify-content: space-between; align-items: center;'>
        //        <div style='flex: 1;'></div>
        //        <div style='flex: 1; text-align: center;'></div>
        //        <div style='flex: 1; text-align: right;'>
        //            Page <span class='pageNumber'></span> of <span class='totalPages'></span>
        //        </div>
        //    </div>";
        //    string watermarkCss = @"
        //    <style>
        //      .watermark {
        //        position: fixed;
        //        top: 50%;
        //        left: 50%;
        //        transform: translate(-50%, -50%) rotate(-45deg);
        //        font-size: 100px;
        //        color: rgba(180, 180, 180, 0.2);
        //        z-index: 9999;
        //        pointer-events: none;
        //        font-family: Arial, sans-serif;
        //        white-space: nowrap;
        //        text-align: center;
        //      }
        //      </style>

        //    <div class='watermark'>PREVIEW</div>";

        //    using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        //    {
        //        ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
        //        Headless = true

        //    }))

        //    using (var page = await browser.NewPageAsync())
        //    {
        //        await page.SetContentAsync(watermarkCss + htmlString);
        //        page.DefaultNavigationTimeout = 0;
        //        var pdfBytes = await page.PdfDataAsync(pdfOptions);
        //        Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
        //        return File(pdfBytes, "application/pdf");
        //    }


        //}


        public async Task<ActionResult> GetPreviewCommisioner(int AgendaId, string refid)
        {
            string htmlString;
            string formatType = ConfigurationManager.AppSettings["FormatType"];
            var MCHeader = "Digital MC House " + CurrentSession.MCName;


            if (formatType == "1")
                htmlString = GetProceedingCoveringLetterpdf(AgendaId, refid, 1);
            else
                htmlString = GetProceedingCoveringFormatedLetterpdf(AgendaId, refid, 1);

            var pdfOptions = new PuppeteerSharp.PdfOptions
            {
                Format = PuppeteerSharp.Media.PaperFormat.A4,
                Landscape = true,
                PrintBackground = true,
                DisplayHeaderFooter = true,
                Scale = 1m,
                MarginOptions = new MarginOptions
                {
                    Top = "15mm",
                    Bottom = "15mm",
                    Left = "10mm",
                    Right = "10mm"
                },
                HeaderTemplate = "<div></div>",

                FooterTemplate = @"<div style='width:100%; font-size:11px; text-align:left; padding-left:15px;'>
                <span>" + MCHeader + @"</span>
               </div><div style='width:100%; font-size:11px; text-align:right; padding-right:15px;'>
                Page <span class='pageNumber'></span> of <span class='totalPages'></span>
                </div>"
            };

            string watermarkCss = @"
           .watermark {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%) rotate(-45deg);
            font-size: 100px;
            color: rgba(180, 180, 180, 0.2);
            font-family: Arial, sans-serif;
            z-index: 9999;
            pointer-events: none;
            white-space: nowrap;
            text-align: center;
        }";

            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                Headless = true,
                Args = new[] { "--no-sandbox" }
            }))
            using (var page = await browser.NewPageAsync())
            {
                // 🔥 CRITICAL FIX — SET VIEWPORT
                await page.SetViewportAsync(new ViewPortOptions
                {
                    Width = 1600,   // Very important
                    Height = 1200
                });

                await page.SetContentAsync(htmlString, new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Load }
                });

                await page.AddStyleTagAsync(new AddTagOptions { Content = watermarkCss });

                await page.EvaluateExpressionAsync(
                    "document.body.insertAdjacentHTML('beforeend', '<div class=\"watermark\">PREVIEW</div>');"
                );

                var pdfBytes = await page.PdfDataAsync(pdfOptions);

                Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding.pdf");
                return File(pdfBytes, "application/pdf");
            }
        }





        //[HttpPost]
        //public async Task<ActionResult> GetProceedingCoveringLetterCommisioner(int AgendaId, string refid)
        //{

        //    string filePath = "";
        //    string htmlString = "";
        //    string value = ConfigurationManager.AppSettings["FormatType"];
        //    if (value == "1")
        //    {
        //        htmlString = GetProceedingCoveringLetterpdf(AgendaId, refid);
        //    }
        //    else
        //    {
        //        htmlString = GetProceedingCoveringFormatedLetterpdf(AgendaId, refid);
        //    }
        //    try {
        //        BrowserFetcher browserFetcher = new BrowserFetcher();
        //        await browserFetcher.DownloadAsync();


        //         var pdfOptions = new PuppeteerSharp.PdfOptions
        //    {
        //        Format = PuppeteerSharp.Media.PaperFormat.A4,
        //        Landscape = true,
        //        PrintBackground = true,
        //        DisplayHeaderFooter = true,
        //        MarginOptions = new MarginOptions
        //        {
        //            Top = "15mm",
        //            Bottom = "15mm",
        //            Left = "10mm",
        //            Right = "10mm"
        //        },
        //        HeaderTemplate = "<div style='width:100%; font-size:10px; text-align:center;'></div>",
        //        FooterTemplate = @"
        //        <div style='width: 100%; font-size: 11px; padding-right:15px; display: flex; justify-content: space-between;'>
        //            <div></div>
        //            <div></div>
        //            <div>Page <span class='pageNumber'></span> of <span class='totalPages'></span></div>
        //        </div>"
        //                    };

        //        using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        //        {
        //            ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
        //            Headless = true

        //        }))

        //        using (var page = await browser.NewPageAsync())
        //        {
        //            await page.SetContentAsync(htmlString);
        //            page.DefaultNavigationTimeout = 0;
        //            var pdfBytes = await page.PdfDataAsync(pdfOptions);
        //            filePath = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_ComissionerCommentProceeding.pdf";
        //            BlobStorage.UploadPdfToBlob(pdfBytes, filePath);
        //            SavePDf2(AgendaId.ToString(), filePath, "commisisionerRpt");
        //            if (AgendaId != 0)
        //            {
        //                AgendaModelFunction.ApproveProceedings(Convert.ToInt32(AgendaId), 1);
        //            }

        //        }

        //        string blobUrl = Convert.ToString(BlobStorage.GetStorageAcessingPath()) + "/" + filePath;
        //        return Json(new { success = true, pdfPath = blobUrl });
        //    }

        //    catch (Exception ex)
        //    {
        //            return Json(new { success = false, message = ex.Message
        //     });
        //    }
        //}
        //joginder
        //Agenda Preview,Approve,circular html

        //public async Task<ActionResult> GetProceedingCoveringLetterCommisioner(int AgendaId, string refid)
        //{
        //    string filePath = "";
        //    string htmlString = "";

        //    string value = ConfigurationManager.AppSettings["FormatType"];
        //    htmlString = value == "1"
        //        ? GetProceedingCoveringLetterpdf(AgendaId, refid)
        //        : GetProceedingCoveringFormatedLetterpdf(AgendaId, refid);

        //    try
        //    {
        //        BrowserFetcher browserFetcher = new BrowserFetcher();
        //        await browserFetcher.DownloadAsync();

        //        var pdfOptions = new PuppeteerSharp.PdfOptions
        //        {
        //            Format = PuppeteerSharp.Media.PaperFormat.A4,
        //            Landscape = true,
        //            PrintBackground = true,
        //            DisplayHeaderFooter = true,
        //            MarginOptions = new MarginOptions
        //            {
        //                Top = "15mm",
        //                Bottom = "15mm",
        //                Left = "10mm",
        //                Right = "10mm"
        //            },
        //            HeaderTemplate = "<div></div>",
        //            FooterTemplate = @"
        //        <div style='width:100%; font-size:11px; padding-right:15px; text-align:right;'>
        //            Page <span class='pageNumber'></span> of <span class='totalPages'></span>
        //        </div>"
        //        };

        //        using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        //        {
        //            ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
        //            Headless = true
        //        }))
        //        using (var page = await browser.NewPageAsync())
        //        {
        //            await page.SetContentAsync(htmlString);
        //            page.DefaultNavigationTimeout = 0;

        //            var pdfBytes = await page.PdfDataAsync(pdfOptions);

        //            filePath = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_ComissionerCommentProceeding.pdf";

        //            BlobStorage.UploadPdfToBlob(pdfBytes, filePath);
        //            SavePDf2(AgendaId.ToString(), filePath, "commisisionerRpt");

        //            if (AgendaId != 0)
        //            {
        //                AgendaModelFunction.ApproveProceedings(AgendaId, 1);
        //            }
        //        }

        //        string blobUrl = $"{BlobStorage.GetStorageAcessingPath().TrimEnd('/')}/{filePath}";

        //        // ✅ CORRECT for Azure Blob
        //        return Redirect(blobUrl);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("PDF generation failed: " + ex.Message);
        //    }
        //}

        public async Task<ActionResult> GetProceedingCoveringLetterCommisioner(int AgendaId, string refid)
        {
            string filePath = "";
            string htmlString = "";

            try
            {
                // 1️⃣ Generate HTML
                string value = ConfigurationManager.AppSettings["FormatType"];
                htmlString = value == "1"
                    ? GetProceedingCoveringLetterpdf(AgendaId, refid, 0)
                    : GetProceedingCoveringFormatedLetterpdf(AgendaId, refid,0);

                // 2️⃣ SHORT filename (path-length safe)
                filePath = $"Proceedings/P_{AgendaId}_{DateTime.Now:HHmmss}.pdf";

                // 3️⃣ DEFINE Chrome path HERE (no web.config)
                string chromePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe";

                // 4️⃣ Validate Chrome executable
                if (!System.IO.File.Exists(chromePath))
                {
                    return Content("Chrome executable not found at: " + chromePath);
                }

                // 5️⃣ Short temp directory (VERY IMPORTANT)
                string chromeTempPath = @"C:\TempChrome";
                if (!Directory.Exists(chromeTempPath))
                {
                    Directory.CreateDirectory(chromeTempPath);
                }

                // 6️⃣ PDF options
                var pdfOptions = new PuppeteerSharp.PdfOptions
                {
                    Format = PuppeteerSharp.Media.PaperFormat.A4,
                    Landscape = true,
                    PrintBackground = true,
                    DisplayHeaderFooter = true,
                    MarginOptions = new MarginOptions
                    {
                        Top = "15mm",
                        Bottom = "15mm",
                        Left = "10mm",
                        Right = "10mm"
                    },
                    HeaderTemplate = "<div></div>",
                    FooterTemplate = @"
                <div style='width:100%; font-size:11px; padding-right:15px; text-align:right;'>
                    Page <span class='pageNumber'></span> of <span class='totalPages'></span>
                </div>"
                };

                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    ExecutablePath = chromePath,
                    Headless = true,
                    UserDataDir = chromeTempPath,
                    Args = new[]
                    {
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage"
            }
                }))
                using (var page = await browser.NewPageAsync())
                {
                    page.DefaultNavigationTimeout = 120000;

                    await page.SetContentAsync(htmlString, new NavigationOptions
                    {
                        WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                    });

                    var pdfBytes = await page.PdfDataAsync(pdfOptions);

                    // 7️⃣ Upload directly to blob
                    BlobStorage.UploadPdfToBlob(pdfBytes, filePath);
                    SavePDf2(AgendaId.ToString(), filePath, "commisisionerRpt");

                    if (AgendaId > 0)
                    {
                        AgendaModelFunction.ApproveProceedings(AgendaId, 1);
                    }
                }

                // 8️⃣ Redirect to Blob
                string blobUrl =
                    $"{BlobStorage.GetStorageAcessingPath().TrimEnd('/')}/{filePath}";

                return Redirect(blobUrl);
            }
            catch (Exception ex)
            {
                return Content("PDF generation failed: " + ex.Message);
            }
        }


        public string GetNewapprovepdfhtml(int agendaid, string doctype, ref string meetingDate, int IsCirculate = 0)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();

            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);
            int ofid = 0;
            if (IsCirculate == 1)
            {
                //if (CurrentSession.OfficeId == "47")
                //{
                //    ofid = 2;
                //}
                //else if (CurrentSession.OfficeId == "10")
                //{
                //    ofid = 1;
                //}
                if (officetype != "FNCC")
                {
                    ofid = 1;
                }
                else
                {
                    ofid = 2;
                }
            }
            Authoritylist cl = new Authoritylist();
            cl.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);
            string maintitle = string.Empty;

            var path1 = BlobStorage.GetStorageAcessingPath();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);

            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
           

            string value = ConfigurationManager.AppSettings["StorageType"];

            string signature;
            string Header;string Chairperson; string ChairpersonDesignation;


            if (value == "Blob")
            {
                signature = path1 + "/" + CurrentSession.SignaturePath;
            }
            else
            {
                signature = "https://enigambackuprestore.blob.core.windows.net/securefilestructure" + "/" + CurrentSession.SignaturePath;

            }

            meetingDate = HeaderlistofAgenda.AgendaDate.ToString();
            Header =  HeaderlistofAgenda.Header.ToString();
            Chairperson = HeaderlistofAgenda.Chairperson.ToString();
            ChairpersonDesignation = HeaderlistofAgenda.ChairpersonDesignation.ToString();



            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: ApprovedAgendaReport</title>
                                     <style>
                                         table.maintable {
                                           width: 100%;
                                         border-collapse: collapse;      
                                         border-bottom: 1px solid black;
                                         border-left: 1px solid black;
                                         border-right: 1px solid black;
                                         }


                                        tfoot {
                                          display: table-footer-group; /* forces this row to appear at bottom of each page */
                                        }

                                        table.maintable tr1,table.maintable td1 {
                                         page-break-inside: avoid;
                                        }

                                        body {
                                          page-break-inside: avoid;
                                          overflow: visible;
                                          word-wrap: break-word;
                                        }
                                    .MsoNormalTable tr,.MsoNormalTable td,.MsoNormalTable th,.MsoTableGrid tr,.MsoTableGrid td,.MsoTableGrid th,#tblfooter tr{
                                  page-break-inside: avoid !important;                                                                          
                                    }  
                                    .pdf-class .MsoNormal{
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            table{
                                            width:100%;
                                            }
                                          ol li {
                                          padding: 5px;
                                              }

                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");
            if (ofid == 2) //FNCC
            {
                outXml += @"<div style='text-align: center;font-size: 16pt;font-weight: bold;border-bottom:3px solid #000;width: 100%;'>ਦਫਤਰ " + mdl._LstReferences[0].FromDeptLocal + @"</div>    
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style='font-size: 12pt;float: left;width: 50%;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style='font-size: 12pt;float: right;width: 30%;text-align: right;'> <b>ਮਿਤੀ  ..... " + gt + @" .</b></p> 
                            </div>
                            <br> <br> <br> <br> 
                            <p style='display: inline; font-size: 12pt;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p>

                   <div style='margin-bottom:10px;'><ol style='margin: 20px 65px; font-size:12pt;'>";
                for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                {
                    outXml += @"<li>" + cl.AuthorityId_list[j].name + "," + cl.AuthorityId_list[j].Address + "</li>";
                }
                outXml += @"</ol>";
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> " + mdl._LstReferences[0].FromDeptLocal + @" ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                             
                                </span></p>

                                <p style='text-align: justify; font-size: 12pt;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ਉਕਤ ਵਿਸ਼ੇ ਸਬੰਦੀ ਬੇਨਤੀ ਹੈ ਕੀ " + mdl._LstReferences[0].FromDeptLocal + @"  ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ : " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"
                                  ਨੂ ਸਮਾਂ " + Convert.ToString(HeaderlistofAgenda.StartTime) + @" ਵਜੇ ਮਿਉਂਸਪਲ  ਭਵਨ , " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ " + cl.AuthorityId_list[0].name + @" ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਵਿਖੇ ਹੋਵੇਗੀ |
                        <br>
                        <br>
                        ਨਥੀ : ਏਜੇਂਡੇ ਦੀ ਕਾਪੀ |   
                                </p>
                                <p style='margin: auto; font-size: 12pt;text-align:right;page-break-after: always;'> 
                       ਸਕੱਤਰ ,<br>
                                              
                                              " + mdl._LstReferences[0].FromDeptLocal + @"           
                                </p>";
            }
            else if (ofid != 0)
            {
                outXml += @"<div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align: left;float:left;margin: 0px;'> <b>Website : " + mdl._LstReferences[0].DeptWebsite + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align: right;float:right;margin: 0px;'> <b>" + mdl._LstReferences[0].MayorContactNo + @" (Mayor)</b></p> 
                             </div>
                            <div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align:left;float:left;margin: 0px;'> <b>E-Mail : " + mdl._LstReferences[0].DeptEmail + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align:right;float:right;margin: 0px;'> <b>" + mdl._LstReferences[0].CommContactNo + @" (Commissioner)</b></p>
                            </div>
                            <div style='text-align: center;font-size: 30px;font-weight: bold;'>ਨਗਰ ਨਿਗਮ</div>    
                            <div style='text-align: center;'> 
                                <h3>" + mdl._LstReferences[0].DeptAddress_Local + @"</h3>
                            </div>
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style=' font-size: 16px;float: left;width: 50%;margin: 0px;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style=' font-size: 16px;float: right;width: 30%;text-align: right;margin: 0px;'> <b>ਮਿਤੀ  ..... " + gt + @" .</b></p> 
                            </div>
                            <br> 
            <div style='display:flex;'>
                            <p style='width: 100%;margin: 0px;float: left; font-size: 16px;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p></div>
            <div class='clearfix'></div>
                   <div class='text-align:center;margin-bottom:10px;'>";
                for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                {
                    outXml += @"<div style='margin: 15px 65px; font-size:12pt;'>" + (j + 1).ToString() + ") " + cl.AuthorityId_list[j].name + "," + cl.AuthorityId_list[j].Address + "</div>";
                }
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> " + mdl._LstReferences[0].FromDeptLocal + @" ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                                      
                                </span> 
                            </p>

                            <p style='text-align: justify; font-size: 16px;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ਉਕਤ ਵਿਸ਼ੇ ਸਬੰਦੀ ਬੇਨਤੀ ਹੈ ਕੀ , " + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ  ਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ : " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"
                              ਨੂ ਸਮਾਂ " + Convert.ToString(HeaderlistofAgenda.StartTime) + @" ਵਜੇ ਮਿਉਂਸਪਲ  ਭਵਨ ," + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ " + cl.AuthorityId_list[0].name + @" ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਵਿਖੇ ਹੋਵੇਗੀ |                           
                    <br>
                    <br>
                    ਨਥੀ : ਏਜੇਂਡੇ ਦੀ ਕਾਪੀ | <br>  <br>  

                            </p>        

                            <p style='margin: auto; font-size: 16px;text-align:right;page-break-after: always;'> 
                   ਸਕੱਤਰ ,<br>
                                          " + mdl._LstReferences[0].FromDeptLocal + @"             
                            </p></div>";
            }
            if (ofid == 0)
            {
                if (HeaderlistofAgenda != null && cl.AuthorityId_list.Count > 0 && mdl._LstReferences.Count > 0)
                {
                    outXml += @"<div style='margin: 35px 10px;page-break-before: always;'>";
                    //<h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    //+ HeaderlistofAgenda.AgendaName + "</h1>";
                    //+ HeaderlistofAgenda.Header +
                    outXml += @"<p style='font-size: 12pt; text-align: justify'>";
                    if (officetype != "FNCC")
                        outXml += Header + @"</p></div>";
                    else
                        outXml += Header + @"</p></div>";
                    //outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                }
                else
                {
                    outXml += @"<div style='margin: 35px 10px;page-break-before: always;'>";
                    //<h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    //+ HeaderlistofAgenda.AgendaName + "</h1>";
                    //+ HeaderlistofAgenda.Header +
                    outXml += @"<p style='font-size: 12pt; text-align: justify'>
                 
                 ਮਿਉਂਸਪਲ ਕਾਰਪੋਰੇਸ਼ਨ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: 24/03/2023 ਨੂੰ ਸਮਾਂ ਸਵੇਰੇ 11:30 ਵਜੇ ਸ੍ਰੀ ਅਮਰਜੀਤ ਸਿੰਘ ਸਿੱਧੂ, ਮੇਅਰ, ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਦਫ਼ਤਰ ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                }
            }
            outXml += @"<table  class='pdf-table maintable' style='font-size: 12pt;'>";
            outXml += @"<thead><tr> <td colspan='2'></td> </tr></thead>";
            outXml += @"<tfoot> <tr><td colspan='2' style='border-bottom: 1px solid black;'></td> </tr></tfoot>";
            outXml += @"<tbody><tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse; width:10%;'>Sr. No</th>";
            //<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;  width:15%;'>Date of Meeting</th>
            outXml += @" <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'> " + doctype + " Details </th></tr>";

            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<tr>
										  <td style = 'padding:10px; border:1px solid black; border-collapse:collapse;' > "
                                    + mdl._LstReferences[i].SrNo
                                    + (!string.IsNullOrWhiteSpace(mdl._LstReferences[i].yearno)
                                       && mdl._LstReferences[i].yearno != "0"
                                        ? "." + mdl._LstReferences[i].yearno
                                        : "")
                                    + "</td>";

                    outXml += @"  <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }

                    if (!string.IsNullOrEmpty(LOBText) && LOBText.Contains("<table"))
                    {
                        // Force all <table> to expand
                        LOBText = Regex.Replace(LOBText, "<table[^>]*>",
                            "<table style='width:100%; border-collapse:collapse; border:1px solid black;'>",
                            RegexOptions.IgnoreCase);

                        // Remove inline width attributes from <td>, <th>, <tr>
                        LOBText = Regex.Replace(LOBText, @"<(td|th|tr)([^>]*)\swidth\s*=\s*['""][^'""]*['""]", "<$1$2", RegexOptions.IgnoreCase);

                        // Ensure all <td> allow wrapping
                        LOBText = Regex.Replace(LOBText, "<td([^>]*)>", "<td$1 style='word-wrap:break-word; padding:5px;'>", RegexOptions.IgnoreCase);
                    }

                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</table>";

                        outXml += @"<div style='text-align:right;padding-top:5px'>";

                        if (CurrentSession.SignaturePath != "")
                        {
                            outXml += @"<img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                        }
                        //outXml += @"</br></br><p style ='text-align: right; font-size: 12pt;'>" + CurrentSession.DesignationLocal + ",<br>" + mdl._LstReferences[0].FromOfficeLocal.Replace("House", "").Trim() + "</p>";
                        
                        //outXml += @"</br></br><p style ='text-align: right; font-size: 12pt;'>" + mdl._LstReferences[0].FromDeptLocal + "</p>";
                        //       if (!string.IsNullOrWhiteSpace(Chairperson))
                        //       {
                        //           outXml += @"<p style='text-align: right; font-size: 18px;'>"
                        //                        + Chairperson + ", " + ChairpersonDesignation + @"<br>"
                        //                        + mdl._LstReferences[0].FromDeptLocal + "</p>";
                        //       }
                        //       else
                        //       {
                        //           outXml += @"<p style='text-align: right; font-size: 18px;'> 
                        //ਮੇਅਰ,<br>"
                        //                        + mdl._LstReferences[0].FromDeptLocal
                        //                        + "</p>";
                        //       }

                        outXml += @"<p style='text-align: right; font-size: 18px;'>"
                                        + CurrentSession.DesignationLocal+ @"<br>"
                                        + mdl._LstReferences[0].FromDeptLocal + "</p>";

                    }
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }

        public string GetNewapprovepdfhtmlCouncil(int agendaid, string doctype, ref string meetingDate, int IsCirculate = 0)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();

            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);
            int ofid = 0;
            if (IsCirculate == 1)
            {
          
                if (officetype != "FNCC")
                {
                    ofid = 1;
                }
                else
                {
                    ofid = 2;
                }
            }
            Authoritylist cl = new Authoritylist();
            cl.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);
            string maintitle = string.Empty;

            var path1 = BlobStorage.GetStorageAcessingPath();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);

            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            var Header = HeaderlistofAgenda.Header.ToString();
            var Chairperson = HeaderlistofAgenda.Chairperson.ToString();
            var ChairpersonDesignation = HeaderlistofAgenda.ChairpersonDesignation.ToString();

            string value = ConfigurationManager.AppSettings["StorageType"];

            string signature;

            if (value == "Blob")
            {
                signature = path1 + "/" + CurrentSession.SignaturePath;
            }
            else
            {
                signature = "https://enigambackuprestore.blob.core.windows.net/securefilestructure" + "/" + CurrentSession.SignaturePath;

            }

            DateTime agendaDate;
            string agendaDateStr = Convert.ToString(HeaderlistofAgenda.AgendaDate);

            if (!DateTime.TryParseExact(
                    agendaDateStr,
                    new[] { "dd-MM-yyyy", "dd/MM/yyyy", "yyyy-MM-dd" }, // all possible formats
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out agendaDate))
            {
                // fallback: today's date OR handle as needed
                agendaDate = DateTime.Today;
            }
           // meetingDate = HeaderlistofAgenda.AgendaDate.ToString();
            meetingDate = agendaDate.ToString("dd-MM-yyyy");


            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: ApprovedAgendaReport</title>
                                     <style>
                                         table.maintable {
                                           width: 100%;
                                         border-collapse: collapse;      
                                         border-bottom: 1px solid black;
                                         border-left: 1px solid black;
                                         border-right: 1px solid black;
                                         }


                                        tfoot {
                                          display: table-footer-group; /* forces this row to appear at bottom of each page */
                                        }

                                        table.maintable tr1,table.maintable td1 {
                                         page-break-inside: avoid;
                                        }

                                        body {
                                          page-break-inside: avoid;
                                          overflow: visible;
                                          word-wrap: break-word;
                                        }
                                    .MsoNormalTable tr,.MsoNormalTable td,.MsoNormalTable th,.MsoTableGrid tr,.MsoTableGrid td,.MsoTableGrid th,#tblfooter tr{
                                  page-break-inside: avoid !important;                                                                          
                                    }  
                                    .pdf-class .MsoNormal{
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            table{
                                            width:100%;
                                            }
                                          ol li {
                                          padding: 5px;
                                              }

                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";
            //DateTime agendaDate = Convert.ToDateTime(HeaderlistofAgenda.AgendaDate);
            //string dayEnglish = agendaDate.DayOfWeek.ToString();
            string dayPunjabi = "";

            switch (agendaDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dayPunjabi = "ਸੋਮਵਾਰ";
                    break;
                case DayOfWeek.Tuesday:
                    dayPunjabi = "ਮੰਗਲਵਾਰ";
                    break;
                case DayOfWeek.Wednesday:
                    dayPunjabi = "ਬੁੱਧਵਾਰ";
                    break;
                case DayOfWeek.Thursday:
                    dayPunjabi = "ਵੀਰਵਾਰ";
                    break;
                case DayOfWeek.Friday:
                    dayPunjabi = "ਸ਼ੁੱਕਰਵਾਰ";
                    break;
                case DayOfWeek.Saturday:
                    dayPunjabi = "ਸ਼ਨੀਵਾਰ";
                    break;
                case DayOfWeek.Sunday:
                    dayPunjabi = "ਐਤਵਾਰ";
                    break;
            }


            string gt = DateTime.Now.ToString("dd/MM/yyyy");
            if (ofid == 2) //FNCC
            {
                outXml += @"<div style='text-align: center;font-size: 16pt;font-weight: bold;border-bottom:3px solid #000;width: 100%;'>ਦਫਤਰ " + mdl._LstReferences[0].FromDeptLocal + @"</div>    
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style='font-size: 12pt;float: left;width: 50%;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style='font-size: 12pt;float: right;width: 30%;text-align: right;'> <b>ਮਿਤੀ  ..... " + gt + @" .</b></p> 
                            </div>
                            <br> <br> <br> <br> 
                            <p style='display: inline; font-size: 12pt;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p>

                   <div style='margin-bottom:10px;'><ol style='margin: 20px 65px; font-size:12pt;'>";
                for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                {
                    outXml += @"<li>" + cl.AuthorityId_list[j].name + "," + cl.AuthorityId_list[j].Address + "</li>";
                }
                outXml += @"</ol>";
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> " + mdl._LstReferences[0].FromDeptLocal + @" ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                             
                                </span></p>

                                <p style='text-align: justify; font-size: 12pt;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ਉਕਤ ਵਿਸ਼ੇ ਸਬੰਦੀ ਬੇਨਤੀ ਹੈ ਕੀ " + mdl._LstReferences[0].FromDeptLocal + @"  ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ : " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + "ਦਿਨ  : " + dayPunjabi + @"
                                  ਨੂ ਸਮਾਂ " + Convert.ToString(HeaderlistofAgenda.StartTime) + @" ਵਜੇ ਮਿਉਂਸਪਲ  ਭਵਨ , " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ " + cl.AuthorityId_list[0].name + @" ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਵਿਖੇ ਹੋਵੇਗੀ |
                        <br>
                        <br>
                        ਨਥੀ : ਏਜੇਂਡੇ ਦੀ ਕਾਪੀ |   
                                </p>
                                <p style='margin: auto; font-size: 12pt;text-align:right;page-break-after: always;'> 
                       ਸਕੱਤਰ ,<br>
                                              
                                              " + mdl._LstReferences[0].FromDeptLocal + @"           
                                </p>";
            }
            else if (ofid != 0)
            {
                outXml += @"<div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align: left;float:left;margin: 0px;'> <b>Website : " + mdl._LstReferences[0].DeptWebsite + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align: right;float:right;margin: 0px;'> <b>" + mdl._LstReferences[0].MayorContactNo + @" (Mayor)</b></p> 
                             </div>
                            <div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align:left;float:left;margin: 0px;'> <b>E-Mail : " + mdl._LstReferences[0].DeptEmail + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align:right;float:right;margin: 0px;'> <b>" + mdl._LstReferences[0].CommContactNo + @" (Commissioner)</b></p>
                            </div>
                            <div style='text-align: center;font-size: 30px;font-weight: bold;'>ਨਗਰ ਕੌਂਸਲ</div>    
                            <div style='text-align: center;'> 
                                <h3>" + mdl._LstReferences[0].DeptAddress_Local + @"</h3>
                            </div>
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style=' font-size: 16px;float: left;width: 50%;margin: 0px;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style=' font-size: 16px;float: right;width: 30%;text-align: right;margin: 0px;'> <b>ਮਿਤੀ  ..... " + gt + @" .</b></p> 
                            </div>
                            <br> 
            <div style='display:flex;'>
                            <p style='width: 100%;margin: 0px;float: left; font-size: 16px;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p></div>
            <div class='clearfix'></div>
                   <div class='text-align:center;margin-bottom:10px;'>";
                for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                {
                    outXml += @"<div style='margin: 15px 65px; font-size:12pt;'>" + (j + 1).ToString() + ") " + cl.AuthorityId_list[j].name + "," + cl.AuthorityId_list[j].Address + "</div>";
                }
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> " + mdl._LstReferences[0].FromDeptLocal + @" ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                                      
                                </span> 
                            </p>

                            <p style='text-align: justify; font-size: 16px;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ਉਕਤ ਵਿਸ਼ੇ ਸਬੰਦੀ ਬੇਨਤੀ ਹੈ ਕੀ , " + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ  ਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ : " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"
                              ਨੂ ਸਮਾਂ " + Convert.ToString(HeaderlistofAgenda.StartTime) + @" ਵਜੇ ਮਿਉਂਸਪਲ  ਭਵਨ ," + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ " + cl.AuthorityId_list[0].name + @" ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਵਿਖੇ ਹੋਵੇਗੀ |                           
                    <br>
                    <br>
                    ਨਥੀ : ਏਜੇਂਡੇ ਦੀ ਕਾਪੀ | <br>  <br>  

                            </p>        

                            <p style='margin: auto; font-size: 16px;text-align:right;page-break-after: always;'> 
                   ਸਕੱਤਰ ,<br>
                                          " + mdl._LstReferences[0].FromDeptLocal + @"             
                            </p></div>";
            }
            if (ofid == 0)
            {
                if (HeaderlistofAgenda != null && cl.AuthorityId_list.Count > 0 && mdl._LstReferences.Count > 0)
                {
                    outXml += @"<div style='margin: 35px 10px;page-break-before: always;'>";
                    //<h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    //+ HeaderlistofAgenda.AgendaName + "</h1>";
                    //+ HeaderlistofAgenda.Header +
                    outXml += @"<p style='font-size: 12pt; text-align: justify'>";
                    if (officetype != "FNCC")
                        //outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + @" ਦਿਨ " + dayPunjabi + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                        outXml += Header + @"</p></div>";


                    else
                        outXml += mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + @" ਦਿਨ  " + dayPunjabi + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ  " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                }
                else
                {
                    outXml += @"<div style='margin: 35px 10px;page-break-before: always;'>";
                    //<h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    //+ HeaderlistofAgenda.AgendaName + "</h1>";
                    //+ HeaderlistofAgenda.Header +
                    outXml += @"<p style='font-size: 12pt; text-align: justify'>"  + HeaderlistofAgenda.AgendaDate.ToString() + @"ਦਿਨ  : " + dayPunjabi + @"ਨੂੰ ਸਮਾਂ ਸਵੇਰੇ " + HeaderlistofAgenda.StartTime + "ਵਜੇ ਸ੍ਰੀ ਅਮਰਜੀਤ ਸਿੰਘ ਸਿੱਧੂ, ਮੇਅਰ, ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਦਫ਼ਤਰ ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                }
            }
            outXml += @"<table  class='pdf-table maintable' style='font-size: 12pt;'>";
            outXml += @"<thead><tr> <td colspan='2'></td> </tr></thead>";
            outXml += @"<tfoot> <tr><td colspan='2' style='border-bottom: 1px solid black;'></td> </tr></tfoot>";
            outXml += @"<tbody><tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse; width:10%;'>ਲ਼ੜੀ ਨੰ.</th>";
            //<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;  width:15%;'>Date of Meeting</th>
            //outXml += @" <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'> " + doctype + " ਆਈਟਮ ਦਾ ਨਾਂ </th></tr>";
            outXml += @" <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'> ਆਈਟਮ ਦਾ ਨਾਂ </th></tr>";

            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<tr>
										

                                     <td style = 'padding:10px; border:1px solid black; border-collapse:collapse;' > " 
                                    + mdl._LstReferences[i].SrNo
                                    + (!string.IsNullOrWhiteSpace(mdl._LstReferences[i].yearno)
                                       && mdl._LstReferences[i].yearno != "0"
                                        ? "." + mdl._LstReferences[i].yearno
                                        : "")
                                    + "</td>";
                                  

                    outXml += @"  <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }

                    if (!string.IsNullOrEmpty(LOBText) && LOBText.Contains("<table"))
                    {
                        // Force all <table> to expand
                        LOBText = Regex.Replace(LOBText, "<table[^>]*>",
                            "<table style='width:100%; border-collapse:collapse; border:1px solid black;'>",
                            RegexOptions.IgnoreCase);

                        // Remove inline width attributes from <td>, <th>, <tr>
                        LOBText = Regex.Replace(LOBText, @"<(td|th|tr)([^>]*)\swidth\s*=\s*['""][^'""]*['""]", "<$1$2", RegexOptions.IgnoreCase);

                        // Ensure all <td> allow wrapping
                        LOBText = Regex.Replace(LOBText, "<td([^>]*)>", "<td$1 style='word-wrap:break-word; padding:5px;'>", RegexOptions.IgnoreCase);
                    }

                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</table>";

                        outXml += @"<div style='text-align:right;padding-top:5px'>";

                        if (CurrentSession.SignaturePath != "")
                        {
                            outXml += @"<img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                        }

                        if (!string.IsNullOrWhiteSpace(Chairperson))
                        {
                        outXml += @"</br></br><p style='text-align: right; font-size: 12pt;'>" + ChairpersonDesignation + "<br>"
                        + mdl._LstReferences[0].FromDeptLocal
                         + "</p>";
                        }
                        else
                        {
                         outXml += @"</br></br><p style='text-align: right; font-size: 12pt;'>ਪ੍ਰਧਾਨ,<br>"
                         + mdl._LstReferences[0].FromDeptLocal
                         + "</p>";
                        }
                    }
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }

        public string GetNewapprovepdfPatiala(int agendaid, string doctype, ref string meetingDate, int IsCirculate = 0)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();

            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);
            int ofid = 0;
            if (IsCirculate == 1)
            {
                if (officetype == "FNCC")
                {
                    ofid = 2;
                }
                else
                {
                    ofid = 1;
                }
            }
            Authoritylist cl = new Authoritylist();
            cl.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);
            string maintitle = string.Empty;

            var path1 = BlobStorage.GetStorageAcessingPath();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);

            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            var Header = HeaderlistofAgenda.Header.ToString();

            string value = ConfigurationManager.AppSettings["StorageType"];

            string signature;
            string logo = "";

            var dptobj = AgendaModelFunction.GetDeptData(deptid);
            var deptname = CurrentSession.DeptName;
            meetingDate = HeaderlistofAgenda.AgendaDate.ToString();

            if (dptobj != null)
            {
                if (dptobj.Deptname.Contains("Local"))
                {
                    logo = path1 + "/LG/" + CurrentSession.MCName + "/" + dptobj.logo;
                }
                else
                {
                    logo = path1 + "/MC/" + dptobj.Name + "/" + dptobj.logo;
                }
            }

            if (value == "Blob")
            {
                signature = path1 + "/" + CurrentSession.SignaturePath;
            }
            else
            {
                signature = "https://enigambackuprestore.blob.core.windows.net/securefilestructure" + "/" + CurrentSession.SignaturePath;

            }

            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: ApprovedAgendaReport</title>
                                     <style>
                                         table.maintable {
                                           width: 100%;
                                         border-collapse: collapse;      
                                         border-bottom: 1px solid black;
                                         border-left: 1px solid black;
                                         border-right: 1px solid black;
                                         }
                                          tfoot {
                                              display: table-footer-group; /* forces this row to appear at bottom of each page */
                                            }

                                            table.maintable tr1,table.maintable td1 {
                                             page-break-inside: avoid;
                                            }

                                        body {
                                          page-break-inside: avoid;
                                          overflow: visible;
                                          word-wrap: break-word;
                                        }
                                    .MsoNormalTable tr,.MsoNormalTable td,.MsoNormalTable th,.MsoTableGrid tr,.MsoTableGrid td,.MsoTableGrid th,#tblfooter tr{
                                  page-break-inside: avoid !important;                                                                          
                                    }  
                                    .pdf-class .MsoNormal{
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:balance;
                                        white-space: pre-wrap;
                                        word-wrap: break-word;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            table{
                                            width:100%;
                                            }
                                          ol li {
                                          padding: 5px;
                                              }

                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");
            //covering letter start
            if (ofid == 2) //FNCC
            {
                outXml += @"<div style='font-size: 14pt;font-weight: bold;width: 100%;'>
                <div style='float:left;width: 100px;'>
                <img src='" + logo + @"' height='50' width='55' /></div>
                <div style='text-align: center;'><span style='border-bottom: 1px solid;'>ਦਫਤਰ  " + mdl._LstReferences[0].FromDeptLocal + @" </span><br>
                     <span style='border-top: 1px solid;'>OFFICE OF " + mdl._LstReferences[0].FromDept.ToUpper() + @"<span></div></div>
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style='font-size: 12pt;float: left;width: 50%;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style='font-size: 12pt;float: right;width: 30%;text-align: right;'> <b>ਮਿਤੀ:.................</b></p> 
                            </div>
                            <br> 
                            <p style='display: inline; font-size: 12pt;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p>

                   <div style='margin-bottom:10px;'><ol style='margin: 19px 65px; font-size:12pt;'>";
                for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                {
                    outXml += @"<li>" + cl.AuthorityId_list[j].name + ", </li>";
                }
                outXml += "<span>&nbsp; &nbsp;</span>" + mdl._LstReferences[0].FromDeptLocal + "।";
                outXml += @"</ol>";
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + @" ਨੂੰ ਰੱਖੀ ਗਈ ਐਫ ਐਂਡ ਸੀ.ਸੀ.ਦੀ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                             
                                </span></p>

                                <p style='text-align: justify; font-size: 12pt;line-height:1.4;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
                                 ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਬੇਨਤੀ ਹੈ ਕਿ ਮਾਨਯੋਗ ਮੇਅਰ ਸਾਹਿਬ ਦੇ ਜੁਬਾਨੀ ਹੁਕਮਾਂ ਦੀ ਪਾਲਣਾ ਹਿੱਤ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate.ToString() + @" ਨੂੰ ਸਮਾਂ "
                                 + HeaderlistofAgenda.StartTime + @" ਵਜੇ ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਦੇ ਦਫਤਰ ਵਿਖੇ ਐਫ ਐਂਡ ਸੀ.ਸੀ. ਦੀ ਮੀਟਿੰਗ ਰੱਖੀ ਗਈ ਹੈ ਜਿਸ ਦਾ ਏਜੰਡਾ ਨਾਲ ਨੱਥੀ ਕਰਕੇ ਆਪ ਜੀ ਨੂੰ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ। 
                                   ਕਿਰਪਾ ਕਰਕੇ ਮੀਟਿੰਗ ਵਿੱਚ ਸਮੇਂ ਸਿਰ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਖੇਚਲ ਕਰਨਾ ਜੀ।
                        <br>
                                               
                                </p>";
                outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                outXml += @"<p style='margin: auto; font-size: 12pt;text-align:right;'> 
                       ਸਕੱਤਰ ,<br> 
                                  ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।
                                </p>";
                outXml += @"<p style='text-align: justify; font-size: 13pt; display: flex;' ><b style='margin-right: 20px;'> ਉਤਾਰਾ:-</b></p>";
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;margin-right: 30px'> ਮਾਨਯੋਗ ਮੇਅਰ, ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ ਜੀ ਨੂੰ ਸੂਚਨਾ ਹਿੱਤ।
                         <br>
                        </p></p>";
                outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                outXml += @"<p style='margin: auto; font-size: 12pt;text-align:right;page-break-after: always;'>";
                outXml += @" ਸਕੱਤਰ ,<br> 
                                  ਨਗਰ ਨਿਗਮ, ਪਟਿਆਲਾ।
                                </p>";
            }
            else if (ofid != 0)
            {
                outXml += @" < div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align: left;float:left;margin: 0px;'> <b>Website : " + mdl._LstReferences[0].DeptWebsite + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align: right;float:right;margin: 0px;'> <b>" + mdl._LstReferences[0].MayorContactNo + @" (Mayor)</b></p> 
                             </div>
                            <div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align:left;float:left;margin: 0px;'> <b>E-Mail : " + mdl._LstReferences[0].DeptEmail + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align:right;float:right;margin: 0px;'> <b>" + mdl._LstReferences[0].CommContactNo + @" (Commissioner)</b></p>
                            </div>
                            <div style='text-align: center;font-size: 30px;font-weight: bold;'>ਨਗਰ ਨਿਗਮ</div>    
                            <div style='text-align: center;'> 
                                <h3>" + mdl._LstReferences[0].DeptAddress_Local + @"</h3>
                            </div>
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style=' font-size: 16px;float: left;width: 50%;margin: 0px;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style=' font-size: 16px;float: right;width: 30%;text-align: right;margin: 0px;'> <b>ਮਿਤੀ  ..... " + gt + @" .</b></p> 
                            </div>
                            <br> 
            <div style='display:flex;'>
                            <p style='width: 100%;margin: 0px;float: left; font-size: 16px;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p></div>
            <div class='clearfix'></div>
                   <div class='text-align:center;margin-bottom:10px;'>";
                for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                {
                    outXml += @"<div style='margin: 15px 65px; font-size:12pt;'>" + (j + 1).ToString() + ") " + cl.AuthorityId_list[j].name + "," + cl.AuthorityId_list[j].Address + "</div>";
                }
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> " + mdl._LstReferences[0].FromDeptLocal + @" ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                                      
                                </span> 
                            </p>

                            <p style='text-align: justify; font-size: 16px;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ਉਕਤ ਵਿਸ਼ੇ ਸਬੰਦੀ ਬੇਨਤੀ ਹੈ ਕੀ , " + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ  ਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ : " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"
                              ਨੂ ਸਮਾਂ " + Convert.ToString(HeaderlistofAgenda.StartTime) + @" ਵਜੇ ਮਿਉਂਸਪਲ  ਭਵਨ ," + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ " + cl.AuthorityId_list[0].name + @" ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਵਿਖੇ ਹੋਵੇਗੀ |                           
                    <br>
                    <br>
                    ਨਥੀ : ਏਜੇਂਡੇ ਦੀ ਕਾਪੀ | <br>  <br>  

                            </p>        

                            <p style='margin: auto; font-size: 16px;text-align:right;page-break-after: always;'> 
                   ਸਕੱਤਰ ,<br>
                                          " + mdl._LstReferences[0].FromDeptLocal + @"             
                            </p></div>";
            }
            //covering letter end

            if (ofid == 0)//none circular with
            {
                if (HeaderlistofAgenda != null && cl.AuthorityId_list.Count > 0 && mdl._LstReferences.Count > 0)
                {
                    outXml += @"<div style='margin: 35px 10px;page-break-before: always;'>";
                    //<h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    //+ HeaderlistofAgenda.AgendaName + "</h1>";
                    //+ HeaderlistofAgenda.Header +
                    outXml += @"<p style='font-size: 12pt; text-align: justify'>";
                    if (officetype == "FNCC")
                        outXml += mdl._LstReferences[0].FromDeptLocal.ToString() + " ਦੀ ਫਾਇਨਾਂਸ ਅਤੇ ਕੰਟਰੈਕਟ ਕਮੇਟੀ ਦੀ ਮੀਟਿੰਗ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate.ToString() + " ਨੂੰ ਸਮਾਂ " + HeaderlistofAgenda.StartTime + " ਵਜੇ " + cl.AuthorityId_list[0].name + " ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                    else
                        outXml += Header + @"</p></div>";

                }
                else
                {
                    outXml += @"<div style='margin: 35px 10px;page-break-before: always;'>";
                    //<h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    //+ HeaderlistofAgenda.AgendaName + "</h1>";
                    //+ HeaderlistofAgenda.Header +
                    outXml += @"<p style='font-size: 12pt; text-align: justify'>
                 
                 ਮਿਉਂਸਪਲ ਕਾਰਪੋਰੇਸ਼ਨ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: 24/03/2023 ਨੂੰ ਸਮਾਂ ਸਵੇਰੇ 11:30 ਵਜੇ ਸ੍ਰੀ ਅਮਰਜੀਤ ਸਿੰਘ ਸਿੱਧੂ, ਮੇਅਰ, ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਦਫ਼ਤਰ ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ ਨਗਰ ਵਿਖੇ ਹੋਣੀ ਨਿਸ਼ਚਿਤ ਕੀਤੀ ਗਈ ਹੈ। ਇਸ ਦਾ ਏਜੰਡਾ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਹੈ:—   </p></div>";
                }
            }

            outXml += @"<table  class='pdf-table maintable' style='font-size: 12pt;'>";
            outXml += @"<thead><tr> <td colspan='2'></td> </tr></thead>";
            outXml += @"<tfoot> <tr><td colspan='2' style='border-bottom: 1px solid black;'></td> </tr></tfoot>";
            outXml += @"<tbody><tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse; width:10%;'>Sr. No</th>";
            //<th style ='padding:10px; border: 1px solid black;border - collapse: collapse;  width:15%;'>Date of Meeting</th>
            outXml += @" <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'> " + doctype + " Details </th></tr>";

            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<tr>
										<td style = 'padding:10px; border:1px solid black; border-collapse:collapse;' > "
                                        + mdl._LstReferences[i].SrNo
                                        + (!string.IsNullOrWhiteSpace(mdl._LstReferences[i].yearno)
                                           && mdl._LstReferences[i].yearno != "0"
                                            ? "." + mdl._LstReferences[i].yearno
                                            : "")
                                        + "</td>";
                    //<td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                    //" + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                    //</td>
                    outXml += @"  <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }
                    if (!string.IsNullOrEmpty(LOBText) && LOBText.Contains("<table"))
                    {
                        // Force all <table> to expand
                        LOBText = Regex.Replace(LOBText, "<table[^>]*>",
                            "<table style='width:100%; border-collapse:collapse; border:1px solid black;'>",
                            RegexOptions.IgnoreCase);

                        // Remove inline width attributes from <td>, <th>, <tr>
                        LOBText = Regex.Replace(LOBText, @"<(td|th|tr)([^>]*)\swidth\s*=\s*['""][^'""]*['""]", "<$1$2", RegexOptions.IgnoreCase);

                        // Ensure all <td> allow wrapping
                        LOBText = Regex.Replace(LOBText, "<td([^>]*)>", "<td$1 style='word-wrap:break-word; padding:5px;'>", RegexOptions.IgnoreCase);
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</tbody></table>";

                        //outXml += @"<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";

                        // // if (HeaderlistofAgenda != null)
                        // {
                        //outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                        //             " + HeaderlistofAgenda.Footer + "</p>";
                        outXml += @"<p style='text-align: right; font-size: 18px;'>" + CurrentSession.DesignationLocal + @"<br>" + mdl._LstReferences[0].FromDeptLocal + "</p>";
                        // }

                    }
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }



     
        public string GeteCabinetAgendafhtml(int agendaid, string doctype)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();

            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            int ofid = 0;
            if (CurrentSession.OfficeId == "47")
            {
                ofid = 2;
            }
            else if (CurrentSession.OfficeId == "10")
            {
                ofid = 1;
            }
            Authoritylist cl = new Authoritylist();
            cl.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);
            string maintitle = string.Empty;
            var path1 = BlobStorage.GetStorageAcessingPath();

            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            string signature = path1 + "/Signature/" + CurrentSession.SignaturePath;

            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: ApprovedAgendaReport</title>                                   
                                        <style>
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            table{
                                            width:100%;
                                            }
                                       #bloc1
                       {
                        float:left;
						width: 160px;
                       }
					   #bloc2
					   {
					   text-align: center;
					   }
                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");
            if (HeaderlistofAgenda != null)
            {
                outXml += @"<div style='margin: 10px 20px;page-break-before: always;'>
                  <div id='bloc1'> 
                 ਕੈਬਨਿਟ ਮੀਟਿੰਗ<br>        	                 	    	                                                                
                 ਮੇਨ ਅਜੰਡਾ<br>	         		 	 			                                                 
                 ਗੁਪਤ<br>            
                 </div>  
                 <div id='bloc2'>
	             <span>ਪੰਜਾਬ ਸਰਕਾਰ </span><br/>
	             <span>ਆਮ ਰਾਜ ਪ੍ਰਬੰਧ ਵਿਭਾਗ </span><br/>
	             <span>(ਮੰਤਰੀ ਮੰਡਲ ਮਾਮਲੇ ਸ਼ਾਖਾ)</span><br/>
	             </div>";
                outXml += @"<p style='font-size: 12pt; text-indent: 42px;'>"
                 + HeaderlistofAgenda.Header +
" </p>";
                outXml += @"<p style = 'font-size: 12pt; text-indent: 42px;' > ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate + @" ਦਿਨ  " + HeaderlistofAgenda.Day + @"  ਨੂੰ ਸਵੇਰੇ " + HeaderlistofAgenda.StartTime + @" ਵਜੇ ਸਥਾਨ " + HeaderlistofAgenda.Venue + @" ਵਿਖੇ ਹੋਣ ਵਾਲੀ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖੀਆਂ ਆਈਟਮਾਂ ਤੇ ਵਿਚਾਰ ਕੀਤਾ ਜਾਵੇਗਾ: 
    </p>";


            }
            /*   "ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਮਿਤੀ ... ਦਿਨ …ਵਾਰ ਨੂੰ ਸਵੇਰੇ ..:00 ਵਜੇ, ਸਥਾਨ......... ਵਿਖੇ ਹੋਣ ਵਾਲੀ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖੀਆਂ ਆਈਟਮਾਂ ਤੇ ਵਿਚਾਰ ਕੀਤਾ ਜਾਵੇਗਾ:-*/
            outXml += @"<table style='font-size: 12pt; border: 1px solid black;border-collapse: collapse;'>";
            outXml += @"<tr style = 'padding:5px;'>
             <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse; width:5%;'> ਮੀਟਿੰਗ ਦਾ ਆਈਟਮ ਨੰ.</th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;  width:10%;'> ਸਾਲਾਨਾ ਆਈਟਮ ਨੰ.</th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;'> ਵਿਸ਼ਾ </th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;width:15%;'> ਸਬੰਧਤ ਮੰਤਰੀ / ਸਕੱਤਰ </th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;width:15%;' > ਵਿਭਾਗ ਜਿਨ੍ਹਾਂ ਦੀ ਸਲਾਹ ਲਈ ਗਈ </th>";
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + mdl._LstReferences[i].SrNo + @"    
										</td>
	                                     <td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + mdl._LstReferences[i].yearno + @"    
										</td>
                                       
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>                                       
                     <td style = 'padding:10px; border: 1px solid black;border - collapse: collapse;'>   
                        " + mdl._LstReferences[i].MinisterName + @"  
                                        </td>
                        
                     <td style = 'padding:10px; border: 1px solid black;border - collapse: collapse;'>                                      
                                        </td></tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {
                        outXml += "</table>";
                        outXml += "<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += "ਕੇ ਏ ਪੀ  ਸਿਨਹਾ";
                        outXml += "<br/>";
                        outXml += "ਮੁੱਖ ਸਕੱਤਰ, ਪੰਜਾਬ ਸਰਕਾਰ";
                        outXml += "</div>";

                        //if (HeaderlistofAgenda != null)
                        //{
                        //    outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                        //                 " + HeaderlistofAgenda.Footer + "</p>";
                        //}
                        outXml += "<div class='maindiv'>";
                        outXml += "<div style='float:left;width: 70%;'> ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>";
                        outXml += "<div style ='float:right;width: 30%;text-align: end;'> ਮਿਤੀ:" + gt.ToString() + @"</div>";
                        outXml += "</div>";

                        outXml += "<p style='font-size: 12pt; text-indent: 42px;'>";
                        outXml += @"ਉਤਾਰਾ ਹੇਠ ਲਿਖਿਆਂ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਸ਼ਾਮਲ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ:-</p>
	  <p style = 'font-size: 12pt; text-indent: 50px;'>
       <ol>
       <li>
       <span style='padding-left: 50px;'>ਮੁੱਖ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	   </li>
	   <li>
	   <span style = 'padding-left: 50px;' > ਸਮੂਹ ਕੈਬਨਿਟ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	   </li>
	   <li>
	   <span style = 'padding-left: 50px;' > ਵਿਸ਼ੇਸ਼ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ/ਮੁੱਖ ਮੰਤਰੀ</span>
	   </li></ol>
	   </p>
	   <div style='text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt= 'Signature' style= 'height:50px;'><br/>";
                        outXml += @" ਸੁਪਰਡੰਟ
            </div>
            <div class='maindiv'>
			<div style ='float:left;width: 70%;'> ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>
			<div style ='float:right;width: 30%;text-align: end;'> ਮਿਤੀ:" + gt.ToString() + @"</div>			
			</div>			
 <p style = 'font-size: 12pt; text-indent: 42px;'>
 ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ। 
      </p>	  
	   <div style = 'text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += @" ਸੁਪਰਡੰਟ
            </div>
			<div class='maindiv'>
			<div style = 'float:left;width: 70%;' > ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>
			<div style = 'float:right;width: 30%;text-align: end;' > ਮਿਤੀ:" + gt.ToString() + @"</div>			
			</div>			
 <p style='font-size: 12pt; text-indent: 42px;'>
     ਉਤਾਰਾ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਨਿੱਜੀ ਸਕੱਤਰ/ਮੁੱਖ ਸਕੱਤਰ ਨੂੰ ਮੁੱਖ ਸਕੱਤਰ ਪੰਜਾਬ ਜੀ ਦੀ ਸੂਚਨਾ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।	
      </p>
	   <div style ='text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += @" ਸੁਪਰਡੰਟ</div>
			<div class='maindiv'>
			<div style = 'float:left;width: 70%;' > ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>
			<div style = 'float:right;width: 30%;text-align: end;' > ਮਿਤੀ: " + gt.ToString() + @"</div>			
			</div>			
 <p style = 'font-size: 12pt; text-indent: 42px;'>
     ਉਤਾਰਾ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਵਧੀਕ ਮੁੱਖ ਸਕੱਤਰ/ਰਾਜਪਾਲ ਨੂੰ ਰਾਜਪਾਲ ਪੰਜਾਬ ਜੀ ਦੀ ਸੂਚਨਾ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।		
      </p>";
                    }
                }
                outXml += " </div></div></body> </html>";
            }
            return outXml;
        }

        public string GetNewapprovepdfhtmlPreview(int agendaid, string doctype)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();

            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            int ofid = 0;
            if (CurrentSession.OfficeId == "47")
            {
                ofid = 2;
            }
            else if (CurrentSession.OfficeId == "10")
            {
                ofid = 1;
            }
            Authoritylist cl = new Authoritylist();
            cl.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);
            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            // string officetype = string.Empty;
            // officetype = AgendaModelFunction.GetOfficeType(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>Digital MC House: Approved Report</title>                                  
                                        <style>
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            table{
                                            width:100%;
                                            }
                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");

            if (HeaderlistofAgenda != null)
            {
                outXml += @"<div style='margin: 70px 70px;page-break-before: always;'>
                        <h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    + HeaderlistofAgenda.AgendaName + "</h1>";
                outXml += @"<p style='font-size: 12pt; text-align: center'>"
                 + HeaderlistofAgenda.Header + "</p></div>";
            }

            outXml += @"<table style='font-size: 12pt; border: 1px solid black;border-collapse: collapse;'>";
            outXml += @"<tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse; width:10%;'>Sr. No</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse; width:15%;'>Date of Meeting</th>



                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>" + doctype + "  Details </th></tr>";

            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + mdl._LstReferences[i].SrNo + @"    
										</td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</table>";
                        if (HeaderlistofAgenda != null)
                        {
                            outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                                         " + HeaderlistofAgenda.Footer + "</p>";
                        }

                    }
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }

        [HttpGet]
        public JsonResult GetAgendaFiles(string agendaid)
        {
            var path = BlobStorage.GetStorageAcessingPath();
            var dtt = AgendaModelFunction.GetAgendaFiles(agendaid);

            if (dtt == null || dtt.Rows.Count == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "No data found for the given agenda ID."
                }, JsonRequestBehavior.AllowGet);
            }

            string agendaPdfFileName = dtt.Rows[0][2]?.ToString();
            string proceedingPdfFileName = dtt.Rows[0][3]?.ToString();

            return Json(new
            {
                success = true,
                AgendaApprove = dtt.Rows[0][0]?.ToString(),
                ApproveProceeding = dtt.Rows[0][1]?.ToString(),
                AgendaPdf = !string.IsNullOrWhiteSpace(agendaPdfFileName) ? path + "/" + agendaPdfFileName : null,
                AgendaPdfName = agendaPdfFileName,
                ProceedingPdf = !string.IsNullOrWhiteSpace(proceedingPdfFileName) ? path + "/" + proceedingPdfFileName : null,
                ProceedingPdfName = proceedingPdfFileName
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ApproveProceedings(string agendaid)
        {
            var path = BlobStorage.GetStorageAcessingPath();
            AgendaModel model = new AgendaModel();
            if (agendaid != "")
            {
                AgendaModelFunction.ApproveProceedings(Convert.ToInt32(agendaid), 1);
            }

            return Json(new
            {
                ApproveProceeding = "Yes",
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getDeptWiseCouncillorslist(int agendaid)
        {
            AgendaModel model = new AgendaModel();
            model.Counselorslist = AgendaModelFunction.getDeptWiseCouncillorslist(CurrentSession.DeptID, agendaid);

            //List<string> myStringList = model.mProjectDepartmentList.AsEnumerable().Select(x => x.Value).ToList();

            //IEnumerable<SelectListItem> filteredList = model.mDepartmentList.Where(x => !myStringList.Contains(x.Value));
            //model.mDepartmentList = new SelectList(filteredList, "Value", "Text");
            var dtt = model.Counselorslist;

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpPost]
        public ActionResult SendMeetingSMS(List<string> selectedMembers)
        {
            foreach (var members in selectedMembers)
            {
                string[] userresult = members.Split('-');
                string mobno = "8077449369";// userresult[0].ToString();
                string[] member = userresult[1].Split(',');
                string name = member[0].ToString();

                string meetingdate = userresult[2].ToString();

                string[] credentials = { name, meetingdate, "Secretary,", CurrentSession.DeptName };
                SMSMessage _smsService = new SMSMessage();
                Task<string> task = _smsService.SendMeetingMessageMultipleVariables(mobno, credentials);

            }
            return Content("Received " + selectedMembers.Count + " selected values");
        }
        [HttpPost]
        public ActionResult SendProceedingSMS(List<string> selectedMembers)
        {
            foreach (var members in selectedMembers)
            {
                string[] userresult = members.Split('-');
                string mobno = "8077449369";// userresult[0].ToString();
                string[] member = userresult[1].Split(',');
                string name = member[0].ToString();

                string meetingdate = userresult[2].ToString();

                string[] credentials = { name, meetingdate, "Secretary,", CurrentSession.DeptName };
                SMSMessage _smsService = new SMSMessage();
                Task<string> task = _smsService.SendProceedingMessageMultipleVariables(mobno, credentials);

            }
            return Content("Received " + selectedMembers.Count + " selected values");
        }
        //Generate html report code
        //public ActionResult SampleHtml(int AgendaId)
        //{
        //    string htmlString = GetNewapprovepdfhtml(AgendaId);

        //    string finalpdfpath = "Agenda/" + AgendaId.ToString() + "_Agenda.html";
        //    string folderpath = "~/SecureFileStructure/" + finalpdfpath;
        //    string agendapath = Server.MapPath(folderpath);

        //    var root = "~/SecureFileStructure/Agenda";
        //    bool path = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
        //    if (!path)
        //    {
        //        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
        //    }


        //    System.IO.File.WriteAllText(agendapath, htmlString);
        //    SavePDf(AgendaId.ToString(), finalpdfpath);
        //    Response.ContentType = "text/html";

        //    //Get the byte array representation of the HTML string
        //    byte[] htmlBytes = Encoding.UTF8.GetBytes(htmlString);

        //    //Write the HTML bytes to the response stream
        //    Response.OutputStream.Write(htmlBytes, 0, htmlBytes.Length);
        //    Response.End();
        //    return File(htmlString, "text/html", AgendaId.ToString() + "_agenda.html");
        //}


        public JsonResult getVanueList()
        {
            AgendaModel model = new AgendaModel();
            var s = AgendaModelFunction.getVanueList();
            var dtt = s;
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        public ActionResult CreateMeetingNotice()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            AgendaModel model = new AgendaModel();

            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                ViewBag.OfficeName = CurrentSession.OfficeName;
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                model._LstMeetingNoticeAgenda = AgendaModelFunction.GetAgendaForMeetingNotice();
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return View(model);
            }

        }

        public ActionResult MergeAndLinkMeetingNotice()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            AgendaModel model = new AgendaModel();

            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                ViewBag.OfficeName = CurrentSession.OfficeName;
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                model._LstMeetingNoticeAgenda = AgendaModelFunction.GetAgendaForMeetingNoticeEbook();
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return View(model);
            }

        }

        public ActionResult PreviewReportMeetingNotice(int agendaid)
        {
            try
            {
                var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
                string htmlString = GeteCabinetNoticehtml(agendaid);
                var MCName = HeaderlistofAgenda.Status;
                var MCHeader = "eCabinet Meeting Notice  (" + MCName + ")";
                var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
                {
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
                    CustomSwitches = GetCustomSwitches(MCHeader)
                };
                byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
                byte[] finalPdfBytes = ApplyWatermark(pdfBytes);
                string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{agendaid}_Proceeding.pdf";
                Response.Headers.Add("Content-Disposition", "inline; filename=" + pdfFilename);
                return File(finalPdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }
        public ActionResult ApproveReportMeetingNotice(int agendaid)
        {
            try
            {
                var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
                string htmlString = GeteCabinetNoticehtml(agendaid);
                var Status = HeaderlistofAgenda.Status;
                var MCHeader = "eCabinet Meeting Notice  (" + Status + ")";
                var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
                {
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
                    CustomSwitches = GetCustomSwitches(MCHeader) // Externalized header/footer configuration
                };

                byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
                string filefolder = "MeetingNotice";
                string fileName = $"{DateTime.Now:ddMMyyyyHHmmss}_{agendaid}_MeetingNotice.pdf"; // Correct string interpolation
                string pdfFilename = Path.Combine(filefolder, fileName);

                BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
                AgendaController.SavePDf2(agendaid.ToString(), pdfFilename, "MeetingNotice");
                Circular circular = new Circular();
                //circular.PDFFile = FileHelper.ConvertBytesToHttpPostedFileBase(pdfBytes, fileName, "text/plain");
                circular.PDFPath = pdfFilename;
                circular.Subject = "eCabinet Meeting Notice";
                circular.AgendaId = agendaid;
                //circular.CircularDate = DateTime.Now;
                CircularDS.SaveNoticeRecord(circular);
                Response.Headers.Add("Content-Disposition", "inline; filename=" + pdfFilename);
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }

        public ActionResult CabinetMemorandum(string RefId, string doctype)
        {
            try
            {
                //var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(RefId);
                string htmlString = GeteCabinetMemorandumhtml(RefId);
                //var Status = HeaderlistofAgenda.Status;
                var MCHeader = "NextGen e-Cabinet: Memorandum Report";
                var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
                {
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
                    CustomSwitches = GetCustomSwitches(MCHeader) // Externalized header/footer configuration
                };

                byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
                string filefolder = "MeetingNotice";
                string fileName = $"{DateTime.Now:ddMMyyyyHHmmss}_{RefId}_CabinetMemorandum.pdf"; // Correct string interpolation
                string pdfFilename = Path.Combine(filefolder, fileName);

                BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);

                Response.Headers.Add("Content-Disposition", "inline; filename=" + pdfFilename);
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(500, "Internal Server Error");
            }
        }


        private string GetCustomSwitches(string header)
        {
            return $"--footer-left \"Page: [page] of [toPage]\" " +
                   $"--footer-center \"NextGen e-Cabinet Punjab\" " +
                   $"--footer-right \"PDF Preview Generated on: [date]\" " +
                   $"--footer-font-size 10 --footer-spacing 5 " +
                   $"--header-center \"{header}\" " +
                   "--header-font-size 12 --header-spacing 10 " +
                   "--zoom 1.3 --no-outline";
        }
        private byte[] ApplyWatermark(byte[] pdfBytes)
        {
            using (var inputPdfStream = new MemoryStream(pdfBytes))
            using (var outputPdfStream = new MemoryStream())
            {
                var reader = new iTextSharp.text.pdf.PdfReader(inputPdfStream);
                var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);


                var watermarkFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 100, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(192, 192, 192));
                var watermarkText = "Preview";


                int pageCount = reader.NumberOfPages;
                for (int i = 1; i <= pageCount; i++)
                {
                    var pdfContent = stamper.GetOverContent(i);
                    var pageSize = reader.GetPageSizeWithRotation(i);

                    var gstate = new iTextSharp.text.pdf.PdfGState { FillOpacity = 0.5f }; // Semi-transparent watermark
                    pdfContent.SaveState();
                    pdfContent.SetGState(gstate);


                    pdfContent.BeginText();
                    pdfContent.SetColorFill(iTextSharp.text.BaseColor.LIGHT_GRAY);
                    pdfContent.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED), 100);

                    float x = (pageSize.Left + pageSize.Right) / 2;
                    float y = (pageSize.Top + pageSize.Bottom) / 2;
                    float angle = 45f; // Diagonal angle (can be customized)

                    pdfContent.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, watermarkText, x, y, angle);
                    pdfContent.EndText();

                    pdfContent.RestoreState();
                }


                stamper.Close();
                reader.Close();

                return outputPdfStream.ToArray();
            }
        }
        public string GeteCabinetNoticehtml(int agendaid)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();
            var path1 = BlobStorage.GetStorageAcessingPath();
            var aadhaarId = "cs@punjab.gov.in";
            var userList = CMNirdesh.Models.Login.GetDetailsByadhaarID(aadhaarId);

            // Initialize the signature variable
            string Chiefsignature = "";

            // Check if userList is not empty or null
            if (userList != null && userList.Count > 0)
            { // Use Count instead of length
                foreach (var user in userList)
                {
                    // Check if EmailId matches the provided aadhaarId (Email ID)
                    if (user.UserName == aadhaarId)
                    {
                        Chiefsignature = $"{path1}{user.SignaturePath}"; // Get the SignaturePath
                        break; // Exit the loop once you find the match
                    }
                }
            }

            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            string signature = $"{path1}{CurrentSession.SignaturePath}";

            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                <head><meta charset='UTF-8'><title>NextGen e-Cabinet: ApprovedAgendaReport</title>                                   
                                    <style>
                                .pdf-class .MsoNormal {
                                    text-wrap: wrap;
                                    text-align: justify; /* Justifying paragraphs */
                                }

                                .pdf-class .MsoNormalTable td {
                                    max-width: 65px !important;
                                    padding: 1px !important;
                                    overflow-wrap: break-word;
                                }

                                .pdf-class .MsoNormalTable {
                                    max-width: 700px !important;
                                    margin-left: 0 !important;
                                }

                                .pdf-class table.MsoNormalTable {
                                    max-width: 700px !important;
                                    margin-left: 0 !important;
                                }

                                .pdf-class .MsoTableGrid td {
                                    max-width: 65px !important;
                                    text-wrap: wrap;
                                }

                                .pdf-class .MsoTableGrid {
                                    max-width: 500px !important;
                                    margin-left: 0 !important;
                                }

                                .pdf-class table.MsoTableGrid {
                                    max-width: 500px !important;
                                    margin-left: 0 !important;
                                }

                                p.MsoListParagraphCxSpFirst,
                                p.MsoListParagraphCxSpMiddle,
                                p.MsoListParagraphCxSpLast {
                                    margin-left: 20px;
                                }

                                td {
                                    vertical-align: top;
                                }

                                @media print {
                                    .header {
                                        display: none;
                                    }
                                }

                                p {
                                    line-height: 1.7;
                                    text-align: justify; /* Justifying paragraphs */
                                }

                                table {
                                    width: 100%;
                                }

                                #bloc1 {
                                    float: left;
                                    width: 200px;
                                }

                                #bloc2 {
                                    text-align: center;
                                }
                                    </style>
                                </head> 
                             <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");

            outXml += @"<div style='margin: 5px 5px;page-break-before: always;'>
                <div id='maindiv'>
                  <div id='bloc1' style='border:1px solid;border-radius:5px;padding:7px;'> 
                 ਮੀਟਿੰਗ ਦੀ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate + @"<br>        	                 	    	                                                                
ਸਮਾਂ: " + HeaderlistofAgenda.StartTime + @" ਵਜੇ (ਸਵੇਰੇ/ਦੁਪਹਿਰ ਬਾਅਦ)<br>	         		 	 			                                                 
ਸਥਾਨ: " + HeaderlistofAgenda.Venue + @"         
                 </div>  
                 <div id='bloc2' style='padding:5px;'>
	             <span>ਪੰਜਾਬ ਸਰਕਾਰ </span><br/>
	             <span>ਆਮ ਰਾਜ ਪ੍ਰਬੰਧ ਵਿਭਾਗ </span><br/>
	             <span>(ਮੰਤਰੀ ਮੰਡਲ ਮਾਮਲੇ ਸ਼ਾਖਾ)</span><br/>
	             </div></div>
       <div style='margin:0px 40px'>
       <p style='font-size: 12pt; text-indent: 42px;'>ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਅਗਲੀ ਮੀਟਿੰਗ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate + @" ਦਿਨ  " + HeaderlistofAgenda.Day + @"  ਨੂੰ ਸਵੇਰੇ/ਦੁਪਹਿਰ ਬਾਅਦ " + HeaderlistofAgenda.StartTime + @" ਵਜੇ ਸਥਾਨ " + HeaderlistofAgenda.Venue + @" ਵਿਖੇ ਹੋਵੇਗੀ। ਮੀਟਿੰਗ ਦਾ ਅਜੰਡਾ ਬਾਅਦ ਵਿੱਚ ਜਾਰੀ ਕੀਤਾ ਜਾਵੇਗਾ। 
       </p>
 

            <div style='text-align:right;padding-top:5px'>
			<img src='" + Chiefsignature + @"' alt='Signature' style='height:50px;'><br>
			ਕੇ ਏ ਪੀ  ਸਿਨਹਾ 
			<br>
			ਮੁੱਖ ਸਕੱਤਰ, ਪੰਜਾਬ ਸਰਕਾਰ					
			</div>

<p style='font-size: 12pt; text-indent: 42px;'>
 ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਹੇਠ ਲਿਖਿਆਂ ਨੂੰ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕ੍ਰਿਪਾਲਤਾ ਕੀਤੀ ਜਾਵੇ:-
              </p>
	             <p style='font-size: 12pt; text-indent: 50px;'>
	           <ol>
	           <li>
	           <span style='padding-left: 50px;'>ਮੁੱਖ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	           </li>
	           <li>
	           <span style='padding-left: 50px;'>ਸਮੂਹ ਕੈਬਨਿਟ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	           </li>
	           <li>
	           <span style='padding-left: 50px;'>ਵਿਸ਼ੇਸ਼ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ/ਮੁੱਖ ਮੰਤਰੀ   </span>
	           </li>
	           </p>
	      <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + @"' alt='Signature' style='height:50px;'><br>
			ਸੁਪਰਡੰਟ						
			</div>
			
			<div class='maindiv'>
			<div style='float:left;width: 70%;'>ਪਅੰ.ਵਿ.ਪੱ.ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
			<div class='maindiv'>
			<div style='float:left;width: 70%;padding-top:5px;'>ਪੱ:ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;padding-top:5px;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
            <p style='font-size: 12pt; text-indent: 42px;'>
            ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਸਮੂਹ ਵਿਸ਼ੇਸ ਮੁੱਖ ਸਕੱਤਰ/ਵਧੀਕ ਮੁੱਖ ਸਕੱਤਰ/ਵਿੱਤੀ
            ਕਮਿਸ਼ਨਰ/ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ/ਸਕੱਤਰ, ਪੰਜਾਬ ਸਰਕਾਰ ਨੂੰ ਸੂਚਨਾ ਅਤੇ ਲੋੜੀਂਦੀ ਕਾਰਵਾਈ ਹਿੱਤ
            ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।
            </p>
	  
	   <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + @"' alt='Signature' style='height:50px;'><br>
			ਸੁਪਰਡੰਟ						
			</div>
	  <div class='maindiv'>
			<div style='float:left;width: 70%;padding-top:5px;'>ਪੱ:ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;padding-top:5px;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
       <p style='font-size: 12pt; text-indent: 42px;'>
        ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕੀਤੀ ਜਾਵੇ।
      </p>
	  
	  
	    <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + @"' alt='Signature' style='height:50px;'><br>
			ਸੁਪਰਡੰਟ						
			</div>
	  <div class='maindiv'>
			<div style='float:left;width: 70%;padding-top:5px;'>ਪੱ:ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;padding-top:5px;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
       <p style='font-size: 12pt; text-indent: 42px;'>
        ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਵਿਸ਼ੇਸ਼ ਸਕੱਤਰ/ਮੁੱਖ ਸਕੱਤਰ ਨੂੰ ਮੁੱਖ ਸਕੱਤਰ ਪੰਜਾਬ ਜੀ ਦੀ ਸੂਚਨਾ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।
      </p>
	  
	   <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + @"' alt='Signature' style='height:50px;'><br>
			ਸੁਪਰਡੰਟ						
			</div>
	    <div class='maindiv'>
			<div style='float:left;width: 70%;padding-top:5px;'>ਪੱ:ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;padding-top:5px;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
                </div>";
            return outXml;
        }



        public ActionResult UpdateLinkStatus(string fileId, int isLinked)
        {
            try
            {
                // Call the method to update the link status and get the response
                string reff = AgendaEdit.UpdateLinkStatus(fileId, isLinked);

                // Check the response from the UpdateLinkStatus method
                if (reff == "0")
                {
                    return Json(new { success = true, message = "Link status updated." });
                }
                else if (reff == "2")
                {
                    return Json(new { success = false, message = "EBook is already linked Same Agenda Date and Time.Please De-link that and try again." });
                }
                else if (reff == "3")
                {
                    return Json(new { success = false, message = "This Agenda is already linked"});
                }

                else if (reff == "1")
                {
                    return Json(new { success = false, message = "EBook is already linked." });
                }
                else
                {
                    // If reff is some unexpected value
                    return Json(new { success = false, message = "Unknown error occurred." });
                }
            }
            catch (Exception ex)
            {
                // Log the error (if necessary) or handle it as needed
                return Json(new { success = false, message = "Error updating link status.", error = ex.Message });
            }
        }


        //eCabinet Memorandum
        public static string GeteCabinetMemorandumhtml(string RefId)
        {
            var mdl = new AgendaDetailModel();
            var path1 = BlobStorage.GetStorageAcessingPath();
            mdl._LstReferences = AgendaModelFunction.GetMemoDetails(RefId);

            string signature = string.IsNullOrEmpty(CurrentSession.SignaturePath) ? "" : $"{path1}/Signature/{CurrentSession.SignaturePath}";
            var outXml = new StringBuilder();

            outXml.Append(@"<html style='height: 100%;'>   
           <head><meta charset='UTF-8'><title>NextGen e-Cabinet: Memorandum Report</title>                                   
           <style>
               body { font-size: 16px !important; }
               .pdf-class .MsoNormal { text-wrap: wrap; }
               .pdf-class .MsoNormalTable td { max-width: 65px !important; padding: 1px !important; overflow-wrap: break-word; }
               .pdf-class .MsoNormalTable, .pdf-class table.MsoNormalTable { max-width: 700px !important; margin-left: 0 !important; }
               .pdf-class .MsoTableGrid td { max-width: 65px !important; text-wrap: wrap; }
               .pdf-class .MsoTableGrid, .pdf-class table.MsoTableGrid { max-width: 500px !important; margin-left: 0 !important; }
               p.MsoListParagraphCxSpFirst, p.MsoListParagraphCxSpMiddle, p.MsoListParagraphCxSpLast { margin-left: 20px; }
               td { vertical-align: top; }
               table { width: 100%; border-collapse: collapse; }
               th, td { border: 1px solid black; padding: 8px; }
               @media print { .header { display: none; } }
               p { line-height: 1.7; }
               .page-break { page-break-before: always; }
               #bloc1 { float: left; width: 160px; }
               #bloc2 { text-align: center; }
           </style>
       </head> 
       <body><div class='pdf-class' style='max-width:1050px; margin:auto; min-height: 100%;'>");

            if (mdl._LstReferences?.Count > 0)
            {
                foreach (var reference in mdl._LstReferences)
                {
                    // *Remove font-size from SubjectDetails*
                    //string cleanedSubjectDetails = Regex.Replace(reference.SubjectDetails, @"font-size:\s*\d+(\.\d+)?(px|pt)\s*!important;?", "", RegexOptions.IgnoreCase);
                    string cleanedSubjectDetails = Regex.Replace(reference.SubjectDetails, @"font-size\s*:\s*\d+(\.\d+)?(px|pt)?\s*!?(important)?\s*;?", "", RegexOptions.IgnoreCase);

                    outXml.Append($@"
                <div class='page-break' style='margin: 20px 70px;'>
                    <div id='maindiv' style='line-height: 30px;'>
                        <div style='float:left;width:40%;'> 
                            ਮੱਦ ਨੰ .....<br> (ਸਾਲਾਨਾ)	             
                        </div>  
                        <div style='text-align:end;'>
                            <span>ਮੰਤਰੀ ਮੰਡਲ ਦੀ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ ਮੱਦ ਨੰ.... </span><br/>
                            <span>ਮੰਤਰੀ ਮੰਡਲ ਦੀ ਮੀਟਿੰਗ ਦੀ ਮਿਤੀ ..............</span><br/>
                            <span>ਗੁਪਤ .............</span><br/>
                            <span style='text-align:end;'>ਕਾਪੀ ਨੰ ............</span>	
                        </div>
                    </div>
                    <div style='text-align:center'>
                        <span> ਪੰਜਾਬ ਸਰਕਾਰ </span><br/>
                        <span> {reference.FromDept} </span><br/>
                        <span> ({reference.FromOffice}) </span><br/>
                    </div>
                    <p style='text-align:center; text-decoration:underline;'>ਮੰਤਰੀ ਮੰਡਲ ਲਈ ਮੈਮੋਰੰਡਮ</p>
                    <div id='maindiv' style='margin-top:15px; line-height: 30px;'>
                        <div style='float:left;width:40%;'> 
                            ਮੰਤਰੀ ਇੰਚਾਰਜ<br><br>ਸਕੱਤਰ ਇੰਚਾਰਜ             
                        </div>  
                        <div style='text-align:end;'>
                            <span>{reference.MinisterName}, ਪੰਜਾਬ।</span><br/><br/>
                            <span>{CurrentSession.Designation}</span><br/>
                            <span>{reference.FromDept}</span><br/>
                        </div>
                    </div>
                    <div style='font-size: 16px !important;'>
                       
                                <b>ਵਿਸ਼ਾ :</b> {reference.Subject} <br><br>
                                <p style='font-size: 16px !important;'>{cleanedSubjectDetails}</p>
                            
                    </div>
                    <div style='text-align:right; padding-top:5px'>
                        {(string.IsNullOrEmpty(signature) ? "" : $"<img src='{signature}' alt='Signature' style='height:50px;'><br>")}
                        <span>ਸੁਪਰਡੰਟ</span>
                    </div>
                    <div class='maindiv'>
                        <div style='float:left;width: 30%;'>
                            <span>ਚੰਡੀਗੜ੍ਹ,<br>ਮਿਤੀ: {DateTime.Now:dd/MM/yyyy}</span>
                        </div>
                        <div style='float:right;width: 70%; text-align: end;'>
                            <span>(ਵਿਕਾਸ ਪ੍ਰਤਾਪ)</span><br>
                            <span>ਵਧੀਕ ਮੁੱਖ ਸਕੱਤਰ-ਕਮ-ਵਿੱਤੀ ਕਮਿਸ਼ਨਰ (ਕਰ)</span><br>
                            <span>ਆਬਕਾਰੀ ਤੇ ਕਰ ਵਿਭਾਗ ।</span>
                        </div>			
                    </div>
                </div>");
                }
            }

            outXml.Append(" </div></body></html>");
            return outXml.ToString();
        }


        //Aman dept reply
        public string GeteCabinetAgendaDeptReplyhtml(int agendaid, int deptid)
        {
            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();



            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetDeptlobDataForProcedingPDF(agendaid, deptid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var path1 = BlobStorage.GetStorageAcessingPath();
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            string signature = path1 + "/Signature/" + CurrentSession.SignaturePath;

            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>NextGen e-Cabinet: ApprovedAgendaReport</title>                                   
                                        <style>
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            table{
                                            width:100%;
                                            }
                                       #bloc1
                       {
                        float:left;
						width: 160px;
                       }
					   #bloc2
					   {
					   text-align: center;
					   }
                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");

            outXml += @"<div style='margin: 20px 70px;page-break-before: always;'>
                  <div id='bloc1' style='height:45px'> 
                ਤੁਰੰਤ<br>	         		 	 			                                                 
                ਗੁਪਤ<br>            
                 </div>  
                 <div id='bloc2'>
	             <span>ਪੰਜਾਬ ਸਰਕਾਰ </span><br/>
	             <span>ਆਮ ਰਾਜ ਪ੍ਰਬੰਧ ਵਿਭਾਗ </span><br/>
	             <span>(ਮੰਤਰੀ ਮੰਡਲ ਮਾਮਲੇ ਸ਼ਾਖਾ)</span><br/>
	             </div>";
            string _subject = "";
            string _actiondesc = "";
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                _subject = mdl._LstReferences[0].Subject;
                _actiondesc = mdl._LstReferences[0].ActionDescription;
            }
            outXml += @"<div style='margin-top:35px'>
            <div style='float:left;width:50px;height: 26px;'>
			ਵਿਸ਼ਾ:
			</div>
			<div>" + _subject + @"
            </div>
			</div>
 <p style='text-align:center'>*****</p>
 <p style='font-size: 12pt; text-indent: 42px;'>" + _actiondesc
 + @"</p>
 <p style='font-size: 12pt; text-indent: 42px;'>2. ਮੁਢਲਾ ਸਿਖਿਆ ਵਿਚ ਸਰੀਰਕ ਸਿੱਖਲਾਈ ਇੰਸਟ੍ਰਕਟਰ (ਸੰਖੇਪ ਵਿਚ ਪੀ.ਟੀ.ਆਈ.)ਦੀਆ 2000 ਆਸਾਮੀਆ ਦੀ ਸਿੱਧੀ ਭਰਤੀ
	 ਰਾਜ ਸਰਕਾਰ ਦੁਆਰਾ ਅਧਿਕ੍ਰਿਤ ਭਰਤੀ ਏਜੰਸੀ ਦੁਆਰਾ ਕਰਨ ਲਈ ਸੇਵਾ ਨੀਯਮ ਬਨਾਉਨ ਸਬੰਧੀ.</p>
	  <p style='font-size: 12pt; text-indent: 42px;'>3. ਮੁਢਲਾ ਸਿਖਿਆ ਵਿਚ ਸਰੀਰਕ ਸਿੱਖਲਾਈ ਇੰਸਟ੍ਰਕਟਰ (ਸੰਖੇਪ ਵਿਚ ਪੀ.ਟੀ.ਆਈ.)ਦੀਆ 2000 ਆਸਾਮੀਆ ਦੀ ਸਿੱਧੀ ਭਰਤੀ
	 ਰਾਜ ਸਰਕਾਰ ਦੁਆਰਾ ਅਧਿਕ੍ਰਿਤ ਭਰਤੀ ਏਜੰਸੀ ਦੁਆਰਾ ਕਰਨ ਲਈ ਸੇਵਾ ਨੀਯਮ ਬਨਾਉਨ ਸਬੰਧੀ.</p>
  <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
            outXml += @" ਸੁਪਰਡੰਟ
</div>
<p>ਵਧਿਕ ਮੁਖ ਸਕਤਰ, ਪੰਜਾਬ ਸਰਕਾਰ, <br>ਸਕੂਲ ਸਿਖਿਆ ਵਿਭਾਗ |</p>	
	<div class='maindiv'>
			<div style='float:left;width: 70%;'>ਅੰ.ਵਿੱ.ਪ.ਨੰ:01/130/2023-1 ਕੈਬਨਿਟ/3002</div>
			<div style='float:right;width: 30%;text-align: end;'>ਮਿਤੀ,ਚੰਡੀਗੜ੍ਹ:12/06/2023</div>			
			</div>
			<div class='maindiv'>
			<div style='float:left;width: 70%;'>ਪ.ਨੰ:01/130/2023-1 ਕੈਬਨਿਟ/3003</div>
			<div style='float:right;width: 30%;text-align: end;'>ਮਿਤੀ,ਚੰਡੀਗੜ੍ਹ:12/06/2023</div>			
			</div>
 <p style='font-size: 12pt; text-indent: 42px;'>
 ਉਤਾਰਾ ਹੇਠ ਲਿਖਿਆਂ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਸ਼ਾਮਲ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ:-
      </p>
	   <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
            outXml += @" ਸੁਪਰਡੰਟ
</div>
<div class='maindiv'>
		<div style='float:left;width: 70%;'>ਪ.ਨੰ:01/130/2023-1 ਕੈਬਨਿਟ/3004</div>
		<div style='float:right;width: 30%;text-align: end;'>ਮਿਤੀ,ਚੰਡੀਗੜ੍ਹ:12/06/2023</div>		
		</div>
 <p style='font-size: 12pt; text-indent: 42px;'>
 ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ। 
      </p>
  <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
            outXml += @" ਸੁਪਰਡੰਟ
</div>
<div class='maindiv'>
		<div style='float:left;width: 70%;'>ਪ.ਨੰ:01/130/2023-1 ਕੈਬਨਿਟ/3005</div>
		<div style='float:right;width: 30%;text-align: end;'>ਮਿਤੀ,ਚੰਡੀਗੜ੍ਹ:12/06/2023</div>		
		</div>
 <p style='font-size: 12pt; text-indent: 42px;'>
 ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ। 
      </p>
  <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
            outXml += @" ਸੁਪਰਡੰਟ
</div>
<div class='maindiv'>
		<div style='float:left;width: 70%;'>ਪ.ਨੰ:01/130/2023-1 ਕੈਬਨਿਟ/3006</div>
		<div style='float:right;width: 30%;text-align: end;'>ਮਿਤੀ,ਚੰਡੀਗੜ੍ਹ:12/06/2023</div>		
		</div>
 <p style='font-size: 12pt; text-indent: 42px;'>
 ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ। 
      </p>
  <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
            outXml += @" ਸੁਪਰਡੰਟ
</div><div class='maindiv'>
		<div style='float:left;width: 70%;'>ਪ.ਨੰ:01/130/2023-1 ਕੈਬਨਿਟ/3007</div>
		<div style='float:right;width: 30%;text-align: end;'>ਮਿਤੀ,ਚੰਡੀਗੜ੍ਹ:12/06/2023</div>		
		</div>
 <p style='font-size: 12pt; text-indent: 42px;'>
 ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ। 
      </p>
  <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
            outXml += @" ਸੁਪਰਡੰਟ
</div><div class='maindiv'>
		<div style='float:left;width: 70%;'>ਪ.ਨੰ:01/130/2023-1 ਕੈਬਨਿਟ/3008</div>
		<div style='float:right;width: 30%;text-align: end;'>ਮਿਤੀ,ਚੰਡੀਗੜ੍ਹ:12/06/2023</div>		
		</div>
 <p style='font-size: 12pt; text-indent: 42px;'>
 ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ। 
      </p>
  <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
            outXml += @" ਸੁਪਰਡੰਟ
</div>
";
            outXml += "</div></div></body> </html>";

            return outXml;
        }




        //for supplementry code

        public ActionResult SupplementryAgendaHtmlpreview(int AgendaId, string doctype)
        {
            string htmlString = GeteCabinetAgendafSupplimentaryhtml(AgendaId, doctype);

            // string htmlString = GetNewapprovepdfhtml(AgendaId, doctype);
            var MCName = CurrentSession.MCName;
            var MCHeader = "NextGen e-Cabinet " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                // PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"NextGen e-Cabinet Punjab\" " +
                    "--footer-right \"Agenda Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);

            // Add diagonal watermark using iTextSharp
            using (var inputPdfStream = new MemoryStream(pdfBytes))
            using (var outputPdfStream = new MemoryStream())
            {
                var reader = new iTextSharp.text.pdf.PdfReader(inputPdfStream);
                var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);

                // Create the watermark
                var watermarkFont = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 100, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(192, 192, 192));
                var watermarkText = "Preview";

                // Loop through pages and apply watermark
                int pageCount = reader.NumberOfPages;
                for (int i = 1; i <= pageCount; i++)
                {
                    var pdfContent = stamper.GetOverContent(i);
                    iTextSharp.text.Rectangle pageSize = reader.GetPageSizeWithRotation(i);

                    // Set diagonal rotation
                    var gstate = new iTextSharp.text.pdf.PdfGState { FillOpacity = 0.5f }; // Semi-transparent
                    pdfContent.SaveState();
                    pdfContent.SetGState(gstate);

                    // Calculate position and rotation
                    pdfContent.BeginText();
                    pdfContent.SetColorFill(iTextSharp.text.BaseColor.LIGHT_GRAY);
                    pdfContent.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED), 100);

                    float x = (pageSize.Left + pageSize.Right) / 2;
                    float y = (pageSize.Top + pageSize.Bottom) / 2; // Below content
                    float angle = (float)Math.Atan2(pageSize.Top - pageSize.Bottom, pageSize.Right - pageSize.Left) * (180 / (float)Math.PI);
                    //float angle = 60; // Diagonal angle

                    pdfContent.ShowTextAligned(iTextSharp.text.pdf.PdfContentByte.ALIGN_CENTER, watermarkText, x, y, angle);
                    pdfContent.EndText();

                    pdfContent.RestoreState();
                }

                // Close the stamper
                stamper.Close();
                reader.Close();
                // Get the final PDF with watermark
                pdfBytes = outputPdfStream.ToArray();
            }


            string pdfFilename = $"Agenda/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Agenda.pdf";


            Response.Headers.Add("Content-Disposition", "inline; filename=Agenda");
            return File(pdfBytes, "application/pdf");

            //return File(pdfBytes, "application/pdf", "Agenda.pdf");
        }


        public ActionResult SupplementryAgendaHtml(int AgendaId, string doctype)
        {
            string htmlString = GeteCabinetAgendafSupplimentaryhtml(AgendaId, doctype);

            // string htmlString = GetNewapprovepdfhtml(AgendaId, doctype);
            var MCName = CurrentSession.MCName;
            var MCHeader = "NextGen e-Cabinet " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageSize = Rotativa.Options.Size.A4,
                // PageMargins = new Rotativa.Options.Margins(5, 5, 5, 5),
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"NextGen e-Cabinet Punjab\" " +
                    "--footer-right \"Agenda Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);



            string pdfFilename = $"Agenda/{DateTime.Now:ddMMyyyyHHmmss}_{AgendaId}_Agenda.pdf";

            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            SavePDf(AgendaId.ToString(), pdfFilename);



            //Aprove Agenda
            AgendaModelFunction.ApproveAgenda(Convert.ToInt32(AgendaId.ToString()), 1);
            string folderpath = "~/SecureFileStructure/" + pdfFilename;
            string finalpdfpath = Server.MapPath(folderpath);
            if (!System.IO.File.Exists(finalpdfpath))
            {
                var fileStream = new FileStream(finalpdfpath, FileMode.Create, FileAccess.Write);
                fileStream.Write(pdfBytes, 0, pdfBytes.Length);
                fileStream.Close();
            }

            Response.Headers.Add("Content-Disposition", "inline; filename=Agenda");
            return File(pdfBytes, "application/pdf");

            //return File(pdfBytes, "application/pdf", "Agenda.pdf");
        }


        //Supplementry
        public string GeteCabinetAgendafSupplimentaryhtml(int agendaid, string doctype)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();

            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            int ofid = 0;
            if (CurrentSession.OfficeId == "47")
            {
                ofid = 2;
            }
            else if (CurrentSession.OfficeId == "10")
            {
                ofid = 1;
            }
            Authoritylist cl = new Authoritylist();
            cl.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);
            string maintitle = string.Empty;
            var path1 = BlobStorage.GetStorageAcessingPath();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            string signature = path1 + "/Signature/" + CurrentSession.SignaturePath;

            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>NextGen e-Cabinet: ApprovedAgendaReport</title>                                   
                                        <style>
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            table{
                                            width:100%;
                                            }
                                       #bloc1
                       {
                        float:left;
						width: 160px;
                       }
					   #bloc2
					   {
					   text-align: center;
					   }
                                        </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");
            if (HeaderlistofAgenda != null)
            {
                outXml += @"<div style='margin: 3px 10px;page-break-before: always;'>
                  <div id='bloc1'> 
                  ਕੈਬਨਿਟ ਮੀਟਿੰਗ<br>        	                 	    	                                                                
                 ਸਪਲੀਮੈਂਟਰੀ ਅਜੰਡਾ-1<br>	         		 	 			                                                 
                 ਗੁਪਤ<br>             
                 </div>  
                 <div id='bloc2'>
	             <span>ਪੰਜਾਬ ਸਰਕਾਰ </span><br/>
	             <span>ਆਮ ਰਾਜ ਪ੍ਰਬੰਧ ਵਿਭਾਗ </span><br/>
	             <span>(ਮੰਤਰੀ ਮੰਡਲ ਮਾਮਲੇ ਸ਼ਾਖਾ)</span><br/>
	             </div>";
                outXml += @"<p style='font-size: 12pt; text-indent: 42px;'>"
                 + HeaderlistofAgenda.Header +
" </p>";

                outXml += @"<p style = 'font-size: 12pt; text-indent: 42px;' > ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate + @" ਦਿਨ  " + HeaderlistofAgenda.Day + @"  ਨੂੰ ਸਵੇਰੇ " + HeaderlistofAgenda.StartTime + @" ਵਜੇ ਸਥਾਨ " + HeaderlistofAgenda.Venue + @" ਵਿਖੇ ਹੋਣ ਵਾਲੀ ਮੀਟਿੰਗ ਵਿੱਚ ਇਸ ਵਿਭਾਗ ਦੇ ਪਿੱਠ ਅੰਕਣ ਨੰ: …….., ਮਿਤੀ …… ਰਾਹੀਂ ਜਾਰੀ ਕੀਤੇ ਮੇਨ ਅਜੰਡੇ ਦੀ ਲਗਾਤਾਰਤਾ ਵਿੱਚ ਹੇਠ ਲਿਖੀਆਂ ਆਈਟਮਾਂ ਤੇ ਵੀ ਵਿਚਾਰ ਕੀਤਾ ਜਾਵੇਗਾ:-  
    </p>";


            }
            /*   "ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਮਿਤੀ ... ਦਿਨ …ਵਾਰ ਨੂੰ ਸਵੇਰੇ ..:00 ਵਜੇ, ਸਥਾਨ......... ਵਿਖੇ ਹੋਣ ਵਾਲੀ ਮੀਟਿੰਗ ਵਿੱਚ ਹੇਠ ਲਿਖੀਆਂ ਆਈਟਮਾਂ ਤੇ ਵਿਚਾਰ ਕੀਤਾ ਜਾਵੇਗਾ:-*/
            outXml += @"<table style='font-size: 12pt; border: 1px solid black;border-collapse: collapse;'>";
            outXml += @"<tr style = 'padding:5px;'>
             <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse; width:5%;'> ਮੀਟਿੰਗ ਦਾ ਆਈਟਮ ਨੰ.</th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;  width:10%;'> ਸਾਲਾਨਾ ਆਈਟਮ ਨੰ.</th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;'> ਵਿਸ਼ਾ </th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;width:15%;'> ਸਬੰਧਤ ਮੰਤਰੀ / ਸਕੱਤਰ </th>
       <th style = 'padding:10px; border: 1px solid black;border - collapse: collapse;width:15%;' > ਵਿਭਾਗ ਜਿਨ੍ਹਾਂ ਦੀ ਸਲਾਹ ਲਈ ਗਈ </th>";
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + mdl._LstReferences[i].SrNo + @"    
										</td>
	                                     <td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + mdl._LstReferences[i].yearno + @"    
										</td>
                                       
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>                                       
                     <td style = 'padding:10px; border: 1px solid black;border - collapse: collapse;'>   
                        " + mdl._LstReferences[i].MinisterName + @"  
                                        </td>
                        
                     <td style = 'padding:10px; border: 1px solid black;border - collapse: collapse;'>                                      
                                        </td></tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {
                        outXml += "</table>";
                        outXml += "<div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += "ਕੇ ਏ ਪੀ  ਸਿਨਹਾ";
                        outXml += "<br/>";
                        outXml += "ਮੁੱਖ ਸਕੱਤਰ, ਪੰਜਾਬ ਸਰਕਾਰ";
                        outXml += "</div>";

                        //if (HeaderlistofAgenda != null)
                        //{
                        //    outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                        //                 " + HeaderlistofAgenda.Footer + "</p>";
                        //}
                        outXml += "<div class='maindiv'>";
                        outXml += "<div style='float:left;width: 70%;'> ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>";
                        outXml += "<div style ='float:right;width: 30%;text-align: end;'> ਮਿਤੀ:" + gt.ToString() + @"</div>";
                        outXml += "</div>";

                        outXml += "<p style='font-size: 12pt; text-indent: 42px;'>";
                        outXml += @"ਉਤਾਰਾ ਹੇਠ ਲਿਖਿਆਂ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਸ਼ਾਮਲ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ:-</p>
	  <p style = 'font-size: 12pt; text-indent: 50px;'>
       <ol>
       <li>
       <span style='padding-left: 50px;'>ਮੁੱਖ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	   </li>
	   <li>
	   <span style = 'padding-left: 50px;' > ਸਮੂਹ ਕੈਬਨਿਟ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	   </li>
	   <li>
	   <span style = 'padding-left: 50px;' > ਵਿਸ਼ੇਸ਼ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ/ਮੁੱਖ ਮੰਤਰੀ</span>
	   </li></ol>
	   </p>
	   <div style='text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt= 'Signature' style= 'height:50px;'><br/>";
                        outXml += @" ਸੁਪਰਡੰਟ
            </div>
            <div class='maindiv'>
			<div style ='float:left;width: 70%;'> ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>
			<div style ='float:right;width: 30%;text-align: end;'> ਮਿਤੀ:" + gt.ToString() + @"</div>			
			</div>			
 <p style = 'font-size: 12pt; text-indent: 42px;'>
 ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਉਹ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕਰਨ। 
      </p>	  
	   <div style = 'text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += @" ਸੁਪਰਡੰਟ
            </div>
			<div class='maindiv'>
			<div style = 'float:left;width: 70%;' > ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>
			<div style = 'float:right;width: 30%;text-align: end;' > ਮਿਤੀ:" + gt.ToString() + @"</div>			
			</div>			
 <p style='font-size: 12pt; text-indent: 42px;'>
     ਉਤਾਰਾ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਵਿਸ਼ੇਸ਼ ਸਕੱਤਰ/ਮੁੱਖ ਸਕੱਤਰ ਨੂੰ ਮੁੱਖ ਸਕੱਤਰ ਪੰਜਾਬ ਜੀ ਦੀ ਸੂਚਨਾ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।	
      </p>
	   <div style ='text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += @" ਸੁਪਰਡੰਟ</div>
			<div class='maindiv'>
			<div style = 'float:left;width: 70%;' > ਪਿੱਠ ਅੰਕਣ ਨੰ:</div>
			<div style = 'float:right;width: 30%;text-align: end;' > ਮਿਤੀ: " + gt.ToString() + @"</div>			
			</div>			
 <p style = 'font-size: 12pt; text-indent: 42px;'>
     ਉਤਾਰਾ ਯਾਦ-ਪੱਤਰਾਂ ਦੀਆਂ ਕਾਪੀਆਂ ਸਮੇਤ ਵਧੀਕ ਮੁੱਖ ਸਕੱਤਰ/ਰਾਜਪਾਲ ਨੂੰ ਰਾਜਪਾਲ ਪੰਜਾਬ ਜੀ ਦੀ ਸੂਚਨਾ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।		
      </p>
  <div style ='text-align:right;padding-top:5px'>
            <img src='" + signature + "' alt='Signature' style='height:50px;'><br/>";
                        outXml += " ਸੁਪਰਡੰਟ</div>";
                    }
                }
                outXml += "</div></div></body> </html>";
            }
            return outXml;
        }



        [HttpGet]
        public JsonResult GetDecisionList(string agendaId, string viewtype)
        {
            try
            {
                if (agendaId != "")
                {
                    DataTable data = new DataTable();
                    DiaryViewModel dvm = new DiaryViewModel();
                    string DeptId = CurrentSession.DeptID;
                    data = dvm.GetDecisionList(Convert.ToInt32(agendaId), viewtype);
                    var dtt = dvm.GetTableRows(data);
                    //return Json(dtt, JsonRequestBehavior.AllowGet);

                    return new JsonResult()
                    {
                        Data = dtt,
                        MaxJsonLength = 86753090,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    //return Json(dtt, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }



            }
            catch (Exception)
            {
                throw;
            }
        }
        //[HttpPost]
        //public JsonResult SendSMS(List<string> mobileNumbers, string agendaId, string agendaPath, string DocumentType)
        //{
        //    // TODO: Send SMS using your service

        //    string MCName = "";
        //    string MeetingDate = "";
        //    string MeetingTitle = "";
        //    string DeptID = "";
        //    string OfficeName = "";
        //    string StartTime = "";


        //    // Retrieve approval data
        //    var approval = AgendaModelFunction.GetAgendaDetailsforApproval(Convert.ToInt16(agendaId)).FirstOrDefault();

        //    if (approval != null)
        //    {
        //        // Extract data from the single record
        //        MCName = approval.DepartmentName;
        //        MeetingDate = Convert.ToDateTime(approval.AgendaDate).ToString("dd/MM/yyyy");
        //        MeetingTitle = Convert.ToString(approval.AgendaName);
        //        DeptID = Convert.ToString(approval.DeptId);
        //        OfficeName = Convert.ToString(approval.OfficeName);
        //        StartTime = Convert.ToString(approval.StartTime);
        //    }

        //    foreach (var mobile in mobileNumbers)
        //    {
        //        if (DocumentType == "20")
        //        {
        //            string[] credentials = {OfficeName, MeetingDate};
        //            SMSMessage _smsService = new SMSMessage();
        //            Task<string> task = _smsService.SendLoginMessageMultipleVariables(mobile, credentials, "1407175221939482657");
        //        }
        //        if (DocumentType == "22")
        //        {
        //            string[] credentials = {MeetingDate};
        //            SMSMessage _smsService = new SMSMessage();
        //            Task<string> task = _smsService.SendLoginMessageMultipleVariables(mobile, credentials, "1407175221945202790");
        //        }
        //    }

        //    return Json(new { success = true, count = mobileNumbers.Count });
        //}

        [HttpPost]
        public JsonResult SendEmailFromAzureFile(List<string> emails, string agendaId, string agendaPath, string CoveringLetter, string DocumentType)
        {
            string azureFileUrl = agendaPath;

            string MCName = "";
            DateTime MeetingDate = DateTime.MinValue;
            string MeetingTitle = "";
            string DeptID = "";
            string OfficeName = "";
            string StartTime = "";

            // Retrieve approval data
            var approval = AgendaModelFunction.GetAgendaDetailsforApproval(Convert.ToInt16(agendaId)).FirstOrDefault();
            var _LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(Convert.ToInt16(agendaId));
            string firstResolutionNo = _LstReferences[0].LetterNo;
            string LastResolutionNo = _LstReferences[_LstReferences.Count - 1].LetterNo;

            if (approval != null)
            {
                // Extract data from the single record
                MCName = approval.DepartmentName;
                MeetingDate = Convert.ToDateTime(approval.AgendaDate);
                MeetingTitle = Convert.ToString(approval.AgendaName);
                DeptID = Convert.ToString(approval.DeptId);
                OfficeName = Convert.ToString(approval.OfficeName);
                StartTime = Convert.ToString(approval.StartTime);
            }

            string subject = "";
            string Day = MeetingDate.ToString("dddd");
            string body = "";
            if (DocumentType == "20")
            {
                subject= $"Agenda of the {OfficeName} meeting scheduled on {MeetingDate:dd/MM/yyyy}.";

                body = $@"<p>
                With reference to the above subject, it is requested that, as per the directions of the Honorable Chairperson, 
                the meeting has been scheduled on {Day}, {MeetingDate:dd/MM/yyyy} at {StartTime}, 
                at {MCName}. The agenda of the meeting is enclosed herewith. 
                You are kindly requested to ensure your timely presence at the meeting.
                </p>

                <p>Regards,<br/>
                Digital MC House, {MCName}</p>
                <p><strong>Note:</strong> This is a system-generated email. Please do not reply to this message.</p>
";
            }

            if (DocumentType == "22")
            {
                subject= $"Regarding Proceedings of the {OfficeName} meeting held on {MeetingDate:dd/MM/yyyy}. at {StartTime} ";

                body = $@"
                <p>
                Regarding the above subject, it is requested a copy of the proceedings (Resolution No. {firstResolutionNo} to {LastResolutionNo}) 
                of the meeting held on {Day}, {MeetingDate:dd/MM/yyyy} at {StartTime} 
                at {MCName} 
                is being sent to you for your information and further appropriate action.
                </p>

                

                <p>Regards,<br/>
                Digital MC House, {MCName}</p></html>
                <p><strong>Note: This is a system-generated email. Please do not reply to this message.</strong></p>
             ";
            }

            if (emails == null || !emails.Any() || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
            {
                return Json(new { success = false, message = "Missing required fields." });
            }

            //try
            //{
            //    string emailUser = WebConfigurationManager.AppSettings["emailuser"];
            //    string emailPwd = WebConfigurationManager.AppSettings["emailpwd"];
            //    string emailHost = WebConfigurationManager.AppSettings["emailhost"];
            //    int emailPort = Convert.ToInt32(WebConfigurationManager.AppSettings["emailport"]);
            //    bool enableSsl = Convert.ToBoolean(WebConfigurationManager.AppSettings["enablessl"]);

            //    using (MailMessage mail = new MailMessage())
            //    {
            //        mail.From = new MailAddress(emailUser);

            //        foreach (string email in emails)
            //        {
            //            if (!string.IsNullOrWhiteSpace(email))
            //                mail.To.Add(email.Trim());
            //        }

            //        mail.Subject = subject;
            //        mail.Body = body;
            //        mail.IsBodyHtml = true;
            //        mail.BodyEncoding = Encoding.UTF8;

            //        // ---------- Main Attachment ----------
            //        if (!string.IsNullOrWhiteSpace(azureFileUrl))
            //        {
            //            byte[] fileBytes = new WebClient().DownloadData(azureFileUrl);
            //            string fileName = Path.GetFileName(new Uri(azureFileUrl).LocalPath);
            //            mail.Attachments.Add(new Attachment(new MemoryStream(fileBytes), fileName));
            //        }

            //        // ---------- Covering Letter Attachments ----------
            //        if (!string.IsNullOrWhiteSpace(CoveringLetter))
            //        {
            //            foreach (var letter in CoveringLetter.Split(','))
            //            {
            //                string url = letter.Trim();
            //                if (string.IsNullOrEmpty(url)) continue;

            //                byte[] fileBytes = new WebClient().DownloadData(url);
            //                string fileName = Path.GetFileName(new Uri(url).LocalPath);
            //                mail.Attachments.Add(new Attachment(new MemoryStream(fileBytes), fileName));
            //            }
            //        }

            //        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //        using (SmtpClient smtp = new SmtpClient(emailHost, emailPort))
            //        {
            //            smtp.UseDefaultCredentials = false;
            //            smtp.Credentials = new NetworkCredential(emailUser, emailPwd);
            //            smtp.EnableSsl = enableSsl;
            //            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //            smtp.Send(mail);
            //        }
            //    }

            //    return Json(new { success = true, message = "Email sent successfully." });
            //}
            //catch (SmtpException smtpEx)
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        message = "SMTP Error: " + smtpEx.Message,
            //        detail = smtpEx.InnerException?.Message
            //    });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        message = "Error sending email.",
            //        detail = ex.Message
            //    });
            //}

            try
            {
                EmailService emailService =
                    new EmailService();


                EmailResult result =
                    emailService.SendEmail(
                        emails,
                        subject,
                        body,
                        azureFileUrl,
                        CoveringLetter,
                        User?.Identity?.Name,
                        Request.UserHostAddress);


                return Json(new
                {
                    success = result.Success,
                    status = result.Status,
                    message = result.Message,
                    trackingId = result.TrackingId,
                    recipient = result.ToEmail
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


        public JsonResult GetVistorList(string sessionid, string SessionDate, string gallaryId)
        {
            try
            {
                if (sessionid != "")
                {

                    return Json("0", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("0", JsonRequestBehavior.AllowGet);
                }



            }
            catch (Exception)
            {
                throw;
            }
        }

        public string VisitorPDFhtml(int agendaid)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();
            var path1 = BlobStorage.GetStorageAcessingPath();
            var aadhaarId = "cs@punjab.gov.in";
            var userList = CMNirdesh.Models.Login.GetDetailsByadhaarID(aadhaarId);

            // Initialize the signature variable
            string Chiefsignature = "";

            // Check if userList is not empty or null
            if (userList != null && userList.Count > 0)
            { // Use Count instead of length
                foreach (var user in userList)
                {
                    // Check if EmailId matches the provided aadhaarId (Email ID)
                    if (user.UserName == aadhaarId)
                    {
                        Chiefsignature = $"{path1}{user.SignaturePath}"; // Get the SignaturePath
                        break; // Exit the loop once you find the match
                    }
                }
            }

            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            string signature = $"{path1}{CurrentSession.SignaturePath}";

            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                <head><meta charset='UTF-8'><title>NextGen e-Cabinet: ApprovedAgendaReport</title>                                   
                                    <style>
                                .pdf-class .MsoNormal {
                                    text-wrap: wrap;
                                    text-align: justify; /* Justifying paragraphs */
                                }

                                .pdf-class .MsoNormalTable td {
                                    max-width: 65px !important;
                                    padding: 1px !important;
                                    overflow-wrap: break-word;
                                }

                                .pdf-class .MsoNormalTable {
                                    max-width: 700px !important;
                                    margin-left: 0 !important;
                                }

                                .pdf-class table.MsoNormalTable {
                                    max-width: 700px !important;
                                    margin-left: 0 !important;
                                }

                                .pdf-class .MsoTableGrid td {
                                    max-width: 65px !important;
                                    text-wrap: wrap;
                                }

                                .pdf-class .MsoTableGrid {
                                    max-width: 500px !important;
                                    margin-left: 0 !important;
                                }

                                .pdf-class table.MsoTableGrid {
                                    max-width: 500px !important;
                                    margin-left: 0 !important;
                                }

                                p.MsoListParagraphCxSpFirst,
                                p.MsoListParagraphCxSpMiddle,
                                p.MsoListParagraphCxSpLast {
                                    margin-left: 20px;
                                }

                                td {
                                    vertical-align: top;
                                }

                                @media print {
                                    .header {
                                        display: none;
                                    }
                                }

                                p {
                                    line-height: 1.7;
                                    text-align: justify; /* Justifying paragraphs */
                                }

                                table {
                                    width: 100%;
                                }

                                #bloc1 {
                                    float: left;
                                    width: 200px;
                                }

                                #bloc2 {
                                    text-align: center;
                                }
                                    </style>
                                </head> 
                             <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";

            string gt = DateTime.Now.ToString("dd/MM/yyyy");

            outXml += @"<div style='margin: 5px 5px;page-break-before: always;'>
                <div id='maindiv'>
                  <div id='bloc1' style='border:1px solid;border-radius:5px;padding:7px;'> 
                 ਮੀਟਿੰਗ ਦੀ ਮਿਤੀ: " + HeaderlistofAgenda.AgendaDate + @"<br>        	                 	    	                                                                
ਸਮਾਂ: " + HeaderlistofAgenda.StartTime + @" ਵਜੇ (ਸਵੇਰੇ/ਦੁਪਹਿਰ ਬਾਅਦ)<br>	         		 	 			                                                 
ਸਥਾਨ: " + HeaderlistofAgenda.Venue + @"         
                 </div>  
                 <div id='bloc2' style='padding:5px;'>
	             <span>ਪੰਜਾਬ ਸਰਕਾਰ </span><br/>
	             <span>ਆਮ ਰਾਜ ਪ੍ਰਬੰਧ ਵਿਭਾਗ </span><br/>
	             <span>(ਮੰਤਰੀ ਮੰਡਲ ਮਾਮਲੇ ਸ਼ਾਖਾ)</span><br/>
	             </div></div>
       <div style='margin:0px 40px'>
       <p style='font-size: 12pt; text-indent: 42px;'>ਮੰਤਰੀ ਪ੍ਰੀਸ਼ਦ ਦੀ ਅਗਲੀ ਮੀਟਿੰਗ ਮਿਤੀ " + HeaderlistofAgenda.AgendaDate + @" ਦਿਨ  " + HeaderlistofAgenda.Day + @"  ਨੂੰ ਸਵੇਰੇ/ਦੁਪਹਿਰ ਬਾਅਦ " + HeaderlistofAgenda.StartTime + @" ਵਜੇ ਸਥਾਨ " + HeaderlistofAgenda.Venue + @" ਵਿਖੇ ਹੋਵੇਗੀ। ਮੀਟਿੰਗ ਦਾ ਅਜੰਡਾ ਬਾਅਦ ਵਿੱਚ ਜਾਰੀ ਕੀਤਾ ਜਾਵੇਗਾ। 
       </p>
 

            <div style='text-align:right;padding-top:5px'>
			<img src='" + Chiefsignature + @"' alt='Signature' style='height:50px;'><br>
			ਕੇ ਏ ਪੀ  ਸਿਨਹਾ 
			<br>
			ਮੁੱਖ ਸਕੱਤਰ, ਪੰਜਾਬ ਸਰਕਾਰ					
			</div>

<p style='font-size: 12pt; text-indent: 42px;'>
 ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਹੇਠ ਲਿਖਿਆਂ ਨੂੰ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕ੍ਰਿਪਾਲਤਾ ਕੀਤੀ ਜਾਵੇ:-
              </p>
	             <p style='font-size: 12pt; text-indent: 50px;'>
	           <ol>
	           <li>
	           <span style='padding-left: 50px;'>ਮੁੱਖ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	           </li>
	           <li>
	           <span style='padding-left: 50px;'>ਸਮੂਹ ਕੈਬਨਿਟ ਮੰਤਰੀ ਪੰਜਾਬ</span>
	           </li>
	           <li>
	           <span style='padding-left: 50px;'>ਵਿਸ਼ੇਸ਼ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ/ਮੁੱਖ ਮੰਤਰੀ   </span>
	           </li>
	           </p>
	      <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + @"' alt='Signature' style='height:50px;'><br>
			ਸੁਪਰਡੰਟ						
			</div>
			
			<div class='maindiv'>
			<div style='float:left;width: 70%;'>ਪਅੰ.ਵਿ.ਪੱ.ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
			<div class='maindiv'>
			<div style='float:left;width: 70%;padding-top:5px;'>ਪੱ:ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;padding-top:5px;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
            <p style='font-size: 12pt; text-indent: 42px;'>
            ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਸਮੂਹ ਵਿਸ਼ੇਸ ਮੁੱਖ ਸਕੱਤਰ/ਵਧੀਕ ਮੁੱਖ ਸਕੱਤਰ/ਵਿੱਤੀ
            ਕਮਿਸ਼ਨਰ/ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ/ਸਕੱਤਰ, ਪੰਜਾਬ ਸਰਕਾਰ ਨੂੰ ਸੂਚਨਾ ਅਤੇ ਲੋੜੀਂਦੀ ਕਾਰਵਾਈ ਹਿੱਤ
            ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।
            </p>
	  
	   <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + @"' alt='Signature' style='height:50px;'><br>
			ਸੁਪਰਡੰਟ						
			</div>
	  <div class='maindiv'>
			<div style='float:left;width: 70%;padding-top:5px;'>ਪੱ:ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;padding-top:5px;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
       <p style='font-size: 12pt; text-indent: 42px;'>
        ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਪ੍ਰਮੁੱਖ ਸਕੱਤਰ, ਵਿੱਤ ਨੂੰ ਭੇਜਦੇ ਹੋਏ ਬੇਨਤੀ ਕੀਤੀ ਜਾਂਦੀ ਹੈ ਕਿ ਇਸ ਮੀਟਿੰਗ ਵਿੱਚ ਹਾਜ਼ਰ ਹੋਣ ਦੀ ਕਿਰਪਾਲਤਾ ਕੀਤੀ ਜਾਵੇ।
      </p>
	  
	  
	    <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + @"' alt='Signature' style='height:50px;'><br>
			ਸੁਪਰਡੰਟ						
			</div>
	  <div class='maindiv'>
			<div style='float:left;width: 70%;padding-top:5px;'>ਪੱ:ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;padding-top:5px;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
       <p style='font-size: 12pt; text-indent: 42px;'>
        ਉਪਰੋਕਤ ਦਾ ਇੱਕ ਉਤਾਰਾ ਵਿਸ਼ੇਸ਼ ਸਕੱਤਰ/ਮੁੱਖ ਸਕੱਤਰ ਨੂੰ ਮੁੱਖ ਸਕੱਤਰ ਪੰਜਾਬ ਜੀ ਦੀ ਸੂਚਨਾ ਹਿੱਤ ਭੇਜਿਆ ਜਾਂਦਾ ਹੈ।
      </p>
	  
	   <div style='text-align:right;padding-top:5px'>
			<img src='" + signature + @"' alt='Signature' style='height:50px;'><br>
			ਸੁਪਰਡੰਟ						
			</div>
	    <div class='maindiv'>
			<div style='float:left;width: 70%;padding-top:5px;'>ਪੱ:ਨੰ:</div>
			<div style='float:right;width: 30%;text-align: end;padding-top:5px;'>ਮਿਤੀ:" + gt + @"</div>
			
			</div>
                </div>";
            return outXml;
        }


        [HttpGet]
        public JsonResult GetCirculationContacts(string Department)
        {
            try
            {
                List<CirculationContactViewModel> contacts = DiaryDashboard.GetCirculationContacts(Department);
                return Json(contacts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateCoverLetter(int agendaid, string DocType)
        {
            string DeptId = CurrentSession.DeptID;
            var (coverHtml, pdfPath) = PDFCoverList.GetCoverHtmlByDeptId(DeptId, agendaid, DocType);
            var model = new PdfCoverLetterViewModel
            {
                DeptId = DeptId,
                AgendaId = agendaid,
                DocumentType = DocType,
                CoverHtml = coverHtml,
                ExistingPdfPath = pdfPath  // assuming your view model has this property
            };
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> SaveCoverLetterAsync(string DeptId, int AgendaID, string DocumentType, string CoverHtml)
        {
            string pdfUrl = await GenerateAndUploadCoverPdfAsync(CoverHtml, DeptId);
            bool success = PDFCoverList.InsertOrUpdateCoveringLetter(DeptId, AgendaID, DocumentType, CoverHtml, pdfUrl);
            return Json(new
            {
                success = success,
                message = success ? "Cover letter saved successfully!" : "An error occurred while saving."
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> PreviewCoverLetter(CoverLetterRequest reql)
        {
            byte[] pdfBytes;

            try
            {
                pdfBytes = await GenerateCoverPdfPreviewAsync(reql.CoverHtml);
            }
            catch (Exception ex)
            {
                // LOG the error (important)
                try
                {
                    System.IO.File.AppendAllText(
                        @"C:\temp\pdf_error.log",
                        DateTime.Now + " : " + ex.ToString() + Environment.NewLine
                    );
                }
                catch { }

                // 🔴 IMPORTANT: return a SAFE fallback PDF
                pdfBytes = GenerateFallbackPdf(
                    "Preview temporarily unavailable. Please try again."
                );
            }

            // ✅ ALWAYS return PDF
            return File(pdfBytes, "application/pdf");
        }

        private byte[] GenerateFallbackPdf(string message)
        {
            string pdf =
                        @"%PDF-1.4
                1 0 obj
                << /Type /Catalog /Pages 2 0 R >>
                endobj
                2 0 obj
                << /Type /Pages /Kids [3 0 R] /Count 1 >>
                endobj
                3 0 obj
                << /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792]
                   /Contents 4 0 R
                   /Resources << /Font << /F1 5 0 R >> >> >>
                endobj
                4 0 obj
                << /Length 60 >>
                stream
                BT /F1 12 Tf 72 720 Td (" + message + @") Tj ET
                endstream
                endobj
                5 0 obj
                << /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>
                endobj
                xref
                0 6
                0000000000 65535 f
                0000000010 00000 n
                0000000053 00000 n
                0000000102 00000 n
                0000000265 00000 n
                0000000360 00000 n
                trailer
                << /Size 6 /Root 1 0 R >>
                startxref
                440
                %%EOF";

            return System.Text.Encoding.ASCII.GetBytes(pdf);
        }

        public async Task<string> GenerateAndUploadCoverPdfAsync(string coverHtml, string deptId)
        {
            var pdfBytes = await GenerateCoverPdfAsync(coverHtml);
            string folder = "CoveringLetters/";
            //string blobName = folder + $"CoverLetter_{deptId}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";

            string blobName = folder + "CoverLetter_"
                  + deptId + "_"
                  + DateTime.UtcNow.ToString("yyyyMMddHHmmss")
                  + ".pdf";
            string path = BlobStorage.UploadApprovalsToBlob(pdfBytes, blobName);
            return path;
        }

        public async Task<byte[]> GenerateCoverPdfAsync(string coverHtml)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(coverHtml))
                    throw new ArgumentException("Cover HTML is empty.");

                string mcName = Convert.ToString(CurrentSession.MCName);
                string MCHeader = "Digital MC House " + mcName;

                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                    Headless = true,
                    Args = new[]
                    {
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-gpu",
                "--disable-dev-shm-usage"
            }
                }))
                {
                    using (var page = await browser.NewPageAsync())
                    {
                        string wrappedContent = "<div class='cover-content'>" + coverHtml + "</div>";

                        string htmlTemplate = @"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<style>
    body {
        font-family: Arial, sans-serif;
        font-size: 14px;
        line-height: 1.3;
        margin: 0;
        padding: 30px;
    }
    h1, h2, h3, h4, h5 { margin: 10px 0; }
    p { margin: 8px 0; }

    table {
        border-collapse: collapse;
        width: 100%;
    }
    th, td {
        border: 1px solid #ccc;
        padding: 6px;
    }
    img {
        max-width: 100%;
        height: auto;
    }
    .cover-content {
        width: 100%;
        word-wrap: break-word;
    }
</style>
</head>
<body>
{{CONTENT}}
</body>
</html>";

                        string fullHtml = htmlTemplate.Replace("{{CONTENT}}", wrappedContent);

                        await page.SetContentAsync(fullHtml, new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Load },
                            Timeout = 30000
                        });

                        // Ensure fonts fully loaded
                        await page.EvaluateExpressionAsync("document.fonts.ready");

                        var pdfOptions = new PdfOptions
                        {
                            Format = PaperFormat.A4,
                            PrintBackground = true,
                            DisplayHeaderFooter = true,
                            MarginOptions = new MarginOptions
                            {
                                Top = "60px",
                                Bottom = "60px",
                                Left = "40px",
                                Right = "40px"
                            },
                            HeaderTemplate =
                                "<div style='font-size:10px;width:100%;text-align:center;'></div>",

                            FooterTemplate =
                                "<div style='width:100%;font-size:11px;text-align:right;padding-right:20px;'>" +
                                MCHeader +
                                " | Page <span class='pageNumber'></span> of <span class='totalPages'></span>" +
                                "</div>"
                        };

                        return await page.PdfDataAsync(pdfOptions);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Error while generating cover letter PDF.",
                    ex);
            }
        }


        public async Task<byte[]> GenerateCoverPdfPreviewAsync(string coverHtml)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(coverHtml))
                    throw new ArgumentException("Cover HTML is empty.");

                string mcName = Convert.ToString(CurrentSession.MCName);
                string MCHeader = "Digital MC House " + mcName;

                // Launch browser directly
                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                    Headless = true,
                    Args = new[]
                    {
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-gpu",
                "--disable-dev-shm-usage"
            }
                }))
                {
                    using (var page = await browser.NewPageAsync())
                    {
                        // HTML template
                        string htmlTemplate = @"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<style>
    body {
        font-family: Arial, sans-serif;
        font-size: 14px;
        line-height: 1.3;
        margin: 0;
        padding: 30px;
    }
    table {
        border-collapse: collapse;
        width: 100%;
    }
    th, td {
        border: 1px solid #ccc;
        padding: 6px;
    }
    img {
        max-width: 100%;
        height: auto;
    }
    .watermark {
        position: fixed;
        top: 45%;
        left: 50%;
        transform: translate(-50%, -50%) rotate(-30deg);
        opacity: 0.08;
        font-size: 80px;
        font-weight: bold;
        color: #cc0000;
        pointer-events: none;
        white-space: nowrap;
    }
</style>
</head>
<body>
<div class='watermark'>PREVIEW</div>
<div class='cover-content'>
{{CONTENT}}
</div>
</body>
</html>";

                        string fullHtml = htmlTemplate.Replace("{{CONTENT}}", coverHtml);

                        await page.SetContentAsync(fullHtml, new NavigationOptions
                        {
                            WaitUntil = new[] { WaitUntilNavigation.Load },
                            Timeout = 30000
                        });

                        // Ensure fonts fully loaded
                        await page.EvaluateExpressionAsync("document.fonts.ready");

                        var pdfOptions = new PdfOptions
                        {
                            Format = PaperFormat.A4,
                            PrintBackground = true,
                            DisplayHeaderFooter = true,
                            MarginOptions = new MarginOptions
                            {
                                Top = "60px",
                                Bottom = "60px",
                                Left = "40px",
                                Right = "40px"
                            },
                            HeaderTemplate =
                                "<div style='font-size:10px;width:100%;text-align:center;'></div>",
                            FooterTemplate =
                                "<div style='width:100%;font-size:11px;text-align:right;padding-right:20px;'>" +
                                MCHeader +
                                " | Page <span class='pageNumber'></span> of <span class='totalPages'></span>" +
                                "</div>"
                        };

                        return await page.PdfDataAsync(pdfOptions);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Error while generating preview cover letter PDF.",
                    ex);
            }
        }


        public JsonResult GetUserSignature()
        {
            var path1 = BlobStorage.GetStorageAcessingPath();
            var signature = System.IO.Path.Combine(path1, CurrentSession.SignaturePath);
            return Json(new { url = signature }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendTestEmail()
        {
            return View();
        }



        //sms Implementation
      


        private async Task SendSmsApiAsync(string mobile,string message,string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://mseva.lgpunjab.gov.in/");

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);

                var formData = new FormUrlEncodedContent(new[]
                {
                 new System.Collections.Generic.KeyValuePair<string,string>("mobileNumber", mobile),
                 new System.Collections.Generic.KeyValuePair<string,string>("message", message),
                 new System.Collections.Generic.KeyValuePair<string,string>("tenantId","pb.punjab")
                });

                var response = await client.PostAsync("sms/send", formData);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }

        [HttpPost]
        public async Task<JsonResult> SendSMS( List<string> mobileNumbers,string agendaId, string agendaPath, string DocumentType)
        {
            if (mobileNumbers == null || mobileNumbers.Count == 0)
                return Json(new { success = false, message = "No mobile numbers"});

            // Fetch agenda details
            var approval = AgendaModelFunction
                .GetAgendaDetailsforApproval(Convert.ToInt16(agendaId))
                .FirstOrDefault();

            if (approval == null)
                return Json(new { success = false, message = "Agenda not found" });

            string meetingDate =
                Convert.ToDateTime(approval.AgendaDate).ToString("dd/MM/yyyy");

            string officeName = approval.OfficeName;

            // Decide template code & variables (NO TEMPLATE ID HERE)
            string templateCode;
            string[] variables;

            if (DocumentType == "20")   // Agenda circulated
            {
                templateCode = "eNigamAgenda";   // exists in your template table
                variables = new[] { officeName, meetingDate };
            }
            else if (DocumentType == "22") // Proceedings circulated
            {
                templateCode = "eNigamProceedings";
                variables = new[] { meetingDate };
            }
            else
            {
                return Json(new { success = false, message = "Invalid document type" });
            }

            int sent = 0, failed = 0;

            SmsOrchestrationService smsService = new SmsOrchestrationService();

            foreach (string mobile in mobileNumbers)
            {
                if (string.IsNullOrWhiteSpace(mobile))
                    continue;

                try
                {
                    await smsService.SendAgendaSmsAsync(
                        new List<string> { mobile },
                        templateCode,
                        variables
                    );

                    sent++;
                }
                catch
                {
                    failed++;
                }
            }

            return Json(new
            {
                success = true,
                total = mobileNumbers.Count,
                sent,
                failed
            });
        }

       
        [HttpPost]
        public ActionResult SaveManualContact(string MemberType, string MemberName, string Designation, string Mobile, string Email)
        {

            try
            {
                string DeptId = CurrentSession.DeptID;
                bool success = AgendaEdit.SaveCommitteeMember(DeptId,MemberType,MemberName,Designation,Mobile,
                    Email,CurrentSession.UserID);

                return Json(new
                {
                    success = true,
                    message = "Committee member saved successfully!"
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

        [HttpGet]
        public JsonResult CheckAllLinkedActionCode13(int agendaId)
        {
            bool result = AgendaEdit.CheckAllLinkedActionCode13(agendaId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckAllActLinked(int agendaId)
        {
            bool result = AgendaEdit.CheckAllLinkedAct(agendaId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}


