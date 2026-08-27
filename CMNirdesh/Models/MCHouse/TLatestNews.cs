using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.MCHouse
{
    public class TLatestNews
    {
        public List<TLatestNews> _LstTLatestNews;
        public string fileacessingUrl { get; set; }
        public string NewsTitle { get; set; }
        public string NewsTitle_local { get; set; }
        public string NewsDescription { get; set; }

        public string NewsDescription_local { get; set; }

        public string Images { get; set; }

        public DateTime CreatedDate { get; set; }

        public UniqueConstraint CreatedBy { get; set; }

        public UniqueConstraint ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string FileName { get; set; }

        public int MCId { get; set; }

        public int Ordering { get; set; }

        public int LatestNewsID { get; set; }

        public bool IsDeleted { get; set; }

        public static List<TLatestNews> GetNewsList()
        {

            List<TLatestNews> lst = new List<TLatestNews>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "TLatestNewsIndex");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TLatestNews items = new TLatestNews();
                    items.LatestNewsID = Convert.ToInt32(dr["LatestNewsID"]);
                    items.NewsTitle = Convert.ToString(dr["NewsTitle"]);
                    items.NewsTitle_local = Convert.ToString(dr["NewsTitle_local"]);
                    items.NewsDescription = Convert.ToString(dr["NewsDescription"]);
                    items.NewsDescription_local = Convert.ToString(dr["NewsDescription_local"]);
                    items.Images = Convert.ToString(dr["FileName"]);
                    items.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    //items.Ordering = Convert.ToInt32(dr["Ordering"]);
                    lst.Add(items);
                }
                return lst;
            }
            catch
            {
                return lst;
            }

        }

        public static int InsertNewsRecord(TLatestNews model)
        {

            Guid createdByGuid;
            bool isGuid = Guid.TryParse(CurrentSession.UserID, out createdByGuid);

            

            try
            {
              
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("TLatestNewsInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NewsTitle", model.NewsTitle);
                cmd.Parameters.AddWithValue("@NewsTitle_local", model.NewsTitle_local);
                cmd.Parameters.AddWithValue("@NewsDescription", model.NewsDescription);
                cmd.Parameters.AddWithValue("@NewsDescription_local", model.NewsDescription_local);
                cmd.Parameters.AddWithValue("@FileName", model.FileName);
                if (isGuid)
                {
                    cmd.Parameters.AddWithValue("@CreatedBy", createdByGuid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CreatedBy", DBNull.Value);
                }

               // cmd.Parameters.AddWithValue("@Ordering", model.Ordering);
                cmd.Parameters.AddWithValue("@Mcid", model.MCId);
              
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
                else if (errorCode == 1)
                {
                    return 1; // Duplicate Ordering value
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



        public static List<TLatestNews> GetNewsRecord(int id)
        {
            List<TLatestNews> lst = new List<TLatestNews>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@LatestNewsID", id);
                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "TLatestNewsGetById", param).Tables[0].Rows)
                {
                    TLatestNews items = new TLatestNews();
                    {
                        items.NewsTitle = Convert.ToString(dr["NewsTitle"]);
                        items.NewsTitle_local = Convert.ToString(dr["NewsTitle_local"]);
                        items.NewsDescription = Convert.ToString(dr["NewsDescription"]);
                        items.NewsDescription_local = Convert.ToString(dr["NewsDescription_local"]);
                        items.MCId = Convert.ToInt32(dr["MCId"]);
                        items.Ordering = Convert.ToInt32(dr["Ordering"]);
                        items.Images = "/SecureFileStructure/News/" + Convert.ToString(dr["FileName"]);
                        items.LatestNewsID = id;
                    };
                    lst.Add(items);
                }
                return lst;
            }
            catch (Exception ex)
            {
                return lst;
            }
        }
        public static int UpdateNewsRecord(TLatestNews model)
        {

            Guid createdByGuid;
            bool isGuid = Guid.TryParse(CurrentSession.UserID, out createdByGuid);



            try
            {

                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("TLatestNewsUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LatestNewsID", model.LatestNewsID);
                cmd.Parameters.AddWithValue("@NewsTitle", model.NewsTitle);
                cmd.Parameters.AddWithValue("@NewsTitle_local", model.NewsTitle_local);
                cmd.Parameters.AddWithValue("@NewsDescription", model.NewsDescription);
                cmd.Parameters.AddWithValue("@NewsDescription_local", model.NewsDescription_local);
                cmd.Parameters.AddWithValue("@FileName", model.FileName);
                if (isGuid)
                {
                    cmd.Parameters.AddWithValue("@ModifiedBy", createdByGuid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ModifiedBy", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@Ordering", model.Ordering);
                cmd.Parameters.AddWithValue("@Mcid", model.MCId);

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
                else if (errorCode == 1)
                {
                    return 1; // Duplicate Ordering value
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

        public static string DeleteNewsRecord(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@LatestNewsID", Id),


                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[TLatestNewsDelete]", parameterValues));
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

       



    