using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookIms.Models
{
    //dapper将sql执行结果中字段自动映射到同名的相应的变量中
    public class User2
    {
        public User2()
        {
            posts = new List<Post>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public String Birthday { get; set; }
        //如果执行sql后返回的结果没有对应字段，则为null；
        public string IgnoreField { get; set; }
        public virtual List<Post> posts { get; set; }
    }
}