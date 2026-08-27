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
    
    public class mDepartmentsController : Controller
    {
        // GET: Departments List
        public ActionResult Index()
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                else
                {
                    LstdeptModel model = new LstdeptModel();
                    var data = mDepartment.GetDepartmentsList();
                    model._ListdeptModel = data;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, " Error in " + System.Reflection.MethodBase.GetCurrentMethod() + " ");
                return RedirectToAction("Index");
            }
        }
       
        public ActionResult Create()
        {
            return View(new mDepartment());
        }

        // Create: New Department Record

        [HttpPost]
        public JsonResult Create(mDepartment mDepartment)
        {
            var data = mDepartment.SaveDepartmentRecord(mDepartment);
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        // GET: Department List By Id
        public JsonResult Edit(string id)
        {
            LstdeptModel model = new LstdeptModel();
            var data = mDepartment.GetDepartmentListById(id);
            model._ListdeptModel = data;
            string value = string.Empty;

            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet); 
         }

        // Update: Department List By Selected Id   

        [HttpPost]
       
        public JsonResult Update(mDepartment mDepartment)
        {
            var data = mDepartment.UpdateDepartmentRecordById(mDepartment);
            return Json(data, JsonRequestBehavior.AllowGet);
          
        }

       // Delete: Department List By Selected Id
        public JsonResult Delete(string Id)
        {
            var data = mDepartment.DeleteDepartmentById(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MCDropDown()
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetStatesActive]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["MCName"].ToString());
                    item.Value = Convert.ToString(row["Id"].ToString());
                    
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
        //public JsonResult UserTypeDropDown()
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    try
        //    {
        //        SqlConnection connection = ClsConnection.GetConnection();
        //        if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
        //        {
        //            connection.Open();
        //        }
        //        connection.Close();
        //        foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetUserType]", new object[0]).Tables[0].Rows)
        //        {
        //            SelectListItem item = new SelectListItem();
        //            item.Text = Convert.ToString(row["UserTypeName"].ToString());
        //            item.Value = Convert.ToString(row["UserTypeID"]);
        //            //item.UserTypeAbbreviation = Convert.ToString(row["UserTypeAbbreviation"].ToString());
        //            items.Add(item);
        //        }
        //        return Json(items, "Value", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
        //        return Json(items, "Value", JsonRequestBehavior.AllowGet);
        //    }

        //}

    }
}
