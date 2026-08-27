using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class SectorModel
    {
        public int SectorID { get; set; }
        public int SectorCode { get; set; }
        public string SectorName { get; set; }
        public string SectorNameLocal { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public virtual List<SectorModel> ListSector { get; set; }
    }
}
