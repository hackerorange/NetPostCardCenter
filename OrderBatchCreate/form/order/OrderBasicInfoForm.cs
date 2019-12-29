using System;
using System.Windows.Forms;

namespace OrderBatchCreate.form.order
{
    public partial class OrderBasicInfoForm : DevExpress.XtraEditors.XtraForm
    {
        public OrderBasicInfoForm()
        {
            InitializeComponent();
        }

        public string TaobaoId { get; set; }

        public bool Urgent { get; set; }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            TaobaoId = textBox1.Text;
            Urgent = checkEdit1.Checked;
            DialogResult = DialogResult.OK;
        }
    }
}