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

                    /* Application.EnableVisualStyles();
                     Application.SetCompatibleTextRenderingDefault(false);*/

                    BonusSkins.Register();
                    SkinManager.EnableFormSkins();
                    var a = new UserLogin();
                    if (a.ShowDialog() != DialogResult.OK) return;
//                    WebServiceInvoker.Token = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJhZG1pbiIsInJlYWxOYW1lIjoi5Luy5bSH5ruUIn0.OetJnklm4_kM0AF3d7Lmgh5ukJ1UclwRkqgZhDIWtSA";
                    SystemConstant.InitConstant();
                    Application.Run(new PostCardCenterMainForm());
//                    Application.Run(new OrderBatch());
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