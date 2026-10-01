using CMNirdesh.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;

public class ReportsController : Controller
{
    // =========================================================
    // REPORT VIEW
    // =========================================================
    public ActionResult LatestResolutionReport()
    {
        return View();
    }


    // =========================================================
    // GET REPORT DATA
    // =========================================================
    [HttpGet]
   
    public JsonResult GetLatestResolutionReport(
    string ulbType,
    string fromDate,
    string toDate)
    {
        try
        {
            DateTime? from = null;
            DateTime? to = null;

            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                from = DateTime.Parse(fromDate);
            }

            if (!string.IsNullOrWhiteSpace(toDate))
            {
                to = DateTime.Parse(toDate);
            }

            DataTable dt = GetReportData(
                ulbType,
                from,
                to
            );

            var data = dt.AsEnumerable()
                .Select(row => new
                {
                    ULB =
                        row["ULB"] == DBNull.Value
                            ? ""
                            : row["ULB"].ToString(),

                    ResolutionNumber =
                        row["ResolutionNumber"] == DBNull.Value
                            ? ""
                            : row["ResolutionNumber"].ToString(),

                    MeetingDate =
                        row["MeetingDate"] == DBNull.Value
                            ? ""
                            : row["MeetingDate"].ToString(),

                    Status =
                        row["Status"] == DBNull.Value
                            ? ""
                            : row["Status"].ToString(),

                    CurrentlyWith =
                        row["CurrentlyWith"] == DBNull.Value
                            ? ""
                            : row["CurrentlyWith"].ToString(),

                    PendingSince =
                        row["PendingSince"] == DBNull.Value
                            ? 0
                            : Convert.ToInt32(row["PendingSince"]),

                    Branch =
                        row["Branch"] == DBNull.Value
                            ? ""
                            : row["Branch"].ToString()
                })
                .ToList();

            return Json(
                new
                {
                    success = true,
                    data = data
                },
                JsonRequestBehavior.AllowGet
            );
        }
        catch (Exception ex)
        {
            return Json(
                new
                {
                    success = false,
                    message = ex.Message
                },
                JsonRequestBehavior.AllowGet
            );
        }
    }


    // =========================================================
    // GET DATA FROM STORED PROCEDURE
    // =========================================================
    private DataTable GetReportData(
    string ulbType,
    DateTime? fromDate,
    DateTime? toDate)
    {
        DataTable dt = new DataTable();

        using (SqlConnection con = ClsConnection.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand(
                "GetLatestResolutionFileMovement",
                con))
            {
                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.Add(
                    "@ULBType",
                    SqlDbType.VarChar,
                    100
                ).Value =
                    string.IsNullOrWhiteSpace(ulbType)
                        ? (object)DBNull.Value
                        : ulbType.Trim();

                cmd.Parameters.Add(
                    "@FromDate",
                    SqlDbType.Date
                ).Value =
                    fromDate.HasValue
                        ? (object)fromDate.Value.Date
                        : DBNull.Value;

                cmd.Parameters.Add(
                    "@ToDate",
                    SqlDbType.Date
                ).Value =
                    toDate.HasValue
                        ? (object)toDate.Value.Date
                        : DBNull.Value;

                using (SqlDataAdapter da =
                       new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        return dt;
    }


    // =========================================================
    // EXPORT EXCEL
    // =========================================================
    [HttpGet]
    public ActionResult ExportLatestResolutionReport(
     string ulbType,
     string fromDate,
     string toDate)
    {
        try
        {
            DateTime? from = null;
            DateTime? to = null;


            // =====================================================
            // FROM DATE
            // =====================================================

            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                from = DateTime.Parse(fromDate);
            }


            // =====================================================
            // TO DATE
            // =====================================================

            if (!string.IsNullOrWhiteSpace(toDate))
            {
                to = DateTime.Parse(toDate);
            }


            // =====================================================
            // GET REPORT DATA
            // =====================================================

            DataTable dt =
                GetReportData(
                    ulbType,
                    from,
                    to
                );


            StringBuilder sb =
                new StringBuilder();


            // =====================================================
            // EXCEL HTML HEADER
            // =====================================================

            sb.AppendLine(
                "<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">"
            );

            sb.AppendLine("<head>");

            sb.AppendLine(
                "<meta http-equiv=\"Content-Type\" " +
                "content=\"text/html; charset=utf-8\" />"
            );


            // =====================================================
            // STYLE
            // =====================================================

            sb.AppendLine("<style>");

            sb.AppendLine(
                "table { " +
                "border-collapse: collapse; " +
                "width: 100%; " +
                "}"
            );

            sb.AppendLine(
                "th { " +
                "background-color: #1974d2; " +
                "color: white; " +
                "font-weight: bold; " +
                "border: 1px solid #000; " +
                "padding: 7px; " +
                "text-align: center; " +
                "}"
            );

            sb.AppendLine(
                "td { " +
                "border: 1px solid #000; " +
                "padding: 6px; " +
                "}"
            );

            sb.AppendLine(
                ".title { " +
                "font-size: 18px; " +
                "font-weight: bold; " +
                "text-align: center; " +
                "padding: 10px; " +
                "}"
            );

            sb.AppendLine("</style>");

            sb.AppendLine("</head>");

            sb.AppendLine("<body>");


            // =====================================================
            // REPORT TITLE
            // =====================================================

            sb.AppendLine("<table>");

            sb.AppendLine("<tr>");

            sb.AppendLine(
                "<td colspan=\"8\" class=\"title\">" +
                "Resolutions Pendency Report" +
                "</td>"
            );

            sb.AppendLine("</tr>");

            sb.AppendLine("</table>");

            sb.AppendLine("<br/>");


            // =====================================================
            // REPORT TABLE
            // =====================================================

            sb.AppendLine("<table>");


            // =====================================================
            // HEADER
            // =====================================================

            sb.AppendLine("<tr>");

            sb.AppendLine("<th>S.No.</th>");

            sb.AppendLine("<th>ULB</th>");

            sb.AppendLine("<th>Resolution Number</th>");

            sb.AppendLine("<th>Meeting Date</th>");

            sb.AppendLine("<th>Status</th>");

            sb.AppendLine("<th>Currently With</th>");

            sb.AppendLine("<th>Pending Since</th>");

            sb.AppendLine("<th>Branch</th>");

            sb.AppendLine("</tr>");


            // =====================================================
            // DATA
            // =====================================================

            int serialNo = 1;


            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine("<tr>");


                // =================================================
                // S.NO.
                // =================================================

                sb.Append("<td>");

                sb.Append(serialNo);

                sb.Append("</td>");


                // =================================================
                // ULB
                // =================================================

                sb.Append("<td>");

                sb.Append(
                    Server.HtmlEncode(
                        row["ULB"] == DBNull.Value
                            ? ""
                            : row["ULB"].ToString()
                    )
                );

                sb.Append("</td>");


                // =================================================
                // RESOLUTION NUMBER
                // =================================================

                sb.Append("<td>");

                sb.Append(
                    Server.HtmlEncode(
                        row["ResolutionNumber"] == DBNull.Value
                            ? ""
                            : row["ResolutionNumber"].ToString()
                    )
                );

                sb.Append("</td>");


                // =================================================
                // MEETING DATE
                // =================================================

                sb.Append("<td>");

                sb.Append(
                    Server.HtmlEncode(
                        row["MeetingDate"] == DBNull.Value
                            ? ""
                            : row["MeetingDate"].ToString()
                    )
                );

                sb.Append("</td>");


                // =================================================
                // STATUS
                // =================================================

                sb.Append("<td>");

                sb.Append(
                    Server.HtmlEncode(
                        row["Status"] == DBNull.Value
                            ? ""
                            : row["Status"].ToString()
                    )
                );

                sb.Append("</td>");


                // =================================================
                // CURRENTLY WITH
                // =================================================

                sb.Append("<td>");

                sb.Append(
                    Server.HtmlEncode(
                        row["CurrentlyWith"] == DBNull.Value
                            ? ""
                            : row["CurrentlyWith"].ToString()
                    )
                );

                sb.Append("</td>");


                // =================================================
                // PENDING SINCE
                // =================================================

                sb.Append("<td>");

                int pendingSince = 0;

                if (row["PendingSince"] != DBNull.Value)
                {
                    pendingSince =
                        Convert.ToInt32(
                            row["PendingSince"]
                        );
                }

                sb.Append(pendingSince);

                sb.Append(" Days");

                sb.Append("</td>");


                // =================================================
                // BRANCH
                // =================================================

                sb.Append("<td>");

                sb.Append(
                    Server.HtmlEncode(
                        row["Branch"] == DBNull.Value
                            ? ""
                            : row["Branch"].ToString()
                    )
                );

                sb.Append("</td>");


                sb.AppendLine("</tr>");

                serialNo++;
            }


            sb.AppendLine("</table>");

            sb.AppendLine("</body>");

            sb.AppendLine("</html>");


            // =====================================================
            // RETURN EXCEL FILE
            // =====================================================

            byte[] bytes =
                Encoding.UTF8.GetBytes(
                    sb.ToString()
                );


            return File(
                bytes,
                "application/vnd.ms-excel",
                "LatestResolutionReport_" +
                DateTime.Now.ToString(
                    "yyyyMMddHHmmss"
                ) +
                ".xls"
            );
        }
        catch (Exception ex)
        {
            return Content(
                "Error generating Excel: " +
                ex.Message
            );
        }
    }
}