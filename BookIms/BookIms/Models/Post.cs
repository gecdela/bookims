using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookIms.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int uid { get; set; }
        public virtual User2 Owner { get; set; }
    }
}