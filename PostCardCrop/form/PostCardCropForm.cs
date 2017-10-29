using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using postCardCenterSdk.request.postCard;
using postCardCenterSdk.sdk;
using PostCardCrop.model;
using PostCardCrop.translator.response;
using soho.constant.postcard;
using soho.constant.system;

namespace PostCardCrop.form
{
    public partial class PostCardCropForm : RibbonForm
    {
        private readonly string orderId;

        //明信片集合
        private readonly List<PostCardInfo> postCardList = new List<PostCardInfo>();

        private readonly Dictionary<string, TreeListNode> postCardNodeMap = new Dictionary<string, TreeListNode>();


        public PostCardCropForm(string focusedRowOrderId)
        {
            InitializeComponent();
            //绑定鼠标滑轮事件
            cropControllerCrop.MouseWheel += cropControllerCrop.CanvasMouseWheel;
            orderId = focusedRowOrderId;
            layoutControlGroup5.Visibility = layoutControlGroup6.Visibility = LayoutVisibility.Never;
        }

        private void PostCardCropForm_Load(object sender, EventArgs e)
        {
            WebServiceInvoker.GetAllEnvelopeByOrderId(orderId, envelopeList =>
            {
                var dataSources = new List<EnvelopeInfo>();

                envelopeList.ForEach(tmpEnvelope =>
                {
                    var envelope = tmpEnvelope.TranslateToEnvelope();
                    dataSources.Add(envelope);

                    WebServiceInvoker.GetPostCardByEnvelopeId(envelope.Id, postCards =>
                    {
                        var treeListNode = treeList1.Nodes.Add();
                        treeListNode.Tag = envelope;
                        //明信片集合的名字暂且设置为纸张大小，后续适应需求
                        treeListNode.SetValue("name", envelope.ProductSize);
                        //明信片集合不需要下载缩略图
                        treeListNode.SetValue("thumbnailDownloadStatus", "已下载");
                        if (postCards == null) return;
                        //遍历明信片，对明信片进行Node绑定
                        foreach (var postCard1 in postCards)
                        {
                            var tmpPostCard = postCard1.TranlateToPostCard();
                            envelope.PostCards.Add(tmpPostCard);
                            tmpPostCard.ProductSize = envelope.ProductSize;
                            var listNode = treeListNode.Nodes.Add();
                            //明信片ID到节点的映射
                            postCardNodeMap.Add(tmpPostCard.PostCardId, listNode);
                            //明信片添加到明信片集合中去
                            postCardList.Add(tmpPostCard);
                            //此明信片的名字显示在上面
                            listNode.SetValue("name", tmpPostCard.FileName);
                            listNode.SetValue("ProcessStatus", (int) tmpPostCard.ProcessStatus);
                            //设置此节点的Target为此明信片；
                            listNode.Tag = tmpPostCard;
                            GeneratePostCardThumbnailImage(tmpPostCard);
                        }
                        treeListNode.ExpandAll();
                        //明信片集合信息更新显示
                        if (envelopeInfoController1.EnvelopeInfo == null)
                        {
                            envelopeInfoController1.EnvelopeInfo = envelope;
                            envelopeInfoController1.RefreshEnvelopeInfo();
                        }
                    }, message => { XtraMessageBox.Show(message); });
                });
            });
        }

        /// <summary>
        ///     获取明信片缩略图
        /// </summary>
        /// <param name="treeListNode">此明信片所在的节点上</param>
        /// <param name="tmpPostCard">此明信片信息</param>
        private void GeneratePostCardThumbnailImage(PostCardInfo tmpPostCard)
        {
            var listNode = postCardNodeMap[tmpPostCard.PostCardId];
            listNode.ImageIndex = listNode.SelectImageIndex = 1;
            listNode.SetValue("thumbnailDownloadStatus", "未下载");
            //var card = postCard;
            listNode.SetValue("ProcessStatusText", "请稍候");
            var thumbnailFileId = string.IsNullOrEmpty(tmpPostCard.ThumbnailFileId) ? tmpPostCard.FileId : tmpPostCard.ThumbnailFileId;
            //如果当前明信片没有缩略图文件
            if (string.IsNullOrEmpty(tmpPostCard.ThumbnailFileId))
            {
                //生成一个新的
                listNode.SetValue("ProcessStatusText", "请稍候");
                WebServiceInvoker.GetThumbnailFileId(tmpPostCard.FileId, resp =>
                {
                    tmpPostCard.ThumbnailFileId = string.IsNullOrEmpty(resp.Id) ? tmpPostCard.FileId : resp.Id;
                    GeneratePostCardThumbnailImage(tmpPostCard);
                }, message =>
                {
                    tmpPostCard.FileId = tmpPostCard.FileId;
                    //重新生成缩略图
                    GeneratePostCardThumbnailImage(tmpPostCard);
                });
                return;
            }
            var directoryInfo = new DirectoryInfo(SystemConstants.tmpFilePath);
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            //

            WebServiceInvoker.DownLoadFileByFileId(tmpPostCard.FileId, directoryInfo, fileInfo =>
            {
                tmpPostCard.FileInfo = fileInfo;
                tmpPostCard.ProcessStatusText = tmpPostCard.ProcessStatusText;
                listNode.SetValue("ProcessStatusText", tmpPostCard.ProcessStatusText);
                listNode.SetValue("thumbnailDownloadStatus", "已下载");
                listNode.SetValue("ProcessStatus", (int) tmpPostCard.ProcessStatus);
                if (treeList1.FocusedNode == listNode)
                {
                    cropControllerCrop.PostCardInfo = null;
                    cropControllerPreview.PostCardInfo = null;
                    cropControllerCrop.PostCardInfo = tmpPostCard;
                    cropControllerPreview.PostCardInfo = tmpPostCard;
                }
            });
        }

