using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Office.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.ToastNotifications;
using DevExpress.XtraEditors;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Request.system;
using Hacker.Inko.Net.Response.system;

namespace SystemSetting.backStyle
{
    public partial class BackStyleManageForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public BackStyleManageForm()
        {
            InitializeComponent();

            SystemBackStyleApi.GetAllBackStyleFromServer(
                result => { gridControl1.DataSource = result; },
                message => { XtraMessageBox.Show(message); });
        }

        private void BackStyleManageForm_Load(object sender, EventArgs e)
        {
        }

        private void gridView1_FocusedRowLoaded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
            var row = gridView1.GetRow(e.RowHandle);
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var row = gridView1.GetRow(e.FocusedRowHandle);

            if (row is BackStyleResponse backStyleResponse)
            {
                var fileId = backStyleResponse.FileId;
                try
                {
                    var downloadBytesByFileId = FileApi.DownloadBytesByFileId(fileId);
                    pictureEdit1.Image = Image.FromStream(new MemoryStream(downloadBytesByFileId));
                }
                catch (Exception exception)
                {
                    XtraMessageBox.Show(exception.Message);
                }
            }
        }

        private void gridView1_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            var row = gridView1.GetRow(e.RowHandle);

            if (row is BackStyleResponse backStyleResponse)
            {
                SystemBackStyleApi.UpdateBackStyle(backStyleResponse.Id, new BackStyleUpdateRequest
                {
                    Name = backStyleResponse.Name
                }, result =>
                {
                    // XtraMessageBox.Show("更新成功");
                    // var toastNotification = new                    ToastNotification();
                    // toastNotification.Body = "更新成功！";
                    // toastNotification.
                });
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var xtraOpenFileDialog = new XtraOpenFileDialog
            {
                AddExtension = true,
                Filter = @"图片文件| *.png;*.jpg;*.jpeg",
                Multiselect = true
            };
            if (xtraOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var fileName in xtraOpenFileDialog.FileNames)
                {
                    var fileInfo = new FileInfo(fileName);
                    var fileUploadResponse = fileInfo.UploadFile("反面样式");
                    SystemBackStyleApi.InsertBackStyle(
                        "反面样式" + fileInfo.Name,
                        fileUploadResponse.Id,
                        result =>
                        {
                            if (gridControl1.DataSource is List<BackStyleResponse> dataSource)
                            {
                                dataSource.Add(result);
                                gridControl1.RefreshDataSource();
                            }
                        },
                        message => { XtraMessageBox.Show("反面样式插入失败！"); });
                }
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridView1.GetRow(gridView1.FocusedRowHandle) is BackStyleResponse backStyleResponse)
            {
                SystemBackStyleApi.DeleteById(backStyleResponse.Id, result =>
                {
                    if (gridControl1.DataSource is List<BackStyleResponse> backStyleResponses)
                    {
                        backStyleResponses.Remove(backStyleResponse);
                        gridControl1.RefreshDataSource();
                    }
                });
            }
        }
    }
}