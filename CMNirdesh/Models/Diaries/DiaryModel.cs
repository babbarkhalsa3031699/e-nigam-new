


using CMNirdesh.Models.Session;
using CMNIrdesh.Models.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace CMNirdesh.Models.Diaries
{
    [Serializable]
    public class DiaryModel
    {
        public DiaryModel()
        {
            this.memberList = new List<DiaryModel>();            
            this.DiaryList = new List<DiaryModel>();
            this.SessList = new List<mSession>();
            this.SessDateList = new List<mSessionDate>();
            this.DeptList = new List<mDepartment>();           
        }

        public int AssemblyID { get; set; }
        public int SessionID { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Notice { get; set; }


        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Subject { get; set; }

        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Subject2 { get; set; }


        public bool IsPrinted { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Date Required")]
        public DateTime NoticeDate { get; set; }

        [Required(ErrorMessage = "Time Required")]
        public TimeSpan NoticeTime { get; set; }

        public DateTime? FixedDate { get; set; }

        public string NoticeReply { get; set; }
        [Required(ErrorMessage = "Please Select Department")]
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Please Select Minister")]
        public int? MinistryId { get; set; }
        public int? MinsiterId { get; set; }
        public string CurrentUserName { get; set; }
        public string UserName { get; set; }
        public string UserDesignation { get; set; }
        public int TotalDiaryCnt { get; set; }
        public int SCount { get; set; }
        public int UCount { get; set; }
        public int NCount { get; set; }
        public int NPostCount { get; set; }
        public string DiaryNumber { get; set; }
        public string AskedBy { get; set; }
        public DateTime RecievedDateTime { get; set; }
        public int QuestionId { get; set; }
        public string NoticeNumber { get; set; }
        public string ManualNoticeNumber { get; set; }
        public int NoticeId { get; set; }
        public string PaperEntryType { get; set; }
        public int QuestionTypeId { get; set; }

        [Required(ErrorMessage = "Please Select BusinessType")]
        public int EventId { get; set; }
        [Required(ErrorMessage = "Please Select Member")]
        public int MemberId { get; set; }
        public int? MemId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string MemberName { get; set; }
        public string BusinessType { get; set; }
        public string DepartmentNameLocal { get; set; }
        public string MinistryNameLocal { get; set; }
        public string MinistryName { get; set; }
        public string ViewType { get; set; }
        public string BtnCaption { get; set; }
        public string SubmittedBy { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string MainQuestion { get; set; }
        public int ConstituencyCode { get; set; }
        public string ConstituencyName { get; set; }
        [Required(ErrorMessage = "Please Select Date")]
        public int? SessionDateId { get; set; }
        public int? QuestionNumber { get; set; }
        public string EventName { get; set; }
        public string MinisterName { get; set; }
        [Required(ErrorMessage = "Please Enter Date")]
        public string stringDate { get; set; }
        public string AssesmblyName { get; set; }
        public string SessionName { get; set; }
        public IList<DiaryModel> memberList { get; set; }
        public IList<DiaryModel> MinistryList { get; set; }
       
        public virtual ICollection<DiaryModel> DiaryList { get; set; }
        public virtual ICollection<mSession> SessList { get; set; }
        public virtual ICollection<mSessionDate> SessDateList { get; set; }

        public virtual ICollection<mDepartment> DeptList { get; set; }

       
        public virtual ICollection<DiaryModel> CurrentFixedQuestionsList { get; set; }
        public virtual ICollection<DiaryModel> PostponeFixedQuestionsList { get; set; }
        public DateTime? DateForBind { get; set; }
        public TimeSpan? TimeForBind { get; set; }
        public int SInbox { get; set; }
        public int SPending { get; set; }
        public int SSent { get; set; }
        public int UInbox { get; set; }
        public int UPending { get; set; }
        public int USent { get; set; }
        public int NPending { get; set; }
        public int NInbox { get; set; }
        public int NSent { get; set; }
        public int STotalCnt { get; set; }
        public int UTotalCnt { get; set; }
        public int NTotalCnt { get; set; }

        public int BPending { get; set; }
        public int BSent { get; set; }
        public int BTotalCnt { get; set; }

        public int CPending { get; set; }
        public int CSent { get; set; }
        public int CTotalCnt { get; set; }

        public int OPPending { get; set; }
        public int OPSent { get; set; }
        public int OPTotalCnt { get; set; }

        public bool IsNoticePrinted { get; set; }

        public string CatchWordSubject { get; set; }

        public string StringTime { get; set; }

        #region Variable Declaration For Notices Grid (Pending and Published.)
        [NotMapped]
        public int PageSize { get; set; }
        [NotMapped]
        public int PageIndex { get; set; }
        [NotMapped]
        public int ResultCount { get; set; }
        [NotMapped]
        public int loopStart { get; set; }
        [NotMapped]
        public int loopEnd { get; set; }
        [NotMapped]
        public int selectedPage { get; set; }
        [NotMapped]
        public int PageNumber { get; set; }
        [NotMapped]
        public int RowsPerPage { get; set; }
        #endregion
        public bool? IsContentFreeze { get; set; }
        public bool? IsDetailFreeze { get; set; }
        public bool? IsQuestionFreeze { get; set; }
        public bool? IsInitialApprove { get; set; }
        public bool? IsFinalApprove { get; set; }
        public string RejectRemark { get; set; }
        public string ChangeTypeRemarks { get; set; }
        public string RIds { get; set; }
        public string Message { get; set; }
        // for QuestionRule 
        public int RuleId { get; set; }
        public string RuleName { get; set; }

       
        [HiddenInput(DisplayValue = false)]
        public string lSM { get; set; }
        public int? QuestionStatus { get; set; }

        public int SUnfixCount { get; set; }
        public int SfixCount { get; set; }

        public int? version { get; set; }
        public string QuesIds { get; set; }
        public string FixNums { get; set; }
        public string ManualFixNo { get; set; }
        public DateTime? DeptSubmittedDate { get; set; }
        public int SNewlyDiaried { get; set; }
        public int UNewlyDiaried { get; set; }
        public int NNewlyDiaried { get; set; }

        public bool IsPostPone { get; set; }
        public DateTime? IsPostPoneDate { get; set; }
        public string Flag { get; set; }
        public DateTime? MinisterSubmittedDate { get; set; }
        public int? Priority { get; set; }
        public string ChkLocalLang { get; set; }
        public bool? IsBracket { get; set; }
        public bool IsTranslation { get; set; }

        public int NoticeStatus { get; set; }
        public bool? IsHindi { get; set; }
        public bool IsSubmitToVS { get; set; }
        public string AttachFilePath { get; set; }
        public string AttachFileName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public DateTime? FromDate_DateTime { get; set; }
        public DateTime? ToDate_DateTime { get; set; }

        //for Dept on 12/11/2014
        public int ModuleId { get; set; }

       
        public List<mSession> sessionList { get; set; }

        public string SignFilePath { get; set; }
        public string SignFileName { get; set; }
        public Guid UserId { get; set; }
        public bool? IsQuestionInPart { get; set; }

        // This Two Field is Added for Optional Entry In Diary Section on 12/02/2015
        public int? OptMinistryId { get; set; }
        public string OptDepartmentId { get; set; }

        //Added code venkat for dynamic menu
        [NotMapped]
        public string ModuleName { get; set; }
       
        //[NotMapped]
        //[HiddenInput(DisplayValue = false)]
        //public virtual List<mEvent> EvntList { get; set; }
        public string MobileNumber { get; set; }
        public string OTPQ { get; set; }
        public string EnterOTP { get; set; }
        public int OTPID { get; set; }
        public string PDfDate { get; set; }
        public string PDfTime { get; set; }
        public bool CQPDFApprdBySert { get; set; }
        public bool PQPDFApprdBySert { get; set; }

        public int SPPCnt { get; set; }
        public int UPPCnt { get; set; }

        public bool? IsTyipstReqired { get; set; }
        public string TypistNeede { get; set; }
        public int? CMDemandId { get; set; }
        public string RuleNo { get; set; }
        public bool IsOnlineSubmitted { get; set; }
        public string CutMotionFileName { get; set; }
        public string CutMotionFilePath { get; set; }
        public int? DemandNo { get; set; }

        public string OnLineAttachFilePath { get; set; }
        public string OnLineAttachFileName { get; set; }
        public string DemandName { get; set; }

        public DateTime FixSessionDate { get; set; }
        public int FixSessionDateId { get; set; }
        [NotMapped]
        public string UserGuid { get; set; }
        [NotMapped]
        public DateTime DateEntry { get; set; }
        public string ButtonOnlineShow { get; set; }

        public string CurrentQuesIds { get; set; }
        public string PostPoneQuesIds { get; set; }
        public string SessionDate { get; set; }
        [HiddenInput(DisplayValue = false)]
        public bool UnstarredApprovedTrans { get; set; }
        [HiddenInput(DisplayValue = false)]
        public bool UnstarredPQApprovedTrans { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string RoolId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int StarredCountExess { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int StarredCountExessed { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int UnStarredCountExess { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int UnStarredCountExessed { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int StarredCountForWithdrawn { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int StarredWithdrawnCount { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int UnstarredCountForWithdrawn { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int UntarredWithdrawnCount { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string TimeBarredDt { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime? MatchDateTimeBarred { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int countClub { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int countClubbed { get; set; }
        [Required(ErrorMessage = "Please Select Question Language")]
        public int? QuestPdfLang { get; set; }
        public string ConsituencyLocal { get; set; }
        public bool? IsFixed { get; set; }
        public string DeActivateFlag { get; set; }
        [NotMapped]
        public string SaveFilePath { get; set; }
        [NotMapped]
        public int MemberCode { get; set; }
        [NotMapped]
        public string StarredQListPath { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string AcessPath { get; set; }

        public int InboxCount { get; set; }

        public int DraftCount { get; set; }
        //[NotMapped]
        //public string DashboardSession { get; set; }

        public int? DesiredQuestionDateId { get; set; }

        //[NotMapped]
        //public SelectList SessionDateList { get; set; }

        public List<mSessionDate> SessionDateList { get; set; }
        //[NotMapped]
        //public string DashboardSession { get; set; }

    }
}