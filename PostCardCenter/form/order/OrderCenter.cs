using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraNavBar;
using postCardCenterSdk;
using postCardCenterSdk.sdk;
using PostCardCrop.form;
using PostCardCrop.model;
using PostCardCrop.translator.response;

namespace PostCardCenter.form.order
{
    public partial class OrderCenter : XtraForm
    {
        public OrderCenter()
        {
            InitializeComponent();
        }

        private void orderDetailGridController_Click(object sender, EventArgs e)
        {
        }

        private void orderDetailGridController_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                popupMenu1.ShowPopup(MousePosition);
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            RefreshOrderList();
        }

        public void RefreshOrderList()
        {
            simpleButton1.Enabled = false;
            WebServiceInvoker.GetInstance().GetOrderDetails(dateEdit1.DateTime, dateEdit2.DateTime, orders =>
            {
                var orderInfoList = new List<OrderInfo>();
                orders.ForEach(orderResponse => { orderInfoList.Add(orderResponse.TranslateToOrderInfo()); });
                orderDetailGridController.DataSource = orderInfoList;
                orderDetailGridController.RefreshDataSource();
                simpleButton1.Enabled = true;
            }, message =>
            {
                XtraMessageBox.Show(message);
                simpleButton1.Enabled = true;
            });
        }


        private void OrderDetailGridController_DoubleClick(object sender, EventArgs e)
        {
            //如果当前选择
            if (!(gridView1.GetFocusedRow() is OrderInfo orderInfo)) return;
            WebServiceInvoker.GetInstance().ChangeOrderProcessor(orderInfo.Id, order =>
                {
                    if (order.ProcessorName != SecurityInfo.UserName)
                    {
                        XtraMessageBox.Show("很抱歉，此订单[" + order.ProcessorName + "]已经开始处理！");
                        return;
                    }

                    orderInfo.ProcessorName = order.ProcessorName;
                    var xtraForm1 = new PostCardCropForm(orderInfo.Id);
                    xtraForm1.ShowDialog(this);
                    if (xtraForm1.NeedRefresh)
                    {
                        RefreshOrderList();
                    }
                },
                message => { XtraMessageBox.Show(message); });
        }

        private void OrderCenter_Load(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            IDictionary<string, TimeArea> oo = new Dictionary<string, TimeArea>();

            oo.Add("今天", new TimeArea(now.Date, now.Date.AddDays(1).AddMilliseconds(-1)));
            oo.Add("昨天", new TimeArea(now.Date.AddDays(-1), now.Date.AddMilliseconds(-1)));
            oo.Add("本月", new TimeArea(new DateTime(now.Year, now.Month, 1), new DateTime(now.Year, now.Month, 1).AddMonths(1).AddMilliseconds(-1)));
            oo.Add("上月", new TimeArea(new DateTime(now.Year, now.Month, 1).AddMonths(-1), new DateTime(now.Year, now.Month, 1).AddMilliseconds(-1)));
            oo.Add("10天内", new TimeArea(now.Date.AddDays(-10), now.Date.AddDays(1).AddMilliseconds(-1)));
            oo.Add("30天内", new TimeArea(now.Date.AddDays(-30), now.Date.AddDays(1).AddMilliseconds(-1)));
            oo.Add("全部", new TimeArea(new DateTime(2000, 1, 1), new DateTime(2999, 1, 1)));

            radioGroup1.Tag = oo;
            radioGroup1.SelectedIndex = 0;
            //刷新列表
            RefreshOrderList();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioGroup a)) return;
            var oo = a.Tag as IDictionary<string, TimeArea>;
            TimeArea timeArea = null;
            oo?.TryGetValue(a.Text, out timeArea);
            if (timeArea == null)
            {
                dateEdit1.DateTime = DateTime.Now.Date;
                dateEdit1.Properties.ReadOnly = false;
                dateEdit2.DateTime = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
                dateEdit2.Properties.ReadOnly = false;
            }
            else
            {
                dateEdit1.DateTime = timeArea.Start;
                dateEdit1.Properties.ReadOnly = true;
                dateEdit2.DateTime = timeArea.End;
                dateEdit2.Properties.ReadOnly = true;
            }

            RefreshOrderList();
        }

        private void gridView1_FocusedRowChanged(object sender,
            FocusedRowChangedEventArgs e)
        {
            if (gridView1.GetFocusedRow() is OrderInfo orderInfo)
                barButtonItem4.Enabled = !"已完成".Equals(orderInfo.ProcessStatus);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(gridView1.GetFocusedRow() is OrderInfo orderInfo)) return;
            if (orderInfo.ProcessorName != SecurityInfo.UserName)
            {
                XtraMessageBox.Show("当前订单已经有负责人，如需交接，请联系负责人[" + orderInfo.ProcessorName + "]");
                return;
            }

            WebServiceInvoker.GetInstance().ChangeOrderProcessor(orderInfo.Id, order => { RefreshOrderList(); }, message => { XtraMessageBox.Show(message); });
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(gridView1.GetFocusedRow() is OrderInfo orderInfo)) return;
            if (orderInfo.ProcessorName != SecurityInfo.UserName)
            {
                XtraMessageBox.Show("只有订单的负责人才能修改订单状态，订单负责人为[" + orderInfo.ProcessorName + "]");
                return;
            }

            if (XtraMessageBox.Show("是否真的将订单状态修改为已处理？", "订单状态修改", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                WebServiceInvoker.GetInstance().ChangeOrderStatus(orderInfo.Id, "4", re => { RefreshOrderList(); });
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle > -1)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void navBarItem1_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            var ba = sender as NavBarItem;
            if (ba.Tag != null)
                orderProcessStatus.FilterInfo = new ColumnFilterInfo(orderProcessStatus, ba.Tag);
            else
                orderProcessStatus.ClearFilter();
        }
    }

    public class TimeArea
    {
        public TimeArea(DateTime startDate, DateTime endDate)
        {
            Start = startDate;
            End = endDate;
        }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}