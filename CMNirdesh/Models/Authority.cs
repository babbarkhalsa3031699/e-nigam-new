using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CMNirdesh.Models
{
    public class Authority
    {
        public int AuthorityId { get; set; }
        public string AuthorityName { get; set; }
        public string PriFixName { get; set; }
        public string Name { get; set; }
        public string Name_Local { get; set; }
        public string Designation { get; set; }
        public string Designation_Local { get; set; }
        public string DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string SpouseName { get; set; }
        public string SpouseName_Local { get; set; }
        public string EducationalQualification { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Postings { get; set; }
        public string Training { get; set; }
        public string CountriesVisited { get; set; }
        public string SpecialInterest { get; set; }
        public string MeetingsAttended { get; set; }
        public string DesignationDescriptions { get; set; }
        public string Photo { get; set; }
        public string DateofMarriage { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public bool? IsDeleted { get; set; }
        public string FileName { get; set; }
        public string ThumbName { get; set; }
        public string FilePath { get; set; }
        public int? SecretoryId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int? Officetype { get; set; }
        public int? Order { get; set;}
    }

        public class AuthorityModel
        {
        public static List<Authority> AuthorityList()
            {
                List<Authority> authorities = new List<Authority>();
                try
                {
                    using (SqlConnection con = ClsConnection.GetConnection())
                    {
                        if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                            con.Open();

                        using (SqlCommand cmd = new SqlCommand("GetAuthorityList", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataSet ds = new DataSet();
                                da.Fill(ds);

                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    Authority auth = new Authority
                                    {
                                        AuthorityId = Convert.ToInt32(dr["AuthorityId"]),
                                        AuthorityName = Convert.ToString(dr["AuthorityName"]),
                                        PriFixName = Convert.ToString(dr["PriFixName"]),
                                        Name = Convert.ToString(dr["Name"]),
                                        Name_Local = Convert.ToString(dr["Name_Local"]),
                                        Designation = Convert.ToString(dr["Designation"]),
                                        Designation_Local = Convert.ToString(dr["Designation_Local"]),
                                        DateOfBirth = Convert.ToString(dr["DateOfBirth"]),
                                        MaritalStatus = Convert.ToString(dr["MaritalStatus"]),
                                        SpouseName = Convert.ToString(dr["SpouseName"]),
                                        SpouseName_Local = Convert.ToString(dr["SpouseName_Local"]),
                                        EducationalQualification = Convert.ToString(dr["EducationalQualification"]),
                                        PresentAddress = Convert.ToString(dr["PresentAddress"]),
                                        PermanentAddress = Convert.ToString(dr["PermanentAddress"]),
                                        Postings = Convert.ToString(dr["Postings"]),
                                        Training = Convert.ToString(dr["Training"]),
                                        CountriesVisited = Convert.ToString(dr["CountriesVisited"]),
                                        SpecialInterest = Convert.ToString(dr["SpecialInterest"]),
                                        MeetingsAttended = Convert.ToString(dr["MeetingsAttended"]),
                                        DesignationDescriptions = Convert.ToString(dr["DesignationDescriptions"]),
                                        Photo = Convert.ToString(dr["Photo"]),
                                        DateofMarriage = Convert.ToString(dr["DateofMarriage"]),
                                        IsActive = dr["IsActive"] != DBNull.Value ? (bool?)Convert.ToBoolean(dr["IsActive"]) : null,
                                        CreationDate = dr["CreationDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["CreationDate"]) : null,
                                        CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                        ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                        ModifiedWhen = dr["ModifiedWhen"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["ModifiedWhen"]) : null,
                                        IsDeleted = dr["IsDeleted"] != DBNull.Value ? (bool?)Convert.ToBoolean(dr["IsDeleted"]) : null,
                                        FileName = Convert.ToString(dr["FileName"]),
                                        ThumbName = Convert.ToString(dr["ThumbName"]),
                                        FilePath = Convert.ToString(dr["FilePath"]),
                                        SecretoryId = dr["SecretoryId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["SecretoryId"]) : null,
                                        Email = Convert.ToString(dr["Email"]),
                                        Mobile = Convert.ToString(dr["Mobile"]),
                                        Officetype = dr["officetype"] != DBNull.Value ? (int?)Convert.ToInt32(dr["officetype"]) : null,
                                        Order =  dr["Order"] != DBNull.Value ? (int?)Convert.ToInt32(dr["Order"]) : null,
                                    };

                                    authorities.Add(auth);
                                }
                            }
                        }
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    // Optionally log ex
                }

                return authorities;
            }

        public static int SaveAuthority(Authority model)
            {
                try
                {
                    using (SqlConnection con = ClsConnection.GetConnection())
                    {
                        if (con.State == ConnectionState.Closed) con.Open();

                        SqlCommand cmd = new SqlCommand("Authority_Save", con)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@AuthorityName", model.AuthorityName ?? "");
                        cmd.Parameters.AddWithValue("@PriFixName", model.PriFixName ?? "");
                        cmd.Parameters.AddWithValue("@Name", model.Name ?? "");
                        cmd.Parameters.AddWithValue("@Name_Local", model.Name_Local ?? "");
                        cmd.Parameters.AddWithValue("@Designation", model.Designation ?? "");
                        cmd.Parameters.AddWithValue("@Designation_Local", model.Designation_Local ?? "");
                        cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus ?? "");
                        cmd.Parameters.AddWithValue("@SpouseName", model.SpouseName ?? "");
                        cmd.Parameters.AddWithValue("@EducationalQualification", model.EducationalQualification ?? "");
                        cmd.Parameters.AddWithValue("@PresentAddress", model.PresentAddress ?? "");
                        cmd.Parameters.AddWithValue("@PermanentAddress", model.PermanentAddress ?? "");
                        cmd.Parameters.AddWithValue("@Postings", model.Postings ?? "");
                        cmd.Parameters.AddWithValue("@Training", model.Training ?? "");
                        cmd.Parameters.AddWithValue("@CountriesVisited", model.CountriesVisited ?? "");
                        cmd.Parameters.AddWithValue("@SpecialInterest", model.SpecialInterest ?? "");
                        cmd.Parameters.AddWithValue("@MeetingsAttended", model.MeetingsAttended ?? "");
                        cmd.Parameters.AddWithValue("@DesignationDescriptions", model.DesignationDescriptions ?? "");
                        cmd.Parameters.AddWithValue("@FileName", model.FileName ?? "");
                        cmd.Parameters.AddWithValue("@FilePath", model.FilePath ?? "");
                        cmd.Parameters.AddWithValue("@SecretoryId", "");
                        cmd.Parameters.AddWithValue("@Email", model.Email ?? "");
                        cmd.Parameters.AddWithValue("@Mobile", model.Mobile ?? "");
                        cmd.Parameters.AddWithValue("@Officetype", "");
                        cmd.Parameters.AddWithValue("@DateofMarriage", model.DateofMarriage ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                        cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);

                        cmd.ExecuteNonQuery();
                        return 1;
                    }
                }
                catch
                {
                    return 0;
                }
            }

        public static List<Authority> GetAuthorityById(int id)
        {
            List<Authority> authorities = new List<Authority>();

            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("GetAuthorityById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AuthorityId", id);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        authorities.Add(new Authority
                        {
                            AuthorityId = Convert.ToInt32(dr["AuthorityId"]),
                            AuthorityName = Convert.ToString(dr["AuthorityName"]),
                            PriFixName = Convert.ToString(dr["PriFixName"]),
                            Name = Convert.ToString(dr["Name"]),
                            Name_Local = Convert.ToString(dr["Name_Local"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            Designation_Local = Convert.ToString(dr["Designation_Local"]),
                            DateOfBirth = Convert.ToString(dr["DateOfBirth"]),
                            MaritalStatus = Convert.ToString(dr["MaritalStatus"]),
                            SpouseName = Convert.ToString(dr["SpouseName"]),
                            SpouseName_Local = Convert.ToString(dr["SpouseName_Local"]),
                            EducationalQualification = Convert.ToString(dr["EducationalQualification"]),
                            PresentAddress = Convert.ToString(dr["PresentAddress"]),
                            PermanentAddress = Convert.ToString(dr["PermanentAddress"]),
                            Postings = Convert.ToString(dr["Postings"]),
                            Training = Convert.ToString(dr["Training"]),
                            CountriesVisited = Convert.ToString(dr["CountriesVisited"]),
                            SpecialInterest = Convert.ToString(dr["SpecialInterest"]),
                            MeetingsAttended = Convert.ToString(dr["MeetingsAttended"]),
                            DesignationDescriptions = Convert.ToString(dr["DesignationDescriptions"]),
                            Photo = Convert.ToString(dr["Photo"]),
                            DateofMarriage = Convert.ToString(dr["DateofMarriage"]),
                            IsActive = dr["IsActive"] != DBNull.Value ? (bool?)Convert.ToBoolean(dr["IsActive"]) : null,
                            CreationDate = dr["CreationDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["CreationDate"]) : null,
                            CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                            ModifiedWhen = dr["ModifiedWhen"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["ModifiedWhen"]) : null,
                            IsDeleted = dr["IsDeleted"] != DBNull.Value ? (bool?)Convert.ToBoolean(dr["IsDeleted"]) : null,
                            FileName = Convert.ToString(dr["FileName"]),
                            ThumbName = Convert.ToString(dr["ThumbName"]),
                            FilePath = Convert.ToString(dr["FilePath"]),
                            SecretoryId = dr["SecretoryId"] != DBNull.Value ? (int?)Convert.ToInt32(dr["SecretoryId"]) : null,
                            Email = Convert.ToString(dr["Email"]),
                            Mobile = Convert.ToString(dr["Mobile"]),
                            Officetype = dr["officetype"] != DBNull.Value ? (int?)Convert.ToInt32(dr["officetype"]) : null
                        });
                    }
                }
            }

            return authorities;
        }

        public static int UpdateAuthority(Authority model)
        {
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed) con.Open();

                    using (SqlCommand cmd = new SqlCommand("Authority_Update", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AuthorityId", model.AuthorityId);
                        cmd.Parameters.AddWithValue("@AuthorityName", model.AuthorityName ?? "");
                        cmd.Parameters.AddWithValue("@PriFixName", model.PriFixName ?? "");
                        cmd.Parameters.AddWithValue("@Name", model.Name ?? "");
                        cmd.Parameters.AddWithValue("@Name_Local", model.Name_Local ?? "");
                        cmd.Parameters.AddWithValue("@Designation", model.Designation ?? "");
                        cmd.Parameters.AddWithValue("@Designation_Local", model.Designation_Local ?? "");
                        cmd.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaritalStatus", model.MaritalStatus ?? "");
                        cmd.Parameters.AddWithValue("@SpouseName", model.SpouseName ?? "");
                        cmd.Parameters.AddWithValue("@SpouseName_Local", model.SpouseName_Local ?? "");
                        cmd.Parameters.AddWithValue("@EducationalQualification", model.EducationalQualification ?? "");
                        cmd.Parameters.AddWithValue("@PresentAddress", model.PresentAddress ?? "");
                        cmd.Parameters.AddWithValue("@PermanentAddress", model.PermanentAddress ?? "");
                        cmd.Parameters.AddWithValue("@Postings", model.Postings ?? "");
                        cmd.Parameters.AddWithValue("@Training", model.Training ?? "");
                        cmd.Parameters.AddWithValue("@CountriesVisited", model.CountriesVisited ?? "");
                        cmd.Parameters.AddWithValue("@SpecialInterest", model.SpecialInterest ?? "");
                        cmd.Parameters.AddWithValue("@MeetingsAttended", model.MeetingsAttended ?? "");
                        cmd.Parameters.AddWithValue("@DesignationDescriptions", model.DesignationDescriptions ?? "");
                        cmd.Parameters.AddWithValue("@FileName", model.FileName ?? "");
                        cmd.Parameters.AddWithValue("@FilePath", model.FilePath ?? "");
                        cmd.Parameters.AddWithValue("@SecretoryId", "");
                        cmd.Parameters.AddWithValue("@Email", model.Email ?? "");
                        cmd.Parameters.AddWithValue("@Mobile", model.Mobile ?? "");
                        cmd.Parameters.AddWithValue("@Officetype", "");
                        cmd.Parameters.AddWithValue("@DateofMarriage", model.DateofMarriage ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                        cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                        cmd.Parameters.AddWithValue("@ModifiedWhen", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error here if needed
                return 0;
            }
        }


        public static int AuthorityDeleteById(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@AuthorityId", Id),


                };
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[AuthorityRecordDeleteById]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }
        public static void ChangeOrder(int id, int order)
        {

            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                        con.Open();

                    using (SqlCommand cmd = new SqlCommand("UpdateAuthorityOrder", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AuthorityId", id);
                        cmd.Parameters.AddWithValue("@Order", order);
                        cmd.ExecuteNonQuery();
                    }
                    // result = 1;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Optional: Log exception
                //result = 0;
            }


        }


    }

    public class AuthorityList
        {
            public List<Authority> _LstAuthority { get; set; }
            public string fileacessingUrl { get; set; }
        }
}
