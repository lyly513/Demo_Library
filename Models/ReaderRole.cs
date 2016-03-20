using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    //读者角色实体类
    [Serializable]
    public class ReaderRole
    {
        public int RoleId { set; get; }

        public string RoleName { set; get; }

        public int AllowDay { set; get; }

        public int AllowCount { get; set; } 

    }
}
