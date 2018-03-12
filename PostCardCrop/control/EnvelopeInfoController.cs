using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using postCardCenterSdk.sdk;
using PostCardCrop.model;
using PostCardCrop.translator.response;

namespace PostCardCrop.control
{
    public partial class EnvelopeInfoController : UserControl
    {
        private EnvelopeInfo _envelopeInfo;
//        private EnvelopeInfo _envelope;

        public EnvelopeInfoController()
        {
            InitializeComponent();
        }

        public EnvelopeInfo EnvelopeInfo
        {
            get => _envelopeInfo;
            set
            {
                _envelopeInfo = value;
                RefreshEnvelopeInfo();
            }
        }

        public void RefreshEnvelopeInfo()
        {
            if (_envelopeInfo == null) return;
            var tmpEnvelope = _envelopeInfo;
            WebServiceInvoker.GetInstance().GetEnvelopeInfoById(tmpEnvelope.Id, result =>
            {
                var envelopeInfo = result.TranslateToEnvelope();
                //更新成品ID
                tmpEnvelope.ProductFileId = envelopeInfo.ProductFileId;

                if (!string.IsNullOrEmpty(tmpEnvelope.ProductFileId))
                {
                    downloadProductFile.Tag = tmpEnvelope.ProductFileId;
                    downloadProductFile.Enabled = true;
                }
                else
                {
                    downloadProductFile.Tag = null;
                    downloadProductFile.Enabled = false;
                }
                WebServiceInvoker.GetInstance().GetOrderInfo(tmpEnvelope.OrderId,
                    orderInfo => { customerName.Text = orderInfo.TaobaoId; },
                    message => { XtraMessageBox.Show(message); });
                paperName.Text = tmpEnvelope.PaperName;
                productWidth.Text = tmpEnvelope.ProductSize.Width.ToString();
                productHeight.Text = tmpEnvelope.ProductSize.Height.ToString();
            });
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            if (_envelopeInfo == null) return;
            var tmpEnvelope = _envelopeInfo;

            var fileName = customerName.Text;
            fileName += "_" + tmpEnvelope.PaperName + (tmpEnvelope.DoubleSide ? "_双面" : "_单面");
            var postCardCount = 0;
            tmpEnvelope.PostCards.ForEach(tmpPostCard => { postCardCount += tmpPostCard.Copy; });
            fileName += "_共" + postCardCount + "张";

            //_envelope.PostCards.

            var saveFileDialog = new SaveFileDialog
            {
                FileName = fileName,
                Filter = @"PDF文件|*.pdf",
                OverwritePrompt = true,
                DefaultExt = "pdf"
            };


            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            WebServiceInvoker.GetInstance().GetEnvelopeInfoById(tmpEnvelope.Id, tmpresult =>
            {
                var result = tmpresult.TranslateToEnvelope();
                var productFileId = result.ProductFileId;
                if (string.IsNullOrEmpty(productFileId))
                {
                    XtraMessageBox.Show("此集合没有成品，请稍后重试");
                    return;
                }
                if (File.Exists(saveFileDialog.FileName))
                    try
                    {
                        File.Delete(saveFileDialog.FileName);
                    }
                    catch
                    {
                        XtraMessageBox.Show("文件已经被占用，请关闭后重新下载!");
                        return;
                    }
                var fileInfo = new FileInfo(saveFileDialog.FileName);

                WebServiceInvoker.GetInstance().DownLoadFileByFileId(productFileId, fileInfo, downloadFileInfo =>
                {
                    layoutControlItem8.Visibility = LayoutVisibility.Never;
                    if (XtraMessageBox.Show("文件下载完成，是否打开文件", "下载完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;
                    Process.Start("explorer.exe", downloadFileInfo.FullName);
                }, proce =>
                {
                    layoutControlItem8.Visibility = LayoutVisibility.Always;
                    progressBarControl1.EditValue = proce;
                });
            });
        }

        private void SimpleButton2_Click(object sender, EventArgs e)
        {
            if (_envelopeInfo != null)
                EnvelopeInfo = _envelopeInfo;
        }
    }
}