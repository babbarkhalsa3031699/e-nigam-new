using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models.MCHouse
{
    [Serializable]
    [Table("mMCMaster")]
    public class mCouncilCorporation
    {
        [Key]
        public int Id { get; set; }
        public int IsMC { get; set; }
        public string unitname { get; set; }

        public string AboutMC { get; set; }
        public string AboutMCLocal { get; set; }
        public string Address { get; set; }
        public string AddressLocal { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string MCName { get; set; }

        public string MCName_local { get; set; }

        public string MClogo { get; set; }

        public string HeadPhoto { get; set; }

        public string HeadName { get; set; }

        public string HeadNameLocal { get; set; }

        public string HeadDesignation { get; set; }

        public string HeadDesignationLocal { get; set; }

        public string HeadMessage { get; set; }
        public string HeadMessageLocal { get; set; }


        public string SubHeadPhoto { get; set; }

        public string SubHeadName { get; set; }
        public string SubHeadNameLocal { get; set; }

        public string SubHeadDesignation { get; set; }
        public string SubHeadDesignationLocal { get; set; }

        public string SubHeadMessage { get; set; }
        public string SubHeadMessageLocal { get; set; }
        public bool IsDeleted { get; set; }

        public static List<mCouncilCorporation> GetMCModelList()
        {

            List<mCouncilCorporation> lstitem = new List<mCouncilCorporation>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@Id", CurrentSession.StateId);
                param[1] = new SqlParameter("@roleId", Convert.ToInt32(CurrentSession.RoleID));
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mMCListIndex",param);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mCouncilCorporation items = new mCouncilCorporation();
                    items.unitname = Convert.ToString(dr["unitName"]);
                    items.MClogo = "MC/" + dr["MCName"] + "/" + Convert.ToString(dr["MClogo"]);
                    items.MCName = Convert.ToString(dr["MCName"]);
                    items.AboutMC = Convert.ToString(dr["AboutMC"]);
                    items.AboutMCLocal = Convert.ToString(dr["AboutMC_local"]);
                    items.Address = Convert.ToString(dr["Address"]);
                    items.AddressLocal = Convert.ToString(dr["Address_local"]);
                    items.Email = Convert.ToString(dr["Email"]);
                    items.Phone = Convert.ToString(dr["PhoneNo"]);
                    items.MCName_local = Convert.ToString(dr["MCName_local"]);
                    items.HeadName = Convert.ToString(dr["HeadName"]);
                    items.HeadNameLocal = Convert.ToString(dr["HeadName_local"]);
                    items.HeadMessage = Convert.ToString(dr["HeadMessage"]);
                    items.HeadMessageLocal = Convert.ToString(dr["HeadMessage_local"]);
                    items.HeadDesignation = Convert.ToString(dr["HeadDesignation"]);
                    items.HeadDesignationLocal = Convert.ToString(dr["HeadDesignation_local"]);
                    items.HeadPhoto = "MC/" + dr["MCName"] + "/" + Convert.ToString(dr["HeadPhoto"]);
                    items.SubHeadName = Convert.ToString(dr["SubHeadName"]);
                    items.SubHeadNameLocal = Convert.ToString(dr["SubHeadName_local"]);
                    items.SubHeadPhoto = "MC/" + dr["MCName"] + "/" + Convert.ToString(dr["SubHeadPhoto"]);
                    items.SubHeadDesignation = Convert.ToString(dr["SubHeadDesignation"]);
                    items.SubHeadDesignationLocal = Convert.ToString(dr["SubHeadDesignation_local"]);
                    items.SubHeadMessage = Convert.ToString(dr["SubHeadMessage"]);
                    items.SubHeadMessageLocal = Convert.ToString(dr["SubHeadMessage_local"]);
                    items.Id = Convert.ToInt32(dr["Id"]);
                    items.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    lstitem.Add(items);
                }
                return lstitem;
            }
            catch
            {
                return lstitem;
            }

        }
        public static int SaveMCRecord(mCouncilCorporation model, string folderpath)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mcMenuRecordInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MClogo", SqlDbType.VarChar).Value = (model.MClogo == null) ? string.Empty : folderpath + "/" + model.MClogo;
                cmd.Parameters.AddWithValue("@MCName", model.MCName);
                cmd.Parameters.AddWithValue("@IsMC", model.IsMC);
                cmd.Parameters.AddWithValue("@MCName_local", model.MCName_local);
                cmd.Parameters.AddWithValue("@HeadName", SqlDbType.VarChar).Value = (model.HeadName == null) ? string.Empty : model.HeadName;
                cmd.Parameters.AddWithValue("@HeadMessage", SqlDbType.VarChar).Value = (model.HeadMessage == null) ? string.Empty : model.HeadMessage;
                cmd.Parameters.AddWithValue("@HeadDesignation", SqlDbType.VarChar).Value = (model.HeadDesignation == null) ? string.Empty : model.HeadDesignation;
                cmd.Parameters.AddWithValue("@HeadPhoto", SqlDbType.VarChar).Value = (model.HeadPhoto == null) ? string.Empty : folderpath + "/" + model.HeadPhoto;
                cmd.Parameters.AddWithValue("@SubHeadName", SqlDbType.VarChar).Value = (model.SubHeadName == null) ? string.Empty : model.SubHeadName;
                cmd.Parameters.AddWithValue("@SubHeadPhoto", SqlDbType.VarChar).Value = (model.SubHeadPhoto == null) ? string.Empty : folderpath + "/" + model.SubHeadPhoto;
                cmd.Parameters.AddWithValue("@SubHeadDesignation", SqlDbType.VarChar).Value = (model.SubHeadDesignation == null) ? string.Empty : model.SubHeadDesignation;
                cmd.Parameters.AddWithValue("@SubHeadMessage", SqlDbType.VarChar).Value = (model.SubHeadMessage == null) ? string.Empty : model.SubHeadMessage;
                cmd.Parameters.AddWithValue("@HeadNameLocal", SqlDbType.VarChar).Value = (model.HeadNameLocal == null) ? string.Empty : model.HeadNameLocal;
                cmd.Parameters.AddWithValue("@HeadMessageLocal", SqlDbType.VarChar).Value = (model.HeadMessageLocal == null) ? string.Empty : model.HeadMessageLocal;
                cmd.Parameters.AddWithValue("@HeadDesignationLocal", SqlDbType.VarChar).Value = (model.HeadDesignationLocal == null) ? string.Empty : model.HeadDesignationLocal;
                cmd.Parameters.AddWithValue("@SubHeadNameLocal", SqlDbType.VarChar).Value = (model.SubHeadNameLocal == null) ? string.Empty : model.SubHeadNameLocal;
                cmd.Parameters.AddWithValue("@SubHeadDesignationLocal", SqlDbType.VarChar).Value = (model.SubHeadDesignationLocal == null) ? string.Empty : model.SubHeadDesignationLocal;
                cmd.Parameters.AddWithValue("@SubHeadMessageLocal", SqlDbType.VarChar).Value = (model.SubHeadMessageLocal == null) ? string.Empty : model.SubHeadMessageLocal;
                cmd.Parameters.AddWithValue("@AboutMC", SqlDbType.VarChar).Value = (model.AboutMC == null) ? string.Empty : model.AboutMC;
                cmd.Parameters.AddWithValue("@AboutMC_local", SqlDbType.VarChar).Value = (model.AboutMCLocal == null) ? string.Empty : model.AboutMCLocal;
                cmd.Parameters.AddWithValue("@Address", SqlDbType.VarChar).Value = (model.Address == null) ? string.Empty : model.Address;
                cmd.Parameters.AddWithValue("@Address_local", SqlDbType.VarChar).Value = (model.AddressLocal == null) ? string.Empty : model.AddressLocal;
                cmd.Parameters.AddWithValue("@PhoneNo", SqlDbType.VarChar).Value = (model.Phone == null) ? string.Empty : model.Phone;
                cmd.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = (model.Email == null) ? string.Empty : model.Email;
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



        public static List<mCouncilCorporation> GetMCRecordById(int id)
        {
            List<mCouncilCorporation> lstdept = new List<mCouncilCorporation>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@Id", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mMCGetID", param).Tables[0].Rows)
                {
                    mCouncilCorporation item = new mCouncilCorporation();
                    {

                        item.MClogo = BlobStorage.GetStorageAcessingPath() + "/MC/" + dr["MCName"] + "/"+Convert.ToString(dr["MClogo"]);
                        item.MCName = Convert.ToString(dr["MCName"]);
                        item.IsMC = Convert.ToInt32(dr["IsMC"]);
                        item.MCName_local = Convert.ToString(dr["MCName_local"]);
                        item.AboutMC = Convert.ToString(dr["AboutMC"]);

                        item.AboutMCLocal = Convert.ToString(dr["AboutMC_local"]);
                        item.Address = Convert.ToString(dr["Address"]);
                        item.AddressLocal = Convert.ToString(dr["Address_local"]);
                        item.Email = Convert.ToString(dr["Email"]);
                        item.Phone = Convert.ToString(dr["PhoneNo"]);

                        item.HeadName = Convert.ToString(dr["HeadName"]);
                        item.HeadMessage = Convert.ToString(dr["HeadMessage"]);
                        item.HeadDesignation = Convert.ToString(dr["HeadDesignation"]);
                        item.HeadPhoto = BlobStorage.GetStorageAcessingPath() + "/MC/" + dr["MCName"] + "/" + Convert.ToString(dr["HeadPhoto"]);
                        item.SubHeadName = Convert.ToString(dr["SubHeadName"]);
                        item.SubHeadMessage = Convert.ToString(dr["SubHeadMessage"]);
                        item.SubHeadDesignation = Convert.ToString(dr["SubHeadDesignation"]);
                        item.SubHeadPhoto = BlobStorage.GetStorageAcessingPath() + "/MC/" + dr["MCName"] + "/" + Convert.ToString(dr["SubHeadPhoto"]);
                        item.HeadNameLocal = Convert.ToString(dr["HeadName_local"]);
                        item.HeadMessageLocal = Convert.ToString(dr["HeadMessage_local"]);
                        item.HeadDesignationLocal = Convert.ToString(dr["HeadDesignation_local"]);
                        item.SubHeadNameLocal = Convert.ToString(dr["SubHeadName_local"]);
                        item.SubHeadMessageLocal = Convert.ToString(dr["SubHeadMessage_local"]);
                        item.SubHeadDesignationLocal = Convert.ToString(dr["SubHeadDesignation_local"]);
                        item.Id = Convert.ToInt32(dr["Id"]);



                    };
                    lstdept.Add(item);
                }

                return lstdept;
            }
            catch (Exception ex)
            {
                return lstdept;

            }
        }
        public static int UpdateMCRecordById(mCouncilCorporation model,string folderpath)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mMCUpdateRecordById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@MClogo", SqlDbType.VarChar).Value = (model.MClogo == null) ? string.Empty :  model.MClogo;
                cmd.Parameters.AddWithValue("@MCName", model.MCName);
                cmd.Parameters.AddWithValue("@MCName_local", model.MCName_local);
                cmd.Parameters.AddWithValue("@HeadName", SqlDbType.VarChar).Value = (model.HeadName == null) ? string.Empty : model.HeadName;
                cmd.Parameters.AddWithValue("@HeadMessage", SqlDbType.VarChar).Value = (model.HeadMessage == null) ? string.Empty : model.HeadMessage;
                cmd.Parameters.AddWithValue("@HeadDesignation", SqlDbType.VarChar).Value = (model.HeadDesignation == null) ? string.Empty : model.HeadDesignation;
                cmd.Parameters.AddWithValue("@HeadPhoto", SqlDbType.VarChar).Value = (model.HeadPhoto == null) ? string.Empty :  model.HeadPhoto;
                cmd.Parameters.AddWithValue("@SubHeadName", SqlDbType.VarChar).Value = (model.SubHeadName == null) ? string.Empty : model.SubHeadName;
                cmd.Parameters.AddWithValue("@SubHeadPhoto", SqlDbType.VarChar).Value = (model.SubHeadPhoto == null) ? string.Empty :  model.SubHeadPhoto;
                cmd.Parameters.AddWithValue("@SubHeadDesignation", SqlDbType.VarChar).Value = (model.SubHeadDesignation == null) ? string.Empty : model.SubHeadDesignation;
                cmd.Parameters.AddWithValue("@SubHeadMessage", SqlDbType.VarChar).Value = (model.SubHeadMessage == null) ? string.Empty : model.SubHeadMessage;
                cmd.Parameters.AddWithValue("@HeadNameLocal", SqlDbType.VarChar).Value = (model.HeadNameLocal == null) ? string.Empty : model.HeadNameLocal;
                cmd.Parameters.AddWithValue("@HeadMessageLocal", SqlDbType.VarChar).Value = (model.HeadMessageLocal == null) ? string.Empty : model.HeadMessageLocal;
                cmd.Parameters.AddWithValue("@HeadDesignationLocal", SqlDbType.VarChar).Value = (model.HeadDesignationLocal == null) ? string.Empty : model.HeadDesignationLocal;
                cmd.Parameters.AddWithValue("@SubHeadNameLocal", SqlDbType.VarChar).Value = (model.SubHeadNameLocal == null) ? string.Empty : model.SubHeadNameLocal;
                cmd.Parameters.AddWithValue("@SubHeadDesignationLocal", SqlDbType.VarChar).Value = (model.SubHeadDesignationLocal == null) ? string.Empty : model.SubHeadDesignationLocal;
                cmd.Parameters.AddWithValue("@SubHeadMessageLocal", SqlDbType.VarChar).Value = (model.SubHeadMessageLocal == null) ? string.Empty : model.SubHeadMessageLocal;

                cmd.Parameters.AddWithValue("@AboutMC", SqlDbType.VarChar).Value = (model.AboutMC == null) ? string.Empty : model.AboutMC;
                cmd.Parameters.AddWithValue("@AboutMC_local", SqlDbType.VarChar).Value = (model.AboutMCLocal == null) ? string.Empty : model.AboutMCLocal;
                cmd.Parameters.AddWithValue("@Address", SqlDbType.VarChar).Value = (model.Address == null) ? string.Empty : model.Address;
                cmd.Parameters.AddWithValue("@Address_local", SqlDbType.VarChar).Value = (model.AddressLocal == null) ? string.Empty : model.AddressLocal;
                cmd.Parameters.AddWithValue("@PhoneNo", SqlDbType.VarChar).Value = (model.Phone == null) ? string.Empty : model.Phone;
                cmd.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = (model.Email == null) ? string.Empty : model.Email;

                cmd.Parameters.AddWithValue("@IsMC", model.IsMC);

                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    
        public static int DeleteMCRecordById(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@Id", Id),


                };
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mMCMasterDeleteRowById]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

        public class LstMCModel
        {
            public List<mCouncilCorporation> _ListMCModel { get; set; }
            public string fileacessingUrl { get; set; }
        }
    }
}