using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class AssemblyModel
    {
        public int AssemblyID { get; set; }
        public int AssemblyCode { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyNameLocal { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public virtual List<AssemblyModel> ListAssembly { get; set; }
    }
}
