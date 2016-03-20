using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    //借阅信息主表
    [Serializable]
    public class BorrowInfo
    {
        public string BorrowId { set; get; }

        public int ReaderId { set; get; }

        public DateTime BorrowDate { set; get; }

        public string AdminName_B { set; get; }
    }
}
