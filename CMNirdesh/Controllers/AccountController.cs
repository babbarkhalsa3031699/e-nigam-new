using CMNirdesh.DTO;
using CMNirdesh.Extensions;
using CMNirdesh.Filters;
using CMNirdesh.Models;
//using DocumentFormat.OpenXml.EMMA;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.Security.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMNirdesh.Controllers
{
    public class AccountController : Controller
    {


        //public ActionResult Index(String SiteURL, String MCId, String Logo)
        //{

        //    Console.WriteLine(SiteURL);

        //    return View();
        //}





        [HttpPost]
        public ActionResult GetGeneratedSalt()
        {
            try
            {
                //var Salt = (string)Helper.ExecuteService("User", "GetGeneratedSalt", null);
                var Salt = "saltGenerated";
                return Json(Salt, JsonRequestBehavior.AllowGet);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                throw;
            }

        }
        [CMNirdeshAuthorize(Allow = "All")]
        [NoCache]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {


                // var url = HttpContext.Request.Url.Host;
                // var url = "https://mcmohali.punjab.gov.in";
                LoginModel objLoginModel = new LoginModel();

                //  var MC = LoginModel.GetMCId(url.ToString());

                // objLoginModel.MCName = MC.MCName;
                //  objLoginModel.MCLogo = MC.MCLogo;
                //  objLoginModel.SiteUrl = MC.SiteUrl;
                //if (TempData["Register"] != null)
                //{
                //    TempData["Rg"] = TempData["Register"];
                //}
                var x = CurrentSession.Islogged;


                Session["LoggedIn"] = "no";
                Session["HomeButton"] = "yes";

                Session.Abandon();//by ashwani


                if (returnUrl == null)
                {
                    ViewBag.ReturnUrl = string.Empty;
                }
                else
                {
                    ViewBag.ReturnUrl = returnUrl;
                }
                if (HttpContext.Request.Cookies["userAadharID"] != null)
                {
                    objLoginModel.RememberMe = true;
                    try
                    {
                        string str = Decrypt(HttpContext.Request.Cookies["userAadharID"].Value);
                        objLoginModel.UserName = str.Substring(0, str.Length - 5);
                    }
                    catch { }
                }
                else
                {
                    objLoginModel.RememberMe = false;
                }
                HttpCookie reqCookies = Request.Cookies["Login"];
                if (reqCookies != null)
                {
                    LoginModel model = new LoginModel();
                    var User_name = reqCookies["Password"].ToString();
                    var Password = reqCookies["Name"].ToString();
                    Session["CaptchaImageText"] = "CookieCaptcha";
#pragma warning disable CS0219 // The variable 'CaptchaText' is assigned but its value is never used
                    string CaptchaText = "CookieCaptcha";
#pragma warning restore CS0219 // The variable 'CaptchaText' is assigned but its value is never used
                    model.UserName = User_name;
                    model.ActualPassword = Password;

                    //Login(model, false, CaptchaText);
                    return View("Login", model);
                }
                else
                {
                    return View("Login", objLoginModel);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, "Error at Login");
                throw ex;
            }
        }


        public ActionResult GetCaptchaImage()
        {
            string captchaText = GenerateCaptchaText(6);
            Session["CaptchaImageText"] = captchaText;

            using (var bitmap = new Bitmap(280, 80))
            using (var graphics = Graphics.FromImage(bitmap))
            using (var stream = new MemoryStream())
            {
                graphics.Clear(Color.White);

                // Smooth text rendering
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                // Add light noise lines
                using (var pen = new Pen(Color.LightGray))
                {
                    Random rand = new Random();
                    for (int i = 0; i < 8; i++)
                    {
                        graphics.DrawLine(pen,
                            rand.Next(bitmap.Width), rand.Next(bitmap.Height),
                            rand.Next(bitmap.Width), rand.Next(bitmap.Height));
                    }
                }

                // Draw captcha text
                using (System.Drawing.Font font = new System.Drawing.Font("Arial", 32, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.FromArgb(30, 30, 30)))
                {
                    graphics.DrawString(captchaText, font, brush, 20, 15);
                }

                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;

                // Prevent caching
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

                return File(stream.ToArray(), "image/png");
            }
        }


        private string GenerateCaptchaText(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);

                return new string(data.Select(b => chars[b % chars.Length]).ToArray());
            }
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
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string urlHs = "http://localhost:8083/handshake" + "/" + encrString + "/" + "CMNirdesh";
                using (HttpClient client = new HttpClient())
                {

                    client.Timeout = TimeSpan.FromSeconds(30);
                    String resposedata = "";
                    var responseTask = client.GetAsync(urlHs).Result;
                    Console.WriteLine("responseTask :" + responseTask);
                    if (responseTask.StatusCode == HttpStatusCode.OK)
                    {
                        try
                        {
                            resposedata = responseTask.Content.ReadAsStringAsync().Result;

                            return resposedata;
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


        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        [CMNirdeshAuthorize(Allow = "All")]
        public ActionResult RedirectedLogin(string returnUrl)
        {
            returnUrl = Sanitizer.GetSafeHtmlFragment(returnUrl);
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public static string RemoveSalt(string str)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 1; i < str.Length; i += 2)
                {
                    sb.Append(str[i]);
                }
                string strNew = sb.ToString();
                StringBuilder sb1 = new StringBuilder();
                for (int i = 0; i < strNew.Length; i += 2)
                {
                    int value = Convert.ToInt32(strNew.Substring(i, 2), 16);
                    sb1.Append(Char.ConvertFromUtf32(value));
                }
                string str64 = sb1.ToString();
                byte[] hash = Convert.FromBase64String(str64);
                string getHash = System.Text.Encoding.Default.GetString(hash);
                return getHash;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
            }
            return "";
        }

        [HttpPost]
        [CMNirdeshAuthorize(Allow = "All")]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AllowAnonymous]//this is for Antiforgery token


        public string loginPageredirect()
        {
            string response = "";
            DiaryViewModel dvm = new DiaryViewModel();

            response = dvm.loginPageCheckredirection(CurrentSession.UserID.ToString()); //loginPageCheckredirection UpdateDiaryDocument

            return response;
        }

        public ActionResult LoginCMNirdesh(LoginModel model, bool saveUserName = false, string CaptchaText="")
        {
            //string newpwd=forgetUserPassword(model);
            Session["LoggedIn"] = "no";

            string returnUrl = model.returnUrl;
            CurrentSession.ClearSessions();
            var data = false;
            //bool IsPasswordAthenticated = false;
            Int64 n;
            bool isNumeric = Int64.TryParse(model.UserName, out n);
            try
            {
                if (Session["CaptchaImageText"] != null)
                {
                    string sessionCaptcha = (this.Session["CaptchaImageText"] ?? "").ToString().Trim().ToUpper();
                    string userCaptcha = (CaptchaText ?? "").Trim().ToUpper();
                    if (sessionCaptcha != userCaptcha)
                    {
                        model.ErrorMessage = "Captcha Validation Failed!";
                        return View("Login", model);
                    }
                }

                bool isOnlyLower = model.UserName.Any(c => Char.IsUpper(c));
                if (isOnlyLower)
                {
                    model.UserName = null;
                    model.ErrorMessage = "User Name should be Lower Case";
                    return View("Login", model);

                }

                string hashedPassword = model.ActualPassword.Substring(0, 64);
                string pwd = hashedPassword;

                string actualPass = pwd;
                // string actualPass = RemoveSalt(model.ActualPassword);
                string input = model.UserName;
                string sub = input.Substring(0, 1);
                string subUser = "";
                if (input.Length >= 5)
                {
                    subUser = input.Substring(0, 5);
                }
                else
                {
                    subUser = input;
                }
                string subUsercon = "";
                if (input.Length >= 3)
                {
                    subUsercon = input.Substring(0, 3);
                }
                else
                {
                    subUsercon = input;
                }
                //if (subUsercon == "HPD" || subUsercon == "MLA" || subUsercon == "hpd" || subUsercon == "MLA" || subUsercon == "ADM")
                //{
                #region login through Adhaar ID
                // string correctPassword = RemoveSalt(model.ActualPassword);
                string correctPassword = pwd;
                mUsers user = new mUsers();
                user.AadarId = model.UserName;
                CurrentSession.AadharId = user.AadarId;
                user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(model.UserName);
                if (user.mUserList != null && user.mUserList.Count > 0)
                {
                    Console.WriteLine(user.mUserList);
                    foreach (var list in user.mUserList)
                    {
                        //string a= CMNirdesh.Models.Login.ComputeHash("Test@123#", "SHA1",null);

                        if (CMNirdesh.Models.Login.VerifyHash(correctPassword, "SHA256", list.Password) == true)
                        {
                            var result = CMNirdesh.Models.Login.WrongAttemptLoginData();
                            if (result.IsValid)
                            {
                                if (saveUserName)
                                {
                                    HttpCookie cookie = new HttpCookie("userAadharID");
                                    cookie.Value = Encrypt(user.AadarId + Guid.NewGuid().ToString().Substring(0, 5));
                                    cookie.HttpOnly = true;
                                    cookie.Expires = DateTime.Now.AddDays(-1);
                                    HttpContext.Response.Cookies.Remove("userAadharID");
                                    HttpContext.Response.SetCookie(cookie);
                                }
                                else
                                {
                                    if (Request.Cookies["userAadharID"] != null)
                                    {
                                        var c = new HttpCookie("userAadharID");
                                        c.Expires = DateTime.Now.AddDays(-1);
                                        c.HttpOnly = true;
                                        Response.Cookies.Add(c);
                                    }
                                }
                                // set status session var as successful
                                CurrentSession.LoginStatus = "Successful";
                                // Avoid Session Hijacking
                                string _browserInfo = Request.Browser.Browser
                                + Request.Browser.Version
                                + Request.UserAgent + "~"
                                + Request.ServerVariables["REMOTE_ADDR"];
                                string _sessionValue = Convert.ToString(Session["UserId"]) + "^"
                                                        + DateTime.Now.Ticks + "^"
                                                        + _browserInfo + "^"
                                                        + System.Guid.NewGuid();
                                byte[] _encodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_sessionValue);
                                string _newEncryptedString = System.Convert.ToBase64String(_encodeAsBytes);
                                if (Request.Cookies["AuthToken"] != null)
                                {
                                    var c = new HttpCookie("AuthToken");
                                    c.Value = _newEncryptedString;
                                    c.Expires = DateTime.Now.AddDays(-1);
                                    if (!Request.IsLocal)
                                    {
                                        c.Secure = true;
                                    }
                                    Response.SetCookie(c);
                                }
                                else
                                {
                                    var c = new HttpCookie("AuthToken");
                                    c.Value = _newEncryptedString;
                                    if (!Request.IsLocal)
                                    {
                                        c.Secure = true;
                                    }
                                    c.Expires = DateTime.Now.AddDays(-1);
                                    Response.Cookies.Add(c);
                                }
                                Session["encryptedSession"] = _newEncryptedString;
                                model.UserName = list.UserName;
                                FormsAuthentication.SetAuthCookie(list.UserName, false);
                                CurrentSession.UserID = list.UserId.ToString();
                                ActiveSessionManager.RegisterSession(CurrentSession.UserID);
                                CurrentSession.UserName = list.UserName;
                                //CustomCryptoService.EncryptString(model.Name.Trim());
                                CurrentSession.loginEmailID =  list.EmailId;
                                //CurrentSession.loginEmailID = CustomCryptoService.DecryptString(list.EmailId);
                                CurrentSession.SubUserTypeID = list.UserType.ToString();
                                CurrentSession.IsMember = list.IsMember;
                                CurrentSession.OfficeId = list.OfficeId.ToString();
                                CurrentSession.MCName = list.MCName.ToString();
                                CurrentSession.OfficeLevel = list.OfficeLevel;
                                CurrentSession.StateId = list.StateId.ToString();
                                CurrentSession.parentYN = list.isParent;
                                CurrentSession.isDeptApex = list.isDeptApex.ToString();
                                CurrentSession.DesignationLocal = list.DesignationLocal;
                                CurrentSession.MemberMobile = list.MobileNo;
                                //CurrentSession.MemberMobile = CustomCryptoService.DecryptString(list.MobileNo);
                                // CurrentSession.DeptName = list.DepartmentName;
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


                                if (list.DeptId != null && list.DeptId != "")
                                {
                                    CurrentSession.DeptID = Convert.ToString(list.DeptId);
                                }
                                if (list.SubdivisionCode != null && list.SubdivisionCode != "")
                                {
                                    CurrentSession.SubDivisionId = Convert.ToString(list.SubdivisionCode);
                                }
                                SetSessions();
                                if (list.AadarId.Contains("MMC"))
                                {
                                    CurrentSession.ConstituencyID = Convert.ToString(list.UserTypeConstituencyId);
                                }
                                else
                                {

                                }


                                if (CurrentSession.DeptID.Contains("LG") || CurrentSession.DeptID.Contains("UDD"))
                                {
                                    data = true;
                                    var res = Json(data, JsonRequestBehavior.AllowGet);
                                    Session["LoggedIn"] = "yes";
                                    if (list.IsPasswordChanged != true)
                                    {
                                        return RedirectToAction("UpdatePassword", "Account", new { @type = 7 });
                                    }
                                    return RedirectToAction("Dashboard", "References", new { @type = 7 });
                                }
                                else
                                {
                                    data = true;
                                    var res = Json(data, JsonRequestBehavior.AllowGet);
                                    //return res; return PartialView("/Views/References/_dashboardType.cshtml");


                                    Session["LoggedIn"] = "yes";
                                    //return RedirectToAction("Dashboard", "References", new { @type =3,@menu=0,@MenuName="Cu Questions" });
                                    if (list.IsPasswordChanged != true)
                                    {
                                        return RedirectToAction("UpdatePassword", "Account", new { @type = 0 });
                                    }

                                    return RedirectToAction("Dashboard", "References", new { @type = 0 });

                                    // return RedirectToAction("Dashboard", "References");


                                }
                            }
                            else
                            {
                                FormsAuthentication.SignOut();
                                CurrentSession.ClearSessions();
                            }
                        }
                        else
                        {
                            FormsAuthentication.SignOut();
                            CurrentSession.ClearSessions();
                        }
                    }
                }
                else
                {
                    FormsAuthentication.SignOut();
                    CurrentSession.ClearSessions();
                }

                TempData["ChngPwdSucessMsg"] = "Username or Password incorrect";
                #endregion
                //                }
                FormsAuthentication.SignOut();
                CurrentSession.ClearSessions();
                return View("Login", model);
            }

