using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    //还书实体类
    [Serializable]
    public class ReturnBook
    {
        public int ReturnId { set; get; }

        public int BorrowDetailId { set; get; }

        public int ReturnCount { set; get; }

        public DateTime ReturnDate { set; get; }

        public string AdminName_R { set; get; }
    }
}
