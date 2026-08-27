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
    public class GetZonesController : Controller
    {
        // GET: GetZones
       
         
            public ActionResult Index()
            {
               if (string.IsNullOrEmpty(CurrentSession.UserID))
                {
                    return RedirectToAction("Login", "Account", new { @area = "" });
                }
                else
               {
                    GetZones model = new GetZones();
                    var data = GetZones.GetList();
                    model._ListZonesModel = data;
                    return View(model);
               }
            }
            public ActionResult Create()
            {
                return View(new GetZones());
            }

            // POST: Insert SubMenu record

            [HttpPost]
            public ActionResult Create(GetZones Model)
            {
                var data = GetZones.Save(Model);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            // GET: Selected Record By Id
            public JsonResult Edit(int id)
            {
                GetZones model = new GetZones();
                var data = GetZones.GetById(id);
                model._ListZonesModel = data;
                string value = string.Empty;

                value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(value, JsonRequestBehavior.AllowGet);

            }


            //Post: Alter selected record with value

            [HttpPost]

            public JsonResult Update(GetZones Model)
            {
                var data = GetZones.UpdateById(Model);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            // POST: Delete record By Id
            public JsonResult Delete(int id)
            {
                var data = GetZones.DeleteById(id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            // GET: Menu Drop Down

            public JsonResult DistrictDropDown()
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
                    foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetDistricts]", new object[0]).Tables[0].Rows)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = Convert.ToString(row["DistrictName"].ToString());
                        item.Value = Convert.ToString(row["DistrictCode"].ToString());
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
        public JsonResult ZoneDropDown()
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetZones]", new object[0]).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["Zone"].ToString());
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
    }
}
