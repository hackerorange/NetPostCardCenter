using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using soho.translator;
using soho.security;
using soho.web;
using Spring.Http;
using Spring.Http.Converters;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;
using postCardCenterSdk.sdk;

namespace PostCardCenter.security
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
            WebServiceInvoker.UserLogin(textEdit1.Text, textEdit2.Text, success:result => {
                Security.AccountSessionInfo = new AccountSessionInfo
                {
                    realName = result.RealName,
                    accountName = result.AccountName
                };                
                DialogResult = DialogResult.OK;
            }, failure:message=>
            {
                XtraMessageBox.Show(message);
            });
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}