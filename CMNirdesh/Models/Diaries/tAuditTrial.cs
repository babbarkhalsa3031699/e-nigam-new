using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNirdesh.Models.Diaries
{
    [Serializable]
    [Table("tAuditTrial")]
    public class tAuditTrial
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int auditTrialID { get; set; }

        public long? paperLaidID { get; set; }

        public long paperLaidTempID { get; set; }

        public string departmentID { get; set; }

        public string documentReferenceNumber { get; set; }

        public string filePath { get; set; }

        public string verifiedBy { get; set; }

        public DateTime? DateTime { get; set; }

        public bool status { get; set; }

        public string type { get; set; }

    }
    [Serializable]
    public class tAuditTrialViewModel
    {
        public long? paperLaidID { get; set; }

        public long paperLaidTempID { get; set; }

        public string departmentID { get; set; }

        [NotMapped]
        public string departmentName { get; set; }

        public int? questionNumber { get; set; }

        public string documentReferenceNumber { get; set; }

        public string filePath { get; set; }
        public string filePath2 { get; set; }

        public string filePathName { get; set; }
        public string filePath2Name { get; set; }

        public string SecurePath { get; set; }
        public string verifiedBy { get; set; }

        public string subject { get; set; }

        public string type { get; set; }

        public bool status { get; set; }

        public DateTime? DateTime { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? lyingDate { get; set; }
        public DateTime? FixedDate { get; set; }

        [DefaultValue(false)]
        public bool isLaid { get; set; }

        public int? sessionDateID { get; set; }
        public int? desireLayingDateID { get; set; }
        public long? DeptActivePaperId { get; set; }
        public int? ordering { get; set; }

        public DateTime? ReferencedDate { get; set; }
    }


}
