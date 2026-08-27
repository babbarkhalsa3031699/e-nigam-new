using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using CMNirdesh.Models;


namespace CMNirdesh.Models.Diaries
{
    [Serializable]
    [Table("mMlaDiaryDepartment")]
    public class MlaDiaryDepartment
    {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int mlaDiaryId { get; set; }
            public int? RecordNumber { get; set; }
            [Required(ErrorMessage = "Please Select Document Type")]
            public string DocmentType { get; set; }
            public string Subject { get; set; }
            public int? MinisterId { get; set; }
            public string DeptId { get; set; }
            public int? ActionCode { get; set; }
            public DateTime? ForwardDate { get; set; }
            public DateTime? DiaryDate { get; set; }
            public DateTime? ModifyDate { get; set; }
            public int? DiaryLanguage { get; set; }
            public string DocumentCC { get; set; }
            public string DiaryRefId { get; set; }

            //
            public string Subject2 { get; set; }
            //

            [NotMapped]
            public string ForwardDateForUse { get; set; }
            [UIHint("tinymce_jquery_full"), AllowHtml]
            public string ItemDescription { get; set; }
            [UIHint("tinymce_jquery_full"), AllowHtml]
            public string Remarks { get; set; }
            [UIHint("tinymce_jquery_full"), AllowHtml]
            public string letterDescription { get; set; }
            [NotMapped]
            public string RawItemDescription { get; set; }
            [NotMapped]
            public string hdnItemDescription { get; set; }
            public string ForwardedFile { get; set; }
            public string CreatedBy { get; set; }
            public int? AssemblyId { get; set; }
            public int? ConstituencyCode { get; set; }
            public int? MlaCode { get; set; }
            public string RecieveFrom { get; set; }
            //public Int32? MobileNo { get; set; }
            public string MobileNumber { get; set; }
            public string ApplicantName { get; set; }
            public string ApplicantAddress { get; set; }
            public string ApplicantMobile { get; set; }
            public string PresentPlace { get; set; }
            public string ProposedPlace { get; set; }
            public int? OfficeId { get; set; }
            public string OfficeId_CC { get; set; }
            [NotMapped]
            public List<string> AllOfficeIds { get; set; }
            public int[] SelectedValues { get; set; }

            public string LetterNo { get; set; }
            public int? SubDivisionId { get; set; }
            public string ActiontakenFile { get; set; }
            public DateTime? ActiontakenDate { get; set; }
            [NotMapped]
            public string ActiontakenDateForUse { get; set; }
            [NotMapped]
            public string EnclosureDateForUse { get; set; }
            public string EnclosureFile { get; set; }
            public DateTime? EnclosureDate { get; set; }
            [DefaultValue(0)]
            public Int32 SanctionAmount { get; set; }
            public bool? IsDeleted { get; set; }
            public string DocumentTo { get; set; }
            public string FinancialYear { get; set; }
            //public string GrantAmount { get; set; }
            [NotMapped]
            public string AdhrId { get; set; }
            [NotMapped]
            public string DistrictName { get; set; }
            [NotMapped]
            public string officenameCC { get; set; }

            [NotMapped]
            public string officenameCCforUse { get; set; }


            [NotMapped]
            //[Required(ErrorMessage = "Please Select Member")]
            public string MappedMemberCode { get; set; }
            [NotMapped]
            //[Required(ErrorMessage = "Please Select Member")]
            public int MemberCode { get; set; }
            [NotMapped]
            public string MappedMemberName { get; set; }
            [NotMapped]
            public int? distcd { get; set; }
            [NotMapped]
            public string officename { get; set; }
            [NotMapped]
            public string SubDivisionName { get; set; }
            [NotMapped]
            public List<MlaDiaryDepartment> MlaDiaryList { get; set; }
            [NotMapped]
            public string IsFile { get; set; }
            [NotMapped]
            public string IsForwardFile { get; set; }
            [NotMapped]
            public string IsEnclosureFile { get; set; }
            [NotMapped]
            public string DeptName { get; set; }
            [NotMapped]
            public string MinisterName { get; set; }
            [NotMapped]
            public string ActionName { get; set; }
            [NotMapped]
            public string DocmentTypeName { get; set; }
            [NotMapped]
            public string BtnCaption { get; set; }
            [NotMapped]
            public List<MlaDiaryDepartment> DocumentTypelist { get; set; }
            [NotMapped]
            public List<MlaDiaryDepartment> ActionTypeList { get; set; }
            [NotMapped]
            public string PaperEntryType { get; set; }
           
            [NotMapped]
            public virtual ICollection<mDepartment> DeptList { get; set; }
            [NotMapped]
            public List<mOffice> OfficeList { get; set; }

            [NotMapped]
            public List<MlaDiaryDepartment> OfficeSubList { get; set; }

            [NotMapped]
            public List<MlaDiaryDepartment> ActionDetailsList { get; set; }

            [NotMapped]
            public List<MlaDiaryDepartment> OfficeDeptList { get; set; }


            [NotMapped]
            public List<MlaDiaryDepartment> DepartmentList { get; set; }

            [NotMapped]
            public List<mOffice> OfficeList_search { get; set; }
            [NotMapped]
            public List<MlaDiaryDepartment> SubDivisionList { get; set; }
            [NotMapped]
            public List<DistrictModel> Districtist { get; set; }
            [NotMapped]
            public List<MlaDiaryDepartment> MappedMemberList { get; set; }


            /// <summary>
            /// By Joginder For Action Taken
            /// </summary>
            /// 

            [NotMapped]
            public string ActionBy { get; set; }
            [NotMapped]
            public string ActionDescription { get; set; }

            [NotMapped]
         
            public DateTime? ActionDate { get; set; }


            [NotMapped]
            public string ActionEnclosureAttachment { get; set; }




            [NotMapped]
            public int PendencySince { get; set; }
            [NotMapped]
            public List<SelectListItem> FinanCialYearList { get; set; }
            [NotMapped]
            public String UserDeptId { get; set; }
            [NotMapped]
            public string UserofcId { get; set; }
            [NotMapped]
            public string MultipleOfcId { get; set; }
            [NotMapped]
            public int? ActionOfficeId { get; set; }

        /// <summary>
        /// Added by joginder
        /// </summary>
            [NotMapped]
            public int? MemberCodeForUse { get; set; }
        


    }
}
