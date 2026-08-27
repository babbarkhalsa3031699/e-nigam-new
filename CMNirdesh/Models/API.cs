using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using PoliteCaptcha;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public class APIConstituency
    {
        public int ConstituencyId { get; set; }
         public int Type { get; set; }
        public int Clicked { get; set; }
        public SelectList Constituencylist { get; set; }

        public bool IsOpen { get; set; }
        public bool EmpOpen { get; set; }
        public List<Constituency> Officelist { get; set; }
        public List<PswcOffice> OfficeData { get; set; }
        public List<EmployeeData> EmployeeData { get; set; }
        public string PostDeptID { get; set; }
        public int PostOfficeID { get; set; }
        public PswcOffice OfficeDetial { get; set; }
        public string typechkbox { get; set; }
    }
    public class Constituency
    {
        public int ConstituencyId { get; set; }
        public string ConstituencyName { get; set; }

    }

    public class PswcRequest
    {
        public int StateId { get; set; }
        public int ConstituencyID { get; set; }
        public string PostDeptID { get; set; }
        public int PostOfficeID { get; set; }

        
    }

    public class PswcOffice
    {
        public string DepartmentID { get; set; }
        public string DepartmentAbbr { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentNameLocal { get; set; }
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string OfficeNameLocal { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficeLandlineNumber { get; set; }
        public string OfficeEmailID { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        public string OfficeHead { get; set; }
        public string OfficeHeadContactNo { get; set; }
        public string Flag { get; set; }

    }
    public class PswcResponse
    {
        public bool Status { get; set; }
        public List<PswcOffice> Data { get; set; }
    }


    public class EmployeeData
    {
        public string EmployeeName { get; set; }
        public string EmployeeNameLocal { get; set; }
        public string Designation { get; set; }
        public string DesignationLocal { get; set; }
        public string MobileNumber { get; set; }
        public string eMailID { get; set; }
        public string DateOfJoiningCurrentOffice { get; set; }
       
    }

    public class EmployeeApiResponse
    {
        public bool status { get; set; }
        public List<EmployeeData> data { get; set; }
    }
    public class APIlist
    {
        public static List<Constituency> GetConstituency(string consid)
        {
            List<Constituency> model = new List<Constituency>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();
             
                con.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@consid", consid)  };
                DataSet ds = SqlHelper.ExecuteDataset(con, "[GetDistrictConstituency]", parameterValues);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Constituency item = new Constituency();
                        item.ConstituencyId = Convert.ToInt32(dr["ConstituencyCode"]);
                        item.ConstituencyName = Convert.ToString(dr["ConstituencyName"]);
                        model.Add(item);
                    };

                }
                return model;

            }
            catch (Exception ex)
            {
                return model;

            }

        }

        public static SelectList GetDistrictConstituency(string consid)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@consid", consid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetDistrictConstituency]", parameterValues);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ConstituencyName"].ToString());
                    item.Value = Convert.ToString(row["ConstituencyCode"].ToString());
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

        public DataTable UpdateFlag(int ConstituencyId, int OfficeId, int flag)
        {
            SqlConnection connection = ClsConnection.GetConnection();
            if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
            {
                connection.Open();
            }
            connection.Close();
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@ConstituencyId", ConstituencyId));
            p.Add(new SqlParameter("@OfficeId", OfficeId));
            p.Add(new SqlParameter("@flag", flag));
            var dtt = SqlHelper.ExecuteDataset(connection, "InsertFlagOffice", p.ToArray());
            if (dtt.Tables.Count == 0)
            {
                return null;
            }
            DataTable dt = dtt.Tables[0];
            return dt;
        }


        public static List<PswcOffice> GetFlagedOffices(string consid)
        {
            List<PswcOffice> model = new List<PswcOffice>();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
                var p = new List<SqlParameter>();

                con.Close();
                SqlParameter[] parameterValues = new SqlParameter[] {
                new SqlParameter("@consid", consid)  };
                DataSet ds = SqlHelper.ExecuteDataset(con, "[GetFlagedOffices]", parameterValues);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        PswcOffice item = new PswcOffice();
                        item.OfficeID = dr["OfficeId"].ToString();
                        item.Flag =dr["OfficeId"].ToString();
                        model.Add(item);
                    };

                }
                return model;

            }
            catch (Exception ex)
            {
                return model;

            }

        }



    }


    public class VisitorModel
    {
        public int visitorId { get; set; }
        public string visitorName { get; set; }
        public string fatherName { get; set; }
        public string phoneNumber { get; set; }
        public string designation { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string bloodGroup { get; set; }
        public string idCardType { get; set; }
        public string idNumber { get; set; }
        public string photoName { get; set; }
        public string photoPath { get; set; }
        public string document { get; set; }
        public string status { get; set; }
        public string pdfUrl { get; set; }
        public string visitorRefID { get; set; }
        public string approvalID { get; set; }
        public string assemblyName { get; set; }
        public string sessionName { get; set; }
        public string gallaryName { get; set; }
        public string createdOn { get; set; }
        public string Name { get; set; }
        public string mediaHouse { get; set; }
        public string addOns { get; set; }
        public string sessionDate { get; set; }
        public string lying_With { get; set; }
        public string modifiedName { get; set; }

        public string sessionFromDate { get; set; }
        public string sessionToDate { get; set; }

    }
} 