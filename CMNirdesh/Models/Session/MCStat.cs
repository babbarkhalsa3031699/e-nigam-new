using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.Session
{
    public class MCStat
    {
        public string OfficeName{ get; set; }
        public string OfficeId { get; set; }
        public string DepartmentName { get; set; }
        public string TotalFiles { get; set; }
        public string TotalResolutions { get; set; }

        public string TotalResolutionsPassed { get; set; }

        public string TotalResolutionsDraft { get; set; }
        public string TotalResolutionsApproved { get; set; }
        public string TotalResolutionsPending { get; set; }

        public string DeptID { get; set; }

        public string TotalResolutionsRejected { get; set; }

        public string TotalResolutionsClerified { get; set; }
        public string TotalResolutionsClerification { get; set; }

        public string TotalWorks { get; set; }
        public string TotalImplemented { get; set; }
        public string TotalUImplementation { get; set; }
        public string TotalNotImplementation { get; set; }
    }
}