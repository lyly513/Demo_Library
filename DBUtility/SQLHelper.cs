using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DBUtility
{

    //针对SQLServer数据库的通用访问类
    public class SQLHelper
    {
        //封装数据库连接字符串
        private static string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();

        #region 封装格式化SQL语句执行的各种方法

        public static int Update(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                //将异常信息写入日志
                
            }
        }

        #endregion

        #region 封装带参数SQL语句执行的各种方法

        #endregion

        #region 封装调用存储过程执行的各种方法

        #endregion

        #region 其他方法

        private static void WriteLog(string log)
        {
            FileStream fs = new FileStream("sqlhelper.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString() + "" + log);
            sw.Close();
            fs.Close();
        }

        #endregion

    }
}
