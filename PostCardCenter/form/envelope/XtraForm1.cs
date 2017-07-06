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
using soho.webservice;

namespace PostCardCenter.form.envelope
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm1()
        {
            InitializeComponent();
        }

        public XtraForm1(string orderId) : this()
        {
            var orderDetails = EnvelopeInvoker.GetOrderDetails(orderId);
            gridControl1.DataSource = orderDetails;
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {

        }
    }
}