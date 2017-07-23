using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using PostCardCenter.myController;
using soho.domain;
using soho.webservice;
using Padding = System.Windows.Forms.Padding;

namespace PostCardCenter.form.postCard
{
    public partial class PostCardCropForm : RibbonForm
    {
        private readonly IList<TreeListNode> needProcess = new List<TreeListNode>();
        private readonly string orderId;


        public PostCardCropForm(string focusedRowOrderId)
        {
            InitializeComponent();
            orderId = focusedRowOrderId;
        }

        private void postCardCropForm_Load(object sender, EventArgs e)
        {
            EnvelopeInvoker.GetAllEnvelopeByOrderId(orderId, envelopeList =>
            {
                envelopeList.ForEach(envelope =>
                {
                    EnvelopeInvoker.GetPostCardByEnvelopeId(envelope.envelopeId, postCards =>
                    {
                        envelope.postCards = postCards;
                        var treeListNode = treeList1.Nodes.Add();
                        treeListNode.Tag = envelope;
                        treeListNode.SetValue("name", "成品尺寸:" + envelope.productWidth + "×" + envelope.productHeight);
                        if (envelope.postCards == null) return;
                        foreach (var postCard in envelope.postCards)
                        {
                            var listNode = treeListNode.Nodes.Add();
                            listNode.SetValue("name", postCard.fileName);
                            listNode.Tag = postCard;

                            listNode.ImageIndex = listNode.SelectImageIndex = 1;
                            if (postCard.cropInfo == null)
                            {
                                needProcess.Add(listNode);
                                postCard.processStatus = "未提交";
                            }
                            else
                            {
                                postCard.processStatus = "已提交";
                            }
                            listNode.SetValue("status", postCard.processStatus);

                            var card = postCard;
                            SohoInvoker.downLoadFile(postCard.fileId, false, fileInfo => { card.fileInfo = fileInfo; });
                        }
                        treeListNode.ExpandAll();
                    }, message => { XtraMessageBox.Show(message); });
                });
            });
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (treeList1.FocusedNode == null) return;
            if (e.OldNode != null && "已修改".Equals(e.OldNode.GetValue("status")))
            {
                XtraMessageBox.Show("明信片信息已修改，请提交后再切换到下一个图片");
                treeList1.SetFocusedNode(e.OldNode);
                return;
            }
            var focusedNodeTag = e.Node.Tag;
            if (focusedNodeTag == null) return;

            //如果是明信片集合
            if (focusedNodeTag.GetType() == typeof(Envelope))
            {
                var tmpEnvelope = focusedNodeTag as Envelope;
                cropControllerPreview.Image = cropControllerCrop.Image = null;
                cropControllerPreview.RefreshPostCard();
                cropControllerCrop.RefreshPostCard();
                cropControllerPreview.PostCardId = null;
                cropControllerCrop.PostCardId = null;
                envelopeInfoController1.EnvelopeId = tmpEnvelope.envelopeId;
                envelopeDetailGroup.Visibility = LayoutVisibility.Always;
                
            }

            if (focusedNodeTag.GetType() == typeof(PostCard))
            {
                envelopeDetailGroup.Visibility = LayoutVisibility.Never;
                var envelopeInfo = treeList1.FocusedNode.ParentNode.Tag as Envelope;
                var tmpPostCard = focusedNodeTag as PostCard;
                if (tmpPostCard.fileInfo == null)
                {
                    treeList1.FocusedNode = e.OldNode;
                    return;
                }
                cropControllerPreview.PostCardId = cropControllerCrop.PostCardId = tmpPostCard.postCardId;
                cropControllerPreview.Image = cropControllerCrop.Image = Image.FromFile(tmpPostCard.fileInfo.FullName);
                cropControllerPreview.Margin = cropControllerCrop.Margin = new Padding(5);
                cropControllerPreview.Scale = cropControllerCrop.Scale = 0;

                if (tmpPostCard.frontStyle.Equals("A", StringComparison.CurrentCultureIgnoreCase))
                {
                    cropControllerPreview.Margin = cropControllerCrop.Margin = new Padding(5);
                    cropControllerPreview.Scale = cropControllerCrop.Scale = 0;
                }
                if (tmpPostCard.frontStyle.Equals("B", StringComparison.CurrentCultureIgnoreCase))
                {
                    cropControllerPreview.Margin = cropControllerCrop.Margin = new Padding(0);
                    cropControllerPreview.Scale = cropControllerCrop.Scale = 0;
                }
                if (tmpPostCard.frontStyle.Equals("C", StringComparison.CurrentCultureIgnoreCase))
                {
                    cropControllerPreview.Margin = cropControllerCrop.Margin = new Padding(5);
                    cropControllerPreview.Scale = cropControllerCrop.Scale = 1;
                }

                cropControllerPreview.ProductSize = cropControllerCrop.ProductSize =
                    new Size(envelopeInfo.productWidth, envelopeInfo.productHeight);
                cropControllerPreview.CropInfo = cropControllerCrop.CropInfo = tmpPostCard.cropInfo;
                cropControllerPreview.RefreshPostCard();
                cropControllerCrop.RefreshPostCard();
                e.Node.SetValue("status", tmpPostCard.processStatus);
                layoutControlGroup6.Selected = true;
                envelopeInfoController1.EnvelopeId = envelopeInfo.envelopeId;
            }
        }


