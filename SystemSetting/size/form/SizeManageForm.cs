using System.Windows.Forms;
using SystemSetting.size.constant;
using SystemSetting.size.model;
using DevExpress.XtraEditors;

namespace SystemSetting.size.form
{
    public partial class SizeManageForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public SizeManageForm()
        {
            InitializeComponent();

            ProductSizeFactory.GetProductSizeListFromServer(postSizeList => { gridControl1.DataSource = postSizeList; });
        }

        private void SizeManageForm_Load(object sender, System.EventArgs e)
        {
        }

        private void BarButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new NewPostCardSizeForm().ShowDialog(this);
            ProductSizeFactory.GetProductSizeListFromServer(postSizeList =>
            {
                gridControl1.DataSource = postSizeList;
                gridView1.RefreshData();
            });
        }

        private void BarButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.GetFocusedRow() is PostSize postSize)
            {
                XtraMessageBox.Show("此功能正在开发中，敬请期待！");
            }
        }

        private void ribbon_Click(object sender, System.EventArgs e)
        {
        }
    }
}