
using CMNirdesh.Models;
using System;
using System.Web.Mvc;
using CMNirdesh.Filters;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web;
using iTextSharp.tool.xml;
using System.Diagnostics;
using System.Security.Cryptography;
using Syncfusion.Pdf.Graphics;
using NReco.PdfGenerator;
using System.Data;


namespace CMNirdesh.Controllers
{
    [Audit]
    [NoCache]
    [CMNirdeshAuthorize(Allow = "Authenticated")]
    public class PdfController : Controller
    {
        public ActionResult GeneratePdf()
        {
            exportpdf();
            return View("PdfView", model: null);
        }
        public ActionResult GeneratePdf1()
        {
            int agendaid = 3;
            string htmlString = GetNewapprovepdfhtml2(agendaid);
            var htmlToPdf = new HtmlToPdfConverter();
            string htmlUrl = "https://mcmohali.punjab.gov.in/SecureFileStructure/Proceedings/1_Proceeding.html";

            var pdfContentType = "application/pdf";
            if (!String.IsNullOrEmpty(htmlUrl))
            {
                return File(htmlToPdf.GeneratePdfFromFile(htmlUrl, null), pdfContentType);
            }
            else
            {
                return File(htmlToPdf.GeneratePdf(htmlString, null), pdfContentType);
            }
        }
        private void exportpdf()
        {

            using (MemoryStream ms = new MemoryStream())
            {
                string raaviFont = Server.MapPath("~/Fonts/raavi.ttf");// @"C:\Windows\Fonts\Raavi.ttf";
                Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate(), 5, 5, 10, 10);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
               // FontSet set = new FontSet();
               // set.AddFont(raaviFont);

                BaseFont customFont = BaseFont.CreateFont(raaviFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                // Create a font object
                Font font = new Font(customFont, 12);

                writer.CloseStream = false;
                doc.Open();

                int serial_cnt = 1;
                for (int i = 0; i < 10; i++)
                {
                    //Creating paragraph for header
                    String strHeader = String.Empty;
                    BaseFont bfntHead = BaseFont.CreateFont(raaviFont, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font fntHead = new iTextSharp.text.Font(bfntHead, 13, 1, iTextSharp.text.BaseColor.BLACK);
                    Font TableContentFont = new Font(customFont, 12); //FontFactory.GetFont("Arial", 10);
                    Paragraph prgHeading = new Paragraph();
                    prgHeading.Alignment = Element.ALIGN_LEFT;
                    strHeader = "ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ.ਨਗਰ ਮੁਹਾਲੀ";
                    prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
                    doc.Add(prgHeading);

                    strHeader = "ਉਪਰੋਕਤ ਵਸ਼ੇਸਬੰਧੀ ਬੇਨਤੀ ਹੈਿਕ ਨਗਰ ਿਨਗਮ, ਐਸ.ਏ.ਐਸ.ਨਗਰ ਮੁਹਾਲੀ ਜੀ ਦੀ ਪਧਾਨਗੀ ਹੇਠ ਮੇਅਰ ਦੇਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਿਟੰਗ ਰਾਂਹੀ ਿਮਤੀ: 10/07/2023 ਸਮਾਂ ਬਾਅਦ ਦੁਪਿਹਰ 12:00 PM ਵਜੇਦੇਮਤਾ ਨੰ: 225 ਤ$253 ਤੱਕ ਦੀ ਕਾਰਵਾਈ ਨੱਥੀ ਕਰਕੇਆਪ ਜੀ ਪਾਸ ਅਗਲੇਰੀ ਕਾਰਵਾਈ ਿਹੱਤ ਭੇਜੀ ਜਾਂਦੀ ਹੈਜੀ";
                    prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
                    doc.Add(prgHeading);

                    //Adding a line  
                    Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, iTextSharp.text.BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    doc.Add(p);

                    //Adding line break  
                    doc.Add(new Chunk("\n", fntHead));

                    //Adding  PdfPTable  
                    PdfPTable pdftable = new PdfPTable(10);
                    float[] widths = new float[] { 0.3f, 0.4f, 0.4f, 1.5f, 0.5f, 0.4f, 1.2f, 1.5f, 1.5f, 1.5f };
                    pdftable.TotalWidth = 810;
                    pdftable.SetWidths(widths);
                    pdftable.LockedWidth = true;
                    pdftable.WidthPercentage = 100;
                    pdftable.SpacingBefore = 2f;
                    pdftable.SpacingAfter = 2f;

                    String cellText = "#";
                    PdfPCell cell = new PdfPCell();
                    cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10));
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C8C8C8"));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingBottom = 2;
                    pdftable.AddCell(cell);

                    cellText = "Description";
                    cell = new PdfPCell();
                    cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10));
                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C8C8C8"));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingBottom = 2;
                    pdftable.AddCell(cell);

