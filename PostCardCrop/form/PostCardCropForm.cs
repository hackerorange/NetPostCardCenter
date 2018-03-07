using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using pictureCroper.model;
using postCardCenterSdk.request.postCard;
using postCardCenterSdk.sdk;
using PostCardCrop.model;
using PostCardCrop.translator.response;
using soho.constant.postcard;
using soho.constant.system;
using CropInfo = pictureCroper.model.CropInfo;

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
            //绑定鼠标滑轮事件
//            cropControllerCrop.MouseWheel += cropControllerCrop.CanvasMouseWheel;
            _orderId = focusedRowOrderId;
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

                        //明信片集合信息更新显示
                        if (envelopeInfoController1.EnvelopeInfo == null)
                        {
                            envelopeInfoController1.EnvelopeInfo = envelope;
                            envelopeInfoController1.RefreshEnvelopeInfo();
                        }
                    }, message => { XtraMessageBox.Show(message); });
                });

                envelopeListControl.DataSource = envelopeInfos;
                if (envelopeInfos.Count > 0)
                {
                    EnvelopeView.FocusedRowHandle = 0;
                    if (EnvelopeView.GetFocusedRow() is EnvelopeInfo tmpEnvelopeInfo)
                    {
                        postCardControl.DataSource = tmpEnvelopeInfo.PostCards;
                    }
                }
            });
        }

        /// <summary>
        ///     获取明信片缩略图
        /// </summary>
        //private void GeneratePostCardThumbnailImage(PostCardInfo tmpPostCard)
        //{
        //    var listNode = _postCardNodeMap[tmpPostCard.PostCardId];
        //    listNode.ImageIndex = listNode.SelectImageIndex = 1;
        //    listNode.SetValue("thumbnailDownloadStatus", "未下载");
        //    //var card = postCard;
        //    listNode.SetValue("ProcessStatusText", "请稍候");
        //    var thumbnailFileId = string.IsNullOrEmpty(tmpPostCard.ThumbnailFileId) ? tmpPostCard.FileId : tmpPostCard.ThumbnailFileId;
        //    //如果当前明信片没有缩略图文件
        //    if (string.IsNullOrEmpty(tmpPostCard.ThumbnailFileId))
        //    {
        //        //生成一个新的
        //        listNode.SetValue("ProcessStatusText", "请稍候");
        //        WebServiceInvoker.GetThumbnailFileId(tmpPostCard.FileId, resp =>
        //        {
        //            tmpPostCard.ThumbnailFileId = string.IsNullOrEmpty(resp.Id) ? tmpPostCard.FileId : resp.Id;
        //            GeneratePostCardThumbnailImage(tmpPostCard);
        //        }, message =>
        //        {
        //            tmpPostCard.FileId = tmpPostCard.FileId;

        //            if (tmpPostCard.RetryGenerateThumbnail())
        //            {
        //                //重新生成缩略图
        //                GeneratePostCardThumbnailImage(tmpPostCard);
        //            }
        //            else
        //            {
        //                XtraMessageBox.Show("图像有问题，无法生成缩略图！");
        //            }
        //        });
        //        return;
        //    }

        //    var directoryInfo = new DirectoryInfo(SystemConstants.tmpFilePath);
        //    if (!directoryInfo.Exists)
        //        directoryInfo.Create();
        //    //

        //    WebServiceInvoker.DownLoadFileByFileId(tmpPostCard.FileId, directoryInfo, fileInfo =>
        //    {
        //        tmpPostCard.FileInfo = fileInfo;
        //        tmpPostCard.ProcessStatusText = tmpPostCard.ProcessStatusText;
        //        listNode.SetValue("ProcessStatusText", tmpPostCard.ProcessStatusText);
        //        listNode.SetValue("thumbnailDownloadStatus", "已下载");
        //        listNode.SetValue("ProcessStatus", (int) tmpPostCard.ProcessStatus);
        //        if (treeList1.FocusedNode == listNode)
        //        {
        //            cropControllerCrop.PostCardInfo = null;
        //            cropControllerPreview.PostCardInfo = null;
        //            cropControllerCrop.PostCardInfo = tmpPostCard;
        //            cropControllerPreview.PostCardInfo = tmpPostCard;
        //        }
        //    });
        //}
        //private void TreeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        //{
        //    var focusedNodeTag = e.Node.Tag;
        //    if (focusedNodeTag == null) return;

        //    if (focusedNodeTag is EnvelopeInfo tmpEnvelope)
        //    {
        //        envelopeInfoController1.EnvelopeInfo = tmpEnvelope;
        //        layoutControlGroup2.Visibility = LayoutVisibility.Always;
        //        layoutControlGroup6.Visibility = LayoutVisibility.Never;
        //        postCardInfoController1.PostCardInfo = null;
        //    }
        //    else if (focusedNodeTag is PostCardInfo tmpPostCard)
        //    {
        //        //如果当前选择的明信片非空，并且文件信息为空
        //        if (tmpPostCard.FileInfo == null)
        //        {
        //            treeList1.FocusedNode = e.OldNode;
        //            return;
        //        }

        //        layoutControlGroup2.Visibility = LayoutVisibility.Never;
        //        layoutControlGroup6.Visibility = LayoutVisibility.Always;
        //        pictureCropControl1.CropContext = null;
        //        postCardInfoController1.PostCardInfo = tmpPostCard;
        //    }
        //}
        private void PostCardCropForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                pictureCropControl1.IsPreview = true;
            }
        }

        private void PostCardCropForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                pictureCropControl1.IsPreview = false;
            }
        }


        private void RibbonControl1_Click(object sender, EventArgs e)
        {
        }


        private void BarButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            pictureCropControl1.Rotate(270);
        }

        private void BarButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            pictureCropControl1.Rotate(90);
        }

        private void BarButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            pictureCropControl1.Rotate(180);
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

        private void BarButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            WebServiceInvoker.GetInstance().ChangeOrderStatus(_orderId, "4", re =>
            {
                NeedRefresh = true;
                if (XtraMessageBox.Show("操作成功,是否关闭当前裁切窗口？", "订单完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                }
            });
        }

        private void EnvelopeView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (EnvelopeView.GetFocusedRow() is EnvelopeInfo envelope)
            {
                postCardControl.DataSource = envelope.PostCards;
            }
        }

        private void PostCardView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (PostCardView.GetFocusedRow() is PostCardInfo postCardInfo)
            {
                progressBarControl1.EditValue = 0;
                WebServiceInvoker.GetFileServerInstance().DownLoadBytesAsync("http://127.0.0.1:8089/file/" + postCardInfo.ThumbnailFileId, fileInfo =>
                {
                    if (PostCardView.GetFocusedRow() == postCardInfo)
                    {
                        var cropContext = new CropContext
                        {
                            Image = Image.FromStream(new MemoryStream(fileInfo)),
                            ProductSize = new Size(postCardInfo.ProductSize.Width, postCardInfo.ProductSize.Height),
                            StyleInfo = new StyleInfo()
                            {
                                MarginAll = 10
                            }
                        };

                        cropContext.CropInfo = new CropInfo(cropContext.Image.Size, cropContext.PicturePrintAreaSize, fit: cropContext.StyleInfo.Fit);
                        pictureCropControl1.CropContext = cropContext;


                        pictureCropControl1.RefreshImage();
                    }
                }, process: pro => { progressBarControl1.EditValue = pro; });
            }
        }

        private void PostCardCropForm_Shown(object sender, EventArgs e)
        {
            envelopeListControl.RefreshDataSource();
            postCardControl.RefreshDataSource();
            EnvelopeView.RefreshData();
            PostCardView.RefreshData();
        }

        private void barToggleSwitchItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            pictureCropControl1.IsPreview = barToggleSwitchItem1.BindableChecked;
        }
    }
}