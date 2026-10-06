using Antlr.Runtime.Misc;
using Azure;
using BotDetect;
using CMNirdesh.Error;
using CMNirdesh.Extensions;
using CMNirdesh.Filters;
using CMNirdesh.Models;
using CMNirdesh.Models.Diaries;
using CMNirdesh.Models.Email;





//using CMNirdesh.Models.Email;

//using CMNirdesh.Models.Email;
using CMNirdesh.Models.PaperLaid;
using CMNirdesh.Models.Session;

//using DocumentFormat.OpenXml.Spreadsheet;
//using DocumentFormat.OpenXml.Wordprocessing;

//using DocumentFormat.OpenXml.Wordprocessing;

//using DocumentFormat.OpenXml.Wordprocessing;
using iText.Commons.Utils;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml.html;
using Microsoft.Extensions.Primitives;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using PVSWebSite.Models;
using Rotativa;
using Rotativa.Options;
using SelectPdf;
using Syncfusion.CompoundFile.DocIO.Native;
using Syncfusion.DocIO.DLS;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Lists;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WebGrease.Activities;
using static Azure.Core.HttpHeader;
using static iTextSharp.text.pdf.PdfDocument;
using static System.Net.Mime.MediaTypeNames;
using static System.Web.Razor.Parser.SyntaxConstants;
using ErrorLog = CMNirdesh.Error.ErrorLog;
using FundReportModel = CMNirdesh.Models.Diaries.FundReportModel;
using Login = CMNirdesh.Models.Login;

namespace CMNirdesh.Controllers
{
    [Audit]
    [NoCache]
    [CMNirdeshAuthorize(Allow = "Authenticated")]
    public class ReferencesController : Controller
    {
        HtmlToPdf converter = new HtmlToPdf();
        public ActionResult Index(string encrString, string type = null)
        {

            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            encrString = Request.QueryString["string"];


            //DataSet dashboardtype = UserModelFunction.Getdashbaordmenutype(type);



            if (encrString != null)
            {
                var statuss = hsFunc(encrString);
                if (statuss != "False")
                {
                    var newstatus = decrptFunc(statuss);
                    string email = newstatus.data.signature.email;
                    CurrentSession.UserID = newstatus.data.signature.user_id;
                    CurrentSession.UserName = newstatus.data.signature.fullName;
                    CurrentSession.ServiceSessionID = newstatus.data.signature.sessionId;
                    CurrentSession.browserId = newstatus.data.signature.browserId;
                    CurrentSession.localTokenId = newstatus.data.signature.localTokenId;
                    if (email != "")
                    {
                        mUsers user = new mUsers();
                        user.AadarId = email;
                        CurrentSession.AadharId = user.AadarId;


                        user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(email);

                        if (user.mUserList.Count == 0)
                        {
                            TempData["IsRegister"] = "User is not Rigistered with e-Nigam";
                            Session["LoggedIn"] = "no";
                            CurrentSession.ServiceSessionID = "";
                            return RedirectToAction("NotRegisterUser", "References");

                        }



                        if (user.mUserList != null && user.mUserList.Count > 0)
                        {
                            foreach (var list in user.mUserList)
                            {

                                Session["LoggedIn"] = "yes";
                                email = list.UserName;
                                FormsAuthentication.SetAuthCookie(list.UserName, true);
                                CurrentSession.UserID = list.UserId.ToString();
                                CurrentSession.UserName = list.UserName;
                                CurrentSession.SubUserTypeID = list.UserType.ToString();
                                CurrentSession.IsMember = list.IsMember;
                                CurrentSession.OfficeId = list.OfficeId.ToString();
                                CurrentSession.OfficeLevel = list.OfficeLevel;
                                CurrentSession.OfficeId = list.OfficeId.ToString();
                                CurrentSession.isApex = list.isApex.ToString();
                                CurrentSession.DeptID = list.DeptId.ToString();
                                CurrentSession.Name = list.Name.ToString();
                                CurrentSession.Designation = list.Designation;
                                CurrentSession.DeptName = list.DepartmentName.ToString();

                            }

                        }



                    }
                }
                else
                {
                    CurrentSession.ServiceSessionID = "";
                    return RedirectToAction("LogOff", "Account", new { @area = "" });
                }
            }

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;
                List<Dashboard> lstDashboard = new List<Dashboard>();
                LstDashboard model = new LstDashboard();

                try
                {
                    DataSet ds = new DataSet();





                    DiaryViewModel dvm = new DiaryViewModel();
                    DataTable dtinbox = new DataTable();
                    DataTable dtinboxfile = new DataTable();
                    DataTable dtoutbox = new DataTable();
                    DataTable dtoutboxfile = new DataTable();
                    DataTable dtarchive = new DataTable();
                    DataTable dtarchivefile = new DataTable();

                    //if (SiteName == "PunjabAssembly")
                    //{

                    //    dtinbox = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.OfficeId),type);
                    //    dtinboxfile = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.OfficeId), type);
                    //}

                    DataSet menuds = UserModelFunction.Getmenutype_dashboard(Convert.ToInt32(type), Convert.ToInt32(CurrentSession.SubUserTypeID));
                    if (menuds.Tables[0].Rows.Count > 0)
                    {
                        model.RefMenutype = Convert.ToInt32(menuds.Tables[0].Rows[0]["DocumentTypeId"].ToString());
                        model.RefMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["DocumentTypeName"].ToString());
                        model.RMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["MenuName"].ToString());
                        model.FileMenuType = Convert.ToInt32(menuds.Tables[1].Rows[0]["DocumentTypeId"].ToString());
                        model.dashboardtype = Convert.ToString(menuds.Tables[0].Rows[0]["MValue"].ToString());
                    }


                    dtinbox = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), model.RefMenutype.ToString());
                    dtinboxfile = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), model.FileMenuType.ToString());



                    dtoutbox = dvm.DispatchOutbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), model.RefMenutype.ToString());
                    dtoutboxfile = dvm.DispatchOutbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), model.FileMenuType.ToString());



                    dtarchive = dvm.DispatchArchive(Convert.ToString(CurrentSession.UserID), "Letter", model.RefMenutype.ToString());
                    dtarchivefile = dvm.DispatchArchive(Convert.ToString(CurrentSession.UserID), "File", model.FileMenuType.ToString());
                    model.InLetter = dtinbox.Rows.Count;
                    model.InFile = dtinboxfile.Rows.Count;
                    model.OutLetter = dtoutbox.Rows.Count;
                    model.OutFile = dtoutboxfile.Rows.Count;
                    model.ArchLetter = dtarchive.Rows.Count;
                    model.ArchFile = dtarchivefile.Rows.Count;
                    model.Type = type;
                    // model.MenuName = MenuName;


                    model.IsFileCount = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "F").Count();
                    string DeptId = CurrentSession.DeptID;
                    model.AgendaList = DiaryViewModel.GetMeetingFileList(CurrentSession.UserID.ToString(), CurrentSession.DeptID.ToString(), Convert.ToInt32(CurrentSession.OfficeId), model.FileMenuType, 0);
                    model.ButtonPermission = AgendaModelFunction.GetMenuButtonPermission(CurrentSession.UserID.ToString(), "44");
                    //model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "4");
                    return View("Dashboard", model);
                }

                catch (Exception ex)
                {

                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("Dashboard", model);
                }
            }

        }



        //public ActionResult Dashboard_Mobile(string encrString, string type = null, string devicetype = null,string email=null)



        //{

        //    string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
        //    encrString = Request.QueryString["string"];





        //        mUsers user = new mUsers();
        //        user.AadarId = "cs@punjab.gov.in";
        //        CurrentSession.AadharId = user.AadarId;


        //        user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(email);

        //        if (user.mUserList.Count == 0)
        //        {
        //            TempData["IsRegister"] = "User is not Rigistered with e-Nigam";
        //            Session["LoggedIn"] = "no";
        //            CurrentSession.ServiceSessionID = "";
        //            return RedirectToAction("NotRegisterUser", "References");

        //        }



        //        if (user.mUserList != null && user.mUserList.Count > 0)
        //        {
        //            foreach (var list in user.mUserList)
        //            {

        //                Session["LoggedIn"] = "yes";
        //                email = list.UserName;
        //                FormsAuthentication.SetAuthCookie(list.UserName, false);
        //                CurrentSession.UserID = list.UserId.ToString();
        //                CurrentSession.UserName = list.UserName;
        //                CurrentSession.SubUserTypeID = list.UserType.ToString();
        //                CurrentSession.IsMember = list.IsMember;
        //                CurrentSession.OfficeId = list.OfficeId.ToString();
        //                CurrentSession.OfficeLevel = list.OfficeLevel;
        //                CurrentSession.OfficeId = list.OfficeId.ToString();
        //                CurrentSession.isApex = list.isApex.ToString();
        //                CurrentSession.DeptID = list.DeptId.ToString();
        //                CurrentSession.Name = list.Name.ToString();
        //                CurrentSession.Designation = list.Designation;
        //                CurrentSession.DeptName = list.DepartmentName.ToString();


        //            }

        //        }







        //    if (string.IsNullOrEmpty(CurrentSession.UserID))
        //    {
        //        return RedirectToAction("Login", "Account", new { @area = "" });
        //    }
        //    else
        //    {
        //        Session["isApex"] = CurrentSession.isApex;
        //        List<Dashboard> lstDashboard = new List<Dashboard>();
        //        LstDashboard model = new LstDashboard();

        //        try
        //        {
        //            DataSet ds = new DataSet();





        //            DiaryViewModel dvm = new DiaryViewModel();
        //            DataTable dtinbox = new DataTable();
        //            DataTable dtinboxfile = new DataTable();
        //            DataTable dtoutbox = new DataTable();
        //            DataTable dtoutboxfile = new DataTable();
        //            DataTable dtarchive = new DataTable();
        //            DataTable dtarchivefile = new DataTable();

        //            //if (SiteName == "PunjabAssembly")
        //            //{

        //            //    dtinbox = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.OfficeId),type);
        //            //    dtinboxfile = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.OfficeId), type);
        //            //}

        //            DataSet menuds = UserModelFunction.Getmenutype_dashboard(Convert.ToInt32(type), Convert.ToInt32(CurrentSession.SubUserTypeID));

        //            DataSet homeDashboard = UserModelFunction.HomeDashboard(CurrentSession.DeptID.ToString(), CurrentSession.isDeptApex);
        //            model.Meetings = homeDashboard.Tables[0].Rows[0]["TotalFile"].ToString();
        //            model.Resolutions = homeDashboard.Tables[0].Rows[0]["TotalResolution"].ToString();
        //            model.Approved = homeDashboard.Tables[0].Rows[0]["Approved"].ToString();
        //            model.Rejected = homeDashboard.Tables[0].Rows[0]["Rejected"].ToString();
        //            model.Pending = homeDashboard.Tables[0].Rows[0]["Pending"].ToString();
        //            model.Deferred = homeDashboard.Tables[0].Rows[0]["Deferred"].ToString();






        //            if (menuds.Tables[0].Rows.Count > 0)
        //            {
        //                model.RefMenutype = Convert.ToInt32(menuds.Tables[0].Rows[0]["DocumentTypeId"].ToString());
        //                model.RefMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["DocumentTypeName"].ToString());
        //                model.RMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["MenuName"].ToString());
        //                model.FileMenuType = Convert.ToInt32(menuds.Tables[1].Rows[0]["DocumentTypeId"].ToString());
        //                model.dashboardtype = Convert.ToString(menuds.Tables[0].Rows[0]["MValue"].ToString());
        //            }


        //            dtinbox = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.OfficeId), model.RefMenutype.ToString());
        //            dtinboxfile = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.OfficeId), model.FileMenuType.ToString());



        //            dtoutbox = dvm.DispatchOutbox(Convert.ToString(CurrentSession.UserID), "Letter", model.RefMenutype.ToString());
        //            dtoutboxfile = dvm.DispatchOutbox(Convert.ToString(CurrentSession.UserID), "File", model.FileMenuType.ToString());



        //            dtarchive = dvm.DispatchArchive(Convert.ToString(CurrentSession.UserID), "Letter", model.RefMenutype.ToString());
        //            dtarchivefile = dvm.DispatchArchive(Convert.ToString(CurrentSession.UserID), "File", model.FileMenuType.ToString());
        //            model.InLetter = dtinbox.Rows.Count;
        //            model.InFile = dtinboxfile.Rows.Count;
        //            model.OutLetter = dtoutbox.Rows.Count;
        //            model.OutFile = dtoutboxfile.Rows.Count;
        //            model.ArchLetter = dtarchive.Rows.Count;
        //            model.ArchFile = dtarchivefile.Rows.Count;
        //            model.Type = type;
        //            model.Devicetype = devicetype;
        //            // model.MenuName = MenuName;

        //            CurrentSession.DeviceName = devicetype;

        //            model.IsFileCount = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "F").Count();
        //            string DeptId = CurrentSession.DeptID;
        //            model.AgendaList = DiaryViewModel.GetMeetingFileList(CurrentSession.UserID.ToString(), CurrentSession.DeptID.ToString(), Convert.ToInt32(CurrentSession.OfficeId), model.FileMenuType, 0);
        //            model.ButtonPermission = AgendaModelFunction.GetMenuButtonPermission(CurrentSession.UserID.ToString(), "44");
        //            //model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "4");
        //            return View("Dashboard", model);
        //        }

        //        catch (Exception ex)
        //        {

        //            ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
        //            return View("Dashboard", model);
        //        }
        //    }
        //}

        public ActionResult Dashboard_addmobile(string encrString, string type = null, string devicetype = null, string email = null)

        {

            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            encrString = Request.QueryString["string"];

            mUsers user = new mUsers();
            user.AadarId = email;
            CurrentSession.AadharId = user.AadarId;


            user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(email);

            if (user.mUserList.Count == 0)
            {
                TempData["IsRegister"] = "User is not Registered with e-Nigam";
                Session["LoggedIn"] = "no";
                CurrentSession.ServiceSessionID = "";
                return RedirectToAction("NotRegisterUser", "References");

            }



            if (user.mUserList != null && user.mUserList.Count > 0)
            {
                foreach (var list in user.mUserList)
                {

                    Session["LoggedIn"] = "yes";
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
                    CurrentSession.DeptID = list.DeptId.ToString();
                    CurrentSession.Name = list.Name.ToString();
                    CurrentSession.Designation = list.Designation;
                    CurrentSession.DeptName = list.DepartmentName.ToString();
                    CurrentSession.DesignationLocal = list.DesignationLocal;

                }

            }


            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;
                List<Dashboard> lstDashboard = new List<Dashboard>();
                LstDashboard model = new LstDashboard();

                try
                {
                    DataSet ds = new DataSet();





                    DiaryViewModel dvm = new DiaryViewModel();
                    DataTable dtinbox = new DataTable();
                    DataTable dtinboxfile = new DataTable();
                    DataTable dtoutbox = new DataTable();
                    DataTable dtoutboxfile = new DataTable();
                    DataTable dtarchive = new DataTable();
                    DataTable dtarchivefile = new DataTable();

                    //if (SiteName == "PunjabAssembly")
                    //{

                    //    dtinbox = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.OfficeId),type);
                    //    dtinboxfile = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.OfficeId), type);
                    //}

                    DataSet menuds = UserModelFunction.Getmenutype_dashboard(Convert.ToInt32(type), Convert.ToInt32(CurrentSession.SubUserTypeID));

                    DataSet homeDashboard = UserModelFunction.HomeDashboard(CurrentSession.DeptID.ToString(), CurrentSession.isDeptApex);
                    model.Meetings = homeDashboard.Tables[0].Rows[0]["TotalFile"].ToString();
                    model.Resolutions = homeDashboard.Tables[0].Rows[0]["TotalResolution"].ToString();
                    model.Approved = homeDashboard.Tables[0].Rows[0]["Approved"].ToString();
                    model.Rejected = homeDashboard.Tables[0].Rows[0]["Rejected"].ToString();
                    model.Pending = homeDashboard.Tables[0].Rows[0]["Pending"].ToString();
                    model.Deferred = homeDashboard.Tables[0].Rows[0]["Deferred"].ToString();






                    if (menuds.Tables[0].Rows.Count > 0)
                    {
                        model.RefMenutype = Convert.ToInt32(menuds.Tables[0].Rows[0]["DocumentTypeId"].ToString());
                        model.RefMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["DocumentTypeName"].ToString());
                        model.RMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["MenuName"].ToString());
                        model.FileMenuType = Convert.ToInt32(menuds.Tables[1].Rows[0]["DocumentTypeId"].ToString());
                        model.dashboardtype = Convert.ToString(menuds.Tables[0].Rows[0]["MValue"].ToString());
                    }


                    dtinbox = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), model.RefMenutype.ToString());
                    dtinboxfile = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), model.FileMenuType.ToString());



                    dtoutbox = dvm.DispatchOutbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), model.RefMenutype.ToString());
                    dtoutboxfile = dvm.DispatchOutbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), model.FileMenuType.ToString());



                    dtarchive = dvm.DispatchArchive(Convert.ToString(CurrentSession.UserID), "Letter", model.RefMenutype.ToString());
                    dtarchivefile = dvm.DispatchArchive(Convert.ToString(CurrentSession.UserID), "File", model.FileMenuType.ToString());
                    model.InLetter = dtinbox.Rows.Count;
                    model.InFile = dtinboxfile.Rows.Count;
                    model.OutLetter = dtoutbox.Rows.Count;
                    model.OutFile = dtoutboxfile.Rows.Count;
                    model.ArchLetter = dtarchive.Rows.Count;
                    model.ArchFile = dtarchivefile.Rows.Count;
                    model.Type = type;
                    model.Devicetype = devicetype;
                    // model.MenuName = MenuName;


                    model.IsFileCount = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "F").Count();
                    string DeptId = CurrentSession.DeptID;
                    model.AgendaList = DiaryViewModel.GetMeetingFileList(CurrentSession.UserID.ToString(), CurrentSession.DeptID.ToString(), Convert.ToInt32(CurrentSession.OfficeId), model.FileMenuType, 0);
                    model.ButtonPermission = AgendaModelFunction.GetMenuButtonPermission(CurrentSession.UserID.ToString(), "44");
                    //model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "4");
                    return View("Dashboard_addmobile", model);
                }

                catch (Exception ex)
                {

                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("Dashboard_addmobile", model);
                }
            }
        }

        public ActionResult Dashboard(string encrString, string type = null)
        {

            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            encrString = Request.QueryString["string"];
            //DataSet dashboardtype = UserModelFunction.Getdashbaordmenutype(type);

            if (encrString != null)
            {
                var statuss = hsFunc(encrString);
                if (statuss != "False")
                {
                    var newstatus = decrptFunc(statuss);
                    string email = newstatus.data.signature.email;
                    CurrentSession.UserID = newstatus.data.signature.user_id;
                    CurrentSession.UserName = newstatus.data.signature.fullName;
                    CurrentSession.ServiceSessionID = newstatus.data.signature.sessionId;
                    CurrentSession.browserId = newstatus.data.signature.browserId;
                    CurrentSession.localTokenId = newstatus.data.signature.localTokenId;
                    if (email != "")
                    {
                        mUsers user = new mUsers();
                        user.AadarId = email;
                        CurrentSession.AadharId = user.AadarId;


                        user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(email);

                        if (user.mUserList.Count == 0)
                        {
                            DiaryViewModel dvm = new DiaryViewModel();
                            var data = dvm.GetSwitchUser(email);
                            if (data.Rows.Count > 0)
                            {
                                string loginemail = data.Rows[0]["UserName"].ToString();
                                user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(loginemail);
                                CurrentSession.loginEmailID = email.ToString();
                            }
                            else
                            {
                                TempData["IsRegister"] = "User is not Rigistered with e-Nigam";
                                Session["LoggedIn"] = "no";
                                CurrentSession.ServiceSessionID = "";
                                return RedirectToAction("NotRegisterUser", "References");
                            }

                        }



                        if (user.mUserList != null && user.mUserList.Count > 0)
                        {
                            foreach (var list in user.mUserList)
                            {

                                Session["LoggedIn"] = "yes";
                                //email = list.UserName;
                                FormsAuthentication.SetAuthCookie(list.UserName, false);
                                CurrentSession.UserID = list.UserId.ToString();
                                CurrentSession.UserName = list.UserName;
                                CurrentSession.loginEmailID = email;
                                CurrentSession.SubUserTypeID = list.UserType.ToString();
                                CurrentSession.IsMember = list.IsMember;
                                CurrentSession.OfficeId = list.OfficeId.ToString();
                                CurrentSession.OfficeLevel = list.OfficeLevel;
                                CurrentSession.OfficeId = list.OfficeId.ToString();
                                CurrentSession.isApex = list.isApex.ToString();
                                CurrentSession.DeptID = list.DeptId.ToString();
                                CurrentSession.Name = list.Name.ToString();
                                CurrentSession.Designation = list.Designation;
                                CurrentSession.DeptName = list.DepartmentName.ToString();
                                CurrentSession.DesignationLocal = list.DesignationLocal;
                                CurrentSession.serviceName = ConfigurationManager.AppSettings["serviceName"];
                                CurrentSession.officeType = list.ofcType;
                                bool s = AuditTrailsExtensionMethods.FillAuditTrail();
                                CurrentSession.isApex = list.isApex.ToString();
                                CurrentSession.ApprovalAuthority = list.isApprovalAuthority.ToString();
                                CurrentSession.MapId = list.MapId.ToString();
                                CurrentSession.RCerificate = list.registeredCert;
                                CurrentSession.SignaturePath = list.SignaturePath;


                                Models.MenuModel _users = new Models.MenuModel();
                                String menuid = _users.MenuId.ToString();
                                return RedirectToAction("Dashboard", "References", new { @type = 0 });
                            }

                        }



                    }
                }
                else
                {
                    CurrentSession.ServiceSessionID = "";
                    return RedirectToAction("LogOff", "Account", new { @area = "" });
                }
            }

            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;
                List<Dashboard> lstDashboard = new List<Dashboard>();
                LstDashboard model = new LstDashboard();

                try
                {
                    DataSet ds = new DataSet();
                    DiaryViewModel dvm = new DiaryViewModel();



                    DataSet menuds = UserModelFunction.Getmenutype_dashboard(Convert.ToInt32(type), Convert.ToInt32(CurrentSession.SubUserTypeID));
                    model.Type = type;
                    

                    if (menuds.Tables[0].Rows.Count > 0)
                    {
                        model.RefMenutype = Convert.ToInt32(menuds.Tables[0].Rows[0]["DocumentTypeId"].ToString());
                        model.RefMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["DocumentTypeName"].ToString());
                        model.RMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["MenuName"].ToString());
                        model.FileMenuType = Convert.ToInt32(menuds.Tables[1].Rows[0]["DocumentTypeId"].ToString());
                        model.dashboardtype = Convert.ToString(menuds.Tables[0].Rows[0]["MValue"].ToString());
                    }

                    DataTable dtinboxoutboxcount = new DataTable();
                    dtinboxoutboxcount = dvm.DispatchInboxOutboxCount(Convert.ToString(CurrentSession.UserID), Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), model.RefMenutype.ToString(), model.FileMenuType.ToString());
                    if (dtinboxoutboxcount.Rows.Count > 0)
                    {
                        model.InFile = Convert.ToInt32(dtinboxoutboxcount.Rows[0]["InboxFile"]);
                        model.InLetter = Convert.ToInt32(dtinboxoutboxcount.Rows[0]["InboxRef"]);
                        model.OutFile = Convert.ToInt32(dtinboxoutboxcount.Rows[0]["OutboxFile"]);
                        model.OutLetter = Convert.ToInt32(dtinboxoutboxcount.Rows[0]["OutboxRef"]);
                        model.ArchFile = Convert.ToInt32(dtinboxoutboxcount.Rows[0]["ArchiveFile"]);
                        model.ArchLetter = Convert.ToInt32(dtinboxoutboxcount.Rows[0]["ArchiveRef"]);
                        model.IsFileCount = Convert.ToInt32(dtinboxoutboxcount.Rows[0]["IsFileCount"]);
                    }
                    else
                    {
                        model.InFile = 0;
                        model.InLetter = 0;
                        model.OutFile = 0;
                        model.OutLetter = 0;
                        model.ArchLetter = 0;
                        model.ArchFile = 0;
                        model.IsFileCount = 0;
                    }




                    //if (SiteName == "PunjabAssembly")
                    //{

                    //    dtinbox = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.OfficeId),type);
                    //    dtinboxfile = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.OfficeId), type);
                    //}
                    //DataSet homeDashboard = UserModelFunction.HomeDashboard(CurrentSession.DeptID.ToString(), CurrentSession.isDeptApex);
                    //model.Meetings = homeDashboard.Tables[0].Rows[0]["TotalFile"].ToString();
                    //model.Resolutions = homeDashboard.Tables[0].Rows[0]["TotalResolution"].ToString();
                    //model.Approved = homeDashboard.Tables[0].Rows[0]["Approved"].ToString();
                    //model.Rejected = homeDashboard.Tables[0].Rows[0]["Rejected"].ToString();
                    //model.Pending = homeDashboard.Tables[0].Rows[0]["Pending"].ToString();
                    //model.Deferred = homeDashboard.Tables[0].Rows[0]["Deferred"].ToString();

                    //DataTable dtinbox = new DataTable();
                    //DataTable dtinboxfile = new DataTable();
                    //DataTable dtoutbox = new DataTable();
                    //DataTable dtoutboxfile = new DataTable();
                    //DataTable dtarchive = new DataTable();
                    //DataTable dtarchivefile = new DataTable();
                    //dtinbox = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.MapId), model.RefMenutype.ToString());
                    //dtinboxfile = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.MapId), model.FileMenuType.ToString());

                    //dtoutbox = dvm.DispatchOutbox(Convert.ToString(CurrentSession.UserID), "Letter", Convert.ToInt32(CurrentSession.MapId), model.RefMenutype.ToString());
                    //dtoutboxfile = dvm.DispatchOutbox(Convert.ToString(CurrentSession.UserID), "File", Convert.ToInt32(CurrentSession.MapId), model.FileMenuType.ToString());

                    //dtarchive = dvm.DispatchArchive(Convert.ToString(CurrentSession.UserID), "Letter", model.RefMenutype.ToString());
                    //dtarchivefile = dvm.DispatchArchive(Convert.ToString(CurrentSession.UserID), "File", model.FileMenuType.ToString());

                    //model.InLetter = dtinbox.Rows.Count;
                    //model.InFile = dtinboxfile.Rows.Count;
                    //model.OutLetter = dtoutbox.Rows.Count;
                    //model.OutFile = dtoutboxfile.Rows.Count;
                    //model.ArchLetter = dtarchive.Rows.Count;
                    //model.ArchFile = dtarchivefile.Rows.Count;
                    //
                    // model.Devicetype = devicetype;
                    // model.MenuName = MenuName;


                    //model.IsFileCount = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "F").Count();
                    //string DeptId = CurrentSession.DeptID;
                    //model.AgendaList = DiaryViewModel.GetMeetingFileList(CurrentSession.UserID.ToString(), CurrentSession.DeptID.ToString(), Convert.ToInt32(CurrentSession.OfficeId), model.FileMenuType, 0);
                    //model.ButtonPermission = AgendaModelFunction.GetMenuButtonPermission(CurrentSession.UserID.ToString(), "44");
                    //model.AgendaList = AgendaModelFunction.GetAgendaSelectList(DeptId, "4");
                    return View("Dashboard", model);
                }

                catch (Exception ex)
                {

                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("Dashboard", model);
                }
            }
        }


        public ActionResult DiaryDetails()
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
                    ds = DiaryViewModel.GetDataByDocType(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID), "Letter");
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
                    return View("DiaryDetails", model);
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("DiaryDetails", model);
                }
            }
        }

        public ActionResult DiaryDetailsStatus()
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
                    ds = DiaryViewModel.GetDataByDocType(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID), "Letter");
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
                    return View("DiaryDetailsStatus", model);
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("DiaryDetailsStatus", model);
                }
            }
        }

        public ActionResult DiaryDetailsStatusType()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;

                List<Dashboard> lstDashboard = new List<Dashboard>();
                LstDashboard model = new LstDashboard();
                try
                {
                    DataSet ds = new DataSet();
                    ds = DiaryViewModel.GetDataByDocType(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID), "Letter");
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
                    return View("DiaryDetailsStatusType", model);
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("DiaryDetailsStatusType", model);
                }
            }
        }

        public ActionResult DiaryDetailsStatusTypeFile()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;

                List<Dashboard> lstDashboard = new List<Dashboard>();
                LstDashboard model = new LstDashboard();
                try
                {
                    DataSet ds = new DataSet();
                    ds = DiaryViewModel.GetDataByDocType(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID), "File");
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
                    return View("DiaryDetailsStatusTypeFile", model);
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("DiaryDetailsStatusTypeFile", model);
                }
            }
        }

        public ActionResult DispatchDetails()
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
                    return View("DispatchDetails", model);
                }

                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("DispatchDetails", model);
                }
            }
        }

        public ActionResult Inbox()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;
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
                    return View("Inbox", model);
                }

                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("DispatchDetailsStatusType", model);
                }
            }
        }

        public ActionResult Outbox()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;
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
                    return View("Outbox", model);
                }

                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("DispatchDetailsStatusType", model);
                }
            }
        }

        public ActionResult DispatchDetailsStatusType()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;
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
                    return View("DispatchDetailsStatusType", model);
                }

                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("DispatchDetailsStatusType", model);
                }
            }
        }

        public ActionResult DispatchDetailsStatusTypeFile()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;
                List<Dashboard> lstDashboard = new List<Dashboard>();
                LstDashboard model = new LstDashboard();
                try
                {
                    DataSet ds = new DataSet();
                    ds = DiaryViewModel.GetDataByDocTypeDispatch(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID), "File");
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
                    return View("DispatchDetailsStatusTypeFile", model);
                }

                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                    return View("DispatchDetailsStatusType", model);
                }
            }
        }

        public ActionResult WeeklyUpdations()
        {
            return View();
        }

        public ActionResult WeeklyUpdationsDispatch()
        {
            return View();
        }

        public ActionResult parichaylogin(String encrString, string status)
        {
            return View();
        }

        public ActionResult GetActionView(int SubMenuID, int AssemId, int SessId, int MainMenuID, int Count, int pageId = 1, int pageSize = 20)
        {
            try
            {
                if (MainMenuID == 83)
                {
                    if (SubMenuID == 1304)
                    {
                        return PartialView("/Views/References/_MlaDiaryForConstituency.cshtml");
                    }
                    if (SubMenuID == 1313)
                    {
                        return PartialView("/Views/References/_ViewMLADiary.cshtml");
                    }
                    if (SubMenuID == 1317)
                    {
                        return PartialView("/Views/References/_ViewMLADiary.cshtml");
                    }
                }
                else if (MainMenuID == 87)
                {
                    if (SubMenuID == 1320)
                    {
                        return PartialView("/Views/References/_ViewMLADiary.cshtml");
                    }
                    if (SubMenuID == 1313)
                    {
                        return PartialView("/Views/References/_ViewMLADiary.cshtml");
                    }
                    if (SubMenuID == 1321)
                    {
                        return PartialView("/Views/References/_ViewMLADiaryDispatch.cshtml");
                    }
                    if (SubMenuID == 1322)
                    {
                        return PartialView("/Views/References/_Analytics.cshtml");
                    }
                    if (SubMenuID == 1323)
                    {
                        return PartialView("/Views/References/_Reports.cshtml");
                    }
                    if (SubMenuID == 1324)
                    {
                        return PartialView("/Views/References/_SearchByRefId.cshtml");
                    }
                    if (SubMenuID == 1325)
                    {
                        return PartialView("/Views/References/_AdvanceSearch.cshtml");
                    }
                    if (SubMenuID == 1326)
                    {
                        return PartialView("/Views/References/_dashboardType.cshtml");
                    }
                    if (SubMenuID == 1327)
                    {
                        return PartialView("/Views/References/_WeeklyUpdations.cshtml");
                    }
                    if (SubMenuID == 1328)
                    {
                        return PartialView("/Views/References/_WeeklyUpdationsDispatch.cshtml");
                    }
                }

                else if (MainMenuID == 88)
                {
                    return PartialView("/Views/References/_TenderList.cshtml");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                throw ex;
            }
            return null;
        }

        public ActionResult NewMlaEntryForm(string reffiletype, string RefType, string FileType, string MenuName)
        {
            string isapex = CurrentSession.isApex;
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            MlaDiaryData model = new MlaDiaryData();
            try
            {
                model.mDepartmentList = DiaryViewModel.getDepartment();
                if (SiteName == "CMNirdesh")
                {
                    if (isapex == "0")
                    {
                        //model.mDepartmentListPbi = model.mDepartmentListPbi.Where(x => x.Value == "HPD0001").First();
                        //List<SelectListItem> _list = model.mDepartmentList.Where(x => x.Value == "HPD0001").ToList();
                        model.mDepartmentList = new SelectList(model.mDepartmentList.Where(x => x.Value == "HPD0001").ToList(), "Value", "Text");
                    }
                }
                else if (SiteName == "MC")
                {
                    if (isapex == "0")
                    {
                        model.mDepartmentList = new SelectList(model.mDepartmentList.Where(x => x.Value == CurrentSession.DeptID).ToList(), "Value", "Text");
                    }
                }

                else if (SiteName == "PunjabAssembly")
                {
                    if (isapex == "0")
                    {
                        model.mDepartmentList = DiaryViewModel.getDepartment();
                        //model.mDepartmentList = new SelectList(model.mDepartmentList.Where(x => x.Value == "HPD0001").ToList(), "Value", "Text");
                    }
                }


                model.RefFileType = reffiletype;
                model.TemplateList = DiaryViewModel.getTeamplateList(Convert.ToInt32(CurrentSession.OfficeId));
                model.OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                model.officename = CurrentSession.OfficeName;
                model.deptname = CurrentSession.DeptName;
                var a = CurrentSession.SPMinId;

                model.DeptId = CurrentSession.DeptID;
                model.ToDeptId = CurrentSession.DeptID;
                model.OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                model.ToOfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                //model.DocumentTypelist = DiaryViewModel.GetDocumentTypeList(CurrentSession.UserID.ToString());
                //  model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(1,"diary");


                model.DocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "R");
                if (RefType != "")
                {
                    model.DocumentTypelist = new SelectList(model.DocumentTypelist.Where(x => x.Value == RefType).ToList(), "Value", "Text");
                }

                model.FileDocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "F");




                if (FileType != "")
                {

                    model.FileDocumentTypelist = new SelectList(model.FileDocumentTypelist.Where(x => x.Value == FileType).ToList(), "Value", "Text");
                }




                //model.DocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "R");
                // model.FileDocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "F");
                model.ReferencePriorityTypeList = DiaryViewModel.GetPriorityTypeList();

                model.PaperEntryType = "NewEntry";
                model.BtnCaption = "Save";


            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
            }
            return PartialView("_MlaDiaryEntryForConstit", model);

        }

        private static string hsFunc(string encrString)
        {
            if (encrString == null)
            {
                return encrString;
            }

            else
            {
                string urlHs = "http://localhost:8085/handshake" + "/" + encrString + "/" + "eNigam";
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
                            Console.WriteLine(" statuscode is :" + responseTask.StatusCode);
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

        private static Getdata_api decrptFunc(string hs_resp)
        {
            string requestBody = $"{{\"EncryptedString\":\"{hs_resp}\"}}";
            string DEC_URL = "http://localhost:8085/decryption";
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

        public ActionResult NewMlaDispatchEntryForm(string reffiletype, string RefType, string FileType, string MenuName, string DashboardType)
        {
            MlaDiaryData model = new MlaDiaryData();
            string isapex = CurrentSession.isApex;
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            try
            {
                model.mDepartmentListPbi = DiaryViewModel.getDepartmentPbi();
                model.AssemblyList = DiaryViewModel.Assembly();
                model.SessionList = DiaryViewModel.Session();
                model.SessDatelist = DiaryViewModel.SessionDates();
                model.MemberList = DiaryViewModel.Members();
                model.MinisterList = DiaryViewModel.Minister();
                model.WardList = DiaryViewModel.WardList();
                string deptName = model.mDepartmentListPbi.Where(x => x.Value == CurrentSession.DeptID).Select(x => x.Text).FirstOrDefault();
                model.deptname = deptName;



                model.DocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "R");
                if (RefType != "")
                {
                    if (RefType == "1")
                    {
                        model.DocumentTypelist = new SelectList(model.DocumentTypelist.Where(x => x.Value == "1" || x.Value == "2" || x.Value == "3" || x.Value == "6").ToList().OrderBy(x => x.Value), "Value", "Text");
                    }
                    else
                    {
                        model.DocumentTypelist = new SelectList(model.DocumentTypelist.Where(x => x.Value == RefType).ToList(), "Value", "Text");
                    }

                }

                model.FileDocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "F");




                if (FileType != "")
                {

                    model.FileDocumentTypelist = new SelectList(model.FileDocumentTypelist.Where(x => x.Value == FileType).ToList(), "Value", "Text");
                }



                model.IsFileCount = model.FileDocumentTypelist.Count();

                if (SiteName == "CMNirdesh")
                {
                    if (isapex == "0")
                    {
                        //model.mDepartmentListPbi = model.mDepartmentListPbi.Where(x => x.Value == "HPD0001").First();
                        List<DepartmentSelectListItem> _list = model.mDepartmentListPbi.Where(x => x.Value == "HPD0001").ToList();
                        model.mDepartmentListPbi = _list;
                    }
                }
                else if (SiteName == "e-Cabinate")
                {
                    List<DepartmentSelectListItem> _list1 = model.mDepartmentListPbi.Where(x => x.Value == "72").ToList();
                    model.mDepartmentListPbi = _list1;


                }


                else if (SiteName == "MC")
                {

                    if (model.IsFileCount > 0 && RefType != "1")
                    {
                        List<DepartmentSelectListItem> _list = model.mDepartmentListPbi.Where(x => x.Value == CurrentSession.DeptID).ToList();
                        //List<DepartmentSelectListItem> _list = model.mDepartmentListPbi.Where(x=>x.Value == "LG00001").ToList();
                        model.mDepartmentListPbi = _list;
                    }
                    else
                    {
                        List<DepartmentSelectListItem> _list = model.mDepartmentListPbi.Where(x => x.Value == CurrentSession.DeptID).ToList();
                        model.mDepartmentListPbi = _list;
                    }
                }

                else if (SiteName == "PunjabAssembly")
                {
                    List<DepartmentSelectListItem> _list = model.mDepartmentListPbi.ToList();
                    model.mDepartmentListPbi = _list;
                }
                //model.OfficeList = DiaryViewModel.GetFromOfficeByUser(CurrentSession.DeptID, CurrentSession.UserID);
                if (RefType == "4")
                {
                    var officeList = DiaryViewModel.GetFromOfficeByUser(CurrentSession.DeptID, CurrentSession.UserID).Where(x => x.Value == CurrentSession.OfficeId.ToString()).ToList();
                    model.OfficeList = new SelectList(officeList, "Value", "Text");
                }
                else
                {
                    model.OfficeList = DiaryViewModel.GetFromOfficeByUser(CurrentSession.DeptID, CurrentSession.UserID);
                }
                model.TimeLine = DiaryViewModel.GetTimeline();
                model.DeparmentList = DiaryViewModel.getDepartmentPbi().Where(x => x.isAdvice == "1").ToList();
                model.CMApproval = DiaryViewModel.GetCMApprovalList();
                model.RefFileType = reffiletype;
                model.RefType = RefType;
                model.MenuName = MenuName;
                model.dashboardtype = DashboardType;
                model.TemplateList = DiaryViewModel.getTeamplateList(Convert.ToInt32(CurrentSession.OfficeId));
                model.ReferencePriorityTypeList = DiaryViewModel.GetPriorityTypeList();
                model.OfficeId = Convert.ToInt32(Session["officeid"]);
                model.officename = CurrentSession.OfficeNameNew;
                
                var a = CurrentSession.SPMinId;
                model.DeptId = CurrentSession.DeptID;
                model.FromDeptId = CurrentSession.DeptID;
                model.OfficeId = Convert.ToInt32(Session["officeid"]);
                model.FromOfficeId = Convert.ToInt32(Session["officeid"]);


                //model.MeetingList = DiaryViewModel.GetMeetingList(Convert.ToInt32(CurrentSession.OfficeId));
                // model.ActionTypeList = DiaryViewModel.GetAllActionTypeList();
                model.ActList = AgendaModelFunction.GetActList();
                model.PaperEntryType = "NewEntry";
                model.BtnCaption = "Save";

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
            }

            return PartialView("_MlaDiaryDispatchEntryForConstit", model);

        }

        public ActionResult OfficeDrpList(string deptid)
        {
            MlaDiaryData model = new MlaDiaryData();
            model.OfficeList = DiaryViewModel.GetOfficeByDept(deptid, CurrentSession.OfficeId);
            return PartialView("_OfficeDropDown", model);

        }

        public ActionResult ShowRefLink()
        {
            return PartialView("_LinkRefNo");
        }

        public ActionResult SearchRefLink(string RefId)
        {
            DiaryViewModel diaryModel = new DiaryViewModel();
            MlaDiaryData model = new MlaDiaryData();
            try
            {
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById((Int64)Convert.ToInt64(RefId), DeptId);
                if (diaryModel != null)
                {
                    model.ActionCode = diaryModel.ActionCode;
                    model.CreatedBy = diaryModel.CreatedBy;
                    model.LinkRefID = (Int64)diaryModel.LinkRefID;
                    model.FromDeptId = diaryModel.FromDeptId;
                    model.FromOfficeId = diaryModel.FromOfficeId;
                    model.DispatchNumber = diaryModel.DispatchNumber;
                    model.ToDeptId = diaryModel.ToDeptId;
                    model.ToOfficeId = diaryModel.ToOfficeId;
                    model.DiaryNumber = diaryModel.DiaryNumber;
                    model.DocmentType = diaryModel.DocmentType;
                    model.LetterNo = diaryModel.LetterNo;
                    model.isLetter = diaryModel.isLetter;
                    model.LetterDate = diaryModel.LetterDate;
                    model.Subject = diaryModel.Subject;
                    model.SubjectDetails = diaryModel.SubjectDetails;
                    model.EnclosureFile = diaryModel.EnclosureFile;
                    model.ApplicantName = diaryModel.ApplicantName;

                    model.ActiontakenDate = diaryModel.ActionTakenDate;
                    model.ApplicantAddress = diaryModel.ApplicantAddress;
                    model.ApplicantMobile = diaryModel.ApplicantMobile;
                    model.ApplicantEmail = diaryModel.ApplicantEmail;
                    model.DiaryDate = diaryModel.DiaryDate;
                    model.officename = diaryModel.FromOffice;
                    model.deptname = diaryModel.FromDept;
                    model.RefId = (Int64)diaryModel.RefId;
                    model.FromDept = diaryModel.FromDept.ToString();
                    model.FromOffice = diaryModel.FromOffice.ToString();
                    model.ToDept = diaryModel.ToDept.ToString();
                    model.ToOffice = diaryModel.ToOffice.ToString();

                    model.DocmentType = diaryModel.DocmentType.ToString();
                    model.DocumentTypeName = diaryModel.DocumentTypeName.ToString();
                    model.ToOffice = diaryModel.ToOffice.ToString();
                    model.CreatedDate = diaryModel.CreatedDate;
                    model.CurrentDate = diaryModel.CurrentDate;
                    model.DocumentTypelist = DiaryViewModel.GetDocumentTypeList(CurrentSession.UserID.ToString());
                    //  model.ActionTypeList = DiaryViewModel.GetAllActionTypeList((Int64)Convert.ToInt64(RefId),"diary");
                    model.mDepartmentList = DiaryViewModel.getDepartment();
                    model.OfficeList = DiaryViewModel.GetOfficeByDept(model.DeptId, CurrentSession.OfficeId);
                    model.WardList = DiaryViewModel.WardList();
                    model.WardListItems = DiaryViewModel.GetProposalWards(model.RefId.ToString());
                    if (model.WardListItems != null && model.WardListItems.Count > 0)
                    {
                        model.SelectedWardIds = model.WardListItems.Select(x => x.WardId).ToArray();
                    }
                    //model.PaperEntryType = "NewEntry";
                    //model.BtnCaption = "Save";
                    model.PaperEntryType = "EditEntry";
                    model.BtnCaption = "Update";
                    List<DiaryActionData> lstDiaryaction = new List<DiaryActionData>();

                    foreach (var item in diaryModel.lstDiaryActionViewModel)
                    {
                        DiaryActionData diaryActionData = new DiaryActionData();
                        diaryActionData.ActionAmountSanction = item.ActionAmount;
                        diaryActionData.ActionBy = item.ActionBy;
                        diaryActionData.ActionDate = item.ActionDate;
                        diaryActionData.ActionDescription = item.ActionDescription;
                        diaryActionData.RefId = (Int64)item.RefId;
                        diaryActionData.OfficeId = item.OfficeId;
                        diaryActionData.Id = item.Id;
                        diaryActionData.Enclosure = item.Enclosure;
                        diaryActionData.StatusName = item.StatusName;
                        lstDiaryaction.Add(diaryActionData);

                    }
                    model.ActionDetailsList = lstDiaryaction;
                    TempData["Diary"] = model;
                    return PartialView("_LinkRefNo", model);
                }
                else
                {
                    TempData["Diary"] = null;
                    return PartialView("_LinkRefNo", null);
                }

            }

            catch (Exception ex)
            {
                TempData["Diary"] = null;
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return PartialView("_LinkRefNo", null);
            }

        }

        public ActionResult LinkSearchRefLink(string RefId)
        {

            DiaryViewModel diaryModel = new DiaryViewModel();
            MlaDiaryData model = new MlaDiaryData();
            try
            {
                diaryModel = DiaryViewModel.GetDiaryByLinkRefId((Int64)Convert.ToInt64(RefId));

                if (diaryModel != null)
                {
                    model.ActionCode = diaryModel.ActionCode;
                    model.CreatedBy = diaryModel.CreatedBy;
                    model.LinkRefID = (int)diaryModel.LinkRefID;
                    model.FromDeptId = diaryModel.FromDeptId;
                    model.FromOfficeId = diaryModel.FromOfficeId;
                    model.DispatchNumber = diaryModel.DispatchNumber;
                    model.ToDeptId = diaryModel.ToDeptId;
                    model.ToOfficeId = diaryModel.ToOfficeId;
                    model.DiaryNumber = diaryModel.DiaryNumber;
                    model.DocmentType = diaryModel.DocmentType;
                    model.LetterNo = diaryModel.LetterNo;
                    model.LetterDate = diaryModel.LetterDate;
                    model.Subject = diaryModel.Subject;
                    model.SubjectDetails = diaryModel.SubjectDetails;
                    model.EnclosureFile = diaryModel.EnclosureFile;
                    model.ApplicantName = diaryModel.ApplicantName;
                    model.ActiontakenDate = diaryModel.ActionTakenDate;
                    model.ApplicantAddress = diaryModel.ApplicantAddress;
                    model.ApplicantMobile = diaryModel.ApplicantMobile;
                    model.ApplicantEmail = diaryModel.ApplicantEmail;
                    model.DiaryDate = diaryModel.DiaryDate;
                    model.officename = diaryModel.FromOffice;
                    model.deptname = diaryModel.FromDept;
                    model.RefId = (Int64)diaryModel.RefId;
                    model.FromDept = diaryModel.FromDept.ToString();
                    model.CurrentDate = diaryModel.CurrentDate;
                    model.FromOffice = diaryModel.FromOffice.ToString();

                    model.ToDept = diaryModel.ToDept.ToString();

                    model.ToOffice = diaryModel.ToOffice.ToString();

                    model.DocmentType = diaryModel.DocmentType.ToString();
                    model.DocumentTypeName = diaryModel.DocumentTypeName.ToString();
                    model.ToOffice = diaryModel.ToOffice.ToString();
                    model.CreatedDate = diaryModel.CreatedDate;

                    model.DocumentTypelist = DiaryViewModel.GetDocumentTypeList(CurrentSession.UserID.ToString());
                    //   model.ActionTypeList = DiaryViewModel.GetAllActionTypeList();
                    model.mDepartmentList = DiaryViewModel.getDepartment();
                    model.OfficeList = DiaryViewModel.GetOfficeByDept(model.DeptId, CurrentSession.OfficeId);
                    model.WardList = DiaryViewModel.WardList();
                    model.WardListItems = DiaryViewModel.GetProposalWards(model.RefId.ToString());
                    if (model.WardListItems != null && model.WardListItems.Count > 0)
                    {
                        model.SelectedWardIds = model.WardListItems.Select(x => x.WardId).ToArray();
                    }
                    model.PaperEntryType = "NewEntry";
                    model.BtnCaption = "Save";
                    model.PaperEntryType = "EditEntry";
                    model.BtnCaption = "Update";
                    List<DiaryActionData> lstDiaryaction = new List<DiaryActionData>();


                    foreach (var item in diaryModel.lstDiaryActionViewModel)
                    {
                        DiaryActionData diaryActionData = new DiaryActionData();
                        diaryActionData.ActionAmountSanction = item.ActionAmount;
                        diaryActionData.ActionBy = item.ActionBy;
                        diaryActionData.ActionDate = item.ActionDate;
                        diaryActionData.ActionDescription = item.ActionDescription;
                        diaryActionData.RefId = (Int64)item.RefId;
                        diaryActionData.OfficeId = item.OfficeId;
                        diaryActionData.Id = item.Id;
                        diaryActionData.Enclosure = item.Enclosure;
                        diaryActionData.StatusName = item.StatusName;
                        lstDiaryaction.Add(diaryActionData);

                    }
                    model.ActionDetailsList = lstDiaryaction;


                    return PartialView("_SearchLinkRefId", model);
                }
                else
                {

                    return PartialView("_SearchLinkRefId", null);

                }

            }

            catch (Exception ex)
            {

                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return PartialView("_SearchLinkRefId", null);
            }


        }

        public ActionResult OfficeDrpListTo(string deptid)
        {
            MlaDiaryData model = new MlaDiaryData();
            model.OfficeList = DiaryViewModel.GetOfficeByDept(deptid, CurrentSession.OfficeId);
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];

            if (SiteName == "MC")
            {
                if (deptid == "LG00001")
                {
                    model.OfficeList = new SelectList(model.OfficeList.Where(x => x.Value == "180").ToList(), "Value", "Text");
                }
                else
                {
                    //  Add "Commissioner Office" by Joginder

                    //model.OfficeList = new SelectList(model.OfficeList.Where(x => x.Text.Trim().Contains("House") || x.Text.Trim().Contains("FNCC") ||
                    //x.Text.Trim().Contains("Commissioner Office") || x.Text.Trim().Contains("Mayor Office")).ToList() , "Value", "Text");

                    var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                    {
                        "House",
                        "FNCC",
                        "Commissioner Office",
                        "Mayor Office"
                    };

                    model.OfficeList = new SelectList(
                        model.OfficeList
                            .Where(x => !string.IsNullOrWhiteSpace(x.Text) &&
                                        allowed.Contains(x.Text.Trim()))
                            .ToList(),
                        "Value",
                        "Text"
                    );
                }
            }
            else
            {
                model.OfficeList = new SelectList(model.OfficeList.ToList(), "Value", "Text");
            }


            return PartialView("_OfficeDropDownTo", model);
        }

        public ActionResult BindDiaryByAction(string RefId)
        {
            List<DiaryActionData> lstDiaryaction = new List<DiaryActionData>();
            MlaDiaryData model = new MlaDiaryData();
            DiaryViewModel diaryModel = new DiaryViewModel();
            string DeptId = CurrentSession.DeptID;
            diaryModel = DiaryViewModel.GetDiaryById((Int64)Convert.ToInt64(RefId), DeptId);
            foreach (var item in diaryModel.lstDiaryActionViewModel)
            {
                DiaryActionData diaryActionData = new DiaryActionData();
                diaryActionData.ActionAmountSanction = item.ActionAmount;
                diaryActionData.ActionBy = item.ActionBy;
                diaryActionData.ActionDate = item.ActionDate;
                diaryActionData.ActionDescription = item.ActionDescription;
                diaryActionData.RefId = (Int64)item.RefId;
                diaryActionData.OfficeId = item.OfficeId;
                diaryActionData.Id = item.Id;
                diaryActionData.Enclosure = item.Enclosure;
                diaryActionData.StatusName = item.StatusName;
                lstDiaryaction.Add(diaryActionData);

            }

            return Json(lstDiaryaction, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveUpdateMlaDiary(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {
                string res = "0";

                if (model.BtnCaption == "Draft")
                {
                    model.ActionDescription = "<p>REFERENCE ENTERED</p>";
                    if (model.isLetter == "Letter")
                    {
                        model.ActionCode = 11;
                    }
                    else
                    {
                        model.ActionCode = 12;
                    }

                }

                if (model.BtnCaption == "Save")
                {
                    model.ActionDescription = "<p>REFERENCE ENTERED</p>";
                    model.ActionCode = 0;
                }



                if (model.BtnCaption == "Update")
                {
                }



                //if (model.BtnCaption == "Save")
                //{
                //    model.ActionDescription = "<p>REFERENCE ENTERED</p>";
                //    model.ActionCode = 0;

                //}
                //if (model.BtnCaption == "Update")
                //{

                //}
                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                //var FileSettings = CMNirdesh.Models.FileSetting.GetDISFileSetting();

                if (model.EnclosureFile != null)
                {

                    if (model.IsEnclosureFile == "Y")
                    {
                        model.EnclosureFile = model.EnclosureFile;
                    }
                    else
                    {
                        ErrorLog.WriteToLog("2");
                        string urlEnclosure = "/References/";
                        //string urlEnclosure = "/MlaDiary/EnclosureFile";
                        string directoryEnclosure = Server.MapPath(urlEnclosure);



                        if (!System.IO.Directory.Exists(directoryEnclosure))
                        {
                            System.IO.Directory.CreateDirectory(directoryEnclosure);
                        }

                        string TempurlEnclosure1 = "~/TempFile";
                        string directoryEnclosure1 = Server.MapPath(TempurlEnclosure1);

                        if (!System.IO.Directory.Exists(directoryEnclosure1))
                        {
                            System.IO.Directory.CreateDirectory(directoryEnclosure1);
                        }

                        if (Directory.Exists(directoryEnclosure))
                        {
                            string[] savedFileName = Directory.GetFiles(Server.MapPath(TempurlEnclosure1));
                            if (savedFileName.Length > 0)
                            {
                                ErrorLog.WriteToLog("3");
                                string SourceFileold = savedFileName[0];
                                string SourceFile = System.IO.Path.Combine(directoryEnclosure1, model.EnclosureFile);
                                foreach (string page in savedFileName)
                                {
                                    // Guid FileName = Guid.NewGuid();
                                    string ext = Path.GetExtension(SourceFile);
                                    string name = Path.GetFileName(page);
                                    if (name == model.EnclosureFile)
                                    {
                                        var NFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("ddyymm") + ext;
                                        string path1 = System.IO.Path.Combine(directoryEnclosure, NFileName);
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            System.IO.File.Copy(SourceFile, path1, true);
                                            model.EnclosureFile = urlEnclosure + NFileName;
                                            System.IO.File.Delete(SourceFile);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                model.EnclosureFile = null;
                            }
                        }
                        else
                        {
                            model.EnclosureFile = null;
                        }
                    }
                }

                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.Subject = model.Subject;
                // diaryModel.ReferencePriorityType = model.ReferencePriorityType;
                diaryModel.isLetter = model.isLetter;
                if (diaryModel.isLetter == "Letter")
                {
                    //diaryModel.MeetingId = 0;
                    diaryModel.DocmentType = model.DocmentType;
                }
                else
                {
                    //diaryModel.MeetingId = 0;
                    diaryModel.DocmentType = model.MeetingId.ToString();
                    //diaryModel.MeetingId = model.MeetingId;
                    ///diaryModel.DocmentType = "0";
                }
                diaryModel.ApplicantAddress = model.ApplicantAddress;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                //diaryModel.DocmentType = model.DocmentType;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                diaryModel.ActionCode = model.ActionCode;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                diaryModel.ActionDescription = model.ActionDescription;
                diaryModel.ActionTakenDate = model.ActiontakenDate;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                res = DiaryViewModel.SaveDiary(diaryModel);

                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveUpdateMlaDispatch(MlaDiaryData model)
        {
            try
            {
                string res = "0";
                DiaryViewModel diaryModel = new DiaryViewModel();
                if (model.BtnCaption == "Draft")
                {
                    model.ActionDescription = "<p>REFERENCE ENTERED</p>";
                    if (model.isLetter == "Letter")
                    {
                        model.ActionCode = 11;
                    }
                    else
                    {
                        model.ActionCode = 20;
                    }

                }

                if (model.BtnCaption == "Save")
                {
                    model.ActionDescription = "<p>REFERENCE ENTERED</p>";
                    model.ActionCode = 0;
                }



                if (model.BtnCaption == "Update")
                {
                }

                //StatementFile
                if (model.StatementImplementation != null)
                {
                    //List<MultifileJson> FileTable = new List<MultifileJson>();
                    //FileTable = new JavaScriptSerializer().Deserialize<List<MultifileJson>>(model.StatementImplementation);

                    ErrorLog.WriteToLog("2");
                    string urlEnclosure = "/References/";
                    //string urlEnclosure = "/MlaDiary/EnclosureFile";
                    // string directoryEnclosure = FileSettings.SettingValue + urlEnclosure;
                    string directoryEnclosure = Server.MapPath(urlEnclosure);


                    if (!System.IO.Directory.Exists(directoryEnclosure))
                    {
                        System.IO.Directory.CreateDirectory(directoryEnclosure);
                    }
                    // int fileIDEnclosure = GetFileRandomNo();

                    string TempurlEnclosure1 = "~/TempFile";
                    string directoryEnclosure1 = Server.MapPath(TempurlEnclosure1);



                    if (!System.IO.Directory.Exists(directoryEnclosure1))
                    {
                        System.IO.Directory.CreateDirectory(directoryEnclosure1);
                    }

                    if (Directory.Exists(directoryEnclosure))
                    {
                        string FileName = model.StatementImplementation;
                        string SourceFile = System.IO.Path.Combine(directoryEnclosure1, model.StatementImplementation);
                        string ext = Path.GetExtension(SourceFile);
                        var NFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("ddyymm") + ext;
                        string path1 = System.IO.Path.Combine(directoryEnclosure, NFileName);
                        //System.IO.File.Copy(SourceFile, path1, true);
                        BlobStorage.CopyPdfLocalToBlob(path1, "", SourceFile);
                        System.IO.File.Delete(SourceFile);
                        diaryModel.EnclosureFile = urlEnclosure + NFileName;
                    }
                    else
                    {
                        diaryModel.EnclosureFile = null;
                    }

                }

                //StatementFilePB
                if (model.StatementImplementation_local != null)
                {
                    //List<MultifileJson> FileTable = new List<MultifileJson>();
                    //FileTable = new JavaScriptSerializer().Deserialize<List<MultifileJson>>(model.StatementImplementation);

                    ErrorLog.WriteToLog("2");
                    string urlEnclosure = "/References/";
                    //string urlEnclosure = "/MlaDiary/EnclosureFile";
                    // string directoryEnclosure = FileSettings.SettingValue + urlEnclosure;
                    string directoryEnclosure = Server.MapPath(urlEnclosure);


                    if (!System.IO.Directory.Exists(directoryEnclosure))
                    {
                        System.IO.Directory.CreateDirectory(directoryEnclosure);
                    }
                    // int fileIDEnclosure = GetFileRandomNo();

                    string TempurlEnclosure1 = "~/TempFile";
                    string directoryEnclosure1 = Server.MapPath(TempurlEnclosure1);



                    if (!System.IO.Directory.Exists(directoryEnclosure1))
                    {
                        System.IO.Directory.CreateDirectory(directoryEnclosure1);
                    }

                    if (Directory.Exists(directoryEnclosure))
                    {
                        string FileName = model.StatementImplementation_local;
                        string SourceFile = System.IO.Path.Combine(directoryEnclosure1, model.StatementImplementation_local);
                        string ext = Path.GetExtension(SourceFile);
                        var NFileName = Guid.NewGuid().ToString() + DateTime.Now.ToString("ddyymm") + ext;

                        string path1 = System.IO.Path.Combine(directoryEnclosure, NFileName);
                        //System.IO.File.Copy(SourceFile, path1, true);
                        BlobStorage.CopyPdfLocalToBlob(path1, "", SourceFile);
                        System.IO.File.Delete(SourceFile);
                        diaryModel.StatementImplementation_local = urlEnclosure + NFileName;
                    }
                    else
                    {
                        diaryModel.StatementImplementation_local = null;
                    }

                }


                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                // var FileSettings = FileSetting.GetDISFileSetting();



                //Delete the temp files if exists

                diaryModel.Subject = model.Subject;

                diaryModel.ApplicantAddress = model.ApplicantAddress;
                // diaryModel.ReferencePriorityType = model.ReferencePriorityType;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;

                diaryModel.isLetter = model.isLetter;
                if (diaryModel.isLetter == "Letter")
                {
                    diaryModel.MeetingId = 0;
                    diaryModel.DocmentType = model.DocmentType;
                }
                else
                {
                    diaryModel.MeetingId = 0;
                    diaryModel.DocmentType = model.MeetingId.ToString();
                    //diaryModel.MeetingId = model.MeetingId;
                    ///diaryModel.DocmentType = "0";
                }
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;


                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.SubjectPB = model.SubjectPB;
                diaryModel.SubjectDetailsPB = model.SubjectDetailsPB;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                diaryModel.ActionCode = model.ActionCode;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                diaryModel.ActionDescription = model.ActionDescription;
                diaryModel.ActionTakenDate = model.ActiontakenDate;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.ActId = model.ActId;
                diaryModel.IsSup = model.IsSup;
                diaryModel.PressNoteENG = model.PressNoteENG;
                diaryModel.PressNotePun = model.PressNotePun;
                diaryModel.TimelineId = model.TimelineId;
                diaryModel.TimelineRemarks = model.TimelineRemarks;
                diaryModel.DepartmentListIds = model.DepartmentListIds;
                diaryModel.ApprovalListIds = model.ApprovalListIds;
                diaryModel.ProposalAmount = Convert.ToDecimal(model.ProposalAmount);

                diaryModel.SentFromOfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                //diaryModel.EnclosureFile = model.StatementImplementation;
                //diaryModel.StatementImplementation_local = model.StatementImplementation_local;
                res = DiaryViewModel.SaveDispatch(diaryModel);
                if (res != "0")
                {
                    Int64 refid = Convert.ToInt64(res);

                    if (diaryModel.DepartmentListIds != null)
                    {
                        foreach (var item in diaryModel.DepartmentListIds)
                        {
                            int deptid = Convert.ToInt32(item);
                            DiaryViewModel.InsertConcernedDeptId(refid, deptid);

                        }
                    }

                    if (diaryModel.ApprovalListIds != null)
                    {
                        foreach (var item in diaryModel.ApprovalListIds)
                        {
                            int approvalid = Convert.ToInt32(item);
                            DiaryViewModel.InsertCMApproval(refid, approvalid);

                        }
                    }

                    // Process and Save Ward Mappings to detail table
                    if ((model.WardListItems == null || model.WardListItems.Count == 0) && !string.IsNullOrEmpty(model.WardListItemsJson))
                    {
                        try
                        {
                            model.WardListItems = new JavaScriptSerializer().Deserialize<List<ProposalWardModel>>(model.WardListItemsJson);
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.WriteToLog(ex, "Deserializing WardListItemsJson in SaveUpdateMlaDispatch");
                        }
                    }

                    // Fallback: If WardListItems not directly supplied but SelectedWardIds was selected
                    if ((model.WardListItems == null || model.WardListItems.Count == 0) && model.SelectedWardIds != null && model.SelectedWardIds.Length > 0)
                    {
                        var allWards = DiaryViewModel.WardList();
                        model.WardListItems = new List<ProposalWardModel>();
                        foreach (var wid in model.SelectedWardIds)
                        {
                            var match = allWards.FirstOrDefault(w => w.Value == wid.ToString());
                            model.WardListItems.Add(new ProposalWardModel
                            {
                                WardId = wid,
                                WardName = match != null ? match.Text : ("Ward " + wid)
                            });
                        }
                    }

                    if (model.WardListItems != null && model.WardListItems.Count > 0)
                    {
                        DiaryViewModel.SaveProposalWards(refid.ToString(), model.WardListItems, CurrentSession.Name);
                    }

                    if (model.EnclosureFile != null)
                    {
                        //upload enclosure file 
                        if (model.IsEnclosureFile == "Y")
                        {
                            model.EnclosureFile = model.EnclosureFile;
                        }
                        else
                        {

                            List<MultifileJson> FileTable = new List<MultifileJson>();
                            FileTable = new JavaScriptSerializer().Deserialize<List<MultifileJson>>(model.EnclosureFile);

                            ErrorLog.WriteToLog("2");
                            string urlEnclosure = "/References/";
                            //string urlEnclosure = "/MlaDiary/EnclosureFile";
                            // string directoryEnclosure = FileSettings.SettingValue + urlEnclosure;
                            string directoryEnclosure = Server.MapPath(urlEnclosure);


                            if (!System.IO.Directory.Exists(directoryEnclosure))
                            {
                                System.IO.Directory.CreateDirectory(directoryEnclosure);
                            }
                            // int fileIDEnclosure = GetFileRandomNo();

                            string TempurlEnclosure1 = "~/TempFile";
                            string directoryEnclosure1 = Server.MapPath(TempurlEnclosure1);



                            if (!System.IO.Directory.Exists(directoryEnclosure1))
                            {
                                System.IO.Directory.CreateDirectory(directoryEnclosure1);
                            }



                            if (Directory.Exists(directoryEnclosure))
                            {
                                foreach (MultifileJson index in FileTable)
                                {
                                    string FileName = index.name;
                                    string SourceFile = System.IO.Path.Combine(directoryEnclosure1, index.name);
                                    string ext = Path.GetExtension(SourceFile);
                                    BlobStorage.CopyLocalFileToBlob(SourceFile, "References");
                                    System.IO.File.Delete(SourceFile);
                                    model.EnclosureFile = urlEnclosure + FileName;
                                    DiaryViewModel.InsertMultiFiles(refid, model.EnclosureFile,index.FileName);
                                }
                            }
                            else
                            {
                                model.EnclosureFile = null;
                            }
                        }
                    }




                }

                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");

                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveCopy(MlaDiaryData model)
        {
            try
            {
                string res = "0";
                model.ActionDescription = "<p>REFERENCE ENTERED</p>";
                if (model.isLetter == "Letter")
                {
                    model.ActionCode = 11;
                }
                else
                {
                    model.ActionCode = 12;
                }

                DiaryViewModel diaryModel = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById((Int64)Convert.ToInt64(model.RefId), DeptId);
                diaryModel.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                diaryModel.RefId = 0;
                diaryModel.isLetter = diaryModel.isLetter;
                diaryModel.MeetingId = 0;
                diaryModel.ApplicantName = diaryModel.ApplicantName;
                diaryModel.CreatedBy = diaryModel.CreatedBy;
                diaryModel.DiaryDate = diaryModel.DiaryDate;
                diaryModel.DiaryNumber = diaryModel.DiaryNumber;
                diaryModel.DispatchNumber = diaryModel.DispatchNumber;
                diaryModel.EnclosureFile = diaryModel.EnclosureFile;
                diaryModel.FromDeptId = diaryModel.FromDeptId;
                diaryModel.FromOfficeId = diaryModel.FromOfficeId;
                diaryModel.LetterDate = diaryModel.LetterDate;
                diaryModel.LetterNo = diaryModel.LetterNo;
                diaryModel.LinkRefID = diaryModel.LinkRefID;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = diaryModel.SubjectDetails;
                diaryModel.ToDeptId = diaryModel.ToDeptId;
                diaryModel.ToOfficeId = diaryModel.ToOfficeId;
                diaryModel.ActionCode = diaryModel.ActionCode;
                diaryModel.ActionAmountSanction = diaryModel.ActionAmountSanction;
                diaryModel.ActionDescription = diaryModel.ActionDescription;
                diaryModel.ActionTakenDate = diaryModel.ActionTakenDate;
                diaryModel.ActionTypeid = diaryModel.ActionTypeid;
                diaryModel.ActionTakenFile = diaryModel.ActionTakenFile;
                diaryModel.OfficeId = diaryModel.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.ActId = diaryModel.ActId;
                diaryModel.SentFromOfficeId = Convert.ToInt32(CurrentSession.OfficeId.ToString());
                diaryModel.ActionCode = 22;

                diaryModel.FromDeptId = CurrentSession.DeptID.ToString();
                diaryModel.FromOfficeId = Convert.ToInt32(CurrentSession.OfficeId.ToString());
                res = DiaryViewModel.SaveDispatch(diaryModel);
                if (res != "")
                {
                    diaryModel.LinkRefID = Convert.ToInt64(res);
                    if (model.MultiUpdate != "")
                    {
                        DiaryViewModel dvm = new DiaryViewModel();
                        List<string> refList = model.MultiUpdate.Split(',').ToList();
                        foreach (string mrefid in refList)
                        {
                            diaryModel.RefId = Convert.ToInt64(mrefid);
                            var response = dvm.AddToPartFile(Convert.ToInt64(diaryModel.RefId), Convert.ToInt64(model.RefId), Convert.ToInt64(diaryModel.LinkRefID), Convert.ToInt32(0), diaryModel.CreatedBy);

                        }
                    }
                    int ShowType = 0;
                    var rps = DiaryViewModel.UpdateDiaryMovement(Convert.ToInt64(model.RefId), Convert.ToInt64(diaryModel.LinkRefID), diaryModel.CreatedBy, 2, 0, ShowType);
                }




                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveSubFile(MlaDiaryData model)
        {
            try
            {
                string res = "0";
                model.ActionDescription = "<p>REFERENCE ENTERED</p>";
                if (model.isLetter == "Letter")
                {
                    model.ActionCode = 11;
                }
                else
                {
                    model.ActionCode = 12;
                }

                DiaryViewModel diaryModel = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById((Int64)Convert.ToInt64(model.RefId), DeptId);
                diaryModel.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                diaryModel.RefId = 0;
                diaryModel.isLetter = diaryModel.isLetter;
                diaryModel.MeetingId = 0;
                diaryModel.ApplicantName = diaryModel.ApplicantName;
                diaryModel.CreatedBy = diaryModel.CreatedBy;
                diaryModel.DiaryDate = diaryModel.DiaryDate;
                diaryModel.DiaryNumber = diaryModel.DiaryNumber;
                diaryModel.DispatchNumber = diaryModel.DispatchNumber;
                diaryModel.EnclosureFile = diaryModel.EnclosureFile;
                diaryModel.FromDeptId = diaryModel.FromDeptId;
                diaryModel.FromOfficeId = diaryModel.FromOfficeId;
                diaryModel.LetterDate = diaryModel.LetterDate;
                diaryModel.LetterNo = diaryModel.LetterNo;
                diaryModel.LinkRefID = diaryModel.LinkRefID;
                diaryModel.Subject = diaryModel.Subject;
                diaryModel.SubjectDetails = diaryModel.SubjectDetails;
                diaryModel.ToDeptId = diaryModel.ToDeptId;
                diaryModel.ToOfficeId = diaryModel.ToOfficeId;
                diaryModel.ActionCode = diaryModel.ActionCode;
                diaryModel.ActionAmountSanction = diaryModel.ActionAmountSanction;
                diaryModel.ActionDescription = diaryModel.ActionDescription;
                diaryModel.ActionTakenDate = diaryModel.ActionTakenDate;
                diaryModel.ActionTypeid = diaryModel.ActionTypeid;
                diaryModel.ActionTakenFile = diaryModel.ActionTakenFile;
                diaryModel.OfficeId = diaryModel.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.ActId = diaryModel.ActId;



                res = DiaryViewModel.SaveDispatch(diaryModel);
                if (res != "")
                {
                    diaryModel.LinkRefID = Convert.ToInt64(res);
                    int ShowType = 0;
                    var rps = DiaryViewModel.UpdateDiaryMovement(Convert.ToInt64(model.RefId), Convert.ToInt64(diaryModel.LinkRefID), diaryModel.CreatedBy, 3, 0, ShowType);
                }




                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveSubFileFromSent(Int64 ID)
        {
            try
            {
                string response = ""; string res = "0";
                DiaryViewModel diaryModel = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById(ID, DeptId);
                diaryModel.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                diaryModel.RefId = 0;
                diaryModel.isLetter = diaryModel.isLetter;
                diaryModel.MeetingId = 0;
                diaryModel.ApplicantName = diaryModel.ApplicantName;
                diaryModel.CreatedBy = diaryModel.CreatedBy;
                diaryModel.DiaryDate = diaryModel.DiaryDate;
                diaryModel.DiaryNumber = diaryModel.DiaryNumber;
                diaryModel.DispatchNumber = diaryModel.DispatchNumber;
                diaryModel.EnclosureFile = diaryModel.EnclosureFile;
                diaryModel.FromDeptId = diaryModel.FromDeptId;
                diaryModel.FromOfficeId = diaryModel.FromOfficeId;
                diaryModel.LetterDate = diaryModel.LetterDate;
                diaryModel.LetterNo = diaryModel.LetterNo;
                diaryModel.LinkRefID = diaryModel.LinkRefID;
                diaryModel.Subject = diaryModel.Subject;
                diaryModel.SubjectDetails = diaryModel.SubjectDetails;
                diaryModel.ToDeptId = diaryModel.ToDeptId;
                diaryModel.ToOfficeId = diaryModel.ToOfficeId;
                diaryModel.ActionCode = diaryModel.ActionCode;
                diaryModel.ActionAmountSanction = diaryModel.ActionAmountSanction;
                diaryModel.ActionDescription = diaryModel.ActionDescription;
                diaryModel.ActionTakenDate = diaryModel.ActionTakenDate;
                diaryModel.ActionTypeid = diaryModel.ActionTypeid;
                diaryModel.ActionTakenFile = diaryModel.ActionTakenFile;
                diaryModel.OfficeId = diaryModel.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.ActId = diaryModel.ActId;
                diaryModel.SubjectPB = diaryModel.SubjectPB;
                diaryModel.SubjectDetailsPB = diaryModel.SubjectDetailsPB;
                diaryModel.StatementImplementation_local = diaryModel.StatementImplementation_local;
                diaryModel.PressNoteENG = diaryModel.PressNoteENG;
                diaryModel.PressNotePun = diaryModel.PressNotePun;

                res = DiaryViewModel.SaveDispatch(diaryModel);
                if (res != "")
                {
                    diaryModel.LinkRefID = Convert.ToInt64(res);
                    int ShowType = 0;
                    var rps = DiaryViewModel.UpdateDiaryMovement(Convert.ToInt64(ID), Convert.ToInt64(diaryModel.LinkRefID), diaryModel.CreatedBy, 3, 0, ShowType);
                }

                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult SaveProceedingFile(Int64 ID)
        {
            try
            {
                string response = ""; string res = "0";
                DiaryViewModel diaryModel = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById(ID, DeptId);
                diaryModel.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                diaryModel.RefId = 0;
                diaryModel.isLetter = diaryModel.isLetter;
                diaryModel.MeetingId = 0;
                diaryModel.ApplicantName = diaryModel.ApplicantName;
                diaryModel.CreatedBy = diaryModel.CreatedBy;
                diaryModel.DiaryDate = diaryModel.DiaryDate;
                diaryModel.DiaryNumber = diaryModel.DiaryNumber;
                diaryModel.DispatchNumber = diaryModel.DispatchNumber;
                diaryModel.EnclosureFile = diaryModel.EnclosureFile;
                diaryModel.FromDeptId = diaryModel.FromDeptId;
                diaryModel.FromOfficeId = diaryModel.FromOfficeId;
                diaryModel.LetterDate = diaryModel.LetterDate;
                diaryModel.LetterNo = diaryModel.LetterNo;
                diaryModel.LinkRefID = diaryModel.LinkRefID;
                diaryModel.Subject = diaryModel.Subject;
                diaryModel.SubjectDetails = diaryModel.SubjectDetails;
                diaryModel.ToDeptId = diaryModel.ToDeptId;
                diaryModel.ToOfficeId = diaryModel.ToOfficeId;
                diaryModel.ActionCode = diaryModel.ActionCode;
                diaryModel.ActionAmountSanction = diaryModel.ActionAmountSanction;
                diaryModel.ActionDescription = diaryModel.ActionDescription;
                diaryModel.ActionTakenDate = diaryModel.ActionTakenDate;
                diaryModel.ActionTypeid = diaryModel.ActionTypeid;
                diaryModel.ActionTakenFile = diaryModel.ActionTakenFile;
                diaryModel.OfficeId = diaryModel.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.ActId = diaryModel.ActId;
                diaryModel.SubjectPB = diaryModel.SubjectPB;
                diaryModel.SubjectDetailsPB = diaryModel.SubjectDetailsPB;
                diaryModel.StatementImplementation_local = diaryModel.StatementImplementation_local;
                diaryModel.PressNoteENG = diaryModel.PressNoteENG;
                diaryModel.PressNotePun = diaryModel.PressNotePun;
                diaryModel.SentFromOfficeId = Convert.ToInt32(CurrentSession.OfficeId.ToString());
                //diaryModel.DocmentType = "17"; 
                //diaryModel.ActionCode = 22;
                res = DiaryViewModel.SaveDispatch(diaryModel);
                if (res != "")
                {
                    diaryModel.RefId = Convert.ToInt64(res);
                    int ShowType = 0;
                    int FileType = 5;  // 3 PartFile, 4 Mergerd File, 5 Proceeding File

                    var rps = DiaryViewModel.SaveProceedingFile(Convert.ToInt64(ID), Convert.ToInt64(diaryModel.LinkRefID), Convert.ToInt64(diaryModel.RefId), diaryModel.CreatedBy, FileType, 0, ShowType);
                }

                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void SavePDf(string AgendaId, string filepath)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveAgendaPdf(Convert.ToInt32(AgendaId), filepath);
            }

        }

        public static void SavePDf3(string AgendaId, string filepath)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveAgendaPdf(Convert.ToInt32(AgendaId), filepath);
            }

        }

        public static void SavePDf2(string AgendaId, string filepath, string ReportType)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveAgendaPdf2(Convert.ToInt32(AgendaId), filepath, ReportType);
            }

        }




        public int GetFinYear()
        {
            string finyear = "0";
            DateTime today = DateTime.Today;
            if (today.Month <= 3)
            {
                finyear = today.AddYears(-1).ToString("yyyy");
            }
            else
            {
                finyear = today.ToString("yyyy");
            }

            return Convert.ToInt32(finyear);
        }

        public ActionResult SaveUpdateMlaDiaryAction(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {
                string res = "0";
                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                //  var FileSettings = FileSetting.GetDISFileSetting();

                //upload action file 
                if (model.IsFile == "Y")
                {
                    model.ActionTakenFile = model.ActionTakenFile;
                }
                else
                {
                    string urlA = "/References/";
                    string directoryA = Server.MapPath(urlA); ;

                    string tempurlA1 = "~/TempFile";
                    string tempdirectoryA1 = Server.MapPath(tempurlA1);

                    if (!string.IsNullOrWhiteSpace(model.ActionTakenFile))
                    {
                        string sourceFile = Path.Combine(tempdirectoryA1, model.ActionTakenFile);

                        if (System.IO.File.Exists(sourceFile))
                        {
                            string ext = Path.GetExtension(sourceFile);
                            string newFileName = Guid.NewGuid().ToString() + ext;

                            BlobStorage.CopyLocalFileToBlobNew(sourceFile, "References", newFileName);

                            //Delete Temp
                            string tempFile = Path.Combine(tempdirectoryA1, Path.GetFileName(model.ActionTakenFile));
                            if (System.IO.File.Exists(tempFile))
                            {
                                System.IO.File.Delete(tempFile);
                            }

                            model.ActionTakenFile = urlA + newFileName;
                        }
                        else
                        {
                            model.ActionTakenFile = null;
                        }

                        
                    }
                    else
                    {
                        model.ActionTakenFile = null;
                    }
                }
                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.Subject = model.Subject;
                diaryModel.ApplicantAddress = model.ApplicantAddress;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                diaryModel.DocmentType = model.DocmentType;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                diaryModel.ActionCode = model.ActionCode;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                diaryModel.ActionDescription = model.ActionDescription;
                diaryModel.ActionTakenDate = model.ActiontakenDate;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.isLetter = model.isLetter;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.SentFromOfficeId = Convert.ToInt32(CurrentSession.OfficeId.ToString());
                diaryModel.MoveId = model.MoveId;
                diaryModel.FileDraft = model.FileDraft;
                if (model.SentType == "MLAOffice")
                {
                    var useridoffice = model.SentMlaUserId.Split('~');
                    diaryModel.SentToUserId = useridoffice[0];
                    diaryModel.AssinedOfficeId = Convert.ToInt32(useridoffice[1]);
                    diaryModel.InboxType = "Diary";
                    diaryModel.SentType = "Diary";
                }
                else if (model.SentType == "FromOffice")
                {
                    var useridoffice = model.SentFromUserId.Split('~');
                    diaryModel.SentToUserId = useridoffice[0];
                    diaryModel.AssinedOfficeId = Convert.ToInt32(useridoffice[1]);
                    diaryModel.InboxType = "Diary";
                    diaryModel.SentType = "Diary";
                }
                else
                {
                    var useridoffice = model.SentToUserId.Split('~');
                    diaryModel.SentToUserId = useridoffice[0];
                    diaryModel.AssinedOfficeId = Convert.ToInt32(useridoffice[1]);
                    if (diaryModel.FromDeptId == diaryModel.ToDeptId)
                    {
                        diaryModel.InboxType = "Dispatch";
                    }
                    else
                    {
                        diaryModel.InboxType = "Diary";
                    }


                    diaryModel.SentType = "Diary";
                }

                res = DiaryViewModel.SaveDiary(diaryModel);

                if (model.FileDraft == false)
                {
                    if (diaryModel.ActionCode == 5 || diaryModel.ActionCode == 26)
                    {
                        string Obtype = "";
                        if(diaryModel.ActionCode == 5)
                        {
                            Obtype = "LG ";
                        }
                        else
                        {
                            Obtype = "ADC ";
                        }

                        DataTable dt = new DataTable();
                        dt = DiaryViewModel.GetFileRefId(Convert.ToInt64(diaryModel.RefId));
                        if (dt.Rows.Count > 0)
                        {
                            // diaryModel.LinkRefID = Convert.ToInt64(dt.Rows[0]["RefId"]);
                            foreach (DataRow dr in dt.Rows)
                            {
                                diaryModel.LinkRefID = Convert.ToInt64(dr["RefId"].ToString());
                                DataSet ds = DiaryViewModel.GetMovementbyRefid(Convert.ToInt64(diaryModel.RefId), Convert.ToInt64(diaryModel.LinkRefID));
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dr1 in ds.Tables[0].Rows)
                                    {
                                        string SentToUserId = dr1["ToUserId"].ToString();
                                        string Notification = Obtype + "Observation Reply File RefId: " + diaryModel.LinkRefID;
                                        var response = DiaryViewModel.InsertNotification(Convert.ToInt64(diaryModel.RefId), 0, 7, Notification, SentToUserId, 1, diaryModel.Userid);

                                    }

                                }

                            }
                        }

                        

                    }
                }


                if (model.isfund == 1) //|| model.isfund == 0
                {

                    FundSanctionModel fundsanction = new FundSanctionModel();
                    fundsanction.RefId = Convert.ToInt64(model.RefId);
                    fundsanction.FinYear = GetFinYear();
                    fundsanction.Remarks = "";
                    fundsanction.deptId = model.FromDeptId;
                    fundsanction.officeid = model.FromOfficeId;
                    fundsanction.Createdby = model.CreatedBy;

                    //Fund Add
                    if (model.ActionCode == 1)
                    {
                        if (model.fundtype_3 > 0)  //ReliefFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_3;
                            fundsanction.FundType = 3;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }

                        if (model.fundtype_4 > 0) //SmallSavingFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_4;
                            fundsanction.FundType = 4;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }
                        if (model.fundtype_13 > 0) //UntiedFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_13;
                            fundsanction.FundType = 13;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }

                        if (model.fundtype_14 > 0) //VivekiFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_14;
                            fundsanction.FundType = 14;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }

                        if (model.fundtype_15 > 0) //CattleFairFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_15;
                            fundsanction.FundType = 15;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }

                    }




                    DataSet ds, ds1 = new DataSet();
                    ds1 = FundFunctionModel.GetFundByRefId((Int64)diaryModel.RefId);

                    //Fund Update
                    if (model.ActionCode == 3)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                int FundSanctionId = Convert.ToInt32(dr["Id"]);
                                int FundSancAmount = Convert.ToInt32(Request.Form["San_" + FundSanctionId]);
                                if (FundSancAmount > 0)
                                {
                                    fundsanction.AmountSanctioned = FundSancAmount;
                                    fundsanction.SanctionDate = model.SanctionDate;
                                    fundsanction.id = FundSanctionId;
                                    DiaryViewModel.UpdateSanction(fundsanction);
                                }
                            }

                        }
                    }


                    //Update

                    //    if (model.San_ReliefFunds > 0)
                    //{
                    //    fundsanction.SanctionDate = model.SanctionDate;
                    //    fundsanction.AmountSanctioned = model.San_ReliefFunds;
                    //    fundsanction.FundType = 3;
                    //    DiaryViewModel.UpdateSanction(fundsanction);
                    //}

                    //if (model.San_SmallSavingFunds > 0)
                    //{
                    //    fundsanction.SanctionDate = model.SanctionDate;
                    //    fundsanction.AmountSanctioned = model.San_SmallSavingFunds;
                    //    fundsanction.FundType = 4;
                    //    DiaryViewModel.UpdateSanction(fundsanction);
                    //}
                    //if (model.San_UntiedFunds > 0)
                    //{
                    //    fundsanction.SanctionDate = model.SanctionDate;
                    //    fundsanction.AmountSanctioned = model.San_UntiedFunds;
                    //    fundsanction.FundType = 13;
                    //    DiaryViewModel.UpdateSanction(fundsanction);
                    //}

                    //if (model.San_VivekiFunds > 0)
                    //{
                    //    fundsanction.SanctionDate = model.SanctionDate;
                    //    fundsanction.AmountSanctioned = model.San_VivekiFunds;
                    //    fundsanction.FundType = 14;
                    //    DiaryViewModel.UpdateSanction(fundsanction);
                    //}

                    //if (model.San_CattleFairFunds > 0)
                    //{
                    //    fundsanction.SanctionDate = model.SanctionDate;
                    //    fundsanction.AmountSanctioned = model.San_CattleFairFunds;
                    //    fundsanction.FundType = 15;
                    //    DiaryViewModel.UpdateSanction(fundsanction);
                    //}






                    List<FundReportModel> lstfund = new List<FundReportModel>();
                    List<FundReportModel> sanctionedfund = new List<FundReportModel>();
                    string userid = CurrentSession.UserID;
                    ds = FundFunctionModel.GetFundAvailable(GetFinYear(), userid);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FundReportModel fr = new FundReportModel();
                        fr.FundTypeId = Convert.ToInt32(dr["DocTypeId"]);
                        fr.FundType = Convert.ToString(dr["FundType"]);
                        fr.FundAllocated = Convert.ToInt32(dr["FundAllocated"]);
                        //fr.FundApproved  = Convert.ToInt32(dr["ApprovedAmount"]);
                        //fr.FundSanctioned = Convert.ToInt32(dr["FundSanctioned"]);
                        fr.FundAvaiable = Convert.ToInt32(dr["FundAllocated"]) - Convert.ToInt32(dr["ApprovedAmount"]);
                        fr.DeptName = Convert.ToString(dr["deptname"]);
                        lstfund.Add(fr);
                    }

                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        FundReportModel fr = new FundReportModel();
                        fr.FundTypeId = Convert.ToInt32(dr["DocTypeId"]);
                        fr.FundType = Convert.ToString(dr["DocumentTypeName"]);
                        fr.FundApproved = Convert.ToInt32(dr["ApprovedAmount"]);
                        fr.FundSanctioned = Convert.ToInt32(dr["AmountSanctioned"]);
                        sanctionedfund.Add(fr);
                    }



                    model._LstFundReport = lstfund;
                    model._LstFundSactioned = sanctionedfund;

                }


                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveUpdateMlaDispatchAction(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {
                string res = "0";
                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;

                if (model.IsFile == "Y")
                {
                    model.ActionTakenFile = model.ActionTakenFile;
                }
                
                else
                {
                    string urlA = "/References/";
                    string directoryA = Server.MapPath(urlA); ;

                    string tempurlA1 = "~/TempFile";
                    string tempdirectoryA1 = Server.MapPath(tempurlA1);

                    if (!string.IsNullOrWhiteSpace(model.ActionTakenFile))
                    {
                        string sourceFile = Path.Combine(tempdirectoryA1, model.ActionTakenFile);

                        if (System.IO.File.Exists(sourceFile))
                        {
                            string ext = Path.GetExtension(sourceFile);
                            string newFileName = Guid.NewGuid().ToString() + ext;

                            BlobStorage.CopyLocalFileToBlobNew(sourceFile, "References", newFileName);

                            //Delete Temp
                            string tempFile = Path.Combine(tempdirectoryA1, Path.GetFileName(model.ActionTakenFile));
                            if (System.IO.File.Exists(tempFile))
                            {
                                System.IO.File.Delete(tempFile);
                            }

                            model.ActionTakenFile = urlA + newFileName;
                        }
                        else
                        {
                            model.ActionTakenFile = null;
                        }
                    }
                    else
                    {
                        model.ActionTakenFile = null;
                    }
                }
                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.Subject = model.Subject;
                diaryModel.ApplicantAddress = model.ApplicantAddress;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                diaryModel.DocmentType = model.DocmentType;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                //diaryModel.ActionCode = model.ActionCode;

                // FIX: Fallback to ActionCodeMulti if ActionCode is null/0 to prevent NULL in mMlaDiary

                if (model.ActionCode.HasValue && model.ActionCode.Value != 0)
                {
                    diaryModel.ActionCode = model.ActionCode.Value;
                }
                else if (model.ActionCodeMulti.HasValue && model.ActionCodeMulti.Value != 0)
                {
                    diaryModel.ActionCode = model.ActionCodeMulti.Value;
                }
                else
                {
                    ErrorLog.WriteToLog(
                        "ActionCode and ActionCodeMulti are both NULL or 0."
                    );

                    return Json(
                        "Action Code cannot be NULL or 0.",
                        JsonRequestBehavior.AllowGet
                    );
                }


                //diaryModel.ActionCode = (model.ActionCode.HasValue && model.ActionCode.Value != 0) ? model.ActionCode : model.ActionCodeMulti;

                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                diaryModel.ActionDescription = model.ActionDescription;
                diaryModel.ActionTakenDate = model.ActiontakenDate;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.isLetter = model.isLetter;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.SentFromOfficeId = Convert.ToInt32(CurrentSession.OfficeId.ToString());
                diaryModel.MoveId = model.MoveId;
                diaryModel.FileDraft = model.FileDraft;
                if (model.SentType == "MLAOffice")
                {
                    var useridoffice = model.SentMlaUserId.Split('~');
                    diaryModel.SentToUserId = useridoffice[0];
                    diaryModel.AssinedOfficeId = Convert.ToInt32(useridoffice[1]);
                    diaryModel.InboxType = "Diary";
                    diaryModel.SentType = "Diary";
                }
                else if(model.SentType == "FromOffice")
                {
                    var useridoffice = model.SentFromUserId.Split('~');
                    diaryModel.SentToUserId = useridoffice[0];
                    diaryModel.AssinedOfficeId = Convert.ToInt32(useridoffice[1]);
                    diaryModel.InboxType = "Dispatch";
                    diaryModel.SentType = "Dispatch";
                }
                else
                {
                    var useridoffice = model.SentToUserId.Split('~');
                    diaryModel.SentToUserId = useridoffice[0];
                    diaryModel.AssinedOfficeId = Convert.ToInt32(useridoffice[1]);
                    if (diaryModel.FromDeptId == diaryModel.ToDeptId)
                    {
                        diaryModel.InboxType = "Dispatch";
                    }
                    else
                    {
                        diaryModel.InboxType = "Diary";
                    }

                    diaryModel.SentType = "Dispatch";
                }

                if (model.EditType == "Agenda")
                {
                    diaryModel.EditType = model.EditType;
                    res = DiaryViewModel.SaveDispatchAgenda(diaryModel);
                }
                else
                {
                    if (model.CopyTo == true)
                    {
                        diaryModel.RefId = 0;
                        res = DiaryViewModel.SaveDispatch(diaryModel);
                        if (res != "")
                        {
                            diaryModel.LinkRefID = Convert.ToInt64(res);
                            var rps = DiaryViewModel.UpdateDiaryMovement(Convert.ToInt64(model.RefId), Convert.ToInt64(diaryModel.LinkRefID), diaryModel.CreatedBy, 1, 1, 1);
                            diaryModel.RefId = Convert.ToInt64(res);
                            if (rps != "")
                            {
                                diaryModel.MoveId = Convert.ToInt32(rps);
                            }
                            res = DiaryViewModel.SaveDispatch(diaryModel);
                        }
                    }
                    else
                    {
                        res = DiaryViewModel.SaveDispatch(diaryModel);
                    }

                }



                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult SaveUpdateMultiInboxAction(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {
                string res = "0";
                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                //  var FileSettings = FileSetting.GetDISFileSetting();

                //upload action file 
                if (model.IsFile == "Y")
                {
                    model.ActionTakenFile = model.ActionTakenFile;
                }
                else
                {
                    string urlA = "/References/";
                    string directoryA = Server.MapPath(urlA); ;

                    string tempurlA1 = "~/TempFile";
                    string tempdirectoryA1 = Server.MapPath(tempurlA1);

                    if (!string.IsNullOrWhiteSpace(model.ActionTakenFile))
                    {
                        string sourceFile = Path.Combine(tempdirectoryA1, model.ActionTakenFile);

                        if (System.IO.File.Exists(sourceFile))
                        {
                            string ext = Path.GetExtension(sourceFile);
                            string newFileName = Guid.NewGuid().ToString() + ext;

                            BlobStorage.CopyLocalFileToBlobNew(sourceFile, "References", newFileName);

                            //Delete Temp
                            string tempFile = Path.Combine(tempdirectoryA1, Path.GetFileName(model.ActionTakenFile));
                            if (System.IO.File.Exists(tempFile))
                            {
                                System.IO.File.Delete(tempFile);
                            }

                            model.ActionTakenFile = urlA + newFileName;
                        }
                        else
                        {
                            model.ActionTakenFile = null;
                        }
                    }
                    else
                    {
                        model.ActionTakenFile = null;
                    }
                }
                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.Subject = model.Subject;
                diaryModel.ApplicantAddress = model.ApplicantAddress;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                diaryModel.DocmentType = model.DocmentType;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                diaryModel.ActionCode = model.ActionCode;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                diaryModel.ActionDescription = model.ActionDescription;
                diaryModel.ActionTakenDate = DateTime.Now.Date;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.MoveId = model.MoveId;
                diaryModel.isLetter = model.isLetter;
                diaryModel.SentFromOfficeId = Convert.ToInt32(CurrentSession.OfficeId.ToString());
                if (model.SentType == "FromOffice")
                {
                    var useridoffice = model.SentFromUserId.Split('~');
                    diaryModel.SentToUserId = useridoffice[0];
                    diaryModel.AssinedOfficeId = Convert.ToInt32(useridoffice[1]);

                    //diaryModel.SentToUserId = model.SentFromUserId;
                    //if (model.AssinedOfficeId != 0)
                    //{
                    //    diaryModel.AssinedOfficeId = model.AssinedOfficeId;
                    //}
                    //else
                    //{
                    //    diaryModel.AssinedOfficeId = 0;
                    //}
                    diaryModel.InboxType = "Dispatch";
                    //diaryModel.InboxType = "Diary";
                    diaryModel.SentType = "Diary";
                }
                else
                {
                    var useridoffice = model.SentToUserId.Split('~');
                    diaryModel.SentToUserId = useridoffice[0];
                    diaryModel.AssinedOfficeId = Convert.ToInt32(useridoffice[1]);

                    //diaryModel.SentToUserId = model.SentToUserId;
                    if (diaryModel.FromDeptId == diaryModel.ToDeptId)
                    {
                        diaryModel.InboxType = "Dispatch";
                    }
                    else
                    {
                        diaryModel.InboxType = "Diary";
                    }
                    diaryModel.SentType = "Diary";
                }



                List<SendInboxJson> indexTable = new List<SendInboxJson>();
                indexTable = new JavaScriptSerializer().Deserialize<List<SendInboxJson>>(model.MultiUpdate);
                int i = 1;

                foreach (SendInboxJson index in indexTable)
                {
                    int MoveId = Convert.ToInt32(index.MoveId);
                    Int64 refid = Convert.ToInt64(index.RefId);
                    diaryModel.RefId = refid;
                    diaryModel.MoveId = MoveId;
                    res = DiaryViewModel.UpdateInboxMultiAction(diaryModel);

                }






                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }



        public static string CleanHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            string text = html;


            string prev;
            do
            {
                prev = text;
                text = WebUtility.HtmlDecode(text);
            } while (!prev.Equals(text));

            // 🔹 Remove all tags except <p>, <br>, <b>, <i>
            text = Regex.Replace(text, @"<(?!\/?(p|br|b|i)\b)[^>]*>", "", RegexOptions.IgnoreCase);

            // 🔹 Remove inline styles and font info
            text = Regex.Replace(text, @"\s*style\s*=\s*""[^""]*""", "", RegexOptions.IgnoreCase);
            text = Regex.Replace(text, @"font-family\s*:\s*[^;""']+;?", "", RegexOptions.IgnoreCase);

            // 🔹 Replace non-breaking spaces with normal space
            text = text.Replace("\u00A0", " ");
            text = Regex.Replace(text, @"\s{2,}", " "); // collapse multiple spaces

            // 🔹 Normalize paragraph endings with newline for readability
            text = Regex.Replace(text, @"</p\s*>", "</p>\n", RegexOptions.IgnoreCase);

            // 🔹 Ensure <br> stays as line breaks
            text = Regex.Replace(text, @"<br\s*/?>", "<br/>", RegexOptions.IgnoreCase);

            // 🔹 Trim start/end whitespace
            return text.Trim();
        }




        public ActionResult SaveAnxFiles(MlaDiaryData model, HttpPostedFileBase file)
        {
            string res = "0";
            try
            {

                Int64 refid = model.RefId;
                if (model.EnclosureFile != null)
                {
                    //upload enclosure file 
                    if (model.IsEnclosureFile == "Y")
                    {
                        model.EnclosureFile = model.EnclosureFile;
                    }
                    else
                    {

                        List<MultifileJson> FileTable = new List<MultifileJson>();
                        FileTable = new JavaScriptSerializer().Deserialize<List<MultifileJson>>(model.EnclosureFile);

                        ErrorLog.WriteToLog("2");
                        string urlEnclosure = "/References/";
                        //string urlEnclosure = "/MlaDiary/EnclosureFile";
                        // string directoryEnclosure = FileSettings.SettingValue + urlEnclosure;
                        string directoryEnclosure = Server.MapPath(urlEnclosure);


                        if (!System.IO.Directory.Exists(directoryEnclosure))
                        {
                            System.IO.Directory.CreateDirectory(directoryEnclosure);
                        }
                        // int fileIDEnclosure = GetFileRandomNo();

                        string TempurlEnclosure1 = "~/TempFile";
                        string directoryEnclosure1 = Server.MapPath(TempurlEnclosure1);



                        if (!System.IO.Directory.Exists(directoryEnclosure1))
                        {
                            System.IO.Directory.CreateDirectory(directoryEnclosure1);
                        }



                        if (Directory.Exists(directoryEnclosure))
                        {
                            foreach (MultifileJson index in FileTable)
                            {
                                string FileName = index.name;
                                string SourceFile = System.IO.Path.Combine(directoryEnclosure1, index.name);
                                string ext = Path.GetExtension(SourceFile);
                                BlobStorage.CopyLocalFileToBlob(SourceFile, "References");
                                System.IO.File.Delete(SourceFile);
                                model.EnclosureFile = urlEnclosure + FileName;
                                DiaryViewModel.InsertMultiFiles(refid, model.EnclosureFile, index.FileName);
                            }
                        }
                        else
                        {
                            model.EnclosureFile = null;
                        }
                    }
                }


                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveUpdateMultiAction(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {
                string res = "0";


                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                //  var FileSettings = FileSetting.GetDISFileSetting();

                //upload action file 
                if (model.IsFile == "Y")
                {
                    model.ActionTakenFile = model.ActionTakenFile;

                    string urlA = "/References/";
                    string directoryA = Server.MapPath(urlA);

                    if (!Directory.Exists(directoryA))
                    {
                        Directory.CreateDirectory(directoryA);
                    }

                    string tempUrl = "~/TempFile";
                    string tempDirectory = Server.MapPath(tempUrl);

                    if (!string.IsNullOrEmpty(model.ActionTakenFile))
                    {
                        // Get file name from model (example: /TempFile/abc.pdf)
                        string fileName = Path.GetFileName(model.ActionTakenFile);
                        string sourcePath = Path.Combine(tempDirectory, fileName);

                        if (System.IO.File.Exists(sourcePath))
                        {
                            string ext = Path.GetExtension(fileName);
                            string newFileName = Guid.NewGuid().ToString() + ext;
                            string destinationPath = Path.Combine(directoryA, newFileName);

                            // Move file (transfer)
                            System.IO.File.Move(sourcePath, destinationPath);

                            // Save new path in model
                            model.ActionTakenFile = urlA + newFileName;
                        }
                        else
                        {
                            model.ActionTakenFile = null;
                        }
                    }


                }
                else
                {
                    model.ActionTakenFile = null;

                }

                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.ActionCode = model.ActionCodeMulti;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
            
                diaryModel.Draftchk = model.Draftchk;
               

                if (model.MultiUpdate != "") {
                    string refIds = model.MultiUpdate; // already comma separated
                    diaryModel.ActionDescription = model.ActionDescriptionMulti;
                    diaryModel.ActionDescriptionPB = model.ActionDescriptionMultiPB;
                    res = DiaryViewModel.UpdateMultiActionBulk(diaryModel, refIds);


                    //// 👉 Send email only when ActionCode = 5
                    //if (diaryModel.ActionCode == 5)
                    //{
                    //    try
                    //    {
                    //        EmailHelper EmailService = new EmailHelper();
                    //        EmailHelper.SendActionNotification(refIds,diaryModel.ActionDescription);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        // log error, don't break main flow
                    //    }
                    //}
                }

                

                //if (model.MultiUpdate != "")
                //{
                //    List<string> refList = model.MultiUpdate.Split(',').ToList();
                //    foreach (string mrefid in refList)
                //    {
                //        diaryModel.RefId = Convert.ToInt64(mrefid); // 🔥 MISSING
                //        //diaryModel.ActionDescription = CleanHtml(model.ActionDescriptionMulti);
                //        diaryModel.ActionDescription = model.ActionDescriptionMulti;
                //        diaryModel.ActionDescriptionPB = model.ActionDescriptionMultiPB;


                //            res = DiaryViewModel.UpdateMultiAction(diaryModel);
                //            if (diaryModel.ActionCode == 5)
                //            {

                //                DataTable dt = new DataTable();
                //                dt = DiaryViewModel.GetFileRefId(Convert.ToInt64(diaryModel.RefId));
                //                if (dt.Rows.Count > 0)
                //                {
                //                    diaryModel.LinkRefID = Convert.ToInt64(dt.Rows[0]["RefId"]);

                //                }

                //                DataSet ds = DiaryViewModel.GetMovementbyRefid(Convert.ToInt64(diaryModel.RefId), Convert.ToInt64(diaryModel.LinkRefID));
                //                if (ds.Tables[0].Rows.Count > 0)
                //                {
                //                    foreach (DataRow dr in ds.Tables[0].Rows)
                //                    {
                //                        string SentToUserId = dr["ToUserId"].ToString();
                //                        string Notification = "LG Observation Raised";
                //                        var response = DiaryViewModel.InsertNotification(Convert.ToInt64(diaryModel.RefId), 0, 6, Notification, SentToUserId, 1, diaryModel.Userid);

                //                    }

                //                }


                //            }



                //    }
                //}

                model.IsFile = "N";

                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult SaveUpdateMultiActionTemp(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {
                string res = "0";


                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                //  var FileSettings = FileSetting.GetDISFileSetting();

                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.Subject = model.Subject;
                diaryModel.ApplicantAddress = model.ApplicantAddress;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                diaryModel.DocmentType = model.DocmentType;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.LinkRefID = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                diaryModel.ActionCode = model.ActionCodeMulti;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                //diaryModel.ActionDescription = CleanHtml(model.ActionDescriptionMulti);
                diaryModel.ActionDescription = model.ActionDescriptionMulti;
                diaryModel.ActionDescriptionPB = model.ActionDescriptionMultiPB;
                diaryModel.ActionTakenDate = DateTime.Now;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.SwipeId = model.SwipeId;
                diaryModel.MoveId = model.MoveId;
                diaryModel.Draftchk = model.Draftchk;
                if (model.SentType == "FromOffice")
                {
                    diaryModel.SentToUserId = model.SentFromUserId;
                    if (model.AssinedOfficeId != 0)
                    {
                        diaryModel.AssinedOfficeId = model.AssinedOfficeId;
                    }
                    else
                    {
                        diaryModel.AssinedOfficeId = 0;
                    }
                    diaryModel.InboxType = "Dispatch";
                    diaryModel.SentType = "Diary";
                }
                else
                {
                    diaryModel.SentToUserId = model.SentToUserId;
                    if (diaryModel.FromDeptId == diaryModel.ToDeptId)
                    {
                        diaryModel.InboxType = "Dispatch";
                    }
                    else
                    {
                        diaryModel.InboxType = "Diary";
                    }
                    diaryModel.SentType = "Diary";
                }

                if (model.MultiUpdate != "")
                {
                    List<string> refList = model.MultiUpdate.Split(',').ToList();
                    foreach (string mrefid in refList)
                    {
                        //diaryModel.ActionDescription = CleanHtml(model.ActionDescriptionMulti);
                        diaryModel.ActionDescription = model.ActionDescriptionMulti;
                        diaryModel.ActionDescriptionPB = model.ActionDescriptionMultiPB;

                        diaryModel.RefId = Convert.ToInt64(mrefid);
                        //res = DiaryViewModel.UpdateMultiAction(diaryModel);
                        string userid = CurrentSession.UserID;
                        DiaryViewModel.InsertActionSession(Convert.ToInt64(diaryModel.RefId), userid, diaryModel.ActionDescription, Convert.ToInt32(diaryModel.ActionCode));
                        
                    }
                }
                model.IsFile = "N";

                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveAllTemp(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {
                string res = "0";


                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                //  var FileSettings = FileSetting.GetDISFileSetting();



                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.Subject = model.Subject;
                diaryModel.ApplicantAddress = model.ApplicantAddress;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                diaryModel.DocmentType = model.DocmentType;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.LinkRefID = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                diaryModel.ActionCode = model.ActionCodeMulti;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                //diaryModel.ActionDescription = CleanHtml(model.ActionDescriptionMulti);
                diaryModel.ActionDescription = model.ActionDescriptionMulti;
                diaryModel.ActionDescriptionPB = model.ActionDescriptionMultiPB;
                diaryModel.ActionTakenDate = DateTime.Now;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.SwipeId = model.SwipeId;
                diaryModel.MoveId = model.MoveId;
                diaryModel.Draftchk = model.Draftchk;
                if (model.SentType == "FromOffice")
                {
                    diaryModel.SentToUserId = model.SentFromUserId;
                    if (model.AssinedOfficeId != 0)
                    {
                        diaryModel.AssinedOfficeId = model.AssinedOfficeId;
                    }
                    else
                    {
                        diaryModel.AssinedOfficeId = 0;
                    }
                    diaryModel.InboxType = "Dispatch";
                    diaryModel.SentType = "Diary";
                }
                else
                {
                    diaryModel.SentToUserId = model.SentToUserId;
                    if (diaryModel.FromDeptId == diaryModel.ToDeptId)
                    {
                        diaryModel.InboxType = "Dispatch";
                    }
                    else
                    {
                        diaryModel.InboxType = "Diary";
                    }
                    diaryModel.SentType = "Diary";
                }

                DataTable dt = new DataTable();
                dt = DiaryViewModel.GetFileRefId(Convert.ToInt64(diaryModel.RefId));
                if (dt.Rows.Count > 0)
                {
                    diaryModel.LinkRefID = Convert.ToInt64(dt.Rows[0]["RefId"]);
                }

                DataTable lr = new DataTable();
                lr = DiaryViewModel.GetFileReferences(Convert.ToInt64(diaryModel.LinkRefID), CurrentSession.UserID);
                if (lr.Rows.Count > 0)
                {
                    foreach (DataRow row in lr.Rows)
                        {

                            string RefId = row["LinkRefid"].ToString();
                            string ActionRemarks = row["ActionRemarks"].ToString();
                            int ActionCode = Convert.ToInt32(row["TempActionCode"]);

                            diaryModel.ActionCode = ActionCode;
                            diaryModel.ActionDescription = ActionRemarks;
                            diaryModel.ActionDescriptionPB = model.ActionDescriptionMultiPB;
                            diaryModel.RefId = Convert.ToInt64(RefId);

                            if(diaryModel.ActionCode != 0)
                            {
                                res = DiaryViewModel.UpdateMultiAction(diaryModel);
                            }
                           

                                if (diaryModel.ActionCode == 5)
                                {



                                    DataSet ds = DiaryViewModel.GetMovementbyRefid(Convert.ToInt64(diaryModel.RefId), Convert.ToInt64(diaryModel.LinkRefID));
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in ds.Tables[0].Rows)
                                        {
                                            string SentToUserId = dr["ToUserId"].ToString();
                                            string Notification = "LG Observation Raised";
                                            var response = DiaryViewModel.InsertNotification(Convert.ToInt64(diaryModel.RefId), 0, 6, Notification, SentToUserId, 1, diaryModel.Userid);

                                        }

                                    }


                                }

                    

                        }
                }



                if (model.MultiUpdate != "")
                {
                    List<string> refList = model.MultiUpdate.Split(',').ToList();
                    
                }

                model.IsFile = "N";

                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveNewReference(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {
                string res = "0";
                DiaryViewModel dvm = new DiaryViewModel();
                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.isLetter = "Letter";
                diaryModel.ActionCode = 11;
                diaryModel.DocmentType = "1";


                diaryModel.ApplicantAddress = model.ApplicantAddress;
                // diaryModel.ReferencePriorityType = model.ReferencePriorityType;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.MeetingId = 0;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                //diaryModel.LetterDate = model.LetterDate;
                //diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = 0;
                //diaryModel.Subject = model.Subject;
                //diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                //diaryModel.ActionCode = model.ActionCode;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                diaryModel.ActionDescription = model.ActionDescription;
                diaryModel.ActionTakenDate = model.ActiontakenDate;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.ActId = model.ActId;

                List<AddNewReferenceJson> indexTable = new List<AddNewReferenceJson>();
                indexTable = new JavaScriptSerializer().Deserialize<List<AddNewReferenceJson>>(model.AddDetails);
                int i = 1;
                foreach (AddNewReferenceJson index in indexTable)
                {
                    int order = i;
                    string Letter = index.LetterNo;
                    DateTime LetterDate = Convert.ToDateTime(index.LetterDate);
                    string Subject = index.Subject;
                    diaryModel.LetterNo = Letter;
                    diaryModel.LetterDate = LetterDate;
                    diaryModel.Subject = Subject;
                    res = DiaryViewModel.SaveDispatch(diaryModel);
                    if (res != "")
                    {
                        diaryModel.LinkRefID = Convert.ToInt64(res);
                        var response = dvm.AddToFile(Convert.ToInt64(diaryModel.LinkRefID), Convert.ToInt64(model.RefId), Convert.ToInt32(0), diaryModel.Userid);
                        diaryModel.LinkRefID = 0;
                    }

                    i++;
                }







                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SaveUpdateSingleAction(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {

                string res = "0";
                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                //  var FileSettings = FileSetting.GetDISFileSetting();

                //upload action file 
                if (model.IsFile == "Y")
                {
                    model.ActionTakenFile = model.ActionTakenFile;
                }
                else
                {
                    //string urlA = "/MlaDiary/ActionFile";
                    string urlA = "/References/";
                    string directoryA = Server.MapPath(urlA); ;

                    if (!System.IO.Directory.Exists(directoryA))
                    {
                        System.IO.Directory.CreateDirectory(directoryA);
                    }
                    // int fileID = GetFileRandomNo();

                    string tempurlA1 = "~/TempFile";
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
                                string ext = Path.GetExtension(SourceFile);
                                string name = Path.GetFileName(page);
                                var fname = Guid.NewGuid().ToString();
                                string path1 = System.IO.Path.Combine(directoryA, fname + ext);
                                if (!string.IsNullOrEmpty(name))
                                {
                                    System.IO.File.Copy(SourceFile, path1, true);
                                    model.ActionTakenFile = urlA + fname + ext;
                                }
                            }
                        }
                        else
                        {
                            model.ActionTakenFile = null;
                        }
                    }
                    else
                    {
                        model.ActionTakenFile = null;
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
                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.Subject = model.Subject;
                diaryModel.ApplicantAddress = model.ApplicantAddress;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                diaryModel.DocmentType = model.DocmentType;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                diaryModel.ActionCode = model.ActionCodeMulti;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                diaryModel.ActionDescription = model.ActionDescriptionMulti;
                diaryModel.ActionTakenDate = DateTime.Now;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();

                diaryModel.MoveId = model.MoveId;
                if (model.SentType == "FromOffice")
                {
                    diaryModel.SentToUserId = model.SentFromUserId;
                    diaryModel.InboxType = "Dispatch";
                    diaryModel.SentType = "Diary";
                }
                else
                {
                    diaryModel.SentToUserId = model.SentToUserId;
                    if (diaryModel.FromDeptId == diaryModel.ToDeptId)
                    {
                        diaryModel.InboxType = "Dispatch";
                    }
                    else
                    {
                        diaryModel.InboxType = "Diary";
                    }
                    diaryModel.SentType = "Diary";

                }
                res = DiaryViewModel.UpdateMultiAction(diaryModel);
                if (model.isfund == 1) //|| model.isfund == 0
                {

                    FundSanctionModel fundsanction = new FundSanctionModel();
                    fundsanction.RefId = Convert.ToInt64(model.RefId);
                    fundsanction.FinYear = GetFinYear();
                    fundsanction.Remarks = "";
                    fundsanction.deptId = model.FromDeptId;
                    fundsanction.officeid = model.FromOfficeId;
                    fundsanction.Createdby = model.CreatedBy;

                    //Fund Add
                    if (model.ActionCode == 1)
                    {
                        if (model.fundtype_3 > 0)  //ReliefFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_3;
                            fundsanction.FundType = 3;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }

                        if (model.fundtype_4 > 0) //SmallSavingFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_4;
                            fundsanction.FundType = 4;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }
                        if (model.fundtype_13 > 0) //UntiedFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_13;
                            fundsanction.FundType = 13;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }

                        if (model.fundtype_14 > 0) //VivekiFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_14;
                            fundsanction.FundType = 14;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }

                        if (model.fundtype_15 > 0) //CattleFairFunds
                        {
                            fundsanction.ApproveDate = model.ApproveDate;
                            fundsanction.AmountSanctioned = 0;
                            fundsanction.ApprovedAmount = model.fundtype_15;
                            fundsanction.FundType = 15;
                            DiaryViewModel.SaveSanction(fundsanction);
                        }

                    }




                    DataSet ds, ds1 = new DataSet();
                    ds1 = FundFunctionModel.GetFundByRefId((Int64)diaryModel.RefId);

                    //Fund Update
                    if (model.ActionCode == 3)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds1.Tables[0].Rows)
                            {
                                int FundSanctionId = Convert.ToInt32(dr["Id"]);
                                int FundSancAmount = Convert.ToInt32(Request.Form["San_" + FundSanctionId]);
                                if (FundSancAmount > 0)
                                {
                                    fundsanction.AmountSanctioned = FundSancAmount;
                                    fundsanction.SanctionDate = model.SanctionDate;
                                    fundsanction.id = FundSanctionId;
                                    DiaryViewModel.UpdateSanction(fundsanction);
                                }
                            }

                        }
                    }








                    List<FundReportModel> lstfund = new List<FundReportModel>();
                    List<FundReportModel> sanctionedfund = new List<FundReportModel>();
                    string userid = CurrentSession.UserID;
                    ds = FundFunctionModel.GetFundAvailable(GetFinYear(), userid);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FundReportModel fr = new FundReportModel();
                        fr.FundTypeId = Convert.ToInt32(dr["DocTypeId"]);
                        fr.FundType = Convert.ToString(dr["FundType"]);
                        fr.FundAllocated = Convert.ToInt32(dr["FundAllocated"]);
                        //fr.FundApproved  = Convert.ToInt32(dr["ApprovedAmount"]);
                        //fr.FundSanctioned = Convert.ToInt32(dr["FundSanctioned"]);
                        fr.FundAvaiable = Convert.ToInt32(dr["FundAllocated"]) - Convert.ToInt32(dr["ApprovedAmount"]);
                        fr.DeptName = Convert.ToString(dr["deptname"]);
                        lstfund.Add(fr);
                    }

                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        FundReportModel fr = new FundReportModel();
                        fr.FundTypeId = Convert.ToInt32(dr["DocTypeId"]);
                        fr.FundType = Convert.ToString(dr["DocumentTypeName"]);
                        fr.FundApproved = Convert.ToInt32(dr["ApprovedAmount"]);
                        fr.FundSanctioned = Convert.ToInt32(dr["AmountSanctioned"]);
                        sanctionedfund.Add(fr);
                    }



                    model._LstFundReport = lstfund;
                    model._LstFundSactioned = sanctionedfund;

                }


                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult SaveUpdateSend(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {
                string res = "0";


                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                //  var FileSettings = FileSetting.GetDISFileSetting();

                //upload action file 
                if (model.IsFile == "Y")
                {
                    model.ActionTakenFile = model.ActionTakenFile;
                }
                else
                {
                    //string urlA = "/MlaDiary/ActionFile";
                    string urlA = "/References/";
                    string directoryA = Server.MapPath(urlA); ;

                    if (!System.IO.Directory.Exists(directoryA))
                    {
                        System.IO.Directory.CreateDirectory(directoryA);
                    }
                    // int fileID = GetFileRandomNo();

                    string tempurlA1 = "~/TempFile";
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
                                string ext = Path.GetExtension(SourceFile);
                                string name = Path.GetFileName(page);
                                var fname = Guid.NewGuid().ToString();
                                string path1 = System.IO.Path.Combine(directoryA, fname + ext);
                                if (!string.IsNullOrEmpty(name))
                                {
                                    System.IO.File.Copy(SourceFile, path1, true);
                                    model.ActionTakenFile = urlA + fname + ext;
                                }
                            }
                        }
                        else
                        {
                            model.ActionTakenFile = null;
                        }
                    }
                    else
                    {
                        model.ActionTakenFile = null;
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
                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.Subject = model.Subject;
                diaryModel.ApplicantAddress = model.ApplicantAddress;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                diaryModel.DocmentType = model.DocmentType;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                diaryModel.ActionCode = model.ActionCode;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                diaryModel.ActionDescription = model.ActionDescription;
                diaryModel.ActionTakenDate = DateTime.Now;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();

                diaryModel.MoveId = model.MoveId;
                if (model.SentType == "FromOffice")
                {
                    diaryModel.SentToUserId = model.SentFromUserId;
                    if (model.AssinedOfficeId != 0)
                    {
                        diaryModel.AssinedOfficeId = model.AssinedOfficeId;
                    }
                    else
                    {
                        diaryModel.AssinedOfficeId = 0;
                    }
                    diaryModel.InboxType = "Dispatch";
                    diaryModel.SentType = "Diary";
                }
                else
                {
                    diaryModel.SentToUserId = model.SentToUserId;
                    if (diaryModel.FromDeptId == diaryModel.ToDeptId)
                    {
                        diaryModel.InboxType = "Dispatch";
                    }
                    else
                    {
                        diaryModel.InboxType = "Diary";
                    }
                    diaryModel.SentType = "Diary";
                }

                res = DiaryViewModel.UpdateSend(diaryModel);



                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult SaveUpdateDispatchSend(MlaDiaryData model, HttpPostedFileBase file)
        {
            try
            {

                string res = "0";
                model.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                //  var FileSettings = FileSetting.GetDISFileSetting();

                //upload action file 
                if (model.IsFile == "Y")
                {
                    model.ActionTakenFile = model.ActionTakenFile;
                }
                else
                {
                    //string urlA = "/MlaDiary/ActionFile";
                    string urlA = "/References/";
                    string directoryA = Server.MapPath(urlA); ;

                    if (!System.IO.Directory.Exists(directoryA))
                    {
                        System.IO.Directory.CreateDirectory(directoryA);
                    }
                    // int fileID = GetFileRandomNo();

                    string tempurlA1 = "~/TempFile";
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
                                string ext = Path.GetExtension(SourceFile);
                                string name = Path.GetFileName(page);
                                var fname = Guid.NewGuid().ToString();
                                string path1 = System.IO.Path.Combine(directoryA, fname + ext);
                                if (!string.IsNullOrEmpty(name))
                                {
                                    System.IO.File.Copy(SourceFile, path1, true);
                                    model.ActionTakenFile = urlA + fname + ext;
                                }
                            }
                        }
                        else
                        {
                            model.ActionTakenFile = null;
                        }
                    }
                    else
                    {
                        model.ActionTakenFile = null;
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
                DiaryViewModel diaryModel = new DiaryViewModel();
                diaryModel.Subject = model.Subject;
                diaryModel.ApplicantAddress = model.ApplicantAddress;
                diaryModel.ApplicantEmail = model.ApplicantEmail;
                diaryModel.ApplicantMobile = model.ApplicantMobile;
                diaryModel.ApplicantName = model.ApplicantName;
                diaryModel.CreatedBy = model.CreatedBy;
                diaryModel.DiaryDate = model.DiaryDate;
                diaryModel.DiaryNumber = model.DiaryNumber;
                diaryModel.DispatchNumber = model.DispatchNumber;
                diaryModel.DocmentType = model.DocmentType;
                diaryModel.EnclosureFile = model.EnclosureFile;
                diaryModel.FromDeptId = model.FromDeptId;
                diaryModel.FromOfficeId = model.FromOfficeId;
                diaryModel.LetterDate = model.LetterDate;
                diaryModel.LetterNo = model.LetterNo;
                diaryModel.LinkRefID = model.LinkRefID;
                diaryModel.RefId = model.RefId;
                diaryModel.Subject = model.Subject;
                diaryModel.SubjectDetails = model.SubjectDetails;
                diaryModel.ToDeptId = model.ToDeptId;
                diaryModel.ToOfficeId = model.ToOfficeId;
                diaryModel.ActionCode = model.ActionCode;
                diaryModel.ActionAmountSanction = model.ActionAmountSanction;
                diaryModel.ActionDescription = model.ActionDescription;
                diaryModel.ActionTakenDate = model.ActiontakenDate;
                diaryModel.ActionTypeid = model.ActionTypeid;
                diaryModel.ActionTakenFile = model.ActionTakenFile;
                diaryModel.OfficeId = model.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();

                diaryModel.MoveId = model.MoveId;
                if (model.SentType == "FromOffice")
                {
                    diaryModel.SentToUserId = model.SentFromUserId;
                    if (model.AssinedOfficeId != 0)
                    {
                        diaryModel.AssinedOfficeId = model.AssinedOfficeId;
                    }
                    else
                    {
                        diaryModel.AssinedOfficeId = 0;
                    }
                    diaryModel.InboxType = "Dispatch";
                    diaryModel.SentType = "Dispatch";
                }
                else
                {
                    diaryModel.SentToUserId = model.SentToUserId;
                    if (diaryModel.FromDeptId == diaryModel.ToDeptId)
                    {
                        diaryModel.InboxType = "Dispatch";
                    }
                    else
                    {
                        diaryModel.InboxType = "Diary";
                    }

                    diaryModel.SentType = "Dispatch";
                }

                res = DiaryViewModel.UpdateSend(diaryModel);




                if (res != "0" && res != "-1")
                {
                    ErrorLog.WriteToLog("5");
                    return Json(res, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("Index");
                }
                else if (res == "0")
                {
                    ErrorLog.WriteToLog("5");
                    return Json("U", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ErrorLog.WriteToLog("6");
                    return Json("E", JsonRequestBehavior.AllowGet);
                }

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult UpdateActionDescription(string Refid, string ActionId, string ActionDes)
        {
            if (ActionId != "")
            {
                DiaryViewModel.UpdateActionDescription(Convert.ToInt64(Refid), Convert.ToInt32(ActionId), ActionDes);
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }

        public ActionResult DiaryRefWise(int status)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items = DiaryDashboard.DiaryRefWise(Convert.ToInt32(CurrentSession.OfficeId), status, Convert.ToString(CurrentSession.UserID.ToString()));
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DiaryRefWiseChart(int status, string diarydispatchvalue, string offid)
        {
            var newOffid = "";
            if (string.IsNullOrEmpty(offid))
            {
                newOffid = CurrentSession.OfficeId;
            }
            else
            {
                newOffid = offid;
            }
            List<SelectListItem> items = new List<SelectListItem>();
            items = DiaryDashboard.DiaryRefWiseChart(Convert.ToInt32(newOffid), status, diarydispatchvalue);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DiaryRefActionWise(int status)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items = DiaryDashboard.DiaryRefActionWise(Convert.ToInt32(CurrentSession.OfficeId), status, Convert.ToString(CurrentSession.UserID.ToString()));
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DispatchRefWise(int status)
        {
            string res = "0";
            List<SelectListItem> items = new List<SelectListItem>();
            items = DiaryDashboard.DispatchRefWise(Convert.ToInt32(CurrentSession.OfficeId), status, Convert.ToString(CurrentSession.UserID.ToString()));
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DispatchActionRefWise(int status)
        {
            string res = "0";
            List<SelectListItem> items = new List<SelectListItem>();
            items = DiaryDashboard.DispatchActionRefWise(Convert.ToInt32(CurrentSession.OfficeId), status, Convert.ToString(CurrentSession.UserID.ToString()));
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDispatchDataByDept(string deptId, int status, string startDt, string endDt, string since)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var response = dvm.GetDispatchDataByDeptnew(deptId, status, Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since);
            var dtt = dvm.GetTableRows(response);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetDispatchDataByDeptType(string deptId, int status, string startDt, string endDt, string since)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var response = dvm.GetDispatchDataByDeptType(deptId, status, Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since);
            var dtt = dvm.GetTableRows(response);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //[HttpGet]
        //public JsonResult GetDiaryDataByDeptt(string deptId, int status, string startDt, string endDt, string since)
        //{
        //    DiaryViewModel dvm = new DiaryViewModel();
        //    var response = dvm.getDashboardDataByDept(deptId, status, Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since);
        //    var dtt = dvm.GetTableRows(response);
        //    var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}


        //  public ActionResult GetDiaryDataByDepartment(int status, string startDt, string endDt, string since)


        [HttpGet]
        public JsonResult GetDiaryDataByDepttType(string deptId, int status, string startDt, string endDt, string since)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var response = dvm.getDashboardDataByDeptType(deptId, status, Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since);
            var dtt = dvm.GetTableRows(response);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult getAnalyticsBarChart()
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var response = dvm.getAnalyticsBarChart(CurrentSession.OfficeId);
            var dtt = dvm.GetTableRows(response);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        [HttpGet]
        public JsonResult GetDocumentsbyOfficeIdAnalyticsBarChart(string status, string diarydispatchvalue, string offid, string since, string startDt, string endDt)
        {
            var newOffid = "";
            if (string.IsNullOrEmpty(offid))
            {
                newOffid = CurrentSession.OfficeId;
            }
            else
            {
                newOffid = offid;
            }

            DiaryViewModel dvm = new DiaryViewModel();
            var response = dvm.GetDocumentsbyOfficeIdAnalyticsBarChart(newOffid, Convert.ToInt32(status), diarydispatchvalue, since, startDt, endDt);
            var dtt = dvm.GetTableRows(response);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }

        public ActionResult DashboardBind(int status, string startDt, string endDt, string since)
        {
            Session["since"] = "";
            Session["since"] = since;

            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = DiaryDashboard.getDashboardData(status, Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since, CurrentSession.UserID, out dtstatus);
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'>" +
                    "<td class='td-heading-2'></td>");
                sb = sb.Append("<td class='td-heading-2' id='tdFrom'>From</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='center td-heading' style='" + result1[0].ItemArray[2] + ";'>" + result1[0].ItemArray[1].ToString().Trim() + "</td>");
                    }
                }
                sb = sb.Append("<td class='td-heading total'>Total</td>");
                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
                sb = sb.Append("<td class='center td-heading' onclick='return expand(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading'>" + drDyn["deptName"].ToString() + "</td>");
                int Total = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardData(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardData(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 0 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            sb = sb.Append("<tr id='trtd0'>");
            sb = sb.Append("<td class='center td-heading'></td> ");
            sb = sb.Append("<td class='center td-heading total'>Total</td> ");

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                if (result1.Length > 0)
                {
                    int docid = (int)result1[0].ItemArray[0];
                    int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                    GTotal = GTotal + cnt;
                    sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardData(this)' DeptId='" + "0" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                }
            }
            sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardData(this)' DeptId='0' DocTypeId='0'>" + GTotal + " </a></td></tr> ");
            return Content(sb.ToString());
        }

        public ActionResult GetDiaryDataByDepartment(string tdid)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = DiaryDashboard.getDashboardDataByDepartment(out dtstatus);

            sb = sb.Append("<tr class='trofc" + tdid + "'>");
            sb = sb.Append("<td class='center td-heading'></td> ");
            int j = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {

                sb = sb.Append("<td  class='center td-heading'>" + drDyn["officename"].ToString().Trim() + "</td>");
                int Total = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDataByOffice(this)' DeptId='" + drDyn["DeptId"] + "'   OfcId='" + drDyn["officeid"] + "' DocTypeId='" + drDyn["DocumentTypeId"] + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDataByOffice(this)' DeptId='" + drDyn["DeptId"] + "'   OfcId='" + drDyn["officeid"] + "' DocTypeId='" + 0 + "'>" + Total + "</a></td>");
            }
            sb = sb.Append("</tr>");
            return Content(sb.ToString());
        }

        public ActionResult DashboardBindType(int status, string startDt, string endDt, string since)
        {
            Session["sinceType"] = "";
            Session["sinceType"] = since;

            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = DiaryDashboard.getDashboardDataType(status, Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since, out dtstatus);
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'>" +
                    "<td class='td-heading'></td>");
                sb = sb.Append("<td class='td-heading' id='tdFrom'>From</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim() + "</td>");
                    }
                }
                sb = sb.Append("<td class='td-heading' >Total</td>");
                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
                sb = sb.Append("<td onclick='return expandType(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading'>" + drDyn["deptName"].ToString() + "</td>");
                int Total = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDataType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDataType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 99 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            sb = sb.Append("<tr id='trtd0'>");
            sb = sb.Append("<td class='center td-heading'></td> ");
            sb = sb.Append("<td class='center td-heading'>Total</td> ");

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                if (result1.Length > 0)
                {
                    int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                    int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                    //int cnt = 0;
                    GTotal = GTotal + cnt;
                    sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDataType(this)' DeptId='" + "0" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                }
            }
            sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDataType(this)' DeptId='0' DocTypeId='99'>" + GTotal + " </a></td></tr> ");
            return Content(sb.ToString());
        }

        public ActionResult DashboardBindByDeptAndOfficeId(int status, string startDt, string endDt, string since, string officeid, string offName, string deptnameee)
        {
            Session["since"] = "";
            Session["since"] = since;
            Session["officeid"] = "";
            Session["officeid"] = officeid;

            Session["offNamenew"] = "";
            Session["offNamenew"] = offName;
            Session["deptnameee"] = "";
            Session["deptnameee"] = deptnameee;



            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = DiaryDashboard.getDashboardData(status, Convert.ToInt32(officeid), startDt, endDt, since, CurrentSession.UserID, out dtstatus);
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'>" +
                    "<td class='td-heading' ></td>");
                sb = sb.Append("<td class='td-heading' id='tdFrom'>From</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='center td-heading' style='" + result1[0].ItemArray[2] + ";'>" + result1[0].ItemArray[1].ToString().Trim() + "</td>");
                    }
                }
                sb = sb.Append("<td class='td-heading'>Total</td>");
                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr style='cursor:pointer;' id='trtd" + drDyn["deptId"] + "'>");
                sb = sb.Append("<td  class='center td-heading' style='cursor:pointer;' onclick='return expand(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading'>" + drDyn["deptName"].ToString() + "</td>");
                int Total = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardData(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardData(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 0 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            sb = sb.Append("<tr id='trtd0'>");
            sb = sb.Append("<td class='center td-heading'></td> ");
            sb = sb.Append("<td class='center td-heading'>Total</td> ");

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                if (result1.Length > 0)
                {
                    int docid = (int)result1[0].ItemArray[0];
                    int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                    GTotal = GTotal + cnt;
                    sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardData(this)' DeptId='" + "0" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                }
            }
            sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardData(this)' DeptId='0' DocTypeId='0'>" + GTotal + " </a></td></tr> ");
            return Content(sb.ToString());
        }

        [HttpGet]
        public JsonResult BindActionStatus(string startDt, string endDt, string since)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListnew(Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since, Convert.ToString(CurrentSession.UserID));
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult BindActionStatusMla(string since, string reffiletype)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListMla(Convert.ToInt32(CurrentSession.OfficeId), since, Convert.ToString(CurrentSession.UserID), reffiletype);
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult BindActionStatusMlaD(string since, string reffiletype)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListMlaD(Convert.ToInt32(CurrentSession.OfficeId), since, Convert.ToString(CurrentSession.UserID), reffiletype);
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        [HttpGet]
        public JsonResult BindActionStatusType(string startDt, string endDt, string since)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListnewType(Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since);
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult BindActionStatusDispatch(string startDt, string endDt, string since)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListDispatch(Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since, Convert.ToString(CurrentSession.UserID));
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult BindActionStatusDispatchType(string startDt, string endDt, string since)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListDispatchType(Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since);
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult BindActionStatusAnalytics(string OfficeId, string startDt, string endDt, string since)
        {
            var newOffid = "";
            if (string.IsNullOrEmpty(OfficeId))
            {
                newOffid = CurrentSession.OfficeId;
            }
            else
            {
                newOffid = OfficeId;
            }
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListnew(Convert.ToInt32(newOffid), startDt, endDt, since, Convert.ToString(CurrentSession.UserID));
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult BindActionStatusAnalyticsDispatch(string OfficeId, string startDt, string endDt, string since)
        {
            var newOffid = "";
            if (string.IsNullOrEmpty(OfficeId))
            {
                newOffid = CurrentSession.OfficeId;
            }
            else
            {
                newOffid = OfficeId;
            }
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListDispatch(Convert.ToInt32(newOffid), startDt, endDt, since, Convert.ToString(CurrentSession.UserID));
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult BindActionStatusByDeptAndOfficeId(string startDt, string endDt, string since, string OfficeId)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListnew(Convert.ToInt32(OfficeId), startDt, endDt, since, Convert.ToString(CurrentSession.UserID));
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpGet]
        public JsonResult BindActionStatusDispatchByDeptAndOfficeId(string startDt, string endDt, string since, string OfficeId)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var actList = dvm.GetAllActionTypeListDispatch(Convert.ToInt32(OfficeId), startDt, endDt, since, Convert.ToString(CurrentSession.UserID));
            var dtt = dvm.GetTableRows(actList);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public static string ConvertIntoJson(DataTable dt)
        {
            var jsonString = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                jsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                        jsonString.Append("\"" + dt.Columns[j].ColumnName + "\":\""
                            + dt.Rows[i][j].ToString().Replace('"', '\"') + (j < dt.Columns.Count - 1 ? "\"," : "\""));

                    jsonString.Append(i < dt.Rows.Count - 1 ? "}," : "}");
                }
                return jsonString.Append("]").ToString();
            }
            else
            {
                return "[]";
            }
        }

        public ActionResult GetAllDiaryDataByStatus(string status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetAllDiaryDataByStatus(Convert.ToInt32(CurrentSession.OfficeId), status, startDt, endDt, since);

            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();
            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dairySearchList", model);
        }

        public ActionResult GetAllDispatchDataByStatus(string status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetAllDispatchDataByStatus(Convert.ToInt32(CurrentSession.OfficeId), status, startDt, endDt, since);
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();
            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dairySearchList", model);
        }

        public ActionResult DashDispatchboardBind(int status, string startDt, string endDt, string since)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus = new DataTable();
            DataTable dt = DiaryDashboard.getDispatchDashboardData(Convert.ToInt32(status), Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since, Convert.ToString(CurrentSession.UserID), out dtstatus);
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'><td class='td-heading'></td>");
                sb = sb.Append("<td class='td-heading' id='tdFrom'>To</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='center td-heading' style='" + result1[0].ItemArray[2] + ";'>" + result1[0].ItemArray[1] + "</td>");
                    }
                }
                sb = sb.Append("<td class='td-heading-2'>Total</td>");
                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
                sb = sb.Append("<td onclick='return expandD(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading'>" + drDyn["deptName"] + "</td>");
                int Total = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchData(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchData(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 99 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            sb = sb.Append("<tr id='trtd0'>");
            sb = sb.Append("<td class='center td-heading'></td> ");
            sb = sb.Append("<td class='center td-heading'>Total</td> ");

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                if (result1.Length > 0)
                {
                    int docid = (int)result1[0].ItemArray[0];
                    int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                    GTotal = GTotal + cnt;
                    sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchData(this)' DeptId='" + "0" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                }
            }
            sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchData(this)' DeptId='0' DocTypeId='0'>" + GTotal + " </a></td></tr> ");
            return Content(sb.ToString());
        }

        public ActionResult DashDispatchboardBindType(int status, string startDt, string endDt, string since)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus = new DataTable();
            DataTable dt = DiaryDashboard.getDispatchDashboardDataType(Convert.ToInt32(status), Convert.ToInt32(CurrentSession.OfficeId), startDt, endDt, since, out dtstatus);
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'><td class='td-heading'></td>");
                sb = sb.Append("<td class='td-heading' id='tdFrom'>To</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1] + "</td>");
                    }
                }
                sb = sb.Append("<td class='td-heading'>Total</td>");
                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
                sb = sb.Append("<td onclick='return expandDType(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center'>" + drDyn["deptName"] + "</td>");
                int Total = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchDataType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchDataType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 99 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            sb = sb.Append("<tr id='trtd0'>");
            sb = sb.Append("<td class='center td-heading'></td> ");
            sb = sb.Append("<td class='center td-heading'>Total</td> ");

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                if (result1.Length > 0)
                {
                    int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                    int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                    GTotal = GTotal + cnt;
                    sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchDataType(this)' DeptId='" + "0" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                }
            }
            sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchDataType(this)' DeptId='0' DocTypeId='99'>" + GTotal + " </a></td></tr> ");
            return Content(sb.ToString());
        }

        public ActionResult DashDispatchboardBindByDeptAndOfficeId(int status, string startDt, string endDt, string since, string officeid, string offName)
        {
            Session["officeid"] = "";
            Session["officeid"] = officeid;
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus = new DataTable();
            DataTable dt = DiaryDashboard.getDispatchDashboardData(Convert.ToInt32(status), Convert.ToInt32(officeid), startDt, endDt, since, Convert.ToString(CurrentSession.UserID), out dtstatus);
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'><td class='td-heading'></td>");
                sb = sb.Append("<td class='td-heading' id='tdFrom'>To</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='center td-heading' style='" + result1[0].ItemArray[2] + ";'>" + result1[0].ItemArray[1] + "</td>");
                    }
                }
                sb = sb.Append("<td class='td-heading'>Total</td>");
                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
                sb = sb.Append("<td onclick='return expandD(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading'>" + drDyn["deptName"] + "</td>");
                int Total = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchData(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchData(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 0 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            sb = sb.Append("<tr id='trtd0'>");
            sb = sb.Append("<td class='center td-heading'></td> ");
            sb = sb.Append("<td class='center td-heading'>Total</td> ");
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                if (result1.Length > 0)
                {
                    int docid = (int)result1[0].ItemArray[0];
                    int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                    GTotal = GTotal + cnt;
                    sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchData(this)' DeptId='" + "0" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                }
            }
            sb = sb.Append("<td class='center td-heading'><a onclick='GetDasdhboardDispatchData(this)' DeptId='0' DocTypeId='0'>" + GTotal + " </a></td></tr> ");
            return Content(sb.ToString());
        }

        public ActionResult EditMlaDairyForm(Int64 ID, string ActId = "", int MoveId = 0, Int64 MainRefId = 0, string forceShowUpdate = "")
        {
            try
            {
                DiaryViewModel diaryModel = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById(ID, DeptId);
                if (MoveId != 0) /*Set Read*/
                {
                    string isRead = DiaryViewModel.SetRead(MoveId);
                }
                MlaDiaryData model = new MlaDiaryData();
                model.ActionCode = diaryModel.ActionCode;
                model.MoveId = MoveId;
                model.MainRefId = MainRefId;
                model.CreatedBy = diaryModel.CreatedBy;
                model.LinkRefID = Convert.ToInt64(diaryModel.LinkRefID);
                model.FromDeptId = diaryModel.FromDeptId;
                model.FromOfficeId = diaryModel.FromOfficeId;
                model.DispatchNumber = diaryModel.DispatchNumber;
                model.ToDeptId = diaryModel.ToDeptId;
                model.ToOfficeId = diaryModel.ToOfficeId;
                model.DiaryNumber = diaryModel.DiaryNumber;
                model.DocmentType = diaryModel.DocmentType;
                model.LetterNo = diaryModel.LetterNo;
                model.LetterDate = diaryModel.LetterDate;
                if (diaryModel.LetterDate != null)
                {
                    model.LetterDateNew = Convert.ToDateTime(diaryModel.LetterDate.ToString()).ToString("dd/MM/yyyy");
                }
                model.Subject = diaryModel.Subject;
                model.SubjectDetails = diaryModel.SubjectDetails;
                model.EnclosureFile = diaryModel.EnclosureFile;
                model.ApplicantName = diaryModel.ApplicantName;
                model.ActiontakenDate = diaryModel.ActionTakenDate;
                model.ApplicantAddress = diaryModel.ApplicantAddress;
                model.ApplicantMobile = diaryModel.ApplicantMobile;
                model.ApplicantEmail = diaryModel.ApplicantEmail;
                model.DiaryDate = diaryModel.DiaryDate;
                model.officename = diaryModel.FromOffice;
                model.deptname = diaryModel.FromDept;
                model.RefId = (Int64)diaryModel.RefId;
                model.FromDept = diaryModel.FromDept.ToString();
                model.FromOffice = diaryModel.FromOffice.ToString();
                model.ToDept = diaryModel.ToDept.ToString();
                model.ToOffice = diaryModel.ToOffice.ToString();
                model.isfund = diaryModel.isfund;
                model.IsSignedApprovalPDf = diaryModel.IsSignedApprovalPDf;
                model.IsApprovalPDf = diaryModel.IsApprovalPDf;
                model.DocmentType = diaryModel.DocmentType.ToString();
                model.DocumentTypeName = diaryModel.DocumentTypeName.ToString();
                model.ToOffice = diaryModel.ToOffice.ToString();
                model.CreatedDate = diaryModel.CreatedDate;
                model.CurrentDate = diaryModel.CurrentDate;
                model.ReferencePriorityType = diaryModel.ReferencePriorityType;
                model.ReferenceTerm = diaryModel.ReferenceTerm;
                model.MapId = CurrentSession.MapId.ToString();
                model.isLetter = diaryModel.isLetter.ToString();
                model.agendaId = diaryModel.agendaId;
                model.agendaDate = diaryModel.agendaDate;
                model.filetype = diaryModel.filetype;
                model.SubjectPB = diaryModel.SubjectPB;
                model.SubjectDetailsPB = diaryModel.SubjectDetailsPB;
                model.StatementImplementation_local = diaryModel.StatementImplementation_local;
                model.MultiFileModel = diaryModel.MultiFileModel;
                model.isPartFile = diaryModel.IsPartFile;
                model.LinkedFromDeptName = diaryModel.LinkedFromDeptName;
                if (model.FromDeptId == model.ToDept)
                {
                    model.IsMeetingFile = "Yes";
                }
                else
                {
                    model.IsMeetingFile = "No";
                }
                model.ResCount = diaryModel.ResCount;
                model.ActName = diaryModel.ActName;
                model.ActionDescription = diaryModel.ActionDescription; //ActionSession
                //string ActionDescription = DiaryViewModel.GetActionSession(ID, CurrentSession.UserID);
                // model.ActionDescription = ActionDescription;
                model.ActionDescriptionMulti = "";
                //model.DocumentTypelist = DiaryViewModel.GetDocumentTypeList(CurrentSession.UserID.ToString());
                
                if (model.isLetter == "File")
                {
                    DiaryViewModel.LGAllDropdowns(model, CurrentSession.UserID.ToString(), "Diary", "F", 1, 0, Convert.ToInt32(model.agendaId), model.DocumentTypeName, model.RefId, CurrentSession.OfficeId);
                    //model.DocumentTypelist = DiaryViewModel.GetDiaryFileDocumentTypeList(CurrentSession.UserID.ToString(), "F");
                    //model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Diary", 1);
                    //model.ActionTypeList_Ref = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Diary", 0);
                    //model.Act = DiaryViewModel.BindAct();
                }
                else
                {
                    DiaryViewModel.LGAllDropdowns(model, CurrentSession.UserID.ToString(), "Diary", "R", 0, 0, Convert.ToInt32(model.agendaId), model.DocumentTypeName, model.RefId, CurrentSession.OfficeId);

                    //model.DocumentTypelist = DiaryViewModel.GetDiaryFileDocumentTypeList(CurrentSession.UserID.ToString(), "R");
                    //model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Diary", 0);
                    //model.ActionTypeList_Ref = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Diary", 0);

                    if (diaryModel.DocmentType.ToString() == "7")
                    {
                        model.DocumentTypelist = new SelectList(model.DocumentTypelist.Where(x => x.Value == "1" || x.Value == "2" || x.Value == "3" || x.Value == "7").ToList().OrderBy(x => x.Value), "Value", "Text");
                    }
                    else
                    {
                        model.DocumentTypelist = new SelectList(model.DocumentTypelist.Where(x => x.Value == "1" || x.Value == "2" || x.Value == "3").ToList().OrderBy(x => x.Value), "Value", "Text");
                    }

                    
                    model.HideRef = "Yes";
                }

                if (MainRefId != 0)
                {
                    model.HideUpdate = "Yes";
                }

                //added by joginder
                //model.fileacessingUrl= BlobStorage.GetStorageAcessingPath();
               
                if (diaryModel.ProceedingPdf.ToString() != "")
                {
                    model.ProceedingPdf = model.fileacessingUrl + "/" + diaryModel.ProceedingPdf.ToString();
                }
                else
                {
                    model.ProceedingPdf = "";
                }
                if (diaryModel.ProceedingCommCommentPdf.ToString() != "")
                {
                    model.ProceedingCommCommentPdf = model.fileacessingUrl + "/" + diaryModel.ProceedingCommCommentPdf.ToString();
                }
                else
                {
                    model.ProceedingCommCommentPdf = "";
                }

               if (diaryModel.SignedProceedingCommissionerPath.ToString() != "")
                {
                    model.SignedProceedingCommissionerPath = model.fileacessingUrl + "/" + diaryModel.SignedProceedingCommissionerPath.ToString();
                }
                else
                {
                    model.SignedProceedingCommissionerPath = "";
                }

                //close

                //model.ActionListFilter = DiaryViewModel.ActionListFilter(CurrentSession.UserID.ToString(), "Diary", 0, ID);
                model.SentFromOffice = diaryModel.FromOfficeId.ToString();
                model.SentToOffice = diaryModel.ToOfficeId.ToString();

                //model.FromOfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.FromDeptId, "0");
                
                //model.ToOfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.ToDeptId, CurrentSession.OfficeId);
                
                //model.ToUserList = DiaryViewModel.GetToUserlist(model.RefId);
                
                //model.FromUserList = DiaryViewModel.GetFromUserlist(model.RefId);

                //model.MLAOfficeList = DiaryViewModel.GetOfficeByDept("UDD0001", CurrentSession.OfficeId);

                model.MLAOfficeList = new SelectList(
                        model.MLAOfficeList
                            .Where(x => x.Value == CurrentSession.OfficeId.ToString()),
                        "Value",
                        "Text"
                    );
                //model.MlaUserList = DiaryViewModel.GetToUserlist(0);
                //if (CurrentSession.DeptID == "68") // Check MLA
                //{
                //    string LastSentByUserId = DiaryViewModel.GetSentByUserId(ID, CurrentSession.UserID);
                //    model.SentToUserId = LastSentByUserId;
                //    model.SentFromUserId = LastSentByUserId;

                //    //model.SentFromUserId = "D2E76386-E625-4D38-BDBB-B161508B9098~1";
                //}

                //model.mDepartmentList = DiaryViewModel.getDepartment();
                //model.CMApproval = DiaryViewModel.CMApprovedList(model.RefId);
                //model.ReferencePriorityTypeList = DiaryViewModel.GetPriorityTypeList();
                //model.OfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.ToDeptId, "0");
                model.PaperEntryType = "NewEntry";
                model.BtnCaption = "Save";
                model.PaperEntryType = "EditEntry";
                model.BtnCaption = "Update";
                List<DiaryActionData> lstDiaryaction = new List<DiaryActionData>();
                List<DiaryActionData> MainfileDiaryaction = new List<DiaryActionData>();

                List<Note> lstnote = new List<Note>();
                foreach (var item in diaryModel.lstDiaryActionViewModel)
                {
                    DiaryActionData diaryActionData = new DiaryActionData();

                    diaryActionData.ActionAmountSanction = item.ActionAmount;
                    diaryActionData.ActionBy = item.ActionBy;
                    diaryActionData.ActionDate = item.ActionDate;
                    diaryActionData.ActionDescription = item.ActionDescription;
                    diaryActionData.RefId = (Int64)item.RefId;
                    diaryActionData.OfficeId = item.OfficeId;
                    diaryActionData.Id = item.Id;
                    diaryActionData.Enclosure = item.Enclosure;
                    diaryActionData.ActionId = item.ActionCode;
                    diaryActionData.StatusName = item.StatusName;
                    diaryActionData.NoteId = item.NoteId;
                    diaryActionData.Freezed = item.Freezed;
                    diaryActionData.isDraft = item.isDraft;
                    lstDiaryaction.Add(diaryActionData);

                    Note note = new Note();
                    note.type = "choiceitem";
                    note.text = "Note:" + item.NoteId.ToString();
                    note.value = item.NoteId.ToString() + ":" + item.Id.ToString();
                    lstnote.Add(note);
                }

                model.MultiFileModel = diaryModel.MultiFileModel;
                model.linkedFilesList = diaryModel.linkedFilesList;
               // model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();

                string FileDraft = "";
                if (diaryModel.lstDiaryActionViewModel.Count > 0)
                {
                    FileDraft = diaryModel.lstDiaryActionViewModel.LastOrDefault().isDraft.ToString();
                    if (FileDraft == "1")
                    {
                        model.FileDraft = true;
                    }
                    else
                    {
                        model.FileDraft = false;
                    }
                }
                else
                {
                    model.FileDraft = true;
                }

                if (ActId != "")
                {
                    model.ActionDetailsList = lstDiaryaction.Where(x => x.Id <= Convert.ToInt32(ActId)).ToList();
                    model.HideUpdate = "Yes";
                }
                else
                {
                    model.ActionDetailsList = lstDiaryaction;
                    model.NoteList = lstnote;
                }
                if (model.filetype == "2")
                {
                    foreach (var item in diaryModel.mainfileDiaryActionViewModel)
                    {
                        DiaryActionData diaryActionData = new DiaryActionData();

                        diaryActionData.ActionAmountSanction = item.ActionAmount;
                        diaryActionData.ActionBy = item.ActionBy;
                        diaryActionData.ActionDate = item.ActionDate;
                        diaryActionData.ActionDescription = item.ActionDescription;
                        diaryActionData.RefId = (Int64)item.RefId;
                        diaryActionData.OfficeId = item.OfficeId;
                        diaryActionData.Id = item.Id;
                        diaryActionData.Enclosure = item.Enclosure;
                        diaryActionData.ActionId = item.ActionCode;
                        diaryActionData.StatusName = item.StatusName;
                        diaryActionData.NoteId = item.NoteId;
                        diaryActionData.Freezed = item.Freezed;
                        diaryActionData.isDraft = item.isDraft;
                        MainfileDiaryaction.Add(diaryActionData);
                    }

                    model.MainFileActionDetailsList = MainfileDiaryaction;
                }
                else
                {
                    model.MainFileActionDetailsList = MainfileDiaryaction;
                }
                
                if (MoveId == 0 && forceShowUpdate != "Yes")
                {
                    model.HideUpdate = "Yes";
                }
                model.isLGoffice = "N";
                if (CurrentSession.DeptID.StartsWith("LG") || CurrentSession.DeptID.StartsWith("UDD"))
                {
                    model.isLGoffice = "Y";
                }
                if (CurrentSession.DeptID.StartsWith("UDD"))
                {
                    model.ToOfficeList = new SelectList(
                       model.ToOfficeList
                           .Where(x => x.Value == "1220"),
                       "Value",
                       "Text"
                   );
                }
                model.WardList = DiaryViewModel.WardList();
                model.WardListItems = DiaryViewModel.GetProposalWards(model.RefId.ToString());
                if (model.WardListItems != null && model.WardListItems.Count > 0)
                {
                    model.SelectedWardIds = model.WardListItems.Select(x => x.WardId).ToArray();
                }
                if (model.isLetter == "File")
                {
                    return PartialView("_ActionTaken", model);
                }
                else
                {
                    return PartialView("_ActionReference", model);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult EditMlaDispatchForm(Int64 ID, string EditType = "", int MoveId = 0)
        {
            try
            {
                DiaryViewModel diaryModel = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDispatchById(ID, DeptId);
                if (MoveId != 0) /*Set Read*/
                {
                    string isRead = DiaryViewModel.SetRead(MoveId);
                }
                MlaDiaryData model = new MlaDiaryData();
                model.ActionCode = diaryModel.ActionCode;

                model.ActiontakenDate = diaryModel.ActionTakenDate;
                model.CreatedBy = diaryModel.CreatedBy;
                model.LinkRefID = (Int64)diaryModel.LinkRefID;
                model.FromDeptId = diaryModel.FromDeptId;
                model.FromOfficeId = diaryModel.FromOfficeId;
                model.DispatchNumber = diaryModel.DispatchNumber;
                model.ToDeptId = diaryModel.ToDeptId;
                model.ToOfficeId = diaryModel.ToOfficeId;
                model.DiaryNumber = diaryModel.DiaryNumber;
                model.DocmentType = diaryModel.DocmentType;
                model.LetterNo = diaryModel.LetterNo;
                model.LetterDate = diaryModel.LetterDate;
                if (diaryModel.LetterDate != null)
                {
                    model.LetterDateNew = Convert.ToDateTime(diaryModel.LetterDate.ToString()).ToString("dd/MM/yyyy");
                }

                model.Subject = diaryModel.Subject;
                model.SubjectDetails = diaryModel.SubjectDetails;
                model.EnclosureFile = diaryModel.EnclosureFile;
                model.ApplicantName = diaryModel.ApplicantName;
                model.ApplicantAddress = diaryModel.ApplicantAddress;
                model.ApplicantMobile = diaryModel.ApplicantMobile;
                model.ApplicantEmail = diaryModel.ApplicantEmail;
                model.DiaryDate = diaryModel.DiaryDate;
                model.officename = diaryModel.FromOffice;
                model.deptname = diaryModel.FromDept;
                model.RefId = (Int64)diaryModel.RefId;
                model.FromDept = diaryModel.FromDept.ToString();
                model.CurrentDate = diaryModel.CurrentDate;
                model.FromOffice = diaryModel.FromOffice.ToString();
                model.ToDept = diaryModel.ToDept.ToString();
                model.ToOffice = diaryModel.ToOffice.ToString();
                model.DocmentType = diaryModel.DocmentType.ToString();
                model.DocumentTypeName = diaryModel.DocumentTypeName.ToString();
                model.ToOffice = diaryModel.ToOffice.ToString();

                model.CreatedDate = diaryModel.CreatedDate;
                model.isLetter = diaryModel.isLetter.ToString();
                model.MapId = CurrentSession.MapId.ToString();
                model.MoveId = MoveId;
                string ActionDescription = DiaryViewModel.GetActionSession(ID, CurrentSession.UserID);
                model.ActionDescription = ActionDescription;
                if (model.isLetter == "File")
                {
                    model.DocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "F");
                    model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Dispatch", 1);
                    model.ActionTypeList_Ref = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Dispatch", 0);
                }
                else
                {
                    model.DocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "R");
                    model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Dispatch", 0);
                    model.ActionTypeList_Ref = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Dispatch", 0);
                    model.HideRef = "Yes";
                }


                model.FromUserList = DiaryViewModel.GetFromUserlist(model.RefId);
                model.ToUserList = DiaryViewModel.GetToUserlist(model.RefId);
                model.mDepartmentList = DiaryViewModel.getDepartment();
                model.ReferencePriorityTypeList = DiaryViewModel.GetPriorityTypeList();
                model.ReferencePriorityType = diaryModel.ReferencePriorityType;
                model.ReferenceTerm = diaryModel.ReferenceTerm;
                model.OfficeList = DiaryViewModel.GetOfficeByDept(model.DeptId, CurrentSession.OfficeId);
                model.PaperEntryType = "NewEntry";
                model.BtnCaption = "Save";
                model.PaperEntryType = "EditEntry";
                model.BtnCaption = "Update";
                List<DiaryActionData> lstDiaryaction = new List<DiaryActionData>();
                foreach (var item in diaryModel.lstDiaryActionViewModel)
                {
                    DiaryActionData diaryActionData = new DiaryActionData();
                    diaryActionData.ActionAmountSanction = item.ActionAmount;
                    diaryActionData.ActionBy = item.ActionBy;
                    diaryActionData.ActionDate = item.ActionDate;
                    diaryActionData.ActionDescription = item.ActionDescription;
                    diaryActionData.RefId = (Int64)item.RefId;
                    diaryActionData.OfficeId = item.OfficeId;
                    diaryActionData.Id = item.Id;
                    diaryActionData.Enclosure = item.Enclosure;
                    diaryActionData.StatusName = item.StatusName;
                    diaryActionData.NoteId = item.NoteId;
                    lstDiaryaction.Add(diaryActionData);
                }
                model.ActionDetailsList = lstDiaryaction;
                model.MeetingList = DiaryViewModel.GetMeetingList(CurrentSession.OfficeId);

                model.WardList = DiaryViewModel.WardList();
                model.WardListItems = DiaryViewModel.GetProposalWards(model.RefId.ToString());
                if (model.WardListItems != null && model.WardListItems.Count > 0)
                {
                    model.SelectedWardIds = model.WardListItems.Select(x => x.WardId).ToArray();
                }

                if (EditType == "Agenda" || EditType == "PassedAgenda")
                {
                    model.EditType = EditType;
                    return PartialView("_AgendaDispatchActionTaken", model);
                }
                else
                {
                    model.EditType = "";
                    if (model.isLetter == "File")
                    {
                        return PartialView("_DispatchActionTaken", model);
                    }
                    else
                    {
                        return PartialView("_ActionReference", model);
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult EditMlaDispatchFormOutbox(Int64 ID, string EditType = "", string ActId = "", int MoveId = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                else
                {

                    DiaryViewModel diaryModel = new DiaryViewModel();
                    string DeptId = CurrentSession.DeptID;
                    if (MoveId != 0) /*Set Read*/
                    {
                        string isRead = DiaryViewModel.SetRead(MoveId);
                    }
                    diaryModel = DiaryViewModel.GetDispatchById(ID, DeptId);
                    MlaDiaryData model = new MlaDiaryData();
                    CurrentSession.referenceId = ID;
                    model.MoveId = MoveId;
                    model.ActionCode = diaryModel.ActionCode;
                    model.ActiontakenDate = diaryModel.ActionTakenDate;
                    //model.CreatedBy = diaryModel.CreatedBy;
                    model.LinkRefID = (Int64)diaryModel.LinkRefID;
                    model.FromDeptId = diaryModel.FromDeptId;
                    model.FromOfficeId = diaryModel.FromOfficeId;
                    //model.DispatchNumber = diaryModel.DispatchNumber;
                    model.ToDeptId = diaryModel.ToDeptId;
                    model.ToOfficeId = diaryModel.ToOfficeId;
                    model.DiaryNumber = diaryModel.DiaryNumber;
                    model.DocmentType = diaryModel.DocmentType;
                    model.LetterNo = diaryModel.LetterNo;
                    model.LetterDate = diaryModel.LetterDate;
                    if (diaryModel.LetterDate != null)
                    {
                        model.LetterDateNew = Convert.ToDateTime(diaryModel.LetterDate.ToString()).ToString("dd/MM/yy");
                    }
                    model.Subject = diaryModel.Subject;
                    model.SubjectDetails = diaryModel.SubjectDetails;
                    // model.SubjectPB = diaryModel.SubjectPB;
                    //model.SubjectDetailsPB = diaryModel.SubjectDetailsPB;
                    model.EnclosureFile = diaryModel.EnclosureFile;
                    //model.ApplicantName = diaryModel.ApplicantName;
                    //model.ApplicantAddress = diaryModel.ApplicantAddress;
                    //model.ApplicantMobile = diaryModel.ApplicantMobile;
                    //model.ApplicantEmail = diaryModel.ApplicantEmail;
                    //model.DiaryDate = diaryModel.DiaryDate;
                    model.officename = diaryModel.FromOffice;
                    model.deptname = diaryModel.FromDept;
                    model.deptnamelocal = diaryModel.FromDeptLocal;
                    model.RefId = (Int64)diaryModel.RefId;
                    model.FromDept = diaryModel.FromDept.ToString();
                    model.CurrentDate = diaryModel.CurrentDate;
                    model.FromOffice = diaryModel.FromOffice.ToString();
                    model.ToDept = diaryModel.ToDept.ToString();
                    model.ToOffice = diaryModel.ToOffice.ToString();
                    //model.DocmentType = diaryModel.DocmentType.ToString();
                    model.DocumentTypeName = diaryModel.DocumentTypeName.ToString();
                    model.ToOffice = diaryModel.ToOffice.ToString();
                    model.CreatedDate = diaryModel.CreatedDate;
                    model.isLetter = diaryModel.isLetter.ToString();
                    model.MapId = CurrentSession.MapId.ToString();
                    // model.StatementImplementation_local = diaryModel.StatementImplementation_local;
                    // model.PressNoteENG = diaryModel.PressNoteENG;
                    //model.PressNotePun = diaryModel.PressNotePun;
                    model.isPartFile = diaryModel.IsPartFile;
                    model.eBookFile = diaryModel.eBookFile;
                    model.LinkList = diaryModel.LinkList;
                    model.MergeLinkList = diaryModel.MergeLinkList;
                    model.IsSup = false;
                    model.filetype = "";
                    //if (diaryModel.isSupFile.ToString() == "1")
                    //{
                    //    model.IsSup = true;
                    //    model.filetype = "Supplementary Agenda File";
                    //}
                    //else
                    //{

                    //}
                    //Agenda
                    model.agendaId = diaryModel.agendaId;
                    model._MeetingType = MeetingTypeModel.GetMeetingTypes();
                    model.AgendaApprove = diaryModel.AgendaApprove.ToString();
                    model.ProceedingApprove = diaryModel.ProceedingApprove.ToString();
                    model.ProposalAmount = Convert.ToDecimal(diaryModel.ProposalAmount.ToString());
                    if (diaryModel.DocmentType == "7")
                    {
                        model.HideUpdate = "Yes";
                    }
                    if (diaryModel.ActionCode == 5 || diaryModel.ActionCode == 26)
                    {
                        model.HideUpdate = "No";
                    }

                    if (model.isLetter == "File")
                    {
                        DiaryViewModel.BindAllDropdowns(model, CurrentSession.UserID.ToString(), "Dispatch", "F", 1, 0, Convert.ToInt32(model.agendaId), model.DocumentTypeName, model.RefId);
                        //model.DocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "F");
                        // model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Dispatch", 1);
                        //model.ActionTypeList_Ref = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Dispatch", 0);
                        //model.Act = DiaryViewModel.BindAct();
                    }
                    else
                    {
                        DiaryViewModel.BindAllDropdowns(model, CurrentSession.UserID.ToString(), "Dispatch", "R", 0, 0, Convert.ToInt32(model.agendaId), model.DocumentTypeName, model.RefId);
                        //model.DocumentTypelist = new SelectList(model.DocumentTypelist.Where(x => x.Value == "1" || x.Value == "2" || x.Value == "3").ToList().OrderBy(x => x.Value), "Value", "Text");
                        // model.DocumentTypelist = DiaryViewModel.GetDispatchDocumentTypeList(CurrentSession.UserID.ToString(), "R");
                        //model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Dispatch", 0);
                        //model.ActionTypeList_Ref = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Dispatch", 0);
                        model.HideRef = "Yes";
                    }

                   

                    if (CurrentSession.DeptID == diaryModel.FromDeptId)
                    {
                        model.ToOfficeList = new SelectList(model.ToOfficeList.Where(x => x.Value == "1220"), "Value", "Text", "1220");
                    }

                    if (diaryModel.AgendaPdf.ToString() != "")
                    {
                        model.AgendaPdf = model.fileacessingUrl + "/" + diaryModel.AgendaPdf.ToString();
                    }
                    else
                    {
                        model.AgendaPdf = "";
                    }

                    model.AgendaPdfName = diaryModel.AgendaPdf.ToString();
                    if (diaryModel.ProceedingPdf.ToString() != "")
                    {
                        model.ProceedingPdf = model.fileacessingUrl + "/" + diaryModel.ProceedingPdf.ToString();
                    }
                    else
                    {
                        model.ProceedingPdf = "";
                    }
                    if (diaryModel.ProceedingCommCommentPdf.ToString() != "")
                    {
                        model.ProceedingCommCommentPdf = model.fileacessingUrl + "/" + diaryModel.ProceedingCommCommentPdf.ToString();
                    }
                    else
                    {
                        model.ProceedingCommCommentPdf = "";
                    }

                    if (diaryModel.SignedProceedingMayorPath.ToString() != "")
                    {
                        model.SignedProceedingMayorPath = model.fileacessingUrl + "/" + diaryModel.SignedProceedingMayorPath.ToString();
                    }
                    else
                    {
                        model.SignedProceedingMayorPath = "";
                    }
                    if (diaryModel.SignedProceedingCommissionerPath.ToString() != "")
                    {
                        model.SignedProceedingCommissionerPath = model.fileacessingUrl + "/" + diaryModel.SignedProceedingCommissionerPath.ToString();
                    }
                    else
                    {
                        model.SignedProceedingCommissionerPath = "";
                    }

                    model.ProceedingPdfName = diaryModel.ProceedingPdf.ToString();
                    model.SentFromOffice = diaryModel.FromOfficeId.ToString();
                    model.SentToOffice = diaryModel.ToOfficeId.ToString();
                    model.ActName = diaryModel.ActName;
                    //model.FromOfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.FromDeptId, "0");
                    //model.ToOfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.ToDeptId, "0");


                    //model.FromUserList = DiaryViewModel.GetFromUserlist(model.RefId);
                    //model.ToUserList = DiaryViewModel.GetToUserlist(model.RefId);
                    // model.ResCount = diaryModel.ResCount;
                    // string ActionDescription = DiaryViewModel.GetActionSession(ID, CurrentSession.UserID);
                    // model.ActionDescription = ActionDescription;
                    // model.ActionListFilter = DiaryViewModel.ActionListFilter(CurrentSession.UserID.ToString(), "Dispatch", 0, ID);
                    //model.MLAOfficeList = DiaryViewModel.GetOfficeByDept("68", "0");
                    //model.MlaUserList = DiaryViewModel.GetToUserlist(0);
                    //model.mDepartmentList = DiaryViewModel.getDepartment();
                    //model.CMApproval = DiaryViewModel.CMApprovedList(model.RefId);
                    //model.ReferencePriorityTypeList = DiaryViewModel.GetPriorityTypeList();
                    // model.ReferencePriorityType = diaryModel.ReferencePriorityType;
                    // model.ReferenceTerm = diaryModel.ReferenceTerm;
                    // model.OfficeList = DiaryViewModel.GetOfficeByDept(model.ToDeptId, CurrentSession.OfficeId);
                    model.PaperEntryType = "NewEntry";
                    model.BtnCaption = "Save";
                    model.PaperEntryType = "EditEntry";
                    model.BtnCaption = "Update";
                    List<DiaryActionData> lstDiaryaction = new List<DiaryActionData>();
                    List<Note> lstnote = new List<Note>();
                    foreach (var item in diaryModel.lstDiaryActionViewModel)
                    {
                        DiaryActionData diaryActionData = new DiaryActionData();
                        //diaryActionData.ActionAmountSanction = item.ActionAmount;
                        diaryActionData.ActionBy = item.ActionBy;
                        diaryActionData.ActionDate = item.ActionDate;
                        diaryActionData.ActionDescription = item.ActionDescription;
                        //diaryActionData.RefId = (Int64)item.RefId;
                        // diaryActionData.OfficeId = item.OfficeId;
                        diaryActionData.Id = item.Id;
                        diaryActionData.Enclosure = item.Enclosure;
                        diaryActionData.StatusName = item.StatusName;
                        diaryActionData.NoteId = item.NoteId;
                        diaryActionData.isDraft = item.isDraft;
                        //diaryActionData.MergeRefid = item.MergeRefId;
                        lstDiaryaction.Add(diaryActionData);
                        Note note = new Note();
                        note.type = "choiceitem";
                        note.text = "Note:" + item.NoteId.ToString();
                        note.value = item.NoteId.ToString() + ":" + item.Id.ToString();
                        lstnote.Add(note);

                    }
                    model.MultiFileModel = diaryModel.MultiFileModel;
                    string FileDraft = "";
                    if (diaryModel.lstDiaryActionViewModel.Count > 0)
                    {
                        FileDraft = diaryModel.lstDiaryActionViewModel.LastOrDefault().isDraft.ToString();
                        if (FileDraft == "1")
                        {
                            model.FileDraft = true;
                        }
                        else
                        {
                            model.FileDraft = false;
                        }

                        if (model.DocmentType == "17")
                        {
                            diaryModel.lstDiaryActionViewModel = diaryModel.lstDiaryActionViewModel.Where(x => x.ActionCode == 22).ToList();
                            if (diaryModel.lstDiaryActionViewModel.Count > 0)
                            {
                                FileDraft = diaryModel.lstDiaryActionViewModel.LastOrDefault().isDraft.ToString();
                                if (FileDraft == "1")
                                {
                                    model.FileDraft = true;
                                }
                                else
                                {
                                    model.FileDraft = false;
                                }
                            }
                            else
                            {
                                model.FileDraft = true;
                            }
                            
                          

                        }

                    }
                    else
                    {
                        model.FileDraft = true;
                    }


                    if (model.DocmentType == "17")
                    {
                        model.ActionTypeList = new SelectList(model.ActionTypeList.Where(x => x.Value == "22"), "Value", "Text", diaryModel.ActionCode);
                    }else if(model.DocmentType == "14")
                    {
                        model.ActionTypeList = new SelectList(model.ActionTypeList.Where(x => x.Value == "20"), "Value", "Text", diaryModel.ActionCode);
                    }
                    else if (model.DocmentType == "1")
                    {
                        model.ActionTypeList = new SelectList(model.ActionTypeList.Where(x => x.Value == "11"), "Value", "Text", diaryModel.ActionCode);
                    }
                   else  if (model.DocmentType == "7")
                    {
                        model.ActionTypeList = new SelectList(model.ActionTypeList.Where(x => x.Value == diaryModel.ActionCode.ToString()), "Value", "Text", diaryModel.ActionCode);
                    }



                    if (ActId != "")
                    {
                        model.ActionDetailsList = lstDiaryaction.Where(x => x.Id <= Convert.ToInt32(ActId)).ToList();
                        model.HideUpdate = "Yes";
                    }
                    else
                    {
                        model.ActionDetailsList = lstDiaryaction;
                        model.NoteList = lstnote;
                    }

                    // model.MeetingList = DiaryViewModel.GetMeetingList(CurrentSession.OfficeId);

                    //model.FileList = DiaryViewModel.GetFileList(CurrentSession.UserID, CurrentSession.OfficeId, model.RefId.ToString(), model.DocmentType);
                    //model._LstAgenda = AgendaModelFunction.GetAgendabyAgendaId(Convert.ToInt32(model.agendaId));


                    //model._MeetingSettingReferences = AgendaModelFunction.GetDishpatchReferences(Convert.ToInt32(model.agendaId));
                    //MeetingSettingModel ms = new MeetingSettingModel();
                    //ms.Count = model._MeetingSettingReferences.Count.ToString();
                    //ms.FileRefId = model.RefId.ToString();
                    //ms.freezeButton = true;
                    //model.MeetingSetting = ms;
                    model.ActList = model.Act;
                    //model.isLGoffice = "N";
                    //if (CurrentSession.DeptID.StartsWith("LG"))
                    //{
                    //    model.isLGoffice = "Y";
                    //}
                    //if (EditType == "Agenda" || EditType == "PassedAgenda")
                    //{
                    //    model.EditType = EditType;
                    //    return PartialView("_AgendaDispatchActionTaken", model);
                    //}
                    //else
                    //{


                    //}

                    model.WardList = DiaryViewModel.WardList();
                    model.WardListItems = DiaryViewModel.GetProposalWards(model.RefId.ToString());
                    if (model.WardListItems != null && model.WardListItems.Count > 0)
                    {
                        model.SelectedWardIds = model.WardListItems.Select(x => x.WardId).ToArray();
                    }

                    model.EditType = "";
                    if (model.isLetter == "File")
                    {
                        return PartialView("_DispatchActionTaken", model);
                    }
                    else
                    {
                        return PartialView("_ActionReference", model);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult EditSingleEntry(Int64 ID, string ActId = "", int MoveId = 0, Int64 MainRefId = 0)
        {
            try
            {
                DiaryViewModel diaryModel = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById(ID, DeptId);
                MlaDiaryData model = new MlaDiaryData();
                model.ActionCode = diaryModel.ActionCode;
                model.MoveId = MoveId;
                model.MainRefId = MainRefId;
                model.CreatedBy = diaryModel.CreatedBy;
                model.LinkRefID = (int)diaryModel.LinkRefID;
                model.FromDeptId = diaryModel.FromDeptId;
                model.FromOfficeId = diaryModel.FromOfficeId;
                model.DispatchNumber = diaryModel.DispatchNumber;
                model.ToDeptId = diaryModel.ToDeptId;
                model.ToOfficeId = diaryModel.ToOfficeId;
                model.DiaryNumber = diaryModel.DiaryNumber;
                model.DocmentType = diaryModel.DocmentType;
                model.LetterNo = diaryModel.LetterNo;
                model.LetterDate = diaryModel.LetterDate;
                model.ActId = diaryModel.ActId;
                if (diaryModel.LetterDate != null)
                {
                    model.LetterDateNew = Convert.ToDateTime(diaryModel.LetterDate.ToString()).ToString("dd/MM/yy");
                }
                model.Subject = diaryModel.Subject;
                model.SubjectDetails = diaryModel.SubjectDetails;
                model.SubjectPB = diaryModel.SubjectPB;
                model.SubjectDetailsPB = diaryModel.SubjectDetailsPB;
                model.EnclosureFile = diaryModel.EnclosureFile;
                model.StatementImplementation_local = diaryModel.StatementImplementation_local;
                model.ApplicantName = diaryModel.ApplicantName;
                model.ActiontakenDate = diaryModel.ActionTakenDate;
                model.ApplicantAddress = diaryModel.ApplicantAddress;
                model.ApplicantMobile = diaryModel.ApplicantMobile;
                model.ApplicantEmail = diaryModel.ApplicantEmail;
                model.DiaryDate = diaryModel.DiaryDate;
                model.officename = diaryModel.FromOffice;
                model.deptname = diaryModel.FromDept;
                model.RefId = (Int64)diaryModel.RefId;
                model.FromDept = diaryModel.FromDept.ToString();
                model.FromOffice = diaryModel.FromOffice.ToString();
                model.ToDept = diaryModel.ToDept.ToString();
                model.ToOffice = diaryModel.ToOffice.ToString();
                model.isfund = diaryModel.isfund;
                model.DocmentType = diaryModel.DocmentType.ToString();
                model.DocumentTypeName = diaryModel.DocumentTypeName.ToString();
                model.ToOffice = diaryModel.ToOffice.ToString();
                model.CreatedDate = diaryModel.CreatedDate;
                model.CurrentDate = diaryModel.CurrentDate;
                model.ReferencePriorityType = diaryModel.ReferencePriorityType;
                model.ReferenceTerm = diaryModel.ReferenceTerm;
                model.isLetter = diaryModel.isLetter.ToString();
                model.agendaId = diaryModel.agendaId;
                model.PressNotePun = diaryModel.PressNotePun;
                model.PressNoteENG = diaryModel.PressNoteENG;
                //model.MemberName = diaryModel.MemberName.ToString();
                //model.MinisteryName = diaryModel.MinisteryName.ToString();
                model.ActName = diaryModel.ActName;
                string ActionDescription = DiaryViewModel.GetActionSession(ID, CurrentSession.UserID);
                model.ActionDescription = ActionDescription;
                model.ActionDescriptionMulti = "";
                model.ResolutionNo = diaryModel.ResolutioNo;
                //model.DocumentTypelist = DiaryViewModel.GetDocumentTypeList(CurrentSession.UserID.ToString());
                if (model.isLetter == "File")
                {
                    model.DocumentTypelist = DiaryViewModel.GetDiaryFileDocumentTypeList(CurrentSession.UserID.ToString(), "F");
                    model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Diary", 1);
                    model.ActionTypeList_Ref = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Diary", 0);

                }
                else
                {
                    model.DocumentTypelist = DiaryViewModel.GetDiaryFileDocumentTypeList(CurrentSession.UserID.ToString(), "R");
                    model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Diary", 0);
                    model.ActionTypeList_Ref = DiaryViewModel.GetAllActionTypeList(ID, CurrentSession.UserID.ToString(), "Diary", 0);
                    model.HideRef = "Yes";
                }
                model.ActList = AgendaModelFunction.GetActList();
                if (MainRefId != 0)
                {
                    model.HideUpdate = "Yes";
                }
                if (CurrentSession.DeptID == "LG00001")
                {
                    model.HideUpdate = "Yes";
                }

                model.SentFromOffice = diaryModel.FromOfficeId.ToString();
                model.SentToOffice = diaryModel.ToOfficeId.ToString();
                model.FromOfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.FromDeptId, "0");
                model.ToOfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.ToDeptId, "0");
                model.FromUserList = DiaryViewModel.GetFromUserlist(model.RefId);
                model.ToUserList = DiaryViewModel.GetToUserlist(model.RefId);
                model.mDepartmentList = DiaryViewModel.getDepartment();
                model.ReferencePriorityTypeList = DiaryViewModel.GetPriorityTypeList();
                model.OfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.ToDeptId, "0");
                model.WardList = DiaryViewModel.WardList();
                model.WardListItems = DiaryViewModel.GetProposalWards(model.RefId.ToString());
                if (model.WardListItems != null && model.WardListItems.Count > 0)
                {
                    model.SelectedWardIds = model.WardListItems.Select(x => x.WardId).ToArray();
                }
                model.PaperEntryType = "NewEntry";
                model.BtnCaption = "Save";
                model.PaperEntryType = "EditEntry";
                model.BtnCaption = "Update";
                List<DiaryActionData> lstDiaryaction = new List<DiaryActionData>();

                if (diaryModel.isfund == 1 || diaryModel.isfund == 0)
                {
                    List<FundReportModel> lstfund = new List<FundReportModel>();
                    List<FundReportModel> sanctionedfund = new List<FundReportModel>();
                    DataSet ds, ds1 = new DataSet();
                    string userid = CurrentSession.UserID;
                    ds = FundFunctionModel.GetFundAvailable(GetFinYear(), userid);
                    ds1 = FundFunctionModel.GetFundByRefId((Int64)diaryModel.RefId);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FundReportModel fr = new FundReportModel();
                        fr.FundTypeId = Convert.ToInt32(dr["DocTypeId"]);
                        fr.FundType = Convert.ToString(dr["FundType"]);
                        fr.FundAllocated = Convert.ToInt32(dr["FundAllocated"]);
                        //fr.FundApproved  = Convert.ToInt32(dr["ApprovedAmount"]);//not related to MC
                        //fr.FundSanctioned = Convert.ToInt32(dr["FundSanctioned"]);
                        fr.FundAvaiable = Convert.ToInt32(dr["FundAllocated"]) - Convert.ToInt32(dr["ApprovedAmount"]);
                        fr.DeptName = Convert.ToString(dr["deptname"]);
                        lstfund.Add(fr);
                    }

                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        FundReportModel fr = new FundReportModel();
                        fr.FundSanctionId = Convert.ToInt32(dr["Id"]);
                        fr.FundTypeId = Convert.ToInt32(dr["DocTypeId"]);
                        fr.FundType = Convert.ToString(dr["DocumentTypeName"]);
                        fr.FundApproved = Convert.ToInt32(dr["ApprovedAmount"]);
                        fr.FundSanctioned = Convert.ToInt32(dr["AmountSanctioned"]); //ok
                        if (fr.FundSanctioned > 0)
                        {
                            fr.DisableClass = "readonly";
                        }
                        else
                        {
                            fr.DisableClass = "";
                        }
                        if (String.IsNullOrEmpty(dr["SanctionDate"].ToString()))
                        {
                            fr.SanctionDate = "";
                        }
                        else
                        {
                            fr.SanctionDate = Convert.ToDateTime(dr["SanctionDate"].ToString()).ToString("dd/MM/yyyy");

                        }

                        fr.ApproveDate = Convert.ToDateTime(dr["ApproveDate"].ToString()).ToString("dd/MM/yyyy");
                        sanctionedfund.Add(fr);
                    }
                    model._LstFundReport = lstfund.OrderBy(x => x.FundType).ToList();
                    model._LstFundSactioned = sanctionedfund.OrderBy(x => x.FundType).ToList();
                    model.CMApproval = DiaryViewModel.CMApprovedList(model.RefId);
                    model.MultiFileModel = diaryModel.MultiFileModel;
                }

                else
                {
                    model.Availablefund = 0;
                }

                foreach (var item in diaryModel.lstDiaryActionViewModel)
                {
                    DiaryActionData diaryActionData = new DiaryActionData();
                    diaryActionData.ActionAmountSanction = item.ActionAmount;
                    diaryActionData.ActionBy = item.ActionBy;
                    diaryActionData.ActionDate = item.ActionDate;
                    diaryActionData.ActionDescription = item.ActionDescription;
                    diaryActionData.RefId = (Int64)item.RefId;
                    diaryActionData.OfficeId = item.OfficeId;
                    diaryActionData.Id = item.Id;
                    diaryActionData.Enclosure = item.Enclosure;
                    diaryActionData.StatusName = item.StatusName;
                    diaryActionData.NoteId = item.NoteId;
                    diaryActionData.isDraft = item.isDraft;
                    lstDiaryaction.Add(diaryActionData);
                }
                if (ActId != "")
                {
                    model.ActionDetailsList = lstDiaryaction.Where(x => x.Id <= Convert.ToInt32(ActId)).ToList();
                    model.HideUpdate = "Yes";
                }
                else
                {
                    model.ActionDetailsList = lstDiaryaction;
                }
                model.isLGoffice = "N";
                if (CurrentSession.DeptID.StartsWith("LG"))
                {
                    model.isLGoffice = "Y";
                }
               
                return PartialView("_ActionSingle", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult RevertBack(Int64 ID, int MoveId = 0)
        {
            try
            {
                string response = "";
                DiaryViewModel diaryModel = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById(ID, DeptId);
                if (MoveId != 0) /*Revert Back UnRead*/
                {
                    response = DiaryViewModel.RevertBack(MoveId);

                }

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult EditMultiForm(string MultiId)
        {
            try
            {
                DiaryViewModel diaryModel = new DiaryViewModel();
                MlaDiaryData model = new MlaDiaryData();

                List<SendInboxJson> indexTable = new List<SendInboxJson>();
                indexTable = new JavaScriptSerializer().Deserialize<List<SendInboxJson>>(MultiId);
                int i = 1;
                string MultiRef = "";
                foreach (SendInboxJson index in indexTable)
                {
                    int order = i;
                    int MoveId = Convert.ToInt32(index.MoveId);
                    Int64 refid = Convert.ToInt64(index.RefId);

                    if (model.RefId == 0)
                    {
                        model.RefId = Convert.ToInt64(refid);
                    }
                    MultiRef = MultiRef + refid + ",";

                    i++;
                }

                MultiRef = MultiRef.Substring(0, MultiRef.Length - 1);
                model.MultiUpdate = MultiId;
                model.ActionDescriptionMulti = MultiRef;
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById(model.RefId, DeptId);
                string acttype = "";
                if (CurrentSession.DeptID == "LG00001" || CurrentSession.DeptID == "UDD0001")
                {
                    acttype = "Diary";
                    if (diaryModel.isLetter == "File")
                    {
                        DiaryViewModel.LGAllDropdowns(model, CurrentSession.UserID.ToString(), "Diary", "F", 1, 0, Convert.ToInt32(model.agendaId), model.DocumentTypeName, model.RefId, CurrentSession.OfficeId);
                        

                    }
                    else
                    {
                        DiaryViewModel.LGAllDropdowns(model, CurrentSession.UserID.ToString(), "Diary", "R", 0, 0, Convert.ToInt32(model.agendaId), model.DocumentTypeName, model.RefId, CurrentSession.OfficeId);
                        

                    }

                } else {
                    acttype = "Dispatch";
                    if (diaryModel.isLetter == "File")
                    {
                        DiaryViewModel.BindAllDropdowns(model, CurrentSession.UserID.ToString(), "Dispatch", "F", 1, 0, Convert.ToInt32(model.agendaId), model.DocumentTypeName, model.RefId);
                    }
                    else
                    {
                        DiaryViewModel.BindAllDropdowns(model, CurrentSession.UserID.ToString(), "Dispatch", "R", 0, 0, Convert.ToInt32(model.agendaId), model.DocumentTypeName, model.RefId);
                    }
                } 
                //if (diaryModel.isLetter == "File")
                //{
                //    model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(model.RefId, CurrentSession.UserID.ToString(), acttype, 1);
                //    model.ActionTypeList = new SelectList(model.ActionTypeList.Cast<SelectListItem>()
                //        .Where(x => x.Value == diaryModel.ActionCode.ToString()).ToList(),"Value","Text");
                //}
                //else
                //{
                //    model.ActionTypeList = DiaryViewModel.GetAllActionTypeList(model.RefId, CurrentSession.UserID.ToString(), "Dispatch", 0);
                //    model.ActionTypeList = new SelectList(model.ActionTypeList.Cast<SelectListItem>()
                //        .Where(x => x.Value == diaryModel.ActionCode.ToString()).ToList(), "Value", "Text");
                //}
                model.isLetter = diaryModel.isLetter;
                model.SentFromOffice = diaryModel.FromOfficeId.ToString();
                model.SentToOffice = diaryModel.ToOfficeId.ToString();
                model.FromOfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.FromDeptId, "0");
                model.ToOfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.ToDeptId, "0");
                if(CurrentSession.DeptID== diaryModel.FromDeptId)
                {
                    model.ToOfficeList = new SelectList(model.ToOfficeList.Where(x => x.Value == diaryModel.ToOfficeId.ToString()), "Value", "Text", diaryModel.ToOfficeId);
                }
                model.FromUserList = DiaryViewModel.GetFromUserlist(model.RefId);
                model.ToUserList = DiaryViewModel.GetToUserlist(model.RefId);
                //model.mDepartmentList = DiaryViewModel.getDepartment();
                //model.OfficeList = DiaryViewModel.GetOfficeByDept(diaryModel.ToDeptId, "0");
                //model.DocumentTypelist = DiaryViewModel.GetDiaryFileDocumentTypeList(CurrentSession.UserID.ToString(), "R");
                if (CurrentSession.DeptID == "UDD0001")
                {
                    model.MLAOfficeList = new SelectList(
                       model.MLAOfficeList
                           .Where(x => x.Value == CurrentSession.OfficeId.ToString()),
                       "Value",
                       "Text"
                   );
                }
                   
                model.FromDeptId = diaryModel.FromDeptId;
                model.FromOfficeId = diaryModel.FromOfficeId;
                model.ToDeptId = diaryModel.ToDeptId;
                model.ToOfficeId = diaryModel.ToOfficeId;
                model.FromDept = diaryModel.FromDept.ToString();
                model.FromOffice = diaryModel.FromOffice.ToString();
                model.ToDept = diaryModel.ToDept.ToString();
                model.ToOffice = diaryModel.ToOffice.ToString();
                model.CurrentDate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                model.PaperEntryType = "NewEntry";
                model.BtnCaption = "Save";
                model.PaperEntryType = "EditEntry";
                model.BtnCaption = "Update";


                return PartialView("_ActionTakenMulti", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult OpenPartFile(string refid, string linkrefid)
        {
            try
            {

                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                //if (deptid == "")
                //{
                data = dvm.PartFileList(Convert.ToInt64(linkrefid));
                //}
                //else
                //{
                //   data = dvm.GetDataForDispatchStatusTypeFilter(officeid, CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority);

                //}

                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult EditRevert(string MultiId)
        {
            try
            {
                DiaryViewModel diaryModel = new DiaryViewModel();
                MlaDiaryData model = new MlaDiaryData();
                string response = "";
                List<SendInboxJson> indexTable = new List<SendInboxJson>();
                indexTable = new JavaScriptSerializer().Deserialize<List<SendInboxJson>>(MultiId);
                int i = 1;
                string MultiRef = "";
                var createdBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                string userId = CurrentSession.UserID;
                int officeid = Convert.ToInt32(CurrentSession.OfficeId);
                foreach (SendInboxJson index in indexTable)
                {
                    int order = i;
                    int MoveId = Convert.ToInt32(index.MoveId);
                    Int64 refid = Convert.ToInt64(index.RefId);
                    DataSet ds = DiaryViewModel.GetMovementbyRefid(refid, refid);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string SentToUserId = dr["ToUserId"].ToString();
                            string Notification = "Proposals Added in Proceeding";
                            response = DiaryViewModel.InsertNotification(refid, MoveId, 5, Notification, SentToUserId, 0, userId);
                        }

                    }

                    response = DiaryViewModel.UpdateRevert(refid, MoveId, officeid, userId, createdBy);

                }
                model.MultiUpdate = MultiId;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SendToEOffice(string dairyId)
        {
            string response = "";
            DiaryViewModel diaryModel = new DiaryViewModel();
            string DeptId = CurrentSession.DeptID;
            diaryModel = DiaryViewModel.GetDiaryById(Convert.ToInt32(dairyId), DeptId);
            MlaDiaryData model = new MlaDiaryData();
            model.ActionCode = diaryModel.ActionCode;
            model.CreatedBy = diaryModel.CreatedBy;
            model.LinkRefID = (int)diaryModel.LinkRefID;
            model.FromDeptId = diaryModel.FromDeptId;
            model.FromOfficeId = diaryModel.FromOfficeId;
            model.DispatchNumber = diaryModel.DispatchNumber;
            model.ToDeptId = diaryModel.ToDeptId;
            model.ToOfficeId = diaryModel.ToOfficeId;
            model.DiaryNumber = diaryModel.DiaryNumber;
            model.DocmentType = diaryModel.DocmentType;
            model.LetterNo = diaryModel.LetterNo;
            model.LetterDate = diaryModel.LetterDate;
            model.Subject = diaryModel.Subject;
            model.SubjectDetails = diaryModel.SubjectDetails;
            model.EnclosureFile = diaryModel.EnclosureFile;
            model.ApplicantName = diaryModel.ApplicantName;
            model.ActiontakenDate = diaryModel.ActionTakenDate;
            model.ApplicantAddress = diaryModel.ApplicantAddress;
            model.ApplicantMobile = diaryModel.ApplicantMobile;
            model.ApplicantEmail = diaryModel.ApplicantEmail;
            model.DiaryDate = diaryModel.DiaryDate;
            model.officename = diaryModel.FromOffice;
            model.deptname = diaryModel.FromDept;
            model.RefId = (Int64)diaryModel.RefId;
            model.FromDept = diaryModel.FromDept.ToString();
            model.FromOffice = diaryModel.FromOffice.ToString();
            model.ToDept = diaryModel.ToDept.ToString();
            model.ToOffice = diaryModel.ToOffice.ToString();
            model.isfund = diaryModel.isfund;
            model.DocmentType = diaryModel.DocmentType.ToString();
            model.DocumentTypeName = diaryModel.DocumentTypeName.ToString();
            model.ToOffice = diaryModel.ToOffice.ToString();
            model.CreatedDate = diaryModel.CreatedDate;
            model.CurrentDate = diaryModel.CurrentDate;
            response = "Updated";
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //GetDocumentType

        [HttpPost]
        public ActionResult UpdateDiaryDocument(string refid, string documentid)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                response = dvm.UpdateDiaryDocument(Convert.ToInt64(refid), documentid, createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]

        public ActionResult SaveContent(string ActionDescription, int ActionCode, string refid)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                string userid = CurrentSession.UserID;
                response = DiaryViewModel.InsertActionSession(Convert.ToInt64(refid), userid, ActionDescription, ActionCode);

            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateToOffice(string refid, string ToOfficeId)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                response = dvm.UpdateDiaryToOffice(Convert.ToInt64(refid), ToOfficeId, createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult UpdateFromOffice(string refid, string FromOfficeId)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                response = dvm.UpdateDiaryFromOffice(Convert.ToInt64(refid), FromOfficeId, createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdatePriorityType(string refid, string PriorityType)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                response = dvm.UpdatePriorityType(Convert.ToInt64(refid), Convert.ToInt16(PriorityType), createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult updatesubject(string refid, string subject, string subjectold, string subject_local, string subjectold_local)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.UserID;
                response = dvm.updatesubject(Convert.ToInt64(refid), subject, subjectold, subject_local, subjectold_local, createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult updateamount(string refid, string amount, string amountold)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.UserID;
                response = dvm.updateamount(Convert.ToInt64(refid), Convert.ToDecimal(amount), Convert.ToDecimal(amountold), createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult updatesubjectdetails(string refid, string subjectdetail, string subjectdetail_local)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.UserID;
                response = dvm.updatesubjectdetail(Convert.ToInt64(refid), subjectdetail, subjectdetail_local, createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateTerm(string refid, string Term)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                response = dvm.UpdateTerm(Convert.ToInt64(refid), Convert.ToInt16(Term), createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddMeetingFile(string refid, string agendaid)
        {
            string response = "";
            try
            {
                string check = "";
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.UserID.ToString();
                DataTable data = dvm.GetDataForAgendaFile(refid, CurrentSession.UserID.ToString(), Convert.ToInt32(agendaid));
                foreach (DataRow dtRow in data.Rows)
                {
                    Int64 includedrefid = Convert.ToInt64(dtRow["RefId"]);
                    check = dvm.CheckInAgenda(Convert.ToInt64(includedrefid), Convert.ToInt32(agendaid));
                    if (check == "0")
                    {
                        response = dvm.AddToAgenda(Convert.ToInt64(includedrefid), Convert.ToInt32(agendaid), createdBy);
                    }

                }

            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddToAgenda(string refid, string agendaid)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.UserID.ToString();
                response = dvm.AddToAgenda(Convert.ToInt64(refid), Convert.ToInt32(agendaid), createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckInAgenda(string refid, string agendaid)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.CheckInAgenda(Convert.ToInt64(refid), Convert.ToInt32(agendaid));
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckInFile(string refid, string mainRefid)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.CheckInFile(Convert.ToInt64(refid), Convert.ToInt64(mainRefid));
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddToFile(string refid, string mainRefid, string MeetingId)
        {
            string response = "";
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var createdBy = CurrentSession.UserID.ToString();
                response = dvm.AddToFile(Convert.ToInt64(refid), Convert.ToInt64(mainRefid), Convert.ToInt32(MeetingId), createdBy);
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult UpdateArchive(string archIds)
        {
            string response = "";
            try
            {
                if (archIds != "")
                {
                    List<string> lstArch = archIds.Split(',').ToList();
                    foreach (string archid in lstArch)
                    {
                        response = DiaryViewModel.UpdateArchive(Convert.ToInt32(archid));
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateMerge(string MultiId)
        {
            string response = "";
            MlaDiaryData model = new MlaDiaryData();
            DiaryViewModel dvm = new DiaryViewModel();
            string AgendaNotApproved = "0"; Int64 refid = 0;
            List<SendInboxJson> indexTable = new List<SendInboxJson>();
            indexTable = new JavaScriptSerializer().Deserialize<List<SendInboxJson>>(MultiId);
            foreach (SendInboxJson index in indexTable)
            {
                refid = Convert.ToInt64(index.RefId);
                response = dvm.FileAgendaApprove(Convert.ToInt64(refid));
                if (response == "0")
                {
                    AgendaNotApproved = "1";
                }
            }
            if (AgendaNotApproved == "0")
            {

                string mainlinkedRefid = dvm.GetMainFile(Convert.ToInt64(refid));
                if (mainlinkedRefid != "0")
                {
                    refid = Convert.ToInt64(mainlinkedRefid);
                }
                string res = "0";
                DiaryViewModel diaryModel = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                diaryModel = DiaryViewModel.GetDiaryById(refid, DeptId);
                diaryModel.CreatedBy = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                diaryModel.RefId = 0;
                diaryModel.isLetter = diaryModel.isLetter;
                diaryModel.MeetingId = 0;
                diaryModel.ApplicantName = diaryModel.ApplicantName;
                diaryModel.CreatedBy = diaryModel.CreatedBy;
                diaryModel.DiaryDate = diaryModel.DiaryDate;
                diaryModel.DiaryNumber = diaryModel.DiaryNumber;
                diaryModel.DispatchNumber = diaryModel.DispatchNumber;
                diaryModel.EnclosureFile = diaryModel.EnclosureFile;
                diaryModel.FromDeptId = diaryModel.FromDeptId;
                diaryModel.FromOfficeId = diaryModel.FromOfficeId;
                diaryModel.LetterDate = diaryModel.LetterDate;
                diaryModel.LetterNo = diaryModel.LetterNo;
                diaryModel.LinkRefID = diaryModel.LinkRefID;
                diaryModel.Subject = diaryModel.Subject;
                diaryModel.SubjectDetails = diaryModel.SubjectDetails;
                diaryModel.ToDeptId = diaryModel.ToDeptId;
                diaryModel.ToOfficeId = diaryModel.ToOfficeId;
                diaryModel.ActionCode = diaryModel.ActionCode;
                diaryModel.ActionAmountSanction = diaryModel.ActionAmountSanction;
                diaryModel.ActionDescription = diaryModel.ActionDescription;
                diaryModel.ActionTakenDate = diaryModel.ActionTakenDate;
                diaryModel.ActionTypeid = diaryModel.ActionTypeid;
                diaryModel.ActionTakenFile = diaryModel.ActionTakenFile;
                diaryModel.OfficeId = diaryModel.OfficeId;
                diaryModel.Userid = CurrentSession.UserID.ToString();
                diaryModel.MapId = Convert.ToInt32(CurrentSession.MapId.ToString());
                diaryModel.ActId = diaryModel.ActId;
                diaryModel.SubjectPB = diaryModel.SubjectPB;
                diaryModel.SubjectDetailsPB = diaryModel.SubjectDetailsPB;
                diaryModel.StatementImplementation_local = diaryModel.StatementImplementation_local;
                diaryModel.PressNoteENG = diaryModel.PressNoteENG;
                diaryModel.PressNotePun = diaryModel.PressNotePun;

                res = DiaryViewModel.SaveDispatch(diaryModel);
                if (res != "")
                {
                    diaryModel.LinkRefID = Convert.ToInt64(res);
                    int ShowType = 0;
                    var ros = DiaryViewModel.UpdateDiaryMovement(Convert.ToInt64(refid), Convert.ToInt64(diaryModel.LinkRefID), diaryModel.CreatedBy, 4, 0, ShowType);

                    foreach (SendInboxJson index in indexTable)
                    {
                        Int64 mergerefid = Convert.ToInt64(index.RefId);
                        if (refid != Convert.ToInt64(diaryModel.LinkRefID))
                        {
                            DiaryViewModel.UpdateMerge(Convert.ToInt64(diaryModel.LinkRefID), mergerefid);
                            //int MoveId = Convert.ToInt32(index.MoveId);
                            //response = DiaryViewModel.UpdateArchive(Convert.ToInt32(MoveId));

                        }
                        // MultiRef = MultiRef + refid + ",";
                        response = "1";
                    }
                }


            }
            else
            {
                response = "0";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetDocumentType()
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetDocumentType(Convert.ToString(CurrentSession.UserID));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetAgendaDate(string type)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetAgendaDate(Convert.ToInt32(CurrentSession.OfficeId), type);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetLinkedReferenceId(string refid)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetLinkedReferenceId(refid);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDairyDispatchDepartments(string type, string deptid)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetDiaryDispatchDepartment(deptid);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetAgendaDepartment(string type)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetAgendaDepartment(Convert.ToInt32(CurrentSession.OfficeId), type, Convert.ToString(CurrentSession.UserID));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetAgendaStatus()
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetAgendaStatus(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetAgendaDocument()
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetAgendaDocument(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDocumentTypeD()
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetDocumentTypeD(Convert.ToString(CurrentSession.UserID));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetFinancialYearList()
        {
            var methodparameter = new List<KeyValuePair<string, string>>();
            int year = 0;
            int year1 = 0;
            year = System.DateTime.Today.Year - 1;
            year1 = year + 1;
            string Year2 = year + "-" + year1;
            int i = 1;
            List<SelectListItem> YearList = new List<SelectListItem>();
            //YearList.Add(new SelectListItem { Text = "Select", Value = "0" });
            while (year < System.DateTime.Today.Year + 1)
            {
                YearList.Add(new SelectListItem { Text = Year2, Value = Year2 });
                year = year + 1;
                year1 = year + 1;
                Year2 = year + "-" + year1;
                i = i + 1;
            }
            return YearList;
        }
        private static void ConvertBase64StringToBytes(string base64String, ref byte[] bytes1, ref string Type)
        {
            string[] split = base64String.Split(',');
            bytes1 = System.Convert.FromBase64String(base64String.Substring(base64String.IndexOf(',') + 1));
            if (base64String != "")
            {
                split = base64String.Split(',');
                Type = split[0];
            }
        }
        public ActionResult GetDivData()
        {
            return PartialView("_DivData");
        }

        public FileResult GetDiaryPrint(Int64 RefId)  //GenerateDiaryPrint
        {
            string msg = string.Empty;
            string body = string.Empty;
            DiaryViewModel mdl = new DiaryViewModel();
            DataTable dt = mdl.getPrintData(RefId);
            //StringBuilder body = new StringBuilder();
            string imagebody = string.Empty;
            string FTemplatepath = "~/Template/generalbranch.html";
            using (StreamReader reader = new StreamReader(Server.MapPath(FTemplatepath)))
            {
                body = reader.ReadToEnd();
                // body.Append(reader.ReadToEnd());
            }
            body = body.Replace("$$dcname$$", dt.Rows[0]["tooffice"].ToString());
            body = body.Replace("$$refid$$", dt.Rows[0]["RefId"].ToString());
            body = body.Replace("$$name$$", dt.Rows[0]["ApplicantName"].ToString());
            body = body.Replace("$$address$$", dt.Rows[0]["ApplicantAddress"].ToString());
            body = body.Replace("$$mobile$$", dt.Rows[0]["ApplicantMobile"].ToString());

            if (string.IsNullOrEmpty(dt.Rows[0]["LetterDate"].ToString()))
            {
            }
            else
            {
                DateTime filedate = Convert.ToDateTime(dt.Rows[0]["LetterDate"].ToString());
                body = body.Replace("$$date$$", filedate.ToString("dd/MM/yyyy"));
            }
            DateTime curdate = DateTime.Now;
            body = body.Replace("$$Curdate$$", curdate.ToString("dd/MM/yyyy"));
            body = body.Replace("$$fromOffice$$", dt.Rows[0]["fromoffice"].ToString());
            body = body.Replace("$$subject$$", dt.Rows[0]["Subject"].ToString());
            body = body.Replace("$$body$$", dt.Rows[0]["SubjectDetails"].ToString());

            converteroption();
            string finalpdfpath = "~/MLADIARY/PRINTDIARYDATA/" + RefId.ToString() + "_DispatchPrint.pdf";
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(body);
            string propertycardpath = Server.MapPath(finalpdfpath);
            doc.Save(propertycardpath);

            //string pdfFilePath = Server.MapPath(finalpdfpath);
            //byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            //doc.Close();
            //MemoryStream output = new MemoryStream();
            //FileStream _FileStream = new FileStream(propertycardpath, System.IO.FileMode.Create,
            //System.IO.FileAccess.Write);
            //_FileStream.Write(pdfBytes, 0, pdfBytes.Length);
            ////// close file stream
            //_FileStream.Close();
            return File(propertycardpath, "application/pdf");

            //MemoryStream output = new MemoryStream();

            //int crntyear = DateTime.Now.Year;
            //EvoPdf.Document document1 = new EvoPdf.Document();
            //document1.LicenseKey = "vjAjMSQhMSAoMSQ/ITEiID8gIz8oKCgo";
            //document1.CompressionLevel = PdfCompressionLevel.Best;
            //document1.Margins = new Margins(0, 0, 0, 0);
            //Font systemFontNormal = new Font("Arial Unicode MS Font", 10, GraphicsUnit.Point);
            //PdfFont embeddedSystemFontNormal = document1.AddFont(systemFontNormal);
            //PdfPage page = document1.Pages.AddNewPage(PdfPageSize.A4, new Margins(0, 0, 40, 40),
            //PdfPageOrientation.Portrait);
            //string htmlStringToConvert2 = body.ToString();
            //HtmlToPdfElement htmlToPdfElement2 = new HtmlToPdfElement(0, 0, 0, 0, htmlStringToConvert2, "");
            //AddElementResult addResult2 = page.AddElement(htmlToPdfElement2);
            //byte[] pdfBytes = document1.Save();
            //output.Write(pdfBytes, 0, pdfBytes.Length);
            //output.Position = 0;
            //string url = "/mlaDiary/" + "PrintDiaryData/";
            //string directory = Server.MapPath(url);
            //if (!Directory.Exists(directory))
            //{
            //    Directory.CreateDirectory(directory);
            //}
            //string path = Path.Combine(Server.MapPath("~" + url), "Print.pdf");
            //FileStream _FileStream = new FileStream(path, System.IO.FileMode.Create,
            //System.IO.FileAccess.Write);
            //_FileStream.Write(pdfBytes, 0, pdfBytes.Length);
            //// close file stream
            //_FileStream.Close();
            //return new FileStreamResult(output, "application/pdf");
        }


        public FileResult GetFundPrint(Int64 RefId, int actionid)  //GenerateDiaryPrint
        {
            string msg = string.Empty;
            string body = string.Empty;
            DiaryViewModel mdl = new DiaryViewModel();
            DataTable dt = mdl.getActionData(RefId, 1, actionid);
            //StringBuilder body = new StringBuilder();
            string imagebody = string.Empty;
            string FTemplatepath = "~/Template/fundletter.html";
            using (StreamReader reader = new StreamReader(Server.MapPath(FTemplatepath)))
            {
                body = reader.ReadToEnd();
                // body.Append(reader.ReadToEnd());
            }
            body = body.Replace("$$dcname$$", dt.Rows[0]["tooffice"].ToString());
            body = body.Replace("$$refid$$", dt.Rows[0]["RefId"].ToString());
            body = body.Replace("$$name$$", dt.Rows[0]["ApplicantName"].ToString());
            body = body.Replace("$$address$$", dt.Rows[0]["ApplicantAddress"].ToString());
            body = body.Replace("$$mobile$$", dt.Rows[0]["ApplicantMobile"].ToString());

            if (string.IsNullOrEmpty(dt.Rows[0]["LetterDate"].ToString()))
            {
            }
            else
            {
                DateTime filedate = Convert.ToDateTime(dt.Rows[0]["LetterDate"].ToString());
                body = body.Replace("$$date$$", filedate.ToString("dd/MM/yyyy"));
            }
            DateTime curdate = DateTime.Now;
            body = body.Replace("$$Curdate$$", curdate.ToString("dd/MM/yyyy"));
            body = body.Replace("$$fromOffice$$", dt.Rows[0]["fromoffice"].ToString());
            body = body.Replace("$$subject$$", dt.Rows[0]["Subject"].ToString());
            body = body.Replace("$$body$$", dt.Rows[0]["ActionDescription"].ToString());

            converteroption();
            string finalpdfpath = "~/MLADIARY/PRINTDIARYDATA/" + RefId.ToString() + "_DispatchPrint.pdf";
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(body);
            string propertycardpath = Server.MapPath(finalpdfpath);
            doc.Save(propertycardpath);

            //string pdfFilePath = Server.MapPath(finalpdfpath);
            //byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            //doc.Close();
            //MemoryStream output = new MemoryStream();
            //FileStream _FileStream = new FileStream(propertycardpath, System.IO.FileMode.Create,
            //System.IO.FileAccess.Write);
            //_FileStream.Write(pdfBytes, 0, pdfBytes.Length);
            ////// close file stream
            //_FileStream.Close();
            return File(propertycardpath, "application/pdf");

            //MemoryStream output = new MemoryStream();

            //int crntyear = DateTime.Now.Year;
            //EvoPdf.Document document1 = new EvoPdf.Document();
            //document1.LicenseKey = "vjAjMSQhMSAoMSQ/ITEiID8gIz8oKCgo";
            //document1.CompressionLevel = PdfCompressionLevel.Best;
            //document1.Margins = new Margins(0, 0, 0, 0);
            //Font systemFontNormal = new Font("Arial Unicode MS Font", 10, GraphicsUnit.Point);
            //PdfFont embeddedSystemFontNormal = document1.AddFont(systemFontNormal);
            //PdfPage page = document1.Pages.AddNewPage(PdfPageSize.A4, new Margins(0, 0, 40, 40),
            //PdfPageOrientation.Portrait);
            //string htmlStringToConvert2 = body.ToString();
            //HtmlToPdfElement htmlToPdfElement2 = new HtmlToPdfElement(0, 0, 0, 0, htmlStringToConvert2, "");
            //AddElementResult addResult2 = page.AddElement(htmlToPdfElement2);
            //byte[] pdfBytes = document1.Save();
            //output.Write(pdfBytes, 0, pdfBytes.Length);
            //output.Position = 0;
            //string url = "/mlaDiary/" + "PrintDiaryData/";
            //string directory = Server.MapPath(url);
            //if (!Directory.Exists(directory))
            //{
            //    Directory.CreateDirectory(directory);
            //}
            //string path = Path.Combine(Server.MapPath("~" + url), "Print.pdf");
            //FileStream _FileStream = new FileStream(path, System.IO.FileMode.Create,
            //System.IO.FileAccess.Write);
            //_FileStream.Write(pdfBytes, 0, pdfBytes.Length);
            //// close file stream
            //_FileStream.Close();
            //return new FileStreamResult(output, "application/pdf");
        }

        void converteroption()
        {
            // set converter options
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.DisplayFooter = false;
            converter.Options.DisplayHeader = false;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginRight = 20;
            converter.Options.MarginTop = 10;
            converter.Options.MarginBottom = 0;
            converter.Options.DisplayHeader = false;
            converter.Options.DisplayFooter = false;
            converter.Options.CssMediaType = HtmlToPdfCssMediaType.Screen;
            converter.Options.EmbedFonts = true;

        }

        public ActionResult GetDiaryData()
        {
            return PartialView("/Views/References/_ViewMLADiary.cshtml");
        }

        public PartialViewResult MLADiaryReport()
        {
            List<Dashboard> lstDashboard = new List<Dashboard>();
            LstDashboard model = new LstDashboard();
            try
            {
                DataSet ds = new DataSet();
                ds = DiaryViewModel.GetDataByDocType(Convert.ToInt32(CurrentSession.OfficeId), Convert.ToString(CurrentSession.UserID), "Letter");
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
                return PartialView("_MLADiaryDetails", model);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return PartialView("_MLADiaryDetails", model);
            }
        }

        public ActionResult MLADiaryReportDispatch()
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
                return PartialView("_MLADiaryDetailsDispatch", model);
            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return PartialView("_MLADiaryDetailsDispatch", model);
            }
        }

        [HttpGet]
        public JsonResult GetDataForDiary(string DocumentType, string status, string since)
        {
            try
            {

                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.GetDocumentName(DocumentType);
                Session["DocumentTypeName"] = "";
                Session["DocumentTypeName"] = response;

                var data = dvm.GetDataForDiary(Convert.ToInt32(CurrentSession.OfficeId), DocumentType, status, Convert.ToString(CurrentSession.UserID), since);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDataForDiaryStatus(string DocumentType, string status, string since, string deptid, string officeid, string term, string priority)
        {
            try
            {

                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.GetDocumentName(DocumentType);
                Session["DocumentTypeName"] = "";
                Session["DocumentTypeName"] = response;

                var data = dvm.GetDataForDiaryStatus(Convert.ToInt32(CurrentSession.OfficeId), DocumentType, status, Convert.ToString(CurrentSession.UserID), since, deptid, term, priority);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDataForDiaryStatusType(string DocumentType, string status, string since, string deptid, string officeid, string term, string priority, string reffiletype)
        {
            try
            {

                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.GetDocumentName(DocumentType);
                Session["DocumentTypeName"] = "";
                Session["DocumentTypeName"] = response;
                DataTable data = new DataTable();
                if (deptid == "")
                {
                    data = dvm.GetDataForDiaryStatusType(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), since, CurrentSession.DeptID, term, priority, reffiletype);
                }
                else
                {
                    data = dvm.GetDataForDiaryStatusTypeFilter(officeid, CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority, reffiletype);

                }

                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDataForDispatchStatusType(string DocumentType, string status, string since, string deptid, string officeid, string term, string priority, string notchecked, string reffiletype)
        {
            try
            {

                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.GetDocumentName(DocumentType);
                Session["DocumentTypeName"] = "";
                Session["DocumentTypeName"] = response;
                DataTable data = new DataTable();
                if (deptid == "")
                {
                    data = dvm.GetDataForDispatchStatusType(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), since, CurrentSession.DeptID, term, priority, reffiletype);
                }
                else
                {
                    data = dvm.GetDataForDispatchStatusTypeFilter(officeid, CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority, reffiletype);

                }

                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDataForDispatchInbox(string DocumentType, string status, string since, string deptid, string officeid, string term, string priority, string notchecked, string isLetter)
        {
            try
            {

                var response = "";


                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.GetDocumentName(DocumentType);
                Session["DocumentTypeName"] = "";
                Session["DocumentTypeName"] = response;
                DataTable data = new DataTable();
                //if (deptid == "")
                //{
                data = dvm.DispatchInbox(Convert.ToString(CurrentSession.UserID), isLetter, Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), DocumentType);
                //}
                //else
                //{
                //   data = dvm.GetDataForDispatchStatusTypeFilter(officeid, CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority);

                //}
                var refIds = data.AsEnumerable().Where(r => r["IsRead"] == DBNull.Value).Select(r => r["RefId"].ToString()).ToList();

                if (refIds.Any())
                {
                    UnReadDeleteDraft(Convert.ToString(CurrentSession.UserID), string.Join(",", refIds));
                }


                var dtt = dvm.GetTableRows(data);
                return new JsonResult
                {
                    Data = dtt,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDataForDispatchOutbox(string DocumentType, string status, string since, string till, string deptid, string officeid, string term, string priority, string notchecked, string isLetter)
        {
            try
            {

                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.GetDocumentName(DocumentType);
                Session["DocumentTypeName"] = "";
                Session["DocumentTypeName"] = response;
                DateTime fromDate = DateTime.Parse(since);
                DateTime toDate = DateTime.Parse(till);
                DataTable data = new DataTable();
                //if (deptid == "")
                // {
                data = dvm.DispatchOutboxNew(Convert.ToString(CurrentSession.UserID), isLetter, Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), DocumentType, fromDate,toDate);
                // }
                //else
                //{
                //    data = dvm.GetDataForDispatchStatusTypeFilter(officeid, CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority);

                //}

                var dtt = dvm.GetTableRows(data);

                return new JsonResult
                {
                    Data = dtt,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDataForDispatchArchive(string DocumentType, string status, string since, string deptid, string officeid, string term, string priority, string notchecked, string isLetter)
        {
            try
            {

                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.GetDocumentName(DocumentType);
                Session["DocumentTypeName"] = "";
                Session["DocumentTypeName"] = response;
                DataTable data = new DataTable();
                //if (deptid == "")
                //{
                data = dvm.DispatchArchive(Convert.ToString(CurrentSession.UserID), isLetter, DocumentType);
                //}
                //else
                //{
                //    data = dvm.GetDataForDispatchStatusTypeFilter(officeid, CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority);

                //}

                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetUserInbox(string UserId, string isLetter)
        {
            try
            {

                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                //if (deptid == "")
                //{
                data = dvm.UserInbox(UserId, isLetter);

                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDashboardDataForDispatchStatusTypeCount(string status, string since, string deptid, string officeid, string term, string priority, string notchecked, string reffiletype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetDashboardDataForDispatchStatusTypeCount(officeid, CurrentSession.OfficeId, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority, reffiletype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDashboardDataForDispatchTypeCount(string status, string since, string documentype, string deptid, string officeid, string term, string priority, string notchecked, string reffiletype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetDashboardDataForDispatchTypeCount(officeid, CurrentSession.OfficeId, status, Convert.ToString(CurrentSession.UserID), since, documentype, deptid, CurrentSession.DeptID, term, priority, reffiletype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetIconCountDispatch(string deptid, string officeid, string type, string status, string term, string priority, string since, string param, string reffiletype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetIconCountDispatch(deptid, CurrentSession.DeptID, officeid, CurrentSession.OfficeId, Convert.ToString(CurrentSession.UserID), type, status, term, priority, since, param, reffiletype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetIconCountDiary(string deptid, string officeid, string type, string status, string term, string priority, string since, string param, string reffiletype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetIconCountDiary(deptid, CurrentSession.DeptID, officeid, CurrentSession.OfficeId, Convert.ToString(CurrentSession.UserID), type, status, term, priority, since, param, reffiletype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDashboardDataForDispatchStatusTotalCount(string deptid, string officeid, string documentype, string status, string term, string priority, string since, string notchecked, string reffiletype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetDashboardDataForDispatchStatusTotalCount(deptid, CurrentSession.DeptID, officeid, CurrentSession.OfficeId, documentype, status, term, priority, since, Convert.ToString(CurrentSession.UserID), reffiletype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDashboardDataForDiaryStatusTotalCount(string deptid, string officeid, string documentype, string status, string term, string priority, string since, string reffiletype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetDashboardDataForDiaryStatusTotalCount(deptid, CurrentSession.DeptID, officeid, CurrentSession.OfficeId, documentype, status, term, priority, since, Convert.ToString(CurrentSession.UserID), reffiletype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDashboardDataForDispatchTypeTotalCount(string deptid, string officeid, string documentype, string status, string term, string priority, string since, string notchecked, string reffiletype)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetDashboardDataForDispatchTypeTotalCount(deptid, CurrentSession.DeptID, officeid, CurrentSession.OfficeId, documentype, status, term, priority, since, Convert.ToString(CurrentSession.UserID), reffiletype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDashboardDataForDiaryTypeTotalCount(string deptid, string officeid, string documentype, string status, string term, string priority, string since)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetDashboardDataForDiaryTypeTotalCount(deptid, CurrentSession.DeptID, officeid, CurrentSession.OfficeId, documentype, status, term, priority, since, Convert.ToString(CurrentSession.UserID));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDashboardDataForDiaryTypeCount(string status, string since, string documentype, string deptid, string officeid, string term, string priority)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetDashboardDataForDiaryTypeCount(officeid, CurrentSession.OfficeId, status, Convert.ToString(CurrentSession.UserID), since, documentype, deptid, CurrentSession.DeptID, term, priority);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDashboardDataForDiaryStatusTypeCount(string status, string since, string deptid, string officeid, string term, string priority, string documentype, string reffiletype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetDashboardDataForDiaryStatusTypeCount(officeid, CurrentSession.OfficeId, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority, documentype, reffiletype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDashboardDataForDiaryOfficeCount(string status, string since, string deptid, string officeid, string term, string priority, string documentype)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetDashboardDataForDiaryOfficeCount(officeid, CurrentSession.OfficeId, status, Convert.ToString(CurrentSession.UserID), since, deptid, CurrentSession.DeptID, term, priority, documentype);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult GetDataForDispatchDashboardStatus(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority, string notchecked, string reffiletype)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];

            if (deptid != "" || DocumentType != "" || status != "10" || term != "" || priority != "")
            {

                dt = DiaryDashboard.GetDataForDispatchDashboardStatusFilter(CurrentSession.OfficeId, officeid, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, since, deptid, term, priority, notchecked, out dtstatus, reffiletype);
            }
            else
            {
                dt = DiaryDashboard.GetDataForDispatchDashboardStatus(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus, reffiletype);
            }
            if (dt.Rows.Count > 0 || dt.Rows.Count == 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'>Department</td>");
                sb = sb.Append("<td class='center td-heading' >Total</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</td>");
                    }
                }



                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading' >Pendency(%)</td>");
                }


                sb = sb.Append("</tr>");


                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'></td>");
                sb = sb.Append("<td class='td-heading' id='tdFrom'></td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
                    }
                }

                //sb = sb.Append("<td class='center td-heading' ></td>");
                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading' ></td>");
                }


                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            double Gni = 0, Guip = 0, Gna = 0;
            double Gper;
            //row data bind

            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "' >");
                //  sb = sb.Append("<td onclick='return expandType(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading pointer' onclick='GoToStatusType(&#39" + drDyn["deptId"] + "&#39)'>" + drDyn["deptName"].ToString() + "</td>");




                //For Total
                int Total = 0; string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }

                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");

                double ni = 0, uip = 0, na = 0;
                double per;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading' ><a onclick='GetDispatchDasdhboardDataStatusType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Not Implemented")
                        {
                            ni = Convert.ToInt32(cnt);
                        }
                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Under Implementation")
                        {
                            uip = Convert.ToInt32(cnt);
                        }

                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "NA")
                        {
                            na = Convert.ToInt32(cnt);
                        }
                    }
                }
                double sum = ni + uip;
                double den = Convert.ToDouble(Total) - na;

                per = sum / den;
                double prcentage = per * 100;


                // sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");
                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading'>" + Math.Round(prcentage, 2) + "</td>");
                }

                sb = sb.Append("</tr>");
                j = j + 1;
            }
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr id='trtd0'>");
                // sb = sb.Append("<td class='center'></td> ");

                sb = sb.Append("<td class='center td-heading'>Total</td> ");
                int Gcnt = 0;


                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result11 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result11.Length > 0)
                    {
                        int docid = Convert.ToInt32(result11[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result11[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                    }
                }

                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td>");



                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        Gcnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;


                        GTotal = GTotal + Gcnt;
                        sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + Gcnt + "</a></td>");


                    }

                    if (dt.Columns[i].ColumnName != "deptId")
                    {


                        //if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Not Implemented")
                        //{
                        //    Gni = Convert.ToInt32(Gcnt);
                        //}
                        //if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Under Implementation")
                        //{
                        //    Guip = Convert.ToInt32(Gcnt);
                        //}

                        //if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "NA")
                        //{
                        //    Gna = Convert.ToInt32(Gcnt);
                        //}

                    }


                }

                double Gsum = Gni + Guip;
                double Gden = Convert.ToDouble(GTotal) - Gna;

                Gper = Gsum / Gden;
                double Gprcentage = Gper * 100;

                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading'>" + Math.Round(Gprcentage, 2) + "</td></tr> ");
                }

            }
            else
            {
                sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
            }
            return Content(sb.ToString());
        }


        public ActionResult GetDataForDispatchDashboardOffice(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority, string notchecked, string reffiletype)
        {
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            if (deptid != "" || DocumentType != "" || status != "10" || term != "" || priority != "")
            {

                dt = DiaryDashboard.GetDataForDispatchDashboardOfficeFilter(CurrentSession.OfficeId, officeid, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, since, deptid, term, priority, notchecked, out dtstatus, reffiletype);
            }
            else
            {
                dt = DiaryDashboard.GetDataForDispatchDashboardOffice(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus);
            }
            if (dt.Rows.Count > 0 || dt.Rows.Count == 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'>Office</td>");
                sb = sb.Append("<td class='td-heading' id='tdFrom'>Total</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</td>");
                    }
                }

                //sb = sb.Append("<td class='center td-heading' >Total</td>");
                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading' >Pendency(%)</td>");
                }


                sb = sb.Append("</tr>");


                sb = sb.Append("<tr role='row' class='table-heading'>"
                  /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'></td>");
                sb = sb.Append("<td class='td-heading' id='tdFrom'></td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
                    }
                }

                //sb = sb.Append("<td class='center td-heading' ></td>");
                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading' ></td>");
                }


                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            double Gni = 0, Guip = 0, Gna = 0;
            double Gper;
            //row data bind

            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["officeid"] + "' >");
                //  sb = sb.Append("<td onclick='return expandType(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading pointer' onclick='GoToStatusType(&#39" + drDyn["officeid"] + "&#39)'>" + drDyn["officename"].ToString().Trim() + "</td>");



                //For Total
                int Total = 0; string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }

                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalOffice(this)' deptId='" + drDyn["deptId"] + "' OfficeId='" + drDyn["OfficeId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");



                double ni = 0, uip = 0, na = 0;
                double per;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading' ><a onclick='GetDispatchDasdhboardDataOfficeCount(this)' deptId='" + drDyn["deptId"] + "' OfficeId ='" + drDyn["officeid"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Not Implemented")
                        {
                            ni = Convert.ToInt32(cnt);
                        }
                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Under Implementation")
                        {
                            uip = Convert.ToInt32(cnt);
                        }

                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "NA")
                        {
                            na = Convert.ToInt32(cnt);
                        }
                    }
                }
                double sum = ni + uip;
                double den = Convert.ToDouble(Total) - na;

                per = sum / den;
                double prcentage = per * 100;


                //sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalOffice(this)' deptId='" + drDyn["deptId"] + "' OfficeId='" + drDyn["OfficeId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");
                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading'>" + Math.Round(prcentage, 2) + "</td>");
                }

                sb = sb.Append("</tr>");
                j = j + 1;
            }
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr id='trtd0'>");
                // sb = sb.Append("<td class='center'></td> ");

                sb = sb.Append("<td class='center td-heading'>Total</td> ");
                int Gcnt = 0;

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

                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td>");


                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        Gcnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;


                        GTotal = GTotal + Gcnt;
                        sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + Gcnt + "</a></td>");


                    }
                    if (dt.Columns[i].ColumnName.ToString() != "deptId" && dt.Columns[i].ColumnName.ToString() != "OfficeId" && dt.Columns[i].ColumnName.ToString() != "officename")
                    {
                        //if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Not Implemented")
                        //{
                        //    Gni = Convert.ToInt32(Gcnt);
                        //}
                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Under Implementation")
                        {
                            Guip = Convert.ToInt32(Gcnt);
                        }

                        //if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "NA")
                        //{
                        //    Gna = Convert.ToInt32(Gcnt);
                        //}
                    }

                }

                double Gsum = Gni + Guip;
                double Gden = Convert.ToDouble(GTotal) - Gna;

                Gper = Gsum / Gden;
                double Gprcentage = Gper * 100;
                //  sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalStatus(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td>");

                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading'>" + Math.Round(Gprcentage, 2) + "</td></tr> ");
                }
            }
            else
            {
                sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
            }
            return Content(sb.ToString());
        }


        public ActionResult GetDataForDispatchDashboardType(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority, string notchecked, string reffiletype)
        {
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            if (deptid != "" || DocumentType != "" || status != "10" || term != "" || priority != "")
            {

                dt = DiaryDashboard.GetDataForDispatchDashboardTypeFilter(CurrentSession.OfficeId, officeid, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, since, deptid, term, priority, out dtstatus, reffiletype);
            }
            else
            {
                dt = DiaryDashboard.GetDataForDispatchDashboardType(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus, reffiletype);
            }
            if (dt.Rows.Count >= 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'>Type</td>");
                sb = sb.Append("<td class='td-heading'>Total</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td  class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</td>");
                    }
                }


                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td  class='td-heading'>Pendency(%)</td>");
                }


                sb = sb.Append("</tr>");


                sb = sb.Append("<tr role='row' class='table-heading'>"
                  /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'></td>");
                sb = sb.Append("<td class='td-heading'></td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td  class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
                    }
                }


                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td  class='td-heading'></td>");
                }


                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            double Gni = 0, Guip = 0, Gna = 0;
            double Gper;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "' >");
                //  sb = sb.Append("<td onclick='return expandType(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading'>" + drDyn["deptName"].ToString() + "</td>");

                //For Total
                int Total = 0; string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }


                sb = sb.Append("<td class='center td-heading total' ><a onclick='GetDispatchDasdhboardDataTotalType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");

                double ni = 0, uip = 0, na = 0;
                double per;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDispatchDasdhboardDataType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Not Implemented")
                        {
                            ni = Convert.ToInt32(cnt);
                        }
                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Under Implementation")
                        {
                            uip = Convert.ToInt32(cnt);
                        }

                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "NA")
                        {
                            na = Convert.ToInt32(cnt);
                        }
                    }

                }
                double sum = ni + uip;
                double den = Convert.ToDouble(Total) - na;

                per = sum / den;
                double prcentage = per * 100;


                //sb = sb.Append("<td class='center td-heading total' ><a onclick='GetDispatchDasdhboardDataTotalType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");

                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading' >" + Math.Round(prcentage, 2) + "</td>");
                }

                sb = sb.Append("</tr>");
                j = j + 1;
            }
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr id='trtd0'>");
                // sb = sb.Append("<td class='center td-heading'></td> ");

                sb = sb.Append("<td class='center td-heading'>Total</td> ");
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

                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDispatchDasdhboardDataTotalType(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td>");
                int Gcnt = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        Gcnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + Gcnt;
                        sb = sb.Append("<td class='center td-heading total " + dt.Columns[i].ColumnName.ToString().Replace(" ", "_") + "'><a onclick='GetDispatchDasdhboardDataTotalType(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + Gcnt + "</a></td>");

                    }

                    if (dt.Columns[i].ColumnName.ToString() != "deptId")
                    {
                        //if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Not Implemented")
                        //{
                        //    Gni = Convert.ToInt32(Gcnt);
                        //}
                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Under Implementation")
                        {
                            Guip = Convert.ToInt32(Gcnt);
                        }

                        //if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "NA")
                        //{
                        //    Gna = Convert.ToInt32(Gcnt);
                        //}
                    }

                }

                double Gsum = Gni + Guip;
                double Gden = Convert.ToDouble(GTotal) - Gna;

                Gper = Gsum / Gden;
                double Gprcentage = Gper * 100;

                if (SiteName == "CMNirdesh")
                {
                    sb = sb.Append("<td class='center td-heading'>" + Math.Round(Gprcentage, 2) + "</td></tr> ");
                }

            }
            else
            {
                sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
            }
            return Content(sb.ToString());
        }








        public ActionResult GetDataForDiaryDashboardType(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority, string reffiletype)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            if (deptid != "" || DocumentType != "" || status != "10" || term != "" || priority != "")
            {

                dt = DiaryDashboard.GetDataForDiaryDashboardTypeFilter(CurrentSession.OfficeId, officeid, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, since, deptid, term, priority, out dtstatus, reffiletype);
            }
            else
            {
                dt = DiaryDashboard.GetDataForDiaryDashboardType(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus, reffiletype);
            }
            if (dt.Rows.Count >= 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'>Type</td>");
                sb = sb.Append("<td class='td-heading total'>Total</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</td>");
                    }
                }

                sb = sb.Append("</tr>");


                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'></td>");
                sb = sb.Append("<td class='td-heading'></td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
                    }
                }

                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
                //  sb = sb.Append("<td onclick='return expandType(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading'>" + drDyn["deptName"].ToString() + "</td>");

                //For Total
                int Total = 0; string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }
                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");



                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);

                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDiaryDasdhboardDataType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");


                    }
                }

                //sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");

                sb = sb.Append("</tr>");
                j = j + 1;
            }
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr id='trtd0'>");
                // sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center td-heading'>Total</td> ");

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

                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalType(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td>");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;

                        sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalType(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");

                    }
                }

                sb = sb.Append("</tr> ");
            }
            else
            {
                sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
            }

            return Content(sb.ToString());
        }


        public ActionResult GetDataForDiaryDashboardStatus(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority, string reffiletype)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            if (deptid != "" || DocumentType != "" || status != "10" || term != "" || priority != "")
            {

                dt = DiaryDashboard.GetDataForDiaryDashboardStatusFilter(CurrentSession.OfficeId, officeid, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, since, deptid, term, priority, out dtstatus, reffiletype);
            }
            else
            {
                dt = DiaryDashboard.GetDataForDiaryDashboardStatus(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus, reffiletype);
            }
            if (dt.Rows.Count >= 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'>Department</td>");
                sb = sb.Append("<td class='td-heading total'>Total</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</td>");
                    }
                }

                sb = sb.Append("</tr>");

                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'></td>");
                sb = sb.Append("<td class='td-heading'></td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
                    }
                }

                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
                //  sb = sb.Append("<td onclick='return expandType(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading' onclick='GoToStatusType(&#39" + drDyn["deptId"] + "&#39)'>" + drDyn["deptName"].ToString() + "</td>");


                //For Total
                int Total = 0; string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }
                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");




                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDiaryDasdhboardDataStatusType(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                //sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' DeptId='" + drDyn["deptId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr id='trtd0'>");
                // sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center td-heading'>Total</td> ");

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
                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' DeptId='99' DocTypeId='10'>" + GTotal + " </a></td> ");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                        sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
            }
            else
            {
                sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
            }
            sb = sb.Append("</tr> ");


            return Content(sb.ToString());
        }

        public ActionResult GetDataForDiaryDashboardOffice(string DocumentType, string status, string deptid, string since, string officeid, string term, string priority, string reffiletype)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            if (deptid != "" || DocumentType != "" || status != "10" || term != "" || priority != "")
            {

                dt = DiaryDashboard.GetDataForDiaryDashboardOfficeFilter(CurrentSession.OfficeId, officeid, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, since, deptid, term, priority, out dtstatus, reffiletype);
            }
            else
            {
                dt = DiaryDashboard.GetDataForDiaryDashboardOffice(CurrentSession.OfficeId, DocumentType, status, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus, reffiletype);
            }
            if (dt.Rows.Count >= 0)
            {
                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'>Office</td>");
                sb = sb.Append("<td class='td-heading'>Total</td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</td>");
                    }
                }

                sb = sb.Append("</tr>");



                sb = sb.Append("<tr role='row' class='table-heading'>"
                   /* "<td class='center' style='width: 50px;background-color: rgb(28,80,147);'></td>"*/);
                sb = sb.Append("<td class='td-heading' id='tdFrom'></td>");
                sb = sb.Append("<td class='td-heading'></td>");
                //column bind
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
                    }
                }

                sb = sb.Append("</tr>");
            }
            int j = 0, GTotal = 0;
            //row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["OfficeId"] + "'>");
                //  sb = sb.Append("<td onclick='return expandType(this)' deptid='" + drDyn["deptId"] + "'   id='td" + drDyn["deptId"] + "' class='plus'></td> ");
                sb = sb.Append("<td  class='center td-heading' onclick='GoToStatusType(" + drDyn["OfficeId"] + ")'>" + drDyn["officename"].ToString().Trim() + "</td>");



                //For Total
                int Total = 0; string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }
                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalOffice(this)' deptId='" + drDyn["deptId"] + "' OfficeId='" + drDyn["OfficeId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");







                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a onclick='GetDiaryDasdhboardDataOffice(this)' deptId='" + drDyn["deptId"] + "' OfficeId ='" + drDyn["OfficeId"] + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
                //sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalOffice(this)' deptId='" + drDyn["deptId"] + "' OfficeId='" + drDyn["OfficeId"] + "' DocTypeId='" + 10 + "'>" + Total + "</a></td>");
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr id='trtd0'>");
                // sb = sb.Append("<td class='center'></td> ");
                sb = sb.Append("<td class='center td-heading'>Total</td> ");

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

                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' OfficeId='9999' DocTypeId='10'>" + GTotal + " </a></td> ");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                        sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' OfficeId='" + "9999" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
            }
            else
            {
                sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
            }
            sb = sb.Append("</tr> ");


            return Content(sb.ToString());
        }



        //[HttpGet]
        //public JsonResult GetDataForAgenda(string deptId, string Document, string status, string Term, string Priority, string since, string type, string AgendaOfficeId, string AgendaId)
        //{
        //    try
        //    {
        //        string userid = CurrentSession.UserID.ToString();
        //        DataTable data = new DataTable();
        //        DiaryViewModel dvm = new DiaryViewModel();
        //        //if (type == "Dispatch")
        //        //{
        //        //    data = dvm.GetDataForAgenda(deptId, CurrentSession.DeptID.ToString(), AgendaOfficeId, CurrentSession.OfficeId.ToString(), Document, status, Term, Priority, since);
        //        //}
        //        //else
        //        //{
        //        //    data = dvm.GetDataForAgendaDiary(deptId, CurrentSession.DeptID.ToString(), AgendaOfficeId, CurrentSession.OfficeId.ToString(), Document, status, Term, Priority, since, userid, Convert.ToInt32(AgendaId));
        //        //}

        //         data = dvm.GetDataForAgenda(deptId, CurrentSession.DeptID.ToString(), AgendaOfficeId, CurrentSession.OfficeId.ToString(), Document, status, Term, Priority, since);
        //       // data = dvm.GetDataForAgendaDiary(deptId, CurrentSession.DeptID.ToString(), AgendaOfficeId, CurrentSession.OfficeId.ToString(), Document, status, Term, Priority, since, userid, Convert.ToInt32(AgendaId));
        //        var dtt = dvm.GetTableRows(data);
        //        return Json(dtt, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [HttpGet]
        public JsonResult GetDataForAgenda(string FileRefid, string deptId, string Document, string status, string Term, string Priority, string since, string type, string AgendaOfficeId, string AgendaId)
        {
            try
            {
                string userid = CurrentSession.UserID.ToString();
                DataTable data = new DataTable();
                DiaryViewModel dvm = new DiaryViewModel();
                if (type == "Dispatch")
                {
                    data = dvm.GetDataForAgenda(FileRefid, deptId, CurrentSession.DeptID.ToString(), AgendaOfficeId, CurrentSession.OfficeId.ToString(), Document, status, Term, Priority, since);
                }
                else
                {
                    data = dvm.GetDataForAgendaDiary(FileRefid, deptId, CurrentSession.DeptID.ToString(), AgendaOfficeId, CurrentSession.OfficeId.ToString(), Document, status, Term, Priority, since, userid, Convert.ToInt32(AgendaId));
                }

                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDataForFile(string deptId, string Document, string status, string Term, string Priority, string since, string type, string AgendaOfficeId)
        {
            try
            {
                DataTable data = new DataTable();
                DiaryViewModel dvm = new DiaryViewModel();
                if (type == "Dispatch")
                {
                    data = dvm.GetDataForFile(deptId, CurrentSession.DeptID.ToString(), AgendaOfficeId, CurrentSession.OfficeId.ToString(), Document, status, Term, Priority, since, CurrentSession.UserID);
                }
                else
                {
                    // data = dvm.GetDataForAgendaDiary(deptId, CurrentSession.DeptID.ToString(), AgendaOfficeId, CurrentSession.OfficeId.ToString(), Document, status, Term, Priority, since);
                }

                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDataForMeeting(string MeetingId)
        {
            try
            {
                DataTable data = new DataTable();
                DiaryViewModel dvm = new DiaryViewModel();
                data = dvm.GetDataForMeeting(Convert.ToInt32(MeetingId));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDataFromFile(string Refid)
        {
            try
            {
                DataTable data = new DataTable();
                DiaryViewModel dvm = new DiaryViewModel();
                data = dvm.GetDataFromFile(Convert.ToInt64(Refid));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetIncludedAllActions(Int64 refid)
        {

            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                var userid = Convert.ToString(CurrentSession.UserID);
                string DeptId = CurrentSession.DeptID;
                string viewtype = "LG";
                data = dvm.GetIncludedAllActions(Convert.ToInt64(refid), viewtype, DeptId, userid);
                var dtt = mdl.GetTableRows(data);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public JsonResult GetIncludedRefActions(Int64 refid, string viewtype)
        {

            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                var userid = Convert.ToString(CurrentSession.UserID);
                string DeptId = CurrentSession.DeptID;
                data = dvm.GetIncludedRefActions(Convert.ToInt64(refid), viewtype, DeptId, userid);
                var dtt = mdl.GetTableRows(data);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public JsonResult GetIncludedRefNew(string refid, string viewtype)
        {
            try
            {
                DataTable data = new DataTable();
                DiaryViewModel dvm = new DiaryViewModel();
                string DeptId = CurrentSession.DeptID;
                string userId = CurrentSession.UserID;
                data = dvm.GetIncludedRefNew(Convert.ToInt64(refid), viewtype, DeptId, userId);
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
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        public JsonResult GetActionFilter(string refid)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = DiaryViewModel.ActionListFilter(CurrentSession.UserID.ToString(), "Diary", 0, Convert.ToInt64(refid));
                //  var dtt = dvm.GetTableRows(data);
                //return Json(dtt, JsonRequestBehavior.AllowGet);

                return new JsonResult()
                {
                    Data = data,
                    MaxJsonLength = 86753090,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                //return Json(dtt, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        public JsonResult GetSubjectDetails(string refid)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = DiaryViewModel.ActionListFilter(CurrentSession.UserID.ToString(), "Diary", 0, Convert.ToInt64(refid));
                //  var dtt = dvm.GetTableRows(data);
                //return Json(dtt, JsonRequestBehavior.AllowGet);

                return new JsonResult()
                {
                    Data = data,
                    MaxJsonLength = 86753090,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                //return Json(dtt, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult DeleteIncludeRef(string Lrid, string agendaid, string refid)
        {
            if (Lrid != "")
            {
                DiaryViewModel dvm = new DiaryViewModel();
                dvm.DeleteIncludedRef(Convert.ToInt32(Lrid), Convert.ToInt32(agendaid), Convert.ToInt64(refid));
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetAgendaType(string AgendaId)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var response = dvm.GetAgendaType(AgendaId);
            var dtt = dvm.GetTableRows(response);
            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetResAndRemarks(long refId)
        {
            DiaryViewModel dvm = new DiaryViewModel();

            // Call stored procedure / business layer
            var response = dvm.GetResAndRemarks(refId);

            // Convert to DataTable
            var dtt = dvm.GetTableRows(response);

            if (dtt == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            // var data = new[]
            //{
            //     new { RefId = refId, ResNo = 1, Remakes = "Approved" },
            //     new { RefId = refId, ResNo = 2, Remakes = "Pending review" },
            //     new { RefId = refId, ResNo = 3, Remakes = "Clarification required" }
            // };
        }

        [HttpPost]
        public JsonResult GetResAndRemarksbyId(string ids, long FileRefid)
        {
            DiaryViewModel dvm = new DiaryViewModel();
            var idList = ids.Split(',').Select(x => x.Trim()).ToList();

            DataTable dt = dvm.GetResAndRemarksbyIds(idList,Convert.ToInt64(FileRefid));

            var dtt = dvm.GetTableRows(dt);

            if (dtt == null)
            {
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        [HttpGet]
        public JsonResult GetDataForDispatch(string DocumentType, string status, string since)
        {
            try
            {

                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.GetDocumentName(DocumentType);
                Session["DocumentTypeName"] = "";
                Session["DocumentTypeName"] = response;

                var data = dvm.GetDataForDispatch(Convert.ToInt32(CurrentSession.OfficeId), DocumentType, status, Convert.ToString(CurrentSession.UserID), since);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult FilteredMLADiaryListForAgency(string DocumentType, string Office)
        {
            var response = "";
            DiaryViewModel dvm = new DiaryViewModel();
            response = dvm.GetDocumentName(DocumentType);
            Session["DocumentTypeName"] = "";
            Session["DocumentTypeName"] = response;


            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            if (Office == null)
            {
                Office = CurrentSession.OfficeId;
            }
            _Diary = DiaryViewModel.GetDiaryByFilter(Office, DocumentType);
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();
            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dairySearchList", model);
        }

        public ActionResult FilteredMLADiaryListForAgencyRefid(string refid)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            var Office = CurrentSession.OfficeId;
            _Diary = DiaryViewModel.GetDiaryByFiltersearchByRefid(Office, Convert.ToInt64(refid));
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dairySearchList", model);
        }

        public ActionResult FilteredMLADiaryListForDept(string deptId, string DocumentType, int status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetDiaryByDashboard(deptId, Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(DocumentType), Convert.ToInt32(status), Convert.ToString(CurrentSession.UserID), startDt, endDt, since);
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dairySearchList", model);
        }

        public ActionResult FilteredMLADiaryListForDeptType(string deptId, string DocumentType, int status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetDiaryByDashboardType(deptId, Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(DocumentType), Convert.ToInt32(status), startDt, endDt, since);
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            return PartialView("_dairySearchList", model);
        }


        public ActionResult FilteredMLADiaryListForOfc(string DeptId, string officeId, string DocumentType, int status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.FilteredMLADiaryListForOfc(DeptId, Convert.ToInt32(officeId), Convert.ToInt32(DocumentType), Convert.ToInt32(status), startDt, endDt, since);

            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;

            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();

            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;

            return PartialView("_dairySearchList", model);
        }

        public ActionResult DiaryDataByOfficeandDeptTypedashboard(string DeptId, string officeId, string DocumentType, int status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.DiaryDataByOfficeandDeptTypedashboard(DeptId, Convert.ToInt32(officeId), Convert.ToInt32(DocumentType), Convert.ToInt32(status), startDt, endDt, since);
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            return PartialView("_dairySearchList", model);
        }



        public ActionResult GetDataForDiaryDashboardPartFile(string RefrenceId, string Departmentid)
        {

            string status = "Diary";
            string type = "";
            string userid = CurrentSession.UserID;
            LstDashboard model = new LstDashboard();
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();

            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
            string deptd = CurrentSession.DeptID;



            sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
            sb.Append("<h6>Part File Detail :</h6><button style='float:right; margin-top:-32px' type='button' onclick='GotoUser()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
            sb.Append("</div>");
            sb.Append("</br>");


            sb.Append("<table id='test111' style='padding:10px;font-size: 12px;' class='table table-bordered'>");

            type = "3";
            dt = tPaperLaidV.GetMCStatDetailsListPartFile(RefrenceId, userid, out dtstatus, deptd);
            sb.Append("<thead>");
            if (CurrentSession.isDeptApex == "0")
            {
                sb = sb.Append("<tr role='row' class='table-heading'>");
                sb = sb.Append("<td style= 'text-align:center;' colspan= 8 class='' id='tdFrom'></td>");
                sb = sb.Append("<td style= 'text-align:center;' colspan= 4 class='td-heading' id='tdreflabel'>Pending</td>");
                sb = sb.Append("<td style= 'text-align:center;' colspan= 2 class='' id='tdreflabel'>Approved</td>");
                sb = sb.Append("</tr>");
            }
            sb.Append("<tr role='row' class='table-heading'>");
            sb.Append("<th class='text-center td-heading'></th>");
            sb.Append("<th class='text-center td-heading'>Meeting Date/File ID</th>");
            sb.Append("<th class='text-center td-heading' style='width:25%'>Subject</th>");
            sb.Append("<th class='text-center td-heading' >Currently With</th>");
            sb.Append("<th class='text-center td-heading' >House Resolutions</th>");

            foreach (var item1 in useraction)
            {
                sb.Append("<th class='text-center td-heading'>" + item1.DocumentTypeName + "</th>");
            }
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            int ct = 1;
            foreach (DataRow item in dt.Rows)
            {
                string Mainref = Convert.ToString(item["FileReference"]);
                var partfile = tPaperLaidV.GetPartfile(CurrentSession.UserID, Mainref);
                sb.Append("<tr role='row' id='trtdRef" + item["FileReference"] + "'>");
                sb.Append("<td class='text-center td-heading'><a class='green' id=" + item["FileReference"] + "><i class='ace-icon fa fa-pencil bigger-150'></i> </a>  </td>");
                sb.Append("<td class='text-center td-heading'>" +
                Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") +
                "<br><a id='" + item["FileReference"] + ">" +
                 item["FileReference"] + "</a></br>");
                if (partfile.Count > 0)
                {
                    sb.Append("</br>");
                    sb.Append("<h6>(Part File)</h6>");

                    foreach (var part in partfile)
                    {
                        sb.Append("<div>");

                        if (part.Islink)
                        {
                            sb.Append("<a class='blue' id=" + part.ReferenceId + "> " + part.ReferenceId + "</a>");
                        }
                        else
                        {
                            // Append plain text if Islink is false
                            sb.Append($"<span>{part.ReferenceId}</span>");
                        }

                        sb.Append("</div>");
                    }
                }

                sb.Append("</td>");
                sb.Append("<td class='td-heading'>" + item["MettingName"] + "</td>");
                sb.Append("<td class='td-heading'>" + item["CurrentlyWith"] + "</td>");
                sb.Append("<td class='text-center td-heading'><a id=" + item["FileReference"] + " documenttypeid=" + 222 + " deptid= " + "" + " officeid =" + "" + " Department= '" + "" + "'  FileType= " + "" + " RefType =" + "" + " MenuName=" + "" + ">" + item["Resolution"] + "</a></td>");


                foreach (var item1 in useraction)
                {
                    sb.Append("<td class='text-center center td-heading'><a id=" + item["FileReference"] + "  documenttypeid=" + item1.DocmentType + "  deptid= " + "" + " officeid =" + "" + " Department= '" + "" + "'  FileType= " + "" + " RefType =" + "" + " MenuName=" + "" + ">" + item[item1.DocumentTypeName] + "</a></td>");
                }
                sb.Append("</tr>");
                ct++;
            }



            sb.Append("</tbody>");
            sb.Append("</table>");
            return Content(sb.ToString());

        }

        public ActionResult GetDiaryDataByDate(string DiaryDate)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetDiaryDataByDate(Convert.ToInt32(CurrentSession.OfficeId), DiaryDate);
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            model.Header = "Reference Date wise";
            return PartialView("_dairySearchList", model);
        }

        public ActionResult GetDiaryActionDataByDate(string DiaryDate)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetDiaryActionDataByDate(Convert.ToInt32(CurrentSession.OfficeId), DiaryDate);
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            model.Header = "Action Date wise";
            return PartialView("_dairySearchList", model);
        }
        public ActionResult GetDispatchDataByDate(string DispatchDate)
        {

            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetDisptachDataByDate(Convert.ToInt32(CurrentSession.OfficeId), DispatchDate
            , Convert.ToString(CurrentSession.UserID.ToString()));
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();
            model.Header = "Dispatch Reference Date wise";

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dispatchSearchList", model);
        }
        public ActionResult GetActionDispatchDataByDate(string DispatchDate)
        {

            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetActionDisptachDataByDate(Convert.ToInt32(CurrentSession.OfficeId), DispatchDate, Convert.ToString(CurrentSession.UserID.ToString()));
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();
            model.Header = "Dispatch Action Date wise";

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dispatchSearchList", model);
        }

        public ActionResult FilteredMLADispatchListForDept(string deptId, string DocumentType, string status,
                string startDt, string endDt, string since)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetDiaryDispatchByDashboard(deptId, Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(DocumentType), Convert.ToInt32(status), Convert.ToString(CurrentSession.UserID), startDt, endDt, since);
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            model.Header = "Dispatch";
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dispatchSearchList", model);
        }


        public ActionResult FilteredMLADispatchListForDeptType(string deptId, string DocumentType, string status,
               string startDt, string endDt, string since)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.GetDiaryDispatchByDashboardType(deptId, Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(DocumentType), Convert.ToInt32(status), startDt, endDt, since);
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            model.Header = "Dispatch";
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dispatchSearchList", model);
        }

        public ActionResult FilteredMLADispatchListForDeptByOfc(string deptId, string OfficeId,
        string DocumentType, string status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            _Diary = DiaryViewModel.FilteredMLADispatchListForDeptByOfc(deptId, Convert.ToInt32(OfficeId), Convert.ToInt32(DocumentType), Convert.ToInt32(status), startDt, endDt, since);

            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            model.Header = "Dispatch";
            var Total = _Diary;

            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();

            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;

            return PartialView("_dispatchSearchList", model);
        }

        public ActionResult FilteredMLADiaryListForAgencyDispatch(string DocumentType, string Office, string reffiletype)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            if (string.IsNullOrEmpty(Office))
            {

                Office = Session["officeid"].ToString();
            }
            _Diary = DiaryViewModel.GetDiaryByFilterDispatch(Office, DocumentType, reffiletype);

            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;

            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();

            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;

            return PartialView("_dispatchSearchList", model);
        }

        public ActionResult FilteredMLADispatchListForAgencyRefid(Int64 refid)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();

            var Office = CurrentSession.OfficeId;

            _Diary = DiaryViewModel.GetSearchByRefidDispatch(Office, refid);

            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;

            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();

            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;

            return PartialView("_dispatchSearchList", model);
        }

        [HttpGet]
        public JsonResult DairyDispatchReport()
        {
            int officeid = Convert.ToInt16(CurrentSession.OfficeId);
            try
            {
                var response = DiaryViewModel.DairyDispatchReport(officeid);
                var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult DairyDispatchReportNew()
        {
            int officeid = Convert.ToInt16(CurrentSession.OfficeId);
            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();

                var response = mdl.DairyDispatchReportNew(officeid);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult ShowButton(string type)
        {
            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();
                var userid = Convert.ToString(CurrentSession.UserID);
                var response = mdl.ShowEditButtonDiarySearchList(userid, type);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public JsonResult getsubjectandsubjectDetails(Int64 refid)
        {

            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();
                var userid = Convert.ToString(CurrentSession.UserID);
                var response = mdl.getsubjectandsubjectDetails(refid);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult GetDiaryDocumentTypeList(string reftype)
        {

            try
            {
                DiaryViewModel mdl = new DiaryViewModel();
                var userid = Convert.ToString(CurrentSession.UserID);
                var response = mdl.GetDiaryDocumentTypeList(userid, reftype);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult GetDispatchDocumentTypeList(string reftype)
        {

            try
            {
                DiaryViewModel mdl = new DiaryViewModel();
                var userid = Convert.ToString(CurrentSession.UserID);
                var response = mdl.GetDispatchDocumentType(userid, reftype);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult LinkedReferences(Int64 refid)
        {

            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();
                var userid = Convert.ToString(CurrentSession.UserID);
                var OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                var response = mdl.LinkedReferences(userid, refid, OfficeId);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public void downloadDairyDispatchReportData()
        {
            DiaryDispatchModel mdl = new DiaryDispatchModel();
            int officeid = Convert.ToInt16(CurrentSession.OfficeId);
            var response = mdl.DairyDispatchReport(officeid);
            var gv = new System.Web.UI.WebControls.GridView();
            gv.DataSource = response;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DairyDispatch.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
        }

        [HttpGet]
        public JsonResult GetAllDiaryCount(string startDt, string endDt, string since)
        {
            int officeid = Convert.ToInt16(CurrentSession.OfficeId);
            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();
                var response = mdl.GetAllDiaryCount(officeid, startDt, endDt, since);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetAllDispatchCount(string startDt, string endDt, string since)
        {
            int officeid = Convert.ToInt16(CurrentSession.OfficeId);
            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();
                var response = mdl.GetAllDispatchCount(officeid, startDt, endDt, since);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult SearchByReferenceId(Int64 refid)
        {
            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();
                var response = mdl.SearchByreferenceId(refid);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult AdvancesearchForDispatch(string advanceparam)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();

            var officeid = CurrentSession.OfficeId;

            _Diary = DiaryViewModel.AdvanceSearchForDispatch(advanceparam, Convert.ToInt32(officeid));

            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;
            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dispatchSearchList", model);
        }

        public ActionResult AdvancesearchForDiary(string advanceparam)
        {
            List<DiaryViewModel> _Diary = new List<DiaryViewModel>();
            var officeid = CurrentSession.OfficeId;
            _Diary = DiaryViewModel.AdvanceSearchForDiary(advanceparam, Convert.ToInt32(officeid));
            DiaryViewModel model = new DiaryViewModel();
            model.lstDiaryViewModel = _Diary;
            var Total = _Diary;

            var Development = Total.Where(x => x.DocmentType == Convert.ToString(1)).Count();
            var Transfer = Total.Where(x => x.DocmentType == Convert.ToString(2)).Count();
            var CMReliefFund = Total.Where(x => x.DocmentType == Convert.ToString(3)).Count();
            var DiscretionaryGrant = Total.Where(x => x.DocmentType == Convert.ToString(4)).Count();
            var Grievance = Total.Where(x => x.DocmentType == Convert.ToString(5)).Count();
            var Demand = Total.Where(x => x.DocmentType == Convert.ToString(6)).Count();
            var Direction = Total.Where(x => x.DocmentType == Convert.ToString(7)).Count();
            var CMAnnouncements = Total.Where(x => x.DocmentType == Convert.ToString(8)).Count();

            ViewBag.Devlopement = Development;
            ViewBag.Transfer = Transfer;
            ViewBag.CMReliefFund = CMReliefFund;
            ViewBag.DiscretionaryGrant = DiscretionaryGrant;
            ViewBag.Grievance = Grievance;
            ViewBag.Demand = Demand;
            ViewBag.Direction = Direction;
            ViewBag.CMAnnouncements = CMAnnouncements;
            return PartialView("_dairySearchList", model);
        }

        [HttpGet]
        public JsonResult GetDepartments()


        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetDepartments();
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        public JsonResult GetSwitchUser()
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetSwitchUser(CurrentSession.loginEmailID);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult SetSwitchUser(string email)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                mUsers user = new mUsers();
                var data = dvm.GetSwitchUser(CurrentSession.loginEmailID);
                bool isExist = data.AsEnumerable()
                   .Any(row => row.Field<string>("UserName")
                   .Equals(email, StringComparison.OrdinalIgnoreCase));
                if (isExist)
                {
                    CurrentSession.ClearSessions();
                    CurrentSession.Clear();
                    user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(email);

                    if (user.mUserList != null && user.mUserList.Count > 0)
                    {
                        foreach (var list in user.mUserList)
                        {

                            Session["LoggedIn"] = "yes";
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
                            CurrentSession.DeptID = list.DeptId.ToString();
                            CurrentSession.Name = list.Name.ToString();
                            CurrentSession.Designation = list.Designation;
                            CurrentSession.DeptName = list.DepartmentName.ToString();
                            CurrentSession.DesignationLocal = list.DesignationLocal;
                            CurrentSession.serviceName = ConfigurationManager.AppSettings["serviceName"];
                            CurrentSession.officeType = list.ofcType;
                            bool s = AuditTrailsExtensionMethods.FillAuditTrail();
                            CurrentSession.isApex = list.isApex.ToString();
                            CurrentSession.ApprovalAuthority = list.isApprovalAuthority.ToString();
                            CurrentSession.MapId = list.MapId.ToString();
                            CurrentSession.RCerificate = list.registeredCert;
                            CurrentSession.SignaturePath = list.SignaturePath;
                            Models.MenuModel _users = new Models.MenuModel();
                            String menuid = _users.MenuId.ToString();

                        }

                    }

                }



                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public JsonResult GetDepartmentsNav()
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetDepartmentsbyUser(CurrentSession.UserID);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetDataCountByIdForDispatch(string id)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetDataCountByIdForDispatch(Convert.ToInt32(id), Convert.ToInt32(CurrentSession.OfficeId));
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetOfficebyDepartmentId(string deptid)
        {
            try
            {

                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetOfficebyUser(deptid, CurrentSession.UserID);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetOfficebyDepartmentIdNav(string deptid)
        {
            try
            {
                Session["sessdeptid"] = "";

                if (Session["sessdeptid"].ToString() == "")
                {
                    Session["sessdeptid"] = deptid;
                }
                else
                {
                    CurrentSession.DeptID = CurrentSession.DeptID;
                    Session["sessdeptid"] = deptid;
                }
                DiaryViewModel dvm = new DiaryViewModel();
                var data = dvm.GetMappedOfficebyUser(deptid, CurrentSession.UserID);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult SetOffice(string deptid, string officeid)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                Session["sessofficeid"] = "";

                if (Session["sessofficeid"].ToString() == "")
                {
                    // CurrentSession.DeptID = deptid;
                    CurrentSession.MapId = officeid;
                    Session["sessofficeid"] = officeid;
                    DataSet ds = dvm.GetOfficeDetails(Convert.ToInt32(officeid), CurrentSession.UserID);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //CurrentSession.OfficeName = Convert.ToString(dt.Rows[0]["officename"]);
                        // CurrentSession.DeptName = Convert.ToString(dt.Rows[0]["deptname"]);
                        // CurrentSession.OfficeNameNew = Convert.ToString(dt.Rows[0]["officename"]);
                        // CurrentSession.DeptNameNew = Convert.ToString(dt.Rows[0]["deptname"]);
                        // CurrentSession.isApex = Convert.ToString(dt.Rows[0]["isApex"]);
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        string Designation = Convert.ToString(ds.Tables[1].Rows[0]["Designation"]);
                        if (Designation != "")
                        {
                            //  CurrentSession.Designation = Designation;
                        }
                    }
                    else
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            string UserDesignation = Convert.ToString(ds.Tables[2].Rows[0]["Designation"]);
                            // CurrentSession.Designation = UserDesignation;

                        }
                    }

                }
                else
                {
                    //CurrentSession.OfficeId = CurrentSession.OfficeId;
                }

                var data = dvm.GetOfficebyDepartmentId(deptid);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult SetAssignedOffice(string deptid, string officeid)
        {
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                Session["sessAssignedofficeid"] = "";

                if (Session["sessAssignedofficeid"].ToString() == "")
                {
                    CurrentSession.DeptID = deptid;
                    CurrentSession.OfficeId = officeid;
                    Session["sessAssignedofficeid"] = officeid;
                    DataSet ds = dvm.GetOfficeDetails(Convert.ToInt32(officeid), CurrentSession.UserID);
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        CurrentSession.OfficeName = Convert.ToString(dt.Rows[0]["officename"]);
                        CurrentSession.DeptName = Convert.ToString(dt.Rows[0]["deptname"]);
                        CurrentSession.OfficeNameNew = Convert.ToString(dt.Rows[0]["officename"]);
                        CurrentSession.DeptNameNew = Convert.ToString(dt.Rows[0]["deptname"]);
                        CurrentSession.isApex = Convert.ToString(dt.Rows[0]["isApex"]);
                        CurrentSession.MCName = Convert.ToString(dt.Rows[0]["MCName"]);
                        CurrentSession.StateId = Convert.ToString(dt.Rows[0]["StateId"]);
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        string Designation = Convert.ToString(ds.Tables[1].Rows[0]["Designation"]);
                        if (Designation != "")
                        {
                            CurrentSession.Designation = Designation;
                        }
                    }
                    else
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            string UserDesignation = Convert.ToString(ds.Tables[2].Rows[0]["Designation"]);
                            CurrentSession.Designation = UserDesignation;

                        }
                    }

                }
                else
                {
                    CurrentSession.OfficeId = CurrentSession.OfficeId;
                }

                var data = dvm.GetOfficebyDepartmentId(deptid);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetInboxMeetingFiles(string refid, int MeetingId)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                DataTable data = new DataTable();
                data = dvm.GetInboxMeetingFiles(CurrentSession.UserID, MeetingId);
                var dtt = dvm.GetTableRows(data);
                return Json(dtt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //File PDF Generate Code-- Ritika-------------
        [HttpPost]
        public ActionResult DispatchAgendaDetail(AgendaDetailModel mdl)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                if (Request.Form["getProceedingpdf"] != null)
                {
                    Session["userExist"] = "";
                    Session["userAdded"] = "";
                    int agendaid = mdl.AgendaId;// 10045;
                    string refid = "";// model.RefId.ToString();
                                      // read parameters from the webpage
                    string htmlString = GetProceedingCoveringLetterpdf(agendaid, refid);


                    PdfPageOrientation pdfOrientation =
                        (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                        "Portrait", true);
                    HtmlToPdf converter = new HtmlToPdf();

                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                    converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                    converter.Options.MarginBottom = 20;
                    converter.Options.MarginTop = 30;
                    converter.Options.MarginLeft = 10;
                    converter.Options.MarginRight = 10;

                    //-------------------PDF Footer Starts
                    converter.Options.DisplayFooter = true;
                    converter.Footer.DisplayOnFirstPage = true;
                    converter.Footer.DisplayOnOddPages = true;
                    converter.Footer.DisplayOnEvenPages = true;
                    converter.Footer.Height = 50;

                    // page numbers can be added using a PdfTextSection object
                    PdfTextSection text = new PdfTextSection(0, 10, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("Arial", 8));
                    text.HorizontalAlign = PdfTextHorizontalAlign.Right;
                    converter.Footer.Add(text);


                    //-------------------PDF Footer Ends
                    SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlString);

                    string finalpdfpath = "Agenda/" + agendaid.ToString() + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_Export.pdf";
                    string folderpath = "~/MLADIARY/" + finalpdfpath;
                    string agendapath = Server.MapPath(folderpath);
                    pdfDoc.Save(agendapath);
                    return File(folderpath, "application/pdf", "Export.pdf");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        //VIEWREFERENCEPDF
        public string Getpdfhtml(int agendaid, string refid, string actionCode)
        {
            //agendaid = 11047; 
            AgendaDetailModel mdl = new AgendaDetailModel();

            string maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            mdl._LstResolutionActions = AgendaModelFunction.GetProcedingDataForCommPDF(refid, actionCode);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            string outXml = "";

            var path1 = BlobStorage.GetStorageAcessingPath();
            //string logo = path1 + "/LG/" + "logo.png";

            //string url = "/Assets/Images/Govt-of-punjab.png";
            //string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            //string imagePath = baseUrl+url;


            outXml = outXml + @"<html>
                                <head><meta charset='UTF-8'><title>FileActionsReport</title>
                                    
                                        <style>
                                     td {
                                        vertical-align: top;
                                    }
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
                                    .watermark {
                                            position: fixed;
                                            top: 50%;
                                            left: 50%;
                                            transform: translate(-50%, -50%);
                                            font-size: 50px;
                                            color: rgba(0, 0, 0, 0.1); /* semi-transparent */
                                            white-space: nowrap;
                                            pointer-events: none;
                                            z-index: -1; /* behind the content */
                                            font-weight: bold;
                                            text-align: center;
                                            width: 50%;
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
                                    <body>
                                 <div class='watermark'>Preview</div>
                                <div class='pdf-class' style='max-width:1050px; margin:auto; '>
             <div><div class='clearfix'></div>";
            outXml = outXml + @"<div style='text-align:center'>";
            //outXml = outXml + @"<img src='" + logo + "' height='50' width='55' />";

            //outXml += @"<p style='margin:0px'>ਪੰਜਾਬ ਸਰਕਾਰ</p>";
            //outXml += @"<p style='margin:0px'>ਸਥਾਨਕ ਸਰਕਾਰ ਵਿਭਾਗ</p>";
            //outXml += @"<p style='margin:0px'>ਪਲਾਟ ਨੰ. 03, ਮਿਉਂਸਪਲ ਭਵਨ, ਸੈਕਟਰ-35 ਏ, ਚੰਡੀਗੜ੍ਹ 160036</p><hr></div>";
            //outXml += @"<p style='font-size: 15px;font-weight:bold; text-align: center'>" + mdl._LstReferences[0].FromDeptLocal + " : " + HeaderlistofAgenda.AgendaName + ", ਮਿਤੀ:- " + HeaderlistofAgenda.AgendaDate + "</p></div>";
            //if (HeaderlistofAgenda != null)
            //{
            //    outXml += @"<div class='margin: 50px 70px;'>
            //            <h1 style='font-size: 32px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; width: fit-content; margin: auto;'>"
            //        + HeaderlistofAgenda.AgendaName + "</h1>";
            //    outXml += @"<p style='font-size: 15px; text-align: center'>"
            //     + HeaderlistofAgenda.Header + "</p></div>";
            //}

            outXml += @"<table style='font-size:15px;width:100%; vertical-align: top;border: 1px solid #cdcdcd;border-collapse: collapse;'>";
            outXml = outXml + @"<tr style = 'padding:5px;'>
                  <th style ='padding:10px;border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:left;width:100px !important'>Resolution No.</th> 
                  <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:left; max-width:600px;'>Resolution Description </th>
                  <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:left; width:150px !important'>Comments at " + @CurrentSession.MCName + @"</th></tr>";
            for (int i = 0; i < mdl._LstResolutionActions.Count; i++)
            {
                outXml += @"<tr>										
										<td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top; '>
                                        " + Convert.ToString(mdl._LstResolutionActions[i].LetterNo) + @"<br>" + Convert.ToString(mdl._LstResolutionActions[i].AgendaDate) + @"   
                                        </td>
                                        <td style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top; max-width:500px;'>";
                string _subjectDetails = Convert.ToString(mdl._LstResolutionActions[i].SubjectDetails);
                string _subject = Convert.ToString(mdl._LstResolutionActions[i].Subject);
                if (_subject != "")
                {

                    _subject = "<b>" + _subject + " :- </b>";
                }

                outXml += @"<u>" + _subject + "</u> " + _subjectDetails;
                outXml += @"<br><b>House Decision:- </b>" + Convert.ToString(mdl._LstResolutionActions[i].HouseDecision);
                outXml += @"<br><br><b>Specify Under Which Point of Instruction Dated 10.10.2022 this Resolution is covered:-</b>" + Convert.ToString(mdl._LstResolutionActions[i].Act);
                outXml += @"<br><br><b>Comments of Commissioner:-</b>" + Convert.ToString(mdl._LstResolutionActions[i].commissioner);
                outXml += @"</td><td style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;'>";

                string[] lgcommentsarray = mdl._LstResolutionActions[i].LGComments.Split('~');
                if (lgcommentsarray.Length > 0)
                {

                    //outXml += @"<ol style='padding-left: 15px'>";
                    int j = 1;
                    foreach (string lgcomments in lgcommentsarray)
                    {
                        //string[] lgcommentsarray2 = lgcomments.Split(new string[] { "::" }, StringSplitOptions.None);
                        //outXml += @"<li>" + Convert.ToString(lgcommentsarray2[2]) + "<div class='clearfix'></div>";
                        //outXml += @"<div style='float:left;'><b>" + Convert.ToString(lgcommentsarray2[0]) + "</b></div><div style='float:right;'><b>" + Convert.ToString(lgcommentsarray2[1]) + "</b></div></li>";


                        string[] ActionBy = lgcomments.Split(new string[] { "::" }, StringSplitOptions.None);

                        if (ActionBy.Length > 2)
                        {
                            string fixedHtml = FixHtml(ActionBy[2]);

                            outXml += @"<div id='LGComments' class='SDHide'>";
                            outXml += @"<div class='row'>
                    <p class='col-12'>" + j + @". " + fixedHtml + @"</p>
                    <p style='font-size:12px'>
                        " + HttpUtility.HtmlEncode(ActionBy[1]) + @"<br/>
                        " + HttpUtility.HtmlEncode(ActionBy[0]) + @"
                    </p>
                    <hr class='dotted'>
                </div>";
                            outXml += @"</div>";

                            j++;
                        }
                    }
                    //outXml += @"</ol>";
                }
                //outXml += Convert.ToString(mdl._LstReferences[i].LGComments);
                outXml += @"</td></tr>";
            }
            outXml += "</table>";
            outXml += "</div> </div> </body> </html>";
            return outXml;
        }
        public static string FixHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            html = HttpUtility.HtmlDecode(html);

            html = html.Replace("\"\"", "\"");

            html = Regex.Replace(html, @"mso-[^:]+:[^;""]+;?", "", RegexOptions.IgnoreCase);

            html = html.Replace("&nbsp;", " ");

            return html.Trim();
        }
        //VIEW CONSOLIDATED PDF
        public string GetLGSameResolutionpdfhtml(int AgendaId)//By Ritika
        {

            //Proceeding PDF first
            AgendaDetailModel mdl = new AgendaDetailModel();

            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(AgendaId);
            mdl._LstResolutionActions = AgendaModelFunction.GetLGSameResolutionDataPDF(AgendaId);
            string outXml = "";
            var path1 = BlobStorage.GetStorageAcessingPath();
            string logo = path1 + "/LG/" + "logo.png";
            string signature = path1 + "/" + CurrentSession.SignaturePath;


            var firstResolutionNo = "";
            var lastResolutionNo = "";
            for (int i = 0; i < mdl._LstReferences.Count; i++)
            {
                firstResolutionNo = mdl._LstReferences[0].ResolutionNo;
            }
            if (mdl._LstReferences.Count > 1)
            {
                foreach (var item2 in mdl._LstReferences)
                {
                    lastResolutionNo = item2.ResolutionNo;
                }
            }
            // if(mdl.)

            outXml = outXml + @"<html>
                                    <head><meta charset='UTF-8'><title>MC: ReplyToReport</title>
                                    
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
                                    <body>
                    <div class='pdf-class' style='max-width:1050px; margin:auto;'><div>
            <div class='clearfix'></div></div><div style='margin: 0px 10px;'>";
            outXml = outXml + @"<div style='text-align:center'>";
            outXml = outXml + @"<img src='" + logo + "' height='50' width='55' />";

            outXml += @"<p style='margin:0px'>ਪੰਜਾਬ ਸਰਕਾਰ</p>";
            outXml += @"<p style='margin:0px'>ਸਥਾਨਕ ਸਰਕਾਰ ਵਿਭਾਗ</p>";
            outXml += @"<p style='font-weight:bold;margin:0px'>(ਮਿਉਨਿਸਪਲ ਸਰਵਿਸ ਸੈੱਲ)</p>";
            outXml += @"<p style='font-weight:bold;margin:0px'>ਪਲਾਟ ਨੰ. 03, ਮਿਉਂਸਪਲ ਭਵਨ, ਸੈਕਟਰ-35 ਏ, ਚੰਡੀਗੜ੍ਹ 160036</p></div>";

            outXml += @"<div style='width:100%;><span style='float:left;'>ਨੰ:..... </br> ਮਿਤੀ:-..... </span><span style='float:right;'>Ph. 0172-2619145</br>Email: dlgmsc123@gmail.com</span></br></div>";
            outXml += @"<hr style='margin-top: 20px;'>";
            outXml += @"<span style='float:left;'>ਸੇਵਾ ਵਿਖੇ,</span></br>";
            outXml += @"<div style='width:100%;float:left;margin-left:80px'><span>ਕਮਿਸ਼ਨਰ,</br>" + mdl._LstReferences[0].FromDeptLocal + "</br></br></span></div>";
            outXml += @"<div class='clearfix'></div>";
            outXml += @"<b><div style='width:100%;margin-left:30px'>ਵਿਸ਼ਾ:-" + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਮਤਾ ਨੰ. " + firstResolutionNo + " ਤੋਂ " + lastResolutionNo + " ਮਿਤੀ: " + mdl._LstReferences[0].meetingDate + " ਸਬੰਧੀ।</b><br><br></div>";
            outXml += @"<span style='float:left;margin-left:70px'>ਆਪ ਦੇ ਦਫਤਰ ਦੇ ਪੱਤਰ ਨੰ: " + mdl._LstReferences[0].FileNo + " ਮਿਤੀ: " + mdl._LstReferences[0].ActionTakenDate + " ਦੇ ਹਵਾਲੇ ਵਿੱਚ। </span>";
            outXml += @"</br><span style='float:left;margin-left:70px;margin-bottom: 20px;margin-top: 6px;'>2.     ਉਪਰੋਕਤ ਵਿਸੇ ਸਬੰਧੀ ਹਵਾਲੇ ਅਧੀਨ ਪੱਤਰ ਰਾਹੀਂ ਪ੍ਰਾਪਤ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ਜਨਰਲ ਹਾਊਸ ਦੇ " + mdl._LstReferences[0].FromDeptLocal + " ਦੇ ";
            outXml += @" ਮਤਾ ਨੰ: " + firstResolutionNo + " ਤੋ " + lastResolutionNo + " ਮਿਤੀ: " + mdl._LstReferences[0].meetingDate + " ਤੇ ਵਿਚਾਰ ਕਰਨ ਉਪਰੰਤ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਫੈਸਲਾ ਕੀਤਾ ਗਿਆ ਹੈ :-</span>";
            outXml += @"<table style='width:80%;font-size: 15px; border: 1px solid #cdcdcd;border-collapse: collapse;text-align: left;margin: auto;'>";
            outXml = outXml + @"<tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>ਮਤਾ ਨੰ:</th> 
                  <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>ਫੈਸਲਾ</th></tr>";

            if (mdl._LstResolutionActions != null && mdl._LstResolutionActions.Count > 0)
            {
                for (int i = 0; i < mdl._LstResolutionActions.Count; i++)
                {
                    //if (i == 0)
                    //{
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid #cdcdcd;border-collapse: collapse;vertical-align:top;text-align:center'>               
												<b>" + Convert.ToString(mdl._LstResolutionActions[i].ResolutionNo) + @"</b>
										</td>
										<td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>
                                        " + Convert.ToString(mdl._LstResolutionActions[i].LGComments) + @"
                                        </td></tr>";
                }
            }
            else

            {
                outXml += @"<tr><td colspan='2' style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>Comments not available</td></tr>";
            }
            outXml += " </table></br>";
            outXml += @"<div style='text-align:right;'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
            outXml += @"</br><span style='float:right;text-align:right'>ਸੁਪਰਡੈਂਟ,</br>(ਮਿਉਨਿਸਪਲ ਸਰਵਿਸ ਸੈੱਲ)</span></div></body> </html>";
            return outXml;

        }

        //VIEW ACTIONWISE PDF
        //        public string GetLGActionWisepdfhtml(int AgendaId, string ActionIds, string FileType)
        //        {

        //            AgendaDetailModel mdl = new AgendaDetailModel();
        //            var basicDetail = AgendaModelFunction.GetApprovalDepartmentAndStartandEndResolution(AgendaId, ActionIds);
        //            var lgBranch = AgendaModelFunction.LGBranchDetails(CurrentSession.OfficeId).FirstOrDefault();

        //            string email = lgBranch?.DeptEmail ?? "";
        //            string mobileNo = lgBranch?.DeptPhoneNo ?? "";
        //            string BranchName = lgBranch?.FromOfficeLocal ?? "";
        //            string DepartmentName = lgBranch?.FromDeptLocal ?? "";
        //            string DepartmentAddress= lgBranch?.DeptAddress ?? ""; 
        //            string Logo = lgBranch?.DeptLogo ?? "";
        //            //string ToOffice = lgBranch?.ToOffice ?? "";
        //            mdl._LstResolutionActions = AgendaModelFunction.GetApprovalActionData(AgendaId, ActionIds, FileType);

        //            var path1 = BlobStorage.GetStorageAcessingPath();
        //            string logo = path1 + Logo;

        //            string firstResolutionNo = "";
        //            string lastResolutionNo = "";
        //            string FromDeptLocal = "";
        //            string meetingDate = "";
        //            string ActionTakenDate = "";
        //            string ApprovalDate = "";
        //            string ToOffice = "";

        //            foreach (var d in basicDetail)
        //            {
        //                firstResolutionNo = Convert.ToString(d.SrNo1);
        //                lastResolutionNo = Convert.ToString(d.SrNo2);
        //                FromDeptLocal = Convert.ToString(d.FromDept);
        //                meetingDate = Convert.ToString(d.AgendaDate);
        //                ApprovalDate = Convert.ToString(d.CreatedDate);
        //                ActionTakenDate = Convert.ToString(d.ActionTakenDate);
        //                ToOffice= Convert.ToString(d.ToOffice);
        //            }

        //            string outXml = @"<!DOCTYPE html>
        //<html>
        //<head>
        //<meta charset='UTF-8'>
        //<title>Approval</title>

        //<style>
        //body{
        //    font-family:'Noto Sans Punjabi','Arial',sans-serif;font-size:12px;
        //    line-height:1.7;
        //}

        //.pdf-class{
        //    max-width:1050px;
        //    margin:auto;
        //}

        ///* ===== PERFECT PDF TABLE ===== */
        //table.approval-table{
        //    width:100%;
        //    margin:20px auto;
        //    font-size:12px;

        //    border-collapse:separate;
        //    border-spacing:0;

        //    outline:1px solid #000;      /* outer border */
        //    outline-offset:-1px;

        //    page-break-inside:auto;
        //}

        //table.approval-table tr{
        //    page-break-inside:auto;
        //}

        //table.approval-table th,
        //table.approval-table td{
        //    padding:10px;
        //    vertical-align:top;
        //    word-break:break-word;
        //    overflow-wrap:break-word;

        //    border-left:1px solid #000;
        //    border-right:1px solid #000;
        //    border-bottom:1px solid #000;
        //}

        ///* top border safe (never cuts) */
        //table.approval-table tr:first-child th,
        //table.approval-table tr:first-child td{
        //    box-shadow: inset 0 1px 0 #000;
        //}

        ///* 🔥 remove double bottom border */
        //table.approval-table tr:last-child th,
        //table.approval-table tr:last-child td{
        //    border-bottom:none !important;
        //}
        //</style>
        //</head>

        //<body>
        //<div class='pdf-class'>

        //<div style='text-align:center'>
        //    <img src='" + logo + @"' height='50' width='55'/>
        //    <p style='margin:0'>ਪੰਜਾਬ ਸਰਕਾਰ</p>
        //    <p style='margin:0'>" + DepartmentName + @"</p>";

        //   if (!string.IsNullOrWhiteSpace(BranchName))
        //   {
        //    outXml += " <p style = 'font-weight:bold;margin:0' > (" + BranchName + ") </ p >";
        //   };

        //            outXml += "<p style='font-weight:bold;margin:0'>"
        //            + DepartmentAddress +
        //             @"</p>
        //</div>

        //<div style='width:100%;margin-top:12px'>
        //    <span style='float:left'>ਨੰ:.....<br/>ਮਿਤੀ: " + ApprovalDate + @"</span>
        //    <span style='float:right'>Ph: " + mobileNo + @"<br/>Email: " + email + @"</span>
        //</div>

        //<div style='clear:both'></div>
        //<hr/>

        //<p>ਸੇਵਾ ਵਿਖੇ,</p>

        //<div style='margin-left:80px'>
        //    <p>" + ToOffice + ",<br/>" + FromDeptLocal + @"</p>
        //</div>

        //<p style='margin-left:30px;font-weight:bold'>
        //ਵਿਸ਼ਾ:- " + FromDeptLocal + @" ਦੇ ਮਤਾ ਨੰ. " + firstResolutionNo + @" ਤੋਂ "
        //                 + lastResolutionNo + @" ਮਿਤੀ: " + meetingDate + @" ਸਬੰਧੀ।
        //</p>";

        ////<p style='margin-left:70px'>
        ////ਆਪ ਦੇ ਦਫਤਰ ਦੇ ਪੱਤਰ ਨੰ: _____ ਮਿਤੀ: " + ActionTakenDate + @" ਦੇ ਹਵਾਲੇ ਵਿੱਚ।
        ////</p>

        //outXml += @"<p style='margin-left:70px'>
        //ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਮਤਾ ਨੰ: "
        //        + firstResolutionNo + @" ਤੋਂ " + lastResolutionNo +
        //        @" ਮਿਤੀ: " + meetingDate + @" ਉਪਰੰਤ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਫੈਸਲਾ ਕੀਤਾ ਗਿਆ ਹੈ:
        //</p>";

        //            /* ===== SINGLE TABLE (NO THEAD, NO FOOTER ROW) ===== */
        //            outXml += @"<table class='approval-table'>
        //<tr>
        //    <th style='width:20%;text-align:center'>ਮਤਾ ਨੰ:</th>
        //    <th style='width:50%;text-align:center'>ਫੈਸਲਾ</th>
        //    <th style='width:30%;text-align:center'>ਸਥਿਤੀ</th>
        //</tr>";

        //            if (mdl._LstResolutionActions != null && mdl._LstResolutionActions.Count > 0)
        //            {
        //                foreach (var r in mdl._LstResolutionActions)
        //                {
        //                    outXml += @"<tr>
        //<td style='text-align:center'><b>" + r.ResolutionNo + @"</b></td>
        //<td>" + System.Net.WebUtility.HtmlDecode(r.LGComments) + @"</td>
        //<td>" + r.StatusName + @"</td>
        //</tr>";
        //                }
        //            }
        //            else
        //            {
        //                outXml += @"<tr>
        //<td colspan='3' style='text-align:center'>Comments not available</td>
        //</tr>";
        //            }

        //            outXml += @"</table>

        //<br/><br/>

        //<p style='text-align:right;font-weight:bold'>"
        //+ CurrentSession.DesignationLocal + "</br>"   + DepartmentName +    @"</p>

        //</div>
        //</body>
        //</html>";

        //            return outXml;
        //        }


        public ActionResult EditApproval(int AgendaId, string ActionIds, string FileType, string Refs)
        {
            if (CurrentSession.SubUserTypeID != "4" && CurrentSession.SubUserTypeID != "21" && CurrentSession.SubUserTypeID != "20")
            {
                return RedirectToAction("Dashboard", "References");
            }

            AgendaDetailModel mdl = new AgendaDetailModel();
            AgendaModelFunction.GetProceedingReply(mdl,AgendaId, ActionIds, FileType, Refs);
            return View("EditApproval_Action", mdl);
            //return Json(mdl._LstResolutionActions, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditApprovalNew(int AgendaId, string ActionIds, string FileType, string Refs)
        {
            if (CurrentSession.SubUserTypeID != "4" && CurrentSession.SubUserTypeID != "21" && CurrentSession.SubUserTypeID != "20")
            {
                return RedirectToAction("Dashboard", "References");
            }

            AgendaDetailModel mdl = new AgendaDetailModel();
            AgendaModelFunction.GetProceedingReplyNew(mdl, AgendaId, ActionIds, FileType, Refs);
            //return View("EditApproval_Action", mdl);
            return View("EditApproval_Action", mdl);
            //return Json(mdl._LstResolutionActions, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditApprovalNewNoting(int AgendaId, string ActionIds, string FileType, string Refs)
        {
            if (CurrentSession.SubUserTypeID != "4" && CurrentSession.SubUserTypeID != "21" && CurrentSession.SubUserTypeID != "20")
            {
                return RedirectToAction("Dashboard", "References");
            }

            AgendaDetailModel mdl = new AgendaDetailModel();
            AgendaModelFunction.GetProceedingReplyNew(mdl, AgendaId, ActionIds, FileType, Refs);
            
            return View("EditApproval", mdl);
            //return Json(mdl._LstResolutionActions, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveDraftRow(string RefId,string Comments,int Status)
        {
            try
            {
                string result =DiaryViewModel.InsertActionSessionRes(RefId,CurrentSession.UserID.ToString(),Comments ?? "",Status);

                if (result == "1")
                {
                    return Json(new
                    {
                        Status = 1,
                        Message = "Draft saved successfully"
                    });
                }
                else
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = "Error saving draft"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Status = 0,
                    Message = ex.Message
                });
            }
        }
        

        
        public class ResolutionUpdateModel
        {
            public string RefId { get; set; }     // can be "101,102"
            public string Comments { get; set; }
            public string Status { get; set; }
        }

        [HttpPost]
        public ActionResult UpdateBulkData(List<ResolutionUpdateModel> data)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("RefId", typeof(string));
                dt.Columns.Add("Comments", typeof(string));
                dt.Columns.Add("Status", typeof(string));

                foreach (var item in data)
                {
                    dt.Rows.Add(item.RefId, item.Comments, item.Status);

                    //DataSet ds = DiaryViewModel.GetMovementbyRefid(Convert.ToInt64(item.RefId), Convert.ToInt64(diaryModel.LinkRefID));
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    foreach (DataRow dr in ds.Tables[0].Rows)
                    //    {
                    //        string SentToUserId = dr["ToUserId"].ToString();
                    //        string Notification = "LG Observation Raised";
                    //        var response = DiaryViewModel.InsertNotification(Convert.ToInt64(item.RefId), 0, 6, Notification, SentToUserId, 1, diaryModel.Userid);

                    //    }

                    //}
                }

                SqlConnection connection = ClsConnection.GetConnection();

                if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                {
                    connection.Open();
                }

                using (SqlCommand cmd = new SqlCommand("sp_UpdateBulkDiaryAction", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // ✅ TVP parameter
                    var tvpParam = cmd.Parameters.AddWithValue("@BulkData", dt);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.ResolutionBulkType";

                    // ✅ Other parameters
                    cmd.Parameters.Add("@userid", SqlDbType.NVarChar, 100).Value = CurrentSession.UserID?.ToString();

                    cmd.Parameters.Add("@FromOfficeId", SqlDbType.Int).Value = Convert.ToInt32(CurrentSession.OfficeId);

                    cmd.Parameters.Add("@FromDeptId", SqlDbType.NVarChar, 50).Value = CurrentSession.DeptID?.ToString();

                    // 🔥 Better to pass DateTime instead of string
                    cmd.Parameters.Add("@CreatedStamp", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

          public string GetLGActionWisepdfhtml(int AgendaId, string ActionIds, string Refs, string FileType, bool IsPreview)
          {

            AgendaDetailModel mdl = new AgendaDetailModel();
            var basicDetail = AgendaModelFunction.GetApprovalDepartmentAndStartandEndResolution(AgendaId, ActionIds, Refs);
            var Endorsements = AgendaModelFunction.GetApprovalEndorsement(AgendaId, ActionIds, Refs);
            string DispachNoAuto = "";
            if (IsPreview)
            {
                DispachNoAuto = "XXXXX";
            }
            else
            {
                DispachNoAuto = AgendaModelFunction.GenerateDispatch(AgendaId);
            }
            var RecievedDateandFileId = AgendaModelFunction.GetApprovalFileIDandRecievedDate(AgendaId);

            var RecievedDateandFileIds = RecievedDateandFileId.FirstOrDefault();
            string RefId = RecievedDateandFileIds?.FileRefId ?? "";
            string RecievedDate = RecievedDateandFileIds?.LetterDate ?? "";


            var lgBranch = AgendaModelFunction.LGBranchDetails(CurrentSession.OfficeId).FirstOrDefault();

            string email = lgBranch?.DeptEmail ?? "";
            string mobileNo = lgBranch?.DeptPhoneNo ?? "";
            string BranchName = lgBranch?.FromOfficeLocal ?? "";
            string DepartmentName = lgBranch?.FromDeptLocal ?? "";
            string DepartmentAddress = lgBranch?.DeptAddress ?? "";
            string Logo = lgBranch?.DeptLogo ?? "";
            //string ToOffice = lgBranch?.ToOffice ?? "";

            var endorsement = Endorsements.FirstOrDefault();
            string endormentText = endorsement?.Endorsement ?? "";
            string FooterText= endorsement?.Footer ?? "";
            string Subject = endorsement?.Subject ?? "";
            string Header = endorsement?.Header ?? "";
            string SendTo = endorsement?.SendTo ?? "";
            string RejectionFormat= endorsement?.RejectionFormat ?? "";
            string DispachNo = endorsement?.UserDispactchNo ?? "";

            mdl._LstResolutionActions = AgendaModelFunction.GetApprovalActionData(AgendaId, ActionIds, FileType, Refs);
            var path1 = BlobStorage.GetStorageAcessingPath();
            string logo = path1 + Logo;

            string firstResolutionNo = "";
            string lastResolutionNo = "";
            string FromDeptLocal = "";
            string meetingDate = "";
            string ActionTakenDate = "";
            string ApprovalDate = "";
            string ToOffice = "";
            string FromDeptID = "";
            string DestinationDept = "";
            string SrNos = "";

            foreach (var d in basicDetail)
            {
                firstResolutionNo = Convert.ToString(d.StartResolution);
                lastResolutionNo = Convert.ToString(d.EndResolution);
                FromDeptLocal = Convert.ToString(d.FromDept);
                meetingDate = Convert.ToString(d.AgendaDate);
                ApprovalDate = Convert.ToString(d.CreatedDate);
                ActionTakenDate = Convert.ToString(d.ActionTakenDate);
                ToOffice = Convert.ToString(d.ToOffice);
                FromDeptID = Convert.ToString(d.FromDeptID);
                DestinationDept = Convert.ToString(d.FromDeptLocal);
                SrNos = Convert.ToString(d.SerialNo);
            }

            string outXml = @"<!DOCTYPE html>
                <html>
                <head>
                <meta charset='UTF-8'>
                <title>Approval</title>

                <style>
                body{
                    font-family:'Noto Sans Punjabi','Arial',sans-serif;font-size:12px;
                    line-height:1.7;
                }

                .pdf-class{
                    max-width:1050px;
                    margin:auto;
                }

                /* ===== PERFECT PDF TABLE ===== */
                table.approval-table{
                    width:100%;
                    margin:20px auto;
                    font-size:12px;

                    border-collapse:separate;
                    border-spacing:0;

                    outline:1px solid #000;      /* outer border */
                    outline-offset:-1px;

                    page-break-inside:auto;
                }

                table.approval-table tr{
                    page-break-inside:auto;
                }

                table.approval-table th,
                table.approval-table td{
                    padding:10px;
                    vertical-align:top;
                    word-break:break-word;
                    overflow-wrap:break-word;

                    border-left:1px solid #000;
                    border-right:1px solid #000;
                    border-bottom:1px solid #000;
                }

                /* top border safe (never cuts) */
                table.approval-table tr:first-child th,
                table.approval-table tr:first-child td{
                    box-shadow: inset 0 1px 0 #000;
                }

                /* 🔥 remove double bottom border */
                table.approval-table tr:last-child th,
                table.approval-table tr:last-child td{
                    border-bottom:none !important;
                }
                </style>
                </head>

                <body>
                <div class='pdf-class'>

                  <div style='text-align:center'>";
                    if (!string.IsNullOrWhiteSpace(Logo))
                    {
                        outXml += "<img src='" + logo + @"' height='50' width='55'/>";
                    };

                    outXml += "<p style='font-weight:bold; margin:0'>" + DepartmentName + @"</p>";
                            outXml += "<p style='margin:0'>"
                                      + DepartmentAddress +
                                       @"</p>";
                            if (!string.IsNullOrWhiteSpace(BranchName))
                            {
                                outXml += "<p style = 'margin:0'> (" + BranchName + ") </p>";
                            }
                            ;
            outXml += @"</div>
                     <div style='width:100%;margin-top:12px'>
                     <span style='float:left'>ਨੰਬਰ: " + DispachNo + '/' + DispachNoAuto + "<br/>ਮਿਤੀ: " + ApprovalDate + @"</span>
                     <span style='float:right'>ਫੋਨ ਨੰ.: " + mobileNo + @"<br/>ਈ.ਮੇਲ:: " + email + @"</span>
                    </div>

                <div style='clear:both'></div>
                <hr/>";

            if (!string.IsNullOrWhiteSpace(RejectionFormat))
            {
                outXml += @"<div style='margin-left:70px'>"
                     + RejectionFormat + @"</div>";

                //outXml += RejectionFormat;
            }

            else
            {

                outXml +=  @"<p>ਸੇਵਾ ਵਿਖੇ,</p>";

                if (!string.IsNullOrWhiteSpace(SendTo))
                {
                    outXml += @"<div style='margin-left:80px'>
                    <p>" + SendTo + @"</p>
                </div>";
                }
                else
                {
                    outXml += @"<div style='margin-left:80px'>
                    <p>" + ToOffice +
                                    (string.IsNullOrEmpty(FromDeptLocal) ? "" : ",<br />" + FromDeptLocal) +
                                    @"</p>
                </div>";
                }

                if (!string.IsNullOrWhiteSpace(Subject))
                {
                    outXml += @"<p style='margin-left:30px;font-weight:bold'>
                ਵਿਸ਼ਾ:- " + Subject + @"
                </p>";
                }
                else
                {
                    if (Refs == "")
                    {

                        if (firstResolutionNo != lastResolutionNo)
                        {
                            if (FromDeptID == "UDD0001")
                            {
                                outXml += @"<p style='margin-left:30px;font-weight:bold'>
                        ਵਿਸ਼ਾ:- " + DestinationDept + @" ਦੇ ਮਤਾ ਨੰ. " + firstResolutionNo + @" ਤੋਂ "
                                                     + lastResolutionNo + @" ਮਿਤੀ: " + meetingDate + @" ਸਬੰਧੀ।
                        </p>";
                            }
                            else
                            {
                                outXml += @"<p style='margin-left:30px;font-weight:bold'>
                        ਵਿਸ਼ਾ:- " + FromDeptLocal + @" ਦੇ ਮਤਾ ਨੰ. " + firstResolutionNo + @" ਤੋਂ "
                                                    + lastResolutionNo + @" ਮਿਤੀ: " + meetingDate + @" ਸਬੰਧੀ।
                        </p>";
                            }
                        }
                        else
                        {
                            if (FromDeptID == "UDD0001")
                            {
                                outXml += @"<p style='margin-left:30px;font-weight:bold'>
                        ਵਿਸ਼ਾ:- " + DestinationDept + @" ਦੇ ਮਤਾ ਨੰ. " + firstResolutionNo + @" ਮਿਤੀ: " + meetingDate + @" ਸਬੰਧੀ।
                       </p>";
                            }
                            else
                            {
                                outXml += @"<p style='margin-left:30px;font-weight:bold'>
                        ਵਿਸ਼ਾ:- " + FromDeptLocal + @" ਦੇ ਮਤਾ ਨੰ. " + firstResolutionNo + @" ਮਿਤੀ: " + meetingDate + @" ਸਬੰਧੀ।
                        </p>";
                            }
                        }
                    }
                    else
                    {
                        outXml += @"<p style='margin-left:30px;font-weight:bold'>
                ਵਿਸ਼ਾ:- " + FromDeptLocal + @" ਦੇ ਮਤਾ ਨੰ. " + SrNos + @" ਮਿਤੀ: " + meetingDate + @" ਸਬੰਧੀ।
                </p>";
                    }
                }

                outXml += @"<div style = 'text-align:center;'>
                    <p> ********************</p>
                </div>";


                if (!string.IsNullOrWhiteSpace(Header))
                {
                    outXml += "<p style = 'margin-left:70px'>ਆਪ ਦੇ ਦਫਤਰ ਦੇ ਪੱਤਰ ਨੰ:<b>" + "#" + RefId + "</b> ਮਿਤੀ: <b>" + RecievedDate + @"</b> ਦੇ ਹਵਾਲੇ ਵਿੱਚ।
                </p>";
                    outXml += "<div style='margin-left:70px;'>"
                       + Header
                       + @"</div>";

                }
                else
                {

                    outXml += "<p style = 'margin-left:70px'>ਆਪ ਦੇ ਦਫਤਰ ਦੇ ਪੱਤਰ ਨੰ:<b>" + "#" + RefId + "</b> ਮਿਤੀ: <b>" + RecievedDate + @"</b> ਦੇ ਹਵਾਲੇ ਵਿੱਚ।
                </p>";

                    if (firstResolutionNo != lastResolutionNo)
                    {

                        outXml += @"<p style='margin-left:70px'>
                ਵਿਸੇ ਅਧੀਨ ਕੇਸ ਸਬੰਧੀ ਹਵਾਲੇ ਅਧੀਨ ਪੱਤਰ ਰਾਹੀਂ ਪ੍ਰਾਪਤ " + DestinationDept + @" ਦੇ ਮਤਾ ਨੰ. "
                                            + firstResolutionNo + @" ਤੋਂ " + lastResolutionNo +
                                            @" ਤੱਕ ਮਿਤੀ: " + meetingDate + @" ਸਬੰਧੀ ਸਮਰੱਥ ਅਥਾਰਟੀ ਵੱਲੋਂ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਫੈਸਲਾ ਕੀਤਾ ਗਿਆ ਹੈ: -
                </p>";
                    }
                    else
                    {
                        outXml += @"<p style='margin-left:70px'>
                ਵਿਸੇ ਅਧੀਨ ਕੇਸ ਸਬੰਧੀ ਹਵਾਲੇ ਅਧੀਨ ਪੱਤਰ ਰਾਹੀਂ ਪ੍ਰਾਪਤ " + DestinationDept + @" ਦੇ ਮਤਾ ਨੰ. "
                                          + firstResolutionNo +
                                          @" ਮਿਤੀ: " + meetingDate + @" ਸਬੰਧੀ ਸਮਰੱਥ ਅਥਾਰਟੀ ਵੱਲੋਂ ਹੇਠ ਲਿਖੇ ਅਨੁਸਾਰ ਫੈਸਲਾ ਕੀਤਾ ਗਿਆ ਹੈ: -
                </p>";
                    }
                }


                //else
                //{
                outXml += @"<table class='approval-table'>
                    <tr>
                            <th style='width:20%;text-align:center'>ਮਤਾ ਨੰ:</th>
                            <th style='width:50%;text-align:center'>ਫੈਸਲਾ</th>
   
                    </tr>";

                if (mdl._LstResolutionActions != null && mdl._LstResolutionActions.Count > 0)
                {
                    foreach (var r in mdl._LstResolutionActions)
                    {
                        var fixedfonthtml = "";
                        string comments = System.Net.WebUtility.HtmlDecode(r.LGComments);

                        comments = Regex.Replace(
                            comments,
                            @"font-size\s*:\s*\d+(\.\d+)?(px|pt|em|rem|%)\s*;?",
                            "",
                            RegexOptions.IgnoreCase);

                          fixedfonthtml += $@"<td style='font-size:12px;'>{comments}</td>";


                        outXml += @"<tr>
                            <td style='text-align:center'><b>" + r.ResolutionNo + @"</b></td>"
                            + fixedfonthtml +"</tr>";
                    }
                }
                else
                {
                    outXml += @"<tr>
                        <td colspan='3' style='text-align:center'>Comments not available</td>
                        </tr>";
                }

                outXml += @"</table>";
                //}
                if (!string.IsNullOrWhiteSpace(FooterText))
                {
                    outXml += "</br><div style = 'margin-left:70px'>"
                    + FooterText + @"</div><br/><br/>";

                }
                ;

                outXml += @"<p style='text-align:right;font-weight:bold'>"
                + CurrentSession.DesignationLocal + "</br>" + BranchName + @"</p>";
            }

            if (!string.IsNullOrWhiteSpace(endormentText))
            {
                outXml += "</br><div style = 'margin-left:70px'>" + endormentText + "</div>";

                outXml += @"<p style='text-align:right;font-weight:bold'>"
            + CurrentSession.DesignationLocal + "</br>" + BranchName + @"</p></div>";
            }
            else
            {
                if (FromDeptID == "UDD0001")
                {
                    outXml += "</br><div style = 'margin-left:70px'>ਲੋੜੀਂਦੀ ਕਾਰਵਾਈ ਲਈ :- ਕਾਰਜਕਾਰੀ ਅਧਿਕਾਰੀ ਦਫਤਰ," + DestinationDept + "</div>";
                    outXml += @"<p style='text-align:right;font-weight:bold'>"
                    + CurrentSession.DesignationLocal + "</br>" + BranchName + @"</p></div>";
                }
            }
            outXml += @"</body>
            </html>";
            return outXml;
        }


        public string GetProceedingCoveringLetterpdf(int agendaid, string refid)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            mdl._LstResolutionActions = AgendaModelFunction.GetProcedingDataForCommPDF(refid, "0");
            string outXml = "";
            string meetingdate = "";
            string meetingtime = "";
            string url = "~/SecureFileStructure/";
            string path = Server.MapPath(url);
            var path1 = BlobStorage.GetStorageAcessingPath();
            string logo = path1 + "MC/" + CurrentSession.MCName + "/mc_logo.jpg";

            string relativePath = "~/"; //"~/SecureFileStructure/";
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            string signature = path1 + CurrentSession.SignaturePath;


            if (HeaderlistofAgenda != null)
            {
                meetingdate = HeaderlistofAgenda.AgendaDate;
                meetingtime = HeaderlistofAgenda.StartTime;
            }
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                string imagePath = baseUrl + "/" + mdl._LstReferences[0].DeptLogo;
                outXml = outXml + @"<html>
                                            <head><meta charset='UTF-8'><title>MC: ProceedingsReport</title>
                                            
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
                                            <body><div class='pdf-class' style='max-width:1050px; margin:auto; '><div class='clearfix'></div>";
                outXml = outXml + @"<div>";
                outXml = outXml + @"<img src='" + logo + "' height='50' width='55' style='float:left' />";

                outXml += @"<p style='text-align: center !important;text-decoration: underline;font-weight:bold;font-size:20pt;padding-top: 16px;'>" + mdl._LstReferences[0].FromDeptLocal + "</p></br>";

                outXml += @"<span style='float:left;'>ਨੰਬਰ:... </span><span style='float:right;'>" + "ਮਿਤੀ: " + DateTime.Now.ToString("dd/MM/yyyy") + "</span></br></br>";
                outXml += @"<span style='float:left;'>ਸੇਵਾ ਵਿਖੇ,</span></br>";
                outXml += @"<div style='float:left;margin-left:80px;width:100%'><span>ਡਾਇਰੈਕਟਰ,</br>ਸਥਾਨਕ ਸਰਕਾਰ ਵਿਭਾਗ,</br>ਪੰਜਾਬ ।</span></br></br></div>";
                var firstResolutionNo = "";
                var lastResolutionNo = "";
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    firstResolutionNo = mdl._LstReferences[0].ResolutionNo;
                }
                outXml += @"<div class='clearfix'></div><span style='float:left;width:50px'>ਵਿਸ਼ਾ:- </span>" + "<span style='text-align: justify;'> " + mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: <b>" + meetingdate + "</b> ਦੇ ਮਤਾ ਨੰਬਰ <b>";
                if (mdl._LstReferences.Count > 1)
                {
                    foreach (var item2 in mdl._LstReferences)
                    {
                        lastResolutionNo = item2.ResolutionNo;
                    }
                    outXml += firstResolutionNo + "</b> ਤੋਂ <b>" + lastResolutionNo + "</b> ਤੱਕ ਦੀ ਕਾਰਵਾਈ ਭੇਜਣ ਬਾਰੇ।</br></br>";
                }
                else
                {
                    outXml += firstResolutionNo + "</b> ਦੀ ਕਾਰਵਾਈ ਭੇਜਣ ਬਾਰੇ।</br></br>";
                }
                outXml += @"<span style='float:left;margin-left:50px;text-align: justify;'>ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਬੇਨਤੀ ਹੈ ਕਿ " + mdl._LstReferences[0].FromDeptLocal + " ਜੀ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਮੇਅਰ ਦੇ ਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਰਾਂਹੀ ਮਿਤੀ: <b>";
                if (mdl._LstReferences.Count > 1)
                {
                    outXml += meetingdate + "</b> ਸਮਾਂ ਬਾਅਦ ਦੁਪਹਿਰ <b>" + meetingtime + " </b> ਵਜੇ ਦੇ ਮਤਾ ਨੰ: <b>" + firstResolutionNo + "</b> ਤੋਂ <b>" + lastResolutionNo + "</b> ਤੱਕ ਦੀ ਕਾਰਵਾਈ ਨੱਥੀ ਕਰਕੇ ਆਪ ਜੀ ਪਾਸ ਅਗਲੇਰੀ ਕਾਰਵਾਈ ਹਿੱਤ ਭੇਜੀ ਜਾਂਦੀ ਹੈ ਜੀ।</span></br></br></br>";
                }
                else
                {
                    outXml += meetingdate + "</b> ਸਮਾਂ ਬਾਅਦ ਦੁਪਹਿਰ <b>" + meetingtime + " </b> ਵਜੇ ਦੇ ਮਤਾ ਨੰ: <b>" + firstResolutionNo + "</b> ਦੀ ਕਾਰਵਾਈ ਨੱਥੀ ਕਰਕੇ ਆਪ ਜੀ ਪਾਸ ਅਗਲੇਰੀ ਕਾਰਵਾਈ ਹਿੱਤ ਭੇਜੀ ਜਾਂਦੀ ਹੈ ਜੀ।</span></br></br></br>";
                }

                outXml += @"</br></br></br><span style='float:left'>ਉਕਤ ਅਨੁਸਾਰ PDF ਫਾਈਲ ਨਾਲ ਲੱਗੀ ਹੈ।</span>";
                outXml += @"<span style='float:right;text-align:right'>ਸਕੱਤਰ,</br>ਨਗਰ ਨਿਗਮ,</br>" + mdl._LstReferences[0].FromDeptLocal + "</span></br></div></br></br></br>";
                outXml = outXml + @"<div style='page-break-before: always;'>";
                if (mdl._LstExtraReferences.Count > 0)
                {
                    for (int i = 0; i < mdl._LstExtraReferences.Count; i++)
                    {
                        outXml += @"<p>" + Convert.ToString(mdl._LstExtraReferences[i].SubjectDetails) + "</p><br>";
                    }
                }
                outXml += @"</div></br></br></br>";
              
                //Resolutions list start from here
                outXml += @"<div><h1 style='font-size: 20px; font-weight: 700; text-align: center;margin: auto;text-transform: uppercase;'>ਕਮਿਸ਼ਨਰ ਦਾ ਦਫ਼ਤਰ "
                        + mdl._LstReferences[0].FromDeptLocal + "</h1>";
                outXml += @"<p style='font-size: 16px; text-align: center;font-weight:bold'>Telephone No. " + mdl._LstReferences[0].DeptPhoneNo + "<br> Email:" + mdl._LstReferences[0].DeptEmail + "</p></div>";
                outXml += @"<table style='width:100%;font-size: 15px; border: 1px solid #cdcdcd;border-collapse: collapse;text-align: left;'>";
                outXml = outXml + @"<tr style = 'padding:5px;'>
                          <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>Sr. No</th>
                          <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>Resolution No. & Date</th> 
                          <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>Resolution Details </th>
                          <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>Please specify under which point<br> of instructions dated 10.10.2022<br> this resolution is covered</th>
                          <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>Comments of Commissioner/ E.O.</th></tr>";

                if (mdl._LstResolutionActions != null && mdl._LstResolutionActions.Count > 0)
                {
                    int j = 0;
                    for (int i = 0; i < mdl._LstResolutionActions.Count; i++)
                    {
                        if (mdl._LstResolutionActions[i].SerialNo == null || mdl._LstResolutionActions[i].SerialNo == "")
                        {
                            if (j == 0)
                            {
                                outXml += @"<tr>
        										<td colspan='5' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
        												Table Items</td></tr>";
                            }
                            j = j + 1;
                        }
                        //if (i == 0)
                        //{
                        outXml += @"<tr>
        										<td style='padding:10px; border: 1px solid #cdcdcd;border-collapse: collapse;vertical-align:top'>               
        												" + Convert.ToString(i + 1) + @"<br>    
        										</td>
        										<td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top '>
                                                " + Convert.ToString(mdl._LstResolutionActions[i].ResolutionNo) + @"<br>" + Convert.ToString(mdl._LstResolutionActions[i].AgendaDate) + @"
                                                </td>
                                               

                        <td style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>";
                        string _SubjectDetails = Convert.ToString(mdl._LstResolutionActions[i].SubjectDetails);
                        string _subject = Convert.ToString(mdl._LstResolutionActions[i].Subject);
                        if (_subject != "")
                        {

                            _subject = "<b>" + _subject + " :- </b>";
                        }


                        outXml += @"" + _subject + " " + _SubjectDetails;
                        outXml += @"<br><b>House Decision:</b><br>" + Convert.ToString(mdl._LstResolutionActions[i].HouseDecision) + @" </td>
                                               

                       <td style ='padding:10px;margin-bottom: 20px; border: 1px solid #cdcdcd;border-collapse: collapse;vertical-align:top'>
                                               " + Convert.ToString(mdl._LstResolutionActions[i].Act) + @"                                             
                                                </td><td style ='padding:10px;margin-bottom: 20px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>
                                               " + Convert.ToString(mdl._LstResolutionActions[i].commissioner) + @"
                                                </tr>";
                    }
                }
                outXml += @"</table>";
                outXml += @"</div></body> </html>";
            }
            return outXml;
        }

        //aman
        public string GetProceedingCoveringLetterpdfNew(int agendaid, string refid)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            mdl._LstExtraReferences = AgendaModelFunction.GetAdminlobExtraDataForProcedingPDF(agendaid);
            mdl._LstResolutionActions = AgendaModelFunction.GetProcedingDataForCommPDF(refid, "0");
            string outXml = "";
            string meetingdate = "";
            string meetingtime = "";
            string url = "~/SecureFileStructure/";
            string path = Server.MapPath(url);
            var path1 = BlobStorage.GetStorageAcessingPath();
            string logo = path1 + "MC/" + CurrentSession.MCName + "/mc_logo.jpg";

            string relativePath = "~/"; //"~/SecureFileStructure/";
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            string signature = path1 + "/" + CurrentSession.SignaturePath;

            string officetype = string.Empty;
            officetype = AgendaModelFunction.GetOfficeType(agendaid);

            if (HeaderlistofAgenda != null)
            {
                meetingdate = HeaderlistofAgenda.AgendaDate;
                meetingtime = HeaderlistofAgenda.StartTime;
            }
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                string imagePath = baseUrl + "/" + mdl._LstReferences[0].DeptLogo;
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
                                            <body><div class='pdf-class' style='max-width:1050px; margin:auto; '><div class='clearfix'></div>";
                outXml = outXml + @"<div>";
                outXml = outXml + @"<img src='" + logo + "' height='50' width='55' style='float:left' />";

                outXml += @"<p style='text-align: center !important;text-decoration: underline;font-weight:bold;font-size:20pt;padding-top: 16px;'>" + mdl._LstReferences[0].FromDeptLocal + "</p></br>";

                outXml += @"<span style='float:left;'>ਨੰਬਰ:... </span><span style='float:right;'>" + "ਮਿਤੀ: " + DateTime.Now.ToString("dd/MM/yyyy") + "</span></br></br>";
                outXml += @"<span style='float:left;'>ਸੇਵਾ ਵਿਖੇ,</span></br>";
                outXml += @"<div style='float:left;margin-left:80px;width:100%'><span>ਡਾਇਰੈਕਟਰ,</br>ਸਥਾਨਕ ਸਰਕਾਰ ਵਿਭਾਗ,</br>ਪੰਜਾਬ ।</span></br></br></div>";
                var firstResolutionNo = "";
                var lastResolutionNo = "";
                //  for (int i = 0; i < mdl._LstReferences.Count; i++)
                //  {
                firstResolutionNo = mdl._LstReferences[0].SrNo.ToString();
                //  }
                //  outXml += @"<div class='clearfix'></div><span style='float:left;width:50px'>ਵਿਸ਼ਾ:- </span>" + "<span style='text-align: justify;'> " + mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: <b>" + meetingdate + "</b> ਦੇ ਮਤਾ ਨੰਬਰ <b>";
                outXml += @"<div class='clearfix'></div>";
                if (officetype == "F&CC")
                    outXml += @"</br><div><span style='float:left;width:50px'>ਵਿਸ਼ਾ:- </span>" + "<span style='text-align: justify;'> " + mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਵਿੱਤ ਅਤੇ ਠੇਕਾ ਕਮੇਟੀ ਮੀਟਿੰਗ ਮਿਤੀ: <b>" + meetingdate + "</b> ਦੇ ਮਤਾ ਨੰਬਰ <b>";
                else
                    outXml += @"</br><div><span style='float:left;width:50px'>ਵਿਸ਼ਾ:- </span>" + "<span style='text-align: justify;'> " + mdl._LstReferences[0].FromDeptLocal + " ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: <b>" + meetingdate + "</b> ਦੇ ਮਤਾ ਨੰਬਰ <b>";
                if (mdl._LstReferences.Count > 1)
                {
                    // foreach (var item2 in mdl._LstReferences)
                    //  {
                    lastResolutionNo = mdl._LstReferences[mdl._LstReferences.Count - 1].SrNo.ToString();
                    //  }
                    outXml += firstResolutionNo + "</b> ਤੋਂ <b>" + lastResolutionNo + "</b> ਤੱਕ ਦੀ ਕਾਰਵਾਈ ਭੇਜਣ ਬਾਰੇ।</br></br>";
                }
                else
                {
                    outXml += firstResolutionNo + "</b> ਦੀ ਕਾਰਵਾਈ ਭੇਜਣ ਬਾਰੇ।</br></br>";
                }
                // outXml += @"<span style='float:left;margin-left:50px;text-align: justify;'>ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਬੇਨਤੀ ਹੈ ਕਿ " + mdl._LstReferences[0].FromDeptLocal + " ਜੀ ਦੀ ਪ੍ਰਧਾਨਗੀ ਹੇਠ ਮੇਅਰ ਦੇ ਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਰਾਂਹੀ ਮਿਤੀ: <b>";
                if (officetype == "F&CC")
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

                outXml += @"</br></br></br><span style='float:left'>ਉਕਤ ਅਨੁਸਾਰ PDF ਫਾਈਲ ਨਾਲ ਲੱਗੀ ਹੈ।</span>";
                outXml += @"<span style='float:right;text-align:right'>ਸਕੱਤਰ,</br>ਨਗਰ ਨਿਗਮ,</br>" + mdl._LstReferences[0].FromDeptLocal + "</span></br></div></br></br></br>";
                outXml = outXml + @"<div style='page-break-before: always;'>";
                if (mdl._LstExtraReferences.Count > 0)
                {
                    for (int i = 0; i < mdl._LstExtraReferences.Count; i++)
                    {
                        outXml += @"<p>" + Convert.ToString(mdl._LstExtraReferences[i].SubjectDetails) + "</p><br>";
                    }
                }
                outXml += @"</div></br></br></br>";
                //Resolutions list start from here
                outXml += @"<div><h1 style='font-size: 20px; font-weight: 700; text-align: center;margin: auto;text-transform: uppercase;'>ਕਮਿਸ਼ਨਰ ਦਾ ਦਫ਼ਤਰ "
                        + mdl._LstReferences[0].FromDeptLocal + "</h1>";
                outXml += @"<p style='font-size: 16px; text-align: center;font-weight:bold'>Telephone No. " + mdl._LstReferences[0].DeptPhoneNo + "<br> Email:" + mdl._LstReferences[0].DeptEmail + "</p></div>";
                outXml += @"<table style='width:100%;font-size: 15px; border: 1px solid #cdcdcd;border-collapse: collapse;text-align: left;'>";
                outXml = outXml + @"<tr style = 'padding:5px;'>
                          <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>Sr. No</th>
                          <th style ='padding:10px; border: 1px solid  #cdcdcd;border - collapse: collapse;vertical-align:top'>Resolution No. & Date</th> 
                          <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>Resolution Details </th>
                          <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>Please specify under which point<br> of instructions dated 10.10.2022<br> this resolution is covered</th>
                          <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>Comments of Commissioner/ E.O.</th></tr>";

                if (mdl._LstResolutionActions != null && mdl._LstResolutionActions.Count > 0)
                {
                    int j = 0;
                    for (int i = 0; i < mdl._LstResolutionActions.Count; i++)
                    {
                        if (mdl._LstResolutionActions[i].SerialNo == null || mdl._LstResolutionActions[i].SerialNo == "")
                        {
                            if (j == 0)
                            {
                                //              outXml += @"<tr>
                                //<td colspan='5' style='padding:10px; border: 1px solid black;border-collapse: collapse;font-weight:bold'>               
                                //		Table Items</td></tr>";
                            }
                            j = j + 1;
                        }
                        //if (i == 0)
                        //{
                        outXml += @"<tr>
        										<td style='padding:10px; border: 1px solid #cdcdcd;border-collapse: collapse;vertical-align:top'>               
        												" + Convert.ToString(i + 1) + @"<br>    
        										</td>
        										<td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top '>
                                                " + Convert.ToString(mdl._LstResolutionActions[i].LetterNo) + @"<br>" + Convert.ToString(mdl._LstResolutionActions[i].AgendaDate) + @"
                                                </td>
                                               

                        <td style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>";
                        string _SubjectDetails = Convert.ToString(mdl._LstResolutionActions[i].SubjectDetails);
                        string _subject = Convert.ToString(mdl._LstResolutionActions[i].Subject);
                        if (_subject != "")
                        {

                            _subject = "<b>" + _subject + " :- </b>";
                        }


                        outXml += @"" + _subject + " " + _SubjectDetails;
                        outXml += @"<br><b>House Decision:</b><br>" + Convert.ToString(mdl._LstResolutionActions[i].HouseDecision) + @" </td>
                                               

                       <td style ='padding:10px;margin-bottom: 20px; border: 1px solid #cdcdcd;border-collapse: collapse;vertical-align:top'>
                                               " + Convert.ToString(mdl._LstResolutionActions[i].Act) + @"                                             
                                                </td><td style ='padding:10px;margin-bottom: 20px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>
                                               " + Convert.ToString(mdl._LstResolutionActions[i].commissioner) + @"
                                                </tr>";
                    }
                }
                outXml += @"</table>";
                outXml += @"<br/><br/><div style='text-align:right;padding-top:5px'><img src='" + signature + "' alt='Signature' style='height:50px;'></div>";
                //if (HeaderlistofAgenda != null)
                //{
                outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                                         ਕਮਿਸ਼ਨਰ,<br>" + mdl._LstReferences[0].FromOfficeLocal + "</p>";
                outXml += @"</div></body> </html>";
            }
            return outXml;
        }

        static string RemoveHtmlTags(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", String.Empty);
        }
        public ActionResult GetDashboardStatDetail(string DepartmentID, string FromDeptId, string FileType, string documentype, string detail, string userid)
        {
            try
            {
                LstDashboard model = new LstDashboard();
                var data1 = tPaperLaidV.GetMCStatDetailsList(DepartmentID, FromDeptId, FileType, documentype, userid, "1", 0, 0, 2);
                model._LstStatDetail = data1;
                model.OutLineHeader = detail;
                return PartialView(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetDataForDiaryDashboardStatusNew(string financialyear, string mType, int FileType = 0, int RefType = 0, string MenuName = "Reference")
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            int ShowType = 0;
            if (CurrentSession.isDeptApex == "1")
            {
                ShowType = 2;
            }
            else
            {
                ShowType = 1;
            }
            dt = DiaryDashboard.GetDataForDiaryDashboardStat(CurrentSession.OfficeId, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus, financialyear, Convert.ToInt16(mType), FileType, RefType, ShowType);

            if (dt.Rows.Count > 0)
            {
                int count = dt.Columns.Count;
                if (CurrentSession.isDeptApex == "1")
                {

                    sb = sb.Append("<tr role='row' class='table-heading'>");
                    sb = sb.Append("<td style= 'text-align:center; ' class='td-heading' id='tdFrom'></td>");
                    sb = sb.Append("<td style= 'text-align:center; ' colspan= 1 class='td-heading' id='tdFrom'>(Inbox+Sent)</td>");
                    sb = sb.Append("<td style= 'text-align:center; ' colspan= " + count + " class='td-heading' id='tdreflabel'>" + MenuName + "</td>");
                    sb = sb.Append("</tr>");

                }
                else
                {
                    sb = sb.Append("<tr role='row' class='table-heading'>");
                    sb = sb.Append("<th style= 'text-align:center;background-color: white !important;' colspan= 3 class='' id='tdFrom'></td>");
                    sb = sb.Append("<th style= 'text-align:center; ' colspan= " + 2 + " class='td-heading' id='tdreflabel'>Agenda Items</td>");
                    sb = sb.Append("<th style= 'text-align:center; ' colspan= " + 4 + " class='td-heading' id='tdreflabel'>Cabinet Decisions</td>");
                    sb = sb.Append("<th style= 'text-align:center;background-color: white !important;' colspan= " + 3 + " class='' id='tdreflabel'>Approved</td>");
                    sb = sb.Append("</tr>");

                }

                sb = sb.Append("<body>");

                sb = sb.Append("<tr role='row' class='table-heading'>");

                sb = sb.Append("<td class='td-heading' id='tdFrom'>Department</td>");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</td>");
                    }
                }

                sb = sb.Append("</tr>");
                sb = sb.Append("<tr role='row' class='table-heading'>");
                sb = sb.Append("<td class='td-heading' id='tdFrom'></td>");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        if (dt.Columns[i].ColumnName.ToString() == "Meeting Files_")
                        {
                            sb = sb.Append("<td class='td-heading' >" + "GAD" + "</td>");
                        }
                        else if (dt.Columns[i].ColumnName.ToString() == "House Resolutions_")
                        {
                            sb = sb.Append("<td class='td-heading' >" + "GAD" + "</td>");
                        }
                        else if (dt.Columns[i].ColumnName.ToString() == "Pending_")
                        {
                            sb = sb.Append("<td class='td-heading' >" + "GAD" + "</td>");
                        }
                        else
                        {
                            sb = sb.Append("<td class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
                        }


                    }
                    //count++;
                }

                sb = sb.Append("</tr>");

            }
            int j = 0, GTotal = 0;

            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
                sb = sb.Append("<td  class='center td-heading' onclick='GoToStatusType(&#39" + drDyn["deptId"] + "&#39)'>" + drDyn["deptName"].ToString() + "</td>");

                int Total = 0; string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a data-target='DashboardStat' DeptName='" + drDyn["deptName"] + "'  Mainid='trtd" + drDyn["deptId"] + "'FromDeptId='" + drDyn["FromDeptId"] + "'  onclick ='GetDiaryDasdhboardDataChange(this)' DeptId='" + drDyn["deptId"] + "'DocTypeId='" + docid + "'OfficeId='" + drDyn["officeid"] + "' + >" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr id='trtd0'>");
                sb = sb.Append("<td class='center td-heading'>Total</td> ");

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
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                        sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
            }
            else
            {
                sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
            }
            sb = sb.Append("</tr> ");

            sb = sb.Append("</body>");
            return Content(sb.ToString());
        }

        public ActionResult GetDataForDiaryDashboardStatusDepartment(string financialyear, string mType, int FileType = 0, int RefType = 0, string MenuName = "Reference")
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            int ShowType = 0;
            if (CurrentSession.isDeptApex == "1")
            {
                ShowType = 2;
            }
            else
            {
                ShowType = 1;
            }
            dt = DiaryDashboard.GetDataForDiaryDashboardStatDepartment(CurrentSession.OfficeId, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus, financialyear, Convert.ToInt16(mType), FileType, RefType, ShowType);

            if (dt.Rows.Count > 0)
            {
                int count = dt.Columns.Count;
                sb = sb.Append("<table class='table table-bordered' id='StatsDepartment' aria-describedby='sample-table-3_info'>");
                sb = sb.Append("<thead>");
                if (CurrentSession.isDeptApex == "1")
                {

                    sb = sb.Append("<tr role='row' class='table-heading'>");
                    sb = sb.Append("<th style= 'text-align:center; ' class='td-heading' id='tdFrom'></td>");
                    sb = sb.Append("<th style= 'text-align:center; ' colspan= 1 class='td-heading' id='tdFrom'>(Inbox+Sent)</td>");
                    sb = sb.Append("<th style= 'text-align:center; ' colspan= " + count + " class='td-heading' id='tdreflabel'>" + MenuName + "</td>");
                    sb = sb.Append("</tr>");
                    // sb = sb.Append("<tr role='row' class='table-heading'>");
                }
                else
                {
                    sb = sb.Append("<tr role='row' class='table-heading'>");
                    sb = sb.Append("<th style= 'text-align:center;background-color: white !important;' colspan= 3 class='' id='tdFrom'></td>");
                    sb = sb.Append("<th style= 'text-align:center; ' colspan= " + 2 + " class='td-heading' id='tdreflabel'>Agenda Items</td>");
                    sb = sb.Append("<th style= 'text-align:center; ' colspan= " + 4 + " class='td-heading' id='tdreflabel'>Cabinet Decisions</td>");
                    sb = sb.Append("<th style= 'text-align:center;background-color: white !important;' colspan= " + 3 + " class='' id='tdreflabel'>Approved</td>");
                    sb = sb.Append("</tr>");

                }



                sb = sb.Append("<tr role='row' class='table-heading'>");

                sb = sb.Append("<th class='td-heading' id='tdFrom'>Department</td>");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb = sb.Append("<th class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</td>");
                    }
                }

                sb = sb.Append("</tr>");
                sb = sb.Append("<tr role='row' class='table-heading'>");
                sb = sb.Append("<th class='td-heading' id='tdFrom'></td>");



                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        if (dt.Columns[i].ColumnName.ToString() == "Meeting Files_")
                        {
                            sb = sb.Append("<th class='td-heading' >" + "GAD" + "</td>");
                        }
                        else if (dt.Columns[i].ColumnName.ToString() == "House Resolutions_")
                        {
                            sb = sb.Append("<th class='td-heading' >" + "GAD" + "</td>");
                        }
                        else if (dt.Columns[i].ColumnName.ToString() == "Pending_")
                        {
                            sb = sb.Append("<th class='td-heading' >" + "GAD" + "</td>");
                        }
                        else
                        {
                            sb = sb.Append("<th class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
                        }

                    }
                    //count++;
                }
                sb = sb.Append("</thead>");
                sb = sb.Append("<tbody>");
                sb = sb.Append("</tr>");

            }
            int j = 0, GTotal = 0;

            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
                sb = sb.Append("<td  class='center td-heading' onclick='GoToStatusType(&#39" + drDyn["deptId"] + "&#39)'>" + drDyn["deptName"].ToString() + "</td>");

                int Total = 0; string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a data-target='DashboardStat' DeptName='" + drDyn["deptName"] + "'  Mainid='trtd" + drDyn["deptId"] + "'FromDeptId='" + drDyn["FromDeptId"] + "'  onclick ='GetDiaryDasdhboardDataChangeDepartment(this)' DeptId='" + drDyn["deptId"] + "'DocTypeId='" + docid + "'OfficeId='" + drDyn["officeid"] + "' + >" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr id='trtd0'>");
                sb = sb.Append("<td class='center td-heading'>Total</td> ");

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
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                        sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
            }
            else
            {
                sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
            }
            sb = sb.Append("</tr> ");
            sb = sb.Append("</tbody>");
            sb = sb.Append("</table>");
            return Content(sb.ToString());
        }

        //public ActionResult GetDataForDiaryDashboardStatusDepartmentOpenAll(string financialyear, string mType, int FileType = 0, int RefType = 0, string MenuName = "Reference", string selectedType = "", string MCType = "", string MCClass = "")
        //{
        //    StringBuilder sb = new StringBuilder();
        //    DataTable dtstatus;
        //    DataTable dt = new DataTable();
        //    int ShowType = 0;
        //    if (CurrentSession.isDeptApex == "1")
        //    {
        //        ShowType = 2;
        //    }
        //    else
        //    {
        //        ShowType = 1;
        //    }
        //    dt = DiaryDashboard.GetDataForDiaryDashboardStatDepartmentOpenAll(CurrentSession.OfficeId, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus, financialyear, Convert.ToInt16(mType), FileType, RefType, ShowType, selectedType, MCType, MCClass);

        //    if (dt.Rows.Count > 0)
        //    {
        //        int count = dt.Columns.Count;
        //        sb = sb.Append("<table class='table table-bordered' id='StatsDepartment' aria-describedby='sample-table-3_info'>");
        //        sb = sb.Append("<thead>");

        //        sb = sb.Append("<tr role='row' class='table-heading'>");
        //        sb = sb.Append("<th style= 'text-align:center;background-color: white !important;' colspan= 6 class='' id='tdFrom'></td>");
        //        sb = sb.Append("<th style= 'text-align:center;background-color: white !important; ' colspan= " + 3 + " class='td-heading' id='tdreflabel'></td>");
        //        sb = sb.Append("<th style= 'text-align:center;background-color: white !important;' colspan= " + 3 + " class='td-heading' id='tdreflabel'></td>");
        //        sb = sb.Append("</tr>");


        //        sb = sb.Append("<tr role='row' class='table-heading'>");

        //        sb = sb.Append("<th class='td-heading' id='tdFrom'>MC</td>");

        //        for (int i = 1; i < dt.Columns.Count; i++)
        //        {
        //            DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
        //            if (result1.Length > 0)
        //            {
        //                var s = result1[0].ItemArray[1].ToString();

        //                if (s == "Comr/EO Comments_MC")
        //                {
        //                    s = "Not Dealt_LG";
        //                }
        //                if (s == "ADC Comment_ADC")
        //                {
        //                    s = "Not Dealt_LG";
        //                }

        //                sb = sb.Append("<th class='td-heading' >" + s.Trim().Substring(0, s.IndexOf("_")) + "</td>");
        //            }
        //        }

        //        sb = sb.Append("</tr>");
        //        sb = sb.Append("<tr role='row' class='table-heading'>");
        //        sb = sb.Append("<th class='td-heading' id='tdFrom'></td>");



        //        for (int i = 1; i < dt.Columns.Count; i++)
        //        {
        //            DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
        //            if (result1.Length > 0)
        //            {
        //                if (dt.Columns[i].ColumnName.ToString() == "Files Received_")
        //                {
        //                    sb = sb.Append("<th class='td-heading' >" + "LG" + "</td>");
        //                }
        //                else if (dt.Columns[i].ColumnName.ToString() == "Total Resolutions_")
        //                {
        //                    sb = sb.Append("<th class='td-heading' >" + "LG" + "</td>");
        //                }
        //                else if (dt.Columns[i].ColumnName.ToString() == "Comr/EO Comments_MC")
        //                {
        //                    sb = sb.Append("<th class='td-heading' >" + "LG" + "</td>");
        //                }
        //                else if (dt.Columns[i].ColumnName.ToString() == "ADC Comment_ADC")
        //                {
        //                    sb = sb.Append("<th class='td-heading' >" + "LG" + "</td>");
        //                }

        //                else
        //                {
        //                    sb = sb.Append("<th class='td-heading' >" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
        //                }

        //            }
        //            //count++;
        //        }
        //        sb = sb.Append("</thead>");
        //        sb = sb.Append("<tbody>");
        //        sb = sb.Append("</tr>");

        //    }
        //    int j = 0, GTotal = 0;

        //    foreach (DataRow drDyn in dt.Rows)
        //    {
        //        sb = sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");
        //        sb = sb.Append("<td  class='center td-heading' onclick='GoToStatusType(&#39" + drDyn["deptId"] + "&#39)'>" + drDyn["deptName"].ToString() + "</td>");

        //        int Total = 0; string cnt;
        //        for (int i = 1; i < dt.Columns.Count; i++)
        //        {
        //            DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
        //            if (result.Length > 0)
        //            {
        //                int docid = Convert.ToInt32(result[0].ItemArray[0]);
        //                cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
        //                Total = Total + Convert.ToInt32(cnt);
        //            }
        //        }

        //        for (int i = 1; i < dt.Columns.Count; i++)
        //        {
        //            DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
        //            if (result.Length > 0)
        //            {
        //                int docid = Convert.ToInt32(result[0].ItemArray[0]);
        //                cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
        //                Total = Total + Convert.ToInt32(cnt);
        //                sb.Append("<td class='center td-heading'><a data-target='DashboardStat' DeptName='" + drDyn["deptName"] + "' Mainid='trtd" + drDyn["deptId"] + "' FromDeptId='" + drDyn["FromDeptId"] + "' DestinationDeptID='" + drDyn["DestinationDeptID"] + "' onclick='OpenChangeDepartment(this)' DeptId='" + drDyn["deptId"] + "' fyear='" + financialyear + "' DocTypeId='" + docid + "' OfficeId='" + drDyn["officeid"] + "'>" + cnt + "</a></td>");
        //            }
        //        }
        //        sb = sb.Append("</tr>");
        //        j = j + 1;
        //    }
        //    if (dt.Rows.Count > 0)
        //    {
        //        sb = sb.Append("<tr id='trtd0'>");
        //        sb = sb.Append("<td class='center td-heading'>Total</td> ");

        //        for (int i = 1; i < dt.Columns.Count; i++)
        //        {
        //            DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
        //            if (result1.Length > 0)
        //            {
        //                int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
        //                int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
        //                //int cnt = 0;
        //                GTotal = GTotal + cnt;
        //            }
        //        }
        //        for (int i = 1; i < dt.Columns.Count; i++)
        //        {
        //            DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
        //            if (result1.Length > 0)
        //            {
        //                int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
        //                int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
        //                //int cnt = 0;
        //                GTotal = GTotal + cnt;
        //                sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
        //    }
        //    sb = sb.Append("</tr> ");
        //    sb = sb.Append("</tbody>");
        //    sb = sb.Append("</table>");
        //    return Content(sb.ToString());
        //}


       public ActionResult GetDataForDiaryDashboardStatusDepartmentOpenAll(string financialyear,string mType,int FileType = 0,int RefType = 0,string MenuName = "Reference", string selectedType = "",
       string MCType = "", string MCClass = "")
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();

            int ShowType = (CurrentSession.isDeptApex == "1") ? 2 : 1;

            dt = DiaryDashboard.GetDataForDiaryDashboardStatDepartmentOpenAll(
                CurrentSession.OfficeId,
                Convert.ToString(CurrentSession.UserID),
                CurrentSession.DeptID,
                out dtstatus,
                financialyear,
                Convert.ToInt16(mType),
                FileType,
                RefType,
                ShowType,
                selectedType,
                MCType,
                MCClass
            );

            if (dt.Rows.Count > 0)
            {
                int top1 = 0;
                int top2 = 28; // compact header height

                sb.Append("<div class='table-responsive' style='max-height:520px; overflow:auto;'>");
                sb.Append("<table class='table table-bordered table-striped dataTable no-footer' id='StatsDepartment' " +
                "style='font-size:12px;'>");

                if (CurrentSession.DeptID == "LG00001")
                {

                    sb.Append("<thead>");

                    // ========================= ROW 1 =========================

                    sb.Append("<tr>");

                    sb.Append("<th rowspan='2' style='position:sticky;top:" + top1 + "px;z-index:5;" +
                              "background:#1e293b;color:#fff;" +
                              "border:1px solid #334155;text-align:center;" +
                              "vertical-align:middle;font-weight:700;'>MC</th>");

                    sb.Append("<th colspan='2' style='position:sticky;top:" + top1 + "px;z-index:5;" +
                              "background:#1e40af;color:#fff;" +
                              "border:1px solid #3b82f6;text-align:center;" +
                              "font-weight:700;'>Cumulative</th>");

                    sb.Append("<th colspan='2' style='position:sticky;top:" + top1 + "px;z-index:5;" +
                              "background:#047857;color:#fff;" +
                              "border:1px solid #10b981;text-align:center;" +
                              "font-weight:700;'>Approved</th>");

                    sb.Append("<th rowspan='2' style='position:sticky;top:" + top1 + "px;z-index:5;" +
                              "background:#b45309;color:#fff;" +
                              "border:1px solid #f59e0b;text-align:center;" +
                              "vertical-align:middle;font-weight:700;'>Rejected<br/><span style='font-size:9.5px;font-weight:500;opacity:0.9;display:block;'>Head Office / MC</span></th>");

                    sb.Append("<th colspan='2' style='position:sticky;top:" + top1 + "px;z-index:5;" +
                              "background:#6d28d9;color:#fff;" +
                              "border:1px solid #8b5cf6;text-align:center;" +
                              "font-weight:700;'>LG Observations / Advice</th>");

                    sb.Append("<th colspan='2' style='position:sticky;top:" + top1 + "px;z-index:5;" +
                              "background:#be123c;color:#fff;" +
                              "border:1px solid #f43f5e;text-align:center;" +
                              "font-weight:700;'>Branch Comments</th>");

                    sb.Append("</tr>");


                    // ========================= ROW 2 =========================

                    sb.Append("<tr>");

                    sb.Append("<th style='position:sticky;top:" + top2 + "px;z-index:4;" +
                              "background:#eff6ff;color:#1e40af;border:1px solid #cbd5e1;" +
                              "text-align:center;font-weight:600;'>Proceedings<br/><span style='font-size:9.5px;font-weight:500;color:#64748b;display:block;'>Head Office</span></th>");

                    sb.Append("<th style='position:sticky;top:" + top2 + "px;z-index:4;" +
                              "background:#eff6ff;color:#1e40af;border:1px solid #cbd5e1;" +
                              "text-align:center;font-weight:600;'>Resolutions<br/><span style='font-size:9.5px;font-weight:500;color:#64748b;display:block;'>Head Office</span></th>");

                    sb.Append("<th style='position:sticky;top:" + top2 + "px;z-index:4;" +
                              "background:#ecfdf5;color:#047857;border:1px solid #cbd5e1;" +
                              "text-align:center;font-weight:600;'>Proceedings<br/><span style='font-size:9.5px;font-weight:500;color:#64748b;display:block;'>Head Office</span></th>");

                    sb.Append("<th style='position:sticky;top:" + top2 + "px;z-index:4;" +
                              "background:#ecfdf5;color:#047857;border:1px solid #cbd5e1;" +
                              "text-align:center;font-weight:600;'>Resolutions<br/><span style='font-size:9.5px;font-weight:500;color:#64748b;display:block;'>Head Office</span></th>");

                    sb.Append("<th style='position:sticky;top:" + top2 + "px;z-index:4;" +
                              "background:#f5f3ff;color:#6d28d9;border:1px solid #cbd5e1;" +
                              "text-align:center;font-weight:600;'>Observations<br/><span style='font-size:9.5px;font-weight:500;color:#64748b;display:block;'>HO / Advisor</span></th>");

                    sb.Append("<th style='position:sticky;top:" + top2 + "px;z-index:4;" +
                              "background:#f5f3ff;color:#6d28d9;border:1px solid #cbd5e1;" +
                              "text-align:center;font-weight:600;'>Advice<br/><span style='font-size:9.5px;font-weight:500;color:#64748b;display:block;'>Head Office</span></th>");

                    sb.Append("<th style='position:sticky;top:" + top2 + "px;z-index:4;" +
                              "background:#fff1f2;color:#be123c;border:1px solid #cbd5e1;" +
                              "text-align:center;font-weight:600;'>In Progress<br/><span style='font-size:9.5px;font-weight:500;color:#64748b;display:block;'>Head Office</span></th>");

                    sb.Append("<th style='position:sticky;top:" + top2 + "px;z-index:4;" +
                              "background:#fff1f2;color:#be123c;border:1px solid #cbd5e1;" +
                              "text-align:center;font-weight:600;'>Not Started<br/><span style='font-size:9.5px;font-weight:500;color:#64748b;display:block;'>Head Office</span></th>");

                    sb.Append("</tr>");

                    sb.Append("</thead>");


                }
                // ================= HEADER =================
                else
                {
                    sb.Append("<thead>");

                    // -------- HEADER ROW 1 --------
                    sb.Append("<tr>");
                    sb.Append("<th style='position:sticky; top:" + top1 + "px; background:#f8f9fa; z-index:3; border-bottom:1px solid #ddd;'>MC</th>");

                    for (int i = 1; i < dt.Columns.Count; i++)
                    {
                        DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName + "'");
                        if (result1.Length > 0)
                        {
                            string s = result1[0].ItemArray[1].ToString();

                            if (s == "Comr/EO Comments_MC" || s == "ADC Comment_ADC" && CurrentSession.DeptID == "LG00001")
                                s = "Not Dealt_LG";

                            if (s == "Comr/EO Comments_MC" && CurrentSession.DeptID == "UDD0001")
                                s = "Not Dealt_ADC";

                            string header = s.Contains("_") ? s.Split('_')[0] : s;

                            sb.Append("<th style='position:sticky; top:" + top1 + "px; background:#f8f9fa; z-index:3; border-bottom:1px solid #ddd;'>" + header + "</th>");
                        }
                    }
                    sb.Append("</tr>");

                    // -------- HEADER ROW 2 --------
                    sb.Append("<tr>");
                    sb.Append("<th style='position:sticky; top:" + top2 + "px; background:#ffffff; z-index:2;'></th>");

                    var deptMap = new Dictionary<string, string>
                {
                    { "LG00001", "LG" },
                    { "UDD0001", "ADC" }
                };

                    string dept = CurrentSession.DeptID;

                    for (int i = 1; i < dt.Columns.Count; i++)
                    {
                        DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName + "'");
                        if (result1.Length > 0)
                        {
                            string colName = dt.Columns[i].ColumnName;

                            if (colName == "Files Received_" ||
                                colName == "Total Resolutions_" ||
                                colName == "Comr/EO Comments_MC" ||
                                colName == "ADC Comment_ADC")
                            {
                                string headerText = deptMap.ContainsKey(dept) ? deptMap[dept] : "";
                                sb.Append("<th style='position:sticky; top:" + top2 + "px; background:#ffffff; z-index:2;'>" + headerText + "</th>");
                            }
                            else
                            {
                                string s = result1[0].ItemArray[1].ToString();
                                string subHeader = s.Contains("_") ? s.Split('_')[1] : "";

                                sb.Append("<th style='position:sticky; top:" + top2 + "px; background:#ffffff; z-index:2;'>" + subHeader + "</th>");
                            }
                        }
                    }

                    sb.Append("</tr>");
                    sb.Append("</thead>");
                }

                // ================= BODY =================
                sb.Append("<tbody>");

                foreach (DataRow drDyn in dt.Rows)
                {
                    sb.Append("<tr id='trtd" + drDyn["deptId"] + "'>");

                    sb.Append("<td class='center td-heading' style='cursor:pointer;' onclick='GoToStatusType(\""
                        + drDyn["deptId"] + "\")'>" + drDyn["deptName"] + "</td>");

                    for (int i = 1; i < dt.Columns.Count; i++)
                    {
                        DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName + "'");
                        if (result.Length > 0)
                        {
                            int docid = Convert.ToInt32(result[0].ItemArray[0]);

                            string cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName]))
                                ? "0"
                                : Convert.ToString(drDyn[dt.Columns[i].ColumnName]);

                            sb.Append("<td class='center td-heading' style='text-align:center;'>");

                            if (cnt == "0")
                            {
                                sb.Append("<span>" + cnt + "</span>");
                            }
                            else
                            {
                                sb.Append("<a style='display:inline-block;color:#0d6efd;" +
                                          "border-radius:6px;font-weight:600;text-decoration:none;cursor:pointer;" +
                                          "transition:all .2s ease;' " +

                                          "onmouseover='this.style.background=\"#d0e2ff\"' " +
                                          "onmouseout='this.style.background=\"#e7f1ff\"' " +

                                          "data-target='DashboardStat' " +
                                          "DeptName='" + drDyn["deptName"] + "' " +
                                          "Mainid='trtd" + drDyn["deptId"] + "' " +
                                          "FromDeptId='" + drDyn["FromDeptId"] + "' " +
                                          "DestinationDeptID='" + drDyn["DestinationDeptID"] + "' " +
                                          "onclick='OpenChangeDepartment(this)' " +
                                          "DeptId='" + drDyn["deptId"] + "' " +
                                          "fyear='" + financialyear + "' " +
                                          "DocTypeId='" + docid + "' " +
                                          "OfficeId='" + drDyn["officeid"] + "'>" +

                                          cnt + "</a>");
                            }

                            sb.Append("</td>");
                        }
                    }

                    sb.Append("</tr>");
                }

                // ================= TOTAL =================
                sb.Append("<tr>");
                sb.Append("<td class='center td-heading' style='text-align:center;'><b>Total</b></td>");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName + "'");
                    if (result1.Length > 0)
                    {
                        string colName = dt.Columns[i].ColumnName;

                        int total = dt.AsEnumerable()
                            .Sum(row => string.IsNullOrEmpty(Convert.ToString(row[colName])) ? 0 : Convert.ToInt32(row[colName]));

                        sb.Append("<td class='center td-heading' style='text-align:center;'><b>" + total + "</b></td>");
                    }
                }

                sb.Append("</tr>");

                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("</div>");
            }
            else
            {
                sb.Append("<div>No Data found</div>");
            }

            return Content(sb.ToString());
        }



   public ActionResult GetDataForDiaryDashboardStatusDepartmentPendency(string financialyear, string mType, int FileType = 0, int RefType = 0,string MenuName = "Reference", string selectedType = "", string MCType = "", string MCClass = "")
   {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();

            int ShowType = (CurrentSession.isDeptApex == "1") ? 2 : 1;

            dt = DiaryDashboard.GetDataForDiaryDashboardStatDepartmentPendency(
                CurrentSession.OfficeId,
                Convert.ToString(CurrentSession.UserID),
                CurrentSession.DeptID,
                out dtstatus,
                financialyear,
                Convert.ToInt16(mType),
                FileType,
                RefType,
                ShowType,
                selectedType,
                MCType,
                MCClass
            );

            if (dt != null && dt.Rows.Count > 0)
            {
                List<string> hiddenCols = new List<string> { "OfficeId", "DeptId" };

                sb.Append("<div style='max-height:500px; overflow-y:auto;'>");
                sb.Append("<table id='StatsDepartment' class='table table-bordered table-striped' style='font-size:12px;width:100%;'>");

                // ================= HEADER =================
                sb.Append("<thead><tr>");

                int firstCols = 2;
                int visibleIndex = 0;

                foreach (DataColumn col in dt.Columns)
                {
                    if (hiddenCols.Contains(col.ColumnName)) continue;

                    string style = "position:sticky; top:0; z-index:2; text-align:center;";
                    style += (visibleIndex < firstCols) ? "background:#f8f9fb;" : "background:#f2f7ff;";

                    string[] parts = col.ColumnName.Split('_');
                    string firstPart = parts.Length > 0 ? parts[0].Trim() : "";
                    string secondPart = parts.Length > 1 ? parts[1].Trim() : "";

                    sb.Append("<th style='" + style + "'>");
                    sb.Append(firstPart);

                    if (!string.IsNullOrEmpty(secondPart))
                        sb.Append("<br/><small>" + secondPart + "</small>");

                    sb.Append("</th>");

                    visibleIndex++;
                }

                sb.Append("</tr></thead>");
                sb.Append("<tbody>");

                // ================= BODY =================
                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");

                    string deptId = row["DeptId"].ToString();
                    string officeId = row["OfficeId"].ToString();

                    int visibleIndexBody = 0;

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colName = dt.Columns[i].ColumnName;
                        if (hiddenCols.Contains(colName)) continue;

                        string[] parts = colName.Split('_');
                        string firstPart = parts.Length > 0 ? parts[0].Trim() : "";
                        string secondPart = parts.Length > 1 ? parts[1].Trim() : "";

                        string value = row[i] == DBNull.Value ? "0" : row[i].ToString();

                        int actionCode = 0;
                        string level = "";

                        // ===== MAPPING =====

                        // ADC
                        if (firstPart.Contains("Files Received") && secondPart == "ADC")
                        { actionCode = 17; level = "ADC"; }
                        else if (firstPart.Contains("Total Proposals") && secondPart == "ADC")
                        { actionCode = 333; level = "ADC"; }
                        else if (firstPart.Contains("Not Dealt") && secondPart == "ADC")
                        { actionCode = 13; level = "ADC"; }
                        else if (firstPart.Contains("Observation") && secondPart == "ADC")
                        { actionCode = 26; level = "ADC"; }
                        else if (firstPart.Contains("Comment") && secondPart == "ADC")
                        { actionCode = 31; level = "ADC"; }
                        else if (firstPart.Contains("Approved") && secondPart == "ADC")
                        { actionCode = 32; level = "ADC"; }
                        else if (firstPart.Contains("Rejected") && secondPart == "ADC")
                        { actionCode = 33; level = "ADC"; }

                        // LG

                        if (firstPart.Contains("Files Received") && secondPart == "LG")
                        { actionCode = 17; level = "LG"; }
                        else if (firstPart.Contains("Total Proposals") && secondPart == "LG")
                        { actionCode = 333; level = "LG"; }
                        else if (firstPart.Contains("Not Dealt") && secondPart == "LG" && MCType == "1")
                        {
                            actionCode = 13;   
                            level = "LG";
                        }
                        else if (firstPart.Contains("Not Dealt") && secondPart == "LG" && MCType == "2")
                        {
                            actionCode = 31;
                            level = "LG";
                        }
                        else if (firstPart.Contains("Branch") && secondPart == "LG")
                        { actionCode = 14; level = "LG"; }
                        else if (firstPart.Contains("Approval") && secondPart == "LG")
                        { actionCode = 7; level = "LG"; }
                        else if (firstPart.Contains("Rejected") && secondPart == "LG")
                        { actionCode = 4; level = "LG"; }
                        else if (firstPart.Contains("Observation") && secondPart == "LG")
                        { actionCode = 5; level = "LG"; }

                        // ===== CLICKABLE =====
                        bool isClickableColumn = false;

                        if (firstPart.Contains("Total Proposals"))
                            isClickableColumn = true;
                        else if (visibleIndexBody >= firstCols)
                            isClickableColumn = true;

                        // ===== RENDER =====
                        if (int.TryParse(value, out int num) && isClickableColumn && num > 0)
                        {
                            sb.Append("<td style='text-align:center;'>");
                            sb.Append("<span class='count-btn1 text-primary fw-bold' " +
                                      "style='cursor:pointer;text-decoration:underline;' " +
                                      "data-dept='" + deptId + "' " +
                                      "data-office='" + officeId + "' " +
                                      "data-action='" + actionCode + "' " +
                                      "data-level='" +  level + "'" +
                                      "data-mctype='" + MCType + "'>" +
                                       value + "</span>");
                            sb.Append("</td>");
                        }
                        else
                        {
                            sb.Append("<td style='text-align:center;'>" + value + "</td>");
                        }

                        visibleIndexBody++;
                    }

                    sb.Append("</tr>");
                }

                // ================= TOTAL =================
                sb.Append("<tr style='font-weight:bold;background:#f1f1f1;'>");

                bool first = true;

                foreach (DataColumn col in dt.Columns)
                {
                    if (hiddenCols.Contains(col.ColumnName)) continue;

                    if (first)
                    {
                        sb.Append("<td>Total</td>");
                        first = false;
                        continue;
                    }

                    int total = dt.AsEnumerable()
                        .Where(r => r[col] != DBNull.Value && int.TryParse(r[col].ToString(), out _))
                        .Sum(r => Convert.ToInt32(r[col]));

                    sb.Append("<td style='text-align:center;'>" + total + "</td>");
                }

                sb.Append("</tr>");

                sb.Append("</tbody></table></div>");
            }
            else
            {
                sb.Append("<table class='table'><tr><td>No Data Found</td></tr></table>");
            }

            return Content(sb.ToString());
        }


  public ActionResult GetDataForDiaryDashboardStatusDepartmentPendencyADC(string financialyear, string mType, int FileType = 0, int RefType = 0, string MenuName = "Reference", string selectedType = "", string MCType = "", string MCClass = "")
  {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();

            int ShowType = (CurrentSession.isDeptApex == "1") ? 2 : 1;

            dt = DiaryDashboard.GetDataForDiaryDashboardStatDepartmentPendency(
                "",
                Convert.ToString(CurrentSession.UserID),
                "UDD0001",
                out dtstatus,
                financialyear,
                Convert.ToInt16(mType),
                FileType,
                RefType,
                ShowType,
                selectedType,
                MCType,
                MCClass
            );

            if (dt != null && dt.Rows.Count > 0)
            {
                List<string> hiddenCols = new List<string> { "OfficeId", "DeptId" };

                sb.Append("<div style='max-height:500px; overflow-y:auto;'>");
                sb.Append("<table id='StatsDepartment' class='table table-bordered table-striped' style='font-size:12px;width:100%;'>");

                // ================= HEADER =================
                sb.Append("<thead><tr>");

                int firstCols = 2;
                int visibleIndex = 0;

                foreach (DataColumn col in dt.Columns)
                {
                    if (hiddenCols.Contains(col.ColumnName)) continue;

                    string style = "position:sticky; top:0; z-index:2; text-align:center;";
                    style += (visibleIndex < firstCols) ? "background:#f8f9fb;" : "background:#f2f7ff;";

                    string[] parts = col.ColumnName.Split('_');
                    string firstPart = parts.Length > 0 ? parts[0].Trim() : "";
                    string secondPart = parts.Length > 1 ? parts[1].Trim() : "";

                    sb.Append("<th style='" + style + "'>");
                    sb.Append(firstPart);

                    if (!string.IsNullOrEmpty(secondPart))
                        sb.Append("<br/><small>" + secondPart + "</small>");

                    sb.Append("</th>");

                    visibleIndex++;
                }

                sb.Append("</tr></thead>");
                sb.Append("<tbody>");

                // ================= BODY =================
                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");

                    string deptId = row["DeptId"].ToString();
                    string officeId = row["OfficeId"].ToString();

                    int visibleIndexBody = 0;

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string colName = dt.Columns[i].ColumnName;
                        if (hiddenCols.Contains(colName)) continue;

                        string[] parts = colName.Split('_');
                        string firstPart = parts.Length > 0 ? parts[0].Trim() : "";
                        string secondPart = parts.Length > 1 ? parts[1].Trim() : "";

                        string value = row[i] == DBNull.Value ? "0" : row[i].ToString();

                        int actionCode = 0;
                        string level = "";

                        // ===== MAPPING =====

                        // ADC
                        if (firstPart.Contains("Files Received") && secondPart == "ADC")
                        { actionCode = 17; level = "ADC"; }
                        else if (firstPart.Contains("Total Proposals") && secondPart == "ADC")
                        { actionCode = 333; level = "ADC"; }
                        else if (firstPart.Contains("Not Dealt") && secondPart == "ADC")
                        { actionCode = 13; level = "ADC"; }
                        else if (firstPart.Contains("Observation") && secondPart == "ADC")
                        { actionCode = 26; level = "ADC"; }
                        else if (firstPart.Contains("Comment") && secondPart == "ADC")
                        { actionCode = 31; level = "ADC"; }
                        else if (firstPart.Contains("Approved") && secondPart == "ADC")
                        { actionCode = 32; level = "ADC"; }
                        else if (firstPart.Contains("Rejected") && secondPart == "ADC")
                        { actionCode = 33; level = "ADC"; }

                        // LG

                        if (firstPart.Contains("Files Received") && secondPart == "LG")
                        { actionCode = 17; level = "LG"; }
                        else if (firstPart.Contains("Total Proposals") && secondPart == "LG")
                        { actionCode = 333; level = "LG"; }
                        else if (firstPart.Contains("Not Dealt") && secondPart == "LG")
                        { actionCode = 13; level = "LG"; }
                        else if (firstPart.Contains("Branch") && secondPart == "LG")
                        { actionCode = 14; level = "LG"; }
                        else if (firstPart.Contains("Approval") && secondPart == "LG")
                        { actionCode = 7; level = "LG"; }
                        else if (firstPart.Contains("Rejected") && secondPart == "LG")
                        { actionCode = 4; level = "LG"; }
                        else if (firstPart.Contains("Observation") && secondPart == "LG")
                        { actionCode = 5; level = "LG"; }

                        // ===== CLICKABLE =====
                        bool isClickableColumn = false;

                        if (firstPart.Contains("Total Proposals"))
                            isClickableColumn = true;
                        else if (visibleIndexBody >= firstCols)
                            isClickableColumn = true;


                        // ===== RENDER =====
                        //if (int.TryParse(value, out int num) && isClickableColumn && num > 0)
                        //{
                        //    sb.Append("<td style='text-align:center;'>");
                        //    sb.Append("<span class='count-btn1 text-primary fw-bold' " +
                        //              "style='cursor:pointer;text-decoration:underline;' " +
                        //              "data-dept='" + deptId + "' " +
                        //              "data-office='" + officeId + "' " +
                        //              "data-action='" + actionCode + "' " +
                        //              "data-level='" + level + "' " +
                        //              "data-mctype='" + MCType + "'>" +
                        //              value + "</span>");
                        //    sb.Append("</td>");
                        //}
                        //else
                        //{
                            sb.Append("<td style='text-align:center;'>" + value + "</td>");
                        //}

                        visibleIndexBody++;
                    }

                    sb.Append("</tr>");
                }

                // ================= TOTAL =================
                sb.Append("<tr style='font-weight:bold;background:#f1f1f1;'>");

                bool first = true;

                int colIndex = 0;
                //bool first = true;

                foreach (DataColumn col in dt.Columns)
                {
                    if (hiddenCols.Contains(col.ColumnName))
                    {
                        colIndex++;
                        continue;
                    }

                    if (first)
                    {
                        sb.Append("<td>Total</td>");
                        first = false;
                        colIndex++;
                        continue;
                    }

                    // 👉 Skip 4th column (index 3)
                    if (colIndex == 3)
                    {
                        sb.Append("<td></td>");
                        colIndex++;
                        continue;
                    }

                    int total = dt.AsEnumerable()
                        .Where(r => r[col] != DBNull.Value
                                    && int.TryParse(r[col].ToString(), out _))
                        .Sum(r => int.Parse(r[col].ToString()));

                    sb.Append("<td style='text-align:center;'>" + total + "</td>");

                    colIndex++;
                }

                sb.Append("</tr>");

                sb.Append("</tbody></table></div>");
            }
            else
            {
                sb.Append("<table class='table'><tr><td>No Data Found</td></tr></table>");
            }

            return Content(sb.ToString());
        }


        public ActionResult GetPendencyDetailList(string deptId, string officeId, int actionCode, string level, string MCType)
        {
            DataTable dt = new DataTable();

            try
            {
                int actualActionCode = actionCode;
                bool isTotal = false;

                if (actionCode == 333)
                {
                    actualActionCode = 0;
                    isTotal = true;
                }

                short mcTypeValue = 0;
                if (!string.IsNullOrEmpty(MCType))
                    short.TryParse(MCType, out mcTypeValue);

                SqlParameter[] param = {
                    new SqlParameter("@DeptId", string.IsNullOrEmpty(deptId) ? (object)DBNull.Value : deptId),
                    new SqlParameter("@OfficeId", string.IsNullOrEmpty(officeId) ? (object)DBNull.Value : officeId),
                    new SqlParameter("@ActionCode", actualActionCode),
                    new SqlParameter("@Level", string.IsNullOrEmpty(level) ? (object)DBNull.Value : level),
                    new SqlParameter("@IsTotal", isTotal),
                    new SqlParameter("@MCType", mcTypeValue)
                     };

                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    DataSet ds = SqlHelper.ExecuteDataset(
                        con,
                        CommandType.StoredProcedure,
                        "PendencyDetail_SP",
                        param
                    );

                    if (ds != null && ds.Tables.Count > 0)
                        dt = ds.Tables[0];
                }

                StringBuilder sb = new StringBuilder();

                sb.Append("<div style='max-height:500px; overflow:auto;'>");

                sb.Append("<table id='innerDepartment' ");
                sb.Append("class='table table-bordered table-striped table-hover' ");
                sb.Append("style='font-size:13px;width:100%;border-collapse:collapse;'>");

                // ================= THEAD =================
                sb.Append("<thead>");

                // 🔹 TITLE ROW
                sb.Append("<tr style='position:sticky; top:0; z-index:30; background:#9cceff;'>");
                sb.Append("<th colspan='" + (dt.Columns.Count + 1) + "' style='padding:10px;'>");

                sb.Append("<span style='font-weight:bold;font-size:15px;'>");
                if (isTotal)
                    sb.Append("Total Proposals");
                else
                    sb.Append(HttpUtility.HtmlEncode(level) + " - Action Code: " + actionCode);
                sb.Append("</span>");

                sb.Append("<button style='float:right;' type='button' ");
                sb.Append("onclick='transitionBetweenPendDivs()' ");
                sb.Append("class='btn btn-primary btn-sm'>Back</button>");

                sb.Append("</th>");
                sb.Append("</tr>");

                // 🔹 HEADER ROW (Checkbox FIRST)
                sb.Append("<tr style='position:sticky; top:42px; z-index:20; background:#f4f6f9;'>");

                // ✅ FIRST COLUMN - Select All
                sb.Append("<th style='text-align:center;width:40px;'>");
                sb.Append("<input type='checkbox' onclick='toggleAllCheckboxes(this)' />");
                sb.Append("</th>");

                foreach (DataColumn col in dt.Columns)
                {
                    sb.Append("<th style='text-align:center;border:1px solid #ddd;'>");
                    sb.Append(HttpUtility.HtmlEncode(col.ColumnName));
                    sb.Append("</th>");
                }

                sb.Append("</tr>");
                sb.Append("</thead>");

                // ================= TBODY =================
                sb.Append("<tbody>");

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");

                        // 🔍 Find RefId dynamically
                        string refIdValue = "";
                        string res = "";

                        foreach (DataColumn col in dt.Columns)
                        {
                            var colName = col.ColumnName.ToLower();

                            if (colName.Contains("refid"))
                            {
                                refIdValue = row[col] == DBNull.Value ? "" : row[col].ToString();
                            }

                            if (colName.Contains("resolution"))
                            {
                                res = row[col] == DBNull.Value ? "" : row[col].ToString();
                            }

                            // ✅ break only when both found
                            if (!string.IsNullOrEmpty(refIdValue) && !string.IsNullOrEmpty(res))
                            {
                                break;
                            }
                        }

                        // ✅ FIRST COLUMN - Checkbox
                        sb.Append("<td style='text-align:center;'>");
                        sb.Append("<input type='checkbox' class='chkRef' value='" + HttpUtility.HtmlEncode(refIdValue) + "' data-refid='" + HttpUtility.HtmlEncode(refIdValue) + "' data-resolution='" + HttpUtility.HtmlEncode(res) +"' />");
                        sb.Append("</td>");

                        // 🔹 Remaining columns
                        foreach (DataColumn col in dt.Columns)
                        {
                            string rawValue = row[col] == DBNull.Value ? "" : row[col].ToString();
                            string value = HttpUtility.HtmlEncode(rawValue);

                            if (col.ColumnName.ToLower().Contains("refid") && !string.IsNullOrEmpty(rawValue))
                            {
                                string safeValue = rawValue.Replace("'", "\\'");

                                sb.Append("<td style='text-align:center;'>");
                                sb.Append("<span style='cursor:pointer;text-decoration:underline;color:blue;' ");
                                sb.Append("onclick=\"openFileDetail('" + safeValue + "')\">");
                                sb.Append(value);
                                sb.Append("</span></td>");
                            }
                            else
                            {
                                sb.Append("<td style='text-align:center;'>");
                                sb.Append(value);
                                sb.Append("</td>");
                            }
                        }

                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append("<tr>");
                    sb.Append("<td colspan='" + (dt.Columns.Count + 1) + "' style='text-align:center;color:red;'>");
                    sb.Append("No Data Found</td>");
                    sb.Append("</tr>");
                }

                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("<button id='btnGenerateMail' style='float:left;' type='button' ");
                sb.Append("onclick='GetTemplate()' ");
                sb.Append("class='btn btn-primary btn-sm hide'>Generate Reminder</button>");

                sb.Append("</div>");

                return Content(sb.ToString());
            }
            catch (Exception)
            {
                return Content("<div style='color:red;'>Error loading data</div>");
            }
        }

        public ActionResult GetDashboardBranchReport()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                DataSet ds = SqlHelper.ExecuteDataset(
                    con,
                    CommandType.StoredProcedure,
                    "DashboardBranchReport"
                );

                if (ds != null && ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }

            StringBuilder sb = new StringBuilder();

            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("<table id='StatsDepartmentOffice' class='table table-bordered table-striped' style='width:100%;font-size:13px;'>");

                // 🔹 HEADER
                sb.Append("<thead style='position:sticky; top:0; background:#fff; z-index:10;'>");

                sb.Append("<tr>");
                sb.Append("<th rowspan='2'>Branches</th>");
                sb.Append("<th rowspan='2'>Files (Currently With)</th>");
                sb.Append("<th rowspan='2'>Total Resolutions</th>");
                sb.Append("<th colspan='6' style='text-align:center;background:#fdeeee;'>LG</th>");
                sb.Append("<th colspan='2' style='text-align:center;background:#e7f1ff;'>MC</th>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<th>Not Dealt</th>");
                sb.Append("<th>Branch Comments</th>");
                sb.Append("<th>Approved</th>");
                sb.Append("<th>Rejected</th>");
                sb.Append("<th>LG Observations</th>");
                sb.Append("<th>For Advice</th>");
                sb.Append("<th>Under Implement</th>");
                sb.Append("<th>Implemented</th>");
                sb.Append("</tr>");

                sb.Append("</thead>");

                // 🔹 BODY
                sb.Append("<tbody>");

                foreach (DataRow row in dt.Rows)
                {
                    string branchName = row["Branches"].ToString();
                    string officeId = row["OfficeId"].ToString();   // ✅ NEW

                    sb.Append("<tr>");

                    sb.Append("<td>" + HttpUtility.HtmlEncode(branchName) + "</td>");

                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName == "Branches" || col.ColumnName == "OfficeId") continue;

                        string value = row[col] == DBNull.Value ? "0" : row[col].ToString();

                        // 🔥 TYPE + ACTION MAPPING
                        string type = "";
                        string actionCode = "";

                        switch (col.ColumnName)
                        {
                            case "Files Currently With":
                                type = "FILE";
                                break;

                            case "Total Resolutions":
                                type = "PROPOSAL";
                                break;

                            case "Not Dealt":
                                type = "ACTION"; actionCode = "13"; break;
                                //--Branch Comments
                            case "Branch Comments":
                                type = "ACTION"; actionCode = "14"; break;

                            case "Approved":
                                type = "ACTION"; actionCode = "7"; break;

                            case "Rejected":
                                type = "ACTION"; actionCode = "4"; break;

                            case "LG Observations":
                                type = "ACTION"; actionCode = "5"; break;

                            case "Advice":
                            case "For Advice":
                                type = "ACTION"; actionCode = "15"; break;

                            case "Under Implement":
                                type = "ACTION"; actionCode = "21"; break;

                            case "Implemented":
                                type = "ACTION"; actionCode = "22"; break;
                        }

                        // 🔥 CLICKABLE
                        if (int.TryParse(value, out int val) && val > 0)
                        {
                            sb.Append("<td style='text-align:center;'>"
                           + "<span class='count-btn2' style='cursor:pointer;color:#007bff;font-weight:600;' "
                           + "data-branchid='" + HttpUtility.HtmlAttributeEncode(officeId) + "' "
                           + "data-type='" + type + "' "
                           + "data-action='" + actionCode + "'>"
                           + value + "</span></td>");
                        }
                        else
                        {
                            sb.Append("<td style='text-align:center;'>" + value + "</td>");
                        }
                    }

                    sb.Append("</tr>");
                }

                sb.Append("</tbody>");
                sb.Append("</table>");
            }
            else
            {
                sb.Append("<div>No Data Found</div>");
            }

            return Content(sb.ToString());
        }


        public ActionResult ResolutionSearch(
    string RefId = null,
    string MCID = null,
    string ResolutionNo = null,
    DateTime? MeetingDate = null)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DataSet ds = new DataSet();

                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "usp_GetDiaryFileDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@RefId",
                        string.IsNullOrWhiteSpace(RefId)
                            ? (object)DBNull.Value
                            : RefId);

                    cmd.Parameters.AddWithValue(
                        "@mcid",
                        string.IsNullOrWhiteSpace(MCID)
                            ? (object)DBNull.Value
                            : MCID);

                    cmd.Parameters.AddWithValue(
                        "@ResoltionNo",
                        string.IsNullOrWhiteSpace(ResolutionNo)
                            ? (object)DBNull.Value
                            : ResolutionNo);

                    cmd.Parameters.AddWithValue(
                        "@MeetingDate",
                        MeetingDate.HasValue
                            ? (object)MeetingDate.Value
                            : DBNull.Value);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }

                if (ds != null &&
                    ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    sb.Append("<div class='table-responsive sr-table-wrap'>");
                    sb.Append("<table id='StatsResSearch' class='table table-bordered table-hover sr-data-table align-middle' style='width:100%; border:1px solid #cbd5e1 !important; border-collapse:collapse !important;'>");

                    // =====================================================
                    // HEADER
                    // =====================================================
                    sb.Append("<thead>");
                    sb.Append("<tr class='sr-table-header-row' style='background-color:#1e3a8a !important;'>");

                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName.Equals("FileType", StringComparison.OrdinalIgnoreCase))
                            continue;

                        string colName = col.ColumnName;
                        string thClass = "sr-th";
                        string thStyle = "";
                        string iconHtml = "";

                        if (colName.Equals("MC Name", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:130px;";
                            thClass += " sr-th-mc";
                            iconHtml = "<i class='fa fa-university me-1' style='color:#93c5fd;'></i>";
                        }
                        else if (colName.Equals("Proceedings", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:160px;";
                            thClass += " sr-th-proc";
                            iconHtml = "<i class='fa fa-folder-open me-1' style='color:#93c5fd;'></i>";
                        }
                        else if (colName.Equals("Resolution Unique ID", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:150px;";
                            thClass += " sr-th-uid";
                            iconHtml = "<i class='fa fa-hashtag me-1' style='color:#93c5fd;'></i>";
                        }
                        else if (colName.Equals("Meeting Date", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:115px;white-space:nowrap;";
                            thClass += " sr-th-date";
                            iconHtml = "<i class='fa fa-calendar me-1' style='color:#93c5fd;'></i>";
                        }
                        else if (colName.Equals("Subject", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:320px;max-width:550px;";
                            thClass += " sr-th-subject";
                            iconHtml = "<i class='fa fa-file-text-o me-1' style='color:#93c5fd;'></i>";
                        }
                        else if (colName.Equals("Resolution No", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:90px;text-align:center;";
                            thClass += " sr-th-resno";
                            iconHtml = "<i class='fa fa-tag me-1' style='color:#93c5fd;'></i>";
                        }
                        else if (colName.Equals("Status", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:180px;";
                            thClass += " sr-th-status";
                            iconHtml = "<i class='fa fa-info-circle me-1' style='color:#93c5fd;'></i>";
                        }
                        else if (colName.Equals("Currently With", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:190px;";
                            thClass += " sr-th-with";
                            iconHtml = "<i class='fa fa-user me-1' style='color:#93c5fd;'></i>";
                        }
                        else if (colName.Equals("Approvals", StringComparison.OrdinalIgnoreCase) ||
                                 colName.Equals("Approval Orders", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:130px;";
                            thClass += " sr-th-approvals";
                            iconHtml = "<i class='fa fa-check-circle-o me-1' style='color:#93c5fd;'></i>";
                        }
                        else if (colName.Equals("Details", StringComparison.OrdinalIgnoreCase))
                        {
                            thStyle = "min-width:200px;";
                            thClass += " sr-th-details";
                        }
                        else
                        {
                            thStyle = "min-width:110px;";
                        }

                        sb.AppendFormat("<th class='{0}' style='{1} background-color:#1e3a8a !important; color:#ffffff !important; border:1px solid #334155 !important; text-align:center; vertical-align:middle; padding:9px 8px; font-weight:700;'>{2}{3}</th>", thClass, thStyle, iconHtml, HttpUtility.HtmlEncode(colName));
                    }

                    sb.Append("</tr>");
                    sb.Append("</thead>");

                    // =====================================================
                    // BODY
                    // =====================================================
                    sb.Append("<tbody>");

                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr class='sr-row'>");

                        foreach (DataColumn col in dt.Columns)
                        {
                            if (col.ColumnName.Equals("FileType", StringComparison.OrdinalIgnoreCase))
                                continue;

                            if (col.ColumnName.Equals("Proceedings", StringComparison.OrdinalIgnoreCase))
                            {
                                string fileNo = row[col] == DBNull.Value ? "" : row[col].ToString();
                                string fileType = (dt.Columns.Contains("FileType") && row["FileType"] != DBNull.Value) ? row["FileType"].ToString() : "";
                                string typeBadge = !string.IsNullOrWhiteSpace(fileType)
                                    ? string.Format("<span class='sr-pill-badge sr-badge-filetype'>{0}</span>", HttpUtility.HtmlEncode(fileType))
                                    : "";

                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-proc'>" +
                                    "<a href='javascript:void(0);' class='sr-table-link sr-link-proceedings' id='{0}' onclick=\"EditMlaDispatch(this,0,'Diary')\" title='Open Proceeding Details'>" +
                                    "<i class='fa fa-folder-open-o me-1 text-primary'></i><span class='sr-link-text'>{1}</span>{2}</a></td>",
                                    HttpUtility.HtmlEncode(fileNo),
                                    HttpUtility.HtmlEncode(fileNo),
                                    typeBadge);
                            }
                            else if (col.ColumnName.Equals("Resolution Unique ID", StringComparison.OrdinalIgnoreCase))
                            {
                                string referenceId = row[col] == DBNull.Value ? "" : row[col].ToString();
                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-uid'>" +
                                    "<a href='javascript:void(0);' class='sr-table-link sr-link-uid' id='{0}' onclick=\"EditMlaDispatch(this,0,'Dispatch')\" title='Open Resolution Dispatch Details'>" +
                                    "<i class='fa fa-hashtag me-1 text-success'></i><span class='sr-link-text'>{1}</span></a></td>",
                                    HttpUtility.HtmlEncode(referenceId),
                                    HttpUtility.HtmlEncode(referenceId));
                            }
                            else if (col.ColumnName.Equals("MC Name", StringComparison.OrdinalIgnoreCase))
                            {
                                string mcName = row[col] == DBNull.Value ? "" : row[col].ToString();
                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-mc'><div class='sr-mc-cell'><i class='fa fa-university text-primary me-1'></i><span class='fw-semibold'>{0}</span></div></td>",
                                    HttpUtility.HtmlEncode(mcName));
                            }
                            else if (col.ColumnName.Equals("Meeting Date", StringComparison.OrdinalIgnoreCase))
                            {
                                string meetingDate = row[col] != DBNull.Value
                                    ? Convert.ToDateTime(row[col]).ToString("dd-MMM-yyyy")
                                    : "";
                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-date'><div class='sr-date-pill'><i class='fa fa-calendar-o me-1 text-muted'></i><span>{0}</span></div></td>",
                                    HttpUtility.HtmlEncode(meetingDate));
                            }
                            else if (col.ColumnName.Equals("Resolution No", StringComparison.OrdinalIgnoreCase))
                            {
                                string resNo = row[col] == DBNull.Value ? "" : row[col].ToString();
                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-resno text-center'><span class='sr-res-badge'>{0}</span></td>",
                                    HttpUtility.HtmlEncode(resNo));
                            }
                            else if (col.ColumnName.Equals("Status", StringComparison.OrdinalIgnoreCase))
                            {
                                string status = row[col] == DBNull.Value ? "" : row[col].ToString();
                                status = status.Replace("</br>", "<br />").Replace("<br>", "<br />");
                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-status'><div class='sr-status-cell'>{0}</div></td>",
                                    status);
                            }
                            else if (col.ColumnName.Equals("Currently With", StringComparison.OrdinalIgnoreCase))
                            {
                                string withUser = row[col] == DBNull.Value ? "" : row[col].ToString();
                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-with'><div class='sr-with-cell'><i class='fa fa-user-circle-o text-secondary me-1'></i><span>{0}</span></div></td>",
                                    HttpUtility.HtmlEncode(withUser));
                            }
                            else if (col.ColumnName.Equals("Approval Orders", StringComparison.OrdinalIgnoreCase) ||
                                     col.ColumnName.Equals("Approvals", StringComparison.OrdinalIgnoreCase))
                            {
                                string approvals = row[col] == DBNull.Value ? "" : row[col].ToString();
                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-approvals'>{0}</td>",
                                    approvals);
                            }
                            else if (col.ColumnName.Equals("Details", StringComparison.OrdinalIgnoreCase))
                            {
                                string details = row[col] == DBNull.Value ? "" : row[col].ToString();
                                details = details.Replace("</br>", "<br />").Replace("<br>", "<br />");
                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-details'><div class='sr-details-box'>{0}</div></td>",
                                    details);
                            }
                            else if (col.ColumnName.Equals("Subject", StringComparison.OrdinalIgnoreCase))
                            {
                                string subject = row[col] == DBNull.Value ? "" : row[col].ToString();
                                sb.AppendFormat(
                                    "<td class='sr-td sr-td-subject'><div class='sr-subject-box' title='{0}'>{0}</div></td>",
                                    HttpUtility.HtmlEncode(subject));
                            }
                            else
                            {
                                string value = row[col] == DBNull.Value ? "" : row[col].ToString();
                                sb.AppendFormat(
                                    "<td class='sr-td'>{0}</td>",
                                    HttpUtility.HtmlEncode(value));
                            }
                        }

                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                }
                else
                {
                    sb.Append(
                        "<div class='sr-no-records text-center py-5 my-3'>" +
                        "<div class='sr-no-records-icon mb-3'><i class='fa fa-search text-muted' style='font-size:38px;opacity:0.5;'></i></div>" +
                        "<h5 class='fw-semibold text-dark mb-1'>No Resolution Records Found</h5>" +
                        "<p class='text-muted small mb-0'>No matching resolution proceedings found for the selected parameters. Please adjust your filters and try again.</p>" +
                        "</div>");
                }
            }
            catch (Exception ex)
            {
                sb.Clear();

                sb.Append(
                    "<div class='alert alert-danger'>" +
                    HttpUtility.HtmlEncode(ex.Message) +
                    "</div>");
            }

            return Content(sb.ToString(), "text/html");
        }
        public ActionResult GetBranchDetailList(string branchId, string type, string actionCode)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("DashboardBranchDetail", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OfficeId", branchId);
                    cmd.Parameters.AddWithValue("@Type", type);

                    if (string.IsNullOrEmpty(actionCode))
                        cmd.Parameters.AddWithValue("@ActionCode", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ActionCode", Convert.ToInt32(actionCode));

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            StringBuilder sb = new StringBuilder();

            if (dt != null && dt.Rows.Count > 0)
            {
                sb.Append("<table id='detailTable' class='table table-bordered table-striped' style='width:100%;font-size:13px;'>");

                // 🔹 TITLE ROW
                sb.Append("<thead><tr style='position:sticky; top:0; z-index:30; background:#9cceff;'>");
                sb.Append("<th colspan='" + (dt.Columns.Count + 1) + "' style='padding:10px;'>");
                if (type == "FILE")
                {
                    sb.Append("<span style='font-weight:bold;font-size:15px;'>Files");
                }
                else
                {
                    sb.Append("<span style='font-weight:bold;font-size:15px;'>Resolution Details");
                }
            
                sb.Append("</span>");
                sb.Append("<button style='float:right;' type='button' ");
                sb.Append("onclick='transitionBetweenOfficeWiseDivs()' ");
                sb.Append("class='btn btn-primary btn-sm'>Back</button>");

                sb.Append("</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                foreach (DataColumn col in dt.Columns)
                {
                    sb.Append("<th>" + HttpUtility.HtmlEncode(col.ColumnName) + "</th>");
                }
                sb.Append("</tr></thead>");

                // 🔹 BODY
                sb.Append("<tbody>");
                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn col in dt.Columns)
                    {
                        string value = row[col] == DBNull.Value ? "" : row[col].ToString();
                        sb.Append("<td>" + HttpUtility.HtmlEncode(value) + "</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");

                sb.Append("</table>");
            }
            else
            {
                sb.Append("<div style='text-align:center;padding:20px;'>No Details Found</div>");
            }

            return Content(sb.ToString());
        }

        public ActionResult GetDataForDiaryDashboardList(string deptid, string FromDeptId, string officeid, string documentype, string DepartmentName, int FileType = 0, int RefType = 0, string MenuName = "")
        {
            string status = "Diary";
            string type = "";
            string userid = CurrentSession.UserID;
            LstDashboard model = new LstDashboard();
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();

            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);

            string deptd = CurrentSession.DeptID;
            //if (deptd != "LG00001")
            //{
            //    type = "2";
            //}
            //else
            //{
            //    type = "3";
            //}
            int ShowType = 0;
            if (CurrentSession.isDeptApex == "1") { ShowType = 2; } else { ShowType = 1; }
            var data1 = tPaperLaidV.GetMCStatDetailsList(deptid, FromDeptId, officeid, documentype, userid, type, FileType, RefType, ShowType);
            model._LstStatDetail = data1;
            if (documentype == "111")
            {
                sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
                sb.Append("<h6>" + DepartmentName + ">Meeting Files</h6><button style='float:right; margin-top:-32px' type='button' onclick='transitionBetweenDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</br>");

            }
            //sb.Append("<table id='test111' style='padding:10px;font-size: 12px;font-weight: bold;' class='display'>");
            sb.Append("<table id='test111' style='padding:10px;font-size: 12px;' class='table table-bordered'>");
            if (documentype == "111")
            {
                type = "3";
                dt = tPaperLaidV.GetDataForDiaryDashboardList(deptid, FromDeptId, officeid, documentype, userid, out dtstatus, type, FileType, RefType, ShowType);
                sb.Append("<thead>");
                if (CurrentSession.isDeptApex == "0")
                {
                    sb = sb.Append("<tr role='row' class='table-heading'>");
                    sb = sb.Append("<td style= 'text-align:center;' colspan= 8 class='' id='tdFrom'></td>");
                    sb = sb.Append("<td style= 'text-align:center;' colspan= 4 class='td-heading' id='tdreflabel'>Pending</td>");
                    sb = sb.Append("<td style= 'text-align:center;' colspan= 2 class='' id='tdreflabel'>Approved</td>");
                    sb = sb.Append("</tr>");
                }
                sb.Append("<tr role='row' class='table-heading'>");
                sb.Append("<th class='text-center td-heading'></th>");
                sb.Append("<th class='text-center td-heading'>Meeting Date/File ID</th>");
                sb.Append("<th class='text-center td-heading' style='width:25%'>Subject</th>");
                sb.Append("<th class='text-center td-heading' >Currently With</th>");
                sb.Append("<th class='text-center td-heading'>" + MenuName + "</th>");
                foreach (var item1 in useraction)
                {
                    sb.Append("<th class='text-center td-heading'>" + item1.DocumentTypeName + "</th>");
                }
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");
                int ct = 1;
                foreach (DataRow item in dt.Rows)
                {

                    string Mainref = Convert.ToString(item["FileReference"]);
                    var partfile = tPaperLaidV.GetPartfile(CurrentSession.UserID, Mainref);


                    sb.Append("<tr role='row' id='trtdRef" + item["FileReference"] + "'>");
                    //sb.Append("<td class='text-center td-heading'>" + ct + "</td>");
                    //sb.Append("<td class='text-center td-heading'>" + Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td class='text-center td-heading'><a class='green' id=" + item["FileReference"] + " onclick = EditMlaDispatch(this,0,'" + status + "')><i class='ace-icon fa fa-pencil bigger-150'></i> </a>  </td>");
                    sb.Append("<td class='text-center td-heading'>" +
                    Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") +
                    "<br><a id='" + item["FileReference"] + "' onclick=\"EditMlaDispatch(this, 0, '" + status + "')\">" +
                     item["FileReference"] + "</a></br>");

                    if (partfile.Count > 0)
                    {
                        sb.Append("</br>");
                        sb.Append("<h6 class='PartFile'>(Part File)</h6>");
                        foreach (var part in partfile)
                        {
                            sb.Append("<div>");

                            if (part.Islink)
                            {
                                sb.Append("<a class='blue' id=" + part.ReferenceId + " onclick = EditMlaDispatchOutbox(this,0,'" + status + "')> " + part.ReferenceId + "</a>");
                            }
                            else
                            {
                                // Append plain text if Islink is false
                                sb.Append($"<span>{part.ReferenceId}</span>");
                            }

                            sb.Append("</div>");
                        }
                    }

                    sb.Append("</td>");



                    sb.Append("<td class='td-heading'>" + item["MettingName"] + "</td>");
                    sb.Append("<td class='td-heading'>" + item["CurrentlyWith"] + "</td>");
                    //sb.Append("<td class='text-center td-heading'><a id=" + item["FileReference"] + " onclick = EditMlaDispatch(this,0,'" + status + "')>" + item["Resolution"] + "</a></td>");
                    //foreach (var item1 in useraction)
                    //{
                    //    sb.Append("<td class='text-center center td-heading'><a id=" + item["FileReference"] + " onclick = EditMlaDispatch(this,0,'" + status + "')>" + item[item1.DocumentTypeName] + "</a></td>");
                    //}
                    sb.Append("<td class='text-center td-heading'><a id=" + item["FileReference"] + " documenttypeid=" + 222 + " deptid= " + deptid + " officeid =" + officeid + " Department= '" + DepartmentName + "'  FileType= " + FileType + " RefType =" + RefType + " MenuName=" + MenuName + "' FromDeptId='" + FromDeptId + "'  onclick ='GetDataForDiaryDashboardForFile(this)'>" + item["Resolution"] + "</a></td>");


                    foreach (var item1 in useraction)
                    {
                        sb.Append("<td class='text-center center td-heading'><a id=" + item["FileReference"] + "  documenttypeid=" + item1.DocmentType + "  deptid= " + deptid + " officeid =" + officeid + " Department= '" + DepartmentName + "'  FileType= " + FileType + " RefType =" + RefType + " MenuName=" + MenuName + "' FromDeptId='" + FromDeptId + "' onclick ='GetDataForDiaryDashboardForFile(this)'>" + item[item1.DocumentTypeName] + "</a></td>");
                    }
                    sb.Append("</tr>");
                    ct++;
                }
            }
            else
            {
                if (documentype == "222")
                {

                    sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
                    sb.Append("<h6>" + DepartmentName + "> Total " + MenuName + " </h6><button style='float:right; margin-top:-32px' type='button' onclick='transitionBetweenDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</br>");
                }

                else if (documentype == "333")
                {
                    string documentTypeName = "Pending";
                    sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
                    sb.Append("<h6>" + DepartmentName + " > " + documentTypeName + "</h6><button style='float:right; margin-top:-32px' type='button' onclick='transitionBetweenDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</br>");

                }

                else
                {
                    string documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(documentype)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();
                    sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
                    sb.Append("<h6>" + DepartmentName + " > " + documentTypeName + "</h6><button style='float:right; margin-top:-32px' type='button' onclick='transitionBetweenDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</br>");
                }


                sb.Append("<thead>");
                sb.Append("<tr class='table-heading'>");

                sb.Append("<th class='td-heading'></th>");
                sb.Append("<th class='text-center td-heading'></th>");
                sb.Append("<th class='text-center td-heading'>Meeting Date</th>");
                sb.Append("<th class='text-center td-heading'>No.</th>");
                //sb.Append("<th class='text-center td-heading'>From</th>");
                //sb.Append("<th class='text-center td-heading'>To</th>");
                sb.Append("<th style='' class='text-center td-heading'>Subject</th>");
                //sb.Append("<th class='text-center td-heading'>Type</th>");
                sb.Append("<th class='text-center td-heading'>Status</th>");
                sb.Append("<th class='text-center td-heading'>Ref.ID</th>");
                sb.Append("<th class='text-center td-heading'>Pendency</th>");

                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");

                foreach (var mdl1 in model._LstStatDetail)
                {

                    sb.Append("<tr  id='trtd" + mdl1.Resolutionno + "' role='row'>");

                    sb.Append("<td class='td-data'><input type='checkbox' id=" + mdl1.ReferenceId + " onclick='return checkInAgendaD(this)' />    </td>");

                    sb.Append("<td class='td-data'><a class='green' id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-pencil bigger-150'></i></a></td>");
                    sb.Append("<td class='td-data'>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd/MM/yyyy") + "</td>");

                    //sb.Append("<td>" + mdl1.Meeting + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.Resolutionno + "</td>");
                    //sb.Append("<td class='td-data'>" + mdl1.FromDepartment +"-"+ mdl1.FromOffice + "</td>");
                    //sb.Append("<td class='td-data'>" + mdl1.ToDepartment + "-" + mdl1.ToOffice + "</td>");
                    sb.Append("<td>" + mdl1.Meeting + "</td>");
                    //sb.Append("<td class='td-data'>" + mdl1.DocType + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.ActionName + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.ReferenceId + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.Pendency + "</td>");


                    sb.Append("</tr>");
                }

            }

            sb.Append("</tbody>");
            sb.Append("</table>");
            return Content(sb.ToString());

        }


        public ActionResult GetDataForDiaryDashboardListDepartment(string deptid, string FromDeptId, string officeid, string documentype, string DepartmentName, int FileType = 0, int RefType = 0, string MenuName = "", string DashType = "", string financialyear = "",string Destinationdept="")
        {
            string status = "Diary";
            string type = "";
            string userid = CurrentSession.UserID;
            LstDashboard model = new LstDashboard();
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();

            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);

            string deptd = CurrentSession.DeptID;
            //if (deptd != "LG00001")
            //{
            //    type = "2";
            //}
            //else
            //{
            //    type = "3";
            //}
            int ShowType = 0;
            if (CurrentSession.isDeptApex == "1") { ShowType = 2; } else { ShowType = 1; }
            var data1 = tPaperLaidV.GetMCStatDetailsListDepartment(deptid, FromDeptId, officeid, documentype, userid, type, FileType, RefType, ShowType, DashType, financialyear, Destinationdept);
            model._LstStatDetail = data1;
            if (documentype == "111")
            {
                sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
                sb.Append("<h6>" + DepartmentName + ">Meeting Files</h6><button style='float:right; margin-top:-32px' type='button' onclick='DepartmentDashBoardDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</br>");

            }
            //sb.Append("<table id='test111' style='padding:10px;font-size: 12px;font-weight: bold;' class='display'>");
            sb.Append("<table id='innerDepartment' style='padding:10px;font-size: 12px;' class='table table-bordered'>");
            if (documentype == "111")
            {
                type = "3";
                dt = tPaperLaidV.GetDataForDiaryDashboardListDepartment(deptid, FromDeptId, officeid, documentype, userid, out dtstatus, type, FileType, RefType, ShowType, DashType, financialyear, Destinationdept);
                sb.Append("<thead>");
                if (CurrentSession.isDeptApex == "0")
                {
                    sb = sb.Append("<tr role='row' class='table-heading'>");
                    sb = sb.Append("<td style= 'text-align:center;' colspan= 9 class='' id='tdFrom'></td>");
                    sb = sb.Append("<td style= 'text-align:center;' colspan= 4 class='td-heading' id='tdreflabel'>Pending</td>");
                    sb = sb.Append("<td style= 'text-align:center;' colspan= 3 class='' id='tdreflabel'>Approved</td>");
                    sb = sb.Append("</tr>");
                }
                sb.Append("<tr role='row' class='table-heading'>");
                sb.Append("<th class='text-center td-heading'></th>");
                sb.Append("<th class='text-center td-heading'>Meeting Date/File ID</th>");
                sb.Append("<th class='text-center td-heading' style='width:25%'>Subject</th>");
                sb.Append("<th class='text-center td-heading' >Currently With</th>");
                sb.Append("<th class='text-center td-heading'>" + MenuName + "</th>");
                foreach (var item1 in useraction)
                {
                    sb.Append("<th class='text-center td-heading'>" + item1.DocumentTypeName + "</th>");
                }
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");
                int ct = 1;
                foreach (DataRow item in dt.Rows)
                {

                    string Mainref = Convert.ToString(item["FileReference"]);
                    var partfile = tPaperLaidV.GetPartfile(CurrentSession.UserID, Mainref);


                    sb.Append("<tr role='row' id='trtdRef" + item["FileReference"] + "'>");
                    //sb.Append("<td class='text-center td-heading'>" + ct + "</td>");
                    //sb.Append("<td class='text-center td-heading'>" + Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td class='text-center td-heading'><a class='green' id=" + item["FileReference"] + " onclick = EditMlaDispatch(this,0,'" + status + "')><i class='ace-icon fa fa-pencil bigger-150'></i> </a>  </td>");
                    sb.Append("<td class='text-center td-heading'>" +
                    Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") +
                    "<br><a id='" + item["FileReference"] + "' onclick=\"EditMlaDispatch(this, 0, '" + status + "')\">" +
                     item["FileReference"] + "</a></br>");

                    if (partfile.Count > 0)
                    {
                        sb.Append("</br>");
                        sb.Append("<h6 class='PartFile'>(Part File)</h6>");
                        foreach (var part in partfile)
                        {
                            sb.Append("<div>");

                            if (part.Islink)
                            {
                                sb.Append("<a class='blue' id=" + part.ReferenceId + " onclick = EditMlaDispatchOutbox(this,0,'" + status + "')> " + part.ReferenceId + "</a>");
                            }
                            else
                            {
                                // Append plain text if Islink is false
                                sb.Append($"<span>{part.ReferenceId}</span>");
                            }

                            sb.Append("</div>");
                        }
                    }

                    sb.Append("</td>");



                    sb.Append("<td class='td-heading'>" + item["MettingName"] + "</td>");
                    sb.Append("<td class='td-heading'>" + item["CurrentlyWith"] + "</td>");
                    //sb.Append("<td class='text-center td-heading'><a id=" + item["FileReference"] + " onclick = EditMlaDispatch(this,0,'" + status + "')>" + item["Resolution"] + "</a></td>");
                    //foreach (var item1 in useraction)
                    //{
                    //    sb.Append("<td class='text-center center td-heading'><a id=" + item["FileReference"] + " onclick = EditMlaDispatch(this,0,'" + status + "')>" + item[item1.DocumentTypeName] + "</a></td>");
                    //}
                    sb.Append("<td class='text-center td-heading'><a id=" + item["FileReference"] + " documenttypeid=" + 222 + " deptid= " + deptid + " officeid =" + officeid + " Department= '" + DepartmentName + "'  FileType= " + FileType + " RefType =" + RefType + " MenuName=" + MenuName + "' FromDeptId='" + FromDeptId + "'  onclick ='GetDataForDiaryDashboardForFileDepartment(this)'>" + item["Resolution"] + "</a></td>");


                    foreach (var item1 in useraction)
                    {
                        sb.Append("<td class='text-center center td-heading'><a id=" + item["FileReference"] + "  documenttypeid=" + item1.DocmentType + "  deptid= " + deptid + " officeid =" + officeid + " Department= '" + DepartmentName + "'  FileType= " + FileType + " RefType =" + RefType + " MenuName=" + MenuName + "' FromDeptId='" + FromDeptId + "' onclick ='GetDataForDiaryDashboardForFileDepartment(this)'>" + item[item1.DocumentTypeName] + "</a></td>");
                    }
                    sb.Append("</tr>");
                    ct++;
                }
            }
            else
            {
                if (documentype == "222")
                {

                    sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
                    sb.Append("<h6>" + DepartmentName + ">Total" + MenuName + " </h6><button style='float:right; margin-top:-32px' type='button' onclick='DepartmentDashBoardDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</br>");
                }

                else if (documentype == "333")
                {
                    string documentTypeName = "Pending";
                    sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
                    sb.Append("<h6>" + DepartmentName + " > " + documentTypeName + "</h6><button style='float:right; margin-top:-32px' type='button' onclick='DepartmentDashBoardDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</br>");

                }

                else
                {
                    string documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(documentype)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();
                    sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
                    sb.Append("<h6>" + DepartmentName + " > " + documentTypeName + "</h6><button style='float:right; margin-top:-32px' type='button' onclick='DepartmentDashBoardDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</br>");
                }


                sb.Append("<thead>");
                sb.Append("<tr class='table-heading'>");

                sb.Append("<th class='td-heading'></th>");
                sb.Append("<th class='text-center td-heading'></th>");
                sb.Append("<th class='text-center td-heading'>Meeting Date</th>");
                sb.Append("<th class='text-center td-heading'>No.</th>");
                //sb.Append("<th class='text-center td-heading'>From</th>");
                //sb.Append("<th class='text-center td-heading'>To</th>");
                sb.Append("<th style='' class='text-center td-heading'>Subject</th>");
                //sb.Append("<th class='text-center td-heading'>Type</th>");
                sb.Append("<th class='text-center td-heading'>Status</th>");
                sb.Append("<th class='text-center td-heading'>Ref.ID</th>");
                sb.Append("<th class='text-center td-heading'>Pendency</th>");

                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");

                foreach (var mdl1 in model._LstStatDetail)
                {

                    sb.Append("<tr  id='trtd" + mdl1.Resolutionno + "' role='row'>");

                    sb.Append("<td class='td-data'><input type='checkbox' id=" + mdl1.ReferenceId + " onclick='return checkInAgendaD(this)' />    </td>");

                    sb.Append("<td class='td-data'><a class='green' id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-pencil bigger-150'></i></a></td>");
                    sb.Append("<td class='td-data'>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd/MM/yyyy") + "</td>");

                    //sb.Append("<td>" + mdl1.Meeting + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.Resolutionno + "</td>");
                    //sb.Append("<td class='td-data'>" + mdl1.FromDepartment +"-"+ mdl1.FromOffice + "</td>");
                    //sb.Append("<td class='td-data'>" + mdl1.ToDepartment + "-" + mdl1.ToOffice + "</td>");
                    sb.Append("<td>" + mdl1.Meeting + "</td>");
                    //sb.Append("<td class='td-data'>" + mdl1.DocType + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.ActionName + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.ReferenceId + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.Pendency + "</td>");


                    sb.Append("</tr>");
                }

            }

            sb.Append("</tbody>");
            sb.Append("</table>");
            return Content(sb.ToString());

        }


        public ActionResult GetDataForDiaryDashboardListDepartmentOpen(string deptid, string FromDeptId, string officeid, string documentype, string DepartmentName, int FileType = 0, int RefType = 0, string MenuName = "", string DeshType = "", string FinancialYear = "", string DestinationDept="")
        {

            string fileFolderpath = BlobStorage.GetStorageAcessingPath();
            string status = "Dispatch";
            string type = "";
            string userid = CurrentSession.UserID;
            officeid = CurrentSession.OfficeId;
            LstDashboard model = new LstDashboard();
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
            string deptd = CurrentSession.DeptID;
            int ShowType = 0;
            if (CurrentSession.isDeptApex == "1") { ShowType = 2; } else { ShowType = 1; }
            var data1 = tPaperLaidV.GetMCStatDetailsListDepartment(deptid, FromDeptId, officeid, documentype, userid, type, FileType, RefType, ShowType, DeshType, FinancialYear, DestinationDept);
            model._LstStatDetail = data1;

            sb.Append("<div class='table-responsive table-cus' style='margin-top: 20px; height:100%;'>");
            sb.Append("<table id='innerDepartment' style='width: 100%; display: inline-table; border-width: 0 0 1px 0;' class='table table-bordered table-striped'>");

            if (documentype == "111")
            {
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='19' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > " + "Meeting Files" + "</h6>");
                sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</th>");
                sb.Append("</tr>");
                type = "3";
                dt = tPaperLaidV.GetDataForDiaryDashboardListDepartment(deptid, FromDeptId, officeid, documentype, userid, out dtstatus, type, FileType, RefType, ShowType, DeshType, FinancialYear, DestinationDept);
                sb.Append("<tr role='row' class='table-heading' style='border: 1px solid #D3D3D3;text-align: center'>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'></th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>#</th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>Meeting Date</th>");
                sb.Append("<th class='text-center td-heading' style='width:25%; border: 1px solid #D3D3D3;text-align: center'>Meeting Name</th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>File ID</th>");

                sb.Append("<th class='text-center td-heading' style='width:8%;border: 1px solid #D3D3D3;text-align: center'>Total Resolutions</th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>Currently With</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");
                int ct = 1;
                foreach (DataRow item in dt.Rows)
                {

                    string Mainref = Convert.ToString(item["FileReference"]);
                    var FileSettings = CMNirdesh.Models.FileSetting.GetSecureFileSettingLocation();
                    var partfile = tPaperLaidV.GetPartfile(CurrentSession.UserID, Mainref);
                    sb.Append("<tr role='row' style='border: 1px solid #D3D3D3;text-align: center' id='trtdRef" + item["FileReference"] + "'>");
                    sb.Append("<td class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'><a class='green' id=" + item["FileReference"] + " onclick = EditMlaDispatchOutbox(this,0,'" + status + "')><i class='ace-icon fa fa-eye bigger-150'></i> </a>  </td>");
                    sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" + ct + "</td>");
                    sb.Append("<td class='text-center td-heading'>" + Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" + item["MettingName"] + "</td>");
                    sb.Append("<td class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" +
                        "<a id='" + item["FileReference"] + "' onclick=\"EditMlaDispatchOutbox(this, 0, '" + status + "')\">" +
                     item["FileReference"] + "</a></br>");

                    if (CurrentSession.DeptID == "LG00001")
                    {
                        if (partfile.Count > 0)
                        {
                            sb.Append("</br>");
                            sb.Append("<h6 class='PartFile'>(Part File)</h6>");
                            foreach (var part in partfile)
                            {
                                sb.Append("<div>");

                                if (part.Islink)
                                {
                                    sb.Append("<a class='blue' id=" + part.ReferenceId + " onclick = EditMlaDispatchOutbox(this,0,'" + status + "')> " + part.ReferenceId + "</a>");
                                }
                                else
                                {
                                    sb.Append($"<span>{part.ReferenceId}</span>");
                                }

                                sb.Append("</div>");
                            }
                        }
                    }

                    sb.Append("</td>");
                    sb.Append("<td class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'><a id=" + item["FileReference"] + " documenttypeid=" + 222 + " deptid= " + deptid + " officeid ='" + officeid + "' Department= '" + DepartmentName + "'  FileType= " + FileType + " RefType =" + RefType + " MenuName=" + MenuName + "' FromDeptId='" + FromDeptId + "'  onclick ='GetDataForDiaryDashboardForFileDepartment(this)'>" + item["Resolution"] + "</a></td>");
                    //sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" + item["CurrentlyWith"] + "</td>");
                    sb.Append("<td style='border: 1px solid #D3D3D3;text-align: center;'>");

                    // 🔵 MAIN FILE LINK
                    sb.Append("<a class='blue' " +
                              "id='" + item["FileReference"] + "' " +
                              "data-type='main' " +
                              "style='cursor:pointer;color:#0a58ca;' " +
                              "onclick='openFileHistory(this); return false;'>" +
                              item["CurrentlyWith"] + "</a><br/>");


                    if (CurrentSession.DeptID == "LG00001")
                    {
                        if (partfile.Count > 0)
                        {
                            sb.Append("<br/>");
                            sb.Append("<h6 class='PartFile'>(Part File Currently With)</h6>");

                            foreach (var part in partfile)
                            {
                                sb.Append("<div>");

                                if (part.Islink)
                                {
                                    sb.Append("<a class='blue' " +
                                              "id='" + part.ReferenceId + "' " +
                                               "onclick='openFileHistory(this); return false;'>" +
                                              part.CreatedBy + "</a>");
                                }
                                else
                                {
                                    sb.Append("<a class='blue' " +
                                               "id='" + part.ReferenceId + "' " +
                                                "onclick='openFileHistory(this); return false;'>" +
                                               part.CreatedBy + "</a>");
                                }

                                sb.Append("</div>");
                            }
                        }
                    }

                    sb.Append("</td>");



                    sb.Append("</tr>");
                    ct++;
                }
            }
            else if (documentype == "444")
            {
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='19' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > " + "Meeting Files" + "</h6>");
                sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</th>");
                sb.Append("</tr>");
                type = "3";
                dt = tPaperLaidV.GetDataForDiaryDashboardListDepartment(deptid, FromDeptId, officeid, documentype, userid, out dtstatus, type, FileType, RefType, ShowType, DeshType, FinancialYear, DestinationDept);
                sb.Append("<tr role='row' class='table-heading' style='border: 1px solid #D3D3D3;text-align: center'>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'></th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>#</th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>Meeting Date</th>");
                sb.Append("<th class='text-center td-heading' style='width:25%; border: 1px solid #D3D3D3;text-align: center'>Meeting Name</th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>File ID</th>");

                sb.Append("<th class='text-center td-heading' style='width:8%;border: 1px solid #D3D3D3;text-align: center'>Total Resolutions</th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>Currently With</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");
                int ct = 1;
                foreach (DataRow item in dt.Rows)
                {

                    string Mainref = Convert.ToString(item["FileReference"]);
                    var FileSettings = CMNirdesh.Models.FileSetting.GetSecureFileSettingLocation();
                    var partfile = tPaperLaidV.GetPartfile(CurrentSession.UserID, Mainref);
                    sb.Append("<tr role='row' style='border: 1px solid #D3D3D3;text-align: center' id='trtdRef" + item["FileReference"] + "'>");
                    sb.Append("<td class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'><a class='green' id=" + item["FileReference"] + " onclick = EditMlaDispatchOutbox(this,0,'" + status + "')><i class='ace-icon fa fa-eye bigger-150'></i> </a>  </td>");
                    sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" + ct + "</td>");
                    sb.Append("<td class='text-center td-heading'>" + Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" + item["MettingName"] + "</td>");
                    sb.Append("<td class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" +
                        "<a id='" + item["FileReference"] + "' onclick=\"EditMlaDispatchOutbox(this, 0, '" + status + "')\">" +
                     item["FileReference"] + "</a></br>");

                    if (CurrentSession.DeptID == "LG00001")
                    {
                        if (partfile.Count > 0)
                        {
                            sb.Append("</br>");
                            sb.Append("<h6 class='PartFile'>(Part File)</h6>");
                            foreach (var part in partfile)
                            {
                                sb.Append("<div>");

                                if (part.Islink)
                                {
                                    sb.Append("<a class='blue' id=" + part.ReferenceId + " onclick = EditMlaDispatchOutbox(this,0,'" + status + "')> " + part.ReferenceId + "</a>");
                                }
                                else
                                {
                                    sb.Append($"<span>{part.ReferenceId}</span>");
                                }

                                sb.Append("</div>");
                            }
                        }
                    }

                    sb.Append("</td>");
                    sb.Append("<td class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'><a id=" + item["FileReference"] + " documenttypeid=" + 222 + " deptid= " + deptid + " officeid ='" + officeid + "' Department= '" + DepartmentName + "'  FileType= " + FileType + " RefType =" + RefType + " MenuName=" + MenuName + "' FromDeptId='" + FromDeptId + "'  onclick ='GetDataForDiaryDashboardForFileDepartment(this)'>" + item["Resolution"] + "</a></td>");
                    //sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" + item["CurrentlyWith"] + "</td>");
                    sb.Append("<td style='border: 1px solid #D3D3D3;text-align: center;'>");

                    // 🔵 MAIN FILE LINK
                    sb.Append("<a class='blue' " +
                              "id='" + item["FileReference"] + "' " +
                              "data-type='main' " +
                              "style='cursor:pointer;color:#0a58ca;' " +
                              "onclick='openFileHistory(this); return false;'>" +
                              item["CurrentlyWith"] + "</a><br/>");


                    if (CurrentSession.DeptID == "LG00001")
                    {
                        if (partfile.Count > 0)
                        {
                            sb.Append("<br/>");
                            sb.Append("<h6 class='PartFile'>(Part File Currently With)</h6>");

                            foreach (var part in partfile)
                            {
                                sb.Append("<div>");

                                if (part.Islink)
                                {
                                    sb.Append("<a class='blue' " +
                                              "id='" + part.ReferenceId + "' " +
                                               "onclick='openFileHistory(this); return false;'>" +
                                              part.CreatedBy + "</a>");
                                }
                                else
                                {
                                    sb.Append("<a class='blue' " +
                                               "id='" + part.ReferenceId + "' " +
                                                "onclick='openFileHistory(this); return false;'>" +
                                               part.CreatedBy + "</a>");
                                }

                                sb.Append("</div>");
                            }
                        }
                    }

                    sb.Append("</td>");



                    sb.Append("</tr>");
                    ct++;
                }
            }

            else
            {
                if (documentype == "222")
                {

                    sb.Append("<thead>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='8' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                    sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                    sb.Append("<h6>" + DepartmentName + ">Total Resolutions" + MenuName + " </h6>");
                    sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</th>");
                    sb.Append("</tr>");

                }
                else
                {
                    string documentTypeName = "Details";

                    sb.Append("<thead>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='8' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                    sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                    sb.Append("<h6>" + DepartmentName + " > " + documentTypeName + "</h6>");
                    sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</th>");
                    sb.Append("</tr>");

                }

                sb.Append("<tr class='table-heading'>");

                sb.Append("<th class='td-heading'></th>");
                //sb.Append("<th class='text-center td-heading'></th>");
                sb.Append("<th class='text-center td-heading'>Meeting Date</th>");
                sb.Append("<th class='text-center td-heading'>No.</th>");
                sb.Append("<th style='' class='text-center td-heading'>Subject</th>");
                sb.Append("<th class='text-center td-heading'>Status</th>");
                sb.Append("<th class='text-center td-heading'>Ref.ID</th>");
                //sb.Append("<th class='text-center td-heading'>Pendency</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");

                foreach (var mdl1 in model._LstStatDetail)
                {

                    sb.Append("<tr  id='trtd" + mdl1.Resolutionno + "' role='row'>");
                    //sb.Append("<td class='td-data'><input type='checkbox' id=" + mdl1.ReferenceId + " onclick='return checkInAgendaD(this)' />    </td>");
                    sb.Append("<td class='td-data'><a class='green' id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-eye bigger-150'></i></a></td>");
                    sb.Append("<td class='td-data'>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.Resolutionno + "</td>");
                    sb.Append("<td>" + mdl1.Meeting + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.ActionName + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.ReferenceId + "</td>");
                    //sb.Append("<td class='td-data'>" + mdl1.Pendency + "</td>");
                    sb.Append("</tr>");
                }

            }

            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");
            return Content(sb.ToString());

        }

        public ActionResult GetDataForDiaryDashboardListDepartmentPendency(string deptid, string FromDeptId, string officeid, string documentype, string DepartmentName, int FileType = 0, int RefType = 0, string MenuName = "", string DeshType = "", string FinancialYear = "", string DestinationDeptd="")
        {

            string fileFolderpath = BlobStorage.GetStorageAcessingPath();
            string status = "Dispatch";
            string type = "";
            string userid = CurrentSession.UserID;
            officeid = CurrentSession.OfficeId;
            LstDashboard model = new LstDashboard();
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
            string deptd = CurrentSession.DeptID;
            int ShowType = 0;
            if (CurrentSession.isDeptApex == "1") { ShowType = 2; } else { ShowType = 1; }
            var data1 = tPaperLaidV.GetMCStatDetailsListDepartmentPendency(deptid, FromDeptId, officeid, documentype, userid, type, FileType, RefType, ShowType, DeshType, FinancialYear);
            model._LstStatDetail = data1;

            sb.Append("<div class='table-responsive table-cus' style='margin-top: 20px; height:100%;'>");
            sb.Append("<table id='innerDepartment' style='width: 100%; display: inline-table; border-width: 0 0 1px 0;' class='table table-bordered'>");

            if (documentype == "111")
            {
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='19' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > " + "Meeting Files" + "</h6>");
                sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</th>");
                sb.Append("</tr>");
                type = "3";
                dt = tPaperLaidV.GetDataForDiaryDashboardListDepartment(deptid, FromDeptId, officeid, documentype, userid, out dtstatus, type, FileType, RefType, ShowType, DeshType, FinancialYear, DestinationDeptd);
                sb.Append("<tr role='row' class='table-heading' style='border: 1px solid #D3D3D3;text-align: center'>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'></th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>#</th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>Meeting Date</th>");
                sb.Append("<th class='text-center td-heading' style='width:25%; border: 1px solid #D3D3D3;text-align: center'>Meeting Name</th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>File ID</th>");

                sb.Append("<th class='text-center td-heading' style='width:8%;border: 1px solid #D3D3D3;text-align: center'>Total Resolutions</th>");
                sb.Append("<th class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>Currently With</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");
                int ct = 1;
                foreach (DataRow item in dt.Rows)
                {

                    string Mainref = Convert.ToString(item["FileReference"]);
                    var FileSettings = CMNirdesh.Models.FileSetting.GetSecureFileSettingLocation();
                    var partfile = tPaperLaidV.GetPartfile(CurrentSession.UserID, Mainref);
                    sb.Append("<tr role='row' style='border: 1px solid #D3D3D3;text-align: center' id='trtdRef" + item["FileReference"] + "'>");
                    sb.Append("<td class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'><a class='green' id=" + item["FileReference"] + " onclick = EditMlaDispatchOutbox(this,0,'" + status + "')><i class='ace-icon fa fa-eye bigger-150'></i> </a>  </td>");
                    sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" + ct + "</td>");
                    sb.Append("<td class='text-center td-heading'>" + Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" + item["MettingName"] + "</td>");
                    sb.Append("<td class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" +
                        "<a id='" + item["FileReference"] + "' onclick=\"EditMlaDispatchOutbox(this, 0, '" + status + "')\">" +
                     item["FileReference"] + "</a></br>");

                    if (CurrentSession.DeptID == "LG00001")
                    {
                        if (partfile.Count > 0)
                        {
                            sb.Append("</br>");
                            sb.Append("<h6 class='PartFile'>(Part File)</h6>");
                            foreach (var part in partfile)
                            {
                                sb.Append("<div>");

                                if (part.Islink)
                                {
                                    sb.Append("<a class='blue' id=" + part.ReferenceId + " onclick = EditMlaDispatchOutbox(this,0,'" + status + "')> " + part.ReferenceId + "</a>");
                                }
                                else
                                {
                                    sb.Append($"<span>{part.ReferenceId}</span>");
                                }

                                sb.Append("</div>");
                            }
                        }
                    }

                    sb.Append("</td>");
                    sb.Append("<td class='text-center td-heading' style='border: 1px solid #D3D3D3;text-align: center'><a id=" + item["FileReference"] + " documenttypeid=" + 222 + " deptid= " + deptid + " officeid ='" + officeid + "' Department= '" + DepartmentName + "'  FileType= " + FileType + " RefType =" + RefType + " MenuName=" + MenuName + "' FromDeptId='" + FromDeptId + "'  onclick ='GetDataForDiaryDashboardForFileDepartment(this)'>" + item["Resolution"] + "</a></td>");
                    //sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;text-align: center'>" + item["CurrentlyWith"] + "</td>");
                    sb.Append("<td id=" + item["FileReference"] + " class='td-heading' onclick ='openFileHistory(this)' style='border: 1px solid #D3D3D3;text-align: center ;cursor:pointer;color:#0a58ca;'>" + item["CurrentlyWith"] + "</td>");
                    sb.Append("</tr>");
                    ct++;
                }
            }
            else
            {
                if (documentype == "222")
                {

                    sb.Append("<thead>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='8' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                    sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                    sb.Append("<h6>" + DepartmentName + ">Total Resolutions" + MenuName + " </h6>");
                    sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</th>");
                    sb.Append("</tr>");

                }

                else if (documentype == "333")
                {

                    sb.Append("<thead>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='8' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                    sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                    sb.Append("<h6>" + DepartmentName + ">Total Resolutions" + MenuName + " </h6>");
                    sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</th>");
                    sb.Append("</tr>");

                }
                else
                {
                    string documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(documentype)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();

                    sb.Append("<thead>");
                    sb.Append("<tr>");
                    sb.Append("<th colspan='8' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                    sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                    sb.Append("<h6>" + DepartmentName + " > " + documentTypeName + "</h6>");
                    sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                    sb.Append("</div>");
                    sb.Append("</th>");
                    sb.Append("</tr>");

                }

                sb.Append("<tr class='table-heading'>");

                sb.Append("<th class='td-heading'></th>");
                sb.Append("<th class='text-center td-heading'></th>");
                sb.Append("<th class='text-center td-heading'>Meeting Date</th>");
                sb.Append("<th class='text-center td-heading'>No.</th>");
                sb.Append("<th style='' class='text-center td-heading'>Subject</th>");
                sb.Append("<th class='text-center td-heading'>Status</th>");
                sb.Append("<th class='text-center td-heading'>Ref.ID</th>");
                sb.Append("<th class='text-center td-heading'>Pendency</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");

                foreach (var mdl1 in model._LstStatDetail)
                {

                    sb.Append("<tr  id='trtd" + mdl1.Resolutionno + "' role='row'>");
                    sb.Append("<td class='td-data'><input type='checkbox' id=" + mdl1.ReferenceId + " onclick='return checkInAgendaD(this)' />    </td>");
                    sb.Append("<td class='td-data'><a class='green' id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-pencil bigger-150'></i></a></td>");
                    sb.Append("<td class='td-data'>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.Resolutionno + "</td>");
                    sb.Append("<td>" + mdl1.Meeting + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.ActionName + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.ReferenceId + "</td>");
                    sb.Append("<td class='td-data'>" + mdl1.Pendency + "</td>");
                    sb.Append("</tr>");
                }

            }

            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");
            return Content(sb.ToString());

        }

        //public ActionResult GetDataForDiaryDashboardListDepartmentOpen(string deptid, string FromDeptId, string officeid, string documentype, string DepartmentName, int FileType = 0, int RefType = 0, string MenuName = "")
        //{
        //    string status = "Diary";
        //    string type = "";
        //    string userid = CurrentSession.UserID;
        //    LstDashboard model = new LstDashboard();
        //    StringBuilder sb = new StringBuilder();
        //    DataTable dtstatus;
        //    DataTable dt = new DataTable();

        //    var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);

        //    string deptd = CurrentSession.DeptID;
        //    //if (deptd != "LG00001")
        //    //{
        //    //    type = "2";
        //    //}
        //    //else
        //    //{
        //    //    type = "3";
        //    //}
        //    int ShowType = 0;
        //    if (CurrentSession.isDeptApex == "1") { ShowType = 2; } else { ShowType = 1; }
        //    var data1 = tPaperLaidV.GetMCStatDetailsListDepartment(deptid, FromDeptId, officeid, documentype, userid, type, FileType, RefType, ShowType);
        //    model._LstStatDetail = data1;
        //    if (documentype == "111")
        //    {
        //        sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
        //        sb.Append("<h6>" + DepartmentName + ">Meeting Files</h6><button style='float:right; margin-top:-32px' type='button' onclick='FileDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
        //        sb.Append("</div>");
        //        sb.Append("</br>");

        //    }
        //    //sb.Append("<table id='test111' style='padding:10px;font-size: 12px;font-weight: bold;' class='display'>");
        //    sb.Append("<table id='innerDepartment' style='padding:10px;font-size: 12px;' class='table table-bordered'>");
        //    if (documentype == "111")
        //    {
        //        type = "3";
        //        dt = tPaperLaidV.GetDataForDiaryDashboardListDepartment(deptid, FromDeptId, officeid, documentype, userid, out dtstatus, type, FileType, RefType, ShowType);
        //        sb.Append("<thead>");
        //        if (CurrentSession.isDeptApex == "0")
        //        {
        //            sb = sb.Append("<tr role='row' class='table-heading'>");
        //            sb = sb.Append("<td style= 'text-align:center;' colspan= 9 class='' id='tdFrom'></td>");
        //            sb = sb.Append("<td style= 'text-align:center;' colspan= 4 class='td-heading' id='tdreflabel'>Pending</td>");
        //            sb = sb.Append("<td style= 'text-align:center;' colspan= 3 class='' id='tdreflabel'>Approved</td>");
        //            sb = sb.Append("</tr>");
        //        }
        //        sb.Append("<tr role='row' class='table-heading'>");
        //        sb.Append("<th class='text-center td-heading'></th>");
        //        sb.Append("<th class='text-center td-heading'>Meeting Date/File ID</th>");
        //        sb.Append("<th class='text-center td-heading' style='width:25%'>Subject</th>");
        //        sb.Append("<th class='text-center td-heading' >Currently With</th>");
        //        sb.Append("<th class='text-center td-heading'>" + MenuName + "</th>");
        //        foreach (var item1 in useraction)
        //        {
        //            sb.Append("<th class='text-center td-heading'>" + item1.DocumentTypeName + "</th>");
        //        }
        //        sb.Append("</tr>");
        //        sb.Append("</thead>");
        //        sb.Append("<tbody>");
        //        int ct = 1;
        //        foreach (DataRow item in dt.Rows)
        //        {

        //            string Mainref = Convert.ToString(item["FileReference"]);
        //            var partfile = tPaperLaidV.GetPartfile(CurrentSession.UserID, Mainref);


        //            sb.Append("<tr role='row' id='trtdRef" + item["FileReference"] + "'>");
        //            //sb.Append("<td class='text-center td-heading'>" + ct + "</td>");
        //            //sb.Append("<td class='text-center td-heading'>" + Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") + "</td>");
        //            sb.Append("<td class='text-center td-heading'><a class='green' id=" + item["FileReference"] + " onclick = EditMlaDispatch(this,0,'" + status + "')><i class='ace-icon fa fa-pencil bigger-150'></i> </a>  </td>");
        //            sb.Append("<td class='text-center td-heading'>" +
        //            Convert.ToDateTime(item["MettingDate"]).ToString("dd/MM/yyyy") +
        //            "<br><a id='" + item["FileReference"] + "' onclick=\"EditMlaDispatch(this, 0, '" + status + "')\">" +
        //             item["FileReference"] + "</a></br>");

        //            if (partfile.Count > 0)
        //            {
        //                sb.Append("</br>");
        //                sb.Append("<h6 class='PartFile'>(Part File)</h6>");
        //                foreach (var part in partfile)
        //                {
        //                    sb.Append("<div>");

        //                    if (part.Islink)
        //                    {
        //                        sb.Append("<a class='blue' id=" + part.ReferenceId + " onclick = EditMlaDispatchOutbox(this,0,'" + status + "')> " + part.ReferenceId + "</a>");
        //                    }
        //                    else
        //                    {
        //                        // Append plain text if Islink is false
        //                        sb.Append($"<span>{part.ReferenceId}</span>");
        //                    }

        //                    sb.Append("</div>");
        //                }
        //            }

        //            sb.Append("</td>");



        //            sb.Append("<td class='td-heading'>" + item["MettingName"] + "</td>");
        //            sb.Append("<td class='td-heading'>" + item["CurrentlyWith"] + "</td>");
        //            //sb.Append("<td class='text-center td-heading'><a id=" + item["FileReference"] + " onclick = EditMlaDispatch(this,0,'" + status + "')>" + item["Resolution"] + "</a></td>");
        //            //foreach (var item1 in useraction)
        //            //{
        //            //    sb.Append("<td class='text-center center td-heading'><a id=" + item["FileReference"] + " onclick = EditMlaDispatch(this,0,'" + status + "')>" + item[item1.DocumentTypeName] + "</a></td>");
        //            //}
        //            sb.Append("<td class='text-center td-heading'><a id=" + item["FileReference"] + " documenttypeid=" + 222 + " deptid= " + deptid + " officeid =" + officeid + " Department= '" + DepartmentName + "'  FileType= " + FileType + " RefType =" + RefType + " MenuName=" + MenuName + "' FromDeptId='" + FromDeptId + "'  onclick ='GetDataForDiaryDashboardForFileDepartment(this)'>" + item["Resolution"] + "</a></td>");


        //            foreach (var item1 in useraction)
        //            {
        //                sb.Append("<td class='text-center center td-heading'><a id=" + item["FileReference"] + "  documenttypeid=" + item1.DocmentType + "  deptid= " + deptid + " officeid =" + officeid + " Department= '" + DepartmentName + "'  FileType= " + FileType + " RefType =" + RefType + " MenuName=" + MenuName + "' FromDeptId='" + FromDeptId + "' onclick ='GetDataForDiaryDashboardForFileDepartment(this)'>" + item[item1.DocumentTypeName] + "</a></td>");
        //            }
        //            sb.Append("</tr>");
        //            ct++;
        //        }
        //    }
        //    else
        //    {
        //        if (documentype == "222")
        //        {

        //            sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
        //            sb.Append("<h6>" + DepartmentName + ">Total Agenda" + MenuName + " </h6><button style='float:right; margin-top:-32px' type='button' onclick='FileDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
        //            sb.Append("</div>");
        //            sb.Append("</br>");
        //        }

        //        else if (documentype == "333")
        //        {
        //            string documentTypeName = "Pending";
        //            sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
        //            sb.Append("<h6>" + DepartmentName + " > " + documentTypeName + "</h6><button style='float:right; margin-top:-32px' type='button' onclick='FileDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
        //            sb.Append("</div>");
        //            sb.Append("</br>");

        //        }

        //        else
        //        {
        //            string documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(documentype)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();
        //            sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
        //            sb.Append("<h6>" + DepartmentName + " > " + documentTypeName + "</h6><button style='float:right; margin-top:-32px' type='button' onclick='FileDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
        //            sb.Append("</div>");
        //            sb.Append("</br>");
        //        }


        //        sb.Append("<thead>");
        //        sb.Append("<tr class='table-heading'>");

        //        sb.Append("<th class='td-heading'></th>");
        //        sb.Append("<th class='text-center td-heading'></th>");
        //        sb.Append("<th class='text-center td-heading'>Meeting Date</th>");
        //        sb.Append("<th class='text-center td-heading'>No.</th>");
        //        //sb.Append("<th class='text-center td-heading'>From</th>");
        //        //sb.Append("<th class='text-center td-heading'>To</th>");
        //        sb.Append("<th style='' class='text-center td-heading'>Subject</th>");
        //        //sb.Append("<th class='text-center td-heading'>Type</th>");
        //        sb.Append("<th class='text-center td-heading'>Status</th>");
        //        sb.Append("<th class='text-center td-heading'>Ref.ID</th>");
        //        sb.Append("<th class='text-center td-heading'>Pendency</th>");

        //        sb.Append("</tr>");
        //        sb.Append("</thead>");
        //        sb.Append("<tbody>");

        //        foreach (var mdl1 in model._LstStatDetail)
        //        {

        //            sb.Append("<tr  id='trtd" + mdl1.Resolutionno + "' role='row'>");

        //            sb.Append("<td class='td-data'><input type='checkbox' id=" + mdl1.ReferenceId + " onclick='return checkInAgendaD(this)' />    </td>");

        //            sb.Append("<td class='td-data'><a class='green' id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-pencil bigger-150'></i></a></td>");
        //            sb.Append("<td class='td-data'>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd/MM/yyyy") + "</td>");

        //            //sb.Append("<td>" + mdl1.Meeting + "</td>");
        //            sb.Append("<td class='td-data'>" + mdl1.Resolutionno + "</td>");
        //            //sb.Append("<td class='td-data'>" + mdl1.FromDepartment +"-"+ mdl1.FromOffice + "</td>");
        //            //sb.Append("<td class='td-data'>" + mdl1.ToDepartment + "-" + mdl1.ToOffice + "</td>");
        //            sb.Append("<td>" + mdl1.Meeting + "</td>");
        //            //sb.Append("<td class='td-data'>" + mdl1.DocType + "</td>");
        //            sb.Append("<td class='td-data'>" + mdl1.ActionName + "</td>");
        //            sb.Append("<td class='td-data'>" + mdl1.ReferenceId + "</td>");
        //            sb.Append("<td class='td-data'>" + mdl1.Pendency + "</td>");


        //            sb.Append("</tr>");
        //        }

        //    }

        //    sb.Append("</tbody>");
        //    sb.Append("</table>");
        //    return Content(sb.ToString());

        //}




        //public ActionResult GetDataForDiaryDashboardForFile(string FileReference, string documentype)
        //{
        //    string status = "Diary";
        //    string userid = CurrentSession.UserID;
        //    LstDashboard model = new LstDashboard();
        //    StringBuilder sb = new StringBuilder();
        //    DataTable dt = new DataTable();
        //    var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
        //    string deptd = CurrentSession.DeptID;
        //    var data1 = tPaperLaidV.GetMCStatDetailsListForFile(FileReference, documentype);
        //    model._LstStatDetail = data1;
        //    if (documentype == "333")
        //    {
        //        string documentTypeName = "Pending";
        //        sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
        //        sb.Append("<h6>" + documentTypeName + "</h6><button style='float:right; margin-top:-32px' type='button' onclick='transitionBetweenDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
        //        sb.Append("</div>");
        //        sb.Append("</br>");

        //    }

        //    else
        //    {
        //        string documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(documentype)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();
        //        sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
        //        sb.Append("<h6>" + documentTypeName + "</h6><button style='float:right; margin-top:-32px' type='button' onclick='transitionBetweenDivs()' class='btn btn-primary mb-3' aria-label='Close'>Back</button>");
        //        sb.Append("</div>");
        //        sb.Append("</br>");
        //    }
        //    sb.Append("<table id='test111' style='padding:10px;font-size: 12px;' class='table table-bordered'>");

        //    sb.Append("<thead>");
        //    sb.Append("<tr class='table-heading'>");

        //    sb.Append("<th class='td-heading'></th>");
        //    sb.Append("<th class='text-center td-heading'></th>");
        //    sb.Append("<th class='text-center td-heading'>Meeting Date</th>");
        //    sb.Append("<th class='text-center td-heading'>No.</th>");
        //    sb.Append("<th style='' class='text-center td-heading'>Subject</th>");
        //    //sb.Append("<th class='text-center td-heading'>Type</th>");
        //    sb.Append("<th class='text-center td-heading'>Status</th>");



        //    sb.Append("</tr>");
        //    sb.Append("</thead>");
        //    sb.Append("<tbody>");

        //    foreach (var mdl1 in model._LstStatDetail)
        //    {

        //        sb.Append("<tr  id='trtd" + mdl1.Resolutionno + "' role='row'>");

        //        sb.Append("<td class='td-data'><input type='checkbox' id=" + mdl1.ReferenceId + " onclick='return checkInAgendaD(this)' />    </td>");

        //        sb.Append("<td class='td-data'><a class='green' id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-pencil bigger-150'></i></a></td>");
        //        sb.Append("<td class='td-data'>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd/MM/yyyy") + "</td>");

        //        //sb.Append("<td>" + mdl1.Meeting + "</td>");
        //        sb.Append("<td class='td-data'>" + mdl1.Resolutionno + "</td>");
        //        //sb.Append("<td class='td-data'>" + mdl1.FromDepartment +"-"+ mdl1.FromOffice + "</td>");
        //        //sb.Append("<td class='td-data'>" + mdl1.ToDepartment + "-" + mdl1.ToOffice + "</td>");
        //        sb.Append("<td>" + mdl1.Meeting + "</td>");
        //        //sb.Append("<td class='td-data'>" + mdl1.DocType + "</td>");
        //        sb.Append("<td class='td-data'>" + mdl1.ActionName + "</td>");



        //        sb.Append("</tr>");
        //    }



        //    sb.Append("</tbody>");
        //    sb.Append("</table>");
        //    return Content(sb.ToString());

        //}

        public ActionResult GetDataForDiaryDashboardForFile(string FileReference, string FromDeptId, string documentype, string deptid, string officeid, string DepartmentName, int FileType = 0, int RefType = 0, string MenuName = "", string DeshType = "")
        {
            string status = "Diary";
            string userid = CurrentSession.UserID;
            LstDashboard model = new LstDashboard();
            StringBuilder sb = new StringBuilder();
            DataTable dt = new DataTable();
            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
            string deptd = CurrentSession.DeptID;
            var data1 = tPaperLaidV.GetMCStatDetailsListForFile(FileReference, documentype);
            model._LstStatDetail = data1;
            sb.Append("<table id='FlipFileData' style='padding:10px;font-size: 12px;' class='table table-bordered'>");
            if (documentype == "222")
            {
                string documentTypeName = "Total Agenda";
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='8' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > " + documentTypeName + "</h6>");
                sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FlipFileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</th>");
                sb.Append("</tr>");


            }
            else
            {
                string documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(documentype)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='8' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > " + documentTypeName + "</h6>");
                sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FlipFileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</th>");
                sb.Append("</tr>");

            }


            //sb.Append("<thead>");
            sb.Append("<tr class='table-heading'>");

            sb.Append("<th class='td-heading'></th>");
            sb.Append("<th class='text-center td-heading'></th>");
            sb.Append("<th class='text-center td-heading'>Meeting Date</th>");
            sb.Append("<th class='text-center td-heading'>No.</th>");
            sb.Append("<th style='' class='text-center td-heading'>Subject</th>");
            //sb.Append("<th class='text-center td-heading'>Type</th>");
            sb.Append("<th class='text-center td-heading'>Status</th>");
            sb.Append("<th class='text-center td-heading'>Ref.ID</th>");
            sb.Append("<th class='text-center td-heading'>Pendency</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (var mdl1 in model._LstStatDetail)
            {

                sb.Append("<tr  id='trtd" + mdl1.Resolutionno + "' role='row'>");

                sb.Append("<td class='td-data'><input type='checkbox' id=" + mdl1.ReferenceId + " onclick='return checkInAgendaD(this)' />    </td>");

                sb.Append("<td class='td-data'><a class='green' id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-pencil bigger-150'></i></a></td>");
                sb.Append("<td class='td-data'>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd/MM/yyyy") + "</td>");

                //sb.Append("<td>" + mdl1.Meeting + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.Resolutionno + "</td>");
                //sb.Append("<td class='td-data'>" + mdl1.FromDepartment +"-"+ mdl1.FromOffice + "</td>");
                //sb.Append("<td class='td-data'>" + mdl1.ToDepartment + "-" + mdl1.ToOffice + "</td>");
                sb.Append("<td>" + mdl1.Meeting + "</td>");
                //sb.Append("<td class='td-data'>" + mdl1.DocType + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.ActionName + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.ReferenceId + "</td>");

                sb.Append("<td class='td-data'>" + mdl1.Pendency + "</td>");
                sb.Append("</tr>");
            }



            sb.Append("</tbody>");
            sb.Append("</table>");
            return Content(sb.ToString());

        }


        public ActionResult GetDataECabinetDashboardForFile(string FileReference, string FromDeptId, string documentype, string deptid, string officeid, string DepartmentName, int FileType = 0, int RefType = 0, string MenuName = "", string DeshType = "")
        {
            string status = "Diary";
            string userid = CurrentSession.UserID;
            LstDashboard model = new LstDashboard();
            StringBuilder sb = new StringBuilder();
            DataTable dt = new DataTable();
            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
            string deptd = CurrentSession.DeptID;
            var data1 = tPaperLaidV.GetMCStatDetailsListForFile(FileReference, documentype);
            model._LstStatDetail = data1;
            if (documentype == "222")
            {
                string documentTypeName = "Total Agenda";

                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='6' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > " + documentTypeName + "</h6>");
                sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FlipFileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</th>");
                sb.Append("</tr>");



            }
            else
            {
                string documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(documentype)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='6' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > " + documentTypeName + "</h6>");
                sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='FlipFileDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</th>");
                sb.Append("</tr>");
            }
            sb.Append("<table id='test111' style='padding:10px;font-size: 12px;' class='table table-bordered'>");

            //sb.Append("<thead>");
            sb.Append("<tr class='table-heading'>");

            sb.Append("<th class='td-heading'></th>");
            sb.Append("<th class='text-center td-heading'></th>");
            sb.Append("<th class='text-center td-heading'>Meeting Date</th>");
            sb.Append("<th class='text-center td-heading'>No.</th>");
            sb.Append("<th style='' class='text-center td-heading'>Subject</th>");
            //sb.Append("<th class='text-center td-heading'>Type</th>");
            sb.Append("<th class='text-center td-heading'>Status</th>");
            sb.Append("<th class='text-center td-heading'>Ref.ID</th>");
            sb.Append("<th class='text-center td-heading'>Pendency</th>");

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (var mdl1 in model._LstStatDetail)
            {

                sb.Append("<tr  id='trtd" + mdl1.Resolutionno + "' role='row'>");

                sb.Append("<td class='td-data'><input type='checkbox' id=" + mdl1.ReferenceId + " onclick='return checkInAgendaD(this)' />    </td>");

                sb.Append("<td class='td-data'><a class='green' id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-pencil bigger-150'></i></a></td>");
                sb.Append("<td class='td-data'>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd/MM/yyyy") + "</td>");

                //sb.Append("<td>" + mdl1.Meeting + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.Resolutionno + "</td>");
                //sb.Append("<td class='td-data'>" + mdl1.FromDepartment +"-"+ mdl1.FromOffice + "</td>");
                //sb.Append("<td class='td-data'>" + mdl1.ToDepartment + "-" + mdl1.ToOffice + "</td>");
                sb.Append("<td>" + mdl1.Meeting + "</td>");
                //sb.Append("<td class='td-data'>" + mdl1.DocType + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.ActionName + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.ReferenceId + "</td>");

                sb.Append("<td class='td-data'>" + mdl1.Pendency + "</td>");
                sb.Append("</tr>");
            }



            sb.Append("</tbody>");
            sb.Append("</table>");
            return Content(sb.ToString());

        }

        public ActionResult GetDataDashboardStatType(string financialYear, string DashType, string meetingType)
        {
            string DeptID = @CurrentSession.DeptID;
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            dt = DiaryDashboard.GetDataDashboardStatsType(CurrentSession.OfficeId, "", "10", Convert.ToString(CurrentSession.UserID), DeptID, out dtstatus, "Letter", financialYear, DashType, meetingType);

            sb.Append("<table id='MemoType' style='padding:10px;font-size: 12px; border-collapse: collapse;' class='table table-bordered'>");
            sb.Append("<thead>");
            if (dt.Rows.Count > 0)
            {
                // Table header rows with dynamically calculated titles
                sb.Append("<tr role='row' class='table-heading' style='border: 1px solid #D3D3D3;'>");
                sb.Append("<td style='text-align:center;background-color:white; border: 1px solid #D3D3D3;' colspan='5' id='tdFrom'></td>");
                //sb.Append("<td style='text-align:center; border: 1px solid #D3D3D3;' colspan='2' class='td-heading' id='tdreflabel'>Agenda Items</td>");
                sb.Append("<td style='text-align:center;background-color:white; border: 1px solid #D3D3D3;' colspan='3' class='td-heading' id='tdreflabel'></td>");
                sb.Append("<td style='text-align:center;background-color:white; border: 1px solid #D3D3D3;color:black' colspan='3' class='td-heading' id='tdreflabel'></td>");
                sb.Append("</tr>");


                sb.Append("<tr role='row' class='table-heading' style='border: 1px solid #D3D3D3;'>");
                if (dt.Rows.Count > 0)
                {
                    sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;' id='tdFrom'>Type</td>");
                    sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;'>Total</td>");

                    // Dynamically generate table headers based on dtstatus
                    for (int i = 1; i < dt.Columns.Count; i++)
                    {
                        DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                        if (result1.Length > 0)
                        {
                            sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;'>" + result1[0].ItemArray[1].ToString().Trim().Substring(0, result1[0].ItemArray[1].ToString().IndexOf("_")) + "</td>");
                        }
                    }
                    sb.Append("</tr>");
                }

                sb.Append("</thead>");

                sb.Append("<tbody>");
                sb.Append("<tr role='row' class='table-heading' style='border: 1px solid #D3D3D3;'>");
                sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;' id='tdFrom'></td>");
                sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;'></td>");

                // Dynamically generate the second row with the values from the dtstatus
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        sb.Append("<td class='td-heading' style='border: 1px solid #D3D3D3;'>" + result1[0].ItemArray[1].ToString().Trim().Substring(result1[0].ItemArray[1].ToString().LastIndexOf("_") + 1) + "</td>");
                    }
                }
                sb.Append("</tr>");
            }

            int j = 0, GTotal = 0;
            double Gni = 0, Guip = 0, Gna = 0;
            double Gper;

            // Row data bind
            foreach (DataRow drDyn in dt.Rows)
            {
                sb.Append("<tr id='trtd" + drDyn["deptId"] + "' style='border: 1px solid #D3D3D3;'>");
                sb.Append("<td class='center td-heading' style='border: 1px solid #D3D3D3;'><a onclick='GetDataStatTypeDetailOpen(this)' DeptId='" + drDyn["deptId"] + "' DepartmentName='" + drDyn["deptName"] + "' actionCode='" + null + "'>" + drDyn["deptName"].ToString().Trim() + "</a></td>");

                int Total = 0;
                string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total += Convert.ToInt32(cnt);
                    }
                }

                sb.Append("<td class='center td-heading total' style='border: 1px solid #D3D3D3;'><a onclick='GetDataStatTypeDetailOpen(this)' DeptId='" + drDyn["deptId"] + "' DepartmentName='" + drDyn["deptName"] + "' actionCode='" + null + "'>" + Total + "</a></td>");

                double ni = 0, uip = 0, na = 0;
                double per;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total += Convert.ToInt32(cnt);
                        sb.Append("<td class='center td-heading' style='border: 1px solid #D3D3D3;'><a onclick='GetDataStatTypeDetailOpen(this)' DeptId='" + drDyn["deptId"] + "' DepartmentName='" + drDyn["deptName"] + "' actionCode='" + docid + "'>" + cnt + "</a></td>");

                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Not Implemented")
                        {
                            ni = Convert.ToInt32(cnt);
                        }
                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "Under Implementation")
                        {
                            uip = Convert.ToInt32(cnt);
                        }

                        if (dt.Columns[i].ColumnName.ToString().Substring(0, dt.Columns[i].ColumnName.ToString().IndexOf("_")) == "NA")
                        {
                            na = Convert.ToInt32(cnt);
                        }
                    }
                }

                double sum = ni + uip;
                double den = Convert.ToDouble(Total) - na;
                per = sum / den;
                double prcentage = per * 100;

                if (SiteName == "CMNirdesh")
                {
                    sb.Append("<td class='center td-heading ' style='border: 1px solid #D3D3D3;'>" + Math.Round(prcentage, 2) + "</td>");
                }

                sb.Append("</tr>");
                j++;
            }

            if (dt.Rows.Count > 0)
            {
                sb.Append("<tr id='trtd0' style='border: 1px solid #D3D3D3;'>");
                sb.Append("<td class='center td-heading' style='border: 1px solid #D3D3D3;'>Total</td>");
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        GTotal += cnt;
                    }
                }

                sb.Append("<td class='center td-heading total' style='border: 1px solid #D3D3D3; text-align: center !important;'><span class='total-badge'>" + GTotal + "</span></td>");
                int Gcnt = 0;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        Gcnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        GTotal += Gcnt;

                        sb.Append("<td class='center td-heading total " + dt.Columns[i].ColumnName.ToString().Replace(" ", "_") + "' style='border: 1px solid #D3D3D3; text-align: center !important;'><span class='total-badge'>" + Gcnt + "</span></td>");
                    }
                }

                double Gsum = Gni + Guip;
                double Gden = Convert.ToDouble(GTotal) - Gna;

                Gper = Gsum / Gden;
                double Gprcentage = Gper * 100;

                if (SiteName == "CMNirdesh")
                {
                    sb.Append("<td class='center td-heading' style='border: 1px solid #D3D3D3;'>" + Math.Round(Gprcentage, 2) + "</td></tr>");
                }

            }
            else
            {

                sb.Append("<tr class='table-heading'><td class='center td-heading' style='border: 1px solid #D3D3D3;'><i class='fa fa-exclamation-circle' style='font-size: 24px; color: #E4A11B;'></i></td></tr>");
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            return Content(sb.ToString());
        }

        public ActionResult GetDataDashboardStatTypeDetail(int deptId, string ActionCode, string DepartmentName)
        {
            string status = "Diary";
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            StringBuilder sb = new StringBuilder();
            LstDashboard model = new LstDashboard();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            var UserId = CurrentSession.UserID.ToString();
            string DepartmentId = CurrentSession.DeptID.ToString();

            var data1 = DiaryDashboard.GetMCStatDetailsTypeListType(deptId, UserId, out dtstatus, DepartmentId, Convert.ToString(ActionCode), "", "");
            model._LstStatDetail = data1;
            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
            string documentTypeName = "Total";
            if (ActionCode != null && ActionCode != "")
            {
                documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(ActionCode)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();
            }

            sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
            sb.Append("<h6>" + DepartmentName + " >" + documentTypeName + "</h6><button style='float:right; margin-top:-22px' type='button' onclick='transitionBetweenDivsType()' class='btn-close' aria-label='Close'></button>");
            sb.Append("</div>");
            sb.Append("</br>");

            sb.Append("<table id='TypeDetails' style='padding:10px' class='table table-bordered'>");
            sb.Append("<thead>");
            sb.Append("<tr class='table-heading'>");

            sb.Append("<th class='td-heading'>Sr.No.</th>");
            sb.Append("<th class='text-center td-heading'>Reference No</th>");
            sb.Append("<th style='width:10%' class='text-center td-heading'>To Office</th>");
            sb.Append("<th style='width:10%' class='text-center td-heading'>Date</th>");
            //sb.Append("<th style='width:15%' class='text-center td-heading'>Subject</th>");
            sb.Append("<th class='text-center td-heading'>Number</th>");
            sb.Append("<th style='width:40%' class='text-center td-heading'>Subject</th>");
            sb.Append("<th class='text-center td-heading'>Status</th>");
            sb.Append("<th class='text-center td-heading'>View</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (var mdl1 in model._LstStatDetail)
            {
                sb.Append("<td class='text-center'>" + mdl1.SrNo + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.ReferenceId + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.CreatedBy + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.MeetingDate + "</td>");
                //sb.Append("<td>" + mdl1.Meeting + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.Resolutionno + "</td>");
                sb.Append("<td>" + mdl1.Subject + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.ActionName + "</td>");
                sb.Append("<td class='text-center'><a id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-eye'></i></a></td>");
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");

            return Content(sb.ToString());
        }

        public ActionResult GetDataDashboardStatTypeDetailOpen(int deptId, string ActionCode, string DepartmentName, string financialYear, string DashType)
        {
            string status = "Diary";
            StringBuilder sb = new StringBuilder();
            LstDashboard model = new LstDashboard();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            var UserId = CurrentSession.UserID.ToString();
            string DepartmentId = CurrentSession.DeptID.ToString();

            var data1 = DiaryDashboard.GetMCStatDetailsTypeListType(deptId, UserId, out dtstatus, DepartmentId, Convert.ToString(ActionCode), financialYear, DashType);
            model._LstStatDetail = data1;

            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
            string documentTypeName = "Total Agenda";
            if (!string.IsNullOrEmpty(ActionCode))
            {
                documentTypeName = useraction
                    .Where(f => f.DocmentType == Convert.ToInt16(ActionCode))
                    .Select(n => n.DocumentTypeName)
                    .FirstOrDefault()?.ToString() ?? "Total Agenda";
            }

            sb.Append("<table id='TypeDetails' style='border-collapse: collapse; width: 100%;' class='table table-bordered'>");

            // Add the custom header div inside the table header
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th colspan='6' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
            sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
            sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > " + documentTypeName + "</h6>");
            sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='MemoDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
            sb.Append("</div>");
            sb.Append("</th>");
            sb.Append("</tr>");

            // Add standard table column headers
            sb.Append("<tr class='table-heading'>");
            sb.Append("<th class='text-center td-heading'>Action</th>");
            sb.Append("<th class='text-center td-heading'>Memo No</th>");
            sb.Append("<th class='text-center td-heading'>From Dept</th>");
            sb.Append("<th class='text-center td-heading'>Date</th>");
            sb.Append("<th class='text-center td-heading'>Subject</th>");
            sb.Append("<th class='text-center td-heading'>Status</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");

            sb.Append("<tbody>");

            // Add table rows dynamically
            foreach (var mdl1 in model._LstStatDetail)
            {
                sb.Append("<tr style='border: 1px solid #D3D3D3;'>");
                sb.Append("<td class='text-center' style='border: 1px solid #D3D3D3;'>");
                sb.Append("<a id='" + mdl1.ReferenceId + "' onclick='EditMlaDispatch(this, 0, \"" + status + "\")'><i class='fa fa-pencil'></i></a>");
                sb.Append("</td>");
                sb.Append("<td class='text-center' style='border: 1px solid #D3D3D3;'>" + mdl1.ReferenceId + "</td>");
                sb.Append("<td class='text-center' style='border: 1px solid #D3D3D3;'>" + mdl1.CreatedBy + "</td>");
                sb.Append("<td class='text-center' style='border: 1px solid #D3D3D3;'>" + mdl1.MeetingDate + "</td>");
                sb.Append("<td style='border: 1px solid #D3D3D3;'>" + mdl1.Subject + "</td>");
                sb.Append("<td class='text-center' style='border: 1px solid #D3D3D3;'>" + mdl1.ActionName + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            return Content(sb.ToString(), "text/html");
        }

        public ActionResult GetDataDashboardStatTypeDetailLG(int deptId, string ActionCode, string DepartmentName, string DepartmentId)
        {
            string status = "Diary";
            string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
            StringBuilder sb = new StringBuilder();
            LstDashboard model = new LstDashboard();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            var UserId = CurrentSession.UserID.ToString();
            //string DepartmentId = CurrentSession.DeptID.ToString();

            var data1 = DiaryDashboard.GetMCStatDetailsTypeListType(deptId, UserId, out dtstatus, DepartmentId, Convert.ToString(ActionCode), "", "");
            model._LstStatDetail = data1;
            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
            string documentTypeName = "Total";
            if (ActionCode != null && ActionCode != "")
            {
                documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(ActionCode)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();
            }

            sb.Append("<div style='background: #9cceff;padding: 8px 15px;border-bottom: 5px solid #658fdb;'>");
            sb.Append("<h6>" + DepartmentName + " >" + documentTypeName + "</h6><button style='float:right; margin-top:-22px' type='button' onclick='transitionBetweenDivsType2()' class='btn-close' aria-label='Close'></button>");
            sb.Append("</div>");
            sb.Append("</br>");

            sb.Append("<table id='TypeDetails1' style='padding:10px' class='table table-bordered'>");
            sb.Append("<thead>");
            sb.Append("<tr class='table-heading'>");

            sb.Append("<th class='td-heading'>Sr.No.</th>");
            sb.Append("<th class='text-center td-heading'>Reference No</th>");
            sb.Append("<th style='width:10%' class='text-center td-heading'>To Office</th>");
            sb.Append("<th style='width:10%' class='text-center td-heading'>Meeting Date</th>");
            sb.Append("<th style='width:15%' class='text-center td-heading'>Meeting Name</th>");
            sb.Append("<th class='text-center td-heading'>Resolution Number</th>");
            sb.Append("<th style='width:40%' class='text-center td-heading'>Subject</th>");
            sb.Append("<th class='text-center td-heading'>Status</th>");
            sb.Append("<th class='text-center td-heading'>View</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (var mdl1 in model._LstStatDetail)
            {
                sb.Append("<td class='text-center'>" + mdl1.SrNo + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.ReferenceId + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.CreatedBy + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.MeetingDate + "</td>");
                sb.Append("<td>" + mdl1.Meeting + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.Resolutionno + "</td>");
                sb.Append("<td>" + mdl1.Subject + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.ActionName + "</td>");
                sb.Append("<td class='text-center'><a id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-eye'></i></a></td>");
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");


            return Content(sb.ToString());
        }

        public ActionResult ResolutionDetails(string ResolutionNo)
        {
            string ResolutionDetail = tPaperLaidV.ResolutionDetail(ResolutionNo);
            StringBuilder sb = new StringBuilder();
            sb.Append("<h5>Details Resolution No(" + ResolutionNo + ")</h4>");
            sb.Append("<p style='font-size: 15px;!important'>" + ResolutionDetail + "</p>");
            return Content(sb.ToString());
        }

        public ActionResult GetDataForDiaryDashboardListByReference(string ReferenceNo)
        {

            LstDashboard model = new LstDashboard();
            StringBuilder sb = new StringBuilder();
            var data1 = tPaperLaidV.GetMCStatDetailsByFile(ReferenceNo);
            model._LstStatDetail = data1;

            sb.Append("<table id='ReferenceNo' style='padding:10px' class='table styled-table'>");
            sb.Append("<thead>");
            sb.Append("<tr class='table-heading'>");
            sb.Append("<th class='td-heading'>Sr.No.</th>");
            sb.Append("<th class='text-center td-heading'>Meeting Date</th>");
            sb.Append("<th class='text-center td-heading'>Meeting Name</th>");
            sb.Append("<th class='text-center td-heading'>Resolution Number</th>");
            sb.Append("<th style='width:50%' class='text-center td-heading'>Subject</th>");
            sb.Append("<th class='text-center td-heading'>Status</th>");
            sb.Append("<th class='text-center td-heading'>View</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (var mdl1 in model._LstStatDetail)
            {
                sb.Append("<tr  id='trtdin" + mdl1.Resolutionno + "' role='row'>");
                sb.Append("<td class='text-center'>" + mdl1.SrNo + "</td>");
                sb.Append("<td class='text-center'>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd /MM/yyyy") + "</td>");
                sb.Append("<td>" + mdl1.Meeting + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.Resolutionno + "</td>");
                sb.Append("<td>" + mdl1.Subject + "</td>");
                sb.Append("<td class='text-center'>" + mdl1.ActionName + "</td>");
                sb.Append("<td class='text-center'><a data-target='DashboardStat' resolutionno= '" + mdl1.Resolutionno + "' Mainid='trtdin" + mdl1.Resolutionno + "' onclick ='GetDiaryDasdhboardDataChange2ndlevel(this)'><i class='fa fa-eye'></i></a></td>");
                sb.Append("</tr>");

            }

            sb.Append("</tbody>");
            sb.Append("</table>");
            return Content(sb.ToString());

        }

        //public ActionResult PDFPreviewReport1(string refid, string doctype)
        //{
        //    Session["userExist"] = "";
        //    Session["userAdded"] = "";


        //    string htmlString = GetResolutionpdfhtml(refid, doctype);

        //    string htmlpath = "Proceedings/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + refid.ToString() + "_MCReport.html";
        //    string folderpath = "~/SecureFileStructure/" + htmlpath;
        //    htmlpath = Server.MapPath(folderpath);

        //    string pdfpath = "Proceedings/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + refid.ToString() + "_MCReport.pdf";
        //    folderpath = "~/SecureFileStructure/" + pdfpath;
        //    string finalpdfpath = Server.MapPath(folderpath);
        //    var root = "~/SecureFileStructure/Proceedings";
        //    bool path = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
        //    if (!path)
        //    {
        //        System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
        //    }


        //    System.IO.File.WriteAllText(htmlpath, htmlString);
        //    Response.ContentType = "text/html";
        //    //  SavePDf3(refid.ToString(), pdfpath);
        //    byte[] htmlBytes = Encoding.UTF8.GetBytes(htmlString);




        //    //  PDFModel.ConvertChromePdfRenderer(htmlpath, finalpdfpath);

        //    var report = new Rotativa.UrlAsPdf("https://punjabassembly.nic.in/eNigamMC/SecureFileStructure/Proceedings/31142024111407_202400000238_MCReport.html")

        //    //  var report = new Rotativa.UrlAsPdf("https://punjabassembly.nic.in/eNigamMC/SecureFileStructure/" + htmlpath)
        //    {
        //        PageOrientation = Rotativa.Options.Orientation.Portrait,
        //        PageSize = Rotativa.Options.Size.A3,
        //        // PageMargins = new Margins(0, 0, 0, 0),
        //        PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
        //    };
        //    if (!System.IO.File.Exists(finalpdfpath))
        //    {
        //        var byteArray = report.BuildPdf(ControllerContext);
        //        var fileStream = new FileStream(finalpdfpath, FileMode.Create, FileAccess.Write);
        //        fileStream.Write(byteArray, 0, byteArray.Length);
        //        fileStream.Close();
        //    }
        //    return report;
        //}

        public ActionResult PDFPreviewReportSttausReport(string refid, string doctype)
        {
            string htmlString = GetResolutionpdfhtmlPreview(refid, doctype);


            //var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            //{
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageSize = Rotativa.Options.Size.A4,
            //    PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
            //};


            var MCName = CurrentSession.MCName;
            var MCHeader = "";
            if (MCName != "")
            {
                MCHeader = "Municipal Corporation " + MCName;
            }
            // var MCHeader = "Municipal Corporation " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"MC Punjab\" " +
                    "--footer-right \"PDF Preview Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };



            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{refid}_Proceeding.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            return File(pdfBytes, "application/pdf", "Proceeding.pdf");

        }













        //Rajeev
        //Preview List
        //public ActionResult PDFPreviewReportSttausReportt(string refid, string doctype)
        //{
        //    string htmlString = GetResolutionpdfhtmlPrevieww(refid, doctype);
        //    var MCName = CurrentSession.MCName;

        //    var MCHeader = "";
        //    if (MCName != "")
        //    {
        //        MCHeader = "Municipal Corporation " + MCName;
        //    }



        //    var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
        //    {
        //        PageOrientation = Rotativa.Options.Orientation.Landscape,
        //        PageSize = Rotativa.Options.Size.A4,
        //        PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),


        //        CustomSwitches =
        //            "--footer-left \"Page: [page] of [toPage]\" " +
        //            "--footer-center \"Digital MC House\" " +
        //            "--footer-right \"PDF Preview Generated on: [date]\" " +
        //            "--footer-font-size 10 --footer-spacing 5 " +
        //            "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
        //            "--header-font-size 12 --header-spacing 10 " +
        //            "--zoom 1.3 --no-outline"
        //    };



        //    byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);




        //    string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{refid}_Proceeding.pdf";


        //    //BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
        //    Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
        //    return File(pdfBytes, "application/pdf");



        //}



        public async Task<ActionResult> PDFPreviewReportSttausReportt(
            string refid,
            string doctype)
        {
            try
            {
                // 1️⃣ Generate HTML
                string htmlString = GetResolutionpdfhtmlPrevieww(refid, doctype);

                string MCName = CurrentSession.MCName;
                string MCHeader = string.IsNullOrEmpty(MCName)
                    ? ""
                    : "Municipal Corporation " + MCName;

                byte[] pdfBytes;

                // 2️⃣ Launch Chrome Headless
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
                using (var page = await browser.NewPageAsync())
                {
                    // 3️⃣ Load HTML
                    await page.SetContentAsync(htmlString, new NavigationOptions
                    {
                        WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                    });

                    // 4️⃣ PDF Options (Rotativa → Puppeteer mapping)
                    var pdfOptions = new PdfOptions
                    {
                        Landscape = true,
                        Format = PaperFormat.A4,
                        PrintBackground = true,
                        DisplayHeaderFooter = true,

                        HeaderTemplate = $@"
                <div style='width:100%; text-align:center;
                            font-size:12px; font-weight:600;'>
                    {MCHeader}
                </div>",

                        FooterTemplate = @"
                <div style='width:100%; font-size:10px;
                            padding:0 20px;
                            display:flex;
                            justify-content:space-between;'>
                    <span>Page <span class='pageNumber'></span> of <span class='totalPages'></span></span>
                    <span>Digital MC House</span>
                    <span>PDF Preview Generated on: <span class='date'></span></span>
                </div>",

                        MarginOptions = new MarginOptions
                        {
                            Top = "20mm",
                            Right = "10mm",
                            Bottom = "20mm",
                            Left = "10mm"
                        },
                        Scale = 1.3m
                    };

                    // 5️⃣ Generate PDF
                    pdfBytes = await page.PdfDataAsync(pdfOptions);
                }

                // 6️⃣ Optional Blob Upload
                string pdfFilename =
                    $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{refid}_Proceeding.pdf";

                // BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);

                // 7️⃣ Inline Preview
                Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding.pdf");
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, "Error in PDFPreviewReportSttausReportt Puppeteer");
                TempData["Message"] = ex.Message;
                return RedirectToAction("Index");
            }
        }




        private byte[] GeneratePdfFromHtml(string inputPath, string outputPath)
        {
            string wkhtmlPath = @"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"; // Path to wkhtmltopdf executable

            // Build the command arguments
            string arguments = $"--lowquality --disable-smart-shrinking --no-images --no-javascript --viewport-size 1280x1024 \"{inputPath}\" \"{outputPath}\"";

            // Configure the process
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = wkhtmlPath,
                    Arguments = arguments,
                    RedirectStandardOutput = false,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            try
            {
                process.Start();
                process.WaitForExit(); // Wait for the process to complete

                if (process.ExitCode != 0)
                {
                    string error = process.StandardError.ReadToEnd();
                    throw new Exception($"Wkhtmltopdf error: {error}");
                }

                // Read the generated PDF file as bytes
                return System.IO.File.ReadAllBytes(outputPath);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating PDF", ex);
            }
            finally
            {
                process.Dispose();
            }
        }


        public string GetResolutionpdfhtmlPrevieww(string refid, string doctype)
        {

            DiaryViewModel mdl = new DiaryViewModel();

            mdl._LstActions = DiaryViewModel.GetProcedingDataForCommPDF(refid, doctype);

            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }

            var dptobj = AgendaModelFunction.GetDeptData(deptid);
            string outXml = "";
            string logo = "";

            var path1 = BlobStorage.GetStorageAcessingPath();



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

            outXml = outXml + @"<html>
                                <head><meta charset='UTF-8'><title>FileActionsReport</title>
                                    
                                        <style>

table.maintable { overflow: visible !important; }
thead { display: table-header-group }
tr { page-break-inside: avoid }

                                     td {
                                        vertical-align: top;
                                    }
                                    .pdf-class table.maintable td.secondtd {
                                                                                                                
                                      }
                          .MsoNormalTable tr,.MsoNormalTable td,.MsoNormalTable th,.MsoTableGrid tr,.MsoTableGrid td,.MsoTableGrid th{
                                   page-break-inside: avoid !important;                                                                          
                                    }    
                                .pdf-class tbody .MsoNormal{
                                        text-wrap:wrap;                                       
                                        white-space: pre-wrap;
                                    }

                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;                                       
                                         white-space: pre-wrap;
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
                                        </style>
                                    </head> 
                                    <body>
                                <div class='pdf-class' style='max-width:1050px; margin:auto; '>
             <div><div class='clearfix'></div>";
            outXml = outXml + @"<div style='text-align:center' >";
            outXml = outXml + @"<img src='" + logo + "' height='50' width='55' />";



            outXml += @"<table class='maintable'   style='width:100%;font-size: 15px; border: 1px solid #cdcdcd;border-collapse: collapse;text-align: left;margin: auto;'>";
            outXml = outXml + @"<tr style = 'padding:5px;'>
            <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>No. & Date</th> 
            <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>" + doctype + " Details" + "</th>" +
            " <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center;width: 20%;'>Status</th>" +
            //"<th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>Status Details</th>" +
            "</tr>";



            for (int i = 0; i < mdl._LstActions.Count; i++)
            {
                outXml += @"<tr>										
										<td  class='pdf-class1' style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top; '>
                                        " + Convert.ToString(mdl._LstActions[i].LetterNo) + @"<br>" + Convert.ToString(mdl._LstActions[i].agendadate) + @"   
                                        </td>
                                        <td class='secondtd' style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top; '>";




                string _subjectDetailslocal = Convert.ToString(mdl._LstActions[i].SubjectDetailsLocal);
                string _subjectlocal = Convert.ToString(mdl._LstActions[i].SubjectLocal);

                string _subject = Convert.ToString(mdl._LstActions[i].Subject);
                string _subjectDetails = Convert.ToString(mdl._LstActions[i].SubjectDetails);

                //if (_subject != "")
                //{
                //    _subjectlocal = "<b>" + _subjectlocal + " :- </b></br>";
                //    _subject = "<b>" + _subject + " :- </b>";
                //}

                outXml += @"<u>" + _subjectlocal + "</u> " + _subjectDetailslocal + "<u>" + _subject + "</u> "
                    + _subjectDetails;
                // outXml += @"<u>" + _subject + "</u> " + _subjectDetails;

                outXml += @"</td><td style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>";



                outXml += @"<u>" + Convert.ToString(mdl._LstActions[i].StatusName) + "</u> " + Convert.ToString(mdl._LstActions[i].agendaaction);
                // outXml += @"</td><td style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;'>";

                string[] lgcommentsarray = mdl._LstActions[i].LGComments.Split('~');
                if (lgcommentsarray.Length > 0)
                {


                    int j = 1;
                    foreach (string lgcomments in lgcommentsarray)
                    {


                        string[] ActionBy = lgcomments.Split(new string[] { "::" }, StringSplitOptions.None);
                        if (ActionBy.Length > 1)
                        {
                            outXml += @"<div id='LGComments' Class='SDHide'>";
                            outXml += @"<div class='row'><p class='col-12'>" + (j) + ". " + ActionBy[2] + " </p> <p style='font-size:12px'>" + ActionBy[1] + "</br >  " + ActionBy[0] + "</br ></p> <hr class='dotted'> </div>";
                            outXml += @"</div>";
                            j++;
                        }
                    }

                }

                outXml += @"</td></tr>";
            }
            outXml += "</table>";
            outXml += "</div> </div> </body> </html>";


            return outXml;
        }

        public string GetResolutionpdfhtmlPreview(string refid, string doctype)
        {
            DataTable data = new DataTable();
            DiaryViewModel dvm = new DiaryViewModel();
            string DeptId = CurrentSession.DeptID;
            data = dvm.GetIncludedRefPDF(Convert.ToInt64(refid), doctype, DeptId);
            var dtt = dvm.GetTableRows(data);

            string outXml = "";
            outXml = outXml + @"<html>
                                    <head><meta charset='UTF-8'><title>MC: References</title>
                                   
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
                                    <body>
                    <div class='pdf-class' style='max-width:1050px; margin:auto;'><div>
            <div class='clearfix'></div></div><div style='margin: 5px 5px 5px 5px;'>";

            outXml += @"<table style='width:100%;font-size: 15px; border: 1px solid #cdcdcd;border-collapse: collapse;text-align: left;margin: auto;'>";
            outXml = outXml + @"<tr style = 'padding:5px;'>
            <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>No. & Date</th> 
            <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>" + doctype + "</th>" +
            " <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>Status</th>" +
            "<th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>Status Details</th></tr>";



            foreach (DataRow row in data.Rows)
            {
                outXml += @"<tr>
										<td style='padding:10px; border: 1px solid #cdcdcd;border-collapse: collapse;vertical-align:top;text-align:center'>               
												<b>" + Convert.ToString(row["LetterNo"]) + @"</b></br>
                                                <b>" + Convert.ToString(row["CreatedDate"]) + @"</b>
										</td>
									    <td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>
                                        " + Convert.ToString(row["Subject"]) + @"</b></br>
                                         " + Convert.ToString(row["SubjectDetails"]) + @"
                                        </td>
                                          <td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>
                                        " + Convert.ToString(row["StatusName"]) + @"
                                         <b>" + Convert.ToString(row["ActionTakenDate"]) + @"</b>
                                        </td>








                                         <td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>
                                        " + Convert.ToString(row["LGComments"]) + @"
                                        </td>











                 </tr>";
            }




            //  else

            //  {
            //      outXml += @"<tr><td colspan='2' style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>Comments not available</td></tr>";
            //  }
            outXml += " </table></br>";
            outXml += @"</div></body> </html>";
            return outXml;
        }



        public ActionResult PDFPreviewReport(string refid, string doctype)
        {
            string htmlString = GetResolutionpdfhtml(refid, doctype);


            //var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            //{
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageSize = Rotativa.Options.Size.A4,
            //    PageMargins = new Rotativa.Options.Margins(0, 0, 0, 0)
            //};


            var MCName = CurrentSession.MCName;
            var MCHeader = "";
            if (MCName != "")
            {
                MCHeader = " " + MCName;
            }

            // var MCHeader = "Municipal Corporation " + MCName;

            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"MC \" " +
                    "--footer-right \"PDF Preview Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + MCHeader + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };



            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
            string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{refid}_Proceeding.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);
            //SavePDf2(refid.ToString(), pdfFilename, "ProceedingRpt");

            //var fileaccessingurl = BlobStorage.GetStorageAcessingPath();
            //var path = fileaccessingurl + "/" + pdfFilename;
            //var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            //var fsResult = new FileStreamResult(fileStream, "application/pdf");
            //Response.AppendHeader("Content-Disposition", "inline; filename=PDFPreview.pdf");
            //return fsResult;

            return File(pdfBytes, "application/pdf", "Proceeding.pdf");

        }





        public string GetResolutionpdfhtml(string refid, string doctype)
        {
            DataTable data = new DataTable();
            DiaryViewModel dvm = new DiaryViewModel();
            string DeptId = CurrentSession.DeptID;
            data = dvm.GetIncludedRefPDF(Convert.ToInt64(refid), "", DeptId);
            var dtt = dvm.GetTableRows(data);

            string outXml = "";
            outXml = outXml + @"<html>
                                    <head><meta charset='UTF-8'><title>MC: References</title>
                                    
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
                                    <body>
                    <div class='pdf-class' style='max-width:1050px; margin:auto;'><div>
            <div class='clearfix'></div></div><div style='margin: 5px 5px 5px 5px;'>";

            outXml += @"<table style='width:100%;font-size: 15px; border: 1px solid #cdcdcd;border-collapse: collapse;text-align: left;margin: auto;'>";
            outXml = outXml + @"<tr style = 'padding:5px;'>
            <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>No. & Date</th> 
            <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>" + doctype + "</th>" +
            " <th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>Status</th>" +
            "<th style ='padding:10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>Status Details</th></tr>";



            foreach (DataRow row in data.Rows)
            {
                outXml += @"<tr>
										<td style='padding:10px; border: 1px solid #cdcdcd;border-collapse: collapse;vertical-align:top;text-align:center'>               
												<b>" + Convert.ToString(row["LetterNo"]) + @"</b></br>
                                                <b>" + Convert.ToString(row["CreatedDate"]) + @"</b>
										</td>
									    <td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>
                                        " + Convert.ToString(row["Subject"]) + @"</b></br>
                                         " + Convert.ToString(row["SubjectDetails"]) + @"
                                        </td>
                                          <td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>
                                        " + Convert.ToString(row["StatusName"]) + @"
                                         <b>" + Convert.ToString(row["ActionTakenDate"]) + @"</b>
                                        </td>

                                         <td style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top'>
                                        " + Convert.ToString(row["HouseDecision"]) + @"
                                        </td>
                 </tr>";
            }




            //  else

            //  {
            //      outXml += @"<tr><td colspan='2' style='padding: 10px; border: 1px solid #cdcdcd;border - collapse: collapse;vertical-align:top;text-align:center'>Comments not available</td></tr>";
            //  }
            outXml += " </table></br>";
            outXml += @"</div></body> </html>";
            return outXml;
        }


        //VIEWREFERENCESTATUS PDF
        public async Task<ActionResult> FileActionsReport(string RefId, string agendaId, string actionCode)
        {
            try
            {
                Session["userExist"] = "";
                Session["userAdded"] = "";
                int agendaid = Convert.ToInt32(agendaId);//  10045;
                string refid = RefId;
                AgendaDetailModel mdl = new AgendaDetailModel();
                var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
                string header = "";
                string footer = "";
                if (actionCode == "")
                {
                    actionCode = "0";
                }
                if (HeaderlistofAgenda != null)
                {
                    //header = HeaderlistofAgenda.Header;string 
                    header += @"<html><body><div class='margin: 50px 70px;'><h1 style='font-size: 32px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; width: fit-content; margin: auto;'>"
                        + HeaderlistofAgenda.AgendaName + "</h1>";
                    header += "<p style='font-size: 15px; text-align: center'>"
                 + HeaderlistofAgenda.Header + "</p></div></body></html>";
                    footer = HeaderlistofAgenda.Footer;
                }

                string htmlString = Getpdfhtml(agendaid, refid, actionCode);

                string MCName = CurrentSession.MCName;
                string MCHeader = string.IsNullOrEmpty(MCName)
                    ? ""
                    : MCName;

                byte[] pdfBytes;

                // 2️⃣ Launch Chrome Headless
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


                using (var page = await browser.NewPageAsync())
                {
                    // 3️⃣ Load HTML
                    await page.SetContentAsync(htmlString, new NavigationOptions
                    {
                        WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                    });

                    // 4️⃣ PDF Options (Rotativa → Puppeteer mapping)
                    var pdfOptions = new PdfOptions
                    {
                        Landscape = true,
                        Format = PaperFormat.A4,
                        PrintBackground = true,
                        DisplayHeaderFooter = true,

                        HeaderTemplate = $@"
     <div style='width:100%; text-align:center;
                 font-size:12px; font-weight:600;'>
         {MCHeader}
     </div>",

                        FooterTemplate = @"
     <div style='width:100%; font-size:10px;
                 padding:0 20px;
                 display:flex;
                 justify-content:space-between;'>
         <span>Page <span class='pageNumber'></span> of <span class='totalPages'></span></span>
         <span>Digital MC House</span>
         <span>PDF Preview Generated on: <span class='date'></span></span>
     </div>",

                        MarginOptions = new MarginOptions
                        {
                            Top = "20mm",
                            Right = "10mm",
                            Bottom = "20mm",
                            Left = "10mm"
                        },
                        Scale = 1.3m
                    };

                    // 5️⃣ Generate PDF
                    pdfBytes = await page.PdfDataAsync(pdfOptions);
                }

                // 6️⃣ Optional Blob Upload
                string pdfFilename =
                    $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{refid}__RefAction.pdf";

                // BlobStorage.UploadPdfToBlob(pdfBytes, pdfFilename);

                // 7️⃣ Inline Preview
                Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding.pdf");
                return File(pdfBytes, "application/pdf");


                /* var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
                 {
                     PageOrientation = Rotativa.Options.Orientation.Landscape,
                     PageSize = Rotativa.Options.Size.A4,
                     PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),


                     // Custom switches for footer, header, and other styling options
                     CustomSwitches =
                         "--footer-left \"Page: [page] of [toPage]\" " +
                         "--footer-center \"MC Punjab\" " +
                         "--footer-right \"Proceedings Generated on: [date]\" " +
                         "--footer-font-size 10 --footer-spacing 5 " +
                         "--header-center \"" + "" + "\" " + // Corrected string concatenation
                         "--header-font-size 12 --header-spacing 10 " +
                         "--zoom 1.3 --no-outline"
                 };
                 byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);

                 string pdfpath = "Proceedings/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + agendaid.ToString() + "__RefAction.pdf";



                 // BlobStorage.UploadPdfToBlob(pdfBytes, pdfpath);

                 Response.Headers.Add("Content-Disposition", "inline; filename=Proceeding");
                 return File(pdfBytes, "application/pdf");*/

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                TempData["Message"] = ex.Message.ToString();
                return RedirectToAction("Index");
            }
        }







        //[HttpPost]
        //public ActionResult GenerateApproval(string RefId, string agendaId, string actionCode, string FileType)
        //{
        //    int approvalid = 0;
        //    Session["userExist"] = "";
        //    Session["userAdded"] = "";
        //    int agendaid = Convert.ToInt32(agendaId);//  10045;
        //    string htmlString = GetLGActionWisepdfhtml(agendaid, actionCode, FileType);
        //    string pdfpath = "Proceedings/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + agendaid.ToString() + "_ReplytoMCReport.pdf";
        //    var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
        //    {
        //        PageOrientation = Rotativa.Options.Orientation.Landscape,
        //        PageSize = Rotativa.Options.Size.A4,
        //        PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),
        //        // Custom switches for footer, header, and other styling options
        //        //CustomSwitches =
        //        //    "--footer-left \"Page: [page] of [toPage]\" " +
        //        //    "--footer-center \"NextGen e-Cabinet Punjab\" " +
        //        //    "--footer-right \"Report Generated on: [date]\" " +
        //        //    "--footer-font-size 10 --footer-spacing 5 " +
        //        //    "--header-center \"" + "" + "\" " + // Corrected string concatenation
        //        //    "--header-font-size 12 --header-spacing 10 " +
        //        //    "--zoom 1.3 --no-outline"
        //    };

        //    byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);


        //    string ApprovalpdfPath = BlobStorage.UploadApprovalsToBlob(pdfBytes, pdfpath);

        //    if (actionCode == "" && actionCode == "0")
        //    {

        //        AgendaController.SavePDf2(agendaid.ToString(), pdfpath, "ReplyToMC");
        //        approvalid = SaveApprovals(Convert.ToInt16(agendaid), Convert.ToString(CurrentSession.UserID), pdfpath);
        //        approvalid = 0;
        //    }
        //    else
        //    {
        //        approvalid = SaveApprovals(Convert.ToInt16(agendaid), Convert.ToString(CurrentSession.UserID), pdfpath);
        //    }

        //    //return RedirectToAction("SignApprovals", "demo", new { Blobpath = pdfpath, AgendaId = agendaid, ApprovalId = approvalid});
        //    return Json(new { success = true, pdfPath = pdfpath, AgendaId = agendaid, ApprovalId = approvalid });
        //    //return Json(new { foo = "bar", baz = "Blech" });
        //}





        [HttpPost]
        public async Task<ActionResult> GenerateApproval(string RefId, string agendaId, string actionCode, string FileType, string Refs)
        {

            var ApprovalAuthorrity = CurrentSession.DeptID;
            var replypath = "";
            if (ApprovalAuthorrity == "LG00001")
            {
                replypath = "ReplyToMC";
            }
            if (ApprovalAuthorrity == "UDD0001")
            {
                replypath = "ReplyToMCByADC";
            }
            int approvalid = 0;

            try
            {
                // 1️⃣ Parse Agenda ID
                int agendaid = Convert.ToInt32(agendaId);

                // 2️⃣ Generate HTML
                string htmlString = GetLGActionWisepdfhtml(agendaid, actionCode, Refs, FileType, false);

                if (string.IsNullOrWhiteSpace(htmlString))
                {
                    return Json(new { success = false, message = "Generated HTML is empty." });
                }

                // 3️⃣ PDF Path
                string pdfpath = "Proceedings/" +
                    DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" +
                    agendaid + "_ReplytoMCReport.pdf";

                byte[] pdfBytes;

                // 4️⃣ Launch Chrome Headless (NO download, NO internet)
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
                using (var page = await browser.NewPageAsync())
                {
                    // 5️⃣ Load HTML
                    await page.SetContentAsync(htmlString, new NavigationOptions
                    {
                        WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                    });

                    // 6️⃣ PDF Options (Rotativa → Puppeteer equivalent)
                    var pdfOptions = new PdfOptions
                    {
                        Landscape = false,
                        Format = PaperFormat.A4,
                        PrintBackground = true,
                        MarginOptions = new MarginOptions
                        {
                            Top = "10mm",
                            Right = "10mm",
                            Bottom = "10mm",
                            Left = "10mm"
                        }
                    };

                    // 7️⃣ Generate PDF
                    pdfBytes = await page.PdfDataAsync(pdfOptions);
                }

               
                string approvalPdfPath = BlobStorage.UploadApprovalsToBlob(pdfBytes, pdfpath);

               
                if (string.IsNullOrEmpty(actionCode) || actionCode == "0")
                {
                    AgendaController.SavePDf2(
                        agendaid.ToString(),
                        pdfpath,
                        replypath
                    );

                    approvalid = SaveApprovals(
                        agendaid,
                        Convert.ToString(CurrentSession.UserID),
                        pdfpath, CurrentSession.DeptID
                    );
                }
                else
                {
                    approvalid = SaveApprovals(
                        agendaid,
                        Convert.ToString(CurrentSession.UserID),
                        pdfpath, ApprovalAuthorrity
                    );
                }

             
                return Json(new
                {
                    success = true,
                    pdfPath = pdfpath,
                    AgendaId = agendaid,
                    ApprovalId = approvalid
                });
            }
            catch (Exception ex)
            {
               
                return Json(new
                {
                    success = false,
                    message = "PDF generation failed.",
                    error = ex.Message
                });
            }
        }

        public async Task<ActionResult> GetPDF(string pdfFileName)
        {
            var pdfBytes = await BlobStorage.GetFileFromAzureAsync(pdfFileName);

            Response.AppendHeader(
                "Content-Disposition",
                "inline; filename=Approval.pdf");

            return File(pdfBytes, "application/pdf");
        }

        //public async Task<ActionResult> GetPDF(string pdfFileName)
        //{
        //    var pdfBytes = await BlobStorage.GetFileFromAzureAsync(pdfFileName);
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-Disposition", "inline; filename=Approvals");
        //    Response.BinaryWrite(pdfBytes);
        //    Response.End();
        //    return new EmptyResult();
        //}
        public async Task<ActionResult> PreviewApproval(string RefId, string agendaId,string Refs, string actionCode, string FileType)
        {
            AgendaModelFunction.SaveAgendaReferenceHistory(agendaId, Refs, CurrentSession.UserID);
            int agendaid = Convert.ToInt32(agendaId);
            string htmlString = GetLGActionWisepdfhtml(agendaid, actionCode, Refs, FileType,true);
            var MCName = CurrentSession.MCName;
            var MCHeader = "Digital MC House " + MCName;
            var pdfOptions = new PuppeteerSharp.PdfOptions();
            pdfOptions.Format = PuppeteerSharp.Media.PaperFormat.A4;
            pdfOptions.Landscape = false;
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
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                ExecutablePath = @"C:\chrome-headless-shell-win64\chrome-headless-shell.exe",
                Headless = true
            }))

            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(watermarkCss + htmlString);
                page.DefaultNavigationTimeout = 0;
                var pdfBytes = await page.PdfDataAsync(pdfOptions);
                Response.Headers.Add("Content-Disposition", "inline; filename=AgendaPreview");
                return File(pdfBytes, "application/pdf");
            }
        }

        //VIEWCONSOLIDATED PDF
        public ActionResult ReplyToMCReport(string RefId, string agendaId)
        {
            Session["userExist"] = "";
            Session["userAdded"] = "";
            int agendaid = Convert.ToInt32(agendaId);//  10045;
            string htmlString = GetLGSameResolutionpdfhtml(agendaid);
            string pdfpath = "Proceedings/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + agendaid.ToString() + "_ReplytoMCReport.pdf";
            //AgendaController.SavePDf2(agendaid.ToString(), pdfpath, "_CommissionerReport");
            var pdfResult = new ViewAsPdf("_PdfContent", htmlString)
            {
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Rotativa.Options.Margins(20, 10, 20, 10),

                // Custom switches for footer, header, and other styling options
                CustomSwitches =
                    "--footer-left \"Page: [page] of [toPage]\" " +
                    "--footer-center \"MC Punjab\" " +
                    "--footer-right \"Report Generated on: [date]\" " +
                    "--footer-font-size 10 --footer-spacing 5 " +
                    "--header-center \"" + "" + "\" " + // Corrected string concatenation
                    "--header-font-size 12 --header-spacing 10 " +
                    "--zoom 1.3 --no-outline"
            };

            byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);
            //string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{agendaid.ToString()}_Proceeding.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfpath);




            AgendaController.SavePDf2(agendaid.ToString(), pdfpath, "ReplyToMC");
            //SaveApprovals(Convert.ToInt16(agendaid), Convert.ToString(CurrentSession.UserID));


            return File(pdfBytes, "application/pdf", "ReplyToMC.pdf");
        }




        public int SaveApprovals(int AgendaId, string UserId, string PDFpath, string ApprovalAuthority)
        {
            // Initialize variables
            string MCName = "";
            DateTime MeetingDate = DateTime.MinValue;
            string MeetingTitle = "";
            string FilePath = "";
            string DeptID = "";
            int ApprovalId = 0;

            // Retrieve approval data
            var approval = AgendaModelFunction.GetAgendaDetailsforApproval(AgendaId).FirstOrDefault();

            if (approval != null)
            {
                // Extract data from the single record
                MCName = approval.DepartmentName;
                MeetingDate = Convert.ToDateTime(approval.AgendaDate);
                MeetingTitle = Convert.ToString(approval.AgendaName);
                FilePath = Convert.ToString(PDFpath);
                DeptID = Convert.ToString(approval.DeptId);
            }

            // Save approval details if valid
            if (AgendaId != 0 && approval != null)
            {
                ApprovalId = AgendaModelFunction.SaveApproval(AgendaId, MCName, MeetingDate, MeetingTitle, FilePath, UserId, DeptID, ApprovalAuthority);
            }
            return ApprovalId;
        }

        public ActionResult ProceedingsReport(string RefId, string agendaId)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            Session["userExist"] = "";
            Session["userAdded"] = "";
            int agendaid = Convert.ToInt32(agendaId);//  10045;
            string refid = RefId.ToString();

            //string htmlString = GetProceedingCoveringLetterpdf(agendaid, refid);
            string htmlString = GetProceedingCoveringLetterpdfNew(agendaid, refid);

            string pdfpath = "Proceedings/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + agendaid.ToString() + "_ProceedingsReport.pdf";

            AgendaController.SavePDf2(agendaid.ToString(), pdfpath, "commisisionerRpt");

            var MCName = CurrentSession.MCName;
            var MCHeader = "";
            if (MCName != "")
            {
                MCHeader = "Municipal Corporation " + MCName;
            }
            // var MCHeader = "Municipal Corporation" + MCName;

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
            //string pdfFilename = $"Proceedings/{DateTime.Now:ddMMyyyyHHmmss}_{agendaid.ToString()}_Proceeding.pdf";
            BlobStorage.UploadPdfToBlob(pdfBytes, pdfpath);
            AgendaController.SavePDf2(agendaid.ToString(), pdfpath, "commisisionerRpt");
            return File(pdfBytes, "application/pdf", "Proceeding.pdf");

        }

        public string GetObservationReplypdf(int agendaid, string refid)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);
            mdl._LstResolutionActions = AgendaModelFunction.GetProcedingDataForCommPDF(refid, "0");
            string outXml = "";
            string meetingdate = "";
            string meetingtime = "";
            string url = "~/SecureFileStructure/";
            string path = Server.MapPath(url);

            string relativePath = "~/"; //"~/SecureFileStructure/";
            string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            string imagePath = baseUrl + mdl._LstReferences[0].DeptLogo;

            if (HeaderlistofAgenda != null)
            {
                meetingdate = HeaderlistofAgenda.AgendaDate;
                meetingtime = HeaderlistofAgenda.StartTime;
            }
            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                outXml = outXml + @"<html><body><div>";
                outXml = outXml + @"<div style='page-break-after: always;'>";
                outXml = outXml + @"<img src='" + imagePath + "' height='50' width='55' style='float:left' />";

                outXml += @"<p style='text-align: center !important;text-decoration: underline;font-weight:bold;font-size:20pt;padding-top: 16px;'>" + mdl._LstReferences[0].FromDeptLocal + "</p></br>";

                outXml += @"<span style='float:left;'>ਸੇਵਾ ਵਿਖੇ,</span></br>";
                outXml += @"<span style='float:left;margin-left:80px'>ਮਾਨਯੋਗ ਡਾਇਰੈਕਟਰ ਸਾਹਿਬ,</span></br>";
                outXml += @"<span style='float:left;margin-left:80px'>ਸਥਾਨਕ ਸਰਕਾਰ ਵਿਭਾਗ,(ਪੰਜਾਬ)</span></br>";
                outXml += @"<span style='float:left;margin-left:80px'>ਸੈਕਟਰ-35, ਚੰਡੀਗੜ੍ਹ ।</span></br></br></br>";
                outXml += @"<span style='float:left;'>ਨੰਬਰ:... </span><span style='float:right;'>" + "ਮਿਤੀ: " + DateTime.Now.ToString("dd/MM/yyyy") + "</span></br></br>";
                var firstResolutionNo = "";
                var lastResolutionNo = "";
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    firstResolutionNo = mdl._LstReferences[0].ResolutionNo;
                }
                outXml += @"<span style='float:left;width:50px'>ਵਿਸ਼ਾ:- </span>" + "<span style='text-align: justify;'> ਮਤਾ ਨੰ <b>" + mdl._LstReferences[0].FromDeptLocal + "</b> ਮਿਤੀ: <b>" + meetingdate + "</b> ਦੀ ਪ੍ਰਵਾਨਗੀ ਦੇਣ ਸਬੰਧੀ। ";
                outXml += @"<span style='float:left;width:50px'>ਹਵਾਲਾ:- </span>" + "<span style='text-align: justify;'> ਡਾਇਰੈਕਟਰ ਸਥਾਨਕ ਸਰਕਾਰ ਦਫ਼ਤਰ ਦੇ ਪੱਤਰ ਨੰ- ਸ5-ਡਸਸ-ਮਸਸ-2022/11599 ਮਿਤੀ- 29/03/22 ਉਪਰੋਕਤ ਵਿਸ਼ੇ ਸਬੰਧੀ ਦੱਸਿਆ ਜਾਂਦਾ ਹੈ ਕਿ ਨਗਰ ਨਿਗਮ ਦੇ ਮਤਾ ਨੰ 103 ਮਿਤੀ-10/01/22 ਨਾਲ ਹੇਠ ਲਿਖੇ ਕੰਮਾਂ ਵਿੱਚ 25 ਵਾਧਾ ਕੀਤਾ ਗਿਆ ਸੀ।";


                outXml += @"</br></br></br><span style='float:left'><b>Observation Text</b></span>";
                outXml += @"<span style='float:right;'>ਨਿਗਰਾਨ ਇੰਜੀਨੀਅਰ,</span></br>";
                outXml += @"<span style='float:right;'>ਨਗਰ ਨਿਗਮ,</span></br>";
                outXml += @"<span style='float:right;'>" + mdl._LstReferences[0].FromDeptLocal + "</span></br>";


                outXml += @"</div></body> </html>";
            }
            return outXml;
        }

        public ActionResult GetDataForDiaryIndivisualReferences(string financialyear, int RefType = 0, string MenuName = "", string MeetingType = "", string selectedType = "")
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();
            dt = DiaryDashboard.GetDataForDiaryDashboardIndivisual(CurrentSession.OfficeId, Convert.ToString(CurrentSession.UserID), CurrentSession.DeptID, out dtstatus, financialyear, RefType, MeetingType, selectedType);

            sb.Append("<table id='MemoType' class='table table-bordered'>");
            sb.Append("<thead>");
            if (dt.Rows.Count >= 0)
            {
                int count = dt.Columns.Count;
                sb = sb.Append("<tr role='row' class='table-heading'>");
                sb = sb.Append("<th style='text-align:center;' colspan='" + count + "' class='td-heading'>House Resolutions</th>");
                sb = sb.Append("</tr>");
                sb = sb.Append("<tr role='row' class='table-heading'>");
                sb = sb.Append("<th class='td-heading' id='tdFrom'>Department</th>");

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        string title = result1[0].ItemArray[1].ToString().Trim();
                        int idx = title.IndexOf("_");
                        sb = sb.Append("<th class='td-heading'>" + (idx >= 0 ? title.Substring(0, idx) : title) + "</th>");
                    }
                }

                sb = sb.Append("</tr>");

                bool hasSubHeaders = false;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        string title = result1[0].ItemArray[1].ToString().Trim();
                        int lastIdx = title.LastIndexOf("_");
                        if (lastIdx >= 0 && lastIdx < title.Length - 1)
                        {
                            hasSubHeaders = true;
                            break;
                        }
                    }
                }

                if (hasSubHeaders)
                {
                    sb = sb.Append("<tr role='row' class='table-heading'>");
                    sb = sb.Append("<th class='td-heading' id='tdFrom'></th>");

                    for (int i = 1; i < dt.Columns.Count; i++)
                    {
                        DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                        if (result1.Length > 0)
                        {
                            string title = result1[0].ItemArray[1].ToString().Trim();
                            int lastIdx = title.LastIndexOf("_");
                            sb = sb.Append("<th class='td-heading'>" + (lastIdx >= 0 ? title.Substring(lastIdx + 1) : "") + "</th>");
                        }
                    }

                    sb = sb.Append("</tr>");
                }
                sb.Append("</thead>");
            }

            sb.Append("<tbody>");
            //sb.Append("<thead>");
            int j = 0, GTotal = 0;

            foreach (DataRow drDyn in dt.Rows)
            {
                sb = sb.Append("<tr id='trtdIn" + drDyn["deptId"] + "'>");
                sb = sb.Append("<td  class='center td-heading' onclick='GoToStatusType(&#39" + drDyn["deptId"] + "&#39)'>" + drDyn["deptName"].ToString() + "</td>");

                int Total = 0; string cnt;
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                    }
                }

                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result.Length > 0)
                    {
                        int docid = Convert.ToInt32(result[0].ItemArray[0]);
                        cnt = string.IsNullOrEmpty(Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()])) ? "0" : Convert.ToString(drDyn[dt.Columns[i].ColumnName.ToString()]);
                        Total = Total + Convert.ToInt32(cnt);
                        sb = sb.Append("<td class='center td-heading'><a data-target='DashboardStat1' department= '" + drDyn["deptName"].ToString() + "' Mainid='trtdIn" + drDyn["deptId"] + "'  onclick ='GetDiaryDasdhboardDataChangeIndivisual(this)' DeptId='" + drDyn["deptId"] + "'DocTypeId='" + docid + "'OfficeId='" + drDyn["officeid"] + "' + >" + cnt + "</a></td>");
                    }
                }
                sb = sb.Append("</tr>");
                j = j + 1;
            }
            if (dt.Rows.Count > 0)
            {
                sb = sb.Append("<tr id='trtd0'>");
                sb = sb.Append("<td class='center td-heading'>Total</td> ");

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
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataRow[] result1 = dtstatus.Select("DocumentTypeName='" + dt.Columns[i].ColumnName.ToString() + "'");
                    if (result1.Length > 0)
                    {
                        int docid = Convert.ToInt32(result1[0].ItemArray[0].ToString());
                        int cnt = dt.AsEnumerable().Sum(row => row.Field<int>(result1[0].ItemArray[1].ToString()));
                        //int cnt = 0;
                        GTotal = GTotal + cnt;
                        sb = sb.Append("<td class='center td-heading total'><a onclick='GetDiaryDasdhboardDataTotalStatus(this)' DeptId='" + "99" + "' DocTypeId='" + docid + "'>" + cnt + "</a></td>");
                    }
                }
            }
            else
            {
                sb = sb.Append("<tr><td class='center td-heading'>No Data found</td></tr> ");
            }
            sb = sb.Append("</tr> ");
            sb.Append("</tbody>");

            return Content(sb.ToString());
        }

        public ActionResult GetDataForDiaryIndivisualReferencesList(string deptid, string officeid, string documentype, string DepartmentName, int RefType = 0, string MenuName = "", string selectedType = "", string MeetingType = "")
        {
            string status = "Diary";
            string type = "";
            string userid = CurrentSession.UserID;
            LstDashboard model = new LstDashboard();
            StringBuilder sb = new StringBuilder();
            DataTable dtstatus;
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(MeetingType) && CurrentSession.MapId != "0")
            {
                MeetingType = CurrentSession.MapId;
            }

            var useraction = tPaperLaidV.GetUserAction(CurrentSession.UserID);
            string deptd = CurrentSession.DeptID;


            sb.Append("<div class='table-responsive table-cus' style='margin-top: 20px; height:100%;'>");
            sb.Append("<table id='Ind111' style='width: 100%; display: inline-table; border-width: 0 0 1px 0;' class='table table-bordered'>");


            if (documentype == "222")
            {
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='8' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > Total Proposals </h6>");
                sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='MemoDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</th>");
                sb.Append("</tr>");
            }
            else
            {
                string documentTypeName = useraction.Where(f => f.DocmentType == Convert.ToInt16(documentype)).Select(n => n.DocumentTypeName).FirstOrDefault().ToString();
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan='8' style='background: #d9eaff; padding: 10px; border-bottom: 3px solid #a3c2f5;'>");
                sb.Append("<div style='display: flex; justify-content: space-between; align-items: center; height: 100%;'>");
                sb.Append("<h6 style='margin: 0; font-size: 16px; color: #ffffffff; font-weight: 600;'>" + DepartmentName + " > " + documentTypeName + "</h6>");
                sb.Append("<button style='padding: 5px 15px; font-size: 14px;' type='button' onclick='MemoDivs()' class='btn btn-primary' aria-label='Close'>Back</button>");
                sb.Append("</div>");
                sb.Append("</th>");
                sb.Append("</tr>");

            }






            var data1 = tPaperLaidV.GetMCStatDetailsIndivisualList(deptid, officeid, documentype, userid, RefType, selectedType, MeetingType);
            model._LstStatDetail = data1;

            //sb.Append("<thead>");
            sb.Append("<tr class='table-heading'>");
            sb.Append("<th class='text-center td-heading'></th>");
            sb.Append("<th class='text-center td-heading'>No.</th>");
            sb.Append("<th class='text-center td-heading'>From</th>");
            sb.Append("<th class='text-center td-heading'>To</th>");
            sb.Append("<th style='' class='text-center td-heading'>Subject</th>");
            //sb.Append("<th class='text-center td-heading'>Type</th>");
            sb.Append("<th class='text-center td-heading'>Status</th>");
            sb.Append("<th class='text-center td-heading'>RefId/Date</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (var mdl1 in model._LstStatDetail)
            {
                sb.Append("<tr  id='trtdInd" + mdl1.Resolutionno + "' role='row'>");

                sb.Append("<td class='td-data'><a class='green' id='" + mdl1.ReferenceId + "' onclick = EditMlaDispatch(this,0,'" + status + "')><i class='fa fa-eye bigger-150'></i></a></td>");

                sb.Append("<td class='td-data'>" + mdl1.Resolutionno + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.FromDepartment + "-" + mdl1.FromOffice + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.ToDepartment + "-" + mdl1.ToOffice + "</td>");
                sb.Append("<td>" + mdl1.Subject + "</td>");
                //sb.Append("<td class='td-data'>" + mdl1.DocType + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.ActionName + "</td>");
                sb.Append("<td class='td-data'>" + mdl1.ReferenceId + "<br>" + Convert.ToDateTime(mdl1.AgendaDate).ToString("dd /MM/yyyy") + "</td>");

                sb.Append("</tr>");
            }



            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");
            return Content(sb.ToString());

        }
        [HttpGet]
        public JsonResult getRefHistory(string refId)
        {
            var dtt = AgendaModelFunction.GetRefChangeHistoryy(refId);

            var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult NotRegisterUser()
        {
            return View();
        }





        public JsonResult GetSectors()
        {
            SectorModel mdl = new SectorModel();
            try
            {


                DataSet dataSetUser = new DataSet();
                dataSetUser = CMNirdesh.Models.DiaryViewModel.GetSectordata();

                List<SectorModel> assList = new List<SectorModel>();
                if (dataSetUser != null)
                {
                    for (int i = 0; i < dataSetUser.Tables[0].Rows.Count; i++)
                    {
                        SectorModel m = new SectorModel();
                        m.SectorID = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["DocumentTypeId"]);
                        m.SectorName = Convert.ToString(dataSetUser.Tables[0].Rows[i]["DocumentTypeName"]);
                        m.SectorNameLocal = Convert.ToString(dataSetUser.Tables[0].Rows[i]["DocumentTypeNameLocal"]);
                        assList.Add(m);
                    }


                }
                mdl.ListSector = assList;


            }
            catch (Exception ex)
            {
                // CommonModel.SendErrorToText(ex);
            }
            return Json(mdl.ListSector, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAssembly()
        {
            AssemblyModel mdl = new AssemblyModel();
            try
            {


                //model.AssemblyList = DiaryViewModel.Assembly();
                //model.SessionList = DiaryViewModel.Session();
                //model.SessDatelist = DiaryViewModel.SessionDates();

                DataSet dataSetUser = new DataSet();
                dataSetUser = CMNirdesh.Models.DiaryViewModel.Assembly2();

                List<AssemblyModel> assList = new List<AssemblyModel>();
                if (dataSetUser != null)
                {
                    for (int i = 0; i < dataSetUser.Tables[0].Rows.Count; i++)
                    {
                        AssemblyModel m = new AssemblyModel();
                        m.AssemblyID = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["AssemblyID"]);
                        m.AssemblyCode = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["AssemblyCode"]);
                        m.AssemblyName = Convert.ToString(dataSetUser.Tables[0].Rows[i]["AssemblyName"]);
                        m.AssemblyNameLocal = Convert.ToString(dataSetUser.Tables[0].Rows[i]["AssemblyNameLocal"]);
                        assList.Add(m);
                    }
                }
                mdl.ListAssembly = assList;


            }
            catch (Exception ex)
            {
                // CommonModel.SendErrorToText(ex);
            }
            return Json(mdl.ListAssembly, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSession()
        {
            SessionModel mdl = new SessionModel();
            try
            {
                List<KeyValuePair<string, string>> methodParameter = new List<KeyValuePair<string, string>>();

                //model.AssemblyList = DiaryViewModel.Assembly();
                //model.SessionList = DiaryViewModel.Session();
                //model.SessDatelist = DiaryViewModel.SessionDates();

                DataSet dataSetUser = new DataSet();
                dataSetUser = CMNirdesh.Models.DiaryViewModel.Session2();

                List<SessionModel> assList = new List<SessionModel>();
                if (dataSetUser != null)
                {
                    for (int i = 0; i < dataSetUser.Tables[0].Rows.Count; i++)
                    {
                        SessionModel m = new SessionModel();
                        m.AssemblyCode = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["AssemblyID"]);
                        m.SessionID = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["SessionID"]);
                        m.SessionCode = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["SessionCode"]);

                        m.SessionName = Convert.ToString(dataSetUser.Tables[0].Rows[i]["SessionName"]);
                        m.SessionNameLocal = Convert.ToString(dataSetUser.Tables[0].Rows[i]["SessionNameLocal"]);
                        assList.Add(m);
                    }
                }
                mdl.ListSession = assList;
            }
            catch (Exception ex)
            {
                // CommonModel.SendErrorToText(ex);
            }
            return Json(mdl.ListSession, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSessionDates()
        {
            SessionDateModel mdl = new SessionDateModel();
            try
            {



                DataSet dataSetUser = new DataSet();
                dataSetUser = CMNirdesh.Models.DiaryViewModel.SessionDate();


                List<SessionDateModel> assList = new List<SessionDateModel>();
                if (dataSetUser != null)
                {
                    for (int i = 0; i < dataSetUser.Tables[0].Rows.Count; i++)
                    {
                        SessionDateModel m = new SessionDateModel();
                        m.ID = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["Id"]);
                        m.AssemblyCode = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["AssemblyID"]);
                        m.SessionID = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["SessionID"]);
                        m.SessionDate = Convert.ToDateTime(dataSetUser.Tables[0].Rows[i]["SessionDate"]).ToString("dd-MM-yyyy");

                        assList.Add(m);
                    }
                }
                mdl.ListSessionDate = assList;
            }
            catch (Exception ex)
            {
                // CommonModel.SendErrorToText(ex);
            }
            return Json(mdl.ListSessionDate, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetSessionByAssemblyId(int AssemblyCode)
        {
            SessionModel mdl = new SessionModel();
            try
            {
                DataSet dataSetUser = new DataSet();
                dataSetUser = CMNirdesh.Models.DiaryViewModel.SessionByAssemblyId(Convert.ToString(AssemblyCode));

                List<SessionModel> assList = new List<SessionModel>();
                if (dataSetUser != null)
                {
                    for (int i = 0; i < dataSetUser.Tables[0].Rows.Count; i++)
                    {
                        SessionModel m = new SessionModel();
                        m.AssemblyCode = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["AssemblyID"]);
                        m.SessionID = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["SessionID"]);
                        m.SessionCode = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["SessionCode"]);

                        m.SessionName = Convert.ToString(dataSetUser.Tables[0].Rows[i]["SessionName"]);
                        m.SessionNameLocal = Convert.ToString(dataSetUser.Tables[0].Rows[i]["SessionNameLocal"]);
                        assList.Add(m);
                    }
                }
                mdl.ListSession = assList;
            }
            catch (Exception ex)
            {
                //CommonModel.SendErrorToText(ex);
            }
            return Json(mdl.ListSession, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSessionDateByAssemblyId(int AssemblyCode, int SessionCode)
        {
            SessionDateModel mdl = new SessionDateModel();
            try
            {


                DataSet dataSetUser = new DataSet();
                dataSetUser = CMNirdesh.Models.DiaryViewModel.SessionByAssemblySessionId(Convert.ToString(AssemblyCode), Convert.ToString(SessionCode));

                List<SessionDateModel> assList = new List<SessionDateModel>();
                if (dataSetUser != null)
                {
                    for (int i = 0; i < dataSetUser.Tables[0].Rows.Count; i++)
                    {
                        SessionDateModel m = new SessionDateModel();
                        m.ID = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["Id"]);
                        m.AssemblyCode = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["AssemblyID"]);
                        m.SessionID = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["SessionID"]);
                        m.SessionDate = Convert.ToDateTime(dataSetUser.Tables[0].Rows[i]["SessionDate"]).ToString("dd-MM-yyyy");

                        assList.Add(m);
                    }
                }
                mdl.ListSessionDate = assList;
            }
            catch (Exception ex)
            {
                // CommonModel.SendErrorToText(ex);
            }
            return Json(mdl.ListSessionDate, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentTypeVal()
        {
            DocumentTypeModel mdl = new DocumentTypeModel();
            try
            {


                DataSet dataSetUser = new DataSet();
                dataSetUser = DiaryViewModel.GetDocumentTypeListval(CurrentSession.UserID.ToString());

                List<DocumentTypeModel> assList = new List<DocumentTypeModel>();
                if (dataSetUser != null)
                {
                    for (int i = 0; i < dataSetUser.Tables[0].Rows.Count; i++)
                    {
                        DocumentTypeModel m = new DocumentTypeModel();

                        m.DocmentType = Convert.ToInt32(dataSetUser.Tables[0].Rows[i]["DocmentType"]);
                        m.DocumentTypeName = Convert.ToString(dataSetUser.Tables[0].Rows[i]["DocumentTypeName"]);
                        assList.Add(m);
                    }
                }
                mdl.Documenttypelist = assList;
            }
            catch (Exception ex)
            {
                // CommonModel.SendErrorToText(ex);
            }
            return Json(mdl.Documenttypelist, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult FreezeAction(string ActionId)
        {
            string response = "";
            Int64 refid = 0, fileRefId = 0;
            int resnumber = 0;
            DataSet ds = new DataSet();
            try
            {
                DiaryViewModel dvm = new DiaryViewModel();
                string userid = CurrentSession.UserID.ToString();
                string actionby = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
                ds = dvm.FreezeAction(Convert.ToInt32(ActionId), userid, actionby);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    refid = Convert.ToInt64(ds.Tables[0].Rows[0]["DiaryRefId"]);
                    fileRefId = Convert.ToInt64(ds.Tables[0].Rows[0]["fileRefId"]);
                    resnumber = Convert.ToInt32(ds.Tables[0].Rows[0]["ResolutionNo"]);
                }

                //Notifications
                DataTable dt = new DataTable();
                dt = UserModelFunction.GetUserbyOfficeId("LG00001", "159");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string SentToUserId = dr["id"].ToString();
                        string Notification = "Observation Replied Resolution No: " + resnumber;
                        response = DiaryViewModel.InsertNotification(Convert.ToInt64(fileRefId), 0, 7, Notification, SentToUserId, Convert.ToInt32(ActionId), userid);
                    }

                }

                dt = UserModelFunction.GetUserbyOfficeId("LG00001", "180");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string SentToUserId = dr["id"].ToString();
                        string Notification = "Observation Replied Resolution No: " + resnumber;
                        response = DiaryViewModel.InsertNotification(Convert.ToInt64(fileRefId), 0, 7, Notification, SentToUserId, Convert.ToInt32(ActionId), userid);
                    }

                }

            }
            catch (Exception ex)
            {
                return Json("Some technical error occur, please try after some time.", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAnex(string fileid, string isDeleted, string filepath)
        {
            if (fileid != "")
            {
                DiaryViewModel dvm = new DiaryViewModel();
                dvm.UpdateAnex(Convert.ToInt32(fileid), isDeleted, filepath);
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }

        public ActionResult UpdateIsSup(string isSup, string refid)
        {
            if (refid != "")
            {
                DiaryViewModel dvm = new DiaryViewModel();
                dvm.UpdateIsSup(Convert.ToInt32(isSup), Convert.ToInt64(refid));
            }
            return Json("0", JsonRequestBehavior.AllowGet);

        }


        public ActionResult SaveAgenda(MlaDiaryData mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            string res;
            AgendaModel model = new AgendaModel();
            model.FileRefId = mdl.RefId;
            model.AgendaName = mdl.CreateMeeting.AgendaName;
            model.AgendaDate = mdl.CreateMeeting.AgendaDate;
            model.Remarks = mdl.Remarks;
            model.ChairPerson = mdl.CreateMeeting.ChairPerson;
            model.ChairPersonDesignation = mdl.CreateMeeting.ChairPersonDesignation;
           
            model.MeetingType = mdl.CreateMeeting.MeetingType;
            model.UserId = CurrentSession.UserID;
            model.DeptId = CurrentSession.DeptID;
            model.OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            model.Agendatype = "Diary";
            DateTime dateTime = new DateTime();
            if (mdl.CreateMeeting.StartTime == null)
            {
                dateTime = DateTime.Parse(Convert.ToString("01:00:00"), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
            }
            else
            {
                dateTime = DateTime.Parse(Convert.ToString(mdl.CreateMeeting.StartTime), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
            }


            // Get the time in the format "01:15:00.0000000".
            string formattedTime = dateTime.ToString("HH:mm:ss", CultureInfo.GetCultureInfo("en-US"));

            model.StartTime = formattedTime;



            model.Header = mdl.CreateMeeting.Header;
            model.Footer = mdl.CreateMeeting.Footer;

            //model.Footer = model.Footer.Replace("\r\n", "<br>");
            string msg = AgendaModelFunction.SaveMeeting(model);

            if (msg != "0")
            {
                res = "Meeting Added Successfully";

            }
            else if (msg == "-1")
            {
                res = "House ID Does Not Exist";


            }
            else if (msg == "0")
            {
                res = "Meeting Already Exists";


            }
            else
            {
                res = "Something went Wrong";


            }
            ViewBag.OfficeName = CurrentSession.OfficeName;
            int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            mdl._LstAgenda = AgendaModelFunction.GetAgenda(OfficeId, "Dispatch");
            mdl._LstAgenda = mdl._LstAgenda.Where(m => m.AgendaId == msg).ToList();
            //return Json("U", JsonRequestBehavior.AllowGet);
            return PartialView("_CreateMeeting", mdl);
            //return Json(new { Alert = res.ToString(), AgendaId= msg });
        }


        public ActionResult UpdateAgenda(AgendaModel mdl)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            string res;
            MlaDiaryData model = new MlaDiaryData();
            //model.CreateMeeting.FileRefId = mdl.FileRefId;
            //model.CreateMeeting.AgendaName = mdl.AgendaName;
            //model.CreateMeeting.AgendaDate = mdl.AgendaDate;
            //model.CreateMeeting.Remarks = mdl.Remarks;
            //model.CreateMeeting.UserId = CurrentSession.UserID;
            //model.CreateMeeting.DeptId = CurrentSession.DeptID;
            //model.OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            //model.CreateMeeting.Agendatype = "Diary";
            DateTime dateTime = DateTime.Parse(Convert.ToString(mdl.StartTime), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);

            // Get the time in the format "01:15:00.0000000".
            string formattedTime = dateTime.ToString("HH:mm:ss", CultureInfo.GetCultureInfo("en-US"));

            //model.CreateMeeting.StartTime = formattedTime;


            //model.CreateMeeting.Header = mdl.Header;
            //model.CreateMeeting.Footer = mdl.Footer;

            //model.CreateMeeting.Footer = mdl.Footer.Replace("\r\n", "<br>");

            var data = AgendaEdit.UpdateRowById(mdl);

            ViewBag.OfficeName = CurrentSession.OfficeName;
            int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            model._LstAgenda = AgendaModelFunction.GetAgenda(OfficeId, "Dispatch");
            model._LstAgenda = model._LstAgenda.Where(m => m.AgendaId == mdl.AgendaId).ToList();
            //return Json("U", JsonRequestBehavior.AllowGet);
            return PartialView("_CreateMeeting", model);
            //return Json(new { Alert = res.ToString(), AgendaId= msg });
        }

        public ActionResult GetAgendaSetting(List<AdminLOBJson> Indexing)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            string res = "";
            int AgendaId = 0;
            foreach (AdminLOBJson index in Indexing)
            {
                AgendaId = Convert.ToInt32(index.AgendaId);
            }

            MlaDiaryData model = new MlaDiaryData();
            model._MeetingSettingReferences = AgendaModelFunction.GetDishpatchReferences(Convert.ToInt32(AgendaId));
            MeetingSettingModel ms = new MeetingSettingModel();
            ms.Count = model._MeetingSettingReferences.Count.ToString();
            ms.FileRefId = model.RefId.ToString();
            ms.freezeButton = true;
            ms.refresh = res.ToString();
            model.MeetingSetting = ms;
            model.agendaId = AgendaId.ToString();
            return PartialView("_MeetingSetting", model);
            //return Json(new { Alert = res.ToString(), AgendaId= msg });
        }

        public ActionResult SaveIndex(List<AdminLOBJson> Indexing)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            string res = "";
            MlaDiaryData model = new MlaDiaryData();

            //List<AdminLOBJson> indexTable = new List<AdminLOBJson>();
            //indexTable = new JavaScriptSerializer().Deserialize<List<AdminLOBJson>>("Indexing");
            int i = 1;
            int AgendaId = 0;
            foreach (AdminLOBJson index in Indexing)
            {
                int order = i;
                AgendaId = Convert.ToInt32(index.AgendaId);
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
                if (index.IndexTwo != null && index.IndexTwo.ToString() != "")
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
            AgendaModelFunction.FreezeUnfreezeAgenda(Convert.ToInt32(AgendaId), 1);

            model._MeetingSettingReferences = AgendaModelFunction.GetDishpatchReferences(Convert.ToInt32(AgendaId));
            MeetingSettingModel ms = new MeetingSettingModel();
            ms.Count = model._MeetingSettingReferences.Count.ToString();
            ms.FileRefId = model.RefId.ToString();
            ms.freezeButton = true;
            ms.refresh = res.ToString();
            model.MeetingSetting = ms;
            model.agendaId = AgendaId.ToString();
            return PartialView("_MeetingSetting", model);
            //return Json(new { Alert = res.ToString() });
        }

        public ActionResult SaveResolution(List<AdminLOBJson> Indexing)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            MlaDiaryData model = new MlaDiaryData();
            string res = "", resval = "";
            int i = 1; int AgendaId = 0;
            string emptyindex = "", ResolutionExist = "";
            Boolean validate;
            int SesOfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            foreach (AdminLOBJson index in Indexing)
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
                    var uc = Indexing.Where(x => x.Index == index1.ToString());
                    int ac = uc.Count();
                    if (ac > 1)
                    {
                        ResolutionExist = ResolutionExist + index1.ToString() + ",";
                    }
                }
                else
                {
                    var uc = Indexing.Where(x => x.Index.ToString().Contains("." + index2.ToString()));
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
                foreach (AdminLOBJson index in Indexing)
                {
                    //
                    AgendaId = Convert.ToInt32(index.AgendaId);
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
                AgendaModelFunction.FreezeProceedings(Convert.ToInt32(AgendaId), 1);
                res = "Proceedings Saved Successfully.";
                resval = "1";
            }
            else
            {
                res = "Resolution No. is Empty Or Resolution No. " + ResolutionExist + " Already Assign.";
                resval = "0";
            }

            model._MeetingSettingReferences = AgendaModelFunction.GetDishpatchReferences(Convert.ToInt32(model.agendaId));
            MeetingSettingModel ms = new MeetingSettingModel();
            ms.Count = model._MeetingSettingReferences.Count.ToString();
            ms.FileRefId = model.RefId.ToString();
            ms.freezeButton = true;
            model.MeetingSetting = ms;

            return Json(new { Alert = res.ToString(), result = resval });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddTableItem(string agendaid, string subject, string subjectdetails)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            string res;
            MlaDiaryData model = new MlaDiaryData();
            AgendaDetailModel agendamodel = new AgendaDetailModel();
            agendamodel.NewSubject = subject;
            agendamodel.Createdby = CurrentSession.UserID;
            agendamodel.attachfile = "";
            agendamodel.AgendaId = Convert.ToInt32(agendaid);
            agendamodel.NewSubjectDetails = subjectdetails;
            agendamodel.NewLetterNo = "";
            agendamodel.HdnAgendaId = 0;
            agendamodel.AgendaType = "";
            AgendaModelFunction.SaveNewRefAgenda(agendamodel);

            ViewBag.OfficeName = CurrentSession.OfficeName;
            int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
            model._LstAgenda = AgendaModelFunction.GetAgenda(OfficeId, "Dispatch");
            model._LstAgenda = model._LstAgenda.Where(m => m.AgendaId == agendaid).ToList();
            model._LstReferences = AgendaModelFunction.GetApproveReferences(Convert.ToInt32(agendaid));
            MeetingSettingModel ms = new MeetingSettingModel();
            ms.Count = model._LstReferences.Count.ToString();
            ms.FileRefId = model.RefId.ToString();
            model.MeetingSetting = ms;
            model.AgendaCount = model._LstReferences.Count.ToString();
            return PartialView("_ResolutionSetting", model);

        }

        public async Task<ActionResult> SignPdfC(string AgendaId, string TypeofDocument)
        {
            if (string.IsNullOrEmpty(AgendaId))
                return new HttpStatusCodeResult(400, "Invalid token");
           
            string decryptedAgendaId = CryptoHelper.Decrypt(HttpUtility.UrlDecode(AgendaId));
            string decryptedDocType = CryptoHelper.Decrypt(HttpUtility.UrlDecode(TypeofDocument));
            string decryptedApprovalId = "0";

            AgendaDetailModel agendaDetailModel = new AgendaDetailModel();
            string Blobpath = AgendaModelFunction.GetUnsignedPdf(Convert.ToInt32(decryptedAgendaId), decryptedDocType, decryptedApprovalId);
            byte[] fileBytes = await BlobStorage.GetFileFromAzureAsync(Blobpath);
            string fileBase64 = Convert.ToBase64String(fileBytes);
            agendaDetailModel.AgendaId = Convert.ToInt32(decryptedAgendaId);
            agendaDetailModel.agendadetails = fileBase64;
            agendaDetailModel.AgendaType = decryptedDocType;
            agendaDetailModel.ApprovalId = Convert.ToInt32(decryptedApprovalId);
            return View("SignPdf",agendaDetailModel);
        }



       public async Task<ActionResult> SignPdf(string token)
        {
            if (string.IsNullOrEmpty(token))
                return new HttpStatusCodeResult(400, "Invalid token");
            string decodedToken = HttpUtility.UrlDecode(token);
            string decryptedValue = CryptoHelper.Decrypt(decodedToken);
            var parts = decryptedValue.Split('|');
            
            string decryptedAgendaId = Convert.ToString(parts[0]);
            string decryptedApprovalId = Convert.ToString(parts[1]);
            string decryptedDocType = parts[2];

            //string decryptedAgendaId = CryptoHelper.Decrypt(HttpUtility.UrlDecode(AgendaId));
            //string decryptedDocType = CryptoHelper.Decrypt(HttpUtility.UrlDecode(TypeofDocument));
            //string decryptedApprovalId = CryptoHelper.Decrypt(HttpUtility.UrlDecode(ApprovalId));
             
            AgendaDetailModel agendaDetailModel = new AgendaDetailModel();
            string Blobpath = AgendaModelFunction.GetUnsignedPdf(Convert.ToInt32(decryptedAgendaId), decryptedDocType, decryptedApprovalId);
            byte[] fileBytes = await BlobStorage.GetFileFromAzureAsync(Blobpath);
            string fileBase64 = Convert.ToBase64String(fileBytes);
            agendaDetailModel.AgendaId = Convert.ToInt32(decryptedAgendaId);
            agendaDetailModel.agendadetails = fileBase64;
            agendaDetailModel.AgendaType = decryptedDocType;
            agendaDetailModel.ApprovalId = Convert.ToInt32(decryptedApprovalId);
            return View(agendaDetailModel);
        }


        [HttpPost]
        public ActionResult GetEncryptedSignUrl(int AgendaId,int ApprovalId,string TypeofDocument)
        {
            var payload = $"{AgendaId}|{ApprovalId}|{TypeofDocument}";
            var encryptedToken = HttpUtility.UrlEncode(
                CryptoHelper.Encrypt(payload)
            );
            var url = Url.Action("SignPdf","References", new { token = encryptedToken });
            return Json(url);
        }


        public async Task<ActionResult> GenerateNotingPdf(int? from, int? to, string RefId)
        {
            var DeptId = CurrentSession.DeptID;

            // 1️⃣ Get diary data
            var diaryModel = DiaryViewModel.GetDiaryById(
                Convert.ToInt64(RefId), DeptId);

            if (diaryModel == null)
                return HttpNotFound("Diary not found");

            // 2️⃣ Prepare action list
            List<DiaryActionData> list = new List<DiaryActionData>();

            foreach (var item in diaryModel.lstDiaryActionViewModel)
            {
                list.Add(new DiaryActionData
                {
                    NoteId = item.NoteId,
                    ActionBy = item.ActionBy,
                    ActionDate = item.ActionDate,
                    ActionDescription = item.ActionDescription,
                    StatusName = item.StatusName,
                    Enclosure = item.Enclosure
                });
            }

            // 3️⃣ Apply range filter
            if (from.HasValue && to.HasValue)
            {
                list = list
                    .Where(x => x.NoteId >= from && x.NoteId <= to)
                    .OrderBy(x => x.NoteId)
                    .ToList();
            }
            else
            {
                list = list.OrderBy(x => x.NoteId).ToList();
            }

            // 4️⃣ Build HTML string
            StringBuilder sb = new StringBuilder();

            sb.Append(@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<style>
    html, body {
        height: 100%;
        margin: 0;
        padding: 0;
    }

    body {
        font-family: Arial, Helvetica, sans-serif;
        font-size: 12px;
        color: #000;
        background-color: #bcfdca; /* FULL SHEET GREEN */
    }

    /* ===== MC INFO SECTION ===== */
    .mc-info {
        border: 2px solid #000;
        padding: 8px 10px;
        margin-bottom: 12px;
       font-size:14px;
    }

    .mc-row {
        display: flex;
        justify-content: space-between;
        margin-bottom: 4px;
    }

    .mc-label {
        font-weight: bold;
    }

    /* ===== NOTE ===== */
    .note {
        margin-bottom: 22px;
        padding: 12px;
        border-bottom: 1px solid #000;
        page-break-inside: avoid;
    }

    .note-header {
        display: flex;
        justify-content: space-between;
        font-weight: bold;
        margin-bottom: 6px;
    }

    .note-body {
        margin: 8px 0;
        text-align: justify;
    }

    .enclosure {
        font-style: italic;
        margin-bottom: 6px;
    }

    .note-footer {
        display: flex;
        justify-content: space-between;
        font-size: 11px;
    }
</style>
</head>
<body>
");

            // 🔹 MC INFO SECTION (ADDED)
            sb.Append($@"
<div class='mc-info'>
    <div class='mc-row'>
        
        <div><span class='mc-label'>File ID :</span> {RefId}</div>
        <div><span class='mc-label'></span> {diaryModel.FromDept}</div>
        <div><span class='mc-label'>Meeting Date :</span> {diaryModel.agendaDate:dd-MM-yyyy}</div>
    </div>
   
</div>
");

            // 🔹 NOTES
            foreach (var item in list)
            {
                DateTime dt = Convert.ToDateTime(item.ActionDate);

                sb.Append($@"
                <div class='note'>
                    <div class='note-header'>
                        <div>Note: {item.NoteId}</div>
                        <div>{item.StatusName}</div>
                    </div>
                  
                    <div class='note-body'>
                        {item.ActionDescription}
                    </div> </br></br>");
                
                if (!string.IsNullOrEmpty(item.Enclosure))
                {
                    sb.Append("<div class='enclosure'>Enclosure: Attached</div>");
                }

                sb.Append($@"
    <div class='note-footer'>
        <div>{dt:dd/MM/yyyy hh:mm tt}</div>
        <div><b>{item.ActionBy}</b></div>
    </div>
</div>
");
            }

            sb.Append(@"
</body>
</html>");

            string htmlString = sb.ToString();

            // 5️⃣ Generate PDF using Puppeteer
            byte[] pdfBytes;

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
            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(htmlString, new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Networkidle0 }
                });

                pdfBytes = await page.PdfDataAsync(new PdfOptions
                {
                    Format = PaperFormat.Legal,
                    Landscape = false,
                    PrintBackground = true,
                    DisplayHeaderFooter = true,

                    FooterTemplate = $@"
                    <div style='
                        width:100%;
                        font-size:9px;
                        display:flex;
                        justify-content:space-between;
                        padding:0 10px;'>
                        <div><b>Digital MC House</b></div>
                        <div>
                            Generated on: {DateTime.Now:dd/MM/yyyy hh:mm tt} |
                            Page <span class='pageNumber'></span> of <span class='totalPages'></span>
                        </div>
                    </div>",

                    MarginOptions = new MarginOptions
                    {
                        Top = "15mm",
                        Bottom = "25mm",
                        Left = "15mm",
                        Right = "15mm"
                    }
                });
            }

            // 6️⃣ Return PDF
            string fileName = (from.HasValue && to.HasValue)
                ? $"Noting_{from}_to_{to}.pdf"
                : "Noting_All.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

        public ActionResult GetFileMovementHistory(string refId)
        {
            var data = FileMovementHistoryVM.GetFileMovementHistory(refId);
            if (data == null || data.Count == 0)
                return Content("<tr><td colspan='4' class='text-center'>No history found</td></tr>");
            StringBuilder sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + item.SentFrom + "</td>");
                sb.Append("<td>" + item.SentTo + "</td>");
                sb.Append(
                    "<td>" +
                    item.SentDate.ToString("dd-MMM-yyyy") +
                    "<br/>" +
                    item.SentDate.ToString("hh:mm tt") +
                    "</td>"
                );
                sb.Append("<td class='text-center'>" + item.DelayedDays + "</td>");
                sb.Append("</tr>");
            }

            return Content(sb.ToString());
        }

        //public ActionResult SendTestEmail()
        //{
        //    SendEmailModel model = new SendEmailModel
        //    {
        //        RecipientEmail = "joginder.pathania@gmail.com",
        //        Subject = "Test Email from NIC SMTP",
        //        Body = "<h3>This is a test email</h3>",
        //        Sender = "noreply.dggit@punjab.gov.in"
        //    };

        //    bool result = EmailHelper.SendEmail(model);
        //    if (result)
        //        return Content("Email Sent Successfully ✅");
        //    else
        //        return Content("Email Failed");
        //}

        [HttpPost]
        public JsonResult GenerateReminderEmail(List<ReminderItem> items)
        {
            // Example: Fetch email data from DB
            var refIds = items.Select(x => x.RefId).ToList();

            //var data = db.mmladiary
            //    .Where(x => refIds.Contains(x.RefId))
            //    .Select(x => new
            //    {
            //        x.RefId,
            //        x.ResolutionNo,
            //        x.EmailTo,
            //        x.EmailCC
            //    }).ToList();

            // 🔹 TO & CC (distinct)
            var to = string.Join(";", "pankajmanchanda99@gmail.com");
            var cc = string.Join(";", "pankajmanchanda99@gmail.com");

            // 🔹 Subject (ALL selected refIds)
            var subject = "Reminder Mail: ";

            // 🔹 Body
            string body = "Dear Sir,<br/><br/>";

            body += "This is a reminder for the following cases:<br/><br/>";

            foreach (var item in items)
            {
                body += $"RefId: {item.RefId} | Resolution: {item.resolution}";
                body += "<br/>";
            }

            body += "<br/>Kindly take necessary action.<br/><br/>Thanks";

            return Json(new
            {
                to,
                cc,
                subject,
                body
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendReminderEmail(string to, string cc, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            if (!string.IsNullOrEmpty(cc))
                mail.CC.Add(cc);

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient smtpServer = new SmtpClient();
            smtpServer.Port = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["emailport"]);
            smtpServer.Host = System.Web.Configuration.WebConfigurationManager.AppSettings["emailhost"];
            smtpServer.Credentials = new System.Net.NetworkCredential(System.Web.Configuration.WebConfigurationManager.AppSettings["emailuser"], System.Web.Configuration.WebConfigurationManager.AppSettings["emailpwd"]);
            mail.From = new MailAddress("no-reply.pmidc@punjab.gov.in");
            smtpServer.EnableSsl = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["enablessl"]);
            smtpServer.Send(mail);

            return Json(true);

            //bool result = EmailHelper.SendEmail(model);

            //if (result)
            //    return Content("Email Sent Successfully ✅");
            //else
            //    return Content("Email Failed");
        }

        [HttpGet]
        public JsonResult GetMCNames(string term)
        {
            List<mDepartment> mcList = new List<mDepartment>();

            using (SqlConnection connection = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("USP_GetMCNames", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchText", term ?? "");

                    connection.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        mcList.Add(new mDepartment
                        {
                            deptId = Convert.ToString(dr["MCId"]),
                            deptname = dr["MCName"].ToString()
                        });
                    }
                }
            }

            return Json(mcList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getsubjectandsubjectDetailsAll(Int64 RefId)
        {
            try
            {
                DiaryDispatchModel mdl = new DiaryDispatchModel();
                var userid = Convert.ToString(CurrentSession.UserID);
                var response = mdl.getsubjectandsubjectDetailsAll(RefId);
                var dtt = mdl.GetTableRows(response);
                var jsonResult = Json(dtt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AdvanceResolutioSearch(string type = null)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }

        public ActionResult ObservationReply(string type = null)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }

        public ActionResult ObservationRaised(string type = null)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }

        public ActionResult ObservationReplySearch()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            StringBuilder sb = new StringBuilder();

            try
            {
                DataSet ds = new DataSet();

                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "GetObservationReplyFromConceroned", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@UserId",
                        string.IsNullOrWhiteSpace(CurrentSession.UserID)
                            ? (object)DBNull.Value
                            : CurrentSession.UserID
                    );

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }

                if (ds != null &&
                    ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    // =========================================================
                    // CSS - SAME STYLE AS ObservationRaisedSearch
                    // =========================================================

                    sb.Append(@"
<style>

.obs-table-wrapper {
    width: 100%;
    background: #fff;
    border: 1px solid #aeb8c3;
    border-radius: 10px;
    box-shadow: 0 3px 12px rgba(0,0,0,.06);
    overflow-x: auto;
    overflow-y: hidden;
    -webkit-overflow-scrolling: touch;
}

#ObservationReplySearch {
    width: 100% !important;
    min-width: 1000px;
    margin: 0 !important;
    font-size: 12px;
    border-collapse: collapse !important;
    border: 1px solid #aeb8c3 !important;
}

#ObservationReplySearch thead th {
    background: #1769aa !important;
    color: #fff !important;
    font-size: 11px;
    font-weight: 600;
    padding: 10px 9px !important;
    border: 1px solid #0f5a92 !important;
    white-space: nowrap;
    vertical-align: middle;
    text-align: left;
}

#ObservationReplySearch tbody td {
    padding: 9px 10px !important;
    vertical-align: middle;
    border: 1px solid #c7d0d9 !important;
    color: #374151;
    line-height: 1.45;
    background: #fff;
}

#ObservationReplySearch tbody tr:nth-child(even) td {
    background: #f8fafc;
}

#ObservationReplySearch tbody tr:hover td {
    background: #eaf4ff !important;
    border-color: #aebfd0 !important;
}

.obs-subject {
    min-width: 350px;
    max-width: 600px;
    white-space: normal !important;
    word-break: break-word;
    overflow-wrap: anywhere;
    line-height: 1.5;
}

.obs-resolution {
    color: #1769aa;
    font-weight: 600;
    text-decoration: none;
    cursor: pointer;
    white-space: nowrap;
}

.obs-resolution:hover {
    color: #0b4f86;
    text-decoration: underline;
}

.obs-date {
    white-space: nowrap;
    color: #4b5563;
    font-size: 11px;
}

.obs-table-wrapper .dataTables_wrapper {
    width: 100%;
    padding: 12px;
}

.obs-table-wrapper .dataTables_filter input {
    border: 1px solid #c7d0d9;
    border-radius: 5px;
    padding: 6px 9px;
    font-size: 12px;
    width: 220px;
    max-width: 100%;
    outline: none;
}

.obs-table-wrapper .dataTables_filter input:focus {
    border-color: #1769aa;
    box-shadow: 0 0 0 2px rgba(23,105,170,.10);
}

.obs-table-wrapper .dataTables_length select {
    border: 1px solid #c7d0d9;
    border-radius: 5px;
    padding: 4px 6px;
    font-size: 12px;
}

.obs-table-wrapper .dt-button {
    border: 1px solid #c7d0d9 !important;
    background: #fff !important;
    color: #374151 !important;
    border-radius: 5px !important;
    padding: 5px 10px !important;
    font-size: 11px !important;
    margin-right: 5px !important;
}

.obs-table-wrapper .dt-button:hover {
    background: #f1f5f9 !important;
}

.obs-table-wrapper .dataTables_paginate .paginate_button {
    padding: 4px 9px !important;
    font-size: 11px !important;
    border-radius: 5px !important;
}

@media (max-width: 992px) {

    #ObservationReplySearch {
        min-width: 900px;
    }

    .obs-subject {
        min-width: 300px;
        max-width: 450px;
    }
}

@media (max-width: 768px) {

    .obs-table-wrapper {
        border-radius: 6px;
        overflow-x: auto !important;
    }

    #ObservationReplySearch {
        min-width: 850px;
        font-size: 11px;
    }

    #ObservationReplySearch thead th {
        font-size: 10px;
        padding: 8px 7px !important;
    }

    #ObservationReplySearch tbody td {
        font-size: 11px;
        padding: 8px 7px !important;
    }

    .obs-subject {
        min-width: 280px;
        max-width: 350px;
    }

    .obs-table-wrapper .dataTables_wrapper {
        padding: 8px;
    }

    .obs-table-wrapper .dataTables_filter {
        width: 100%;
        text-align: left !important;
        margin-top: 8px;
    }

    .obs-table-wrapper .dataTables_filter input {
        width: 100%;
    }

    .obs-table-wrapper .dt-buttons {
        display: flex;
        flex-wrap: wrap;
        gap: 5px;
    }

    .obs-table-wrapper .dt-button {
        margin-right: 0 !important;
    }
}

@media (max-width: 480px) {

    #ObservationReplySearch {
        min-width: 800px;
    }

    #ObservationReplySearch thead th {
        font-size: 9px;
        padding: 7px 6px !important;
    }

    #ObservationReplySearch tbody td {
        font-size: 10px;
        padding: 7px 6px !important;
    }

    .obs-subject {
        min-width: 250px;
        max-width: 300px;
    }
}

</style>
");

                    // =========================================================
                    // TABLE WRAPPER
                    // =========================================================

                    sb.Append("<div class='obs-table-wrapper'>");

                    sb.Append(
                        "<table id='ObservationReplySearch' " +
                        "class='table table-hover'>"
                    );

                    // =========================================================
                    // HEADER
                    // =========================================================

                    sb.Append("<thead><tr>");

                    foreach (DataColumn col in dt.Columns)
                    {
                        // Hide Id and IsReply
                        if (col.ColumnName.Equals(
                                "Id",
                                StringComparison.OrdinalIgnoreCase) ||
                            col.ColumnName.Equals(
                                "IsReply",
                                StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        string headerClass = "";

                        if (col.ColumnName.Equals(
                                "Subject",
                                StringComparison.OrdinalIgnoreCase))
                        {
                            headerClass = " class='obs-subject'";
                        }

                        sb.AppendFormat(
                            "<th{0}>{1}</th>",
                            headerClass,
                            HttpUtility.HtmlEncode(col.ColumnName)
                        );
                    }

                    sb.Append("</tr></thead>");

                    // =========================================================
                    // BODY
                    // =========================================================

                    sb.Append("<tbody>");

                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");

                        // =====================================================
                        // GET ID
                        // =====================================================

                        string id = row["Id"] == DBNull.Value
                            ? "0"
                            : Convert.ToString(row["Id"]);

                        string isReply = row["IsReply"] == DBNull.Value
                            ? "0"
                            : Convert.ToString(row["IsReply"]);

                        string jsId =
                            HttpUtility.JavaScriptStringEncode(id);

                        // =====================================================
                        // COLUMNS
                        // =====================================================

                        foreach (DataColumn col in dt.Columns)
                        {
                            // Hide Id and IsReply
                            if (col.ColumnName.Equals(
                                    "Id",
                                    StringComparison.OrdinalIgnoreCase) ||
                                col.ColumnName.Equals(
                                    "IsReply",
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            // =================================================
                            // REPLY FILE
                            // =================================================

                            if (col.ColumnName.Equals(
                                    "Reply File",
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                string fileNo = row[col] == DBNull.Value
                                    ? ""
                                    : Convert.ToString(row[col]);

                                isReply = IsReplyExist(
                                    fileNo,
                                    Convert.ToString(CurrentSession.UserID)
                                );

                                if (isReply == "0")
                                {
                                    string jsFileNo =
                                        HttpUtility.JavaScriptStringEncode(fileNo);

                                    string htmlFileNo =
                                        HttpUtility.HtmlEncode(fileNo);

                                    sb.AppendFormat(
                                        @"<td>
                                    <a id='{0}'
                                       href='javascript:void(0);'
                                       class='obs-resolution'
                                       onclick=""EditMlaDispatch(this, '{1}', 'Diary'); return false;"">
                                        {2}
                                    </a>
                                </td>",
                                        jsFileNo,
                                        jsId,
                                        htmlFileNo
                                    );
                                }
                                else
                                {
                                    sb.AppendFormat(
                                        @"<td>
                                    {0}
                                </td>",
                                        HttpUtility.HtmlEncode(fileNo)
                                    );
                                }
                            }

                            // =================================================
                            // RESOLUTION UNIQUE ID
                            // =================================================

                            else if (col.ColumnName.Equals(
                                        "Resolution Unique ID",
                                        StringComparison.OrdinalIgnoreCase))
                            {
                                string referenceId =
                                    row[col] == DBNull.Value
                                        ? ""
                                        : Convert.ToString(row[col]);

                                string htmlReferenceId =
                                    HttpUtility.HtmlEncode(referenceId);

                                sb.AppendFormat(
                                    @"<td>
                                <a href='javascript:void(0);'
                                   class='obs-resolution'
                                   id='{0}'
                                   onclick=""ViewSingleEdit(this); return false;"">
                                    {1}
                                </a>

                                <input type='hidden'
                                       id='hfRefId_{0}'
                                       value='{0}' />
                            </td>",
                                    HttpUtility.HtmlAttributeEncode(referenceId),
                                    htmlReferenceId
                                );
                            }

                            // =================================================
                            // REPLY DATE
                            // =================================================

                            else if (col.ColumnName.Equals(
                                        "Reply Date",
                                        StringComparison.OrdinalIgnoreCase))
                            {
                                string replyDate = "";

                                if (row[col] != DBNull.Value)
                                {
                                    DateTime dateValue;

                                    if (DateTime.TryParse(
                                            Convert.ToString(row[col]),
                                            out dateValue))
                                    {
                                        replyDate =
                                            dateValue.ToString("dd-MMM-yyyy");
                                    }
                                }

                                sb.AppendFormat(
                                    "<td class='obs-date'>{0}</td>",
                                    HttpUtility.HtmlEncode(replyDate)
                                );
                            }

                            // =================================================
                            // SUBJECT
                            // =================================================

                            else if (col.ColumnName.Equals(
                                        "Subject",
                                        StringComparison.OrdinalIgnoreCase))
                            {
                                string subject =
                                    row[col] == DBNull.Value
                                        ? ""
                                        : Convert.ToString(row[col]);

                                sb.AppendFormat(
                                    "<td class='obs-subject'>{0}</td>",
                                    HttpUtility.HtmlEncode(subject)
                                );
                            }

                            // =================================================
                            // OTHER COLUMNS
                            // =================================================

                            else
                            {
                                string value =
                                    row[col] == DBNull.Value
                                        ? ""
                                        : Convert.ToString(row[col]);

                                sb.AppendFormat(
                                    "<td>{0}</td>",
                                    HttpUtility.HtmlEncode(value)
                                );
                            }
                        }

                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                }
                else
                {
                    // =========================================================
                    // NO RECORDS
                    // =========================================================

                    sb.Append(@"
                <div class='alert alert-warning text-center'
                     style='margin-top:15px;
                            border-radius:8px;
                            border:1px solid #e5c46a;'>
                    <i class='fa fa-info-circle'></i>
                    &nbsp; No Observation Reply records found.
                </div>");
                }
            }
            catch (Exception ex)
            {
                // =============================================================
                // ERROR
                // =============================================================

                sb.Clear();

                sb.AppendFormat(
                    @"<div class='alert alert-danger'
                     style='margin-top:15px;
                            border-radius:8px;
                            border:1px solid #e0a4a4;'>
                <i class='fa fa-exclamation-triangle'></i>
                &nbsp; {0}
              </div>",
                    HttpUtility.HtmlEncode(ex.Message)
                );
            }

            return Content(sb.ToString(), "text/html");
        }


        public string IsReplyExist(string refId, string userId)
        {
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand("GetDiaryMovementByRefId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RefId", refId);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    if (con.State != ConnectionState.Open)
                        con.Open();

                    return Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch
            {
                return "1";
            }
        }

        public ActionResult ObservationRaisedSearch()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            try
            {
                DataSet ds = new DataSet();

                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand("GetObservationRaised", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@UserId",
                        string.IsNullOrWhiteSpace(CurrentSession.UserID)
                            ? (object)DBNull.Value
                            : CurrentSession.UserID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }

                if (ds != null &&
                    ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    sb.Append(@"
<style>

.obs-table-wrapper {
    width: 100%;
    background: #fff;
    border: 1px solid #aeb8c3;
    border-radius: 10px;
    box-shadow: 0 3px 12px rgba(0,0,0,.06);
    overflow-x: auto;
    overflow-y: hidden;
    -webkit-overflow-scrolling: touch;
}

#ObservationRaisedSearch {
    width: 100% !important;
    min-width: 1000px;
    margin: 0 !important;
    font-size: 12px;
    border-collapse: collapse !important;
    border: 1px solid #aeb8c3 !important;
}

#ObservationRaisedSearch thead th {
    background: #1769aa !important;
    color: #fff !important;
    font-size: 11px;
    font-weight: 600;
    padding: 10px 9px !important;
    border: 1px solid #0f5a92 !important;
    white-space: nowrap;
    vertical-align: middle;
    text-align: left;
}

#ObservationRaisedSearch tbody td {
    padding: 9px 10px !important;
    vertical-align: middle;
    border: 1px solid #c7d0d9 !important;
    color: #374151;
    line-height: 1.45;
    background: #fff;
}

#ObservationRaisedSearch tbody tr:nth-child(even) td {
    background: #f8fafc;
}

#ObservationRaisedSearch tbody tr:hover td {
    background: #eaf4ff !important;
    border-color: #aebfd0 !important;
}

.obs-subject {
    min-width: 350px;
    max-width: 600px;
    white-space: normal !important;
    word-break: break-word;
    overflow-wrap: anywhere;
    line-height: 1.5;
}

.obs-resolution {
    color: #1769aa;
    font-weight: 600;
    text-decoration: none;
    cursor: pointer;
    white-space: nowrap;
}

.obs-resolution:hover {
    color: #0b4f86;
    text-decoration: underline;
}

.obs-date {
    white-space: nowrap;
    color: #4b5563;
    font-size: 11px;
}

.obs-table-wrapper .dataTables_wrapper {
    width: 100%;
    padding: 12px;
}

.obs-table-wrapper .dataTables_filter input {
    border: 1px solid #c7d0d9;
    border-radius: 5px;
    padding: 6px 9px;
    font-size: 12px;
    width: 220px;
    max-width: 100%;
    outline: none;
}

.obs-table-wrapper .dataTables_filter input:focus {
    border-color: #1769aa;
    box-shadow: 0 0 0 2px rgba(23,105,170,.10);
}

.obs-table-wrapper .dataTables_length select {
    border: 1px solid #c7d0d9;
    border-radius: 5px;
    padding: 4px 6px;
    font-size: 12px;
}

.obs-table-wrapper .dt-button {
    border: 1px solid #c7d0d9 !important;
    background: #fff !important;
    color: #374151 !important;
    border-radius: 5px !important;
    padding: 5px 10px !important;
    font-size: 11px !important;
    margin-right: 5px !important;
}

.obs-table-wrapper .dt-button:hover {
    background: #f1f5f9 !important;
}

.obs-table-wrapper .dataTables_paginate .paginate_button {
    padding: 4px 9px !important;
    font-size: 11px !important;
    border-radius: 5px !important;
}

@media (max-width: 992px) {

    #ObservationRaisedSearch {
        min-width: 900px;
    }

    .obs-subject {
        min-width: 300px;
        max-width: 450px;
    }
}

@media (max-width: 768px) {

    .obs-table-wrapper {
        border-radius: 6px;
        overflow-x: auto !important;
    }

    #ObservationRaisedSearch {
        min-width: 850px;
        font-size: 11px;
    }

    #ObservationRaisedSearch thead th {
        font-size: 10px;
        padding: 8px 7px !important;
    }

    #ObservationRaisedSearch tbody td {
        font-size: 11px;
        padding: 8px 7px !important;
    }

    .obs-subject {
        min-width: 280px;
        max-width: 350px;
    }

    .obs-table-wrapper .dataTables_wrapper {
        padding: 8px;
    }

    .obs-table-wrapper .dataTables_filter {
        width: 100%;
        text-align: left !important;
        margin-top: 8px;
    }

    .obs-table-wrapper .dataTables_filter input {
        width: 100%;
    }

    .obs-table-wrapper .dt-buttons {
        display: flex;
        flex-wrap: wrap;
        gap: 5px;
    }

    .obs-table-wrapper .dt-button {
        margin-right: 0 !important;
    }
}

@media (max-width: 480px) {

    #ObservationRaisedSearch {
        min-width: 800px;
    }

    #ObservationRaisedSearch thead th {
        font-size: 9px;
        padding: 7px 6px !important;
    }

    #ObservationRaisedSearch tbody td {
        font-size: 10px;
        padding: 7px 6px !important;
    }

    .obs-subject {
        min-width: 250px;
        max-width: 300px;
    }
}

</style>
");

                    sb.Append("<div class='obs-table-wrapper'>");

                    sb.Append(
                        "<table id='ObservationRaisedSearch' " +
                        "class='table table-hover'>");

                    // ================= HEADER =================

                    sb.Append("<thead><tr>");

                    foreach (DataColumn col in dt.Columns)
                    {
                        // Hide these columns
                        if (col.ColumnName.Equals(
                                "NotifyTo",
                                StringComparison.OrdinalIgnoreCase) ||
                            col.ColumnName.Equals(
                                "Reply File",
                                StringComparison.OrdinalIgnoreCase) ||
                            col.ColumnName.Equals(
                                "ActionCode",
                                StringComparison.OrdinalIgnoreCase))
                            continue;

                        string headerClass = "";

                        if (col.ColumnName.Equals(
                            "Subject",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            headerClass = " class='obs-subject'";
                        }

                        sb.AppendFormat(
                            "<th{0}>{1}</th>",
                            headerClass,
                            HttpUtility.HtmlEncode(col.ColumnName));
                    }

                    sb.Append("</tr></thead>");

                    // ================= BODY =================

                    sb.Append("<tbody>");

                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");

                        foreach (DataColumn col in dt.Columns)
                        {
                            // Hide NotifyTo, Reply File and ActionCode
                            if (col.ColumnName.Equals(
                                    "NotifyTo",
                                    StringComparison.OrdinalIgnoreCase) ||
                                col.ColumnName.Equals(
                                    "Reply File",
                                    StringComparison.OrdinalIgnoreCase) ||
                                col.ColumnName.Equals(
                                    "ActionCode",
                                    StringComparison.OrdinalIgnoreCase))
                                continue;

                            // ================= RESOLUTION ID =================

                            if (col.ColumnName.Equals(
                                "Resolution Unique ID",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                string referenceId =
                                    row[col] == DBNull.Value
                                        ? ""
                                        : row[col].ToString();

                                if (CurrentSession.DeptID == "LG00001")
                                {
                                    sb.AppendFormat(
                                        @"<td>
                                    <a href='javascript:void(0);'
                                       class='obs-resolution'
                                       id='{0}'
                                       onclick=""ViewSingleEdit(this); return false;"">
                                        {1}
                                    </a>
                                    <input type='hidden'
                                           id='hfRefId_{0}'
                                           value='{0}' />
                                  </td>",
                                        HttpUtility.HtmlAttributeEncode(referenceId),
                                        HttpUtility.HtmlEncode(referenceId));
                                }
                                else
                                {
                                    sb.AppendFormat(
                                        @"<td>
                                    <a href='javascript:void(0);'
                                       class='obs-resolution'
                                       id='{0}'
                                       onclick=""EditMlaDispatch(this, 0, 'Dispatch'); return false;"">
                                        {1}
                                    </a>
                                    <input type='hidden'
                                           id='hfRefId_{0}'
                                           value='{0}' />
                                  </td>",
                                        HttpUtility.HtmlAttributeEncode(referenceId),
                                        HttpUtility.HtmlEncode(referenceId));
                                }
                            }

                            // ================= MEETING DATE =================

                            else if (col.ColumnName.Equals(
                                "Meeting Date",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                string meetingDate = "";

                                if (row[col] != DBNull.Value)
                                {
                                    meetingDate =
                                        Convert.ToDateTime(row[col])
                                            .ToString("dd-MMM-yyyy");
                                }

                                sb.AppendFormat(
                                    "<td class='obs-date'>{0}</td>",
                                    HttpUtility.HtmlEncode(meetingDate));
                            }

                            // ================= RAISED DATE =================

                            else if (col.ColumnName.Equals(
                                "Raised Date",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                string raisedDate = "";

                                if (row[col] != DBNull.Value)
                                {
                                    raisedDate =
                                        Convert.ToDateTime(row[col])
                                            .ToString("dd-MMM-yyyy hh:mm tt");
                                }

                                sb.AppendFormat(
                                    "<td class='obs-date'>{0}</td>",
                                    HttpUtility.HtmlEncode(raisedDate));
                            }

                            // ================= SUBJECT =================

                            else if (col.ColumnName.Equals(
                                "Subject",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                string subject =
                                    row[col] == DBNull.Value
                                        ? ""
                                        : row[col].ToString();

                                sb.AppendFormat(
                                    "<td class='obs-subject'>{0}</td>",
                                    HttpUtility.HtmlEncode(subject));
                            }

                            // ================= OBSERVATION TYPE =================

                            else if (col.ColumnName.Equals(
                                "Observation Type",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                string observationType =
                                    row[col] == DBNull.Value
                                        ? ""
                                        : row[col].ToString();

                                // HTML is already generated by SQL.
                                // Do NOT HtmlEncode it.
                                sb.AppendFormat(
                                    "<td style='text-align:center;'>{0}</td>",
                                    observationType);
                            }

                            // ================= OTHER COLUMNS =================

                            else
                            {
                                string value =
                                    row[col] == DBNull.Value
                                        ? ""
                                        : row[col].ToString();

                                sb.AppendFormat(
                                    "<td>{0}</td>",
                                    HttpUtility.HtmlEncode(value));
                            }
                        }

                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("</div>");
                }
                else
                {
                    sb.Append(@"
                <div class='alert alert-warning text-center'
                     style='margin-top:15px;
                            border-radius:8px;
                            border:1px solid #e5c46a;'>
                    <i class='fa fa-info-circle'></i>
                    &nbsp; No Observation Raised records found.
                </div>");
                }
            }
            catch (Exception ex)
            {
                sb.Clear();

                sb.AppendFormat(
                    @"<div class='alert alert-danger'
              style='margin-top:15px;
                     border-radius:8px;
                     border:1px solid #e0a4a4;'>
                <i class='fa fa-exclamation-triangle'></i>
                &nbsp; {0}
              </div>",
                    HttpUtility.HtmlEncode(ex.Message));
            }

            return Content(sb.ToString(), "text/html");
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveDraft(UserDraft model)
        {
            bool result = model.SaveDraft();

            return Json(new
            {
                success = result
            });
        }

        public JsonResult GetDraft(string draftKey)
        {
            UserDraft repo = new UserDraft();

            var draft = repo.GetDraft(CurrentSession.UserID, draftKey);

            return Json(new
            {
                DraftContent = draft
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDraft(string draftKey)
        {
            try
            {
                UserDraft draft = new UserDraft();

                draft.DeleteDraft(CurrentSession.UserID, draftKey);

                return Json(new { success = true });
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

        [HttpPost]
        public JsonResult DeleteDraftDirect(string draftKey, List<string> data)
        {
            try
            {
                UserDraft draft = new UserDraft();
                var res = draft.GetDraft(CurrentSession.UserID, draftKey);

                JObject draftObj = JObject.Parse(res);
                List<string> draftLs = draftObj.Properties().Select(el => el.Name).ToList();

                var set1 = new HashSet<string>(data);
                var result = new List<string>();

                foreach (var item in draftLs)
                {
                    if (set1.Contains(item))
                    {
                        draftObj.Remove(item);
                    }
                }

                if (draftObj.Count > 0)
                {
                    return SaveDraft(new UserDraft()
                    {
                        UserId = CurrentSession.UserID,
                        DraftKey = draftKey,
                        DraftContent = draftObj.ToString(Formatting.None)
                    });
                }
                else
                {
                    draft.DeleteDraftDirect(CurrentSession.UserID, draftKey);
                    return Json(new { success = true });
                }
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

        public void UnReadDeleteDraft(string userId, string draftKey)
        {
            UserDraft draft = new UserDraft();
            draft.UnReadDeleteDraft(userId, draftKey);
        }



        [HttpGet]
        public JsonResult GetDataForDispatchInboxFilter(string DocumentType, string isLetter, string RefId, string MCID, string ResolutionNo, DateTime? MeetingDate = null)
        {
            try
            {
                var response = "";
                DiaryViewModel dvm = new DiaryViewModel();
                response = dvm.GetDocumentName(DocumentType);
                Session["DocumentTypeName"] = "";
                Session["DocumentTypeName"] = response;
                DataTable data = new DataTable();
                data = dvm.DispatchInboxFilter(Convert.ToString(CurrentSession.UserID), isLetter, Convert.ToInt32(CurrentSession.OfficeId), Convert.ToInt32(CurrentSession.MapId), DocumentType, MCID, MeetingDate, ResolutionNo);
                var refIds = data.AsEnumerable().Where(r => r["IsRead"] == DBNull.Value).Select(r => r["RefId"].ToString()).ToList();

                if (refIds.Any())
                {
                    UnReadDeleteDraft(Convert.ToString(CurrentSession.UserID), string.Join(",", refIds));
                }

                var dtt = dvm.GetTableRows(data);
                return new JsonResult
                {
                    Data = dtt,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

       [HttpGet]
        public JsonResult GetPartFileResolution(long? refid = 0, string isLetter = "File", int AssignedOfficeId = 0, int SessToOfficeId = 0, string Type = "")
        {
            try
            {
                var list = new List<Dictionary<string, object>>();
                long targetRefId = refid ?? 0;

                string userId = CurrentSession.UserID != null ? CurrentSession.UserID.ToString() : "";
                if (AssignedOfficeId == 0 && CurrentSession.OfficeId != null)
                {
                    int.TryParse(CurrentSession.OfficeId.ToString(), out AssignedOfficeId);
                }

                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand("GetPartFileResolution", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userid", string.IsNullOrWhiteSpace(userId) ? (object)DBNull.Value : userId);
                    cmd.Parameters.AddWithValue("@isLetter", string.IsNullOrWhiteSpace(isLetter) ? (object)DBNull.Value : isLetter);
                    cmd.Parameters.AddWithValue("@AssignedOfficeId", AssignedOfficeId);
                    cmd.Parameters.AddWithValue("@SessToOfficeId", SessToOfficeId);
                    cmd.Parameters.AddWithValue("@Type", Type ?? "");
                    cmd.Parameters.AddWithValue("@refid", targetRefId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        foreach (DataRow row in dt.Rows)
                        {
                            var dict = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                            }
                            list.Add(dict);
                        }
                    }
                }

                return new JsonResult
                {
                    Data = list,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { success = false, message = ex.Message },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public JsonResult NotifyLocalGovt(long RefId, int NotifyTo)
        {
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                 {
                    using (SqlCommand cmd = new SqlCommand(@"
                        UPDATE mNotification
                        SET NotifyTo = @NotifyTo
                        WHERE RefId = @RefId", con))
                            {
                        cmd.Parameters.AddWithValue("@RefId", RefId);
                        cmd.Parameters.AddWithValue("@NotifyTo", NotifyTo);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        return Json(new
                        {
                            success = rowsAffected > 0,
                            message = rowsAffected > 0
                                ? "Notify status updated successfully."
                                : "Record not found."
                        });
                    }
                }
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


        [HttpPost]
        public JsonResult NotifyLocalGovtMutiple(string RefId, int NotifyTo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RefId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "RefId is required."
                    });
                }

                // Split comma-separated RefIds
                var refIds = RefId
                    .Split(',')
                    .Select(x =>
                    {
                        long id;
                        return long.TryParse(x.Trim(), out id) ? (long?)id : null;
                    })
                    .Where(x => x.HasValue)
                    .Select(x => x.Value)
                    .Distinct()
                    .ToList();

                if (refIds.Count == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid RefId."
                    });
                }

                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    var parameters = new List<string>();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;

                        for (int i = 0; i < refIds.Count; i++)
                        {
                            string parameterName = "@RefId" + i;

                            parameters.Add(parameterName);

                            cmd.Parameters.Add(parameterName, SqlDbType.BigInt)
                                .Value = refIds[i];
                        }

                        cmd.Parameters.Add("@NotifyTo", SqlDbType.Int)
                            .Value = NotifyTo;

                        cmd.CommandText = @"
                        UPDATE mNotification
                        SET NotifyTo = @NotifyTo
                        WHERE RefId IN (" + string.Join(",", parameters) + @")
                        AND NotificationType = 7";

                        con.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return Json(new
                        {
                            success = rowsAffected > 0,
                            message = rowsAffected > 0
                                ? "Notify status updated successfully."
                                : "Record not found."
                        });
                    }
                }
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
        public JsonResult GetPartFileDetails(string RefId, string ResRefID)
        {
            try
            {
                var list = new List<object>();

                using (SqlConnection con = ClsConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand("GetPartFileDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RefId", RefId);
                    cmd.Parameters.AddWithValue("@ResRefID", ResRefID);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new
                            {
                                PartFileId = dr["PartFileId"].ToString(),
                                IsReply = dr["IsReply"].ToString(),
                                CurrentlyWith = dr["CurrentlyWith"].ToString()
                            });
                        }
                    }
                }

                return Json(new
                {
                    success = true,
                    data = list
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}





public class ReminderItem
{
    public long RefId { get; set; }
    public string resolution { get; set; }
}