#pragma warning disable CS0168 // The variable 'exception' is declared but never used
            catch (Exception exception)
#pragma warning restore CS0168 // The variable 'exception' is declared but never used
            {
                TempData["ChngPwdSucessMsg"] = "Username or Password incorrect";
                ViewBag.HasError = true;
            }

            var userid = CurrentSession.UserID;

            UserModel obj = new UserModel(); // Create an instance of MyClass
            obj.AddLoginUser(userid);
            //  SBL.eLegistrator.HouseController.Web.Utility.CurrentSession.UpdateMenuandModules();
            data = false;

            var res2 = Json(data, JsonRequestBehavior.AllowGet);
            return res2;
        }



        public ActionResult LoginMobile()
        {
            Session["LoggedIn"] = "no";

            bool saveUserName = true;
            string ActualPassword = "";

            var username = Request.QueryString["Username"];
            var password = Request.QueryString["password"];
            ActualPassword = password;
            var data = false;
            //bool IsPasswordAthenticated = false;
            Int64 n;
            bool isNumeric = Int64.TryParse(username, out n);
            try
            {

                bool isOnlyLower = username.Any(c => Char.IsUpper(c));




                string hashedPassword = ActualPassword.Substring(0, 64);
                string pwd = hashedPassword;

                string actualPass = pwd;
                // string actualPass = RemoveSalt(model.ActualPassword);
                string input = username;
                string sub = input.Substring(0, 1);
                string subUser = "";
                if (input.Length >= 5)
                {
                    subUser = input.Substring(0, 5);
                }
                else
                {
                    subUser = input;
                }
                string subUsercon = "";
                if (input.Length >= 3)
                {
                    subUsercon = input.Substring(0, 3);
                }
                else
                {
                    subUsercon = input;
                }
                //if (subUsercon == "HPD" || subUsercon == "MLA" || subUsercon == "hpd" || subUsercon == "MLA" || subUsercon == "ADM")
                //{
                #region login through Adhaar ID
                // string correctPassword = RemoveSalt(model.ActualPassword);
                string correctPassword = pwd;
                mUsers user = new mUsers();
                user.AadarId = username;
                CurrentSession.AadharId = user.AadarId;
                user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(username);
                if (user.mUserList != null && user.mUserList.Count > 0)
                {
                    Console.WriteLine(user.mUserList);
                    foreach (var list in user.mUserList)
                    {
                        //string a= CMNirdesh.Models.Login.ComputeHash("Test@123#", "SHA1",null);

                        if (CMNirdesh.Models.Login.VerifyHash(correctPassword, "SHA256", list.Password) == true)
                        {
                            var result = CMNirdesh.Models.Login.WrongAttemptLoginData();
                            if (result.IsValid)
                            {
                                if (saveUserName)
                                {
                                    HttpCookie cookie = new HttpCookie("userAadharID");
                                    cookie.Value = Encrypt(user.AadarId + Guid.NewGuid().ToString().Substring(0, 5));
                                    cookie.HttpOnly = true;
                                    cookie.Expires = DateTime.Now.AddDays(-1);
                                    HttpContext.Response.Cookies.Remove("userAadharID");
                                    HttpContext.Response.SetCookie(cookie);
                                }
                                else
                                {
                                    if (Request.Cookies["userAadharID"] != null)
                                    {
                                        var c = new HttpCookie("userAadharID");
                                        c.Expires = DateTime.Now.AddDays(-1);
                                        c.HttpOnly = true;
                                        Response.Cookies.Add(c);
                                    }
                                }
                                // set status session var as successful
                                CurrentSession.LoginStatus = "Successful";
                                // Avoid Session Hijacking
                                string _browserInfo = Request.Browser.Browser
                                + Request.Browser.Version
                                + Request.UserAgent + "~"
                                + Request.ServerVariables["REMOTE_ADDR"];
                                string _sessionValue = Convert.ToString(Session["UserId"]) + "^"
                                                        + DateTime.Now.Ticks + "^"
                                                        + _browserInfo + "^"
                                                        + System.Guid.NewGuid();
                                byte[] _encodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_sessionValue);
                                string _newEncryptedString = System.Convert.ToBase64String(_encodeAsBytes);
                                if (Request.Cookies["AuthToken"] != null)
                                {
                                    var c = new HttpCookie("AuthToken");
                                    c.Value = _newEncryptedString;
                                    c.Expires = DateTime.Now.AddDays(-1);
                                    if (!Request.IsLocal)
                                    {
                                        c.Secure = true;
                                    }
                                    Response.SetCookie(c);
                                }
                                else
                                {
                                    var c = new HttpCookie("AuthToken");
                                    c.Value = _newEncryptedString;
                                    if (!Request.IsLocal)
                                    {
                                        c.Secure = true;
                                    }
                                    c.Expires = DateTime.Now.AddDays(-1);
                                    Response.Cookies.Add(c);
                                }
                                Session["encryptedSession"] = _newEncryptedString;
                                username = list.UserName;
                                FormsAuthentication.SetAuthCookie(list.UserName, false);
                                CurrentSession.UserID = list.UserId.ToString();
                                ActiveSessionManager.RegisterSession(CurrentSession.UserID);
                                CurrentSession.UserName = list.UserName;
                                CurrentSession.SubUserTypeID = list.UserType.ToString();
                                CurrentSession.IsMember = list.IsMember;
                                CurrentSession.OfficeId = list.OfficeId.ToString();
                                CurrentSession.MCName = list.MCName.ToString();
                                CurrentSession.OfficeLevel = list.OfficeLevel;
                                CurrentSession.StateId = list.StateId.ToString();
                                CurrentSession.parentYN = list.isParent;
                                CurrentSession.MemberMobile = list.MobileNo;
                                CurrentSession.loginEmailID= 
                                // CurrentSession.Designation = list.Designation;
                                // CurrentSession.DeptName = list.DepartmentName;
                                CurrentSession.serviceName = ConfigurationManager.AppSettings["serviceName"];
                                CurrentSession.officeType = list.ofcType;
                                bool s = AuditTrailsExtensionMethods.FillAuditTrail();
                                CurrentSession.isApex = list.isApex.ToString();
                                CurrentSession.MapId = list.MapId.ToString();



                                Models.MenuModel _users = new Models.MenuModel();
                                String menuid = _users.MenuId.ToString();


                                if (list.DeptId != null && list.DeptId != "")
                                {
                                    CurrentSession.DeptID = Convert.ToString(list.DeptId);
                                }
                                if (list.SubdivisionCode != null && list.SubdivisionCode != "")
                                {
                                    CurrentSession.SubDivisionId = Convert.ToString(list.SubdivisionCode);
                                }
                                SetSessions();
                                if (list.AadarId.Contains("MMC"))
                                {
                                    CurrentSession.ConstituencyID = Convert.ToString(list.UserTypeConstituencyId);
                                }
                                else
                                {

                                }

                                {
                                    data = true;
                                    var res = Json(data, JsonRequestBehavior.AllowGet);
                                    //return res; return PartialView("/Views/References/_dashboardType.cshtml");


                                    Session["LoggedIn"] = "yes";
                                    //return RedirectToAction("Dashboard", "References", new { @type =3,@menu=0,@MenuName="Cu Questions" });

                                    return RedirectToAction("Dashboard", "References");




                                }
                            }
                            else
                            {
                                FormsAuthentication.SignOut();
                                CurrentSession.ClearSessions();
                                CurrentSession.Clear();
                            }
                        }
                        else
                        {
                            FormsAuthentication.SignOut();
                            CurrentSession.ClearSessions();
                            CurrentSession.Clear();
                        }
                    }
                }
                else
                {
                    FormsAuthentication.SignOut();
                    CurrentSession.ClearSessions();
                    CurrentSession.Clear();
                }

                TempData["ChngPwdSucessMsg"] = "Username or Password incorrect";
                #endregion
                //                }
                FormsAuthentication.SignOut();
                CurrentSession.ClearSessions();
                CurrentSession.Clear();

            }

