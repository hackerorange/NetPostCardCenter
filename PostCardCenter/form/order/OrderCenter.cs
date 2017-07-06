using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using PostCardCenter.form.envelope;
using PostCardCenter.form.postCard;
using soho.domain.orderCenter;
using soho.webservice;

namespace PostCardCenter.form.order
{
    public partial class OrderCenter : DevExpress.XtraEditors.XtraForm
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
            {
                popupMenu1.ShowPopup(Control.MousePosition);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var orderDetails = OrderCenterInvoker.GetOrderDetails(dateEdit1.DateTime, dateEdit2.DateTime);
            orderDetailGridController.DataSource = orderDetails;
            orderDetailGridController.RefreshDataSource();
        }

        private void orderDetailGridController_DoubleClick(object sender, EventArgs e)
        {
            var focusedRow = gridView1.GetFocusedRow() as EnvelopeInfo;
            if (focusedRow != null)
            {
                var xtraForm1 = new PostCardCropForm(focusedRow.orderId);
                xtraForm1.Show();
            }
        }

        private void OrderCenter_Load(object sender, EventArgs e)
        {
            IDictionary <String, TimeArea> oo = new Dictionary<string, TimeArea>();
            DateTime now = DateTime.Now;
            oo.Add("今天", new TimeArea(now.Date, now.Date.AddDays(1).AddMilliseconds(-1)));
            oo.Add("昨天", new TimeArea(now.Date.AddDays(-1), now.Date.AddMilliseconds(-1)));
            oo.Add("本月", new TimeArea(new DateTime(now.Year, now.Month, now.Day), new DateTime(now.Year, now.Month, now.Day).AddMonths(1).AddMilliseconds(-1)));
            oo.Add("上月", new TimeArea(new DateTime(now.Year, now.Month, now.Day).AddMonths(-1), new DateTime(now.Year, now.Month, now.Day).AddMilliseconds(-1)));
            oo.Add("10天内", new TimeArea(now.Date.AddDays(-10), now.Date.AddDays(1).AddMilliseconds(-1)));
            oo.Add("30天内", new TimeArea(now.Date.AddDays(-30), now.Date.AddDays(1).AddMilliseconds(-1)));
            oo.Add("全部", new TimeArea(new DateTime(2000,1,1), new DateTime(2999,1,1)));
            radioGroup1.Tag = oo;
            radioGroup1.SelectedIndex = 0;
            
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var a = sender as DevExpress.XtraEditors.RadioGroup;
            var oo = a.Tag as IDictionary<String, TimeArea>;
            TimeArea timeArea;
            oo.TryGetValue(a.Text, out timeArea);
            if (timeArea == null)
            {
                dateEdit1.DateTime = DateTime.Now.Date;
                dateEdit1.Properties.ReadOnly = false;
                dateEdit2.DateTime = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
                dateEdit2.Properties.ReadOnly =false;
            }
            else
            {
                dateEdit1.DateTime = timeArea.start;
                dateEdit1.Properties.ReadOnly = true;
                dateEdit2.DateTime = timeArea.end;
                dateEdit2.Properties.ReadOnly = true;
            }
        }
    }
    public class TimeArea
    {
        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public TimeArea(DateTime startDate, DateTime endDate)
        {
            this.start = startDate;
            this.end = endDate;
        }
    }
}