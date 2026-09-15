using CMNirdesh.Error;
using CMNirdesh.Models;
using CMNIrdesh.Models.Session;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
//using AngleSharp.Io;


namespace CMNirdesh.Models
{
    public class AgendaModel
    {
        public string AgendaId { get; set; }
        public string AgendaName { get; set; }
        public DateTime? AgendaDate { get; set; }

        public int MeetingType { get; set; }
        public string ChairPerson  { get; set; }
        public string ChairPersonDesignation { get; set; }

        public string RecordCount { get; set; }

        public string MeetingDate { get; set; }

        public string MeetingTypeName { get; set; }
        public string AgendaDateDisplay { get; set; }
        public string Remarks { get; set; }

        public int OfficeId { get; set; }
        public string DeptId { get; set; }
        public string DepartmentName { get; set; }
        public string OfficeName { get; set; }
        public int IsActive { get; set; }
        public int IsFreeze { get; set; }
        public string UserId { get; set; }
        public string AgendaPdf { get; set; }
        public string ProceedingPdf { get; set; }

        public string CabinetDecsions { get; set; }
        public string DecsionsToBeGenerated { get; set; }

        public SelectList mDepartmentList { get; set; }
        public SelectList OfficeList { get; set; }
        public List<AgendaModel> _LstAgenda { get; set; }
        public SelectList AgendaList { get; set; }
        public List<Councillorslist> Counselorslist { get; set; }

        public List<AgendaModel> Agendadecionlist;

        public List<MeetingNoticeAgenda> _LstMeetingNoticeAgenda { get; set; }

        public Int64 FileRefId { get; set; }
        public string RefId { get; set; }

        public string FromDeptId { get; set; }
      

        public string Agendatype { get; set; }
        public bool IsApprove { get; set; }
        public bool IsMatch { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Header { get; set; }

        public string Footer { get; set; }

        public string Status { get; set; }

        public string Venue { get; set; }

    }


    public class AgendaDetailModel
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

        public int ApprovalId { get; set; }

        public List<AgendaReferences> _LstReferences { get; set; }

        public List<AgendaReferences> _LstDataReferences { get; set; }

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
        public string CurrentDate   {  get; set;   }
        public string MultiUpdate { get; set; }

        public int ButtonPermission { get; set; }

        public SelectList ActionTypeList_Ref { get; set; }
    }

    public class AgendaReferences
    {
        public string Id { get; set; }
        public string RefId { get; set; }
        public string LetterNo { get; set; }
        public string FileNo { get; set; }
        public string LetterDate { get; set; }

        public string FromDeptID { get; set; }

        public string FromDept { get; set; }
        public string FromDeptLocal { get; set; }
        public string FromOffice { get; set; }
        public string FromOfficeLocal { get; set; }

        public string ToOffice { get; set; }
        public string DeptLogo { get; set; }
        public string DeptEmail { get; set; }
        public string DeptPhoneNo { get; set; }
        public string DeptMayor { get; set; }
        public string CommContactNo { get; set; }
        public string MayorContactNo { get; set; }
        public string DeptAddress { get; set; }
        public string DeptAddress_Local { get; set; }
        public string DeptWebsite { get; set; }

        public string yearno { get; set; }

        public string HouseDecision { get; set; }
        public string commissioner { get; set; }
        public string LGComments { get; set; }
        public string ActionType { get; set; }
        public string ActionDescription { get; set; }
        public string ActionBy { get; set; }
        public string Agendatitle { get; set; }
        public string ActionUserType { get; set; }

        public string ActionTakenDate { get; set; }
        public string meetingDate { get; set; }

        public string StatusName { get; set; }

        public string CreatedDate { get; set; }

        public string ReferencePriorityType { get; set; }
        public string ReferenceTerm { get; set; }

        public string Subject { get; set; }
        public string SubjectDetails { get; set; }
        public string MinisterName { get; set; }
        
        public string attachedfile { get; set; }

        public string Color { get; set; }

        public string DisplaySrNo { get; set; }

        public int SrNo { get; set; }
        public int SrNo1 { get; set; }

        public string StartResolution { get; set; }
        public string EndResolution { get; set; }

        public int SrNo2 { get; set; }
        public int SrNo3 { get; set; }

        public int Voting { get; set; }

        public string SerialNo { get; set; }
        public string SerialIndex { get; set; }
        public string SerialIndexTwo { get; set; }
        public string ResolutionNo { get; set; }
        public string AgendaFreeze { get; set; }
        public string ProceedingFreeze { get; set; }
        public string AgendaApprove { get; set; }
        public string AgendaPassed { get; set; }
        public string Remarks { get; set; }
        public string AgendaName { get; set; }
        public string AgendaDate { get; set; }
        public string StartTime { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string SendTo { get; set; }
        public string RejectionFormat { get; set; }
        public string Act { get; set; }
        public string FileRefId { get; set; }

        public string Reply1 { get; set; }
        public string Reply2 { get; set; }
        public string Reply3 { get; set; }
        public string Observation1 { get; set; }

        public string Observation2 { get; set; }
        public string Observation3 { get; set; }

        public string Endorsement { get; set; }
        

        public string UserDispactchNo { get; set; }

        public string TableItem { get; set; }
    }

    public class AdminLOB
    {
        public int AgendaId { get; set; }
        public int AgendaRecordId { get; set; }
        public int SrNo1 { get; set; }
        public int SrNo2 { get; set; }
        public int SrNo3 { get; set; }
        public DateTime SessionDate { get; set; }
        public string TextLOB { get; set; }
        public string pdflocation { get; set; }
        public int islaid { get; set; }
        public int isdeleted { get; set; }
        public string CreatedBy { get; set; }


    }


    public class VanueList
    {
        public string VenueName { get; set; }
        public int VenueId { get; set; }

    }

    public class Councillorslist
    {
        public string councillorname { get; set; }
        public int id { get; set; }
        public string meetingType { get; set; }

        public string wardname { get; set; }
        public string MobileNo { get; set; }

        public string Designation { get; set; }

        public string signature { get; set; }

        public List<Councillorslist> Councillor_list { get; set; }

        public string wardid { get; set; }
    }


    public class Authoritylist
    {
        public string name { get; set; }

        public string Prefix { get; set; }

        public string Designation { get; set; }

        public List<Authoritylist> AuthorityId_list { get; set; }

        public string Address { get; set; }
    }

    public class AdminLOBJson
    {
        public int AgendaRecordId { get; set; }
        public string Index { get; set; }
        public string IndexTwo { get; set; }
        public string AgendaId { get; set; }

    }

    //aman
    public class DeptDetails
    {
        public int DeptId { get; set; }
        public string Name { get; set; }
        public string logo { get; set; }
        public string Deptname { get; set; }

    }


