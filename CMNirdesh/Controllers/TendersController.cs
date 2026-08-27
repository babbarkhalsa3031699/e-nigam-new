using CMNirdesh.Models;
using CMNirdesh.Models.MCHouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMNirdesh.Controllers
{
    public class TendersController : Controller
    {
        // GET: TenderList
    

        public ActionResult Index()
        {
            //if (string.IsNullOrEmpty(CurrentSession.UserID))
            //{
            //    return RedirectToAction("Login", "Account", new { @area = "" });
            //}
            //else
            //{
                TenderMenu model = new TenderMenu();
                var data = TenderMenu.GetList();
                model._LstTenderMenu = data;
                return View(model);
            //}
        }

        public ActionResult Create()
        {
            return View(new TenderMenu());
        }

        // Create: New  Record

        [HttpPost]
        public JsonResult Create(TenderMenu model)
        {
            var data = TenderMenu.SaveRecord(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //GET:  List By Id
        public JsonResult Edit(int Id)
        {
            TenderMenu model = new TenderMenu();
            var data = TenderMenu.GetRowById(Id);
            model._LstTenderMenu = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // Update: List By Selected Id   

        [HttpPost]

        public JsonResult Update(TenderMenu model)
        {
            var data = TenderMenu.UpdateRecord(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        // Delete: List By Selected Id
        //public JsonResult Delete(string Id)
        //{
        //    var data = TenderMenu.DeleteRecord(Id);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult UnitsDropDown()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetUnitsName]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["unitname"].ToString());
                    item.Value = Convert.ToString(row["id"].ToString());

                    items.Add(item);
                }
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Delete(int Id)
        {
            var data = TenderMenu.DeleteRecord(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}

