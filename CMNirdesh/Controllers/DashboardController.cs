using CMNirdesh.Filters;
using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMNirdesh.Models.PaperLaid;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Drawing;
using CMNirdesh.Models.Diaries;
using FundReportModel = CMNirdesh.Models.Diaries.FundReportModel;
using System.Data;
using System.Text;
using CMNirdesh.Error;
using ErrorLog = CMNirdesh.Error.ErrorLog;

using SelectPdf;

using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web.Security;
using System.Data.SqlClient;

namespace CMNirdesh.Controllers
{

    public class DashboardController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }



        //public static DataTable getDashboardData(int status, int offcieId, string startDt, string endDt, string since, string userid, out DataTable dtStatus)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        SqlConnection connection = ClsConnection.GetConnection();
        //        if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
        //        {
        //            connection.Open();
        //        }
        //        connection.Close();
        //        SqlParameter[] parameterValues = new SqlParameter[] {
        //            new SqlParameter("@status", Convert.ToInt32(status)),
        //            new SqlParameter("@offcieId", offcieId),
        //            new SqlParameter("@startDt", startDt),
        //            new SqlParameter("@endDt", endDt),
        //            new SqlParameter("@since", since) ,
        //            new SqlParameter("@userid", Convert.ToString(userid))

        //        };

        //        DataSet ds = SqlHelper.ExecuteDataset(connection, "[DiaryDashbordBind1neww]", parameterValues);
        //        DataView view = new DataView(ds.Tables[0]);
        //        DataTable dtDeptName = view.ToTable(true, "deptname", "deptId");
        //        dtStatus = ds.Tables[1].Copy();

        //        dt.Clear();
        //        dt.Columns.Add("deptname");
        //        foreach (DataRow dr in ds.Tables[1].Rows)
        //        {
        //            dt.Columns.Add(dr["DocumentTypeName"].ToString(), typeof(int));
        //        }

        //        dt.Columns.Add("deptId");

        //        foreach (DataRow dr1 in dtDeptName.Rows)
        //        {
        //            DataRow dr11 = dt.NewRow();

        //            dr11["deptname"] = dr1["deptname"].ToString();
        //            dr11["deptId"] = dr1["deptId"].ToString();
        //            DataTable dtResult = ds.Tables[0].Select("deptid LIKE '%" + dr1["deptId"].ToString() + "%'").CopyToDataTable();

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





    }
}
