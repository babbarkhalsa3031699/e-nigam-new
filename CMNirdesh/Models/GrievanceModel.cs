
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMNirdesh.Models
{
    public class GrievanceModel
    {
       
      
    }
    public class pdfGrievance
    {

        public string Prefix { get; set; }
        public string MemName { get; set; }
        public string PublicName { get; set; }
        //public string LastName { get; set; }
        public string PublicMobile { get; set; }
        public string PublicAddress { get; set; }
        public string PublicAAdharId { get; set; }
        public string Public_Profile { get; set; }
        public string GrvCode { get; set; }
        public string MemCode { get; set; }
        public string UserCode { get; set; }
        public Int64 Guid { get; set; }
        public string GrvImage { get; set; }
        public string PdfName { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string MemberName { get; set; }
        public string InformationContent { get; set; }
        public string Location { get; set; }
        public string PostedOn { get; set; }
        public string PostedTo { get; set; }
        public string GrvStatus { get; set; }
        public string Deptname { get; set; }
        public string Officename { get; set; }
        public string ActionTaken { get; set; }
        public string ActionDate { get; set; }
        public string ActionBy { get; set; }
        public string GrvDesc { get; set; }
        public string SubmitDate { get; set; }
        public string Mem_MobileNo { get; set; }
        public string GrvActionStatus { get; set; }
        public List<pdfGrievance> GrvList1 { get; set; }
    }
}