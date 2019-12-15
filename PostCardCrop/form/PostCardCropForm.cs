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
using soho.constant.postcard;
using soho.helper;
using soho.web;
using CropInfo = PhotoCropper.viewModel.CropInfo;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Size = System.Windows.Size;


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
            if (!(elementHost1.Child is Photocroper photocroper)) return;

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

        private readonly List<PostCardInfo> _postCardNeedUpdateProductInfos=new List<PostCardInfo>();

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
                    _postCardNeedUpdateProductInfos.Add(postCardInfo);
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
                    postCard.FrontStyle = postCardInfo.FrontStyle;
                    postCard.ProcessStatusText = postCardInfo.ProcessStatusText;
                    PostCardChanged();
                }, message => { XtraMessageBox.Show(message); });
            }
        }

        private EnvelopeInfo _currentEnvelopeInfo;

        private void EnvelopeView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            _currentEnvelopeInfo = EnvelopeView.GetFocusedRow() as EnvelopeInfo;
            if (_currentEnvelopeInfo != null)
            {
                postCardControl.DataSource = _currentEnvelopeInfo.PostCards;
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
                WebServiceInvoker.GetInstance().SubmitPostCardProductFile(postCardInfo.PostCardId, "", () =>
                {
                    postCardInfo.ProductFileId = "";
                    PostCardChanged();
                });
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            _postCardNeedUpdateProductInfos.ForEach(postCardInfo =>
            {
                if (string.IsNullOrEmpty(postCardInfo.ProductFileId))
                {
                    WebServiceInvoker.GetInstance().GetPostCardInfo(postCardInfo.PostCardId, result =>
                    {
                        if (string.IsNullOrEmpty(result.ProductFileId))
                        {
                            return;
                        }
                        postCardInfo.ProductFileId = result.ProductFileId;
                    });
                }
            });
        }

        private void BarButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}