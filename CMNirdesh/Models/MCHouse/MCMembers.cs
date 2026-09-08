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
    [Table("mMembers")]
    public class MCMembers
    {
        [Key]
        public int MemberID { get; set; }
        public string Prefix { get; set; }
        public string Prefix_local { get; set; }

        public string Name { get; set; }

        public string Name_local { get; set; }
        public string CommitteeTypeId { get; set; }
        public string CommitteeTypeName { get; set; }

        public string PermanentAddress { get; set; }
        public string PermanentAddress_local { get; set; }
        public string Sex { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public string Designation { get; set; }

        public string Designation_local { get; set; }
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public DateTime ModifiedBy { get; set; }

        public bool Active { get; set; }
        public string Icon { get; set; }
        public int MemberCode { get; set; }
        public int MemberCodeOld { get; set; }
        public string Gender { get; set; }
        public string memberDOB { get; set; }
        public string Description { get; set; }
        public int MCCode { get; set; }
        public string MCName { get; set; }
        public int Ward { get; set; }
        public int Party { get; set; }
        public string OfficePhoneNo { get; set; }

        public string WardName { get; set;  }
        public int State { get; set; }
        public int District { get; set; }
        public string IsAlive { get; set; }
        public string committeeType { get; set; }
        public bool IsWardActive { get; set; }
        public bool IsHouseActive { get; set; }

        public static List<MCMembers> MembersListMaster(bool isSearched = false, int assemblyID = 0)
        {

            List<MCMembers> lstitem = new List<MCMembers>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@McID", CurrentSession.StateId);
                param[1] = new SqlParameter("@roleId", Convert.ToInt32(CurrentSession.RoleID));
                param[2] = new SqlParameter("@isSearched", isSearched);
                param[3] = new SqlParameter("@houseID", assemblyID);

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con,CommandType.StoredProcedure, "[MembersListMaster]", param);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MCMembers items = new MCMembers();
                    items.Name = Convert.ToString(dr["Name"]);
                    items.Name_local = Convert.ToString(dr["Name_local"]);
                    items.PermanentAddress = Convert.ToString(dr["PermanentAddress"]);
                    items.PermanentAddress_local = Convert.ToString(dr["PermanentAddress_local"]);
                    items.Mobile = Convert.ToString(dr["Mobile"]);
                    items.Icon = Convert.ToString(dr["FilePath"]) + Convert.ToString(dr["FileName"]);
                    items.FilePath = Convert.ToString(dr["FilePath"]);
                    items.FileName = Convert.ToString(dr["FileName"]);
                    items.Email = Convert.ToString(dr["Email"]);
                    items.Designation = Convert.ToString(dr["Designation"]);
                    items.Designation_local = Convert.ToString(dr["Designation_local"]);
                    items.MemberCode = Convert.ToInt32(dr["MemberCode"]);
                    items.MemberID = Convert.ToInt32(dr["MemberId"]);
                    items.MemberCodeOld = Convert.ToInt32(ds.Tables[1].Rows[0]["MemberCode"]);

                    items.Gender = Convert.ToString(dr["Sex"]);
                    items.memberDOB = Convert.ToString(dr["memdob"]);
                    items.Description = Convert.ToString(dr["Description"]);
                    items.WardName= Convert.ToString(dr["WardName"]);
                    items.MCCode = 0;
                    items.Ward = 0;
                    items.Party = 0;
                    items.State = 0;
                    items.District = 0;
                    items.Active = false;
                    items.IsWardActive = Convert.ToBoolean(dr["IsWardActive"]);
                    items.IsHouseActive = Convert.ToBoolean(dr["IsHouseActive"]);
                    if (dr["Active"] != System.DBNull.Value)
                    {
                        items.Active = Convert.ToBoolean(dr["Active"]);
                    }
                    if (dr["MCCode"] != System.DBNull.Value)
                    {
                        items.MCCode = Convert.ToInt32(dr["MCCode"]);
                    }
                    if (dr["WardId"] != System.DBNull.Value)
                    {
                        items.Ward = Convert.ToInt32(dr["WardId"]);
                    }
                    if (dr["PartyId"] != System.DBNull.Value)
                    {
                        items.Party = Convert.ToInt32(dr["PartyId"]);
                    }
                    items.OfficePhoneNo = Convert.ToString(dr["TelOffice"]);
                    if (dr["StateNameID"] != System.DBNull.Value)
                    {
                        items.State = Convert.ToInt32(dr["StateNameID"]);
                    }
                    if (dr["District"] != System.DBNull.Value)
                    {
                        items.District = Convert.ToInt32(dr["District"]);
                    }
                    items.IsAlive = Convert.ToString(dr["isAlive"]);

                    lstitem.Add(items);
                }
                return lstitem;
            }
            catch (Exception ex)
            {
                return lstitem;
            }

        }
        public static SelectList GetCommitteeTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "Get_CommitteeType");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = Convert.ToString(dr["CommitteeId"]);
                    item.Text = Convert.ToString(dr["CommitteeName"]);

                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch
            {
                return new SelectList(items, "Value", "Text");
            }

        }


        public static SelectList GetMemberDesignation(string lang = "E")
        {
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("Get_MemberDesignations", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@lang", lang);

                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                items.Add(new SelectListItem
                                {
                                    Value = dr["memDesigId"].ToString(),
                                    Text = dr["memDesigname"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // optional: log ex
            }

            // 🔑 Mandatory dropdown needs empty first item
            //items.Insert(0, new SelectListItem
            //{
            //    Text = "-- Select Designation --",
            //    Value = ""
            //});

            return new SelectList(items, "Value", "Text");
        }


        [HttpPost]
        public static int MemberRecordSave(MCMembers model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("MemberRecordSave", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Prefix", model.Prefix);
                cmd.Parameters.AddWithValue("@Prefix_local", model.Prefix_local);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Name_local", model.Name_local);
                cmd.Parameters.AddWithValue("@PermanentAddress", SqlDbType.VarChar).Value = (model.PermanentAddress == null) ? string.Empty : model.PermanentAddress;
                cmd.Parameters.AddWithValue("@PermanentAddress_local", SqlDbType.VarChar).Value = (model.PermanentAddress_local == null) ? string.Empty : model.PermanentAddress_local;
                cmd.Parameters.AddWithValue("@FileName", SqlDbType.VarChar).Value = (model.FileName == null) ? string.Empty : model.FileName;
                cmd.Parameters.AddWithValue("@FilePath", SqlDbType.VarChar).Value = (model.FilePath == null) ? string.Empty : model.FilePath;
                cmd.Parameters.AddWithValue("@Mobile", SqlDbType.VarChar).Value = (model.Mobile == null) ? string.Empty : model.Mobile;
                cmd.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = (model.Email == null) ? string.Empty : model.Email;
                cmd.Parameters.AddWithValue("@Designation", SqlDbType.VarChar).Value = (model.Designation == null) ? string.Empty : model.Designation;
                cmd.Parameters.AddWithValue("@Designation_local", SqlDbType.VarChar).Value = (model.Designation_local == null) ? string.Empty : model.Designation_local;
                cmd.Parameters.AddWithValue("@MemberCode", model.MemberCode);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);


                cmd.Parameters.AddWithValue("@Gender", SqlDbType.VarChar).Value = (model.Gender == null) ? string.Empty : model.Gender;
                cmd.Parameters.AddWithValue("@memberDOB", SqlDbType.VarChar).Value = (model.memberDOB == null) ? string.Empty : model.memberDOB;
                cmd.Parameters.AddWithValue("@Description", SqlDbType.VarChar).Value = (model.Description == null) ? string.Empty : model.Description;
                cmd.Parameters.AddWithValue("@MCCode", SqlDbType.VarChar).Value = model.MCCode;
                cmd.Parameters.AddWithValue("@Ward", SqlDbType.VarChar).Value = model.Ward;
                cmd.Parameters.AddWithValue("@Party", SqlDbType.VarChar).Value = model.Party;
                cmd.Parameters.AddWithValue("@OfficePhoneNo", SqlDbType.VarChar).Value = (model.OfficePhoneNo == null) ? string.Empty : model.OfficePhoneNo;
                cmd.Parameters.AddWithValue("@State", SqlDbType.VarChar).Value = model.State;
                cmd.Parameters.AddWithValue("@District", SqlDbType.VarChar).Value = model.District;
                cmd.Parameters.AddWithValue("@IsAlive", SqlDbType.VarChar).Value = (model.IsAlive == null) ? string.Empty : model.IsAlive;
                cmd.Parameters.AddWithValue("@committeeType", SqlDbType.VarChar).Value = (model.committeeType == null) ? string.Empty : model.committeeType.Trim();


                cmd.ExecuteNonQuery();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        [HttpGet]
        public static List<MCMembers> MemberRecordGetById(int id)
        {
            List<MCMembers> lstItem = new List<MCMembers>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@MemberID", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "MemberRecordGetById", param).Tables[0].Rows)
                {
                    MCMembers item = new MCMembers();
                    {
                        item.MemberID = Convert.ToInt32(dr["MemberID"]);
                        item.Name = Convert.ToString(dr["Name"]);
                        item.Name_local = Convert.ToString(dr["Name_local"]);
                        item.PermanentAddress = Convert.ToString(dr["PermanentAddress"]);
                        item.PermanentAddress_local = Convert.ToString(dr["PermanentAddress_local"]);
                        item.Icon =  BlobStorage.GetStorageAcessingPath()+"/"+Convert.ToString(dr["FilePath"]) + Convert.ToString(dr["FileName"]);
                        item.FileName = Convert.ToString(dr["FileName"]);
                        item.FilePath = Convert.ToString(dr["FilePath"]);
                        item.Mobile = Convert.ToString(dr["Mobile"]);
                        item.Email = Convert.ToString(dr["Email"]);
                        item.Designation = Convert.ToString(dr["Designation"]);
                        item.Designation_local = Convert.ToString(dr["Designation_local"]);
                        item.MemberCode = Convert.ToInt32(dr["MemberCode"]);
                        item.Prefix = Convert.ToString(dr["Prefix"]);
                        item.Prefix_local = Convert.ToString(dr["Prefix_local"]);
                        item.WardName=Convert.ToString(dr["ConstituencyName"]);
                        item.Gender = Convert.ToString(dr["Sex"]);
                        item.Description = Convert.ToString(dr["Description"]);
                        item.memberDOB = Convert.ToString(dr["memberdob"]);
                        item.MCCode = 0;
                        item.Ward = 0;
                        item.Party = 0;
                        item.State = 0;
                        item.District = 0;
                        if (dr["MCCode"] != System.DBNull.Value)
                        {
                            item.MCCode = Convert.ToInt32(dr["MCCode"]);
                        }
                        if (dr["WardId"] != System.DBNull.Value)
                        {
                            item.Ward = Convert.ToInt32(dr["WardId"]);
                        }
                        if (dr["PartyId"] != System.DBNull.Value)
                        {
                            item.Party = Convert.ToInt32(dr["PartyId"]);
                        }
                        item.OfficePhoneNo = Convert.ToString(dr["TelOffice"]);
                        if (dr["StateNameID"] != System.DBNull.Value)
                        {
                            item.State = Convert.ToInt32(dr["StateNameID"]);
                        }
                        if (dr["District"] != System.DBNull.Value)
                        {
                            item.District = Convert.ToInt32(dr["District"]);
                        }
                        item.IsAlive = Convert.ToString(dr["isAlive"]);
                        item.committeeType = Convert.ToString(dr["commiteetype"]);

                    };
                    lstItem.Add(item);
                }

                return lstItem;
            }
            catch (Exception ex)
            {
                return lstItem;

            }
        }

        [HttpPost]
        public static int MemberRecordUpdate(MCMembers model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("MemberRecordUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Prefix", model.Prefix);
                cmd.Parameters.AddWithValue("@Prefix_local", model.Prefix_local);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Name_local", model.Name_local);
                cmd.Parameters.AddWithValue("@PermanentAddress", SqlDbType.VarChar).Value = (model.PermanentAddress == null) ? string.Empty : model.PermanentAddress;
                cmd.Parameters.AddWithValue("@PermanentAddress_local", SqlDbType.VarChar).Value = (model.PermanentAddress_local == null) ? string.Empty : model.PermanentAddress_local;
                cmd.Parameters.AddWithValue("@FileName", SqlDbType.VarChar).Value = (model.FileName == null) ? string.Empty : model.FileName;
                cmd.Parameters.AddWithValue("@FilePath", SqlDbType.VarChar).Value = (model.FilePath == null) ? string.Empty : model.FilePath;
                cmd.Parameters.AddWithValue("@Mobile", SqlDbType.VarChar).Value = (model.Mobile == null) ? string.Empty : model.Mobile;
                cmd.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = (model.Email == null) ? string.Empty : model.Email;
                cmd.Parameters.AddWithValue("@Designation", SqlDbType.VarChar).Value = (model.Designation == null) ? string.Empty : model.Designation;
                cmd.Parameters.AddWithValue("@Designation_local", SqlDbType.VarChar).Value = (model.Designation_local == null) ? string.Empty : model.Designation_local;
                cmd.Parameters.AddWithValue("@MemberCode", model.MemberCode);
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@MemberID", model.MemberID);
                cmd.Parameters.AddWithValue("@Gender", SqlDbType.VarChar).Value = (model.Gender == null) ? string.Empty : model.Gender;
                cmd.Parameters.AddWithValue("@memberDOB", SqlDbType.VarChar).Value = (model.memberDOB == null) ? string.Empty : model.memberDOB;
                cmd.Parameters.AddWithValue("@Description", SqlDbType.VarChar).Value = (model.Description == null) ? string.Empty : model.Description;
                cmd.Parameters.AddWithValue("@MCCode", SqlDbType.VarChar).Value = model.MCCode;
                cmd.Parameters.AddWithValue("@Ward", SqlDbType.VarChar).Value = model.Ward;
                cmd.Parameters.AddWithValue("@Party", SqlDbType.VarChar).Value = model.Party;
                cmd.Parameters.AddWithValue("@OfficePhoneNo", SqlDbType.VarChar).Value = (model.OfficePhoneNo == null) ? string.Empty : model.OfficePhoneNo;
                cmd.Parameters.AddWithValue("@State", SqlDbType.VarChar).Value = model.State;
                cmd.Parameters.AddWithValue("@District", SqlDbType.VarChar).Value = model.District;
                cmd.Parameters.AddWithValue("@IsAlive", SqlDbType.VarChar).Value = (model.IsAlive == null) ? string.Empty : model.IsAlive.Trim();
                cmd.Parameters.AddWithValue("@committeeType", SqlDbType.VarChar).Value = (model.committeeType == null) ? string.Empty : model.committeeType.Trim();

                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int MemberRecordDeleteById(int Id)
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
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[MemberRecordDeleteById]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

        public static List<HouseList> FetchHouses()
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@McID", CurrentSession.StateId);

                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "FETCH_HOUSE_LIST_ALL", param);
                con.Close();

                List<HouseList> houses = new List<HouseList>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    HouseList items = new HouseList()
                    {
                        AssemblyID = Convert.ToInt32(dr["AssemblyID"]),
                        AssemblyName = Convert.ToString(dr["AssemblyName"]),
                        AssemblyNameLocal = Convert.ToString(dr["AssemblyNameLocal"]),
                    };
                    houses.Add(items);
                }
                return houses;
            }
            catch (Exception)
            {
                return new List<HouseList>();
            }
        }
    }

    public class FNCCMember {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string Designation { get; set; }
        public int IsPresent { get; set; }
        public string Status { get; set; }

    }
        public class LstMembers
        {
        public List<MCMembers> _LstMembers { get; set; }
        public SelectList _LstDesignation { get; set; }
        public SelectList _LstDesignationlocal { get; set; }
        public List<FNCCMember> _LstFNCCMembers { get; set; }
        public SelectList _CommitteeType { get; set; }
        public int MemberCodeOld { get; set; }

        public string fileacessingUrl { get; set; }
        public List<HouseList> Houses { get; set; }
        }

    public class HouseList
    {
        public string AssemblyName { get; set; }
        public int AssemblyID { get; set; }
        public string AssemblyNameLocal { get; set; }
    }

}