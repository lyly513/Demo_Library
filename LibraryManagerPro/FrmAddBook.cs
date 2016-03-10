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
        //这个名字有点奇怪
        private List<Book> boolList = new List<Book>();//保存当前已经添加到数据库的图书对象

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

            //禁止数据列表控件dgv自动生成列
            this.dgvBookList.AutoGenerateColumns = false;
            new Common.DataGridViewStyle().DgvStyle1(this.dgvBookList);
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
            catch (Exception ex)
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
                    MessageBox.Show("上传文件格式不正确！\r（格式应为jpg,png,bmp,gif)", "上传提示");
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
            if (this.txtBarCode.Text.Trim().Length > 0)
            {
                if (objBookManager.BarCodeIsExisted(this.txtBarCode.Text.Trim()))
                {
                    MessageBox.Show("该条码已经存在！", "提示信息");
                    this.txtBarCode.SelectAll();
                    this.txtBarCode.Focus();
                }
            }
        }
        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            //if (this.txtBarCode.Text.Trim().Length > 0 && e.KeyValue == 13)
            //{
            //    if (objBookManager.BarCodeIsExisted(this.txtBarCode.Text.Trim()))
            //    {
            //        MessageBox.Show("该条码已经存在！", "提示信息");
            //        this.txtBarCode.SelectAll();
            //        this.txtBarCode.Focus();
            //    }
            //}
            if (e.KeyValue == 13)//此处调用相同的功能，精简代码
            {
                txtBarCode_Leave(null, null);
            }
        }


        //确认添加
        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region 数据验证

            //图书名称
            if (this.txtBookName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入图书名称！", "验证信息");
                this.txtBookName.Focus();
                return;
            }
            //主编人
            if (this.txtAuthor.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入主编人名称！", "验证信息");
                this.txtAuthor.Focus();
                return;
            }
            //图书原价
            if (this.txtUnitPrice.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入图书原价！", "验证信息");
                this.txtUnitPrice.Focus();
                return;
            }
            //图书条码
            if (this.txtBarCode.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入图书条码！", "验证信息");
                this.txtBarCode.Focus();
                return;
            }
            //收藏总数
            if (this.txtBookCount.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入收藏总数！", "验证信息");
                this.txtBookCount.Focus();
                return;
            }
            //书架位置
            if (this.txtBookPosition.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入书架位置！", "验证信息");
                this.txtBookPosition.Focus();
                return;
            }

            //下拉框验证
            if (this.cboBookCategory.SelectedIndex == -1)
            {
                MessageBox.Show("请选择图书分类！", "验证信息");
                return;
            }
            if (this.cboPublisher.SelectedIndex == -1)
            {
                MessageBox.Show("请选择出版社！", "验证信息");
                return;
            }

            //验证图片
            if (this.pbCurrentImage.Image == null)
            {
                MessageBox.Show("请选择图书封面！", "验证提示");
                return;
            }

            #endregion
            //封装图书对象
            Book objBook = new Book()
            {
                BookName = this.txtBookName.Text.Trim(),
                BookCategory = Convert.ToInt32(this.cboBookCategory.SelectedValue),
                PublisherId = Convert.ToInt32(this.cboPublisher.SelectedValue),
                PublishDate = Convert.ToDateTime(this.dtpPublishDate.Text),
                Author = this.txtAuthor.Text.Trim(),
                UnitPrice = Convert.ToDouble(this.txtUnitPrice.Text.Trim()),
                BarCode = this.txtBarCode.Text.Trim(),
                BookCount = Convert.ToInt32(this.txtBookCount.Text.Trim()),
                Remainder = Convert.ToInt32(this.txtBookCount.Text.Trim()),
                BookPosition = this.txtBookPosition.Text.Trim(),
                BookImage = new Common.SerializeObjectToString().SerializeObject(this.pbCurrentImage.Image),
                PublisherName=this.cboPublisher.Text

            };
            //调用后台添加图书对象
            try
            {
                objBookManager.AddBook(objBook);//提交给数据库完成对象添加
                
                //在下面的列表中同步显示要添加的图书对象
                this.boolList.Add(objBook);
                this.dgvBookList.DataSource = null;
                this.dgvBookList.DataSource = this.boolList;
                //清空当前用户输入的信息，等待用户输入新的内容
                foreach(Control item in this.gbBook.Controls)
                {
                    if (item is TextBox) item.Text = "";
                    else if(item is ComboBox)
                    {
                        ((ComboBox)item).SelectedIndex = -1;
                    }
                }
                this.pbCurrentImage.Image = null;
                this.txtBookName.Focus();
            }
            catch(Exception ex)
            {
                MessageBox.Show("添加出现异常" + ex.Message, "错误提示");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //添加行号
        private void dgvBookList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Common.DataGridViewStyle.DgvRowPostPaint(this.dgvBookList, e);
        }

    }
}
