using System;

namespace CMNirdesh.DTO
{
    public class DocumentD
    {
        public Int64 ProgrammeDocsId { get; set; }
        public string DocumentTitle { get; set; }


        public string DocumentLocation
        {
            get;
            set;
        }

        public DateTime? CreatedDate { get; set; }
    }

    [Serializable]
    public class DocumentDList : System.Collections.ObjectModel.ObservableCollection<DocumentD> { }
}
