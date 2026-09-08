using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data;

using PVSWebSite.Models;
using System.Configuration;
using System.Net;
using System.Text;

using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json.Linq;
using CMNirdesh.Models;


namespace PVSWebSite.Controllers
{
    public class NevaQuestionController : Controller
    {
       // string defaultLang = ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
        string token = ConfigurationManager.AppSettings["Token"].ToString();
        string nevaurl = ConfigurationManager.AppSettings["NevaServerPath"].ToString();
        SqlConnection con;
        SqlParameter[] param;
        SqlParameter[] param1;
        string urlString;
        //string serverFileAcessUrlPath = Utility.FileUtils.GetFileSetting("FileAccessingUrlPath");
        public ActionResult Index(string defaultLang = null)
        {
            var selectedLang = "";
            if (defaultLang == null)
            {
                if (Session["Language"] != null)
                {
                    selectedLang = Session["Language"].ToString();
                }
                else { selectedLang = defaultLang; Session["Language"] = selectedLang; }
            }
            else
            {
                selectedLang = defaultLang;
                Session["Language"] = selectedLang;
            }




            NevaStarredQuestion mdl = new NevaStarredQuestion();


            mdl.AssemblyList = DiaryViewModel.Assembly();
            mdl.SessionList = DiaryViewModel.Session();
            mdl.SessDatelist = DiaryViewModel.SessionDates();
            mdl.MemberList = DiaryViewModel.Members();


            try
            {

              
                var obj = new
                {
                    Token = token,
                    AssemblyID = "16",
                    SessionID = "41",
                    SessionDateID = "172",
                    MemberCode = "",
                    QuestionType = "0"
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(obj);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "text/plain";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(nevaurl + "QuestionList", inputJson);
                JArray array = JArray.Parse(json);

                var deserialized = JsonConvert.DeserializeObject<List<NevaStarredQuestion>>(array.ToString());
                List<NevaStarredQuestion> question = new List<NevaStarredQuestion>();
                if (deserialized != null)
                {
                    for (int n = 0; n < deserialized.Count; n++)
                    {
                        dynamic data = JObject.Parse(array[n].ToString());
                        NevaStarredQuestion q = new NevaStarredQuestion();
                        q.AssemblyId =data["AssemblyID"];
                        q.SessionId = data["SessionID"];
                        q.SessionDateId = data["SessionDateID"];
                        q.QuestionId = data["QuestionId"];
                        q.QuestionNumber = data["QuestionNumber"];
                        q.QuestionType = data["QuestionType"];
                        q.Subject = data["Subject"];
                        q.Questions = data["Questions"];
                        q.DepartmentID = data["DepartmentID"];
                        q.DepartmentLocal = data["DepartmentLocal"];
                        q.Department = data["Department"];
                        q.MinistryID = data["MinistryID"];
                        q.MinisteryName = data["MinisteryName"];
                        q.MinisteryNameLocal = data["MinisteryNameLocal"];
                        q.MemberCode = data["MemberCode"];
                        q.Name = data["Name"];
                        q.NameLocal = data["NameLocal"];
                        q.AskedBy = data["AskedBy"];
                        q.Reply = data["Reply"];
                        q.PaperLaidId = data["PaperLaidId"];
                        q.MainQuestion = data["MainQuestion"];
                        q.ConstituencyName = data["ConstituencyName"];
                        q.ConstituencyNameLocal = data["ConstituencyNameLocal"];
                        q.ReplyPDF = data["ReplyPDF"];
                        question.Add(q);




                        urlString = q.ReplyPDF.ToString();
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        using (WebClient client1 = new WebClient())
                        {
                            byte[] fileBytes = client1.DownloadData(urlString);


                            string fileName = Path.GetFileName(new Uri(urlString).LocalPath);
                            string savePath = Server.MapPath("~/References/" + fileName);


                            System.IO.File.WriteAllBytes(savePath, fileBytes);
                        }

                        SqlConnection connection = ClsConnection.GetConnection();
                        if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                        {
                            connection.Open();
                        }

                        // string connectionString = ConfigurationManager.ConnectionStrings["pvsDatabase"].ConnectionString;
                        // con = new SqlConnection(connectionString);
                        //if ((con.State == ConnectionState.Closed) || (con.State == ConnectionState.Broken))
                        //{
                        //    con.Open();
                        //}
                        //con.Close();
                        //param = new SqlParameter[24];
                        //param[0] = new SqlParameter("@AssemblyId", Convert.ToString(q.AssemblyId));
                        //param[1] = new SqlParameter("@SessionId", Convert.ToString(q.SessionId.ToString()));
                        //param[2] = new SqlParameter("@SessionDateId", Convert.ToString(q.SessionDateId.ToString()));
                        //param[3] = new SqlParameter("@QuestionId", Convert.ToString(q.QuestionId.ToString()));
                        //param[4] = new SqlParameter("@QuestionNumber", Convert.ToString(q.QuestionNumber.ToString()));
                        //param[5] = new SqlParameter("@QuestionType", Convert.ToString(q.QuestionType.ToString()));
                        //param[6] = new SqlParameter("@Subject", Convert.ToString(q.Subject));
                        //param[7] = new SqlParameter("@Questions", Convert.ToString(q.Questions));
                        //param[8] = new SqlParameter("@DepartmentID", Convert.ToString(q.DepartmentID));
                        //param[9] = new SqlParameter("@DepartmentLocal", Convert.ToString(q.DepartmentLocal));
                        //param[10] = new SqlParameter("@Department", Convert.ToString(q.Department));
                        //param[11] = new SqlParameter("@MinistryID", Convert.ToString(q.MinistryID));
                        //param[12] = new SqlParameter("@MinisteryName", Convert.ToString(q.MinisteryName));
                        //param[13] = new SqlParameter("@MinisteryNameLocal", Convert.ToString(q.MinisteryNameLocal));
                        //param[14] = new SqlParameter("@MemberCode", Convert.ToString(q.MemberCode));
                        //param[15] = new SqlParameter("@Name", Convert.ToString(q.Name));
                        //param[16] = new SqlParameter("@NameLocal", Convert.ToString(q.NameLocal));
                        //param[17] = new SqlParameter("@AskedBy", Convert.ToString(q.AskedBy));
                        //param[18] = new SqlParameter("@Reply", Convert.ToString(q.Reply));
                        //param[19] = new SqlParameter("@PaperLaidId", Convert.ToString(q.PaperLaidId));
                        //param[20] = new SqlParameter("@MainQuestion", Convert.ToString(q.MainQuestion));
                        //param[21] = new SqlParameter("@ConstituencyName", Convert.ToString(q.ConstituencyName));
                        //param[22] = new SqlParameter("@ConstituencyNameLocal", Convert.ToString(q.ConstituencyNameLocal));
                        //param[23] = new SqlParameter("@ReplyPDF", Convert.ToString(q.ReplyPDF));

                        //SqlHelper.ExecuteDataset(con, "sp_Neva_AddUpdateQuestion", param);

                        string userid = CurrentSession.UserID;
                        string deptid = CurrentSession.DeptID;






                        SqlParameter[] parameterValues = new SqlParameter[] { 
                            new SqlParameter("@RefId", Convert.ToInt64(0)),
                    new SqlParameter("@LinkRefID",Convert.ToInt64(0)),
                            new SqlParameter("@FromDeptId", "60"),
                    new SqlParameter("@FromOfficeId",1),
                    new SqlParameter("@DispatchNumber",q.QuestionNumber),
                    new SqlParameter("@ToDeptId", q.DepartmentID),
                    new SqlParameter("@ToOfficeId",2),
                    new SqlParameter("@DocmentType", 3),
                            new SqlParameter("@LetterNo",q.QuestionId),
                    new SqlParameter("@LetterDate", string.IsNullOrEmpty("") ? (DateTime?)null : Convert.ToDateTime("")),
                    new SqlParameter("@Subject", q.Subject),
                            new SqlParameter("@SubjectDetails", q.MainQuestion),
                    new SqlParameter("@EnclosureFile", urlString),
                    new SqlParameter("@ApplicantName", ""),
                            new SqlParameter("@ApplicantAddress", ""),
                    new SqlParameter("@ApplicantMobile", ""),
                    new SqlParameter("@ApplicantEmail", ""),
                            new SqlParameter("@CreatedBy", CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName),
                    new SqlParameter("@DiaryDate", string.IsNullOrEmpty("") ? (DateTime?)null : Convert.ToDateTime("")),
                    new SqlParameter("@ActionCode", 11),
                    new SqlParameter("@ActionTakenDate",string.IsNullOrEmpty("") ? (DateTime?)null : Convert.ToDateTime("")),
                    new SqlParameter("@ActionDescription",""),
                    new SqlParameter("@ActionAmountSanction", Convert.ToDouble("23")),
                    new SqlParameter("@ActionTakenFile", ""),
                    new SqlParameter("@OfficeId", 1),
                    new SqlParameter("@letter","Letter"),
                    new SqlParameter("@MeetingId",0),
                    new SqlParameter("@userid", CurrentSession.UserID),
                    new SqlParameter("@sentTouserid",CurrentSession.UserID),
                    new SqlParameter("@MoveId",0),
                     new SqlParameter("@InboxType", ""),
                    new SqlParameter("@SentType",""),
                     new SqlParameter("@MapId",0),
                     new SqlParameter("@ActId", 0),
                     new SqlParameter("@AssemblyCode", q.AssemblyId),
                     new SqlParameter("@SessionCode", q.SessionId),
                     new SqlParameter("@MemberCode", q.MemberCode),
                     new SqlParameter("@MinistryID", q.MinistryID),
                     new SqlParameter("@MemberName", q.Name),
                     new SqlParameter("@MemberNameLocal", q.NameLocal),
                     new SqlParameter("@MinisteryName", q.MinisteryName),
                     new SqlParameter("@MinisteryNameLocal", q.MinisteryNameLocal)


                };

                       string ret = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[MLADispatchInsert]", parameterValues));
                        connection.Close();



                      

                       //qlHelper.ExecuteDataset(con, "NevaMLADispatchInsert", param1);





                        //urlString = q.ReplyPDF.ToString();
                        //ServicePointManager.Expect100Continue = true;
                        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                        //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        //using (WebClient client1 = new WebClient())
                        //{
                        //    byte[] fileBytes = client1.DownloadData(urlString);


                        //    string fileName = Path.GetFileName(new Uri(urlString).LocalPath);
                        //    string savePath = Server.MapPath("~/QuestionReply/" + fileName);


                        //    System.IO.File.WriteAllBytes(savePath, fileBytes);
                        //}




                    }
                   
                    mdl.mQuestiionList = question;


                }

            }
            catch (Exception ex)
            {
                CommonModel.SendErrorToText(ex);
            }


            return View(mdl);
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

        //public JsonResult GetAssembly()
        //{
        //    AssemblyModel mdl = new AssemblyModel();
        //    try
        //    {

        //        var obj = new
        //        {
        //            Token = token
        //        };
        //        string inputJson = (new JavaScriptSerializer()).Serialize(obj);
        //        WebClient client = new WebClient();
        //        client.Headers["Content-type"] = "text/plain";
        //        client.Encoding = Encoding.UTF8;
        //        string json = client.UploadString(nevaurl + "AssemblyList", inputJson);
        //        JArray array = JArray.Parse(json);

        //        var deserialized = JsonConvert.DeserializeObject<List<AssemblyModel>>(array.ToString());
        //        List<AssemblyModel> assembly = new List<AssemblyModel>();
        //        if (deserialized != null)
        //        {
        //            for (int n = 0; n < deserialized.Count; n++)
        //            {
        //                dynamic data = JObject.Parse(array[n].ToString());
        //                AssemblyModel q = new AssemblyModel();
        //                q.AssemblyID = data["AssemblyID"];
        //                q.AssemblyName = data["AssemblyName"];
        //                q.AssemblyNameLocal = data["AssemblyNameLocal"];
        //                assembly.Add(q);

        //                string connectionString = ConfigurationManager.ConnectionStrings["pvsDatabase"].ConnectionString;
        //                con = new SqlConnection(connectionString);
        //                if ((con.State == ConnectionState.Closed) || (con.State == ConnectionState.Broken))
        //                {
        //                    con.Open();
        //                }
        //                con.Close();
        //                param = new SqlParameter[3];
        //                param[0] = new SqlParameter("@AssemblyId", Convert.ToString(q.AssemblyID));
        //                param[1] = new SqlParameter("@AssemblyName", Convert.ToString(q.AssemblyName.ToString()));
        //                param[2] = new SqlParameter("@AssemblyNameLocal", Convert.ToString(q.AssemblyNameLocal.ToString()));

        //                SqlHelper.ExecuteDataset(con, "sp_Neva_AddUpdateAssembly", param);

        //            }
        //            mdl.ListAssembly = assembly;


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        CommonModel.SendErrorToText(ex);
        //    }
        //    //try
        //    //{
        //    //    List<KeyValuePair<string, string>> methodParameter = new List<KeyValuePair<string, string>>();
        //    //    DataSet data = PVSWebSite.Utility.ServiceAdapter.ServiceAdaptor.GetDataSetFromService("SelectMSSql", "sp_GetAssemblyDetails", methodParameter);
        //    //    List<AssemblyModel> assList = new List<AssemblyModel>();
        //    //    if (data != null)
        //    //    {
        //    //        for (int i = 0; i < data.Tables[0].Rows.Count; i++)
        //    //        {
        //    //            AssemblyModel m = new AssemblyModel();
        //    //            m.AssemblyID = Convert.ToInt32(data.Tables[0].Rows[i]["AssemblyID"]);
        //    //            m.AssemblyCode = Convert.ToInt32(data.Tables[0].Rows[i]["AssemblyCode"]);
        //    //            m.AssemblyName = Convert.ToString(data.Tables[0].Rows[i]["AssemblyName"]);
        //    //            m.AssemblyNameLocal = Convert.ToString(data.Tables[0].Rows[i]["AssemblyNameLocal"]);
        //    //            assList.Add(m);
        //    //        }
        //    //    }
        //    //    mdl.ListAssembly = assList;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    CommonModel.SendErrorToText(ex);
        //    //}
        //    return Json(mdl.ListAssembly, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetSession()
        //{
        //    SessionModel mdl = new SessionModel();
        //    try
        //    {

        //        var obj = new
        //        {
        //            Token = token
        //        };
        //        string inputJson = (new JavaScriptSerializer()).Serialize(obj);
        //        WebClient client = new WebClient();
        //        client.Headers["Content-type"] = "text/plain";
        //        client.Encoding = Encoding.UTF8;
        //        string json = client.UploadString(nevaurl + "SessionList", inputJson);
        //        JArray array = JArray.Parse(json);

        //        var deserialized = JsonConvert.DeserializeObject<List<SessionModel>>(array.ToString());
        //        List<SessionModel> session = new List<SessionModel>();
        //        if (deserialized != null)
        //        {
        //            for (int n = 0; n < deserialized.Count; n++)
        //            {
        //                dynamic data = JObject.Parse(array[n].ToString());
        //                SessionModel q = new SessionModel();
        //                q.AssemblyCode = data["AssemblyID"];
        //                q.SessionID = data["SessionID"];
        //                q.SessionName = data["SessionName"];
        //                q.SessionNameLocal = data["SessionNameLocal"];
        //                session.Add(q);


        //                string connectionString = ConfigurationManager.ConnectionStrings["pvsDatabase"].ConnectionString;
        //                con = new SqlConnection(connectionString);
        //                if ((con.State == ConnectionState.Closed) || (con.State == ConnectionState.Broken))
        //                {
        //                    con.Open();
        //                }
        //                con.Close();
        //                param = new SqlParameter[4];
        //                param[0] = new SqlParameter("@AssemblyCode", Convert.ToString(q.AssemblyCode));
        //                param[1] = new SqlParameter("@SessionID", Convert.ToString(q.SessionID.ToString()));
        //                param[2] = new SqlParameter("@SessionName", Convert.ToString(q.SessionName.ToString()));
        //                param[3] = new SqlParameter("@SessionNameLocal", Convert.ToString(q.SessionNameLocal.ToString()));


        //                SqlHelper.ExecuteDataset(con, "sp_Neva_AddUpdateSession", param);

        //            }
        //            mdl.ListSession = session;


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        CommonModel.SendErrorToText(ex);
        //    }
        //    //try
        //    //{
        //    //    List<KeyValuePair<string, string>> methodParameter = new List<KeyValuePair<string, string>>();
        //    //    DataSet data = PVSWebSite.Utility.ServiceAdapter.ServiceAdaptor.GetDataSetFromService("SelectMSSql", "sp_GetSessionDetails", methodParameter);
        //    //    List<SessionModel> assList = new List<SessionModel>();
        //    //    if (data != null)
        //    //    {
        //    //        for (int i = 0; i < data.Tables[0].Rows.Count; i++)
        //    //        {
        //    //            SessionModel m = new SessionModel();
        //    //            m.AssemblyCode = Convert.ToInt32(data.Tables[0].Rows[i]["AssemblyID"]);
        //    //            m.SessionID = Convert.ToInt32(data.Tables[0].Rows[i]["SessionID"]);
        //    //            m.SessionCode = Convert.ToInt32(data.Tables[0].Rows[i]["SessionCode"]);

        //    //            m.SessionName = Convert.ToString(data.Tables[0].Rows[i]["SessionName"]);
        //    //            m.SessionNameLocal = Convert.ToString(data.Tables[0].Rows[i]["SessionNameLocal"]);
        //    //            assList.Add(m);
        //    //        }
        //    //    }
        //    //    mdl.ListSession = assList;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    CommonModel.SendErrorToText(ex);
        //    //}
        //    return Json(mdl.ListSession, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetSessionDates()
        //{
        //    SessionDateModel mdl = new SessionDateModel();
        //    try
        //    {

        //        var obj = new
        //        {
        //            Token = token,
        //            AssemblyID="15", 
        //            SessionID="7"
        //        };
        //        string inputJson = (new JavaScriptSerializer()).Serialize(obj);
        //        WebClient client = new WebClient();
        //        client.Headers["Content-type"] = "text/plain";
        //        client.Encoding = Encoding.UTF8;
        //        string json = client.UploadString(nevaurl + "SessionDateList", inputJson);
        //        JArray array = JArray.Parse(json);

        //        var deserialized = JsonConvert.DeserializeObject<List<SessionDateModel>>(array.ToString());
        //        List<SessionDateModel> sessiondate = new List<SessionDateModel>();
        //        if (deserialized != null)
        //        {
        //            for (int n = 0; n < deserialized.Count; n++)
        //            {
        //                dynamic data = JObject.Parse(array[n].ToString());
        //                SessionDateModel q = new SessionDateModel();

        //                q.SessionDate = data["SessionDate"];
        //                q.SessionDate_Local = data["SessionDate_Local"];
        //                q.SessionID = data["SessionDateId"];
        //                sessiondate.Add(q);


        //                string connectionString = ConfigurationManager.ConnectionStrings["pvsDatabase"].ConnectionString;
        //                con = new SqlConnection(connectionString);
        //                if ((con.State == ConnectionState.Closed) || (con.State == ConnectionState.Broken))
        //                {
        //                    con.Open();
        //                }
        //                con.Close();
        //                param = new SqlParameter[3];
        //                param[0] = new SqlParameter("@SessionDateId", Convert.ToString(q.SessionID));
        //                param[1] = new SqlParameter("@SessionDate", Convert.ToDateTime(q.SessionDate).ToString("dd-MM-yyyy"));
        //                param[2] = new SqlParameter("@SessionDate_Local", Convert.ToString(q.SessionDate_Local.ToString()));



        //                SqlHelper.ExecuteDataset(con, "sp_Neva_AddUpdateSessionDates", param);

        //            }
        //            mdl.ListSessionDate = sessiondate;


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        CommonModel.SendErrorToText(ex);
        //    }
        //    //try
        //    //{
        //    //    List<KeyValuePair<string, string>> methodParameter = new List<KeyValuePair<string, string>>();
        //    //    DataSet data = PVSWebSite.Utility.ServiceAdapter.ServiceAdaptor.GetDataSetFromService("SelectMSSql", "sp_GetSessionDatesDetails", methodParameter);
        //    //    List<SessionDateModel> assList = new List<SessionDateModel>();
        //    //    if (data != null)
        //    //    {
        //    //        for (int i = 0; i < data.Tables[0].Rows.Count; i++)
        //    //        {
        //    //            SessionDateModel m = new SessionDateModel();
        //    //            m.ID = Convert.ToInt32(data.Tables[0].Rows[i]["Id"]);
        //    //            m.AssemblyCode = Convert.ToInt32(data.Tables[0].Rows[i]["AssemblyID"]);
        //    //            m.SessionID = Convert.ToInt32(data.Tables[0].Rows[i]["SessionID"]);
        //    //            m.SessionDate = Convert.ToDateTime(data.Tables[0].Rows[i]["SessionDate"]).ToString("dd-MM-yyyy");

        //    //            assList.Add(m);
        //    //        }
        //    //    }
        //    //    mdl.ListSessionDate = assList;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    CommonModel.SendErrorToText(ex);
        //    //}
        //    return Json(mdl.ListSessionDate, JsonRequestBehavior.AllowGet);
        //}


    }
}
