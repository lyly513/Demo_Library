using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BLL;
using Models;
using Common; 

namespace LibraryManagerPro
{
    public partial class FrmAdminLogin : Form
    {
      //创建业务逻辑对象
        private SysAdminManager objAdminManager = new SysAdminManager();

        public FrmAdminLogin()
        {
            InitializeComponent();

        }

        //登录
        private void btnLogin_Click(object sender, EventArgs e)
        {
        #region 数据验证
                    //帐号验证
                    if(this.txtAdminId.Text.Trim().Length ==0)
                    {
                        MessageBox.Show("请输入账号!", "登录提示");
                        this.txtAdminId.Focus();
                        return;
                    }
                    if(!DataValidate.IsInteger(this.txtAdminId.Text.Trim()))
                    {
                        MessageBox.Show("登录账号必须是整数", "登录提示");
                        this.txtAdminId.Focus();
                        return;
                    }
                    //密码验证
                    if (this.txtLoginPwd.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("请输入密码!", "登录提示");
                        this.txtLoginPwd.Focus();
                        return;
                    }
                    else if (this.txtLoginPwd.Text.Trim().Length < 6 )
                    {
                        MessageBox.Show("登录密码必须大于等于六位数", "登录提示");
                        this.txtLoginPwd.Focus();
                        return;
                    }
                    else if (this.txtLoginPwd.Text.Trim().Length > 18)
                    {
                        MessageBox.Show("登录密码必须小于等于十八位数", "登录提示");
                        this.txtLoginPwd.Focus();
                        return;
                    }

        #endregion

            //封装对象（将用户输入的帐号和密码封装到用户对象中）
            SysAdmin objAdmin = new SysAdmin()
            {
                AdminId=Convert.ToInt32(this.txtAdminId.Text.Trim()),
                LoginPwd = this.txtLoginPwd.Text.Trim()

            };
            try
            {
                //调用业务逻辑完成登录帐号和密码的对比
                objAdmin = objAdminManager.AdminLogin(objAdmin);
                //判断登录是否成功
                if(objAdmin != null)//如果用户名和密码正确
                {
                    if(objAdmin.StatusId==1)//账号状态正常
                    {
                        Program.objCurrentAdmin = objAdmin;//保存当前登录用户对象
                        this.DialogResult=DialogResult.OK;//设置窗体返回值
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("当前登录账号被禁用！无法登录，请联系管理员！","登录提示");
                    }
                }
                else
                {
                    MessageBox.Show("登录账号或密码不正确！","登录提示");
                }

                
            }
            catch(Exception ex)
            {
                MessageBox.Show("登录出现异常："+ex.Message,"登录提示");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