                    //writing table Data
                    String pnr = String.Empty;
                    String destination = String.Empty;
                    int total_passenger_cnt = 10;

                    destination = "ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ.ਨਗਰ ਮੁਹਾਲੀ";
                    for (int subcnt = i; subcnt < 10; subcnt++)
                    {
                        cell = new PdfPCell();
                        cell.Phrase = new Phrase(serial_cnt.ToString(), TableContentFont);
                        cell.PaddingTop = 2;
                        cell.PaddingBottom = 2;
                        pdftable.AddCell(cell);

                        pdftable.AddCell(new Phrase("1", TableContentFont));
                        pdftable.AddCell(new Phrase("ਨਗਰ ਨਿਗਮ, ਐਸ.ਏ.ਐਸ.ਨਗਰ ਮੁਹਾਲੀ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ: 10/07/2023 ਦੇ ਮਤਾ ਨੰਬਰ 225 ਤੋਂ 253 ਤੱਕ ਦੀ ਕਾਰਵਾਈ ਭੇਜਣ ਬਾਰੇ।", TableContentFont));
                        total_passenger_cnt++;
                        serial_cnt++;
                        i = subcnt;
                    }
                    doc.Add(pdftable);
                    doc.NewPage();
                }
                doc.Close();
                writer.Close();
                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition", String.Format("attachment;filename=agenda.pdf", HttpUtility.HtmlEncode(DateTime.Now.ToShortDateString())));
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            }
        }
        public ActionResult GeneratePdf2()
        {
            try
            {
                int agendaid = 3;
                string htmlString = GetNewapprovepdfhtml2(agendaid);

                string BlobfolderPath = "Proceedings/";
                string htmlfilename = DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + agendaid.ToString() + "_Proceeding.html";
                string pdffilename = DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + agendaid.ToString() + "_Proceeding.pdf";


                string finalpdfpath = "Agenda/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + agendaid.ToString() + "_Agenda.html";
                string folderpath = "~/SecureFileStructure/" + finalpdfpath;
                string htmlpath = Server.MapPath(folderpath);

                string pdfpath = "~/SecureFileStructure/" + "Agenda/" + DateTime.Now.ToString("ddmmyyyyhhmmss") + "_" + agendaid.ToString() + "_Agenda.pdf";
                pdfpath = Server.MapPath(pdfpath);

                string batchFilePath = Server.MapPath("~/pdf.bat");
                string confFilePath = Server.MapPath("~/wkhtmltopdf.conf");

                var root = "~/SecureFileStructure/Agenda";
                bool path = System.IO.Directory.Exists(HttpContext.Server.MapPath(root));
                if (!path)
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath(root));
                }


                System.IO.File.WriteAllText(htmlpath, htmlString);
                Response.ContentType = "text/html";

                byte[] htmlBytes = Encoding.UTF8.GetBytes(htmlString);
                PDFModel.ConvertPdfUsingBatchFile(htmlpath, pdfpath, batchFilePath);
               // PDFModel.ConvertChromePdfRenderer(htmlpath, pdfpath, confFilePath);

                byte[] fileBytes = System.IO.File.ReadAllBytes(pdfpath);

                //code to save file in blob storage
                MemoryStream memoryStream = new MemoryStream(fileBytes);
                HttpPostedFileBase postedFile = new PDFModel.MockHttpPostedFileBase(memoryStream, Path.GetFileName(pdfpath));
                BlobStorage.Savefile(postedFile, BlobfolderPath, pdffilename);

                //string pdfHash = GetPdfHash(pdfpath);
                //ViewBag.PdfHash = pdfHash;
                //CdacDigitalSigningService dsc = new CdacDigitalSigningService();
                //System.Threading.Tasks.Task task = dsc.SignPdfHashAsync(pdfHash);
                var fileStream = new FileStream(pdfpath, FileMode.Open, FileAccess.Read);
                var fsResult = new FileStreamResult(fileStream, "application/pdf");
                Response.AppendHeader("Content-Disposition", "inline; filename=agenda.pdf");
                //return fsResult;
                return View("PdfView", model: pdfpath);

                //return File(pdfpath, "application/pdf", "agenda.pdf");
            }
            catch (Exception e)
            {
                string s = e.Message;
                return View("PdfView", model: null); ;
            }
        }
        public ActionResult GetPdf(string filePath)
        {
            // Return the PDF file
            return File(filePath, "application/pdf");
        }
        private string GetPdfHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
        }
        public string GetNewapprovepdfhtml2(int agendaid)
        {
            AgendaDetailModel mdl = new AgendaDetailModel();

            string deptid = string.Empty;

            if (CurrentSession.DeptID != null)
            {
                deptid = CurrentSession.DeptID.ToString();
            }
            int ofid = 0;
            if (CurrentSession.OfficeId == "47")
            {
                ofid = 2;
            }
            else if (CurrentSession.OfficeId == "10")
            {
                ofid = 1;
            }
            Authoritylist cl = new Authoritylist();
            cl.AuthorityId_list = AgendaModelFunction.getAuthoritylist(deptid);
            string maintitle = string.Empty;
            mdl._LstReferences = AgendaModelFunction.GetAdminlobDataForPDF(agendaid);
            maintitle = AgendaModelFunction.GetAgendaMainTitle(agendaid);
            var HeaderlistofAgenda = AgendaModelFunction.GetAgendaHeader(agendaid);

            string outXml = "";
            outXml = outXml + @"<html style='height: 100%;'>   
                                    <head><meta charset='UTF-8'><title>ApprovedProceedingReport</title>                       
                                        <style>
                                    .pdf-class .MsoNormal{
                                        text-wrap:wrap;
                                    }
                                    .pdf-class .MsoNormalTable td{
                                        max-width: 65px !important;
                                        padding: 1px !important;
                                        overflow-wrap: break-word;
                                    }

                                    .pdf-class .MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    .pdf-class table.MsoNormalTable {
                                        max-width: 700px !important;
                                        margin-left: 0 !important;
                                    }
                                    
                                    .pdf-class .MsoTableGrid td{
                                        max-width: 65px !important;
                                        text-wrap:wrap;
                                    }

                                    .pdf-class .MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }

                                    .pdf-class table.MsoTableGrid {
                                        max-width: 500px !important;
                                        margin-left: 0 !important;
                                    }
                                    p.MsoListParagraphCxSpFirst,p.MsoListParagraphCxSpMiddle,p.MsoListParagraphCxSpLast {
                                        margin-left: 20px;
                                    }
                                    td {
                                        vertical-align: top;
                                    }
                                        @media print {
                                            .header {
                                                display: none;
                                            }
                                        }
                                            p{
                                            line-height:1.7;
                                            }
                                            table{
                                            width:100%;
                                            }
