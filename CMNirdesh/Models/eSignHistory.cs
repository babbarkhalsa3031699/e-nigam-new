using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class eSignHistory
    {
        public int Id { get; set; }              
        public string UserID { get; set; }      
        public string Status { get; set; }       
        public string TransactionID { get; set; } 
        public DateTime ResponseTime { get; set; } 
        public string Name { get; set; }        
        public int SerialNumber { get; set; }   
        public DateTime SignedAt { get; set; }
        public string requestXML { get; set; }
        public string responseXML { get; set; }
    }

    public class eSignHistoryModel
    {
        public static string SaveESignHistory(eSignHistory history)
        {
            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("sp_InserteSignHistory", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Adding parameters
                        command.Parameters.AddWithValue("@UserID", history.UserID);
                        command.Parameters.AddWithValue("@Status", history.Status);
                        command.Parameters.AddWithValue("@TransactionID", history.TransactionID);
                        command.Parameters.AddWithValue("@ResponseTime", history.ResponseTime);
                        command.Parameters.AddWithValue("@Name", history.Name);
                        command.Parameters.AddWithValue("@SerialNumber", history.SerialNumber);
                        command.Parameters.AddWithValue("@requestXML", history.requestXML);
                        command.Parameters.AddWithValue("@responseXML", history.responseXML);

                        // Add an output parameter to capture the return code from the stored procedure
                        SqlParameter returnValue = new SqlParameter("@RETURN_VALUE", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.ReturnValue
                        };

                        command.Parameters.Add(returnValue);

                        // Execute the command
                        command.ExecuteNonQuery();

                        // Get the return code
                        int returnCode = (int)returnValue.Value;

                        if (returnCode == 0)
                        {
                            // Success
                            return "Success";
                        }
                        else if (returnCode == 1)
                        {
                            // Handle specific return code (example)
                            return "Transaction ID already exists";
                        }
                        else
                        {
                            // Handle other return codes if needed
                            return "Other return code: " + returnCode.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (assuming ErrorLog is defined)
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return "Error: " + ex.Message;
            }
        }
    }
}
    