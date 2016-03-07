using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibraryManagerPro
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            //显示登录用户名
            this.tssl_AdminName.Text = Program.objCurrentAdmin.AdminName;

            //根据不同用户的角色，设定不同权限

        }

        //封装方法
        private void OpenForm(Form objFrm)
        {
            //首先判断容器中是否有其他窗体，如果有，将其关闭
            foreach (Control item in this.spContainer.Panel2.Controls)
            {
                if (item is Form)
                {
                    ((Form)item).Close();
                }
            }
            //其次嵌入新的子窗体
            //FrmAddBook objFrm = new FrmAddBook(); ------这个是变化的，以下语句为不变的。
            objFrm.TopLevel = false;//将子窗体设置成非顶级控件
            objFrm.FormBorderStyle = FormBorderStyle.None;//去掉子窗体的边框
            objFrm.Parent = this.spContainer.Panel2;//指定子窗体显示的容器
            objFrm.Dock = DockStyle.Fill;//随着容器大小自动调整窗体大小
            objFrm.Show();
        }

#region 方法具体调用

     //新增图书 
        private void btnAddBook_Click(object sender, EventArgs e)
        {
            FrmAddBook objFrm = new FrmAddBook();
            OpenForm(objFrm);
            this.lblOperationName.Text = "新增图书";

        }
        //图书管理
        private void btnBookManage_Click(object sender, EventArgs e)
        {
            FrmBookManage objFrm = new FrmBookManage();
            OpenForm(objFrm);
            this.lblOperationName.Text = "图书信息维护";
        }
        //图书出借
        private void btnBorrowBook_Click(object sender, EventArgs e)
        {
            FrmBorrowBook objFrm = new FrmBorrowBook();
            OpenForm(objFrm);
            this.lblOperationName.Text = "图书出借";
        }
        //图书上架
        private void btnBookNew_Click(object sender, EventArgs e)
        {
            FrmNewBook objFrm = new FrmNewBook();
            OpenForm(objFrm);
            this.lblOperationName.Text = "图书上架";
        }
        //图书归还
        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            FrmReturnBook objFrm = new FrmReturnBook();
            OpenForm(objFrm);
            this.lblOperationName.Text = "图书归还";
        }
        //读者管理
        private void btnReaderManager_Click(object sender, EventArgs e)
        {
            FrmReaderManger objFrm = new FrmReaderManger();
            OpenForm(objFrm);
            this.lblOperationName.Text = "读者管理";
        }

        //密码修改
        private void btnModifyPwd_Click(object sender, EventArgs e)
        {
            FrmModifyPwd objFrm = new FrmModifyPwd();
            objFrm.ShowDialog();
        }
        //退出系统
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
#endregion

        //窗体关闭之前发生的事件
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出系统", "退出询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;//取消窗体关闭操作
            }
        }

    }
}
