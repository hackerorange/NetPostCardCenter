using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using PostCardCenter.form.postCard;
using soho.domain;
using soho.domain.orderCenter;
using postCardCenterSdk.sdk;
using soho.translator;
using soho.translator.response;

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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            RefreshOrderList();
        }

        public void RefreshOrderList()
        {
            WebServiceInvoker.GetOrderDetails(dateEdit1.DateTime, dateEdit2.DateTime, orders =>
            {
                var orderInfoList = new List<OrderInfo>();
                orders.ForEach(orderResponse =>
                {
                    orderInfoList.Add(orderResponse.TranslateToOrderInfo());
                });
                orderDetailGridController.DataSource = orderInfoList;
                orderDetailGridController.RefreshDataSource();
            }, message => { XtraMessageBox.Show(message); });
        }


        private void orderDetailGridController_DoubleClick(object sender, EventArgs e)
        {
            var focusedRow = gridView1.GetFocusedRow() as OrderInfo;
            if (focusedRow != null)
            {
                var xtraForm1 = new PostCardCropForm(focusedRow.Id);
                xtraForm1.Show();
            }
        }

        private void OrderCenter_Load(object sender, EventArgs e)
        {
            IDictionary<string, TimeArea> oo = new Dictionary<string, TimeArea>();
            var now = DateTime.Now;
            oo.Add(
                "今天",
                new TimeArea(
                    now.Date,
                    now.Date.AddDays(1).AddMilliseconds(-1)
                )
            );
            oo.Add(
                "昨天",
                new TimeArea(
                    now.Date.AddDays(-1),
                    now.Date.AddMilliseconds(-1)
                )
            );
            oo.Add("本月",
                new TimeArea(
                    new DateTime(now.Year, now.Month, now.Day),
                    new DateTime(now.Year, now.Month, now.Day).AddMonths(1).AddMilliseconds(-1)));
            oo.Add("上月",
                new TimeArea(new DateTime(now.Year, now.Month, now.Day).AddMonths(-1),
                    new DateTime(now.Year, now.Month, now.Day).AddMilliseconds(-1)));
            oo.Add(
                "10天内",
                new TimeArea(
                    now.Date.AddDays(-10),
                    now.Date.AddDays(1).AddMilliseconds(-1)
                )
            );
            oo.Add(
                "30天内",
                new TimeArea(
                    now.Date.AddDays(-30),
                    now.Date.AddDays(1).AddMilliseconds(-1)
                )
            );
            oo.Add(
                "全部",
                new TimeArea(
                    new DateTime(2000, 1, 1),
                    new DateTime(2999, 1, 1)
                )
            );
            radioGroup1.Tag = oo;
            radioGroup1.SelectedIndex = 0;
            //刷新列表
            RefreshOrderList();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = sender as RadioGroup;
            if (a == null) return;
            var oo = a.Tag as IDictionary<string, TimeArea>;
            TimeArea timeArea = null;
            if (oo != null) oo.TryGetValue(a.Text, out timeArea);
            if (timeArea == null)
            {
                dateEdit1.DateTime = DateTime.Now.Date;
                dateEdit1.Properties.ReadOnly = false;
                dateEdit2.DateTime = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
                dateEdit2.Properties.ReadOnly = false;
            }
            else
            {
                dateEdit1.DateTime = timeArea.start;
                dateEdit1.Properties.ReadOnly = true;
                dateEdit2.DateTime = timeArea.end;
                dateEdit2.Properties.ReadOnly = true;
            }
        }

        private void gridView1_FocusedRowChanged(object sender,
            DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var orderInfo = gridView1.GetFocusedRow() as OrderInfo;
            if (orderInfo != null)
            {
                //如果当前没有处理人，我来处理可用
                barButtonItem1.Enabled = string.IsNullOrEmpty(orderInfo.ProcessorName);
                barButtonItem4.Enabled = !"已完成".Equals(orderInfo.ProcessStatus);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var orderInfo = gridView1.GetFocusedRow() as OrderInfo;
            if (orderInfo == null) return;
            WebServiceInvoker.ChangeOrderProcessor(orderInfo.Id, order => { RefreshOrderList(); },
                message => { XtraMessageBox.Show(message); });
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var orderInfo = gridView1.GetFocusedRow() as OrderInfo;
            if (orderInfo == null) return;
            WebServiceInvoker.ChangeOrderStatus(orderInfo.Id, "4", re => { RefreshOrderList(); });
        }
    }

    public class TimeArea
    {
        public TimeArea(DateTime startDate, DateTime endDate)
        {
            start = startDate;
            end = endDate;
        }

        public DateTime start { get; set; }

        public DateTime end { get; set; }
    }
}