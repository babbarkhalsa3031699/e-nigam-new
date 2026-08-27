using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace PVSWebSite.Utility
{
    public class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["pvsDatabase"].ConnectionString);
            return DBSQLServerUtils.GetDBConnection(connection.DataSource, connection.Database, connection.Credential.UserId, connection.Credential.Password.ToString());
        }
        private static void CreateCommand(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}