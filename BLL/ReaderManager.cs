using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL;
using Models;
using System.Data;


namespace BLL
{
    //读者模块业务逻辑
    public class ReaderManager
    {
        private ReaderService objReaderService = new ReaderService();

         //会员办证（添加读者信息）
        public int AddReader(Reader objReader)
        {
            return objReaderService.AddReader(objReader);
        }

        //修改读者信息(不带参数)
        public int EditReader(Reader objReader)
        {
            return objReaderService.EditReader(objReader);
        }
        ////修改读者信息(带参数)
        //public int EditReader(string readerId, Reader objReader)
        //{
        //    return objReaderService.EditReader(readerId, objReader);
        //}

        //借阅证挂失
        public int ForbiddenReaderCard(string readerId)
        {
            return objReaderService.ForbiddenReaderCard(readerId);
        }

        //查询全部会员角色
        public DataTable GetAllReaderRole()
        {
            return objReaderService.GetAllReaderRole();
        }

        //根据借阅证号查询读者信息
        public Reader GetReaderByReadingCard(string readingCard)
        {
            return objReaderService.GetReaderByReadingCard(readingCard);
        }

         //根据身份证号查询读者信息
        public Reader GetReaderByIDCard(string IDCard)
        {
            return objReaderService.GetReaderByIDCard(IDCard);
        }

        
        /// <summary>
        /// 根据会员角色查询读者信息（同时找到该角色的会员总数）
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="readerCount">返回的读者总数</param>
        /// <returns>返回读者对象列表</returns>
        public List<Reader> GetReaderByRole(string roleId, out int readerCount)//使用out参数，返回多个数据
        {
            List<Reader> readerList = objReaderService.GetReaderByRole(roleId, out readerCount);
            //根据借阅证状态编号修改成对应名称
            foreach (Reader item in readerList)//(差异：遍历方法不一样；老师：for（int i=0;i<readerList.Count;i++））
            {
                if (item.StatusId == 0)
                {
                    item.StatusDesc = "禁用";
                    //break;
                }
                else if(item.StatusId == 1)
                {
                    item.StatusDesc = "正常";
                    
                }
            }
            return readerList;
        }

    }
}
