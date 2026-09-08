using CMNirdesh.Error;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CMNirdesh.Models
{
    public static class Helper
    {
        public static object ExecuteService(string ModuleName, string MethodName, object Parameter)
        {

            try
            {


            }
            catch (Exception ex)
            {

                ErrorLog.WriteToLog(ex, "housecontrollerservice.ExecuteService");
            }

            return null;
        }


        // this is for create paging

        public static List<ListItem> BindPager(int recordCount, int currentPage, int pageSize)
        {
            double dblPageCount = (double)((decimal)recordCount / pageSize);
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 1)
            {
                pages.Add(new ListItem("<<", "1", true));
                if (currentPage > 4)
                {
                    int counnt = 0;
                    int diffrence = pageCount - currentPage;
                    if (diffrence >= 0 && diffrence < 5)
                    {
                        pages.Add(new ListItem("<", (pageCount - 5).ToString(), currentPage > 1));
                        for (int i = pageCount - 4; i <= pageCount; i++)
                        {

                            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                            counnt++;
                            if (counnt == 5)
                                break;
                        }
                        pages.Add(new ListItem(">", (currentPage + 1).ToString(), currentPage < pageCount));
                    }
                    else
                    {
                        pages.Add(new ListItem("<", (currentPage - 1).ToString(), currentPage > 1));
                        for (int i = currentPage; i <= pageCount; i++)
                        {
                            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                            counnt++;
                            if (counnt == 5)
                                break;
                        }
                        pages.Add(new ListItem(">", (currentPage + 1).ToString(), currentPage < pageCount));

                    }


                }
                else
                {

                    pages.Add(new ListItem("<", (currentPage - 1).ToString(), currentPage > 1));
                    for (int i = 1; i <= pageCount; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                        if (i == 5)
                            break;
                    }
                    pages.Add(new ListItem(">", (currentPage + 1).ToString(), currentPage < pageCount));

                }

                pages.Add(new ListItem(">>", pageCount.ToString(), true));

            }
            return pages;
        }

    }

    //public static class HtmlExtension
    //{
    //    #region Paging

    //    public static MvcHtmlString Pager(this HtmlHelper helper, int NoOfPages, int loopStart, int loopEnd, int currentPage)
    //    {
    //        if (NoOfPages < 1) return MvcHtmlString.Create(string.Empty);
    //        string GoToTheFirstPage = "GoToTheFirstPage";
    //        string GotoNextPage = "GoToTheNextPage";
    //        string GotoPrevPage = "GoToThePreviousPage";
    //        string of = "Of";

    //        string Pages = "Pages";
    //        int loopDifference = loopStart - 5;
    //        int loopStartSum = loopStart + 5;
    //        int loopEndSum = loopEnd + 5;

    //        StringBuilder html = new StringBuilder();
    //        html.Append("<div class=\"grid-ex-footer\">");
    //        html.Append("<div class=\"grid-pagination\">");
    //        if (NoOfPages > 0)
    //        {
    //            if (NoOfPages < loopEnd) loopEnd = NoOfPages;
    //            html.Append("<a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(1,1,5)\" title=\"" + GoToTheFirstPage + "\" class=\"grid-page-link grp-state-disabled\"><span class=\"gridpag-icon\" id=\"gridpag-firstpage\">first page</span></a>");
    //            if (loopStart > 5)
    //            {
    //                html.Append("<a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(" + loopDifference + "," + loopDifference + "," + loopDifference + ")\" title=\"" + GotoPrevPage + "\" class=\"grid-page-link grp-state-disabled\">");
    //                html.Append("<span class=\"gridpag-icon\" id=\"gridpag-prevpage\">previous page</span></a>");
    //            }
    //            html.Append("<ul class=\"gridpage-numbers\">");
    //            for (int i = loopStart; i <= loopEnd; i++)
    //            {
    //                if (i == currentPage)
    //                {
    //                    html.Append("<li><span class=\"grid-page-selected\">" + i + "</span></li>");
    //                }
    //                else
    //                {
    //                    html.Append("<li><a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(" + i + "," + loopStart + "," + loopEnd + ")\" class=\"grid-num-link\" data-page=\"" + i + "\">" + i + "</a></li>");
    //                }
    //            }
    //            html.Append("</ul>");
    //            if (loopStart + 5 < NoOfPages)
    //            {
    //                html.Append("<a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(" + loopStartSum + "," + loopStartSum + "," + loopEndSum + ")\" title=\"" + GotoNextPage + "\" class=\"grid-page-link\"><span class=\"gridpag-icon\" id=\"gridpag-nextpage\">next page</span></a>");
    //            }
    //            int value = NoOfPages - loopEnd + 1;
    //            html.Append("<a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(" + NoOfPages + "," + value + "," + NoOfPages + ")\" title=\"" + GotoNextPage + "\" class=\"grid-page-link\"><span class=\"gridpag-icon\" id=\"gridpag-lastpage\">last page</span></a>");
    //            html.Append("<span class=\"grid-page-info\">" + loopStart + " - " + loopEnd + " " + of + " " + NoOfPages + " " + Pages + ".</span>");
    //        }

    //        html.Append("</div></div>");
    //        if (NoOfPages > 0)
    //        {
    //            //html.Append("<div class=\"grid-scroller\">");
    //            //html.Append("<span class=\"grid-scroller-header\">" + GoToPage + "</span>");
    //            //html.Append("<div class=\"sliderctr\">");
    //            //html.Append("<span class=\"slidertooltip\"></span>");
    //            //html.Append("<div id=\"slider\"></div>");
    //            //html.Append("</div>");
    //            //html.Append("</div> </div>");
    //        }
    //        return MvcHtmlString.Create(html.ToString());
    //    }

    //    #endregion Paging
    //}

    #region Notification Helper

    public static class Notification
    {
        

        /// <summary>
        ///
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="functionName"></param>
        /// <param name="parameters"></param>
        /// <param name="processingDate"></param>
     
        

        //public static List<SMSMessagesModel> GetNotificationStatus(int modeulActionID, int UIID)
        //{
        //    //var returned = SMSServiceClient.GetSMSStatusByUIModuleID(new SMSMessageList { UniqueIdentificationID = UIID, ModuleActionID = modeulActionID }) as byte[];
        //    //var returnedList = returned.ObjectFromByteArray() as List<SMSMessagesModel>;
        //    var returnedList = new List<SMSMessagesModel>();
        //    return returnedList;
        //}
    }

    #endregion Notification Helper

    #region Validation at Server Side

    public static class ValidationAtServer
    {

        public static bool CheckMobileNumber(string MobileNo)
        {
            if (MobileNo.Trim().Length == 10)
            {
                string MatchPhoneNumberPattern = @"\d{10}";
                if (MobileNo != null) return Regex.IsMatch(MobileNo, MatchPhoneNumberPattern);
                else return false;
            }
            else
            {
                return false;
            }
        }

    }
    #endregion

    #region Pagination at Server Side

    public class Pagination
    {
        public int PageId { get; set; }
        public int PageSize { get; set; }
        public string SortCoulmnName { get; set; }
        public string SortOrder { get; set; }
        public string SortOrderingClass { get; set; }
        public string SearchWord { get; set; }
        public string TableId { get; set; }
        public int TotalRow { get; set; }
        public string FunctionType { get; set; }
    }
    #endregion
}