using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Models;
using BLL;

namespace LibraryManagerPro
{
    public partial class FrmNewBook : Form
    {
        private BookManager objBookManager = new BookManager();
        private List<Book> bookList = new List<Book>();

        public FrmNewBook()
        {
            InitializeComponent();
            this.txtAddCount.Enabled = false;//禁止用户直接输入
            this.dgvBookList.AutoGenerateColumns = false;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //根据图书条码查询图书详细信息
       
        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.txtBarCode.Text.Trim().Length > 0 && e.KeyValue == 13)
            {

                //根据图书条码查询图书详细信息
                Book objBook = objBookManager.GetBookByBarCode(this.txtBarCode.Text.Trim());
                if (objBook != null)
                {
                    //显示图书信息
                    this.lblBookName.Text = objBook.BookName;
                    this.lblBookPosition.Text = objBook.BookPosition;
                    this.lblBookCount.Text = objBook.BookCount.ToString();
                    this.lblCategory.Text = objBook.CategoryName;
                    this.lblBookId.Text = objBook.BookId.ToString();
                    this.pbImage.Image = objBook.BookImage.Length == 0 ? null :
                        (Image)new Common.SerializeObjectToString().DeserializeObject(objBook.BookImage);

                    //开启新增图书数量文本
                    this.txtAddCount.Enabled = true;
                    this.txtAddCount.Focus();
                    //同步在列表中显示（先判断当前列表中是否存在该对象）
                    int count = (from b in bookList where b.BookId == objBook.BookId select b).Count();
                    if (count == 0)
                    {
                        this.bookList.Add(objBook);
                        this.dgvBookList.DataSource = null;
                        this.dgvBookList.DataSource = bookList;
                    }

                }
                else
                {
                    MessageBox.Show("图书不存在！", "查询提示");
                }

            }

        }


        //增加图书总量
        private void btnSave_Click(object sender, EventArgs e)
        {
            //数据验证
            if(this.txtAddCount.Text.Trim().Length==0)
            {
                MessageBox.Show("请输入新增图书总数！", "提示信息");
                this.txtAddCount.Focus();
                return;
            }
            if(!Common.DataValidate.IsInteger(this.txtAddCount.Text.Trim()))
            {
                MessageBox.Show("新增图书总数必须是一个正整数！", "提示信息");
                this.txtAddCount.SelectAll();
                this.txtAddCount.Focus();
                return;

            }
            //提交给数据库
            try
            {
                int result = objBookManager.AddBookCount(this.txtBarCode.Text.Trim(), Convert.ToInt32(this.txtAddCount.Text.Trim()));
                if(result==1)
                {
                    //在dgv中显示当前的图书数量和其他信息
                    Book objBook=(from b in bookList where b.BarCode==this.txtBarCode.Text.Trim() select b).First<Book>();
                    objBook.BookCount = objBook.BookCount + Convert.ToInt32(this.txtAddCount.Text.Trim());
                    this.dgvBookList.Refresh();

                    //清空图书信息的显示
                    this.lblBookName.Text = "";
                    this.lblBookPosition.Text = "";
                    this.lblBookCount.Text = "";
                    this.lblCategory.Text = "";
                    this.lblBookId.Text = "";
                    this.pbImage.Image = null;
                    //图书条码文本框获取焦点
                    this.txtBarCode.Clear();
                    this.txtAddCount.Clear();
                    this.txtAddCount.Enabled = false;
                    this.txtBarCode.Focus();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("当前操作出现问题" + ex.Message);
            }
        }

        private void txtAddCount_KeyDown(object sender, KeyEventArgs e)
        {
            if(this.txtAddCount.Text.Trim().Length>0&&e.KeyValue==13)
            {
                btnSave_Click(null, null);
            }
        }

        
    }
}
