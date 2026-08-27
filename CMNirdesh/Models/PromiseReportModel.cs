using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;


namespace CMNirdesh.Models
{

    public class PromiseReportModel
    {      
        public string EndDate { get; set; }
        public string EstimatedAmount { get; set; }
        public string Expenditure { get; set; }
        public string FirmName { get; set; }
        public string FundReleased { get; set; }
        public string PhysicalProgress { get; set; }
        public string ProjectName { get; set; }
        public string Remark { get; set; }
        public string StartDate { get; set; }
        public string CategoryName { get; set; }
        public string ProjectStatusName { get; set; }
        public string PromiseId { get; set; }
        public string PromiseName { get; set; }
        public string deptname { get; set; }
        public string ProjectId { get; set; }

        public List<PromiseNameDetail> _PromiseDetail { get; set; }

    }

    public class PromiseNameDetail
    {
        public string PromiseName
        {
            get;
            set;

        }
        public int PromiseId
        {
            get;
            set;

        }

        public string Dept_Id { get; set; }
        public string Dept_Name { get; set; }

    }


    
}