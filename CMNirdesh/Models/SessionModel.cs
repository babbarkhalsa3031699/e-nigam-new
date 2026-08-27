using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class SessionModel
    {
        public int AssemblyCode { get; set; }
        public int SessionID { get; set; }
        public int SessionCode { get; set; }
        public string SessionName { get; set; }
        public string SessionNameLocal { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public virtual List<SessionModel> ListSession{ get; set; }
    }
}

