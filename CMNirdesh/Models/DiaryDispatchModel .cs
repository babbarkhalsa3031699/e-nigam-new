using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using CMNirdesh.Models;
using System.ComponentModel;
using CMNirdesh.Error;

namespace CMNirdesh.Models
{
    public class DiaryDispatchModel
    {
        public DataTable DairyDispatchReport(int officeid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            DataTable data = new DataTable();
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@officeid", officeid);
                var dtt = SqlHelper.ExecuteDataset(connection, "getreportbyloginIdReport", p);

                if (dtt.Tables.Count == 0)
                {
                    return data;
                }
                data = dtt.Tables[0];
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable DairyDispatchReportNew(int officeid)
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
            var dtt = SqlHelper.ExecuteDataset(connection, "getreportbyloginId", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable ShowEditButtonDiarySearchList(string userid, string type)
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
            p.Add(new SqlParameter("@type", type));
            var dtt = SqlHelper.ExecuteDataset(connection, "getButtonPermission", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        public DataTable getsubjectandsubjectDetails(Int64 refid)
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
            var dtt = SqlHelper.ExecuteDataset(connection, "getSubjectandsubjectDetail", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public DataTable getsubjectandsubjectDetailsAll(Int64 refid)
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
            var dtt = SqlHelper.ExecuteDataset(connection, "getSubjectandsubjectDetailAll", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }




        public DataTable LinkedReferences(string userid, Int64 refid,  Int32 OfficeId) 
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
            p.Add(new SqlParameter("@refid", refid));
            p.Add(new SqlParameter("@OfficeId", OfficeId));
            var dtt = SqlHelper.ExecuteDataset(connection, "getLinkedReferences", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }
        

        //ShowEditButtonDiarySearchList

        public DataTable SearchByreferenceId(Int64 advanceparam)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@advanceparam", advanceparam));
            var dtt = SqlHelper.ExecuteDataset(connection, "advanceSearchForDiary", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable AdvanceSearchForDiary(string advanceparam, int officeid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@advanceparam", advanceparam));
            p.Add(new SqlParameter("@officeid", officeid));
            var dtt = SqlHelper.ExecuteDataset(connection, "advanceSearchForDiary", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable AdvanceSearchForDispatch(string advanceparam, int officeid)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@advanceparam", advanceparam));
            p.Add(new SqlParameter("@officeid", officeid));
            var dtt = SqlHelper.ExecuteDataset(connection, "advanceSearchForDispatch", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAllDiaryCount(int officeid, string startDt, string endDt, string since)
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
            p.Add(new SqlParameter("@startDt", startDt));
            p.Add(new SqlParameter("@endDt", endDt));
            p.Add(new SqlParameter("@since", since));
            var dtt = SqlHelper.ExecuteDataset(connection, "getAllDiaryCount", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable GetAllDispatchCount(int officeid, string startDt, string endDt, string since)
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
            p.Add(new SqlParameter("@startDt", startDt));
            p.Add(new SqlParameter("@endDt", endDt));
            p.Add(new SqlParameter("@since", since));
            var dtt = SqlHelper.ExecuteDataset(connection, "getAllDispatchCount", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable PromisesPrintData(int @promiseid, string Dept_Id)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();

            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@promiseid", promiseid));
            p.Add(new SqlParameter("@Dept_Id", Dept_Id));
            var dtt = SqlHelper.ExecuteDataset(connection, "getreportpromisesdatann", p.ToArray());

            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }

        public DataTable PromisesPrintDataN()
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            var dtt = SqlHelper.ExecuteDataset(connection, "getreportpromisesdataN");

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

    }
}