
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace CMNirdesh.Models
{
    public static class ExtensionMethods
    {
        public const int SALT_BYTE_SIZE = 24;

        public static string GetGeneratedSalt()
        {
            // Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);
            string getSalt = System.Text.Encoding.Default.GetString(salt);
            //   string getSalt = "oô4ŸB›hûùnQ-–Gêð6Ž„yH";
            return getSalt;
        }
    }

    public static class HtmlExtension
    {
        #region Paging
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="NoOfPages"></param>
        /// <param name="loopStart"></param>
        /// <param name="loopEnd"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static MvcHtmlString Pager(this HtmlHelper helper, int NoOfPages, int loopStart, int loopEnd, int currentPage)
        {
            if (NoOfPages < 1) return MvcHtmlString.Create(string.Empty);
            string GoToTheFirstPage = "GoToTheFirstPage";
            string GotoNextPage = "GoToTheNextPage";
            string GotoPrevPage = "GoToThePreviousPage";
            string of = "Global.Of";

            string Pages = "Pages";
            int loopDifference = loopStart - 5;
            int loopStartSum = loopStart + 5;
            int loopEndSum = loopEnd + 5;

            StringBuilder html = new StringBuilder();
            html.Append("<div class=\"grid-ex-footer\">");
            html.Append("<div class=\"grid-pagination\">");
            if (NoOfPages > 0)
            {
                if (NoOfPages < loopEnd) loopEnd = NoOfPages;
                html.Append("<a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(1,1,5)\" title=\"" + GoToTheFirstPage + "\" class=\"grid-page-link grp-state-disabled\"><span class=\"gridpag-icon\" id=\"gridpag-firstpage\">first page</span></a>");
                if (loopStart > 5)
                {
                    html.Append("<a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(" + loopDifference + "," + loopDifference + "," + loopDifference + ")\" title=\"" + GotoPrevPage + "\" class=\"grid-page-link grp-state-disabled\">");
                    html.Append("<span class=\"gridpag-icon\" id=\"gridpag-prevpage\">previous page</span></a>");
                }
                html.Append("<ul class=\"gridpage-numbers\">");
                for (int i = loopStart; i <= loopEnd; i++)
                {
                    if (i == currentPage)
                    {
                        html.Append("<li><span class=\"grid-page-selected\">" + i + "</span></li>");
                    }
                    else
                    {
                        html.Append("<li><a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(" + i + "," + loopStart + "," + loopEnd + ")\" class=\"grid-num-link\" data-page=\"" + i + "\">" + i + "</a></li>");
                    }
                }
                html.Append("</ul>");
                if (loopStart + 5 < NoOfPages)
                {
                    html.Append("<a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(" + loopStartSum + "," + loopStartSum + "," + loopEndSum + ")\" title=\"" + GotoNextPage + "\" class=\"grid-page-link\"><span class=\"gridpag-icon\" id=\"gridpag-nextpage\">next page</span></a>");
                }
                int value = NoOfPages - loopEnd + 1;
                html.Append("<a href=\"javascript:void(0);\" onclick=\"NextAndPreviousButton(" + NoOfPages + "," + value + "," + NoOfPages + ")\" title=\"" + GotoNextPage + "\" class=\"grid-page-link\"><span class=\"gridpag-icon\" id=\"gridpag-lastpage\">last page</span></a>");
                html.Append("<span class=\"grid-page-info\">" + loopStart + " - " + loopEnd + " " + of + " " + NoOfPages + " " + Pages + ".</span>");
            }

            html.Append("</div></div>");
            if (NoOfPages > 0)
            {
                //html.Append("<div class=\"grid-scroller\">");
                //html.Append("<span class=\"grid-scroller-header\">" + GoToPage + "</span>");
                //html.Append("<div class=\"sliderctr\">");
                //html.Append("<span class=\"slidertooltip\"></span>");
                //html.Append("<div id=\"slider\"></div>");
                //html.Append("</div>");
                //html.Append("</div> </div>");
            }
            return MvcHtmlString.Create(html.ToString());
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        public static string Truncate(this string str, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("Length must be >= 0");
            }

            if (str == null)
            {
                return null;
            }

            int maxLength = Math.Min(str.Length, length);
            return str.Substring(0, maxLength);
        }
        public static String LimitLength(this String text, int length)
        {
            return (text != null && text != string.Empty && (text.Length > length)) ? text.Substring(0, length) + ".." : text;

        }
    }

    public static class LinqExtensions
    {
        /// <summary>
        /// Orders the sequence by specific column and direction.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="direction">If set to "asc" then ascending otherwise descending.</param>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string sortColumn, string direction)
        {
            string methodName = string.Format("OrderBy{0}", direction.ToLower() == "asc" ? "" : "descending");
            ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");
            MemberExpression memberAccess = null;
            //
            foreach (var property in sortColumn.Split('.'))
            {
                memberAccess = MemberExpression.Property(memberAccess ?? (parameter as Expression), property);
            }
            //
            LambdaExpression orderByLambda = Expression.Lambda(memberAccess, parameter);
            MethodCallExpression result = Expression.Call(
                      typeof(Queryable),
                      methodName,
                      new[] { query.ElementType, memberAccess.Type },
                      query.Expression,
                      Expression.Quote(orderByLambda));
            //
            return query.Provider.CreateQuery<T>(result);
        }


    }

    #region PDF Related Extension Method
    public static class PDFExtensions
    {
        static int leftMargin = 10;
        static int rightMargin = 30;
        static int topMargin = 10;
        static int bottomMargin = 30;

        /// <summary>
        /// Will get PDF from provided location add image to every page of PDF and Save on the newFileLocation.
        /// </summary>
        /// <param name="sourceFileLocation">PDF File where we will be adding image on every page.</param>
        /// <param name="imageFileName">Image which needs to be added to PDF pages.</param>
        /// <param name="newFileLocation">Location where we need to save the updated PDF.</param>
        public static void InsertImageToPdf(string sourceFileLocation, string imageFileName, string newFileLocation)
        {
            CreateDirFromFilePath(newFileLocation);

            using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))
            using (Stream imageStream = new FileStream(imageFileName, FileMode.Open))
            using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfReader pdfReader = new PdfReader(pdfStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);

                var size = pdfReader.GetPageSize(1);
                var page = pdfReader.NumberOfPages + 1;
                pdfStamper.InsertPage(page, pdfReader.GetPageSize(1));
                image.Alignment = Element.ALIGN_BOTTOM;
                image.SetAbsolutePosition(size.Width - (image.Width + leftMargin), rightMargin);

                for (int i = 1; i <= page; i++)
                {
                    pdfStamper.GetOverContent(i).AddImage(image);
                }

                pdfStamper.Close();

                PDFDeleteMove(sourceFileLocation, newFileLocation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileLocation">PDF File where we will be adding image on every page.</param>
        /// <param name="imageFileName">Image which needs to be added to PDF pages.</param>
        /// <param name="newFileLocation">Location where we need to save the updated PDF.</param>
        /// <param name="signPosition">Where to add the image on the PDF page [Top Left, Top Right, Bottom Left and Bottom right]</param>
        /// <param name="pagesToSign">Pages on which we will be adding the image [All Pages, Alternate pages, First Page, Last page].</param>
        public static void InsertImageToPdf(string sourceFileLocation, string imageFileName, string newFileLocation, int signPosition, int pagesToSign)
        {
            CreateDirFromFilePath(newFileLocation);

            using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))
            using (Stream imageStream = new FileStream(imageFileName, FileMode.Open))
            using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfReader pdfReader = new PdfReader(pdfStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);

                var size = pdfReader.GetPageSize(1);
                var page = pdfReader.NumberOfPages + 1;
                pdfStamper.InsertPage(page, pdfReader.GetPageSize(1));
                image.Alignment = Element.ALIGN_BOTTOM;

                PdfSignatureAppearance sap = pdfStamper.SignatureAppearance;
                //sap.Image.Transparency.SetValue(30, 0);



                pdfStamper.Close();

                PDFDeleteMove(sourceFileLocation, newFileLocation);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileLocation">PDF File where we will be adding image on every page.</param>
        /// <param name="imageFileName">Image which needs to be added to PDF pages.</param>
        /// <param name="newFileLocation">Location where we need to save the updated PDF.</param>
        /// <param name="signPosition">Where to add the image on the PDF page [Top Left, Top Right, Bottom Left and Bottom right]</param>
        /// <param name="pagesToSign">Pages on which we will be adding the image [All Pages, Alternate pages, First Page, Last page].</param>

        public static void InsertImageToPdf(string sourceFileLocation, string imageFileName, string newFileLocation, int signPosition, int pagesToSign, string textLine1, string textLine2)
        {
            Int32 defaultImageWidth = 180;
            Int32 defaultImageHeight = 150;


            CreateDirFromFilePath(newFileLocation);

            using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))

            using (Stream imageStream = new FileStream(imageFileName, FileMode.Open))

            using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfReader pdfReader = new PdfReader(pdfStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);
                //image.Transparency.SetValue(50, 0);
                //image.ScaleToFit(150, 100);
                // image.ScaleAbsolute(150, 100);
                pagesToSign = 3;
                //image.Height = 150;
                var size = pdfReader.GetPageSize(1);
                var page = pdfReader.NumberOfPages;
                //pdfStamper.InsertPage(page, pdfReader.GetPageSize(1));
                image.Alignment = Element.ALIGN_MIDDLE;

                PdfSignatureAppearance sap = pdfStamper.SignatureAppearance;
                //sap.Image.Transparency.SetValue(30, 0);

                if (signPosition == (int)SignPositionEnum.TopLeft) //To add image to Top Left area
                {
                    image.SetAbsolutePosition(leftMargin, size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.TopRight) //To add image to Top right area
                {
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.BottomLeft) //To add image to bottom Left area
                {
                    image.SetAbsolutePosition(leftMargin, bottomMargin);
                }
                else //To add image to bottom right area
                {
                    //int bottomMargin = 480;
                    //int rightMargin = 30;
                    int bottomMargin = 400;
                    int rightMargin = 25;
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), bottomMargin);
                }


                if (pagesToSign == (int)PagesToSignEnum.AllPages) //For all pages.
                {
                    for (int i = 1; i <= page; i++)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);
                        if (i == page)
                        {
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }


                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.AlternatePages) //For Alternate pages.
                {
                    for (int i = 1; i <= page; i = i + 2)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);

                        if (i == page - 1)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }

                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.FirstPage) // For FIrst Page.
                {

                    for (int i = 1; i <= page; i++)
                    {
                        if (i == 1)
                        {
                            pdfStamper.GetOverContent(1).AddImage(image);
                        }
                        if (i == page)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }

                    }

                }
                else //For Last page.
                {
                    pdfStamper.GetOverContent(page).AddImage(image);
                    InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                }

                pdfStamper.Close();

                PDFDeleteMove(sourceFileLocation, newFileLocation);

            }
        }

        private static void InsertTextToPdf(PdfContentByte pdfContentByte, float x, float y, float textRotation, string firstLineText, string secondLineText)
        {
            BaseFont baseFontHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            pdfContentByte.SetColorFill(BaseColor.BLACK);
            pdfContentByte.SetFontAndSize(baseFontHeader, 12);
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, firstLineText, x, y - 5, textRotation);
            pdfContentByte.EndText();

            BaseFont baseFontSubHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            pdfContentByte.SetColorFill(BaseColor.BLACK);
            pdfContentByte.SetFontAndSize(baseFontSubHeader, 12);
            pdfContentByte.BeginText();
            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, secondLineText, x, y - 25, textRotation);
            pdfContentByte.EndText();
        }

        public static void InsertImageToPdfForNotice(string sourceFileLocation, string imageFileName, string newFileLocation, int signPosition, int pagesToSign, string textLine1, string textLine2)
        {
            Int32 defaultImageWidth = 180;
            Int32 defaultImageHeight = 150;


            CreateDirFromFilePath(newFileLocation);

            using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))

            using (Stream imageStream = new FileStream(imageFileName, FileMode.Open))

            using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfReader pdfReader = new PdfReader(pdfStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);

                pagesToSign = 3;
                //image.Height = 150;
                var size = pdfReader.GetPageSize(1);
                var page = pdfReader.NumberOfPages;
                //pdfStamper.InsertPage(page, pdfReader.GetPageSize(1));
                image.Alignment = Element.ALIGN_MIDDLE;

                PdfSignatureAppearance sap = pdfStamper.SignatureAppearance;
                //sap.Image.Transparency.SetValue(30, 0);

                if (signPosition == (int)SignPositionEnum.TopLeft) //To add image to Top Left area
                {
                    image.SetAbsolutePosition(leftMargin, size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.TopRight) //To add image to Top right area
                {
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.BottomLeft) //To add image to bottom Left area
                {
                    image.SetAbsolutePosition(leftMargin, bottomMargin);
                }
                else //To add image to bottom right area
                {
                    int bottomMargin = 450;
                    int rightMargin = 20;
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), bottomMargin);
                }


                if (pagesToSign == (int)PagesToSignEnum.AllPages) //For all pages.
                {
                    for (int i = 1; i <= page; i++)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);
                        if (i == page)
                        {
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }


                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.AlternatePages) //For Alternate pages.
                {
                    for (int i = 1; i <= page; i = i + 2)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);

                        if (i == page - 1)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }

                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.FirstPage) // For FIrst Page.
                {

                    for (int i = 1; i <= page; i++)
                    {
                        if (i == 1)
                        {
                            pdfStamper.GetOverContent(1).AddImage(image);
                        }
                        if (i == page)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }

                    }

                }
                else //For Last page.
                {
                    pdfStamper.GetOverContent(page).AddImage(image);
                    InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                }

                pdfStamper.Close();

                PDFDeleteMove(sourceFileLocation, newFileLocation);

            }
        }
        public static void InsertImageToPdfForNoticeRule61(string sourceFileLocation, string imageFileName, string newFileLocation, int signPosition, int pagesToSign, string textLine1, string textLine2)
        {
            Int32 defaultImageWidth = 180;
            Int32 defaultImageHeight = 150;


            CreateDirFromFilePath(newFileLocation);

            using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))

            using (Stream imageStream = new FileStream(imageFileName, FileMode.Open))

            using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfReader pdfReader = new PdfReader(pdfStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);

                pagesToSign = 3;
                //image.Height = 150;
                var size = pdfReader.GetPageSize(1);
                var page = pdfReader.NumberOfPages;
                //pdfStamper.InsertPage(page, pdfReader.GetPageSize(1));
                image.Alignment = Element.ALIGN_MIDDLE;

                PdfSignatureAppearance sap = pdfStamper.SignatureAppearance;
                //sap.Image.Transparency.SetValue(30, 0);

                if (signPosition == (int)SignPositionEnum.TopLeft) //To add image to Top Left area
                {
                    image.SetAbsolutePosition(leftMargin, size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.TopRight) //To add image to Top right area
                {
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.BottomLeft) //To add image to bottom Left area
                {
                    image.SetAbsolutePosition(leftMargin, bottomMargin);
                }
                else //To add image to bottom right area
                {
                    int bottomMargin = 350;
                    int rightMargin = 20;
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), bottomMargin);
                }


                if (pagesToSign == (int)PagesToSignEnum.AllPages) //For all pages.
                {
                    for (int i = 1; i <= page; i++)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);
                        if (i == page)
                        {
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }


                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.AlternatePages) //For Alternate pages.
                {
                    for (int i = 1; i <= page; i = i + 2)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);

                        if (i == page - 1)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }

                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.FirstPage) // For FIrst Page.
                {

                    for (int i = 1; i <= page; i++)
                    {
                        if (i == 1)
                        {
                            pdfStamper.GetOverContent(1).AddImage(image);
                        }
                        if (i == page)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }

                    }

                }
                else //For Last page.
                {
                    pdfStamper.GetOverContent(page).AddImage(image);
                    InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                }

                pdfStamper.Close();

                PDFDeleteMove(sourceFileLocation, newFileLocation);

            }
        }
        public static void InsertImageToPdfForNoticeRuleDisaprroval(string sourceFileLocation, string imageFileName, string newFileLocation, int signPosition, int pagesToSign, string textLine1, string textLine2)
        {
            Int32 defaultImageWidth = 180;
            Int32 defaultImageHeight = 150;


            CreateDirFromFilePath(newFileLocation);

            using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))

            using (Stream imageStream = new FileStream(imageFileName, FileMode.Open))

            using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfReader pdfReader = new PdfReader(pdfStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);
                //image.Transparency.SetValue(50, 0);
                //image.ScaleToFit(150, 100);
                // image.ScaleAbsolute(150, 100);
                pagesToSign = 3;
                //image.Height = 150;
                var size = pdfReader.GetPageSize(1);
                var page = pdfReader.NumberOfPages;
                //pdfStamper.InsertPage(page, pdfReader.GetPageSize(1));
                image.Alignment = Element.ALIGN_MIDDLE;

                PdfSignatureAppearance sap = pdfStamper.SignatureAppearance;
                //sap.Image.Transparency.SetValue(30, 0);

                if (signPosition == (int)SignPositionEnum.TopLeft) //To add image to Top Left area
                {
                    image.SetAbsolutePosition(leftMargin, size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.TopRight) //To add image to Top right area
                {
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.BottomLeft) //To add image to bottom Left area
                {
                    image.SetAbsolutePosition(leftMargin, bottomMargin);
                }
                else //To add image to bottom right area
                {
                    int bottomMargin = 240;
                    int rightMargin = 20;
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), bottomMargin);
                }


                if (pagesToSign == (int)PagesToSignEnum.AllPages) //For all pages.
                {
                    for (int i = 1; i <= page; i++)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);
                        if (i == page)
                        {
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }


                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.AlternatePages) //For Alternate pages.
                {
                    for (int i = 1; i <= page; i = i + 2)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);

                        if (i == page - 1)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }

                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.FirstPage) // For FIrst Page.
                {

                    for (int i = 1; i <= page; i++)
                    {
                        if (i == 1)
                        {
                            pdfStamper.GetOverContent(1).AddImage(image);
                        }
                        if (i == page)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }
                        //pdfStamper.GetOverContent(1).AddImage(image);
                        //if (i == page)
                        //{
                        //    pdfStamper.GetOverContent(page).AddImage(image);
                        //    InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        //}


                    }

                }
                else //For Last page.
                {
                    pdfStamper.GetOverContent(page).AddImage(image);
                    InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                }

                pdfStamper.Close();

                PDFDeleteMove(sourceFileLocation, newFileLocation);

            }
        }
        public static void InsertImageToPdfForNoticeRule71(string sourceFileLocation, string imageFileName, string newFileLocation, int signPosition, int pagesToSign, string textLine1, string textLine2)
        {
            Int32 defaultImageWidth = 180;
            Int32 defaultImageHeight = 150;


            CreateDirFromFilePath(newFileLocation);

            using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))

            using (Stream imageStream = new FileStream(imageFileName, FileMode.Open))

            using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfReader pdfReader = new PdfReader(pdfStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);
                //image.Transparency.SetValue(50, 0);
                //image.ScaleToFit(150, 100);
                // image.ScaleAbsolute(150, 100);
                pagesToSign = 3;
                //image.Height = 150;
                var size = pdfReader.GetPageSize(1);
                var page = pdfReader.NumberOfPages;
                //pdfStamper.InsertPage(page, pdfReader.GetPageSize(1));
                image.Alignment = Element.ALIGN_MIDDLE;

                PdfSignatureAppearance sap = pdfStamper.SignatureAppearance;
                //sap.Image.Transparency.SetValue(30, 0);

                if (signPosition == (int)SignPositionEnum.TopLeft) //To add image to Top Left area
                {
                    image.SetAbsolutePosition(leftMargin, size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.TopRight) //To add image to Top right area
                {
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.BottomLeft) //To add image to bottom Left area
                {
                    image.SetAbsolutePosition(leftMargin, bottomMargin);
                }
                else //To add image to bottom right area
                {
                    int bottomMargin = 400;
                    int rightMargin = 20;
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), bottomMargin);
                }


                if (pagesToSign == (int)PagesToSignEnum.AllPages) //For all pages.
                {
                    for (int i = 1; i <= page; i++)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);
                        if (i == page)
                        {
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }


                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.AlternatePages) //For Alternate pages.
                {
                    for (int i = 1; i <= page; i = i + 2)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);

                        if (i == page - 1)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }

                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.FirstPage) // For FIrst Page.
                {

                    for (int i = 1; i <= page; i++)
                    {
                        if (i == 1)
                        {
                            pdfStamper.GetOverContent(1).AddImage(image);
                        }
                        if (i == page)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }
                        //pdfStamper.GetOverContent(1).AddImage(image);
                        //if (i == page)
                        //{
                        //    pdfStamper.GetOverContent(page).AddImage(image);
                        //    InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        //}


                    }

                }
                else //For Last page.
                {
                    pdfStamper.GetOverContent(page).AddImage(image);
                    InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                }

                pdfStamper.Close();

                PDFDeleteMove(sourceFileLocation, newFileLocation);

            }
        }
        public static void InsertImageToPdfForNotice44(string sourceFileLocation, string imageFileName, string newFileLocation, int signPosition, int pagesToSign, string textLine1, string textLine2)
        {
            Int32 defaultImageWidth = 180;
            Int32 defaultImageHeight = 150;


            CreateDirFromFilePath(newFileLocation);

            using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))

            using (Stream imageStream = new FileStream(imageFileName, FileMode.Open))

            using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
            {
                PdfReader pdfReader = new PdfReader(pdfStream);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);
                //image.Transparency.SetValue(50, 0);
                //image.ScaleToFit(150, 100);
                // image.ScaleAbsolute(150, 100);
                pagesToSign = 3;
                //image.Height = 150;
                var size = pdfReader.GetPageSize(1);
                var page = pdfReader.NumberOfPages;
                //pdfStamper.InsertPage(page, pdfReader.GetPageSize(1));
                image.Alignment = Element.ALIGN_MIDDLE;

                PdfSignatureAppearance sap = pdfStamper.SignatureAppearance;
                //sap.Image.Transparency.SetValue(30, 0);

                if (signPosition == (int)SignPositionEnum.TopLeft) //To add image to Top Left area
                {
                    image.SetAbsolutePosition(leftMargin, size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.TopRight) //To add image to Top right area
                {
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), size.Height - (defaultImageHeight + topMargin));
                }
                else if (signPosition == (int)SignPositionEnum.BottomLeft) //To add image to bottom Left area
                {
                    image.SetAbsolutePosition(leftMargin, bottomMargin);
                }
                else //To add image to bottom right area
                {
                    int bottomMargin = 355;
                    int rightMargin = 80;
                    image.SetAbsolutePosition(size.Width - (defaultImageWidth + rightMargin), bottomMargin);
                }


                if (pagesToSign == (int)PagesToSignEnum.AllPages) //For all pages.
                {
                    for (int i = 1; i <= page; i++)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);
                        if (i == page)
                        {
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }


                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.AlternatePages) //For Alternate pages.
                {
                    for (int i = 1; i <= page; i = i + 2)
                    {
                        pdfStamper.GetOverContent(i).AddImage(image);

                        if (i == page - 1)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }

                    }
                }
                else if (pagesToSign == (int)PagesToSignEnum.FirstPage) // For FIrst Page.
                {

                    for (int i = 1; i <= page; i++)
                    {
                        if (i == 1)
                        {
                            pdfStamper.GetOverContent(1).AddImage(image);
                        }
                        if (i == page)
                        {
                            pdfStamper.GetOverContent(page).AddImage(image);
                            InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        }
                        //pdfStamper.GetOverContent(1).AddImage(image);
                        //if (i == page)
                        //{
                        //    pdfStamper.GetOverContent(page).AddImage(image);
                        //    InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                        //}


                    }

                }
                else //For Last page.
                {
                    pdfStamper.GetOverContent(page).AddImage(image);
                    InsertTextToPdf(pdfStamper.GetOverContent(page), image.AbsoluteX, image.AbsoluteY, 0, textLine1, textLine2);
                }

                pdfStamper.Close();

                PDFDeleteMove(sourceFileLocation, newFileLocation);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdfContentByte"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="textRotation"></param>
        /// <param name="firstLineText"></param>
        /// <param name="secondLineText"></param>
        //private static void InsertTextToPdf(PdfContentByte pdfContentByte, float x, float y, float textRotation, string firstLineText, string secondLineText)
        //{
        //    BaseFont baseFontHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
        //    pdfContentByte.SetColorFill(BaseColor.Black);
        //    pdfContentByte.SetFontAndSize(baseFontHeader, 12);
        //    pdfContentByte.BeginText();
        //    pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, firstLineText, x, y - 5, textRotation);
        //    pdfContentByte.EndText();

        //    BaseFont baseFontSubHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
        //    pdfContentByte.SetColorFill(BaseColor.Black);
        //    pdfContentByte.SetFontAndSize(baseFontSubHeader, 12);
        //    pdfContentByte.BeginText();
        //    pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, secondLineText, x, y - 25, textRotation);
        //    pdfContentByte.EndText();
        //}
        //private static void InsertTextToPdfcomeetiee(PdfContentByte pdfContentByte, float x, float y, float textRotation, string firstLineText, string secondLineText)
        //{
        //    BaseFont baseFontHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
        //    pdfContentByte.SetColorFill(BaseColor.Black);
        //    pdfContentByte.SetFontAndSize(baseFontHeader, 12);
        //    pdfContentByte.BeginText();
        //    pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, firstLineText, x, y - 5, textRotation);
        //    pdfContentByte.EndText();

        //    BaseFont baseFontSubHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
        //    pdfContentByte.SetColorFill(BaseColor.Black);
        //    pdfContentByte.SetFontAndSize(baseFontSubHeader, 12);
        //    pdfContentByte.BeginText();
        //    pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, secondLineText, x, y - 25, textRotation);
        //    pdfContentByte.EndText();


        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileLocation">PDF File where we will be adding image on every page.</param>
        /// <param name="imageFileAsBytes">Image as byte array which needs to be added to PDF pages.</param>
        /// <param name="newFileLocation">Location where we need to save the updated PDF.</param>
        /// <param name="signPosition">Where to add the image on the PDF page [Top Left, Top Right, Bottom Left and Bottom right]</param>
        /// <param name="pagesToSign"></param>
        //public static void InsertImageToPdf(string sourceFileLocation, Byte[] imageFileAsBytes, string newFileLocation, int signPosition, int pagesToSign)
        //{
        //    CreateDirFromFilePath(newFileLocation);

        //    using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))
        //    using (Stream imageStream = new MemoryStream(imageFileAsBytes))
        //    using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
        //    {
        //        PdfReader pdfReader = new PdfReader(pdfStream);
        //        PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
        //        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageStream);

        //        var size = pdfReader.GetPageSize(1);
        //        var page = pdfReader.NumberOfPages + 1;
        //        pdfStamper.InsertPage(page, pdfReader.GetPageSize(1));
        //        image.Alignment = Element.ALIGN_BOTTOM;

        //        if (signPosition == (int)SignPositionEnum.TopLeft) //To add image to Top Left area
        //        {
        //            image.SetAbsolutePosition(leftMargin, size.Height - (image.Height + topMargin));
        //        }
        //        else if (signPosition == (int)SignPositionEnum.TopRight) //To add image to Top right area
        //        {
        //            image.SetAbsolutePosition(size.Width - (image.Width + rightMargin), size.Height - (image.Height + topMargin));
        //        }
        //        else if (signPosition == (int)SignPositionEnum.BottomLeft) //To add image to bottom Left area
        //        {
        //            image.SetAbsolutePosition(leftMargin, bottomMargin);
        //        }
        //        else //To add image to bottom right area
        //        {
        //            image.SetAbsolutePosition(size.Width - (image.Width + rightMargin), bottomMargin);
        //        }

        //        if (pagesToSign == (int)PagesToSignEnum.AllPages) //For all pages.
        //        {
        //            for (int i = 1; i <= page; i++)
        //            {
        //                pdfStamper.GetOverContent(i).AddImage(image);
        //            }
        //        }
        //        else if (pagesToSign == (int)PagesToSignEnum.AlternatePages) //For Alternate pages.
        //        {
        //            for (int i = 1; i <= page; i = i + 2)
        //            {
        //                pdfStamper.GetOverContent(i).AddImage(image);
        //            }
        //        }
        //        else if (pagesToSign == (int)PagesToSignEnum.FirstPage) // For FIrst Page.
        //        {
        //            pdfStamper.GetOverContent(1).AddImage(image);
        //        }
        //        else //For Last page.
        //        {
        //            pdfStamper.GetOverContent(page).AddImage(image);
        //        }

        //        pdfStamper.Close();

        //        PDFDeleteMove(sourceFileLocation, newFileLocation);

        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileLocation">File on which we will be adding the Watermark.</param>
        /// <param name="watermarkText">Text needs to be inserted to the PDF pages as Water-mark.</param>
        /// <param name="newFileLocation">location where PDF file with the Watermark.</param>
        //public static void InsertWaterMarkToPdf(string sourceFileLocation, string watermarkText, string newFileLocation)
        //{
        //    CreateDirFromFilePath(newFileLocation);

        //    using (Stream pdfStream = new FileStream(sourceFileLocation, FileMode.Open))
        //    using (Stream newpdfStream = new FileStream(newFileLocation, FileMode.Create, FileAccess.ReadWrite))
        //    {
        //        PdfReader pdfReader = new PdfReader(pdfStream);
        //        PdfStamper pdfStamper = new PdfStamper(pdfReader, newpdfStream);
        //        int pageCount1 = pdfReader.NumberOfPages;
        //        PdfLayer layer = new PdfLayer("WatermarkLayer", pdfStamper.Writer);
        //        for (int i = 1; i <= pageCount1; i++)
        //        {
        //            iTextSharp.text.Rectangle rect = pdfReader.GetPageSize(i);
        //            PdfContentByte cb = pdfStamper.GetUnderContent(i);
        //            cb.BeginLayer(layer);
        //            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 50);
        //            PdfGState gState = new PdfGState();
        //            gState.FillOpacity = 0.25f;
        //            cb.SetGState(gState);
        //            cb.SetColorFill(BaseColor.Black);
        //            cb.BeginText();
        //            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, rect.Width / 2, rect.Height / 2, 45f);
        //            cb.EndText();
        //            cb.EndLayer();
        //        }
        //        pdfStamper.Close();

        //        PDFDeleteMove(sourceFileLocation, newFileLocation);

        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFileLocation"></param>
        /// <param name="newFileLocation"></param>
        public static void PDFDeleteMove(string sourceFileLocation, string newFileLocation)
        {
            File.Delete(sourceFileLocation);
            File.Copy(newFileLocation, sourceFileLocation);
            //File.Move(newFileLocation, sourceFileLocation);
            //File.Delete(newFileLocation);
        }

        //public static void PDFMove(string sourceFileLocation, string newFileLocation)
        //{
        //   // File.Delete(sourceFileLocation);
        // //   File.Copy(newFileLocation, sourceFileLocation);
        //    File.Move(newFileLocation, sourceFileLocation);
        //    //File.Delete(newFileLocation);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newFileLocation"></param>
        public static void CreateDirFromFilePath(string newFileLocation)
        {
            var fileName = Path.GetFileName(newFileLocation);
            if (!string.IsNullOrEmpty(fileName))
            {
                string dirName = Path.GetDirectoryName(newFileLocation);
                DirectoryInfo Dir = new DirectoryInfo(dirName);
                if (!Dir.Exists)
                {
                    Dir.Create();
                }
            }
        }
    }
    #endregion

    #region Image Encryption/Decription
    public static class EncryptionDecryptionExtensions
    {
        /// <summary>
        /// Encrypt the Bytes with AES-256 cryptography teqnique.
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="Key">AES symmetric key which will be used for encryption.</param>
        /// <param name="IV">AES initialization vector (IV) for the symmetric algorithm.</param>
        /// <returns>Encrypted byte array of the Provided bytes.</returns>
        public static byte[] EncryptBytesToBytes_AES(byte[] byteArray, byte[] Key, byte[] IV)
        {
            if (byteArray == null || byteArray.Length <= 0)
                throw new ArgumentNullException("byteArray");

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("KeyIV");

            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;

            AesCryptoServiceProvider AesManagedAlg = null;

#pragma warning disable CS0219 // The variable 'encrypted' is assigned but its value is never used
            byte[] encrypted = null;
#pragma warning restore CS0219 // The variable 'encrypted' is assigned but its value is never used

            try
            {
                // Create an AesCryptoServiceProvider object with the specified key and IV.
                AesManagedAlg = new AesCryptoServiceProvider();
                AesManagedAlg.Key = Key;
                AesManagedAlg.IV = IV;

                ICryptoTransform encryptor = AesManagedAlg.CreateEncryptor(AesManagedAlg.Key, AesManagedAlg.IV);

                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);
                csEncrypt.Write(byteArray, 0, byteArray.Length);
                csEncrypt.FlushFinalBlock();
            }

            finally
            {
                if (swEncrypt != null)
                    swEncrypt.Close();
                if (csEncrypt != null)
                    csEncrypt.Close();
                if (msEncrypt != null)
                    msEncrypt.Close();

                if (AesManagedAlg != null)
                    AesManagedAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return msEncrypt.ToArray();
        }

        /// <summary>
        /// Decrypt the already encrypted bytes using AES(Advance Encryption )-256.
        /// </summary>
        /// <param name="cipherArray">Encrypted Byte Array WHich needs Decryption.</param>
        /// <param name="Key">AES symmetric key that is used for encryption.</param>
        /// <param name="IV">AES initialization vector (IV) for the symmetric algorithm.</param>
        /// <returns>Decrypted Bytes for the provided Encrypted AES ciper Bytes.</returns>
        public static byte[] DecryptBytesFromBytes_AES(byte[] cipherArray, byte[] Key, byte[] IV)
        {
            if (cipherArray == null || cipherArray.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;

            AesCryptoServiceProvider AesManagedAlg = null;
            byte[] resultBytes;

            try
            {
                // Create an AesCryptoServiceProvider object with the specified key and IV.
                AesManagedAlg = new AesCryptoServiceProvider();
                AesManagedAlg.Key = Key;
                AesManagedAlg.IV = IV;

                ICryptoTransform decryptor = AesManagedAlg.CreateDecryptor(AesManagedAlg.Key, AesManagedAlg.IV);

                msDecrypt = new MemoryStream(cipherArray);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);

                // Read the decrypted bytes from the decrypting stream
                var scratchBytes = new byte[cipherArray.Length];
                int decryptedByteCount = csDecrypt.Read(scratchBytes, 0, scratchBytes.Length);
                resultBytes = new byte[decryptedByteCount];

                for (int i = 0; i < decryptedByteCount; i++)
                {
                    resultBytes[i] = scratchBytes[i];
                }
            }

            finally
            {
                if (srDecrypt != null)
                    srDecrypt.Close();
                if (csDecrypt != null)
                    csDecrypt.Close();
                if (msDecrypt != null)
                    msDecrypt.Close();

                //Clear the AesCryptoServiceProvider object.
                if (AesManagedAlg != null)
                    AesManagedAlg.Clear();
            }
            return resultBytes;
        }
    }
    #endregion

    #region ImageToByte and Vice versa Extensions
    public static class ImageExtensions
    {
        public static byte[] ImageToByteArray(string imagefilePath)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imagefilePath);
            byte[] imageByte = ImageToByteArraybyMemoryStream(image);
            return imageByte;
        }

        public static System.Drawing.Image ByteArrayToImage(byte[] imageByte)
        {
            MemoryStream ms = new MemoryStream(imageByte);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }

        private static byte[] ImageToByteArraybyMemoryStream(System.Drawing.Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public static string BytesToString(byte[] byteArray)
        {
            return Convert.ToBase64String(byteArray);
        }

        public static byte[] stringToBytes(string stringArray)
        {
            return Convert.FromBase64String(stringArray);
        }

    }
    #endregion

    #region QRCode Encode/Decode functionality Helper methods.
    public static class QRCodeExtensions
    {

    }
    #endregion
    #region imagesize
    public class ImageResizerExtensions
    {

        public int MaxX { get; set; }

        public int MaxY { get; set; }

        public bool TrimImage { get; set; }


        public ImageFormat SaveFormat { get; set; }

        public ImageResizerExtensions(int size)
        {
            MaxX = MaxY = size;
            //TrimImage = false;
            SaveFormat = ImageFormat.Jpeg;
        }


        public bool Resize(string source, string target)
        {
            using (System.Drawing.Image src = System.Drawing.Image.FromFile(source, true))
            {
                // Check that we have an image
                if (src != null)
                {
                    int origX, origY, newX, newY;
                    int trimX = 0, trimY = 0;

                    // Default to size of source image
                    newX = origX = src.Width;
                    newY = origY = src.Height;

                    // Does image exceed maximum dimensions?
                    if (origX > MaxX || origY > MaxY)
                    {
                        // Need to resize image

                        // Resize (no trim) to keep within maximum dimensions
                        double factor = Math.Min((double)MaxX / (double)origX,
                            (double)MaxY / (double)origY);
                        newX = (int)Math.Ceiling((double)origX * factor);
                        newY = (int)Math.Ceiling((double)origY * factor);

                    }

                    // Create destination image
                    using (System.Drawing.Image dest = new Bitmap(newX - trimX, newY - trimY))
                    {
                        Graphics graph = Graphics.FromImage(dest);
                        graph.InterpolationMode =
                            System.Drawing.Drawing2D.InterpolationMode.High;
                        graph.DrawImage(src, -(trimX / 2), -(trimY / 2), newX, newY);

                        dest.Save(target, SaveFormat);
                        // Indicate success

                        return true;
                    }
                }
            }
            // Indicate failure
            return false;
        }
    }
    #endregion

    //error log added by Durgesh 
    public static class ErrorLog
    {
        public static void WriteToLog(Exception ex, string CustomMessage)
        {

            string filePath = Path.Combine("C:\\inetpub\\e_Vidhan\\FileStructure\\", "ExceptionLog");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);

            }
            filePath = Path.Combine(filePath, "ErrorLog.txt");
            FileStream fs = new FileStream(filePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);

            if (ex != null)
            {

                sw.Write("Custom Message : " + CustomMessage + Environment.NewLine + " Message :" + ex.Message + Environment.NewLine + "StackTrace :" + ex.StackTrace +
           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
            }
            else
            {
                sw.Write("Custom Message : " + CustomMessage + Environment.NewLine + "Date :" + DateTime.Now.ToString());
            }
            string New = Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine;
            sw.Write(New);

            sw.Flush();
            sw.Close();
            fs.Close();
            File.ReadAllLines(filePath);

        }


        public static void WriteToLog(string CustomMessage)
        {

            string filePath = Path.Combine("C:\\inetpub\\e_Vidhan\\FileStructure\\", "ExceptionLog");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);

            }
            filePath = Path.Combine(filePath, "ErrorLog.txt");
            FileStream fs = new FileStream(filePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);


            sw.Write("Custom Message : " + CustomMessage + Environment.NewLine + "Date :" + DateTime.Now.ToString());

            string New = Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine;
            sw.Write(New);

            sw.Flush();
            sw.Close();
            fs.Close();
            File.ReadAllLines(filePath);

        }
    }

    public static class EncryptionUtility
    {


        public static string Encrypt(string text, string pwd = "qwerty12345")
        {
            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes = null;
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            // Generating salt bytes
            byte[] saltBytes = GetRandomBytes();

            // Appending salt bytes to original bytes
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(encryptedBytes).Replace('/', '`').Replace('%', '^');
        }
        public static byte[] GetRandomBytes()
        {
            int _saltSize = 4;
            byte[] ba = new byte[_saltSize];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }
        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        public static string Decrypt(string decryptedText, string pwd = "qwerty12345")
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText.Replace('`', '/').Replace('%', '^'));
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            // Getting the size of salt
            int _saltSize = 4;

            // Removing salt bytes, retrieving original bytes
            byte[] originalBytes = new byte[decryptedBytes.Length - _saltSize];
            for (int i = _saltSize; i < decryptedBytes.Length; i++)
            {
                originalBytes[i - _saltSize] = decryptedBytes[i];
            }

            return Encoding.UTF8.GetString(originalBytes);
        }

    }

}