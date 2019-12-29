using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Request.system;

namespace SystemSetting.size.form
{
    public partial class NewPostCardSizeForm : DevExpress.XtraEditors.XtraForm
    {
        public NewPostCardSizeForm()
        {
            InitializeComponent();
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textEdit1.Text))
            {
                XtraMessageBox.Show("请输入尺寸名称！");
                return;
            }

            var width = (int) spinEdit1.Value;
            var height = (int) spinEdit2.Value;

            SystemSizeApi.InsertProductSizeToServer("成品尺寸", new SizeRequest
            {
                Height = height,
                Width = width,
                Name = textEdit1.Text
            }, success =>
            {
                XtraMessageBox.Show("尺寸插入成功！");
                DialogResult = DialogResult.OK;
            });
        }

        private void NewPostCardSizeForm_Load(object sender, EventArgs e)
        {
        }
    }
}