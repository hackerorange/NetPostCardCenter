using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using soho.helper;
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
            var restTemplate = new RestTemplate();

            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
//            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("userName", textEdit1.Text);
            nameValueCollection.Add("password", textEdit2.Text);

            //            dictionary.Add("userName", Encoding.UTF8.GetBytes(textEdit1.Text));
//            dictionary.Add("password", Encoding.UTF8.GetBytes(textEdit2.Text));
            var httpHeaders = new HttpHeaders {{"tokenId", "123456"}};

            var headers = new HttpEntity(nameValueCollection, httpHeaders);


            restTemplate.PostForObjectAsync<BodyResponse<AccountSessionInfo>>(RequestUtils.GetUrl("loginUrl"),
                headers, resp =>
                {
                    if (resp.Error != null)
                    {
                        XtraMessageBox.Show(resp.Error.Message);
                    }
                    else
                    {
                        if (resp.Response.code == 200)
                        {                            
                            Security.AccountSessionInfo = resp.Response.body;
                            Security.TokenId = Security.AccountSessionInfo.tokenId;
                            WebServiceInvoker.Token = Security.AccountSessionInfo.tokenId;
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            XtraMessageBox.Show(resp.Response.message);
                        }
                    }
                });
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}