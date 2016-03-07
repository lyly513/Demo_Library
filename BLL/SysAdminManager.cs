using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL;
using Models;


namespace BLL
{


    public class SysAdminManager
    {
        //创建数据访问对象
        private SysAdminService objSysAdminService = new SysAdminService();
        
        //第一个作用：起到数据传递作用
        public SysAdmin AdminLogin(SysAdmin objAdmin)
        {
            return objSysAdminService.AdminLogin(objAdmin);
        }

        //第二个作用:起到业务逻辑处理(暂无需使用)

    }
}
