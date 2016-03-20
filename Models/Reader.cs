using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    //读者实体类
    [Serializable]
    public class Reader
    {
        public int ReaderId { set; get; }

        public string ReadingCard { set; get; }

        public string ReaderName { set; get; }

        public string Gender { set; get; }

        public string IDCard { set; get; }

        public string ReaderAddress { set; get; }

        public string PostCode { set; get; }

        public string PhoneNumber { set; get; }

        public int RoleId { set; get; }

        public string ReaderImage { set; get; }

        public DateTime RegTime { set; get; }

        public string ReaderPwd { set; get; }

        public int AdminId { set; get; }

        public int StatusId { set; get; }

        //定义扩展属性

        public string RoleName { get; set; }//读者角色名称

        public string StatusDesc { set; get; }//账号状态描述

    }



}
