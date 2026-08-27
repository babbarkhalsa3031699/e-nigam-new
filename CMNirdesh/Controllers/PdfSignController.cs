using CMNirdesh.Models;
using CMNirdesh.Models.eSign;

using iTextSharp.text.pdf;

using iTextSharp.text.pdf.parser;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static CMNirdesh.Models.eSign.eSignRequest;

namespace CMNirdesh.Controllers
{
    [SessionState(SessionStateBehavior.Required)]

    public class PdfSignController : Controller
    {
        private static XmlDocument xmldoc;
        private static PdfSignatureAppearance appearance;
        private static MemoryStream fout;
        static int csize = 65536;

        [HttpGet]

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Esign()
        {
            return View(new EsignRequestModel());
        }

        [HttpPost]
        public ActionResult Submit(EsignRequestModel model)
        {
            string AgendaId = "4";

            if (ModelState.IsValid)
            {
                var fileUpload = Request.Files[0];

                try
                {
                    if (fileUpload != null && System.IO.Path.GetExtension(fileUpload.FileName).ToLower() == ".pdf")
                    {
                        ClsEsignXml esignXml = new ClsEsignXml
                        {
                            aspId = "PMID-900",
                            ver = "2.1",
                            AuthMode = "1",
                            ekycId = "",
                            ekycIdType = "A",
                            responseSigType = "pkcs7",
                            responseUrl = Url.Action("ResponseHandler", "PdfSign", null, Request.Url.Scheme),
                            sc = "Y",
                            ts = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"),
                            txn = Guid.NewGuid().ToString("N"),
                        };

                        Session["txn"] = esignXml.txn;
                        string filePath = System.IO.Path.Combine(Server.MapPath("~/App_Data"), System.IO.Path.GetFileName(fileUpload.FileName));
                        fileUpload.SaveAs(filePath);

                        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                        Session["fileBytesUpload"] = fileBytes;

                        string hash = CreateStamper(fileBytes, CurrentSession.Name, CurrentSession.UserName, 1, 1);
                        var s = CurrentSession.Layer2Text;
                        System.IO.File.Delete(filePath);
                        esignXml.Docs.InputHash.id = "1";
                        esignXml.Docs.InputHash.docInfo = "mydoc";
                        esignXml.Docs.InputHash.hashAlgorithm = "SHA256";
                        esignXml.Docs.InputHash.Hash = hash;
                        string signedXml = prepareXML(esignXml, Server.MapPath("~/eNigam.pem"));
                        Session["xmlData"] = signedXml;
                        ASPSession.Current.XmlData = signedXml;
                        ASPSession.Current.AgendaID = AgendaId;
                        SaveEsignStateToCache(esignXml.txn);
                        return RedirectToAction("ProcessXml", new { txn = esignXml.txn });
                    }
                    else
                    {
                        ViewBag.Message = "Please select a PDF file.";
                        return View("Index");
                    }
                }
                catch (Exception exp)
                {
                    ViewBag.Message = exp.Message;
                    return View("Index");
                }
            }
            return View("Esign");
        }



