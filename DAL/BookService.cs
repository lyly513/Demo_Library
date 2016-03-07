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

        //获取全部的图书分类
        public List<Catetgory> GetAllCategory()
        {
            string sql = "select CategoryId, CategoryName from Category";
            List<Catetgory> list=new List<Catetgory>();
            SqlDataReader objReader = SQLHelper.GetReader(sql);
            //循环读取并封装对象
            while(objReader.Read())
            {
                list.Add(new Catetgory()
                {
                    CategoryId=Convert.ToInt32(objReader["CategoryId"]),
                    CategoryName=objReader["CategoryName"].ToString()
                });
            }
            objReader.Close();
            return list;
        }
    }
}
