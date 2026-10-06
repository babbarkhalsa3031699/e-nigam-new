using CMNirdesh.Error;
using CMNirdesh.Models;
//using DocumentFormat.OpenXml.EMMA;

//using DocumentFormat.OpenXml.EMMA;
//using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace CMNirdesh.Models.Diaries
{
    [Serializable]
    [Table("mMlaDiarybak")]
    public class MlaDiary
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mlaDiaryId { get; set; }
        public int? RecordNumber { get; set; }
        [Required(ErrorMessage = "Please Select Document Type")]
        public string DocmentType { get; set; }
        public string Subject { get; set; }
        public int? MinisterId { get; set; }
        public string DeptId { get; set; }
        public int? ActionCode { get; set; }
        public DateTime? ForwardDate { get; set; }
        public DateTime? DiaryDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? DiaryLanguage { get; set; }
        public string DocumentCC { get; set; }
        public string DiaryRefId { get; set; }

        //
        public string Subject2 { get; set; }
        //

        [NotMapped]
        public string ForwardDateForUse { get; set; }
        [UIHint("tinymce_jquery_Diary"), AllowHtml]
        public string ItemDescription { get; set; }
        [UIHint("tinymce_jquery_DiaryAction"), AllowHtml]
        public string Remarks { get; set; }
        [UIHint("tinymce_jquery_Dispatch"), AllowHtml]
        public string letterDescription { get; set; }
        [NotMapped]
        public string RawItemDescription { get; set; }
        [NotMapped]
        public string hdnItemDescription { get; set; }
        public string ForwardedFile { get; set; }
        public string CreatedBy { get; set; }
        public int? AssemblyId { get; set; }
        public int? ConstituencyCode { get; set; }
        public int? MlaCode { get; set; }
        public string RecieveFrom { get; set; }
        //public Int32? MobileNo { get; set; }
        public string MobileNumber { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
        public string ApplicantMobile { get; set; }
        public string PresentPlace { get; set; }
        public string ProposedPlace { get; set; }
        public int? OfficeId { get; set; }
        public string OfficeId_CC { get; set; }
        [NotMapped]
        public List<string> AllOfficeIds { get; set; }
        public int[] SelectedValues { get; set; }

        public string LetterNo { get; set; }
        public int? SubDivisionId { get; set; }
        public string ActiontakenFile { get; set; }
        public DateTime? ActiontakenDate { get; set; }
        [NotMapped]
        public string ActiontakenDateForUse { get; set; }
        [NotMapped]
        public string EnclosureDateForUse { get; set; }
        public string EnclosureFile { get; set; }
        public DateTime? EnclosureDate { get; set; }
        [DefaultValue(0)]
        public Int32 SanctionAmount { get; set; }
        public bool? IsDeleted { get; set; }
        public string DocumentTo { get; set; }
        public string FinancialYear { get; set; }
        //public string GrantAmount { get; set; }
        [NotMapped]
        public string AdhrId { get; set; }
        [NotMapped]
        public string DistrictName { get; set; }
        [NotMapped]
        public string officenameCC { get; set; }

        [NotMapped]
        public string officenameCCforUse { get; set; }


        [NotMapped]
        //[Required(ErrorMessage = "Please Select Member")]
        public string MappedMemberCode { get; set; }
        [NotMapped]
        //[Required(ErrorMessage = "Please Select Member")]
        public int MemberCode { get; set; }
        [NotMapped]
        public string MappedMemberName { get; set; }
        [NotMapped]
        public int? distcd { get; set; }
        [NotMapped]
        public string officename { get; set; }
        [NotMapped]
        public string SubDivisionName { get; set; }
        [NotMapped]
        public List<MlaDiary> MlaDiaryList { get; set; }
        [NotMapped]
        public string IsFile { get; set; }
        [NotMapped]
        public string IsForwardFile { get; set; }
        [NotMapped]
        public string IsEnclosureFile { get; set; }
        [NotMapped]
        public string DeptName { get; set; }
        [NotMapped]
        public string MinisterName { get; set; }
        [NotMapped]
        public string ActionName { get; set; }
        [NotMapped]
        public string DocmentTypeName { get; set; }
        [NotMapped]
        public string BtnCaption { get; set; }
        [NotMapped]
        public List<MlaDiary> DocumentTypelist { get; set; }
        [NotMapped]
        public List<MlaDiary> ActionTypeList { get; set; }
        [NotMapped]
        public string PaperEntryType { get; set; }

        [NotMapped]
        public virtual ICollection<mDepartment> DeptList { get; set; }
        [NotMapped]
        public List<mOffice> OfficeList { get; set; }

        [NotMapped]
        public List<MlaDiary> OfficeSubList { get; set; }

        [NotMapped]
        public List<MlaDiary> ActionDetailsList { get; set; }

        [NotMapped]
        public List<MlaDiary> OfficeDeptList { get; set; }


        [NotMapped]
        public List<MlaDiary> DepartmentList { get; set; }

        [NotMapped]
        public List<mOffice> OfficeList_search { get; set; }
        [NotMapped]
        public List<MlaDiary> SubDivisionList { get; set; }
        [NotMapped]
        public List<DistrictModel> Districtist { get; set; }
        [NotMapped]
        public List<MlaDiary> DistrictList { get; set; }
        [NotMapped]
        public List<MlaDiary> MappedMemberList { get; set; }
        [NotMapped]
        public int? DistrictCode { get; set; }


        /// <summary>
        /// By Joginder For Action Taken
        /// </summary>
        /// 

        [NotMapped]
        public string ActionBy { get; set; }
        [NotMapped]
        public string ActionDescription { get; set; }

        [NotMapped]
        public DateTime? ActionDate { get; set; }


        [NotMapped]
        public string ActionEnclosureAttachment { get; set; }

        [NotMapped]
        public string IsMember { get; set; }

        [NotMapped]
        public string ActionByMember { get; set; }

        [NotMapped]
        public int PendencySince { get; set; }
        [NotMapped]
        public List<SelectListItem> FinanCialYearList { get; set; }
        [NotMapped]
        public String UserDeptId { get; set; }
        [NotMapped]
        public string UserofcId { get; set; }
        [NotMapped]
        public string MultipleOfcId { get; set; }
        [NotMapped]
        public int? ActionOfficeId { get; set; }

        /// <summary>
        /// Added by joginder
        /// </summary>
        [NotMapped]
        public int? MemberCodeForUse { get; set; }

        /// Added by Madhur
        public List<MlaDiary> myDiaryList { get; set; }

        // public <MLADairySearch> myDiaryListTest { get; set; }

       


    }
    [Serializable]
    public class mdiaryno
    {
        public int? Diaryno { get; set; }
        // public string ActionName { get; set; }
        //[NotMapped]
        //public List<mdiaryno> DiaryNulist { get; set; }
    }



    [Serializable]
    [Table("mTypeOfAction")]
    public class mTypeOfAction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionId { get; set; }
        public int ActionCode { get; set; }
        public string ActionName { get; set; }
        public string ActionNameLocal { get; set; }
        public int? ActionOrder { get; set; }
        public string ActionNameStatus { get; set; }
        public bool IsDeleted { get; set; }
        public string ActionSubtitle { get; set; }
        public static List<mTypeOfAction> GetActionsList()
        {

            List<mTypeOfAction> lstdept = new List<mTypeOfAction>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mActionStatusIndex");
                con.Close();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mTypeOfAction item = new mTypeOfAction();
                    item.ActionId = Convert.ToInt32(dr["ActionId"]);
                    item.ActionName = Convert.ToString(dr["ActionName"]);
                    item.ActionNameStatus = Convert.ToString(dr["ActionNameStatus"]);
                    item.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    lstdept.Add(item);
                }
                return lstdept;
            }
            catch
            {
                return lstdept;
            }

        }

        public static int SaveActionRecord(mTypeOfAction mTypeOfAction)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@ActionName", mTypeOfAction.ActionName);
                param[1] = new SqlParameter("@ActionNameStatus", mTypeOfAction.ActionNameStatus);
                param[2] = new SqlParameter("@ActionOrder", mTypeOfAction.ActionOrder);
                param[3] = new SqlParameter("@ActionCode", mTypeOfAction.ActionCode);
                param[4] = new SqlParameter("@ActionSubtitle", mTypeOfAction.ActionSubtitle);
                param[5] = new SqlParameter("@CreatedBy", CurrentSession.UserID);
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "mTypeOfActionInsert", param);
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<mTypeOfAction> GetActionRecordById(int id)
        {
            List<mTypeOfAction> lstdept = new List<mTypeOfAction>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ActionId", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mTypeOfActionGetId", param).Tables[0].Rows)
                {
                    mTypeOfAction dept = new mTypeOfAction();
                    {
                        dept.ActionId = Convert.ToInt32(dr["ActionId"]);
                        dept.ActionName = Convert.ToString(dr["ActionName"]);
                        dept.ActionNameStatus = Convert.ToString(dr["ActionNameStatus"]);
                        if (dr["orderId"] != System.DBNull.Value)
                        {
                            dept.ActionOrder = Convert.ToInt32(dr["orderId"]);
                        }
                        if (dr["ActionCode"] != System.DBNull.Value)
                        {
                            dept.ActionCode = Convert.ToInt32(dr["ActionCode"]);
                        }
                        if (dr["ActionSubtitle"] != System.DBNull.Value)
                        {
                            dept.ActionSubtitle = Convert.ToString(dr["ActionSubtitle"]);
                        }
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
        //public static int UpdateActionRecordById(mTypeOfAction mTypeOfAction)
        //{
        //    try
        //    {
        //        SqlConnection con = ClsConnection.GetConnection();
        //        if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
        //            con.Open();
        //        SqlParameter[] param = new SqlParameter[4];
        //        param[0] = new SqlParameter("@ActionId", mTypeOfAction.ActionId);
        //        param[1] = new SqlParameter("@ActionName", mTypeOfAction.ActionName);
        //        param[2] = new SqlParameter("@ActionNameStatus", mTypeOfAction.ActionNameStatus);
        //        param[3] = new SqlParameter("@ModifiedBy", CurrentSession.UserID);
        //        CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "mTypeOfActionUpdate", param);
        //        con.Close();
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}
        public static int UpdateActionRecordById(mTypeOfAction mTypeOfAction)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@ActionId", mTypeOfAction.ActionId);
                param[1] = new SqlParameter("@ActionName", mTypeOfAction.ActionName);
                param[2] = new SqlParameter("@ActionNameStatus", mTypeOfAction.ActionNameStatus);
                param[3] = new SqlParameter("@ActionOrder", mTypeOfAction.ActionOrder);
                param[4] = new SqlParameter("@ActionCode", mTypeOfAction.ActionCode);
                param[5] = new SqlParameter("@ActionSubtitle", mTypeOfAction.ActionSubtitle);
                param[6] = new SqlParameter("@CreatedBy", CurrentSession.UserID);
                CMNirdesh.Models.SqlHelper.ExecuteNonQuery(con, "mTypeOfActionUpdate", param);
                con.Close();



                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int DeleteActionRecordById(int Id)
        {


            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@Action", Id),


                };
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mTypeOfActionDelete]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }


    }
    public class LstActionStatus
    {
        private List<mTypeOfAction> lstActionStatus;

        public List<mTypeOfAction> _LstActionStatus { get => lstActionStatus; set => lstActionStatus = value; }
    }
    [Serializable]
    [Table("mTypeOfDocument")]
    public class mTypeOfDocument
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeNameLocal { get; set; }
        public string fileacessingUrl { get; set; }
        public string DocumentType { get; set; }

        public string DocumentIcon { get; set; }
        public string DeptId { get; set; }
        public string OfficeId { get; set; }
        public string Office { get; set; }
        [AllowHtml]

        public string AppIcon { get; set; }
        public bool IsDeleted { get; set; }
        public HttpPostedFileBase ImageLocation
        {

            get;
            set;
        }
        public static List<mTypeOfDocument> GetTypeOfDocumentList()
        {

            List<mTypeOfDocument> lstdept = new List<mTypeOfDocument>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@OfficceId", Convert.ToInt32(CurrentSession.OfficeId));
                param[1] = new SqlParameter("@roleId", Convert.ToInt32(CurrentSession.RoleID));
                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mTypeOfDocumentIndex", param).Tables[0].Rows)
                {
                    mTypeOfDocument item = new mTypeOfDocument();
                    item.DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]);
                    item.DocumentTypeName = Convert.ToString(dr["DocumentTypeName"]);
                    item.DocumentTypeNameLocal = Convert.ToString(dr["DocumentTypeNameLocal"]);
                    item.DocumentIcon = Convert.ToString(dr["DocumentIcon"]);
                    item.AppIcon = Convert.ToString(dr["AppIcon"]);
                    item.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    item.DeptId = Convert.ToString(dr["DeptId"]);
                    item.OfficeId = Convert.ToString(dr["officeId"]);
                    item.Office = Convert.ToString(dr["Office"]);
                    lstdept.Add(item);
                }
                return lstdept;
            }
            catch
            {
                return lstdept;
            }

        }

        public static int SaveTypeOfDocumentRecord(mTypeOfDocument mTypeOfDocument)
        {
            try
            {
                string DeptId = Convert.ToString(CurrentSession.DeptID);
                string OfficeId = Convert.ToString(CurrentSession.OfficeId);
                if (CurrentSession.RoleID == "18")
                {
                    DeptId = mTypeOfDocument.DeptId.ToString();
                    OfficeId = mTypeOfDocument.OfficeId.ToString();
                }
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlCommand cmd = new SqlCommand("mTypeOfDocumentInsert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DocumentTypeName", mTypeOfDocument.DocumentTypeName);
                cmd.Parameters.AddWithValue("@DocumentTypeNameLocal", mTypeOfDocument.DocumentTypeNameLocal);
                cmd.Parameters.AddWithValue("@AppIcon", SqlDbType.VarChar).Value = (mTypeOfDocument.AppIcon == null) ? string.Empty : mTypeOfDocument.AppIcon;
                cmd.Parameters.AddWithValue("@reftype", mTypeOfDocument.DocumentType);
                cmd.Parameters.AddWithValue("@DeptId", DeptId);
                cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                cmd.Parameters.AddWithValue("@CreatedBy", CurrentSession.UserID);
                cmd.ExecuteScalar().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static List<mTypeOfDocument> GetTypeOfDocumentRecordById(int id)
        {
            List<mTypeOfDocument> lstdept = new List<mTypeOfDocument>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@DocumentTypeId", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "mTypeOfDocumentGetID", param).Tables[0].Rows)
                {
                    mTypeOfDocument item = new mTypeOfDocument();
                    {

                        item.DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]);
                        item.DocumentTypeName = Convert.ToString(dr["DocumentTypeName"]);
                        item.DocumentTypeNameLocal = Convert.ToString(dr["DocumentTypeNameLocal"]);
                        item.AppIcon = Convert.ToString(dr["AppIcon"]);
                        item.DocumentType = Convert.ToString(dr["reftype"]);
                        item.DeptId = Convert.ToString(dr["DeptId"]);
                        item.OfficeId = Convert.ToString(dr["officeId"]);

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

        public static int UpdateTypeOfDocumentRecordById(mTypeOfDocument mTypeOfDocument, string val)
        {
            try
            {
                string DeptId = Convert.ToString(CurrentSession.DeptID);
                string OfficeId = Convert.ToString(CurrentSession.OfficeId);
                if (CurrentSession.RoleID == "18")
                {
                    DeptId = mTypeOfDocument.DeptId.ToString();
                    OfficeId = mTypeOfDocument.OfficeId.ToString();
                }

                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("mTypeOfDocumentUpdate", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@op", val);
                cmd.Parameters.AddWithValue("@DocumentTypeId", mTypeOfDocument.DocumentTypeId);
                cmd.Parameters.AddWithValue("@DocumentTypeName", mTypeOfDocument.DocumentTypeName);
                cmd.Parameters.AddWithValue("@DocumentTypeNameLocal", mTypeOfDocument.DocumentTypeNameLocal);
                cmd.Parameters.AddWithValue("@reftype", mTypeOfDocument.DocumentType);
                cmd.Parameters.AddWithValue("@AppIcon", SqlDbType.VarChar).Value = (mTypeOfDocument.AppIcon == null) ? string.Empty : mTypeOfDocument.AppIcon;
                cmd.Parameters.AddWithValue("@DeptId", DeptId);
                cmd.Parameters.AddWithValue("@OfficeId", OfficeId);
                cmd.Parameters.AddWithValue("@ModifiedBy", CurrentSession.UserID);
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int DeleteTypeOfDocumentId(int Id)
        {

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@DocumentTypeId", Id),


                };
                int refid = Convert.ToInt32(SqlHelper.ExecuteScalar(connection, "[mTypeOfDocumentDelete]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return 0;
            }

        }

        public static bool SaveEndorsement(EndorsementApprovalVM model, string userId, string Deptd, string OfficeId)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_LG_SaveEndorsementUpdated", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EndorsementId", model.EndorsementId);
                cmd.Parameters.AddWithValue("@AgendaId", model.AgendaId);
                cmd.Parameters.AddWithValue("@EndorsementSubject", model.EndorsementSubject ?? "");
                cmd.Parameters.AddWithValue("@FooterSubject", model.FooterSubject ?? "");
                cmd.Parameters.AddWithValue("@DispatchNumber",
                    string.IsNullOrWhiteSpace(model.DispatchNumber)
                        ? (object)DBNull.Value
                        : model.DispatchNumber);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Deptd", Deptd);
                cmd.Parameters.AddWithValue("@OfficeId", Convert.ToInt32(OfficeId));
                cmd.Parameters.AddWithValue("@RefID", model.RefID);
                cmd.Parameters.AddWithValue("@SendTo", model.SendTo ?? "");
                cmd.Parameters.AddWithValue("@ApprovalSubject", model.Subject ?? "");
                cmd.Parameters.AddWithValue("@ApprovalHeader", model.Header ?? "");
                cmd.Parameters.AddWithValue("@RejectionFormat", model.RejectionFormat ?? "");

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int success))
                {
                    return success == 1;
                }

                return false;
            }
        }

        public static List<EndorsementApprovalVM> GetEndorsementList(int agendaId)
        {
            List<EndorsementApprovalVM> list = new List<EndorsementApprovalVM>();

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_LG_GetEndorsementList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AgendaId", agendaId);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new EndorsementApprovalVM
                        {
                            EndorsementId = Convert.ToInt32(dr["EndorsementId"]),
                            EndorsementSubject = dr["EndorsementSubject"].ToString(),
                            FooterSubject = dr["FooterSubject"].ToString(),
                            Subject = dr["ApprovalSubject"].ToString(),
                            DispatchNumber = dr["DispatchNumber"].ToString(),
                            Header = dr["ApprovalHeader"].ToString(),
                            SendTo= dr["SendTo"].ToString(),
                            RejectionFormat = dr["RejectionFormat"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public static EndorsementApprovalVM GetEndorsementById(int id)
        {
            EndorsementApprovalVM model = new EndorsementApprovalVM();
            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_LG_GetEndorsementById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EndorsementId", id);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        model.EndorsementId = Convert.ToInt32(dr["EndorsementId"]);
                        model.AgendaId = Convert.ToInt32(dr["AgendaId"]);
                        model.EndorsementSubject = dr["EndorsementSubject"].ToString();
                        model.FooterSubject = dr["FooterSubject"].ToString();
                        model.DispatchNumber = dr["DispatchNumber"].ToString();
                        model.Subject = dr["ApprovalSubject"].ToString();
                        model.Header = dr["ApprovalHeader"].ToString();
                        model.SendTo = dr["SendTo"].ToString();
                        model.RejectionFormat = dr["RejectionFormat"].ToString();
                    }
                }
            }

            return model;
        }

        public static bool SaveCreateApproval(AgendaApprovalStatusModel model)
        {
            SqlConnection con = ClsConnection.GetConnection();
            try
            {
                SqlParameter[] param =
                {
                new SqlParameter("@AgendaId", model.AgendaId),
                new SqlParameter("@IsCreateApproval", model.IsCreateApproval),
                new SqlParameter("@UserId", model.UserId)
            };

                SqlHelper.ExecuteNonQuery(
                    con,
                    CommandType.StoredProcedure,
                    "sp_SaveAgendaApprovalStatus",
                    param);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static AgendaApprovalStatusModel GetCreateApprovalStatus(int AgendaId)
        {
            SqlConnection con = ClsConnection.GetConnection();
            AgendaApprovalStatusModel model =
                new AgendaApprovalStatusModel();

            try
            {
                SqlParameter[] param =
                {
                new SqlParameter("@AgendaId", AgendaId)
            };

                DataTable dt = SqlHelper.ExecuteDataset(
                    con,
                    CommandType.StoredProcedure,
                    "sp_GetAgendaApprovalStatus",
                    param).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    model.AgendaId = AgendaId;

                    model.IsCreateApproval =
                        Convert.ToBoolean(dt.Rows[0]["IsCreateApproval"]);
                }
            }
            catch
            {

            }

            return model;
        }

    }
    public class LstTypeOfDocumentModel
    {
        public List<mTypeOfDocument> _ListTypeOfDocumentModel { get; set; }
        public SelectList mDepartmentList { get; set; }

        public SelectList OfficeList { get; set; }
        public string fileacessingUrl { get; set; }
        public string DeptId { get; set; }
        public int OfficeId { get; set; }

    }
    public class Note
    {
        public string type { get; set; }
        public string text { get; set; }
        public string value { get; set; }
    }
        [Serializable]
    [Table("tDiaryAction")]
    public class DiaryActionData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int NoteId { get; set; }
        public int Freezed { get; set; }
        public string isDraft { get; set; }
        public int Id
        {

            get;
            set;
        }

        public int OfficeId
        {

            get; set;
        }
        public int DiaryRefId
        {

            get; set;
        }

        public string MergeRefid
        {

            get; set;
        }
        public int ActionId
        {

            get; set;
        }

        public DateTime ActionDate
        {

            get; set;
        }
        public double ActionAmountSanction
        {
            get;
            set;
        }
        public string ActionBy
        {
            get;
            set;
        }

        [UIHint("tinymce_jquery_DiaryAction"), AllowHtml]
        public string ActionDescription { get; set; }

        public string Enclosure
        {

            get;
            set;
        }
        public int Status
        {

            get;
            set;
        }
        public bool? IsMember
        {

            get;
            set;
        }
        public string ActionByMember

        {

            get;
            set;
        }
        public Int64 RefId
        {

            get;
            set;
        }

        [NotMapped]
        public string StatusName
        {

            get;
            set;
        }



    }

    public class ProposalWardModel
    {
        public int WardId { get; set; }
        public string WardName { get; set; }
    }

    [Serializable]
    [Table("mMlaDiary")]
    public class MlaDiaryData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 RefId { get; set; }
        public Int64 LinkRefID { get; set; }
        [Required(ErrorMessage = "Please Select From Department")]

        public string FromDeptId { get; set; }
        [Required(ErrorMessage = "Please Select From Office")]
        public int FromOfficeId { get; set; }

        public int AssinedOfficeId { get; set; }
        public int MoveId { get; set; }
        public bool IsSup { get; set; }
        public string agendaId { get; set; }
        public string agendaDate { get; set; }
        public string IsMeetingFile { get; set; }
        public string AgendaApprove { get; set; }

        public bool ResButton { get; set; }
        public bool PassButton { get; set; }
        public bool MeeingEnd { get; set; }
        public string ProceedingApprove { get; set; }
        public string AgendaPdf { get; set; }
        public string AgendaPdfName { get; set; }
       
        
        public string ProceedingPdf { get; set; }
        public string ProceedingCommCommentPdf { get; set; }

        public string SignedProceedingMayorPath { get; set; }
        public string SignedProceedingCommissionerPath { get; set; }




        public string ProceedingPdfName { get; set; }

        public List<AgendaReferences> _LstReferences { get; set; }

        public AgendaModel CreateMeeting { get; set; }
        public MeetingSettingModel MeetingSetting { get; set; }
        public List<AgendaReferences> _MeetingSettingReferences { get; set; }
        public string AgendaCount { get; set; }
        public string MapId { get; set; }
        public Int64 MainRefId { get; set; }
        public string RefType { get; set; }
        public string RefFileType { get; set; }

        public string dashboardtype { get; set; }

        public string MultiUpdate { get; set; }
        public string SwipeId { get; set; }
        public int MeetingId { get; set; }
        public int? DispatchNumber { get; set; }
        public string ToDeptId { get; set; }
        public int ToOfficeId { get; set; }
        public int Memembercode { get; set; }

        public int MinisterCode { get; set; }
        public int AssemblyCode { get; set; }

        

         public int sectorID { get; set; }

        public bool PdfButtons { get; set; }
        public int ButtonPermission { get; set; }
        public int Id { get; set; }
        public string AssemblyName { get; set; }

        public string SessionName { get; set; }

        public string MemberName { get; set; }
        public string MinisteryName { get; set; }
        public string AssemblyCodee { get; set; }

        public string SessionCodee { get; set; }

        public int MemberCode { get; set; }

        public int SessionCode { get; set; }

        public string MenuName { get; set; }
        public string DiaryNumber { get; set; }

        public string DocmentType { get; set; }
        public int ReferencePriorityType { get; set; }
        public int ReferenceTerm { get; set; }
        [Required(ErrorMessage = "Please Select Priority Type")]
        public string LetterNo { get; set; }

        public DateTime? LetterDate { get; set; }
        public string LetterDateNew { get; set; }


        public string StatementImplementation { get; set; }
        public string StatementImplementation_local { get; set; }

        public string PressNoteENG { get; set; }
        public string PressNotePun { get; set; }

        public string TimelineId { get; set; }
        public string TimelineRemarks { get; set; }


        public List<string> DepartmentListIds { get; set; }
        public List<string> ApprovalListIds { get; set; }

        public int ApprovalID { get; set; }

        public string Subject { get; set; }

        public string SubjectPB { get; set; }

        //[UIHint("tinymce_jquery_Diary"), AllowHtml]
        [AllowHtml]
        public string SubjectDetails { get; set; }

        [AllowHtml]
        public string SubjectDetailsPB { get; set; }

        public string EnclosureFile { get; set; }

        public string isLGoffice { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
        public string ApplicantMobile { get; set; }

        public string ApplicantEmail { get; set; }
        public decimal ProposalAmount { get; set; }
        public DateTime? DiaryDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ActionCode { get; set; }
        public int? ActionCodeMulti { get; set; }

        public string SentToOffice { get; set; }
        public string SentFromOffice { get; set; }
        public string SentToUserId { get; set; }
        public string SentFromUserId { get; set; }
        public string SentMlaUserId { get; set; }
        public string SentMLAOffice { get; set; }
        public SelectList MLAOfficeList { get; set; }
        public SelectList FromOfficeList { get; set; }
        public SelectList ToOfficeList { get; set; }

        public SelectList FromUserList { get; set; }
        public SelectList ToUserList { get; set; }
        public SelectList MlaUserList { get; set; }
        public string HideUpdate { get; set; }
        public string HideRef { get; set; }

        public string ResolutionNo { get; set; }
        public string filetype { get; set; }

        public string isPartFile { get; set; }
        public string LinkedFromDeptName { get; set; }
        public string eBookFile { get; set; }
        public string LinkList { get; set; }
        public string MergeLinkList { get; set; }
        public string ResCount { get; set; }
        public DateTime? ActiontakenDate { get; set; }

        //[UIHint("tinymce_jquery_DiaryAction"), AllowHtml]
        [AllowHtml]
        public string ActionDescription { get; set; }

        public List<string> CheckedRefs { get; set; }

        [AllowHtml]
        public string ActionDescriptionMulti { get; set; }

        [AllowHtml]
        public string ActionDescriptionMultiPB { get; set; }
        public double? ActionAmountSanction { get; set; }
        public string ActionTakenFile { get; set; }
        [NotMapped]
        public string CurrentDate
        {

            get;
            set;
        }

        public int? ActionTypeid
        {

            get;
            set;
        }


        [NotMapped]
        public int? DiaryActionCode
        {
            get; set;


        }
        
       [NotMapped]
        public string IsFile { get; set; }

        [NotMapped]
        public string IsAgendaFile { get; set; }

        [NotMapped]
        public string IsEnclosureFile { get; set; }

        [NotMapped]
        public string isLetter { get; set; }

        [NotMapped]
        public int OfficeId { get; set; }



        //[Required(ErrorMessage = "Please select an Act.")]
        public int ActId { get; set; }

        [NotMapped]
        public string DeptId { get; set; }
        [NotMapped]
        public string officename { get; set; }

        [NotMapped]
        public string deptname { get; set; }

        [NotMapped]
        public string deptnamelocal { get; set; }

        [NotMapped]
        public string PaperEntryType { get; set; }

        public bool Draftchk { get; set; }
        public bool FileDraft { get; set; }

        [NotMapped]
        public SelectList DocumentTypelist { get; set; }

        [NotMapped]
        public SelectList TimeLine { get; set; }

        [NotMapped]
        public List<DepartmentSelectListItem> DeparmentList { get; set; }

        [NotMapped]
        public SelectList CMApproval { get; set; }

        [NotMapped]
        public SelectList Act { get; set; }
        [NotMapped]
        public SelectList Status { get; set; }

        [NotMapped]
        public SelectList FileDocumentTypelist { get; set; }

        public string ActName { get; set; }
        public int IsFileCount { get; set; }

        [NotMapped]
        public SelectList MeetingList { get; set; }

        public SelectList FileList { get; set; }

        public List<AgendaModel> _LstAgenda { get; set; }

        [NotMapped]
        public SelectList ReferencePriorityTypeList { get; set; }
        [NotMapped]
        public SelectList ActionTypeList { get; set; }


        [NotMapped]
        public SelectList ActionTypeList_Ref { get; set; }

        [NotMapped]
        public SelectList ActionListFilter { get; set; }

        [NotMapped]
        public SelectList mDepartmentList { get; set; }

        public List<AssemblySelectItem> mAssemblyList { get; set; }

        [NotMapped]
        public List<DepartmentSelectListItem> mDepartmentListPbi { get; set; }
        [NotMapped]
        public SelectList OfficeList { get; set; }

        public SelectList MinisterList { get; set; }

     
        public int[] SelectedWardIds { get; set; }
        public List<ProposalWardModel> WardListItems { get; set; }
        public string WardListItemsJson { get; set; }

       
        public SelectList WardList { get; set; }

        public SelectList ActList { get; set; }
        public SelectList MemberList { get; set; }



        public SelectList SessionList { get; set; }

        public SelectList SectorList { get; set; }

        public SelectList ProposalList { get; set; }

        public SelectList SessDatelist { get; set; }

        public SelectList AssemblyList { get; set; }
        [NotMapped]
        public string BtnCaption { get; set; }
        [NotMapped]
        public string ToOffice { get; set; }
        [NotMapped]
        public string ToDept { get; set; }

        [NotMapped]
        public string FromDept { get; set; }
        [NotMapped]
        public string FromOffice { get; set; }
        [NotMapped]
        public string DocumentTypeName { get; set; }
        public string DocumentType { get; set; }

        [NotMapped]
        public List<DiaryActionData> ActionDetailsList { get; set; }

        public List<DiaryActionData> MainFileActionDetailsList { get; set; }
        public List<MultifileJson> MultiFileModel { get; set; }
        public List<LinkedFiles> linkedFilesList { get; set; }
        public string fileacessingUrl { get; set; }


        public List<Note> NoteList { get; set; }
        public string NoteString { get; set; }

        [NotMapped]
        public List<MlaDiaryData> myDiaryList { get; set; }


        public bool CopyTo { get; set; }

        [NotMapped]
        public List<TemplateList> TemplateList { get; set; }

        [NotMapped]
        public int isfund { get; set; }


        [NotMapped]
        public int IsSignedApprovalPDf { get; set; }
        [NotMapped]
        public int IsApprovalPDf { get; set; }



        [NotMapped]
        public string FundName { get; set; }


        [NotMapped]
        public int Availablefund { get; set; }
        [NotMapped]
        public int SantionAmount { get; set; }

        [NotMapped]
        public DateTime SanctionDate { get; set; }

        [NotMapped]
        public DateTime ApproveDate { get; set; }

        [NotMapped]
        public string Remarks { get; set; }

        [NotMapped]
        public List<FundReportModel> _LstFundReport { get; set; }

        [NotMapped]
        public List<FundReportModel> _LstFundSactioned { get; set; }

        [NotMapped]
        public int fundtype_3 { get; set; }

        [NotMapped]
        public int fundtype_4 { get; set; }

        [NotMapped]
        public int fundtype_13 { get; set; }
        [NotMapped]
        public int fundtype_14 { get; set; }
        [NotMapped]
        public int fundtype_15 { get; set; }


        [NotMapped]
        public int San_ReliefFunds { get; set; }

        [NotMapped]
        public int San_SmallSavingFunds { get; set; }

        [NotMapped]
        public int San_UntiedFunds { get; set; }
        [NotMapped]
        public int San_VivekiFunds { get; set; }
        [NotMapped]
        public int San_CattleFairFunds { get; set; }

        public string EditType { get; set; }

        public string SentType { get; set; }

        public string AddDetails { get; set; }


        public string MeetingTypeName { get; set; }

        [NotMapped]
        public List<MeetingType> _MeetingType { get; set; }

        [NotMapped]
        public List<CoveringLetter> _CoveringLetterList { get; set; }
        
        public List<EndorsementApprovalVM> EndorsementList { get; set; }


       
    }

    public class MeetingSettingModel
    {
        public int AgendaId { get; set; }
        public Int64 RefId { get; set; }
        public string AgendaName { get; set; }
        public DateTime? AgendaDate { get; set; }
        public string AgendaDateDisplay { get; set; }
        public bool IsOpen { get; set; }
        public SelectList AgendaList { get; set; }
        public SelectList FileList { get; set; }
        public int MeetingId { get; set; }

        public List<AgendaReferences> _LstReferences { get; set; }
        public List<AgendaReferences> _LstExtraReferences { get; set; }
        public List<AgendaReferences> _LstResolutionActions { get; set; }
        public List<AgendaReferences> _LstFileDetails { get; set; }

        public string NewLetterNo { get; set; }

        public string NewSubject { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string NewSubjectDetails { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string EditSubjectDetails { get; set; }
        public string attachfile { get; set; }
        public string IsFile { get; set; }
        public string Createdby { get; set; }
        public int HdnAgendaId { get; set; }

        public bool MeeingEnd { get; set; }
        public bool freezeButton { get; set; }
        public bool PassButton { get; set; }

        public bool PdfButtons { get; set; }
        public int AgendaDetailId { get; set; }

        public string AgendaType { get; set; }
        public string FileRefId { get; set; }
        public string Count { get; set; }

        public string agendadetails { get; set; }

        public string refresh { get; set; }
        public string freezed { get; set; }

        public string unfreezed { get; set; }
        public string passed { get; set; }


        public int ActId { get; set; }
        public SelectList ActList { get; set; }

        public int ActionCodeMulti { get; set; }
        public SelectList ActionList { get; set; }
        [AllowHtml]
        [DataType(DataType.Html)]
        public string ActionDescriptionMulti { get; set; }
        public string CurrentDate { get; set; }
        public string MultiUpdate { get; set; }

        public int ButtonPermission { get; set; }
    }
    public class AddNewReferenceJson
    {
        public string LetterNo { get; set; }
        public DateTime LetterDate { get; set; }
        public string Subject { get; set; }

    }

    public class AssemblySelectItem
    {
        public Int64? AssemblyID { get; set; }
        public string AssemblyCode { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyNameLocal { get; set; }

    }

    public class DepartmentSelectListItem
    {
        public bool Selected { get; set; }

        public string Text { get; set; }

        public string Value { get; set; }

        public string TextPbi { get; set; }
        public string SecPbi { get; set; }
        public int FileNo { get; set; }
        public string isAdvice { get; set; }
    }
    public class FundReportModel
    {
        public int FundSanctionId { get; set; }
        public int FundTypeId { get; set; }
        public string FundType { get; set; }
        public int FundAllocated { get; set; }

        public int FundApproved { get; set; }
        public int FundSanctioned { get; set; }
        public int FundAvaiable { get; set; }

        public string DeptName { get; set; }

        public string ApproveDate { get; set; }
        public string SanctionDate { get; set; }
        public string DisableClass { get; set; }

        public int OfficeId { get; set; }
        public string OfficeName { get; set; }

    }

    public class EndorsementApprovalVM
    {  
        public string RefID { get; set; }
        public int EndorsementId { get; set; }
        public int AgendaId { get; set; }

        public string SendTo { get; set; }
        public string Subject { get; set; }
        public string Header { get; set; }
        public string EndorsementSubject { get; set; }
        public string FooterSubject { get; set; }
        public string DispatchNumber { get; set; }
        public string RejectionFormat { get; set; }

    }

    public class AgendaApprovalStatusModel
    {
        public int AgendaId { get; set; }

        public bool IsCreateApproval { get; set; }

        public string UserId { get; set; }
    }


    public class TemplateList
    {
        public int Id
        {

            get;
            set;
        }
        public int OfficeId
        {

            get;
            set;
        }

        public string TemplateName { get; set; }
        public string TemplateHead { get; set; }
        public string TemplateSubjectEng
        {

            get;
            set;

        }

        public string TemplateSubjectLocal
        {

            get;
            set;

        }

        public string BodyTextEng
        {

            get;
            set;

        }
        public string BodyTextLocal
        {

            get;
            set;

        }

        public string SignStamp
        {

            get;
            set;
        }





    }




}

