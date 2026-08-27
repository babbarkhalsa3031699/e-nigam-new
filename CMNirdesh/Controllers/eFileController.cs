using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using CMNirdesh.Models;
using Newtonsoft.Json;
using Syncfusion.Pdf.Parsing;
//using NPOI.XWPF.UserModel;

namespace CmNirdesh.eFile
{

    public class eFileController : Controller
    {
        //
        // GET: /eFile/eFile/


        public ContentResult UploadMlaEnclosureFiles()
        {
            string url = "~/TempFile";
            string directory = Server.MapPath(url);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            string[] filePaths = Directory.GetFiles(directory);


            var r = new List<eFileAttachment>();
            var NFileName = "";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;

                if (hpf.ContentLength == 0)
                    continue;
                var fileName1 = Path.GetFileName(hpf.FileName);
                string ext = Path.GetExtension(hpf.FileName);
                NFileName = Guid.NewGuid().ToString() + ext;

                string savedFileName = Path.Combine(Server.MapPath("~/TempFile"), NFileName);
                hpf.SaveAs(savedFileName);

                string path = "/TempFile";
                path = Path.Combine(path, NFileName);
                r.Add(new eFileAttachment()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType,
                    eFilePath = path


                });

            }
            CurrentSession.IsUploadPdfFile = "Y";
            //  return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} KB", Convert.ToInt32(r[0].Length / 1024)) + "\"}", "application/json");
            return Content("{\"name\":\"" + NFileName + "\",\"pdfname\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} KB", Convert.ToInt32(r[0].Length / 1024)) + "\"}", "application/json");


        }        
        public ContentResult UploadMlaActionFiles()
        {
            string url = "~/TempFile";
            string directory = Server.MapPath(url);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            string[] filePaths = Directory.GetFiles(directory);


            var r = new List<eFileAttachment>();
            var NFileName = "";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;

                if (hpf.ContentLength == 0)
                    continue;
                var fileName1 = Path.GetFileName(hpf.FileName);
                string ext = Path.GetExtension(hpf.FileName);
                NFileName = Guid.NewGuid().ToString() + ext;

                string savedFileName = Path.Combine(Server.MapPath("~/TempFile"), NFileName);
                hpf.SaveAs(savedFileName);

                //string path = System.Web.Configuration.WebConfigurationManager.AppSettings["SiteUrl"] + "/ActionTempFile";
                string path =  "/TempFile";
                path = Path.Combine(path, NFileName);
                r.Add(new eFileAttachment()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType,
                    eFilePath = path


                });

            }
            CurrentSession.IsUploadPdfFile = "Y";
            //  return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} KB", Convert.ToInt32(r[0].Length / 1024)) + "\"}", "application/json");
            return Content("{\"name\":\"" + NFileName + "\",\"pdfname\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} KB", Convert.ToInt32(r[0].Length / 1024)) + "\"}", "application/json");

        }


        public ContentResult UploadAnnexure()
        {
            string url = "~/References";
            string directory = Server.MapPath(url);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            string[] filePaths = Directory.GetFiles(directory);


            var r = new List<eFileAttachment>();
            var NFileName = "";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;

                if (hpf.ContentLength == 0)
                    continue;
                var fileName1 = Path.GetFileName(hpf.FileName);
                string ext = Path.GetExtension(hpf.FileName);
                NFileName = Guid.NewGuid().ToString() + ext;

                string savedFileName = Path.Combine(Server.MapPath(url), NFileName);
                //hpf.SaveAs(savedFileName);
                string url1 = "References/";
                BlobStorage.Savefile(hpf, url1, NFileName);
                string path = "/References";
                path = Path.Combine(path, NFileName);
                r.Add(new eFileAttachment()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType,
                    eFilePath = path
                });
            }
            CurrentSession.IsUploadPdfFile = "Y";
            //  return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} KB", Convert.ToInt32(r[0].Length / 1024)) + "\"}", "application/json");
            return Content("{\"name\":\"" + NFileName + "\",\"pdfname\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} KB", Convert.ToInt32(r[0].Length / 1024)) + "\"}", "application/json");


        }

        public ContentResult UploadFundFiles()
        {
            string url = "~/MlaDiary/TempFile";
            string directory = Server.MapPath(url);
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            if (Directory.Exists(directory))
            {
                string[] filePaths = Directory.GetFiles(directory);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
            }

            var r = new List<eFileAttachment>();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;

                if (hpf.ContentLength == 0)
                    continue;
                string savedFileName = Path.Combine(Server.MapPath("~/MlaDiary/TempFile"), Path.GetFileName(hpf.FileName));
                hpf.SaveAs(savedFileName);

                r.Add(new eFileAttachment()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType,
                    eFilePath = savedFileName

                });

            }
            CurrentSession.IsUploadPdfFile = "Y";
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} KB", Convert.ToInt32(r[0].Length / 1024)) + "\"}", "application/json");
        }
    }
        public class eFileAttachment
    {
        public int eFileAttachmentId { get; set; }
        public int eFileID { get; set; }
        public int DocumentTypeId { get; set; }
        public string eFileName { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool SendStatus { get; set; }
        public string DepartmentId { get; set; }
        [NotMapped]
        public string AllDeptName { get; set; }
        public string Title { get; set; }
        public Guid eFileNameId { get; set; }
        public string Type { get; set; }
        public int ParentId { get; set; }
        public int SplitFileCount { get; set; }
        public int ReportLayingHouse { get; set; }
        public int Approve { get; set; }
        [NotMapped]
        public string mode { get; set; }
        [NotMapped]
        public List<int> ImageCount { get; set; }
        public int OfficeCode { get; set; }
        public int PaperNature { get; set; }
        public string PaperNatureDays { get; set; }
        public string ToDepartmentID { get; set; }
        public int ToOfficeCode { get; set; }
        public string PaperStatus { get; set; }
        public DateTime SendDate { get; set; }
        public string PType { get; set; }
        public string eFilePath { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }

        public DateTime ReceivedDate { get; set; }
        public string PaperRefNo { get; set; }
        public List<string> PaperRefNoList { get; set; }
        public string ReplyStatus { get; set; }
        public DateTime ReplyDate { get; set; }

        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public int Length { get; set; }
        [NotMapped]
        public int[] eFileAttachmentIds { get; set; }

        public string LinkedRefNo { get; set; }
        public int ToPaperNature { get; set; }
        public string ToPaperNatureDays { get; set; }
        public string RevedOption { get; set; }
        public string RecvDetails { get; set; }
        public string ToRecvDetails { get; set; }
        public int eMode { get; set; }
        [NotMapped]
        public string DocuemntPaperType { get; set; }

        public int CommitteeId { get; set; }
        [NotMapped]
        public string CommitteeName { get; set; }
        [NotMapped]
        public string RecvdPaperNature { get; set; }

        public string PaperTicketValue { get; set; }
        public string PaperNo { get; set; }
        public string DispatchtoWhom { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public string DeptAbbr { get; set; }
        [NotMapped]
        public string ToDeptAbbr { get; set; }
        [NotMapped]
        public string ToDepartmentName { get; set; }

        public string eFilePathWord { get; set; }

        public int ReFileID { get; set; }

        [NotMapped]
        public string MainEFileName { get; set; }
        public string DiaryNo { get; set; }
        [NotMapped]
        public string Year { get; set; }
        public int CurYear { get; set; }
        public int CurDNo { get; set; }
        public int CurrentBranchId { get; set; }
        public string CurrentEmpAadharID { get; set; }
        public DateTime? AssignDateTime { get; set; }
        [NotMapped]
        public string BranchName { get; set; }
        [NotMapped]
        public string EmpName { get; set; }
        [NotMapped]
        public List<string> DeptIds { get; set; }
        [NotMapped]
        public string ToListComplN { get; set; }
        [NotMapped]
        public string CCListComplN { get; set; }
        [NotMapped]
        public string ToMemIds { get; set; }
        [NotMapped]
        public string ToDepIds { get; set; }
        [NotMapped]
        public string ToOtherIds { get; set; }
        [NotMapped]
        public string CCMemIds { get; set; }
        [NotMapped]
        public string CCDepIds { get; set; }
        [NotMapped]
        public string CCOtherIds { get; set; }
        [NotMapped]
        public string ToListComplIds { get; set; }
        [NotMapped]
        public string CCListComplIds { get; set; }
        [NotMapped]
        public string eFilePathPDFN { get; set; }
        [NotMapped]
        public string eFilePathWordN { get; set; }

        public int? DraftId { get; set; }
        public int? ToMemberId { get; set; }
        public int? ToOtherId { get; set; }
        public string TOCCtype { get; set; }

        [NotMapped]
        public string Desingnation { get; set; }
        [NotMapped]
        public string AbbrevBranch { get; set; }
        [NotMapped]
        public string ToAllDraft { get; set; }
        [NotMapped]
        public string CCAllDraft { get; set; }
        [NotMapped]
        public string SignFilePath { get; set; }
        public string AnnexPdfPath { get; set; }
        public int? CountsAPS { get; set; }
        [NotMapped]
        public string DraftedBy { get; set; }
        [NotMapped]
        public int ItemTypeName { get; set; }
        [NotMapped]
        public string ValueType { get; set; }
        [NotMapped]
        public int YearId { get; set; }
        [NotMapped]
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public string ItemNumber { get; set; }
        public string ReplyRefNo { get; set; }
        [NotMapped]
        public string ItemSubject { get; set; }
        [NotMapped]
        public string ItemDescription { get; set; }
        [NotMapped]
        public int CountRply { get; set; }
        public int creDraftID { get; set; }
        public bool IsVerified { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        [NotMapped]
        public string IsPdf { get; set; }
        [NotMapped]
        public string IsWord { get; set; }
        public string PaperRefrenceManualy { get; set; }
        //[UIHint("tinymce_jquery_full_LOB"), AllowHtml]
        //public string PaperDraft { get; set; }
        [NotMapped]
        public string ComAbbr { get; set; }
        [NotMapped]
        public string AnnexPDF { get; set; }
        public bool IsBackLog { get; set; }
        public bool IsRejected { get; set; }
        public string RejectedRemarks { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        [NotMapped]
        public string IsDataRead { get; set; }
        [NotMapped]
        public string SettingValue_file { get; set; }
        [NotMapped]
        public string eFileNumber { get; set; }
        [NotMapped]
        public List<eFileAttachment> DocDatalist { get; set; }
        //For Letter/Correspondence


    }
}