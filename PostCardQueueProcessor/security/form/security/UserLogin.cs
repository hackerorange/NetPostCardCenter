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
            loginButton.Enabled = false;
            WebServiceInvoker.GetInstance().UserLogin(textEdit1.Text, textEdit2.Text,success:result=>
            {
                loginButton.Enabled = true;
                SecurityInfo.Token = result.Token;
                SecurityInfo.UserId = result.UserId;
                SecurityInfo.UserName = result.RealName;
                DialogResult = DialogResult.OK;
            },failure:message=>
            {
                XtraMessageBox.Show(message);
            });
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