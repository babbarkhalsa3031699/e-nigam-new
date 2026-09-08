using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMNirdesh.Models;
using Newtonsoft.Json;

namespace CMNirdesh.Controllers
{
    public class mOfficeController : Controller
    {
        // GET: Fetch Office List of Records
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(CurrentSession.UserID))
            {
                return RedirectToAction("Login", "Account", new { @area = "" });
            }
            else
            {
                LstofficeModel model = new LstofficeModel();
                var data = mOffice.GetOfficeList();
                model._ListofficeModel = data;
                return View(model);
            }

        }

        public ActionResult Create()
        {
            return View(new mOffice());
        }

        //Post: Insert New Office Record

        [HttpPost]

        public JsonResult Create(mOffice mOffice)
        {
            var data = mOffice.SaveOfficeRecord(mOffice);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Get Office Reccord By Id
        public JsonResult Edit(int id)
        {
            LstofficeModel model = new LstofficeModel();
            var data = mOffice.GetOfficeRecordById(id);
            model._ListofficeModel = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        // POST: Update Office Record Selected ID

        [HttpPost]

        public JsonResult Update(mOffice mOffice)
        {
            var data = mOffice.UpdateOfficeRecordById(mOffice);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //POST: Delete Selected Row

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var data = mOffice.DeleteOfficeRecordById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //Populating Drop Down

        public JsonResult DepartmentDropDown()
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDepartment]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    if (CurrentSession.RoleID.ToString() == "18")
                    {
                        item.Text = Convert.ToString(row["deptname"].ToString());
                        item.Value = Convert.ToString(row["deptid"].ToString());
                        items.Add(item);
                    }
                    else
                    {
                        if (row["deptid"].ToString() == CurrentSession.DeptID)
                        {
                            item.Text = Convert.ToString(row["deptname"].ToString());
                            item.Value = Convert.ToString(row["deptid"].ToString());
                            items.Add(item);
                        }
                    }
                }
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return Json(items, "Value", JsonRequestBehavior.AllowGet);
            }

        }
    }
}







