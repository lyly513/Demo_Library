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
        
        // 执行不带参数的更新SQL语句
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
                string errorInfo = "调用public static int Update(string sql)方法时发生错误：" + ex.Message;
                WriteLog(errorInfo);
                throw new Exception(errorInfo);

            }
            finally
            {
                conn.Close();
            }
        }

        public static object GetSingleResult(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //将异常信息写入日志
                string errorInfo = "调用public static int GetSingleResult(string sql)方法时发生错误：" + ex.Message;
                WriteLog(errorInfo);
                throw new Exception(errorInfo);

            }
            finally
            {
                conn.Close();
            }
        }

        public static SqlDataReader GetReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                //将异常信息写入日志
                string errorInfo = "调用public static int GetSingleResult(string sql)方法时发生错误：" + ex.Message;
                WriteLog(errorInfo);
                throw new Exception(errorInfo);
            }
            
        }

        public static DataSet GetDataSet(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);//创建数据适配器对象
            DataSet ds = new DataSet();//创建一个内存数据集
            try
            {
                conn.Open();
                da.Fill(ds);//使用数据适配器填充数据集
                return ds;
            }
            catch (Exception ex)
            {
                //将异常信息写入日志
                string errorInfo = "调用public static int GetDataSet(string sql)方法时发生错误：" + ex.Message;
                WriteLog(errorInfo);
                throw new Exception(errorInfo);

            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region 封装带参数SQL语句执行的各种方法
        // 执行带参数的更新SQL语句
        public static int Update(string sql, params SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(param);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //将异常信息写入日志
                string errorInfo = "调用public static int Update(string sql, params SqlParameter[] param)方法时发生错误：" + ex.Message;
                WriteLog(errorInfo);
                throw new Exception(errorInfo);

            }
            finally
            {
                conn.Close();
            }
        }

        public static object GetSingleResult(string sql, params SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(param);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //将异常信息写入日志
                string errorInfo = "调用public static int GetSingleResult(string sql, params SqlParameter[] param)方法时发生错误：" + ex.Message;
                WriteLog(errorInfo);
                throw new Exception(errorInfo);

            }
            finally
            {
                conn.Close();
            }
        }

        public static SqlDataReader GetReader(string sql, params SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(param);
            try
            {                
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                //将异常信息写入日志
                string errorInfo = "调用public static int GetReader(string sql, params SqlParameter[] param)方法时发生错误：" + ex.Message;
                WriteLog(errorInfo);
                throw new Exception(errorInfo);
            }
        }

        public static DataSet GetDataSet(string sql, params SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(param);
            SqlDataAdapter da = new SqlDataAdapter(cmd);//创建数据适配器对象
            DataSet ds = new DataSet();//创建一个内存数据集
            try
            {
                conn.Open();
                da.Fill(ds);//使用数据适配器填充数据集
                return ds;
            }
            catch (Exception ex)
            {
                //将异常信息写入日志
                string errorInfo = "调用public static int GetDataSet(string sql, params SqlParameter[] param)方法时发生错误：" + ex.Message;
                WriteLog(errorInfo);
                throw new Exception(errorInfo);

            }
            finally
            {
                conn.Close();
            }
        }

        #endregion
        
        
        #region 封装调用存储过程执行的各种方法
        
        //执行调用存储过程更新的方法
        
        public static int UpdateByProcedure(string ProcName, params SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure; //指定执行存储过程操作 
            cmd.CommandText = ProcName; //存储过程名称 

            ////对应存储过程QueryInfoByName的第一个参数@BarCode 
            //SqlParameter BarCode = new SqlParameter("@BarCode", SqlDbType.VarChar, 20);
            ////指定参数@BarCode要转入的值 
            //BarCode.Value = txt_name;

            ////对应存储过程QueryInfoByName的第二个参数@BookName 
            //SqlParameter BookName = new SqlParameter("@BookName", SqlDbType.VarChar, 100);
            ////指定参数@age要转入的值 
            //BookName.Value = txt_age; 

            cmd.Parameters.AddRange(param);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //将异常信息写入日志
                string errorInfo = "调用public static int UpdateByProcedure(string sql, params SqlParameter[] param)方法时发生错误：" + ex.Message;
                WriteLog(errorInfo);
                throw new Exception(errorInfo);
            }
            finally
            {
                conn.Close();
            }
        }
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
