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
namespace BookIms.Dapper
{
    public class User2DAO
    {
        public  MySqlConnection con = new MySqlConnection("server=localhost;database=dapper;user id=root;password=123456;charset='gbk'");


        #region 一般的数据库操作
        //插入数据
        public int Insert(User2 user)
        {
            int count = con.Execute($"insert into person values(null,@Name,@Birthday);",user);

            return count;
         }

        //插入数据，集合
        private static void InsertCollection(MySqlConnection connection)
        { 
            int count = connection.Execute(@"insert into person values (null,@a, @b)", new[] { new { a = "test1", b = DateTime.Now }, new { a = "test2", b = DateTime.Now }, new { a = "test3", b = DateTime.Now } });
            Console.WriteLine(count);
        }

        //删除数据
        public int Delete(User2 user)
        {
            int count = con.Execute($"delete from person where id = @id;",user);
            return count;
        }

        //修改数据
        public  int Update(User2 user)
        {
            int count = con.Execute($"update person set name = @Name,birthday = @Birthday where id = @id;",user);
            return count;
        }


        //查找数据
        public List<User2> Search()
        {
            List<User2> users = con.Query<User2>("select * from person;").ToList();
            return users;

        }
        //查找数据
        public List<User2> getByPage(int pageIndex,int size)
        {
            List<User2> users = con.Query<User2>("select * from person limit "+(pageIndex*size)+","+size+";").ToList();
            return users;

        }

        //简单事务
        public void Transaction(MySqlConnection connection)
        {
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();
            int count = connection.Execute($"insert into person values(null,'王五','{DateTime.Now}');");
            transaction.Commit();
            connection.Clone();
        }
        //简单事务，回滚
        private static void TransactionRoolback(MySqlConnection connection)
        {
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();
            int count = connection.Execute($"delete from person;", transaction);
            transaction.Rollback();
            connection.Clone();
        }

        //简单存储过程
        private static void StoredProcedures(MySqlConnection connection)
        {
            var user = connection.Query<User2>("spGetUser", new { uid = 10 }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            Console.WriteLine(user.Name);
        }
        #endregion

        #region 其他特性

        //动态类型转化
        private static void DynamicObjects(MySqlConnection connection)
        {
            var rows = connection.Query("select * from person;");
            var id = ((int)rows.First().id);
            Console.WriteLine(id);
        }

        //集合参数化
        private static void IEnumerableParameterize(MySqlConnection connection)
        {
            //下面的写法等价于  connection.Query<int>("select * from person where Id in (@Ids1, @Ids2)", new { Ids1 = 10, Ids2 = 11 });
            var users = connection.Query<int>("select * from person where id in @Ids", new { Ids = new int[] { 10, 11 } });
            Console.WriteLine(users.Count());
        }

        //多对象映射 1-1映射
        public List<Post> MultiMapping()
        {
            var sql =
                @"select * from post p 
                left join person u on u.id = p.uid 
                Order by p.id";

            var data = con.Query<Post, User2, Post>(sql, (post, user) => { post.Owner = user; return post; });
            var item = data.First();
            List<Post> list = new List<Post>();
            foreach(Post i in data)
            {
                list.Add(i);
            }
            return list;
        }
        public User2 mutipping()
        {

            //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
            string sql = "SELECT * FROM person u LEFT JOIN post p ON u.id = p.uid WHERE u.id = @id";
            User2 lookup = null;
            //Query<TFirst, TSecond, TReturn>
            var data = con.Query<User2, Post, User2>(sql,(user, post) =>
           {
                //扫描第一条记录，判断非空和非重复
                if (lookup == null || lookup.Id != user.Id)
                               lookup = user;
               //书对应的书评非空，加入当前书的书评List中，最后把重复的书去掉。
               if (post != null)
               {
                   lookup.posts.Add(post);
               }
                           return lookup;
           }, new { id = 15 }).Distinct().SingleOrDefault();
            return data;
        }
        //一次获取多个对象
        private static void MultipleResults(MySqlConnection connection)
        {
            var sql =
                @"
                select * from person where id = @uid;
                select * from post where id = @pid";

            using (var multi = connection.QueryMultiple(sql, new { uid = 10, pid = 1 }))
            {
                var users = multi.Read<User2>().ToList();
                Console.WriteLine(users.First().Name);
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


