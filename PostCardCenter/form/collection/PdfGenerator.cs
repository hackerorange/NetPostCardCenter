using DevExpress.XtraEditors;
using Hacker.Inko.Net.Api;
using System;
using System.Collections.Generic;
using System.IO;

namespace PostCardCenter.form.collection
{
    public partial class PdfGenerator : DevExpress.XtraEditors.XtraForm
    {
        private List<FileInfo> _fileInfoList = new List<FileInfo>();

        private string _orderId;

        public PdfGenerator(string orderId)
        {
            this._orderId = orderId;
            InitializeComponent();
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_orderId))
            {
                XtraMessageBox.Show("订单为空，无法生成订单PDF");
            }

            PostCardCollectionApi.GetAllEnvelopeByOrderId(_orderId, result =>
            {
                // 处理每一个集合
                foreach (var envelopeResponse in result)
                {
                    //_fileInfoList.Add(envelopeResponse.GeneratePdfByteArray(message =>
                    //{
                    //    XtraMessageBox.Show(message);
                    //}));
                }
            }, failure: message => { XtraMessageBox.Show(message); });
        }
    }
}