using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;
using DBUtility;
using System.Data.SqlClient;

namespace DAL
{
    //管理员数据访问类
    public class SysAdminService
    {
        //根据登录账号和密码从数据库对比
        //<param name = "objAdmin">包含账号和密码的管理员对象</param>
        //返回管理员对象

        //根据登录账户和密码查询数据库
        public SysAdmin AdminLogin(SysAdmin objAdmin)
        {
            //定义登录的SQL语句
            string sql = "select AdminName,StatusId,RoleId from SysAdmins where ";
            sql += "AdminId=@AdminId and LoginPwd=@LoginPwd";
            //封装参数
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@AdminId",objAdmin.AdminId),
                //下面的LoginPwd可能写错为LoginPws
                new SqlParameter("@LoginPwd",objAdmin.LoginPwd)
            };
            //执行查询
            SqlDataReader objReader = SQLHelper.GetReader(sql, param);
            //处理查询结果
            if(objReader.Read())
            {
                objAdmin.AdminName = objReader["AdminName"].ToString();
                objAdmin.StatusId = Convert.ToInt32(objReader["StatusId"]);
                objAdmin.RoleId = Convert.ToInt32(objReader["RoleId"]);
            }
            else
            {
                objAdmin = null;//如果账号或密码不正确，则清空当前对象

            }
            objReader.Close();
            return objAdmin;
        }
    }
}
