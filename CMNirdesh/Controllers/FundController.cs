
using CMNirdesh.Models;
using CMNirdesh.Models.Session;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using System.IO;
using CMNirdesh.Filters;
using CMNirdesh.Error;
using ErrorLog = CMNirdesh.Error.ErrorLog;
using System.Linq;

namespace CMNirdesh.Controllers
{
    [Audit]
    [NoCache]
    [CMNirdeshAuthorize(Allow = "Authenticated")]
    public class FundController : Controller
    {


        public ActionResult FundReport(int year = 0)
        {
            List<FundReportModel> lstfund = new List<FundReportModel>();
            LstFundReportModel model = new LstFundReportModel();
            try
            {
                if (year == 0)
                {
                    year = GetFinYear();
                }
                DataSet ds = new DataSet();
                string userid = CurrentSession.UserID;
                ds = FundFunctionModel.GetFundAvailable(year,userid);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    FundReportModel fr = new FundReportModel();
                    fr.FundTypeId = Convert.ToInt32(dr["DocTypeId"]);
                    fr.FundType = Convert.ToString(dr["FundType"]);
                    fr.FundAllocated = Convert.ToInt32(dr["FundAllocated"]);
                    fr.FundApproved = Convert.ToInt32(dr["ApprovedAmount"]);
                    fr.FundSanctioned = Convert.ToInt32(dr["SanctionedAmount"]);
                    fr.FundAvaiable = Convert.ToInt32(dr["FundAllocated"]) - Convert.ToInt32(dr["ApprovedAmount"]);
                    fr.DeptName = Convert.ToString(dr["deptname"]);
                    lstfund.Add(fr);
                }
                model._LstFundReport = lstfund;
                model.FilterFinYear = year;
                ViewBag.OfficeName = CurrentSession.OfficeName;
                model.FinYearList = GetYears();
                return View(model);

            }

            catch (Exception ex)
            {
                Error.ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return View(model);
            }

        }



        public ActionResult GetSanctionDetails(int docId, int FinYear)
        {

            List<SanctionDetailModel> lstsan = new List<SanctionDetailModel>();

            DataSet ds = new DataSet();
            ds = FundFunctionModel.GetSanctionDetails(docId, FinYear);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                SanctionDetailModel fr = new SanctionDetailModel();

                fr.DocmentType = Convert.ToInt32(dr["DocmentType"]);
                fr.DocumentTypeName = Convert.ToString(dr["DocumentTypeName"]);
                fr.AmountApproved = Convert.ToInt32(dr["ApprovedAmount"]);
                fr.AmountSanctioned = Convert.ToInt32(dr["AmountSanctioned"]);
                fr.SanctionDate = Convert.ToDateTime(dr["ApproveDate"]).ToString("dd/MM/yyyy");
                fr.RefId = Convert.ToInt64(dr["RefId"]);
                fr.Dept = Convert.ToString(dr["deptname"]);
                fr.Subject = Convert.ToString(dr["Subject"]);
                fr.SubjectDetails = Convert.ToString(dr["ActionDescription"]);
                fr.Constituency = Convert.ToString(dr["officename"]);
                lstsan.Add(fr);
            }


