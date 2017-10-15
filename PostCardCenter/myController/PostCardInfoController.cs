using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using soho.domain;
using postCardCenterSdk.sdk;
using System.IO;
using DevExpress.XtraLayout.Utils;
using System.Diagnostics;
using DevExpress.XtraEditors;
using soho.translator;
using postCardCenterSdk.request.postCard;
using soho.translator.response;

namespace PostCardCenter.myController
{
    public partial class PostCardInfoController : UserControl
    {

        public delegate void PostCardModifiedHandler(PostCardInfo node);

        public event PostCardModifiedHandler FileChanged;
        

        public PostCardInfoController()
        {
            InitializeComponent();
        }

        private PostCardInfo _postCardInfo;

        public PostCardInfo postCardInfo {

            get {
                return _postCardInfo;
            }
            set {
                _postCardInfo = value;
                if (_postCardInfo != null)
                {
                    postCardFileNameTextEdit.Text = _postCardInfo.FileName;
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (_postCardInfo == null) return;

            var saveFileDialog = new SaveFileDialog
            {
                FileName = _postCardInfo.FileName,
                Filter = @"图像文件|*.jpg|所有文件|*.*",                
                OverwritePrompt=true,
                FilterIndex = 1
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            //FileInfo fileInfo = new FileInfo();

            try
            {
                if (File.Exists(saveFileDialog.FileName))
                {
                    File.Delete(saveFileDialog.FileName);
                }
                FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);
                WebServiceInvoker.DownLoadFileByFileId(_postCardInfo.FileId, fileInfo, success: downloadFileInfo =>
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

                    }, process: proce =>
                    {
                        layoutControlItem4.Visibility = LayoutVisibility.Always;
                        progressBarControl1.EditValue = proce;
                    });
            }
            catch
            {
                XtraMessageBox.Show("文件被占用，无法删除，操作取消！");
                return;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            PostCardInfo tmpPostCardInfo = _postCardInfo;
            if (tmpPostCardInfo == null) return;

            var saveFileDialog = new OpenFileDialog
            {
                FileName = _postCardInfo.FileName,
                Filter = @"所有文件|*.*| 图像文件|*.jpg;*.png;*.tiff ",
                FilterIndex = 1
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            FileInfo fileInfo =new FileInfo(saveFileDialog.FileName);


            
            fileInfo.Upload("明信片原始文件",
            success: result =>
            {
                tmpPostCardInfo.FileId = result;
                tmpPostCardInfo.FileName = tmpPostCardInfo.FileInfo.Name;

                PostCardInfoPatchRequest request = new PostCardInfoPatchRequest()
                {
                    PostCardId = tmpPostCardInfo.PostCardId,
                    FileId = result,
                    FileName = fileInfo.Name
                };

                WebServiceInvoker.ChangePostCardFrontStyle(request, success: resp =>
                {

                    
                    var postCardInfo = resp.TranlateToPostCard();
                    if (tmpPostCardInfo.FileInfo != null)
                    {                        
                        tmpPostCardInfo.FileInfo = null;
                    }
                    tmpPostCardInfo.FileId = postCardInfo.FileId;
                    tmpPostCardInfo.CropInfo = null;
                    tmpPostCardInfo.FileName = postCardInfo.FileName;
                    tmpPostCardInfo.ProcessStatus = postCardInfo.ProcessStatus;
                    tmpPostCardInfo.ProcessStatusText = postCardInfo.ProcessStatusText;
                    FileChanged?.Invoke(tmpPostCardInfo);                    
                }, failure: message =>
                {
                    XtraMessageBox.Show(message);
                });
                Application.DoEvents();
            },
            failure: message =>
            {
                XtraMessageBox.Show("文件上传失败！" + message);                
            });
        }
    }
}
