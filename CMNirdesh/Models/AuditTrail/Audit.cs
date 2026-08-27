using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMNirdesh.Models.AuditTrail
{
    [Serializable]
    [Table("AuditLog")]
    public class Audit
    {
        [Key]
        public int LogID { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public DateTime LoginDateTime { get; set; }
        public string LoginStatus { get; set; }
        public DateTime 
            
            
            DateTime { get; set; }
        public string ModuleName { get; set; }
        public string ActionName { get; set; }
        public string ActionType { get; set; }        
        //public string ModelAndActionName { get; set; }
        public string URL { get; set; }
        public DateTime ActionDate { get; set; }
        //Default Constructor
        public Audit() { }
        //UserID, IP Address, Login Date & Time, Login Status, Logout Date & Time, Action Type, Module & Action Name, Action Date.
    }
}