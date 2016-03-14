using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL;
using Models;

namespace BLL
{
    //图书管理业务逻辑
    public class BookManager
    {
        private BookService objBookService = new BookService();
        
        //获取全部的图书分类
        public List<Catetgory> GetAllCategory()
        {
            return objBookService.GetAllCategory();
        }
        //获取全部的出版社信息
        public List<Publisher> GetAllPublisher()
        {
            return objBookService.GetAllPublisher();  
        }
        //判断图书条码是否存在
        public bool BarCodeIsExisted(string barCode)
        {
            //老师写成了CarCode...
            int count = objBookService.GetCountByCarCode(barCode);
            if (count == 1)
                return true;
            else
                return false;
        }

        //添加图书
        public int AddBook(Book objBook)
        {
            return objBookService.AddBook(objBook);
        }

        //根据图书条码查询图书对象
        public Book GetBookByBarCode(string barCode)
        {
            return objBookService.GetBookByBarCode(barCode);
        }

        //更新图书收藏总数
        public int AddBookCount(string barCode, int bookCount)
        {
            return objBookService.AddBookCount(barCode, bookCount);
        }

        /// <summary>
        /// 根据组合条件查询信息
        /// </summary>
        /// <param name="categoryId">图书分类编号</param>
        /// <param name="barCode">图书条码</param>
        /// <param name="bookName">图书名称</param>
        /// <returns>图书对象集合</returns>
        public List<Book> GetBooks(int categoryId, string barCode, string bookName)
        {
            return objBookService.GetBooks(categoryId, barCode, bookName);
        }
    }
}