#pragma warning disable CS0168 // The variable 'exception' is declared but never used
            catch (Exception exception)
#pragma warning restore CS0168 // The variable 'exception' is declared but never used
            {
                TempData["ChngPwdSucessMsg"] = "Username or Password incorrect";
                ViewBag.HasError = true;
            }

            var userid = CurrentSession.UserID;

            UserModel obj = new UserModel(); // Create an instance of MyClass
            obj.AddLoginUser(userid);
            //  SBL.eLegistrator.HouseController.Web.Utility.CurrentSession.UpdateMenuandModules();
            data = false;

            var res2 = Json(data, JsonRequestBehavior.AllowGet);
            return res2;
        }

        //[HttpPost]
        //[CMNirdeshAuthorize(Allow = "All")]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            string ChekUser = "";
            var userid = CurrentSession.UserID;
            ActiveSessionManager.RemoveSession(userid);
            UserModel obj = new UserModel(); // Create an instance of MyClass
            obj.AddLogoutUser(userid);
            bool s = AuditTrailsExtensionMethods.FillAuditTrailLogOut();

            string parichay_id = CurrentSession.ServiceSessionID;

            if (parichay_id != "")
            {
                return RedirectToAction("parichaylogout", "Home");
            }
            else
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                CurrentSession.ClearSessions();
                CurrentSession.Clear();
                HttpCookie reqCookies = Request.Cookies["Login"];
                if (reqCookies != null)
                {
                    HttpCookie nameCookie = Request.Cookies["Login"];
                    nameCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(nameCookie);
                }
                if (TempData["IsRegister"] != null)
                {
                    ChekUser = Convert.ToString(TempData["IsRegister"]);
                    CurrentSession.Islogged = ChekUser;
                }
                return RedirectToAction("Login", "Account");
            }

        }

        public string Encrypt(string text, string pwd = "qwerty12345")
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes = null;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Generating salt bytes
            byte[] saltBytes = GetRandomBytes();

            // Appending salt bytes to original bytes
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(encryptedBytes);
        }

        public byte[] GetRandomBytes()
        {
            int _saltSize = 4;
            byte[] ba = new byte[_saltSize];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public string Decrypt(string decryptedText, string pwd = "qwerty12345")
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            // Getting the size of salt
            int _saltSize = 4;

            // Removing salt bytes, retrieving original bytes
            byte[] originalBytes = new byte[decryptedBytes.Length - _saltSize];
            for (int i = _saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - _saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }
        internal void SetSessions()
        {
            var methodParameter = new List<KeyValuePair<string, string>>();
            DataSet dataSetUser = new DataSet();
            dataSetUser = CMNirdesh.Models.Login.GetLogin(CurrentSession.UserID);
            if (dataSetUser != null && dataSetUser.Tables.Count > 0 && dataSetUser.Tables[0].Rows.Count > 0)
            {
                bool IsMember = false;
                CurrentSession.RoleID = "0";
                if (dataSetUser.Tables[0].Rows[0]["Roleid"] != DBNull.Value)
                {
                    CurrentSession.RoleID = Convert.ToString(dataSetUser.Tables[0].Rows[0]["Roleid"]);
                    CurrentSession.RoleName = Convert.ToString(dataSetUser.Tables[0].Rows[0]["RoleName"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["officename"] != DBNull.Value)
                {
                    CurrentSession.OfficeName = Convert.ToString(dataSetUser.Tables[0].Rows[0]["officename"]);
                    CurrentSession.OfficeNameNew = Convert.ToString(dataSetUser.Tables[0].Rows[0]["officename"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["deptname"] != DBNull.Value)
                {
                    CurrentSession.DeptName = Convert.ToString(dataSetUser.Tables[0].Rows[0]["deptname"]);
                    CurrentSession.DeptNameNew = Convert.ToString(dataSetUser.Tables[0].Rows[0]["deptname"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["UserTypeConstituencyId"] != DBNull.Value)
                {
                    CurrentSession.UserTypeConstituencyId = Convert.ToString(dataSetUser.Tables[0].Rows[0]["UserTypeConstituencyId"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["DistrictCode"] != DBNull.Value)
                {
                    CurrentSession.DistrictCode = Convert.ToString(dataSetUser.Tables[0].Rows[0]["DistrictCode"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["DeptId"] != DBNull.Value)
                {
                    if (dataSetUser.Tables[0].Rows[0]["ApprovedAdditionalDept"] != DBNull.Value)
                    {
                        string Department = Convert.ToString(dataSetUser.Tables[0].Rows[0]["DeptId"]);
                        string AdditionalDept = Convert.ToString(dataSetUser.Tables[0].Rows[0]["ApprovedAdditionalDept"]);
                        string alldept = Department + "," + AdditionalDept;
                        List<string> deptlist = alldept.Split(',').Distinct().ToList();
                        string DistDept = deptlist.Aggregate((a, x) => a + "," + x);
                        CurrentSession.DeptID = DistDept;
                    }
                    else
                    {
                        CurrentSession.DeptID = Convert.ToString(dataSetUser.Tables[0].Rows[0]["DeptId"]);
                    }
                }
                if (dataSetUser.Tables[0].Rows[0]["SignaturePath"] != DBNull.Value)
                {
                    CurrentSession.SignaturePath = Convert.ToString(dataSetUser.Tables[0].Rows[0]["SignaturePath"]);
                }




                if (dataSetUser.Tables[0].Rows[0]["Designation"] != DBNull.Value)
                {
                    CurrentSession.Designation = Convert.ToString(dataSetUser.Tables[0].Rows[0]["Designation"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["SecretoryId"] != DBNull.Value)
                {
                    CurrentSession.SecretaryId = Convert.ToString(dataSetUser.Tables[0].Rows[0]["SecretoryId"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["IsHOD"] != DBNull.Value)
                {
                    CurrentSession.IsHOD = Convert.ToString(dataSetUser.Tables[0].Rows[0]["IsHOD"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["IsSecretoryId"] != DBNull.Value)
                {
                    CurrentSession.IsSecretoryId = Convert.ToString(dataSetUser.Tables[0].Rows[0]["IsSecretoryId"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["IsMember"] != DBNull.Value)
                {
                    CurrentSession.MemberDesignation = Convert.ToString(dataSetUser.Tables[0].Rows[0]["Designation"]);
                    IsMember = Convert.ToBoolean(dataSetUser.Tables[0].Rows[0]["IsMember"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["OfficeId"] != DBNull.Value)
                {
                    CurrentSession.OfficeId = Convert.ToString(dataSetUser.Tables[0].Rows[0]["OfficeId"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["isApex"] != DBNull.Value)
                {
                    CurrentSession.isApex = Convert.ToString(dataSetUser.Tables[0].Rows[0]["isApex"]);
                }

                if (dataSetUser.Tables[0].Rows[0]["OfficeId"] != DBNull.Value)
                {
                    CurrentSession.OfficeId = Convert.ToString(dataSetUser.Tables[0].Rows[0]["OfficeId"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["SubdivisionCode"] != DBNull.Value)
                {
                    CurrentSession.SubDivisionId = Convert.ToString(dataSetUser.Tables[0].Rows[0]["SubdivisionCode"]);
                }
                if (dataSetUser.Tables[0].Rows[0]["ConstituencyID"] != DBNull.Value)
                {
                    CurrentSession.ConstituencyID = Convert.ToString(dataSetUser.Tables[0].Rows[0]["ConstituencyID"]);
                }

                CurrentSession.MemberCode = CurrentSession.UserName;
                if (dataSetUser.Tables[0].Rows[0]["Name"] != DBNull.Value)
                {
                    //CurrentSession.Name = CustomCryptoService.DecryptString(dataSetUser.Tables[0].Rows[0]["Name"]?.ToString() ?? string.Empty);
                    CurrentSession.Name = Convert.ToString(dataSetUser.Tables[0].Rows[0]["Name"]);
                }

            }
        }
        public class RandomPasswordGenerator
        {
            public static string GenerateRandomPassword(int length)
            {
                const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#$";

                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] randomBytes = new byte[length];
                    rng.GetBytes(randomBytes);

                    char[] password = new char[length];
                    for (int i = 0; i < length; i++)
                    {
                        password[i] = validChars[randomBytes[i] % validChars.Length];
                    }

                    return new string(password);
                }
            }
            public static string GenerateRandomOtp(int length)
            {
                const string validChars = "1234567890";

                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] randomBytes = new byte[length];
                    rng.GetBytes(randomBytes);

                    char[] password = new char[length];
                    for (int i = 0; i < length; i++)
                    {
                        password[i] = validChars[randomBytes[i] % validChars.Length];
                    }

                    return new string(password);
                }
            }
        }
        public ActionResult ChangePassword(LoginModel model)
        {
            Session["LoggedIn"] = "no";
            Session["HomeButton"] = "yes";
            CurrentSession.CurrentAction = "";
            return View("ChangePassword", model);
        }
        public async Task<ActionResult> forgetUserPassword(LoginModel model, string CaptchaText)
        {
            try
            {
                if (Session["CaptchaImageText"] != null)
                {
                    string sessionCaptcha = (this.Session["CaptchaImageText"] ?? "").ToString().Trim().ToUpper();
                    string userCaptcha = (CaptchaText ?? "").Trim().ToUpper();
                    if (sessionCaptcha != userCaptcha)
                    {
                        model.ErrorMessage = "Captcha Validation Failed!";
                        return View("ChangePassword", model);
                    }
                }
                Session["LoggedIn"] = "no";
                Session["HomeButton"] = "yes";

                string username = model.UserName;
                CurrentSession.UserName = model.UserName;
                string res = CMNirdesh.Models.Login.CheckUserExist(username);
                string[] userresult = res.Split('-');
                res = userresult[0].Trim();
                string mobno = "";
                if (userresult.Length > 1)
                {
                    mobno = userresult[1].Trim();
                }
                else
                {
                    model.ErrorMessage = "User Mobile No. not available";
                    return View("ChangePassword", model);
                }
                mobno = mobno.Trim();
                CurrentSession.MbNo = mobno;
                if (userresult[0] == "Y")
                {
                    int otplength = 5;
                    string randomOTP = RandomPasswordGenerator.GenerateRandomOtp(otplength);

                    Session["userOTP"] = randomOTP;

                    SmsOrchestrationService smsService = new SmsOrchestrationService();

                    // ✅ Proper async call
                    string result = await smsService.SendForgetPasswordOtp(mobno, randomOTP, username);

                    // Optional: check SMS result
                    if (!result.Contains("success"))
                    {
                        TempData["ErrorMsg"] = result;
                        return View(model);
                    }

                    TempData["ChngPwdSucessMsg"] =
                        "OTP has been sent to your registered Mobile no. Please proceed with OTP Verification";

                    CurrentSession.CurrentAction = "userpass";
                    return View("ChangePassword", model);
                }

                model.ErrorMessage = "Invalid Username!";
                return View("ChangePassword", model);
            }
            catch (Exception ex)
            {
                return View("ChangePassword", model);
            }
        }
        public ActionResult verifyOTP(LoginModel model, string CaptchaText)
        {
            try
            {
                if (Session["CaptchaImageText"] != null)
                {
                    string sessionCaptcha = (this.Session["CaptchaImageText"] ?? "").ToString().Trim().ToUpper();
                    string userCaptcha = (CaptchaText ?? "").Trim().ToUpper();
                    if (sessionCaptcha != userCaptcha)
                    {
                        model.ErrorMessage = "Captcha Validation Failed!";
                        return View("ChangePassword", model);
                    }
                }
                Session["LoggedIn"] = "no";
                Session["HomeButton"] = "yes";
                string otp = model.userOTP;
                if (otp == Session["userOTP"].ToString())
                {
                    TempData["ChngPwdSucessMsg"] = "OTP Verified. Please proceed with password change";
                    CurrentSession.CurrentAction = "otppass";
                    return View("ChangePassword", model);
                }
                model.ErrorMessage = "Invalid User OTP!";
                return View("ChangePassword", model);
            }
            catch (Exception ex)
            {
                return View("ChangePassword", model);
            }
        }
        public ActionResult SaveUserNewPassword(LoginModel model, string CaptchaText)
        {
            try
            {
                if (Session["CaptchaImageText"] != null)
                {
                    string sessionCaptcha = (this.Session["CaptchaImageText"] ?? "").ToString().Trim().ToUpper();
                    string userCaptcha = (CaptchaText ?? "").Trim().ToUpper();
                    if (sessionCaptcha != userCaptcha)
                    {
                        model.ErrorMessage = "Captcha Validation Failed!";
                        return View("ChangePassword", model);
                    }
                }

                string username = CurrentSession.UserName;
                string newPassword = model.Password;



                //newPassword = CMNirdesh.Models.Login.ComputeHash(newPassword, "SHA256", null);

                string hashedPassword = model.Password.Substring(0, 64);
                newPassword = hashedPassword;



                string res = CMNirdesh.Models.Login.changeUserLoginPwd(username, newPassword);
                if (res == "Y")
                {
                    string[] credentials = { username, model.Password };
                    SMSMessage _smsService = new SMSMessage();
                    Task<string> task = _smsService.SendLoginMessageMultipleVariables(CurrentSession.MbNo, credentials, "1407170659322347791");


                    TempData["ChngPwdSucessMsg"] = "Password changed successfully.Please login again with new credentials";
                    return RedirectToAction("Login", "Account");
                }
                model.ErrorMessage = "There is some error!";
                return View("ChangePassword", model);
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "There is some error!";
                return View("ChangePassword", model);
            }
        }
        public async Task<ActionResult> ResendOtp()
        {
            LoginModel model = new LoginModel();
            int otplength = 5;
            string randomOTP = RandomPasswordGenerator.GenerateRandomOtp(otplength);
            Session["userOTP"] = randomOTP;
            string mobno = CurrentSession.MbNo;
            string Username = CurrentSession.UserName;

            SmsOrchestrationService smsService = new SmsOrchestrationService();
            string result = await smsService.SendForgetPasswordOtp(mobno, randomOTP, Username);

            // Optional: check SMS result
            if (!result.Contains("success"))
            {
                TempData["ErrorMsg"] = result;
                return View(model);
            }
            CurrentSession.CurrentAction = "userpass";
            return View("ChangePassword", model);
        }

        [HttpPost]
        public JsonResult RefreshSession()
        {
            // Access session variable to refresh session timeout
            Session["LastActivity"] = DateTime.Now;
            CurrentSession.LoginTime = DateTime.Now;
            // You can also update any other session data here if needed

            return Json(new { success = true, message = "Session refreshed successfully" });
        }


        [HttpGet]
        public ActionResult UpdatePassword(int type)
        {
            // Session check
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var model = new UpdatePasswordViewModel
            {
                ChangeType = type   // e.g. 1 = First Login, 2 = Forgot Password
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(UpdatePasswordViewModel model)
        {
            // 1️⃣ Session check
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // 2️⃣ Model validation
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 3️⃣ Get username from session
            string userName = CurrentSession.UserName;

            // 4️⃣ Fetch user from DB
            var user = CMNirdesh.Models.Login
                .GetDetailsByadhaarID(userName)
                .FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(model);
            }

            // 5️⃣ Hash passwords (SERVER SIDE)
            string oldPasswordHash = ComputeSha256(model.ActualPassword);
            string newPasswordHash = ComputeSha256(model.Password);

            // 6️⃣ Verify old password
            if (!string.Equals(user.Password, oldPasswordHash, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("ActualPassword", "Current password is incorrect");
                return View(model);
            }

            bool mobileExists = CMNirdesh.Models.Login.IsMobileAlreadyExists(model.Mobile, user.UserId);

            if (mobileExists)
            {
                ModelState.AddModelError("Mobile", "Mobile number already exists for another user");
                ClearPasswordFields(model);
                return View(model);
            }



            // 7️⃣ Update password (HASH ONLY)
            CMNirdesh.Models.Login.UpdateUserPassword(user.UserId, newPasswordHash,model.Mobile, model.Email);

            // 8️⃣ Mark password as changed
            //CMNirdesh.Models.Login.SetPasswordChanged(user.UserId, true);

            // 9️⃣ Redirect to dashboard
            return RedirectToAction("Dashboard", "References", new { type = model.ChangeType });
        }

        public static string ComputeSha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private void ClearPasswordFields(UpdatePasswordViewModel model)
        {
            if (model == null) return;
            model.ActualPassword = string.Empty;
            model.Password = string.Empty;
            model.confirmPassword = string.Empty;
        }

        [HttpGet]
        public ActionResult UpdateUserDetails()
        {
            var deptId = CurrentSession.DeptID;
            // Session check
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            mUsers user = new mUsers();
            user.DeptId = deptId;
            return View(user);
        }

        [HttpGet]
        public ActionResult KeepAlive()
        {
            Session["LastActive"] = DateTime.Now;
            return Json(new { status = "success", timestamp = DateTime.Now.ToString("o") }, JsonRequestBehavior.AllowGet);
        }

    }
}