        public async Task<ActionResult> SignPdfCommissioner(string AgendaId, string Type, string ApprovalID = "")
        {

            AgendaDetailModel agendaDetailModel = new AgendaDetailModel();
            string Blobpath = AgendaModelFunction.GetUnsignedPdf(Convert.ToInt32(AgendaId), Type, ApprovalID);
            byte[] fileBytes1 = await BlobStorage.GetFileFromAzureAsync(Blobpath);
            string fileBase64 = Convert.ToBase64String(fileBytes1);

            if (ModelState.IsValid)
            {
                try
                {
                    if (fileBase64 != null)
                    {
                        ClsEsignXml esignXml = new ClsEsignXml
                        {
                            aspId = "PMID-001",
                            ver = "2.1",
                            AuthMode = "1",
                            ekycId = "",
                            ekycIdType = "A",
                            responseSigType = "pkcs7",
                            //responseUrl = Url.Action("ResponseHandler", "PdfSign", null, Request.Url.Scheme),
                            responseUrl = Url.Action("ResponseHandler", "PdfSign", null, "https"),
                            sc = "Y",
                            ts = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                            txn = Guid.NewGuid().ToString("N"),
                        };


                        byte[] fileBytes = await BlobStorage.GetFileFromAzureAsync(Blobpath);
                        string hash = CreateStamper(fileBytes, CurrentSession.Name, CurrentSession.UserName, 1, 1);
                        var s = CurrentSession.Layer2Text;
                        //System.IO.File.Delete(filePath);
                        esignXml.Docs.InputHash.id = "1";
                        esignXml.Docs.InputHash.docInfo = "mydoc";
                        esignXml.Docs.InputHash.hashAlgorithm = "SHA256";
                        esignXml.Docs.InputHash.Hash = hash;
                        string signedXml = prepareXML(esignXml, Server.MapPath("~/eNigamProduction.pem"));
                        ESignTxnRepository.Save(new ESignTxnContext
                        {
                            TxnId = esignXml.txn,
                            AgendaId = AgendaId,
                            ApprovalId = ApprovalID,
                            SignatureType = Type,
                            PdfBlobPath = Blobpath,
                            RequestXML = signedXml,
                            CreatedBy = CurrentSession.UserID,
                            CreatedOn = DateTime.Now,
                            DocuementHash = hash,
                        });
                        SaveEsignStateToCache(esignXml.txn);
                        return RedirectToAction("ProcessXml", new { txn = esignXml.txn });
                    }
                    else
                    {
                        ViewBag.Message = "Please select a PDF file.";
                        return View("Index");
                    }
                }
                catch (Exception exp)
                {
                    ViewBag.Message = exp.Message;
                    return View("Index");
                }
            }
            return View("Esign");
        }


        public async Task<ActionResult> SignApproval(string AgendaId, string Blobpath, string ApprovedId)
        {
            //CDAC - 901

            AgendaDetailModel agendaDetailModel = new AgendaDetailModel();
            //string Blobpath = AgendaModelFunction.GetCommissionerPdf(Convert.ToInt32(AgendaId));
            byte[] fileBytes1 = await BlobStorage.GetFileFromAzureAsync(Blobpath);
            string fileBase64 = Convert.ToBase64String(fileBytes1);

            if (ModelState.IsValid)
            {


                try
                {
                    if (fileBase64 != null)
                    {
                        ClsEsignXml esignXml = new ClsEsignXml
                        {
                            aspId = "PMID-900",
                            ver = "2.1",
                            AuthMode = "1",
                            ekycId = "",
                            ekycIdType = "A",
                            responseSigType = "pkcs7",
                            responseUrl = Url.Action("ResponseHandler", "PdfSign", null, Request.Url.Scheme),
                            sc = "Y",
                            ts = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                            txn = Guid.NewGuid().ToString("N"),
                        };

                        Session["txn"] = esignXml.txn;
                        //string filePath = System.IO.Path.Combine(Server.MapPath("~/App_Data"), System.IO.Path.GetFileName(fileUpload.FileName));
                        //fileUpload.SaveAs(filePath);


                        byte[] fileBytes = await BlobStorage.GetFileFromAzureAsync(Blobpath);
                        Session["fileBytesUpload"] = fileBytes;

                        string hash = CreateStamper(fileBytes, CurrentSession.Name, CurrentSession.UserName, 1, 1);
                        var s = CurrentSession.Layer2Text;
                        //System.IO.File.Delete(filePath);
                        esignXml.Docs.InputHash.id = "1";
                        esignXml.Docs.InputHash.docInfo = "mydoc";
                        esignXml.Docs.InputHash.hashAlgorithm = "SHA256";
                        esignXml.Docs.InputHash.Hash = hash;
                        string signedXml = prepareXML(esignXml, Server.MapPath("~/eNigamProduction.pem"));
                        Session["xmlData"] = signedXml;
                        ASPSession.Current.XmlData = signedXml;
                        ASPSession.Current.AgendaID = AgendaId;
                        ASPSession.Current.ApprovalId = ApprovedId;
                        SaveEsignStateToCache(esignXml.txn);
                        return RedirectToAction("ProcessXml", new { txn = esignXml.txn });
                    }
                    else
                    {
                        ViewBag.Message = "Please select a PDF file.";
                        return View("Index");
                    }
                }
                catch (Exception exp)
                {
                    ViewBag.Message = exp.Message;
                    return View("Index");
                }
            }
            return View("Esign");
        }

