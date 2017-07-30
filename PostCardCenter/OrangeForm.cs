using System;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraTab;
using DevExpress.XtraTabbedMdi;
using PostCardCenter.form;
using PostCardCenter.form.order;
using PostCardCenter.security;
using soho.domain;
using soho.security;

namespace PostCardCenter
{
    public partial class OrangeForm : RibbonForm
    {
        public OrangeForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var formUserLogin = new UserLogin();
            while (Security.AccountSessionInfo == null)
            {
                if (formUserLogin.ShowDialog(this) != DialogResult.Cancel) continue;
                Application.Exit();
                break; //中断当前循环
            }
            if (Security.AccountSessionInfo != null)
            {
                barStaticItem1.Caption = Security.AccountSessionInfo.realName;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void batchCreateOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            var orderBatchCreate = new OrderBatchCreateForm();
            if (orderBatchCreate.ShowDialog(this) == DialogResult.OK)
            {
                var openOrderCenter = OpenOrderCenter();
                openOrderCenter.RefreshOrderList();
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenOrderCenter();
        }

        private OrderCenter OpenOrderCenter()
        {
            xtraTabbedMdiManager1.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeader;
            var flag = false;
            foreach (XtraMdiTabPage page in xtraTabbedMdiManager1.Pages)
            {
                if (page.MdiChild.GetType().FullName != typeof(OrderCenter).FullName) continue;
                xtraTabbedMdiManager1.SelectedPage = page;
                var pageMdiChild = page.MdiChild as OrderCenter;
                return pageMdiChild;
            }
            var orderCenter = new OrderCenter
            {
                MdiParent = this
            };
            orderCenter.Show();
            xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[orderCenter];
            return orderCenter;
        }
    }
}