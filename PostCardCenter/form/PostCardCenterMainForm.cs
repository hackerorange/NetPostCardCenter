using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SystemSetting.size.form;
using DevExpress.Utils.Internal;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraTabbedMdi;
using OrderBatchCreate.form;
using OrderBatchCreate.form.order;
using PostCardCenter.form.order;
using soho.security;

namespace PostCardCenter.form
{
    public partial class PostCardCenterMainForm : RibbonForm
    {
        public PostCardCenterMainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Security.AccountSessionInfo == null)
            {
                Application.Exit();
                return;
            }
            barStaticItem1.Caption = Security.AccountSessionInfo.RealName;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Security.AccountSessionInfo == null)
                return;

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
            var enumerator = BaseBath.GetEnumerator();
            while (enumerator.MoveNext())
            {
                
            }
            enumerator.Dispose();


            new OrderCreateForm().Show();
        }
    }
}