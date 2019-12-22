using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using postCardCenterSdk.request.postCard;
using postCardCenterSdk.sdk;
using PostCardCrop.model;
using PostCardCrop.translator.response;
using postCardCenterSdk.helper;

namespace PostCardCrop.control
{
    public partial class PostCardInfoController : UserControl
    {
        public delegate void PostCardModifiedHandler(PostCardInfo node);

        private PostCardInfo _postCardInfo;


        public PostCardInfoController()
        {
            InitializeComponent();
        }

        public PostCardInfo PostCardInfo
        {
            get => _postCardInfo;
            set
            {
                _postCardInfo = value;
                if (_postCardInfo != null)
                    postCardFileNameTextEdit.Text = _postCardInfo.FileName;
            }
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            if (_postCardInfo == null) return;

            var saveFileDialog = new SaveFileDialog
            {
                FileName = _postCardInfo.FileName,
                Filter = @"图像文件|*.jpg|所有文件|*.*",
                OverwritePrompt = true,
                FilterIndex = 1
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            //FileInfo fileInfo = new FileInfo();

            try
            {
                if (File.Exists(saveFileDialog.FileName))
                    File.Delete(saveFileDialog.FileName);
                var fileInfo = new FileInfo(saveFileDialog.FileName);
                WebServiceInvoker.GetInstance().DownLoadFileByFileId(_postCardInfo.FileId, fileInfo, downloadFileInfo =>
                {
                    layoutControlItem4.Visibility = LayoutVisibility.Never;
                    if (XtraMessageBox.Show("文件下载完成，是否使用PhotoShop打开文件", "下载完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;
                    try
                    {
                        Process.Start("photoshop.exe", downloadFileInfo.FullName);
                    }
                    catch
                    {
                        XtraMessageBox.Show("调用PhotoShop失败，可能没有安装PhotoShop或者文件格式有误");
                    }
                }, proce =>
                {
                    layoutControlItem4.Visibility = LayoutVisibility.Always;
                    progressBarControl1.EditValue = proce;
                });
            }
            catch
            {
                XtraMessageBox.Show("文件被占用，无法删除，操作取消！");
            }
        }

        private void SimpleButton2_Click(object sender, EventArgs e)
        {
            var tmpPostCardInfo = _postCardInfo;
            if (tmpPostCardInfo == null) return;

            var saveFileDialog = new OpenFileDialog
            {
                FileName = _postCardInfo.FileName,
                Filter = @"所有文件|*.*| 图像文件|*.jpg;*.png;*.tiff ",
                FilterIndex = 1
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            var fileInfo = new FileInfo(saveFileDialog.FileName);


            fileInfo.UploadSynchronize("明信片原始文件",
                success: result =>
                {
                    if (result.ImageAvailable)
                    {
                        tmpPostCardInfo.FileId = result.FileId;
                        tmpPostCardInfo.FileName = tmpPostCardInfo.FileInfo.Name;

                        var request = new PostCardInfoPatchRequest
                        {
                            PostCardId = tmpPostCardInfo.PostCardId,
                            FileId = result.FileId,
                            FileName = fileInfo.Name
                        };
                        //TODO:重新上传废弃
                        //WebServiceInvoker.GetInstance().ChangePostCardFrontStyle(request, resp =>
                        //{
                        //    var postCardInfo = resp.TranlateToPostCard();
                        //    if (tmpPostCardInfo.FileInfo != null)
                        //        tmpPostCardInfo.FileInfo = null;
                        //    tmpPostCardInfo.FileId = postCardInfo.FileId;
                        //    tmpPostCardInfo.FileName = postCardInfo.FileName;
                        //    tmpPostCardInfo.ProcessStatus = postCardInfo.ProcessStatus;
                        //    tmpPostCardInfo.ProcessStatusText = postCardInfo.ProcessStatusText;
                        //    FileChanged?.Invoke(tmpPostCardInfo);
                        //}, message => { XtraMessageBox.Show(message); });
                        //Application.DoEvents();
                    }
                    else
                    {
                        XtraMessageBox.Show("上传的文件不是图像文件，请重新另存！");
                    }
                },
                failure: message => { XtraMessageBox.Show("文件上传失败！" + message); });
        }
    }
}