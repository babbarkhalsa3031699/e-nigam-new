using CMNirdesh.Models.Diaries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class NevaStarredQuestion
    {
        [RegularExpression("^[0-9]*$", ErrorMessage = "RecordId must be numeric")]
        public int RecordId { get; set; }
        [Required(ErrorMessage = "Title Required!")]
        public string Heading { get; set; }
        public string HeadingLocal { get; set; }
        //[Required(ErrorMessage = "File Required!")]
        public string FilePath { get; set; }
        public string FilePathLocal { get; set; }
        public string CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public string Mode { get; set; }
        public byte Status { get; set; }        
        public HttpPostedFileBase file { get; set; }
        public HttpPostedFileBase fileLocal { get; set; }
        public int AssemblyId { get; set; }
        public int SessionId { get; set; }
        public string SessionDateId { get; set; }
        public int DepartmentID { get; set; }

        public int MinistryID { get; set; }

        
        public int QuestionId { get; set; }

        public string QuestionNumber { get; set; }

        public string QuestionType { get; set; }

        public string Subject { get; set; }

        public string Questions { get; set; }

        public string DepartmentLocal { get; set; }
        public string MemberCode { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string AskedBy { get; set; }
        public string Reply { get; set; }

        public int PaperLaidId { get; set; }
        
        public string ReplyPDF { get; set; }
        public string ConstituencyNameLocal { get; set; }
        public string ConstituencyName { get; set; }
        public string MainQuestion { get; set; }

        public DateTime? LetterDate { get; set; }

        public string MinisteryNameLocal { get; set; }
        public string MinisteryName { get; set; }

        public string MemberName { get; set; }
        public string MemberNameLocal { get; set; }

        public string Department { get; set; }
        
        public string SessionDate { get; set; }
        [NotMapped]

        public int AssemblyCode { get; set; }

        public int Id { get; set; }
        public string AssemblyName { get; set; }

        public string SessionName { get; set; }


        public string AssemblyCodee { get; set; }

        public string SessionCodee { get; set; }

 

        public int SessionCode { get; set; }

        public SelectList SessionList { get; set; }

        public SelectList SessDatelist { get; set; }

        public SelectList AssemblyList { get; set; }




        public List<NevaStarredQuestion> mQuestiionList { get; set; }
        [NotMapped]
        public string ErrorMessage { get; set; }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 RefId { get; set; }
        public Int64 LinkRefID { get; set; }
        [Required(ErrorMessage = "Please Select From Department")]

        public string FromDeptId { get; set; }
        [Required(ErrorMessage = "Please Select From Office")]
        public int FromOfficeId { get; set; }

        public int AssinedOfficeId { get; set; }
        public int MoveId { get; set; }

        public string agendaId { get; set; }
        public string MapId { get; set; }
        public Int64 MainRefId { get; set; }
        public string RefFileType { get; set; }
        public string MultiUpdate { get; set; }
        public string SwipeId { get; set; }
        public int MeetingId { get; set; }
        public int? DispatchNumber { get; set; }
        public string ToDeptId { get; set; }
        public int ToOfficeId { get; set; }
        public int Memembercode { get; set; }

        public int MinisterCode { get; set; }
        


        public string DiaryNumber { get; set; }

        public string DocmentType { get; set; }
        public int ReferencePriorityType { get; set; }
        public int ReferenceTerm { get; set; }
        [Required(ErrorMessage = "Please Select Priority Type")]
        public string LetterNo { get; set; }


        public string LetterDateNew { get; set; }
    

        //[UIHint("tinymce_jquery_Diary"), AllowHtml]
        [AllowHtml]
        public string SubjectDetails { get; set; }

        public string EnclosureFile { get; set; }

        public string isLGoffice { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
        public string ApplicantMobile { get; set; }

        public string ApplicantEmail { get; set; }

        public DateTime? DiaryDate { get; set; }
        public DateTime? ModifyDate { get; set; }

     
        public int? ActionCode { get; set; }
        public int? ActionCodeMulti { get; set; }

        public string SentToOffice { get; set; }
        public string SentFromOffice { get; set; }
        public string SentToUserId { get; set; }
        public string SentFromUserId { get; set; }

        public SelectList FromOfficeList { get; set; }
        public SelectList ToOfficeList { get; set; }

        public SelectList FromUserList { get; set; }
        public SelectList ToUserList { get; set; }

        public string HideUpdate { get; set; }
        public string HideRef { get; set; }

        public string ResolutionNo { get; set; }

        public string ResCount { get; set; }
        public DateTime? ActiontakenDate { get; set; }

        //[UIHint("tinymce_jquery_DiaryAction"), AllowHtml]
        [AllowHtml]
        public string ActionDescription { get; set; }

        [AllowHtml]
        public string ActionDescriptionMulti { get; set; }
        public double? ActionAmountSanction { get; set; }
        public string ActionTakenFile { get; set; }
        [NotMapped]
        public string CurrentDate
        {

            get;
            set;
        }

        public int? ActionTypeid
        {

            get;
            set;
        }


        [NotMapped]
        public int? DiaryActionCode
        {
            get; set;


        }




        [NotMapped]
        public string IsFile { get; set; }

        [NotMapped]
        public string IsEnclosureFile { get; set; }

        [NotMapped]
        public string isLetter { get; set; }

        [NotMapped]
        public int OfficeId { get; set; }


        public int ActId { get; set; }
        [NotMapped]
        public string DeptId { get; set; }
        [NotMapped]
        public string officename { get; set; }

        [NotMapped]
        public string deptname { get; set; }

        [NotMapped]
        public string PaperEntryType { get; set; }

        [NotMapped]
        public SelectList DocumentTypelist { get; set; }

        [NotMapped]
        public SelectList Act { get; set; }
        [NotMapped]
  

        public SelectList FileDocumentTypelist { get; set; }

        public string ActName { get; set; }
        public int IsFileCount { get; set; }

        [NotMapped]
        public SelectList MeetingList { get; set; }

        public SelectList FileList { get; set; }


        [NotMapped]
        public SelectList ReferencePriorityTypeList { get; set; }
        [NotMapped]
        public SelectList ActionTypeList { get; set; }


        [NotMapped]
        public SelectList ActionTypeList_Ref { get; set; }

        [NotMapped]
        public SelectList ActionListFilter { get; set; }

        [NotMapped]
        public SelectList mDepartmentList { get; set; }

        public List<AssemblySelectItem> mAssemblyList { get; set; }

        [NotMapped]
        public List<DepartmentSelectListItem> mDepartmentListPbi { get; set; }
        [NotMapped]
        public SelectList OfficeList { get; set; }

        public SelectList MinisterList { get; set; }

        public SelectList ActList { get; set; }
        public SelectList MemberList { get; set; }





        public string BtnCaption { get; set; }
        [NotMapped]
        public string ToOffice { get; set; }
        [NotMapped]
        public string ToDept { get; set; }

        [NotMapped]
        public string FromDept { get; set; }
        [NotMapped]
        public string FromOffice { get; set; }
        [NotMapped]
        public string DocumentTypeName { get; set; }
        public string DocumentType { get; set; }

        [NotMapped]
        public List<DiaryActionData> ActionDetailsList { get; set; }

        [NotMapped]
        public List<MlaDiaryData> myDiaryList { get; set; }


        public bool CopyTo { get; set; }

        [NotMapped]
        public List<TemplateList> TemplateList { get; set; }

        [NotMapped]
        public int isfund { get; set; }

        [NotMapped]
        public string FundName { get; set; }


        [NotMapped]
        public int Availablefund { get; set; }
        [NotMapped]
        public int SantionAmount { get; set; }

        [NotMapped]
        public DateTime SanctionDate { get; set; }

        [NotMapped]
        public DateTime ApproveDate { get; set; }

        [NotMapped]
        public string Remarks { get; set; }

        [NotMapped]
        public List<FundReportModel> _LstFundReport { get; set; }

        [NotMapped]
        public List<FundReportModel> _LstFundSactioned { get; set; }

        [NotMapped]
        public int fundtype_3 { get; set; }

        [NotMapped]
        public int fundtype_4 { get; set; }

        [NotMapped]
        public int fundtype_13 { get; set; }
        [NotMapped]
        public int fundtype_14 { get; set; }
        [NotMapped]
        public int fundtype_15 { get; set; }


        [NotMapped]
        public int San_ReliefFunds { get; set; }

        [NotMapped]
        public int San_SmallSavingFunds { get; set; }

        [NotMapped]
        public int San_UntiedFunds { get; set; }
        [NotMapped]
        public int San_VivekiFunds { get; set; }
        [NotMapped]
        public int San_CattleFairFunds { get; set; }

        public string EditType { get; set; }

        public string SentType { get; set; }



    }


  
    




}

