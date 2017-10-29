using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace OrderBatchCreate.form
{
    public partial class PostCardCopySet : XtraForm
    {
        public PostCardCopySet()
        {
            InitializeComponent();
        }

        public bool IsCopy { get; set; } = true;

        public int Number => (int) spinEdit1.Value;

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            spinEdit1.Value = 1;
        }

        private void SimpleButton3_Click(object sender, EventArgs e)
        {
            IsCopy = false;
            DialogResult = DialogResult.OK;
        }

        private void SimpleButton2_Click_1(object sender, EventArgs e)
        {
            IsCopy = true;
            DialogResult = DialogResult.OK;
        }
    }
}