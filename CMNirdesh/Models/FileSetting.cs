using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    public class FileSetting
    {

        public static SiteSettings GetDISFileSetting()
        {
            SiteSettings st = new SiteSettings();
            DataSet set = new DataSet();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
               
                set = SqlHelper.ExecuteDataset(connection, "GetDISFileSetting",null);

                foreach (DataRow dr in set.Tables[0].Rows)
                {
                    st.SettingValue = Convert.ToString(dr["SettingValue"]);
                    st.SettingName = Convert.ToString(dr["SettingName"]);
                }
             

            }
            catch (Exception)
            {
            }
            return st;

        }


        public static SiteSettings GetSecureFileSettingLocation()
        {
            SiteSettings st = new SiteSettings();
            DataSet set = new DataSet();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                set = SqlHelper.ExecuteDataset(connection, "GetSecureFileSettingLocation", null);

                foreach (DataRow dr in set.Tables[0].Rows)
                {
                    st.SettingValue = Convert.ToString(dr["SettingValue"]);
                    st.SettingName = Convert.ToString(dr["SettingName"]);
                }


            }
            catch (Exception)
            {
            }
            return st;

        }
    }
}