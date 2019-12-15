using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using postCardCenterSdk;
using postCardCenterSdk.sdk;

namespace PostCardCenter.form.security
{
    public partial class UserLogin : XtraForm
    {
        public UserLogin()
        {
            InitializeComponent();
        }

        private void UserLogin_Load(object sender, EventArgs e)
        {
        }


        private void LoginButton_Click(object sender, EventArgs e)
        {
            var result = WebServiceInvoker.GetInstance().UserLogin(textEdit1.Text, textEdit2.Text);
            if (result != null)
            {
                SecurityInfo.Token = result.Token;
                SecurityInfo.UserId = result.UserId;
                SecurityInfo.UserName = result.RealName;
                DialogResult = DialogResult.OK;
            }
            else
            {
                // TODO: 处理用户名密码问题
                XtraMessageBox.Show("登录失败，用户名或密码不正确!");
            }           
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {

        }
    }
}