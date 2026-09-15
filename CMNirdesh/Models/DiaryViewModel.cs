using CMNirdesh.Models.Diaries;
using CMNirdesh.Models.Session;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Tsp;
using PoliteCaptcha;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class DiaryViewModel
    {
        public Int64? RefId { get;  set; }
        public Int64? LinkRefID { get; set; }
        public string LinkList { get; set; }
        public string MergeLinkList { get; set; }
        public string FromDeptId { get; set; }

        public string FromDeptLocal{ get; set; }
        public int FromOfficeId { get; set; }
        public int? DispatchNumber { get; set; }
        public string ToDeptId { get; set; }
    
        public bool Draftchk { get; set; }
        public bool FileDraft { get; set; }
        public string LGComments { get; set; } 
        public int ToOfficeId { get; set; }
        public int MoveId { get; set; }
        public string DiaryNumber { get; set; }
        public int ReferencePriorityType { get; set; }
        public int ReferenceTerm { get; set; }
        public string DocmentType { get; set; }
        public string LetterNo { get; set; }
        public DateTime? LetterDate { get; set; }
        public string Subject { get; set; }
        public string SubjectDetails { get; set; }

        public string SubjectLocal { get; set; }
        public string SubjectDetailsLocal { get; set; }
        public string SubjectPB { get; set; }
        public string SubjectDetailsPB { get; set; }
        public string EnclosureFile { get; set; }
        public string StatementImplementation_local { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
        public string ApplicantMobile { get; set; }
        public string ApplicantEmail { get; set; }
        public DateTime? DiaryDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ActionCode { get; set; }
        public DateTime? ActionTakenDate { get; set; }
        public string CurrentDate { get; set; }
        public string ActionDescription { get; set; }
        public string ActionDescriptionPB { get; set; }
        public double? ActionAmountSanction { get; set; }
        public string ActionTakenFile { get; set; }
        public string Userid { get; set; }
        public string BriefLastAction { get; set; }
        public int? ActionTypeid { get; set; }
        public string FromOffice { get; set; }
        public string FromDept { get; set; }
        public string DocumentTypeName { get; set; }
        public string ToOffice { get; set; }
        public string ToDept { get; set; }
        public int OfficeId { get; set; }
        public int isfund { get; set; }
        public int IsSignedApprovalPDf { get; set; }
        public int IsApprovalPDf { get; set; }
        public string StatusName { get; set; }

        public Int64? AssemblyID { get; set; }
        public int AssemblyCode { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyNameLocal { get; set; }
        public int MemberCode { get; set; }
        public int MinistryID { get; set; }
        public string MemberName { get; set; }
        public int SessionCode { get; set; }
        public string SessionName { get; set; }

        public string Header { get; set; }
        public string Date { get; set; }
        public string isLetter { get; set; }
        public int MeetingId { get; set; }
        public string FromDepartment { get; set; }
        public string officeName { get; set; }
        public string subject { get; set; }
        public string dispatchDate { get; set; }
        public string sentToDepartment { get; set; }
        public string sentToOffice { get; set; }
        public string sentActionStatus { get; set; }
        public string SentToUserId { get; set; }
        public string InboxType { get; set; }
        public string SentType { get; set; }
        public string ActionBy { get; set; }
        public string agendaId { get; set; }
        public string agendaDate { get; set; }
        public int EditPermission { get; set; }

        public int AssinedOfficeId { get; set; }
        public int SentFromOfficeId { get; set; }
        public string EditType { get; set; }
        public int MapId { get; set; }
        public int ActId { get; set; }
        public string ActName { get; set; }
        public string SwipeId { get; set; }
        public bool IsSup { get; set; }
        public string isSupFile { get; set; }
        public string IsPartFile { get; set; }

        public string LinkedFromDeptName { get; set; }
        public string eBookFile { get; set; }
        public string PressNoteENG { get; set; }
        public string PressNotePun { get; set; }

        public string TimelineId { get; set; }
        public string TimelineRemarks { get; set; }

        public List<string> DepartmentListIds { get; set; }
        public List<string> ApprovalListIds { get; set; }

        public string AgendaApprove { get; set; }

        public string ProceedingApprove { get; set; }
        public string AgendaPdf { get; set; }
        public string AgendaPdfName { get; set; }
        public string ProceedingPdf { get; set; }
        public string ProceedingCommCommentPdf { get; set; }


       
        //these are the new fields added by joginder in order to get signed pdf's from the database
        public string SignedAgendaPdfPath { get; set; }
        public string SignedProceedingMayorPath { get; set; }
        public string SignedProceedingCommissionerPath { get; set; }


        public string MemberNameLocal { get; set; }
        public string MinisteryName { get; set; }
        public string MinisteryNameLocal { get; set; }

        public string agendadate { get; set; }
        public string agendaaction { get; set; }
        public string filetype { get; set; }
        public List<DiaryViewModel> _LstActions { get; set; }

        public string ResolutioNo { get; set; }

        public Decimal ProposalAmount { get; set; }
        public string ResCount { get; set; }



        public static List<DiaryViewModel> GetProcedingDataForCommPDF(string refid, string actionCode)
        {
            List<DiaryViewModel> lstagd = new List<DiaryViewModel>();
            DataSet ds = new DataSet();
            ds = GetincludeReff(refid, "dept");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DiaryViewModel fr = new DiaryViewModel();

                fr.LetterNo = dr["LetterNo"].ToString();
                fr.SubjectLocal = dr["Subject_local"].ToString();
                fr.Subject = dr["Subject"].ToString();
                fr.SubjectDetails = dr["SubjectDetails"].ToString();
                fr.SubjectDetailsLocal = dr["SubjectDetails_local"].ToString();
                fr.StatusName = dr["StatusName"].ToString();
                fr.LGComments = dr["LGComments"].ToString();
                fr.agendadate = dr["CreatedDate"].ToString();
                fr.agendaaction = dr["ActionTakenDate"].ToString();
  


        lstagd.Add(fr);
            }
            return lstagd;

        }


        public static DataSet GetincludeReff(string refid, string actioncode)
        {

            DataSet ds = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@refid", Convert.ToInt64(refid)),
                    new SqlParameter("@actioncode", actioncode) };
                ds = SqlHelper.ExecuteDataset(connection, "GetIncludedRefPDFF", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }


        public static List<DiaryViewModel> DairyDispatchReport(int OfficeId)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@officeid", OfficeId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "getreportbyloginId", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.Date = Convert.ToString(row["Date"].ToString());
                    model.FromDepartment = Convert.ToString(row["FromDepartment"].ToString());
                    model.officeName = Convert.ToString(row["FromOffice"].ToString());
                    model.subject = Convert.ToString(row["subject"].ToString());
                    model.StatusName = Convert.ToString(row["ActionStatus"].ToString());
                    model.LinkRefID = Convert.ToInt64(row["DispatchRefId"].ToString());
                    model.dispatchDate = Convert.ToString(row["DispatchDate"].ToString());
                    model.sentToDepartment = Convert.ToString(row["ToDepartment"].ToString());
                    model.sentToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.sentActionStatus = Convert.ToString(row["ActionStatus"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

      


        public List<DiaryViewModel> lstDiaryViewModel { get; set; }

        public List<DiaryActionViewModel> lstDiaryActionViewModel { get; set; }
        public List<DiaryActionViewModel> mainfileDiaryActionViewModel { get; set; }
        public List<MultifileJson> MultiFileModel { get; set; }
        public List<LinkedFiles> linkedFilesList { get; set; }
        public static string SaveDiary(DiaryViewModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@RefId", model.RefId),
                    new SqlParameter("@LinkRefID", model.LinkRefID),
                    new SqlParameter("@FromDeptId", model.FromDeptId),
                    new SqlParameter("@FromOfficeId", model.FromOfficeId),
                    new SqlParameter("@DispatchNumber", model.DispatchNumber),
                    new SqlParameter("@ToDeptId", model.ToDeptId),
                    new SqlParameter("@ToOfficeId", model.ToOfficeId),
                    new SqlParameter("@DocmentType", model.DocmentType),
                    new SqlParameter("@LetterNo", model.LetterNo),
                    new SqlParameter("@LetterDate", model.LetterDate),
                    new SqlParameter("@Subject", model.Subject),
                    new SqlParameter("@SubjectDetails", model.SubjectDetails),
                    new SqlParameter("@EnclosureFile", model.ActionTakenFile),
                    new SqlParameter("@ApplicantName", model.ApplicantName),
                    new SqlParameter("@ApplicantAddress", model.ApplicantAddress),
                    new SqlParameter("@ApplicantMobile", model.ApplicantMobile),
                    new SqlParameter("@ApplicantEmail", model.ApplicantEmail),
                    new SqlParameter("@CreatedBy", model.CreatedBy),
                    new SqlParameter("@DiaryDate", model.DiaryDate),
                    new SqlParameter("@ActionCode", model.ActionCode),
                    new SqlParameter("@ActionTakenDate", model.ActionTakenDate),
                    new SqlParameter("@ActionDescription", model.ActionDescription),
                    new SqlParameter("@ActionAmountSanction", model.ActionAmountSanction),
                    new SqlParameter("@ActionTakenFile", model.ActionTakenFile),
                    new SqlParameter("@OfficeId", model.OfficeId),
                    new SqlParameter("@letter", model.isLetter),
                    new SqlParameter("@userid", model.Userid),
                     new SqlParameter("@sentTouserid", model.SentToUserId),
                    new SqlParameter("@MoveId", model.MoveId),
                     new SqlParameter("@InboxType", model.InboxType),
                      new SqlParameter("@SentType", model.SentType),
                        new SqlParameter("@MapId", model.MapId),
                        new SqlParameter("@AssignedOffice", model.AssinedOfficeId),
                         new SqlParameter("@FileDraft", model.FileDraft),
                         new SqlParameter("@SentFromOffice", model.SentFromOfficeId),

                   // new SqlParameter("@ReferencePriorityType", model.ReferencePriorityType),

            };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[MLADiaryInsert]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string UpdateInboxMultiAction(DiaryViewModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@RefId", model.RefId),
                    new SqlParameter("@LinkRefID", model.LinkRefID),
                    new SqlParameter("@FromDeptId", model.FromDeptId),
                    new SqlParameter("@FromOfficeId", model.FromOfficeId),
                    new SqlParameter("@DispatchNumber", model.DispatchNumber),
                    new SqlParameter("@ToDeptId", model.ToDeptId),
                    new SqlParameter("@ToOfficeId", model.ToOfficeId),
                    new SqlParameter("@DocmentType", model.DocmentType),
                    new SqlParameter("@LetterNo", model.LetterNo),
                    new SqlParameter("@LetterDate", model.LetterDate),
                    new SqlParameter("@Subject", model.Subject),
                    new SqlParameter("@SubjectDetails", model.SubjectDetails),
                    new SqlParameter("@EnclosureFile", model.EnclosureFile),
                    new SqlParameter("@ApplicantName", model.ApplicantName),
                    new SqlParameter("@ApplicantAddress", model.ApplicantAddress),
                    new SqlParameter("@ApplicantMobile", model.ApplicantMobile),
                    new SqlParameter("@ApplicantEmail", model.ApplicantEmail),
                    new SqlParameter("@CreatedBy", model.CreatedBy),
                    new SqlParameter("@DiaryDate", model.DiaryDate),
                    new SqlParameter("@ActionCode", model.ActionCode),
                    new SqlParameter("@ActionTakenDate", model.ActionTakenDate),
                    new SqlParameter("@ActionDescription", model.ActionDescription),
                    new SqlParameter("@ActionAmountSanction", model.ActionAmountSanction),
                    new SqlParameter("@ActionTakenFile", model.ActionTakenFile),
                    new SqlParameter("@OfficeId", model.OfficeId),
                    new SqlParameter("@letter", model.isLetter),
                    new SqlParameter("@userid", model.Userid),
                     new SqlParameter("@sentTouserid", model.SentToUserId),
                    new SqlParameter("@MoveId", model.MoveId),
                     new SqlParameter("@InboxType", model.InboxType),
                      new SqlParameter("@SentType", model.SentType),
                        new SqlParameter("@MapId", model.MapId),
                        new SqlParameter("@AssignedOffice", model.AssinedOfficeId),
                         new SqlParameter("@SentFromOffice", model.SentFromOfficeId)
                     

                   // new SqlParameter("@ReferencePriorityType", model.ReferencePriorityType),

            };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InboxMultiUpdate]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string UpdateMultiAction(DiaryViewModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@RefId", model.RefId),
                    new SqlParameter("@LinkRefID", model.LinkRefID),
                    new SqlParameter("@FromDeptId", model.FromDeptId),
                    new SqlParameter("@FromOfficeId", model.FromOfficeId),
                    new SqlParameter("@DispatchNumber", model.DispatchNumber),
                    new SqlParameter("@ToDeptId", model.ToDeptId),
                    new SqlParameter("@ToOfficeId", model.ToOfficeId),
                    new SqlParameter("@DocmentType", model.DocmentType),
                    new SqlParameter("@LetterNo", model.LetterNo),
                    new SqlParameter("@LetterDate", model.LetterDate),
                    new SqlParameter("@Subject", model.Subject),
                    new SqlParameter("@SubjectDetails", model.SubjectDetails),
                    new SqlParameter("@EnclosureFile", model.EnclosureFile),
                    new SqlParameter("@ApplicantName", model.ApplicantName),
                    new SqlParameter("@ApplicantAddress", model.ApplicantAddress),
                    new SqlParameter("@ApplicantMobile", model.ApplicantMobile),
                    new SqlParameter("@ApplicantEmail", model.ApplicantEmail),
                    new SqlParameter("@CreatedBy", model.CreatedBy),
                    new SqlParameter("@DiaryDate", model.DiaryDate),
                    new SqlParameter("@ActionCode", model.ActionCode),
                    new SqlParameter("@ActionTakenDate", model.ActionTakenDate),
                    new SqlParameter("@ActionDescription", model.ActionDescription),
                    new SqlParameter("@ActionDescriptionPB", model.ActionDescriptionPB),
                    new SqlParameter("@ActionAmountSanction", model.ActionAmountSanction),
                    new SqlParameter("@ActionTakenFile", model.ActionTakenFile),
                    new SqlParameter("@OfficeId", model.OfficeId),
                    new SqlParameter("@letter", model.isLetter),
                    new SqlParameter("@userid", model.Userid),
                     new SqlParameter("@sentTouserid", model.SentToUserId),
                    new SqlParameter("@MoveId", model.MoveId),
                     new SqlParameter("@InboxType", model.InboxType),
                      new SqlParameter("@SentType", model.SentType),
                       new SqlParameter("@Draftchk", model.Draftchk)

                   // new SqlParameter("@ReferencePriorityType", model.ReferencePriorityType),

            };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[MLADiaryMultiAction]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }



        public static string UpdateMultiActionBulk(DiaryViewModel diaryModel, string refIds)
        {
            int result = 0;
            // ✅ Clean RefIds (VERY IMPORTANT)
            var cleanedIds = string.Join(",",
                (refIds ?? "")
                    .Split(',')
                    .Where(x => long.TryParse(x.Trim(), out _))
                    .Select(x => x.Trim())
            );
            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand("UpdateMultiActionBulk", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@RefIds", SqlDbType.NVarChar).Value = cleanedIds ?? "";
                        cmd.Parameters.Add("@ActionCode", SqlDbType.Int).Value = diaryModel.ActionCode;
                        cmd.Parameters.Add("@ActionDescription", SqlDbType.NVarChar).Value =
                            (object)diaryModel.ActionDescription ?? DBNull.Value;
                        cmd.Parameters.Add("@ActionDescriptionPB", SqlDbType.NVarChar).Value =
                            (object)diaryModel.ActionDescriptionPB ?? DBNull.Value;
                        cmd.Parameters.Add("@ActionTakenFile", SqlDbType.NVarChar).Value =
                            (object)diaryModel.ActionTakenFile ?? DBNull.Value;
                        cmd.Parameters.Add("@userid", SqlDbType.NVarChar).Value = diaryModel.Userid;
                        cmd.Parameters.Add("@Draftchk", SqlDbType.Bit).Value = diaryModel.Draftchk;

                        // ✅ FIX: Use ExecuteScalar
                        object obj = cmd.ExecuteScalar();

                        if (obj != null && obj != DBNull.Value)
                            result = Convert.ToInt32(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateMultiActionBulk: " + ex.Message);
            }

            return result.ToString();
        }

        public static string UpdateNoting(DiaryViewModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@RefId", model.RefId),
                    new SqlParameter("@LinkRefID", model.LinkRefID),
                    new SqlParameter("@FromDeptId", model.FromDeptId),
                    new SqlParameter("@FromOfficeId", model.FromOfficeId),
                    new SqlParameter("@DispatchNumber", model.DispatchNumber),
                    new SqlParameter("@ToDeptId", model.ToDeptId),
                    new SqlParameter("@ToOfficeId", model.ToOfficeId),
                    new SqlParameter("@DocmentType", model.DocmentType),
                    new SqlParameter("@LetterNo", model.LetterNo),
                    new SqlParameter("@LetterDate", model.LetterDate),
                    new SqlParameter("@Subject", model.Subject),
                    new SqlParameter("@SubjectDetails", model.SubjectDetails),
                    new SqlParameter("@EnclosureFile", model.EnclosureFile),
                    new SqlParameter("@ApplicantName", model.ApplicantName),
                    new SqlParameter("@ApplicantAddress", model.ApplicantAddress),
                    new SqlParameter("@ApplicantMobile", model.ApplicantMobile),
                    new SqlParameter("@ApplicantEmail", model.ApplicantEmail),
                    new SqlParameter("@CreatedBy", model.CreatedBy),
                    new SqlParameter("@DiaryDate", model.DiaryDate),
                    new SqlParameter("@ActionCode", model.ActionCode),
                    new SqlParameter("@ActionTakenDate", model.ActionTakenDate),
                    new SqlParameter("@ActionDescription", model.ActionDescription),
                    new SqlParameter("@ActionAmountSanction", model.ActionAmountSanction),
                    new SqlParameter("@ActionTakenFile", model.ActionTakenFile),
                    new SqlParameter("@OfficeId", model.OfficeId),
                    new SqlParameter("@letter", model.isLetter),
                    new SqlParameter("@userid", model.Userid),
                     new SqlParameter("@sentTouserid", model.SentToUserId),
                    new SqlParameter("@MoveId", model.MoveId),
                     new SqlParameter("@InboxType", model.InboxType),
                      new SqlParameter("@SentType", model.SentType)

                   // new SqlParameter("@ReferencePriorityType", model.ReferencePriorityType),

            };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateNoting]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string UpdateSend(DiaryViewModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@RefId", model.RefId),
                    new SqlParameter("@LinkRefID", model.LinkRefID),
                    new SqlParameter("@FromDeptId", model.FromDeptId),
                    new SqlParameter("@FromOfficeId", model.FromOfficeId),
                    new SqlParameter("@DispatchNumber", model.DispatchNumber),
                    new SqlParameter("@ToDeptId", model.ToDeptId),
                    new SqlParameter("@ToOfficeId", model.ToOfficeId),
                    new SqlParameter("@DocmentType", model.DocmentType),
                    new SqlParameter("@LetterNo", model.LetterNo),
                    new SqlParameter("@LetterDate", model.LetterDate),
                    new SqlParameter("@Subject", model.Subject),
                    new SqlParameter("@SubjectDetails", model.SubjectDetails),
                    new SqlParameter("@EnclosureFile", model.EnclosureFile),
                    new SqlParameter("@ApplicantName", model.ApplicantName),
                    new SqlParameter("@ApplicantAddress", model.ApplicantAddress),
                    new SqlParameter("@ApplicantMobile", model.ApplicantMobile),
                    new SqlParameter("@ApplicantEmail", model.ApplicantEmail),
                    new SqlParameter("@CreatedBy", model.CreatedBy),
                    new SqlParameter("@DiaryDate", model.DiaryDate),
                    new SqlParameter("@ActionCode", model.ActionCode),
                    new SqlParameter("@ActionTakenDate", model.ActionTakenDate),
                    new SqlParameter("@ActionDescription", model.ActionDescription),
                    new SqlParameter("@ActionAmountSanction", model.ActionAmountSanction),
                    new SqlParameter("@ActionTakenFile", model.ActionTakenFile),
                    new SqlParameter("@OfficeId", model.OfficeId),
                    new SqlParameter("@letter", model.isLetter),
                    new SqlParameter("@userid", model.Userid),
                     new SqlParameter("@sentTouserid", model.SentToUserId),
                    new SqlParameter("@MoveId", model.MoveId),
                     new SqlParameter("@InboxType", model.InboxType),
                      new SqlParameter("@SentType", model.SentType)

                   // new SqlParameter("@ReferencePriorityType", model.ReferencePriorityType),

            };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateSend]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string SaveSanction(FundSanctionModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@FinYear", model.FinYear),
                    new SqlParameter("@refid",Convert.ToInt64(model.RefId)),
                    new SqlParameter("@ApprovedAmount", model.ApprovedAmount),
                    new SqlParameter("@AmountSanctioned", model.AmountSanctioned),
                    new SqlParameter("@Approvedate", model.ApproveDate),
                    new SqlParameter("@DeptId", model.deptId),
                    new SqlParameter("@OfficeId", model.officeid),
                    new SqlParameter("@remarks", model.Remarks),
                    new SqlParameter("@createdby", model.Createdby),
                    new SqlParameter("@fundname", model.FundType)
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertSanctionDetails]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string UpdateSanction(FundSanctionModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@AmountSanctioned", model.AmountSanctioned),
                    new SqlParameter("@sanctiondate", model.SanctionDate),
                    new SqlParameter("@fundId", model.id)
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateSanctionDetails]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string InsertConcernedDeptId(Int64 refid, int deptid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@refid", refid),
                    new SqlParameter("@deptid", deptid),
                };
                string res = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertConcernedDeptId]", parameterValues));
                connection.Close();
                return res;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string InsertCMApproval(Int64 refid, int approvalid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@refid", refid),
                    new SqlParameter("@ApprovalId", approvalid),
                };
                string res = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertCmApproval]", parameterValues));
                connection.Close();
                return res;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string InsertMultiFiles(Int64 refid, string filepath, string filename)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@refid", refid),
                    new SqlParameter("@filepath", filepath),
                    new SqlParameter("@filename", filename),
                };
                string res = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertMultiFiles]", parameterValues));
                connection.Close();
                return res;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string SaveDispatch(DiaryViewModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", model.RefId),
                    new SqlParameter("@LinkRefID", model.LinkRefID), new SqlParameter("@FromDeptId", model.FromDeptId),
                    new SqlParameter("@FromOfficeId", model.FromOfficeId),
                    new SqlParameter("@DispatchNumber", model.DispatchNumber),
                    new SqlParameter("@ToDeptId", model.ToDeptId),
                    new SqlParameter("@ToOfficeId", model.ToOfficeId),
                    new SqlParameter("@DocmentType", model.DocmentType),new SqlParameter("@LetterNo", model.LetterNo),
                    new SqlParameter("@LetterDate", model.LetterDate),
                    new SqlParameter("@Subject", model.Subject),new SqlParameter("@SubjectDetails", model.SubjectDetails),
                    new SqlParameter("@EnclosureFile", model.EnclosureFile),
                    new SqlParameter("@StatementFilePb", model.StatementImplementation_local),
                    new SqlParameter("@ApplicantName", model.ApplicantName),new SqlParameter("@ApplicantAddress", model.ApplicantAddress),
                    new SqlParameter("@ApplicantMobile", model.ApplicantMobile),
                    new SqlParameter("@ApplicantEmail", model.ApplicantEmail),new SqlParameter("@CreatedBy", model.CreatedBy),
                    new SqlParameter("@DiaryDate", model.DiaryDate),
                    new SqlParameter("@ActionCode", model.ActionCode),
                    new SqlParameter("@ActionTakenDate", model.ActionTakenDate),
                    new SqlParameter("@ActionDescription", model.ActionDescription),
                    new SqlParameter("@ActionAmountSanction", model.ActionAmountSanction),
                    new SqlParameter("@ActionTakenFile", model.ActionTakenFile),
                    new SqlParameter("@OfficeId", model.OfficeId),
                    new SqlParameter("@letter", model.isLetter),
                    new SqlParameter("@MeetingId", model.MeetingId),
                    new SqlParameter("@userid", model.Userid),
                    new SqlParameter("@sentTouserid", model.SentToUserId),
                    new SqlParameter("@MoveId", model.MoveId),
                    new SqlParameter("@InboxType", model.InboxType),
                    new SqlParameter("@SentType", model.SentType),
                     new SqlParameter("@MapId", model.MapId),
                     new SqlParameter("@ActId", model.ActId),
                     new SqlParameter("@AssemblyCode", model.AssemblyCode),
                     new SqlParameter("@SessionCode", model.SessionCode),
                     new SqlParameter("@MemberCode", model.MemberCode),
                     new SqlParameter("@MinistryID", model.MinistryID),
                      new SqlParameter("@MemberName", model.MemberName),
                     new SqlParameter("@MemberNameLocal", model.MemberNameLocal),
                     new SqlParameter("@MinisteryName", model.MinisteryName),
                     new SqlParameter("@MinisteryNameLocal", model.MinisteryNameLocal),
                      new SqlParameter("@FileDraft", model.FileDraft),
                     new SqlParameter("@Subject_local", model.SubjectPB),new SqlParameter("@SubjectDetails_local", model.SubjectDetailsPB),
                     new SqlParameter("@IsSup", model.IsSup),
                     new SqlParameter("@PressNoteENG", model.PressNoteENG),
                     new SqlParameter("@PressNotePun", model.PressNotePun),
                     new SqlParameter("@TimelineId", model.TimelineId),
                     new SqlParameter("@TimelineRemarks", model.TimelineRemarks),
                     new SqlParameter("@AssignedOffice", model.AssinedOfficeId),
                     new SqlParameter("@SentFromOffice", model.SentFromOfficeId),
                      new SqlParameter("@ProposalAmount", model.ProposalAmount),
                  //  new SqlParameter("@ReferencePriorityType", model.ReferencePriorityType),

                };
                string ret = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[MLADispatchInsert]", parameterValues));
                connection.Close();
                return ret;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return "0";
            }
        }

        public static string SaveDispatchAgenda(DiaryViewModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", model.RefId),
                    new SqlParameter("@LinkRefID", model.LinkRefID), new SqlParameter("@FromDeptId", model.FromDeptId),
                    new SqlParameter("@FromOfficeId", model.FromOfficeId),
                    new SqlParameter("@DispatchNumber", model.DispatchNumber),
                    new SqlParameter("@ToDeptId", model.ToDeptId),
                    new SqlParameter("@ToOfficeId", model.ToOfficeId),
                    new SqlParameter("@DocmentType", model.DocmentType),new SqlParameter("@LetterNo", model.LetterNo),
                    new SqlParameter("@LetterDate", model.LetterDate),
                    new SqlParameter("@Subject", model.Subject),new SqlParameter("@SubjectDetails", model.SubjectDetails),
                    new SqlParameter("@EnclosureFile", model.EnclosureFile),
                    new SqlParameter("@ApplicantName", model.ApplicantName),new SqlParameter("@ApplicantAddress", model.ApplicantAddress),
                    new SqlParameter("@ApplicantMobile", model.ApplicantMobile),
                    new SqlParameter("@ApplicantEmail", model.ApplicantEmail),new SqlParameter("@CreatedBy", model.CreatedBy),
                    new SqlParameter("@DiaryDate", model.DiaryDate),
                    new SqlParameter("@ActionCode", model.ActionCode),
                    new SqlParameter("@ActionTakenDate", model.ActionTakenDate),
                    new SqlParameter("@ActionDescription", model.ActionDescription),
                    new SqlParameter("@ActionAmountSanction", model.ActionAmountSanction),
                    new SqlParameter("@ActionTakenFile", model.ActionTakenFile),
                    new SqlParameter("@OfficeId", model.OfficeId),
                    new SqlParameter("@letter", model.isLetter),
                    new SqlParameter("@userid", model.Userid),
                  //  new SqlParameter("@ReferencePriorityType", model.ReferencePriorityType),

                };
                string ret = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateAgendaAction]", parameterValues));
                connection.Close();
                return ret;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return "0";
            }
        }

        public static string UpdateActionDescription(Int64 Refid,int actionid, string ActionDesc)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                     new SqlParameter("@Refid", Convert.ToInt64 (Refid)),
                      new SqlParameter("@actionid", actionid),
                        new SqlParameter("@ActionDesc", ActionDesc),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateActionDescription]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static List<CMNirdesh.Models.Diaries.TemplateList> getTeamplateList(int OfficeId)
        {
            List<CMNirdesh.Models.Diaries.TemplateList> items = new List<CMNirdesh.Models.Diaries.TemplateList>();

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@OfficeId", OfficeId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getTeamplateList]", parameterValues).Tables[0].Rows)
                {
                    CMNirdesh.Models.Diaries.TemplateList item = new CMNirdesh.Models.Diaries.TemplateList();
                    item.Id = Convert.ToInt32(row["Id"].ToString());
                    item.OfficeId = Convert.ToInt32(row["OfficeId"].ToString());
                    item.SignStamp = Convert.ToString(row["SignStamp"].ToString());
                    item.TemplateName = Convert.ToString(row["TemplateName"].ToString());
                    item.TemplateHead = Convert.ToString(row["TemplateHead"].ToString());
                    item.TemplateSubjectEng = Convert.ToString(row["TemplateSubjectEng"].ToString());
                    item.TemplateSubjectLocal = Convert.ToString(row["TemplateSubjectLocal"].ToString());
                    item.BodyTextEng = Convert.ToString(row["BodyTextEng"].ToString());
                    item.BodyTextLocal = Convert.ToString(row["BodyTextLocal"].ToString());
                    items.Add(item);
                }
                return items;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return items;
            }
        }

        public static SelectList getDepartment()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDepartment]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["deptname"].ToString());
                    item.Value = Convert.ToString(row["deptid"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }
        public static SelectList getRoleWiseDepartment()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DeptID", CurrentSession.DeptID),new SqlParameter("@RoleID", Convert.ToInt32(CurrentSession.RoleID)) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getRoleWiseDepartment]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["deptname"].ToString());
                    item.Value = Convert.ToString(row["deptid"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }
        public static SelectList getuserRoles()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RoleID",Convert.ToInt32(CurrentSession.RoleID)) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetUserRoles]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["UserTypeName"].ToString());
                    item.Value = Convert.ToString(row["UserTypeID"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static List<DepartmentSelectListItem> getDepartmentPbi()
        {
            List<DepartmentSelectListItem> items = new List<DepartmentSelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDepartment]", new object[0]).Tables[0].Rows)
                {
                    DepartmentSelectListItem item = new DepartmentSelectListItem();
                    item.Text = Convert.ToString(row["deptname"].ToString());
                    item.Value = Convert.ToString(row["deptid"].ToString());
                    item.TextPbi = Convert.ToString(row["deptnameLocal"].ToString());
                    item.SecPbi = Convert.ToString(row["deptsec"].ToString());
                    item.FileNo = Convert.ToInt32(row["filenum"].ToString());
                    item.isAdvice = row["isAdvice"].ToString(); 
                    items.Add(item);
                }
                return items;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return items;
            }
        }



        public static DataSet GetDocumentTypeListval(string userid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserID", userid), new SqlParameter("@reftype", 'R') };
                set = SqlHelper.ExecuteDataset(connection, "GetDiaryDocumentTypeList", parameterValues);
            }
            catch (Exception)
            {
            }
            return set;
        }

        public static DataSet Assembly2()
        {
            DataSet set = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserID", UserID) };
                SqlParameter[] parameterValues = new SqlParameter[] { };
                set = SqlHelper.ExecuteDataset(connection, "sp_GetAssemblyDetails", parameterValues);
            }
            catch (Exception)
            {
            }
            return set;
        }




        public static DataSet GetSectordata()
        {
            DataSet set = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserID", UserID) };
                SqlParameter[] parameterValues = new SqlParameter[] { };
                set = SqlHelper.ExecuteDataset(connection, "[sp_GetSectors]", parameterValues);
            }
            catch (Exception)
            {
            }
            return set;
        }



        public static DataSet Session2()
        {
            DataSet set = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserID", UserID) };
                SqlParameter[] parameterValues = new SqlParameter[] { };
                set = SqlHelper.ExecuteDataset(connection, "sp_GetSessionDetails", parameterValues);
            }
            catch (Exception)
            {
            }
            return set;
        }

        public static DataSet SessionDate()
        {
            DataSet set = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserID", UserID) };
                SqlParameter[] parameterValues = new SqlParameter[] { };
                set = SqlHelper.ExecuteDataset(connection, "sp_GetSessionDatesDetails", parameterValues);
            }
            catch (Exception)
            {
            }
            return set;
        }

        public static DataSet SessionByAssemblyId(string assemblyid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AssemblyCode", assemblyid) };
               // SqlParameter[] parameterValues = new SqlParameter[] { };
                set = SqlHelper.ExecuteDataset(connection, "sp_GetAssemblySessionDetails", parameterValues);
            }
            catch (Exception)
            {
            }
            return set;
        }


        public static DataSet SessionByAssemblySessionId(string assemblyid,string sessionid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AssemblyCode", assemblyid) , new SqlParameter("@SessionCode", sessionid) };
              
                set = SqlHelper.ExecuteDataset(connection, "sp_GetAssemblySessionDatesDetails", parameterValues);
            }
            catch (Exception)
            {
            }
            return set;
        }

        


        public static SelectList Assembly()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {  };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "sp_GetAssemblyDetails", parameterValues).Tables[0].Rows)
                {
                   
                    

                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["AssemblyName"].ToString());
                    item.Value = Convert.ToString(row["AssemblyCode"].ToString());
                    items.Add(item);

                   
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList Session()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "sp_GetSessionDetails", parameterValues).Tables[0].Rows)
                {



                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["SessionName"].ToString());
                    item.Value = Convert.ToString(row["SessionCode"].ToString());
                    items.Add(item);


                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList SessionDates()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "GetSessionDates", parameterValues).Tables[0].Rows)
                {



                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["SessionDate"].ToString());
                    item.Value = Convert.ToString(row["Id"].ToString());
                    items.Add(item);


                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList Members()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "sp_GetMembersDetailsByAssemblyId3", parameterValues).Tables[0].Rows)
                {



                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["Name"].ToString());
                    item.Value = Convert.ToString(row["MemberID"].ToString());
                    items.Add(item);


                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }



        public static SelectList Minister()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "sp_GetMinisterDetailsByAssemblyId3", parameterValues).Tables[0].Rows)
                {



                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["Name"].ToString());
                    item.Value = Convert.ToString(row["MemberCode"].ToString());
                    items.Add(item);


                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList GetDocumentTypeList(string userid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDocumentTypeList]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["DocumentTypeName"].ToString());
                    item.Value = Convert.ToString(row["DocmentType"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList GetDiaryFileDocumentTypeList(string userid, string reftype)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid) , new SqlParameter("@reftype", reftype) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryDocumentTypeList]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["DocumentTypeName"].ToString());
                    item.Value = Convert.ToString(row["DocmentType"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList BindAct()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid), new SqlParameter("@reftype", reftype) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getAct]", null).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActName"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static void BindAllDropdowns(MlaDiaryData model, string userId,string DiaryDispatch, string FileLetter,int ActionTypeList, int ActionTypeList_Ref, int AgendaId, string CoverType, Int64 refid)
        {
            try
            {
                string value = ConfigurationManager.AppSettings["StorageType"];
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@userid", userId),
                    new SqlParameter("@DiaryDispatch", DiaryDispatch),
                    new SqlParameter("@FileLetter", FileLetter),
                     new SqlParameter("@ActionTypeList", ActionTypeList),
                      new SqlParameter("@ActionTypeList_Ref", ActionTypeList_Ref),
                      new SqlParameter("@storageType", value),
                       new SqlParameter("@AgendaId", AgendaId),
                        new SqlParameter("@CoverType", CoverType),
                         new SqlParameter("@RefId", refid),
                         new SqlParameter("@FromdeptId", model.FromDeptId),
                         new SqlParameter("@TodeptId", model.ToDeptId)
                         //diaryModel.FromDeptId
                };

                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    DataSet ds = SqlHelper.ExecuteDataset(connection, "[usp_GetAllDropdownData]", parameters);

                    // Table 0: DocumentTypeList
                    model.DocumentTypelist = new SelectList(ds.Tables[0].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["DocumentTypeName"]?.ToString(),
                        Value = row["DocmentType"]?.ToString()
                    }).ToList(), "Value", "Text");

                    // Table 1: ActionTypeList
                    model.ActionTypeList = new SelectList(ds.Tables[1].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["actionnamestatus"]?.ToString(),
                        Value = row["Actioncode"]?.ToString()
                    }).ToList(), "Value", "Text");

                    // Table 2: ActionTypeList_Ref
                    model.ActionTypeList_Ref = new SelectList(ds.Tables[2].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["actionnamestatus"]?.ToString(),
                        Value = row["Actioncode"]?.ToString()
                    }).ToList(), "Value", "Text");

                    // Table 3: Act
                    model.Act = new SelectList(ds.Tables[3].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["ActName"]?.ToString(),
                        Value = row["Id"]?.ToString()
                    }).ToList(), "Value", "Text");
                    model.fileacessingUrl = ds.Tables[4].Rows[0]["SettingValue"].ToString();

                    if(ds.Tables[5].Rows.Count> 0)
                    {
                        var list = new List<CoveringLetter>();
                        foreach (DataRow row in ds.Tables[5].Rows)
                        {
                            list.Add(new CoveringLetter
                            {
                                PdfPath = row["PdfPath"].ToString()
                            });
                        }
                        model._CoveringLetterList = list;
                    }

                    if (model.AgendaApprove == "1" && model.ProceedingApprove == "0")
                    {
                        if (ds.Tables[6].Rows.Count > 0)
                        {
                            int AgendaCount = Convert.ToInt32(ds.Tables[6].Rows[0][0]);
                            int PendingCount = Convert.ToInt32(ds.Tables[6].Rows[0][1]);
                            string EndTime = ds.Tables[6].Rows[0][2].ToString();
                            int AttandanceCount = Convert.ToInt32(ds.Tables[6].Rows[0][3]);

                            if (EndTime == "" || AttandanceCount == 0)
                            {
                                model.MeeingEnd = true;
                                model.ResButton = false;
                            }
                            else
                            {
                                model.MeeingEnd = true;
                                model.ResButton = true;
                            }

                            if (AgendaCount > 0 & PendingCount == 0 && EndTime != "")
                            {
                                model.PassButton = true;
                            }
                            else
                            {
                                model.PassButton = false;
                            }
                        }

                    }


                    model.FromOfficeList = new SelectList(ds.Tables[7].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["officename"]?.ToString(),
                        Value = row["FromOfficeId"]?.ToString()
                    }).ToList(), "Value", "Text");

                    model.ToOfficeList = new SelectList(ds.Tables[8].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["officename"]?.ToString(),
                        Value = row["FromOfficeId"]?.ToString()
                    }).ToList(), "Value", "Text");


                   

                    model.FromUserList = new SelectList(ds.Tables[9].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["UserName"]?.ToString(),
                        Value = row["UserId"]?.ToString()
                    }).ToList(), "Value", "Text");


                    model.ToUserList = new SelectList(ds.Tables[10].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["UserName"]?.ToString(),
                        Value = row["UserId"]?.ToString()
                    }).ToList(), "Value", "Text");


                   



                    List<AgendaModel> lstagd = new List<AgendaModel>();
                    foreach (DataRow dr in ds.Tables[11].Rows)
                    {
                        AgendaModel fr = new AgendaModel();
                        fr.AgendaId = dr["Id"].ToString();
                        fr.AgendaName = dr["AgendaName"].ToString();
                        string agendadate = Convert.ToString(dr["AgendaDate"]);
                        if (agendadate != "")
                        {
                            fr.AgendaDateDisplay = Convert.ToDateTime(dr["AgendaDate"]).ToString("dd/MM/yyyy");
                        }

                        string startTimeValue = Convert.ToString(dr["StartTime"]);
                        TimeSpan startTime;
                        if (TimeSpan.TryParse(startTimeValue, out startTime))
                        {
                            fr.StartTime = Converter.ConvertTime(Convert.ToString(startTime));
                        }
                        ;

                        string endTimeValue = Convert.ToString(dr["EndTime"]);
                        TimeSpan endTime;
                        if (TimeSpan.TryParse(endTimeValue, out endTime))

                        {
                            fr.EndTime = Converter.ConvertTime(Convert.ToString(endTime));
                        }
                        ;
                        lstagd.Add(fr);
                    }

                    model._LstAgenda= lstagd;


                    model.MLAOfficeList = new SelectList(ds.Tables[12].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["officename"]?.ToString(),
                        Value = row["FromOfficeId"]?.ToString()
                    }).ToList(), "Value", "Text");

                    model.MlaUserList = new SelectList(ds.Tables[10].AsEnumerable()
                        .Where(r => r.Field<string>("UserId") == "0")
                        .Select(row => new SelectListItem
                    {
                        Text = row["UserName"]?.ToString(),
                        Value = row["UserId"]?.ToString()
                    }).ToList(), "Value", "Text");


                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }
        }

        public static void LGAllDropdowns(MlaDiaryData model, string userId, string DiaryDispatch, string FileLetter, int ActionTypeList, int ActionTypeList_Ref, int AgendaId, string CoverType, Int64 refid, String SessionOfficeId)
        {
            try
            {
                string value = ConfigurationManager.AppSettings["StorageType"];
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@userid", userId),
                    new SqlParameter("@DiaryDispatch", DiaryDispatch),
                    new SqlParameter("@FileLetter", FileLetter),
                     new SqlParameter("@ActionTypeList", ActionTypeList),
                      new SqlParameter("@ActionTypeList_Ref", ActionTypeList_Ref),
                      new SqlParameter("@storageType", value),
                       new SqlParameter("@AgendaId", AgendaId),
                        new SqlParameter("@CoverType", CoverType),
                         new SqlParameter("@RefId", refid),
                         new SqlParameter("@FromdeptId", model.FromDeptId),
                         new SqlParameter("@TodeptId", model.ToDeptId),
                         new SqlParameter("@SessionOfficeId", SessionOfficeId),
                         
                         //diaryModel.FromDeptId
                };

                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    DataSet ds = SqlHelper.ExecuteDataset(connection, "[LGAllDropdowns]", parameters);

                    // Table 0: DocumentTypeList
                    model.DocumentTypelist = new SelectList(ds.Tables[0].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["DocumentTypeName"]?.ToString(),
                        Value = row["DocmentType"]?.ToString()
                    }).ToList(), "Value", "Text");

                    // Table 1: ActionTypeList
                    model.ActionTypeList = new SelectList(ds.Tables[1].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["actionnamestatus"]?.ToString(),
                        Value = row["Actioncode"]?.ToString()
                    }).ToList(), "Value", "Text");

                    // Table 2: ActionTypeList_Ref
                    model.ActionTypeList_Ref = new SelectList(ds.Tables[2].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["actionnamestatus"]?.ToString(),
                        Value = row["Actioncode"]?.ToString()
                    }).ToList(), "Value", "Text");

                    // Table 3: Act
                    model.Act = new SelectList(ds.Tables[3].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["ActName"]?.ToString(),
                        Value = row["Id"]?.ToString()
                    }).ToList(), "Value", "Text");

                    //Table 4: FileAccessing URL
                    model.fileacessingUrl = ds.Tables[4].Rows[0]["SettingValue"].ToString();

                    //Table 5 ActionListFilter
                    model.ActionListFilter = new SelectList(ds.Tables[5].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = Convert.ToString(row["ActionName"].ToString() + ", (" + row["rc"].ToString() + ")"),
                        Value = row["ActionCode"]?.ToString()
                    }).ToList(), "Value", "Text");


                    //Table 6  ADCOfficeList
                    model.MLAOfficeList = new SelectList(ds.Tables[6].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["officename"]?.ToString(),
                        Value = row["FromOfficeId"]?.ToString()
                    }).ToList(), "Value", "Text");

                    //Table 7  FromOfficeList
                    model.FromOfficeList = new SelectList(ds.Tables[7].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["officename"]?.ToString(),
                        Value = row["FromOfficeId"]?.ToString()
                    }).ToList(), "Value", "Text");

                    //Table 8  ToOfficeList
                    model.ToOfficeList = new SelectList(ds.Tables[8].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["officename"]?.ToString(),
                        Value = row["FromOfficeId"]?.ToString()
                    }).ToList(), "Value", "Text");



                    //Table 9  FromUserList
                    model.FromUserList = new SelectList(ds.Tables[9].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["UserName"]?.ToString(),
                        Value = row["UserId"]?.ToString()
                    }).ToList(), "Value", "Text");

                    //Table 10  ToUserList
                    model.ToUserList = new SelectList(ds.Tables[10].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["UserName"]?.ToString(),
                        Value = row["UserId"]?.ToString()
                    }).ToList(), "Value", "Text");


                    //Table 11  MLAUserList
                    model.MlaUserList = new SelectList(ds.Tables[11].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["UserName"]?.ToString(),
                        Value = row["UserId"]?.ToString()
                    }).ToList(), "Value", "Text");

                    //Table 12  DeptList
                    model.mDepartmentList= new SelectList(ds.Tables[12].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["deptname"]?.ToString(),
                        Value = row["deptId"]?.ToString()
                    }).ToList(), "Value", "Text");

                    ////Table 8  OfficeList
                    model.OfficeList = new SelectList(ds.Tables[8].AsEnumerable().Select(row => new SelectListItem
                    {
                        Text = row["officename"]?.ToString(),
                        Value = row["FromOfficeId"]?.ToString()
                    }).ToList(), "Value", "Text");

                    //Table 13  SessionAction
                    if (ds.Tables[13].Rows.Count > 0)
                    {
                        model.ActionDescription = Convert.ToString(ds.Tables[0].Rows[0]["ActionRemarks"]);
                    }

                    if(ds.Tables.Count > 14 && ds.Tables[14].Rows.Count > 0)
                    {
                        model.CheckedRefs = ds.Tables[14]
                                              .AsEnumerable()
                                              .Select(r => r["ReferenceId"].ToString())
                                              .ToList();
                    }
                   else
                    {
                        model.CheckedRefs = new List<string>();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }
        }


        public static SelectList BindStatus(string userid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryDocumentTypeList]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["DocumentTypeName"].ToString());
                    item.Value = Convert.ToString(row["DocmentType"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public DataTable GetDiaryDocumentTypeList(string userid,string reftype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>(); 
            p.Add(new SqlParameter("@userid", userid));
            p.Add(new SqlParameter("@reftype", reftype));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDocumentTypeList", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDispatchDocumentType(string userid,string reftype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", userid));
            p.Add(new SqlParameter("@reftype", reftype));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetDispatchDocumentTypeList", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public static SelectList GetTimeline()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
               // SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid), new SqlParameter("@reftype", reftype) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetTimeline]", null).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["timeline"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList GetCMApprovalList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                // SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid), new SqlParameter("@reftype", reftype) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetCMApprovalList]", null).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ApprovalType"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList CMApprovedList(Int64 refid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                 SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@refid", refid) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[CMApprovedList]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ApprovalType"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList GetDispatchDocumentTypeList(string userid,string reftype)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid), new SqlParameter("@reftype", reftype) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetFileDocumentTypeList]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["DocumentTypeName"].ToString());
                    item.Value = Convert.ToString(row["DocmentType"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public DataSet FreezeAction(Int32 ActionId, string userid, string actionby)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet ds = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@ActionId", ActionId));
                p.Add(new SqlParameter("@UserId", userid));
                p.Add(new SqlParameter("@Actionby", actionby));
                ds = SqlHelper.ExecuteDataset(connection, "FreezeAction", p.ToArray());
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetDataForFile(string deptId, string deptidsession, string AgendaOfficeId, string offidsession, string Document, string status, string Term, string Priority, string since, string userid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();

            p.Add(new SqlParameter("@DeptID", deptId));
            p.Add(new SqlParameter("@DeptIDsess", deptidsession));
            p.Add(new SqlParameter("@AgendaOfficeId", AgendaOfficeId));
            p.Add(new SqlParameter("@AgendaOfficeIdsess", offidsession));
            p.Add(new SqlParameter("@Document", Document));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@Term", Term));
            p.Add(new SqlParameter("@Priority", Priority));
            p.Add(new SqlParameter("@Since", since));
            p.Add(new SqlParameter("@userid", userid));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetDataForFile", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }




        public static string UpdateRevert(Int64 Refid, int moveid, int officeid, string userid, string createdby)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@RefId", Refid),
                    new SqlParameter("@MoveId", moveid),
                     new SqlParameter("@FromOfficeId", officeid),
                     new SqlParameter("@CreatedBy", createdby),
                    new SqlParameter("@userid", userid),

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[RevertBackInsert]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static SelectList GetMeetingList(string officeid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@OfficeId", officeid) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetAllofficeMeeting]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["AgendaName"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList GetFileList(string userid, string officeid, string refid, string type)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@userid", userid), new SqlParameter("@isLetter", "File"), new SqlParameter("@SessToOfficeId", officeid), new SqlParameter("@Type", type) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[DispatchInbox]", parameterValues).Tables[0].Rows)
                {
                    if (row["RefId"].ToString() == refid)
                    {

                    }
                    else
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = Convert.ToString(row["Subject"].ToString());
                        item.Value = Convert.ToString(row["RefId"].ToString());
                        items.Add(item);
                    }

                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList GetMeetingFileList(string userid, string deptId, int officeid, int Type, Int64 refid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@UserId", userid), new SqlParameter("@deptId", deptId), new SqlParameter("@OfficeId", officeid), new SqlParameter("@Type", Type), new SqlParameter("@Refid", refid) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetMeetingFileList]", parameterValues).Tables[0].Rows)
                {
                        SelectListItem item = new SelectListItem();
                        item.Text = Convert.ToString(row["Subject"].ToString());
                        item.Value = Convert.ToString(row["RefId"].ToString());
                        items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList GetFromUserlist(long refid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@refid", Convert.ToInt64(refid)) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetFromUserlist]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["UserName"].ToString());
                    item.Value = Convert.ToString(row["UserId"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList GetToUserlist(long refid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@refid", Convert.ToInt64(refid)) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetToUserlist]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["UserName"].ToString());
                    item.Value = Convert.ToString(row["UserId"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }
        public static SelectList GetPriorityTypeList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetReferencePriorityTypeList]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["PriorityTypeName"].ToString());
                    item.Value = Convert.ToString(row["PriorityTypeId"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList ActionListFilter(string userid, string Type, int isFile, Int64 refId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@Type", Type),
                     new SqlParameter("@IsFile", isFile),
                      new SqlParameter("@refid", refId)
                    };

                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetActionFilter]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ActionName"].ToString() + ", (" +row["rc"].ToString()+ ")");
                    item.Value = Convert.ToString(row["ActionCode"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }


        public static SelectList GetAllActionTypeList(Int64 ID, string userid, string Type, int isFile)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@ID", ID),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@Type", Type),
                     new SqlParameter("@IsFile", isFile)
                    };

                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetAllActionTypeList]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["actionnamestatus"].ToString());
                    item.Value = Convert.ToString(row["Actioncode"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public DataTable GetAllActionTypeListnew(int officeid, string startDt, string endDt, string since, string userid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeID", officeid));
            p.Add(new SqlParameter("@startDt", startDt));
            p.Add(new SqlParameter("@endDt", endDt));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@userid", userid));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetAllActionTypeListnew", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAllActionTypeListMla(int officeid, string since, string userid,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeID", officeid));
            p.Add(new SqlParameter("@userid", userid));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@IsLetter", reffiletype));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetAllActionTypeListMla", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable GetAllActionTypeListMlaD(int officeid, string since, string userid,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeID", officeid));
            p.Add(new SqlParameter("@userid", userid));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@IsLetter", reffiletype));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetAllActionTypeListMlaD", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAllActionTypeListnewType(int officeid, string startDt, string endDt, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeID", officeid));
            p.Add(new SqlParameter("@startDt", startDt));
            p.Add(new SqlParameter("@endDt", endDt));
            p.Add(new SqlParameter("@since", since));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetAllActionTypeListDashboardTypeWise", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAllActionTypeListDispatch(int officeid, string startDt, string endDt, string since, string userid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeID", officeid));
            p.Add(new SqlParameter("@startDt", startDt));
            p.Add(new SqlParameter("@endDt", endDt));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@userid", userid));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetAllActionTypeListDispatch", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAllActionTypeListDispatchType(int officeid, string startDt, string endDt, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeID", officeid));
            p.Add(new SqlParameter("@startDt", startDt));
            p.Add(new SqlParameter("@endDt", endDt));
            p.Add(new SqlParameter("@since", since));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetAllActionTypeListDispatchTypeWise", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public static SelectList GetMinisterByDept(string deptId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", deptId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetMinisterByName]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["officename"].ToString());
                    item.Value = Convert.ToString(row["FromOfficeId"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public static SelectList GetOfficeByDept(string deptId, string OfficeId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                    
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", deptId), new SqlParameter("@OfficeId", OfficeId), };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetOfficeByDept]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["officename"].ToString());
                    item.Value = Convert.ToString(row["FromOfficeId"].ToString());
                    
                    items.Add(item);
                }
                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = Convert.ToString(row["officename"].ToString());
                        item.Value = Convert.ToString(row["FromOfficeId"].ToString());
                        items.Add(item);
                    }
                }
               
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }
        public static SelectList GetFromOfficeByUser(string deptId, string UserId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", deptId), new SqlParameter("@userId", UserId), };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetFromOfficeByUser]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["name"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }


                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }
        public static SelectList GetOfficeByUser(string deptId, string UserId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", deptId), new SqlParameter("@userId", UserId), };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetOfficeByUser]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["name"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());
                    items.Add(item);
                }
               

                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }
        public static DataSet GetDataByDocType(int OfficeId, string userid,string IsLetter)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[]
                {
                    new SqlParameter("@OfficeId", OfficeId),
                    new SqlParameter("@userid", userid),
                     new SqlParameter("@IsLetter", IsLetter),
                };
                ds = SqlHelper.ExecuteDataset(connection, "[GetDataByDocType]", parameterValues);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }
            return ds;
        }


     








        public static DataSet GetDataByDocTypeDispatch(int OfficeId, string userid, string IsLetter)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] 
                { 
                    new SqlParameter("@OfficeId", OfficeId), 
                    new SqlParameter("@userid", userid) ,
                     new SqlParameter("@IsLetter", IsLetter),
                };
                
                ds = SqlHelper.ExecuteDataset(connection, "[GetDataByDocTypeDispatch]", parameterValues);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }
            return ds;
        }

        public static List<DiaryViewModel> GetDiaryByFilter(string officeid, string documentType)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@officeid", Convert.ToInt32(officeid)), new SqlParameter("@documentType", Convert.ToInt32(documentType)) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryByFilter]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

                return lst;
            }
        }

        public static List<DiaryViewModel> GetDiaryByFiltersearchByRefid(string officeid, Int64 refid)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@officeid", Convert.ToInt32(officeid)), new SqlParameter("@refid", Convert.ToInt64(refid)) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryByFilterSearchByReferenceId]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

                return lst;
            }
        }

        public static List<DiaryViewModel> GetDiaryByDashboard(string deptId, int OfficeId, int documentType, int status, string userid, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", Convert.ToString(deptId)), new SqlParameter("@documentType", Convert.ToInt32(documentType)), new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@status", status), new SqlParameter("@userid", userid), new SqlParameter("@startDt", startDt), new SqlParameter("@endDt", endDt), new SqlParameter("@since", since) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryByDashboardnew]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.EditPermission = Convert.ToInt16(row["Editpermission"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    //model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    //model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    //model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    //model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetDiaryByDashboardType(string deptId, int OfficeId, int documentType, int status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", Convert.ToString(deptId)), new SqlParameter("@documentType", Convert.ToInt32(documentType)), new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@status", status), new SqlParameter("@startDt", startDt), new SqlParameter("@endDt", endDt), new SqlParameter("@since", since) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryByDashboardType]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    //model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    //model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    //model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    //model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> FilteredMLADiaryListForOfc(string deptId, int OfficeId, int documentType, int status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", Convert.ToString(deptId)), new SqlParameter("@documentType", Convert.ToInt32(documentType)), new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@status", status), new SqlParameter("@startDt", startDt), new SqlParameter("@endDt", endDt), new SqlParameter("@since", since) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[FilteredMLADiaryListForOfc]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> DiaryDataByOfficeandDeptTypedashboard(string deptId, int OfficeId, int documentType, int status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", Convert.ToString(deptId)), new SqlParameter("@documentType", Convert.ToInt32(documentType)), new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@status", status), new SqlParameter("@startDt", startDt), new SqlParameter("@endDt", endDt), new SqlParameter("@since", since) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "DiaryDataByOfficeandDeptTypedashboard", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetAllDiaryDataByStatus(int OfficeId, string status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@status", status), new SqlParameter("@startDt", startDt), new SqlParameter("@endDt", endDt), new SqlParameter("@since", since) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetAllDiaryDataByStatus]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetAllDispatchDataByStatus(int OfficeId, string status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@status", status), new SqlParameter("@startDt", startDt), new SqlParameter("@endDt", endDt), new SqlParameter("@since", since) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetAllDispatchDataByStatus]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetDiaryDataByDate(int OfficeId, string DiaryDate)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DiaryDate", DiaryDate), new SqlParameter("@officeid", OfficeId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryDataByDate]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    model.ActionBy = Convert.ToString(row["actionby"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetDiaryActionDataByDate(int OfficeId, string DiaryDate)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DiaryDate", DiaryDate), new SqlParameter("@officeid", OfficeId) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDiaryActionDataByDate]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    model.ActionBy = Convert.ToString(row["actionby"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetDisptachDataByDate(int OfficeId, string DispatchDate, string userid)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DispatchDate", DispatchDate), new SqlParameter("@officeid", OfficeId)
                , new SqlParameter("@userid", userid)};
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDispatchDataByDate]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    model.ActionBy = Convert.ToString(row["actionby"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetActionDisptachDataByDate(int OfficeId, string DispatchDate, string userid)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DispatchDate", DispatchDate), new SqlParameter("@officeid", OfficeId)
                , new SqlParameter("@userid", userid)};
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetActionDispatchDataByDate]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    model.ActionBy = Convert.ToString(row["actionby"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public DataTable getPrintData(Int64 RefId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", Convert.ToInt64(RefId)) };
            return SqlHelper.ExecuteDataset(connection, "[generatetemplate]", parameterValues).Tables[0];

        }

        public DataTable getActionData(Int64 RefId, int status, int actionid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", Convert.ToInt64(RefId)), new SqlParameter("@status", status), new SqlParameter("@actionid", actionid) };
            return SqlHelper.ExecuteDataset(connection, "[generafundtetemplate]", parameterValues).Tables[0];

        }

        public static List<DiaryViewModel> GetDiaryDispatchByDashboard(string deptId, int OfficeId, int documentType, int status, string userid, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@deptId", Convert.ToString(deptId)),
                    new SqlParameter("@documentType", Convert.ToInt32(documentType)),
                    new SqlParameter("@OfficeId", OfficeId) ,
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@startDt", startDt =="" ? null: startDt),
                    new SqlParameter("@endDt", endDt == "" ? null : endDt),
                    new SqlParameter("@since", since == null ? "" : since),
                };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDispatchByDashboardnew]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.EditPermission = Convert.ToInt16(row["Editpermission"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetDiaryDispatchByDashboardType(string deptId, int OfficeId, int documentType, int status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@deptId", Convert.ToString(deptId)),
                    new SqlParameter("@documentType", Convert.ToInt32(documentType)),
                    new SqlParameter("@OfficeId", OfficeId) ,
                    new SqlParameter("@status", status),
                    new SqlParameter("@startDt", startDt =="" ? null: startDt),
                    new SqlParameter("@endDt", endDt == "" ? null : endDt),
                    new SqlParameter("@since", since == null ? "" : since),
                };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDispatchByDashboardType]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> FilteredMLADispatchListForDeptByOfc(string deptId, int OfficeId, int documentType, int status, string startDt, string endDt, string since)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", Convert.ToString(deptId)), new SqlParameter("@documentType", Convert.ToInt32(documentType)), new SqlParameter("@OfficeId", OfficeId) ,
                 new SqlParameter("@status", status),
                  new SqlParameter("@startDt", startDt),
                    new SqlParameter("@endDt", endDt),
                     new SqlParameter("@since", since)
                };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[FilteredMLADispatchListForDeptByOfc]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt16(row["ReferenceTerm"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetDiaryByFilterDispatch(string officeid, string documentType,string reffiletype)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] 
                { 
                    new SqlParameter("@officeid", Convert.ToInt32(officeid)),
                    new SqlParameter("@documentType", Convert.ToInt32(documentType)),
                    new SqlParameter("@IsLetter", Convert.ToString(reffiletype))
                };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDispatchByFilter]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.BriefLastAction = Convert.ToString(row["ad"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> GetSearchByRefidDispatch(string officeid, Int64 refid)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@officeid", Convert.ToInt32(officeid)), new SqlParameter("@refid", Convert.ToInt64(refid)) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDispatchByRefid]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> AdvanceSearchForDispatch(string advanceparam, int officeid)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@advanceparam", advanceparam), new SqlParameter("@officeid", Convert.ToInt32(officeid)) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[advanceSearchForDispatch]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst;
            }
        }

        public static List<DiaryViewModel> AdvanceSearchForDiary(string advanceparam, int officeid)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@advanceparam", advanceparam), new SqlParameter("@officeid", Convert.ToInt32(officeid)) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[advanceSearchForDiary]", parameterValues).Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                    model.ActionTakenFile = Convert.ToString(row["ActiontakenFile"].ToString());
                    model.StatusName = Convert.ToString(row["StatusName"].ToString());
                    model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

                return lst;
            }
        }

        public  DataTable GetActionDetails(Int64 RefId, string DeptId)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", RefId), new SqlParameter("@DeptId", DeptId) };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDiaryById]", parameterValues);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    return ds.Tables[1];
                }
                else
                {
                    return null;

                }
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public static DiaryViewModel GetDiaryById(Int64 RefId, string DeptId)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", RefId), new SqlParameter("@DeptId", DeptId) };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDiaryById]", parameterValues);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DiaryViewModel model = new DiaryViewModel();
                        List<DiaryActionViewModel> lstAction = new List<DiaryActionViewModel>();
                        List<DiaryActionViewModel> mainfileAction = new List<DiaryActionViewModel>();
                        model.RefId = Convert.ToInt64(row["RefId"].ToString());
                        model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                        model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                        model.FromDept = Convert.ToString(row["FromDept"].ToString());
                        model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                        model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                        model.ToDept = Convert.ToString(row["ToDept"].ToString());
                        model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                        model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                        model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                        model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                        model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                        model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                        model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                        model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                        model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                        model.Subject = Convert.ToString(row["Subject"].ToString());
                        model.SubjectDetails = Convert.ToString(row["newsubjectDetails"].ToString());
                        //model.SubjectPB = Convert.ToString(row["Subject_local"].ToString());
                        //model.SubjectDetailsPB = Convert.ToString(row["SubjectDetails_local"].ToString());
                        model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                        //model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                        //model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                       // model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                        //model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                        model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                        //model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                        //model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                        model.ActionCode = row.Field<int?>("ActionCode") ?? 0;
                        model.ActionDescription = Convert.ToString(row["ActionRemarks"].ToString());
                        model.isLetter = Convert.ToString(row["isletter"].ToString());
                        model.agendaId = Convert.ToString(row["AgendaId"].ToString());
                        model.agendaDate = Convert.ToString(row["AgendaDate"].ToString());
                        //model.ReferencePriorityType = Convert.ToInt32(row["ReferencePriorityType"].ToString());
                        //model.ReferenceTerm = Convert.ToInt32(row["ReferenceTerm"].ToString());
                        model.CurrentDate = Convert.ToDateTime(row["crntDt"]).ToString("dd/MM/yyyy");
                        model.ActionTakenDate = Convert.ToDateTime(row["crntDt"]);
                        model.ResolutioNo = row["ResolutionNo"].ToString();
                        //model.MemberName = row["MemberName"].ToString();
                        //model.MinisteryName = row["MinisteryName"].ToString();
                        model.ResCount = row["RefCount"].ToString();
                        model.filetype= row["Filetype"].ToString();
                        model.LinkedFromDeptName = row["LinkedFromDeptName"].ToString();
                        var isfunddd = row["isfund"].ToString();
                        model.IsSignedApprovalPDf = Convert.ToInt16(row["IsSignedApprovalPDf"]);
                        model.IsApprovalPDf = Convert.ToInt16(row["IsApprovalPDf"]);
                        model.ActName = Convert.ToString(row["ActName"].ToString());
                        model.ActId= row.Field<int?>("ActId") ?? 0; 
                        //model.PressNoteENG = row["PressNote"].ToString();
                        //model.PressNotePun = row["PressNotePB"].ToString();
                        //model.StatementImplementation_local = row["StatementFilePb"].ToString();
                        model.ProceedingPdf = Convert.ToString(row["HouseDecisionPdf"].ToString());
                        model.ProceedingCommCommentPdf = Convert.ToString(row["commisisionerRpt"].ToString());
                        model.SignedProceedingCommissionerPath = Convert.ToString(row["SignedProceedingCommissionerPath"].ToString());
                        if (isfunddd == null || isfunddd == "")
                        {
                            isfunddd = "0";
                            model.isfund = Convert.ToInt32(isfunddd);
                        }
                        else
                        {
                            model.isfund = Convert.ToInt32(isfunddd);
                        }

                        foreach (DataRow row1 in ds.Tables[1].Rows)
                        {
                            DiaryActionViewModel diaryActionViewModel = new DiaryActionViewModel();
                            diaryActionViewModel.Id = Convert.ToInt32(row1["id"].ToString());
                            diaryActionViewModel.ActionDescription = Convert.ToString(row1["ActionDescription"].ToString());
                            diaryActionViewModel.ActionAmount = string.IsNullOrEmpty(row1["ActionAmountSanction"].ToString()) ? 0 : Convert.ToDouble(row1["ActionAmountSanction"].ToString());
                            diaryActionViewModel.Enclosure = Convert.ToString(row1["Enclosure"].ToString());
                            diaryActionViewModel.ActionBy = Convert.ToString(row1["ActionBy"].ToString());
                            //diaryActionViewModel.ActionCode = Convert.ToInt32(row1["ActionId"].ToString());
                            //diaryActionViewModel.ActionCode = int.TryParse(row1["ActionId"]?.ToString(), out int actionCode) && actionCode != 0 ? actionCode: (int?)null;
                            diaryActionViewModel.ActionCode = int.TryParse(row1["ActionId"]?.ToString(), out int actionCode)  ? actionCode : 0;

                            diaryActionViewModel.ActionDate = string.IsNullOrEmpty(row1["createdDate"].ToString()) ? DateTime.Now : Convert.ToDateTime(row1["createdDate"].ToString());
                            diaryActionViewModel.StatusName = Convert.ToString(row1["actionname"].ToString());
                            diaryActionViewModel.NoteId = Convert.ToInt32(row1["NoteId"].ToString());
                            diaryActionViewModel.Freezed = string.IsNullOrEmpty(row1["Freezed"].ToString()) ? 0 : Convert.ToInt32(row1["Freezed"]);
                            diaryActionViewModel.isDraft = row1["ActionisDraft"].ToString();
                            lstAction.Add(diaryActionViewModel);
                        }
                        List<MultifileJson> lstmultifile = new List<MultifileJson>();
                        foreach (DataRow row1 in ds.Tables[2].Rows)
                        {
                            MultifileJson multifile = new MultifileJson();
                            multifile.FileName = row1["FilePath"].ToString();
                            multifile.name = row1["ABC"].ToString();
                            multifile.fileid = row1["id"].ToString();
                            multifile.isDeleted = row1["FileName"].ToString();
                            lstmultifile.Add(multifile);
                        }

                        List<LinkedFiles> linklist = new List<LinkedFiles>();
                        foreach (DataRow row1 in ds.Tables[3].Rows)
                        {
                            LinkedFiles linkfile = new LinkedFiles();
                            linkfile.RefId = Convert.ToInt64(row1["linklist"].ToString());
                            linklist.Add(linkfile);
                        }

                        if (model.filetype == "2")
                        {
                            foreach (DataRow row1 in ds.Tables[4].Rows)
                            {
                                DiaryActionViewModel diaryActionViewModel = new DiaryActionViewModel();
                                diaryActionViewModel.Id = Convert.ToInt32(row1["id"].ToString());
                                diaryActionViewModel.ActionDescription = Convert.ToString(row1["ActionDescription"].ToString());
                                diaryActionViewModel.ActionAmount = string.IsNullOrEmpty(row1["ActionAmountSanction"].ToString()) ? 0 : Convert.ToDouble(row1["ActionAmountSanction"].ToString());
                                diaryActionViewModel.Enclosure = Convert.ToString(row1["Enclosure"].ToString());
                                diaryActionViewModel.ActionBy = Convert.ToString(row1["ActionBy"].ToString());
                                diaryActionViewModel.ActionCode = Convert.ToInt32(row1["ActionId"].ToString());
                                diaryActionViewModel.ActionDate = string.IsNullOrEmpty(row1["createdDate"].ToString()) ? DateTime.Now : Convert.ToDateTime(row1["createdDate"].ToString());
                                diaryActionViewModel.StatusName = Convert.ToString(row1["actionname"].ToString());
                                diaryActionViewModel.NoteId = Convert.ToInt32(row1["NoteId"].ToString());
                                diaryActionViewModel.Freezed = string.IsNullOrEmpty(row1["Freezed"].ToString()) ? 0 : Convert.ToInt32(row1["Freezed"]);
                                diaryActionViewModel.isDraft = row1["ActionisDraft"].ToString();
                                mainfileAction.Add(diaryActionViewModel);
                            }
                            model.mainfileDiaryActionViewModel = mainfileAction;
                        }


                        model.MultiFileModel = lstmultifile;
                        model.linkedFilesList = linklist;
                        model.lstDiaryActionViewModel = lstAction;
                        
                        lst.Add(model);
                    }
                    return lst[0];
                }
                else
                {
                    return null;

                }
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        


              public static string Updatenoting(Int64 refid, Int64 LinkedRefid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@LinkedRefid", Convert.ToInt64(LinkedRefid)));
               
                dtt = SqlHelper.ExecuteDataset(connection, "UpdatelastdiaryMovement", p.ToArray());
                if (dtt.Tables[0].Rows.Count > 0)
                {
                    return dtt.Tables[0].Rows[0][0].ToString();
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public static string SetRead(int MoveId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@MoveId", MoveId));
                dtt = SqlHelper.ExecuteDataset(connection, "SetRead", p.ToArray());
                if (dtt.Tables[0].Rows.Count > 0)
                {
                    return dtt.Tables[0].Rows[0][0].ToString();
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public static string RevertBack(int MoveId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@MoveId", MoveId));
                dtt = SqlHelper.ExecuteDataset(connection, "RevertBack", p.ToArray());
                if (dtt.Tables[0].Rows.Count > 0)
                {
                    return dtt.Tables[0].Rows[0][0].ToString();
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public static string UpdateDiaryMovement(Int64 refid, Int64 LinkedRefid, string createdby, int FileType, int CopyAllReferences,int ShowType)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@LinkedRefid", Convert.ToInt64(LinkedRefid)));
                p.Add(new SqlParameter("@Createdby", createdby));
                p.Add(new SqlParameter("@FileType", FileType));
                p.Add(new SqlParameter("@CopyAllReferences", CopyAllReferences));
                p.Add(new SqlParameter("@ShowType", ShowType));
                
                dtt = SqlHelper.ExecuteDataset(connection, "UpdateDiaryMovement", p.ToArray());
                if (dtt.Tables[0].Rows.Count > 0)
                {
                    return dtt.Tables[0].Rows[0][0].ToString();
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        public static string SaveProceedingFile(Int64 refid, Int64 MainRefid, Int64 NewRefid, string createdby, int FileType, int CopyAllReferences, int ShowType)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@Mainrefid", Convert.ToInt64(MainRefid)));
                p.Add(new SqlParameter("@NewRefid", Convert.ToInt64(NewRefid)));
                p.Add(new SqlParameter("@Createdby", createdby));
                p.Add(new SqlParameter("@FileType", FileType));
                p.Add(new SqlParameter("@CopyAllReferences", CopyAllReferences));
                p.Add(new SqlParameter("@ShowType", ShowType));

                dtt = SqlHelper.ExecuteDataset(connection, "SaveProceedingFile", p.ToArray());
                if (dtt.Tables[0].Rows.Count > 0)
                {
                    return dtt.Tables[0].Rows[0][0].ToString();
                }
                else
                    return "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public static DiaryViewModel GetDispatchById(Int64 RefId, string DeptId)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", Convert.ToInt64(RefId)), new SqlParameter("@DeptId", DeptId) };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDispatchById]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    List<DiaryActionViewModel> lstAction = new List<DiaryActionViewModel>();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.FromDeptLocal= Convert.ToString(row["FromDeptLocal"].ToString());
                    model.CurrentDate = Convert.ToDateTime(row["crntDt"]).ToString("dd/MM/yyyy");
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    //model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    //model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    //model.SubjectPB = Convert.ToString(row["Subject_local"].ToString());
                    //model.SubjectDetailsPB = Convert.ToString(row["SubjectDetails_local"].ToString());
                    //model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    //model.ReferencePriorityType = Convert.ToInt32(row["ReferencePriorityType"].ToString());
                    //model.ReferenceTerm = Convert.ToInt32(row["ReferenceTerm"].ToString());
                    //model.ReferenceTerm = Convert.ToInt32(row["ReferenceTerm"].ToString());
                    //model.isSupFile = row["IsSup"].ToString();

                    //commented by Rajeev Sir
                    //model.StatementImplementation_local = Convert.ToString(row["StatementFilePb"].ToString());
                    //model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    //model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    //model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    //model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    //commented by Rajeev Sir
                    //model.PressNoteENG = row["PressNote"].ToString();
                    //model.PressNotePun = row["PressNotePB"].ToString();

                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.ActionCode = row.Field<int?>("ActionCode") ?? 0;
                    model.isLetter = Convert.ToString(row["isletter"].ToString());
                    model.agendaId = Convert.ToString(row["Agendaid"].ToString());
                    model.ActName = Convert.ToString(row["ActName"].ToString());
                    model.AgendaApprove = Convert.ToString(row["isapprove"].ToString());
                    model.ProceedingApprove = Convert.ToString(row["isPassed"].ToString());
                    model.AgendaPdf = Convert.ToString(row["filepath"].ToString());
                    model.ProceedingPdf = Convert.ToString(row["filepath2"].ToString());
                    model.ProceedingCommCommentPdf = Convert.ToString(row["commisisionerRpt"].ToString());

                    model.SignedAgendaPdfPath = Convert.ToString(row["SignedAgendaPdfPath"].ToString());
                    model.SignedProceedingCommissionerPath = Convert.ToString(row["SignedProceedingCommissionerPath"].ToString());
                    model.SignedProceedingMayorPath = Convert.ToString(row["SignedProceedingMayorPath"].ToString());

                    //model.ResCount = row["RefCount"].ToString();
                    model.IsPartFile = row["Filetype"].ToString();
                    model.ProposalAmount = Convert.ToDecimal(row["ProposalAmount"].ToString());
                    model.eBookFile = row["IsLink_eBook"].ToString();
                    model.filetype = row["Filetype"].ToString();
                    model.LinkedFromDeptName = row["LinkedFromDeptName"].ToString();
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        model.LinkList = ds.Tables[3].Rows[0]["linklist"].ToString();
                    }
                    else
                    {
                        model.LinkList = "";
                    }

                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        model.MergeLinkList = ds.Tables[4].Rows[0]["linklist"].ToString();
                    }
                    else
                    {
                        model.MergeLinkList = "";
                    }
                    foreach (DataRow row1 in ds.Tables[1].Rows)
                    {
                        DiaryActionViewModel diaryActionViewModel = new DiaryActionViewModel();
                        diaryActionViewModel.Id = Convert.ToInt32(row1["id"].ToString());
                       // diaryActionViewModel.RefId = Convert.ToInt64(row1["RefId"].ToString());
                       // diaryActionViewModel.MergeRefId = row1["DiaryRefId"].ToString();
                        //if(diaryActionViewModel.RefId.ToString() == diaryActionViewModel.MergeRefId)
                        //{
                        //    diaryActionViewModel.MergeRefId = "";
                        //}
                        diaryActionViewModel.ActionDescription = Convert.ToString(row1["ActionDescription"].ToString());
                        //diaryActionViewModel.ActionAmount = string.IsNullOrEmpty(row1["ActionAmountSanction"].ToString()) ? 0 : Convert.ToDouble(row1["ActionAmountSanction"].ToString());
                        diaryActionViewModel.Enclosure = Convert.ToString(row1["Enclosure"].ToString());
                        diaryActionViewModel.ActionBy = Convert.ToString(row1["ActionBy"].ToString());
                        diaryActionViewModel.ActionCode = Convert.ToInt32(row1["ActionId"].ToString());
                        diaryActionViewModel.ActionDate = string.IsNullOrEmpty(row1["createdDate"].ToString()) ? DateTime.Now : Convert.ToDateTime(row1["createdDate"].ToString());
                        diaryActionViewModel.StatusName = Convert.ToString(row1["actionname"].ToString());
                        diaryActionViewModel.NoteId = Convert.ToInt32(row1["NoteId"].ToString());
                        diaryActionViewModel.isDraft = row1["isDraft"].ToString();
                        lstAction.Add(diaryActionViewModel);
                    }
                   
                    List<MultifileJson> lstmultifile = new List<MultifileJson>();
                    foreach (DataRow row1 in ds.Tables[2].Rows)
                    {
                        MultifileJson multifile = new MultifileJson();
                        multifile.FileName = row1["FilePath"].ToString();
                        multifile.name = row1["ABC"].ToString();
                        multifile.isDeleted = row1["isDeleted"].ToString();
                        multifile.fileid= row1["id"].ToString();
                        lstmultifile.Add(multifile);
                    }

                    model.lstDiaryActionViewModel = lstAction;
                    model.MultiFileModel = lstmultifile;
                    lst.Add(model);
                }
                return lst[0];
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst[0];
            }
        }
        public static string MovementTransfer(Int64 Refid, int moveid,  string OldUserId, string NewUserid, String CreatedBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@MoveId", moveid),
                    new SqlParameter("@refid", Refid),
                    new SqlParameter("@OldUserId", OldUserId),
                    new SqlParameter("@NewUserId", NewUserid),
                       new SqlParameter("@CreatedBy", CreatedBy),

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[MovementTransfer]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string InsertNotification(Int64 Refid, int moveid, int type, string Notification, string sentTouserid, int ActionId, string FromUserId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@RefId", Refid),
                    new SqlParameter("@MoveId", moveid),
                     new SqlParameter("@type", type),
                     new SqlParameter("@Notification", Notification),
                    new SqlParameter("@sentTouserid", sentTouserid),
                    new SqlParameter("@ActionId", ActionId),
                       new SqlParameter("@FromUserId", FromUserId),

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertNotification]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static DataSet GetMovementbyRefid(Int64 refid, Int64 filerefid)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[]
                {
                    new SqlParameter("@RefId", refid),
                      new SqlParameter("@FileRefId", filerefid),
                };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMovementbyRefid]", parameterValues);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }
            return ds;
        }


        public static string GetActionSession(Int64 RefId, string userid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@refId", RefId),
                    new SqlParameter("@userid", userid)
                                  

            };
                string ad = ""; 
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetActionSession]", parameterValues);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ad = Convert.ToString(ds.Tables[0].Rows[0]["ActionRemarks"]);
                }
                connection.Close();
                return ad;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string GetSentByUserId(Int64 RefId, string userid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@refId", RefId),
                    new SqlParameter("@userid", userid)


            };
                string ad = "";
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetSentByUserId]", parameterValues);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ad = Convert.ToString(ds.Tables[0].Rows[0]["FromUserId"])+"~"+Convert.ToString(ds.Tables[0].Rows[0]["FromOfficeId"]);
                }
                connection.Close();
                return ad;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static DiaryViewModel GetDiaryBySearchId(Int64 RefId, int OfficeId)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", Convert.ToInt64(RefId)) };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDiaryById]", parameterValues);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DiaryViewModel model = new DiaryViewModel();
                        List<DiaryActionViewModel> lstAction = new List<DiaryActionViewModel>();
                        model.RefId = Convert.ToInt64(row["RefId"].ToString());
                        model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                        model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                        model.FromDept = Convert.ToString(row["FromDept"].ToString());
                        model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                        model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                        model.ToDept = Convert.ToString(row["ToDept"].ToString());
                        model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                        model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                        model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                        model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                        model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                        model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                        model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                        model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                        model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                        model.Subject = Convert.ToString(row["Subject"].ToString());
                        model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                        model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                        model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                        model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                        model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                        model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                        model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                        model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                        model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                        model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                        model.ProposalAmount = Convert.ToInt64(row["ProposalAmount"].ToString());
                        foreach (DataRow row1 in ds.Tables[1].Rows)
                        {
                            DiaryActionViewModel diaryActionViewModel = new DiaryActionViewModel();
                            diaryActionViewModel.Id = Convert.ToInt32(row1["id"].ToString());
                            diaryActionViewModel.ActionDescription = Convert.ToString(row1["ActionDescription"].ToString());
                            diaryActionViewModel.ActionAmount = string.IsNullOrEmpty(row1["ActionAmountSanction"].ToString()) ? 0 : Convert.ToDouble(row1["ActionAmountSanction"].ToString());
                            diaryActionViewModel.Enclosure = Convert.ToString(row1["Enclosure"].ToString());
                            diaryActionViewModel.ActionBy = Convert.ToString(row1["ActionBy"].ToString());
                            diaryActionViewModel.ActionCode = Convert.ToInt32(row1["ActionId"].ToString());
                            diaryActionViewModel.ActionDate = string.IsNullOrEmpty(row1["createdDate"].ToString()) ? DateTime.Now : Convert.ToDateTime(row1["createdDate"].ToString());
                            diaryActionViewModel.StatusName = Convert.ToString(row1["actionname"].ToString());
                            lstAction.Add(diaryActionViewModel);
                        }
                        
                        model.lstDiaryActionViewModel = lstAction;
                        
                        lst.Add(model);
                    }
                    return lst[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public static DiaryViewModel GetDiaryByLinkRefId(Int64 RefId)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", Convert.ToInt64(RefId)) };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDiaryByLinkRefId]", parameterValues);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DiaryViewModel model = new DiaryViewModel();
                        List<DiaryActionViewModel> lstAction = new List<DiaryActionViewModel>();
                        model.RefId = Convert.ToInt64(row["RefId"].ToString());
                        model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                        model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                        model.FromDept = Convert.ToString(row["FromDept"].ToString());
                        model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                        model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                        model.ToDept = Convert.ToString(row["ToDept"].ToString());
                        model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                        model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                        model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                        model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                        model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                        model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                        model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                        model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                        model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                        model.Subject = Convert.ToString(row["Subject"].ToString());
                        model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                        model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                        model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                        model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                        model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                        model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                        model.ReferencePriorityType = Convert.ToInt16(row["ReferencePriorityType"].ToString());
                        model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                        model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                        model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                        model.ActionTakenDate = string.IsNullOrEmpty(row["ActiontakenDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["ActiontakenDate"].ToString());
                        model.CurrentDate = Convert.ToDateTime(row["crntDt"]).ToString("dd/MM/yyyy");

                        foreach (DataRow row1 in ds.Tables[1].Rows)
                        {
                            DiaryActionViewModel diaryActionViewModel = new DiaryActionViewModel();
                            diaryActionViewModel.Id = Convert.ToInt32(row1["id"].ToString());
                            diaryActionViewModel.ActionDescription = Convert.ToString(row1["ActionDescription"].ToString());
                            diaryActionViewModel.ActionAmount = string.IsNullOrEmpty(row1["ActionAmountSanction"].ToString()) ? 0 : Convert.ToDouble(row1["ActionAmountSanction"].ToString());
                            diaryActionViewModel.Enclosure = Convert.ToString(row1["Enclosure"].ToString());
                            diaryActionViewModel.ActionBy = Convert.ToString(row1["ActionBy"].ToString());
                            diaryActionViewModel.ActionCode = Convert.ToInt32(row1["ActionId"].ToString());
                            diaryActionViewModel.ActionDate = string.IsNullOrEmpty(row1["createdDate"].ToString()) ? DateTime.Now : Convert.ToDateTime(row1["createdDate"].ToString());
                            diaryActionViewModel.StatusName = Convert.ToString(row1["actionname"].ToString());
                            lstAction.Add(diaryActionViewModel);
                        }
                        model.lstDiaryActionViewModel = lstAction;
                        lst.Add(model);
                    }
                    return lst[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }



        public DataTable getDashboardDataByDeptType(string deptId, int status, int offcieId, string startDt, string endDt, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptId", deptId));
            p.Add(new SqlParameter("@status", Convert.ToInt32(status)));
            p.Add(new SqlParameter("@offcieId", offcieId));
            p.Add(new SqlParameter("@startDt", startDt == "" ? null : startDt));
            p.Add(new SqlParameter("@endDt", endDt == "" ? null : endDt));
            p.Add(new SqlParameter("@since", since));
            var dtt = SqlHelper.ExecuteDataset(connection, "DiaryDashbordBindByDeptType", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDocumentType(string userid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", userid));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDocumentNameandIdforEditAction", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAgendaDate(Int32 OfficeId, string type)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@OfficeId", OfficeId));
            p.Add(new SqlParameter("@AgendaType", type));
            var dtt = SqlHelper.ExecuteDataset(connection, "getAgendaList", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable GetLinkedReferenceId(string refid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@refid", refid));
            var dtt = SqlHelper.ExecuteDataset(connection, "getlinkedrefid", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAgendaDepartment(Int32 OfficeId, string type, string userid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@OfficeId", OfficeId));
            p.Add(new SqlParameter("@AgendaType", type));
            p.Add(new SqlParameter("@userid", userid));
            var dtt = SqlHelper.ExecuteDataset(connection, "getagendaDepartment", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable GetDiaryDispatchDepartment(string deptid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@Deptid", deptid));
            var dtt = SqlHelper.ExecuteDataset(connection, "getdiarydispatchDepartment", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAgendaStatus(Int32 OfficeId, string userid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@OfficeId", OfficeId));
            p.Add(new SqlParameter("@userid", userid));
            var dtt = SqlHelper.ExecuteDataset(connection, "getagendaStatus", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAgendaDocument(Int32 OfficeId, string userid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@OfficeId", OfficeId));
            p.Add(new SqlParameter("@userid", userid));
            var dtt = SqlHelper.ExecuteDataset(connection, "getagendaDocument", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable GetDocumentTypeD(string userid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", userid));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDocumentNameandIdforEditActionD", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDepartments()
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDepartments");
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable GetSwitchUser(string userId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", userId));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetSwitchUser", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDepartmentsbyUser(string userId )
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", userId));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDepartmentsbyUser", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDataForDiary(int officeid, string documentType, string status, string userid, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@documentType", Convert.ToInt32(documentType)));
            p.Add(new SqlParameter("@status", Convert.ToInt32(status)));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDiaryByFilter", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDataForDiaryStatus(int officeid, string documentType, string status, string userid, string since, string deptid, string term, string priority)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@documentType", Convert.ToInt32(documentType)));
            p.Add(new SqlParameter("@status", Convert.ToInt32(status)));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDiaryByFilterStatus", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDataForDiaryStatusType(string officeid, string documentType, string status, string userid, string since, string deptid, string term, string priority,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@documentType", documentType));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDiaryByFilterStatusType", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDataForDiaryStatusTypeFilter(string officeid, string officeidsession, string documentType, string status, string userid, string since, string deptid, string deptidsession, string term, string priority,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsession", officeidsession));
            p.Add(new SqlParameter("@documentType", documentType));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsession", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDiaryByFilterStatusTypeFilter", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable GetDataForDispatchStatusType(string officeid, string documentType, string status, string userid, string since, string deptid, string term, string priority,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@documentType", documentType));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDispatchByFilterStatusType", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }




        public DataTable GetDataForDispatchStatusTypeFilter(string officeid, string officeidsession, string documentType, string status, string userid, string since, string deptid, string deptidsession, string term, string priority,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsession", officeidsession));
            p.Add(new SqlParameter("@documentType", documentType));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsession", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDispatchByFilterStatusTypeFilter", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDashboardDataForDispatchStatusTypeCount(string officeid, string officeidsession, string status, string userid, string since, string deptid, string deptidsession, string term, string priority,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "DashboardDataForDispatchStatusTypeCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetInboxMeetingFiles(string userid,int MeetingId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", userid));
            p.Add(new SqlParameter("@MeetingId", MeetingId));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetInboxMeetingFiles", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDashboardDataForDispatchTypeCount(string officeid, string officeidsession, string status, string userid, string since,string documentype, string deptid, string deptidsession, string term, string priority,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@documentype", documentype));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "DashboardDataForDispatchTypeCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable GetIconCountDispatch(string deptid, string deptidsession, string officeid, string officeidsession, string userid,string type, string status,   string term, string priority,string since, string param,string reffiletype )
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@documentype", type));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@param", param));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            
            var dtt = SqlHelper.ExecuteDataset(connection, "GetIconCountData", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetIconCountDiary(string deptid, string deptidsession, string officeid, string officeidsession, string userid, string type, string status, string term, string priority, string since, string param,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@documentype", type));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@param", param));
            p.Add(new SqlParameter("@isLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetIconCountDataDiary", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDispatchOfficeCount(string deptid, string deptidsession, string officeid, string officeidsession, string userid, string type, string status, string term, string priority, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@documentype", type));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDispatchOfficeCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDispatchTypeCount(string deptid, string deptidsession, string officeid, string officeidsession, string userid, string type, string status, string term, string priority, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@documentype", type));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDispatchTypeCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDispatchStatusCount(string deptid, string deptidsession, string officeid, string officeidsession, string userid, string type, string status, string term, string priority, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@documentype", type));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDispatchStatusCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDashboardDataForDispatchStatusTotalCount(string deptid, string deptidsession, string officeid, string officeidsession, string documentype, string status, string term, string priority,string since, string userid,string reffiletype )
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@documentype", documentype));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "DashboardDataForDispatchStatusTotalCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDashboardDataForDiaryStatusTotalCount(string deptid, string deptidsession, string officeid, string officeidsession, string documentype, string status, string term, string priority, string since, string userid,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@documentype", documentype));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "DashboardDataForDiaryStatusTotalCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDashboardDataForDispatchTypeTotalCount(string deptid, string deptidsession, string officeid, string officeidsession, string documentype, string status, string term, string priority, string since, string userid,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@documentype", documentype));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@IsLette", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "DashboardDataForDispatchTypeTotalCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDashboardDataForDiaryTypeTotalCount(string deptid, string deptidsession, string officeid, string officeidsession, string documentype, string status, string term, string priority, string since, string userid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@documentype", documentype));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            var dtt = SqlHelper.ExecuteDataset(connection, "DashboardDataForDiaryTypeTotalCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }



        public DataTable GetDashboardDataForDiaryStatusTypeCount(string officeid, string officeidsession, string status, string userid, string since, string deptid, string deptidsession, string term, string priority, string documentype,string reffiletype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority)); 
            p.Add(new SqlParameter("@documentype", documentype));
            p.Add(new SqlParameter("@IsLetter", reffiletype));
            var dtt = SqlHelper.ExecuteDataset(connection, "DashboardDataForDiaryStatusTypeCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDashboardDataForDiaryOfficeCount(string officeid, string officeidsession, string status, string userid, string since, string deptid, string deptidsession, string term, string priority, string documentype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@documentype", documentype));
            var dtt = SqlHelper.ExecuteDataset(connection, "DashboardDataForDiaryStatusTypeCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDashboardDataForDiaryTypeCount(string officeid, string officeidsession, string status, string userid, string since, string documentype, string deptid, string deptidsession, string term, string priority)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@documentype", documentype));
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            var dtt = SqlHelper.ExecuteDataset(connection, "DashboardDataForDiaryTypeCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }





        // public DataTable GetDataForAgenda( string AgendaId,string type,string status,  string UserID, string since,  string Term, string Priority , string deptId)
        public DataTable GetDataForAgenda(string FileRefid, string deptId, string deptidsession, string AgendaOfficeId, string offidsession, string Document, string status, string Term, string Priority, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();

            p.Add(new SqlParameter("@DeptID", deptId));
            p.Add(new SqlParameter("@DeptIDsess", deptidsession));
            p.Add(new SqlParameter("@AgendaOfficeId", AgendaOfficeId));
            p.Add(new SqlParameter("@AgendaOfficeIdsess", offidsession));
            p.Add(new SqlParameter("@Document", Document));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@Term", Term));
            p.Add(new SqlParameter("@Priority", Priority));
            p.Add(new SqlParameter("@Since", since));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetDataForAgenda", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDataForFile(string deptId, string deptidsession, string AgendaOfficeId, string offidsession, string Document, string status, string Term, string Priority, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();

            p.Add(new SqlParameter("@DeptID", deptId));
            p.Add(new SqlParameter("@DeptIDsess", deptidsession));
            p.Add(new SqlParameter("@AgendaOfficeId", AgendaOfficeId));
            p.Add(new SqlParameter("@AgendaOfficeIdsess", offidsession));
            p.Add(new SqlParameter("@Document", Document));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@Term", Term));
            p.Add(new SqlParameter("@Priority", Priority));
            p.Add(new SqlParameter("@Since", since));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetDataForFile", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable GetDataForMeeting(int MeetingId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();

            p.Add(new SqlParameter("@MeetingId", MeetingId));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDataForMeeting", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDataFromFile(Int64 Refid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();

            p.Add(new SqlParameter("@RefId", Refid));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDataFromFile", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public static DataTable GetFileRefId(Int64 refid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@RefId", Convert.ToInt64(refid)));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetFileRefId", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public static DataTable GetFileReferences(Int64 refid, string userId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@RefId", Convert.ToInt64(refid)));
            p.Add(new SqlParameter("@Userid", userId));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetFileReferences", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable GetFileRef(Int64 refid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetFileRef", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];  
            return dt;
        }

        public DataTable GetIncludedRefPDF(Int64 refid, string viewtype, string DeptId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
            p.Add(new SqlParameter("@viewtype", viewtype));
            p.Add(new SqlParameter("@DeptId", DeptId));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetIncludedRefPDF", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetIncludedAllActions(Int64 refid, string viewtype, string DeptId, string UserId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
            p.Add(new SqlParameter("@viewtype", viewtype));
            p.Add(new SqlParameter("@DeptId", DeptId));
            p.Add(new SqlParameter("@UserId", UserId));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetIncludedAllActions", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable GetIncludedRefActions(Int64 refid, string viewtype, string DeptId, string UserId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
            p.Add(new SqlParameter("@viewtype", viewtype));
            p.Add(new SqlParameter("@DeptId", DeptId));
            p.Add(new SqlParameter("@UserId", UserId));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetIncludedRefActions", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetIncludedRef(Int64 refid, string viewtype, string DeptId, string UserId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
            p.Add(new SqlParameter("@viewtype", viewtype));
            p.Add(new SqlParameter("@DeptId", DeptId));
            p.Add(new SqlParameter("@UserId", UserId));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetIncludedRef", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable GetIncludedRefNew(Int64 refid, string viewtype, string DeptId, string UserId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
            p.Add(new SqlParameter("@viewtype", viewtype));
            p.Add(new SqlParameter("@DeptId", DeptId));
            p.Add(new SqlParameter("@UserId", UserId));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetIncludedRefNew", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable GetDecisionList(int AgendaId, string viewtype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@AgendaId", AgendaId));
            p.Add(new SqlParameter("@viewtype", viewtype));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetDecisionList", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable UpdateAnex(int fileid, string isDeleted , string filepath)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@fileid", fileid));
            p.Add(new SqlParameter("@isDeleted", isDeleted));
            p.Add(new SqlParameter("@filePath", filepath));
            var dtt = SqlHelper.ExecuteDataset(connection, "UpdateAnex", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable UpdateIsSup(int isSup, Int64 refid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@isSup", isSup));
            p.Add(new SqlParameter("@Refid", refid));
            
            var dtt = SqlHelper.ExecuteDataset(connection, "UpdateIsSup", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable DeleteIncludedRef(int lrid, int agendaid, Int64 refid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@lrid", lrid));
            p.Add(new SqlParameter("@agendaid", agendaid));
            p.Add(new SqlParameter("@refid", refid));
            var dtt = SqlHelper.ExecuteDataset(connection, "DeleteIncludedRef", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable GetDataForAgendaFile(string FileRefid,string userid, int AgendaId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();

            p.Add(new SqlParameter("@userid", userid));
            p.Add(new SqlParameter("@AgendaId", AgendaId));
            p.Add(new SqlParameter("@FileRefId", Convert.ToInt64(FileRefid)));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDataForAgendaFile", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDataForAgendaDiary(string FileRefid, string deptId, string deptidsession, string AgendaOfficeId, string offidsession, string Document, string status, string Term, string Priority, string since, string userid, int AgendaId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();

            p.Add(new SqlParameter("@DeptID", deptId));
            p.Add(new SqlParameter("@DeptIDsess", deptidsession));
            p.Add(new SqlParameter("@AgendaOfficeId", AgendaOfficeId));
            p.Add(new SqlParameter("@AgendaOfficeIdsess", offidsession));
            p.Add(new SqlParameter("@Document", Document));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@Term", Term));
            p.Add(new SqlParameter("@Priority", Priority));
            p.Add(new SqlParameter("@Since", since));
            p.Add(new SqlParameter("@userid", userid));
            p.Add(new SqlParameter("@AgendaId", AgendaId));
            if (FileRefid == "0")
            {
                p.Add(new SqlParameter("@FileRefId", DBNull.Value));
            }
            else if (FileRefid == "")
            {
                p.Add(new SqlParameter("@FileRefId", DBNull.Value));
            }
            else
            {
                p.Add(new SqlParameter("@FileRefId", Convert.ToInt64(FileRefid)));
            }
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDataForAgendaDiary", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDataForDispatch(int officeid, string documentType, string status, string userid, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@documentType", Convert.ToInt32(documentType)));
            p.Add(new SqlParameter("@status", Convert.ToInt32(status)));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDispatchByFilter", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetDataCountByIdForDispatch(int id, int OfficeId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@OfficeId", OfficeId));
            p.Add(new SqlParameter("@Id", id));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDataByDocTypeDispatchById", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public  DataTable UserInbox(string userid, string isLetter)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@isLetter", isLetter));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetUserInbox", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable SubmenuInboxCount(int type, string userid, int ToOfficeId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@type", type));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@SessToOfficeId", ToOfficeId));
            
           
            var dtt = SqlHelper.ExecuteDataset(connection, "GetSubMenuCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable DispatchInboxOutboxCount(string userid, int AssignedOfficeId, int ToOfficeId, string Reftype, string Filetype)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@AssignedOfficeId", AssignedOfficeId));
            p.Add(new SqlParameter("@SessToOfficeId", ToOfficeId));
            p.Add(new SqlParameter("@RefType", Reftype));
            p.Add(new SqlParameter("@FileType", Filetype));
            var dtt = SqlHelper.ExecuteDataset(connection, "InboxOutboxCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable DispatchInbox(string userid, string isLetter, int AssignedOfficeId, int ToOfficeId, string type)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@isLetter", isLetter));
            p.Add(new SqlParameter("@AssignedOfficeId", AssignedOfficeId));
            p.Add(new SqlParameter("@SessToOfficeId", ToOfficeId));
            p.Add(new SqlParameter("@Type", type));
            var dtt = SqlHelper.ExecuteDataset(connection, "DispatchInbox", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable DispatchInboxFilter(string userid, string isLetter, int AssignedOfficeId, int ToOfficeId, string type, string DeptId, DateTime? MeetingDate = null, string ResolutionNo= null)
        {
            //string ResolutionNo = "";
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@isLetter", isLetter));
            p.Add(new SqlParameter("@AssignedOfficeId", AssignedOfficeId));
            p.Add(new SqlParameter("@SessToOfficeId", ToOfficeId));
            p.Add(new SqlParameter("@Type", type));
            p.Add(new SqlParameter("@DeptID", string.IsNullOrWhiteSpace(DeptId) ? (object)DBNull.Value : DeptId));
            p.Add(new SqlParameter("@ResolutionNo", string.IsNullOrWhiteSpace(ResolutionNo) ? (object)DBNull.Value : ResolutionNo));
            p.Add(new SqlParameter("@MeetingDate", MeetingDate.HasValue ? (object)MeetingDate.Value : DBNull.Value));

            var dtt = SqlHelper.ExecuteDataset(connection, "DispatchInboxFilter", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable DispatchOutbox(string userid, string isLetter,int AssignedOfficeId, int ToOfficeId, string type)
        {
           

            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@isLetter", isLetter));
            p.Add(new SqlParameter("@AssignedOfficeId", AssignedOfficeId));
            p.Add(new SqlParameter("@SessToOfficeId", ToOfficeId));
            p.Add(new SqlParameter("@Type", type));
          
            var dtt = SqlHelper.ExecuteDataset(connection, "DispatchOutbox", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable DispatchOutboxNew(string userid, string isLetter, int AssignedOfficeId, int ToOfficeId, string type, DateTime fromDate, DateTime toDate)
        {


            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@isLetter", isLetter));
            p.Add(new SqlParameter("@AssignedOfficeId", AssignedOfficeId));
            p.Add(new SqlParameter("@SessToOfficeId", ToOfficeId));
            p.Add(new SqlParameter("@Type", type));
            // ✅ NEW PARAMETERS
            p.Add(new SqlParameter("@fromDate", fromDate));
            p.Add(new SqlParameter("@toDate", toDate));

            var dtt = SqlHelper.ExecuteDataset(connection, "DispatchOutboxNew", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable DispatchArchive(string userid, string isLetter, string type)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@isLetter", isLetter));
            p.Add(new SqlParameter("@Type", type));
            var dtt = SqlHelper.ExecuteDataset(connection, "DispatchArchive", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable PartFileList(Int64 mainrefid)
        {


            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@refid", mainrefid));
            var dtt = SqlHelper.ExecuteDataset(connection, "PartFileList", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public static string UpdateArchive(int archid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@archid", archid),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateArchive]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string UpdateMerge(Int64 MainRefid, Int64 MergeRefId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@MainRefid", MainRefid),
                    new SqlParameter("@MergeRefId", MergeRefId),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateMerge]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        //public static DataSet GetDataCountByIdForDispatch(int id, int OfficeId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlConnection connection = ClsConnection.GetConnection();
        //        if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
        //        {
        //            connection.Open();
        //        }
        //        connection.Close();
        //        SqlParameter[] parameterValues = new SqlParameter[] {  new SqlParameter("@OfficeId", OfficeId),
        //            new SqlParameter("@id", id) };
        //        ds = SqlHelper.ExecuteDataset(connection, "GetDataByDocTypeDispatchById", parameterValues);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
        //    }
        //    return ds;
        //}

        public DataTable GetOfficebyDepartmentId(string deptid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetOfficeByDepartments", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable GetOfficebyUser(string deptid, string userId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@userid", userId));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetOfficeByUser", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetMappedOfficebyUser(string deptid, string userId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", deptid));
            p.Add(new SqlParameter("@userid", userId));
            var dtt = SqlHelper.ExecuteDataset(connection, "[GetMappedOfficebyUser]", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataSet GetOfficeDetails(int officeid, string UserId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@officeId", officeid));
            p.Add(new SqlParameter("@UserId", UserId));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetOfficeDetails", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataSet ds = dtt;
            return ds;
        }

        public string GetDocumentName(string DocumentType)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                ;
                p.Add(new SqlParameter("@documentid", Convert.ToInt32(DocumentType)));
                dtt = SqlHelper.ExecuteDataset(connection, "GetDocumentName", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["DocumentTypeName"].ToString();

                return keyMsg.ToString();

            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        public static string InsertActionSession(Int64 refid,  string userid ,string ActionRemarks,int actioncode)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@userid", userid));
                p.Add(new SqlParameter("@actioncode", actioncode));
                p.Add(new SqlParameter("@actionRemarks", ActionRemarks));
                dtt = SqlHelper.ExecuteDataset(connection, "InsertActionSession", p.ToArray());
                return "1";
                connection.Close();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        public static string InsertActionSessionRes(string refid,string userid,string ActionRemarks,int actioncode)
        {
            try
            {
                SqlConnection connection =ClsConnection.GetConnection();

                if (connection.State == ConnectionState.Closed ||
                    connection.State == ConnectionState.Broken)
                {
                    connection.Open();
                }

                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("@refid",refid));

                p.Add(new SqlParameter("@userid",userid));

                p.Add(new SqlParameter("@actioncode",actioncode));

                p.Add(new SqlParameter("@actionRemarks",ActionRemarks));

                SqlHelper.ExecuteNonQuery(connection,CommandType.StoredProcedure,"sp_SaveActionSession",p.ToArray());

                connection.Close();

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string UpdateDiaryDocument(Int64 refid, string documentid, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@documentid", Convert.ToInt32(documentid)));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "UpdateDiaryDocument", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        public string UpdateDiaryFromOffice(Int64 refid, string officeId, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@officeId", Convert.ToInt32(officeId)));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "UpdateDiaryFromOffice", p.ToArray());

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public string UpdateDiaryToOffice(Int64 refid, string officeId, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@officeId", Convert.ToInt32(officeId)));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "UpdateDiaryToOffice", p.ToArray());

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public string loginPageCheckredirection(string userid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@userid", userid));
                dtt = SqlHelper.ExecuteDataset(connection, "loginPageCheckRedirection", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();

                return keyMsg;

            }
            catch (Exception ex)
            {
                return "Error";
            }
        }




        public string UpdatePriorityType(Int64 refid, int PriorityType, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@PriorityType", Convert.ToInt16(PriorityType)));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "UpdateReferencePriority", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }



        public string updatesubject(Int64 refid, string subject, string subjectold, string subject_local, string subjectold_local, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@subject", subject));
                p.Add(new SqlParameter("@subjectold", subjectold));
                p.Add(new SqlParameter("@subject_local", subject_local));
                p.Add(new SqlParameter("@subjectold_local", subjectold_local));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "UpdateSubject", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public string updateamount(Int64 refid, decimal amount, decimal amountold, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@amount", amount));
                p.Add(new SqlParameter("@amountold", amountold));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "UpdateAmount", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }




        public string updatesubjectdetail(Int64 refid,  string subjectdetail, string subjectdetail_local ,string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
               // p.Add(new SqlParameter("@subject", subject));
                p.Add(new SqlParameter("@subjectdetail", subjectdetail));
                p.Add(new SqlParameter("@subjectdetail_local", subjectdetail_local));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "UpdateSubjectdetail", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        public string UpdateTerm(Int64 refid, int Term, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@Term", Convert.ToInt16(Term)));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "UpdateReferenceTerm", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        public string FreezeAction( Int32 ActionId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@ActionId", ActionId));
                dtt = SqlHelper.ExecuteDataset(connection, "FreezeAction", p.ToArray());
                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        public string AddMeetingFile(Int64 refid, Int32 agendaid, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@agendaid", agendaid));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "AddToAgenda", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        //AddToAgenda
        public string AddToAgenda(Int64 refid, Int32 agendaid, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@agendaid", agendaid));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "AddToAgenda", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

 

        public string CheckInAgenda(Int64 refid, Int32 agendaid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@agendaid", agendaid));
                dtt = SqlHelper.ExecuteDataset(connection, "CheckInAgenda", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public string FileAgendaApprove(Int64 refid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                dtt = SqlHelper.ExecuteDataset(connection, "FileAgendaApprove", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["isapprove"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string GetMainFile(Int64 refid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                dtt = SqlHelper.ExecuteDataset(connection, "GetMainFile", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["LinkRefID"].ToString();
                if (keyMsg =="0")
                {
                    return "0";
                }
                else
                {
                    return keyMsg;
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string CheckInFile(Int64 refid, Int64 mainRefid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@mainRefid", Convert.ToInt64(mainRefid)));
                dtt = SqlHelper.ExecuteDataset(connection, "CheckInFile", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public string AddToPartFile(Int64 refid, Int64 mainRefid, Int64 newRefid, int MeetingId, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@mainRefid", Convert.ToInt64(mainRefid)));
                p.Add(new SqlParameter("@newRefid", Convert.ToInt64(newRefid)));
                p.Add(new SqlParameter("@MeetingId", MeetingId));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "AddToPartFile", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string AddToFile(Int64 refid, Int64 mainRefid, int MeetingId, string createdBy)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();

                DataSet dtt = new DataSet();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@refid", Convert.ToInt64(refid)));
                p.Add(new SqlParameter("@mainRefid", Convert.ToInt64(mainRefid)));
                p.Add(new SqlParameter("@MeetingId", MeetingId));
                p.Add(new SqlParameter("@createdBy", createdBy));
                dtt = SqlHelper.ExecuteDataset(connection, "AddToFile", p.ToArray());

                var keyMsg = dtt.Tables[0].Rows[0]["Msg"].ToString();
                if (keyMsg.Contains("1"))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public DataTable GetDispatchDataByDeptnew(string deptId, int status, int offcieId, string startDt, string endDt, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptId", deptId));
            p.Add(new SqlParameter("@status", Convert.ToInt32(status)));
            p.Add(new SqlParameter("@offcieId", offcieId));
            p.Add(new SqlParameter("@startDt", startDt == "" ? null : startDt));
            p.Add(new SqlParameter("@endDt", endDt == "" ? null : endDt));
            p.Add(new SqlParameter("@since", since));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDispatchDataByDeptnew1", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable GetAgendaType(string AgendaId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@AgendaId", AgendaId));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetAgendatype", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetResAndRemarks(Int64 Refid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@Refid", Refid));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetResAndRemarks", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable GetResAndRemarksbyIds(List<string> ids, long fileRefId)
        {
            // 1. Create TVP DataTable
            DataTable dtIds = new DataTable();
            dtIds.Columns.Add("Id", typeof(long));

            foreach (var id in ids)
                dtIds.Rows.Add(Convert.ToInt64(id));

            using (SqlConnection connection = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetResAndRemarksByIds", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 2. TVP parameter
                    SqlParameter tvpParam = cmd.Parameters.AddWithValue("@Refids", dtIds);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.RefIdListType";

                    // 3. OPTIONAL: Only if SP has @FileRefid
                    cmd.Parameters.AddWithValue("@FileRefid", fileRefId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable result = new DataTable();
                    da.Fill(result);
                    return result;
                }
            }
        }

        //public DataTable GetResAndRemarksbyIds(List<string> ids,Int64 FileRefid)
        //{
        //    DataTable dtIds = new DataTable();
        //    dtIds.Columns.Add("Id", typeof(long));

        //    foreach (var id in ids)
        //        dtIds.Rows.Add(Convert.ToInt64(id));

        //    SqlConnection connection = ClsConnection.GetConnection();
        //    if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
        //    {
        //        connection.Open();
        //    }
        //    connection.Close();

        //    List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        //    var p = new List<SqlParameter>();

        //    p.Add(new SqlParameter("@FileRefid", FileRefid));
        //    p.Add(new SqlParameter("@Refids", dtIds));
        //    var dtt = SqlHelper.ExecuteDataset(connection, "GetResAndRemarksByIds", p.ToArray());
        //    if (dtt.Tables.Count == 0)
        //    {
        //        return null;
        //    }
        //    DataTable dt = dtt.Tables[0];
        //    return dt;
        //}



        public DataTable GetDispatchDataByDeptType(string deptId, int status, int offcieId, string startDt, string endDt, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptId", deptId));
            p.Add(new SqlParameter("@status", Convert.ToInt32(status)));
            p.Add(new SqlParameter("@offcieId", offcieId));
            p.Add(new SqlParameter("@startDt", startDt == "" ? null : startDt));
            p.Add(new SqlParameter("@endDt", endDt == "" ? null : endDt));
            p.Add(new SqlParameter("@since", since));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetDispatchDataByDeptType", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public List<Dictionary<string, object>> GetTableRows(DataTable dtData)
        {
            List<Dictionary<string, object>>
            lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in dtData.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in dtData.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            return lstRows;
        }
        public static List<DiaryViewModel> GetDispatchDetailsById(Int64 RefId)
        {
            List<DiaryViewModel> items = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", Convert.ToInt64(RefId)) };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDispatchById]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DiaryViewModel item = new DiaryViewModel();
                    item.RefId = Convert.ToInt64(row["RefId"].ToString());
                    item.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    item.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    item.FromDept = Convert.ToString(row["FromDept"].ToString());
                    item.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    item.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    item.ToDept = Convert.ToString(row["ToDept"].ToString());
                    item.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    item.CurrentDate = Convert.ToDateTime(row["crntDt"]).ToString("dd/MM/yyyy");
                    item.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    item.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    item.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    item.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    item.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    item.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    item.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    item.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    item.Subject = Convert.ToString(row["Subject"].ToString());
                    item.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    item.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    item.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    item.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    item.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    item.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    item.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    item.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    item.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    item.ReferencePriorityType = Convert.ToInt32(row["ReferencePriorityType"].ToString());
                    item.ReferenceTerm = Convert.ToInt32(row["ReferenceTerm"].ToString());
                    item.isLetter = Convert.ToString(row["isletter"].ToString());
                    items.Add(item);
                }
                return items;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return items;
            }
        }
        public static List<DiaryActionViewModel> GetDispatchActionDetailsById(Int64 RefId)
        {
            List<DiaryActionViewModel> items = new List<DiaryActionViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", Convert.ToInt64(RefId)) };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDispatchById]", parameterValues);
                foreach (DataRow row1 in ds.Tables[1].Rows)
                {
                    DiaryActionViewModel diaryActionViewModel = new DiaryActionViewModel();
                    diaryActionViewModel.Id = Convert.ToInt32(row1["id"].ToString());
                    diaryActionViewModel.ActionDescription = Convert.ToString(row1["ActionDescription"].ToString());
                    diaryActionViewModel.ActionAmount = string.IsNullOrEmpty(row1["ActionAmountSanction"].ToString()) ? 0 : Convert.ToDouble(row1["ActionAmountSanction"].ToString());
                    diaryActionViewModel.Enclosure = Convert.ToString(row1["Enclosure"].ToString());
                    diaryActionViewModel.ActionBy = Convert.ToString(row1["ActionBy"].ToString());
                    diaryActionViewModel.ActionCode = Convert.ToInt32(row1["ActionId"].ToString());
                    diaryActionViewModel.ActionDate = string.IsNullOrEmpty(row1["createdDate"].ToString()) ? DateTime.Now : Convert.ToDateTime(row1["createdDate"].ToString());
                    diaryActionViewModel.StatusName = Convert.ToString(row1["actionname"].ToString());
                    diaryActionViewModel.NoteId = Convert.ToInt32(row1["NoteId"].ToString());
                    items.Add(diaryActionViewModel);
                }
                return items;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return items;
            }
        }
        public static DiaryViewModel GetDispatchById(Int64 RefId)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", Convert.ToInt64(RefId)) };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDispatchById]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DiaryViewModel model = new DiaryViewModel();
                    List<DiaryActionViewModel> lstAction = new List<DiaryActionViewModel>();
                    model.RefId = Convert.ToInt64(row["RefId"].ToString());
                    model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                    model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                    model.FromDept = Convert.ToString(row["FromDept"].ToString());
                    model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                    model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                    model.ToDept = Convert.ToString(row["ToDept"].ToString());
                    model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                    model.CurrentDate = Convert.ToDateTime(row["crntDt"]).ToString("dd/MM/yyyy");
                    model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                    model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                    model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                    model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                    model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                    model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                    model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                    model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                    model.Subject = Convert.ToString(row["Subject"].ToString());
                    model.SubjectDetails = Convert.ToString(row["SubjectDetails"].ToString());
                    model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                    model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                    model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                    model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                    model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                    model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                    model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                    model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                    model.ReferencePriorityType = Convert.ToInt32(row["ReferencePriorityType"].ToString());
                    model.ReferenceTerm = Convert.ToInt32(row["ReferenceTerm"].ToString());
                    model.isLetter = Convert.ToString(row["isletter"].ToString());
                    model.agendaId = Convert.ToString(row["MeetingId"].ToString());
                    //model.ActName= Convert.ToString(row["ActName"].ToString());

                    
                    model.AssemblyCode= Convert.ToInt32(row["AssemblyID"].ToString());
                    model.SessionCode = Convert.ToInt32(row["SessionID"].ToString());
                    model.MemberCode = Convert.ToInt32(row["MemberCode"].ToString());

                    model.AssemblyName = Convert.ToString(row["AssemblyName"].ToString());
                    model.SessionName = Convert.ToString(row["SessionName"].ToString());
                    model.MemberName = Convert.ToString(row["MemberName1"].ToString());

                    foreach (DataRow row1 in ds.Tables[1].Rows)
                    {
                        DiaryActionViewModel diaryActionViewModel = new DiaryActionViewModel();
                        diaryActionViewModel.Id = Convert.ToInt32(row1["id"].ToString());
                        diaryActionViewModel.ActionDescription = Convert.ToString(row1["ActionDescription"].ToString());
                        diaryActionViewModel.ActionAmount = string.IsNullOrEmpty(row1["ActionAmountSanction"].ToString()) ? 0 : Convert.ToDouble(row1["ActionAmountSanction"].ToString());
                        diaryActionViewModel.Enclosure = Convert.ToString(row1["Enclosure"].ToString());
                        diaryActionViewModel.ActionBy = Convert.ToString(row1["ActionBy"].ToString());
                        diaryActionViewModel.ActionCode = Convert.ToInt32(row1["ActionId"].ToString());
                        diaryActionViewModel.ActionDate = string.IsNullOrEmpty(row1["createdDate"].ToString()) ? DateTime.Now : Convert.ToDateTime(row1["createdDate"].ToString());
                        diaryActionViewModel.StatusName = Convert.ToString(row1["actionname"].ToString());
                        diaryActionViewModel.NoteId = Convert.ToInt32(row1["NoteId"].ToString());
                       

                        lstAction.Add(diaryActionViewModel);
                    }
                    model.lstDiaryActionViewModel = lstAction;
                    lst.Add(model);
                }
                return lst[0];
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst[0];
            }
        }

        #region Analytics Chart Functions
        public DataTable getAnalyticsBarChart(string officeId)
        {
            DataTable dt;
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@officeId", Convert.ToString(officeId)) };
            var dtt = SqlHelper.ExecuteDataset(connection, "AnalyticsBarChart", parameterValues);
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            dt = dtt.Tables[0];
            return dt;

        }
        public DataTable GetDocumentsbyOfficeIdAnalyticsBarChart(string officeId, Int32 status, string diarydispatchvalue, string since, string startDt, string endDt)
        {
            DataTable dt;
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@officeId", Convert.ToString(officeId)),
             new SqlParameter("@status", status),
             new SqlParameter("@diarydispatchvalue", diarydispatchvalue),
             new SqlParameter("@since", since),
             new SqlParameter("@startDt", startDt),
             new SqlParameter("@endDt", endDt)
        };
            var dtt = SqlHelper.ExecuteDataset(connection, "[GetDocumentsbyOfficeIdAnalyticsBarChart]", parameterValues);
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            dt = dtt.Tables[0];
            return dt;
        }
        #endregion

    }

    public class SendInboxJson
    {
        public int MoveId { get; set; }
        public string RefId { get; set; }

    }

    public class MultifileJson
    {
        public string fileid { get; set; }
        public string name { get; set; }
        public string FileName { get; set; }

        public string isDeleted { get; set; }

    }

    public class LinkedFiles
    {
        public Int64 RefId { get; set; }

    }
    public class DiaryActionViewModel
    {
        public int OfficeId { get; set; }
        public Int64 RefId { get; set; }

        public string MergeRefId { get; set; }
        public int ActionCode { get; set; }
        public DateTime ActionDate { get; set; }
        public double ActionAmount { get; set; }
        public string ActionBy { get; set; }
        public string ActionDescription { get; set; }
        public string Enclosure { get; set; }
        public int Status { get; set; }
        public int Id { get; set; }

        public int NoteId { get; set; }
        public string isDraft { get; set; }
        public int Freezed { get; set; }
        public string StatusName { get; set; }
    }
    public class Status
    {
        public string StatusName { get; set; }
        public int StatusCnt { get; set; }

    }
    public class DiaryDashboard
    {
        public string DeptId { get; set; }
        public string DocId { get; set; }
        public string DeptName { get; set; }
        public List<Status> DashboardStatus { get; set; }
        public int Development { get; set; }
        public int Transfers { get; set; }
        public int CMReliefFund { get; set; }
        public int Discretionary { get; set; }
        public int Grivances { get; set; }
        public int demand { get; set; }
        public int Directions { get; set; }
        public int CMAnnoucement { get; set; }
        public int Total { get; set; }
        public string Date { get; set; }
        public string deptname { get; set; }
        public string officeName { get; set; }
        public string subject { get; set; }

     
        public string dispatchDate { get; set; }
        public string sentToDepartment { get; set; }
        public string sentToOffice { get; set; }
        public string sentActionStatus { get; set; }

        public static List<DiaryDashboard> getDispatchDashboardData(string deptId, int status, int offcieId, string startDt, string endDt)
        {
            List<DiaryDashboard> lst = new List<DiaryDashboard>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@status", Convert.ToInt32(status)), new SqlParameter("@offcieId", offcieId), new SqlParameter("@startDt", startDt), new SqlParameter("@endDt", endDt) };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DiaryDispatchDashbord]", parameterValues);


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DiaryDashboard model = new DiaryDashboard();

                    model.DeptName = Convert.ToString(row["deptname"]);
                    model.Development = Convert.ToInt32(row["Development"]);
                    model.Transfers = Convert.ToInt32(row["Transfers"]);
                    model.CMReliefFund = Convert.ToInt32(row["CMReliefFund"]);
                    model.Discretionary = Convert.ToInt32(row["Discretionary"]);
                    model.Grivances = Convert.ToInt32(row["Grivances"]);
                    model.demand = Convert.ToInt32(row["demand"]);
                    model.Directions = Convert.ToInt32(row["Directions"]);
                    model.CMAnnoucement = Convert.ToInt32(row["CMAnnoucement"]);
                    model.Total = Convert.ToInt32(row["Total"]);
                    model.demand = Convert.ToInt32(row["demand"]);
                    model.DeptId = Convert.ToString(row["deptId"]);
                    lst.Add(model);
                }
                DiaryDashboard modelsum = new DiaryDashboard();
                modelsum.DeptName = "Total";
                modelsum.Development = Convert.ToInt32(ds.Tables[0].Compute("Sum(Development)", ""));
                modelsum.Transfers = Convert.ToInt32(ds.Tables[0].Compute("Sum(Transfers)", ""));
                modelsum.CMReliefFund = Convert.ToInt32(ds.Tables[0].Compute("Sum(CMReliefFund)", ""));
                modelsum.Discretionary = Convert.ToInt32(ds.Tables[0].Compute("Sum(Discretionary)", ""));
                modelsum.Grivances = Convert.ToInt32(ds.Tables[0].Compute("Sum(Grivances)", ""));
                modelsum.demand = Convert.ToInt32(ds.Tables[0].Compute("Sum(demand)", ""));
                modelsum.Directions = Convert.ToInt32(ds.Tables[0].Compute("Sum(Directions)", ""));
                modelsum.CMAnnoucement = Convert.ToInt32(ds.Tables[0].Compute("Sum(CMAnnoucement)", ""));
                modelsum.Total = Convert.ToInt32(ds.Tables[0].Compute("Sum(Total)", ""));
                modelsum.demand = Convert.ToInt32(ds.Tables[0].Compute("Sum(demand)", ""));
                modelsum.DeptId = "0";
                lst.Add(modelsum);

                return lst;
            }
            catch (Exception)
            {
                return lst;
            }

        }
        public static DataTable getDashboardDataByDepttt(string deptId, int status, int offcieId, string startDt, string endDt)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptId", deptId));
            p.Add(new SqlParameter("@status", Convert.ToInt32(status)));
            p.Add(new SqlParameter("@offcieId", offcieId));
            p.Add(new SqlParameter("@startDt", startDt));
            p.Add(new SqlParameter("@endDt", endDt));
            var dtt = SqlHelper.ExecuteDataset(connection, "DiaryDashbordBindByDeptnew", p.ToArray());



            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];

            return dt;
        }


        public static DataTable getDashboardData(int status, int offcieId, string startDt, string endDt, string since, string userid, out DataTable dtStatus)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@status", Convert.ToInt32(status)),
                    new SqlParameter("@offcieId", offcieId),
                    new SqlParameter("@startDt", startDt),
                    new SqlParameter("@endDt", endDt),
                    new SqlParameter("@since", since) ,
                    new SqlParameter("@userid", Convert.ToString(userid))

                };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DiaryDashbordBind1neww]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }


        //  public static DataTable getDashboardDataByDepartment(int status, int offcieId, string startDt, string endDt, string since, string userid, int isApex, out DataTable dtStatus)
        public static DataTable getDashboardDataByDepartment(out DataTable dtStatus)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                //SqlParameter[] parameterValues = new SqlParameter[] {
                //    new SqlParameter("@status", Convert.ToInt32(status)),
                //    new SqlParameter("@offcieId", offcieId),
                //    new SqlParameter("@startDt", startDt),
                //    new SqlParameter("@endDt", endDt),
                //    new SqlParameter("@since", since) ,
                //    new SqlParameter("@isApex", isApex),
                //    new SqlParameter("@userid", Convert.ToString(userid))

                //};

                // DataSet ds = SqlHelper.ExecuteDataset(connection, "[getDashboardDataByDepartment]", parameterValues);
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[getDashboardDataByDepartment]");
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "officename", "officeid", "DeptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("officename");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("officeid");
                dt.Columns.Add("DeptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["officename"] = dr1["officename"].ToString();
                    dr11["officeid"] = dr1["officeid"].ToString();
                    dr11["DeptId"] = dr1["DeptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("DeptId LIKE '%" + dr1["DeptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static SelectList getMCWiseDepartmentList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DeptID", CurrentSession.DeptID) };
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getMCWiseDepartmentList]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["deptname"].ToString());
                    item.Value = Convert.ToString(row["deptid"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            }
        }

        public DataTable getDashboardDataByDept(string deptId, int status, int offcieId, string startDt, string endDt, string since)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptId", deptId));
            p.Add(new SqlParameter("@status", Convert.ToInt32(status)));
            p.Add(new SqlParameter("@offcieId", offcieId));
            p.Add(new SqlParameter("@startDt", startDt == "" ? null : startDt));
            p.Add(new SqlParameter("@endDt", endDt == "" ? null : endDt));
            p.Add(new SqlParameter("@since", since));
            var dtt = SqlHelper.ExecuteDataset(connection, "DiaryDashbordBindByDeptnew1", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public static DataTable getDashboardDataType(int status, int offcieId, string startDt, string endDt, string since, out DataTable dtStatus)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@status", Convert.ToInt32(status)),
                    new SqlParameter("@offcieId", offcieId),
                    new SqlParameter("@startDt", startDt),
                    new SqlParameter("@endDt", endDt),
                    new SqlParameter("@since", since)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DiaryDashbordBindtype]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }


        public static DataTable GetDataForDispatchDashboardStatus(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus,string reffiletype)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@deptid", deptid), 
                    new SqlParameter("@IsLetter", reffiletype)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDispatchDashboardStatus]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDispatchDashboardStatusFilter(string officeidsess, string officeid, string documentType, string status, string userid, string deptidsess, string since, string deptid, string term, string priority, string notchecked, out DataTable dtStatus,string reffiletype)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeidsession", officeidsess),
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@since", since),
                    new SqlParameter("@deptidsession", deptidsess),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@term", term),
                    new SqlParameter("@priority", priority),
                    new SqlParameter("@notchecked", notchecked),new SqlParameter("@IsLetter", reffiletype)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDispatchDashboardStatusFilter]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }


        public static DataTable GetDataForDispatchDashboardOffice(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@deptid", deptid)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDispatchDashboardOffice]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "officename", "OfficeId", "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                dt.Columns.Add("officename");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");
                dt.Columns.Add("OfficeId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["officename"] = dr1["officename"].ToString();
                    dr11["OfficeId"] = dr1["OfficeId"].ToString();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("OfficeId = " + dr1["OfficeId"].ToString() + "").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDispatchDashboardOfficeFilter(string officeidsess, string officeid, string documentType, string status, string userid, string deptidsess, string since, string deptid, string term, string priority, string notchecked, out DataTable dtStatus, string reffiletype)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeidsession", officeidsess),
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@since", since),
                    new SqlParameter("@deptidsession", deptidsess),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@term", term),
                    new SqlParameter("@priority", priority),
                    new SqlParameter("@notchecked", notchecked),new SqlParameter("@IsLetter",reffiletype)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDispatchDashboardOfficeFilter]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "officename", "OfficeId", "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                dt.Columns.Add("officename");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");
                dt.Columns.Add("OfficeId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["officename"] = dr1["officename"].ToString();
                    dr11["OfficeId"] = dr1["OfficeId"].ToString();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("OfficeId = " + dr1["OfficeId"].ToString() + "").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }


        public static DataTable GetDataForDispatchDashboardType(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus,string reffiletype)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@IsLetter",reffiletype)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDispatchDashboardStatusType]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId").ToString();

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDispatchDashboardTypeFilter(string officeidsess, string officeid, string documentType, string status, string userid, string deptidsess, string since, string deptid, string term, string priority, out DataTable dtStatus,string reffiletype)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeidsession", officeidsess),
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@since", since),
                    new SqlParameter("@deptidsession", deptidsess),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@term", term),
                    new SqlParameter("@priority", priority),
                    new SqlParameter("@IsLetter", reffiletype)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDispatchDashboardTypeFilter]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }



        public static DataTable GetDataForDiaryDashboardType(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus, string reffiletype)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@IsLetter", reffiletype)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDiaryDashboardStatusType]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDiaryDashboardTypeFilter(string officeidsess, string officeid, string documentType, string status, string userid, string deptidsess, string since, string deptid, string term, string priority, out DataTable dtStatus, string reffiletype)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeidsession", officeidsess),
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@since", since),
                    new SqlParameter("@deptidsession", deptidsess),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@term", term),
                    new SqlParameter("@priority", priority),
                    new SqlParameter("@IsLetter", reffiletype)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDiaryDashboardTypeFilter]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }


        public static DataTable GetDataForDiaryDashboardStatus(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus,string reffiletype)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@IsLetter", reffiletype)
               
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDiaryDashboardStatus]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");
                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDiaryDashboardStatusFilter(string officeidsess, string officeid, string documentType, string status, string userid, string deptidsess, string since, string deptid, string term, string priority, out DataTable dtStatus,string reffiletype)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeidsession", officeidsess),
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@since", since),
                    new SqlParameter("@deptidsession", deptidsess),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@term", term),
                    new SqlParameter("@priority", priority),
                    new SqlParameter("@IsLetter", reffiletype)

                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDiaryDashboardStatusFilter]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDiaryDashboardOffice(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus,string reffiletype)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@IsLetter", reffiletype)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDiaryDashboardOffice]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "officename", "OfficeId", "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("officename");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("OfficeId");
                dt.Columns.Add("deptname");
                dt.Columns.Add("deptId");
                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["officename"] = dr1["officename"].ToString();
                    dr11["OfficeId"] = dr1["OfficeId"].ToString();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("OfficeId = " + dr1["OfficeId"].ToString() + "").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDiaryDashboardOfficeFilter(string officeidsess, string officeid, string documentType, string status, string userid, string deptidsess, string since, string deptid, string term, string priority, out DataTable dtStatus,string reffiletype)
        {



            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeidsession", officeidsess),
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@documentType", documentType),
                    new SqlParameter("@status", status),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@since", since),
                    new SqlParameter("@deptidsession", deptidsess),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@term", term),
                    new SqlParameter("@priority", priority),
                    new SqlParameter("@IsLetter", reffiletype)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForDiaryDashboardOfficeFilter]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "officename", "OfficeId", "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("officename");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("OfficeId");
                dt.Columns.Add("deptname");
                dt.Columns.Add("deptId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["officename"] = dr1["officename"].ToString();
                    dr11["OfficeId"] = dr1["OfficeId"].ToString();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("OfficeId = " + dr1["OfficeId"].ToString() + "").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }




        public static List<SelectListItem> DiaryRefWise(int OfficeId, int status, string userid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@nextDate", Convert.ToInt32(status)),
                    new SqlParameter("@officeId", Convert.ToInt32(OfficeId)),
                    new SqlParameter("@userid", userid)
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DiaryWeekly]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem select = new SelectListItem();
                    select.Value = Convert.ToString(row["cnt"]);
                    select.Text = row["dt"].ToString();
                    items.Add(select);
                }
                return items;
            }
            catch (Exception)
            {
                return items;
            }

        }

        public static List<SelectListItem> DiaryRefWiseChart(int OfficeId, int status, string diarydispatchvalue)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@nextDate", Convert.ToInt32(status)),
                    new SqlParameter("@officeId", Convert.ToInt32(OfficeId)),
                new SqlParameter("@diarydispatchvalue", diarydispatchvalue)
                };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DiaryWeeklyChart]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem select = new SelectListItem();
                    select.Value = Convert.ToString(row["cnt"]);
                    select.Text = row["dt"].ToString();
                    items.Add(select);
                }
                return items;
            }
            catch (Exception)
            {
                return items;
            }

        }

        public static List<SelectListItem> DiaryRefActionWise(int OfficeId, int status, string userid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@nextDate", Convert.ToInt32(status)),
                    new SqlParameter("@officeId", Convert.ToInt32(OfficeId)),
                    new SqlParameter("@userid", userid)

                };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DiaryActionWeekly]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem select = new SelectListItem();
                    select.Value = Convert.ToString(row["cnt"]);
                    select.Text = row["dt"].ToString();
                    items.Add(select);
                }
                return items;
            }
            catch (Exception)
            {
                return items;
            }
        }
        public static List<SelectListItem> DispatchRefWise(int OfficeId, int status, string userid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@nextDate", Convert.ToInt32(status)),
                    new SqlParameter("@officeId", Convert.ToInt32(OfficeId)),
                    new SqlParameter("@userid", userid)
                };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DispatchWeekly]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem select = new SelectListItem();
                    select.Value = Convert.ToString(row["cnt"]);
                    select.Text = row["dt"].ToString();
                    items.Add(select);
                }
                return items;
            }
            catch (Exception)
            {
                return items;
            }
        }
        public static List<SelectListItem> DispatchActionRefWise(int OfficeId, int nextDate, string userid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@nextDate", Convert.ToInt32(nextDate)),
                    new SqlParameter("@officeId", Convert.ToInt32(OfficeId)),
                    new SqlParameter("@userid", userid)
                };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DispatchActionWeekly]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem select = new SelectListItem();
                    select.Value = Convert.ToString(row["cnt"]);
                    select.Text = row["dt"].ToString();

                    items.Add(select);

                }
                return items;
            }
            catch (Exception)
            {
                return items;
            }

        }
        public static DataTable getDispatchDashboardData(int status, int offcieId, string startDt, string endDt, string since, string userid, out DataTable dtStatus)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@status", Convert.ToInt32(status)),
                    new SqlParameter("@offcieId", offcieId),
                    new SqlParameter("@startDt", startDt),
                    new SqlParameter("@endDt", endDt),
                    new SqlParameter("@since", since),
                    new SqlParameter("@userid", userid)
                };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DiaryDispatchDashbord1neww]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();
                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");
                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                dtStatus = dt.Copy();
                return dt;
            }
        }
        public static DataTable getDispatchDashboardDataType(int status, int offcieId, string startDt, string endDt, string since, out DataTable dtStatus)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@status", Convert.ToInt32(status)),
                    new SqlParameter("@offcieId", offcieId), new SqlParameter("@startDt", startDt), new SqlParameter("@endDt", endDt),
                    new SqlParameter("@since", since)
                };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DiaryDispatchDashbordType]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();
                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");
                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                dtStatus = dt.Copy();
                return dt;
            }
        }
        public static List<DiaryDashboard> GetDispatchDataByDept(string deptId, int status, int offcieId, string startDt, string endDt)
        {
            List<DiaryDashboard> lst = new List<DiaryDashboard>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptId", deptId), new SqlParameter("@status", Convert.ToInt32(status)), new SqlParameter("@offcieId", offcieId), new SqlParameter("@startDt", startDt), new SqlParameter("@endDt", endDt) };

                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDispatchDataByDept]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DiaryDashboard model = new DiaryDashboard();

                    model.DeptName = Convert.ToString(row["officename"]);
                    model.Development = Convert.ToInt32(row["Development"]);
                    model.Transfers = Convert.ToInt32(row["Transfers"]);
                    model.CMReliefFund = Convert.ToInt32(row["CMReliefFund"]);
                    model.Discretionary = Convert.ToInt32(row["Discretionary"]);
                    model.Grivances = Convert.ToInt32(row["Grivances"]);
                    model.demand = Convert.ToInt32(row["demand"]);
                    model.Directions = Convert.ToInt32(row["Directions"]);
                    model.CMAnnoucement = Convert.ToInt32(row["CMAnnoucement"]);
                    model.Total = Convert.ToInt32(row["Total"]);
                    model.demand = Convert.ToInt32(row["demand"]);
                    model.DeptId = Convert.ToString(row["OfficeId"]);
                    lst.Add(model);
                }
                return lst;
            }
            catch (Exception)
            {
                return lst;
            }

        }

        public static DataTable GetDataForDiaryDashboardStat(string officeid, string userid, string deptid, out DataTable dtStatus, string financialyear, int mType, int FileType, int RefType, int ShowType)
        {
            //CurrentSession.DeptID = "LG0001";
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@Fyear", string.IsNullOrEmpty(financialyear) ? (object)DBNull.Value : (object)financialyear),
                    new SqlParameter("@mType", mType),
                    new SqlParameter("@FileType", FileType),
                    new SqlParameter("@RefType", RefType),
                     new SqlParameter("@ShowType", ShowType),
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DashboardStats]", parameterValues);
                DataRow[] dtdr = ds.Tables[0].Select("DocumentTypeId =111");
                DataTable dvdt1 = dtdr.CopyToDataTable();
                DataView view = new DataView(dvdt1);
                DataTable dtDeptName;
                if (CurrentSession.isDeptApex.ToString() == "1")
                {
                     dtDeptName = view.ToTable(true, "deptname", "deptId", "Toofficeid", "FromDeptId");
                }
                else
                {
                     dtDeptName = view.ToTable(true, "FromDept", "deptId", "Toofficeid", "FromDeptId");
                }
                
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                dt.Columns.Add("FromDeptId");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                   
                }

                dt.Columns.Add("deptId");
                dt.Columns.Add("officeid");
                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();
                    if (CurrentSession.isDeptApex.ToString() == "1")
                    {
                        dr11["deptname"] = dr1["deptname"].ToString();
                        dr11["FromDeptId"] = dr1["FromDeptId"].ToString();
                    }
                    else
                    {
                        dr11["deptname"] = dr1["FromDept"].ToString();
                        dr11["FromDeptId"] = dr1["FromDeptId"].ToString();
                    }
                    
                    dr11["deptId"] = dr1["deptId"].ToString();
                    dr11["officeid"] = dr1["Toofficeid"].ToString();
                    DataTable dtResult;
                    //if (CurrentSession.DeptID.ToString() == "60")
                    //{
                    //     dtResult = ds.Tables[0].Select("deptname LIKE '%" + dr1["deptname"].ToString() + "%'").CopyToDataTable();
                    //}
                    //else
                    //{
                    //     dtResult = ds.Tables[0].Select("FromDept LIKE '%" + dr1["FromDept"].ToString() + "%'").CopyToDataTable();
                    //}

                    if (CurrentSession.isDeptApex.ToString() == "1")
                    {
                        dtResult = ds.Tables[0].Select("deptid='" + dr1["deptId"].ToString() + "'").CopyToDataTable();
                    }
                    else
                    {
                        dtResult = ds.Tables[0].Select("FromDeptId='" + dr1["FromDeptId"].ToString() + "'").CopyToDataTable();
                    }



                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }



                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDiaryDashboardStatDepartment(string officeid, string userid, string deptid, out DataTable dtStatus, string financialyear, int mType, int FileType, int RefType, int ShowType)
        {
            //CurrentSession.DeptID = "LG0001";
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@officeid", officeid),
                    new SqlParameter("@userid", userid),
                    new SqlParameter("@deptid", deptid),
                    new SqlParameter("@Fyear", string.IsNullOrEmpty(financialyear) ? (object)DBNull.Value : (object)financialyear),
                    new SqlParameter("@mType", mType),
                    new SqlParameter("@FileType", FileType),
                    new SqlParameter("@RefType", RefType),
                     new SqlParameter("@ShowType", ShowType),
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DashboardStatsDepartment]", parameterValues);
                DataRow[] dtdr = ds.Tables[0].Select("DocumentTypeId =111");
                DataTable dvdt1 = dtdr.CopyToDataTable();
                DataView view = new DataView(dvdt1);
                DataTable dtDeptName;
                if (CurrentSession.isDeptApex.ToString() == "1")
                {
                    dtDeptName = view.ToTable(true, "deptname", "deptId", "Toofficeid", "FromDeptId");
                }
                else
                {
                    dtDeptName = view.ToTable(true, "FromDept", "deptId", "Toofficeid", "FromDeptId");
                }

                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                dt.Columns.Add("FromDeptId");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));

                }

                dt.Columns.Add("deptId");
                dt.Columns.Add("officeid");
                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();
                    if (CurrentSession.isDeptApex.ToString() == "1")
                    {
                        dr11["deptname"] = dr1["deptname"].ToString();
                        dr11["FromDeptId"] = dr1["FromDeptId"].ToString();
                    }
                    else
                    {
                        dr11["deptname"] = dr1["FromDept"].ToString();
                        dr11["FromDeptId"] = dr1["FromDeptId"].ToString();
                    }

                    dr11["deptId"] = dr1["deptId"].ToString();
                    dr11["officeid"] = dr1["Toofficeid"].ToString();
                    DataTable dtResult;
                    //if (CurrentSession.DeptID.ToString() == "60")
                    //{
                    //     dtResult = ds.Tables[0].Select("deptname LIKE '%" + dr1["deptname"].ToString() + "%'").CopyToDataTable();
                    //}
                    //else
                    //{
                    //     dtResult = ds.Tables[0].Select("FromDept LIKE '%" + dr1["FromDept"].ToString() + "%'").CopyToDataTable();
                    //}

                    if (CurrentSession.isDeptApex.ToString() == "1")
                    {
                        dtResult = ds.Tables[0].Select("deptid='" + dr1["deptId"].ToString() + "'").CopyToDataTable();
                    }
                    else
                    {
                        dtResult = ds.Tables[0].Select("FromDeptId='" + dr1["FromDeptId"].ToString() + "'").CopyToDataTable();
                    }



                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }



                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        //public static DataTable GetDataForDiaryDashboardStatDepartmentOpenAll(string officeid, string userid, string deptid, out DataTable dtStatus, string financialyear, int mType, int FileType, int RefType, int ShowType, string selectedType)
        //{
        //    //CurrentSession.DeptID = "LG0001";
        //    DataTable dt = new DataTable();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlConnection connection = ClsConnection.GetConnection();
        //        if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
        //        {
        //            connection.Open();
        //        }

        //        connection.Close();
        //        SqlParameter[] parameterValues = new SqlParameter[] {
        //            new SqlParameter("@officeid", officeid),
        //            new SqlParameter("@userid", userid),
        //            new SqlParameter("@deptid", deptid),
        //            new SqlParameter("@Fyear", string.IsNullOrEmpty(financialyear) ? (object)DBNull.Value : (object)financialyear),
        //            new SqlParameter("@mType", mType),
        //            new SqlParameter("@FileType", FileType),
        //            new SqlParameter("@RefType", 1),
        //            new SqlParameter("@ShowType", ShowType),
        //            new SqlParameter("@homeType", selectedType),

        //        };
        //        if (CurrentSession.DeptID == "LG00001")
        //        { 
        //            ds = SqlHelper.ExecuteDataset(connection, "[DashboardStatsDepartment_LG]", parameterValues); 
        //        }
        //        else
        //        {
        //            ds = SqlHelper.ExecuteDataset(connection, "DashboardStatsDepartment_MC", parameterValues);
        //        }
        //        DataRow[] dtdr = ds.Tables[0].Select();
        //        DataTable dvdt1 = dtdr.CopyToDataTable();
        //        DataView view = new DataView(dvdt1);
        //        DataTable dtDeptName;
        //        if (CurrentSession.isDeptApex.ToString() == "1")
        //        {
        //            dtDeptName = view.ToTable(true, "deptname", "deptId", "Toofficeid", "FromDeptId");
        //            //dtDeptName = view.ToTable(true, "FromDept", "deptId", "Toofficeid", "FromDeptId");
        //        }
        //        else
        //        {
        //            dtDeptName = view.ToTable(true, "FromDept", "deptId", "Toofficeid", "FromDeptId");
        //        }

        //        dtStatus = ds.Tables[1].Copy();

        //        dt.Clear();
        //        dt.Columns.Add("deptname");
        //        dt.Columns.Add("FromDeptId");
        //        foreach (DataRow dr in ds.Tables[1].Rows)
        //        {
        //            dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));

        //        }

        //        dt.Columns.Add("deptId");
        //        dt.Columns.Add("officeid");
        //        foreach (DataRow dr1 in dtDeptName.Rows)
        //        {
        //            DataRow dr11 = dt.NewRow();
        //            if (CurrentSession.isDeptApex.ToString() == "1")
        //            {
        //                dr11["deptname"] = dr1["deptname"].ToString();
        //                dr11["deptId"] = dr1["deptId"].ToString();
        //            }
        //            else
        //            {
        //                dr11["deptname"] = dr1["FromDept"].ToString();
        //                dr11["FromDeptId"] = dr1["FromDeptId"].ToString();
        //            }

        //            dr11["deptId"] = dr1["deptId"].ToString();
        //            dr11["officeid"] = dr1["Toofficeid"].ToString();
        //            DataTable dtResult;
        //            //if (CurrentSession.DeptID.ToString() == "60")
        //            //{
        //            //     dtResult = ds.Tables[0].Select("deptname LIKE '%" + dr1["deptname"].ToString() + "%'").CopyToDataTable();
        //            //}
        //            //else
        //            //{
        //            //     dtResult = ds.Tables[0].Select("FromDept LIKE '%" + dr1["FromDept"].ToString() + "%'").CopyToDataTable();
        //            //}

        //            //if (CurrentSession.isDeptApex.ToString() == "1")
        //            //{
        //            //    dtResult = ds.Tables[0].Select("deptid='" + dr1["deptId"].ToString() + "'").CopyToDataTable();
        //            //}
        //            //else
        //            //{
        //                dtResult = ds.Tables[0].Select("deptid='" + dr1["deptid"].ToString() + "'").CopyToDataTable();
        //            //}



        //            foreach (DataRow drseach in dtResult.Rows)
        //            {
        //                foreach (DataColumn dc in dt.Columns)
        //                {
        //                    if (drseach["documenttypename"].ToString() == dc.ColumnName)
        //                    {
        //                        dr11[dc.ColumnName] = drseach["cnt"];
        //                        break;
        //                    }
        //                }
        //            }
        //            dt.Rows.Add(dr11);
        //        }

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            foreach (DataColumn col in dt.Columns)
        //            {
        //                //test for null here
        //                if (row[col] == DBNull.Value)
        //                {
        //                    row[col] = 0;
        //                }
        //            }
        //        }



        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
        //        dtStatus = dt.Copy();
        //        return dt;
        //    }
        //}


        public static DataTable GetDataForDiaryDashboardStatDepartmentOpenAll(
           string officeid,
           string userid,
           string deptid,
           out DataTable dtStatus,
           string financialyear,
           int mType,
           int FileType,
           int RefType,
           int ShowType,
           string selectedType, string MCType, string MCClass)
           {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@officeid", officeid),
                        new SqlParameter("@userid", userid),
                        new SqlParameter("@deptid", deptid),

                        new SqlParameter("@Fyear",
                            string.IsNullOrWhiteSpace(financialyear)
                                ? (object)DBNull.Value
                                : financialyear),

                        new SqlParameter("@mType", mType),
                        new SqlParameter("@FileType", FileType),
                        new SqlParameter("@RefType", 1),
                        new SqlParameter("@ShowType", ShowType),
                        new SqlParameter("@homeType", selectedType)
                    };

                    // ✔ Only for LG procedure
                    if (CurrentSession.DeptID == "LG00001" || CurrentSession.DeptID == "UDD0001")
                    {
                        parameters.Add(new SqlParameter("@class",
                            string.IsNullOrWhiteSpace(MCClass)
                                ? (object)DBNull.Value
                                : MCClass));

                        parameters.Add(new SqlParameter("@MCType", SqlDbType.Int)
                        {
                            Value = int.TryParse(MCType, out int mcTypeVal)
                                    ? mcTypeVal
                                    : (object)DBNull.Value
                        });

                        ds = SqlHelper.ExecuteDataset(
                                connection,
                                CommandType.StoredProcedure,
                                "DashboardStatsDepartment_LG_Filter",
                                parameters.ToArray());
                    }
                    else
                    {
                        // NO MCClass / MCType here
                        ds = SqlHelper.ExecuteDataset(
                                connection,
                                CommandType.StoredProcedure,
                                "DashboardStatsDepartment_MC",
                                parameters.ToArray());
                    }
                }


                DataTable raw = ds.Tables[0];
                dtStatus = ds.Tables[1].Copy();

                // ================= CREATE RESULT STRUCTURE =================
                dt.Columns.Add("deptname", typeof(string));
                dt.Columns.Add("FromDeptId", typeof(string));

                // Columns ONLY from dtStatus
                foreach (DataRow r in dtStatus.Rows)
                {
                    dt.Columns.Add(r["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId", typeof(string));
                dt.Columns.Add("officeid", typeof(string));
                dt.Columns.Add("DestinationDeptID", typeof(string));


                // ================= DOCUMENT TYPE MAP =================
                // DocumentTypeId -> DocumentTypeName
                var docTypeMap = dtStatus.AsEnumerable()
                    .ToDictionary(
                        r => r["DocumentTypeId"].ToString(),
                        r => r["DocumentTypeName"].ToString()
                    );

                // ================= GROUP DATA BY DEPARTMENT =================
                var grouped = raw.AsEnumerable()
                    .GroupBy(r => new
                    {
                        DeptId = r["deptid"].ToString(),
                        OfficeId = r["Toofficeid"].ToString(),
                        DeptName = (CurrentSession.isDeptApex == "1")
                                    ? r["deptname"].ToString()
                                    : r["FromDept"].ToString(),
                        FromDeptId = r["FromDeptId"].ToString(),
                        DestinationDeptID= r["DestinationDeptID"].ToString()
                    });

                foreach (var grp in grouped)
                {
                    DataRow newRow = dt.NewRow();

                    newRow["deptname"] = grp.Key.DeptName;
                    newRow["deptId"] = grp.Key.DeptId;
                    newRow["officeid"] = grp.Key.OfficeId;
                    newRow["FromDeptId"] = grp.Key.FromDeptId;
                    newRow["DestinationDeptID"] = grp.Key.DestinationDeptID;
                    // Initialize all document columns to 0
                    foreach (var col in docTypeMap.Values)
                        newRow[col] = 0;

                    // Aggregate counts PER DOCUMENT TYPE
                    foreach (var docGrp in grp.GroupBy(x => x["DocumentTypeId"].ToString()))
                    {
                        if (!docTypeMap.ContainsKey(docGrp.Key))
                            continue;

                        string columnName = docTypeMap[docGrp.Key];

                        int total = docGrp.Sum(x =>
                            x["cnt"] == DBNull.Value ? 0 : Convert.ToInt32(x["cnt"])
                        );

                        newRow[columnName] = total;
                    }

                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(
                    ex,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString()
                );

                dtStatus = new DataTable();
                return new DataTable();
            }
        }


     public static DataTable GetDataForDiaryDashboardStatDepartmentPendency(
      string officeid,
      string userid,
      string deptid,
      out DataTable dtStatus,
      string financialyear,
      int mType,
      int FileType,
      int RefType,
      int ShowType,
      string selectedType, string MCType, string MCClass)
        {
            DataSet ds = new DataSet();
            DataTable dts = new DataTable();
            dtStatus = new DataTable(); // ✅ initialize

            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    ds = SqlHelper.ExecuteDataset(
                        connection,
                        CommandType.StoredProcedure,
                        "Pendency10daysADCandLG",
                        new SqlParameter("@DeptId", deptid),
                        new SqlParameter("@MCType", MCType),
                        new SqlParameter("@Office", officeid)
                    );

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dts = ds.Tables[0];

                        // ✅ second table assign
                        if (ds.Tables.Count > 1)
                        {
                            dtStatus = ds.Tables[1];
                        }
                    }
                }

                return dts;
            
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(
                    ex,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString()
                );

                dtStatus = new DataTable(); // ✅ ensure assign
                return new DataTable();
            }
        }



        public static DataTable GetDataForDiaryDashboardIndivisual(string officeid, string userid, string deptid, out DataTable dtStatus, string financialyear, int RefType, string MeetingType, string selectedType)
        {
            //CurrentSession.DeptID = "LG0001";

            if (string.IsNullOrEmpty(MeetingType) && CurrentSession.MapId != "0")
            {
                MeetingType = CurrentSession.MapId;
            }

            DataTable dt = new DataTable();
            try
            {
                string SiteName = System.Web.Configuration.WebConfigurationManager.AppSettings["serviceName"];
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                   new SqlParameter("@officeid", officeid),
                   new SqlParameter("@userid", userid),
                   new SqlParameter("@deptid", deptid),
                   new SqlParameter("@Fyear", string.IsNullOrEmpty(financialyear) ? (object)DBNull.Value : (object)financialyear),
                   //new SqlParameter("@RefType", 1),
                   new SqlParameter("@mcType", MeetingType),
                   new SqlParameter("@homeType", selectedType),

                  };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[IndivisualReferencesTest]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId", "officeid");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId");
                dt.Columns.Add("officeid");
                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    dr11["officeid"] = dr1["officeid"].ToString();
                    DataTable dtResult = new DataTable();
                    if (SiteName == "MC")
                    {
                        dtResult = ds.Tables[0].Select("officeid = '" + dr1["officeid"].ToString() + "'").CopyToDataTable();
                    }
                    else
                    {
                        dtResult = ds.Tables[0].Select("deptid = '" + dr1["deptId"].ToString() + "'").CopyToDataTable();
                    }
                    

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataDashboardStatsType(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus, string reffiletype, string financialYear, string DashType, string meetingType)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
              new SqlParameter("@officeid", officeid),
              new SqlParameter("@documentType", documentType),
              new SqlParameter("@status", status),
              new SqlParameter("@userid", userid),
              new SqlParameter("@deptid", deptid),
              new SqlParameter("@IsLetter",reffiletype),
              new SqlParameter("@Fyear", string.IsNullOrEmpty(financialYear) ? DBNull.Value : (object)financialYear),
              new SqlParameter("@homeType",DashType),
                new SqlParameter("@meetingType",meetingType),

          };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DashboardStatsByType]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId").ToString();
                //dt.Columns.Add("OfficeName").ToString();

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    //dr11["OfficeName"] = dr1["OfficeName"].ToString();
                    string deptId = Convert.ToString(dr1["deptId"]).Trim();
                    DataTable dtResult = ds.Tables[0].Select($"deptid = {deptId}").CopyToDataTable();

                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataDashboardStatsTypeDetails(int deptid, string userid,out DataTable dtStatus, string Departmentid, string actioncode)
        {
            if (actioncode == "")
            {
                actioncode = null;
            }
            DataTable dt = new DataTable();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@deptid", deptid),
                new SqlParameter("@userid", userid),
                new SqlParameter("@ActionCode", actioncode),
                new SqlParameter("@departmentID", Departmentid),
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[DashboardStatsByTypeDetails]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "deptname", "deptId", "OfficeName", "OfficeID");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("deptname");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("deptId").ToString();
                dt.Columns.Add("OfficeName").ToString();
                dt.Columns.Add("OfficeID").ToString();

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["deptId"] = dr1["deptId"].ToString();
                    dr11["OfficeName"] = dr1["OfficeName"].ToString();
                    dr11["OfficeID"] = dr1["OfficeID"].ToString();
                    //DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();
                    string deptId = Convert.ToString(dr1["deptId"]).Trim();
                    DataTable dtResult = ds.Tables[0].Select($"deptid = {deptId}").CopyToDataTable();
                    foreach (DataRow drseach in dtResult.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (drseach["documenttypename"].ToString() == dc.ColumnName)
                            {
                                dr11[dc.ColumnName] = drseach["cnt"];
                                break;
                            }
                        }
                    }
                    dt.Rows.Add(dr11);
                }

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        //test for null here
                        if (row[col] == DBNull.Value)
                        {
                            row[col] = 0;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }


        public static DiaryViewModel GetDiaryById(Int64 RefId, string DeptId)
        {
            List<DiaryViewModel> lst = new List<DiaryViewModel>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@RefId", RefId), new SqlParameter("@DeptId", DeptId) };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDiaryById]", parameterValues);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DiaryViewModel model = new DiaryViewModel();
                        List<DiaryActionViewModel> lstAction = new List<DiaryActionViewModel>();
                        model.RefId = Convert.ToInt64(row["RefId"].ToString());
                        model.LinkRefID = string.IsNullOrEmpty(row["LinkRefID"].ToString()) ? 0 : Convert.ToInt64(row["LinkRefID"].ToString());
                        model.FromOffice = Convert.ToString(row["FromOffice"].ToString());
                        model.FromDept = Convert.ToString(row["FromDept"].ToString());
                        model.DocumentTypeName = Convert.ToString(row["DocumentTypeName"].ToString());
                        model.ToOffice = Convert.ToString(row["ToOffice"].ToString());
                        model.ToDept = Convert.ToString(row["ToDept"].ToString());
                        model.FromDeptId = Convert.ToString(row["FromDeptId"].ToString());
                        model.FromOfficeId = Convert.ToInt32(row["FromOfficeId"].ToString());
                        model.DispatchNumber = string.IsNullOrEmpty(row["DispatchNumber"].ToString()) ? 0 : Convert.ToInt32(row["DispatchNumber"].ToString());
                        model.ToDeptId = Convert.ToString(row["ToDeptId"].ToString());
                        model.ToOfficeId = Convert.ToInt32(row["ToOfficeId"].ToString());
                        model.DiaryNumber = Convert.ToString(row["DiaryNumber"].ToString());
                        model.DocmentType = Convert.ToString(row["DocmentType"].ToString());
                        model.LetterNo = Convert.ToString(row["LetterNo"].ToString());
                        model.LetterDate = string.IsNullOrEmpty(row["LetterDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["LetterDate"].ToString());
                        model.Subject = Convert.ToString(row["Subject"].ToString());
                        model.SubjectDetails = Convert.ToString(row["newsubjectDetails"].ToString());
                        model.EnclosureFile = Convert.ToString(row["EnclosureFile"].ToString());
                        model.ApplicantName = Convert.ToString(row["ApplicantName"].ToString());
                        model.ApplicantAddress = Convert.ToString(row["ApplicantAddress"].ToString());
                        model.ApplicantMobile = Convert.ToString(row["ApplicantMobile"].ToString());
                        model.ApplicantEmail = Convert.ToString(row["ApplicantEmail"].ToString());
                        model.CreatedDate = string.IsNullOrEmpty(row["CreatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"].ToString());
                        model.DiaryDate = string.IsNullOrEmpty(row["DiaryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["DiaryDate"].ToString());
                        model.ActionCode = Convert.ToInt32(row["ActionCode"].ToString());
                        model.isLetter = Convert.ToString(row["isletter"].ToString());
                        model.agendaId = Convert.ToString(row["MeetingId"].ToString());
                        model.ReferencePriorityType = Convert.ToInt32(row["ReferencePriorityType"].ToString());
                        model.ReferenceTerm = Convert.ToInt32(row["ReferenceTerm"].ToString());
                        model.CurrentDate = Convert.ToDateTime(row["crntDt"]).ToString("dd/MM/yyyy");
                        model.ActionTakenDate = Convert.ToDateTime(row["crntDt"]);
                        model.ResolutioNo = row["ResolutionNo"].ToString();
                        model.ResCount = row["RefCount"].ToString();
                        var isfunddd = row["isfund"].ToString();
                        model.ActName = Convert.ToString(row["ActName"].ToString());
                        if (isfunddd == null || isfunddd == "")
                        {
                            isfunddd = "0";
                            model.isfund = Convert.ToInt32(isfunddd);
                        }
                        else
                        {
                            model.isfund = Convert.ToInt32(isfunddd);
                        }

                        foreach (DataRow row1 in ds.Tables[1].Rows)
                        {
                            DiaryActionViewModel diaryActionViewModel = new DiaryActionViewModel();
                            diaryActionViewModel.Id = Convert.ToInt32(row1["id"].ToString());
                            diaryActionViewModel.ActionDescription = Convert.ToString(row1["ActionDescription"].ToString());
                            diaryActionViewModel.ActionAmount = string.IsNullOrEmpty(row1["ActionAmountSanction"].ToString()) ? 0 : Convert.ToDouble(row1["ActionAmountSanction"].ToString());
                            diaryActionViewModel.Enclosure = Convert.ToString(row1["Enclosure"].ToString());
                            diaryActionViewModel.ActionBy = Convert.ToString(row1["ActionBy"].ToString());
                            diaryActionViewModel.ActionCode = Convert.ToInt32(row1["ActionId"].ToString());
                            diaryActionViewModel.ActionDate = string.IsNullOrEmpty(row1["createdDate"].ToString()) ? DateTime.Now : Convert.ToDateTime(row1["createdDate"].ToString());
                            diaryActionViewModel.StatusName = Convert.ToString(row1["actionname"].ToString());
                            diaryActionViewModel.NoteId = Convert.ToInt32(row1["NoteId"].ToString());
                            diaryActionViewModel.Freezed = string.IsNullOrEmpty(row1["Freezed"].ToString()) ? 0 : Convert.ToInt32(row1["Freezed"]);
                            lstAction.Add(diaryActionViewModel);
                        }
                        List<MultifileJson> lstmultifile = new List<MultifileJson>();
                        foreach (DataRow row1 in ds.Tables[2].Rows)
                        {
                            MultifileJson multifile = new MultifileJson();
                            multifile.FileName = row1["FilePath"].ToString();
                            multifile.name = row1["ABC"].ToString();
                            multifile.isDeleted = row1["isDeleted"].ToString();
                            multifile.fileid = row1["id"].ToString();
                            lstmultifile.Add(multifile);
                        }

                        model.MultiFileModel = lstmultifile;

                        model.lstDiaryActionViewModel = lstAction;
                        lst.Add(model);
                    }
                    return lst[0];
                }
                else
                {
                    return null;

                }
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public static List<MCStatDetail> GetMCStatDetailsTypeListType(int deptid, string userid, out DataTable dtStatus, string Departmentid, string actioncode, string financialYear, string DashType)
        {
            if (actioncode == "")
            {
                actioncode = null;
            }
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@deptid", deptid),
                new SqlParameter("@userid", userid),
                new SqlParameter("@ActionCode", actioncode),
                new SqlParameter("@departmentID", Departmentid),
                new SqlParameter("@Fyear", string.IsNullOrEmpty(financialYear) ? DBNull.Value : (object)financialYear),
                new SqlParameter("@DashType", DashType),
                };
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "DashboardStatsByTypeDetails", parameterValues);

                if (ds.Tables.Count > 0)
                {
                    int s = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MCStatDetail item = new MCStatDetail();
                        item.SrNo = s;
                        item.MeetingDate = Convert.ToString(dr["MettingDate"]);
                        item.ReferenceId = Convert.ToString(dr["Referance"]);
                        item.Meeting = Convert.ToString(dr["MettingName"]);
                        item.CreatedBy = Convert.ToString(dr["officename"]);
                        item.Resolutionno = Convert.ToString(dr["ResolutionNo"]);
                        item.Type = Convert.ToString(dr["type"]);
                        item.ActionName = Convert.ToString(dr["ActionName"]);
                        item.Subject = Convert.ToString(dr["Subject"]);
                        lstitems.Add(item);
                        s++;
                    };

                }

                dtStatus = null;
                return lstitems;
            }
            catch (Exception ex)
            {
                dtStatus = null;
                return lstitems;

            }
        }

        public static List<CirculationContactViewModel> GetCirculationContacts(string Department)
        {
            var contacts = new List<CirculationContactViewModel>();
            //string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //SqlConnection con = ClsConnection.GetConnection();
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("GetHouseandCommitteeMembers", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DeptId", SqlDbType.VarChar, 100).Value = Department;
                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        contacts.Add(new CirculationContactViewModel
                        {
                            OfficeType = rdr["OfficeType"]?.ToString(),
                            Name = rdr["Name"]?.ToString(),
                            Designation = rdr["Designation"]?.ToString(),
                            MobileNo = rdr["MobileNo"]?.ToString(),
                            EmailId = rdr["EmailId"]?.ToString()
                        });
                    }
                }
            }

            return contacts;
        }

        public void SaveOrUpdateCoverLetter(PdfCoverLetterViewModel model)
        {
            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SaveOrUpdateCoveringLetter", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PdfId", model.PdfId);
                    cmd.Parameters.AddWithValue("@DeptId", model.DeptId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CoverHtml", model.CoverHtml ?? (object)DBNull.Value);
                    //cmd.Parameters.AddWithValue("@ExistingPdfPath", model.ExistingPdfPath ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PreviewPdfPath", model.PreviewPdfPath ?? (object)DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

       

    }
    public class TemplateList
    {
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string TemplateSubjectEng { get; set; }
        public string TemplateSubjectLocal { get; set; }
        public string BodyTextEng { get; set; }
        public string BodyTextLocal { get; set; }
        public string SignStamp { get; set; }

    }
    public class Dashboard
    {
        public int DocumentTypeId { get; set; }
        public int DocOrder { get; set; }
        public string DocumentTypeName { get; set; }
        public int Count { get; set; }
        public string CssClass { get; set; }
        public string documentId { get; set; }

    }
    public class LstDashboard
    {
        public int InLetter { get; set; }
        public int InFile { get; set; }
        public int OutLetter { get; set; }
        public int OutFile { get; set; }
        public string Devicetype { get; set; }
        public int ArchLetter { get; set; }
        public int ArchFile { get; set; }
        public int agendaId { get; set; }

        public int IsFileCount { get; set; }
        public List<Dashboard> _LstDashboard { get; set; }

        public List<MCStat> _LstStat { get; set; }

        public string OutLineHeader { get; set; }

        public string Type { get; set; }


        public string MenuName { get; set; }

        public string MenuID { get; set; }
        public int RefMenutype { get; set; }
        public int FileMenuType { get; set; }

        public string RefMenuName { get; set; }

        public string RMenuName { get; set; }
        public string dashboardtype { get; set; }


        public string Meetings { get; set; }
        public string Resolutions { get; set; }
        public string Approved { get; set; }
        public string Rejected { get; set; }
        public string Pending { get; set; }

        public string Deferred { get; set; }


        public int ButtonPermission { get; set; }
        public int AgendaId { get; set; }
        public SelectList AgendaList { get; set; }
        public List<MCStatDetail> _LstStatDetail { get; set; }

        public List<MCStat> _LstStatOfficeWise { get; set; }

    }

    public class CirculationContactViewModel
    {
        public string OfficeType { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
    }

    public class PdfCoverLetterViewModel
    {
        public int PdfId { get; set; }

        public string DeptId { get; set; }

        public int AgendaId { get; set; }

        public string DocumentType { get; set; }

        [AllowHtml]
        public string CoverHtml { get; set; }

        // Optional: path to existing PDF, used internally if needed
        public DateTime MettingDate { get; set; }

        public string ExistingPdfPath { get; set; }
 
        // Optional: path to preview PDF (generated temporarily)
        public string PreviewPdfPath { get; set; }

        public List<PdfCoverLetterViewModel> clist = new List<PdfCoverLetterViewModel>();

    }

    public class CoveringLetter
    {
        public string PdfPath { get; set; }
    }

    public class MeetingType
    {
        public int MeetingTypeId { get; set; }
        public string MeetingTypeName { get; set; }
    }

    public class PDFCoverList
    {
        public static List<PdfCoverLetterViewModel> GetCoveringLettersByDeptId(string deptId)
        {
            List<PdfCoverLetterViewModel> list = new List<PdfCoverLetterViewModel>();

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetCoveringLetterByDeptId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeptId", deptId);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PdfCoverLetterViewModel model = new PdfCoverLetterViewModel
                            {
                                //PdfId = reader["PdfId"] != DBNull.Value ? Convert.ToInt32(reader["PdfId"]) : 0,
                                DeptId = reader["DeptId"]?.ToString(),
                                CoverHtml = reader["CoverHtml"]?.ToString(),
                                ExistingPdfPath = reader["ExistingPdfPath"]?.ToString(),
                                PreviewPdfPath = reader["PreviewPdfPath"]?.ToString()
                            };

                            list.Add(model);
                        }
                    }
                }
            }

            return list;
        }

        public static (string CoverHtml, string PdfPath) GetCoverHtmlByDeptId(string deptId, int AgendaId, string DocumentType)
        {
            string coverHtml = null;
            string pdfPath = null;

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetCoveringLetterByDeptId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeptId", deptId);
                    cmd.Parameters.AddWithValue("@AgendaId", AgendaId);
                    cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                    //cmd.Parameters.AddWithValue("@RefId", RefID);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            coverHtml = reader["CoverHtml"]?.ToString();
                            pdfPath = reader["PdfPath"]?.ToString();
                        }
                    }
                }
            }

            return (coverHtml, pdfPath);
        }


        public static bool InsertOrUpdateCoveringLetter(string deptId,int AgendaID, string DocumentType, string coverHtml, string path)
        {
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("InsertOrUpdate_CoveringLetter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DeptId", deptId);
                        cmd.Parameters.AddWithValue("@AgendaID", AgendaID);
                        cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                        cmd.Parameters.AddWithValue("@PdfPath", path);
                        cmd.Parameters.AddWithValue("@CoverHtml", coverHtml);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log the error
                return false;
            }
        }


        public static List<CoveringLetter> GetCoveringLetters(string deptId, int agendaId, string documentType)
        {
            var list = new List<CoveringLetter>();
            using (SqlConnection conn = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("GetCoveringLetterPdf", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DeptId", deptId);
                cmd.Parameters.AddWithValue("@AgendaId", agendaId);
                cmd.Parameters.AddWithValue("@DocumentType", documentType);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new CoveringLetter
                    {
                        PdfPath = reader["PdfPath"].ToString()
                    });
                }
            }

            return list;
        }

        public static void SavefileSolution(HttpPostedFileBase file, string folderpath, string filename)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    throw new ArgumentException("Invalid file provided.");
                }

                // Get the root path of the application (physical path)
                string rootPath = HttpContext.Current.Server.MapPath("~/"); // solution root

                // Combine with your custom folder path inside the solution (e.g., "UploadedFiles/")
                string fullFolderPath = Path.Combine(rootPath, folderpath);

                // Ensure the directory exists
                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                // Combine folder path with the desired filename
                string fullFilePath = Path.Combine(fullFolderPath, filename);

                // Save the file to the specified path
                file.SaveAs(fullFilePath);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine("Error saving file: " + ex);
            }
        }
    }

    public class FileMovementHistoryVM
    {
        public string SentFrom { get; set; }
        public string SentTo { get; set; }
        public DateTime SentDate { get; set; }
        public int DelayedDays { get; set; }

        public static List<FileMovementHistoryVM> GetFileMovementHistory(string refId)
        {
            List<FileMovementHistoryVM> list = new List<FileMovementHistoryVM>();
            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GetFileMovementHistory", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RefId", refId);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new FileMovementHistoryVM
                        {
                            SentFrom = dr["Sent From"].ToString(),
                            SentTo = dr["Sent To"].ToString(),
                            SentDate = Convert.ToDateTime(dr["Sent Date"]),
                            DelayedDays = Convert.ToInt32(dr["Delayed (Days)"])
                        });
                    }
                }
            }
            return list;
        }



    }

    public class MeetingTypeModel
    {
        public static List<MeetingType> GetMeetingTypes()
        {
            var list = new List<MeetingType>();

            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("GetMeetingTypes", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new MeetingType
                        {
                            MeetingTypeId = Convert.ToInt32(dr["MeetingTypeId"]),
                            MeetingTypeName = dr["MeetingTypeName"].ToString()
                        });
                    }
                }
            }
            return list;
        }
    }

}

public class Converter
{
    public static string ConvertTime(string timeString)
    {
        // Parse the time string into a DateTime object.
        DateTime dateTime = DateTime.Parse(timeString, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);

        // Get the time in the format "01:15:00 AM".
        CultureInfo culture = CultureInfo.GetCultureInfo("en-US");
        string formattedTime = dateTime.ToString("hh:mm tt", culture);

        return formattedTime;
    }
}