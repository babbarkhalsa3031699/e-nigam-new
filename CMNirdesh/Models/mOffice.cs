using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMNirdesh.Models
{
    [Table("mOffice")]
    [Serializable]
    public class mOffice
    {
        [Key]
        public int OfficeId { get; set; }
        public int? officelevel { get; set; }
        public string officename { get; set; }
        public string officeaddr { get; set; }
        public int? distcd { get; set; }
        public string urbrul { get; set; }
        public string tehsilcode { get; set; }
        public string blockcd { get; set; }
        public string panchcd { get; set; }
        public string concd { get; set; }
        public string desigcode { get; set; }
        public string reportoffice { get; set; }
        public string EmailId { get; set; }
        public string Wh_SSt { get; set; }
        public string deptid { get; set; }
        public string officenamelocal { get; set; }
        public string officeaddresslocal { get; set; }
        public string NodalOfficerEmpCode { get; set; }
        public string grcellincharge { get; set; }
        public string IsOnline { get; set; }
        public string isgrcell { get; set; }
        public string grcellinchargedeptid { get; set; }
        public string DEuserid { get; set; }
        public string DEdate { get; set; }
        public string officephone { get; set; }
        public string GenOffice { get; set; }
        public string mapoldofficeid { get; set; }
        public string mapnewofficeid { get; set; }
        public string officetypecd { get; set; }
        public string tribalAreaCode { get; set; }
        public string ddodemandcd { get; set; }
        public string treasurycd { get; set; }
        public string mobilenumber { get; set; }
        public bool IsMapped { get; set; }
        public Int64? OfficeCode { get; set; }
        public bool? IsNodalOffice { get; set; }
        public bool? IsDeleted { get; set; }

        public int IsApex { get; set; }
        public string deptName { get; set; }

        public static List<mOffice> GetOfficeList()
        {

            List<mOffice> lstdept = new List<mOffice>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@DeptId", CurrentSession.DeptID);
                param[1] = new SqlParameter("@roleId", Convert.ToInt32(CurrentSession.RoleID));
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mOfficeIndex",param);
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mOffice item = new mOffice();
                    item.OfficeId = Convert.ToInt32(dr["OfficeId"]);
                    item.officename = Convert.ToString(dr["officename"]);
                    item.officenamelocal = Convert.ToString(dr["officenamelocal"]);
                    item.officeaddr = Convert.ToString(dr["officeaddr"]);
                    item.deptName = Convert.ToString(dr["deptName"]);
                    item.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    item.IsApex = Convert.ToInt32(dr["IsApex"]);
                    lstdept.Add(item);
                }
                return lstdept;
            }
            catch
            {
                return lstdept;
            }

        }
        public static int SaveOfficeRecord(mOffice mOffice)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mOfficeInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@deptid", mOffice.deptid);
                cmd.Parameters.AddWithValue("@Officename", mOffice.officename);
                cmd.Parameters.AddWithValue("@officenamelocal", SqlDbType.VarChar).Value = (mOffice.officenamelocal == null) ? string.Empty : mOffice.officenamelocal;
                cmd.Parameters.AddWithValue("@officeaddr", mOffice.officeaddr);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@IsApex", mOffice.IsApex);
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return 0;
            }
        }

        public static List<mOffice> GetOfficeRecordById(int id)
        {
            List<mOffice> lstdept = new List<mOffice>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@OfficeId", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mOfficeGetID", param).Tables[0].Rows)
                {
                    mOffice ofc = new mOffice();
                    {
                        
                        ofc.OfficeId = Convert.ToInt32(dr["OfficeId"]);
                        ofc.officename = Convert.ToString(dr["officename"]);
                        ofc.officenamelocal = Convert.ToString(dr["officenamelocal"]);
                        ofc.officeaddr = Convert.ToString(dr["officeaddr"]);
                        ofc.deptid = Convert.ToString(dr["deptId"]);
                        ofc.deptName = Convert.ToString(dr["deptname"]);
                        ofc.IsApex = Convert.ToInt32(dr["IsApex"]);

                    };
                    lstdept.Add(ofc);
                }

                return lstdept;
            }
            catch (Exception ex)
            {
                return lstdept;

            }
        }
        public static int UpdateOfficeRecordById(mOffice mOffice)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                

                SqlCommand cmd = new SqlCommand("mOfficeUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OfficeId", mOffice.OfficeId);
                cmd.Parameters.AddWithValue("@officename", mOffice.officename);
                cmd.Parameters.AddWithValue("@officenamelocal", SqlDbType.VarChar).Value = (mOffice.officenamelocal == null) ? string.Empty : mOffice.officenamelocal;

                cmd.Parameters.AddWithValue("@officeaddr", mOffice.officeaddr);
                cmd.Parameters.AddWithValue("@deptid", mOffice.deptid);
                cmd.Parameters.AddWithValue("@IsApex", mOffice.IsApex);
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);

                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return 0;
            }
        }

        public static int DeleteOfficeRecordById(int Id)
        {


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@OfficeId", Id),


                };
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mOfficeDelete]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }


    }

    public class LstofficeModel
    {

        public List<mOffice> _ListofficeModel { get; set; }

    }

}
