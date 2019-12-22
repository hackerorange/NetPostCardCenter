﻿

using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using postCardCenter.form;
using Inko.Security.form.security;
using System;
using System.Threading;
using System.Windows.Forms;
using Inko.Security;
using postCardCenterSdk.constant;

namespace postCardCenter
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
                    string token = InkoSecurityContext.GetToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        GlobalApiContext.Token = token;
                        Application.Run(new PostCardCenterMainForm());
                    }

                   
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