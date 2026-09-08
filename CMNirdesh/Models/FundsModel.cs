using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

using CMNirdesh.Models;
using CMNirdesh.Error;
using System.Web.Mvc;
using Org.BouncyCastle.Ocsp;

namespace CMNirdesh.Models
{

    public class FundsReceiptModel
    {
        public int id { get; set; }
        public int FinYear { get; set; }
        public int DocTypeId { get; set; }
        public string FundName { get; set; }
        public int FundsReceipt { get; set; }
        public string AllocationDate { get; set; }
        public string DeptId { get; set; }
        public string OfficeId { get; set; }
        public string Remarks { get; set; }
        public string Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string attachedfile { get; set; }     

    }

    public class LstFundsReceipt
    {
        public int DocTypeId { get; set; }
        public int FinYear { get; set; }
        public int FundAmount { get; set; }
        public DateTime? AllocationDate { get; set; }
        public string Remarks { get; set; }
        public string attachfile { get; set; }
        public string IsFile { get; set; }
        public List<FundsReceiptModel> _LstFundsReceipt   {  get; set;  }
        public SelectList _Lstdoc   { get; set; }
        public int FilterFinYear { get; set; }
        public int TotalAmount { get; set; }

        public SelectList FinYearList { get; set; }

    }

    public class FundSanctionModel
    {
        public int id { get; set; }
        public int FinYear { get; set; }

        public Int64 RefId { get; set; }
        public int AmountSanctioned { get; set; }

        public int ApprovedAmount { get; set; }

        public DateTime? ApproveDate { get; set; }
        public DateTime? SanctionDate { get; set; }
        public string deptId { get; set; }

        public int officeid { get; set; }
        public string Remarks { get; set; }

        public string Createdby { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int FundType { get; set; }


    }

    public class FundReportModel
    {
        public int FundTypeId { get; set; }
        public string FundType { get; set; }
        public int FundAllocated { get; set; }
        public int FundSanctioned { get; set; }
        public int FundApproved { get; set; }
        public int FundAvaiable { get; set; }      
        public string DeptName { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }

    }

    public class LstFundReportModel
    {
        public List<FundReportModel> _LstFundReport { get; set; }
        public int FilterFinYear { get; set; }
        public SelectList FinYearList { get; set; }
    }

    public class SanctionDetailModel
    {
        public int DocmentType { get; set; }
        public string DocumentTypeName { get; set; }
        public int AmountApproved { get; set; }
        public int AmountSanctioned { get; set; }
        public string SanctionDate { get; set; }
        public Int64 RefId { get; set; }
        public string Dept { get; set; }
        public string Subject { get; set; }
        public string SubjectDetails { get; set; }
        public string Constituency { get; set; }

    }

    public class DocumentTypeModel
    {
        public int DocmentType { get; set; }
        public string DocumentTypeName { get; set; }


        public virtual List<DocumentTypeModel> Documenttypelist { get; set; }
    }


    public class FundFunctionModel
    {
        public static DataSet GetFundAvailable(int finyear,string userid)
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
                 SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@FinYear", finyear), new SqlParameter("@userid", userid) };
                 ds = SqlHelper.ExecuteDataset(connection, "[GetFundAvailable]", parameterValues);
            }
            catch (Exception ex)
            {
                CMNirdesh.Error.ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
            }
            return ds;
        }


        public static DataSet GetFundAvailableByDocId(int finyear,int DocId)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@FinYear", finyear), new SqlParameter("@DocId", DocId), };

                ds = SqlHelper.ExecuteDataset(connection, "[GetFundAvailableByDocId]", parameterValues);
                // ds = SqlHelper.ExecuteDataset(connection, "[GetFundAvailable]");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static DataSet GetFundByRefId(Int64 Refid)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@Refid", Convert.ToInt64(Refid))};

                ds = SqlHelper.ExecuteDataset(connection, "[GetFundbyRefId]", parameterValues);
                // ds = SqlHelper.ExecuteDataset(connection, "[GetFundAvailable]");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static DataSet GetSanctionDetails(int docid,int finyear)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@docid", docid), new SqlParameter("@Finyear", finyear) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetSanctionDetails]", parameterValues);
                // ds = SqlHelper.ExecuteDataset(connection, "[GetFundAvailable]");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetFundDocumentTypebyUser]", parameterValues).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["DocumentTypeName"].ToString());
                    item.Value = Convert.ToString(row["DocumentTypeId"].ToString());
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

        public static string SaveFund(FundsReceiptModel model)
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
                    new SqlParameter("@DocId", model.DocTypeId),
                    new SqlParameter("@fundamount", model.FundsReceipt),
                    new SqlParameter("@allocationdate", model.AllocationDate),

                    new SqlParameter("@deptid", model.DeptId),
                     new SqlParameter("@officeid", model.OfficeId),
                    new SqlParameter("@remarks", model.Remarks),
                    new SqlParameter("@createdby", model.Createdby),
                     new SqlParameter("@attachedfile", model.attachedfile)

                };
                string refid = Convert.ToString(SqlHelper.ExecuteScalar(connection, "[InsertReceiptFund]", parameterValues));
                connection.Close();
                return refid;
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString().ToString());
                return "0";
            }
        }


        public static DataSet GetReceiptFunds(int finyear,string userid)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@FinYear", finyear), new SqlParameter("@userid", userid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetReceiptFunds]", parameterValues);
                
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static DataSet GetMLAFund(int finyear,string deptid)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@FinYear", finyear), new SqlParameter("@DeptId", deptid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMLAFund]", parameterValues);
                // ds = SqlHelper.ExecuteDataset(connection, "[GetFundAvailable]");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


        public static DataSet GetMLASanctionDetails(int docid, int finyear,int officeid)
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
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@docid", docid), new SqlParameter("@Finyear", finyear) , new SqlParameter("@officeid", officeid) };
                ds = SqlHelper.ExecuteDataset(connection, "[GetMLASanctionDetails]", parameterValues);
               
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());

            }
            return ds;
        }


    }

    
}