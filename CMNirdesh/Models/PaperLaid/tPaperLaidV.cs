namespace CMNirdesh.Models.PaperLaid
{
    #region Namespace References

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;
    using System.ComponentModel;
    using CMNirdesh.Models.Session;
    using Lib.Web.Mvc.JQuery.JqGrid.DataAnnotations;
    using Lib.Web.Mvc.JQuery.JqGrid;
    using CMNIrdesh.Models.Session;
    using System.Globalization;
    using System.Data.SqlClient;
    using System.Data;
    using System.Linq;

    #endregion

    [Table("tPaperLaidVS")]
    [Serializable]
    public partial class tPaperLaidV
    {
        public tPaperLaidV()
        {
            this.mSession = new List<mSession>();
            this.mDepartment = new List<mDepartment>();
            this.ListtPaperLaidV = new List<tPaperLaidV>();
            this.ListtPaperLaidVSub = new List<tPaperLaidV>();
            this.mPaperCategoryTypeList = new List<mPaperCategoryType>();
        }

        //Changes Regarding new design
        public tPaperLaidV(tPaperLaidV papLaidV)
        {
            this.NoticeNumber = papLaidV.NoticeNumber;
            this.MemberName = papLaidV.MemberName;
            this.PaperLaidId = papLaidV.PaperLaidId;
            this.MinistryId = papLaidV.MinistryId;
            this.DepartmentId = papLaidV.DepartmentId;
            this.MinistryName = papLaidV.MinistryName;
            this.DeparmentName = papLaidV.DeparmentName;
            this.FileName = papLaidV.FileName;
            this.FilePath = papLaidV.FilePath;

            this.FileVersion = papLaidV.FileVersion;
            this.Title = papLaidV.Title;
            this.actualFilePath = papLaidV.actualFilePath;
            this.NoticeID = papLaidV.NoticeID;
            this.Status = papLaidV.Status;
            this.PaperSent = papLaidV.PaperSent;
            this.DeptSubmittedDate = papLaidV.DeptSubmittedDate;
            this.DesireLayingDate = papLaidV.DesireLayingDate;
            this.BussinessType = papLaidV.BussinessType;
            this.BOTStatus = papLaidV.BOTStatus;
            this.OriDiaryFileName = papLaidV.OriDiaryFileName;
            this.OriDiaryFileName = papLaidV.OriDiaryFilePath;
            this.RuleNo = papLaidV.RuleNo;
            this.evidhanReferenceNumber = papLaidV.evidhanReferenceNumber;
            this.IsAcknowledgmentDate = papLaidV.IsAcknowledgmentDate;
            this.AcknowledgmentDateString = papLaidV.AcknowledgmentDateString;
            this.SubmittedDateString = papLaidV.SubmittedDateString;
            this.Remark = papLaidV.Remark;
            this.isDocFile = papLaidV.isDocFile;
            this.isPdfFile = papLaidV.isPdfFile;
            this.StatusText = papLaidV.StatusText;
        }


        public static List<MCStatDetail> GetMCStatDetailsListForFile(string FileReference, string ActionCode)
        {
            if (string.IsNullOrEmpty(ActionCode))
            {
                ActionCode = null;
            }
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("@filereference", FileReference));
                p.Add(new SqlParameter("@DocumentType", ActionCode));

                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "GetFileReferenceData", p.ToArray());
              
                if (ds.Tables.Count > 0)
                {
                    int s = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string Pendency = GetPendancy(Convert.ToString(dr["Referance"]));
                        MCStatDetail item = new MCStatDetail();
                        item.SrNo = s;
                        item.AgendaDate = Convert.ToDateTime(dr["MettingDate"]);
                        item.ReferenceId = Convert.ToString(dr["Referance"]);
                        item.Meeting = Convert.ToString(dr["MettingName"]);
                        item.TotalResolutions = Convert.ToString(dr["Resolution"]);

                        item.Resolutionno = Convert.ToString(dr["ResolutionNo"]);
                        item.Type = Convert.ToString(dr["type"]);

                        item.ActionName = Convert.ToString(dr["ActionName"]);
                        item.Subject = Convert.ToString(dr["Subject"]);
                        item.FromDepartment = Convert.ToString(dr["FromDepartment"]);
                        item.FromOffice = Convert.ToString(dr["FromOffice"]);
                        item.ToDepartment = Convert.ToString(dr["ToDepartment"]);
                        item.ToOffice = Convert.ToString(dr["ToOffice"]);
                        item.DocType = Convert.ToString(dr["DocumentTypeName"]);
                        item.Pendency = Pendency;
                        lstitems.Add(item);

                        s++;


                    };

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;

            }
        }

        public static DataTable GetMCStatDetailsListPartFile(string RefrenceID, string UserID, out DataTable dtStatus, string Departmentid)
        {
            //Departmentid = "LG00001";
            int Officeid = Convert.ToInt16(CurrentSession.OfficeId);
            DataTable dt = new DataTable();
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@Userid", UserID));
                p.Add(new SqlParameter("@RefId", RefrenceID));
                p.Add(new SqlParameter("@OfficeId", Officeid));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "PartFileDetail", p.ToArray());
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "MettingDate", "MettingName", "Resolution", "FileReference", "ID", "SentTo", "CurrentlyWith");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();

                dt.Columns.Add("MettingDate");
                dt.Columns.Add("MettingName");
                dt.Columns.Add("Resolution");
                dt.Columns.Add("SentTo");
                dt.Columns.Add("CurrentlyWith");
                DataTable All = new DataTable();
                All = dtDeptName.Copy();
                DataTable resultDataTable = new DataTable();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {

                        DataRow dr11 = dt.NewRow();
                        string s = dr["DocumentTypeName"].ToString();
                        DataTable dtResult1 = tPaperLaidV.GetUserActionCount(Departmentid, Convert.ToInt16(dr["DocumentTypeId"]), dr["DocumentTypeName"].ToString(), "3", UserID, Convert.ToInt32("180"));



                        var joinedData = from table1 in All.AsEnumerable()
                                         join table2 in dtResult1.AsEnumerable()
                                         on (long?)table1["ID"] equals (long?)table2["ID"]
                                         // into t
                                         //from rt in t.DefaultIfEmpty()
                                         select new
                                         {
                                             ID = table1.Field<long>("ID"),
                                             FileReference = table1.Field<string>("FileReference"),
                                             MettingDate = table1.Field<DateTime>("MettingDate"),
                                             MettingName = table1.Field<string>("MettingName"),
                                             Resolution = table1.Field<int>("Resolution"),
                                             SentTo = table1.Field<string>("SentTo"),
                                             CurrentlyWith = table1.Field<string>("CurrentlyWith"),
                                             s = table2.Field<int>(s),
                                         };


                        if (!resultDataTable.Columns.Contains("MettingName"))
                        {
                            resultDataTable.Columns.Add("ID", typeof(long));
                            resultDataTable.Columns.Add("FileReference", typeof(string));
                            resultDataTable.Columns.Add("MettingDate", typeof(string));
                            resultDataTable.Columns.Add("MettingName", typeof(string));
                            resultDataTable.Columns.Add("Resolution", typeof(int));
                            resultDataTable.Columns.Add("SentTo", typeof(string));
                            resultDataTable.Columns.Add("CurrentlyWith", typeof(string));
                        }
                        resultDataTable.Columns.Add(s, typeof(int));

                        if (resultDataTable.Rows.Count == 0)
                        {
                            foreach (var item in joinedData)
                            {
                                resultDataTable.Rows.Add(item.ID, item.FileReference, item.MettingDate, item.MettingName, item.Resolution, item.SentTo, item.CurrentlyWith);
                            }
                        }

                        foreach (var item in joinedData)
                        {
                            DataRow matchingRow = resultDataTable.AsEnumerable().FirstOrDefault(row => row.Field<long>("ID") == item.ID);
                            matchingRow[s] = item.s;
                        }

                        dtResult1.Clear();
                    }
                }
                else
                {
                    resultDataTable = dtDeptName.Copy();
                }

                // For Zero Count Records adding row
                foreach (DataRow drr in ds.Tables[0].Rows)
                {
                    string refid = drr["ID"].ToString();
                    DataRow[] dr = resultDataTable.Select("ID=" + refid);
                    if (dr.Length > 0)
                    {

                    }
                    else
                    {
                        resultDataTable.Rows.Add(drr["ID"].ToString(), drr["FileReference"].ToString(), drr["MettingDate"].ToString(), drr["MettingName"].ToString(), "0", drr["SentTo"].ToString(), drr["CurrentlyWith"].ToString());
                    }
                }

                return resultDataTable;

            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static List<DocumentTypeModel> GetUserAction(string UserId)
        {
            List<DocumentTypeModel> lstitems = new List<DocumentTypeModel>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@userId", UserId));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "getuserActions", p.ToArray());

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DocumentTypeModel item = new DocumentTypeModel();
                        item.DocumentTypeName = Convert.ToString(dr["DocumentTypeName"]);
                        item.DocmentType = Convert.ToInt16(dr["DocumentTypeId"]);
                        lstitems.Add(item);

                    };

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;
            }
        }

        public static DataTable GetUserActionCount(string DeptId, int ActionCode, string ActionName, string Type,string userid, int officeId)
        {
            DataSet ds;
            List<DocumentTypeModel> lstitems = new List<DocumentTypeModel>();

            SqlConnection con = ClsConnection.GetConnection();
            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                con.Open();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@deptid", DeptId));
            p.Add(new SqlParameter("@ActionCode", ActionCode));
            p.Add(new SqlParameter("@ActionName", ActionName));
            p.Add(new SqlParameter("@Type", Type));
            p.Add(new SqlParameter("@userid", userid));
            p.Add(new SqlParameter("@OfficeId", officeId));
            con.Close();
            ds = SqlHelper.ExecuteDataset(con, "DashboardStatsLDetailByAction", p.ToArray());
            DataTable dt = ds.Tables[0];
            return dt;
        }


        public static List<MCStatDetail> GetPartfile(string UserId, string FileReferanceId)
        {
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@FileReferanceId", FileReferanceId));
                p.Add(new SqlParameter("@userId", UserId));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "GetPartfileUserInbox", p.ToArray());

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MCStatDetail item = new MCStatDetail();
                        item.ReferenceId = Convert.ToString(dr["LinkRefid"]);
                        item.Islink = Convert.ToBoolean(dr["IsLink"]);
                        item.CreatedBy= Convert.ToString(dr["CurrentlyWith"]);
                        lstitems.Add(item);

                    };

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;
            }
        }

        public static DataTable GetDataForDiaryDashboardList(string DepartmentID,string FromDeptId, string OfficeId, string ActionCode, string userid, out DataTable dtStatus, string Type, int FileType, int RefType,int ShowType)
        {
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(ActionCode))
            {
                ActionCode = null;
            }
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("@officeid", OfficeId));
                p.Add(new SqlParameter("@documentType", ActionCode));
                p.Add(new SqlParameter("@deptid", DepartmentID));
                p.Add(new SqlParameter("@Userid", userid));
                p.Add(new SqlParameter("@Type", Type));
                p.Add(new SqlParameter("@FileType", FileType));
                p.Add(new SqlParameter("@RefType", RefType));
                p.Add(new SqlParameter("@ShowType", ShowType));
                p.Add(new SqlParameter("@FromDeptId", FromDeptId));
                
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "DashboardStatsLDetail", p.ToArray());
                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "MettingDate","MettingName", "Resolution", "FileReference", "ID", "SentTo", "CurrentlyWith");
               // DataTable dtDeptName = view.ToTable(true, "MettingDate", "MettingName", "Resolution", "FileReference", "ID", "SentTo", "CurrentlyWith");
                dtStatus = ds.Tables[1].Copy();

                dt.Clear();

                dt.Columns.Add("MettingDate");
                dt.Columns.Add("MettingName");
                dt.Columns.Add("Resolution");
                dt.Columns.Add("SentTo");
                dt.Columns.Add("CurrentlyWith");
                DataTable All = new DataTable();
                All = dtDeptName.Copy();
                DataTable resultDataTable = new DataTable();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {

                        DataRow dr11 = dt.NewRow();
                        string s = dr["DocumentTypeName"].ToString();
                        DataTable dtResult1 = tPaperLaidV.GetUserActionCount(DepartmentID, Convert.ToInt16(dr["DocumentTypeId"]), dr["DocumentTypeName"].ToString(), Type, userid, Convert.ToInt32(OfficeId));



                        var joinedData = from table1 in All.AsEnumerable()
                                         join table2 in dtResult1.AsEnumerable()
                                         on (long?)table1["ID"] equals (long?)table2["ID"]
                                         // into t
                                         //from rt in t.DefaultIfEmpty()
                                         select new
                                         {
                                             ID = table1.Field<long>("ID"),
                                             FileReference = table1.Field<string>("FileReference"),
                                             MettingDate = table1.Field<DateTime>("MettingDate"),
                                             MettingName = table1.Field<string>("MettingName"),
                                             Resolution = table1.Field<int>("Resolution"),
                                             SentTo = table1.Field<string>("SentTo"),
                                             CurrentlyWith = table1.Field<string>("CurrentlyWith"),
                                             s = table2.Field<int>(s),
                                         };


                        if (!resultDataTable.Columns.Contains("MettingName"))
                        {
                            resultDataTable.Columns.Add("ID", typeof(long));
                            resultDataTable.Columns.Add("FileReference", typeof(string));
                            resultDataTable.Columns.Add("MettingDate", typeof(string));
                            resultDataTable.Columns.Add("MettingName", typeof(string));
                            resultDataTable.Columns.Add("Resolution", typeof(int));
                            resultDataTable.Columns.Add("SentTo", typeof(string));
                            resultDataTable.Columns.Add("CurrentlyWith", typeof(string));
                        }
                        resultDataTable.Columns.Add(s, typeof(int));

                        if (resultDataTable.Rows.Count == 0)
                        {
                            foreach (var item in joinedData)
                            {
                                resultDataTable.Rows.Add(item.ID, item.FileReference, item.MettingDate, item.MettingName, item.Resolution, item.SentTo,item.CurrentlyWith);
                            }
                        }

                        foreach (var item in joinedData)
                        {
                            DataRow matchingRow = resultDataTable.AsEnumerable().FirstOrDefault(row => row.Field<long>("ID") == item.ID);
                            matchingRow[s] = item.s;
                        }

                        dtResult1.Clear();
                    }
                }
                else
                {
                    resultDataTable = dtDeptName.Copy();
                }

                // For Zero Count Records adding row
                foreach (DataRow drr in ds.Tables[0].Rows)
                {
                    string refid = drr["ID"].ToString();
                    DataRow[] dr = resultDataTable.Select("ID="+ refid);
                    if (dr.Length > 0)
                    {

                    }
                    else
                    {
                        resultDataTable.Rows.Add(drr["ID"].ToString(), drr["FileReference"].ToString(), drr["MettingDate"].ToString(), drr["MettingName"].ToString(), "0", drr["SentTo"].ToString(), drr["CurrentlyWith"].ToString() );
                    }
                }

               return resultDataTable;

            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                dtStatus = dt.Copy();
                return dt;
            }
        }

        public static DataTable GetDataForDiaryDashboardListDepartment(string DepartmentID, string FromDeptId, string OfficeId, string ActionCode, string userid, out DataTable dtStatus, string Type, int FileType, int RefType, int ShowType, string DashType, string FinancialYear, string DestinationDept)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(ActionCode))
            {
                ActionCode = null;
            }
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("@officeid", OfficeId));
                p.Add(new SqlParameter("@documentType", ActionCode));
                p.Add(new SqlParameter("@deptid", DepartmentID));
                p.Add(new SqlParameter("@Userid", userid));
                p.Add(new SqlParameter("@Type", Type));
                p.Add(new SqlParameter("@FileType", FileType));
                p.Add(new SqlParameter("@RefType", RefType));
                p.Add(new SqlParameter("@ShowType", ShowType));
                p.Add(new SqlParameter("@FromDeptId", FromDeptId));
                p.Add(new SqlParameter("@Fyear", string.IsNullOrEmpty(FinancialYear) ? DBNull.Value : (object)FinancialYear));
                p.Add(new SqlParameter("@homeType", DashType));
              

                con.Close();
               
                string spName;
                if (CurrentSession.DeptID == "LG00001" || CurrentSession.DeptID == "UDD0001")
                {
                    spName = "DashboardStatsLDetailDepartment_LG";
                    p.Add(new SqlParameter("@DestinationDept", DestinationDept));
                }
                else
                {
                    spName = "DashboardStatsLDetailDepartment_MC";
                }

                ds = SqlHelper.ExecuteDataset(con, spName, p.ToArray());



                DataView view = new DataView(ds.Tables[0]);
                DataTable dtDeptName = view.ToTable(true, "MettingDate", "MettingName", "Resolution", "FileReference", "ID", "SentTo", "CurrentlyWith", "FileType", "sr", "filepath");
                dtStatus = ds.Tables[1].Copy();
                dt.Clear();
                dt.Columns.Add("MettingDate");
                dt.Columns.Add("MettingName");
                dt.Columns.Add("Resolution");
                dt.Columns.Add("SentTo");
                dt.Columns.Add("CurrentlyWith");
                dt.Columns.Add("FileType");
                dt.Columns.Add("sr");
                dt.Columns.Add("filepath");
                DataTable All = new DataTable();
                All = dtDeptName.Copy();
                DataTable resultDataTable = new DataTable();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {

                        DataRow dr11 = dt.NewRow();
                        string s = dr["DocumentTypeName"].ToString();
                        // DataTable dtResult1 = tPaperLaidV.GetUserActionCount(DepartmentID, Convert.ToInt16(dr["DocumentTypeId"]), dr["DocumentTypeName"].ToString(), Type, userid, Convert.ToInt32(OfficeId));
                        DataTable dtResult1 = tPaperLaidV.GetUserActionCount(DepartmentID, Convert.ToInt16(dr["DocumentTypeId"]), dr["DocumentTypeName"].ToString(), Type, userid, Convert.ToInt32(CurrentSession.OfficeId));


                        var joinedData = from table1 in All.AsEnumerable()
                                         join table2 in dtResult1.AsEnumerable()
                                         on (long?)table1["ID"] equals (long?)table2["ID"]
                                         select new
                                         {
                                             ID = table1.Field<long>("ID"),
                                             FileReference = table1.Field<string>("FileReference"),
                                             MettingDate = table1.Field<DateTime>("MettingDate"),
                                             MettingName = table1.Field<string>("MettingName"),
                                             Resolution = table1.Field<int>("Resolution"),
                                             SentTo = table1.Field<string>("SentTo"),
                                             CurrentlyWith = table1.Field<string>("CurrentlyWith"),
                                             FileType = table1.Field<string>("FileType"),
                                             sr = table1["sr"].ToString(),
                                             filepath = table1.Field<string>("filepath"),
                                             s = s.ToString(),
                                         };


                        if (!resultDataTable.Columns.Contains("MettingName"))
                        {
                            resultDataTable.Columns.Add("ID", typeof(long));
                            resultDataTable.Columns.Add("FileReference", typeof(string));
                            resultDataTable.Columns.Add("MettingDate", typeof(string));
                            resultDataTable.Columns.Add("MettingName", typeof(string));
                            resultDataTable.Columns.Add("Resolution", typeof(int));
                            resultDataTable.Columns.Add("SentTo", typeof(string));
                            resultDataTable.Columns.Add("CurrentlyWith", typeof(string));
                            resultDataTable.Columns.Add("FileType", typeof(string));
                            resultDataTable.Columns.Add("sr", typeof(string));
                            resultDataTable.Columns.Add("filepath", typeof(string));
                        }
                        resultDataTable.Columns.Add(s, typeof(string));

                        if (resultDataTable.Rows.Count == 0)
                        {
                            foreach (var item in joinedData)
                            {
                                resultDataTable.Rows.Add(item.ID, item.FileReference, item.MettingDate, item.MettingName, item.Resolution, item.SentTo, item.CurrentlyWith, item.FileType, item.sr, item.filepath);
                            }
                        }

                        foreach (var item in joinedData)
                        {
                            DataRow matchingRow = resultDataTable.AsEnumerable().FirstOrDefault(row => row.Field<long>("ID") == item.ID);
                            matchingRow[s] = item.s;
                        }

                        dtResult1.Clear();
                    }
                }
                else
                {
                    resultDataTable = dtDeptName.Copy();
                }

                // For Zero Count Records adding row
                foreach (DataRow drr in ds.Tables[0].Rows)
                {
                    string refid = drr["ID"].ToString();
                    DataRow[] dr = resultDataTable.Select("ID=" + refid);
                    if (dr.Length > 0)
                    {

                    }
                    else
                    {
                        resultDataTable.Rows.Add(drr["ID"].ToString(), drr["FileReference"].ToString(), drr["MettingDate"].ToString(), drr["MettingName"].ToString(), "0", drr["SentTo"].ToString(), drr["CurrentlyWith"].ToString(), drr["FileType"].ToString(), Convert.ToString(drr["sr"]), drr["filepath"].ToString());
                    }
                }

                return resultDataTable;

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

        public static string ResolutionDetail(string ResolutionNo)
        {
            DataTable dt = new DataTable();
            SqlConnection con = ClsConnection.GetConnection();
            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                con.Open();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@ResolutionNo", ResolutionNo));
            con.Close();
            DataSet ds = SqlHelper.ExecuteDataset(con, "StatsLDetailbyResolutionNo", p.ToArray());
            if (ResolutionNo != "")
                return ds.Tables[0].Rows[0]["SubjectDetails"].ToString();
            else
                return "No Detail Found";
        }

        public static List<MCStatDetail> GetMCStatDetailsByFile(string Referenceno)
        {
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("@Referenceno", Referenceno));

                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "GetfileDetails", p.ToArray());

                if (ds.Tables.Count > 0)
                {
                    int s = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MCStatDetail item = new MCStatDetail();
                        item.SrNo = s;
                        item.AgendaDate = Convert.ToDateTime(dr["MettingDate"]);
                        item.Meeting = Convert.ToString(dr["MettingName"]);
                        item.Resolutionno = Convert.ToString(dr["ResolutionNo"]);
                        item.ActionName = Convert.ToString(dr["ActionName"]);
                        item.Subject = Convert.ToString(dr["Subject"]);
                        lstitems.Add(item);

                        s++;


                    };

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;

            }
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public long PaperLaidId { get; set; }


        [DisplayName("Rule")]
        public string RuleNo { get; set; }


        [NotMapped]
        [DisplayName("Ref.Number")]
        public string evidhanReferenceNumber { get; set; }



        [NotMapped]
        [DisplayName("Number")]
        public string NoticeNumber { get; set; }


        [AllowHtml]
        [Required()]
        [DisplayName("Subject")]

        public string Title { get; set; }

        [NotMapped]
        [DisplayName("Asked By")]
        public string MemberName { get; set; }



        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int? FileVersion { get; set; }

        //Changes based on 13th June 2014 Review
        [DisplayName("Paper")]
        [JqGridColumnLayout(Alignment = JqGridAlignments.Center)]
        [JqGridColumnFormatter("$.noticepaperSentFormatterBills")]
        [NotMapped]
        public string PaperSent { get; set; }

        [DisplayName("Sent on")]
        [JqGridColumnFormatter("$.noticepaperSentOnFormatter")]
        [NotMapped]
        public DateTime? DeptSubmittedDate { get; set; }

        [DisplayName("Desired Date")]
        [JqGridColumnFormatter("$.FixedDateOnFormatter")]
        //[JqGridColumnFormatter("$.paperSentOnFormatter")]
        //[JqGridColumnFormatter("$.noticeFixedDateOnFormatter")]
        public DateTime? DesireLayingDate { get; set; }


        [HiddenInput(DisplayValue = false)]
        public int AssemblyId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SessionId { get; set; }
        [NotMapped]
        public virtual ICollection<mDepartment> mDepartmentList { get; set; }



        [HiddenInput(DisplayValue = false)]
        public int EventId { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int MinistryId { get; set; }

        [StringLength(200)]
        [HiddenInput(DisplayValue = false)]
        public string MinistryName { get; set; }

        [NotMapped]
        [DisplayName("Status")]
        [HiddenInput(DisplayValue = false)]
        public int Status { get; set; }

        [StringLength(200)]
        [HiddenInput(DisplayValue = false)]
        public string MinistryNameLocal { get; set; }

        [Required]
        [StringLength(20)]
        [HiddenInput(DisplayValue = false)]
        public string DepartmentId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ChangedDepartmentId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? ChangedMinisteryId { get; set; }

        [StringLength(200)]
        [HiddenInput(DisplayValue = false)]
        public string DeparmentName { get; set; }

        [StringLength(200)]
        [HiddenInput(DisplayValue = false)]
        public string DeparmentNameByids { get; set; }

        [StringLength(200)]
        [HiddenInput(DisplayValue = false)]
        public string DeparmentNameLocal { get; set; }






        [AllowHtml]
        [Required()]
        [HiddenInput(DisplayValue = false)]

        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? PaperTypeID { get; set; }


        [HiddenInput(DisplayValue = false)]
        public int DesireLayingDateId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? CommitteeId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? CommitteeChairmanMemberId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? LOBRecordId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ProvisionUnderWhich { get; set; }


        [HiddenInput(DisplayValue = false)]
        public bool? IsClubbed { get; set; }
        [AllowHtml]
        [HiddenInput(DisplayValue = false)]

        public string Remark { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? AcknowledgmentBy { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? AcknowledgmentDate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? LaidDate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ModifiedBy { get; set; }


        [HiddenInput(DisplayValue = false)]
        public string CreatedBy { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string OriDiaryFilePath { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string OriDiaryFileName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string NoticePdfPath { get; set; }



        [HiddenInput(DisplayValue = false)]
        public DateTime? ModifiedWhen { get; set; }

        [HiddenInput(DisplayValue = false)]
        public long? DeptActivePaperId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public long? MinisterActivePaperId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public long? CommitteeActivePaeperId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public long? LOBPaperTempId { get; set; }

        //Paging Properties
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int PAGE_SIZE { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int PageIndex { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int ResultCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int loopStart { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int loopEnd { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int selectedPage { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int PageNumber { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int RowsPerPage { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int OtherPaperCount { get; set; }



        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public virtual ICollection<mDepartment> mDepartment { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public virtual ICollection<mSession> mSession { get; set; }





        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public virtual ICollection<mSessionDate> mSessionDates { get; set; }

        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public virtual List<mSessionDate> mSessionDate { get; set; }















        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public int AssemblyCode { get; set; }
        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public int SessionCode { get; set; }
        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public int QuestionTypeId { get; set; }
        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public int noticeTypeId { get; set; }
        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public int QuestionID { get; set; }
        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public int BillTypeId { get; set; }


        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int NoticeID { get; set; }

        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public string categoryType { get; set; }

        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public string FileName { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string FilePath { get; set; }


        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string DSCApplicable { get; set; }
        [NotMapped]

        public string isPdfFile { get; set; }
        [NotMapped]

        public string isDocFile { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int Count { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string RespectivePaper { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public List<SelectListItem> yearList { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string actualFilePath { get; set; }

        //Pending List
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public virtual ICollection<mPaperCategoryType> mPaperCategoryTypeList { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int PaperCategoryTypeId { get; set; }


        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public virtual List<tPaperLaidV> ListtPaperLaidV { get; set; }




        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public IList<tPaperLaidV> MinisterPendingDataList { get; set; }


        // public IList<mEvent> mCategoryTypeList { get; set; }








        //Bill variables
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage = "Please Enter Memorandum Number")]
        public int BillNUmberValue { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int BillNumberYear { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string BillDate { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int MinInboxCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int MinSentCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string SessionName { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string AssesmblyName { get; set; }

        #region Added By Me :)

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string CurrentUserName { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string UserDesignation { get; set; }



        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int? QuestionNumber { get; set; }

        #endregion

        #region Added By Venkat

        [HiddenInput(DisplayValue = false)]
        public int DepartmentwisePriority { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int MinistrywisePriority { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int PaperlaidPriority { get; set; }
        //   [HiddenInput(DisplayValue = false)] 
        //public string AssemblyString { get; set; }
        #endregion


        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string LoginId { get; set; }

        //[NotMapped]
        //[HiddenInput(DisplayValue = false)]
        //public int MinistryID { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string MinisterName { get; set; }




        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public Guid UserID { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string HashKey { get; set; }



        //Himanshu
        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public string EventName { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string MinisterSubmittedDate { get; set; }



        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string BussinessType { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string StatusText { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string lSM { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string CustomMessage { get; set; }

        //Add For RequestID --Sanjay
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string RequestMessage { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string ProgressText { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string ProgressValue { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public ICollection<tPaperLaidV> DepartmentProgressList { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int BOTStatus { get; set; }




        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string AssesmblyNameLocal { get; set; }


        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string SessionNameLocal { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string DiaryNumber { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int? SessionDateId { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public bool IsPermissions { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string MergeDiaryNo { get; set; }



        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public List<mSession> sessionList { get; set; }



        [HiddenInput(DisplayValue = false)]
        public int? MemberId { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int ModuleId { get; set; }


        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string Subject { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public bool? IsAmendment { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int BillId { get; set; }



        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public bool? IsHOD { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool? IsLaid { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string DocFilePath { get; set; }



        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public string DocFileName { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int? SupplementaryFileVersion { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string SupplementaryFilePath { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string DocSupplementaryFilePath { get; set; }

        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public string SupFileName { get; set; }

        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public string SupDocFileName { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string SupFilePath { get; set; }

        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public string SupDocFilepath { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int? SupFileVersion { get; set; }
        //sameer
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string TempDocFile { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string TempPdfFile { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string MobileNo { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int Length { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string Type { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string TempSuplDocFile { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string SuplDocFileStatus { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int RuleId { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int DocTypeId { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int? DocFileVersion { get; set; }


        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int SupPDFCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int MainDocCount { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool? IsBracket { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsVSSecMarked { get; set; }
        ///new added by dharmendra for bil////
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string BillNoPart1 { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string BillNoPart2 { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string FileStructurePath { get; set; }


        //Added venkat code for Dynamic Menu
        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public int TotalStaredPendingForSubmission1 { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int TotalStaredReceived1 { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string PaperEntryType { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string ModuleName { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int? MinID { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string BType1 { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int? BType { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int ReceiptCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int DispatchCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int TotalStaredPendingForSubmissionNew { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int TotalUnStaredPendingForSubmissionNew { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int TotalStaredReplySent { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int TotalUnStaredReplySent { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int NoticeDraftCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int NoticeSentCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int NoticePendingCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int NoticeTotalCount { get; set; }




        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public string DeptSubmittedDateNew { get; set; }
        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public DateTime SessionDate { get; set; }
        [HiddenInput(DisplayValue = false)]
        [NotMapped]
        public bool IsShowPaid { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int TotalStaredPendingAndDraft { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int TotalUnStaredPendingAndDraft { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public long PaperLaidTempId { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public bool? IsValid { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public DateTime? IsAcknowledgmentDate { get; set; }



        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string AcknowledgmentDateString { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string Heading { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string HeadingLocal { get; set; }



        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public virtual List<tPaperLaidV> ListtPaperLaidVSub { get; set; }
        [DisplayName("Sent By MC")]
        [NotMapped]
        public string SubmittedDateString { get; set; }


        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int BranchId { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int CurtSessBranchId { get; set; }


        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int? ReportTypeId { get; set; }

        //For LOB NEW
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public DateTime? DateFromis { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public DateTime? DateTois { get; set; }



        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public virtual List<tPaperLaidV> ListtPaperLaidVNotice { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public virtual List<tPaperLaidV> ListtPaperLaidVBills { get; set; }



        //[NotMapped]
        //[UIHint("tinymce_jquery_full_LOB"), AllowHtml]
        //public string TextLOB { get; set; }

        [NotMapped]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        [HiddenInput(DisplayValue = false)]
        public string TextLOB { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int NoticeCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int BillsCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int OtherPapersCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int CommitteeRepCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int SelectedIndexId { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int RowIndexId { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public string DesireLayingDateString { get; set; }



        //LOB New
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int MarkedPapersCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int UnMarkedPapersCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int NoticeLaidCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int OtherPapersLaidCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int OtherPapersPendingToLayCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int OtherPapersForLayingCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int BillsLaidCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int BillsPendingToLayCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int BillsForLayingCount { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int CommitteeReportsLaidCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int CommitteeReportsPendingToLayCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int CommitteeReportsForLayingCount { get; set; }
        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public bool IsLOBAttached { get; set; }

        public string Email { get; set; }

        public string LoginTime { get; set; }

        public string LogOutTime { get; set; }
        public List<tPaperLaidV> _LstTPaperLaid;

        public static List<tPaperLaidV> GetUserDetails(string id)
        {
            List<tPaperLaidV> lstitems = new List<tPaperLaidV>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@UserId", id);


                con.Close();
                foreach (DataRow dr in CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetUserDetails", param).Tables[0].Rows)
                {
                    tPaperLaidV item = new tPaperLaidV();
                    {

                        item.Name = Convert.ToString(dr["UserName"]);
                        item.Email = Convert.ToString(dr["Email"]);
                        item.MobileNo = Convert.ToString(dr["Mobile"]);


                        string startTimeValue = Convert.ToString(dr["LoginTime"]);
                        TimeSpan startTime;
                        if (TimeSpan.TryParse(startTimeValue, out startTime))
                        {
                            item.LoginTime = Converter.ConvertTime(Convert.ToString(startTime));
                        };

                        string endTimeValue = Convert.ToString(dr["LogOutTime"]);
                        TimeSpan endTime;
                        if (TimeSpan.TryParse(endTimeValue, out endTime))

                        {
                            item.LogOutTime = Converter.ConvertTime(Convert.ToString(endTime));
                        };
                    };
                    lstitems.Add(item);
                }

                return lstitems;
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
        public static List<MCStat> GetMCStatDetails(string Department, int IsApex)
        {
            List<MCStat> lstitems = new List<MCStat>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@DepartmentID", Department));
                p.Add(new SqlParameter("@IApex", Convert.ToInt16(IsApex)));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "Mc_stat", p.ToArray());
                // DataSet s= (DataSet)(CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetUserDetails", param).Tables[0]);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MCStat item = new MCStat();
                        item.DepartmentName = Convert.ToString(dr["Department"]);
                        item.TotalFiles = Convert.ToString(dr["Total Files"]);
                        item.TotalResolutionsDraft = Convert.ToString(dr["Total Resolutions Drafted"]);
                        item.TotalResolutionsApproved = Convert.ToString(dr["Total Resolutions Approved"]);
                        item.TotalResolutionsPassed = Convert.ToString(dr["Total Resolutions Passed"]);
                        item.TotalResolutions = Convert.ToString(dr["Total Resolutions"]);
                        item.TotalResolutionsPending = Convert.ToString(dr["Total Resolutions Pending"]);
                        item.DeptID = Convert.ToString(dr["Deptid"]);
                        item.TotalResolutionsRejected = Convert.ToString(dr["Total Resolutions Rejected"]);
                        item.TotalResolutionsClerified = Convert.ToString(dr["Total Resolutions Clerified"]);
                        item.TotalResolutionsClerification = Convert.ToString(dr["Total Resolutions For Clerification"]);
                        lstitems.Add(item);

                    };

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;

            }
        }

        public static List<MCStatDetail> GetMCStatDetailsList(string DepartmentID, string FromDeptId,string OfficeId, string ActionCode, string userid, string Type, int FileType, int RefType,int ShowType)
        {
            if (string.IsNullOrEmpty(ActionCode))
            {
                ActionCode = null;
            }
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("@officeid", OfficeId));
                p.Add(new SqlParameter("@documentType", ActionCode));
                p.Add(new SqlParameter("@deptid", DepartmentID));
                p.Add(new SqlParameter("@Userid", userid));
                p.Add(new SqlParameter("@Type", Type));
                p.Add(new SqlParameter("@FileType", FileType));
                p.Add(new SqlParameter("@RefType", RefType));
                p.Add(new SqlParameter("@ShowType", ShowType));
                p.Add(new SqlParameter("@FromDeptId", FromDeptId));
                con.Close();
                 DataSet ds = SqlHelper.ExecuteDataset(con, "DashboardStatsLDetail", p.ToArray());

                if (ds.Tables.Count > 0)
                {
                    int s = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string padency = GetPendancy(Convert.ToString(dr["Referance"]));

                        MCStatDetail item = new MCStatDetail();
                        item.SrNo = s;
                        item.AgendaDate = Convert.ToDateTime(dr["MettingDate"]);
                        item.ReferenceId = Convert.ToString(dr["Referance"]);
                        item.Meeting = Convert.ToString(dr["MettingName"]);
                        item.TotalResolutions = Convert.ToString(dr["Resolution"]);

                        item.Resolutionno = Convert.ToString(dr["ResolutionNo"]);
                        item.Type = Convert.ToString(dr["type"]);

                        item.ActionName = Convert.ToString(dr["ActionName"]);
                        item.Subject = Convert.ToString(dr["Subject"]);
                        item.FromDepartment = Convert.ToString(dr["FromDepartment"]);
                        item.FromOffice = Convert.ToString(dr["FromOffice"]);
                        item.ToDepartment = Convert.ToString(dr["ToDepartment"]);
                        item.ToOffice = Convert.ToString(dr["ToOffice"]);
                        item.DocType = Convert.ToString(dr["DocumentTypeName"]);
                        item.Pendency = padency;
                        lstitems.Add(item);

                        s++;


                    };

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;

            }
        }

        public static List<MCStatDetail> GetMCStatDetailsListDepartment(string DepartmentID, string FromDeptId, string OfficeId, string ActionCode, string userid, string Type, int FileType, int RefType, int ShowType, string DashType, string FinancialYear, string DestinationDept)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(ActionCode))
            {
                ActionCode = null;
            }
            if (CurrentSession.DeptID != "LG00001" && CurrentSession.DeptID != "UDD0001")
            {
                FromDeptId = CurrentSession.DeptID;
            }
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("@officeid", CurrentSession.OfficeId));
                p.Add(new SqlParameter("@documentType", ActionCode));
                p.Add(new SqlParameter("@deptid", DepartmentID));
                p.Add(new SqlParameter("@Userid", userid));
                p.Add(new SqlParameter("@Type", Type));
                p.Add(new SqlParameter("@FileType", FileType));
                p.Add(new SqlParameter("@RefType", RefType));
                p.Add(new SqlParameter("@ShowType", ShowType));
                p.Add(new SqlParameter("@FromDeptId", FromDeptId));
                p.Add(new SqlParameter("@Fyear", string.IsNullOrEmpty(FinancialYear) ? DBNull.Value : (object)FinancialYear));
                p.Add(new SqlParameter("@homeType", DashType));
                con.Close();
                string spName;
                if (CurrentSession.DeptID == "LG00001" || CurrentSession.DeptID == "UDD0001")
                {
                    spName = "DashboardStatsLDetailDepartment_LG";
                    p.Add(new SqlParameter("@DestinationDept", DestinationDept));
                }
                else
                {
                    spName = "DashboardStatsLDetailDepartment_MC";
                }

                // Execute
                ds = SqlHelper.ExecuteDataset(con, spName, p.ToArray());

                if (ds.Tables.Count > 0)
                {
                    int s = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string padency = GetPendancy(Convert.ToString(dr["Referance"]));

                        MCStatDetail item = new MCStatDetail();
                        item.SrNo = s;
                        item.AgendaDate = Convert.ToDateTime(dr["MettingDate"]);
                        item.ReferenceId = Convert.ToString(dr["Referance"]);
                        item.Meeting = Convert.ToString(dr["MettingName"]);
                        item.TotalResolutions = Convert.ToString(dr["Resolution"]);
                        item.Resolutionno = Convert.ToString(dr["ResolutionNo"]);
                        item.Type = Convert.ToString(dr["type"]);
                        item.ActionName = Convert.ToString(dr["ActionName"]);
                        item.Subject = Convert.ToString(dr["Subject"]);
                        item.FromDepartment = Convert.ToString(dr["FromDepartment"]);
                        item.FromOffice = Convert.ToString(dr["FromOffice"]);
                        // item.ToDepartment = Convert.ToString(dr["ToDepartment"]);
                        item.ToDepartment = "";
                        // item.ToOffice = Convert.ToString(dr["ToOffice"]);
                        item.ToOffice = "";
                        // item.DocType = Convert.ToString(dr["DocumentTypeName"]);
                        item.DocType = "";
                        item.Pendency = padency;
                        lstitems.Add(item);

                        s++;


                    };

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;

            }
        }

        public static List<MCStatDetail> GetMCStatDetailsListDepartmentPendency(string DepartmentID, string FromDeptId, string OfficeId, string ActionCode, string userid, string Type, int FileType, int RefType, int ShowType, string DashType, string FinancialYear)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(ActionCode))
            {
                ActionCode = null;
            }
            if (CurrentSession.DeptID != "LG00001" && CurrentSession.DeptID != "UDD0001")
            {
                FromDeptId = CurrentSession.DeptID;
            }
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("@officeid", CurrentSession.OfficeId));
                p.Add(new SqlParameter("@documentType", ActionCode));
                p.Add(new SqlParameter("@deptid", DepartmentID));
                p.Add(new SqlParameter("@Userid", userid));
                p.Add(new SqlParameter("@Type", Type));
                p.Add(new SqlParameter("@FileType", FileType));
                p.Add(new SqlParameter("@RefType", RefType));
                p.Add(new SqlParameter("@ShowType", ShowType));
                p.Add(new SqlParameter("@FromDeptId", FromDeptId));
                p.Add(new SqlParameter("@Fyear", string.IsNullOrEmpty(FinancialYear) ? DBNull.Value : (object)FinancialYear));
                p.Add(new SqlParameter("@homeType", DashType));
                con.Close();
             
                ds = SqlHelper.ExecuteDataset(con, "[DashboardStatsLDetailDepartmentPendency]", p.ToArray());
                

                if (ds.Tables.Count > 0)
                {
                    int s = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string padency = GetPendancy(Convert.ToString(dr["Referance"]));

                        MCStatDetail item = new MCStatDetail();
                        item.SrNo = s;
                        item.AgendaDate = Convert.ToDateTime(dr["MettingDate"]);
                        item.ReferenceId = Convert.ToString(dr["Referance"]);
                        item.Meeting = Convert.ToString(dr["MettingName"]);
                        item.TotalResolutions = Convert.ToString(dr["Resolution"]);
                        item.Resolutionno = Convert.ToString(dr["ResolutionNo"]);
                        item.Type = Convert.ToString(dr["type"]);
                        item.ActionName = Convert.ToString(dr["ActionName"]);
                        item.Subject = Convert.ToString(dr["Subject"]);
                        item.FromDepartment = Convert.ToString(dr["FromDepartment"]);
                        item.FromOffice = Convert.ToString(dr["FromOffice"]);
                        // item.ToDepartment = Convert.ToString(dr["ToDepartment"]);
                        item.ToDepartment = "";
                        // item.ToOffice = Convert.ToString(dr["ToOffice"]);
                        item.ToOffice = "";
                        // item.DocType = Convert.ToString(dr["DocumentTypeName"]);
                        item.DocType = "";
                        item.Pendency = padency;
                        lstitems.Add(item);

                        s++;


                    }
                    ;

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;

            }
        }




        public static List<MCStat> GetMCStatDetailsOfficeWise(string Department)
        {
            List<MCStat> lstitems = new List<MCStat>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
                p.Add(new SqlParameter("@DepartmentID", Department));
                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "MC_Stat_OfficeWise", p.ToArray());
                // DataSet s= (DataSet)(CMNirdesh.Models.SqlHelper.ExecuteDataset(con, "GetUserDetails", param).Tables[0]);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MCStat item = new MCStat();
                        item.OfficeName = Convert.ToString(dr["Office"]);
                        item.TotalFiles = Convert.ToString(dr["Total Files"]);
                        item.TotalResolutions = Convert.ToString(dr["Total Resulation Passed"]);
                        item.OfficeId = Convert.ToString(dr["OfficeId"]);
                        item.DeptID = Convert.ToString(dr["DepartmentID"]);
                        item.TotalWorks = Convert.ToString(dr["Total Priority Works"]);
                        item.TotalImplemented = Convert.ToString(dr["Total Works Implemented"]);
                        item.TotalUImplementation = Convert.ToString(dr["Total Works Under Implementation"]);
                        item.TotalNotImplementation = Convert.ToString(dr["Total Works Not Implemented"]);
                        lstitems.Add(item);

                    };

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;

            }
        }



        public static List<MCStatDetail> GetMCStatDetailsIndivisualList(string DepartmentID, string OfficeId, string ActionCode, string userid, int RefType, string selectedType, string MeetingType="")
        {
            if (string.IsNullOrEmpty(ActionCode))
            {
                ActionCode = null;
            }
            List<MCStatDetail> lstitems = new List<MCStatDetail>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                p.Add(new SqlParameter("@officeid", OfficeId));
                p.Add(new SqlParameter("@documentType", ActionCode));
                p.Add(new SqlParameter("@deptid", DepartmentID));
                p.Add(new SqlParameter("@Userid", userid));
                p.Add(new SqlParameter("@RefType", RefType));
                p.Add(new SqlParameter("@homeType", selectedType)); 
                p.Add(new SqlParameter("@mcType", (MeetingType == "") ? "" : Convert.ToString(MeetingType)));

                con.Close();
                DataSet ds = SqlHelper.ExecuteDataset(con, "DashboardStatsLDetailInvisualTest", p.ToArray());

                if (ds.Tables.Count > 0)
                {
                    int s = 1;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MCStatDetail item = new MCStatDetail();
                        item.SrNo = s;
                        item.AgendaDate = Convert.ToDateTime(dr["LetterDate"]);
                        item.ReferenceId = Convert.ToString(dr["Referance"]);
                        item.ActionName = Convert.ToString(dr["ActionName"]);
                        item.Subject = Convert.ToString(dr["Subject"]);
                        item.Resolutionno = Convert.ToString(dr["ResolutionNo"]);
                        item.FromDepartment = Convert.ToString(dr["FromDepartment"]);
                        item.FromOffice = Convert.ToString(dr["FromOffice"]);
                        item.ToDepartment = Convert.ToString(dr["ToDepartment"]);
                        item.ToOffice = Convert.ToString(dr["ToOffice"]);
                        item.DocType = Convert.ToString(dr["DocumentTypeName"]);
                        lstitems.Add(item);

                        s++;


                    };

                }


                return lstitems;
            }
            catch (Exception ex)
            {
                return lstitems;

            }
        }




        public static string GetPendancy(string Resolution)
        {
           var dept=  CurrentSession.DeptID;

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
                        new SqlParameter("@RefrenceId", Resolution),
                        new SqlParameter("@Todept", dept)   // second parameter
                    };
                ds = SqlHelper.ExecuteDataset(connection, "ResolutionPendencyNotApprovedorRejected", parameterValues);
                string res = Convert.ToString(ds.Tables[0].Rows[0]["Pendancy"]);
                return res;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return "No Pendency calculated";

        }
    }
}

    

