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
using postCardCenterSdk.sdk;
using soho.translator;
using soho.translator.response;

namespace PostCardCenter.form.postCard
{
    public partial class PostCardCropForm : RibbonForm
    {
        private readonly IList<TreeListNode> needProcess = new List<TreeListNode>();
        private readonly string orderId;


        public PostCardCropForm(string focusedRowOrderId)
        {
            InitializeComponent();
            //绑定鼠标滑轮事件
            this.cropControllerCrop.MouseWheel += cropControllerCrop.CanvasMouseWheel;
            orderId = focusedRowOrderId;
        }

        private void postCardCropForm_Load(object sender, EventArgs e)
        {
            WebServiceInvoker.GetAllEnvelopeByOrderId(orderId, envelopeList =>
            {
                envelopeList.ForEach(tmpEnvelope =>
                {
                    EnvelopeInfo envelope = tmpEnvelope.TranslateToEnvelope();
                    

                    WebServiceInvoker.GetPostCardByEnvelopeId(envelope.Id, postCards =>
                    {
                        envelope.PostCards = new List<PostCardInfo>();
                        var treeListNode = treeList1.Nodes.Add();
                        treeListNode.Tag = envelope;
                        treeListNode.SetValue("name", envelope.ProductSize.Name);
                        
                        if (postCards==null) return;
                        foreach (var postCard1 in postCards)
                        {
                            var tmpPostCard = postCard1.TranlateToPostCard();
                            var listNode = treeListNode.Nodes.Add();
                            listNode.SetValue("name", tmpPostCard.FileName);
                            listNode.Tag = tmpPostCard;

                            listNode.ImageIndex = listNode.SelectImageIndex = 1;

                            needProcess.Add(listNode);
                            //var card = postCard;
                            listNode.SetValue("status", "下载中");

                            WebServiceInvoker.DownLoadFileByFileId(tmpPostCard.FileId, success:fileInfo =>
                            {
                                tmpPostCard.FileInfo = fileInfo;
                                if (tmpPostCard.CropInfo == null)
                                {
                                    
                                    tmpPostCard.ProcessStatus = "未提交";
                                }
                                else
                                {
                                    if (needProcess.Contains(listNode)){
                                        needProcess.Remove(listNode);
                                    }
                                    tmpPostCard.ProcessStatus = "已提交";
                                }
                                listNode.SetValue("status", tmpPostCard.ProcessStatus);
                                listNode.SetValue("downloadState", true);
                            });
                        }
                        treeListNode.ExpandAll();
                        if (String.IsNullOrEmpty(envelopeInfoController1.EnvelopeId))
                        {
                            envelopeInfoController1.EnvelopeId = envelope.Id;
                            envelopeInfoController1.RefreshEnvelopeInfo();
                        }
                    }, message => { XtraMessageBox.Show(message); });
                });
            });
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            //清理图片
            cropControllerCrop.ReleaseImage();
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
            if (focusedNodeTag.GetType() == typeof(EnvelopeInfo))
            {
                var tmpEnvelope = focusedNodeTag as EnvelopeInfo;

                if (tmpEnvelope != null)
                {
                    envelopeInfoController1.EnvelopeId = tmpEnvelope.Id;
                    envelopeDetailGroup.Enabled = true;
                    
                    cropControllerCrop.Node = e.Node;
                    cropControllerPreview.Node = e.Node;
                }
            }
            //如果不是明信片，直接返回
            if (focusedNodeTag.GetType() != typeof(PostCardInfo)) return;
            var tmpPostCard = focusedNodeTag as PostCardInfo;
            if (tmpPostCard != null && tmpPostCard.FileInfo == null)
            {
                treeList1.FocusedNode = e.OldNode;
                return;
            }
            envelopeDetailGroup.Enabled = false;
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
            if(treeList1.FocusedNode == node.ParentNode)
            {
                envelopeInfoController1.RefreshEnvelopeInfo();
            }
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
                    nextTreeListNode = null;
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
                treeList1.FocusedNode = treeList1.FocusedNode.ParentNode;
            }
        }

        private void PostCardCropForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            cropControllerCrop.Image = cropControllerPreview.Image = null;
        }

        private void cropControllerCrop_Load(object sender, EventArgs e)
        {            
        }

        private void cropControllerCrop_match(TreeListNode node, PostCardInfo postCardInfo)
        {
            if (barCheckItem1.Checked)
            {
                if (node.GetValue("status") == "未提交")
                {
                    timer1.Start();
                }
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            cropControllerCrop.SubmitPostCard();
        }
    }
}