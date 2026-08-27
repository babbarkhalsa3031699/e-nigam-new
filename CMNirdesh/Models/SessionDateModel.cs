using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class SessionDateModel
    {
        public int ID { get; set; }
        public int AssemblyCode { get; set; }
        public int SessionID { get; set; }
        public string SessionDate { get; set; }

        public string SessionDate_Local { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public virtual List<SessionDateModel> ListSessionDate { get; set; }
    }

}

