
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace CMNirdesh.Models.DeptRegisterModel
{
    [Serializable]
    public class DeptRegisterModel
    {
        public DeptRegisterModel()
        {
            this.mDepartment = new List<mDepartment>();
            this.UserList = new List<DeptRegisterModel>();           
          //  this.tStatusPrintingPress = new List<tStatusPrintingPress>();
        }
       
        public string DepartmentId { get; set; }
        public string AadharId { get; set; }
        public string AadharName { get; set; }
        public string DiaryNumber { get; set; }
        public string ChangedDepartment { get; set; }
        public string ChangedDepartmentName { get; set; }
        public string QTypeName { get; set; }
        public string ChangeDate { get; set; }
        public DateTime ? ChangeDateTime { get; set; }
        public int  QuestionType { get; set; }
        public virtual ICollection<DeptRegisterModel> UserList { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public virtual ICollection<mDepartment> mDepartment { get; set; }
        public DateTime? RecievedOnVS { get; set; }
        public string OldDepartmentName { get; set; }
		public virtual ICollection<CMNirdesh.Models.mOffice> OfficeList { get; set; }      
       
    }
}
