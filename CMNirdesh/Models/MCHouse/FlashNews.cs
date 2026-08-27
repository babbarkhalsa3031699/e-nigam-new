using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMNirdesh.Models.MCHouse
{
    [Serializable]
    [Table("mHomeFlashNews")]
    public class FlashNews
    {
        [Key]
        public string FlashNewsText { get; set; }

        public string FlashNewsText_local { get; set; }

        public string FlashNewsFile { get; set; }

        public string FlashNewsFileLink { get; set; }

        public string ActiveNews { get; set; }

        public int FlashNewsID { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public int MCId { get; set; }

        public int FlashNewsOrder { get; set; }

        public string MCName { get; set; }
        public bool Status { get; set; }


        public static List<FlashNews> GetFlashNewsListbyMCId()
        {

            List<FlashNews> lstitems = new List<FlashNews>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@MCId", CurrentSession.StateId);
                param[1] = new SqlParameter("@roleId", Convert.ToInt32(CurrentSession.RoleID));
                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "FlashNewsListbyMCId", param).Tables[0].Rows)
                {
                    FlashNews items = new FlashNews();
                    items.FlashNewsID = Convert.ToInt32(dr["FlashNewsID"]);
                    items.FlashNewsText = Convert.ToString(dr["FlashNewsText"]);
                    items.FlashNewsText_local = Convert.ToString(dr["FlashNewsText_local"]);
                    items.FlashNewsFile = Convert.ToString(dr["FlashNewsFile"]);
                    items.FlashNewsFileLink = Convert.ToString(dr["FlashNewsFileLink"]);
                    items.ActiveNews = Convert.ToString(dr["ActiveNews"]);
                    //items.FlashNewsOrder = Convert.ToInt32(dr["FlashNewsOrder"]);
                    items.MCName = Convert.ToString(dr["MCName"]);
                    items.Status = Convert.ToBoolean(dr["IsActive"]);
                
                    lstitems.Add(items);
                }
                return lstitems;
            }
            catch
            {
                return lstitems;
            }

        }
        public static int SaveFlashNewsRecord(FlashNews model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("FlashNewsInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FlashNewsText", SqlDbType.VarChar).Value = (model.FlashNewsText == null) ? string.Empty : model.FlashNewsText;
                cmd.Parameters.AddWithValue("@FlashNewsText_local", SqlDbType.VarChar).Value = (model.FlashNewsText_local == null) ? string.Empty : model.FlashNewsText_local;
                cmd.Parameters.AddWithValue("@FlashNewsFile", SqlDbType.VarChar).Value = (model.FlashNewsFile == null) ? string.Empty : model.FlashNewsFile;
                cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.VarChar).Value = (CurrentSession.UserID == null) ? string.Empty : CurrentSession.UserID;
                cmd.Parameters.AddWithValue("@MCId", model.MCId);
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static List<FlashNews> GetFlashNewsRecordById(int id)
        {
            List<FlashNews> lstItems = new List<FlashNews>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);
                

                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetFlashNewsDetailbyID", param).Tables[0].Rows)
                {
                    FlashNews item = new FlashNews();
                    {
                        item.FlashNewsText = Convert.ToString(dr["FlashNewsText"]);
                        item.FlashNewsText_local = Convert.ToString(dr["FlashNewsText_local"]);
                       // item.FlashNewsOrder = Convert.ToInt32(dr["FlashNewsOrder"]);
                        item.MCId = Convert.ToInt32(dr["MCId"]);
                        item.FlashNewsID = Convert.ToInt32(dr["FlashNewsID"]);

                    };
                    lstItems.Add(item);
                    con.Close();
                }
                return lstItems;
                
            }
            catch (Exception ex)
            {
                return lstItems;
                

            }
            
        }
        public static int UpdateFlashNewsRecord(FlashNews model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("FlashNewsUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MCId", model.MCId);
                cmd.Parameters.AddWithValue("@FlashNewsID", model.FlashNewsID);
                cmd.Parameters.AddWithValue("@FlashNewsText", SqlDbType.VarChar).Value = (model.FlashNewsText == null) ? string.Empty : model.FlashNewsText;
                cmd.Parameters.AddWithValue("@FlashNewsText_local", SqlDbType.VarChar).Value = (model.FlashNewsText_local == null) ? string.Empty : model.FlashNewsText_local;
                cmd.Parameters.AddWithValue("@FlashNewsFile", SqlDbType.VarChar).Value = (model.FlashNewsFile == null) ? string.Empty : model.FlashNewsFile;
                cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.VarChar).Value = (CurrentSession.UserID == null) ? string.Empty : CurrentSession.UserID;
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int DeleteFlashNewsRecordById(int Id)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", Id);
                
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "DeleteFlashNewsById", param);
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public class LstFlashNewsModel
        {
            public List<FlashNews> _LstFlashNews { get; set; }
            public string fileacessingUrl { get; set; }
        }
    }
}