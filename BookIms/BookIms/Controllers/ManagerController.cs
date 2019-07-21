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

namespace BookIms.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult UserManage()
        {
            return View();
        }
        public ActionResult BookManage()
        {
            return View();
        }
        public ActionResult test()
        {
            return View();
        }
        public ActionResult RecordManage()
        {
            RecordDAO dao = new RecordDAO();
            return View();
        }
        public JsonResult GetBookList()
        {
            BookDAO dao = new BookDAO();
            List<Book> lst = dao.Search();
            return Json(new
            {
                data = lst,
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetBookInfo(int pageIndex)
        {
            pageIndex -= 1;
            BookDAO dao = new BookDAO();
            List<Book> lst = dao.getByPage(pageIndex, 3);
            return Json(new
            {
                data = lst,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertBook(String bookname, String bookauthor,string bookprice)
        {
            Book book = new Book();
            book.BookName = bookname;
            book.BookAuthor = bookauthor;
            book.BookPrice = bookprice;
            book.Status = 1;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase f = Request.Files["file1"];
                f.SaveAs(@"D:\backup\develop\vsSpace\BookIms\BookIms\images\" + f.FileName);
            }

            int result = new BookDAO().Insert(book);
            return Json(new
            {
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditBook(String ID, String bookname, String bookauthor, string bookprice,String status)
        {
            Book book = new Book();
            book.BookId = int.Parse(ID);
            book.BookName = bookname;
            book.BookAuthor = bookauthor;
            book.BookPrice = bookprice;
            book.Status = int.Parse(status);
            int result = new BookDAO().Update(book);
            return Json(new
            {
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelBook(int ID)
        {
            Book book = new Book();
            book.BookId = ID;
            int result = new BookDAO().Delete(book);
            return Json(new
            {
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }


        //用户管理
        public JsonResult GetUserList()
        {
            UserDAO dao = new UserDAO();
            List<User> lst = dao.Search();
            return Json(new
            {
                data = lst,
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetUserInfo(int pageIndex)
        {
            pageIndex -= 1;
            UserDAO dao = new UserDAO();
            List<User> lst = dao.getByPage(pageIndex, 3);
            return Json(new
            {
                data = lst,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertUser(string username,string tel, string email, string per, string password)
        {
            User user = new User();
            user.UserName = username;
            user.Per = per;
            user.Tel = tel;
            user.Email = email;
            user.Password = password;
            int result = new UserDAO().Insert(user);
            return Json(new
            {
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditUser(string username, string tel, string email, string userid)
        {
            User user = new User();
            user.UserId = int.Parse(userid);
            user.UserName = username;
            user.Tel = tel;
            user.Email = email;
            int result = new UserDAO().Update(user);
            return Json(new
            {
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DelUser(int ID)
        {
            User user = new User();
            user.UserId = ID;
            int result = new UserDAO().Delete(user);
            return Json(new
            {
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }

        //租借管理
        public JsonResult GetRecordList()
        {
            RecordDAO dao = new RecordDAO();
            List<Record> lst = dao.GetRecordList();
            return Json(new
            {
                data = lst,
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetRecordByPage(int pageIndex,int size)
        {
            RecordDAO dao = new RecordDAO();
            pageIndex -= 1;
            List<Record> lst = dao.GetRecordByPage(pageIndex,size);
            return Json(new
            {
                data = lst,
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DelRecord(int ID)
        {
            Record record = new Record();
            record.RecordId = ID;
            int result = new RecordDAO().Delete(record);
            return Json(new
            {
                data = result,
            }, JsonRequestBehavior.AllowGet);
        }


        //按条件搜索

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

    }
}
