using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNirdesh.Models
{
    [Serializable]
    [Table("mUsers")]
    public partial class mUsers
    {
        public mUsers()
        {
            mUserList = new List<mUsers>();
        }

        [Key]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(250)]
        public string UserName { get; set; }

        [Required]
        [StringLength(250)]
        public string Password { get; set; }
        public Int32 isDeptApex { get; set; }

        public Int32 isApprovalAuthority { get; set; }
        public Int32 isApex { get; set; }
        public Int32 MapId { get; set; }
        public string ofcType { get; set; }


       
        public bool? IsActive { get; set; }

        //[StringLength(50)]
        public string DeptId { get; set; }

        public string StateId { get; set; }

        
        public string isParent { get; set; }
        public string MCName { get; set; }

        [DefaultValue(0)]
        public Int64 OfficeId { get; set; }

        [StringLength(50)]
        public string OfficeLevel { get; set; }

        [StringLength(50)]
        public string BranchId { get; set; }

        [StringLength(50)]
        public string ConstituencyId { get; set; }

        [StringLength(50)]
        public string Designation { get; set; }

        [StringLength(100)]
        public string DesignationLocal { get; set; }

        [StringLength(50)]
        public string AadarId { get; set; }

        [StringLength(250)]
        public string DSCHash { get; set; }

        [StringLength(50)]
        public string IsMember { get; set; }

        [StringLength(50)]
        public string IsMedia { get; set; }

        [StringLength(25)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedWhen { get; set; }

        public bool IsDSCAuthorized { get; set; }

        [NotMapped]
        public bool? IsPasswordChanged { get; set; }

        [NotMapped]
        public string PasswordString { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }

        public string DOB { get; set; }
        [NotMapped]
        public string UserName_eCcon { get; set; }
        #region User Registration

        public int? SecretoryId { get; set; }
        [NotMapped]
        public string SecretoryName { get; set; }
        [NotMapped]
        public int HODID { get; set; }

        [NotMapped]
        public string DepartmentIDs { get; set; }

        public string SignaturePath { get; set; }

        public string EmpId { get; set; }

        public string Photo { get; set; }

        public string registeredCert { get; set; }




        public string MobileNo { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please Enter Mobile Number")]
        public string Mobile { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string MailID { get; set; }

        public string EmailId { get; set; }
        [NotMapped]
        public List<KeyValuePair<int, string>> radioList { get; set; }

        [NotMapped]
        public virtual ICollection<mDepartment> mDepartment { get; set; }

        [NotMapped]
        public List<string> AllDepartmentIds { get; set; }
        [NotMapped]
        public List<string> AllMemberIds { get; set; }
        [NotMapped]
        public List<string> AllAdditionalDepartmentIds { get; set; }
        [NotMapped]
        //[Required(ErrorMessage = "Please Enter Emp or Member ID")]
        public string EmpOrMemberCode { get; set; }

        [NotMapped]
        public string Name { get; set; }

        public string Address { get; set; }

        [NotMapped]
        public string FatherName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please Enter OTP Value")]
        public string OTPValue { get; set; }

        [NotMapped]
        [StringLength(50)]
        public string AdhaarID { get; set; }

        [NotMapped]
        public string DSCName { get; set; }
        [NotMapped]
        public string DSCType { get; set; }
        [NotMapped]
        public string DSCValidity { get; set; }
        [NotMapped]
        public string CaptchaMessage { get; set; }
        [NotMapped]
        public ICollection<mUsers> mUserList { get; set; }
        [NotMapped]
        public string DscActiveStatus { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public bool IsNotification { get; set; }
        public bool IsAuthorized { get; set; }
        public bool Permission { get; set; }
        [NotMapped]
        public string usertypename { get; set; }
        [NotMapped]
        public int usertypeid { get; set; }
        [NotMapped]
        public string domainAdminName { get; set; }
        [NotMapped]
        public int domainAdminID { get; set; }
        [NotMapped]
        public virtual ICollection<mOffice> OfficeDrpList { get; set; }

        [NotMapped]
        public List<KeyValuePair<int, string>> headList { get; set; }
        public bool? IsSecretoryId { get; set; }
        public bool? IsOther { get; set; }
        public bool? IsHOD { get; set; }
        public int? UserType { get; set; }
        [NotMapped]
        public string RequestedDeptList { get; set; }
        [NotMapped]
        public string ApprovedDeptList { get; set; }
        [NotMapped]
        public string RequestedAdditionalDeptList { get; set; }
        [NotMapped]
        public string ApprovedAdditionalDeptList { get; set; }
        //public int usertypess { get; set; }

        #endregion
        //sameer
        [NotMapped]
        public string OTPId { get; set; }


        #region "Wrong Attempt"
        public DateTime? FirstWrongAttemptTime { get; set; }
        public int WrongAttemptCount { get; set; }
        public DateTime? BlockUntil { get; set; }
        #endregion

        //ashwani

        public bool IsSecChange { get; set; }
        public bool? IsNodalOfficer { get; set; }
        public string RequestedAdditionalDept { get; set; }
        public string ApprovedAdditionalDept { get; set; }
        //[Required]
        //[MinLength(6, ErrorMessage = "Password Must Have Minimum 6 Characters")]
        //[Display(Name = "Assembly Password")]
        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{6,}", ErrorMessage = "Minimum 6 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character.")]
        public string AssemblyPassword { get; set; }
        public int MonthlySMSQuota { get; set; }
        [NotMapped]
        public mUsers musers { get; set; }
        public int? stateCode { get; set; }
        public int? DistrictCode { get; set; }
        public string SubdivisionCode { get; set; }
        //  public string ConstituencyCode { get; set; }
        public int? PanchyatCode { get; set; }
        [NotMapped]
        public string ConstituencyName { get; set; }
        [NotMapped]
        public string SubdivisionName { get; set; }
        [NotMapped]
        public List<string> ConIds { get; set; }
        [NotMapped]
        public string Selected_ConId { get; set; }
        [NotMapped]
        public int? Sel_DistrictCode { get; set; }
        [NotMapped]
        public List<string> SubdivisionIds { get; set; }
        [NotMapped]
        public virtual ICollection<DistrictModel> DistrictList { get; set; }

        [NotMapped]
        public int? Selected_PanchyatCode { get; set; }
        [NotMapped]
        public string Selected_SubCode { get; set; }

        public int? UserTypeConstituencyId { get; set; }
    }

    [Serializable]
    public class LoginMessage
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public mUsers LoggedUser { get; set; }
        public bool IsValidUsername { get; set; }
        public string UserName { get; set; }

    }
}
