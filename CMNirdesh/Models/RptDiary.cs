using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;


namespace CMNirdesh.Models
{
    public class RptDiaryList : System.Collections.ObjectModel.ObservableCollection<RptDiary> { }
    public class RptDiary
    {
        public string deptname { get; set; }
        public string Sanctioned { get; set; }
        public string NotImplemented { get; set; }
        public string UnderImplementation { get; set; }
        public int Total { get; set; }

        public static RptDiaryList GetUserWiseStatusDetails(string UserID, string Type)
        {
            RptDiaryList list = new RptDiaryList();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@UserId", UserID);
                param[1] = new SqlParameter("@Type", Type);

                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[RptDiaryData]", param).Tables[0].Rows)
                {
                    RptDiary item = new RptDiary
                    {
                        deptname = String.IsNullOrEmpty(Convert.ToString(row["deptname"])) ? "0" : Convert.ToString(row["deptname"]),
                        Sanctioned = String.IsNullOrEmpty(Convert.ToString(row["Sanctioned"])) ? "0" : Convert.ToString(row["Sanctioned"]),
                        UnderImplementation = String.IsNullOrEmpty(Convert.ToString(row["UnderImplementation"])) ? "0" : Convert.ToString(row["UnderImplementation"]),
                        NotImplemented = String.IsNullOrEmpty(Convert.ToString(row["NotImplemented"])) ? "0" : Convert.ToString(row["NotImplemented"]),
                       

                    };
                    item.Total = Convert.ToInt32(item.Sanctioned) + Convert.ToInt32(item.NotImplemented) + Convert.ToInt32(item.UnderImplementation);
                    list.Add(item);
                }
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }
    }
}
