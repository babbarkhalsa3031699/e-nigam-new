namespace CMNirdesh.Models.Session
{
    #region Namespace Reffrences
   
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    #endregion

    [Table("mSessions")]
    [Serializable]
    public partial class mSession
    {
        //[Key]
        [Column(Order = 0)]
        public int AssemblyID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionID { get; set; }

        //[Required]
        //[StringLength(200)]
        public string SessionName { get; set; }

        //[StringLength(200)]
        public string SessionNameLocal { get; set; }

        //[Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        // [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public string SessionPeriod { get; set; }

        public string SessionPeriodLocal { get; set; }

        public string SessionDescription { get; set; }

        public int? StaredQuestion { get; set; }

        //[Required]
        public int? SessionType { get; set; }

        public int? UnstaredQuestions { get; set; }

        public int? BillsIntroduced { get; set; }

        public int? BillsPassed { get; set; }

        public bool? CutMotions { get; set; }


        public string ModifiedBy { get; set; }

        public DateTime? ModifiedWhen { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        //[Required]
        public int SessionCode { get; set; }

        public bool? IsPublished { get; set; }

        public bool? IsDeleted { get; set; }

        public string SessionCalanderLocation { get; set; }

        public int? SessionStatus { get; set; }

        public string RotationMinisterLocation { get; set; }

        


        [NotMapped]
        public string GetAssemblyName { get; set; }

        [NotMapped]
        public string GetSessionType { get; set; }

    }
}
