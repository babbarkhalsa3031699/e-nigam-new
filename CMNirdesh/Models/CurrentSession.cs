
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace CMNirdesh.Models
{
	/// <summary>
	/// 
	/// </summary>
	public class CurrentSession
	{
		public static string eVidhanText
		{
			get { return "Punjab CMNirdesh"; }
		}

		public static string ImplementationPlace
		{
			get { return "Punjab"; }
		}

		public static string LoginText
		{
			get { return ((HttpContext.Current.Session["LoginText"] == null) ? "Login" : (string)HttpContext.Current.Session["LoginText"]); }
			set { HttpContext.Current.Session["LoginText"] = value; }
		}
		public static string IsMember
		{
			get { return ((HttpContext.Current.Session["IsMember"] == null) ? string.Empty : (string)HttpContext.Current.Session["IsMember"]); }
			set { HttpContext.Current.Session["IsMember"] = value; }
		}
        public static string OfficeLevel
        {
            get { return ((HttpContext.Current.Session["OfficeLevel"] == null) ? string.Empty : (string)HttpContext.Current.Session["OfficeLevel"]); }
            set { HttpContext.Current.Session["OfficeLevel"] = value; }
        }
		public static string LogoffText
		{
			get { return ((HttpContext.Current.Session["LogoffText"] == null) ? "Logoff" : (string)HttpContext.Current.Session["LogoffText"]); }
			set { HttpContext.Current.Session["LogoffText"] = value; }
		}


		public static string browserId
		{
			get { return ((HttpContext.Current.Session["browserId"] == null) ? string.Empty : (string)HttpContext.Current.Session["browserId"]); }
			set { HttpContext.Current.Session["browserId"] = value; }
		}


		public static string localTokenId
		{
			get { return ((HttpContext.Current.Session["localTokenId"] == null) ? string.Empty : (string)HttpContext.Current.Session["localTokenId"]); }
			set { HttpContext.Current.Session["localTokenId"] = value; }
		}



		public static string ServiceSessionID
		{
			get { return ((HttpContext.Current.Session["ServiceSessionID"] == null) ? string.Empty : (string)HttpContext.Current.Session["ServiceSessionID"]); }
			set { HttpContext.Current.Session["ServiceSessionID"] = value; }
		}

		public static string Islogged 
		{
			get { return ((HttpContext.Current.Session["Islogged"] == null) ? string.Empty : (string)HttpContext.Current.Session["Islogged"]); }
			set { HttpContext.Current.Session["Islogged"] = value; }
		}
		public static string UserID
		{
			get { return ((HttpContext.Current.Session["UserID"] == null) ? string.Empty : (string)HttpContext.Current.Session["UserID"]); }
			set { HttpContext.Current.Session["UserID"] = value; }
		}


		public static string StateId
		{
			get { return ((HttpContext.Current.Session["StateId"] == null) ? string.Empty : (string)HttpContext.Current.Session["StateId"]); }
			set { HttpContext.Current.Session["StateId"] = value; }
		}

		public static string MbNo
		{
			get { return ((HttpContext.Current.Session["MbNo"] == null) ? string.Empty : (string)HttpContext.Current.Session["MbNo"]); }
			set { HttpContext.Current.Session["MbNo"] = value; }
		}
		public static string StartPageID
		{
			get { return ((HttpContext.Current.Session["StartPageID"] == null) ? string.Empty : (string)HttpContext.Current.Session["StartPageID"]); }
			set { HttpContext.Current.Session["StartPageID"] = value; }
		}



        public static string loginEmailID
        {
            get { return ((HttpContext.Current.Session["loginEmailID"] == null) ? string.Empty : (string)HttpContext.Current.Session["loginEmailID"]); }
            set { HttpContext.Current.Session["loginEmailID"] = value; }
        }
        public static string UserName
		{
			get { return ((HttpContext.Current.Session["UserName"] == null) ? string.Empty : (string)HttpContext.Current.Session["UserName"]); }
			set { HttpContext.Current.Session["UserName"] = value; }
		}

		public static string SignaturePath
		{
			get { return ((HttpContext.Current.Session["SignaturePath"] == null) ? string.Empty : (string)HttpContext.Current.Session["SignaturePath"]); }
			set { HttpContext.Current.Session["SignaturePath"] = value; }
		}


		public static string IsSecretoryId
		{
			get { return ((HttpContext.Current.Session["IsSecretoryId"] == null) ? string.Empty : (string)HttpContext.Current.Session["IsSecretoryId"]); }
			set { HttpContext.Current.Session["IsSecretoryId"] = value; }
		}

		public static string SecretaryId
		{
			get { return ((HttpContext.Current.Session["SecretaryId"] == null) ? string.Empty : (string)HttpContext.Current.Session["SecretaryId"]); }
			set { HttpContext.Current.Session["SecretaryId"] = value; }
		}


		public static string UserPicPath
		{
			get { return ((HttpContext.Current.Session["UserPicPath"] == null) ? string.Empty : (string)HttpContext.Current.Session["UserPicPath"]); }
			set { HttpContext.Current.Session["UserPicPath"] = value; }
		}

		public static string LoginDepartmentName
		{
			get { return ((HttpContext.Current.Session["LoginDepartmentName"] == null) ? string.Empty : (string)HttpContext.Current.Session["LoginDepartmentName"]); }
			set { HttpContext.Current.Session["LoginDepartmentName"] = value; }
		}

		public static string Name
		{
			get { return ((HttpContext.Current.Session["Name"] == null) ? string.Empty : (string)HttpContext.Current.Session["Name"]); }
			set { HttpContext.Current.Session["Name"] = value; }
		}

		public static string MemberDesignation
		{
			get { return ((HttpContext.Current.Session["MemberDesignation"] == null) ? string.Empty : (string)HttpContext.Current.Session["MemberDesignation"]); }
			set { HttpContext.Current.Session["MemberDesignation"] = value; }
		}

		public static string MemberID
		{

			get { return ((HttpContext.Current.Session["MemberID"] == null) ? string.Empty : (string)HttpContext.Current.Session["MemberID"]); }
			set { HttpContext.Current.Session["MemberID"] = value; }
		}

		public static string MemberCode
		{

			get { return ((HttpContext.Current.Session["MemberCode"] == null) ? string.Empty : (string)HttpContext.Current.Session["MemberCode"]); }
			set { HttpContext.Current.Session["MemberCode"] = value; }
		}

		public static string RoleID
		{
			get { return ((HttpContext.Current.Session["RoleID"] == null) ? string.Empty : (string)HttpContext.Current.Session["RoleID"]); }
			set { HttpContext.Current.Session["RoleID"] = value; }
		}

		public static string DeptID
		{
			get { return ((HttpContext.Current.Session["DeptID"] == null) ? string.Empty : (string)HttpContext.Current.Session["DeptID"]); }
			set { HttpContext.Current.Session["DeptID"] = value; }
		}

		public static string RoleName
		{
			get { return ((HttpContext.Current.Session["RoleName"] == null) ? string.Empty : (string)HttpContext.Current.Session["RoleName"]); }
			set { HttpContext.Current.Session["RoleName"] = value; }
		}

		public static string ConstituencyID
		{
			get { return ((HttpContext.Current.Session["ConstituencyID"] == null) ? string.Empty : (string)HttpContext.Current.Session["ConstituencyID"]); }
			set { HttpContext.Current.Session["ConstituencyID"] = value; }
		}

		public static string ConstituencyName
		{
			get { return ((HttpContext.Current.Session["ConstituencyName"] == null) ? string.Empty : (string)HttpContext.Current.Session["ConstituencyName"]); }
			set { HttpContext.Current.Session["ConstituencyName"] = value; }
		}
		public static string Designation
		{
			get { return ((HttpContext.Current.Session["Designation"] == null) ? string.Empty : (string)HttpContext.Current.Session["Designation"]); }
			set { HttpContext.Current.Session["Designation"] = value; }
		}

        public static string DesignationLocal
        {
            get { return ((HttpContext.Current.Session["DesignationLocal"] == null) ? string.Empty : (string)HttpContext.Current.Session["DesignationLocal"]); }
            set { HttpContext.Current.Session["DesignationLocal"] = value; }
        }

        public static string LoginStatus
		{
			get { return ((HttpContext.Current.Session["LoginStatus"] == null) ? string.Empty : (string)HttpContext.Current.Session["LoginStatus"]); }
			set { HttpContext.Current.Session["LoginStatus"] = value; }
		}

		public static DateTime LoginTime
		{
			get { return ((HttpContext.Current.Session["LoginTime"] == null) ? DateTime.Now : Convert.ToDateTime(HttpContext.Current.Session["LoginTime"])); }
			set { HttpContext.Current.Session["LoginTime"] = value; }
		}

        public static PdfSignatureAppearance Appearance
        {
            get { return HttpContext.Current.Session["Appearance"] as PdfSignatureAppearance; }
            set { HttpContext.Current.Session["Appearance"] = value; }
        }

        public static string Layer2Text
        {
            get { return ((HttpContext.Current.Session["Layer2Text"] == null) ? string.Empty : (string)HttpContext.Current.Session["Layer2Text"]); }
            set { HttpContext.Current.Session["Layer2Text"] = value; }
        }

        public static MemoryStream Fout
        {
            get { return HttpContext.Current.Session["Fout"] as MemoryStream; }
            set { HttpContext.Current.Session["Fout"] = value; }
        }


        public static string UserId_House
		{
			get;
			set;
		}
		public static string UserId_DeptReply
		{
			get;
			set;
		}
		public static string UserId_eConstituency
		{
			get;
			set;
		}


		public static string OfficeName
		{
			get;
			set;
		}
        public static string OfficeNameNew
        {
            get;
            set;
        }

        public static string DeptName
		{
			get;
			set;
		}
        public static string DeptNameNew
        {
            get;
            set;
        }




        public static string LanguageCulture
		{
			get
			{
				if (HttpContext.Current.Session != null && HttpContext.Current.Session["LanguageCulture"] != null)
				{
					return HttpContext.Current.Session["LanguageCulture"].ToString();
				}
				else
				{
					return string.Empty;
				}
			}
			set { HttpContext.Current.Session["LanguageCulture"] = value; }
		}

		public static String LanguageID
		{
			get
			{
				if (HttpContext.Current.Session != null && HttpContext.Current.Session["LanguageID"] != null)
				{
					return HttpContext.Current.Session["LanguageID"].ToString();
				}
				else
				{
					return "7C4F28F6-02FC-4627-993D-E255037531AD";
				}
			}
			set { HttpContext.Current.Session["LanguageID"] = value; }
		}

		public static String Language
		{
			get
			{
				if (HttpContext.Current.Session != null && HttpContext.Current.Session["Language"] != null)
				{
					return HttpContext.Current.Session["Language"].ToString();
				}
				else
				{
					return "English";
				}
			}
			set { HttpContext.Current.Session["Language"] = value; }
		}

		public static string AssemblyId
		{
			get { return ((HttpContext.Current.Session["AssemblyId"] == null) ? string.Empty : (string)HttpContext.Current.Session["AssemblyId"]); }
			set { HttpContext.Current.Session["AssemblyId"] = value; }
		}

		public static string SessionId
		{
			get { return ((HttpContext.Current.Session["SessionId"] == null) ? string.Empty : (string)HttpContext.Current.Session["SessionId"]); }
			set { HttpContext.Current.Session["SessionId"] = value; }
		}

		public static string ActionIds
		{
			get { return ((HttpContext.Current.Session["ActionIds"] == null) ? string.Empty : (string)HttpContext.Current.Session["ActionIds"]); }
			set { HttpContext.Current.Session["ActionIds"] = value; }
		}

		public static string SPSubject
		{
			get { return ((HttpContext.Current.Session["SPSubject"] == null) ? string.Empty : (string)HttpContext.Current.Session["SPSubject"]); }
			set { HttpContext.Current.Session["SPSubject"] = value; }
		}

		public static string SPMinId
		{
			get { return ((HttpContext.Current.Session["SPMinId"] == null) ? string.Empty : (string)HttpContext.Current.Session["SPMinId"]); }
			set { HttpContext.Current.Session["SPMinId"] = value; }
		}
		public static string SPFDate
		{
			get { return ((HttpContext.Current.Session["SPFDate"] == null) ? string.Empty : (string)HttpContext.Current.Session["SPFDate"]); }
			set { HttpContext.Current.Session["SPFDate"] = value; }
		}
		public static string SPTDate
		{
			get { return ((HttpContext.Current.Session["SPTDate"] == null) ? string.Empty : (string)HttpContext.Current.Session["SPTDate"]); }
			set { HttpContext.Current.Session["SPTDate"] = value; }
		}
		public static string SPDNum
		{
			get { return ((HttpContext.Current.Session["SPDNum"] == null) ? string.Empty : (string)HttpContext.Current.Session["SPDNum"]); }
			set { HttpContext.Current.Session["SPDNum"] = value; }
		}

		public static string MenuId
		{
			get { return ((HttpContext.Current.Session["MenuId"] == null) ? string.Empty : (string)HttpContext.Current.Session["MenuId"]); }
			set { HttpContext.Current.Session["MenuId"] = value; }
		}

		public static string TagId
		{
			get { return ((HttpContext.Current.Session["TagId"] == null) ? string.Empty : (string)HttpContext.Current.Session["TagId"]); }
			set { HttpContext.Current.Session["TagId"] = value; }
		}
		public static string FileAccessPath
		{
			get { return ((HttpContext.Current.Session["FileAccessPath"] == null) ? string.Empty : (string)HttpContext.Current.Session["FileAccessPath"]); }
			set { HttpContext.Current.Session["FileAccessPath"] = value; }
		}
		public static string FileStorePath
		{
			get { return ((HttpContext.Current.Session["FileStorePath"] == null) ? string.Empty : (string)HttpContext.Current.Session["FileStorePath"]); }
			set { HttpContext.Current.Session["FileStorePath"] = value; }
		}
		public static string AadharId
		{
			get { return ((HttpContext.Current.Session["AadharId"] == null) ? string.Empty : (string)HttpContext.Current.Session["AadharId"]); }
			set { HttpContext.Current.Session["AadharId"] = value; }
		}
		public static string Printing
		{
			get { return ((HttpContext.Current.Session["Printing"] == null) ? string.Empty : (string)HttpContext.Current.Session["Printing"]); }
			set { HttpContext.Current.Session["Printing"] = value; }
		}
        public static string BranchId
        {
            get { return ((HttpContext.Current.Session["BranchId"] == null) ? string.Empty : (string)HttpContext.Current.Session["BranchId"]); }
            set { HttpContext.Current.Session["BranchId"] = value; }
        }

        public static string BranchName
        {
            get { return ((HttpContext.Current.Session["BranchName"] == null) ? string.Empty : (string)HttpContext.Current.Session["BranchName"]); }
            set { HttpContext.Current.Session["BranchName"] = value; }
        }
        //added by robin to compare currentsession
        public static string SiteSettingsCurrentSessionId
        {
            get { return ((HttpContext.Current.Session["SiteSettingsCurrentSessionId"] == null) ? string.Empty : (string)HttpContext.Current.Session["SiteSettingsCurrentSessionId"]); }
            set { HttpContext.Current.Session["SiteSettingsCurrentSessionId"] = value; }
        }
        //added by robin to compare current assembly
        public static string SiteSettingsCurrentAssemblyId
        {
            get { return ((HttpContext.Current.Session["SiteSettingsCurrentAssemblyId"] == null) ? string.Empty : (string)HttpContext.Current.Session["SiteSettingsCurrentAssemblyId"]); }
            set { HttpContext.Current.Session["SiteSettingsCurrentAssemblyId"] = value; }
        }
		public static void Clear()
		{
			HttpContext.Current.Session.Remove("UserID");
			HttpContext.Current.Session.Remove("UserName");
			HttpContext.Current.Session.Remove("LanguageID");
			HttpContext.Current.Session.Remove("Language");
			HttpContext.Current.Session.Remove("MainMenu");
			HttpContext.Current.Session.Remove("Printing");

			HttpContext.Current.Session.Remove("CurrentAction");
			HttpContext.Current.Session.Remove("CurrentActionType");
			HttpContext.Current.Session.Remove("CurrentActionValue");
			HttpContext.Current.Session.Remove("CurrentController");
			HttpContext.Current.Session.Remove("CurrentModule");
			HttpContext.Current.Session.Remove("AssemblyId");
			HttpContext.Current.Session.Remove("SessionId");
			HttpContext.Current.Session.Remove("ActionIds");
			HttpContext.Current.Session.Remove("SPSubject");
			HttpContext.Current.Session.Remove("SPMinId");
			HttpContext.Current.Session.Remove("SPFDate");
			HttpContext.Current.Session.Remove("SPTDate");
			HttpContext.Current.Session.Remove("SPDNum");
			HttpContext.Current.Session.Remove("MenuId");
			HttpContext.Current.Session.Remove("TagId");
			HttpContext.Current.Session.Remove("FileAccessPath");
			HttpContext.Current.Session.Remove("FileStorePath");
			HttpContext.Current.Session.Remove("AadharId");
			HttpContext.Current.Session.Remove("UserUpdateFlag");
			HttpContext.Current.Session.Remove("isApex");
			HttpContext.Current.Session.Remove("ServiceSessionID");
			HttpContext.Current.Session.Remove("browserId");
			HttpContext.Current.Session.Remove("localTokenId");
            HttpContext.Current.Session.Remove("DeptNameNew");
            HttpContext.Current.Session.Remove("DeptName");
            HttpContext.Current.Session.Remove("OfficeNameNew");
            HttpContext.Current.Session.Remove("DeptID");
            HttpContext.Current.Session.Remove("OfficeId");
            HttpContext.Current.Session.Remove("Designation");
            HttpContext.Current.Session.Remove("SubUserTypeID");
        }

		public static void ClearSessions()
		{
			CurrentSession.UserID = "";
			CurrentSession.UserName = "";
			CurrentSession.RoleID = "";
			CurrentSession.RoleName = "";
			CurrentSession.DeptID = "";
			CurrentSession.OfficeId = "";
			CurrentSession.DeptNameNew = "";
            CurrentSession.DeptName = "";
            CurrentSession.Name = "";
			CurrentSession.MemberID = "";
			CurrentSession.ConstituencyID = "";
			CurrentSession.ConstituencyName = "";
			CurrentSession.UserPicPath = "";
			CurrentSession.AssemblyId = "";
			CurrentSession.SessionId = "";
			CurrentSession.ActionIds = "";
			CurrentSession.SPSubject = "";
			CurrentSession.SPMinId = "";
			CurrentSession.SPFDate = "";
			CurrentSession.SPTDate = "";
			CurrentSession.SPDNum = "";
			CurrentSession.MenuId = "";
			CurrentSession.TagId = "";
			CurrentSession.FileAccessPath = "";
			CurrentSession.FileStorePath = "";
			CurrentSession.AadharId = "";
			CurrentSession.Printing = "";
			CurrentSession.UserUpdateFlag = "";
			CurrentSession.DeptName = "";
			CurrentSession.OfficeName = "";
            CurrentSession.OfficeNameNew = "";
            CurrentSession.isApex = "";
			CurrentSession.ServiceSessionID = "";
			CurrentSession.browserId = "";
			CurrentSession.localTokenId = "";
			CurrentSession.DeviceName = "";
			CurrentSession.Designation = "";
            CurrentSession.SubUserTypeID = "";
            


            //Session.Abandon();

        }

		public static void InitializeValues()
		{
			UserName = string.Empty;
			HttpContext.Current.Session["PageTitle"] = "Punjab - CMNirdesh";

			LanguageID = "7C4F28F6-02FC-4627-993D-E255037531AD";
			Language = "ENGLISH";
			UserID = string.Empty;

			CurrentAction = "Index";
			CurrentController = "Home";
			CurrentModule = "Home";

			LoginText = "Login";
			LogoffText = "Logoff";

			
			//UpdateMenuandModules();
		}

		public static void GetMainMenuList()
		{
			//RemoveMainMenu();
			//MainMenu = ServiceAdaptor.GetMenuList(UserID, LanguageID);
		}

		public static String CurrentAction
		{
			get { return ((HttpContext.Current.Session["CurrentAction"] == null) ? "Index" : HttpContext.Current.Session["CurrentAction"].ToString()); }
			set { HttpContext.Current.Session["CurrentAction"] = value; }
		}

		public static String DefaultAction
		{
			get
			{

				if (null != System.Configuration.ConfigurationManager.AppSettings["DefaultAction"])
				{
					return System.Configuration.ConfigurationManager.AppSettings["DefaultAction"].ToString();
				}


				return string.Empty;
			}

		}





        public static String DefaultController
		{
			get
			{


				if (null != System.Configuration.ConfigurationManager.AppSettings["DefaultController"])
				{
					return System.Configuration.ConfigurationManager.AppSettings["DefaultController"].ToString();
				}


				return string.Empty;



			}

		}

		public static String DefaultModule
		{
			get
			{


				if (null != System.Configuration.ConfigurationManager.AppSettings["DefaultModule"])
				{
					return System.Configuration.ConfigurationManager.AppSettings["DefaultModule"].ToString();
				}


				return string.Empty;


			}

		}

		public static String DefaultLoginAction
		{
			get
			{

				if (null != System.Configuration.ConfigurationManager.AppSettings["DefaultLoginAction"])
				{
					return System.Configuration.ConfigurationManager.AppSettings["DefaultLoginAction"].ToString();
				}


				return string.Empty;
			}

		}

		public static String DefaultLoginController
		{
			get
			{


				if (null != System.Configuration.ConfigurationManager.AppSettings["DefaultLoginController"])
				{
					return System.Configuration.ConfigurationManager.AppSettings["DefaultLoginController"].ToString();
				}


				return string.Empty;



			}

		}

		public static String DefaultLoginModule
		{
			get
			{
				if (null != System.Configuration.ConfigurationManager.AppSettings["DefaultLoginModule"])
				{
					return System.Configuration.ConfigurationManager.AppSettings["DefaultLoginModule"].ToString();
				}


				return string.Empty;


			}
		}

		public static String CurrentActionType
		{
			get { return ((HttpContext.Current.Session["CurrentActionType"] == null) ? "" : HttpContext.Current.Session["CurrentActionType"].ToString()); }
			set { HttpContext.Current.Session["CurrentActionType"] = value; }
		}

		public static String CurrentActionValue
		{
			get { return ((HttpContext.Current.Session["CurrentActionValue"] == null) ? "" : HttpContext.Current.Session["CurrentActionValue"].ToString()); }
			set { HttpContext.Current.Session["CurrentActionValue"] = value; }
		}

		public static String CurrentController
		{
			get { return ((HttpContext.Current.Session["CurrentController"] == null) ? "Home" : HttpContext.Current.Session["CurrentController"].ToString()); }
			set { HttpContext.Current.Session["CurrentController"] = value; }
		}

		public static String CurrentModule
		{
			get { return ((HttpContext.Current.Session["CurrentModule"] == null) ? "Home" : HttpContext.Current.Session["CurrentModule"].ToString()); }
			set { HttpContext.Current.Session["CurrentModule"] = value; }
		}

        public static String DeviceName
        {
            get { return ((HttpContext.Current.Session["DeviceName"] == null) ? "" : HttpContext.Current.Session["DeviceName"].ToString()); }
            set { HttpContext.Current.Session["DeviceName"] = value; }
        }




        public static void UpdateMenuandModules()
		{
			GetMainMenuList();
			//GetAvailableModules();
		}

		public static List<string> AvailableModules
		{
			get { return ((HttpContext.Current.Session["AvailableModules"] == null) ? new List<string>() : HttpContext.Current.Session["AvailableModules"] as List<string>); }
			set { HttpContext.Current.Session["AvailableModules"] = value; }
		}

		public static void RemoveAvailableModules()
		{
			try
			{
				if (null != HttpContext.Current.Session["AvailableModules"])
					HttpContext.Current.Session.Remove("AvailableModules");
			}
			catch { }
		}

		

		public static string Name_Hindi
		{
			get { return ((HttpContext.Current.Session["Name_Hindi"] == null) ? string.Empty : (string)HttpContext.Current.Session["Name_Hindi"]); }
			set { HttpContext.Current.Session["Name_Hindi"] = value; }
		}

		public static String MemberLocation
		{
			get { return ((HttpContext.Current.Session["MemberLocation"] == null) ? string.Empty : (string)HttpContext.Current.Session["MemberLocation"]); }
			set { HttpContext.Current.Session["MemberLocation"] = value; }
		}

		public static string OldUserName
		{
			get { return ((HttpContext.Current.Session["OldUserName"] == null) ? string.Empty : (string)HttpContext.Current.Session["OldUserName"]); }
			set { HttpContext.Current.Session["OldUserName"] = value; }
		}

		public static string ConstituencyName_Local
		{
			get { return ((HttpContext.Current.Session["ConstituencyName_Local"] == null) ? string.Empty : (string)HttpContext.Current.Session["ConstituencyName_Local"]); }
			set { HttpContext.Current.Session["ConstituencyName_Local"] = value; }
		}

		public static string UserHashKey
		{
			get { return ((HttpContext.Current.Session["UserHashKey"] == null) ? string.Empty : (string)HttpContext.Current.Session["UserHashKey"]); }
			set { HttpContext.Current.Session["UserHashKey"] = value; }
		}

		public static string IsHOD
		{
			get { return ((HttpContext.Current.Session["IsHOD"] == null) ? string.Empty : (string)HttpContext.Current.Session["IsHOD"]); }
			set { HttpContext.Current.Session["IsHOD"] = value; }
		}
		public static string SubUserTypeID
		{
			get { return ((HttpContext.Current.Session["SubUserTypeID"] == null) ? string.Empty : (string)HttpContext.Current.Session["SubUserTypeID"]); }
			set { HttpContext.Current.Session["SubUserTypeID"] = value; }
		}
      
        public static string UserTypeConstituencyId
        {
            get { return ((HttpContext.Current.Session["UserTypeConstituencyId"] == null) ? string.Empty : (string)HttpContext.Current.Session["UserTypeConstituencyId"]); }
            set { HttpContext.Current.Session["UserTypeConstituencyId"] = value; }
        }

		public static string MemberPrefix
		{
			get { return ((HttpContext.Current.Session["Prefix"] == null) ? string.Empty : (string)HttpContext.Current.Session["Prefix"]); }
			set { HttpContext.Current.Session["Prefix"] = value; }
		}
		public static string MemberEmail
		{
			get { return ((HttpContext.Current.Session["Email"] == null) ? string.Empty : (string)HttpContext.Current.Session["Email"]); }
			set { HttpContext.Current.Session["Email"] = value; }
		}
		public static string MemberMobile
		{
			get { return ((HttpContext.Current.Session["Mobile"] == null) ? string.Empty : (string)HttpContext.Current.Session["Mobile"]); }
			set { HttpContext.Current.Session["Mobile"] = value; }
		}

		public static string UserUpdateFlag
		{
			get { return ((HttpContext.Current.Session["UserUpdateFlag"] == null) ? string.Empty : (string)HttpContext.Current.Session["UserUpdateFlag"]); }
			set { HttpContext.Current.Session["UserUpdateFlag"] = value; }
		}
		public static string OfficeId
		{
			get { return ((HttpContext.Current.Session["OfficeId"] == null) ? string.Empty : (string)HttpContext.Current.Session["OfficeId"]); }
			set { HttpContext.Current.Session["OfficeId"] = value; }
		}
        public static string MCName
        {
            get { return ((HttpContext.Current.Session["MCName"] == null) ? string.Empty : (string)HttpContext.Current.Session["MCName"]); }
            set { HttpContext.Current.Session["MCName"] = value; }
        }

		public static string ApprovalAuthority
		{
            get { return ((HttpContext.Current.Session["ApprovalAuthority"] == null) ? string.Empty : (string)HttpContext.Current.Session["ApprovalAuthority"]); }
            set { HttpContext.Current.Session["ApprovalAuthority"] = value; }
        }

        public static string isApex
		{
			get { return ((HttpContext.Current.Session["isApex"] == null) ? string.Empty : (string)HttpContext.Current.Session["isApex"]); }
			set { HttpContext.Current.Session["isApex"] = value; }
		}
        public static string MapId
        {
            get { return ((HttpContext.Current.Session["MapId"] == null) ? string.Empty : (string)HttpContext.Current.Session["MapId"]); }
            set { HttpContext.Current.Session["MapId"] = value; }
        }
        public static string isDeptApex
        {
            get { return ((HttpContext.Current.Session["isDeptApex"] == null) ? string.Empty : (string)HttpContext.Current.Session["isDeptApex"]); }
            set { HttpContext.Current.Session["isDeptApex"] = value; }
        }

        public static string MemberUserID
		{
			get { return ((HttpContext.Current.Session["MemberUserID"] == null) ? string.Empty : (string)HttpContext.Current.Session["MemberUserID"]); }
			set { HttpContext.Current.Session["MemberUserID"] = value; }
		}

        public static string PaperStatus
        {
            get { return ((HttpContext.Current.Session["PaperStatus"] == null) ? string.Empty : (string)HttpContext.Current.Session["PaperStatus"]); }
            set { HttpContext.Current.Session["PaperStatus"] = value; }
        }
        public static string IsUploadWordFile
        {
            get { return ((HttpContext.Current.Session["IsUploadWordFile"] == null) ? string.Empty : (string)HttpContext.Current.Session["IsUploadWordFile"]); }
            set { HttpContext.Current.Session["IsUploadWordFile"] = value; }
        }
        public static string IsUploadPdfFile
        {
            get { return ((HttpContext.Current.Session["IsUploadPdfFile"] == null) ? string.Empty : (string)HttpContext.Current.Session["IsUploadPdfFile"]); }
            set { HttpContext.Current.Session["IsUploadPdfFile"] = value; }
        }
        public static string SubDivisionId
        {
            get { return ((HttpContext.Current.Session["SubDivisionId"] == null) ? string.Empty : (string)HttpContext.Current.Session["SubDivisionId"]); }
            set { HttpContext.Current.Session["SubDivisionId"] = value; }
        }
        public static string DistrictCode
        {
            get { return ((HttpContext.Current.Session["DisttCode"] == null) ? string.Empty : (string)HttpContext.Current.Session["DisttCode"]); }
            set { HttpContext.Current.Session["DisttCode"] = value; }
        }
        public static string SetSessionAlive
        {
            get { return ((HttpContext.Current.Session["SetSessionAlive"] == null) ? string.Empty : (string)HttpContext.Current.Session["SetSessionAlive"]); }
            set { HttpContext.Current.Session["SetSessionAlive"] = value; }
        }
        //public static string SetDiary_Itemdata
        //{
        //    get { return ((HttpContext.Current.Session["SetDiary_Itemdata"] == null) ? string.Empty : (string)HttpContext.Current.Session["SetDiary_Itemdata"]); }
        //    set { HttpContext.Current.Session["SetDiary_Itemdata"] = value; }
        //}
        public static string parentYN
        {
            get { return ((HttpContext.Current.Session["parentYN"] == null) ? string.Empty : (string)HttpContext.Current.Session["parentYN"]); }
            set { HttpContext.Current.Session["parentYN"] = value; }
        }
        public static string workid
        {
            get { return ((HttpContext.Current.Session["workid"] == null) ? string.Empty : (string)HttpContext.Current.Session["workid"]); }
            set { HttpContext.Current.Session["workid"] = value; }
        }
        public static string serviceName
        {
            get { return ((HttpContext.Current.Session["serviceName"] == null) ? string.Empty : (string)HttpContext.Current.Session["serviceName"]); }
            set { HttpContext.Current.Session["serviceName"] = value; }
        }
        public static long referenceId
        {
            get { return (HttpContext.Current.Session["referenceId"] == null) ? (long)0 : (long)HttpContext.Current.Session["referenceId"]; }
            set { HttpContext.Current.Session["referenceId"] = value; }
        }
        public static string officeType
        {
            get { return ((HttpContext.Current.Session["officeType"] == null) ? string.Empty : (string)HttpContext.Current.Session["officeType"]); }
            set { HttpContext.Current.Session["officeType"] = value; }
        }

        public static string RCerificate
        {
            get { return ((HttpContext.Current.Session["RCerificate"] == null) ? string.Empty : (string)HttpContext.Current.Session["RCerificate"]); }
            set { HttpContext.Current.Session["RCerificate"] = value; }
        }
    }
}