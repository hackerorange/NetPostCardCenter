using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using soho.domain;
using soho.webservice;

namespace PostCardCenter.myController
{
    public partial class EnvelopeInfoController : UserControl
    {
        private Envelope _envelope;

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
            EnvelopeInvoker.GetEnvelopeInfoById(_envelopeId, result =>
            {
                _envelope = result;
                if (!string.IsNullOrEmpty(_envelope.productFileId))
                {
                    downloadProductFile.Tag = _envelope.productFileId;
                    downloadProductFile.Enabled = true;
                }
                else
                {
                    downloadProductFile.Tag = null;
                    downloadProductFile.Enabled = false;
                }
                OrderCenterInvoker.GetOrderInfo(_envelope.orderId,
                    orderInfo => { customerName.Text = orderInfo.taobaoId; },
                    message => { XtraMessageBox.Show(message); });
                paperName.Text = _envelope.paperName;
                productWidth.Text = _envelope.productWidth.ToString();
                productHeight.Text = _envelope.productHeight.ToString();
            });
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (_envelope == null) return;
            var saveFileDialog = new SaveFileDialog
            {
                FileName = customerName.Text + "_" + _envelope.paperName + "_正面" + _envelope.frontStyle + "_反面" +
                           _envelope.backStyle,
                Filter = @"PDF文件|*.pdf",
                OverwritePrompt = true,
                DefaultExt = "pdf",
            };


            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            EnvelopeInvoker.GetEnvelopeInfoById(_envelopeId, result =>
            {
                var productFileId = result.productFileId;

                if (string.IsNullOrEmpty(productFileId))
                {
                    XtraMessageBox.Show("此集合没有成品，请稍后重试");
                    return;
                }
                SohoInvoker.DownLoadFile(productFileId, true, fileInfo =>
                {
                    if (File.Exists(saveFileDialog.FileName))
                    {
                        File.Delete(saveFileDialog.FileName);
                    }
                    fileInfo.MoveTo(saveFileDialog.FileName);
                    if (XtraMessageBox.Show("文件下载完成，是否打开文件", "下载完成", MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question) != DialogResult.OK) return;
                    var p = new Process
                    {
                        StartInfo =
                        {
                            FileName = "explorer.exe",
                            Arguments = fileInfo.FullName
                        }
                    };
                    p.Start();
                });
            });
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (_envelope != null)
            {
                EnvelopeId = _envelope.envelopeId;
            }
        }
    }
}