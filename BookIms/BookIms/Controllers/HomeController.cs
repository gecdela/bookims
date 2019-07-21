using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookIms.Dapper;
using BookIms.Models;

namespace BookIms.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        //public JsonResult GetUserList()
        //{
        //    UserDAO dao = new UserDAO();
        //    List<User2> lst = dao.Search();
        //    List<Post> pst = dao.MultiMapping();
        //    User2 user = dao.mutipping();
        //    Console.WriteLine(pst);
        //    return Json(new
        //    {
        //        data = lst,
        //    }, JsonRequestBehavior.AllowGet);
            
        //}
        //public JsonResult GetUserInfo(int pageIndex)
        //{
        //    pageIndex -= 1;
        //    UserDAO dao = new UserDAO();
        //    List<User2> lst = dao.getByPage(pageIndex,3);
        //    return Json(new
        //    {
        //        data = lst,
        //    }, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult InsertUser(String add_name,String add_birth)
        //{
        //    User2 user = new User2();
        //    user.Birthday = add_birth;
        //    user.Name = add_name;
        //    int result = new UserDAO().Insert(user);
        //    return Json(new
        //    {
        //        data = result,
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult EditUser(String edit_ID,String edit_name, String edit_birth)
        //{
        //    User2 user = new User2();
        //    user.Id = int.Parse(edit_ID);
        //    user.Birthday = edit_birth;
        //    user.Name = edit_name;
        //    int result = new UserDAO().Update(user);
        //    return Json(new
        //    {
        //        data = result,
        //    }, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult DelUser(int delete_id)
        //{
        //    User2 user = new User2();
        //    user.Id = delete_id;
        //    int result = new UserDAO().Delete(user);
        //    return Json(new
        //    {
        //        data = result,
        //    }, JsonRequestBehavior.AllowGet);
        //}
    }
}