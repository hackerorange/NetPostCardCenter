using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemSetting.size.constant;
using SystemSetting.size.model;
using DevExpress.XtraEditors;

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

            ProductSizeFactory.InsertNewPostSize(textEdit1.Text, width, height, success =>
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