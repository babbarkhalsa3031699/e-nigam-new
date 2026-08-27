using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace SBL.eLegistrator.HouseController.Web.Utility
{
    public class DataUtility
    {
        public DataUtility()
        {
            sqlcon = new SqlConnection();
            sqlcon.ConnectionString = getConnectionString();
            sqlcon.Open();

        }
        public static string getConnectionString()
        {
            string txtpath = System.Web.HttpContext.Current.Server.MapPath("/ConnectionFile/connectionString.txt");
            try
            {
                if (File.Exists(txtpath))
                {
                    //using (StreamReader sr = new StreamReader(txtpath))
                    //{
                    StreamReader sr = new StreamReader(txtpath);
                        while (sr.Peek() >= 0)
                        {
                            string ss = sr.ReadLine();
                            string[] txtsplit = ss.Split(';');
                            string server = txtsplit[0].ToString();
                            string userid = txtsplit[1].ToString();
                            string password = txtsplit[2].ToString();
                            StringBuilder str = new StringBuilder();
                           SqlConnectionStringBuilder DBconnbuilder = new SqlConnectionStringBuilder("Data Source = "+ server + "; Initial Catalog= Email; User ID ="+ userid + " ; Password = " + password + "; ");
                            //str.Append("Server =").Append(server).Append("; Database = ").Append("Email").Append("; User Id = ").Append(userid).Append("; Password = ").Append(password);
                            return DBconnbuilder.ToString();
                            break;
                        }

                    //}
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }
            return string.Empty;

        }
        # region All Module level variables
        SqlDataAdapter sqlda;
        SqlConnection sqlcon;
        DataSet ds;
        SqlDataReader sqldr;
        SqlTransaction sqltransaction;
        SqlCommand sqlcmd;

        private string query;

        # endregion
    }
}