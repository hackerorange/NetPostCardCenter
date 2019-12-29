using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Hacker.Inko.PostCard.Library;

namespace OrderBatchCreate.form
{
    public partial class CustomerProductSizeForm : XtraForm
    {
        public CustomerProductSizeForm()
        {
            InitializeComponent();
            //绑定高度
            productHeightSpinEdit.DataBindings.Clear();
            productHeightSpinEdit.DataBindings.Add("EditValue", ProductSize, "Height", false, DataSourceUpdateMode.OnPropertyChanged);
            //绑定宽度
            productWidthSpinEdit.DataBindings.Clear();
            productWidthSpinEdit.DataBindings.Add("EditValue", ProductSize, "Width", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        /// <summary>
        ///     成品尺寸
        /// </summary>
        public PostSize ProductSize { get; } = new PostSize {Width = 148, Height = 100, Name = "自定义"};

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void SimpleButton2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }


        /// <summary>
        ///     启动后，修改纸张尺寸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerProductSizeForm_Load(object sender, EventArgs e)
        {
            productHeightSpinEdit.Value = 100;
            productWidthSpinEdit.Value = 148;
        }
    }
}