using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNirdesh.Models
{
    [Serializable]
    [Table("Districts")]
    public class DistrictModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DistrictId { get; set; }
        [Required]
        public int StateCode { get; set; }
        [Required]
        public int? DistrictCode { get; set; }
        public int? distcd { get; set; }
        public string DistrictAbbreviation { get; set; }

        [Required]
        public string DistrictName { get; set; }
        public string DistrictNameLocal { get; set; }
        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedWhen { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedWhen { get; set; }

        [NotMapped]
        public string ErrorMessage { get; set; }

        [NotMapped]
        public string GetStateName { get; set; }
    }
}