        private void TreeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            //清理图片
            cropControllerCrop.ReleaseImage();

            var tmpObj = e.Node.GetValue("thumbnailDownloadStatus") as string;
            if (string.IsNullOrEmpty(tmpObj))
            {
                cropControllerCrop.RefreshPostCard();
                return;
            }
            if (!string.Equals(tmpObj, "已下载"))
            {
                XtraMessageBox.Show("该图片正在下载中,请稍后重试!");
                //if (e.OldNode != null)
                //{
                //    cropControllerCrop.RefreshPostCard();
                //    treeList1.SetFocusedNode(e.OldNode);
                //}
                //清理图片
                cropControllerCrop.ReleaseImage();
                cropControllerCrop.RefreshPostCard();
                return;
            }
            var focusedNodeTag = e.Node.Tag;
            if (focusedNodeTag == null) return;


            if (focusedNodeTag is EnvelopeInfo tmpEnvelope)
            {
                envelopeInfoController1.EnvelopeInfo = tmpEnvelope;
                layoutControlGroup2.Visibility = LayoutVisibility.Always;
                layoutControlGroup6.Visibility = LayoutVisibility.Never;
                layoutControlGroup5.Visibility = LayoutVisibility.Never;
                postCardInfoController1.PostCardInfo = null;
                cropControllerCrop.PostCardInfo = null;
                cropControllerPreview.PostCardInfo = null;
            }
            else if (focusedNodeTag is PostCardInfo tmpPostCard)
            {
                //如果当前选择的明信片非空，并且文件信息为空
                if (tmpPostCard.FileInfo == null)
                {
                    treeList1.FocusedNode = e.OldNode;
                    return;
                }
                layoutControlGroup2.Visibility = LayoutVisibility.Never;
                layoutControlGroup6.Visibility = LayoutVisibility.Always;
                layoutControlGroup5.Visibility = LayoutVisibility.Always;
                postCardInfoController1.PostCardInfo = tmpPostCard;
                cropControllerCrop.PostCardInfo = tmpPostCard;
                cropControllerPreview.PostCardInfo = tmpPostCard;
            }
        }


