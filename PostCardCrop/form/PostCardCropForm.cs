using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraLayout.Utils;
using postCardCenterSdk.request.postCard;
using postCardCenterSdk.sdk;
using PhotoCropper.controller;
using PostCardCrop.model;
using PostCardCrop.translator.response;
using PostCardProcessor;
using PostCardProcessor.model;
using PostCardProcessor.queue;
using postCardCenterSdk.constant.postcard;
using postCardCenterSdk.helper;
using postCardCenterSdk.web;
using CropInfo = PhotoCropper.viewModel.CropInfo;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Size = System.Windows.Size;
using DevExpress.XtraEditors;
using Hacker.Inko.PostCard.Support;

namespace PostCardCrop.form
{
    public partial class PostCardCropForm : RibbonForm
    {
        private readonly string _orderId;
        //明信片集合
        public PostCardCropForm(string focusedRowOrderId)
        {
            InitializeComponent();
            if (elementHost1.Child is Photocroper photocroper)
            {
                photocroper.KeyDown += Photocroper_KeyDown;
                photocroper.KeyUp += Photocroper_KeyUp;
            }

            _orderId = focusedRowOrderId;
        }

        private void Photocroper_KeyUp(object sender, KeyEventArgs e)
        {
            if (elementHost1.Child is Photocroper photocroper)
            {
                //提交
                switch (e.Key)
                {
                    case Key.Enter:
                        SubmitPostCard(PostCardView.FocusedRowHandle, photocroper.CropInfo);
                        //photocroper.FastChange = true;
                        break;
                    case Key.LeftCtrl:
                    case Key.RightCtrl:
                        photocroper.FastChange = true;
                        break;
                    case Key.LeftShift:
                    case Key.RightShift:
                        photocroper.SizeLimit = true;
                        break;
                    case Key.Space:
                        photocroper.Preview = false;
                        break;
                }
            }
        }


        private void SubmitPostCard(int rowHandler, CropInfo cropInfo)
        {
            if (_currentEnvelopeInfo == null)
            {
                return;
            }

            if (PostCardView.GetRow(rowHandler) is PostCardInfo postCardInfo)
            {
                postCardInfo.ProcessStatusText = "裁切中";
                PostCardView.RefreshRow(rowHandler);
                var postCardProcessInfo = new PostCardProcessInfo
                {
                    PostCardId = postCardInfo.PostCardId,
                    CropLeft = cropInfo.CropLeft,
                    CropTop = cropInfo.CropTop,
                    CropHeight = cropInfo.CropHeight,
                    CropWidth = cropInfo.CropWidth,
                    PostCardType = postCardInfo.FrontStyle,
                    Rotation = cropInfo.Rotation,
                    ProductHeight = postCardInfo.ProductSize.Height,
                    ProductWidth = postCardInfo.ProductSize.Width
                };
                PostCardProcessQueue.Process(postCardProcessInfo, ((info) =>
                {
                    postCardInfo.ProcessStatusText = "已提交";
                    PostCardView.RefreshRow(rowHandler);
                }), failure: ((info) =>
                {
                    postCardInfo.ProcessStatusText = "提交失败";
                    PostCardView.RefreshRow(rowHandler);
                }));
            }

            if (PostCardView.DataSource is List<PostCardInfo> postCards)
            {
                var flag = true;
                for (var index = rowHandler; index < postCards.Count; index++)
                {
                    var postCard = postCards[index];
                    if (postCard.ProcessStatusText == "未提交")
                    {
                        PostCardView.FocusedRowHandle = index;
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    if (rowHandler < postCards.Count)
                    {
                        PostCardView.FocusedRowHandle = rowHandler + 1;
                    }
                    else
                    {
                        PostCardView.FocusedRowHandle = 0;
                    }
                }
            }
        }


        private void Photocroper_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(elementHost1.Child is Photocroper photocroper)) return;

            //提交
            switch (e.Key)
            {
                case Key.Enter:
                    //photocroper.FastChange = true;
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    photocroper.FastChange = false;
                    break;
                case Key.LeftShift:
                case Key.RightShift:
                    photocroper.SizeLimit = false;
                    break;
                case Key.Space:
                    photocroper.Preview = true;
                    break;
                case Key.L:
                    photocroper.LeftRotate();
                    break;
                case Key.R:
                    photocroper.RightRotate();
                    break;
            }
        }

        private void PostCardCropForm_Load(object sender, EventArgs e)
        {
            WebServiceInvoker.GetInstance().GetAllEnvelopeByOrderId(_orderId, envelopeList =>
            {
                var envelopeInfos = new List<EnvelopeInfo>();

                envelopeList.ForEach(tmpEnvelope =>
                {
                    var envelope = tmpEnvelope.TranslateToEnvelope();
                    envelopeInfos.Add(envelope);
                    if (envelopeInfos.Count == 1)
                    {
                        postCardControl.DataSource = envelope.PostCards;
                    }

                    WebServiceInvoker.GetInstance().GetPostCardByEnvelopeId(envelope.Id, postCards =>
                    {
                        if (postCards == null) return;
                        //遍历明信片，对明信片进行Node绑定
                        foreach (var postCard1 in postCards)
                        {
                            var tmpPostCard = postCard1.TranlateToPostCard();
                            envelope.PostCards.Add(tmpPostCard);
                            tmpPostCard.ProductSize = envelope.ProductSize;
                        }

                        postCardControl.RefreshDataSource();
                    }, message => { XtraMessageBox.Show(message); });
                });
                envelopeListControl.DataSource = envelopeInfos;
            }, failure: kk =>
            {
                XtraMessageBox.Show(kk);
            });
        }

