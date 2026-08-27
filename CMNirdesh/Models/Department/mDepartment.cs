    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Data.SqlClient;

namespace CMNirdesh.Models
{

    [Serializable]
    [Table("mDepartment2")]
    public partial class mDepartment
    {
        [Key]
        [StringLength(50)]
        
        public string deptId { get; set; }
        [Required]
        public string deptabbr { get; set; }
        [Required]
        public string deptname { get; set; }
        [Required]
        public string deptnameLocal { get; set; }

        public string deptphoto { get; set; }

        public string DeptConStr { get; set; }

        public string enivaran { get; set; }

        public long? GriNumber { get; set; }

        public DateTime? GriNumberUpdationDate { get; set; }

        public string LatestGriNumber { get; set; }

        public int? RowNumber { get; set; }

        public string eSamadhanNote { get; set; }

        public string eSamadhanOnline { get; set; }

        public string eSameekshaOnlineYN { get; set; }

        public int? eSameekshaWorkCode { get; set; }

        public string deptabbrLocal { get; set; }

        public string eSameekshaWorkCodeYear { get; set; }
        public string StarredBeforeFixPath { get; set; }
        public string UnStarredBeforeFixPath { get; set; }
        public string StarredApproved { get; set; }
        public string UnStarredApproved { get; set; }
        public DateTime? SLastUpdatedDate { get; set; }
        public DateTime? UnSLastUpdatedDate { get; set; }


        public string NoticeBeforeFixPath { get; set; }
        public DateTime? NoticeBFLUpdatedDate { get; set; }
        public string NoticeAfterFixPath { get; set; }
        public bool? IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedWhen { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedWhen { get; set; }

        public bool? IsDeleted { get; set; }

        public string Prefix { get; set; }
        [Required]
        public int? UserTypeId { get; set; }
        [Required]
        public int? StateId { get; set; }


        public static List<mDepartment> GetDepartmentsList()
        {

            List<mDepartment> lstdept = new List<mDepartment>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mDepartmentIndex");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mDepartment dept = new mDepartment();
                    dept.deptId = Convert.ToString(dr["deptId"]);
                    dept.deptname = Convert.ToString(dr["deptname"]);
                    dept.deptnameLocal = Convert.ToString(dr["deptnameLocal"]);
                    dept.deptabbr = Convert.ToString(dr["deptabbr"]);
                    dept.IsActive = false;
                    if (dr["IsActive"] != System.DBNull.Value)
                    {
                        dept.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                    lstdept.Add(dept);
                }
                return lstdept;
            }
            catch(Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return lstdept;
            }

        }

        public static int SaveDepartmentRecord(mDepartment mDepartment)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@deptname", mDepartment.deptname);
                param[1] = new SqlParameter("@deptabbr", mDepartment.deptabbr);
                param[2] = new SqlParameter("@deptnameLocal", mDepartment.deptnameLocal);
                param[3] = new SqlParameter("@CreatedBy", CurrentSession.UserID);
                param[4] = new SqlParameter("@StateId", mDepartment.StateId);
            



                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "mDepartmentInsert", param);
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<mDepartment> GetDepartmentListById(string id)
        {
            List<mDepartment> lstdept = new List<mDepartment>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@deptId", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mDepartmentGetID", param).Tables[0].Rows)
                {
                    mDepartment dept = new mDepartment();
                    {
                        dept.deptId = Convert.ToString(dr["deptId"]);
                        dept.deptname = Convert.ToString(dr["deptname"]);
                        dept.deptnameLocal = Convert.ToString(dr["deptnameLocal"]);
                        dept.deptabbr = Convert.ToString(dr["deptabbr"]);
                    };
                    lstdept.Add(dept);
                }

                return lstdept;
            }
            catch (Exception ex)
            {
                return lstdept;

            }
        }
        public static int UpdateDepartmentRecordById(mDepartment mDepartment)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@deptId", mDepartment.deptId);
                param[1] = new SqlParameter("@deptname", mDepartment.deptname);
                param[2] = new SqlParameter("@deptabbr", mDepartment.deptabbr);
                param[3] = new SqlParameter("@deptnameLocal", mDepartment.deptnameLocal);
                param[4] = new SqlParameter("@ModifiedBy", CurrentSession.UserID);
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "mDepartmentUpdate", param);
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string DeleteDepartmentById(string Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@deptId", Id),
                    

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[mDepartmentDelete]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


    }
        public class LstdeptModel
        {

            public List<mDepartment> _ListdeptModel { get; set; }
        }
    }



















