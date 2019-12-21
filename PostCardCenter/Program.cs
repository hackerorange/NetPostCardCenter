using System;
using System.Threading;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using OrderBatchCreate.form;
using postCardCenterSdk.sdk;
using PostCardCenter.form;
using PostCardCenter.form.security;
using soho.domain.system;
using System.IO;
using postCardCenterSdk;

namespace PostCardCenter
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once ArrangeTypeMemberModifiers
        static void Main()
        {
            using (new Mutex(true, Application.ProductName, out var createNew))
            {
                if (createNew)
                {
                    BonusSkins.Register();
                    SkinManager.EnableFormSkins();
                    UserLookAndFeel.Default.SetSkinStyle("DevExpress Style"); // 设置皮肤样式
                    BonusSkins.Register();
                    SkinManager.EnableFormSkins();
                    if (File.Exists("D:\\postCard\\userLogin.ini"))
                    {
                        try
                        {
                            // TODO: 需要校验保存起来的Token是否有效，刷新Token
                            var fileReader = new StreamReader(new FileStream("D:\\postCard\\userLogin.ini", FileMode.Open));
                            var refreshToken = fileReader.ReadLine();
                            var isWait = true;
                            WebServiceInvoker.GetInstance().RefreshToken(refreshToken, success: result =>
                            {
                                SecurityInfo.Token = result.Token;
                                SecurityInfo.UserId = result.UserId;
                                SecurityInfo.UserName = result.RealName;
                                isWait = false;
                            }, null);
                            while (isWait)
                            {

                            }
                        }
                        catch
                        {

                        }
                    }
                    // 如果当前用户没有登录过，Token已经失效
                    if (string.IsNullOrEmpty(SecurityInfo.Token))
                    {
                        var a = new UserLogin();
                        if (a.ShowDialog() != DialogResult.OK) return;
                    }
                    // 运行主程序
                    Application.Run(new PostCardCenterMainForm());
                }
                else
                {
                    MessageBox.Show(@"应用程序已经在运行中...");
                    Thread.Sleep(1000);
                    Environment.Exit(1);
                }
            }
        }
    }
}