@font-face {
    font-family: Raavi,sans-serif;
    src: url('file:///E:/Projects/CMNirdesh_13092023_Latest/CMNirdesh/CMNirdesh/fonts/Raavi.ttf');
}
        body {
            font-family: 'Raavi', sans-serif; /* Use Raavi font for Punjabi text */
        }
    </style>
                                    </head> 
								 <body><div class='pdf-class' style='max-width:1050px; margin:auto;min-height: 100%;'>";
            outXml += @"<p> लोरेम इप्सम , प्लेसहोल्डर या डमी टेक्स्ट का उपयोग लेआउट के पूर्वावलोकन के लिए टाइपसेटिंग और ग्राफिक डिजाइन में किया जाता है। इसमें तले हुए लैटिन पाठ की सुविधा है, जो लेआउट की सामग्री से अधिक डिज़ाइन पर जोर देती है। यह मुद्रण और प्रकाशन उद्योगों का मानक प्लेसहोल्डर पाठ है।</p>";
            string gt = DateTime.Now.ToString("dd/MM/yyyy");
            if (ofid == 2)
            {
                outXml += @"<div style='text-align: center;font-size: 16pt;font-weight: bold;border-bottom:3px solid #000;width: 100%;'>ਦਫਤਰ ਨਗਰ ਨਿਗਮ ,ਸਾਹਿਬਜ਼ਾਦਾ ਅਜੀਤ ਸਿੰਘ ਨਗਰ</div>    
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style='font-size: 12pt;float: left;width: 50%;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style='font-size: 12pt;float: right;width: 30%;text-align: right;'> <b>ਮਿਤੀ  ..... " + gt + @" .</b></p> 
                            </div>
                            <br> <br> <br> <br> 
                            <p style='display: inline; font-size: 12pt;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p>
            
			                <div style='margin-bottom:10px;'><ol style='margin: 15px 65px; font-size:12pt;'>";
                for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                {
                    outXml += @"<li>" + cl.AuthorityId_list[j].name + "," + cl.AuthorityId_list[j].Address + "</li>";
                }
                outXml += @"</ol>";
                outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;'> " + mdl._LstReferences[0].FromDeptLocal + @" ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                             
                                </span></p>
            
                                <p style='text-align: justify; font-size: 12pt;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ਉਕਤ ਵਿਸ਼ੇ ਸਬੰਦੀ ਬੇਨਤੀ ਹੈ ਕੀ ਨਗਰ ਨਿਗਮ , ਸਾਹਿਬਜ਼ਾਦਾ ਅਜੀਤ ਸਿੰਘ ਨਗਰ ਦੇ  ਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ : " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"
                                  ਨੂ ਸਮਾਂ ਸਵੇਰੇ " + Convert.ToString(HeaderlistofAgenda.StartTime) + @" ਮਿਉਂਸਪਲ  ਭਵਨ , ਸੈਕਟਰ-੬੮,ਸਾਹਿਬਜ਼ਾਦਾ ਅਜੀਤ ਸਿੰਘ ਨਗਰ ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ ਸ਼੍ਰੀ ਅਮਰਜੀਤ ਸਿੰਘ ਸਿਧੂ, ਮੇਅਰ ,ਨਗਰ ਨਿਗਮ ਜੀ ਦੀ ਪਧਾਨਗੀ ਹੇਠ ਹੋਵੇਗੀ .
				                    <br>
				                    <br>
				                    ਨਥੀ : ਏਜੇਂਡੇ ਦੀ ਕਾਪੀ |   
                                </p>
                                <p style='margin: auto; font-size: 12pt;text-align:right;'> 
			                    ਸਕੱਤਰ ,<br>
                                               ਨਗਰ ਨਿਗਮ,<br>
                                              ਐਸ .ਏ .ਐਸ ਨਗਰ                
                                </p>";
            }
            else
            {
                if (mdl._LstReferences.Count > 0)
                {
                    outXml += @"<div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align: left;margin: 0px;'> <b>Website : " + mdl._LstReferences[0].DeptWebsite + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align: right;margin: 0px;'> <b>" + mdl._LstReferences[0].MayorContactNo + @" (Mayor)</b></p> 
                             </div>
                            <div style='display:flex;'>
                                <p style='font-size: 16px;width: 60%; text-align:left;margin: 0px;'> <b>E-Mail : " + mdl._LstReferences[0].DeptEmail + @"</b></p>
                                <p style='font-size: 16px;width: 40%; text-align:right;margin: 0px;'> <b>" + mdl._LstReferences[0].CommContactNo + @" (Commissioner)</b></p>
                            </div>
                            <div style='text-align: center;font-size: 30px;font-weight: bold;'>ਨਗਰ ਨਿਗਮ</div>    
                            <div style='text-align: center;'> 
                                <h3>" + mdl._LstReferences[0].DeptAddress_Local + @"</h3>
                            </div>
                               <div style='display: flex; align-items: center; justify-content: space-between;'>
                                <p style=' font-size: 16px;float: left;width: 50%;margin: 0px;'> <b>ਨੰਬਰ  .................</b></p>
                                <p style=' font-size: 16px;float: right;width: 30%;text-align: right;margin: 0px;'> <b>ਮਿਤੀ  ..... " + gt + @" .</b></p> 
                            </div>
                            <br> 
                            <p style='display: inline; font-size: 16px;'><b>ਸੇਵਾ ਵਿਖੇ ,  </b></p>
            
			                <div class='text-align:center;margin-bottom:10px;'>";
                    for (int j = 0; j < cl.AuthorityId_list.Count; j++)
                    {
                        outXml += @"<div style='margin: 15px 65px; font-size:12pt;'>" + (j + 1).ToString() + ") " + cl.AuthorityId_list[j].name + "," + cl.AuthorityId_list[j].Address + "</div>";
                    }
                    outXml += @"<p style='text-align: justify; font-size: 12pt; display: flex;' ><b style='margin-right: 20px;'> ਵਿਸ਼ਾ :- </b> 
                                <span style='text-align: center;font-size:12pt;'> ਮਿਉਂਸਪਲ ਕਾਰਪੋਰੇਸ਼ਨ, " + mdl._LstReferences[0].FromDeptLocal + @" ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਦਾ ਏਜੰਡਾ। ।                                      
                                </span> 
                            </p>
            
                            <p style='text-align: justify; font-size: 16px;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ਉਕਤ ਵਿਸ਼ੇ ਸਬੰਦੀ ਬੇਨਤੀ ਹੈ ਕੀ , " + mdl._LstReferences[0].FromDeptLocal + @" ਦੇ  ਹਾਊਸ ਦੀ ਸਧਾਰਨ ਮੀਟਿੰਗ ਮਿਤੀ : " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"
                              ਨੂ ਸਮਾਂ " + Convert.ToString(HeaderlistofAgenda.StartTime) + @" ਮਿਉਂਸਪਲ  ਭਵਨ , ਸੈਕਟਰ-੬੮," + mdl._LstReferences[0].DeptAddress_Local + @" ਦੇ ਮੀਟਿੰਗ ਹਾਲ ਵਿਖੇ " + mdl._LstReferences[0].DeptMayor + @", ਮੇਅਰ ,ਨਗਰ ਨਿਗਮ ਜੀ ਦੀ ਪਧਾਨਗੀ ਹੇਠ ਹੋਵੇਗੀ .
				                <br>
				                <br>
				                ਨਥੀ : ਏਜੇਂਡੇ ਦੀ ਕਾਪੀ | <br>  <br>  

                            </p>        
        
                            <p style='margin: auto; font-size: 16px;text-align:right;'> 
			                ਸਕੱਤਰ ,<br>
                                          " + mdl._LstReferences[0].FromDeptLocal + @"             
                            </p></div>";
                }
            }
            if (HeaderlistofAgenda != null)
            {
                outXml += @"<div style='margin: 70px 70px;page-break-before: always;'>
                        <h1 style='font-size: 30px; font-weight: 700; text-align: center; border-bottom: 3px solid #000; padding-top: 20px;'>"
                    + HeaderlistofAgenda.AgendaName + "</h1>";
                outXml += @"<p style='font-size: 12pt; text-align: center'>"
                 + HeaderlistofAgenda.Header + "</p></div>";
            }

            outXml += @"<table style='font-size: 12pt; border: 1px solid black;border-collapse: collapse;'>";
            outXml += @"<tr style = 'padding:5px;'>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Sr. No</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Date of Meeting</th>
                  <th style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>Agenda Description </th></tr>";

            if (mdl._LstReferences != null && mdl._LstReferences.Count > 0)
            {
                for (int i = 0; i < mdl._LstReferences.Count; i++)
                {
                    outXml += @"<tr>
										<td style='padding:10px; border: 1px solid black;border-collapse: collapse;'>               
												" + mdl._LstReferences[i].SrNo + @"    
										</td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse; '>
                                        " + Convert.ToString(HeaderlistofAgenda.AgendaDate) + @"  
                                        </td>
                                        <td style ='padding:10px; border: 1px solid black;border - collapse: collapse;'>";
                    string LOBText = Convert.ToString(mdl._LstReferences[i].SubjectDetails);

                    string _subject = Convert.ToString(mdl._LstReferences[i].Subject);
                    if (_subject != "")
                    {

                        _subject = "<b>" + _subject + " :- </b>";
                    }


                    outXml += @"" + _subject + " " + LOBText + "<br/>";
                    outXml += @"</td>
                                        </tr>";
                    //}                                                  
                    if (i == mdl._LstReferences.Count - 1)
                    {


                        outXml += "</table>";
                        if (HeaderlistofAgenda != null)
                        {
                            outXml += @"<p style ='text-align: right; font-size: 18px;'> 
                                         " + HeaderlistofAgenda.Footer + "</p>";
                        }

                    }
                }
                outXml += "</div></body> </html>";
            }
            return outXml;
        }

    }


}
