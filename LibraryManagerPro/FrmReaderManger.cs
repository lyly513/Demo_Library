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
using Common;


namespace LibraryManagerPro
{
    public partial class FrmReaderManger : Form
    {
        //创建业务逻辑对象
        private ReaderManager objReaderManager = new ReaderManager();
        //创建对象的容器(List泛型集合):用来保存查询结果
        List<Reader> listReader = null;
        //摄像头操作类
        private Video objVideo = null;
        //定义用户成员变量
        private Reader objCurrentReader = null;
        public FrmReaderManger()
        {
            InitializeComponent();

            //初始化角色下拉框
            DataTable dt = objReaderManager.GetAllReaderRole();
            this.cboReaderRole.DataSource = dt;
            this.cboReaderRole.DisplayMember = "RoleName";
            this.cboReaderRole.ValueMember = "RoleId";
            //初始化用于查询的用户角色下拉框
            this.cboRole.DataSource = dt.Copy();//复制，防止选择时同步变化
            this.cboRole.DisplayMember = "RoleName";
            this.cboRole.ValueMember = "RoleId";

            //禁用修改按钮和借阅证挂失按钮
            this.btnEdit.Enabled = false;
            this.btnForbidden.Enabled = false;

        }

        

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //按照角色查询
        private void btnQueryReader_Click(object sender, EventArgs e)
        {
            this.lvReader.Items.Clear();//清空所有内容
            int readerCount = 0;//提前定义一个接受“输出参数”的变量
            List<Reader> readerList = objReaderManager.GetReaderByRole(this.cboRole.SelectedValue.ToString(), out readerCount);
            
            //给ListView绑定数据源
            foreach (Reader objReader in readerList)
            {
                //【1】创建一个ListView项
                ListViewItem lvItem = new ListViewItem(objReader.ReaderId.ToString());
                //【2】把ListViewItem对象添加到ListView中
                this.lvReader.Items.Add(lvItem);
                //【3】在当前ListViewItem对象中添加子项
                lvItem.SubItems.AddRange(new string[]{
                    objReader.ReadingCard,
                    objReader.ReaderName,
                    objReader.Gender,
                    objReader.PhoneNumber,
                    objReader.RegTime.ToShortDateString(),
                    objReader.StatusDesc,
                    objReader.ReaderAddress
                });
            }
            //显示当前角色对应读者总数
            this.lblReaderCount.Text = readerCount.ToString();
        }

        #region 摄像头操作
        //启动摄像头
        private void btnStartVideo_Click(object sender, EventArgs e)
        {
            try
            {
                //创建摄像头操作类
                this.objVideo = new Video(this.pbReaderVideo.Handle, this.pbReaderVideo.Left, this.pbReaderVideo.Top, this.pbReaderVideo.Width, (short)this.pbReaderImg.Height);
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
            try
            {
                this.pbReaderPhoto.Image = objVideo.CatchVideo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加照片出错："+ex.Message, "错误提示");
            }
                
            
            
        }

        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //【1】数据验证
            #region 数据验证
            if (this.txtReaderName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入读者姓名！", "输入提示");
                return;
            }
            if (this.txtReadingCard.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入借阅证编号！", "输入提示");
                return;
            }
            if (!this.rdoFemale.Checked && !this.rdoMale.Checked)
            {
                MessageBox.Show("请选择读者性别！", "输入提示");
                return;
            }
            if (this.txtPostcode.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入邮政编号！", "输入提示");
                if (!DataValidate.IsInteger(this.txtPostcode.Text.Trim()))
                {
                    MessageBox.Show("请输入正确格式的邮政编号！", "输入提示");
                }
                return;
            }
            if (this.txtPhone.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入电话号码！", "输入提示");
                return;
            }
            if (this.txtAddress.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入地址！", "输入提示");
                return;
            }
            
            //摄像头暂时无法使用

            //if(this.pbReaderPhoto.Image==null)
            //{
            //    MessageBox.Show("请选择图片！", "输入提示");
            //    return;
            //}
            #endregion 
            //【2】封装对象
            Reader objReader = new Reader()
            {
                ReaderName=this.txtReaderName.Text.Trim(),
                ReadingCard=this.txtReadingCard.Text.Trim(),
                IDCard=this.txtIDCard.Text.Trim(),
                Gender=this.rdoFemale.Checked?"女":"男",
                RoleId=Convert.ToInt32(this.cboReaderRole.SelectedValue),
                PostCode=this.txtPostcode.Text.Trim(),
                PhoneNumber=this.txtPhone.Text.Trim(),
                ReaderAddress=this.txtAddress.Text.Trim(),
                ReaderPwd="111111",
                AdminId=Program.objCurrentAdmin.AdminId,
                ReaderImage=this.pbReaderVideo.Image!=null?
                new Common.SerializeObjectToString().SerializeObject(this.pbReaderVideo.Image):""
            };
            //【3】提交给数据库保存对象
            try
            {
                objReaderManager.AddReader(objReader);
                MessageBox.Show("添加成功！", "提示信息");

                foreach(Control item in tabPage2.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                    if (item is ComboBox)
                    {
                         ((ComboBox)item).SelectedIndex = 0;
                    }
                    if (item is RadioButton)
                    {
                        ((RadioButton)item).Checked=false;
                    }
                }
                this.pbReaderPhoto.Image = null;
                this.txtReaderName.Focus();
            }
            catch(Exception ex)
            {
                MessageBox.Show("添加时出现错误：" + ex.Message);
            }
        }

