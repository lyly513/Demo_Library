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
using MyVideo;//引入摄像头操作

namespace LibraryManagerPro
{
    public partial class FrmAddBook : Form
    {
        private BookManager objBookManager = new BookManager();
        private Video objVideo = null;//定义摄像头操作的成员变量


        public FrmAddBook()
        {
            InitializeComponent();
            //初始化图书分类下拉框
            this.cboBookCategory.DataSource = objBookManager.GetAllCategory();
            this.cboBookCategory.DisplayMember = "CategoryName";
            this.cboBookCategory.ValueMember = "CategoryId";
            this.cboBookCategory.SelectedIndex = -1;//默认不选中
            
            //初始化出版社下拉框
            this.cboPublisher.DataSource = objBookManager.GetAllPublisher();
            this.cboPublisher.DisplayMember = "PublisherName";
            this.cboPublisher.ValueMember = "PublisherId";
            this.cboPublisher.SelectedIndex = -1;

            //禁用摄像头相关类
            this.btnCloseVideo.Enabled = false;
            this.btnTake.Enabled = false;
        }
        //启动摄像头
        private void btnStartVideo_Click(object sender, EventArgs e)
        {
            try
            {
                //创建摄像头操作类
                this.objVideo = new Video(this.pbImage.Handle, this.pbImage.Left, this.pbImage.Top, this.pbImage.Width, (short)this.pbImage.Height);
                //打开摄像头
                objVideo.OpenVideo();
                //同时禁用和打开相关按钮
                this.btnStartVideo.Enabled = false;
                this.btnCloseVideo.Enabled = true;
                this.btnTake.Enabled = true;
                //点击后按钮变颜色
                this.btnStartVideo.BackColor = Color.Gray;
                this.btnStartVideo.ForeColor = Color.White;
                this.btnCloseVideo.BackColor = Color.Green;
                this.btnCloseVideo.ForeColor = Color.White;
                this.btnTake.BackColor = Color.Green;
                this.btnTake.ForeColor = Color.White;
            }
            catch(Exception ex)
            {
                MessageBox.Show("摄像头启动失败！" + ex.Message, "提示信息");
            }
        }
        //关闭摄像头
        private void btnCloseVideo_Click(object sender, EventArgs e)
        {
            this.objVideo.CloseVideo();
            //同时禁用和打开相关按钮
            this.btnStartVideo.Enabled = true;
            this.btnCloseVideo.Enabled = false;
            this.btnTake.Enabled = false;

            this.btnStartVideo.BackColor = Color.Green;
            this.btnStartVideo.ForeColor = Color.White;
            this.btnCloseVideo.ForeColor = Color.White;
            this.btnCloseVideo.BackColor = Color.Gray;
            this.btnTake.BackColor = Color.Gray;
            this.btnTake.ForeColor = Color.White;
        }
        //开始拍照
        private void btnTake_Click(object sender, EventArgs e)
        {
            this.pbCurrentImage.Image = objVideo.CatchVideo();
        }
        //选择图片
        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog objOpenFile = new OpenFileDialog();
            DialogResult result = objOpenFile.ShowDialog();
            if (result == DialogResult.OK)//如果用户选择了图片
            {
                //判断用户是否选择常见的图片
                string extension = System.IO.Path.GetExtension(objOpenFile.FileName);//扩展名
                if (extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".gif")
                {
                    this.pbCurrentImage.Image = Image.FromFile(objOpenFile.FileName);
                }
                else
                {
                    MessageBox.Show("上传文件格式不正确！\r（格式应为jpg,png,bmp,gif)","上传提示");
                }
            }
        }
        //清除   
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.pbCurrentImage.Image = null;
        }
        //判断图书条码是否已经存在
        private void txtBarCode_Leave(object sender, EventArgs e)
        {
        }
        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
        }
        //确认添加
        private void btnAdd_Click(object sender, EventArgs e)
        {
          
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
    }
}
