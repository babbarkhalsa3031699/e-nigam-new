using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    [Serializable]
    [Table("ReferencePriority")]
    public class ReferencePriority
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PriorityType { get; set; }

        public string Description { get; set; }
        public bool IsDeleted { get; set; }



        public static List<ReferencePriority> GetReferencePriorityList()
        {

            List<ReferencePriority> lstdept = new List<ReferencePriority>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mReferencePriorityIndex");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ReferencePriority item = new ReferencePriority();
                    item.Id = Convert.ToInt32(dr["id"]);
                    item.PriorityType = Convert.ToString(dr["PriorityType"]);
                    item.Description = Convert.ToString(dr["Description"]);
                    item.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    lstdept.Add(item);
                }
                return lstdept;
            }
            catch
            {
                return lstdept;
            }

        }
    

        public static int SaveReferencePriorityRecord(ReferencePriority referencePriority)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@PriorityType", referencePriority.PriorityType);
                param[1] = new SqlParameter("@Description", referencePriority.Description);
                param[2] = new SqlParameter("@CreatedBy", CurrentSession.UserID);
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "mReferencePriorityInsert", param);
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<ReferencePriority> GetReferencePriorityById(int Id)
        {
            List<ReferencePriority> lstdept = new List<ReferencePriority>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", Id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mReferencePriorityGetByid", param).Tables[0].Rows)
                {
                    ReferencePriority item = new ReferencePriority();
                    {
                        item.Id = Convert.ToInt32(dr["Id"]);
                        item.PriorityType = Convert.ToString(dr["PriorityType"]);
                        item.Description = Convert.ToString(dr["Description"]);
                    };
                    lstdept.Add(item);
                }

                return lstdept;
            }
            catch (Exception ex)
            {
                return lstdept;

            }
        }
        public static int UpdateReferencePriorityRecordById(ReferencePriority model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@Id", model.Id);
                param[1] = new SqlParameter("@PriorityType", model.PriorityType);
                param[2] = new SqlParameter("@Description", model.Description);
                param[3] = new SqlParameter("@ModifiedBy", CurrentSession.UserID);
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "mReferencePriorityUpdate", param);
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int DeleteReferencePriorityRecordById(int Id)
        {


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@id", Id),


                };
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mReferencePriorityDelete]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

    }



        public class LstReferencePriority
        {
            private List<ReferencePriority> lstReferencePriority;

            public List<ReferencePriority> _LstReferencePriority { get => lstReferencePriority; set => lstReferencePriority = value; }
        }
    }

