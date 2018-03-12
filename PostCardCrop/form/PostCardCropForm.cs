using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using postCardCenterSdk.request.postCard;
using postCardCenterSdk.sdk;
using PhotoCropper.controller;
using PostCardCrop.model;
using PostCardCrop.translator.response;
using soho.constant.postcard;
using CropInfo = PhotoCropper.viewModel.CropInfo;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace PostCardCrop.form
{
    public partial class PostCardCropForm : RibbonForm
    {
        private readonly string _orderId;

        public bool NeedRefresh { get; set; } = false;

        //明信片集合
        public PostCardCropForm(string focusedRowOrderId)
        {
            InitializeComponent();
            if (elementHost1.Child is Photocroper photocroper)
            {
                photocroper.KeyDown += Photocroper_KeyDown;
                photocroper.KeyUp += Photocroper_KeyUp;
            }

            elementHost1.KeyPress += PostCardCropForm_KeyPress;
            elementHost1.KeyDown += ElementHost1_KeyDown;
            //绑定鼠标滑轮事件
            //            cropControllerCrop.MouseWheel += cropControllerCrop.CanvasMouseWheel;
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
                    if (PostCardView.DataSource is List<PostCardInfo> postCards)
                    {
                        for (var index = PostCardView.FocusedRowHandle; index < postCards.Count; index++)
                        {
                            var postCard = postCards[index];
                            if (postCard.ProcessStatusText == "未提交")
                            {
                                PostCardView.FocusedRowHandle = index;
                                break;
                            }
                        }
                    }

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
                case Key.L:
                    photocroper.LeftRotate();
                    break;
                case Key.R:
                    photocroper.RightRotate();
                    break;
            }

            //提交
        }

        private void SubmitPostCard(int rowHandler, CropInfo cropInfo)
        {
            if (PostCardView.GetRow(rowHandler) is PostCardInfo postCardInfo)
            {
                postCardInfo.ProcessStatusText = "提交中";
                WebServiceInvoker.GetInstance().SubmitPostCardCropInfo(postCardInfo.PostCardId, cropInfo.CropLeft, cropInfo.CropTop, cropInfo.CropWidth, cropInfo.CropHeight, cropInfo.Rotation, resp =>
                {
                    postCardInfo.ProcessStatusText = "已提交";
                    postCardInfo.CropInfo = new model.CropInfo
                    {
                        LeftScale = cropInfo.CropLeft,
                        TopScale = cropInfo.CropTop,
                        WidthScale = cropInfo.CropWidth,
                        HeightScale = cropInfo.CropHeight,
                        Rotation = cropInfo.Rotation
                    };
                    PostCardView.RefreshRow(rowHandler);
                },  msg =>
                {
                    postCardInfo.ProcessStatusText = "提交失败";
                    PostCardView.RefreshRow(rowHandler);
                });
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
            }
        }

        private void ElementHost1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Console.WriteLine(@"按下了======");
            Console.WriteLine(e.KeyData);
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
            if (e.KeyCode == Keys.Space)
            {
                if (elementHost1.Child is Photocroper photocroper)
                {
                    photocroper.Preview = true;
                }
            }
        }

        private void PostCardCropForm_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (elementHost1.Child is Photocroper photocroper) photocroper.Preview = false;
            }
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
                postCard.FrontStyle = e.Item.Tag as string;

                var request = new PostCardInfoPatchRequest
                {
                    PostCardId = postCard.PostCardId,
                    FrontStyle = postCard.FrontStyle
                };

                WebServiceInvoker.GetInstance().ChangePostCardFrontStyle(request, resp =>
                {
                    postCard.CropInfo = null;
                    var postCardInfo = resp.TranlateToPostCard();
                    postCard.ProcessStatus = postCardInfo.ProcessStatus;
                    postCard.ProcessStatusText = postCardInfo.ProcessStatusText;
                }, message => { XtraMessageBox.Show(message); });
            }
        }


        private void EnvelopeView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (EnvelopeView.GetFocusedRow() is EnvelopeInfo envelope) postCardControl.DataSource = envelope.PostCards;
        }

        private void PostCardView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (PostCardView.GetFocusedRow() is PostCardInfo postCardInfo)
            {
                progressBarControl1.EditValue = 0;
//                cropContext.CropInfo = new CropInfo(cropContext.Image.Size, cropContext.PicturePrintAreaSize, fit: cropContext.StyleInfo.Fit);
                //                        pictureCropControl1.CropContext = cropContext;
                if (elementHost1.Child is Photocroper photocroper)
                {
                    photocroper.ProductSize = new Size(postCardInfo.ProductSize.Width, postCardInfo.ProductSize.Height);
                    photocroper.FrontStyle = postCardInfo.FrontStyle;

                    CropInfo cropInfo = null;
                    if (postCardInfo.CropInfo != null)
                    {
                        cropInfo = new CropInfo
                        {
                            CropTop = postCardInfo.CropInfo.TopScale,
                            CropLeft = postCardInfo.CropInfo.LeftScale,
                            CropWidth = postCardInfo.CropInfo.WidthScale,
                            CropHeight = postCardInfo.CropInfo.HeightScale,
                            Rotation = postCardInfo.CropInfo.Rotation,
                        };
                    }

                    photocroper.InitImage("http://127.0.0.1:8089/file/" + postCardInfo.ThumbnailFileId, cropInfo);
                }
            }
        }


        private void BarToggleSwitchItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (elementHost1.Child is Photocroper photocroper) photocroper.Preview = barToggleSwitchItem1.BindableChecked;
        }

        private void PostCardCropForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine(@"=========================================");
            Console.WriteLine(@"按下了按键");
            Console.WriteLine(@"=========================================");
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {
        }
    }
}