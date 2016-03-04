using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Models;

namespace LibraryManagerPro
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //显示登录窗体
            FrmAdminLogin frmLogin = new FrmAdminLogin();
            DialogResult result = frmLogin.ShowDialog();
            //判断登录是否成功
            if (result == DialogResult.OK)
                Application.Run(new FrmMain());
            else
                Application.Exit();//退出整个应用程序

            Application.Run(new FrmMain());

        }
        //定义一个全局变量（用来保存当前的登录用户对象）
        public static SysAdmin objCurrentAdmin = null;

    }
}
