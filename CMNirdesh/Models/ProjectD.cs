using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace CMNirdesh.DTO
{
    public class Project
    {

        public int ProgramId { get; set; }
        public string ProgrammeName { get; set; }


    }
    public class ProjectD : Project
    {



        public string ProgrammeTitle { get; set; }

      
        public string IconImage { get; set; }
        public string ProgrammeDescription { get; set; }


        public int? mDepartmentDetailsOrder
        {


            get;
            set;
        }




        public DateTime? CreatedDate { get; set; }

        public List<DocumentD> DocumentList
        {
            get;
            set;
        }

        public List<ImageD> ImageList
        {
            get;
            set;
        }

        public string GenerateProjectNameUrl()
        {
            string phrase = string.Format("{0}-{1}", ProgramId, ProgrammeTitle);

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
        public class PersonViewModel
        {
            public List<ProjectD> ListProject { get; set; }
            public Pager pager { get; set; }
        }

        public class Pager
        {
            public Pager(int totalItems, int? page, int pageSize = 10)
            {
                // Total Paging need to show
                int _totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
                //Current Page
                int _currentPage = page != null ? (int)page : 1;
                //Paging to be starts with
                int _startPage = _currentPage - 5;
                //Paging to be end with
                int _endPage = _currentPage + 4;
                if (_startPage <= 0)
                {
                    _endPage -= (_startPage - 1);
                    _startPage = 1;
                }
                if (_endPage > _totalPages)
                {
                    _endPage = _totalPages;
                    if (_endPage > 10)
                    {
                        _startPage = _endPage - 9;
                    }
                }
                //Setting up the properties
                TotalItems = totalItems;
                CurrentPage = _currentPage;
                PageSize = pageSize;
                TotalPages = _totalPages;
                StartPage = _startPage;
                EndPage = _endPage;
            }
            public int TotalItems { get; set; }
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
            public int TotalPages { get; set; }
            public int StartPage { get; set; }
            public int EndPage { get; set; }
        }
    }
    [Serializable]
    public class ProjectDList : System.Collections.ObjectModel.ObservableCollection<ProjectD> { }
    [Serializable]
    public class ProjectList : System.Collections.ObjectModel.ObservableCollection<Project> { }

    [Serializable]
    public class PriorityProjectList : System.Collections.ObjectModel.ObservableCollection<PriorityProject> { }
    [Serializable]
    public class PromiseList : System.Collections.ObjectModel.ObservableCollection<Promise> { }
    public class Promise

    {

       public Int64 PromiseId
        {

            get;
            set;
        }
        public string PromiseName
        {

            get;
            set;
        }
        public string DeptId
        {

            get;
            set;
        }

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
    }

    public class PromiseDocument

    {
        public string ProjectName
        {

            get;
            set;

        }
        public string ProjectDescription
        {

            get;
            set;

        }
        public List<DocumentD> DocumentList
        {
            get;
            set;
        }

        public List<ImageD> ImageList
        {
            get;
            set;
        }


        public bool IsImage { get; set; }





    }

    public class PriorityProject

    {
        public Int64 ProjectId
        {

            get;
            set;
        }
      public string DeptId
        {

            get;
            set;
        }
        public string DeptName
        {

            get;
            set;
        }

        public string ProjectName
        {

            get;
            set;
        }
       

        public string FirmName
        {

            get;
            set;
        }

        public string CategoryName
        {

            get;
            set;
        }



        public string EstimatedAmount
        {

            get;
            set;
        }
        public string FundReleased
        {

            get;
            set;
        }

        public string Expenditure
        {

            get;
            set;
        }

        public string PhysicalProgress
        {

            get;
            set;
        }

        public string StartDate
        {

            get;
            set;
        }
        public string EndDate
        {

            get;
            set;
        }
        public string ProjectStatusName
        {

            get;
            set;
        }

        public string Remark
        {

            get;
            set;
        }
      


        public string PromiseName
        {

            get;
            set;
        }


    }



}
