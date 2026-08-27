using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace CMNirdesh.Models
{
    public static class ActiveSessionManager
    {
        private static bool _tableInitialized = false;
        private static readonly object _initLock = new object();

        /// <summary>
        /// Ensures the tracking table exists in the database.
        /// </summary>
        private static void EnsureTableExists()
        {
            if (_tableInitialized) return;

            lock (_initLock)
            {

                try
                {
                    using (SqlConnection connection = ClsConnection.GetConnection())
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        // Create table with SessionGuid and IsActive if it does not exist
                        string createTableSql = @"
                            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserActiveSessions' AND xtype='U')
                            BEGIN
                                CREATE TABLE UserActiveSessions (
                                    SessionGuid NVARCHAR(128) PRIMARY KEY,
                                    UserId NVARCHAR(128) NOT NULL,
                                    IsActive INT NOT NULL DEFAULT 1,
                                    CreatedDateTime DATETIME NOT NULL DEFAULT GETDATE()
                                )
                            END";

                        using (SqlCommand cmd = new SqlCommand(createTableSql, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    _tableInitialized = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error initializing UserActiveSessions table: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Registers a new session by deactivating all previous active sessions and inserting a new active session.
        /// </summary>
        public static void RegisterSession(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return;

            EnsureTableExists();

            try
            {
                string sessionGuid = Guid.NewGuid().ToString();
                
                // Store GUID in ASP.NET Session
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["UserSessionGuid"] = sessionGuid;
                }

                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    // Deactivate all previous sessions for this user, then insert the new session
                    string sql = @"
                        UPDATE UserActiveSessions 
                        SET IsActive = 0 
                        WHERE UserId = @UserId AND IsActive = 1;

                        INSERT INTO UserActiveSessions (SessionGuid, UserId, IsActive, CreatedDateTime)
                        VALUES (@SessionGuid, @UserId, 1, GETDATE());";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@SessionGuid", sessionGuid);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error registering session: " + ex.Message);
            }
        }

        /// <summary>
        /// Validates if the current request's session GUID is still active in the database.
        /// </summary>
        public static bool IsSessionActive(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return true;

            EnsureTableExists();

            if (HttpContext.Current.Session == null) return true;

            string sessionGuid = HttpContext.Current.Session["UserSessionGuid"] as string;

            // Fallback for existing sessions (e.g. logged in before deployment): register new session
            if (string.IsNullOrEmpty(sessionGuid))
            {
                RegisterSession(userId);
                return true;
            }

            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    string selectSql = "SELECT IsActive FROM UserActiveSessions WHERE UserId = @UserId AND SessionGuid = @SessionGuid";
                    using (SqlCommand cmd = new SqlCommand(selectSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@SessionGuid", sessionGuid);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            int isActive = Convert.ToInt32(result);
                            return isActive == 1;
                        }
                    }
                }

                // If not found in database but present in session (e.g. database cleared), re-register
                RegisterSession(userId);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error validating session: " + ex.Message);
                return true; // Fallback: allow request to proceed if database check fails
            }
        }

        /// <summary>
        /// Deactivates the user's current session GUID (on logout).
        /// </summary>
        public static void RemoveSession(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return;

            EnsureTableExists();

            if (HttpContext.Current.Session == null) return;

            string sessionGuid = HttpContext.Current.Session["UserSessionGuid"] as string;
            if (string.IsNullOrEmpty(sessionGuid)) return;

            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    string updateSql = "UPDATE UserActiveSessions SET IsActive = 0 WHERE SessionGuid = @SessionGuid";
                    using (SqlCommand cmd = new SqlCommand(updateSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@SessionGuid", sessionGuid);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error removing session: " + ex.Message);
            }
        }
    }
}
