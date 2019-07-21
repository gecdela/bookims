using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using BookIms.Models;
using BookIms.Dapper;
using BookIms.Models;
using System.Web;
using BookIms.tools;
using Newtonsoft.Json;

namespace BookIms.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserRecord(String userId)
        {
            if (userId == null || userId.Equals(""))
            {
                return RedirectToAction("/Login");
            }
                return View();
        }
        //[System.Web.Mvc.HttpPost]
        public ActionResult UploadImg(HttpContext context)

        {
            //if (Request.Files.Count > 0)  
            //{  
            //    HttpPostedFileBase f = Request.Files["file1"];  
            //    f.SaveAs(@"D:\backup\develop\vsSpace\BookIms\BookIms\images\" + f.FileName);  
            //}
            // var username = Request["username"];
            SendFileHandler dao = new SendFileHandler();
            dao.ProcessRequest(context);
           return View();  
       }

public ActionResult UserCenter(String userId)
        {
            if (userId != null && !userId.Equals(""))
            {
                UserDAO userDao = new UserDAO();
                User user = userDao.Search(userId);
                ViewData["username"] = user.UserName;
                ViewData["password"] = user.Password;
                ViewData["per"] = user.Per;
                ViewData["tel"] = user.Tel;
                ViewData["email"] = user.Email;
                ViewData["head"] = user.Head;
            }
            else {
                return RedirectToAction("/Login");
            }

            return View();
        }
        public ActionResult BookDetail(String BookId)
        {
            BookDAO dao = new BookDAO();
            Book book = new Book();
            //Session.Add("BookId", BookId);
            //book.BookId =int.Parse(BookId);
            //book = dao.Search(book);
            book = dao.Search(BookId);
            ViewData["BookId"] = book.BookId;
            ViewData["BookName"] = book.BookName;
            ViewData["BookAuthor"] = book.BookAuthor;
            ViewData["BookPrice"] = book.BookPrice;
            ViewData["BookPic"] = book.BookPic;
            ViewData["Status"] = book.Status;
            return View();
        }
        //租借记录
        public JsonResult GetRecordByPage(String userId,int pageIndex, int size)
        {
            RecordDAO dao = new RecordDAO();
            pageIndex -= 1;
            List<Record> lst = dao.GetRecordByPage(userId,pageIndex, size);
            return Json(new
            {
                data = lst,
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetRecordList(String userId)
        {
            RecordDAO dao = new RecordDAO();
            List<Record> lst = dao.GetRecordList(userId);
            return Json(new
            {
                data = lst,
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DelRecord(String userId,int ID)
        {
            Record record = new Record();
            record.RecordId = ID;
            int result = new RecordDAO().Delete(record);
            return Json(new
            {
                data = result,
            }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetBookByPage(int pageIndex,int size)
        {
            pageIndex -= 1;
            BookDAO dao = new BookDAO();
            List<Book> lst = dao.getByPage(pageIndex, size);
            return Json(new
            {
                data = lst,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBookInfo(int BookId)
        {
            BookDAO dao = new BookDAO();
            Book book = new Book();
            book.BookId = BookId;
            book = dao.Search(book);
            return Json(new
            {
                data = book,
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BorrowBook(string BookId,string bookName,string bookAuthor,string bookPrice,string bookPic,string userId)
        {
            RecordDAO recordDAO = new RecordDAO();
            BookDAO bookDAO = new BookDAO();
            Book book = new Book();
            Record record = new Record();
            book.BookId = int.Parse(BookId);
            book.BookName = bookName;
            book.BookAuthor = bookAuthor;
            book.BookPrice = bookPrice;
            book.BookPic = bookPic;
            book.Status = 0;
            bookDAO.Update(book);
            int count = recordDAO.Insert(int.Parse(BookId), userId);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login()
        {
            return View();
        }
        public JsonResult LoginConfirm(string username, string password, string selectedIndex)
        {
            UserDAO userDao = new UserDAO();
            User user = new User();
            User user2 = new User();
            user.UserName = username;
            user.Password = password;
            user.Per = selectedIndex;
            List<User> uses = userDao.Search(user);
            if (uses.Count != 0) user2 = uses.FirstOrDefault<User>();
            int result = 0;
            if (user2.UserName == username && user2.Password == password && user2.Per == selectedIndex)
            {
                result = 1;
                Session.Add("username",username);
                Session.Add("userId", user2.UserId);
            }
            
                return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOff()
        {
            Session["username"] = null;
            return RedirectToAction("Index", "User");
        }

        public JsonResult Update(String username,String password,String tel,String per,String email,String userId)
        {
            UserDAO userDao = new UserDAO();
            User user = new User();
            user.UserName = username;
            user.Password = password;
            user.Tel = tel;
            user.Email = email;
            user.Per = per;
            user.UserId =int.Parse(userId);
            int count = userDao.Update(user);
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult upload()
        {
            return View();
        }
        private void ReadExcel(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            try
            {
                var file = context.Request.Files[0];
                if (file.FileName == "")
                {
                    context.Response.Write("<script>parent.callback('请先导入文件');</script>");
                }
                var stream = file.InputStream;
                //这里可以对文件流做些什么

            }
            catch (Exception ex)
            {
                context.Response.Write("<script>parent.callback(" + ex.ToString() + ");</script>");
            }
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