        //按照身份证号或者借阅证号进行查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.rdoIDCard.Checked && this.txt_IDCard.Text.Trim().Length > 0)
            {
                this.objCurrentReader = objReaderManager.GetReaderByIDCard(this.txt_IDCard.Text.Trim());
            }
            else if (this.rdoReadingCard.Checked && this.txt_ReadingCard.Text.Trim().Length > 0)
            {
                this.objCurrentReader = objReaderManager.GetReaderByReadingCard(this.txt_ReadingCard.Text.Trim());
            }
            else if (this.txt_ReadingCard.Text.Trim().Length == 0 && this.txt_IDCard.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入相关信息！", "输入提示");
                return;
            }
            else
            {
                MessageBox.Show("无法找到对应信息！", "提示信息");
                return;
            }
            //显示查询结果
            if (this.objCurrentReader != null)
            {
                if(objCurrentReader.StatusId!=0)//如果当前借阅证有效
                {
                    this.btnEdit.Enabled = true;
                this.btnForbidden.Enabled = true;
                }
                
                //显示读者信息
                this.lblAddress.Text = objCurrentReader.ReaderAddress;
                this.lblPhone.Text = objCurrentReader.PhoneNumber;
                this.lblPostCode.Text = objCurrentReader.PostCode;
                this.lblReaderName.Text = objCurrentReader.ReaderName;
                this.lblRoleName.Text = objCurrentReader.RoleName;
                this.lblReadingCard.Text = objCurrentReader.ReadingCard;
                this.lblGender.Text = objCurrentReader.Gender;
                this.pbReaderImg.Image = objCurrentReader.ReaderImage != "" ?
                    (Image)new Common.SerializeObjectToString().DeserializeObject(objCurrentReader.ReaderImage) : null;
            }
            else
            {
                MessageBox.Show("当前读者不存在！", "查询提示");
                ClearReader();
                
                //禁用修改按钮和借阅证挂失按钮
                this.btnEdit.Enabled = false;
                this.btnForbidden.Enabled = false;
            }
        }

        //清空读者信息
        private void ClearReader()
        {
            foreach (Control item in tabPage1.Controls)
            {
                if (item is Label)
                {
                    item.Text = "";
                }
            }
            this.pbReaderImg.Image = null;
        }

        private void rdoIDCard_CheckedChanged(object sender, EventArgs e)
        {
            this.txt_ReadingCard.Clear();
        }

        private void rdoReadingCard_CheckedChanged(object sender, EventArgs e)
        {
            this.txt_IDCard.Text = "";
        }

        //挂失借阅证
        private void btnForbidden_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确认挂失当前的借阅证吗？", "挂失询问", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);
            if(result==DialogResult.OK)
            {
                try
                {
                    objReaderManager.ForbiddenReaderCard(objCurrentReader.ReaderId.ToString());
                    MessageBox.Show("挂失成功", "提示信息");
                    ClearReader();
                }
                catch(Exception)
                {

                }
            }
        }

        //修改信息
        private void btnEdit_Click(object sender, EventArgs e)
        {
            FrmEidtReader frmEdit = new FrmEidtReader(this.objCurrentReader);
            frmEdit.Show();
        }

    }
}
