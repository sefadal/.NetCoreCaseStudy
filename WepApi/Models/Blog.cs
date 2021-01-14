using System;

namespace WepApi.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string BlogDescription { get; set; }
        public string UserName { get; set; }
        public DateTime InstertDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class SearchFilter
    {
        public string searchString { get; set; }
        public string sortOrder { get; set; }
    }
}
