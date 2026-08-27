using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class CoverLetterRequest
    {
        public string DeptId { get; set; }
        public int AgendaID { get; set; }
        public string DocumentType { get; set; }
        public string CoverHtml { get; set; }
    }
}