using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using soho.domain;
using soho.test;
using soho.webservice;

namespace PostCardCenter.myController
{
    public partial class envelopeInfoController : UserControl
    {
        private Envelope _envelope;

        public envelopeInfoController()
        {
            InitializeComponent();
        }


        public string EnvelopeId
        {
            get { return _envelope != null ? _envelope.envelopeId : ""; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                _envelope = EnvelopeInvoker.GetEnvelopeInfoById(value);
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
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = "成品文件.pdf"
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            var tmp = sender as SimpleButton;
            if (tmp == null || tmp.Tag == null) return;
            var downLoadFile = SohoInvoker.downLoadFile(tmp.Tag.ToString(), true);
            downLoadFile.MoveTo(saveFileDialog.FileName);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (_envelope != null)
            {
                EnvelopeId = _envelope.envelopeId;
                MyTest.Orange = Guid.NewGuid().ToString();
            }
        }
    }
}