using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Native;
using DevExpress.XtraTreeList.Nodes;
using PostCardCenter.myController;
using soho.domain;
using soho.webservice;

namespace PostCardCenter.form.postCard
{
    public partial class PostCardCropForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Queue<TreeListNode> needProcess = new Queue<TreeListNode>();

        public PostCardCropForm(string focusedRowOrderId)
        {
            InitializeComponent();
            var orderDetails = EnvelopeInvoker.GetOrderDetails(focusedRowOrderId);
            foreach (var orderDetail in orderDetails)
            {
                var treeListNode = treeList1.Nodes.Add();
                treeListNode.Tag = orderDetail;
                treeListNode.SetValue("name", orderDetail.productWidth + "×" + orderDetail.productHeight);
                if (orderDetail.postCards == null) continue;
                foreach (var orderDetailPostCard in orderDetail.postCards)
                {
                    var listNode = treeListNode.Nodes.Add();
                    listNode.SetValue("name", orderDetailPostCard.fileName);
                    listNode.Tag = orderDetailPostCard;
                    listNode.ImageIndex = listNode.SelectImageIndex = 1;
                    if (orderDetailPostCard.cropInfo == null)
                    {
                        needProcess.Enqueue(listNode);
                        orderDetailPostCard.processStatus = "未提交";
                    }
                    else
                    {
                        orderDetailPostCard.processStatus = "已提交";
                    }
                    listNode.SetValue("status", orderDetailPostCard.processStatus);
                    new Thread(downloadFile).Start(orderDetailPostCard);
                }
                treeListNode.ExpandAll();
            }
        }

        public void downloadFile(object fileId)
        {
            var fileIdString = fileId as PostCard;
            fileIdString.fileInfo = SohoInvoker.downLoadFile(fileIdString.fileId);
        }

        private void postCardCropForm_Load(object sender, EventArgs e)
        {
            if (needProcess.Count > 0)
            {
                var a = needProcess.Dequeue();
                treeList1.SetFocusedNode(a);
            }
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
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
                gridControl1.DataSource = tmpEnvelope.postCards;
                gridControl1.RefreshDataSource();
            }

            if (focusedNodeTag.GetType() == typeof(PostCard))
            {
                var envelopeInfo = treeList1.FocusedNode.ParentNode.Tag as Envelope;
                var tmpPostCard = focusedNodeTag as PostCard;
                var a = SohoInvoker.downLoadFile(tmpPostCard.fileId);
                if (!a.Exists) return;
                cropControllerPreview.PostCardId = cropControllerCrop.PostCardId = tmpPostCard.postCardId;
                cropControllerPreview.Image = cropControllerCrop.Image = Image.FromFile(a.FullName);
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
            }
        }

        private void cropBoxControl1_Load(object sender, EventArgs e)
        {
        }

        private Queue<Image> images = new Queue<Image>();


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
                    {
                        tmpPostCard.cropInfo = newCropInfo.Clone() as CropInfo;
                    }
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
            {
                PostCardInvoker.SubmitPostCardCropInfo(a.PostCardId, a.CropInfo);
            }
        }

        private void cropControllerCrop_Load(object sender, EventArgs e)
        {
        }

        private SubmitWaitForm submitWaitForm = new SubmitWaitForm();

        private void cropControllerCrop_AfterSubmit(object sender)
        {
            var tmpPostCard = treeList1.FocusedNode.Tag as PostCard;

            if (tmpPostCard != null)
            {
                tmpPostCard.processStatus = "已提交";
                treeList1.FocusedNode.SetValue("status", tmpPostCard.processStatus);
                if (splashScreenManager1.IsSplashFormVisible)
                {
                    splashScreenManager1.CloseWaitForm();
                }
                Application.DoEvents();
                if (needProcess.Count > 0)
                {
                    var a = needProcess.Dequeue();
                    var tmpNextPostCard = a.Tag as PostCard;
                    while (tmpNextPostCard == null && needProcess.Count > 0)
                    {
                    }
                    treeList1.SetFocusedNode(a);
                }
                else
                {
                    var nextVisibleNode = treeList1.FocusedNode.NextVisibleNode;
                    if (nextVisibleNode != null)
                    {
                        treeList1.FocusedNode = nextVisibleNode;
                    }
                    else if (treeList1.FocusedNode.ParentNode != null)
                    {
                        treeList1.FocusedNode = treeList1.FocusedNode.ParentNode;
                    }
                }
            }
        }

        private void cropControllerCrop_BeforeSubmit(object sender)
        {
            if ("已修改".Equals(treeList1.FocusedNode.GetValue("status")))
            {
                treeList1.FocusedNode.SetValue("status", "正在提交");
                splashScreenManager1.ShowWaitForm();
            }
            Application.DoEvents();
        }
    }


    public class TestTreeList
    {
        public string name { get; set; }
        public string status { get; set; }

        public List<TestTreeList> tree { get; set; }
    }
}