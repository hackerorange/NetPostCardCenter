using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Hacker.Inko.Net.Api;

namespace Inko.Security.form.security
{
    public partial class UserLogin : XtraForm
    {
        private string _token = null;

        public string Token
        {
            get { return _token; }
        }

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
                var stream = new StreamWriter(new FileStream("D:\\postCard\\userLogin.ini", FileMode.CreateNew));
                stream.WriteLine(result.RefreshToken);
                stream.Flush();
                stream.Close();

                loginButton.Enabled = true;
                _token = result.Token;
                InkoSecurityContext.UserId = result.UserId;
                InkoSecurityContext.UserName = result.RealName;
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
        }
    }
}