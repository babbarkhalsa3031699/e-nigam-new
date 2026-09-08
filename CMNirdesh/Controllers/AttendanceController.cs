using CMNirdesh.Models;
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class AttendanceController : Controller
    {
        // GET: Attendance
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                string deptID = CurrentSession.DeptID;
                MeetingAttendanceMC mAttendanceMC = new MeetingAttendanceMC();
                ViewBag.MCName = Convert.ToString(CurrentSession.MCName);
                //mAttendanceMC.MembersList = AttendenceList.GetMembersList(Convert.ToInt32(CurrentSession.StateId));
                mAttendanceMC.AgendaList = AttendenceList.GetMeetingListforAttendance(deptID.ToString());
                return View(mAttendanceMC);
            }
        }

        public ActionResult GetData(int Id)
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                int OfficeId = Convert.ToInt32(CurrentSession.OfficeId);
                int MCid = Convert.ToInt32(CurrentSession.StateId);
                MeetingAttendanceMC mAttendanceMC = new MeetingAttendanceMC();
                ViewBag.MCName = Convert.ToString(CurrentSession.MCName);

                string officetype = string.Empty;
                officetype = AgendaModelFunction.GetOfficeType(Id);

                if (officetype == "FNCC")
                {
                    var note = AttendenceList.GetNotes(Id);

                    var FNCCMembers = note?.Where(n => n.CategoryName == "FNCC Members").ToList();
                    mAttendanceMC.AddedFNCCMembers = FNCCMembers;
                    //var EndorsementNotes = note?.Where(n => n.CategoryName == "Endorsement").ToList();

                    //foreach (var note1 in FNCCMembers)
                    //{
                    //    outXml += @"<li>" + Convert.ToString(note1.NoteText) + "</li>";
                    //}
                }




                mAttendanceMC.AgendaId = Id;
                mAttendanceMC.HouseCode = officetype;
                mAttendanceMC.MembersList = AttendenceList.GetMCMembersList(Convert.ToInt32(Id), Convert.ToInt32(MCid));
                mAttendanceMC.FNCCMembersList = AttendenceList.GetFNCCMembersList(Convert.ToInt32(Id), Convert.ToInt32(MCid));
                return PartialView("GetData", mAttendanceMC);
            }
        }


        [HttpPost]
        public JsonResult MarkAttendenceAll(int Id)
        {
            int mccode= Convert.ToInt32(CurrentSession.StateId);
            int data= AttendenceList.AddAttendenceAll(Id, mccode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClearAttendenceAll(int Id)
        {
            //int mccode = Convert.ToInt32(CurrentSession.StateId);
            int data = AttendenceList.ClearAttendenceAll(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MarkAttendenceMember(int Id, int MemberCode)
        {
            int mccode = Convert.ToInt32(CurrentSession.StateId);
            int data = AttendenceList.AddAttendenceMember(Id, MemberCode,mccode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClearAttendenceMember(int Id, int MemberCode)
        {
            int mccode = Convert.ToInt32(CurrentSession.StateId);
            int data = AttendenceList.ClearAttendenceMember(mccode, Id, MemberCode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MarkAttendenceFNCC(int Id, string Userid)
        {
            int mccode = Convert.ToInt32(CurrentSession.StateId);
            int data = AttendenceList.AddAttendenceFNCC(Id, Userid, mccode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClearAttendenceFNCC(int Id, string Userid)
        {
            int mccode = Convert.ToInt32(CurrentSession.StateId);
            int data = AttendenceList.ClearAttendenceFNCC( Id, Userid, mccode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadAttendanceDocument(int meetingId)
        {
            var model = new UploadAttendanceViewModel
            {
                MeetingId = meetingId,
                fileacessingUrl = BlobStorage.GetStorageAcessingPath(),
                Documents = AttendenceList.GetAttendanceDocuments(meetingId)
            };
            //return PartialView(model);
            return PartialView("UploadAttendanceDocument", model);
        }

        //[HttpPost]
        //public JsonResult UploadAttendanceDocuments(int meetingId)
        //{
        //    try
        //    {
        //        var file = Request.Files["attendanceFile"];
        //        if (file != null && file.ContentLength > 0)
        //        {

        //            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
        //            string extension = Path.GetExtension(file.FileName);
        //            string uniqueFileName = $"{fileName}_{Guid.NewGuid().ToString("N").Substring(0, 6)}{extension}";
        //            var folderpath = "MC/" + CurrentSession.MCName + "/UploadedAttendanceDocs/";
        //            PDFCoverList.SavefileSolution(file, folderpath, uniqueFileName);


        //            var doc = new AttendanceDocument
        //            {
        //                AgendaId = meetingId,
        //                FileName = uniqueFileName,
        //                FilePath = folderpath,
        //                UploadedBy = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous"
        //            };

        //            AttendenceList.InsertAttendanceDocument(doc);

        //            return Json(new { success = true, message = "File uploaded successfully." });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "No file selected." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}


        [HttpPost]
        public JsonResult UploadAttendanceDocuments(int meetingId)
        {
            try
            {
                var file = Request.Files["attendanceFile"];
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);

                    string uniqueFileName = $"{fileName}_{Guid.NewGuid().ToString("N").Substring(0, 6)}{extension}";
                    var folderpath = "MC/" + CurrentSession.MCName + "/UploadedAttendanceDocs/";
                    BlobStorage.Savefile(file, folderpath, uniqueFileName);
                    string blobPath = folderpath + uniqueFileName;
                    var doc = new AttendanceDocument
                    {
                        AgendaId = meetingId,
                        FileName = uniqueFileName,
                        FilePath = folderpath, 
                        UploadedBy = CurrentSession.UserName
                    };
                    AttendenceList.InsertAttendanceDocument(doc);
                    return Json(new { success = true, message = "File uploaded successfully.", path = blobPath });
                }
                else
                {
                    return Json(new { success = false, message = "No file selected." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public PartialViewResult ListAttendanceDocuments(int meetingId)
        {
           var model = new UploadAttendanceViewModel
            {
                MeetingId = meetingId,
                Documents = AttendenceList.GetAttendanceDocuments(meetingId)
            };
            return PartialView("_AttendanceDocumentList", model);
        }


        public ActionResult AddNote(int meetingId)
        {
            var model = new AgendaNoteViewModel
            {
                MeetingId = meetingId,
                AgendaNoteList = AttendenceList.GetNotes(meetingId),
                Categories =  AttendenceList.GetNoteCategories()
            };
           return PartialView("AddNote", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveNote(AgendaNote model)
        {
            if (model == null || model.MeetingId == 0)
                return Json(new { success = false, message = "Invalid meeting." });
            var username = Convert.ToString(CurrentSession.UserID);
            AttendenceList.InsertNote(model, username);
            var notesList = AttendenceList.GetNotes(model.MeetingId); // List<AgendaNote>
            var viewModel = new AgendaNoteViewModel
            {
                MeetingId = model.MeetingId,
                AgendaNoteList = notesList
            };
            var html = RenderPartialViewToString("_NotesList", viewModel);
            return Json(new { success = true, notesHtml = html });
        }

        [HttpPost]
        public JsonResult UpdateNote(AgendaNoteViewModel model)
        {
            if (model == null || model.MeetingId == 0)
                return Json(new { success = false, message = "Invalid meeting." });
            var username = Convert.ToString(CurrentSession.UserID);
           // AttendenceList.Update(model, username);
            var notesList = AttendenceList.GetNotes(model.MeetingId); // List<AgendaNote>
            var viewModel = new AgendaNoteViewModel
            {
                MeetingId = model.MeetingId,
                AgendaNoteList = notesList
            };
            var html = RenderPartialViewToString("_NotesList", viewModel);
            return Json(new { success = true, notesHtml = html });
        }

        [HttpPost]
        public JsonResult DeleteNote(int noteId, int agendaId)
        {
            if (agendaId == 0)
                return Json(new { success = false, message = "Invalid meeting." });
            var username = Convert.ToString(CurrentSession.UserID);
            AttendenceList.DeleteNote(noteId, agendaId, username);
            var notesList = AttendenceList.GetNotes(agendaId); // List<AgendaNote>
            var viewModel = new AgendaNoteViewModel
            {
                MeetingId = agendaId,
                AgendaNoteList = notesList
            };
            var html = RenderPartialViewToString("_NotesList", viewModel);
            return Json(new { success = true, notesHtml = html });
        }



        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                if (viewResult.View == null)
                {
                    throw new ArgumentException($"Partial view '{viewName}' not found.");
                }
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult TestFontChange()
        {
            return View("TestFontChange");
        }


    }
}