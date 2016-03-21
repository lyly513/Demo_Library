using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MyVideo;
using Models;
using BLL;

namespace LibraryManagerPro
{
    public partial class FrmEidtReader : Form
    {
        private ReaderManager objReaderManager = new ReaderManager();
        private Reader objEditReader = null;

        public FrmEidtReader(Reader objReader)
        {
            InitializeComponent();
            //初始化角色下拉框
            this.cboReaderRole.DataSource=objReaderManager.GetAllReaderRole();
            this.cboReaderRole.DisplayMember="RoleName";
            this.cboReaderRole.ValueMember="RoleId";

            //显示读者信息
            this.txtReaderName.Text = objReader.ReaderName;
            this.txtReadingCard.Text = objReader.ReadingCard;
            this.txtPostcode.Text = objReader.PostCode;
            this.txtPhone.Text = objReader.PhoneNumber;
            this.txtAddress.Text = objReader.ReaderAddress;
            this.cboReaderRole.Text=objReader.RoleName;
            if(objReader.Gender.ToString()=="男"){
                this.rdoMale.Checked=true;
            }         
            else{
                this.rdoFemale.Checked=true;
            }
                this.pbReaderPhoto.Image = objReader.ReaderImage != "" ?
                    (Image)new Common.SerializeObjectToString().DeserializeObject(objReader.ReaderImage) : null;

                objEditReader = objReader;//保存当前的读者对象，为后面的修改使用
        }
        //提交修改
        private void btnSave_Click(object sender, EventArgs e)
        {
            //数据验证

            //封装对象
            Reader objReader = new Reader()
            {
                ReaderName = this.txtReaderName.Text.Trim(),
                ReadingCard = this.txtReadingCard.Text.Trim(),
                Gender = this.rdoFemale.Checked ? "女" : "男",
                RoleId = Convert.ToInt32(this.cboReaderRole.SelectedValue),
                PostCode = this.txtPostcode.Text.Trim(),
                PhoneNumber = this.txtPhone.Text.Trim(),
                ReaderAddress = this.txtAddress.Text.Trim(),
                ReaderImage = this.pbReaderVideo.Image != null ?
                new Common.SerializeObjectToString().SerializeObject(this.pbReaderVideo.Image) : "",
                ReaderId=this.objEditReader.ReaderId
            };
            //数据提交
            try
            {
                objReaderManager.EditReader(objReader);
                MessageBox.Show("提交成功", "提交信息");
            }
            catch (Exception ex)
            {
                MessageBox.Show("提交出现错误："+ex.Message, "提交信息");
            }
        }

    }
}
