using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using Hacker.Inko.Net.Api;
using Hacker.Inko.Net.Api.Collection;
using Hacker.Inko.Net.Base;
using Hacker.Inko.Net.Request.postCard;
using Hacker.Inko.Net.Response.envelope;
using Hacker.Inko.Net.Response.postCard;
using PhotoCropper.controller;
using PostCardCrop.model;
using PostCardCrop.translator.response;
using PostCardProcessor.model;
using PostCardProcessor.queue;
using CropInfo = PhotoCropper.viewModel.CropInfo;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Size = System.Windows.Size;

namespace PostCardCrop.form
{
    public partial class PostCardCropForm : RibbonForm
    {
        private readonly string _orderId;

        private DoubleSidePostCardCropInfo _doubleSidePostCardCropInfo;

        static PostCardCropForm()
        {
            if (Process.GetCurrentProcess().MainModule is ProcessModule processModule)
            {
                var dictionary = new Dictionary<string, string>();
                var fileInfo = new FileInfo(processModule.FileName);
                fileInfo = new FileInfo(fileInfo.DirectoryName + "/inkoConfig.ini");
                if (fileInfo.Exists)
                {
                    var streamReader = new StreamReader(new FileStream(fileInfo.FullName, FileMode.Open));
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        if (line == null) continue;
                        var currentSplit = line.Split('=');
                        if (currentSplit.Length == 2)
                        {
                            dictionary.Add(currentSplit[0], currentSplit[1]);
                        }
                    }

                    streamReader.Close();

                    var queueName = dictionary["queueName"];
                    if (string.IsNullOrEmpty(queueName))
                    {
                        XtraMessageBox.Show("没有配置host，无法初始化网络请求");
                    }
                    else
                    {
                        PostCardProcessQueue.QueueName = queueName;
                    }

                    var providerUri = dictionary["providerUri"];
                    if (string.IsNullOrEmpty(providerUri))
                    {
                        XtraMessageBox.Show("没有配置providerUri，无法初始化消息队列");
                    }
                    else
                    {
                        PostCardProcessQueue.ProviderUri = providerUri;
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("没有找到配置文件");
            }
        }


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
                        }
                    };
                    // 如果是双面，裁切双面
                    if (!string.IsNullOrEmpty(postCardInfo.BackFileId))
                    {
                        // 肯定是的，设置为反面
                        if (elementHost1.Child is Photocroper photoCropper)
                        {
                            photoCropper.ProductSize = new Size(postCardInfo.ProductSize.Width, postCardInfo.ProductSize.Height);
                            photoCropper.FrontStyle = "B";
                            photoCropper.Preview = false;
                            photoCropper.InitImage(NetGlobalInfo.Host + "/file/" + postCardInfo.BackFileId, action: (stream, tempCropInfo) =>
                            {
                                if (Math.Abs(tempCropInfo.CropWidth - 1) <= 0.001 && Math.Abs(tempCropInfo.CropHeight - 1) <= 0.001)
                                {
                                    timer1.Interval = 100;
                                    //开始定时器，自动提交反面
                                    timer1.Start();
                                    // 提交
                                }
                            });
                            return;
                        }
                    }
                }
                else
                {
                    _doubleSidePostCardCropInfo.BackCropCropInfo = new PostCardProcessCropInfo
                    {
                        FileId = postCardInfo.BackFileId,
                        CropLeft = cropInfo.CropLeft, // 反面左边
                        CropTop = cropInfo.CropTop, // 反面右边
                        CropHeight = cropInfo.CropHeight, // 反面高度
                        CropWidth = cropInfo.CropWidth, // 反面宽度
                        Rotation = cropInfo.Rotation, // 反面旋转角度
                    };
                }


                PostCardProcessQueue.Process(_doubleSidePostCardCropInfo,
                    ((info) => { PostCardItemApi.UpdatePostCardProcessStatus(postCardInfo.PostCardId, "AFTER_SUBMIT"); }),
                    failure: ((info) => { PostCardItemApi.UpdatePostCardProcessStatus(postCardInfo.PostCardId, "SUBMIT_FAILURE"); }));
            }

            MoveToNextPosition(rowHandler);
        }

        private void MoveToNextPosition(int startPosition)
        {
            if (PostCardView.DataSource is List<PostCardInfo> postCards)
            {
                var postCardInfo = postCards.Find(k => { return k.ProcessStatusText == "未提交"; });

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
                if (startPosition != 0)
                {
                    MoveToNextPosition(0);
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
            InitData(_orderId);
            this.timer2.Start();
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
                    if (envelopeInfos.Count == 1)
                    {
                        postCardControl.DataSource = envelope.PostCards;
                    }

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
            }, failure: kk => { XtraMessageBox.Show(kk); });
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
                        photoCropper.InitImage(NetGlobalInfo.Host + "/file/" + postCardInfo.FileId, null, (stream, cropInfo) =>
                        {
                            if (postCardInfo.ProcessStatusText == "未提交" && postCardInfo.FrontStyle == "D")
                            {
                                timer1.Interval = 1000;
                                timer1.Start();
                            }

                            switch (postCardInfo.FrontStyle)
                            {
                                case "D":
                                    if (postCardInfo.ProcessStatusText == "未提交")
                                    {
                                        timer1.Interval = 1000;
                                        timer1.Start();
                                    }

                                    break;

                                case "C":
                                    photoCropper.LeftRotate();
                                    photoCropper.FixMin();
                                    break;
                            }
                        });
                    }
                    else
                    {
                        //FileApi.GetInstance().
                        photoCropper.Preview = true;
                        photoCropper.FrontStyle = "B";
                        photoCropper.InitImage(NetGlobalInfo.Host + "/file/" + postCardInfo.ProductFileId);
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
                PostCardItemApi.ReCropPostCard(
                    postCardInfo.PostCardId,
                    (response) =>
                    {
                        postCardInfo.ProductFileId = response.ProductFileId;
                        postCardInfo.BackProductFileId = response.BackProductFileId;
                        postCardInfo.ProcessStatusText = response.ProcessStatusText;
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

            new ExportForm(envelopeId).ShowDialog(this);
        }

        private void BarButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            InitData(_orderId);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (EnvelopeView.GetFocusedRow() is EnvelopeInfo envelopeInfo)
            {
                PostCardItemApi.GetPostCardByEnvelopeId(envelopeInfo.Id,
                    result =>
                    {
                        var dictionary = result.ToDictionary(postCardResponse => postCardResponse.Id);
                        if (postCardControl.DataSource is List<PostCardInfo> postCardResponses)
                        {
                            foreach (var postCardResponse in postCardResponses)
                            {
                                if (!dictionary.ContainsKey(postCardResponse.PostCardId))
                                {
                                    ////////continue;
                                }

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
                    }
                );
            }
        }

        private void PostCardCropForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            timer2.Stop();
        }
    }
}