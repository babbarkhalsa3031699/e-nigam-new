using CMNirdesh.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class AuthorityController : Controller
    {
        /// <summary>
        /// Loads the list of authorities.
        /// </summary>
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            AuthorityList model = new AuthorityList
            {
                _LstAuthority = AuthorityModel.AuthorityList(),
                fileacessingUrl = BlobStorage.GetStorageAcessingPath()
            };

            return View(model);
        }

        /// <summary>
        /// Loads the create authority form.
        /// </summary>
        public ActionResult Create()
        {
            return View(new Authority
            {
                IsActive = true,
                IsDeleted = false,
                CreationDate = DateTime.Now
            });
        }

        /// <summary>
        /// Saves a new authority along with photo upload to blob storage.
        /// </summary>
        [HttpPost]
        public JsonResult Create(Authority model)
        {
            try
            {
                // Handle photo upload
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    var ext = Path.GetExtension(file.FileName);
                    var fileName = $"AuthorityPhoto_{DateTime.Now:MMddyyyy_HHmmss_fff}.{ext}";
                    var folderPath = "LG/Authorities/";

                    BlobStorage.Savefile(file, folderPath, fileName);

                    model.FileName = fileName;
                    model.FilePath = folderPath;
                    model.Photo = folderPath + fileName;
                }

                // Set metadata
                model.CreationDate = DateTime.Now;
                model.IsActive = true;
                model.IsDeleted = false;

                var result = AuthorityModel.SaveAuthority(model);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Optionally log exception here
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Fetches authority details by ID for editing.
        /// </summary>
        public JsonResult Edit(int id)
        {
            var authorityData = AuthorityModel.GetAuthorityById(id);
            var model = new AuthorityList
            {
                _LstAuthority = authorityData
            };

            string json = JsonConvert.SerializeObject(model, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates authority details, including optional photo upload.
        /// </summary>
        [HttpPost]
        public JsonResult Update(Authority model)
        {
            try
            {
                // Handle photo update
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        var fileName = $"AuthorityPhoto_{DateTime.Now:MMddyyyy_HHmmss_fff}.{ext}";
                        var folderPath = "LG/Authorities/";

                        BlobStorage.Savefile(file, folderPath, fileName);

                        model.FileName = fileName;
                        model.FilePath = folderPath;
                        model.Photo = folderPath + fileName;
                    }
                }

                var result = AuthorityModel.UpdateAuthority(model);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Deletes an authority by ID.
        /// </summary>
        public JsonResult Delete(int id)
        {
            var result = AuthorityModel.AuthorityDeleteById(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets list of authority types for dropdown (from stored procedure).
        /// </summary>

        [HttpGet]
        public JsonResult GetAuthorityTypes()
        {
            var list = AuthorityTypeModel.GetAuthorityTypeDropdown_SP();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveOrder(List<Authority> orderedItems)
        {
            if (orderedItems == null || !orderedItems.Any())
            {
                return Json(new { success = false, message = "No data received." });
            }

            try
            {
                foreach (var item in orderedItems)
                {
                    AuthorityModel.ChangeOrder(item.AuthorityId, item.Order ?? 0);
                    //db.Database.ExecuteSqlRaw("EXEC UpdateAuthorityOrder @AuthorityId, @Order", parameters);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Optional: Log the exception
                return Json(new { success = false, message = "Error saving order." });
            }


        }
    }
}
