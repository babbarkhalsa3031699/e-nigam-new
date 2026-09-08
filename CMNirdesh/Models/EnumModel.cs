
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CMNirdesh.Models
{
    public enum Prefix
    {
        Mr = 0,
        Mrs = 1,
        Shri = 2,
        Smt = 3,
        Miss = 4,
        Dr = 5
    }

  
    public enum MedicineType
    {
        [Display(Name = "Allopathic")]
        [Description("Allopathic")]
        Allopathic,
        [Display(Name = "Ayurvedic")]
        [Description("Ayurvedic")]
        Ayurvedic
    }

    public enum GrievanceStatus
    {
        [Display(Name = "Pending")]
        [Description("Pending")]
        Pending,
        [Display(Name = "Forwarded")]
        [Description("Forwarded")]
        Forwarded,
        [Display(Name = "In-Progress")]
        [Description("InProgress")]
        InProgress,
        //[Display(Name = "Rejected")]
        //[Description("Rejected")]
        //Rejected,
        [Display(Name = "Disposed")]
        [Description("Disposed")]
        Disposed
    }

    public enum LibraryRoleType
    {
        [Display(Name = "Documentation Officer")]
        [Description("Documentation Officer")]
        DocumentationOfficer,
        [Display(Name = "Librarion")]
        [Description("Librarion")]
        Librarion,
        [Display(Name = "Librarion Clerk")]
        [Description("Librarion Clerk")]
        LibrarionClerk,
        [Display(Name = "Sale Counter Clerk")]
        [Description("Sale Counter Clerk")]
        SaleCounterClerk
    }

    public enum WardType
    {
        [Display(Name = "General")]
        [Description("General")]
        General,
        [Display(Name = "Private")]
        [Description("Private")]
        Private
    }
    public enum BillStatus
    {
        [Display(Name = "Bill in Process")]
        [Description("Bill in Process")]
        BillInProcess,
        //[Display(Name = "Saction Info Saved")]
        //[Description("Saction Info Saved")]
        //SactionInfoSaved,
        //[Display(Name = "Attach PDF")]
        //[Description("Attach PDF")]
        //AttachPDF,
        [Display(Name = "Sanctioned")]
        [Description("Sanctioned")]
        Sanctioned,
        //[Display(Name = "Update Encashment Info")]
        //[Description("Update Encashment Info")]
        //UpdateEncashmentInfo,
        [Display(Name = "Encashment")]
        [Description("Encashment")]
        Encashment
        //[Display(Name = "Reject")]
        //[Description("Reject")]
        //Reject
    }

    public enum DesignationType
    {
        [Display(Name = "Speaker")]
        [Description("Speaker")]
        Speaker,
        [Display(Name = "Deputy Speaker")]
        [Description("Deputy Speaker")]
        DeputySpeaker,
        [Display(Name = "Member")]
        [Description("Member")]
        Member,
        [Display(Name = "Ex-member or their nominee")]
        [Description("Ex-member or their nominee")]
        ExMember,
        [Display(Name = "Gazetted Staff")]
        [Description("Gazetted Staff")]
        GazettedStaff,
        [Display(Name = "Non-Gazetted Staff")]
        [Description("Non-Gazetted Staff")]
        NonGazettedStaff,
        [Display(Name = "Other")]
        [Description("Other")]
        Other,
        [Display(Name = "Ex-Staff")]
        [Description("Ex-Staff")]
        ExStaff,
        [Display(Name = "ChiefWhip")]
        [Description("ChiefWhip")]
        ChiefWhip
    }

    public enum StaffClass
    {
        [Display(Name = "Class I")]
        [Description("Class I ")]
        ClassI,
        [Display(Name = "Class II")]
        [Description("Class II")]
        ClassII,
        [Display(Name = "Class III")]
        [Description("Class III")]
        ClassIII,
        [Display(Name = "Class IV")]
        [Description("Class IV")]
        ClassIV,
        [Display(Name = "Class III CPF")]
        [Description("Class III CPF")]
        ClassIIICPF,
        [Display(Name = "Class IV CPF")]
        [Description("Class IV CPF")]
        ClassIVCPF,
    }

    public enum AlongWithList
    {
        [Display(Name = "Spouse")]
        [Description("Spouse")]
        Spouse,
        [Display(Name = "Companion")]
        [Description("Companion")]
        Companion,
        [Display(Name = "Alone")]
        [Description("Alone")]
        Alone,
        [Display(Name = "Attendant")]
        [Description("Attendant")]
        Attendant

    }

    public enum JourneyByList
    {
        [Display(Name = "Train")]
        [Description("Train")]
        Train,
        [Display(Name = "Flight")]
        [Description("Flight")]
        Flight,
        [Display(Name = "Bus")]
        [Description("Bus")]
        Bus 

    }

    public enum Availability
    {
        [Display(Name = "Available ")]
        [Description("Available ")]
        Available,
        [Display(Name = "Out of Stock")]
        [Description("Out of Stock")]
        OutofStock


    }


    public enum TitleType
    {
        [Display(Name = " Himachal Pradesh Vidhan Sabha ")]
        [Description(" Himachal Pradesh Vidhan Sabha ")]
        VidhanSabha,
        [Display(Name = "Rajya Sabha")]
        [Description("Rajya Sabha")]
        RajyaSabha,
        [Display(Name = "Lok Sabha")]
        [Description(" Lok Sabha")]
        LokSabha


    }
     
    public enum StatusType
    {
        [Display(Name = "Rules")]
        [Description("Rules")]
        Rules,
        [Display(Name = "Directions")]
        [Description("Directions")]
        Directions
       

    }

    public enum AfterBefore
    {
        [Display(Name = "After")]
        [Description("After")]
        After,
        [Display(Name = "Before")]
        [Description("Before")]
        Before


    }

    public enum AuthorityLetterType
    {
        [Description("Custom Bill Numbers")]
        [Display(Name = "Custom Bill Numbers")]
        CustomBillNumbers,
        [Display(Name = "Ex Member")]
        [Description("Ex Member")]
        ExMember,
        //[Display(Name = "Ex Employee Nominee")]
        //[Description("ExEmployee Nominee")]
        //ExEmployeeNominee,
        [Display(Name = "Members (Compulsory account in UCO Bank)")]
        [Description("Members (Compulsory account in UCO Bank)")]
        Member,
        [Display(Name = "Speaker and Deputy Speaker")]
        [Description("Speaker and Deputy Speaker")]
        SpeakerAndDeputySpeaker,
       
        [Display(Name = "Others")]
        [Description("Others")]
        Others,
        [Display(Name = "Staff")]
        [Description("Staff")]
        Staff

    }
    public enum LoanType
    {
        [Display(Name = "GPF advance")]
        [Description("GPF advance")]
        GPFAdvance,
        [Display(Name = "GPF withdrawal")]
        [Description("GPF withdrawal")]
        GPFWithdrawal,
        [Display(Name = "House Building")]
        [Description("House Building")]
        HouseBuilding,
        [Display(Name = "Motor Car")]
        [Description("Motor Car")]
        MotorCar,
        [Display(Name = "Warm clothing advance")]
        [Description("Warm clothing advance")]
        WarmClothingAdvance,
        [Display(Name = "Festival advance")]
        [Description("Festival advance")]
        FestivalAdvance,
        [Display(Name = "N/A")]
        [Description("N/A")]
        NA

    }

    public enum NoticeStatusEnum
    {
        NoticeInbox = 0,
        NoticePending = 1,
        NoticePublished = 2,
        NoticeFreeze = 4,
        NoticeSent = 3,
        NoticeToTypist = 5,
        NoticeAssignTypist = 6,
    }

    public enum QuestionType
    {
        StartedQuestion = 1,
        UnstaredQuestion = 2
    }
    public enum Questionstatus
    {
        QuestionInbox = 0,
        QuestionPending = 1,
        QuestionSent = 3,
        QuestionAssignTypist = 4,
        QuestionProofReading = 5,
        QuestionFreeze = 6,
        QuestionRejected = 2,
        QuestionFinallySent = 7
    }


    public enum CommitteeStatus
    {
        Draft = 1,
        Approved = 2,
        Send = 3
    }

    public enum MinisterDashboardStatus
    {
        PendingForReply = 1,
        DraftReplies = 2,
        RepliesSent = 3
    }

    public enum QuestionDashboardStatus
    {
        PendingForReply = 1,
        DraftReplies = 2,
        RepliesSent = 3
    }

    public enum PassStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }

    public enum ContactGroupTypeEnum
    {
        Generic = 1,
        panchayat = 2,
        Constituency = 3
    }

    public enum RoleTypeDept
    {
        [Description("Head Of Department")]
        HOD,
        [Description("Secretary")]
        Secretary
    }

    public enum PassAllowsCreation
    {
        Reception = 1,
        Department = 2,
        Media = 3,
        Vehicle = 4,
        SpeakerGalary = 5,

        SpeakerGalaryPassID = 14,
        VisitorsGallery = 7,
        PublicPassID = 17,
        DutyStaffatVS = 20
    }

    public enum PassRequestTypes
    {
        [Description("Department")]
        PassFromDepartment,
        [Description("Reception")]
        PassFromReception,
        [Description("Media")]
        PassFromMedia
    }

    public static class EnumHelper
    {
        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }
    }

    #region PDF updation ENUM's
    public enum SignPositionEnum
    {
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 3,
        BottomRight = 4
    }

    public enum PagesToSignEnum
    {
        AllPages = 1,
        AlternatePages = 2,
        FirstPage = 3,
        LastPage = 4
    }
    #endregion
}