    public class AgendaModelFunction
    {
        public static List<Authoritylist> getAuthoritylist(string deptid)
        {

            List<Authoritylist> lstagd = new List<Authoritylist>();
            DataSet ds = new DataSet();
            ds = GetAuthorityList(deptid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Authoritylist fr = new Authoritylist();
                fr.name = dr["Name"].ToString();
                fr.Designation = dr["Designation"].ToString();
                fr.Address = dr["Address"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;
        }

        //GetMemoDetail
        public static List<AgendaReferences> GetMemoDetails(string ReferenceId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            ds = GetMemoDetail(ReferenceId);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.FromDept = dr["deptname"].ToString();
                fr.MinisterName = dr["MinistryName"].ToString();
                fr.FromOffice = dr["officename"].ToString();
                fr.Subject = dr["Subject"].ToString();
                fr.SubjectDetails = dr["SubjectDetails"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;
        }


        public static List<AgendaReferences> GetRefData(string ReferenceId)
        {

            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            ds = GetReffData(ReferenceId);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                 fr.Subject = dr["Subject"].ToString();
                 fr.SubjectDetails = dr["SubjectDetails"].ToString();
                
                lstagd.Add(fr);
            }
            return lstagd;
        }

        public static DeptDetails GetDeptData(string deptid)
        {

            DeptDetails dpt = new DeptDetails();
            DataSet ds = new DataSet();
            ds = GetDeptDetails(deptid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {                
                dpt.Name = dr["MCName"].ToString();
                dpt.logo = dr["MClogo"].ToString();
                dpt.Deptname= dr["deptname"].ToString();

            }
            return dpt;
        }
        public static DataSet GetReffData(string ReferenceId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            //p.Add(new SqlParameter("@HouseType", 1));
            p.Add(new SqlParameter("@refid", Convert.ToString(ReferenceId)));
            //p.Add(new SqlParameter("@AgendaId", 1));
            DataSet ds = SqlHelper.ExecuteDataset(connection, "GetRefData", p.ToArray());
            if (ds.Tables.Count == 0)
            {
                return null;
            }

            return ds;
        }

        public static DataSet GetMemoDetail(string ReferenceId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@refid", Convert.ToString(ReferenceId)));
            DataSet ds = SqlHelper.ExecuteDataset(connection, "getMemoDetail", p.ToArray());
            if (ds.Tables.Count == 0)
            {
                return null;
            }

            return ds;
        }


        public static DataSet LGPdfDiaried(string ReferenceId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            //p.Add(new SqlParameter("@HouseType", 1));
            p.Add(new SqlParameter("@ReferenceId", Convert.ToString(ReferenceId)));
            //p.Add(new SqlParameter("@AgendaId", 1));
            DataSet ds = SqlHelper.ExecuteDataset(connection, "LGProceddingReply", p.ToArray());
            if (ds.Tables.Count == 0)
            {
                return null;
            }

            return ds;
        }


        public static DataSet GetAuthorityList(string deptid)
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

                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@office", deptid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMCControllingAuthority_Sel]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        public static DataSet GetDeptDetails(string deptid)
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

                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@deptid", deptid) };
                ds = SqlHelper.ExecuteDataset(connection, "[sp_getdeptdetails]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        public static DataTable GetAgendaFiles(string agendaid)
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

                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@agendaid", agendaid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetAgendaFiles]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds.Tables[0];
        }

        public static List<Councillorslist> getCouncillirslist(string deptid, int agendaid)
        {

            List<Councillorslist> lstagd = new List<Councillorslist>();
            DataSet ds = new DataSet();
            ds = GetCouncillorsList(deptid, agendaid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Councillorslist fr = new Councillorslist();
                fr.councillorname = dr["Name"].ToString();
                fr.wardid = dr["WardId"].ToString();
                fr.meetingType = dr["meetingType"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;
        }

        public static List<Councillorslist> getPresentCouncillirslist(int mcid, int agendaid)
        {

            List<Councillorslist> lstagd = new List<Councillorslist>();
            DataSet ds = new DataSet();
            ds = GetPresentCouncillorsList(mcid, agendaid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Councillorslist fr = new Councillorslist();
                fr.councillorname = dr["Name"].ToString();
                fr.wardid = dr["WardId"].ToString();
                fr.meetingType = dr["meetingType"].ToString();
                fr.Designation = dr["Designation"].ToString();
                fr.signature = dr["SignaturePath"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;
        }
        public static List<Councillorslist> getDeptWiseCouncillorslist(string deptid,int agendaid)
        {

            List<Councillorslist> lstagd = new List<Councillorslist>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCid", deptid), new SqlParameter("@meetingid", agendaid) };
            ds = SqlHelper.ExecuteDataset(connection, "[GetMembers_list]", parameterValues);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Councillorslist fr = new Councillorslist();
                fr.councillorname = dr["Name"].ToString();
                fr.id = Convert.ToInt32(dr["id"]);
                fr.wardid = dr["WardId"].ToString();
                fr.meetingType = dr["meetingType"].ToString();
                fr.MobileNo = dr["Mobile"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;
        }
        public static string SaveAgenda(AgendaModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlCommand command = new SqlCommand("[InsertAgenda]", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AgendaName", model.AgendaName);
                command.Parameters.AddWithValue("@AgendaDate", model.AgendaDate);
                command.Parameters.AddWithValue("@OfficeId", Convert.ToInt32(model.OfficeId));
                command.Parameters.AddWithValue("@DeptId", model.DeptId);
                command.Parameters.AddWithValue("@UserId", model.UserId);
                command.Parameters.AddWithValue("@Agendatype", model.Agendatype);
                command.Parameters.AddWithValue("@StartTime", model.StartTime);

                command.Parameters.AddWithValue("@Header", model.Header);
                command.Parameters.AddWithValue("@Footer", model.Footer);
                SqlParameter parmOUT = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
                parmOUT.Direction = ParameterDirection.Output;
                command.Parameters.Add(parmOUT);
                command.ExecuteNonQuery();
                int returnCode = (int)command.Parameters["@RETURN_VALUE"].Value;

                //SqlParameter returnValue = command.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                //returnValue.Direction = ParameterDirection.ReturnValue;

                //command.ExecuteScalar();

                //int returnCode = (int)returnValue.Value;
                connection.Close();

                if (returnCode == 0)
                {
                    // Success
                    return "Success";
                }
                else if (returnCode == 1)
                {
                    // HouseId is null
                    return "HouseId is null";
                }
                else if (returnCode == 2)
                {
                    // HouseId is null
                    return "Meeting Already Exists";
                }
                else
                {
                    // Handle other return codes if needed
                    return "Other return code: " + returnCode.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return "Error: " + ex.Message;
            }
        }


        public static string SaveMeeting(AgendaModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlCommand command = new SqlCommand("[CreateMeetingUpdated]", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AgendaName", model.AgendaName);
                command.Parameters.AddWithValue("@AgendaDate", model.AgendaDate);
                command.Parameters.AddWithValue("@OfficeId", Convert.ToInt32(model.OfficeId));
                command.Parameters.AddWithValue("@DeptId", model.DeptId);
                command.Parameters.AddWithValue("@UserId", model.UserId);
                command.Parameters.AddWithValue("@Agendatype", model.Agendatype);
                command.Parameters.AddWithValue("@StartTime", model.StartTime);

                command.Parameters.AddWithValue("@Header", model.Header);
                command.Parameters.AddWithValue("@Footer", model.Footer);
                command.Parameters.AddWithValue("@MeetingType", model.MeetingType);

                command.Parameters.AddWithValue("@Chairperson", model.ChairPerson);
                command.Parameters.AddWithValue("@ChairpersonDesignation", model.ChairPersonDesignation);


                command.Parameters.AddWithValue("@FileRefId", model.FileRefId);
                SqlParameter parmOUT = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
                parmOUT.Direction = ParameterDirection.Output;
                command.Parameters.Add(parmOUT);
                command.ExecuteNonQuery();
                int returnCode = (int)command.Parameters["@RETURN_VALUE"].Value;

                //SqlParameter returnValue = command.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                //returnValue.Direction = ParameterDirection.ReturnValue;

                //command.ExecuteScalar();

                //int returnCode = (int)returnValue.Value;
                connection.Close();

                    
                return returnCode.ToString();
                               
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return "Error: " + ex.Message;
            }
        }

        public static string GetAgendaPdf(int AgendaId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                };
                object result = SqlHelper.ExecuteScalar(connection, "GetAgendaPdf2", parameterValues);
                string refid = Convert.ToString(result);

                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string GetCommissionerPdf(int AgendaId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                };
                object result = SqlHelper.ExecuteScalar(connection, "GetCommissionerPdf", parameterValues);
                string refid = Convert.ToString(result);

                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string GetUnsignedPdf(int AgendaId, string DocumentType, string ApprovalID="")
        {
            try
            {
                //ApprovalID = CryptoHelper.Decrypt(HttpUtility.UrlDecode(ApprovalID));
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                      new SqlParameter("@SignType", DocumentType),
                      new SqlParameter("@ApprovalId", ApprovalID),
                };
                object result = SqlHelper.ExecuteScalar(connection, "GetUnSignedPdfTest", parameterValues);
                string refid = Convert.ToString(result);

                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }



        public static List<AgendaModel> GetDataForDecisionList(string agendaid)
        {
            List<AgendaModel> lst = new List<AgendaModel>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@agendaid", agendaid);
                con.Close();
                DataSet ds = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "[GetDataByDeptRecord]", param);





                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AgendaModel agenda = new AgendaModel();
                    // Map the data from the DataRow to the Circular object

                   // agenda.DepartmentName = Convert.ToString(dr["deptname"]);
                   // agenda.OfficeName = Convert.ToString(dr["officename"]);
                    agenda.AgendaName = Convert.ToString(dr["AgendaName"]);
                    agenda.AgendaDateDisplay = Convert.ToDateTime(dr["AgendaDate"]).ToString("dd/MM/yyyy");
                   // agenda.RefId = Convert.ToString(dr["FileRefId"]);
                    agenda.RecordCount = Convert.ToString(dr["RecordCount"]);
                    //click

                    agenda.AgendaId = Convert.ToString(dr["AgendaId"]);
                    //agenda.FromDeptId = Convert.ToString(dr["FromDeptId"]);

                    agenda.AgendaPdf = dr["AgendaPdf"].ToString();
                    agenda.ProceedingPdf = dr["ProceedingPdf"].ToString();
                    agenda.CabinetDecsions = dr["ApprovedCount"].ToString();
                    agenda.DecsionsToBeGenerated = dr["NotApprovedCount"].ToString();
                    lst.Add(agenda);
                }

                return lst;
            }
            catch (Exception ex)
            {
                // Log the error
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lst; // Return an empty list in case of error
            }
        }

        public static List<MeetingNoticeAgenda> GetAgendaForMeetingNotice()
        {

            List<MeetingNoticeAgenda> lstagd = new List<MeetingNoticeAgenda>();
            DataSet ds = new DataSet();
            ds = GetAgendaMeetingNoticeList();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                MeetingNoticeAgenda fr = new MeetingNoticeAgenda();
                fr.AgendaID = Convert.ToInt16(dr["id"]);
                fr.MeetingName = dr["MeetingName"].ToString();
                fr.MainFileRefID = dr["MainFileRefID"].ToString();
                fr.PartFileRefID = dr["ProceedingID"].ToString();
                //fr.p = dr["PartFileRefID"].ToString();
                fr.TotalMemo = Convert.ToInt32(dr["TotalMemo"]);
                fr.MeetingDate = dr["MeetingDate"] != DBNull.Value ? Convert.ToDateTime(dr["MeetingDate"]) : DateTime.MinValue; // Default to MinValue or handle differently
                fr.StartTime = dr["StartTime"] != DBNull.Value ? Convert.ToString(dr["StartTime"]) : string.Empty; // Default to empty string
                fr.EndTime = dr["EndTime"] != DBNull.Value ? Convert.ToString(dr["EndTime"]) : string.Empty;
                fr.IsEdit = Convert.ToBoolean(dr["IsEdit"]);
                fr.Noticepath = Convert.ToString(dr["noticepath"]);
                fr.fileaccessingurl = BlobStorage.GetStorageAcessingPath();
                fr.Venue = Convert.ToString(dr["VenueName"]);
                //string startTimeValue = Convert.ToString(dr["StartTime"]);
                //TimeSpan startTime;
                //if (TimeSpan.TryParse(startTimeValue, out startTime))
                //{
                //    fr.StartTime = Converter.ConvertTime(Convert.ToString(startTime));
                //};

                //string endTimeValue = Convert.ToString(dr["EndTime"]);
                //TimeSpan endTime;
                //if (TimeSpan.TryParse(endTimeValue, out endTime))
                //{
                //    fr.EndTime = Converter.ConvertTime(Convert.ToString(endTime));
                //};
                lstagd.Add(fr);
            }
            return lstagd;

        }

        public static List<MeetingNoticeAgenda> GetAgendaForMeetingNoticeEbook()
        {

            List<MeetingNoticeAgenda> lstagd = new List<MeetingNoticeAgenda>();
            DataSet ds = new DataSet();
            ds = GetAgendaMeetingNoticeToBookList();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                MeetingNoticeAgenda fr = new MeetingNoticeAgenda();
                fr.AgendaID = Convert.ToInt16(dr["id"]);
                //fr.Sr = Convert.ToInt16(dr["sr"]);
                fr.MeetingName = dr["MeetingName"].ToString();
                fr.MainFileRefID = dr["RefId"].ToString();
                fr.TotalMemo = Convert.ToInt32(dr["TotalMemo"]);
                fr.MeetingDate = dr["MeetingDate"] != DBNull.Value ? Convert.ToDateTime(dr["MeetingDate"]) : DateTime.MinValue; // Default to MinValue or handle differently
                fr.StartTime = dr["StartTime"] != DBNull.Value ? Convert.ToString(dr["StartTime"]) : string.Empty; // Default to empty string
                fr.EndTime = dr["EndTime"] != DBNull.Value ? Convert.ToString(dr["EndTime"]) : string.Empty;
                fr.IsEdit = Convert.ToBoolean(dr["IsEdit"]);
                fr.Noticepath = Convert.ToString(dr["noticepath"]);
                fr.fileaccessingurl = BlobStorage.GetStorageAcessingPath();
                fr.Venue = Convert.ToString(dr["VenueName"]);
                fr.IsLinkEbook = Convert.ToBoolean(dr["IsLinkEbook"]);
                fr.CurrentlyWith= Convert.ToString(dr["CurrentlyWith"]);
                fr.Agendafile = Convert.ToString(dr["agenda"]);
               // fr.Filetype = Convert.ToString(dr["FileTypeMerged"]);

                lstagd.Add(fr);
            }
            return lstagd;

        }

        public static List<AgendaModel> GetAgenda(int OfficeId, string AgendaType)
        {

            List<AgendaModel> lstagd = new List<AgendaModel>();
            DataSet ds = new DataSet();
            ds = GetAgendaList(OfficeId, AgendaType);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                AgendaModel fr = new AgendaModel();
                fr.AgendaId = dr["Id"].ToString();
                fr.AgendaName = dr["AgendaName"].ToString();
                string agendadate = Convert.ToString(dr["AgendaDate"]);
                if (agendadate !="")
                {
                    fr.AgendaDateDisplay = Convert.ToDateTime(dr["AgendaDate"]).ToString("dd/MM/yyyy");
                }
                
                fr.Remarks = dr["Remarks"].ToString();
                fr.OfficeId = Convert.ToInt32(dr["OfficeId"].ToString());
                fr.DeptId = dr["DeptId"].ToString();
                fr.UserId = dr["CreatedBy"].ToString();
                fr.IsFreeze = Convert.ToInt32(dr["IsFreeze"].ToString());
                fr.IsApprove = Convert.ToBoolean(dr["isapprove"]);
                fr.IsMatch = Convert.ToBoolean(dr["IsMatch"]);
                fr.Header = Convert.ToString(dr["header"]);
                fr.Footer = Convert.ToString(dr["footer"]);
                fr.Agendatype = Convert.ToString(dr["AgendaType"]);

                string startTimeValue = Convert.ToString(dr["StartTime"]);
                TimeSpan startTime;
                if (TimeSpan.TryParse(startTimeValue, out startTime))
                {
                    fr.StartTime = Converter.ConvertTime(Convert.ToString(startTime));
                };

                string endTimeValue = Convert.ToString(dr["EndTime"]);
                TimeSpan endTime;
                if (TimeSpan.TryParse(endTimeValue, out endTime))

                {
                    fr.EndTime = Converter.ConvertTime(Convert.ToString(endTime));
                };
                lstagd.Add(fr);
            }
            return lstagd;

        }

        public static List<AgendaModel> GetAgendabyAgendaId(int AgendaId)
        {

            List<AgendaModel> lstagd = new List<AgendaModel>();
            DataSet ds = new DataSet();
            ds = GetAgendabyId(AgendaId);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                AgendaModel fr = new AgendaModel();
                fr.AgendaId = dr["Id"].ToString();
                fr.AgendaName = dr["AgendaName"].ToString();
                string agendadate = Convert.ToString(dr["AgendaDate"]);
                if (agendadate != "")
                {
                    fr.AgendaDateDisplay = Convert.ToDateTime(dr["AgendaDate"]).ToString("dd/MM/yyyy");
                }

                fr.Remarks = dr["Remarks"].ToString();
                fr.OfficeId = Convert.ToInt32(dr["OfficeId"].ToString());
                fr.DeptId = dr["DeptId"].ToString();
                fr.UserId = dr["CreatedBy"].ToString();
                fr.IsFreeze = Convert.ToInt32(dr["IsFreeze"].ToString());
                fr.IsApprove = Convert.ToBoolean(dr["isapprove"]);
                //fr.IsMatch = Convert.ToBoolean(dr["IsMatch"]);
                fr.Header = Convert.ToString(dr["header"]);
                fr.Footer = Convert.ToString(dr["footer"]);
                fr.Agendatype = Convert.ToString(dr["AgendaType"]);

                string startTimeValue = Convert.ToString(dr["StartTime"]);
                TimeSpan startTime;
                if (TimeSpan.TryParse(startTimeValue, out startTime))
                {
                    fr.StartTime = Converter.ConvertTime(Convert.ToString(startTime));
                };

                string endTimeValue = Convert.ToString(dr["EndTime"]);
                TimeSpan endTime;
                if (TimeSpan.TryParse(endTimeValue, out endTime))

                {
                    fr.EndTime = Converter.ConvertTime(Convert.ToString(endTime));
                };
                lstagd.Add(fr);
            }
            return lstagd;

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

        public static DataSet GetCouncillorsList(string deptid, int agendaid)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCid", deptid), new SqlParameter("@meetingid", agendaid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMembers_list]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }
        public static bool CheckHouseExists(string mcCode)
        {
            bool isExists = false;

            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    SqlParameter[] parameters =
                    {
                new SqlParameter("@MCCode", mcCode)
            };

                    object result = SqlHelper.ExecuteScalar(
                                        connection,
                                        CommandType.StoredProcedure,
                                        "sp_CheckHouseExists",
                                        parameters
                                    );

                    if (result != null && result != DBNull.Value)
                    {
                        isExists = Convert.ToInt32(result) == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(
                    ex,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString()
                );
            }

            return isExists;
        }



        public static DataSet GetPresentCouncillorsList(int mcid, int agendaid)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@MCid", mcid), new SqlParameter("@meetingid", agendaid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetPesentMembers_list]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        
        public static DataSet GetAgendaList(int OfficeId, string AgendaType)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@AgendaType", AgendaType) };
                ds = SqlHelper.ExecuteDataset(connection, "[getAgendaList]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        public static DataSet GetAgendaMeetingNoticeList()
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@Department", CurrentSession.DeptID) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMeetingNoticeAgendaNew]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        public static DataSet GetAgendaMeetingNoticeToBookList()
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@Department", CurrentSession.DeptID)};
                ds = SqlHelper.ExecuteDataset(connection, "[GetAgendaMeetingNoticeToBookListTest]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }



        public static DataSet GetAgendabyId(int AgendaId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
                ds = SqlHelper.ExecuteDataset(connection, "[getAgendabyId]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }

        public static SelectList GetAgendaSelectList(string OfficeId, string freeze)
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
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@freeze", freeze) };
                ds = SqlHelper.ExecuteDataset(connection, "[getunFreezeAgendaList]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToDateTime(row["AgendaDate"]).ToString("dd/MM/yyyy") + ": " + Convert.ToString(row["AgendaName"].ToString());
                    //item.Text = Convert.ToDateTime(row["AgendaDate"]).ToString("dd/MM/yyyy") + ": " + Convert.ToString(row["AgendaName"].ToString() + " [" + Convert.ToString(row["AgendaType"].ToString()) + "]");
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

        public static int GetMenuButtonPermission(string userid, string Menuid)
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
            p.Add(new SqlParameter("@Menuid", Menuid));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetUserMenuPermission", p.ToArray());
            if (dtt.Tables[0].Rows.Count > 0)
            {
                int ButtonPermission = Convert.ToInt32(dtt.Tables[0].Rows[0][3]);
                return ButtonPermission;

            }
            else
            {
                return 0;
            }
         
        }

        public static SelectList GetActList()
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
                DataSet ds = new DataSet();
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@OfficeId", OfficeId), new SqlParameter("@freeze", freeze) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetAct]", null);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = row["ActName"].ToString();
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
        public static List<AgendaReferences> GetDiaryReferences(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            ds = GetDiaryReferencesData(AgendaId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();

                fr.Id = dr["AgendaDetailId"].ToString();
                fr.Subject = dr["Subject"].ToString();
                fr.LetterDate = dr["LetterDate"].ToString();
                fr.LetterNo = dr["LetterNo"].ToString();
                fr.FromOffice = dr["FromDept"].ToString() + " : " + dr["FromOffice"].ToString();
                fr.ActionDescription = dr["ad"].ToString();
                fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                fr.StatusName = dr["StatusName"].ToString();
                fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                fr.attachedfile = dr["ActionTakenFile"].ToString();
                fr.RefId = dr["RefId"].ToString();
                fr.CreatedDate = dr["CreatedDate"].ToString();
                fr.ReferencePriorityType = dr["ReferencePriorityType"].ToString();
                fr.SubjectDetails = dr["SubjectDetails"].ToString();
                if (fr.ReferencePriorityType == "1")
                {
                    fr.ReferencePriorityType = "High";
                }
                else if (fr.ReferencePriorityType == "2")
                {
                    fr.ReferencePriorityType = "Medium";
                }
                else if (fr.ReferencePriorityType == "3")
                {
                    fr.ReferencePriorityType = "Low";
                }

                fr.ReferenceTerm = dr["ReferenceTerm"].ToString();

                if (fr.ReferenceTerm == "1")
                {
                    fr.ReferenceTerm = "Short";
                }
                else if (fr.ReferenceTerm == "2")
                {
                    fr.ReferenceTerm = "Medium";
                }
                else if (fr.ReferenceTerm == "3")
                {
                    fr.ReferenceTerm = "Long";
                }

                //fr.SrNo1 = Convert.ToInt32(dr["SrNo1"].ToString());
                //fr.SrNo2 = Convert.ToInt32(dr["SrNo2"].ToString());  
                //fr.SrNo3 = Convert.ToInt32(dr["SrNo3"].ToString()); 
                //if(fr.SrNo1 != 0)
                //{
                //    fr.SerialIndex = fr.SrNo1.ToString();
                //}
                //if (fr.SrNo2 != 0)
                //{
                //    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo2;
                //}
                //if (fr.SrNo3 != 0)
                //{
                //    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo3;
                //}


                fr.SerialIndex = dr["SrNo"].ToString();
                fr.Voting = Convert.ToInt32(dr["voting"]);

                //fr.Id = dr["Id"].ToString();




                //fr.SubjectDetails = dr["NewSubjectDetails"].ToString();
                if (fr.RefId == "0")
                {
                    if (dr["attachedfile"].ToString() != "")
                    {
                        fr.attachedfile = "\\mlaDiary\\" + dr["attachedfile"].ToString();
                    }
                    else
                    {
                        fr.attachedfile = dr["ActionTakenFile"].ToString();
                    }

                }

                lstagd.Add(fr);
            }
            return lstagd;

        }

        public static DataSet GetDiaryReferencesData(int AgendaId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
                ds = SqlHelper.ExecuteDataset(connection, "[getDiaryAgendaReferences]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }

        public static List<AgendaReferences> GetFileReferences(Int64 RefId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataTable ds = new DataTable();
            DiaryViewModel dvm = new DiaryViewModel();
            ds = dvm.GetFileRef(Convert.ToInt64(RefId));

            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    AgendaReferences fr = new AgendaReferences();

                   
                    fr.Subject = dr["Subject"].ToString();
                    fr.LetterDate = dr["LetterDate"].ToString();
                    fr.LetterNo = dr["LetterNo"].ToString();
                    fr.FromOffice = dr["ToDept"].ToString() + " : " + dr["ToOffice"].ToString();
                    fr.RefId = dr["RefId"].ToString();
                    fr.CreatedDate = dr["CreatedDate"].ToString();
                    fr.SubjectDetails = dr["SubjectDetails"].ToString();
                    fr.StatusName = dr["StatusName"].ToString();
                    fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                    fr.attachedfile = dr["ActionTakenFile"].ToString();
                    fr.LetterNo = dr["LetterNo"].ToString();
                    if (fr.RefId == "0")
                    {
                        if (dr["attachedfile"].ToString() != "")
                        {
                            fr.attachedfile = "/MlaDiary/" + dr["attachedfile"].ToString();
                        }
                        else
                        {
                            fr.attachedfile = dr["ActionTakenFile"].ToString();
                        }

                    }

                    fr.Reply1= dr["Rp1"].ToString();
                    fr.Reply2 = dr["Rp2"].ToString();
                    fr.Reply3 = dr["Rp3"].ToString();

                    fr.Observation1 = dr["ob1"].ToString();
                    fr.Observation2 = dr["ob2"].ToString();
                    fr.Observation3 = dr["ob3"].ToString();

                    lstagd.Add(fr);
                }
            }

            return lstagd;

        }

        public static List<AgendaReferences> GetDishpatchReferences(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            ds = GetDispatchReferencesData(AgendaId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AgendaReferences fr = new AgendaReferences();

                    fr.Id = dr["AgendaDetailId"].ToString();
                    fr.Subject = dr["Subject"].ToString();
                    fr.LetterDate = dr["LetterDate"].ToString();
                    fr.LetterNo = dr["LetterNo"].ToString();
                    fr.ToOffice = dr["ToDept"].ToString() + " : " + dr["ToOffice"].ToString();
                    fr.FromOffice = dr["FromDept"].ToString() + " : " + dr["FromOffice"].ToString();
                    fr.ActionDescription = dr["ad"].ToString();
                    fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                    fr.StatusName = dr["StatusName"].ToString();
                    fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                    fr.attachedfile = dr["ActionTakenFile"].ToString();
                    fr.RefId = dr["RefId"].ToString();
                    fr.CreatedDate = dr["CreatedDate"].ToString();
                    fr.ReferencePriorityType = dr["ReferencePriorityType"].ToString();
                    fr.SubjectDetails = dr["SubjectDetails"].ToString();
                    fr.AgendaFreeze = dr["isFreeze"].ToString();
                    fr.AgendaApprove = dr["isapprove"].ToString();
                    fr.AgendaPassed = dr["isPassed"].ToString();
                    if (fr.ReferencePriorityType == "1")
                    {
                        fr.ReferencePriorityType = "High";
                    }
                    else if (fr.ReferencePriorityType == "2")
                    {
                        fr.ReferencePriorityType = "Medium";
                    }
                    else if (fr.ReferencePriorityType == "3")
                    {
                        fr.ReferencePriorityType = "Low";
                    }

                    fr.ReferenceTerm = dr["ReferenceTerm"].ToString();

                    if (fr.ReferenceTerm == "1")
                    {
                        fr.ReferenceTerm = "Short";
                    }
                    else if (fr.ReferenceTerm == "2")
                    {
                        fr.ReferenceTerm = "Medium";
                    }
                    else if (fr.ReferenceTerm == "3")
                    {
                        fr.ReferenceTerm = "Long";
                    }

                    fr.FileRefId= dr["FileRefId"].ToString();
                    if (fr.FileRefId == "0")
                    {
                        fr.FileRefId = "";
                    }

                    //fr.SrNo1 = Convert.ToInt32(dr["SrNo1"].ToString());
                    //fr.SrNo2 = Convert.ToInt32(dr["SrNo2"].ToString());
                    //fr.SrNo3 = Convert.ToInt32(dr["SrNo3"].ToString());
                    //if (fr.SrNo1 != 0)
                    //{
                    //    fr.SerialIndex = fr.SrNo1.ToString();
                    //}
                    //if (fr.SrNo2 != 0)
                    //{
                    //    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo2;
                    //}
                    //if (fr.SrNo3 != 0)
                    //{
                    //    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo3;
                    //}
                    //fr.Id = dr["Id"].ToString();

                    fr.SerialIndex = dr["SrNo"].ToString();
                    fr.SerialIndexTwo = dr["SrNo2"].ToString();
                    fr.ResolutionNo = dr["ResolutionNo"].ToString();
                    fr.Voting = Convert.ToInt32(dr["voting"]);


                    //fr.SubjectDetails = dr["NewSubjectDetails"].ToString();
                    if (fr.RefId == "0")
                    {
                        if (dr["attachedfile"].ToString() != "")
                        {
                            fr.attachedfile = "/MlaDiary/" + dr["attachedfile"].ToString();
                        }
                        else
                        {
                            fr.attachedfile = dr["ActionTakenFile"].ToString();
                        }

                    }

                    lstagd.Add(fr);
                }
            }

            return lstagd;

        }

        public static DataSet GetDispatchReferencesData(int AgendaId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
                ds = SqlHelper.ExecuteDataset(connection, "[getDispatchAgendaReferences]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }


        public static List<AgendaReferences> GetApproveReferences(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            ds = GetApproveReferencesData(AgendaId);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AgendaReferences fr = new AgendaReferences();

                    fr.Id = dr["AgendaDetailId"].ToString();
                    fr.Subject = dr["Subject"].ToString();
                    fr.LetterDate = dr["LetterDate"].ToString();
                    fr.meetingDate = dr["meetingDate"].ToString();
                    fr.LetterNo = dr["LetterNo"].ToString();
                    fr.ToOffice = dr["ToDept"].ToString() + " " + dr["ToOffice"].ToString();
                    fr.FromOffice = dr["FromDept"].ToString() + " " + dr["FromOffice"].ToString();
                    fr.ActionDescription = dr["ad"].ToString();
                    fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                    fr.StatusName = dr["StatusName"].ToString();
                    fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                    fr.attachedfile = dr["ActionTakenFile"].ToString();
                    fr.RefId = dr["RefId"].ToString();
                    fr.CreatedDate = dr["CreatedDate"].ToString();
                    fr.ReferencePriorityType = dr["ReferencePriorityType"].ToString();
                    fr.SubjectDetails = dr["SubjectDetails"].ToString();
                    fr.AgendaFreeze = dr["isFreeze"].ToString();
                    fr.AgendaApprove = dr["isapprove"].ToString();
                    fr.AgendaPassed = dr["isPassed"].ToString();
                    fr.ProceedingFreeze= dr["FreezeProceeding"].ToString();
                    if (fr.ReferencePriorityType == "1")
                    {
                        fr.ReferencePriorityType = "High";
                    }
                    else if (fr.ReferencePriorityType == "2")
                    {
                        fr.ReferencePriorityType = "Medium";
                    }
                    else if (fr.ReferencePriorityType == "3")
                    {
                        fr.ReferencePriorityType = "Low";
                    }

                    fr.ReferenceTerm = dr["ReferenceTerm"].ToString();

                    if (fr.ReferenceTerm == "1")
                    {
                        fr.ReferenceTerm = "Short";
                    }
                    else if (fr.ReferenceTerm == "2")
                    {
                        fr.ReferenceTerm = "Medium";
                    }
                    else if (fr.ReferenceTerm == "3")
                    {
                        fr.ReferenceTerm = "Long";
                    }

                    fr.Color = dr["AgendaAction"].ToString();
                    if (fr.Color == "3")
                    {
                        fr.Color = "AgendaAction";
                    }
                    else if (fr.Color == "2")
                    {
                        fr.Color = "ActionTakenCancel";
                    }
                    else
                    {
                        fr.Color = "";
                    }
                     
                    //fr.SrNo1 = Convert.ToInt32(dr["SrNo1"].ToString());
                    //fr.SrNo2 = Convert.ToInt32(dr["SrNo2"].ToString());
                    //fr.SrNo3 = Convert.ToInt32(dr["SrNo3"].ToString());
                    //if (fr.SrNo1 != 0)
                    //{
                    //    fr.SerialIndex = fr.SrNo1.ToString();
                    //}
                    //if (fr.SrNo2 != 0)
                    //{
                    //    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo2;
                    //}
                    //if (fr.SrNo3 != 0)
                    //{
                    //    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo3;
                    //}
                    //fr.Id = dr["Id"].ToString();

                    fr.SerialIndex = dr["SrNo"].ToString();
                    
                    if (dr["SubResolutionNo"].ToString() != "0")
                    {
                        fr.ResolutionNo = dr["ResolutionNo"].ToString() + "."+ dr["SubResolutionNo"].ToString();
                    }
                    else
                    {
                        fr.ResolutionNo = dr["ResolutionNo"].ToString();
                    }
                    fr.Voting = Convert.ToInt32(dr["voting"]);
                    fr.Act = dr["ActName"].ToString();

                    //fr.SubjectDetails = dr["NewSubjectDetails"].ToString();
                    if (fr.RefId == "0")
                    {
                        if (dr["attachedfile"].ToString() != "")
                        {
                            fr.attachedfile = "/MlaDiary/" + dr["attachedfile"].ToString();
                        }
                        else
                        {
                            fr.attachedfile = dr["ActionTakenFile"].ToString();
                        }

                    }
                    fr.FileRefId = dr["FileRefId"].ToString();
                    if (fr.FileRefId == "0")
                    {
                        fr.FileRefId = "";
                    }

                    lstagd.Add(fr);
                }
            }

            return lstagd;

        }

        public static DataSet GetApproveReferencesData(int AgendaId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
                ds = SqlHelper.ExecuteDataset(connection, "[getApproveAgendaReferences]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }

        public static string SaveNewRefAgenda(AgendaDetailModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", model.AgendaId),
                    new SqlParameter("@Subject",model.NewSubject),
                    new SqlParameter("@attachfile", model.attachfile),
                    new SqlParameter("@createdby", model.Createdby),
                    new SqlParameter("@SubjectDetails", model.NewSubjectDetails),
                    new SqlParameter("@LetterNo", model.NewLetterNo),

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertNewAgenda]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string EditAgendaDetails(AgendaDetailModel model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaDetailId", model.AgendaDetailId),
                    new SqlParameter("@Subject", model.NewSubject),
                    new SqlParameter("@attachfile", model.attachfile),
                    new SqlParameter("@SubjectDetails", model.NewSubjectDetails),
                    new SqlParameter("@LetterNo", model.NewLetterNo),

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateAgendaDetails]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static DataSet GetAgendaRecord(int AgendaRecordId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaRecordId", AgendaRecordId) };
                ds = SqlHelper.ExecuteDataset(connection, "[getAgendaRecord]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }



        public static List<VanueList> getVanueList()
        {

            List<VanueList> lstagd = new List<VanueList>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            ds = SqlHelper.ExecuteDataset(connection, "[GetMeetingVenue]", null);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                VanueList fr = new VanueList();
                fr.VenueId = Convert.ToInt32(dr["VenueId"]);
                fr.VenueName = dr["VenueName"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;
        }


        public static string FreezeRecord(int AgdId, int index, int indexTwo, int order)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@RecordId", AgdId),
                    new SqlParameter("@index", index),
                    new SqlParameter("@indexTwo", indexTwo),
                    new SqlParameter("@order", order),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[SaveFileIndex]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string FreezeProceeding(int AgdId, int index, int Subindex ,int order)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@RecordId", AgdId),
                    new SqlParameter("@Index", index),
                    new SqlParameter("@SubIndex", Subindex),
                    new SqlParameter("@Order", order),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[FreezeProceeding]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string SaveAdminLOB(AdminLOB model)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", model.AgendaId),
                    new SqlParameter("@RecordId", model.AgendaRecordId),
                    new SqlParameter("@SrNo1", model.SrNo1),
                    new SqlParameter("@SrNo2", model.SrNo2),
                    new SqlParameter("@SrNo3", model.SrNo3),
                    new SqlParameter("@SessionDate", model.SessionDate),
                      new SqlParameter("@TextLOB", model.TextLOB),
                    new SqlParameter("@pdflocation", model.pdflocation),
                    new SqlParameter("@islaid", model.islaid),
                    new SqlParameter("@isdeleted", model.isdeleted),
                      new SqlParameter("@CreatedBy", model.CreatedBy),
                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertLOB]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string DeleteAgenda(int AgendaId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DeleteAgenda]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string DeleteAgendaDetail(int AgendaDetailId, int updateStatus)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaDetailId", AgendaDetailId),
                        new SqlParameter("@UpdateStatus", updateStatus),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[DeleteAgendaDetail]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string SaveAgendaPdf(int AgendaId, string filepath)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@filepath", filepath),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[SaveAgendaPdf]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string SaveAgendaPdfCircular(int AgendaId, string filepath)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@filepath", filepath),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[SaveAgendaCircular]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string SaveProceedingPdfCircular(int AgendaId, string filepath)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@filepath", filepath),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[SaveProceedingCircular]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }
        public static string SaveDeptDecisionPdf(int Deptid, int AgendaId, string filepath, int adid)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@Deptid", Deptid),
                      new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@filepath", filepath),
                    new SqlParameter("@adid", adid)
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[SaveDeptDecisionPdf]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string SaveAgendaPdf3(int AgendaId, string filepath, String filehash)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@filepath", filepath),
                    new SqlParameter("@hash", filehash),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[SaveAgendaPdf3]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }



        public static string SaveAct(Int64 refId, string ActId)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@refid", refId),
                    new SqlParameter("@ActId", ActId),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[SaveAct]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static List<AgendaModel> GetAgendaDetailsforApproval(int AgendaID)
        {
            List<AgendaModel> lstagd = new List<AgendaModel>();
            DataSet ds = new DataSet();
            ds = AgendaDetailsforApproval(AgendaID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaModel fr = new AgendaModel();
                fr.AgendaId = dr["AgendaID"].ToString();
                fr.AgendaName = dr["MeetingTitle"].ToString();
                fr.AgendaDate = Convert.ToDateTime(dr["MeetingDate"]);
                fr.DepartmentName = dr["MCName"].ToString();
                fr.Status = dr["filepaath"].ToString();
                fr.DeptId = dr["DeptId"].ToString();
                fr.StartTime= dr["StartTime"].ToString();
                fr.OfficeName = dr["OfficeName"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;

        }


        public static string SaveAgendaPdf2(int AgendaId, string filepath,string ReportType)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@filepath", filepath),new SqlParameter("@reportType", ReportType),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[SaveAgendaPdf2]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string FreezeUnfreezeAgenda(int AgendaId, int Freeze)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                      new SqlParameter("@IsFreeze", Freeze),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[FreezeUnfreezeAgenda]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static int SaveApproval(int AgendaId, string MCName, DateTime MeetingDate, string MeetingTitle, string PDF, string UserId,string DeptID, string ApprovalAuthority)
        {
            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    SqlCommand cmd = new SqlCommand("InsertLgApprovalOrderTest", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add input parameters with correct types
                    cmd.Parameters.AddWithValue("@AgendaID", AgendaId); // int
                    cmd.Parameters.AddWithValue("@MCName", MCName); // string
                    cmd.Parameters.AddWithValue("@MeetingDate", MeetingDate); // DateTime
                    cmd.Parameters.AddWithValue("@MeetingTitle", MeetingTitle); // string
                    cmd.Parameters.AddWithValue("@PDF", PDF); // string
                    cmd.Parameters.AddWithValue("@UserId", UserId); // string
                    cmd.Parameters.AddWithValue("@DeptID", DeptID);
                    cmd.Parameters.AddWithValue("@ApprovalAuthority", ApprovalAuthority);// string


                    // Add output parameter
                    SqlParameter outputParam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    // Execute the procedure
                    cmd.ExecuteNonQuery();

                    // Retrieve the output parameter value
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 0; // Indicate failure
            }
        }

        public static int UpdateApproval(int AgendaId, string PDF, string UserId, int ApprovedId)
        {
            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    SqlCommand cmd = new SqlCommand("UpdateApprovalOrder", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add input parameters with correct types
                    cmd.Parameters.AddWithValue("@AgendaID", AgendaId); // int
                    cmd.Parameters.AddWithValue("@PDF", PDF); // string
                    cmd.Parameters.AddWithValue("@UserId", UserId); // string
                    cmd.Parameters.AddWithValue("@ApprovalId", ApprovedId); // string
                  
                    // Add output parameter
                    SqlParameter outputParam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    // Execute the procedure
                    cmd.ExecuteNonQuery();

                    // Retrieve the output parameter value
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 0; // Indicate failure
            }
        }


        public static int UpdatePdfCommissioner(int AgendaId, string PDF)
        {
            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    SqlCommand cmd = new SqlCommand("updateCommissionarReport", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add input parameters with correct types
                    cmd.Parameters.AddWithValue("@agendaId", AgendaId); // int
                    cmd.Parameters.AddWithValue("@pdfPath", PDF); // string
                   

                    // Add output parameter
                    SqlParameter outputParam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    // Execute the procedure
                    cmd.ExecuteNonQuery();

                    // Retrieve the output parameter value
                    return Convert.ToInt32(outputParam.Value);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 0; // Indicate failure
            }
        }


        public static int UpdatePdfPath(int agendaId, string pdfPath, string signType, string approvalId)
        {
            try
            {
                using (SqlConnection connection = ClsConnection.GetConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (SqlCommand cmd = new SqlCommand("usp_UpdateSignByeSign", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Input parameters
                        cmd.Parameters.Add("@agendaId", SqlDbType.Int).Value = agendaId;
                        cmd.Parameters.Add("@pdfPath", SqlDbType.VarChar, 500).Value = pdfPath;
                        cmd.Parameters.Add("@SignType", SqlDbType.VarChar, 100).Value = signType;
                        cmd.Parameters.Add("@approvalId", SqlDbType.VarChar, 100).Value = approvalId;

                        // Output parameter
                        SqlParameter outputParam = new SqlParameter("@NewID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        return (outputParam.Value != DBNull.Value)
                            ? Convert.ToInt32(outputParam.Value)
                            : 0; // return 0 if no value returned
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return 0; // Indicate failure
            }
        }



        public static DataSet AgendaDetailsforApproval(int AgendaID)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaID", AgendaID) };

                ds = SqlHelper.ExecuteDataset(connection, "[AgendaDetailsforApproval]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }
        public static string ApproveAgenda(int AgendaId, int approve)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                       new SqlParameter("@approve", approve),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[ApproveAgenda]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static string FreezeProceedings(int AgendaId, int approve)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                       new SqlParameter("@approve", approve),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[FreezeProceedings]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static string ApproveProceedings(int AgendaId, int approve)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgendaId", AgendaId),
                       new SqlParameter("@approve", approve),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[ApproveProceeing]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }




        public static string votingAgenda(int AgdId, int IsVote)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                      new SqlParameter("@AgdId", AgdId),
                      new SqlParameter("@IsVote", IsVote),
                };

                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[VotingAgenda]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static DataSet GetAdminLOBReportByLOBId(int LobId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@@LOBID", LobId) };
                ds = SqlHelper.ExecuteDataset(connection, "[HPMS_SelectAdminLOBReportByLOBId]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }
        public static List<AdminLOBModel> GetAdminlobData(int AgendaId)
        {
            List<AdminLOBModel> lstagd = new List<AdminLOBModel>();
            DataSet ds = new DataSet();
            ds = GetAdminLOBReportByLOBId(AgendaId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AdminLOBModel fr = new AdminLOBModel();

                fr.LOBId = dr["LOBId"].ToString();
                fr.AssemblyName = dr["AssemblyName"].ToString();
                fr.AssemblyNameLocal = dr["AssemblyNameLocal"].ToString();
                fr.SessionName = dr["SessionName"].ToString();
                fr.SessionNameLocal = dr["SessionNameLocal"].ToString();

                fr.SessionDate = dr["SessionDate"].ToString();
                fr.SessionDateLocal = dr["SessionDateLocal"].ToString();
                fr.SessionTime = dr["SessionTime"].ToString();
                fr.SessionTimeLocal = dr["SessionTimeLocal"].ToString();
                fr.SrNo1 = dr["SrNo1"].ToString();
                fr.SrNo2 = dr["SrNo2"].ToString();
                fr.SrNo3 = dr["SrNo3"].ToString();
                fr.TextLOB = dr["TextLOB"].ToString();
                fr.pdflocation = dr["pdflocation"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;
        }



        //CREATE FOR NEW PDF AMAN

        public class HeaderlistofAgenda
        {
            public string Remarks { get; set; }
            public string AgendaName { get; set; }

            public string AgendaDate { get; set; }

            public string StartTime { get; set; }

            public string Header { get; set; }
            public string Footer { get; set; }

            public string Venue { get; set; }
            public string Status { get; set; }

            public string Day { get; set; }
            public string Chairperson { get; set; }
            public string ChairpersonDesignation { get; set; }
            public string MeetingTypeName { get; set; }


        }
        //CREATE FOR NEW PDF AMAN


        public class GetHouseDeatils
        {
            public string MCLogo { get; set; }
            public string MCName { get; set; }
            public string MCName_Local { get; set; }
            public string AgendaName { get; set; }
            public string AgendaDate { get; set; }
            public string Header { get; set; }
            public string Footer { get; set; }
        }
        //CREATE FOR NEW PDF AMAN
        public static GetHouseDeatils GetHouseDeatilsList(int AgendaId, string MCid)
        {
            GetHouseDeatils hd = new GetHouseDeatils();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@MCid", MCid),
                    new SqlParameter("@AgendaId", AgendaId)
                };
                ds = SqlHelper.ExecuteDataset(connection, "GetHouseDeatils", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    hd.MCLogo = Convert.ToString(row["MClogo"].ToString());
                    hd.MCName = Convert.ToString(row["MCName"].ToString());
                    hd.MCName_Local = Convert.ToString(row["MCName_local"].ToString());
                    hd.AgendaName = Convert.ToString(row["AgendaName"].ToString());
                    hd.AgendaDate = Convert.ToString(row["AgendaDate"].ToString());
                    hd.Header = Convert.ToString(row["header"].ToString());
                    hd.Footer = Convert.ToString(row["footer"].ToString());

                }
                return hd;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return hd;
            }
        }
        public static string ImageToBase64(string _imagePath)
        {
            string _base64String = null;
            try
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(_imagePath);
                _base64String = Convert.ToBase64String(imageArray);
                return "data:image/jpg;base64," + _base64String;

                //using (System.Drawing.Image _image = System.Drawing.Image.FromFile(_imagePath))
                //{
                //    using (MemoryStream _mStream = new MemoryStream())
                //    {
                //        _image.Save(_mStream, _image.RawFormat);
                //        byte[] _imageBytes = _mStream.ToArray();
                //        _base64String = Convert.ToBase64String(_imageBytes);

                //        return "data:image/jpg;base64," + _base64String;
                //    }
                //}
            }
            catch (Exception ex)
            {
                return "data:image/jpg;base64,";
            }

        }

        public static HeaderlistofAgenda GetAgendaHeader(int AgendaId)
        {
            HeaderlistofAgenda hlg = new HeaderlistofAgenda();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
                ds = SqlHelper.ExecuteDataset(connection, "getmainAgendatitle", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    hlg.Remarks = Convert.ToString(row["Remarks"].ToString());
                    hlg.AgendaName = Convert.ToString(row["AgendaName"].ToString());
                    hlg.AgendaDate = row["AgendaDate"] != DBNull.Value ? Convert.ToDateTime(row["AgendaDate"]).ToString("dd/MM/yyyy") : "---";
                    hlg.StartTime = Convert.ToString(row["StartTime"]);
                    hlg.Venue = Convert.ToString(row["VenueName_local"]);
                    hlg.Status = Convert.ToString(row["status_description"]);
                    hlg.Day = GetPunjabiDay(row["AgendaDate"] as DateTime?);

                    TimeSpan startTime;
                    if (TimeSpan.TryParse(hlg.StartTime, out startTime))
                    {
                        hlg.StartTime = Converter.ConvertTime(Convert.ToString(startTime));
                    };
                 
                    hlg.Header = Convert.ToString(row["header"].ToString());
                    hlg.Footer = Convert.ToString(row["footer"].ToString());
                    hlg.Chairperson= Convert.ToString(row["AgendaChairperson"].ToString());
                    hlg.ChairpersonDesignation = Convert.ToString(row["AgendaChairpersonDesignation"].ToString());
                    hlg.MeetingTypeName= Convert.ToString(row["MeetingTypeName"].ToString());
                }
                return hlg;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return hlg;
            }
        }


        public static string GetPunjabiDay(DateTime? date)
        {
            if (date == null)
            {
                return "---";
            }

        Dictionary<DayOfWeek, string> punjabiDays = new Dictionary<DayOfWeek, string>
        {
        { DayOfWeek.Sunday, "ਐਤਵਾਰ" },
        { DayOfWeek.Monday, "ਸੋਮਵਾਰ" },
        { DayOfWeek.Tuesday, "ਮੰਗਲਵਾਰ" },
        { DayOfWeek.Wednesday, "ਬੁਧਵਾਰ" },
        { DayOfWeek.Thursday, "ਵੀਰਵਾਰ" },
        { DayOfWeek.Friday, "ਸ਼ੁੱਕਰਵਾਰ" },
        { DayOfWeek.Saturday, "ਸ਼ਨੀਚਰਵਾਰ" }
        };

            return punjabiDays[date.Value.DayOfWeek];
       }



        public static string GetAgendaMainTitle(int AgendaId)
        {
            var Agendatitle = string.Empty;
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
                ds = SqlHelper.ExecuteDataset(connection, "getmainAgendatitle", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    Agendatitle = Convert.ToString(row["AgendaName"].ToString());

                }
                return Agendatitle;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return "";
            }
        }

        //created by aman
        public static string GetOfficeType(int AgendaId)
        {
            var officetype = string.Empty;
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
                ds = SqlHelper.ExecuteDataset(connection, "Getofficetype", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    officetype = Convert.ToString(row["officename"].ToString());

                }
                return officetype;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return "";
            }
        }
        public static List<AgendaReferences> GetAgendaHouseActions(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            DataSet ds = new DataSet();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
            ds = SqlHelper.ExecuteDataset(connection, "getAgendaActions", parameterValues);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();

                fr.Id = dr["id"].ToString();
                fr.RefId = dr["RefId"].ToString();
                fr.ActionDescription = dr["ActionDescription"].ToString();
                fr.ActionBy = dr["ActionBy"].ToString();

                lstagd.Add(fr);
            }
            return lstagd;

        }
        public static List<AgendaReferences> GetAgendaComissionerActions(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            DataSet ds = new DataSet();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
            ds = SqlHelper.ExecuteDataset(connection, "getAgendaActions", parameterValues);

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                AgendaReferences fr = new AgendaReferences();

                fr.Id = dr["id"].ToString();
                fr.RefId = dr["RefId"].ToString();
                fr.ActionDescription = dr["ActionDescription"].ToString();
                fr.ActionBy = dr["ActionBy"].ToString();

                lstagd.Add(fr);
            }
            return lstagd;

        }
        public static List<AgendaReferences> GetAgendaLGActions(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            DataSet ds = new DataSet();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
            ds = SqlHelper.ExecuteDataset(connection, "getAgendaActions", parameterValues);

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                AgendaReferences fr = new AgendaReferences();

                fr.Id = dr["id"].ToString();
                fr.RefId = dr["RefId"].ToString();
                fr.ActionDescription = dr["ActionDescription"].ToString();
                fr.ActionBy = dr["ActionBy"].ToString();

                lstagd.Add(fr);
            }
            return lstagd;

        }


        public static string GetUniqueNumber(int agendaId)
        {
            string uniqueNumber = string.Empty;

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetUniqueNumber", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AgendaId", agendaId);

                    con.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        uniqueNumber = result.ToString();
                    }
                }
            }

            return uniqueNumber;
        }
        public static List<AgendaReferences> GetAdminlobDataForPDF(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            ds = GetAdminLOBPDFReportByLOBId(AgendaId, null);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();

                fr.Id = dr["AgendaDetailId"].ToString();
                fr.Subject = dr["Subject"].ToString();
                fr.LetterDate = dr["LetterDate"].ToString();
                fr.LetterNo = dr["LetterNo"].ToString();
                fr.FileNo = dr["fileno"].ToString();
                fr.FromDept = dr["FromDept"].ToString();
                fr.FromDeptLocal = dr["FromDeptLocal"].ToString();
                fr.FromOffice = dr["FromDept"].ToString() + " " + dr["FromOffice"].ToString();
                fr.FromOfficeLocal = dr["FromDeptLocal"].ToString() + " " + dr["FromOfficeLocal"].ToString();
                fr.DeptLogo = dr["MClogo"].ToString();
                fr.DeptEmail = dr["Email"].ToString();
                fr.DeptPhoneNo = dr["PhoneNo"].ToString();
                fr.DeptAddress = dr["Address"].ToString();
                fr.DeptAddress_Local = dr["Address_local"].ToString();
                fr.DeptMayor = dr["deptmayor"].ToString();
                fr.CommContactNo = dr["CommContactNo"].ToString();
                fr.MayorContactNo = dr["mayorContactNo"].ToString();
                fr.DeptWebsite = dr["website"].ToString();
                fr.yearno = dr["yearno"].ToString();
                fr.ActionDescription = dr["ad"].ToString();
                fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                fr.meetingDate = dr["meetingdate"].ToString();
                fr.StatusName = dr["StatusName"].ToString();
                //fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                fr.attachedfile = dr["ActionTakenFile"].ToString();
                fr.RefId = dr["RefId"].ToString();
                fr.ResolutionNo = dr["ResolutionNo"].ToString();
                fr.CreatedDate = dr["CreatedDate"].ToString();
                fr.ReferencePriorityType = dr["ReferencePriorityType"].ToString();
                fr.SubjectDetails = dr["SubjectDetails"].ToString();
                fr.MinisterName = dr["MinisterName"].ToString();
               
                if (fr.ReferencePriorityType == "1")
                {
                    fr.ReferencePriorityType = "High";
                }
                else if (fr.ReferencePriorityType == "2")
                {
                    fr.ReferencePriorityType = "Medium";
                }
                else if (fr.ReferencePriorityType == "3")
                {
                    fr.ReferencePriorityType = "Low";
                }

                fr.ReferenceTerm = dr["ReferenceTerm"].ToString();

                if (fr.ReferenceTerm == "1")
                {
                    fr.ReferenceTerm = "Short";
                }
                else if (fr.ReferenceTerm == "2")
                {
                    fr.ReferenceTerm = "Medium";
                }
                else if (fr.ReferenceTerm == "3")
                {
                    fr.ReferenceTerm = "Long";
                }
                if (System.DBNull.Value != dr["SrNo"])
                {
                    fr.SrNo = Convert.ToInt32(dr["SrNo"].ToString());
                }
               
                fr.DisplaySrNo = Convert.ToString(dr["DisplaySrNo"].ToString());
                fr.SrNo1 = Convert.ToInt32(dr["SrNo1"].ToString());
                fr.SrNo2 = Convert.ToInt32(dr["SrNo2"].ToString());
                fr.SrNo3 = Convert.ToInt32(dr["SrNo3"].ToString());
                if (fr.SrNo1 != 0)
                {
                    fr.SerialIndex = fr.SrNo1.ToString();
                }
                if (fr.SrNo2 != 0)
                {
                    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo2;
                }
                if (fr.SrNo3 != 0)
                {
                    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo3;
                }



                //fr.Id = dr["Id"].ToString();




                //fr.SubjectDetails = dr["NewSubjectDetails"].ToString();
                if (fr.RefId == "0")
                {
                    if (dr["attachedfile"].ToString() != "")
                    {
                        fr.attachedfile = "\\mlaDiary\\" + dr["attachedfile"].ToString();
                    }
                    else
                    {
                        fr.attachedfile = dr["ActionTakenFile"].ToString();
                    }

                }
                fr.Act = dr["ActName"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;

        }
        public static List<AgendaReferences> GetAdminlobDataForProcedingPDF(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            ds = GetAdminLOBPDFReportByLOBId(AgendaId, "P");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();

                fr.Id = dr["AgendaDetailId"].ToString();
                fr.Subject = dr["Subject"].ToString();
                fr.LetterDate = dr["LetterDate"].ToString();
                fr.LetterNo = dr["LetterNo"].ToString();
                fr.FromDept = dr["FromDept"].ToString();
                fr.FromDeptLocal = dr["FromDeptLocal"].ToString();
                fr.FromOffice = dr["FromDept"].ToString() + " " + dr["FromOffice"].ToString();
                fr.FromOfficeLocal = dr["FromDeptLocal"].ToString() + " " + dr["FromOfficeLocal"].ToString();
                fr.DeptLogo = dr["MClogo"].ToString();
                fr.DeptEmail = dr["Email"].ToString();
                fr.DeptPhoneNo = dr["PhoneNo"].ToString();
                fr.DeptAddress = dr["Address"].ToString();
                fr.DeptAddress_Local = dr["Address_local"].ToString();
                fr.DeptMayor = dr["deptmayor"].ToString();
                fr.CommContactNo = dr["CommContactNo"].ToString();
                fr.MayorContactNo = dr["mayorContactNo"].ToString();
                fr.DeptWebsite = dr["website"].ToString();
                fr.ActionDescription = dr["ad"].ToString();
                fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                fr.StatusName = dr["StatusName"].ToString();
                //fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                fr.attachedfile = dr["ActionTakenFile"].ToString();
                fr.RefId = dr["RefId"].ToString();
                fr.yearno = dr["yearno"].ToString();
                //fr.ResolutionNo = dr["ResolutionNo"].ToString();
                if (dr["SubResolutionNo"].ToString() != "0")
                {
                    fr.ResolutionNo = dr["ResolutionNo"].ToString() + "." + dr["SubResolutionNo"].ToString();
                }
                else
                {
                    fr.ResolutionNo = dr["ResolutionNo"].ToString();
                }
                fr.CreatedDate = dr["CreatedDate"].ToString();
                fr.ReferencePriorityType = dr["ReferencePriorityType"].ToString();
                fr.SubjectDetails = dr["SubjectDetails"].ToString();
                if (fr.ReferencePriorityType == "1")
                {
                    fr.ReferencePriorityType = "High";
                }
                else if (fr.ReferencePriorityType == "2")
                {
                    fr.ReferencePriorityType = "Medium";
                }
                else if (fr.ReferencePriorityType == "3")
                {
                    fr.ReferencePriorityType = "Low";
                }

                fr.ReferenceTerm = dr["ReferenceTerm"].ToString();

                if (fr.ReferenceTerm == "1")
                {
                    fr.ReferenceTerm = "Short";
                }
                else if (fr.ReferenceTerm == "2")
                {
                    fr.ReferenceTerm = "Medium";
                }
                else if (fr.ReferenceTerm == "3")
                {
                    fr.ReferenceTerm = "Long";
                }
                fr.SerialNo = dr["DisplaySrNo"].ToString();
                fr.SrNo1 = Convert.ToInt32(dr["SrNo1"].ToString());
                fr.SrNo2 = Convert.ToInt32(dr["SrNo2"].ToString());
                fr.SrNo3 = Convert.ToInt32(dr["SrNo3"].ToString());
                if (fr.SrNo1 != 0)
                {
                    fr.SerialIndex = fr.SrNo1.ToString();
                }
                if (fr.SrNo2 != 0)
                {
                    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo2;
                }
                if (fr.SrNo3 != 0)
                {
                    fr.SerialIndex = fr.SerialIndex + "." + fr.SrNo3;
                }



                //fr.Id = dr["Id"].ToString();lr.TableItem




                //fr.SubjectDetails = dr["NewSubjectDetails"].ToString();
                if (fr.RefId == "0")
                {
                    if (dr["attachedfile"].ToString() != "")
                    {
                        fr.attachedfile = "\\mlaDiary\\" + dr["attachedfile"].ToString();
                    }
                    else
                    {
                        fr.attachedfile = dr["ActionTakenFile"].ToString();
                    }

                }
                fr.Act = dr["ActName"].ToString();

                fr.TableItem = dr["TableItem"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;

        }





        public static List<AgendaReferences> GetDeptlobDataForProcedingPDF(int agendaid, int deptid)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            ds = GetDeptLOBPDFReportByLOBId(agendaid,deptid);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();

               
                fr.Subject = dr["Subject"].ToString();
               
                fr.ActionDescription = dr["ActionDescription"].ToString();
               
              
                fr.SubjectDetails = dr["SubjectDetails"].ToString();
            
                lstagd.Add(fr);
            }
            return lstagd;

        }



        public static List<AgendaReferences> GetAdminlobExtraDataForProcedingPDF(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();

            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
            ds = SqlHelper.ExecuteDataset(connection, "[getAgendaReferenceExtraDetail]", parameterValues);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();

                fr.Subject = dr["Subject"].ToString();
                fr.SubjectDetails = dr["SubjectDetails"].ToString();

                lstagd.Add(fr);
            }
            return lstagd;

        }


        public static DataSet GetDeptLOBPDFReportByLOBId(int agendaid,int deptid)
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
                //SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@DeptId", deptid) };
                SqlParameter[] parameterValues = new SqlParameter[] {
                  
                    new SqlParameter("@agendaid", agendaid),
                    new SqlParameter("@DeptId", deptid)
                    {
                }
                };



                ds = SqlHelper.ExecuteDataset(connection, "GetDataByDeptDecision", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }


        public static DataSet GetAdminLOBPDFReportByLOBId(int LobId, string type)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", LobId), new SqlParameter("@type", type) };
                ds = SqlHelper.ExecuteDataset(connection, "[getAgendaReferencesforpdf]", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }
        public static List<AgendaReferences> GetAgendaTitleForPDF(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            DataSet ds = new DataSet();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
            ds = SqlHelper.ExecuteDataset(connection, "getmainAgendatitle", parameterValues);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.Agendatitle = Convert.ToString(row["Remarks"].ToString());

                lstagd.Add(fr);
            }
            return lstagd;

        }
        public static List<AgendaReferences> GetAgendaHeaderForPDF(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
                ds = SqlHelper.ExecuteDataset(connection, "getmainAgendatitle", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    AgendaReferences fr = new AgendaReferences();
                    fr.Remarks = Convert.ToString(row["Remarks"].ToString());
                    fr.AgendaName = Convert.ToString(row["AgendaName"].ToString());
                    fr.AgendaDate = Convert.ToDateTime(row["AgendaDate"]).ToString();
                    fr.StartTime = Convert.ToString(row["StartTime"]);


                    TimeSpan startTime;
                    if (TimeSpan.TryParse(fr.StartTime, out startTime))
                    {
                        fr.StartTime = Converter.ConvertTime(Convert.ToString(startTime));
                    };

                    fr.Header = Convert.ToString(row["header"].ToString());
                    fr.Footer = Convert.ToString(row["footer"].ToString());
                    lstagd.Add(fr);
                }
                return lstagd;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return lstagd;
            }
        }

