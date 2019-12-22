using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using postCardCenterSdk.domain.system;
using System.Threading;
using System.IO;
using postCardCenterSdk.sdk;
using postCardCenterSdk;
using Inko.Security;
using postCardCenterSdk.constant;

namespace PostCardQueueProcessor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (new Mutex(true, Application.ProductName, out var createNew))
            {
                if (createNew)
                {
                    BonusSkins.Register();
                    SkinManager.EnableFormSkins();
                    UserLookAndFeel.Default.SetSkinStyle("DevExpress Style"); // 设置皮肤样式

                    /* Application.EnableVisualStyles();
                     Application.SetCompatibleTextRenderingDefault(false);*/

                    BonusSkins.Register();
                    SkinManager.EnableFormSkins();

                    string token = InkoSecurityContext.GetToken();
                    if (!string.IsNullOrEmpty(token)){
                        GlobalApiContext.Token = token;
                        Application.Run(new Form1());
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
