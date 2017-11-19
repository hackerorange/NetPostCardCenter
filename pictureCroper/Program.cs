using System;
using System.Threading;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using pictureCroper.form;

namespace pictureCroper
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
                    SkinManager.EnableFormSkins();
                    UserLookAndFeel.Default.SetSkinStyle("DevExpress Style"); // 设置皮肤样式

                    /* Application.EnableVisualStyles();
                     Application.SetCompatibleTextRenderingDefault(false);*/

                    SkinManager.EnableFormSkins();
                    Application.Run(new CropTestForm());
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