using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class LgPolicy
    {
        public int PolicyId { get; set; }
        public string PolicyTitle { get; set; }
        public string PolicyTitle_Punjabi { get; set; }
        public string Description { get; set; }
        public string Description_Punjabi { get; set; }
        public string PolicyDocumentPath { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DataViewModelPolicy
    {
        public static int SavePolicy(LgPolicy model)
        {
            int errorCode = -1;
            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("InsertLGPolicyRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PolicyTitle", model.PolicyTitle);
                    cmd.Parameters.AddWithValue("@PolicyTitle_Punjabi", (object)model.PolicyTitle_Punjabi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description_Punjabi", (object)model.Description_Punjabi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PolicyDocumentPath", (object)model.PolicyDocumentPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                    cmd.Parameters.AddWithValue("@StartDate", (object)model.StartDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", (object)model.EndDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);

                    SqlParameter errorParam = new SqlParameter("@ErrorCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(errorParam);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    errorCode = Convert.ToInt32(errorParam.Value);
                }
            }
            return errorCode;
        }

        public static List<LgPolicy> GetList()
        {
            List<LgPolicy> list = new List<LgPolicy>();
            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("LGPolicyList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new LgPolicy
                            {
                                PolicyId = Convert.ToInt32(rdr["PolicyId"]),
                                PolicyTitle = rdr["PolicyTitle"].ToString(),
                                PolicyTitle_Punjabi = rdr["PolicyTitle_Punjabi"].ToString(),
                                Description = rdr["Description"].ToString(),
                                Description_Punjabi = rdr["Description_Punjabi"].ToString(),
                                PolicyDocumentPath = rdr["PolicyDocumentPath"].ToString(),
                                CategoryId = Convert.ToInt32(rdr["CategoryId"]),
                                CategoryName = rdr["CategoryName"].ToString(),
                                StartDate = rdr["StartDate"] as DateTime?,
                                EndDate = rdr["EndDate"] as DateTime?,
                                CreatedBy = rdr["CreatedBy"].ToString(),
                                CreatedOn = Convert.ToDateTime(rdr["CreatedOn"]),
                                IsActive = Convert.ToBoolean(rdr["IsActive"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        public static List<LgPolicy> GetRowByIdList(int id)
        {
            List<LgPolicy> list = new List<LgPolicy>();

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("LgPolicy_GetById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            LgPolicy model = new LgPolicy
                            {
                                PolicyId = Convert.ToInt32(rdr["PolicyId"]),
                                PolicyTitle = rdr["PolicyTitle"].ToString(),
                                PolicyTitle_Punjabi = rdr["PolicyTitle_Punjabi"].ToString(),
                                Description = rdr["Description"].ToString(),
                                Description_Punjabi = rdr["Description_Punjabi"].ToString(),
                                PolicyDocumentPath = rdr["PolicyDocumentPath"].ToString(),
                                CategoryId = Convert.ToInt32(rdr["CategoryId"]),
                                StartDate = rdr["StartDate"] as DateTime?,
                                EndDate = rdr["EndDate"] as DateTime?,
                                CreatedBy = rdr["CreatedBy"].ToString(),
                                CreatedOn = Convert.ToDateTime(rdr["CreatedOn"]),
                                ModifiedBy = rdr["ModifiedBy"].ToString(),
                                ModifiedOn = rdr["ModifiedOn"] as DateTime?,
                                IsActive = Convert.ToBoolean(rdr["IsActive"])
                            };
                            list.Add(model);
                        }
                    }
                }
            }

            return list;
        }

        public static bool UpdatePolicy(LgPolicy model)
        {
            bool success = false;
            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("LgPolicy_UpdateById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PolicyId", model.PolicyId);
                    cmd.Parameters.AddWithValue("@PolicyTitle", model.PolicyTitle);
                    cmd.Parameters.AddWithValue("@PolicyTitle_Punjabi", (object)model.PolicyTitle_Punjabi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", (object)model.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description_Punjabi", (object)model.Description_Punjabi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PolicyDocumentPath", (object)model.PolicyDocumentPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                    cmd.Parameters.AddWithValue("@StartDate", (object)model.StartDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", (object)model.EndDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                    cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);

                    con.Open();
                    success = cmd.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        public static bool DeleteRecord(int id)
        {
            bool success = false;
            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("LGPolicy_DeleteRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    success = cmd.ExecuteNonQuery() > 0;
                }
            }
            return success;
        }

        public List<SelectListItem> GetPolicyCategories()
        {
            List<SelectListItem> categories = new List<SelectListItem>();

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("usp_GetPolicyCategories", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    categories.Add(new SelectListItem
                    {
                        Value = rdr["CategoryId"].ToString(),
                        Text = rdr["CategoryName"].ToString()
                    });
                }
                con.Close();
            }

            return categories;
        }
    }

    public class DataViewModelPolicyList
    {
        public List<LgPolicy> lgPolicies { get; set; }
        public string fileacessingUrl { get; set; }
    }


}