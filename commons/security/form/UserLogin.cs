using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Hacker.Inko.Global.Form;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Base;

namespace Inko.Security.form
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
            UserApi.UserLogin(textEdit1.Text, textEdit2.Text, success: result =>
            {
                Properties.Settings.Default.RefreshToken = result.RefreshToken;
                Properties.Settings.Default.Save();
                loginButton.Enabled = true;
                InkoSecurityContext.UserId = result.UserId;
                InkoSecurityContext.UserName = result.RealName;
                NetGlobalInfo.AccessToken = result.Token;
                DialogResult = DialogResult.OK;
            }, failure: message =>
            {
                XtraMessageBox.Show(message);
                loginButton.Enabled = true;
            });
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            new GlobalSettingsForm().ShowDialog();
        }
    }
}