        public ActionResult ProcessXml(string txn)
        {


            if (string.IsNullOrEmpty(txn))
            {
                ViewBag.Message = "Invalid transaction.";
                return View("Index");
            }

            var ctx = ESignTxnRepository.Get(txn);
            if (ctx == null)
            {
                ViewBag.Message = "Transaction not found.";
                return View("Index");
            }

            ViewBag.EsignItemList = GetEsignItemList(ctx.RequestXML, ctx.TxnId);
            return View();
        }

        public string GetEsignItemList(string xml, string txn)
        {
            if (string.IsNullOrEmpty(xml) || string.IsNullOrEmpty(txn))
                return string.Empty;

            StringBuilder items = new StringBuilder();

            string template =
                "<input type=\"hidden\" id=\"eSignRequest\" name=\"eSignRequest\" value='{XML}' />" +
                "<input type=\"hidden\" id=\"aspTxnID\" name=\"aspTxnID\" value=\"{TXN}\" />" +
                "<input type=\"hidden\" id=\"Content-Type\" name=\"Content-Type\" value=\"application/xml\" />";

            template = template
                .Replace("{XML}", HttpUtility.HtmlAttributeEncode(xml))
                .Replace("{TXN}", txn);

            items.Append(template);
            return items.ToString();
        }


        public static string signXML(string filePath, string certificatePath)
        {
            XmlDocument xmldoc = null;

            try
            {
                xmldoc = new XmlDocument();

                xmldoc.LoadXml(filePath);
                // Creating the XML signing object.
                SignedXml sxml = new SignedXml(xmldoc);

                ///////////////////////////////FOR PEM FILE//////////////////////////////

                RSACryptoServiceProvider crypt;
                string path = certificatePath;

                using (var reader = System.IO.File.OpenText(path)) // file containing RSA PKCS1 private key
                    crypt = ImportPrivateKey(reader);

                ////////////////////////// END ////////////////////////////////////////////            

                /////////////////////////////////////////////////
                sxml.SigningKey = crypt;//csp

                // Set the canonicalization method for the document.
                sxml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl; // No comments.

                // Create an empty reference (not enveloped) for the XPath
                // transformation.
                Reference r = new Reference("");

                // Create the XPath transform and add it to the reference list.
                r.AddTransform(new XmlDsigEnvelopedSignatureTransform(false));

                // Add the reference to the SignedXml object.
                sxml.AddReference(r);

                // Compute the signature.
                sxml.ComputeSignature();

                // Get the signature XML and add it to the document element.
                XmlElement sig = sxml.GetXml();
                xmldoc.DocumentElement.AppendChild(sig);

                xmldoc.PreserveWhitespace = true;

                //fMakeLog("ClsSignPdf-signXML: " + " signature xml generated, returning xmldoc.InnerXml");

                // fMakeLog("ClsSignPdf-signXML exit");

                return xmldoc.InnerXml;
            }
            catch (Exception ex)
            {
                //fMakeLog("ClsSignPdf-signXML exception: " + ex.ToString());
                return xmldoc.InnerXml;
            }
        }

        public static RSACryptoServiceProvider ImportPrivateKey(StreamReader pem)
        {
            RSACryptoServiceProvider csp = null;

            try
            {
                PemReader pemReader = new PemReader(pem);
                var pemObject = pemReader.ReadObject();

                if (pemObject is AsymmetricCipherKeyPair keyPair)
                {
                    RsaPrivateCrtKeyParameters privateKeyParams = (RsaPrivateCrtKeyParameters)keyPair.Private;
                    RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(privateKeyParams);

                    csp = new RSACryptoServiceProvider();
                    csp.ImportParameters(rsaParams);
                }
                else if (pemObject is RsaPrivateCrtKeyParameters privateKeyParams)
                {
                    RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(privateKeyParams);

                    csp = new RSACryptoServiceProvider();
                    csp.ImportParameters(rsaParams);
                }
                else
                {
                    throw new Exception("Unsupported PEM object type.");
                }

                return csp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                return csp;
            }
        }


