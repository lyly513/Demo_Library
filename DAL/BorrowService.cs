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
    public class BorrowService
    {
        public int GetBorrowCount(string readingCard){
            object count = SQLHelper.GetSingleResultByProcedure("usp_QueryBorrowCount",
            new SqlParameter[]{new SqlParameter("@ReadingCard",readingCard )});
            return Convert.ToInt32(count);
        }
        /// <summary>
        /// 保存借书信息
        /// </summary>
        /// <param name="main">借书主表对象</param>
        /// <param name="detail">借书明细表对象集合</param>
        /// <returns></returns>
        public bool BorrowBook(BorrowInfo main, List<BorrowDetail>detail)
        {
            //【1】借书主表信息插入的SQL语句
            string mainSql = "insert into BorrowInfo (BorrowId, ReaderId, BorrowDate, AdminName_B) values(@BorrowId, @ReaderId, @BorrowDate, @AdminName_B) ";
            //【2】借书明细表信息插入SQL语句
            StringBuilder detailSql = new StringBuilder();
            detailSql.Append("insert into BorrowDetail( BorrowDetailId, BorrowId, BookId, BorrowCount, ReturnCount, NonReturnCount, Expire) ");
            detailSql.Append("values( @BorrowDetailId, @BorrowId, @BookId, @BorrowCount, @ReturnCount, @NonReturnCount, @Expire)");
            //【3】创建借阅主表参数数组
            SqlParameter[] mainParam = new SqlParameter[]
            {
                new SqlParameter("",main.BorrowId),
                new SqlParameter("",main.ReaderId),
                new SqlParameter("",main.AdminName_B)
            };
            //【4】创建借阅详细表参数数组
            List<SqlParameter[]> detailParam = new List<SqlParameter[]>();
            foreach(BorrowDetail item in detail)
            {
                detailParam.Add(new SqlParameter[]
                    {
                        new SqlParameter("@BorrowId",item.BorrowId),
                        new SqlParameter("@BookId",item.BookId),
                        new SqlParameter("@BorrowCount",item.BorrowCount),
                        new SqlParameter("@ReturnCount",item.ReturnCount),
                        new SqlParameter("@Expire",item.Expire),
                        new SqlParameter("@NonReturnCount",item.NonReturnCount)
                    });
            }
            //【5】启用事务提交多个对象
            return SQLHelper.UpdateByTran(mainSql,mainParam,detailSql.ToString(),detailParam);
        }


    }

    

}

