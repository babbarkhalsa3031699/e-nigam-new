using CMNirdesh.Models;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

public class LGApprovalController : Controller
{
    // GET: Approval/List
    public ActionResult Index(int type=0)
    {
        string Type = CurrentSession.MenuId;
        string deptid = "";

        if (type == 2)
        {
            deptid = "UDD0001";
        }
        else if (type == 1)
        {
            deptid = "LG00001";
        }
        else
        {
            deptid = CurrentSession.DeptID;
        }
        string isDeptApex = CurrentSession.isDeptApex;
        string Office = CurrentSession.OfficeId;
        string UserId = CurrentSession.UserID;

        LGApproval model = new LGApproval
        {
            fileacessingUrl = BlobStorage.GetStorageAcessingPath(),
            LGApprovalList = LGApprovalDS.GetLGApprovalList(deptid, isDeptApex, Office, UserId)
        };

        DataSet menuds = UserModelFunction.Getmenutype_dashboard(Convert.ToInt32(type), Convert.ToInt32(CurrentSession.SubUserTypeID));
        if (menuds.Tables[0].Rows.Count > 0)
        {

            model.RefMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["DocumentTypeName"].ToString());
            model.RMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["MenuName"].ToString());

        }
        else
        {
            if (type == 2)
            {
                model.RefMenuName = "ADC Approvals";
            }
            else if (type == 1)
            {
                model.RefMenuName = "LG Approvals";
            }
            else
            {
                model.RefMenuName = "";
            }
        }
        return View(model);
    }


    public ActionResult LGApprovals(int type = 0)
    {
        string deptid = "";
        deptid = CurrentSession.DeptID;
        string isDeptApex = CurrentSession.isDeptApex;
        string Office = CurrentSession.OfficeId;
        string UserId = CurrentSession.UserID;

        LGApproval model = new LGApproval
        {
            fileacessingUrl = BlobStorage.GetStorageAcessingPath(),
            LGApprovalList = LGApprovalDS.GetLGsApprovalList(deptid, isDeptApex, Office, UserId)
        };

        DataSet menuds = UserModelFunction.Getmenutype_dashboard(Convert.ToInt32(type), Convert.ToInt32(CurrentSession.SubUserTypeID));
        if (menuds.Tables[0].Rows.Count > 0)
        {

            model.RefMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["DocumentTypeName"].ToString());
            model.RMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["MenuName"].ToString());

        }
        else
        {
            model.RefMenuName = "LG Approvals";
        }
        return View("Index", model);
    }

    public ActionResult ADCApprovals(int type = 0)
    {
        string deptid = "";
        deptid = CurrentSession.DeptID;
        string isDeptApex = CurrentSession.isDeptApex;
        string Office = CurrentSession.OfficeId;
        string UserId = CurrentSession.UserID;

        LGApproval model = new LGApproval
        {
            fileacessingUrl = BlobStorage.GetStorageAcessingPath(),
            LGApprovalList = LGApprovalDS.GetADCApprovalList(deptid, isDeptApex, Office, UserId)
        };

        DataSet menuds = UserModelFunction.Getmenutype_dashboard(Convert.ToInt32(type), Convert.ToInt32(CurrentSession.SubUserTypeID));
        if (menuds.Tables[0].Rows.Count > 0)
        {

            model.RefMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["DocumentTypeName"].ToString());
            model.RMenuName = Convert.ToString(menuds.Tables[0].Rows[0]["MenuName"].ToString());

        }
        else
        {
        model.RefMenuName = "ADC Approvals";
        }
        return View(model);
    }

    public ActionResult FileApprovals(string AgendaID)
    {
        if (string.IsNullOrEmpty(AgendaID))
        {
             return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Agenda ID is required.");
        }

        try
        {
            LGApproval model = new LGApproval
            {
                fileacessingUrl = BlobStorage.GetStorageAcessingPath(),
                LGApprovalList = LGApprovalDS.GetFileApprovalList(AgendaID)
            };

            return PartialView("_FileApprovals", model); // Specify the partial view explicitly for clarity
        }
        catch (Exception ex)
        {
            
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
        }
    }


    public ActionResult UnsingedApprovals(string AgendaID)
    {
        if (string.IsNullOrWhiteSpace(AgendaID))
        {
            return new HttpStatusCodeResult(
                HttpStatusCode.BadRequest,
                "Agenda ID is required."
            );
        }

        try
        {
            int agendaId;

            if (!int.TryParse(AgendaID, out agendaId))
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest,
                    "Invalid Agenda ID."
                );
            }

            LGApproval model = new LGApproval
            {
                fileacessingUrl = BlobStorage.GetStorageAcessingPath(),
                LGApprovalList = LGApprovalDS.GetUnSignedApprovalList(AgendaID)
            };

            if (model.LGApprovalList != null)
            {
                foreach (var item in model.LGApprovalList)
                {
                    item.TokenUrl = GetEncryptedSignUrl(
                                        agendaId,
                                        item.LGApprovalID,
                                        "Approval"
                                    );
                }
            }

            return PartialView("_UnsingedApprovals", model);
        }
        catch (Exception ex)
        {
            // Log exception here

            return new HttpStatusCodeResult(
                HttpStatusCode.InternalServerError,
                ex.Message
            );
        }
    }

    [HttpGet]
    public string GetEncryptedSignUrl(
     int AgendaId,
     int ApprovalId,
     string TypeofDocument)
    {
        var payload = $"{AgendaId}|{ApprovalId}|{TypeofDocument}";

        var encryptedToken = HttpUtility.UrlEncode(
            CryptoHelper.Encrypt(payload)
        );

        var url = Url.Action(
            "SignPdf",
            "References",
            new { token = encryptedToken }
        );

        return url;
    }


    public ActionResult FileSignedApprovals(string AgendaID)
    {
        if (string.IsNullOrEmpty(AgendaID))
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Agenda ID is required.");
        }

        try
        {
            LGApproval model = new LGApproval
            {
                fileacessingUrl = BlobStorage.GetStorageAcessingPath(),
                LGApprovalList = LGApprovalDS.GetFileSignedApprovalList(AgendaID)
            };

            return PartialView("_FileApprovals", model); // Specify the partial view explicitly for clarity
        }
        catch (Exception ex)
        {

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
        }
    }
}
