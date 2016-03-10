using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    //图书实体类
    [Serializable]
    public class Book
    {
        private int BookId { set; get; }

        public string BarCode { get; set; }

        public string BookName { get; set; }

        public string Author { set; get; }

        public int PublisherId { set; get; }

        public DateTime PublisherDate { set; get; }

        public int BookCategory { set; get; }

        public double UnitPrice { set; get; }

        public string BookImage { set; get; }

        public int BookCount { set; get; }

        public int Remainder { set; get; }

        public string BookPosition { set; get; }

        public DateTime RegTime { set; get; }

        //扩展属性
        public string PublisherName { set; get; }
    }
}
