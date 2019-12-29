using System;
using System.Windows.Forms;

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