namespace CMNIrdesh.Models.Session
{
    #region Namespace Reffrences

    #endregion

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("mSessionDates")]
    public partial class mSessionDate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SessionId { get; set; }

        public int AssemblyId { get; set; }

        [Column(TypeName = "Date")]
        public System.DateTime SessionDate { get; set; }

        [MaxLength(200)]
        public string SessionDateLocal { get; set; }


        public Nullable<System.TimeSpan> SessionEndTime { get; set; }

        [MaxLength(200)]
        public string SessionEndTimeLocal { get; set; }

        public Nullable<System.TimeSpan> SessionTime { get; set; }

        [MaxLength(200)]
        public string SessionTimeLocal { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "Varchar")]
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedWhen { get; set; }

        [MaxLength(200)]
        public string SessionDate_Local { get; set; }

        [MaxLength(200)]
        public string RotationMinister { get; set; }

        //public bool? Active { get; set; }

        [MaxLength(200)]
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        [NotMapped]
        public string DisplayDate { get; set; }

        [NotMapped]
        public string SessionDateTo { get; set; }

        [NotMapped]
        public virtual ICollection<mSessionDate> SessionDateList { get; set; }

        public string StartQListPath { get; set; }
        public string UnStartQListPath { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsApprovedUnstart { get; set; }
        public bool IsSitting { get; set; }

        [NotMapped]
        public string GetAssemblyName { get; set; }

        [NotMapped]
        public string GetSessionName { get; set; }

        public bool? IsActive { get; set; }




        public bool? IsDeleted { get; set; }

        public bool? IsQuestionHour { get; set; }

        public bool? IsMultipleSitting { get; set; }

        public bool ApprovedByTrans { get; set; }
        public bool UnstarredApprovedByTrans { get; set; }
        public bool ApprovedBySecrt { get; set; }
        public bool UnstarredApprovedBySecrt { get; set; }
        public string SPPQListPath { get; set; }
        public string UPPQListPath { get; set; }
        public bool SPPApprovedByTrans { get; set; }
        public bool SPPApprovedBySecrt { get; set; }
        public bool UPPApprovedByTrans { get; set; }
        public bool UPPApprovedBySecrt { get; set; }






    }
}
