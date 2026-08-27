using CMNirdesh.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

public class CircularController : Controller
{
    // GET: Circular/Add
   
    [HttpGet]
    public ActionResult Add()
    {
        string isDeptApex = CurrentSession.isDeptApex;
        string deptid = CurrentSession.DeptID;

        var model = new Circular
        {
            mDepartmentList = DiaryDashboard.getMCWiseDepartmentList(),
            CircularList = CircularDS.GetCircularList(deptid, isDeptApex),
            fileacessingUrl = BlobStorage.GetStorageAcessingPath()
        };

        // Show success message if set
        if (TempData["SuccessMessage"] != null)
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
        }

        return View(model); // NEW model → empty form
    }

    public ActionResult Add_Mobile(string type = null, string email = null)
    {
        mUsers user = new mUsers();
        user.AadarId = "cs@punjab.gov.in";
        CurrentSession.AadharId = user.AadarId;

        user.mUserList = CMNirdesh.Models.Login.GetDetailsByadhaarID(email);

        if (user.mUserList.Count == 0)
        {
            TempData["IsRegister"] = "User is not Registered with e-Nigam";
            Session["LoggedIn"] = "no";
            CurrentSession.ServiceSessionID = "";
            return RedirectToAction("NotRegisterUser", "References");

        }
        else
        {
            if (user.mUserList != null && user.mUserList.Count > 0)
            {
                foreach (var list in user.mUserList)
                {
                    Session["LoggedIn"] = "yes";

                    email = list.UserName;
                    FormsAuthentication.SetAuthCookie(list.UserName, false);
                    CurrentSession.UserID = list.UserId.ToString();
                    CurrentSession.UserName = list.UserName;
                    CurrentSession.SubUserTypeID = list.UserType.ToString();
                    CurrentSession.IsMember = list.IsMember;
                    CurrentSession.OfficeId = list.OfficeId.ToString();
                    CurrentSession.OfficeLevel = list.OfficeLevel;
                    CurrentSession.OfficeId = list.OfficeId.ToString();
                    CurrentSession.isApex = list.isApex.ToString();
                    CurrentSession.DeptID = list.DeptId.ToString();
                    CurrentSession.Name = list.Name.ToString();
                    CurrentSession.Designation = list.Designation;
                    CurrentSession.DeptName = list.DepartmentName.ToString();



                }
            }


            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                Session["isApex"] = CurrentSession.isApex;



                string isDeptApex = CurrentSession.isApex;
                string deptid = CurrentSession.DeptID;
                Circular model = new Circular
                {
                    fileacessingUrl = BlobStorage.GetStorageAcessingPath(),
                    mDepartmentList = DiaryDashboard.getMCWiseDepartmentList(),
                    CircularList = CircularDS.GetCircularList(deptid, isDeptApex)
                };
                return View("Add_Mobile", model);

            }
        }

        //return View(model);
    }


    // POST: Circular/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Add(Circular model)
    {
        string isDeptApex = CurrentSession.isDeptApex;
        string deptid = CurrentSession.DeptID;

        try
        {
            model.DepartmentId = CurrentSession.DeptID;
            int circularId = 0;

            // FILE VALIDATION
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension != ".pdf")
                    {
                        ModelState.AddModelError("PDFFile", "Only PDF files are allowed.");
                    }
                    else if (file.ContentLength > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("PDFFile", "File size must be less than 5 MB.");
                    }
                    else
                    {
                        string fileName = Guid.NewGuid() + extension;
                        string filePath = "LG/Circulars/";
                        BlobStorage.Savefile(file, filePath, fileName);
                        model.PDFPath = filePath + fileName;
                        circularId = CircularDS.SaveRecord(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("PDFFile", "Please upload a valid PDF file.");
                }
            }
            else
            {
                ModelState.AddModelError("PDFFile", "Please upload a valid PDF file.");
            }

            // DEPARTMENT VALIDATION
            if (string.IsNullOrWhiteSpace(model.SelectedContacts))
            {
                ModelState.AddModelError("SelectedContacts", "Select at least one department.");
            }
            else
            {
                string[] departmentIds = model.SelectedContacts.Split(',');
                foreach (var departmentId in departmentIds)
                {
                    CircularDS.SaveCircularDetails(circularId, departmentId, true, model.DepartmentId);
                }
            }
            // SUCCESS → REDIRECT TO GET 'Add' TO CLEAR MODEL
            TempData["SuccessMessage"] = "Circular added successfully!";
            model.Subject = null;
            return RedirectToAction("Add"); // PRG Pattern
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
            model.fileacessingUrl = BlobStorage.GetStorageAcessingPath();
            model.mDepartmentList = DiaryDashboard.getMCWiseDepartmentList();
            model.CircularList = CircularDS.GetCircularList(deptid, isDeptApex);
            ViewBag.ShowModal = true; // <-- show modal in case of error
            return View(model);
        }
    }



    // Test action (likely for development or testing purposes)

    public ActionResult Deactivate(int id, string MCCode)
    {
        bool success = CircularDS.DeactivateRecord(id, MCCode);

        if (success)
        {
            TempData["Message"] = "Record deactivated successfully.";
        }
        else
        {
            TempData["Error"] = "Failed to deactivate the record.";
        }

        return RedirectToAction("Add");  // Redirect to a page to show the updated list
    }
}
