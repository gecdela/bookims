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
    public class BookDAO
    {
        public MySqlConnection con = new MySqlConnection("server=localhost;database=bookims;user id=root;password=123456;charset='gbk'");


        #region 一般的数据库操作
        //插入数据
        public int Insert(Book book)
        {
            int count = con.Execute($"insert into book (book_id,book_name,book_author,book_price,status) values(null,@BookName,@BookAuthor,@BookPrice,@Status);", book);

            return count;
        }

        //插入数据，集合
        private static void InsertCollection(MySqlConnection connection)
        {
            int count = connection.Execute(@"insert into book values (null,@a, @b)", new[] { new { a = "test1", b = DateTime.Now }, new { a = "test2", b = DateTime.Now }, new { a = "test3", b = DateTime.Now } });
            Console.WriteLine(count);
        }

        //删除数据
        public int Delete(Book book)
        {
            int count = con.Execute($"delete from book where book_id = @BookId;", book);
            return count;
        }

        public Book Search(Book book)
        {
            Book b = new Book();
            b = con.QueryFirst<Book>(@"select book_id BookId,book_name BookName,book_author BookAuthor,book_price BookPrice,book_pic BookPic,status from book where book_id=@BookId;", book);
            return b;
        }
        public Book Search(String bookId)
        {
            Book b = new Book();
            b = con.QueryFirst<Book>(@"select book_id BookId,book_name BookName,book_author BookAuthor,book_price BookPrice,book_pic BookPic,status from book where book_id=@bookId",new { bookId = bookId });
            return b;
        }
        //修改数据
        public int Update(Book book)
        {
            int count = con.Execute($"update book set book_name = @BookName,book_author = @BookAuthor,book_price = @BookPrice,status = @Status where book_id = @BookId;", book);
            return count;
        }


        //查找数据
        public List<Book> Search()
        {
            List<Book> books = con.Query<Book>("select book_id BookId,book_name BookName,book_author BookAuthor,book_price BookPrice,book_pic BookPic,status from book;").ToList();
            return books;

        }
        //查找数据
        public List<Book> getByPage(int pageIndex, int size)
        {
            List<Book> books = con.Query<Book>("select book_id BookId,book_name BookName,book_author BookAuthor,book_price BookPrice,book_pic BookPic,status from book limit " + (pageIndex * size) + "," + size + ";").ToList();
            return books;

        }
        //按条件搜索
        public List<Book> Query(String condition)
        {
            List<Book> books = con.Query<Book>("SELECT book_id BookId,book_name BookName,book_author BookAuthor,book_price BookPrice,book_pic BookPic,status FROM book where CONCAT(book_id, book_name,book_author,book_price,status) like '%" + condition+"%'").ToList();
            //List<Book> books = con.Query<Book>("SELECT * FROM book where CONCAT(book_id, book_name,book_author,book_price,status) like '%@Condition%'", new { Condition = condition }).ToList();
            return books;
        }
        public List<Book> Query(Book book)
        {
            List<Book> books = con.Query<Book>("SELECT * FROM `book` where  book_id like '%@BookId%'and book_name like '%@BookName%'and book_author like '%@BookAuthor%'and book_price like'%@BookPrice%' and status like '%@Status%'", book).ToList();
            return books;
        }
        //简单事务
        public void Transaction(MySqlConnection connection)
        {
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();
            int count = connection.Execute($"insert into book values(null,'王五','{DateTime.Now}');");
            transaction.Commit();
            connection.Clone();
        }
        //简单事务，回滚
        private static void TransactionRoolback(MySqlConnection connection)
        {
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();
            int count = connection.Execute($"delete from book;", transaction);
            transaction.Rollback();
            connection.Clone();
        }

        //简单存储过程
        private static void StoredProcedures(MySqlConnection connection)
        {
            var Book = connection.Query<Book>("spGetBook", new { uid = 10 }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            Console.WriteLine(Book.BookName);
        }
        #endregion

        #region 其他特性

        //动态类型转化
        private static void DynamicObjects(MySqlConnection connection)
        {
            var rows = connection.Query("select * from book;");
            var id = ((int)rows.First().id);
            Console.WriteLine(id);
        }




        //集合参数化
        private static void IEnumerableParameterize(MySqlConnection connection)
        {
            //下面的写法等价于  connection.Query<int>("select * from book where Id in (@Ids1, @Ids2)", new { Ids1 = 10, Ids2 = 11 });
            var Books = connection.Query<int>("select * from book where id in @Ids", new { Ids = new int[] { 10, 11 } });
            Console.WriteLine(Books.Count());
        }

        //多对象映射 1-1映射
        //public List<Post> MultiMapping()
        //{
        //    var sql =
        //        @"select * from post p 
        //        left join book u on u.id = p.uid 
        //        Order by p.id";

        //    var data = con.Query<Post, Book, Post>(sql, (post, book) => { post.Owner = book; return post; });
        //    var item = data.First();
        //    List<Post> list = new List<Post>();
        //    foreach (Post i in data)
        //    {
        //        list.Add(i);
        //    }
        //    return list;
        //}
        //public Book mutipping()
        //{

        //    //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
        //    string sql = "SELECT * FROM book u LEFT JOIN post p ON u.id = p.uid WHERE u.id = @id";
        //    Book lookup = null;
        //    //Query<TFirst, TSecond, TReturn>
        //    var data = con.Query<Book, Post, Book>(sql, (Book, post) =>
        //    {
        //        //扫描第一条记录，判断非空和非重复
        //        if (lookup == null || lookup.Id != Book.Id)
        //            lookup = Book;
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
                select * from book where id = @uid;
                select * from post where id = @pid";

            using (var multi = connection.QueryMultiple(sql, new { uid = 10, pid = 1 }))
            {
                var Books = multi.Read<Book>().ToList();
                Console.WriteLine(Books.First().BookName);
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


