
using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CMNirdesh.Models
{
    [Serializable]
    public class PromiseList : System.Collections.ObjectModel.ObservableCollection<Promise> { }
    public class Promise

    {


        public SelectList mDepartmentList { get; set; }
        public List<PriorityProject> lstPriorityProject
        {

            get;
            set;
        }

        public string GenerateProjectNameUrl()
        {
            string phrase = string.Format("{0}-{1}", PromiseId, PromiseName);

            string str = RemoveAccent(phrase).ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            str = Regex.Replace(str, @"\s+", " ").Trim();

            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }



        public Int64 PromiseId
        {

            get;
            set;
        }
        [Required(ErrorMessage = "Promise Name is Required")]
        public string PromiseName
        {

            get;
            set;
        }
        public Boolean isActive
        {

            get;
            set;
        }
        public Boolean isDeleted
        {

            get;
            set;
        }
        public string createdBy
        {

            get;
            set;
        }

        public string mode
        {
            get;
            set;
        }
        public string DeptId
        {
            get;
            set;
        }

        public SelectList StatusTypeList { get; set; }

        public static PromiseList GetPromiseList(string deptId)
        {
            PromiseList list = new PromiseList();
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@deptId", deptId);

                    connection.Close();
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getPromise]", param).Tables[0].Rows)
                {
                    Promise item = new Promise
                    {
                        PromiseId = Convert.ToInt64(row["PromiseId"].ToString()),
                        isActive = Convert.ToBoolean(row["isActive"]),
                        PromiseName = Convert.ToString(row["PromiseName"].ToString()),
                        DeptId = Convert.ToString(row["DeptId"].ToString()),
                    };
                    list.Add(item);
                }
                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        public static Promise GetPromiseListById(int Id)
        {
            Promise promise = new Promise();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@promiseId", Id);


                DataSet ds = SqlHelper.ExecuteDataset(con, "[getPromiseListbyId]", param);
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    promise.PromiseId = Convert.ToInt64(row["PromiseId"].ToString());
                    promise.isActive = Convert.ToBoolean(row["isActive"]);
                    promise.PromiseName = Convert.ToString(row["PromiseName"].ToString());
                    promise.DeptId =  Convert.ToString(row["DeptId"].ToString());




                }
                return promise;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return promise;

            }
        }

        public static int SavePromise(Promise model)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@CreatedBy", model.createdBy.ToString());
                param[1] = new SqlParameter("@Active", model.isActive);
                param[2] = new SqlParameter("@IsDeleted", model.isDeleted);
                param[3] = new SqlParameter("@PromiseId", model.PromiseId);
                param[4] = new SqlParameter("@PromiseName", model.PromiseName);
                param[5] = new SqlParameter("@deptid", model.DeptId);
                SqlHelper.ExecuteNonQuery(con, "[SavePromise]", param);
                con.Close();
                return 1;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return 0;

            }
        }

        public static SelectList GetStatusTypes()
        {
            var result = new List<SelectListItem>();
            // result.Add(new SelectListItem() { Text = "Select Status Type", Value = "0" });
            result.Add(new SelectListItem() { Text = "Published", Value = "True" });
            result.Add(new SelectListItem() { Text = "Unpublished ", Value = "False" });
            //  result.Add(new SelectListItem() { Text = "Archived", Value = "3" });
            //  result.Add(new SelectListItem() { Text = "Trashed", Value = "4" });


            return new SelectList(result, "Value", "Text");
        }

        public static void DeletePromiseById(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@PromiseId", Id) };
                SqlHelper.ExecuteNonQuery(connection, "[DeletePromiseById]", parameterValues);
                connection.Close();
            }
            catch (Exception)
            {
            }
        }


    }
}