        private void PostCardCropForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Space)
            //{
            //    if (elementHost1.Child is Photocroper photocroper)
            //    {
            //        photocroper.Preview = true;
            //    }
            //}
        }

        private void PostCardCropForm_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Space)
            //{
            //    if (elementHost1.Child is Photocroper photocroper) photocroper.Preview = false;
            //}
        }


        private void RibbonControl1_Click(object sender, EventArgs e)
        {
        }


        private void BarButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (elementHost1.Child is Photocroper photocroper) photocroper.LeftRotate();
        }

        private void BarButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (elementHost1.Child is Photocroper photocroper) photocroper.RightRotate();
        }

        private void BarButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (elementHost1.Child is Photocroper photocroper)
            {
                photocroper.LeftRotate();
                photocroper.LeftRotate();
            }
        }

        /// <summary>
        /// 修改明信片正面版式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (PostCardView.GetFocusedRow() is PostCardInfo postCard)
            {
                WebServiceInvoker.GetInstance().ChangePostCardFrontStyle(postCard.PostCardId, e.Item.Tag as string, resp =>
                {
                    var postCardInfo = resp.TranlateToPostCard();
                    postCard.ProcessStatus = postCardInfo.ProcessStatus;
                    postCard.ProductFileId = postCardInfo.ProductFileId;
                    postCard.FrontStyle = postCardInfo.FrontStyle;
                    postCard.ProcessStatusText = postCardInfo.ProcessStatusText;
                    PostCardChanged();
                }, message => { XtraMessageBox.Show(message); });
            }
        }

        private EnvelopeInfo _currentEnvelopeInfo;

        private void EnvelopeView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

            if (EnvelopeView.GetFocusedRow() is EnvelopeInfo _currentEnvelopeInfo)
            {
                postCardControl.DataSource = _currentEnvelopeInfo.PostCards;
                this._currentEnvelopeInfo = _currentEnvelopeInfo;
            }
        }

        private void PostCardView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            PostCardChanged();
        }

        private void PostCardChanged()
        {
            if (PostCardView.GetFocusedRow() is PostCardInfo postCardInfo)
            {


                progressBarControl1.EditValue = 0;
                //                cropContext.CropInfoSubmitDto = new CropInfoSubmitDto(cropContext.Image.Size, cropContext.PicturePrintAreaSize, fit: cropContext.StyleInfo.Fit);
                //                        pictureCropControl1.CropContext = cropContext;

                if (elementHost1.Child is Photocroper photocroper)
                {
                    photocroper.ProductSize = new Size(postCardInfo.ProductSize.Width, postCardInfo.ProductSize.Height);
                    photocroper.FrontStyle = postCardInfo.FrontStyle;
                    if (string.IsNullOrEmpty(postCardInfo.ProductFileId))
                    {
                        photocroper.Preview = false;

                        //FileApi.GetInstance().
                        photocroper.InitImage(FileApi.GetInstance().BasePath + "/file/" + postCardInfo.FileId, null, (stream) =>
                        {
                            if (postCardInfo.ProcessStatusText == "未提交" && postCardInfo.FrontStyle == "D")
                            {
                                timer1.Start();
                            }
                        });
                    }
                    else
                    {
                        //FileApi.GetInstance().
                        photocroper.Preview = true;
                        photocroper.FrontStyle = "B";
                        photocroper.InitImage(FileApi.GetInstance().BasePath + "/file/" + postCardInfo.ProductFileId);
                    }
                }
            }
        }

        private void BarToggleSwitchItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (elementHost1.Child is Photocroper photocroper) photocroper.Preview = barToggleSwitchItem1.BindableChecked;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (elementHost1.Child is Photocroper photocroper)
            {
                var aaa = new CropInfo
                {
                    Rotation = photocroper.CropInfo.Rotation,
                    CropTop = 0,
                    CropLeft = 0,
                    CropHeight = 1,
                    CropWidth = 1
                };
                SubmitPostCard(PostCardView.FocusedRowHandle, aaa);
            }
        }

        private void BarButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (PostCardView.GetFocusedRow() is PostCardInfo postCardInfo)
            {
                WebServiceInvoker.GetInstance().SubmitPostCardProductFile(postCardInfo.PostCardId, "", (boolean) =>
                {
                    postCardInfo.ProductFileId = "";
                    PostCardChanged();
                });
            }
        }

        /// <summary>
        /// 生成PDF按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_currentEnvelopeInfo == null)
            {
                return;
            }
            var envelopeId = _currentEnvelopeInfo.Id;

            WebServiceInvoker.GetInstance().GetOrderInfo(_currentEnvelopeInfo.OrderId, orderInfo =>
            {
                WebServiceInvoker.GetInstance().GetEnvelopeInfoById(envelopeId, envelopeInfo =>
                {

                    var dialog = new XtraSaveFileDialog
                    {
                        DefaultExt = "pdf",
                        FileName = orderInfo.TaobaoId + "_" + envelopeInfo.PaperName + ".pdf",
                        InitialDirectory = "D://"
                    };
                    var dialogResult = dialog.ShowDialog();

                    if (dialogResult == DialogResult.OK)
                    {
                        var exportForm = new ExportForm();
                        exportForm.Show(this);

                        envelopeInfo.GeneratePdfFile(dialog.FileName, processValue =>
                        {
                            exportForm.RefreshProgress(processValue);
                            Thread.Sleep(100);
                            Application.DoEvents();
                        }, message =>
                         {
                             XtraMessageBox.Show(message);
                         });
                        exportForm.Close();
                        exportForm.Dispose();
                        var result = XtraMessageBox.Show("导出完成，是否定位到当前文件?", "导出完成", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("Explorer.exe", "/select," + dialog.FileName);
                        }
                    }
                }, message =>
                {
                    XtraMessageBox.Show(message);
                });


            }, message => XtraMessageBox.Show(message));




        }
    }
}