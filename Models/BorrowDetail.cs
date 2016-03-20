using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    //借阅详细表
    [Serializable]
    public class BorrowDetail
    {
        public int BorrowDetailId { set; get; }

        public string BorrowId { set; get; }

        public int BookId { set; get; }

        public int BorrowCount { set; get; }

        public int ReturnCount { set; get; }

        public int NonReturnCount { set; get; }

        public DateTime Expire { set; get; }
    }
}
