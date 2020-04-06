using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Properties;
using PhotoCropper.controller;
using PostCardCrop.model;
using PostCardCrop.translator.response;
using PostCardProcessor.model;
using PostCardQueueProcessor.queue;
using CropInfo = PhotoCropper.viewModel.CropInfo;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Size = System.Windows.Size;

namespace PostCardCrop.form
{
    public partial class PostCardCropForm : RibbonForm
    {
        private readonly string _orderId;

        private EnvelopeInfo _currentEnvelopeInfo;

        private DoubleSidePostCardCropInfo _doubleSidePostCardCropInfo;

        //明信片集合
        public PostCardCropForm(string focusedRowOrderId)
        {
            InitializeComponent();
            if (elementHost1.Child is Photocroper photoCropper)
            {
                photoCropper.KeyDown += Photocroper_KeyDown;
                photoCropper.KeyUp += Photocroper_KeyUp;
            }

            postCardControl.KeyUp += PostCardControlKeyUp;

            _orderId = focusedRowOrderId;
        }

        private void PostCardControlKeyUp(object sender, KeyEventArgs e)
        {
            if (!(PostCardView.GetFocusedRow() is PostCardInfo postCard)) return;

            switch (e.KeyCode)
            {
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                    ChangeFrontStyle(e.KeyCode.ToString());
                    break;
                case Keys.Escape:
                    ReCropPostCard(postCard);
                    break;
            }
        }

        private void ChangeFrontStyle(string style)
        {
            if (!(PostCardView.GetFocusedRow() is PostCardInfo postCard)) return;

            PostCardItemApi.ChangePostCardFrontStyle(postCard.PostCardId, style, resp =>
            {
                var postCardInfo = resp.TranlateToPostCard();
                // postCard.ProcessStatus = postCardInfo.ProcessStatus;
                postCard.ProductFileId = postCardInfo.ProductFileId;
                postCard.FrontStyle = postCardInfo.FrontStyle;
                postCard.ProcessStatusText = postCardInfo.ProcessStatusText;
                PostCardChanged();
            }, message => { XtraMessageBox.Show(message); });
        }

        private void Photocroper_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!(PostCardView.GetFocusedRow() is PostCardInfo postCard)) return;

            if (!(elementHost1.Child is Photocroper photoCroper)) return;

