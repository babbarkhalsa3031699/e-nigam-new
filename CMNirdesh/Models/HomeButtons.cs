using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models
{
    [Serializable]
    [Table("HomeButtons")]
    public class HomeButtons
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Url { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public static List<HomeButtons> GetButtonsList()
        {
            List<HomeButtons> lsthome = new List<HomeButtons>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "PopulateButtons");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HomeButtons item = new HomeButtons();
                    item.Id = Convert.ToInt32(dr["Id"]);
                    item.Name = Convert.ToString(dr["Name"]);
                    item.Url = Convert.ToString(dr["Url"]);
                    item.ImageUrl = Convert.ToString(dr["ImageUrl"]);
                    lsthome.Add(item);
                }
                return lsthome;
            }
            catch
            {
                return lsthome;

            }
        }

    }
    [Serializable]
    [Table("HomeButtonsGreenBar")]
    public class HomeButtons2
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        public string ImageUrl { get; set; }

        public static List<HomeButtons2> GetButtonsList2()
        {
            List<HomeButtons2> lsthome = new List<HomeButtons2>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "PopulateButtons2");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HomeButtons2 item = new HomeButtons2();
                    item.Id = Convert.ToInt32(dr["Id"]);
                    item.Name = Convert.ToString(dr["name"]);
                    item.Url = Convert.ToString(dr["url"]);
                    item.ImageUrl = Convert.ToString(dr["imageurl"]);
                    item.Desc1 = Convert.ToString(dr["desc1"]);
                    item.Desc2 = Convert.ToString(dr["desc2"]);
                    item.Desc3 = Convert.ToString(dr["desc3"]);
                    lsthome.Add(item);
                }
                return lsthome;
            }
            catch
            {
                return lsthome;

            }
        }
    }
    public class LstHomeModel
    {
        public List<HomeButtons> ListHomeModel { get; set; }
        public List<HomeButtons2> ListHomeModel2 { get; set; }
    }
}



