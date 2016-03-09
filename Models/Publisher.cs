using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    //出版社实体类
    [Serializable]
    public class Publisher
    {
        public int PublisherId { set; get; }

        public string PublisherName { set; get; }

    }
}
