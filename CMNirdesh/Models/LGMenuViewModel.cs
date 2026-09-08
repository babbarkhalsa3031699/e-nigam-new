using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class LGMenuViewModel
    {
        public int MenuId { get; set; }
        public string MenuTitle { get; set; }
        public string MenuType { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ExternalUrl { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class LGMenuDataViewModel
    {
        public static List<LGMenuViewModel> GetAllMenus()
        {
            var menus = new List<LGMenuViewModel>();

            using (SqlConnection conn = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("spLGMenu_ListAll", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menus.Add(new LGMenuViewModel
                        {
                            MenuId = (int)reader["MenuId"],
                            MenuTitle = reader["MenuTitle"].ToString(),
                            MenuType = reader["MenuType"].ToString(),
                            ControllerName = reader["ControllerName"].ToString(),
                            ActionName = reader["ActionName"].ToString(),
                            ExternalUrl = reader["ExternalUrl"].ToString(),
                            Url = reader["Url"].ToString(),
                            IsActive = (bool)reader["IsActive"],
                            CreatedDate = (DateTime)reader["CreatedDate"]
                        });
                    }
                }
            }

            return menus;
        }

        public static bool InsertMenu(LGMenuViewModel model)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection conn = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("spLGMenu_Insert", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.AddWithValue("@MenuTitle", model.MenuTitle);
                cmd.Parameters.AddWithValue("@MenuType", model.MenuType);
                cmd.Parameters.AddWithValue("@ControllerName", (object)model.ControllerName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ActionName", (object)model.ActionName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ExternalUrl", (object)model.ExternalUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", true); // default active

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    // You can log the error here
                    return false;
                }
            }
        }

        public static bool UpdateMenu(LGMenuViewModel model)
        {
            using (SqlConnection conn = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("spLGMenu_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MenuId", model.MenuId);
                cmd.Parameters.AddWithValue("@MenuTitle", model.MenuTitle);
                cmd.Parameters.AddWithValue("@MenuType", model.MenuType);
                cmd.Parameters.AddWithValue("@ControllerName", (object)model.ControllerName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ActionName", (object)model.ActionName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ExternalUrl", (object)model.ExternalUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", true); 

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static bool DeleteMenu(int menuId)
        {
            using (SqlConnection conn = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("spLGMenu_Delete", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MenuId", menuId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

    }
    public class LGMenuDataList
    {
        public List<LGMenuViewModel> _LstLGMenu{ get; set; }
        public string fileacessingUrl { get; set; }

    }



}