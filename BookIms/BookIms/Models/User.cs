using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookIms.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Per { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Head { get; set; }
    }
}