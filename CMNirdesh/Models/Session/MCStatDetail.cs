using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.Session
{
    public class MCStatDetail
    {
        public int SrNo { get; set; }
        public DateTime AgendaDate { get; set; }

        public string MeetingDate { get; set; }
        public string Meeting { get; set; }

        public string TotalResolutions { get; set; }

        public string Type { get; set; }

        public string Resolutionno { get; set; }
        public string Subject { get; set; }
        public DateTime LetterDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionTaken { get; set; }
        public string Enclosure { get; set; }

        public string ReferenceId { get; set; }
        public string ActionName { get; set; }

        public string FromDepartment { get; set; }
        public string FromOffice { get; set; }
        public string ToDepartment { get; set; }
        public string ToOffice { get; set; }
        public string DocType { get; set; }
        public string Pendency { get; set; }
        public bool Islink { get; set; }
    }
}