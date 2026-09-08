
using CMNirdesh.Models.MCHouse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class LGApproval
    {
        public int LGApprovalID { get; set; }

        public string MCName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Meeting Date")]
        public DateTime MeetingDate { get; set; }

        [Required]
        [StringLength(255)]
        public string MeetingTitle { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Approved Date")]
        public string ApprovedDate { get; set; }

        [Display(Name = "PDF File")]
        public string PDFPath { get; set; }

        public string TokenUrl { get; set; }

        public List<LGApproval> LGApprovalList;
        public SelectList mDepartmentList { get; set; }

        public string fileacessingUrl { get; set; }

        public string RefMenuName { get; set; }

        public string RMenuName { get; set; }

    }
    public class LGApprovalDS
    {
        public static List<LGApproval> GetLGApprovalList(string deptid, string isDeptApex, string Office, string userid)
        {
            List<LGApproval> lst = new List<LGApproval>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@deptid", deptid);
                param[1] = new SqlParameter("@isDeptApex", Convert.ToInt16(isDeptApex));
                param[2] = new SqlParameter("@Userid", userid);
               
                con.Close();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetApprovalOrder", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LGApproval approval = new LGApproval();
                    // Map the data from the DataRow to the Circular object
                    approval.LGApprovalID = Convert.ToInt32(dr["Id"]);
                    approval.MCName = Convert.ToString(dr["MCName"]);
                   
                    if (dr["MeetingDate"] != DBNull.Value)
                    {
                        approval.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                    }

                    approval.MeetingTitle = Convert.ToString(dr["MeetingTitle"]);


                    if (dr["ApprovalDate"] != DBNull.Value)
                    {
                        approval.ApprovedDate = Convert.ToString(dr["ApprovalDate"]);
                    }


                    approval.PDFPath = Convert.ToString(dr["PDF"]);

                    lst.Add(approval);
                }

                return lst;
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst; // Return an empty list in case of error
            }
        }


        public static List<LGApproval> GetADCApprovalList(string deptid, string isDeptApex, string Office, string userid)
        {
            List<LGApproval> lst = new List<LGApproval>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@deptid", deptid);
                param[1] = new SqlParameter("@isDeptApex", Convert.ToInt16(isDeptApex));
                param[2] = new SqlParameter("@Userid", userid);

                con.Close();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetApprovalOrderADC", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LGApproval approval = new LGApproval();
                    // Map the data from the DataRow to the Circular object
                    approval.LGApprovalID = Convert.ToInt32(dr["Id"]);
                    approval.MCName = Convert.ToString(dr["MCName"]);

                    if (dr["MeetingDate"] != DBNull.Value)
                    {
                        approval.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                    }

                    approval.MeetingTitle = Convert.ToString(dr["MeetingTitle"]);


                    if (dr["ApprovalDate"] != DBNull.Value)
                    {
                        approval.ApprovedDate = Convert.ToString(dr["ApprovalDate"]);
                    }


                    approval.PDFPath = Convert.ToString(dr["PDF"]);

                    lst.Add(approval);
                }

                return lst;
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst; // Return an empty list in case of error
            }
        }


        public static List<LGApproval> GetLGsApprovalList(string deptid, string isDeptApex, string Office, string userid)
        {
            List<LGApproval> lst = new List<LGApproval>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@deptid", deptid);
                param[1] = new SqlParameter("@isDeptApex", Convert.ToInt16(isDeptApex));
                param[2] = new SqlParameter("@Userid", userid);

                con.Close();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetApprovalOrderLG", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LGApproval approval = new LGApproval();
                    // Map the data from the DataRow to the Circular object
                    approval.LGApprovalID = Convert.ToInt32(dr["Id"]);
                    approval.MCName = Convert.ToString(dr["MCName"]);

                    if (dr["MeetingDate"] != DBNull.Value)
                    {
                        approval.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                    }

                    approval.MeetingTitle = Convert.ToString(dr["MeetingTitle"]);


                    if (dr["ApprovalDate"] != DBNull.Value)
                    {
                        approval.ApprovedDate = Convert.ToString(dr["ApprovalDate"]);
                    }


                    approval.PDFPath = Convert.ToString(dr["PDF"]);

                    lst.Add(approval);
                }

                return lst;
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst; // Return an empty list in case of error
            }
        }


        public static List<LGApproval> GetUnSignedApprovalList(string AgendaID)
        {
            var CurrentDepartment = CurrentSession.DeptID;
            List<LGApproval> lst = new List<LGApproval>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@AgendaID", AgendaID);
                param[1] = new SqlParameter("@DeptId", CurrentDepartment);
                con.Close();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetUnSignedeApprovalOrder", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LGApproval approval = new LGApproval();
                    // Map the data from the DataRow to the Circular object
                    approval.LGApprovalID = Convert.ToInt32(dr["Id"]);
                    approval.MCName = Convert.ToString(dr["MCName"]);

                    if (dr["MeetingDate"] != DBNull.Value)
                    {
                        approval.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                    }

                    approval.MeetingTitle = Convert.ToString(dr["MeetingTitle"]);


                    if (dr["ApprovalDate"] != DBNull.Value)
                    {
                        approval.ApprovedDate = Convert.ToString(dr["ApprovalDate"]);
                    }
                   // approval.TokenUrl= 
                    //approval.to
                    approval.PDFPath = Convert.ToString(dr["PDF"]);

                    lst.Add(approval);
                }

                return lst;
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst; // Return an empty list in case of error
            }
        }

 
        


        public static List<LGApproval> GetFileApprovalList(string AgendaID)
        {
            var CurrentDepartment = CurrentSession.DeptID;
            List<LGApproval> lst = new List<LGApproval>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@AgendaID", AgendaID);
                param[1] = new SqlParameter("@DeptId", CurrentDepartment);
                con.Close();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetFileApprovalOrderUpdate", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LGApproval approval = new LGApproval();
                    // Map the data from the DataRow to the Circular object
                    approval.LGApprovalID = Convert.ToInt32(dr["Id"]);
                    approval.MCName = Convert.ToString(dr["MCName"]);

                    if (dr["MeetingDate"] != DBNull.Value)
                    {
                        approval.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                    }

                    approval.MeetingTitle = Convert.ToString(dr["MeetingTitle"]);


                    if (dr["ApprovalDate"] != DBNull.Value)
                    {
                        approval.ApprovedDate = Convert.ToString(dr["ApprovalDate"]);
                    }


                    approval.PDFPath = Convert.ToString(dr["PDF"]);

                    lst.Add(approval);
                }

                return lst;
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst; // Return an empty list in case of error
            }
        }


        public static List<LGApproval> GetFileSignedApprovalList(string AgendaID)
        {
            List<LGApproval> lst = new List<LGApproval>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@AgendaID", AgendaID);
                param[1] = new SqlParameter("@DeptId", CurrentSession.DeptID);
                con.Close();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetFileSignedApprovalOrder", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    LGApproval approval = new LGApproval();
                    // Map the data from the DataRow to the Circular object
                    approval.LGApprovalID = Convert.ToInt32(dr["Id"]);
                    approval.MCName = Convert.ToString(dr["MCName"]);

                    if (dr["MeetingDate"] != DBNull.Value)
                    {
                        approval.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                    }

                    approval.MeetingTitle = Convert.ToString(dr["MeetingTitle"]);


                    if (dr["ApprovalDate"] != DBNull.Value)
                    {
                        approval.ApprovedDate = Convert.ToString(dr["ApprovalDate"]);
                    }


                    approval.PDFPath = Convert.ToString(dr["PDF"]);

                    lst.Add(approval);
                }

                return lst;
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst; // Return an empty list in case of error
            }
        }




    }
}