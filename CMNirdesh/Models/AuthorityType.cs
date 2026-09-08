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
    public class AuthorityType
    {
        public int AuthorityTypeId { get; set; }
        public string AuthorityTypeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class AuthorityTypeModel
    {
        public static List<AuthorityType> AuthorityTypeList()
        {
            List<AuthorityType> list = new List<AuthorityType>();

            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();

                    using (SqlCommand cmd = new SqlCommand("GetAuthorityTypeList", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                AuthorityType model = new AuthorityType
                                {
                                    AuthorityTypeId = Convert.ToInt32(dr["AuthorityTypeId"]),
                                    AuthorityTypeName = Convert.ToString(dr["AuthorityTypeName"]),
                                    Description = Convert.ToString(dr["Description"]),
                                    IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                    CreatedDate = (DateTime)(dr["CreatedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["CreatedDate"]) : null),
                                    CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                    ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                    ModifiedDate = dr["ModifiedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["ModifiedDate"]) : null
                                };

                                list.Add(model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log the error
            }

            return list;
        }
        public static int SaveAuthorityType(AuthorityType model)
        {
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    SqlCommand cmd = new SqlCommand("InsertAuthorityType", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@AuthorityTypeName", model.AuthorityTypeName ?? "");
                    cmd.Parameters.AddWithValue("@Description", model.Description ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID); // Or use "admin" if session not available

                    cmd.ExecuteNonQuery();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }
        public static AuthorityType GetAuthorityTypeById(int id)
        {
            AuthorityType model = null;

            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    using (SqlCommand cmd = new SqlCommand("GetAuthorityTypeById", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AuthorityTypeId", id);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                model = new AuthorityType
                                {
                                    AuthorityTypeId = Convert.ToInt32(dr["AuthorityTypeId"]),
                                    AuthorityTypeName = dr["AuthorityTypeName"].ToString(),
                                    Description = dr["Description"]?.ToString(),
                                    IsActive = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]),
                                    CreatedDate = (DateTime)(dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null),
                                    CreatedBy = dr["CreatedBy"]?.ToString(),
                                    ModifiedBy = dr["ModifiedBy"]?.ToString(),
                                    ModifiedDate = dr["ModifiedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedDate"]) : (DateTime?)null
                                };
                            }
                        }
                    }
                }
            }
            catch
            {
                model = null;
            }

            return model;
        }
        public static int UpdateAuthorityType(AuthorityType model)
        {
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    SqlCommand cmd = new SqlCommand("UpdateAuthorityType", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@AuthorityTypeId", model.AuthorityTypeId);
                    cmd.Parameters.AddWithValue("@AuthorityTypeName", model.AuthorityTypeName ?? "");
                    cmd.Parameters.AddWithValue("@Description", model.Description ?? "");
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                    cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID); // or "admin"

                    cmd.ExecuteNonQuery();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static int DeleteAuthorityType(int id)
        {
            int result = 0;
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();

                    using (SqlCommand cmd = new SqlCommand("DeleteAuthorityType", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AuthorityTypeId", id);

                        cmd.ExecuteNonQuery();
                    }
                    result = 1;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Optional: Log exception
                result = 0;
            }

            return result;
        }

        public static List<SelectListItem> GetAuthorityTypeDropdown_SP()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetActiveAuthorityTypes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new SelectListItem
                            {
                                Value = rdr["AuthorityTypeId"].ToString(),
                                Text = rdr["AuthorityTypeName"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }

     }

    public class AuthorityTypeList
    {
        public List<AuthorityType> _LstAuthorityType { get; set; }
        public string fileacessingUrl { get; set; }
    }
}