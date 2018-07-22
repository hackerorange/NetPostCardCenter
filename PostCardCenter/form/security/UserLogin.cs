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


        private void loginButton_Click(object sender, EventArgs e)
        {
            WebServiceInvoker.GetInstance().UserLogin(textEdit1.Text, textEdit2.Text, result =>
            {
                
                //Security.AccountSessionInfo = new AccountSessionInfo
                //{
                //    RealName = result.RealName,
                //    Token = result.Token
                //};
                SecurityInfo.Token = result.Token;
                SecurityInfo.UserName = result.RealName;
                DialogResult = DialogResult.OK;
            }, message => { XtraMessageBox.Show(message); });
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}