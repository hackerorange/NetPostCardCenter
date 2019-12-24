﻿using System;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Hacker.Inko.PostCard.Library.Support;
using postCardCenterSdk.response.envelope;
using postCardCenterSdk.response.postCard;
using postCardCenterSdk.sdk;

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
            WebServiceInvoker.GetInstance().GetEnvelopeInfoById(_postCardCollectionId, envelopeResponse =>
            {
                textEditPaperName.Text = envelopeResponse.PaperName;
                textEditProductWidth.Text = envelopeResponse.ProductWidth.ToString();
                textEditProductHeight.Text = envelopeResponse.ProductHeight.ToString();
                _envelopeResponse = envelopeResponse;

                WebServiceInvoker.GetInstance().GetOrderInfo(_envelopeResponse.OrderId, orderResponse =>
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
            _envelopeResponse.GeneratePdfFile(dialog.FileName, processValue =>
            {
                RefreshProgress(processValue);
                Application.DoEvents();
            }, message => { XtraMessageBox.Show(message); });
            var result = XtraMessageBox.Show("导出完成，是否定位到当前文件?", "导出完成", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) System.Diagnostics.Process.Start("Explorer.exe", "/select," + dialog.FileName);
        }
    }
}