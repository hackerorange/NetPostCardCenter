using System;
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


        public string EnvelopeId
        {
            get { return _envelope != null ? _envelope.envelopeId : ""; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                EnvelopeInvoker.GetEnvelopeInfoById(value, result =>
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
            var tmp = sender as SimpleButton;
            if (tmp == null || tmp.Tag == null) return;
            SohoInvoker.downLoadFile(tmp.Tag.ToString(), true, fileInfo =>
            {
                if (File.Exists(saveFileDialog.FileName))
                {
                    File.Delete(saveFileDialog.FileName);
                }
                fileInfo.MoveTo(saveFileDialog.FileName);
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