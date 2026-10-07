using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class CoverLetterRequest
    {
        public string DeptId { get; set; }
        public int AgendaID { get; set; }
        public string DocumentType { get; set; }
        [AllowHtml]
        public string CoverHtml { get; set; }
    }
}