using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;
using DBUtility;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    
    public class ReaderService
    {
        //会员办证（添加读者信息）
        public int AddReader(Reader objReader)
        {
            //定义SQL语句
            string sql="insert into Readers(ReadingCard, ReaderName, Gender, ";
            sql+="IDCard, ReaderAddress, PostCode, PhoneNumber, RoleId, ReaderImage, ReaderPwd, AdminId) ";
            sql+=" values(@ReadingCard,@ReaderName,@Gender,@IDCard,@ReaderAddress, ";
            sql+=" @PostCode,@PhoneNumber,@RoleId,@ReaderImage,@ReaderPwd,@AdminId) ";

            //定义参数
            SqlParameter[] param=new SqlParameter[]
            {
                new SqlParameter("@ReadingCard",objReader.ReadingCard),
                new SqlParameter("@ReaderName",objReader.ReaderName),
                new SqlParameter("@Gender",objReader.Gender),
                new SqlParameter("@IDCard",objReader.IDCard),
                new SqlParameter("@ReaderAddress",objReader.ReaderAddress),
                new SqlParameter("@PostCode",objReader.PostCode),
                new SqlParameter("@PhoneNumber",objReader.PhoneNumber),
                new SqlParameter("@RoleId",objReader.RoleId),
                new SqlParameter("@ReaderImage",objReader.ReaderImage),
                new SqlParameter("@ReaderPwd",objReader.ReaderPwd),
                new SqlParameter("@AdminId",objReader.AdminId)
            };
            return SQLHelper.Update(sql,param);//（疑问）
        }
        
        //修改读者信息
        public int EditReader(string readerId, Reader objReader)
        {
            string sql = "update Readers set ReaderName=@ReaderName, Gender=@Gender, ReaderAddress=@ReaderAddress, PostCode=@PostCode, PhoneNumber=@PhoneNumber, RoleId=@RoleId, ReaderImage=@ReaderImage, ReaderId=@ReaderId";
            //定义参数
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ReaderName",objReader.ReaderName),
                new SqlParameter("@Gender",objReader.Gender),
                new SqlParameter("@ReaderAddress",objReader.ReaderAddress),
                new SqlParameter("@PostCode",objReader.PostCode),
                new SqlParameter("@PhoneNumber",objReader.PhoneNumber),
                new SqlParameter("@RoleId",objReader.RoleId),
                new SqlParameter("@ReaderImage",objReader.ReaderImage),
                new SqlParameter("@ReaderId",objReader.ReaderId)
            };
            return SQLHelper.Update(sql,param);
        }
        //借阅证挂失
        public int ForbiddenReaderCard(string readerId)
        {
            string sql = "update Readers set StatusId=0 where ReaderId=@ReaderId";
            return SQLHelper.Update(sql,new SqlParameter[]{new SqlParameter("@ReaderId",readerId)});
        }

        //查询全部会员角色
        public DataTable GetAllReaderRole()
        {
            string sql = "select RoleId,RoleName from ReaderRoles";
            return SQLHelper.GetDataSet(sql).Tables[0];
        }

        //根据借阅证号查询读者信息
        public Reader GetReaderByReadingCard(string readingCard)
        {
            return GetReaderBySQL(" where ReadingCard=@ReadingCard", new SqlParameter[] { new SqlParameter("@ReadingCard", readingCard) });
        }
        //根据身份证号查询读者信息
        public Reader GetReaderByIDCard(string IDCard)
        {
            return GetReaderBySQL(" where IDCard=@IDCard ", new SqlParameter[] { new SqlParameter("@IDCard", IDCard) });
        }

        //复用方法
        private Reader GetReaderBySQL(string whereSql, SqlParameter[] param)
        {
            string sql = "select ReaderId, ReadingCard, ReaderName, Gender, IDCard, ReaderAddress, PostCode, PhoneNumber, RoleId, ReaderImage, StatusId, RoleName from Readers ";
            sql += " inner join ReaderRoles on ReaderRoles.RoleId=Readers.RoleId ";
            sql += whereSql;

            //执行查询
            SqlDataReader objReader = SQLHelper.GetReader(sql, param);
            Reader reader = null;
            if (objReader.Read())
            {
                reader = new Reader()
                {
                    ReadingCard = objReader["ReadingCard"].ToString(),
                    ReaderName = objReader["ReaderName"].ToString(),
                    RoleId = Convert.ToInt32(objReader["RoleId"]),
                    ReaderImage = objReader["ReaderImage"] is DBNull ? "" : objReader["ReaderImage"].ToString(),
                    ReaderId = Convert.ToInt32(objReader["ReaderId"]),
                    StatusId = Convert.ToInt32(objReader["StatusId"]),
                    PhoneNumber = objReader["PhoneNumber"].ToString(),
                    ReaderAddress = objReader["ReaderAddress"].ToString(),
                    PostCode = objReader["PostCode"].ToString(),
                    Gender = objReader["Gender"].ToString()
                };
            }
            objReader.Close();
            return reader;
        }


        //根据会员角色查询读者信息（同时找到该角色的会员总数）
        public List<Reader> GetReaderByRole(string roleId, out int readerCount)//使用out参数，返回多个数据
        {
            string sql = "select ReaderId, ReaderCard, ReaderName, Gender, PhoneNumber, ReaderAddress, RegTime, StatusId from Readers";
            sql += " where RoleId=@RoleId; ";
            sql += " select readerCount=count(*) from Readers where RoleId=@RoleId ";

            List<Reader> readerList = new List<Reader>();

            SqlParameter[] param = new SqlParameter[] { new SqlParameter("@RoleId", roleId) };
            SqlDataReader objReader = SQLHelper.GetReader(sql, param);

            while (objReader.Read())
            {
                readerList.Add(new Reader()
                {
                    ReadingCard = objReader["ReadingCard"].ToString(),
                    ReaderName = objReader["ReaderName"].ToString(),
                    ReaderId = Convert.ToInt32(objReader["ReaderId"]),
                    StatusId = Convert.ToInt32(objReader["StatusId"]),
                    RegTime = Convert.ToDateTime(objReader["RegTime"]),
                    PhoneNumber = objReader["PhoneNumber"].ToString(),
                    ReaderAddress = objReader["ReaderAddress"].ToString(),
                    PostCode = objReader["PostCode"].ToString(),
                    Gender = objReader["Gender"].ToString()
                });
            }
            if (objReader.NextResult())//跳转到下一个结果集
            {
                if (objReader.Read())
                {
                    readerCount = Convert.ToInt32(objReader["readerCount"]);
                }
                else readerCount = 0;
            }
            else
            {
                readerCount = 0;
            }
            objReader.Close();
            return readerList;
        }
    }
}