        private void PostCardCropController1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Space) return;
            cropControllerPreview.IsPreview = true;
            cropControllerPreview.RefreshPostCard();
        }

        private void PostCardCropController1_Load(object sender, EventArgs e)
        {
        }

        private void PostCardCropController1_KeyUp(object sender, KeyEventArgs e)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (e.KeyCode)
            {
                case Keys.Space:
                    cropControllerPreview.IsPreview = false;
                    cropControllerPreview.RefreshPostCard();
                    break;
            }
        }

        private void PostCardCropForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                cropControllerPreview.IsPreview = true;
                cropControllerPreview.RefreshPostCard();
            }
        }

        private void PostCardCropForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                cropControllerPreview.IsPreview = false;
                cropControllerPreview.RefreshPostCard();
            }
        }


        private void RibbonControl1_Click(object sender, EventArgs e)
        {
        }

        private void CropControllerCrop_CropInfoChanged(CropInfo newCropInfo)
        {
            cropControllerPreview.CropInfo = newCropInfo;
        }

        private void BarButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            cropControllerCrop.Rotate(270);
        }

        private void BarButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            cropControllerCrop.Rotate(90);
        }

        private void BarButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            cropControllerCrop.Rotate(180);
        }


        private void EnvelopeInfoController1_Load(object sender, EventArgs e)
        {
        }

        private void CropControllerCrop_SuccessSubmit(PostCardInfo postCardInfo)
        {
            var tmpNode = postCardNodeMap[postCardInfo.PostCardId];
            tmpNode.SetValue("ProcessStatusText", postCardInfo.ProcessStatusText);
            tmpNode.SetValue("ProcessStatus", (int) postCardInfo.ProcessStatus);

            if (treeList1.FocusedNode == tmpNode.ParentNode)
                envelopeInfoController1.RefreshEnvelopeInfo();
            Application.DoEvents();
        }

        private void CropControllerCrop_FailureSubmit(PostCardInfo postCardInfo)
        {
            XtraMessageBox.Show("提交失败");
            Application.DoEvents();
        }

        private void CropControllerCrop_OnSubmit(PostCardInfo node)
        {
            var tmpNode = postCardNodeMap[node.PostCardId];
            tmpNode.SetValue("ProcessStatus", (int) node.ProcessStatus);
            tmpNode.SetValue("ProcessStatusText", "提交中");

            foreach (var postCardInfo in postCardList)
                if (postCardInfo.ProcessStatus == PostCardProcessStatusEnum.BEFORE_SUBMIT)
                {
                    treeList1.FocusedNode = postCardNodeMap[postCardInfo.PostCardId];
                    return;
                }
            var parentNode = treeList1.FocusedNode.ParentNode;
            if (parentNode != null)
                treeList1.SetFocusedNode(parentNode);
        }

        private void PostCardCropForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cropControllerCrop.Image = cropControllerPreview.Image = null;
        }

        private void CropControllerCrop_Load(object sender, EventArgs e)
        {
        }

        private void CropControllerCrop_match(PostCardInfo postCardInfo)
        {
            if (barCheckItem1.Checked)
                if (postCardInfo.ProcessStatus == PostCardProcessStatusEnum.BEFORE_SUBMIT)
                    timer1.Start();
        }

        private void BarCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            cropControllerCrop.SubmitPostCard();
        }

        private void CropControllerCrop_error(string message)
        {
            XtraMessageBox.Show(message);
            cropControllerPreview.PostCardInfo = null;
            cropControllerCrop.PostCardInfo = null;
        }

        private void BarButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            postCardList.ForEach(postCard =>
            {
                WebServiceInvoker.GetPostCardInfo(postCard.PostCardId, resp =>
                {
                    var postCardResp = resp.TranlateToPostCard();
                    postCard.ProcessStatus = postCardResp.ProcessStatus;
                    postCard.ProcessStatusText = postCardResp.ProcessStatusText;
                    var tmpNode = postCardNodeMap[postCardResp.PostCardId];
                    tmpNode.SetValue("ProcessStatusText", postCard.ProcessStatusText);
                    tmpNode.SetValue("ProcessStatus", (int) postCard.ProcessStatus);
                    Application.DoEvents();
                }, message => { XtraMessageBox.Show(message); });
            });
        }

        private void BarButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            postCardList.ForEach(postCard =>
            {
                var tmpNode = postCardNodeMap[postCard.PostCardId];
                postCard.ProcessStatus = PostCardProcessStatusEnum.BEFORE_SUBMIT;
                postCard.CropInfo = null;
                postCard.ProcessStatusText = "未提交";
                tmpNode.SetValue("ProcessStatus", (int) PostCardProcessStatusEnum.BEFORE_SUBMIT);
                tmpNode.SetValue("ProcessStatusText", "未提交");
            });
            var a = postCardNodeMap.Values;
        }

        private void BarButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            var postCard = cropControllerCrop.PostCardInfo;
            if (postCard == null) return;
            var barButton = sender as BarButtonItem;
            postCard.FrontStyle = e.Item.Tag as string;

            var request = new PostCardInfoPatchRequest
            {
                PostCardId = postCard.PostCardId,
                FrontStyle = postCard.FrontStyle
            };

            WebServiceInvoker.ChangePostCardFrontStyle(request, resp =>
            {
                postCard.CropInfo = null;
                cropControllerCrop.PostCardInfo = postCard;
                cropControllerPreview.PostCardInfo = postCard;
                var postCardInfo = resp.TranlateToPostCard();
                postCardNodeMap[postCardInfo.PostCardId].SetValue("ProcessStatusText", postCardInfo.ProcessStatusText);
                postCardNodeMap[postCardInfo.PostCardId].SetValue("ProcessStatus", (int) postCardInfo.ProcessStatus);
                postCard.ProcessStatus = postCardInfo.ProcessStatus;
                postCard.ProcessStatusText = postCardInfo.ProcessStatusText;
            }, message => { XtraMessageBox.Show(message); });
        }

        private void PostCardInfoController1_FileChanged(PostCardInfo node)
        {
            if (node == null) return;
            if (postCardNodeMap.ContainsKey(node.PostCardId))
            {
                var tmpNode = postCardNodeMap[node.PostCardId];
                if (treeList1.FocusedNode == tmpNode)
                    cropControllerCrop.PostCardInfo = null;
                tmpNode.SetValue("name", node.FileName);
                tmpNode.SetValue("ProcessStatus", (int) node.ProcessStatus);
                tmpNode.SetValue("ProcessStatusText", node.ProcessStatusText);
                GeneratePostCardThumbnailImage(node);
            }
        }

        private void BarButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (cropControllerCrop.CropInfo == null)
                return;
            cropControllerCrop.CropInfo.HeightScale = 1;
            cropControllerCrop.CropInfo.WidthScale = 1;
            cropControllerCrop.CropInfo.LeftScale = 0;
            cropControllerCrop.CropInfo.TopScale = 0;
            cropControllerCrop.CropInfo = cropControllerCrop.CropInfo;
            cropControllerCrop.SubmitPostCard();
        }
    }
}