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
        public int GetCountByBarCode(string barCode)
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
                new SqlParameter("@PublishDate",objBook.PublisherDate),
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
    }     
}
