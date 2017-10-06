using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.Skins;
using PostCardCenter.form;
using PostCardCenter.form.order;
using PostCardCenter.form.postCard;
using PostCardCenter.security;

namespace PostCardCenter
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once ArrangeTypeMemberModifiers
        static void Main()
        {
            bool createNew;
            using (new System.Threading.Mutex(true, Application.ProductName, out createNew))
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
                    if (a.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new OrangeForm());
                    }
                }
                else
                {
                    MessageBox.Show(@"应用程序已经在运行中...");
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(1);
                }
            }
        }

    }
}