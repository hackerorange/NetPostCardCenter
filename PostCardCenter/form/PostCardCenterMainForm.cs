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
using Inko.Security.form;
using PostCardQueueProcessor;

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

            barSubItem2.Caption = InkoSecurityContext.UserName;
            barSubItem2.RibbonStyle = RibbonItemStyles.SmallWithText;
            OpenOrderCenter();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (XtraMessageBox.Show("是否真的退出应用", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                e.Cancel = true;
        }


        private void BatchCreateOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            var orderBatchCreate = new OrderBatch();
            orderBatchCreate.ShowDialog(this);
        }

        private void BarButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenOrderCenter();
        }

        private void OpenOrderCenter()
        {
            xtraTabbedMdiManager1.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeader;
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

        private void BarButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            new SizeManageForm().ShowDialog();
        }

        public List<DirectoryInfo> BaseBath { get; set; }

        private void BarButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            new BackStyleManageForm().Show(this);
        }

        private void BarButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            InkoSecurityContext.Logout();
            Hide();
            if (new UserLogin().ShowDialog(this) == DialogResult.OK)
            {
                InkoSecurityContext.GetToken();
                Show();
                barSubItem2.Caption = InkoSecurityContext.UserName;
                barSubItem2.RibbonStyle = RibbonItemStyles.SmallWithText;
            }
            else
            {
                Close();
            }
        }

    }
}