﻿using System;
using Hacker.Inko.Net.Api;
using Inko.Security.form.security;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Inko.Security
{
    public static class InkoSecurityContext
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public static string UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public static string UserName { get; set; }

        public static string GetToken()
        {
            string token = null;
            if (File.Exists("D:\\postCard\\userLogin.ini"))
                try
                {
                    // TODO: 需要校验保存起来的Token是否有效，刷新Token
                    var fileReader = new StreamReader(new FileStream("D:\\postCard\\userLogin.ini", FileMode.Open));
                    var refreshToken = fileReader.ReadLine();
                    fileReader.Close();
                    var isWait = true;
                    UserApi.RefreshToken(refreshToken, result =>
                    {
                        token = result.Token;
                        UserId = result.UserId;
                        UserName = result.RealName;
                        isWait = false;
                    }, message =>
                    {
                        XtraMessageBox.Show(message);
                        isWait = false;
                    });
                    while (isWait) Application.DoEvents();
                }
                catch (Exception)
                {
                    // ignored
                }

            // 如果Token有效
            if (!string.IsNullOrEmpty(token))
            {
                return token;
            }

            var a = new UserLogin();
            return a.ShowDialog() != DialogResult.OK ? null : a.Token;
        }
    }
}