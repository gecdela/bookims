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
    public class RecordDAO
    {
        public MySqlConnection con = new MySqlConnection("server=localhost;database=bookims;user id=root;password=123456;charset='gbk'");


        #region 一般的数据库操作
        //插入数据
        public int Insert(Record record)
        {
            int count = con.Execute($"insert into record (record_Id,record_id,user_id,start_date,deadline) values(null,@BookId,@UserId,@StartDate,@Deadline);", record);

            return count;
        }

        //插入数据，集合
        private static void InsertCollection(MySqlConnection connection)
        {
            int count = connection.Execute(@"insert into record values (null,@a, @b)", new[] { new { a = "test1", b = DateTime.Now }, new { a = "test2", b = DateTime.Now }, new { a = "test3", b = DateTime.Now } });
            Console.WriteLine(count);
        }

        //删除数据
        public int Delete(Record record)
        {
            int count = con.Execute($"delete from record where record_id = @RecordId;", record);
            return count;
        }


        //修改数据
        public int Update(Record record)
        {
            int count = con.Execute($"update record set record_name = @RecordName,record_author = @RecordAuthor,record_price = @RecordPrice,status = @Status where record_id = @RecordId;", record);
            return count;
        }


        //查找数据
        public List<Record> Search()
        {
            List<Record> records = con.Query<Record>("select record_id RecordId,record_name RecordName,record_author RecordAuthor,record_price RecordPrice,record_pic RecordPic,status from record;").ToList();
            return records;

        }

       public int Insert(int bookId, string userId)
        {

            string deadLine = DateTime.Now.AddDays(30).ToString("yyyy年MM月dd日");
            string startDate = DateTime.Now.ToString("yyyy年MM月dd日");
            int count = con.Execute($"insert into record(record_Id,book_id,user_id,start_date,deadline) values(null,@BookId,@UserId,@StartDate,@Deadline);", new { BookId = bookId, UserId = userId, StartDate = startDate, DeadLine = deadLine });
            //int count = con.Execute($"insert into record(record_Id,book_id,user_id,start_date,deadline) values(null,@BookId,@UserId,@StartDate,@Deadline);",record);
            return count;
        }

        //查找数据
        public List<Record> GetRecordByPage(int pageIndex,int size)
        {

            //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
            string sql = @"select r.record_Id RecordId,r.start_date StartDate,r.deadline DeadLine,b.book_id BookId,b.book_name BookName,b.book_author BookAuthor,b.book_price BookPrice,b.status,b.book_pic BookPic,u.user_id UserId,u.user_name UserName,u.tel Tel,u.email Email from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id Order by r.record_Id limit " + (pageIndex * size) + "," + size ;
            //string sql = @"select record_Id,book_id,user_id from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id Order by r.record_Id";

            Record lookup = null;
            //Query<TFirst, TSecond, TReturn>
            var data = con.Query<Record, Book, User, Record>(sql, (record, book, user) =>
            {
                record.user = user;
                record.book = book;
                return record;
            }, splitOn: "RecordId,BookId,UserId");
            List<Record> list = new List<Record>();
            foreach (Record i in data)
            {
                list.Add(i);
            }
            return list;
        }
 
        public List<Record> GetRecordList()
        {

            //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
            string sql = @"select r.record_Id RecordId,r.start_date StartDate,r.deadline DeadLine,b.book_id BookId,b.book_name BookName,b.book_author BookAuthor,b.book_price BookPrice,b.status,b.book_pic BookPic,u.user_id UserId,u.user_name UserName,u.tel Tel,u.email Email from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id Order by r.record_Id";
            //string sql = @"select record_Id,book_id,user_id from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id Order by r.record_Id";

            Record lookup = null;
            //Query<TFirst, TSecond, TReturn>
            var data = con.Query<Record, Book, User,Record>(sql, (record, book, user) =>
            {
                record.user = user;
                record.book = book;
                return record;
            }, splitOn: "RecordId,BookId,UserId");
            List<Record> list = new List<Record>();
            foreach (Record i in data)
            {
                list.Add(i);
            }
            return list;
        }
        public List<Record> GetRecordList(String userId)
        {

            //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
            //string sql = @"select r.record_Id RecordId,r.start_date StartDate,r.deadline DeadLine,b.book_id BookId,b.book_name BookName,b.book_author BookAuthor,b.book_price BookPrice,b.status,b.book_pic BookPic,u.user_id UserId,u.user_name UserName,u.tel Tel,u.email Email from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id where user_id = @userId Order by r.record_Id",new { userId=userId});
            //string sql = @"select record_Id,book_id,user_id from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id Order by r.record_Id";

            Record lookup = null;
            //Query<TFirst, TSecond, TReturn>
            var data = con.Query<Record, Book, User, Record>(@"select r.record_Id RecordId,r.start_date StartDate,r.deadline DeadLine,b.book_id BookId,b.book_name BookName,b.book_author BookAuthor,b.book_price BookPrice,b.status,b.book_pic BookPic,u.user_id UserId,u.user_name UserName,u.tel Tel,u.email Email from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id where r.user_id = @userId Order by r.record_Id",  (record, book, user) =>
            {
                record.user = user;
                record.book = book;
                return record;
            }, new { userId = userId }, splitOn: "RecordId,BookId,UserId");
            List<Record> list = new List<Record>();
            foreach (Record i in data)
            {
                list.Add(i);
            }
            return list;
        }
        //查找数据
        public List<Record> GetRecordByPage(String userId, int pageIndex, int size)
        {

            //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
            string sql = @"select r.record_Id RecordId,r.start_date StartDate,r.deadline DeadLine,b.book_id BookId,b.book_name BookName,b.book_author BookAuthor,b.book_price BookPrice,b.status,b.book_pic BookPic,u.user_id UserId,u.user_name UserName,u.tel Tel,u.email Email from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id where r.user_id = @userId Order by r.record_Id limit " + (pageIndex * size) + "," + size;
            //string sql = @"select record_Id,book_id,user_id from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id Order by r.record_Id";

            Record lookup = null;
            //Query<TFirst, TSecond, TReturn>
            var data = con.Query<Record, Book, User, Record>(sql, (record, book, user) =>
            {
                record.user = user;
                record.book = book;
                return record;
            }, new { userId = userId }, splitOn: "RecordId,BookId,UserId");
            List<Record> list = new List<Record>();
            foreach (Record i in data)
            {
                list.Add(i);
            }
            return list;
        }
        //按条件搜索(用户)
        public List<Record> Query(String condition,String userId)
        {

            //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
            string sql = @"select r.record_Id RecordId,r.start_date StartDate,r.deadline DeadLine,b.book_id BookId,b.book_name BookName,b.book_author BookAuthor,b.book_price BookPrice,b.status,b.book_pic BookPic,u.user_id UserId,u.user_name UserName,u.tel Tel,u.email Email from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id where concat(r.record_Id,r.start_date,r.deadline,b.book_id,b.book_name,b.book_author,b.book_price,b.status,b.book_pic,u.user_id,u.user_name,u.tel,u.email) like '%"+condition+"%' order by r.record_Id";
            //string sql = @"select record_Id,book_id,user_id from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id Order by r.record_Id";

            Record lookup = null;
            //Query<TFirst, TSecond, TReturn>
            var data = con.Query<Record, Book, User, Record>(sql, (record, book, user) =>
            {
                record.user = user;
                record.book = book;
                return record;
            }, new { userId = userId }, splitOn: "RecordId,BookId,UserId");
            List<Record> list = new List<Record>();
            foreach (Record i in data)
            {
                list.Add(i);
            }
            return list;
        }
    
    //按条件搜索
    public List<Record> Query(String condition)
    {

        //查询图书时，同时查找对应的书评，并存在List中。实现1--n的查询操作
        string sql = @"select r.record_Id RecordId,r.start_date StartDate,r.deadline DeadLine,b.book_id BookId,b.book_name BookName,b.book_author BookAuthor,b.book_price BookPrice,b.status,b.book_pic BookPic,u.user_id UserId,u.user_name UserName,u.tel Tel,u.email Email from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id where concat(r.record_Id,r.start_date,r.deadline,b.book_id,b.book_name,b.book_author,b.book_price,b.status,b.book_pic,u.user_id,u.user_name,u.tel,u.email) like '%" + condition + "%' order by r.record_Id";
        //string sql = @"select record_Id,book_id,user_id from record r left join book b on r.book_id = b.book_id left join user u on r.user_id = u.user_id Order by r.record_Id";

        Record lookup = null;
        //Query<TFirst, TSecond, TReturn>
        var data = con.Query<Record, Book, User, Record>(sql, (record, book, user) =>
        {
            record.user = user;
            record.book = book;
            return record;
        }, splitOn: "RecordId,BookId,UserId");
        List<Record> list = new List<Record>();
        foreach (Record i in data)
        {
            list.Add(i);
        }
        return list;
    }
    public void close()
        {
            con.Close();
        }

        #endregion

    }

}