        public static string prepareXML(object esignXml, string certificatePath)
        {
            string signedXML = null;

            try
            {


                var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var serializer = new XmlSerializer(esignXml.GetType());
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (var stream = new StringWriter())
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    writer.WriteStartDocument(true);
                    serializer.Serialize(writer, esignXml, emptyNamepsaces);
                    string xml = stream.ToString();



                    signedXML = signXML(xml, certificatePath);
                    return signedXML;
                }
            }
            catch (Exception ex)
            {
                return signedXML;
            }
        }
        public static string CreateStamper(byte[] pdfDocument1, string Name, string UserName, int Serial, int pageNumber)
        {
            string hex = "";
            int count;
            string errorMsg = "";
            PdfReader reader1 = new PdfReader(pdfDocument1);
            fout = new MemoryStream();

            int pageSize = reader1.NumberOfPages;
            pageNumber = pageSize;
            //byte[]  pdfDocument = PdfFooterRemover.RemoveFooter(pdfDocument1, pageNumber);
            PdfReader reader = new PdfReader(pdfDocument1);
            try
            {
                using (PdfStamper stamper = PdfStamper.CreateSignature(reader, fout, '\0', null, true))
                {
                    string Designation = CurrentSession.Designation;
                    stamper.SignatureAppearance.Layer2Text = "Digitally Signed By " + Name + "\r\nDate: " + DateTime.Now;
                    stamper.SignatureAppearance.Layer2Text =
                    "Digitally Signed By: " + Name +
                    "\r\nDesignation: " + Designation +
                    "\r\nDate: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");


                    stamper.SignatureAppearance.Layer4Text = "";
                    //stamper.SignatureAppearance.FieldName = 
                    stamper.SignatureAppearance.Reason = "e-Nigam Application eSign";
                    appearance = stamper.SignatureAppearance;
                    appearance.Acro6Layers = false;
                    string sigName = "Signature";
                    string name = appearance.FieldName;
                    Serial -= 1;
                    sigName = sigName + DateTime.Now.AddMinutes(5);
                    count = Serial - (Serial / 3) * 3;
                    PdfReaderContentParser parser = new PdfReaderContentParser(reader);
                    Models.TextMarginFinder finder = parser.ProcessContent(pageNumber, new Models.TextMarginFinder());
                    List<float> lineHeights = finder.GetLineHeights(); // This method should return Y-positions of each line on the page
                    if (lineHeights.Count >= 2)
                    {
                        // Get the Y-position of the second last line
                        float secondLastLinePos_y = lineHeights[lineHeights.Count - 2];
                        // Define the position for the signature just below the second last line
                        float signPos_x = reader1.GetPageSize(pageNumber).Width - 170; // Adjust X position as needed
                        float signPos_y = secondLastLinePos_y - 70; // Place signature 50 units below the second last line

                        try
                        {
                            // Set visible signature
                            appearance.SetVisibleSignature(new iTextSharp.text.Rectangle(signPos_x, signPos_y, signPos_x + 140, signPos_y + 50), pageNumber, sigName);
                        }
                        catch (Exception e)
                        {
                            errorMsg = "Error while setting signature: " + e.Message;
                            return errorMsg;
                        }
                    }
                    else
                    {
                        return "Error: The page has less than 2 lines of content.";
                    }

                    //float signPos_x = reader.GetPageSize(pageNumber).Width - 170;
                    //float signPos_y = finder.GetBottom() - 70;  
                    //try
                    //{
                    //    appearance.SetVisibleSignature(new iTextSharp.text.Rectangle(signPos_x, signPos_y, signPos_x + 140, signPos_y + 50), pageNumber, sigName);
                    //    var pcb = new iTextSharp.text.pdf.PdfContentByte(stamper.Writer);
                    //    var annot = iTextSharp.text.pdf.PdfAnnotation.CreateText(stamper.Writer, new iTextSharp.text.Rectangle(signPos_x, signPos_y, signPos_x + 140, signPos_y + 50), appearance.Stamper.AcroFields.Fields.Values.ToString(), sigName, true, "");
                    //}
                    //catch (Exception e)
                    //{
                    //    errorMsg = "Error: " + e.Message;
                    //    return e.Message;
                    //}

                    appearance.SignDate = DateTime.Now.AddMinutes(5);

                    PdfSignature objSignature = new PdfSignature(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
                    objSignature.Date = new PdfDate(appearance.SignDate);
                    objSignature.Reason = "e-Nigam eSign";
                    objSignature.Location = "";
                    appearance.CryptoDictionary = objSignature;
                    Dictionary<PdfName, int> exc = new Dictionary<PdfName, int>();
                    exc[PdfName.CONTENTS] = csize * 2 + 2;
                    CurrentSession.Appearance = appearance;
                    CurrentSession.Fout = fout;
                    ASPSession.Current.Appearance = appearance;
                    ASPSession.Current.Fout = fout;

                    appearance.PreClose(exc);
                    Stream hashStream = appearance.GetRangeStream();
                    byte[] documentHash;
                    SHA256Managed hastStr = new SHA256Managed();
                    documentHash = hastStr.ComputeHash(hashStream);
                    foreach (byte x in documentHash)
                    {
                        hex += string.Format("{0:x2}", x);
                    }

                    return hex;
                }
            }
            catch (Exception exp)
            {
                fout.Close();
                reader.Close();

                if (errorMsg != "")
                {

                    return errorMsg;
                }
                else
                {
                    return exp.Message;
                }
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ResponseHandler()
        {
            var response = Request.Form;
            if (response != null && !string.IsNullOrEmpty(response["eSignResponse"]))
            {
                try
                {
                    XElement tempProd = XElement.Parse(response["eSignResponse"]);
                    if (tempProd != null && tempProd.Attribute("txn") != null)
                    {
                        string tempTxn = tempProd.Attribute("txn").Value;
                        RestoreEsignStateFromCache(tempTxn);
                    }
                }
                catch { } // fail silently, let existing logic handle errors
            }

            string message = string.Empty;

            if (!string.IsNullOrEmpty(response["eSignResponse"]))
            {
                string xmlData = response["eSignResponse"];
                if (!IsValidXml(xmlData))
                {
                    ViewBag.ErrorMessage = "Invalid XML format.";
                    return View("Index");
                }
                try
                {
                    ViewBag.ResponseText = xmlData;
                    XElement product = XElement.Parse(xmlData);
                    string signature1 = product.LastNode.ToString();
                    string status = product.Attribute("status").Value;
                    string transID = product.Attribute("txn").Value;
                    string respTime = product.Attribute("ts").Value;
                    string RequestXMLData = ASPSession.Current.XmlData;

                    // Directly lookup transaction by txn GUID which guarantees resolution even without UserID
                    var ctx = ESignTxnRepository.Get(transID);
                    if (ctx == null)
                    {
                        message = "Transaction record not found for transID: " + transID;
                        ViewBag.Message = message;
                        return View("Index");
                    }
                    string AgendaID = ctx.AgendaId;
                    string ApprovalId = ctx.ApprovalId;
                    string SignatureType = ctx.SignatureType;

                    // Use standard db record mapping for the UserID, ensuring total lack of dependency on ASP.NET Cookie
                    string UserID = ctx.CreatedBy;
                    // status = "1";
                    if (status == "1")
                    {
                        try
                        {
                            string pkcs = product.FirstNode.NextNode.ToString();
                            product = XElement.Parse(pkcs);
                            pkcs = product.Value;
                            byte[] bytes = Convert.FromBase64String(pkcs);
                            var cert = new X509Certificate2(bytes);
                            var ret = cert.Subject;
                            var KeyValuePairs = ret.Trim('{', '}').Split(',').Select(s => s.Trim().Split('=')).ToDictionary(a => a[0], a => a[1]);
                            string CommonName = KeyValuePairs["CN"];
                            string UserName = ASPSession.Current.Appearance.Layer4Text;
                            var eSignRecord = new eSignHistory
                            {
                                UserID = UserName,
                                Status = status,
                                TransactionID = transID,
                                ResponseTime = Convert.ToDateTime(respTime),
                                Name = CommonName,
                                SerialNumber = 456,
                                SignedAt = DateTime.Now,
                                requestXML = RequestXMLData,
                                responseXML = xmlData

                            };
                            string result = eSignHistoryModel.SaveESignHistory(eSignRecord);
                            bool isVerify = VerifySignature(xmlData);
                            // bool isVerify = true;
                            if (isVerify == true)
                            {
                                //string AgendaID = ASPSession.Current.AgendaID;
                                //string ApprovalId = ASPSession.Current.ApprovalId;
                                //string SignatureType = ASPSession.Current.SignatureType;


                                appearance.Layer2Text = $"Digitally Signed by {CommonName}\r\nDate: {DateTime.Now}";
                                byte[] fileBytes = embedSignature(pkcs, transID);
                                string fileBase64 = Convert.ToBase64String(fileBytes);
                                //HttpContext.Session["appearance"] = "";
                                //HttpContext.Session["fout"] = "";

                                //code to save aprroval and open pdf created by joginder 
                                if (string.IsNullOrEmpty(fileBase64))
                                {
                                    return Json(new { success = false, message = "Base64 string is empty or null." }, JsonRequestBehavior.AllowGet);
                                }

                                try
                                {
                                    // Generate a unique file name for the PDF
                                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    string sanitizedFilepath = System.IO.Path.GetFileNameWithoutExtension(AgendaID); // Remove unsafe characters
                                    string pdfFileName = string.Format("{0}_{1}_Signed_Proceeding.pdf", sanitizedFilepath, timestamp);
                                    string pdfFilePath = System.IO.Path.Combine("Proceedings", pdfFileName);

                                    // Decode the Base64 string to get the PDF file bytes
                                    //byte[] fileBytes = Convert.FromBase64String(base64String);

                                    // Upload the file to blob storage
                                    string blobUrl = BlobStorage.UploadeSignedFile(fileBytes, pdfFilePath);
                                    Uri blobUri = new Uri(blobUrl);

                                    // Compute the SHA256 hash of the file
                                    string hash;
                                    using (var sha256 = System.Security.Cryptography.SHA256.Create())
                                    {
                                        hash = string.Concat(sha256.ComputeHash(fileBytes).Select(x => x.ToString("X2")));
                                    }

                                    // If `approvedId` is valid, update approvals
                                    //if (int.TryParse(ApprovalId, out int approvalId) && approvalId != 0)
                                    //{
                                    //    string agendaId = AgendaID; // Retrieve agenda ID from session
                                    //    string currentApprovalId = ApprovalId; // Retrieve approval ID from session
                                    //    UpdateApprovals(
                                    //        Convert.ToInt32(agendaId),
                                    //        Convert.ToString(CurrentSession.UserID),
                                    //        pdfFilePath,
                                    //        approvalId
                                    //    );
                                    //}
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(SignatureType))
                                        {
                                            string agendaIdD = AgendaID;

                                            // Update PDF path in database
                                            int updatedId = UpdatePdf(
                                                Convert.ToInt32(agendaIdD),
                                                pdfFilePath,
                                                SignatureType,
                                                ApprovalId
                                            );

                                            if (updatedId <= 0)
                                            {
                                                return Json(new { success = false, message = "UpdatePdf failed for AgendaId=" + agendaIdD }, JsonRequestBehavior.AllowGet);
                                            }
                                        }

                                        // Send file inline to browser
                                        //Response.Headers.Add("Content-Disposition", "inline; filename=SignedPDF.pdf");
                                        //return File(fileBytes, "application/pdf");
                                        return Redirect(blobUrl);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                                        return new HttpStatusCodeResult(500, "Error while signing PDF");
                                    }
                                }
                                catch (FormatException)
                                {
                                    return Json(new { success = false, message = "The provided Base64 string is not valid." }, JsonRequestBehavior.AllowGet);
                                }
                                catch (IOException ioEx)
                                {
                                    System.Diagnostics.Debug.WriteLine($"IOException: {ioEx.Message}");
                                    return Json(new { success = false, message = "An error occurred while processing the file." }, JsonRequestBehavior.AllowGet);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                                    return Json(new { success = false, message = "An unexpected error occurred. Please try again later." }, JsonRequestBehavior.AllowGet);
                                }

                                //return RedirectToAction("OpenPDF", "PDFSign", new { filepath = AgendaID, base64String = fileBase64, approvedId = ApprovalId });
                            }
                            else
                            {
                                return Json(new { success = false, message = "Verification failed or not required." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (Exception ex)
                        {
                            message = "Error processing eSign response: " + ex.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = "Error processing eSign response: " + ex.Message;
                }
            }
            else
            {
                message = "eSign response is empty or null.";
            }

            ViewBag.Message = message;
            return View("Index");
        }


        private bool IsValidXml(string xml)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool VerifySignature(string responce)
        {

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.PreserveWhitespace = true;
            xmldoc.LoadXml(responce);
            SignedXml sxml = new SignedXml(xmldoc);

            try
            {
                XmlNode dsig = xmldoc.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl)[0];
                sxml.LoadXml((XmlElement)dsig);
            }
            catch
            {
                Console.Error.WriteLine("Error: no signature found.");

            }
            X509Certificate2 x509 = new X509Certificate2(Server.MapPath("~/es-staging.pem"));
            //X509Certificate2 x509 = new X509Certificate2(Server.MapPath("~/eNigam_production_Certificate.pem"));
            if (sxml.CheckSignature(x509, true))
                return true;
            else
                return false;


        }


        public static byte[] embedSignature(string pkcdRespose, string transactionId)
        {
            PdfSignatureAppearance appearance1;
            MemoryStream fout1 = new MemoryStream();

            try
            {
                appearance = Models.ASPSession.Current.Appearance;
                appearance1 = Models.ASPSession.Current.Appearance;
                fout = Models.ASPSession.Current.Fout;
                fout1 = Models.ASPSession.Current.Fout;
                string pkcsString = pkcdRespose;
                string s = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(pkcsString));
                byte[] pkcsBytes1 = Convert.FromBase64String(pkcsString);
                byte[] outc = new byte[csize];
                PdfDictionary dic2 = new PdfDictionary();
                Array.Copy(pkcsBytes1, 0, outc, 0, pkcsBytes1.Length);
                appearance1.SignDate = DateTime.Now;
                dic2.Put(PdfName.CONTENTS, new PdfString(outc).SetHexWriting(true));

                appearance1.Close(dic2);
                return fout1.ToArray();
            }
            catch (Exception ex)
            {
                return fout.ToArray();
            }
        }

        [ValidateInput(false)]
        public ActionResult OpenPDF(string filepath, string base64String, string approvedId)
        {
            // Validate the input parameters
            if (string.IsNullOrEmpty(base64String))
            {
                return Json(new { success = false, message = "Base64 string is empty or null." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                // Generate a unique file name for the PDF
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string sanitizedFilepath = System.IO.Path.GetFileNameWithoutExtension(filepath); // Remove unsafe characters
                string pdfFileName = $"{sanitizedFilepath}_{timestamp}_Signed_Proceeding.pdf";
                string pdfFilePath = System.IO.Path.Combine("Proceedings", pdfFileName);

                // Decode the Base64 string to get the PDF file bytes
                byte[] fileBytes = Convert.FromBase64String(base64String);

                // Upload the file to blob storage
                string blobUrl = BlobStorage.UploadeSignedFile(fileBytes, pdfFilePath);
                Uri blobUri = new Uri(blobUrl);

                // Compute the SHA256 hash of the file
                string hash;
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    hash = string.Concat(sha256.ComputeHash(fileBytes).Select(x => x.ToString("X2")));
                }

                // If `approvedId` is valid, update approvals
                if (int.TryParse(approvedId, out int approvalId) && approvalId != 0)
                {
                    string agendaId = ASPSession.Current.AgendaID; // Retrieve agenda ID from session
                    string currentApprovalId = ASPSession.Current.ApprovalId; // Retrieve approval ID from session
                    UpdateApprovals(
                        Convert.ToInt32(agendaId),
                        Convert.ToString(CurrentSession.UserID),
                        pdfFilePath,
                        approvalId
                    );
                }

                Response.Headers.Add("Content-Disposition", "inline; filename=SignedPDF");
                return File(fileBytes, "application/pdf");
            }
            catch (FormatException)
            {
                return Json(new { success = false, message = "The provided Base64 string is not valid." }, JsonRequestBehavior.AllowGet);
            }
            catch (IOException ioEx)
            {
                System.Diagnostics.Debug.WriteLine($"IOException: {ioEx.Message}");
                return Json(new { success = false, message = "An error occurred while processing the file." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                return Json(new { success = false, message = "An unexpected error occurred. Please try again later." }, JsonRequestBehavior.AllowGet);
            }
        }

        public int SaveApprovals(int AgendaId, string UserId, string PDFpath)
        {
            // Initialize variables
            string MCName = "";
            DateTime MeetingDate = DateTime.MinValue;
            string MeetingTitle = "";
            string FilePath = "";
            string DeptID = "";
            int ApprovalId = 0;


            var approval = AgendaModelFunction.GetAgendaDetailsforApproval(AgendaId).FirstOrDefault();

            if (approval != null)
            {
                // Extract data from the single record
                MCName = approval.DepartmentName;
                MeetingDate = Convert.ToDateTime(approval.AgendaDate);
                MeetingTitle = Convert.ToString(approval.AgendaName);
                FilePath = Convert.ToString(PDFpath);
                DeptID = Convert.ToString(approval.DeptId);
            }

            // Save approval details if valid
            if (AgendaId != 0 && approval != null)
            {
                ApprovalId = AgendaModelFunction.SaveApproval(AgendaId, MCName, MeetingDate, MeetingTitle, FilePath, UserId, DeptID, CurrentSession.DeptID);
            }
            return ApprovalId;
        }

        public void UpdateApprovals(int AgendaId, string UserId, string PDFpath, int ApprovedId)
        {

            if (AgendaId != 0 && ApprovedId != 0)
            {
                AgendaModelFunction.UpdateApproval(AgendaId, PDFpath, UserId, ApprovedId);
            }

        }


        public static void SavePDf3(string AgendaId, string filepath, string fileHash)
        {
            if (AgendaId != "")
            {
                AgendaModelFunction.SaveAgendaPdf3(Convert.ToInt32(AgendaId), filepath, fileHash);
            }

        }

        public static void UpdatePdfCommissioner(int AgendaId, string filepath)
        {
            if (AgendaId != 0)
            {
                AgendaModelFunction.UpdatePdfCommissioner(Convert.ToInt32(AgendaId), filepath);
            }

        }

        public static int UpdatePdf(int agendaId, string filePath, string signType, string approvalId = "")
        {
            if (agendaId != 0)
            {
                int status = AgendaModelFunction.UpdatePdfPath(agendaId, filePath, signType, approvalId);
                return status; // returns DB update result
            }

            // Return 0 if AgendaId is 0 (indicates failure)
            return 0;
        }

        private void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        private void SaveEsignStateToCache(string txn)
        {
            if (string.IsNullOrEmpty(txn)) return;
            var eSignState = new Dictionary<string, object>();

            // Backup the entire ASP.NET Session manually to defeat HTTP localhost cookie drops
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                foreach (string key in System.Web.HttpContext.Current.Session.Keys)
                {
                    eSignState[key] = System.Web.HttpContext.Current.Session[key];
                }
            }

            // Explicitly store the unmanaged Appearance and Fout
            eSignState["_EsignAppearance"] = CurrentSession.Appearance;
            eSignState["_EsignFout"] = CurrentSession.Fout;

            System.Web.HttpRuntime.Cache.Insert("eSign_" + txn, eSignState, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        private void RestoreEsignStateFromCache(string txn)
        {
            if (string.IsNullOrEmpty(txn)) return;
            var eSignState = System.Web.HttpRuntime.Cache["eSign_" + txn] as Dictionary<string, object>;
            if (eSignState != null)
            {
                // Restore the entire HTTP Session dictionary into the NEWly tracking cookie
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
                {
                    foreach (var kvp in eSignState)
                    {
                        if (!kvp.Key.StartsWith("_Esign"))
                        {
                            System.Web.HttpContext.Current.Session[kvp.Key] = kvp.Value;
                        }
                    }
                }

                if (eSignState.ContainsKey("_EsignAppearance") && eSignState["_EsignAppearance"] != null)
                    ASPSession.Current.Appearance = (PdfSignatureAppearance)eSignState["_EsignAppearance"];
                if (eSignState.ContainsKey("_EsignFout") && eSignState["_EsignFout"] != null)
                    ASPSession.Current.Fout = (MemoryStream)eSignState["_EsignFout"];
            }
        }
    }
}
