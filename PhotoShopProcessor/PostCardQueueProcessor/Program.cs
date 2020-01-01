using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.Threading;
using DevExpress.XtraEditors;
using Hacker.Inko.Net.Base;
using Inko.Security;

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

                    if (Process.GetCurrentProcess().MainModule is ProcessModule processModule)
                    {
                        var dictionary = new Dictionary<string, string>();
                        var fileInfo = new FileInfo(processModule.FileName);
                        fileInfo = new FileInfo(fileInfo.DirectoryName + "/inkoConfig.ini");
                        if (fileInfo.Exists)
                        {
                            var streamReader = new StreamReader(new FileStream(fileInfo.FullName, FileMode.Open));
                            while (!streamReader.EndOfStream)
                            {
                                var line = streamReader.ReadLine();
                                if (line == null) continue;
                                var currentSplit = line.Split('=');
                                if (currentSplit.Length == 2)
                                {
                                    dictionary.Add(currentSplit[0], currentSplit[1]);
                                }
                            }
                            streamReader.Close();

                            var host = dictionary["host"];
                            if (string.IsNullOrEmpty(host))
                            {
                                XtraMessageBox.Show("没有配置host，无法初始化网络请求");
                            }
                            else
                            {
                                if (host.EndsWith("/"))
                                {
                                    host = host.Substring(0, host.Length - 1);
                                }

                                NetGlobalInfo.Host = host;
                            }
                        }
                    }

                    var token = InkoSecurityContext.GetToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        NetGlobalInfo.AccessToken = token;
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