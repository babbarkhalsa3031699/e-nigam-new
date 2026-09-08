using System;

namespace CMNirdesh.DTO
{
    public class ImageD
    {
        public Int64 ProgrammeImageId { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ImageLocation
        {

            get;
            set;
        }
    }
    [Serializable]
    public class ImageDList : System.Collections.ObjectModel.ObservableCollection<ImageD> { }
}
