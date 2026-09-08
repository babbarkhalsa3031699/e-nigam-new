using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMNirdesh.Models;
using CMNirdesh.Models.Diaries;
using Microsoft.Reporting.WebForms;
using Org.BouncyCastle.Ocsp;

namespace CMNirdesh
{
    public partial class report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long refid = CurrentSession.referenceId;
                string rptPath = "DiaryRpt.rdlc";
                string paramValue = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(paramValue))
                {
                    if(paramValue=="2")
                    {
                        rptPath = "DiaryRptLocal.rdlc";
                    }
                }



                List<DiaryViewModel> lstDiaryViewModel = new List<DiaryViewModel>();
                lstDiaryViewModel = DiaryViewModel.GetDispatchDetailsById(refid);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("DiaryRpt.rdlc");
                ReportViewer1.LocalReport.ReportPath = Server.MapPath(rptPath);
                ReportViewer1.LocalReport.DataSources.Clear();


                //// Create a report parameter and set its value.
                //var reportParameter = new ReportParameter("DeptName", "CM Nirdesh");

                //// Add the parameter to the report.
                //ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { reportParameter });


                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsData", lstDiaryViewModel));

                List<DiaryActionViewModel> lstDiaryViewActionModel = new List<DiaryActionViewModel>();
                lstDiaryViewActionModel = DiaryViewModel.GetDispatchActionDetailsById(refid);

                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsActionsData", lstDiaryViewActionModel));

                DataTable data = new DataTable();
                DiaryViewModel dvm = new DiaryViewModel();
                data = dvm.GetIncludedRef(Convert.ToInt64(refid));

                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsReferenceData", data));

                


                Warning[] warnings;
                string[] streamIds;
                string contentType;
                string encoding;
                string extension; 

                byte[] bytes = ReportViewer1.LocalReport.Render("pdf", null, out contentType, out encoding, out extension, out streamIds, out warnings);

                //Download the RDLC Report in Word, Excel, PDF and Image formats.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = contentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=DiaryFile." + extension);
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();

            }
        }
    }
}