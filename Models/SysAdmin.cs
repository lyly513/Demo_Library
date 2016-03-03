using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    //管理员实体类
    [Serializable]
    public class SysAdmin
    {
        public int AdminId { set; get; }

        public string AdminName { set; get; }

        public string LoginPwd { get; set; }

        public int RoleId { get; set; }

        public int StatusId { get; set; }
    }
}
