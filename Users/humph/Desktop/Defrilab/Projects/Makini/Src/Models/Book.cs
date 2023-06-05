namespace Makini.Src.Models
{
    public  class Book : BaseModel
    {
        public  string BookUri { get; set; }
        public string BookName { get; set; }
        public  string  ContainerType { get; set; }
        public  string  BookTitle { get; set; }
        public  string  Author { get; set; }
        public  string CoverImagePhotoUri { get; set; }
    }
}
