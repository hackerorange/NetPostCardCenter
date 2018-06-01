using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace OrderBatchCreate.form.order
{
    public partial class OrderCreateForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public OrderCreateForm()
        {
            InitializeComponent();
        }

        private void OrderCreateForm_Load(object sender, EventArgs e)
        {
            var orderBasicInfo=new OrderBasicInfoForm();
            if (orderBasicInfo.ShowDialog() == DialogResult.OK)
            {

            }
            else
            {
                this.Close();
            }
        }
    }
}