using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookIms.Models
{
    public class Record
    {
        public int RecordId { get; set; }
        public String StartDate { get; set; }
        public String DeadLine { get; set; }
        public User user { get; set; }
        public Book book { get; set; }
    }
}