using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNirdesh.Models.Session
{
    [Table("mMlaSignature")]
    [Serializable]
    public class mMlaSignature
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string SignatureName { get; set; }       
        public string SignatureName_Local { get; set; }
        public string SignatureDesignation { get; set; }
        public string SignatureDesignation_Local { get; set; }
        public string SignaturePlace1 { get; set; }
        public string SignaturePlace1_Local { get; set; }
        public string SignaturePlace2 { get; set; }
        public string SignaturePlace2_Local { get; set; }
        public string SignaturePin1 { get; set; }
        public string SignaturePin1_Local { get; set; }
        public string SignaturePin2 { get; set; }
        public string SignaturePin2_Local { get; set; }
        public string SignatureDate { get; set; }
        public string SignatureDate_local { get; set; } 
        public string Text1 { get; set; }
        public string Text1_local { get; set; }
        public int? AssemblyId { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public string MlaCode { get; set; }

        public string HeaderText1 { get; set; }
        public string HeaderText2 { get; set; }
        public string HeaderLocalText1 { get; set; }
        public string HeaderLocalText2 { get; set; }
        [NotMapped]
        public List<mMlaSignature> mMlaSignaturelist { get; set; }
    }
}
