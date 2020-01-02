using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Response.envelope;
using Hacker.Inko.Net.Response.postCard;
using Hacker.Inko.PostCard.Library.Support;

namespace PostCardCrop.form
{
    public partial class ExportForm : XtraForm
    {
        private readonly string _postCardCollectionId;
        private EnvelopeResponse _envelopeResponse;
        private OrderResponse _orderResponse;

        public ExportForm(string envelopeId)
        {
            _postCardCollectionId = envelopeId;
            InitializeComponent();
        }

        private void ExportForm_Load(object sender, EventArgs e)
        {
            PostCardCollectionApi.GetEnvelopeInfoById(_postCardCollectionId, envelopeResponse =>
            {
                textEditPaperName.Text = envelopeResponse.PaperName;
                textEditProductWidth.Text = envelopeResponse.ProductWidth.ToString();
                textEditProductHeight.Text = envelopeResponse.ProductHeight.ToString();
                _envelopeResponse = envelopeResponse;

                PostCardBillApi.GetOrderInfo(_envelopeResponse.OrderId, orderResponse =>
                {
                    textEditCustomerName.Text = orderResponse.TaobaoId;

                    textEditFinalMark.Text = textEditCustomerName.Text + @"-" + textEditPaperName.Text;
                    textEditFinalFileName.Text = textEditCustomerName.Text + @"_" + textEditPaperName.Text;
                    simpleButton1.Enabled = true;
                    _orderResponse = orderResponse;
                }, message => { XtraMessageBox.Show("获取明信片订单失败，请检查网络！"); });
            }, failure => { XtraMessageBox.Show("获取明信片集合失败，请检查网络！"); });
        }

        public void RefreshProgress(double progress)
        {
            progressBar1.Value = progressBar1.Minimum + (int) (progress * (progressBar1.Maximum - progressBar1.Minimum));
            progressBar1.Refresh();
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            var dialog = new XtraSaveFileDialog
            {
                DefaultExt = "pdf",
                FileName = textEditFinalFileName.Text,
                InitialDirectory = "D://"
            };
            // 显示文件保存对话框
            var dialogResult = dialog.ShowDialog();

            // 用户取消
            if (dialogResult != DialogResult.OK) return;

            // 生成PDF
            _envelopeResponse.GeneratePdfFile(dialog.FileName, _orderResponse.Urgent, textEditFinalMark.Text, processValue =>
            {
                RefreshProgress(processValue);
                Application.DoEvents();
            }, message => { XtraMessageBox.Show(message); });

            if (checkEdit1.Checked)
            {
                System.Diagnostics.Process.Start("Explorer.exe", "\"" + dialog.FileName + "\"");
            }

            DialogResult = DialogResult.OK;
        }
    }
}