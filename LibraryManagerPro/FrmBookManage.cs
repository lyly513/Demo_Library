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
using MyVideo;


namespace LibraryManagerPro
{
    public partial class FrmBookManage : Form
    {
        //创建业务逻辑对象
        private BookManager objBookManager = new BookManager();
        //创建对象的容器(List泛型集合):用来保存查询结果
        private List<Book> listBook = null;
        //摄像头操作类
        private Video objVideo = null;

        public FrmBookManage()
        {
            InitializeComponent();
            #region 初始化

            //显示图书分类下拉框
            List<Catetgory> list = objBookManager.GetAllCategory();
            //（差异）
            list.Insert(0, new Catetgory() { CategoryId = -1, CategoryName = "全部类别" });//为下拉框添加一个默认选项（差异）

            this.cboCategory.DataSource = list;
            this.cboCategory.DisplayMember = "CategoryName";
            this.cboCategory.ValueMember = "CategoryId";
            this.cboCategory.SelectedIndex = 0;//默认选中“全部类别”(差异)
            //禁用删除和修改按钮
            this.btnDel.Enabled = false;
            this.btnSave.Enabled = false;
            //禁用数据列表自动生成列
            this.dgvBookList.AutoGenerateColumns = false;

            //初始化图书分类下拉框
            this.cbo_BookCategory.DataSource = objBookManager.GetAllCategory();
            this.cbo_BookCategory.DisplayMember = "CategoryName";
            this.cbo_BookCategory.ValueMember = "CategoryId";
            this.cbo_BookCategory.SelectedIndex = -1;

            //初始化出版社下拉框
            this.cbo_Publisher.DataSource = objBookManager.GetAllPublisher();
            this.cbo_Publisher.DisplayMember = "PublisherName";
            this.cbo_Publisher.ValueMember = "PublisherId";
            this.cbo_Publisher.SelectedIndex = -1;

            #endregion
        }

        #region 组合查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //首先断开选择改变事件（防止有些情况的异常）
            this.dgvBookList.SelectionChanged -= new EventHandler(this.dgvBookList_SelectionChanged);

            //判断用户是否输入查询条件
            //this.cboCategory.SelectedIndex.ToString() == "-1"（差异）
            if (this.cboCategory.SelectedIndex.ToString() == "-1" && this.txtBarCode.Text.Trim().Length == 0 && this.txtBookName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请至少选择一个查询条件！", "查询提示");
            }
            else
            {
                //根据条件组合查询
                listBook = objBookManager.GetBooks(Convert.ToInt32(this.cboCategory.SelectedValue),this.txtBarCode.Text.Trim(),this.txtBookName.Text.Trim());
                //在列表中显示查询结果
                this.dgvBookList.DataSource = listBook;
                //根据查询结果决定是否开启和禁用“删除”按钮
                if (listBook.Count == 0)
                {
                    this.btnDel.Enabled = false;
                }
                else
                {
                    this.btnDel.Enabled = true;
                }
            }

            //开启选择改变事件
            this.dgvBookList.SelectionChanged += new EventHandler(this.dgvBookList_SelectionChanged);
            dgvBookList_SelectionChanged(null,null);//使其立刻进行一次同步

        }


        #endregion

        #region 同步显示要修改的信息
        private void dgvBookList_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgvBookList.RowCount == 0)
                return;
            //找到要修改的图书对象

            string barCode = this.dgvBookList.CurrentRow.Cells["BarCode"].Value.ToString();
            Book objBook = (from b in listBook where b.BarCode.Equals(barCode) select b).First<Book>();//在泛型集合列表中查找符合的行及其属性（无需去数据库查找）
            
            
            //显示当前图书对象信息
            this.lbl_BarCode.Text = objBook.BarCode;
            this.txt_Author.Text = objBook.Author;
            this.lbl_BookCount.Text = objBook.BookCount.ToString();
            this.txt_BookName.Text = objBook.BookName;
            this.txt_UnitPrice.Text = objBook.UnitPrice.ToString();
            this.cbo_BookCategory.SelectedValue = objBook.BookCategory;
            this.cbo_Publisher.SelectedValue = objBook.PublisherName;//(差异)
            this.lbl_BookId.Text = objBook.BookId.ToString();
            if(objBook.BookImage.Length!=0)
            {
                this.pbCurrentImage.Image = (Image)new Common.SerializeObjectToString().DeserializeObject(objBook.BookImage);
            }
            else
            {
                this.pbCurrentImage.Image = null;
            }


        }
        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        
    }
}