            //提交
            switch (e.Key)
            {
                case Key.Enter:
                    if (!photoCroper.Preview)
                        SubmitPostCard(PostCardView.FocusedRowHandle, photoCroper.CropInfo);
                    else
                        MoveToNextPosition(PostCardView.FocusedRowHandle);

                    //photocroper.FastChange = true;
                    break;
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    photoCroper.FastChange = true;
                    break;
                case Key.LeftShift:
                case Key.RightShift:
                    photoCroper.SizeLimit = true;
                    break;
                case Key.Space:
                    photoCroper.Preview = false;
                    break;
                case Key.A:
                case Key.B:
                case Key.C:
                case Key.D:
                    ChangeFrontStyle(e.Key.ToString());
                    break;
                case Key.Escape: // 重新裁切
                    ReCropPostCard(postCard);
                    break;
            }
        }


        private void SubmitPostCard(int rowHandler, CropInfo cropInfo)
        {
            if (_currentEnvelopeInfo == null) return;

            if (PostCardView.GetRow(rowHandler) is PostCardInfo postCardInfo)
            {
                // 如果当前提交doubleSide提交为null，创建一个(第一次提交为 null
                if (_doubleSidePostCardCropInfo == null)
                {
                    _doubleSidePostCardCropInfo = new DoubleSidePostCardCropInfo
                    {
                        PostCardId = postCardInfo.PostCardId, // ID，用于提交数据
                        ProductHeight = postCardInfo.ProductSize.Height, // 成品高度，用于确定成品尺寸
                        ProductWidth = postCardInfo.ProductSize.Width, // 成品宽度，用于确定成品尺寸
                        PostCardType = postCardInfo.FrontStyle, // 正面样式，A B C D，用于确定板式
                        FrontCropCropInfo = new PostCardProcessCropInfo // 正面裁切信息
                        {
                            FileId = postCardInfo.FileId,
                            CropLeft = cropInfo.CropLeft,
                            CropTop = cropInfo.CropTop,
                            CropHeight = cropInfo.CropHeight,
                            CropWidth = cropInfo.CropWidth,
                            Rotation = cropInfo.Rotation,
                            ImageWidth = cropInfo.ImageWidth,
                            ImageHeight = cropInfo.ImageHeight
                        }
                    };
                    // 如果是双面，裁切双面
                    if (!string.IsNullOrEmpty(postCardInfo.BackFileId))
                        // 肯定是的，设置为反面
                        if (elementHost1.Child is Photocroper photoCropper)
                        {
                            photoCropper.ProductSize = new Size(postCardInfo.ProductSize.Width, postCardInfo.ProductSize.Height);
                            photoCropper.FrontStyle = "B";
                            photoCropper.Preview = false;
                            photoCropper.InitImage(Settings.Default.Host + "/file/" + postCardInfo.BackFileId, action: tempCropInfo =>
                            {
                                if (Math.Abs(tempCropInfo.CropWidth - 1) <= 0.001 && Math.Abs(tempCropInfo.CropHeight - 1) <= 0.001)
                                {
                                    tempCropInfo.CropHeight = 1;
                                    tempCropInfo.CropWidth = 1;
                                    tempCropInfo.CropTop = 0;
                                    tempCropInfo.CropLeft = 0;
                                    timer1.Interval = 100;
                                    //开始定时器，自动提交反面
                                    timer1.Start();
                                    // 提交
                                }
                            });
                            return;
                        }
                }
                else
                {
                    // 如果有反面文件ID,提交反面数据，否则不提交
                    if (!string.IsNullOrEmpty(postCardInfo.BackFileId))
                        _doubleSidePostCardCropInfo.BackCropCropInfo = new PostCardProcessCropInfo
                        {
                            FileId = postCardInfo.BackFileId,
                            CropLeft = cropInfo.CropLeft, // 反面左边
                            CropTop = cropInfo.CropTop, // 反面右边
                            CropHeight = cropInfo.CropHeight, // 反面高度
                            CropWidth = cropInfo.CropWidth, // 反面宽度
                            Rotation = cropInfo.Rotation, // 反面旋转角度
                            ImageWidth = cropInfo.ImageWidth, // 原始图像宽度
                            ImageHeight = cropInfo.ImageHeight // 原始图像高度
                        };
                }

                postCardInfo.ProcessStatusText = "已提交";

                PostCardProcessQueue.Process(_doubleSidePostCardCropInfo,
                    info => { PostCardItemApi.UpdatePostCardProcessStatus(postCardInfo.PostCardId, "AFTER_SUBMIT"); },
                    info => { PostCardItemApi.UpdatePostCardProcessStatus(postCardInfo.PostCardId, "SUBMIT_FAILURE"); });
            }

            MoveToNextPosition(rowHandler);
        }

        private void MoveToNextPosition(int startPosition)
        {
            if (PostCardView.DataSource is List<PostCardInfo> postCards)
            {
                var postCardInfo = postCards.Find(k => k.ProcessStatusText == "未提交");

                if (postCardInfo == null)
                {
                    XtraMessageBox.Show("当前明信片集合已经提交完毕，请等待处理");
                    return;
                }

                for (var index = startPosition + 1; index < postCards.Count; index++)
                {
                    var postCard = postCards[index];
                    if (postCard.ProcessStatusText == "未提交")
                    {
                        PostCardView.FocusedRowHandle = index;
                        return;
                    }
                }

                // 后面没有未处理的明信片，从头开始找起！
                if (startPosition != 0) MoveToNextPosition(0);
            }
        }


        private void Photocroper_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
            InitData(_orderId);
            timer2.Start();
        }

        private void InitData(string orderId)
        {
            PostCardCollectionApi.GetAllEnvelopeByOrderId(orderId, envelopeList =>
            {
                var envelopeInfos = new List<EnvelopeInfo>();

                envelopeList.ForEach(tmpEnvelope =>
                {
                    var envelope = tmpEnvelope.TranslateToEnvelope();
                    envelopeInfos.Add(envelope);
                    if (envelopeInfos.Count == 1) postCardControl.DataSource = envelope.PostCards;

                    PostCardItemApi.GetPostCardByEnvelopeId(envelope.Id, postCards =>
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
            }, kk => { XtraMessageBox.Show(kk); });
        }


        private void PostCardCropForm_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Space)
            //{
            //    if (elementHost1.Child is Photocroper photocroper)
            //    {
            //        photocroper.Preview = true;
            //    }
            //}
        }

        private void PostCardCropForm_KeyUp(object sender, KeyEventArgs e)
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
        ///     修改明信片正面版式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (PostCardView.GetFocusedRow() is PostCardInfo postCard)
                PostCardItemApi.ChangePostCardFrontStyle(postCard.PostCardId, e.Item.Tag as string, resp =>
                {
                    var postCardInfo = resp.TranlateToPostCard();
                    // postCard.ProcessStatus = postCardInfo.ProcessStatus;
                    postCard.ProductFileId = postCardInfo.ProductFileId;
                    postCard.FrontStyle = postCardInfo.FrontStyle;
                    postCard.ProcessStatusText = postCardInfo.ProcessStatusText;
                    PostCardChanged();
                }, message => { XtraMessageBox.Show(message); });
        }

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
                // 还原当前选中双面裁切信息
                _doubleSidePostCardCropInfo = null;
                progressBarControl1.EditValue = 0;
                //                cropContext.CropInfoSubmitDto = new CropInfoSubmitDto(cropContext.Image.Size, cropContext.PicturePrintAreaSize, fit: cropContext.StyleInfo.Fit);
                //                        pictureCropControl1.CropContext = cropContext;

                if (elementHost1.Child is Photocroper photoCropper)
                {
                    photoCropper.ProductSize = new Size(postCardInfo.ProductSize.Width, postCardInfo.ProductSize.Height);
                    photoCropper.FrontStyle = postCardInfo.FrontStyle;
                    if (string.IsNullOrEmpty(postCardInfo.ProductFileId))
                    {
                        photoCropper.Preview = false;

                        //FileApi.GetInstance().
                        photoCropper.InitImage(Settings.Default.Host + "/file/" + postCardInfo.FileId, tempCropInfo =>
                        {
                            switch (postCardInfo.FrontStyle)
                            {
                                // D版，直接缩放
                                case "D":
                                    tempCropInfo.CropHeight = 1;
                                    tempCropInfo.CropWidth = 1;
                                    tempCropInfo.CropTop = 0;
                                    tempCropInfo.CropLeft = 0;
                                    if (postCardInfo.ProcessStatusText == "未提交")
                                    {
                                        timer1.Interval = 1000;
                                        timer1.Start();
                                    }

                                    break;
                                // C版，向左旋转
                                case "C":
                                    // 向左旋转
                                    photoCropper.LeftRotate();
                                    // 最大化
                                    photoCropper.FixMax();
                                    break;
                                default:
                                    // A版、B版：判断是否自动旋转
                                    if (barCheckItem1.Checked &&
                                        postCardInfo.ProcessStatusText == "未提交" &&
                                        Math.Abs(tempCropInfo.CropWidth - 1) <= 0.001 &&
                                        Math.Abs(tempCropInfo.CropHeight - 1) <= 0.001)
                                    {
                                        tempCropInfo.CropHeight = 1;
                                        tempCropInfo.CropWidth = 1;
                                        tempCropInfo.CropTop = 0;
                                        tempCropInfo.CropLeft = 0;
                                        timer1.Interval = 10;
                                        //开始定时器，自动提交反面
                                        timer1.Start();
                                        // 提交
                                    }

                                    break;
                            }
                        });
                    }
                    else
                    {
                        //FileApi.GetInstance().
                        photoCropper.Preview = true;
                        photoCropper.FrontStyle = "B";
                        photoCropper.InitImage(Settings.Default.Host + "/file/" + postCardInfo.ProductFileId);
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
                SubmitPostCard(PostCardView.FocusedRowHandle, photocroper.CropInfo);
            }
        }

        private void BarButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(PostCardView.GetFocusedRow() is PostCardInfo postCard)) return;

            ReCropPostCard(postCard);
        }

        private void ReCropPostCard(PostCardInfo postCardInfo)
        {
            PostCardItemApi.ReCropPostCard(
                postCardInfo.PostCardId,
                response =>
                {
                    postCardInfo.ProductFileId = response.ProductFileId;
                    postCardInfo.BackProductFileId = response.BackProductFileId;
                    postCardInfo.ProcessStatusText = response.ProcessStatusText;
                    PostCardChanged();
                });
        }

        /// <summary>
        ///     生成PDF按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_currentEnvelopeInfo == null) return;

            if (PostCardView.DataSource is List<PostCardInfo> postCardInfos)
                if (postCardInfos.Any(postCardInfo => postCardInfo.ProcessStatusText != "处理完成"))
                {
                    XtraMessageBox.Show("存在没有处理完的明信片，无法生成PDF");
                    return;
                }

            var envelopeId = _currentEnvelopeInfo.Id;

            new ExportForm(envelopeId).ShowDialog(this);
        }

        private void BarButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            InitData(_orderId);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (!(EnvelopeView.GetFocusedRow() is EnvelopeInfo envelopeInfo)) return;

            if (!(postCardControl.DataSource is List<PostCardInfo> postCardResponses)) return;

            // 如果所有的都处理完成了
            if (postCardResponses.All(k => k.ProcessStatusText == "处理完成"))
            {
                barButtonItem2.Enabled = true;
                return;
            }

            barButtonItem2.Enabled = false;

            PostCardItemApi.GetPostCardByEnvelopeId(envelopeInfo.Id,
                result =>
                {
                    var dictionary = result.ToDictionary(postCardResponse => postCardResponse.Id);

                    foreach (var postCardResponse in postCardResponses)
                    {
                        if (!dictionary.ContainsKey(postCardResponse.PostCardId)) continue;

                        var cardResponse = dictionary[postCardResponse.PostCardId];
                        // 处理状态
                        postCardResponse.ProcessStatusText = cardResponse.ProcessStatusText;
                        // 成品文件ID
                        postCardResponse.ProductFileId = cardResponse.ProductFileId;
                        // 反面成品文件ID
                        postCardResponse.BackProductFileId = cardResponse.BackProductFileId;
                    }

                    postCardControl.RefreshDataSource();
                }
            );
        }

        private void PostCardCropForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer2.Stop();
        }

        private void BarButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            PostCardBillApi.ChangeOrderStatus(
                _orderId,
                "Finished",
                result =>
                {
                    var dialogResult = XtraMessageBox.Show("状态修改成功,是否关闭当前窗口？", "订单完成", MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK) Close();
                },
                message => { XtraMessageBox.Show("订单状态修改失败"); }
            );
        }
    }
}