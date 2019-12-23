using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using postCardCenterSdk.sdk;
using postCardCenterSdk.response.envelope;
using System.IO;
using System.Threading;
using postCardCenterSdk.response.postCard;
using postCardCenterSdk.web;
using postCardCenterSdk.helper;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout;
using iText.IO.Image;
using iText.Kernel.Pdf.Xobject;
using Hacker.Inko.PostCard.Support;

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
            WebServiceInvoker.GetInstance().GetAllEnvelopeByOrderId(_orderId, result =>
             {
                 // 处理每一个集合
                 foreach (EnvelopeResponse envelopeResponse in result)
                 {
                     _fileInfoList.Add(envelopeResponse.GeneratePdfFile(message =>
                     {
                         XtraMessageBox.Show(message);
                     }));
                 }
             }, failure: message =>
              {
                  XtraMessageBox.Show(message);
              });
        }
    }
}

