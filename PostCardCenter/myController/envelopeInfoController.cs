using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using soho.domain;
using postCardCenterSdk.sdk;
using soho.translator;
using soho.translator.response;

namespace PostCardCenter.myController
{
    public partial class EnvelopeInfoController : UserControl
    {
        private EnvelopeInfo _envelope;
              
        public EnvelopeInfoController()
        {
            InitializeComponent();
        }

        private string _envelopeId;

        public string EnvelopeId
        {
            get { return _envelopeId; }
            set
            {
                _envelopeId = value;
                RefreshEnvelopeInfo();
            }
        }

        public void RefreshEnvelopeInfo()
        {
            if (string.IsNullOrEmpty(_envelopeId)) return;
            WebServiceInvoker.GetEnvelopeInfoById(_envelopeId, result =>
            {
                _envelope = result.TranslateToEnvelope();
                if (!string.IsNullOrEmpty(_envelope.ProductFileId))
                {
                    downloadProductFile.Tag = _envelope.ProductFileId;
                    downloadProductFile.Enabled = true;
                }
                else
                {
                    downloadProductFile.Tag = null;
                    downloadProductFile.Enabled = false;
                }
                WebServiceInvoker.GetOrderInfo(_envelope.OrderId,
                    orderInfo => { customerName.Text = orderInfo.TaobaoId; },
                    message => { XtraMessageBox.Show(message); });
                paperName.Text = _envelope.PaperName;
                productWidth.Text = _envelope.ProductSize.Width.ToString();
                productHeight.Text = _envelope.ProductSize.Height.ToString();
            });
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (_envelope == null) return;
            var saveFileDialog = new SaveFileDialog
            {
                FileName = customerName.Text + "_" + _envelope.PaperName + "_正面" + _envelope.FrontStyle + "_反面" +
                           _envelope.BackStyle.Name,
                Filter = @"PDF文件|*.pdf",
                OverwritePrompt = true,
                DefaultExt = "pdf",
            };


            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            WebServiceInvoker.GetEnvelopeInfoById(_envelopeId, tmpresult =>
            {
                var result = tmpresult.TranslateToEnvelope();
                var productFileId = result.ProductFileId;
                if (string.IsNullOrEmpty(productFileId))
                {
                    XtraMessageBox.Show("此集合没有成品，请稍后重试");
                    return;
                }
                if (File.Exists(saveFileDialog.FileName))
                {
                    try
                    {
                        File.Delete(saveFileDialog.FileName);

                    }
                    catch
                    {
                        XtraMessageBox.Show("文件已经被占用，请关闭后重新下载!");
                        return;
                    }
                }
                FileInfo fileInfo= new FileInfo(saveFileDialog.FileName);

                WebServiceInvoker.DownLoadFileByFileId(productFileId, fileInfo, success:downloadFileInfo =>
                {                       
                    if (XtraMessageBox.Show("文件下载完成，是否打开文件", "下载完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;
                    Process.Start("explorer.exe", downloadFileInfo.FullName);
                },process:proce=> {
                    progressBarControl1.EditValue = proce;                    
                });
            });
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (_envelope != null)
            {
                EnvelopeId = _envelope.Id;
            }
        }
    }
}