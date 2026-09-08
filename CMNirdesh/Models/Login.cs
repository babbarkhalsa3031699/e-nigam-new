using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using CMNirdesh.Models;


namespace CMNirdesh.Models
{
    public class Login
    {

        public static LoginMessage WrongAttemptLoginData()
        {

            return new LoginMessage() { IsValid = true, Message = "Login success" };

        }

        public static mUsers GetIsMemberNameAndPhotoDetails(string UserID)
        {
            mUsers model = new mUsers();

            DataSet ds = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserID", UserID) };
                ds = SqlHelper.ExecuteDataset(connection, "GetIsMemberNameAndPhotoDetails", parameterValues);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int num;
                    bool isNum = Int32.TryParse(ds.Tables[0].Rows[0]["UserName"].ToString(), out num);
                    model.Name = ds.Tables[0].Rows[0]["UserName"].ToString();
                    model.Photo = ds.Tables[0].Rows[0]["Photo"].ToString();
                }
            }
            catch (Exception)
            {
            }


            return model;
        }

        public static DataSet GetLogin(string UserID)
        {
            DataSet set = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserID", UserID) };
                set = SqlHelper.ExecuteDataset(connection, "SelectUserbyUserId", parameterValues);
            }
            catch (Exception)
            {
            }
            return set;
        }
        //public static List<mUsers> GetDetailsByadhaarID(string AadarId)
        //{
        //    List<mUsers> lst = new List<mUsers>();
        //    DataSet set = new DataSet();
        //    try
        //    {
        //        SqlConnection connection = ClsConnection.GetConnection();
        //        if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
        //        {
        //            connection.Open();
        //        }
        //        connection.Close();
        //        SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AadarId", AadarId) };
        //        set = SqlHelper.ExecuteDataset(connection, "GetDetailsByadhaarID", parameterValues);

        //        foreach (DataRow dr in set.Tables[0].Rows)
        //        {
        //            mUsers user = new mUsers();
        //            user.AadarId = dr["AadarId"].ToString();
        //            user.UserName = dr["UserName"].ToString();
        //            user.UserId = (Guid)(dr["UserId"]);
        //            user.UserType = Convert.ToInt32(dr["UserType"].ToString());
        //            user.IsMember = Convert.ToString(dr["IsMember"].ToString());
        //            user.OfficeId = Convert.ToInt64(dr["OfficeId"].ToString());
        //            user.ofcType = dr["IsMC"].ToString();
        //            user.DeptId = Convert.ToString(dr["DeptId"].ToString());
        //            user.StateId= Convert.ToString(dr["StateId"].ToString());
        //            user.SubdivisionCode = Convert.ToString(dr["SubdivisionCode"].ToString());
        //            user.Password = Convert.ToString(dr["Password"].ToString());
        //            user.isApex = Convert.ToInt32(dr["isApex"].ToString());
        //            user.MapId = 0;
        //            if (dr["mapid"] != System.DBNull.Value)
        //            {
        //                user.MapId = Convert.ToInt32(dr["mapid"].ToString());
        //            }
        //            user.isParent = dr["isParent"].ToString();
        //            user.MobileNo = dr["mobileno"].ToString();
        //            lst.Add(user);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return lst;

        //}

        public static List<mUsers> GetDetailsByadhaarID(string AadarId)
        {
            List<mUsers> lst = new List<mUsers>();
            DataSet set = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                //connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AadarId", AadarId) };
                set = SqlHelper.ExecuteDataset(connection, "GetDetailsByadhaarID", parameterValues);

                foreach (DataRow dr in set.Tables[0].Rows)
                {
                    mUsers user = new mUsers();
                    user.Name = dr["Name"].ToString();
                    user.Designation = dr["Designation"].ToString();
                    user.DesignationLocal = dr["DesignationLocal"].ToString();
                    user.DepartmentName = dr["deptname"].ToString();
                    user.AadarId = dr["AadarId"].ToString();
                    user.UserName = dr["UserName"].ToString();
                    user.UserId = (Guid)(dr["UserId"]);
                    user.UserType = Convert.ToInt32(dr["UserType"].ToString());
                    user.ofcType = "0";
                    if (dr["IsMC"] != System.DBNull.Value)
                    {
                        user.ofcType = Convert.ToString(dr["IsMC"].ToString());
                    }
                    user.MCName = Convert.ToString(dr["MCName"].ToString());
                    user.IsMember = Convert.ToString(dr["IsMember"].ToString());
                    user.OfficeId = Convert.ToInt64(dr["OfficeId"].ToString());
                    user.DeptId = Convert.ToString(dr["DeptId"].ToString());
                    user.StateId = Convert.ToString(dr["StateId"].ToString());
                    user.SubdivisionCode = Convert.ToString(dr["SubdivisionCode"].ToString());
                    user.Password = Convert.ToString(dr["Password"].ToString());
                    user.SignaturePath= Convert.ToString(dr["SignaturePath"].ToString());
                    user.isDeptApex= Convert.ToInt32(dr["isDeptApex"].ToString());
                    if (dr["ApprovalAuthority"] != System.DBNull.Value)
                    {
                        user.isApprovalAuthority = Convert.ToInt32(dr["ApprovalAuthority"].ToString());
                    }
                    else
                    {
                        user.isApprovalAuthority = 0;
                    }
                  

                    user.isApex = Convert.ToInt32(dr["isApex"].ToString());
                    user.MapId = 0;
                    if (dr["mapid"] != System.DBNull.Value)
                    {
                        user.MapId = Convert.ToInt32(dr["mapid"].ToString());
                    }
                    user.isParent = dr["isParent"].ToString();
                    user.MobileNo = dr["mobileno"].ToString();
                   
                    // ✅ IMPORTANT PART (NULL SAFE)
                    user.IsPasswordChanged =
                        dr.Table.Columns.Contains("IsPasswordChanged") && dr["IsPasswordChanged"] != DBNull.Value
                        ? Convert.ToBoolean(dr["IsPasswordChanged"])
                        : (bool?)null;

                    lst.Add(user);
                }

            }
            catch (Exception)
            {
            }
            return lst;

        }


        public static Boolean VerifyHash(string plainText, string hashAlgorithm, string hashValue)
        {
            string expectedHashString = plainText;

            if (Convert.ToString(hashValue) == expectedHashString)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public static Boolean VerifyHash(string plainText, string hashAlgorithm, string hashValue)
        //{
        //    // Convert base64-encoded hash value into a byte array.
        //    byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

        //    // We must know size of hash (without salt).
        //    int hashSizeInBits, hashSizeInBytes;

        //    // Make sure that hashing algorithm name is specified.
        //    if (hashAlgorithm == null)
        //        hashAlgorithm = "";

        //    // Size of hash is based on the specified algorithm.
        //    switch (hashAlgorithm.ToUpper())
        //    {
        //        case "SHA1":
        //            hashSizeInBits = 160;
        //            break;

        //        case "SHA256":
        //            hashSizeInBits = 256;
        //            break;

        //        case "SHA384":
        //            hashSizeInBits = 384;
        //            break;

        //        case "SHA512":
        //            hashSizeInBits = 512;
        //            break;

        //        default: // Must be MD5
        //            hashSizeInBits = 128;
        //            break;
        //    }

        //    // Convert size of hash from bits to bytes.
        //    hashSizeInBytes = hashSizeInBits / 8;

        //    // Make sure that the specified hash value is long enough.
        //    if (hashWithSaltBytes.Length < hashSizeInBytes)
        //        return false;
         
        //    // Allocate array to hold original salt bytes retrieved from hash.
        //    byte[] saltBytes = new byte[hashWithSaltBytes.Length -
        //                                hashSizeInBytes];

        //    // Copy salt from the end of the hash to the new array.
        //    for (int i = 0; i < saltBytes.Length; i++)
        //        saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

        //    // Compute a new hash string.
        //    string expectedHashString =
        //                ComputeHash(plainText, hashAlgorithm, saltBytes);

        //    // If the computed hash matches the specified hash,
        //    // the plain text value must be correct.

        //    return (hashValue == expectedHashString);
        //}


        #region Encryption/Decription
        public static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];


            HashAlgorithm hash;
            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        //Not in Use....
        public string DecryptData(string encryptedtext)
        {
            TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider();
            // Convert the encrypted text string to a byte array.
            byte[] encryptedBytes = Convert.FromBase64String(encryptedtext);

            // Create the stream.
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            // Create the decoder to write to the stream.
            CryptoStream decStream = new CryptoStream(ms, tripleDes.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);

            // Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            //decStream.Write(encryptedBytes,0,
            decStream.FlushFinalBlock();

            // Convert the plaintext stream to a string.
            return System.Text.Encoding.Unicode.GetString(ms.ToArray());
        }

        #endregion
        public static String changeUserLoginPwd(string username, string password)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@username", username);
                param[1] = new SqlParameter("@password", password);
                foreach (DataRow row in SqlHelper.ExecuteDataset(con, "Updateuserpassword", param).Tables[0].Rows)
                {
                    return row["RESULT"].ToString();
                }
                con.Close();
                return "N";
            }
            catch (Exception ex)
            {
                return "N";

            }
        }
        public static String CheckUserExist(string username)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@username", username);
                foreach (DataRow row in SqlHelper.ExecuteDataset(con, "checkUserExists", param).Tables[0].Rows)
                {
                    return row["RESULT"].ToString();
                }
                con.Close();
                return "N";
            }
            catch (Exception ex)
            {
                return "N";

            }
        }

        public static void UpdateUserPassword(Guid userId, string hashedPassword, string Mobile, string Email)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_UpdateUserPassword", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@Mobile", Mobile);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@UpdatedOn", DateTime.Now);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public static bool IsMobileAlreadyExists(string mobile, Guid userId)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_CheckMobileExists", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mobile", mobile);
                cmd.Parameters.AddWithValue("@UserId", userId);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }


    }
}
