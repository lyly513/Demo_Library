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
    }
}
