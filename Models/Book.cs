using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booklook_crudgui.Models {
    public class Book {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Authors { get; set; } = string.Empty;
        public string ImageSource { get; set; } = string.Empty;
        public string Isbn13 { get; set; } = string.Empty;
        public string Isbn10 { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public string BookLink { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
