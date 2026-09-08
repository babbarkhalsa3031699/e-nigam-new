namespace CMNirdesh.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Serializable]
    [Table("mSiteSettings")]
    public class SiteSettings
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }


        public string SettingName { get; set; }

        public string SettingValue { get; set; }

        public string SettingValueLocal { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string SignaturePath { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        public int AssemblyCode { get; set; }

        [NotMapped]
        public int SessionCode { get; set; }

        [NotMapped]
        public int MaxQuestionReplyDate { get; set; }

        [NotMapped]
        public int MaxQuesSelectNo { get; set; }

        [NotMapped]
        public int MaxStarredQuesAskByMem { get; set; }

        [NotMapped]
        public int MaxUnStarredQuesAskByMem { get; set; }

        [NotMapped]
        public int MaxShortNQuesAskByMem { get; set; }

        [NotMapped]
        public int SessionCalendar { get; set; }

        [NotMapped]
        public string CurrentSecretery { get; set; }

        [NotMapped]
        public string DSCApplicable { get; set; }


        [NotMapped]
        public string CurrentSpeaker { get; set; }

        [NotMapped]
        public int? PreSLQNum { get; set; }

        [NotMapped]
        public int? PreSLQNumUn { get; set; }

        [NotMapped]
        public string AccessFilePath { get; set; }

        public bool? IsDeleted { get; set; }
        [NotMapped]
        public string AccessFilePathPdfView { get; set; }
    }
}
