using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SystemSetting.backStyle;
using SystemSetting.size.form;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraTabbedMdi;
using Hacker.Inko.Net.Base;
using postCardCenter.form.order;
using OrderBatchCreate.form;
using Inko.Security;

namespace postCardCenter.form
{
    public partial class PostCardCenterMainForm : RibbonForm
    {
        public PostCardCenterMainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NetGlobalInfo.AccessToken))
            {
                Application.Exit();
                return;
            }

            barStaticItem1.Caption = InkoSecurityContext.UserName;
            OpenOrderCenter();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (XtraMessageBox.Show("是否真的退出应用", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                e.Cancel = true;
        }


        private void batchCreateOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            var orderBatchCreate = new OrderBatch();
            orderBatchCreate.ShowDialog(this);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenOrderCenter();
        }

        private void OpenOrderCenter()
        {
            xtraTabbedMdiManager1.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeader;
            var flag = false;
            foreach (XtraMdiTabPage page in xtraTabbedMdiManager1.Pages)
            {
                if (page.MdiChild.GetType().FullName != typeof(OrderCenter).FullName) continue;
                xtraTabbedMdiManager1.SelectedPage = page;
                return;
            }

            var orderCenter = new OrderCenter
            {
                MdiParent = this
            };
            orderCenter.Show();
            xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[orderCenter];
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            new SizeManageForm().ShowDialog();
        }

        public List<DirectoryInfo> BaseBath { get; set; }


        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //var enumerator = BaseBath.GetEnumerator();
            //while (enumerator.MoveNext())
            //{

            //}
            //enumerator.Dispose();


            //new OrderCreateForm().Show();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            new BackStyleManageForm().Show(this);
        }
    }
}