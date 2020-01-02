using System.Collections.Generic;
using DevExpress.Office.Utils;
using DevExpress.XtraEditors;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Request.system;
using Hacker.Inko.Net.Response.system;

namespace SystemSetting.size.form
{
    public partial class SizeManageForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public SizeManageForm()
        {
            InitializeComponent();
            SystemSizeApi.GetSizeInfoFromServer("productSize", postSizeList => { gridControl1.DataSource = postSizeList; });
        }

        private void SizeManageForm_Load(object sender, System.EventArgs e)
        {
        }

        private void BarButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barButtonItem1.Enabled = false;
            SystemSizeApi.InsertProductSizeToServer("productSize", new SizeRequest
            {
                Height = 100,
                Width = 148,
                Name = "默认尺寸"
            }, result =>
            {
                if (gridControl1.DataSource is List<PostCardSizeResponse> postCardSizeResponses)
                {
                    postCardSizeResponses.Add(result);
                    gridControl1.RefreshDataSource();
                }

                barButtonItem1.Enabled = true;
            }, failure => { barButtonItem1.Enabled = true; });
        }

        private void BarButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.GetFocusedRow() is PostCardSizeResponse postSize)
                SystemSizeApi.DeleteById(postSize.Id, success =>
                {
                    if (gridControl1.DataSource is List<PostCardSizeResponse> postCardSizeResponses)
                    {
                        postCardSizeResponses.Remove(postSize);
                        gridControl1.RefreshDataSource();
                    }
                }, message => { XtraMessageBox.Show(message); });
        }

        private void GridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (gridView1.GetRow(gridView1.FocusedRowHandle) is PostCardSizeResponse postCardSizeResponse)
            {
                SystemSizeApi.UpdateProductSizeToServer(postCardSizeResponse.Id, new SizeRequest
                {
                    Name = postCardSizeResponse.Name,
                    Width = postCardSizeResponse.Width,
                    Height = postCardSizeResponse.Height
                }, success => { });
            }
        }
    }
}