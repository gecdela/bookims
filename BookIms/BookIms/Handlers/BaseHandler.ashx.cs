using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BookIms.Dapper;
using BookIms.Models;
using Newtonsoft.Json;

namespace BookIms.Handlers
{
    /// <summary>
    /// DaoHandler 的摘要说明
    /// </summary>
    public class BaseHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";
            String method = context.Request["method"];


            System.Reflection.MethodInfo ChosedMethod = this.GetType().GetMethod(method);//利用反射根据传进来的值选取要被执行的方法

            if (ChosedMethod != null)
            {
                ChosedMethod.Invoke(this, new object[] { context });
            }

        }

        public void GetBookList(HttpContext context)
        {
            BookDAO dao = new BookDAO();
            List<Book> lst = dao.Search();


            context.Response.Write(JsonConvert.SerializeObject(new { Result = 1, Msg = "获取全部数据成功!", data = lst }));

        }
        public void GetBookInfo(HttpContext context)
        {
            int pageIndex = int.Parse(context.Request["pageIndex"]);
            pageIndex -= 1;
            BookDAO dao = new BookDAO();
            List<Book> lst = dao.getByPage(pageIndex, 3);
            context.Response.Write(JsonConvert.SerializeObject(new { Result = 1, Msg = "获取第" + pageIndex + "页数据成功!", data = lst }));
        }
        public void InsertBook(HttpContext context)
        {
            String bookname = context.Request["bookname"];
            String bookauthor = context.Request["bookauthor"];
            String bookprice = context.Request["bookprice"];
            Book book = new Book();
            book.BookName = bookname;
            book.BookAuthor = bookauthor;
            book.BookPrice = bookprice;
            book.Status = 1;

            if (context.Request.Files.Count > 0)
            {
                var f = context.Request.Files.Get("file1");
                f.SaveAs(@"D:\backup\develop\vsSpace\BookIms\BookIms\images\" + f.FileName);
            }

            int result = new BookDAO().Insert(book);
            context.Response.Write(JsonConvert.SerializeObject(new { Result = 1, Msg = "新增数据成功!", data = result }));
        }

        public void EditBook(HttpContext context)
        {
            int ID = int.Parse(context.Request["ID"]);
            String bookname = context.Request["bookname"];
            String bookauthor = context.Request["bookauthor"];
            String bookprice = context.Request["bookprice"];
            String status = context.Request["status"];
            Book book = new Book();
            book.BookId = ID;
            book.BookName = bookname;
            book.BookAuthor = bookauthor;
            book.BookPrice = bookprice;
            book.Status = int.Parse(status);
            int result = new BookDAO().Update(book);
            context.Response.Write(JsonConvert.SerializeObject(new { Result = 1, Msg = "编辑数据成功!", data = result }));
        }
        public void DelBook(HttpContext context)
        {
            int ID = int.Parse(context.Request["ID"]);
            Book book = new Book();
            book.BookId = ID;
            int result = new BookDAO().Delete(book);
            context.Response.Write(JsonConvert.SerializeObject(new { Result = 1, Msg = "删除数据成功!", data = result }));
        }

        public void QueryBookList(HttpContext context)
        {
            BookDAO dao = new BookDAO();
            String condition = context.Request["condition"];

            List<Book> lst = dao.Query(condition);
            context.Response.Write(JsonConvert.SerializeObject(new { Result = 1, Msg = "获取全部数据成功!", data = lst }));
        }


        public void QueryUserList(HttpContext context)
        {
            UserDAO dao = new UserDAO();
            String condition = context.Request["condition"];
            List<User> lst = dao.Query(condition);
            context.Response.Write(JsonConvert.SerializeObject(new { Result = 1, Msg = "获取全部数据成功!", data = lst }));
        }
        public void QueryRecordList(HttpContext context)
        {
            RecordDAO dao = new RecordDAO();
            String condition = context.Request["condition"];
            List<Record> lst = dao.Query(condition);
            context.Response.Write(JsonConvert.SerializeObject(new { Result = 1, Msg = "获取全部数据成功!", data = lst }));
        }
        public void QueryUserRecordList(HttpContext context)
        {
            RecordDAO dao = new RecordDAO();
            String condition = context.Request["condition"];
            String userId = context.Request["userId"];
            List<Record> lst = dao.Query(condition, userId);
            context.Response.Write(JsonConvert.SerializeObject(new { Result = 1, Msg = "获取全部数据成功!", data = lst }));
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}