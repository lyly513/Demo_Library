using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using Models;
using DBUtility;

namespace DAL
{
    //老师的英语不好，此处单词有特别
    public class BookService
    {

        #region 封装图书分类和出版社
        //获取全部的图书分类
        public List<Catetgory> GetAllCategory()
        {
            string sql = "select CategoryId, CategoryName from Categories";
            List<Catetgory> list = new List<Catetgory>();
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            //循环读取并封装对象
            while (objReader.Read())
            {
                list.Add(new Catetgory()
                {
                    CategoryId = Convert.ToInt32(objReader["CategoryId"]),
                    CategoryName = objReader["CategoryName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }
        //获取全部的出版社信息
        public List<Publisher> GetAllPublisher()
        {
            string sql = "select PublisherId, PublisherName from Publishers";
            List<Publisher> list = new List<Publisher>();
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            //循环读取并封装对象
            while (objReader.Read())
            {
                list.Add(new Publisher()
                {
                    PublisherId = Convert.ToInt32(objReader["PublisherId"]),
                    PublisherName = objReader["PublisherName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }
        #endregion

        #region 添加图书
        //判断条码是否已经存在
        //老师写成了CarCode...
        public int GetCountByCarCode(string barCode)
        {
            string sql = "select count(*) from Books where BarCode=@BarCode";
            SqlParameter[] param = new SqlParameter[] { new SqlParameter("@BarCode", barCode) };
            return Convert.ToInt32(SQLHelper.GetSingleResult(sql, param));
        }

        //添加图书对象
        public int AddBook(Book objBook)
        {
            //封装参数
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@BarCode",objBook.BarCode),
                new SqlParameter("@BookName",objBook.BookName),
                new SqlParameter("@Author",objBook.Author),
                new SqlParameter("@PublisherId",objBook.PublisherId),
                new SqlParameter("@PublishDate",objBook.PublishDate),
                new SqlParameter("@BookCategory",objBook.BookCategory),
                new SqlParameter("@UnitPrice",objBook.UnitPrice),
                new SqlParameter("@BookImage",objBook.BookImage),
                new SqlParameter("@BookCount",objBook.BookCount),
                new SqlParameter("@Remainder",objBook.Remainder),
                new SqlParameter("@BookPosition",objBook.BookPosition)
            };
            //调用通用数据访问方法提交对象
            return SQLHelper.UpdateByProcedure("usp_AddBook", param);
        }
        #endregion

        #region 图书上架

        //根据图书条码查询图书对象
        //这里老师参数写的是barCode(差异)
        public Book GetBookByBarCode(string barCode)
        {
            string sql = "select BookId, BarCode, BookName, Author, PublisherId, PublishDate, BookCategory, UnitPrice, BookImage, BookCount, Remainder, BookPosition, RegTime, PublisherName, CategoryName from Books";
            sql += "inner join Publishers on Publishers.PublisherId=Books.PublisherId";
            sql += "inner join Categories on Books.BookCategory=Categories.CategoryId";
            sql += "where BarCode=@BarCode";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@BarCode",barCode)
            };
            SqlDataReader objReader = SQLHelper.GetReader(sql, param);
            Book objBook = null;
            if(objReader.Read())
            {
                objBook = new Book()
                {
                    Author = objReader["Author"].ToString(),
                    BarCode = objReader["BarCode"].ToString(),
                    BookCategory = Convert.ToInt32(objReader["BookCategory"]),
                    CategoryName = objReader["CategoryName"].ToString(),
                    BookCount = Convert.ToInt32(objReader["BookCount"]),
                    BookId = Convert.ToInt32(objReader["BookId"]),
                    BookName = objReader["BookName"].ToString(),
                    BookPosition = objReader["BookPosition"].ToString(),
                    PublishDate = Convert.ToDateTime(objReader["PublishDate"]),
                    PublisherId = Convert.ToInt32(objReader["BookCategory"]),
                    PublisherName = objReader["PublisherName"].ToString(),
                    Remainder = Convert.ToInt32(objReader["Remainder"]),
                    UnitPrice = Convert.ToDouble(objReader["UnitPrice"]),
                    RegTime = Convert.ToDateTime(objReader["RegTime"]),
                    BookImage = objReader["BookImage"] is DBNull ? "" : objReader["BookImage"].ToString()
                };
            }
            objReader.Close();
            return objBook;
        }

        //更新图书收藏总数
        public int AddBookCount(string barCode, int bookCount)
        {
            string sql = "update Books set BookCount+@BookCount where BarCode=@BarCode";
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@BarCode",barCode),
                new SqlParameter("@BookCount",bookCount)
            };
            return SQLHelper.Update(sql, param);
        }


        #endregion
    }     
}