        public static DataTable GetDataForDispatchDashboardStatus(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus)
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
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForMOMDashboardStatus]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "AgendaId", "AgendaDate", "AgendaName", "deptname", "filepath", "filepath2", "officename");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("AgendaDate");
                dt.Columns.Add("AgendaName");
                dt.Columns.Add("deptname");
                dt.Columns.Add("filepath");
                dt.Columns.Add("filepath2");
                dt.Columns.Add("officename");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("AgendaId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["AgendaDate"] = dr1["AgendaDate"].ToString();
                    dr11["AgendaId"] = dr1["AgendaId"].ToString();
                    dr11["AgendaName"] = dr1["AgendaName"].ToString();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["filepath"] = dr1["filepath"].ToString();
                    dr11["filepath2"] = dr1["filepath2"].ToString();
                    dr11["officename"] = dr1["officename"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("AgendaId ='" + dr1["AgendaId"].ToString().Replace(" ", "") + "'").CopyToDataTable();

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
                //DataView view1 = dt.DefaultView;
                //view.Sort = "AgendaDate DESC";
                //dt = view.ToTable();
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDispatchDashboardStatusFilter(string officeidsess, string officeid, string documentType, string status, string userid, string deptidsess, string since, string deptid, string term, string priority, out DataTable dtStatus)
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
                    new SqlParameter("@priority", priority)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDataForMOMDashboardStatusFilter]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "AgendaId", "AgendaDate", "AgendaName", "deptname", "filepath", "filepath2", "officename");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("AgendaDate");
                dt.Columns.Add("AgendaName");
                dt.Columns.Add("deptname");
                dt.Columns.Add("filepath");
                dt.Columns.Add("filepath2");
                dt.Columns.Add("officename");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("AgendaId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["AgendaDate"] = dr1["AgendaDate"].ToString();
                    dr11["AgendaId"] = dr1["AgendaId"].ToString();
                    dr11["AgendaName"] = dr1["AgendaName"].ToString();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["filepath"] = dr1["filepath"].ToString();
                    dr11["filepath2"] = dr1["filepath2"].ToString();
                    dr11["officename"] = dr1["officename"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("AgendaId ='" + dr1["AgendaId"].ToString().Replace(" ", "") + "'").CopyToDataTable();

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

        public static DataTable GetAgendaDataForDispatchCount(string agendaid, string officeid, string officeidsession, string status, string userid, string since, string deptid, string deptidsession, string term, string priority, string documentype)
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
            p.Add(new SqlParameter("@agendaid", agendaid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@documentype", documentype));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetAgendaDataForDispatchCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public static DataTable GetDashboardDataForDispatchStatusTotalCount(string agendaid, string deptid, string deptidsession, string officeid, string officeidsession, string documentype, string status, string term, string priority, string since, string userid)
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
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@agendaid", agendaid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@documentype", documentype));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetAgendaDataForDispatchTotalCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


       
        public static DataTable GetDataForDiaryDashboardStatusorder(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus)
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
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDiaryDataTest]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "AgendaId", "AgendaDate", "AgendaName", "deptname", "filepath", "filepath2", "DecisionPath", "officename", "Approval");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("AgendaDate");
                dt.Columns.Add("AgendaName");
                dt.Columns.Add("deptname");
                dt.Columns.Add("filepath");
                dt.Columns.Add("filepath2");
                dt.Columns.Add("DecisionPath");
                dt.Columns.Add("officename");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("AgendaId");
                dt.Columns.Add("Approval");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    //DecisionPath

