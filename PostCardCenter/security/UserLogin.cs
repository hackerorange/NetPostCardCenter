using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using soho.security;
using soho.web;
using Spring.Http;
using Spring.Http.Converters;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;

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
            var restTemplate = new RestTemplate();

            restTemplate.MessageConverters.Add(new SpringJsonHttpMessageConverter());
            restTemplate.MessageConverters.Add(new StringHttpMessageConverter());
//            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("userName", textEdit1.Text);
            nameValueCollection.Add("password", textEdit2.Text);
            var headers = new HttpHeaders {{"token", "123456"}};
            //            dictionary.Add("userName", Encoding.UTF8.GetBytes(textEdit1.Text));
//            dictionary.Add("password", Encoding.UTF8.GetBytes(textEdit2.Text));

            try
            {
                var postForObject =
                    restTemplate.PostForObject<BodyResponse<string>>(webServicePath.loginPath,
                        nameValueCollection,headers);
                if (postForObject.code == 200)
                {
                    Security.TokenId = postForObject.body;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    XtraMessageBox.Show("账号或密码不正确，请重新输入");
                    textEdit1.Text = "";
                    textEdit2.Text = "";
                    textEdit1.Focus();
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show("出现网络异常" + exception.Message);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
        }
    }
}