        private void postCardCropController1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                cropControllerPreview.IsPreview = true;
                cropControllerPreview.RefreshPostCard();
            }
        }

        private void postCardCropController1_Load(object sender, EventArgs e)
        {
        }

        private void postCardCropController1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                cropControllerPreview.IsPreview = false;
                cropControllerPreview.RefreshPostCard();
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


        private void ribbonControl1_Click(object sender, EventArgs e)
        {
        }

        private void cropControllerCrop_CropInfoChanged(CropInfo newCropInfo)
        {
            cropControllerPreview.CropInfo = newCropInfo;
            if (newCropInfo == null) return;
            var focusedNode = treeList1.FocusedNode;
            if (focusedNode.Tag.GetType().FullName.Equals(typeof(PostCard).FullName))
            {
                var tmpPostCard = focusedNode.Tag as PostCard;

                if (tmpPostCard.cropInfo == null || !tmpPostCard.cropInfo.Equals(newCropInfo))
                {
                    if (tmpPostCard.cropInfo == null)
                        tmpPostCard.cropInfo = newCropInfo.Clone() as CropInfo;
                    tmpPostCard.processStatus = "已修改";
                    tmpPostCard.cropInfo.leftScale = newCropInfo.leftScale;
                    tmpPostCard.cropInfo.topScale = newCropInfo.topScale;
                    tmpPostCard.cropInfo.heightScale = newCropInfo.heightScale;
                    tmpPostCard.cropInfo.widthScale = newCropInfo.widthScale;
                    tmpPostCard.cropInfo.rotation = newCropInfo.rotation;
                    focusedNode.SetValue("status", tmpPostCard.processStatus);
                }
            }
        }

        private void cropControllerCrop_OnSubmit(object sender)
        {
            var a = sender as PostCardCropController;
            if ("正在提交".Equals(treeList1.FocusedNode.GetValue("status")))
                PostCardInvoker.SubmitPostCardCropInfo(a.PostCardId, a.CropInfo);
        }

        private void cropControllerCrop_Load(object sender, EventArgs e)
        {
        }

        private void cropControllerCrop_AfterSubmit(object sender)
        {
            var tmpPostCard = treeList1.FocusedNode.Tag as PostCard;

            if (tmpPostCard != null)
            {
                tmpPostCard.processStatus = "已提交";
                treeList1.FocusedNode.SetValue("status", tmpPostCard.processStatus);
                if (splashScreenManager1.IsSplashFormVisible)
                    splashScreenManager1.CloseWaitForm();
                Application.DoEvents();
                TreeListNode nexTreeListNode = null;
                while (needProcess.Count > 0)
                {
                    nexTreeListNode = needProcess[0];
                    if (nexTreeListNode.GetValue("status") == "已提交")
                    {
                        needProcess.Remove(nexTreeListNode);
                        continue;
                    }
                    break;
                }
                if (nexTreeListNode != null)
                {
                    treeList1.SetFocusedNode(nexTreeListNode);
                }
                else
                {
                    var nextVisibleNode = treeList1.FocusedNode.NextVisibleNode;
                    if (nextVisibleNode != null)
                        treeList1.FocusedNode = nextVisibleNode;
                    else if (treeList1.FocusedNode.ParentNode != null)
                        treeList1.FocusedNode = treeList1.FocusedNode.ParentNode;
                }
            }
        }

        private void cropControllerCrop_BeforeSubmit(object sender)
        {
            if (!"已提交".Equals(treeList1.FocusedNode.GetValue("status")))
            {
                treeList1.FocusedNode.SetValue("status", "正在提交");
                if (!splashScreenManager1.IsSplashFormVisible)
                    splashScreenManager1.ShowWaitForm();
            }
            Application.DoEvents();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            cropControllerCrop.Rotate(270);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            cropControllerCrop.Rotate(90);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            cropControllerCrop.Rotate(180);
        }

        private void ribbonStatusBar1_Click(object sender, EventArgs e)
        {
        }

        private void envelopeInfoController1_Load(object sender, EventArgs e)
        {

        }
    }
}