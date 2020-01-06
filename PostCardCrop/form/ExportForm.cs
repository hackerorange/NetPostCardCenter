using System;
using System.Collections.Generic;
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
        internal class WaterMarkContext
        {
            public string UserName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public string PaperName { get; set; }
            public string FrontType { get; set; }
            public string BackType { get; set; }
            public bool DoubleSide { get; set; }

            public int Count { get; set; } = 0;


            public string GenerateWaterMarkString()
            {
                if (!DoubleSide)
                {
                    return UserName +
                           @"_" +
                           Width +
                           @"×" +
                           Height +
                           @"_" +
                           PaperName +
                           @"_正面[" + FrontType +
                           "]_单面打印，一共"
                           + Count +
                           @"张";
                }
                else
                {
                    return UserName
                           + @"_"
                           + Width +
                           @"×" +
                           Height +
                           PaperName +
                           @"_正面[" +
                           FrontType +
                           "]_反面[" +
                           BackType +
                           "]，一共" +
                           Count +
                           @"张";
                }
            }


            public string GenerateFileName()
            {
                return UserName + @"_" + PaperName + "_成品尺寸(" + Width + "×" + Height + ")";
            }
        }

        private readonly string _postCardCollectionId;
        private EnvelopeResponse _envelopeResponse;
        private OrderResponse _orderResponse;
        private List<PostCardResponse> _postCardResponses;

        public ExportForm(string envelopeId)
        {
            _postCardCollectionId = envelopeId;
            InitializeComponent();
        }

        private void ExportForm_Load(object sender, EventArgs e)
        {
            PostCardCollectionApi.GetEnvelopeInfoById(_postCardCollectionId, envelopeResponse =>
            {
                var waterMarkContext = new WaterMarkContext
                {
                    PaperName = envelopeResponse.PaperName, // 纸张名称
                    Width = envelopeResponse.ProductWidth, // 纸张宽度
                    Height = envelopeResponse.ProductHeight, // 纸张高度
                    BackType = envelopeResponse.BackStyle, // 反面样式
                    DoubleSide = envelopeResponse.DoubleSide // 双面还是单面
                };


                textEditPaperName.Text = envelopeResponse.PaperName;
                textEditProductWidth.Text = envelopeResponse.ProductWidth.ToString();
                textEditProductHeight.Text = envelopeResponse.ProductHeight.ToString();
                _envelopeResponse = envelopeResponse;


                PostCardBillApi.GetOrderInfo(_envelopeResponse.OrderId, orderResponse =>
                {
                    textEditCustomerName.Text = orderResponse.TaobaoId;
                    _orderResponse = orderResponse;
                    PostCardItemApi.GetPostCardByEnvelopeId(
                        _postCardCollectionId,
                        postCardResponseList =>
                        {
                            _postCardResponses = postCardResponseList;

                            waterMarkContext.UserName = orderResponse.TaobaoId; // 订单用户ID

                            for (var index = 0; index < _postCardResponses.Count; index++)
                            {
                                var postCardResponse = _postCardResponses[index];
                                // 第一张，默认取正面样式
                                if (index == 0)
                                {
                                    waterMarkContext.FrontType = postCardResponse.FrontStyle; // 正面样式
                                }

                                if (waterMarkContext.FrontType != postCardResponse.FrontStyle)
                                {
                                    waterMarkContext.FrontType = ""; // 多种正面样式
                                }

                                if (!string.IsNullOrEmpty(postCardResponse.BackStyle))
                                {
                                    waterMarkContext.BackType = postCardResponse.BackStyle; //反面样式
                                }

                                waterMarkContext.Count += postCardResponse.Copy; // 张数累加
                            }

                            textEditFinalMark.Text = waterMarkContext.GenerateWaterMarkString();
                            textEditFinalFileName.Text = waterMarkContext.GenerateFileName();
                            simpleButton1.Enabled = true;
                        },
                        postCardListFailMessage => { XtraMessageBox.Show("获取明信片列表发生错误，请检查网络！"); });
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