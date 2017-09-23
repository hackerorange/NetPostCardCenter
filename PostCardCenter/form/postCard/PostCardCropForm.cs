using System;
using System.Collections.Generic;
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
using postCardCenterSdk.sdk;

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
                            var tmpPostCard = postCard;
                            var listNode = treeListNode.Nodes.Add();
                            listNode.SetValue("name", postCard.fileName);
                            listNode.Tag = postCard;

                            listNode.ImageIndex = listNode.SelectImageIndex = 1;


                            var card = postCard;
                            listNode.SetValue("status", "下载中");

                            WebServiceInvoker.DownLoadFileByFileId(postCard.fileId, success:fileInfo =>
                            {
                                card.fileInfo = fileInfo;
                                if (tmpPostCard.cropInfo == null)
                                {
                                    needProcess.Add(listNode);
                                    tmpPostCard.processStatus = "未提交";
                                }
                                else
                                {
                                    tmpPostCard.processStatus = "已提交";
                                }
                                listNode.SetValue("status", tmpPostCard.processStatus);
                                listNode.SetValue("downloadState", true);
                            });
                        }
                        treeListNode.ExpandAll();
                    }, message => { XtraMessageBox.Show(message); });
                });
            });
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if ("下载中".Equals(e.Node.GetValue("status")))
            {
                XtraMessageBox.Show("该图片正在下载中,请稍后重试!");
            }

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

                if (tmpEnvelope != null)
                {
                    envelopeInfoController1.EnvelopeId = tmpEnvelope.envelopeId;
                    envelopeDetailGroup.Visibility = LayoutVisibility.Always;
                    cropControllerCrop.Node = e.Node;
                    cropControllerPreview.Node = e.Node;
                }
            }
            //如果不是明信片，直接返回
            if (focusedNodeTag.GetType() != typeof(PostCard)) return;
            var tmpPostCard = focusedNodeTag as PostCard;
            if (tmpPostCard != null && tmpPostCard.fileInfo == null)
            {
                treeList1.FocusedNode = e.OldNode;
                return;
            }
            envelopeDetailGroup.Visibility = LayoutVisibility.Never;
            cropControllerCrop.Node = e.Node;
            cropControllerPreview.Node = e.Node;
        }


        private void postCardCropController1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Space) return;
            cropControllerPreview.IsPreview = true;
            cropControllerPreview.RefreshPostCard();
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


        private void envelopeInfoController1_Load(object sender, EventArgs e)
        {
        }

        private void cropControllerCrop_SuccessSubmit(TreeListNode node)
        {
            node.SetValue("status", "已提交");
            Application.DoEvents();
        }

        private void cropControllerCrop_FailureSubmit(TreeListNode node)
        {
            XtraMessageBox.Show("提交失败");
            Application.DoEvents();
        }

        private void cropControllerCrop_OnSubmit(TreeListNode node, PostCardCropController sender)
        {
            TreeListNode nextTreeListNode = null;
            while (needProcess.Count > 0)
            {
                nextTreeListNode = needProcess[0];
                if (nextTreeListNode.GetValue("status") == "已提交" || nextTreeListNode.GetValue("status") == "正在提交")
                {
                    needProcess.Remove(nextTreeListNode);
                    continue;
                }
                break;
            }
            if (nextTreeListNode != null)
            {
                treeList1.SetFocusedNode(nextTreeListNode);
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

        private void PostCardCropForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cropControllerCrop.Image = cropControllerPreview.Image = null;
        }
    }
}