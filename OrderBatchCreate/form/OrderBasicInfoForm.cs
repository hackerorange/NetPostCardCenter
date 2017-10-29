using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace OrderBatchCreate.form
{
    public partial class OrderBasicInfoForm : XtraForm
    {
        private readonly DirectoryInfo _directoryInfo;

        public OrderBasicInfoForm(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
            InitializeComponent();
            textEdit2.Text = directoryInfo.FullName;
            //绑定明信片列数
        }

        public string TaobaoId => textEdit1.Text;

        public bool Urgent => checkEdit1.Checked;

        private void OrderBasicInfoForm_Load(object sender, EventArgs e)
        {
            checkEdit1.Checked = _directoryInfo.FullName.Contains("加急");
            textEdit1.Focus();
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textEdit1.Text))
            {
                XtraMessageBox.Show("请输入用户TaoBaoId!");
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}