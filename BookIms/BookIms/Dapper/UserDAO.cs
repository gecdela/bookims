using BookIms.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using BookIms.Models;

namespace BookIms.Dapper
{
    public class UserDAO
    {
        public MySqlConnection con = new MySqlConnection("server=localhost;database=bookims;user id=root;password=123456;charset='gbk'");


        #region 一般的数据库操作
        //插入数据
        public int Insert(User user)
        {
            int count = con.Execute($"insert into user (user_id,user_name,password,per,tel,email) values(null,@UserName,@Password,@Per,@Tel,@Email);", user);

            return count;
        }

        //插入数据，集合
        private static void InsertCollection(MySqlConnection connection)
        {
            int count = connection.Execute(@"insert into user values (null,@a, @b)", new[] { new { a = "test1", b = DateTime.Now }, new { a = "test2", b = DateTime.Now }, new { a = "test3", b = DateTime.Now } });
            Console.WriteLine(count);
        }

        //删除数据
        public int Delete(User user)
        {
            int count = con.Execute($"delete from user where user_id = @UserId;", user);
            return count;
        }

        //修改数据
        public int Update(User user)
        {
            int count = con.Execute($"update user set user_name = @UserName,password = @Password,per = @Per,tel = @Tel,email = @Email where user_id = @UserId;", user);
            return count;
        }


        //查找数据
        public List<User> Search()
        {
            List<User> users = con.Query<User>("select user_id UserId,user_name UserName,per,tel,email from user;").ToList();
            return users;

        }
        //按条件搜索
        public List<User> Query(String condition)
        {
            List<User> users = con.Query<User>("select user_id UserId,user_name UserName,per,tel,email from user where CONCAT(user_id,user_name,per,tel,email) like '%" + condition + "%'").ToList();
            return users;
        }
        public List<User> Search(User user)
        {
            List<User> users = con.Query<User>("select user_id UserId,user_name UserName,password Password,per,tel,email from user where user_name =@UserName and password = @Password and per=@Per;",user).ToList();
            return users;

        }

        internal User Search(string userId)
        { 
            User user = new User();
            user = con.QuerySingle<User>("select user_id UserId,user_name UserName,password Password,per,tel,email,head from user where user_id="+userId);
            return user;
        }

        //查找数据
        public List<User> getByPage(int pageIndex, int size)
        {
            List<User> users = con.Query<User>("select user_id UserId,user_name UserName,per,tel,email from user limit " + (pageIndex * size) + "," + size + ";").ToList();
            return users;

        }

        //简单事务
        public void Transaction(MySqlConnection connection)
        {
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();
            int count = connection.Execute($"insert into user values(null,'王五','{DateTime.Now}');");
            transaction.Commit();
            connection.Clone();
        }
        //简单事务，回滚
        private static void TransactionRoolback(MySqlConnection connection)
        {
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();
            int count = connection.Execute($"delete from user;", transaction);
            transaction.Rollback();
            connection.Clone();
        }

        //简单存储过程
        private static void StoredProcedures(MySqlConnection connection)
        {
            var User = connection.Query<User>("spGetUser", new { uid = 10 }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            Console.WriteLine(User.UserName);
        }
        #endregion

        #region 其他特性

        //动态类型转化
        private static void DynamicObjects(MySqlConnection connection)
        {
            var rows = connection.Query("select * from user;");
            var id = ((int)rows.First().id);
            Console.WriteLine(id);
        }

        //集合参数化
        private static void IEnumerableParameterize(MySqlConnection connection)
        {
            //下面的写法等价于  connection.Query<int>("select * from user where Id in (@Ids1, @Ids2)", new { Ids1 = 10, Ids2 = 11 });
            var Users = connection.Query<int>("select * from user where id in @Ids", new { Ids = new int[] { 10, 11 } });
            Console.WriteLine(Users.Count());
        }

        //多对象映射 1-1映射
        //public List<Post> MultiMapping()
        //{
        //    var sql =
        //        @"select * from post p 
        //        left join user u on u.id = p.uid 
        //        Order by p.id";

        //    var data = con.Query<Post, User, Post>(sql, (post, user) => { post.Owner = user; return post; });
        //    var item = data.First();
        //    List<Post> list = new List<Post>();
        //    foreach (Post i in data)
        //    {
        //        list.Add(i);
        //    }
        //    return list;
        //}
        //public User mutipping()
        //{

        //    //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
        //    string sql = "SELECT * FROM user u LEFT JOIN post p ON u.id = p.uid WHERE u.id = @id";
        //    User lookup = null;
        //    //Query<TFirst, TSecond, TReturn>
        //    var data = con.Query<User, Post, User>(sql, (User, post) =>
        //    {
        //        //扫描第一条记录，判断非空和非重复
        //        if (lookup == null || lookup.Id != User.Id)
        //            lookup = User;
        //        //书对应的书评非空，加入当前书的书评List中，最后把重复的书去掉。
        //        if (post != null)
        //        {
        //            //lookup.posts.Add(post);
        //        }
        //        return lookup;
        //    }, new { id = 15 }).Distinct().SingleOrDefault();
        //    return data;
        //}
        //一次获取多个对象
        private static void MultipleResults(MySqlConnection connection)
        {
            var sql =
                @"
                select * from user where id = @uid;
                select * from post where id = @pid";

            using (var multi = connection.QueryMultiple(sql, new { uid = 10, pid = 1 }))
            {
                var Users = multi.Read<User>().ToList();
                Console.WriteLine(Users.First().UserName);
                var posts = multi.Read<Post>().Single();
                Console.WriteLine(posts.Title);
            }
        }
        public void close()
        {
            con.Close();
        }
        #endregion

    }

}


