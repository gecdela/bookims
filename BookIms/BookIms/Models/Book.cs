using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookIms.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string BookPrice { get; set; }
        public int Status { get; set; }
        public string BookPic { get; set; }
    }
}