                    dr11["AgendaDate"] = dr1["AgendaDate"].ToString();
                    dr11["AgendaId"] = dr1["AgendaId"].ToString();
                    dr11["AgendaName"] = dr1["AgendaName"].ToString();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["filepath"] = dr1["filepath"].ToString();
                    dr11["filepath2"] = dr1["filepath2"].ToString();
                    dr11["DecisionPath"] = dr1["DecisionPath"].ToString();
                    dr11["officename"] = dr1["officename"].ToString();
                    dr11["Approval"] = dr1["Approval"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("AgendaId ='" + dr1["AgendaId"].ToString().Replace(" ", "") + "'").CopyToDataTable();

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
                //DataView view1 = dt.DefaultView;
                //view.Sort = "AgendaDate DESC";
                //dt = view.ToTable();
                return dt;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }




        public static DataTable GetDataForDiaryDashboardStatus(string officeid, string documentType, string status, string userid, string deptid, out DataTable dtStatus)
        {

         DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
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
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDiaryDataTest]", parameterValues);

                dt1 = ds.Tables[0];
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "AgendaId", "AgendaDate", "AgendaName", "deptname", "filepath", "filepath2", "officename", "Approval", "CirculatedAgendaPath", "CirculatedProceedingPath");
                dtStatus = ds.Tables[1].Copy();
                dt.Clear();
                dt.Columns.Add("AgendaDate");
                dt.Columns.Add("AgendaName");
                dt.Columns.Add("deptname");
                dt.Columns.Add("filepath");
                dt.Columns.Add("filepath2");
                dt.Columns.Add("officename");
                dt.Columns.Add("CirculatedAgendaPath");
                dt.Columns.Add("CirculatedProceedingPath");
                
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("AgendaId");
                dt.Columns.Add("Approval");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["AgendaDate"] = dr1["AgendaDate"].ToString();
                    dr11["AgendaId"] = dr1["AgendaId"].ToString();
                    dr11["AgendaName"] = dr1["AgendaName"].ToString();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["filepath"] = dr1["filepath"].ToString();
                    dr11["filepath2"] = dr1["filepath2"].ToString();
                    dr11["officename"] = dr1["officename"].ToString();
                    dr11["CirculatedAgendaPath"] = dr1["CirculatedAgendaPath"].ToString();
                    dr11["CirculatedProceedingPath"] = dr1["CirculatedProceedingPath"].ToString();
                    dr11["Approval"] = dr1["Approval"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("AgendaId ='" + dr1["AgendaId"].ToString().Replace(" ", "") + "'").CopyToDataTable();

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
                //DataView view1 = dt.DefaultView;
                //view.Sort = "AgendaDate DESC";
                //dt = view.ToTable();
                return dt1;
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDiaryDashboardStatusFilter(string officeidsess, string officeid, string documentType, string status, string userid, string deptidsess, string since, string deptid, string term, string priority, out DataTable dtStatus)
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
                    new SqlParameter("@priority", priority)
                    {
                }
                };
                DataSet ds = SqlHelper.ExecuteDataset(connection, "[GetDiaryDataForMOMDashboardStatusFilter]", parameterValues);
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "AgendaId", "AgendaDate", "AgendaName", "deptname", "filepath", "filepath2", "officename");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();
                dt.Columns.Add("AgendaDate");
                dt.Columns.Add("AgendaName");
                dt.Columns.Add("deptname");
                dt.Columns.Add("filepath");
                dt.Columns.Add("filepath2");
                dt.Columns.Add("officename");
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
                }

                dt.Columns.Add("AgendaId");

                foreach (DataRow dr1 in dtDeptName.Rows)
                {
                    DataRow dr11 = dt.NewRow();

                    dr11["AgendaDate"] = dr1["AgendaDate"].ToString();
                    dr11["AgendaId"] = dr1["AgendaId"].ToString();
                    dr11["AgendaName"] = dr1["AgendaName"].ToString();
                    dr11["deptname"] = dr1["deptname"].ToString();
                    dr11["filepath"] = dr1["filepath"].ToString();
                    dr11["filepath2"] = dr1["filepath2"].ToString();
                    dr11["officename"] = dr1["officename"].ToString();
                    DataTable dtResult = ds.Tables[0].Select("AgendaId ='" + dr1["AgendaId"].ToString().Replace(" ", "") + "'").CopyToDataTable();

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

        public static DataTable GetAgendaDataForDiaryCount(string agendaid, string officeid, string officeidsession, string status, string userid, string since, string deptid, string deptidsession, string term, string priority, string documentype)
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
            p.Add(new SqlParameter("@agendaid", agendaid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@documentype", documentype));

            var dtt = SqlHelper.ExecuteDataset(connection, "GetAgendaDataForDiaryCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }



        public static DataTable GetDashboardDataForDiaryStatusTotalCount(string agendaid, string deptid, string deptidsession, string officeid, string officeidsession, string documentype, string status, string term, string priority, string since, string userid)
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
            p.Add(new SqlParameter("@officeid", officeid));
            p.Add(new SqlParameter("@officeidsess", officeidsession));
            p.Add(new SqlParameter("@status", status));
            p.Add(new SqlParameter("@userid", Convert.ToString(userid)));
            p.Add(new SqlParameter("@since", since));
            p.Add(new SqlParameter("@agendaid", agendaid));
            p.Add(new SqlParameter("@deptidsess", deptidsession));
            p.Add(new SqlParameter("@term", term));
            p.Add(new SqlParameter("@priority", priority));
            p.Add(new SqlParameter("@documentype", documentype));
            var dtt = SqlHelper.ExecuteDataset(connection, "GetAgendaDataForDiaryTotalCount", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public static DataSet CheckAgendaStatus(int agendaid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@AgendaId", agendaid));
            DataSet ds = SqlHelper.ExecuteDataset(connection, "CheckAgendaStatus", p.ToArray());
            if (ds.Tables.Count == 0)
            {
                return null;
            }

            return ds;
        }







        public static DataSet CheckResolutionNo(int ResolutionNo, int SubResolutionNo,  int OfficeId)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@ResolutionNo", ResolutionNo));
            p.Add(new SqlParameter("@SubResolutionNo", SubResolutionNo));
            p.Add(new SqlParameter("@OfficeId", OfficeId));
            DataSet ds = SqlHelper.ExecuteDataset(connection, "CheckResolutionNo", p.ToArray());
            if (ds.Tables.Count == 0)
            {
                return null;
            }

            return ds;
        }

        public static SelectList GetAssemblyList(string MCID)
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
                DataSet ds = new DataSet();
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@mcid", MCID) };
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "GetAssemblyList", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = row["AssemblyName"].ToString();
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

        public static List<AgendaReferences> GetProcedingDataForCommPDF(string refid, string actionCode)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            ds = GetCommProceedingPDFReport(refid, actionCode);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();

                fr.Id = dr["lrid"].ToString();
                fr.RefId = dr["refid"].ToString();
                fr.CreatedDate = dr["CreatedDate"].ToString();// Convert.ToDateTime(dr["CreatedDate"]).ToString("dd/MM/yyyy"); 
                fr.AgendaDate = dr["AgendaDate"].ToString();// Convert.ToDateTime().ToString("dd/MM/yyyy"); 
                fr.Subject = dr["Subject"].ToString();
                fr.SubjectDetails = dr["SubjectDetails"].ToString();
                fr.ResolutionNo = dr["ResolutionNo"].ToString();
                fr.Act = dr["ActName"].ToString();
                fr.FromDept = dr["FromDept"].ToString();
                fr.FromOffice = dr["FromOffice"].ToString();
                fr.HouseDecision = dr["HouseDecision"].ToString();
                fr.commissioner = dr["commissioner"].ToString();
                fr.LGComments = dr["LGComments"].ToString();
                fr.ActionTakenDate = dr["ActionTakenDate"].ToString();
                fr.SerialNo = dr["SrNo"].ToString();
                fr.LetterNo = dr["LetterNo"].ToString();
                fr.DisplaySrNo = dr["DisplaySrNo"].ToString();
                fr.TableItem= dr["TableItem"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;

        }
        public static List<AgendaReferences> GetLGSameResolutionDataPDF(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@AgendaId", AgendaId) };
            ds = SqlHelper.ExecuteDataset(connection, "LGProceddingReply", parameterValues);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.ResolutionNo = dr["ResolutionNos"].ToString();
                fr.LGComments = dr["ActionDescription"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;

        }

        public static List<AgendaReferences> GetApprovalActionData(int AgendaId,string ActionIDs, string FileType, string Ref)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@actioncode", ActionIDs),
                    new SqlParameter("@Ref", string.IsNullOrEmpty(Ref) ? (object)DBNull.Value : Ref),
                    new SqlParameter("@deptment", Convert.ToString(CurrentSession.DeptID))

            };
            ds = SqlHelper.ExecuteDataset(connection, "GenerateLGProceddingReplyForApproval", parameterValues);
          
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.ResolutionNo = dr["ResolutionNos"].ToString();
                fr.LGComments = dr["ActionDescription"].ToString();
                fr.StatusName= dr["Status"].ToString();
                lstagd.Add(fr);
            }
           
            return lstagd;

        }

        public static void GetProceedingReply(AgendaDetailModel mdl, int AgendaId, string ActionIDs, string FileType, string Ref)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@actioncode", ActionIDs),
                    new SqlParameter("@Ref", string.IsNullOrEmpty(Ref) ? (object)DBNull.Value : Ref),
                    new SqlParameter("@deptment", Convert.ToString(CurrentSession.DeptID)),
                     new SqlParameter("@userid", Convert.ToString(CurrentSession.UserID))

            };
            ds = SqlHelper.ExecuteDataset(connection, "GetProceedingReply", parameterValues);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.RefId = dr["RefId"].ToString();
                fr.ResolutionNo = dr["ResolutionNos"].ToString();
                fr.ActionDescription = dr["ActionDescription"].ToString();
                fr.LGComments = dr["DraftRemarks"].ToString();

                //fr.StatusName = dr["Status"].ToString();
                fr.StatusName = dr["DraftActionCode"] == DBNull.Value  || string.IsNullOrEmpty(dr["DraftActionCode"].ToString())
         ? dr["ActionId"].ToString(): dr["DraftActionCode"].ToString();
                lstagd.Add(fr);
            }
            mdl._LstResolutionActions=lstagd;
            mdl.AgendaId = AgendaId;
            // Table 2: ActionTypeList_Ref
            mdl.ActionTypeList_Ref = new SelectList(ds.Tables[1].AsEnumerable().Select(row => new SelectListItem
            {
                Text = row["actionnamestatus"]?.ToString(),
                Value = row["Actioncode"]?.ToString()
            }).ToList(), "Value", "Text");

           

        }

        public static void GetProceedingReplyNew(AgendaDetailModel mdl, int AgendaId, string ActionIDs, string FileType, string Ref)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@actioncode", ActionIDs),
                    new SqlParameter("@Ref", string.IsNullOrEmpty(Ref) ? (object)DBNull.Value : Ref),
                    new SqlParameter("@deptment", Convert.ToString(CurrentSession.DeptID)),
                     new SqlParameter("@userid", Convert.ToString(CurrentSession.UserID))

            };
            ds = SqlHelper.ExecuteDataset(connection, "GetProceedingReplyNew", parameterValues);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.RefId = dr["RefId"].ToString();
                fr.ResolutionNo = dr["ResolutionNos"].ToString();
                fr.ActionDescription = dr["ActionDescription"].ToString();
                fr.LGComments = dr["DraftRemarks"].ToString();

                //fr.StatusName = dr["Status"].ToString();
                fr.StatusName = dr["DraftActionCode"] == DBNull.Value || string.IsNullOrEmpty(dr["DraftActionCode"].ToString())
                 ? dr["ActionId"].ToString() : dr["DraftActionCode"].ToString();
                lstagd.Add(fr);
            }
            mdl._LstResolutionActions = lstagd;
            mdl.AgendaId = AgendaId;
            // Table 2: ActionTypeList_Ref
            mdl.ActionTypeList_Ref = new SelectList(ds.Tables[1].AsEnumerable().Select(row => new SelectListItem
            {
                Text = row["actionnamestatus"]?.ToString(),
                Value = row["Actioncode"]?.ToString()
            }).ToList(), "Value", "Text");



        }


        public static List<AgendaReferences> GetApprovalDepartmentAndStartandEndResolution(int AgendaId, string ActionIDs, string Refs)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@actioncode", ActionIDs),
                    new SqlParameter("@Ref", string.IsNullOrEmpty(Refs) ? (object)DBNull.Value : Refs),
                    new SqlParameter("@deptment", Convert.ToString(CurrentSession.DeptID))

            };
            ds = SqlHelper.ExecuteDataset(connection, "GenerateLGProceddingReplyForApproval", parameterValues);

           
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                //fr.SrNo1 = Convert.ToInt32(dr["StartResolution"]);
                //fr.SrNo2 = Convert.ToInt32(dr["EndResolution"]);

                fr.StartResolution = Convert.ToString(dr["StartResolution"]);
                fr.EndResolution = Convert.ToString(dr["EndResolution"]);

                fr.FromDept = Convert.ToString(dr["deptnameLocal"]);
                fr.AgendaDate= Convert.ToDateTime(dr["agendadate"]).ToString("dd/MM/yyyy");
                fr.ActionTakenDate = Convert.ToDateTime(dr["agendadate"]).ToString("dd/MM/yyyy");
                fr.CreatedDate = Convert.ToDateTime(dr["ApprovalDate"]).ToString("dd/MM/yyyy");
                fr.ToOffice =    Convert.ToString(dr["FromOfficer"]);
                fr.FromDeptID =    Convert.ToString(dr["FromDeptId"]);
                fr.SerialNo = Convert.ToString(dr["SrNos"]);
                fr.FromDeptLocal = Convert.ToString(dr["DestinationnameLocal"]);
                lstagd.Add(fr);
            }
            return lstagd;

        }

        public static List<AgendaReferences> GetApprovalEndorsement(int AgendaId, string ActionIDs, string Refs)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] 
            {
                    new SqlParameter("@AgendaId", AgendaId),
                    new SqlParameter("@actioncode", ActionIDs),
                    new SqlParameter("@Ref", string.IsNullOrEmpty(Refs) ? (object)DBNull.Value : Refs),
                    new SqlParameter("@deptment", Convert.ToString(CurrentSession.DeptID))
            };
            ds = SqlHelper.ExecuteDataset(connection, "GenerateLGProceddingReplyForApproval", parameterValues);


            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.Endorsement = Convert.ToString(dr["EndorsementSubject"]);
                fr.Footer = Convert.ToString(dr["FooterSubject"]);
                fr.UserDispactchNo = Convert.ToString(dr["DispatchNumber"]);
                fr.Subject= Convert.ToString(dr["ApprovalSubject"]);
                fr.Header = Convert.ToString(dr["ApprovalHeader"]);
                fr.SendTo = Convert.ToString(dr["SendTo"]);
                fr.RejectionFormat = Convert.ToString(dr["RejectionFormat"]);
                lstagd.Add(fr);
            }
            return lstagd;

        }


        public static void SaveAgendaReferenceHistory(string agendaId, string refs, string userId)
        {
            // Delete previous records for this agenda

            SqlConnection con = ClsConnection.GetConnection();
            try
            {
                SqlParameter[] deleteParam =
            {
            new SqlParameter("@AgendaId", agendaId)
            };

            SqlHelper.ExecuteNonQuery(
                con,
                CommandType.Text,
                "DELETE FROM AgendaReferenceHistory WHERE AgendaId = @AgendaId",
                deleteParam);


           
                if (!string.IsNullOrEmpty(refs))
                {
                    string[] refIds = refs.Split(',');

                    foreach (string item in refIds)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            long referenceId = Convert.ToInt64(item);

                            SqlParameter[] param =
                             {
                                new SqlParameter("@AgendaId", agendaId),
                                new SqlParameter("@ReferenceId", referenceId),
                                new SqlParameter("@CheckedBy", userId)
                            };

                            SqlHelper.ExecuteNonQuery(
                                con,
                                CommandType.StoredProcedure,
                                "usp_InsertAgendaReferenceHistory",
                                param);
                        }
                    }

                }
                else
                {
                    SqlParameter[] param =
                            {
                                new SqlParameter("@AgendaId", agendaId),
                                new SqlParameter("@ReferenceId", ""),
                                new SqlParameter("@CheckedBy", userId)
                            };

                    SqlHelper.ExecuteNonQuery(
                        con,
                        CommandType.StoredProcedure,
                        "usp_InsertAgendaReferenceHistory",
                        param);
                }


            }
            catch (Exception ex)
            {
                // Log Error
            }
        }


        public static DataTable GetCheckedReferencesByAgendaId(string agendaId)
        {
            SqlConnection con = ClsConnection.GetConnection();

                try
                {
                    SqlParameter[] param =
                    {
                new SqlParameter("@AgendaId", agendaId)
                 };

                DataSet ds = SqlHelper.ExecuteDataset(
                    con,
                    CommandType.StoredProcedure,
                    "GetAgendaReferenceHistory",
                    param);

                return ds.Tables[0];
            }
            finally
            {
                con.Close();
            }
        }


        public static List<AgendaReferences> GetApprovalFileIDandRecievedDate(int AgendaId)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@DeptId", CurrentSession.DeptID),
                    new SqlParameter("@AgendaId", @AgendaId) };
            ds = SqlHelper.ExecuteDataset(connection, "GetSentDateByDept", parameterValues);


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.FileRefId = Convert.ToString(dr["RefId"]);
                fr.LetterDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("dd/MM/yyyy");
                lstagd.Add(fr);
            }
            return lstagd;

        }



        public static string GenerateDispatch(int agendaId)
        {
            string dispatchNo = "";

            using (SqlConnection con = ClsConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("sp_GenerateLGDispatchNo", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AgendaId", agendaId);

                SqlParameter output = new SqlParameter("@GeneratedDispatchNo", SqlDbType.NVarChar, 50)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(output);

                con.Open();
                cmd.ExecuteNonQuery();

                dispatchNo = output.Value.ToString();
            }

            return dispatchNo;
        }
        public static List<AgendaReferences> LGBranchDetails(string OfficeID)
        {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@OfficeID", OfficeID) };
                  
            ds = SqlHelper.ExecuteDataset(connection, "sp_LGBranchDetails", parameterValues);


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.FromOfficeLocal = Convert.ToString(dr["officenamelocal"]);
                fr.DeptEmail = Convert.ToString(dr["Email"]);
                fr.DeptPhoneNo = Convert.ToString(dr["PhoneNo"]);
                fr.FromDeptLocal = Convert.ToString(dr["DepartmentName"]);
                fr.DeptAddress = Convert.ToString(dr["Address"]);
                fr.DeptLogo = Convert.ToString(dr["Logo"]);
                lstagd.Add(fr);
            }
            return lstagd;

        }


        public static List<AgendaReferences> GetRefChangeHistoryy(string refId)
         {
            List<AgendaReferences> lstagd = new List<AgendaReferences>();
            DataSet ds = new DataSet();
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@refid", refId) };
            ds = SqlHelper.ExecuteDataset(connection, "Get_ReferenceHistory", parameterValues);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                AgendaReferences fr = new AgendaReferences();
                fr.Subject = dr["subject"].ToString();
                fr.SubjectDetails = dr["subjectdetail"].ToString();
                fr.ActionTakenDate = dr["updatedDate"].ToString();
                fr.ActionBy = dr["updatedby"].ToString();
                lstagd.Add(fr);
            }
            return lstagd;

        }
        public static DataSet GetCommProceedingPDFReport(string refid, string actioncode)
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
                ds = SqlHelper.ExecuteDataset(connection, "GetCommProceedingPDFReport", parameterValues);

            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;

        }

    }


    public class MeetingNoticeAgenda
    {
        public int AgendaID { get; set; }

        public int Sr { get; set; }
        public string MeetingName { get; set; }
        public string MainFileRefID { get; set; }
        public string PartFileRefID { get; set; }
        public int TotalMemo { get; set; }
        public DateTime MeetingDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsEdit { get; set; }   // Indicates if the record is editable (1 = editable, 0 = not)
        public string Noticepath { get; set; }
        public string fileaccessingurl { get; set; }
        public string Venue { get; set; }
        public string Agendafile { get; set; }
        public string Filetype { get; set; }
        public string CurrentlyWith { get; set; }
        public bool IsLinkEbook { get; set; }
    }


    public class AdminLOBModel
    {
        public int AgendaId { get; set; }
        public int AgendaRecordId { get; set; }
        // public int SrNo1 { get; set; }
        // public int SrNo2 { get; set; }
        // public int SrNo3 { get; set; }
        public string SessionDate { get; set; }
        public string TextLOB { get; set; }
        public string pdflocation { get; set; }
        public int islaid { get; set; }
        public int isdeleted { get; set; }
        public string CreatedBy { get; set; }

        public string LOBId { get; set; }
        public string AssemblyId { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyNameLocal { get; set; }
        public string SessionId { get; set; }
        public string SessionName { get; set; }
        public string SessionNameLocal { get; set; }
        //public string SessionDate { get; set; }
        public string SessionDateLocal { get; set; }
        public string SessionTime { get; set; }
        public string SessionTimeLocal { get; set; }
        public string SrNo1 { get; set; }
        public string SrNo2 { get; set; }
        public string SrNo3 { get; set; }
    }
    public class LOBModel
    {
        public LOBModel()
        {
            this.ListAdminLOB = new List<AdminLOBModel>();

        }

        public string outXml { get; set; }

        public DateTime? sessDate { get; set; }
        public List<AdminLOBModel> ListAdminLOB { get; set; }


    }
    public class AgendaEdit
    {
        public static List<AgendaModel> GetRowById(int id)
        {
            List<AgendaModel> lstitems = new List<AgendaModel>();
            try
            {
                using (SqlConnection con = ClsConnection.GetConnection())
                {
                    con.Open();
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@Id", id);

                    using (DataTable dt = CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetAgendaRowById", param).Tables[0])
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            AgendaModel item = new AgendaModel();
                            {
                                item.AgendaName = Convert.ToString(dr["AgendaName"]);
                                //item.AgendaDate = dr["AgendaDate"] != DBNull.Value ? (DateTime?)dr["AgendaDate"] : null;
                                item.AgendaDate = dr["AgendaDate"] != DBNull.Value ? (DateTime?)dr["AgendaDate"] : null;
                                string formattedDate = item.AgendaDate?.ToString("yyyy-MM-dd");
                                item.MeetingDate = formattedDate;
                                item.Header = Convert.ToString(dr["header"]);
                                item.Footer = Convert.ToString(dr["footer"]);
                                item.Status = Convert.ToString(dr["Status"]);
                                item.ChairPerson= Convert.ToString(dr["ChairPerson"]);
                                item.ChairPersonDesignation = Convert.ToString(dr["ChairPersonDesignation"]);
                                string startTimeValue = Convert.ToString(dr["StartTime"]);
                                TimeSpan startTime;
                                if (TimeSpan.TryParse(startTimeValue, out startTime))
                                {
                                    item.StartTime = Converter.ConvertTime(Convert.ToString(startTime));
                                };

                                string endTimeValue = Convert.ToString(dr["EndTime"]);
                                TimeSpan endTime;
                                if (TimeSpan.TryParse(endTimeValue, out endTime))
                                {
                                    item.EndTime = Converter.ConvertTime(Convert.ToString(endTime));
                                };

                                item.Agendatype = Convert.ToString(dr["AgendaType"]);
                                item.Venue = Convert.ToString(dr["VenueId"]);
                            }
                            lstitems.Add(item);
                        }
                        return lstitems;
                    }
                }
            }
            catch (Exception ex)
            {
                return lstitems;

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



        public static int UpdateRowById(AgendaModel model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("UpdateAgendaRowById", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AgendaName", model.AgendaName);
                cmd.Parameters.AddWithValue("@AgendaDate", model.AgendaDate);
                cmd.Parameters.AddWithValue("@AgendaType", model.Agendatype);
                cmd.Parameters.AddWithValue("@header", model.Header);
                cmd.Parameters.AddWithValue("@footer", model.Footer);
                cmd.Parameters.AddWithValue("@ChairPerson", model.ChairPerson);
                cmd.Parameters.AddWithValue("@ChairPersonDesignation", model.ChairPersonDesignation);
                cmd.Parameters.AddWithValue("@StartTime", model.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", model.EndTime);
                cmd.Parameters.AddWithValue("@ModifyBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@Id", model.AgendaId);
                cmd.Parameters.AddWithValue("@Status", model.Status);

                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int UpdateMeetingRowById(AgendaModel model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlCommand cmd = new SqlCommand("UpdateAgendaMeetingNotice", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AgendaName", model.AgendaName);
                cmd.Parameters.AddWithValue("@AgendaDate", model.AgendaDate);
                //cmd.Parameters.AddWithValue("@AgendaType", model.Agendatype);
                cmd.Parameters.AddWithValue("@header", model.Header);
                //cmd.Parameters.AddWithValue("@footer", model.Footer);
                cmd.Parameters.AddWithValue("@StartTime", model.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", model.EndTime);
                cmd.Parameters.AddWithValue("@ModifyBy", CurrentSession.UserID);
                cmd.Parameters.AddWithValue("@Id", model.AgendaId);
                cmd.Parameters.AddWithValue("@Status", Convert.ToInt16(model.Status));
                cmd.Parameters.AddWithValue("@Vanue", Convert.ToInt16(model.Venue));
                cmd.ExecuteNonQuery().ToString();
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static string UpdateLinkStatus(string fileId, int isLinked)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[] {
                    new SqlParameter("@fileId", fileId),
                    new SqlParameter("@isLinked", isLinked),
                

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[UpdateLinkStatus]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }

        public static bool SaveCommitteeMember(string deptId,string memberType,string memberName,string designation, string mobile,
        string email,string createdBy)
        {
            bool result = false;

            try
            {
                SqlConnection connection = ClsConnection.GetConnection();

                if (connection.State == ConnectionState.Closed ||
                    connection.State == ConnectionState.Broken)
                {
                    connection.Open();
                }

                SqlParameter[] parameterValues = new SqlParameter[]
                {
                    new SqlParameter("@DepartmentId", deptId),
                    new SqlParameter("@MemberType", memberType),
                    new SqlParameter("@MemberName", memberName),
                    new SqlParameter("@Designation", designation),
                    new SqlParameter("@Mobile", mobile),
                    new SqlParameter("@Email", email ?? string.Empty),
                    new SqlParameter("@CreatedBy", createdBy)
                };

                int rows = SqlHelper.ExecuteNonQuery(
                    connection,
                    CommandType.StoredProcedure,
                    "sp_CommitteeMember_Save",
                    parameterValues
                );

                result = true;
                connection.Close();
            }
            catch (Exception ex)
            {
                result = false;
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }

            return result;
        }
        public static bool CheckAllLinkedActionCode13(int agendaId)
        {
            bool isValid = false;

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_CheckAllLinkedActionCodeEOComents", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AgendaId", agendaId);

                    con.Open();

                    var result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        isValid = Convert.ToBoolean(result);
                    }
                }
            }

            return isValid;
        }

        public static bool CheckAllLinkedAct(int agendaId)
        {
            bool isValid = false;

            using (SqlConnection con = ClsConnection.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_CehckAct", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AgendaId", agendaId);

                    con.Open();

                    var result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        isValid = Convert.ToBoolean(result);
                    }
                }
            }

            return isValid;
        }

    }
}
