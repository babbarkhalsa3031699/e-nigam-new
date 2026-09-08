namespace CMNirdesh.Models.PaperLaid
{
    #region Namespace Reffrences
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    [Table("mPaperCategoryType")]
    [Serializable]
    public partial class mPaperCategoryType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaperCategoryTypeId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(200)]
        public string NameLocal { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreationDate { get; set; }

        public string CreatedBy { get; set; }



        public string ModifiedBy { get; set; }

        public DateTime? ModifiedWhen { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
