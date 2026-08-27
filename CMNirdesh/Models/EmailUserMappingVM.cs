//using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class EmailUserMappingModel
    {
        public int MappingId { get; set; }

        // Stores UserId (varchar / GUID text)
        public string MappedUserId { get; set; }

        // Display fields
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }

        // Email being mapped
        public string EmailId { get; set; }

        public DateTime ActivatedOn { get; set; }
        public DateTime? DeactivatedOn { get; set; }

        // 🔥 REQUIRED for checkbox (Map / Unmap)
        public bool IsActive { get; set; }

        // For dropdown binding
        public List<mUsers> Users { get; set; }
    }


    public class UpdateUserDetailModel
    {
        [Required]
        public string UserId { get; set; }   // varchar(500)

        [Required(ErrorMessage = "Name is required")]
        [MinLength(3)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters and spaces allowed")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid mobile number")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Valid From date is required")]
       
        
        [DataType(DataType.Date)]
        public DateTime ValidFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ValidTo { get; set; }

        public List<mUsers> Users { get; set; }
        public List<UserModel> _LstUser { get; set; }
    }


    //public class UserDropdownVM
    //{
    //    public long UserId { get; set; }
    //    public string UserName { get; set; }
    //}


    public class EmailUserVM
    {
        public static List<mUsers> GetUsersByDepartment(string deptId)
        {
            List<mUsers> lstUsers = new List<mUsers>();
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetUsersByDepartment", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DeptId", deptId ?? (object)DBNull.Value);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            mUsers user = new mUsers
                            {
                                UserId = dr["UserId"] != DBNull.Value
                                ? Guid.Parse(dr["UserId"].ToString())
                                : Guid.Empty,

                                UserName = dr["UserName"] != DBNull.Value
                                    ? dr["UserName"].ToString()
                                    : string.Empty,
                            };

                            lstUsers.Add(user);
                        }
                    }
                }
            }

            return lstUsers;
        }

        public static void MapUser(string emailId, string userId, string Name, string MobileNo)
        {
            //string encName = CustomCryptoService.EncryptString(Name.Trim());
            //string encMobile = CustomCryptoService.EncryptString(MobileNo.Trim());
            //string encEmail = CustomCryptoService.EncryptString(emailId.Trim());

            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_MapEmailToUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmailId", userId);
                cmd.Parameters.AddWithValue("@UserId", emailId.Trim());
                cmd.Parameters.AddWithValue("@Name", Name.Trim());
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo.Trim());
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public static bool IsUserAlreadyMapped(string userId)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_IsUserAlreadyMapped", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@DeptID", SqlDbType.VarChar).Value = CurrentSession.DeptID;
                con.Open();
                return Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public static List<EmailUserMappingModel> GetMappedUsers()
        {
            string deptId = CurrentSession.DeptID;
            List<EmailUserMappingModel> list = new List<EmailUserMappingModel>();

            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetMappedUsers", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // ✅ Pass DeptId
                cmd.Parameters.AddWithValue("@DeptId", deptId);

                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new EmailUserMappingModel
                        {
                            MappingId = Convert.ToInt32(dr["MappingId"]),
                            EmailId = dr["EmailId"].ToString(),
                            Name = dr["Name"].ToString(),
                            MappedUserId = dr["MappedUserId"].ToString(),
                            UserName = dr["UserName"].ToString(),
                            ActivatedOn = Convert.ToDateTime(dr["ActivatedOn"]),
                            DeactivatedOn = dr["DeactivatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DeactivatedOn"]),
                            Designation = dr["Designation"].ToString(),
                            IsActive = Convert.ToBoolean(dr["IsActive"])
                        });
                    }
                }
            }

            return list;
        }
        public static void ToggleMapping(int mappingId, bool isActive)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_ToggleEmailUserMapping", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MappingId", mappingId);
                cmd.Parameters.AddWithValue("@IsActive", isActive);
                cmd.Parameters.AddWithValue("@ActionBy", "User Admin");
                cmd.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
   
        //this is the Admin Update the user details
        
        public static void UpdateUserProfile(UpdateUserDetailModel model, string deptId, string adminId)
        {

            //string encName = CustomCryptoService.EncryptString(model.Name.Trim());
            //string encMobile = CustomCryptoService.EncryptString(model.Mobile.Trim());
            //string encEmail = CustomCryptoService.EncryptString(model.Email.Trim());


            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_Admin_UpdateUserProfile", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 500).Value = model.UserId;
                cmd.Parameters.Add("@DeptId", SqlDbType.NVarChar, 100).Value = deptId;

                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = model.Name.Trim();
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 200).Value = model.Email.Trim();
                cmd.Parameters.Add("@MobileNo", SqlDbType.NVarChar, 20).Value = model.Mobile;

                cmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = model.ValidFrom;
                cmd.Parameters.Add("@ValidTo", SqlDbType.DateTime).Value =
                    (object)model.ValidTo ?? DBNull.Value;

                cmd.Parameters.Add("@AdminId", SqlDbType.VarChar, 500).Value = adminId;

                if (con.State == ConnectionState.Closed)
                    con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public static UpdateUserDetailModel GetUserDetails(string userId, string deptId)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetDepartmentUserDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 500).Value = userId;
                cmd.Parameters.Add("@DeptId", SqlDbType.NVarChar, 100).Value = deptId;

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new UpdateUserDetailModel
                        {     
                            Name = dr["Name"]?.ToString(),
                            Email = dr["EmailId"]?.ToString(),
                            Mobile = dr["MobileNo"]?.ToString()
                        };

                        //return new UpdateUserDetailModel
                        //{
                        //    Name = CustomCryptoService.DecryptString(dr["Name"]?.ToString()),
                        //    Email = CustomCryptoService.DecryptString(dr["EmailId"]?.ToString()),
                        //    Mobile = CustomCryptoService.DecryptString(dr["MobileNo"]?.ToString())
                        //};
                    }
                }
            }
            return null;
        }

    }
}