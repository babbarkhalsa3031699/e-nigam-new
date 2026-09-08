using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.MCHouse
{
    public class HomeDashBoard
    {
        public List<HomeDashBoard> _LstHomeDashBoard;

        public int MCId { get; set; }

        public string MCName { get; set; }
        public string Houses { get; set; }
        public int Councilors { get; set; }

        public int HouseMeetings { get; set; }

        public int Departments { get; set; }

        public DateTime CreatedDate { get; set; }

        public int FCCMeetings { get; set; }

        public int Employees { get; set; }

        public DateTime ModifiedDate { get; set; }
      
        public bool IsDeleted { get; set; }

        public string AssemblyName { get; set; }

        public static List<HomeDashBoard> GetList()
        {

            List<HomeDashBoard> lst = new List<HomeDashBoard>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "DashBoardList");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HomeDashBoard items = new HomeDashBoard();
                    items.MCId = Convert.ToInt32(dr["mcid"]);
                    items.MCName = Convert.ToString(dr["MCName"]);
                    items.Houses = Convert.ToString(dr["Houses"]);
                    items.Councilors = Convert.ToInt32(dr["Councilors"]);
                    items.HouseMeetings = Convert.ToInt32(dr["HouseMeetings"]);
                    items.Departments = Convert.ToInt32(dr["Departments"]);
                    items.FCCMeetings = Convert.ToInt32(dr["FCCMeetings"]);
                    items.Employees = Convert.ToInt32(dr["Employees"]);
                    items.IsDeleted = Convert.ToBoolean(dr["Active"]);
                    lst.Add(items);
                }
                return lst;
            }
            catch
            {
                return lst;
            }

        }

        public static int InsertRecord(HomeDashBoard model)
        {

          
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("DashBoardInsertRecord", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mcid", model.MCId);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                   //cmd.Parameters.Add("@IsActive", mDepartment.IsActive);
                SqlParameter parmOUT = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parmOUT.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parmOUT);
                cmd.ExecuteNonQuery();
                int errorCode = (int)cmd.Parameters["@ErrorCode"].Value;

                if (errorCode == 0)
                {
                    return 0; // Insertion Successful
                }
                else
                {
                    return -1; // Error occurred during insertion
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return -1; // Exception occurred
            }
        }
       
        public static int UpdateRecord(HomeDashBoard model)
        {
            Guid createdByGuid;
            bool isGuid = Guid.TryParse(CurrentSession.UserID, out createdByGuid);
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("DashBoardUpdateRecord", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mcid", model.MCId);
                cmd.Parameters.AddWithValue("@Houses", model.Houses);
                cmd.Parameters.AddWithValue("@Councilors", model.Councilors);
                cmd.Parameters.AddWithValue("@HouseMeetings", model.HouseMeetings);
                cmd.Parameters.AddWithValue("@Departments", model.Departments);
                cmd.Parameters.AddWithValue("@FCCMeetings", model.FCCMeetings);
                cmd.Parameters.AddWithValue("@Employees", model.Employees);
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                SqlParameter parmOUT = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parmOUT.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parmOUT);
                cmd.ExecuteNonQuery();
                int errorCode = (int)cmd.Parameters["@ErrorCode"].Value;
                if (errorCode == 0)
                {
                    return 0; // Insertion Successful
                }
     
                else
                {
                    return -1; // Error occurred during insertion
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return -1; // Exception occurred
            }
        }

        public static string DeleteRecord(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@Id", Id),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DashBoardDeleteRecord]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

    }
}