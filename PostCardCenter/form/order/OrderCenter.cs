using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraNavBar;
using Hacker.Inko.Net.Api;
using Inko.Security;
using PostCardCenter.form.collection;
using PostCardCrop.form;
using PostCardCrop.model;
using PostCardCrop.translator.response;

namespace postCardCenter.form.order
{
    public partial class OrderCenter : XtraForm
    {
        private OrderInfo FocusOrderInfo { get; set; }

        public OrderCenter()
        {
            InitializeComponent();
        }

        private void OrderDetailGridController_Click(object sender, EventArgs e)
        {
        }

        private void OrderDetailGridController_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu1.ShowPopup(MousePosition);
            }
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            RefreshOrderList();
        }

        public void RefreshOrderList()
        {
            simpleButton1.Enabled = false;
            PostCardBillApi.GetAllOrderFilterByCreateTime(dateEdit1.DateTime, dateEdit2.DateTime, orders =>
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
            //如果当前不是订单，跳过
            if (!(gridView1.GetFocusedRow() is OrderInfo orderInfo)) return;

            if (string.IsNullOrEmpty(orderInfo.ProcessorName))
            {
                // 将当前订单的处理人设置为当前用户
                PostCardBillApi.ChangeOrderProcessor(
                    orderInfo.Id,
                    result =>
                    {
                        orderInfo.ProcessorName = result.ProcessorName;
                        orderInfo.ProcessUserId = result.ProcessUserId;
                        new PostCardCropForm(orderInfo.Id).ShowDialog(this);
                    },
                    message => { XtraMessageBox.Show("订单处理人更新失败，请联系管理员"); });
            }
            else
            {
                // 如果有多人同时提交，取抢到的用户
                if (orderInfo.ProcessUserId != InkoSecurityContext.UserId)
                {
                    XtraMessageBox.Show("当前订单已经有人在处理，当前处理人:" + orderInfo.ProcessorName);
                    return;
                }

                new PostCardCropForm(orderInfo.Id).ShowDialog(this);
            }
        }

        private void OrderCenter_Load(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            radioGroup1.Tag = new Dictionary<string, TimeArea>
            {
                {"今天", new TimeArea(now.Date, now.Date.AddDays(1).AddMilliseconds(-1))},
                {"昨天", new TimeArea(now.Date.AddDays(-1), now.Date.AddMilliseconds(-1))},
                {"本月", new TimeArea(new DateTime(now.Year, now.Month, 1), new DateTime(now.Year, now.Month, 1).AddMonths(1).AddMilliseconds(-1))},
                {"上月", new TimeArea(new DateTime(now.Year, now.Month, 1).AddMonths(-1), new DateTime(now.Year, now.Month, 1).AddMilliseconds(-1))},
                {"10天内", new TimeArea(now.Date.AddDays(-10), now.Date.AddDays(1).AddMilliseconds(-1))},
                {"30天内", new TimeArea(now.Date.AddDays(-30), now.Date.AddDays(1).AddMilliseconds(-1))},
                {"全部", new TimeArea(new DateTime(2000, 1, 1), new DateTime(2999, 1, 1))}
            };
            radioGroup1.SelectedIndex = 0;
            //刷新列表
            RefreshOrderList();
        }

        private void RadioGroup1_SelectedIndexChanged(object sender, EventArgs e)
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


        private void GridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            FocusOrderInfo = gridView1.GetFocusedRow() as OrderInfo;
            if (FocusOrderInfo != null)
            {
                barButtonItem4.Enabled = !"16".Equals(FocusOrderInfo.ProcessStatusCode);
                // 完成状态
                switch (FocusOrderInfo.ProcessStatusCode)
                {
                    case "0": // 未开始
                        barButtonItem2.Enabled = false;
                        barButtonItem4.Enabled = true;
                        break;
                    case "2": // 未完成
                        barButtonItem2.Enabled = false;
                        barButtonItem4.Enabled = true;
                        break;
                    case "4": // 已完成
                        barButtonItem2.Enabled = true;
                        barButtonItem4.Enabled = true;
                        break;
                    case "8": // 已生成PDF
                        barButtonItem2.Enabled = true;
                        barButtonItem4.Enabled = true;
                        break;
                    case "16": // 已关闭
                        barButtonItem2.Enabled = true;
                        barButtonItem4.Enabled = true;
                        break;
                }
            }
        }

        private void BarButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (FocusOrderInfo == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(FocusOrderInfo.ProcessUserId) && FocusOrderInfo.ProcessUserId != InkoSecurityContext.UserId)
            {
                XtraMessageBox.Show("当前订单已经有负责人，请联系负责人[" + FocusOrderInfo.ProcessorName + "]");
                return;
            }

            PostCardBillApi.ChangeOrderProcessor(FocusOrderInfo.Id,
                result => { });
        }

        private void BarButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (FocusOrderInfo == null)
            {
                return;
            }

            if (FocusOrderInfo.ProcessUserId != InkoSecurityContext.UserId)
            {
                XtraMessageBox.Show("只有订单的处理人才能修改订单状态，订单负责人为：" + FocusOrderInfo.ProcessorName);
                return;
            }

            if (XtraMessageBox.Show("是否真的将订单状态修改为已处理？", "订单状态修改", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                PostCardBillApi.ChangeOrderStatus(FocusOrderInfo.Id, "4", re => { RefreshOrderList(); });
        }

        private void GridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle > -1)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void NavBarItem1_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            var ba = sender as NavBarItem;
            if (ba?.Tag != null)
            {

                orderProcessStatus.FilterInfo = new ColumnFilterInfo(orderProcessStatus, ba.Tag);
            }
            else
            {
                orderProcessStatus.ClearFilter();
            }
        }

        private void BarButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void BarButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            new PdfGenerator(FocusOrderInfo.Id).Show(this);
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