            return Json(lstsan, JsonRequestBehavior.AllowGet);




        }




        public ActionResult FundReceipt(int year = 0)
        {

            List<FundsReceiptModel> lstfund = new List<FundsReceiptModel>();
            LstFundsReceipt model = new LstFundsReceipt();
            int totalamount = 0;
            try
            {
                if (year == 0)
                {
                    year = GetFinYear();
                }
                DataSet dsdoc = new DataSet();
                string userid = CurrentSession.UserID;
                model._Lstdoc = FundFunctionModel.GetDocumentTypeList(userid);

                DataSet ds = new DataSet();
                ds = FundFunctionModel.GetReceiptFunds(year, userid);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    FundsReceiptModel fr = new FundsReceiptModel();

                    fr.id = Convert.ToInt32(dr["id"]);
                    fr.FinYear = Convert.ToInt32(dr["FinYear"]);
                    fr.FundName = Convert.ToString(dr["DocumentTypeName"]);
                    fr.FundsReceipt = Convert.ToInt32(dr["FundsReceipt"]);
                    fr.AllocationDate = Convert.ToDateTime(dr["AllocationDate"]).ToString("dd/MM/yyyy");
                    fr.Remarks = Convert.ToString(dr["Remarks"]);
                    fr.attachedfile = Convert.ToString(dr["attachedfile"]);
                    lstfund.Add(fr);
                    totalamount = totalamount + Convert.ToInt32(dr["FundsReceipt"]);
                }

                //FundsReceiptModel fr1 = new FundsReceiptModel();
                model._LstFundsReceipt = lstfund;
                model.FilterFinYear = year;
                model.TotalAmount = totalamount;
                model.FinYearList = GetYears() ;
                ViewBag.OfficeName = CurrentSession.OfficeName;

                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }



        private SelectList GetYears()
        {
            int CurrentYear = GetFinYear();
            List<SelectListItem> ddlYears = new List<SelectListItem>();

            for (int i = CurrentYear; i >= 2020; i--)
            {
                ddlYears.Add(new SelectListItem
                {
                    Text = i.ToString() + "-"+ (i+1).ToString(),
                    Value = i.ToString()
                });
            }

            //Default It will Select Current Year  
            return new SelectList(ddlYears, "Value", "Text",0);

        }


        public ActionResult SaveFundReceipt(LstFundsReceipt mdl)
        {
            LstFundsReceipt model = new LstFundsReceipt();
            FundsReceiptModel fr = new FundsReceiptModel();
            List<FundsReceiptModel> lstfund = new List<FundsReceiptModel>();


            //upload action file 
            if (model.IsFile == "Y")
            {
                model.attachfile = model.attachfile;
            }
            else
            {
                //var FileSettings = SBL.eLegistrator.HouseController.Web.Models.FileSetting.GetDISFileSetting();
                string urlA = "/Fund/";
                //string urlA = "/MlaDiary/ActionFile/" + CurrentSession.AssemblyId + "/" + CurrentSession.MemberCode + "/" + model.DocmentType + "/";
                //string directoryA = FileSettings.SettingValue + urlA;
                string directoryA = "~/MlaDiary/Fund/";
                directoryA = Server.MapPath(directoryA);
                if (!System.IO.Directory.Exists(directoryA))
                {
                    System.IO.Directory.CreateDirectory(directoryA);
                }
                // int fileID = GetFileRandomNo();

                string tempurlA1 = "~/MlaDiary/TempFile";
                string tempdirectoryA1 = Server.MapPath(tempurlA1);
                //Save ActionTaken File
                if (!System.IO.Directory.Exists(tempdirectoryA1))
                {
                    System.IO.Directory.CreateDirectory(tempdirectoryA1);
                }
                if (Directory.Exists(directoryA))// && (model.ActiontakenDate != null ))
                {
                    string[] savedFileName = Directory.GetFiles(Server.MapPath(tempurlA1));
                    if (savedFileName.Length > 0)// && (model.ActiontakenDate != null ))   //&& model.ActiontakenDate != null && model.ActiontakenDate != ""
                    {
                        string SourceFile = savedFileName[0];
                        foreach (string page in savedFileName)
                        {

                            // string path1 = System.IO.Path.Combine(directory, FileName1 + "_" + nametwo.Replace(" ", ""));

                            string ext = Path.GetExtension(SourceFile);
                            string name = Path.GetFileName(page);
                            // string path1 = System.IO.Path.Combine(directoryA, model.RecordNumber.ToString() + ext);
                            var fname = Guid.NewGuid().ToString();
                            string path1 = System.IO.Path.Combine(directoryA, fname + ext);
                            if (!string.IsNullOrEmpty(name))
                            {
                                //  System.IO.File.Delete(System.IO.Path.Combine(directoryA, model.ActiontakenFile));
                                System.IO.File.Copy(SourceFile, path1, true);
                                model.attachfile = urlA + fname + ext;
                                mdl.attachfile = urlA + fname + ext;
                            }
                        }


                    }
                    else
                    {
                        model.attachfile = null;
                        //   return Json("Please select Action Taken file", JsonRequestBehavior.AllowGet);
                        //  TempData["Msg"] = "Please select Action Taken file";
                        // return RedirectToAction("Index");
                    }
                }
                else
                {
                    model.attachfile = null;
                }
                if (Directory.Exists(tempdirectoryA1))   //tempurlA1
                {
                    string[] filePaths = Directory.GetFiles(Server.MapPath(tempurlA1));
                    foreach (string filePath in filePaths)
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

            }


            fr.FinYear = Convert.ToInt32(mdl.FinYear);
            fr.DocTypeId = mdl.DocTypeId;
            fr.FundsReceipt = mdl.FundAmount;
            fr.AllocationDate = mdl.AllocationDate.ToString();
            fr.DeptId = CurrentSession.DeptID;
            fr.OfficeId = CurrentSession.OfficeId;
            fr.Remarks = mdl.Remarks;
            fr.Createdby = CurrentSession.Name + ", " + CurrentSession.Designation + ", " + CurrentSession.OfficeName;
            fr.attachedfile = mdl.attachfile;
            FundFunctionModel.SaveFund(fr);
            ModelState.Clear();

            DataSet dsdoc = new DataSet();
            string userid = CurrentSession.UserID;
            model._Lstdoc = FundFunctionModel.GetDocumentTypeList(userid);

            DataSet ds = new DataSet();
            ds = FundFunctionModel.GetReceiptFunds(GetFinYear(),userid);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                FundsReceiptModel frmodel = new FundsReceiptModel();

                frmodel.id = Convert.ToInt32(dr["id"]);
                frmodel.FinYear = Convert.ToInt32(dr["FinYear"]);
                frmodel.FundName = Convert.ToString(dr["DocumentTypeName"]);
                frmodel.FundsReceipt = Convert.ToInt32(dr["FundsReceipt"]);
                frmodel.AllocationDate = Convert.ToDateTime(dr["AllocationDate"]).ToString("dd/MM/yyyy");
                frmodel.Remarks = Convert.ToString(dr["Remarks"]);
                frmodel.attachedfile = Convert.ToString(dr["attachedfile"]);
                lstfund.Add(frmodel);
            }
            model._LstFundsReceipt = lstfund;
            model.FinYearList = GetYears();

            return View("FundReceipt", model);
        }




        public ActionResult MLAFund(int year = 0)
        {

            List<FundReportModel> lstfund = new List<FundReportModel>();
            LstFundReportModel model = new LstFundReportModel();
            try
            {
                if (year == 0)
                {
                    year = GetFinYear();
                }
                DataSet ds = new DataSet();
                ds = FundFunctionModel.GetMLAFund(year, "MLA0001");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    FundReportModel fr = new FundReportModel();

                    fr.FundTypeId = Convert.ToInt32(dr["DocTypeId"]);
                    fr.FundType = Convert.ToString(dr["FundType"]);
                    fr.FundApproved = Convert.ToInt32(dr["FundApproved"]);
                    fr.FundSanctioned = Convert.ToInt32(dr["FundSanctioned"]);
                    fr.OfficeName = Convert.ToString(dr["officename"]);
                    fr.OfficeId = Convert.ToInt32(dr["officeid"]);
                    fr.DeptName = Convert.ToString(dr["deptname"]);

                    lstfund.Add(fr);
                }


                model._LstFundReport = lstfund;
                model.FilterFinYear = year;
                ViewBag.OfficeName = CurrentSession.OfficeName;
                ViewBag.ReportName = "MLA";
                ViewBag.HeaderName = "MLA";
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }


        public ActionResult DistrictFund(int year = 0)
        {

            List<FundReportModel> lstfund = new List<FundReportModel>();
            LstFundReportModel model = new LstFundReportModel();
            try
            {
                if (year == 0)
                {
                    year = GetFinYear();
                }

                DataSet ds = new DataSet();
                ds = FundFunctionModel.GetMLAFund(year, "HPD0054");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    FundReportModel fr = new FundReportModel();

                    fr.FundTypeId = Convert.ToInt32(dr["DocTypeId"]);
                    fr.FundType = Convert.ToString(dr["FundType"]);
                    fr.FundApproved = Convert.ToInt32(dr["FundApproved"]);
                    fr.FundSanctioned = Convert.ToInt32(dr["FundSanctioned"]);
                    fr.OfficeName = Convert.ToString(dr["officename"]);
                    fr.OfficeId = Convert.ToInt32(dr["officeid"]);
                    fr.DeptName = Convert.ToString(dr["deptname"]);

                    lstfund.Add(fr);
                }


                model._LstFundReport = lstfund;
                model.FilterFinYear = year;
                ViewBag.OfficeName = CurrentSession.OfficeName;
                ViewBag.ReportName = "District";
                ViewBag.HeaderName = "Office";
                return View(model);

            }

            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");

                return View(model);
            }

        }


        public ActionResult GetMLASanctionDetails(int docId, int officeid)
        {

            List<SanctionDetailModel> lstsan = new List<SanctionDetailModel>();

            DataSet ds = new DataSet();
            ds = FundFunctionModel.GetMLASanctionDetails(docId, GetFinYear(), officeid);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                SanctionDetailModel fr = new SanctionDetailModel();

                fr.DocmentType = Convert.ToInt32(dr["DocmentType"]);
                fr.DocumentTypeName = Convert.ToString(dr["DocumentTypeName"]);
                fr.AmountApproved = Convert.ToInt32(dr["ApprovedAmount"]);
                fr.AmountSanctioned = Convert.ToInt32(dr["AmountSanctioned"]);
                fr.SanctionDate = Convert.ToDateTime(dr["ApproveDate"]).ToString("dd/MM/yyyy");
                fr.RefId = Convert.ToInt64(dr["RefId"]);
                fr.Dept = Convert.ToString(dr["deptname"]);
                fr.Subject = Convert.ToString(dr["Subject"]);
                fr.SubjectDetails = Convert.ToString(dr["ActionDescription"]);
                fr.Constituency = Convert.ToString(dr["officename"]);
                lstsan.Add(fr);
            }


            return Json(lstsan, JsonRequestBehavior.AllowGet);




        }


        public int GetFinYear()
        {
            string finyear = "0";
            DateTime today = DateTime.Today;
            if (today.Month <= 3)
            {
                finyear = today.AddYears(-1).ToString("yyyy");
            }
            else
            {
                finyear = today.ToString("yyyy");
            }

            return Convert.ToInt32(finyear);
        }
    }
}
