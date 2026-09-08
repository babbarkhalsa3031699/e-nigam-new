using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using CMNirdesh.Models;
using CMNirdesh.Error;

namespace CMNirdesh.Models
{
    public class ConstituencityList : System.Collections.ObjectModel.ObservableCollection<AssignProjectConstituencyWise> { }

    public class AssignProjectConstituencyWise
    {
        public Int64 ProjectMappedConstituencyId { get; set; }        
        public string cdid { get; set; }
        public Int64 ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Int64 ConstituencyId { get; set; }
        public string ConstituencyName { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "Please select project status")]
        public int ProjectStatusId
        {
            get;
            set;
        }

        public string DeptId
        {
            get;
            set;
        }
        public string Mode { get; set; }
        public Boolean IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Required]  
        [Display(Name = "Choose Multiple Countries")]  
        public List<int> SelectedProjectList { get; set; }  

        public SelectList ProjectList { get; set; }

        public List<SelectListItem> Projects { get; set; }
       
        public SelectList Contituencylist { get; set; }
        public SelectList StatusTypeList { get; set; }
        public bool IsSelected { get; set; }



        public static void GetProjectList(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@programmeDocsId", Id) };
               CMNirdesh.Models.SqlHelper.ExecuteNonQuery(connection, "[DeleteDeptDocumentById]", parameterValues);
                connection.Close();
            }
            catch (Exception)
            {
            }
        }


        public static SelectList getProjectByDeptId(string deptId)
        {

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem
            {
                Text = "--Select Project--",
                Value = "",
                Selected = true,
            });

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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getProjectByDeptId]", param).Tables[0].Rows)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(row["ProjectName"].ToString());
                    item.Value = Convert.ToString(row["ProjectId"].ToString());
                    items.Add(item);
                }
                return new SelectList(items, "Value", "Text");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return new SelectList(items, "Value", "Text");
            };
        }



        public static SelectList ConstituencyByProject
        {
            get
            {
                List<SelectListItem> items = new List<SelectListItem>();
                //items.Add(new SelectListItem
                //{
                //    Text = "--Select Constituency--",
                //    Value = "",
                //    Selected = true,
                //});
                try
                {
                    SqlConnection connection = ClsConnection.GetConnection();
                    if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                    {
                        connection.Open();
                    }
                    connection.Close();
                    SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter() };
                    foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[GetConstituency]", parameterValues).Tables[0].Rows)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = Convert.ToString(row["ConstituencyName"].ToString());
                        item.Value = Convert.ToString(row["ConstituencyCode"].ToString());
                        items.Add(item);
                    }
                    return new SelectList(items, "Value", "Text");
                }
                catch (Exception ex)
                {
                    ErrorLog.WriteToLog(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                    return new SelectList(items, "Value", "Text");
                }
            }
        }

        public static int SaveConstituencyProjectMapping(AssignProjectConstituencyWise model, int ContitunecyId)
        {
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@ProjectMappedConstituencyId", model.ProjectMappedConstituencyId);
                param[1] = new SqlParameter("@ProjectId", model.ProjectId);
                param[2] = new SqlParameter("@ConstituencyId", ContitunecyId);
                param[3] = new SqlParameter("@isActive", model.IsActive);
                param[4] = new SqlParameter("@createdBy", model.CreatedBy.ToString());              
              
                SqlHelper.ExecuteNonQuery(con, "[SaveConstituencyProjectMapping]", param);
                con.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;

            }
        }


        public static SelectList GetStatusTypes()
        {
            var result = new List<SelectListItem>();
            // result.Add(new SelectListItem() { Text = "Select Status Type", Value = "0" });
            result.Add(new SelectListItem() { Text = "Published", Value = "1" });
            result.Add(new SelectListItem() { Text = "Unpublished ", Value = "0" });
            //  result.Add(new SelectListItem() { Text = "Archived", Value = "3" });
            //  result.Add(new SelectListItem() { Text = "Trashed", Value = "4" });


            return new SelectList(result, "Value", "Text");
        }


        public static ConstituencityList GetConstituencyList(string deptId)
        {
            ConstituencityList list = new ConstituencityList();
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
                foreach (DataRow row in SqlHelper.ExecuteDataset(connection, "[getPromiseMappedConstituecyList]", param).Tables[0].Rows)
                {
                    AssignProjectConstituencyWise item = new AssignProjectConstituencyWise
                    {
                        cdid = Convert.ToString(row["ConstituencyId"].ToString()),
                        ConstituencyName = Convert.ToString(row["ProjectName"].ToString()),
                        ProjectName = Convert.ToString(row["ConstituencyName"].ToString()),
                        IsActive = Convert.ToBoolean(row["isActive"]),
                        ProjectId = Convert.ToInt64(row["ProjectId"]),
                        ProjectMappedConstituencyId = Convert.ToInt64(row["ProjectMappedConstituencyId"]),
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


        public static AssignProjectConstituencyWise GetConstituencyListById(int Id)
        {
            AssignProjectConstituencyWise assignProjectConstituencyWise = new AssignProjectConstituencyWise();
            try
            {
                SqlConnection con = ClsConnection.GetConnection();
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
                    con.Open();
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@projectId", Id);


                DataSet ds = SqlHelper.ExecuteDataset(con, "[getConstituencyListById]", param);
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {


                    assignProjectConstituencyWise.ProjectId = Convert.ToInt64(row["ProjectId"].ToString());
                    assignProjectConstituencyWise.IsActive = Convert.ToBoolean(row["IsActive"]);
                    assignProjectConstituencyWise.ConstituencyId = Convert.ToInt64(row["ConstituencyId"]);
                    //  assignProjectConstituencyWise.ProjectName = Convert.ToString(row["ProjectName"].ToString());
                 
                       



                }
                return assignProjectConstituencyWise;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return assignProjectConstituencyWise;

            }
        }


        public static void DeleteProjectConstituencyById(int Id)
        {
            try
            {
                SqlConnection connection = ClsConnection.GetConnection();
                if ((connection.State == ConnectionState.Closed) || (connection.State == ConnectionState.Broken))
                {
                    connection.Open();
                }
                

                SqlParameter[] parameterValues = new SqlParameter[] { new SqlParameter("@ProjectmappedId", Id) };


                SqlHelper.ExecuteNonQuery(connection, "[DeleteConstituencyMappedProjectById]", parameterValues);
                connection.Close();
            }
            catch (Exception)
            {
            }
        }

    }

    public class ConstituencyObj
    {
       
        public int ConstId { get; set; }

     
        public string ConstName { get; set; }

      
    }


}