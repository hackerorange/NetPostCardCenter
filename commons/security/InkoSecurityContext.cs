using System;
using Hacker.Inko.Net.Api;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Hacker.Inko.Net.Base;
using Inko.Security.form;

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

        public static bool GetToken()
        {
            var refreshToken = Properties.Settings.Default.RefreshToken;
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var isWait = true;
                var refreshSuccess = false;

                UserApi.RefreshToken(refreshToken,
                    result =>
                    {
                        refreshSuccess = true;
                        NetGlobalInfo.AccessToken = result.Token;
                        UserId = result.UserId;
                        UserName = result.RealName;
                        isWait = false;
                    },
                    failure: message =>
                    {
                        XtraMessageBox.Show(message);
                        refreshSuccess = false;
                    });
                while (isWait)
                {
                    Application.DoEvents();
                }

                if (refreshSuccess)
                {
                    return true;
                }
            }

            var a = new UserLogin();
            return a.ShowDialog() != DialogResult.OK;
        }

        public static void Logout()
        {
            Properties.Settings.Default.RefreshToken = null;
            Properties.Settings.Default.Save();
            NetGlobalInfo.AccessToken = null;
        }
    }
}