
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
    public class Circular
    {
        public int CircularID { get; set; }

        public int AgendaId { get; set; }
        public string SelectedContacts { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Circular Date")]
        public DateTime CircularDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Subject { get; set; }

        [Display(Name = "PDF File")]
        public string PDFPath { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Department ID")]
        public string DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string MCCode { get; set; }

        [Required(ErrorMessage = "Please upload a PDF file")]
        public HttpPostedFileBase PDFFile { get; set; }

        public List<Circular> CircularList;
        public SelectList mDepartmentList { get; set; }

        public string fileacessingUrl { get; set; }

    }
    public class CircularDS
    {
        public static List<Circular> GetCircularList(string deptid,string isDeptApex)
        {
            List<Circular> lst = new List<Circular>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@deptid", deptid);
                param[1] = new SqlParameter("@isDeptApex",Convert.ToInt16(isDeptApex));
                con.Close();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "Get_eNigamCirculars", param);
               

              


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Circular circular = new Circular();
                    // Map the data from the DataRow to the Circular object
                    circular.CircularID = Convert.ToInt32(dr["CircularId"]);
                    circular.Subject = Convert.ToString(dr["Subject"]);
                    circular.PDFPath = Convert.ToString(dr["PDFPath"]);
                    if (dr["CircularDate"] != DBNull.Value)
                    {
                        circular.CircularDate = Convert.ToDateTime(dr["CircularDate"]);
                    }
                    circular.DepartmentId = Convert.ToString(dr["DepartmentId"]);
                    circular.DepartmentName = Convert.ToString(dr["deptname"]);
                    // circular.MCCode= Convert.ToString(dr["MCID"]);
                    lst.Add(circular);
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

        public static List<Circular> GetCircularListNotice(string deptid)
        {
            List<Circular> lst = new List<Circular>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@deptid", deptid);
                con.Close();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "Get_Circulars", param);





                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Circular circular = new Circular();
                    circular.CircularID = Convert.ToInt32(dr["CircularId"]);
                    circular.Subject = Convert.ToString(dr["Subject"]);
                    circular.PDFPath = Convert.ToString(dr["PDFPath"]);
                    if (dr["CircularDate"] != DBNull.Value)
                    {
                        circular.CircularDate = Convert.ToDateTime(dr["CircularDate"]);
                    }
                    circular.DepartmentId = Convert.ToString(dr["DepartmentId"]);
                    circular.DepartmentName = Convert.ToString(dr["deptname"]);
                  
                    lst.Add(circular);
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

        public static int SaveRecord(Circular model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlCommand cmd = new SqlCommand("Insert_eNigamCircular", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters for the stored procedure
                //cmd.Parameters.AddWithValue("@CircularDate", model.CircularDate);
                cmd.Parameters.AddWithValue("@Subject", model.Subject);
                cmd.Parameters.AddWithValue("@PDFPath", model.PDFPath);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@DepartmentId", CurrentSession.DeptID);
               

                // Add OUTPUT parameter for the new Circular ID
                SqlParameter newCircularIdParam = new SqlParameter("@NewCircularID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(newCircularIdParam);

                // Execute the command
                cmd.ExecuteNonQuery();

                // Retrieve the new Circular ID
                int NewCircularID = (int)newCircularIdParam.Value;
                return NewCircularID;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return -1; // Exception occurred
            }
        }

        public static int SaveNoticeRecord(Circular model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlCommand cmd = new SqlCommand("Insert_eNigamCircular_Notice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Subject", model.Subject);
                cmd.Parameters.AddWithValue("@PDFPath", model.PDFPath);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@DepartmentId", CurrentSession.DeptID);
                cmd.Parameters.AddWithValue("@AgendaId", model.AgendaId);

                // Add OUTPUT parameter for the new Circular ID
                SqlParameter newCircularIdParam = new SqlParameter("@NewCircularID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(newCircularIdParam);

                // Execute the command
                cmd.ExecuteNonQuery();

                // Retrieve the new Circular ID
                int NewCircularID = (int)newCircularIdParam.Value;
                return NewCircularID;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return -1; // Exception occurred
            }
        }

        public static int SaveCircularDetails(int circularID, string mcid, bool active, string departmentSendBy)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                // Begin a transaction to ensure the operation is safe
                SqlTransaction transaction = con.BeginTransaction();

                // Create a command to insert into CircularDetails
                SqlCommand cmd = new SqlCommand("Insert_CircularDetails", con, transaction);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters for the CircularDetails insert
                cmd.Parameters.AddWithValue("@CircularID", circularID);  // CircularID from the Circulars table
                cmd.Parameters.AddWithValue("@MCID", mcid);  // MCID value
                cmd.Parameters.AddWithValue("@Active", active);  // Active status (1 = active, 0 = inactive)
                cmd.Parameters.AddWithValue("@DepartmentSendBy", departmentSendBy);  // Department sender

                // Execute the CircularDetails insert
                cmd.ExecuteNonQuery();

                // Commit the transaction if the operation is successful
                transaction.Commit();

                return circularID;  // Return the CircularID, or another identifier if needed
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return -1;  // Return -1 if an exception occurred
            }
        }

        public static bool DeactivateRecord(int circularId, string MCCode)
        {
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();

                    using (SqlCommand cmd = new SqlCommand("Deactivate_eNigamCircular", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add input parameter for Circular ID
                        cmd.Parameters.AddWithValue("@CircularID", circularId);
                        cmd.Parameters.AddWithValue("@MCCode", MCCode);

                        // Add output parameter for success flag
                        SqlParameter successParam = new SqlParameter("@Success", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(successParam);

                        // Execute the command
                        cmd.ExecuteNonQuery();

                        // Check if the operation was successful
                        return (bool)successParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return false;  // Indicating failure in case of an exception
            }
        }


    }
}