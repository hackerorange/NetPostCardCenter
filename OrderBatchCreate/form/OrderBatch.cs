using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SystemSetting.backStyle.model;
using SystemSetting.size.constant;
using SystemSetting.size.form;
using SystemSetting.size.model;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraLayout;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.StyleFormatConditions;
using OrderBatchCreate.helper;
using OrderBatchCreate.model;
using OrderBatchCreate.Properties;
using OrderBatchCreate.translator.request;
using postCardCenterSdk.sdk;
using soho.domain;
using soho.domain.system;
using soho.helper;
using CellValueChangedEventArgs = DevExpress.XtraTreeList.CellValueChangedEventArgs;

namespace OrderBatchCreate.form
{
    public partial class OrderBatch : RibbonForm
    {
        public readonly List<OrderInfo> OrderInfos = new List<OrderInfo>();

        private readonly PostCardUploadWorker _postCardUploadWorker = new PostCardUploadWorker(100);

        public OrderBatch()
        {
            InitializeComponent();
            //envelopeSizeGridControl.DataSource = soho.domain.system.SystemConstant.ProductSizeList;
            //异步获取尺寸信息
            ProductSizeFactory.GetInstance().GetProductSizeListFromServer(success => { repositoryItemComboBox4.Items.AddRange(success); });
            BackStyleFactory.GetBackStyleFromServer(success => success.ForEach(backStyle => repositoryItemComboBox3.Items.Add(backStyle)));


//            repositoryItemComboBox3.Items.AddRange(SystemConstant.BackStyleList);
            //repositoryItemComboBox3.Items.Add()
            gridControl1.DataSource = OrderInfos;
            BatchStatus.StatusList.ForEach(createStatus =>
            {
                orderDetailListView.FormatRules.Add(new TreeListFormatRule
                {
                    ApplyToRow = true,
                    Column = statusColumn,
                    Rule = createStatus.GenerateRuleFormat()
                });
                gridView2.FormatRules.Add(new GridFormatRule()
                {
                    ApplyToRow = true,
                    Column = orderStatusColumn,
                    Rule = createStatus.GenerateRuleFormat()
                });
            });
            orderDetailListView.KeyFieldName = "Key";
            orderDetailListView.ParentFieldName = "Parent";
        }


        ~OrderBatch()
        {
            orderDetailListView.FormatRules.Clear();
        }

        /// <summary>
        ///     向订单明细列表中拖拽文件夹
        /// </summary>
        private void OrderDetailListView_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            foreach (var s in files)
                //如果存在此路径
                if (Directory.Exists(s))
                    AddNewOrder(s);
        }


        public void AddNewOrder(string s)
        {
            //如果此订单已经提交过
            if (File.Exists(s + "/hasSubmit.ini"))
            {
                XtraMessageBox.Show(s + "文件夹中的订单已经提交过");
                return;
            }

            if (OrderInfos.Exists(orderInfo => orderInfo.DirectoryInfo.FullName == s))
            {
                XtraMessageBox.Show(s + "文件夹中的订单已经在列表中，请勿重复提交！");
                return;
            }

            var tmpDirectoryInfo = new DirectoryInfo(s);

            var tmpOrderBasicInfoForm = new OrderBasicInfoForm(tmpDirectoryInfo);

            if (tmpOrderBasicInfoForm.ShowDialog(this) != DialogResult.OK)
            {
                XtraMessageBox.Show("取消了订单的创建，订单路径为" + s);
                return;
            }


            var tmpOrderInfo = new OrderInfo(tmpDirectoryInfo)
            {
                TaobaoId = tmpOrderBasicInfoForm.TaobaoId,
                Urgent = tmpOrderBasicInfoForm.Urgent
            };
            //追加新的订单
            OrderInfos.Add(tmpOrderInfo);
            Application.DoEvents();
            gridControl1.RefreshDataSource();
            AddNewEnvelope(tmpOrderInfo, tmpOrderInfo.DirectoryInfo);
            RefreshSubmitEnvelopeList();
        }

        private void AddNewEnvelope(OrderInfo orderInfo, DirectoryInfo directoryInfo, EnvelopeInfo parentEnvelopeInfo = null)
        {
            var orderFolderFile = directoryInfo.GetFiles();
            var tmpEnvelopeInfo = new EnvelopeInfo(orderInfo, parentEnvelopeInfo)
            {
                DirectoryInfo = directoryInfo,
                DoubleSide = !directoryInfo.FullName.Contains("单面")
            };
            orderInfo.EnvelopeInfoList.Add(tmpEnvelopeInfo);

            //将基准路径下的明信片添加到明信片集合中
            foreach (var fileInfo in orderFolderFile)
            {
                //AddEnvelopeInfo
                //如果非明信片文件扩展名集合中存在此文件路径，跳过到下一个
                if (Resources.notPostCardFileExtension.Contains(fileInfo.Extension)) continue;

                if (Resources.notPostCardFileExtension.Contains(fileInfo.Extension.ToLower()))
                    continue;

                if (fileInfo.Name.ToUpper().Contains("backgroundPicture".ToUpper()))
                {
                    if (tmpEnvelopeInfo.DoubleSide)
                    {
                        tmpEnvelopeInfo.BackStyle = new CustomerBackStyleInfo(fileInfo);
                        tmpEnvelopeInfo.FrontStyle = "D";
                        //此集合下的所有明信片都设置为D;
                        tmpEnvelopeInfo.PostCards.ForEach(postCard =>
                        {
                            postCard.FrontStyle = "D";
                            postCard.BackStyle = tmpEnvelopeInfo.BackStyle;
                        });
//                        fileInfo.Upload("自定义反面样式", false, fileId => { tmpEnvelopeInfo.BackStyle.FileId = fileId; }, message => { XtraMessageBox.Show("反面文件上传失败！"); });
                    }

                    continue;
                }

                var tmpPostCardInfo = new PostCardInfo(fileInfo, tmpEnvelopeInfo)
                {
                    BackStyle = tmpEnvelopeInfo.BackStyle
                };
                Application.DoEvents();
                //上传文件信息
                _postCardUploadWorker.Upload(tmpPostCardInfo, success => { timer1.Start(); });

                tmpEnvelopeInfo.PostCards.Add(tmpPostCardInfo);
                orderInfo.EnvelopeInfoList.Add(tmpPostCardInfo);
                orderDetailListView.RefreshDataSource();
                //展开所有
                if (gridView2.GetFocusedRow() == orderInfo)
                    orderDetailListView.ExpandAll();
            }


            //获取所有子文件夹
            var directories = directoryInfo.GetDirectories();
            //先递归处理子文件夹
            foreach (var tmpDirectoryInfo in directories)
            {
                Application.DoEvents();
                AddNewEnvelope(orderInfo, tmpDirectoryInfo, tmpEnvelopeInfo);
            }
        }


        private void OrderDetailListView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void OrderBatch_Load(object sender, EventArgs e)
        {
        }

        private void OrderDetailListView_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            SetImageIndex(orderDetailListView.DataSource as List<PostCardBasic>, orderDetailListView, null);

            //orderDetailListView.RefreshDataSource();
            if (e.Node == null) return;
//            var nodes = e.PostCardInfo.ParentNode == null ? orderDetailListView.Nodes : e.PostCardInfo.ParentNode.Nodes;
            if (!(e.Node.GetValue("Key") is PostCardBasic postCardBasic)) return;

            switch (postCardBasic)
            {
                case PostCardInfo tmpPostCardInfo:
                    paperNameColumn.OptionsColumn.AllowEdit = false;
                    paperNameColumn.OptionsColumn.ReadOnly = true;
                    productSizeColumn.OptionsColumn.AllowEdit = false;
                    if (tmpPostCardInfo.Parent is EnvelopeInfo tmpEnvelopeInfo)
                    {
                        envelopeSettingControl2.EnvelopeInfo = tmpEnvelopeInfo;
                        backStyleColumn.OptionsColumn.AllowEdit = tmpEnvelopeInfo.DoubleSide;
                    }

                    break;
                case EnvelopeInfo envelopeInfo:
                    //纸张名称只能是集合上设置
                    paperNameColumn.OptionsColumn.AllowEdit = true;
                    paperNameColumn.OptionsColumn.ReadOnly = false;
                    //成品尺寸只能是集合上设置
                    productSizeColumn.OptionsColumn.AllowEdit = true;
                    //paperNameColumn.OptionsColumn.ReadOnly = false;
                    envelopeSettingControl2.EnvelopeInfo = envelopeInfo;
                    backStyleColumn.OptionsColumn.AllowEdit = envelopeInfo.DoubleSide;
                    break;
            }

            //刷新订单数据
            gridView2.RefreshData();
        }

        /// <summary>
        ///     添加指定文件夹的订单
        /// </summary>
        private void BarButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tmpFolderDialog = new FolderBrowserDialog();
            if (tmpFolderDialog.ShowDialog(this) != DialogResult.OK) return;
            if (!Directory.Exists(tmpFolderDialog.SelectedPath)) return;
            var directoryInfo = new DirectoryInfo(tmpFolderDialog.SelectedPath);
            var directories = directoryInfo.GetDirectories();

            foreach (var t in directories)
                AddNewOrder(t.FullName);
        }


        /// <summary>
        ///     设置TreeList显示的图标
        /// </summary>
        /// <param name="tl">TreeList组件</param>
        /// <param name="node">当前结点，从根结构递归时此值必须=null</param>
        /// <param name="list">明信片列表</param>
        public static void SetImageIndex(List<PostCardBasic> list, TreeList tl, TreeListNode node)
        {
            if (node == null)
            {
                foreach (TreeListNode n in tl.Nodes)
                    SetImageIndex(list, tl, n);
            }
            else
            {
                if (!(node.GetValue("Key") is PostCardBasic postCardBasic)) return;

                node.ImageIndex = postCardBasic.ImageIndex;

                foreach (TreeListNode n in node.Nodes)
                    SetImageIndex(list, tl, n);
            }
        }


        private void OrderDetailListView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            orderDetailListView.RefreshDataSource();
            RefreshSubmitEnvelopeList();
            gridView2.RefreshData();
        }

        /// <summary>
        /// 刷新明信片提交列表
        /// </summary>
        private void RefreshSubmitEnvelopeList()
        {
            if ((gridView2.GetFocusedRow() is OrderInfo orderInfo))
            {
                gridControl2.DataSource = orderInfo.SubmitEnvelopeList;
                gridControl2.RefreshDataSource();

                envelopePrintInfoListView.FocusedRowHandle = 0;

                if (envelopePrintInfoListView.GetFocusedRow() is EnvelopeInfo envelopeInfo)
                {
                    envelopePrintInfo2.EnvelopeInfo = envelopeInfo;
                }
            }
        }

        private void RepositoryItemPopupContainerEdit1_QueryResultValue(object sender, QueryResultValueEventArgs e)
        {
        }

        private void GridControl1_Click(object sender, EventArgs e)
        {
        }

        private void GridView2_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //如果焦点值不是订单信息，直接返回
            if (!(gridView2.GetFocusedRow() is OrderInfo orderInfo)) return;

            orderDetailListView.DataSource = orderInfo.EnvelopeInfoList;
            //更新集合数据源
            gridControl2.DataSource = orderInfo.SubmitEnvelopeList;
            //更新打印
            if (envelopePrintInfoListView.GetFocusedRow() is EnvelopeInfo envelopeInfo)
                envelopePrintInfo2.EnvelopeInfo = envelopeInfo;

            orderDetailListView.ExpandAll();
            SetImageIndex(orderDetailListView.DataSource as List<PostCardBasic>, orderDetailListView, null);
            orderPathTextEdit.Text = orderInfo.DirectoryInfo.FullName;
        }


        private void RepositoryItemComboBox3_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            //if (!(orderDetailListView.DataSource is List<PostCardBasic> list)) return;
            var tmpNode = orderDetailListView.FocusedNode;
            if (tmpNode == null) return;

            if (!(tmpNode.GetValue("Key") is PostCardBasic postCardBasic)) return;
            //如果点击的事打开文件按钮
            if (e.Button.Index == 1)
            {
                var openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                var fileInfo = new FileInfo(openFileDialog.FileName);

                fileInfo.Upload(false, "自定义反面样式", success: imageUploadInfo =>
                {
                    if (imageUploadInfo.ImageAvailable)
                    {
                        var backStyleInfo = new BackStyleInfo
                        {
                            Name = "自定义",
                            FileId = imageUploadInfo.FileId
                        };
                        postCardBasic.BackStyle = backStyleInfo;
                        if (postCardBasic is EnvelopeInfo envelopeInfo)
                        {
                            envelopeInfo.PostCards.ForEach(postCard => postCard.BackStyle = backStyleInfo);
                        }

                        orderDetailListView.RefreshDataSource();
                    }
                    else
                    {
                        XtraMessageBox.Show("图像格式不正确，可能图像有损坏，建议使用Photoshop重新保存后再次上传");
                    }
                }, failure: msg => { XtraMessageBox.Show(msg); });
            }
        }

        private void OrderDetailListView_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Node == null) return;
            if (!(e.Node.GetValue("Key") is PostCardBasic postCardBasic)) return;
            //如果当前在份数选择那一列，返回，不进行后续操作
            if (e.Column == postCardCountColumn) return;

            if (e.Column == paperNameColumn) postCardBasic.PaperName = e.Value as string;
            if (e.Column == doubleSideColumn)
            {
                if ((bool) e.Value)
                {
                    postCardBasic.DoubleSide = true;
                    //如果为明信片集合，更新明信片集合中的所有明信片
                    if (postCardBasic is EnvelopeInfo tmpEnvelopeInfo)
                    {
                        tmpEnvelopeInfo.PostCards.ForEach(postCard => postCard.DoubleSide = true);
                    }
                }
                else
                {
                    postCardBasic.DoubleSide = false;
                    postCardBasic.BackStyle = null;
                    //如果为明信片集合，更新明信片集合中的所有明信片
                    if (postCardBasic is EnvelopeInfo tmpEnvelopeInfo)
                    {
                        tmpEnvelopeInfo.PostCards.ForEach(postCard =>
                        {
                            postCard.DoubleSide = false;
                            postCard.BackStyle = null;
                        });
                    }
                }
            }

            if (e.Column == backStyleColumn) postCardBasic.BackStyle = e.Value as BackStyleInfo;
            if (e.Column == productSizeColumn) postCardBasic.ProductSize = e.Value as PostSize;
            if (e.Column == frontStyleColumn)
            {
                postCardBasic.FrontStyle = e.Value as string;
                if (postCardBasic is EnvelopeInfo envelopeInfo)
                    envelopeInfo.PostCards.ForEach(postCard => postCard.FrontStyle = e.Value as string);
            }

            if (e.Column == backStyleColumn)
            {
                postCardBasic.BackStyle = e.Value as BackStyleInfo;
                if (postCardBasic is EnvelopeInfo envelopeInfo)
                    envelopeInfo.PostCards.ForEach(postCard => postCard.BackStyle = e.Value as BackStyleInfo);
            }

            orderDetailListView.RefreshDataSource();
        }


        private void RepositoryItemComboBox4_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            //if (!(orderDetailListView.DataSource is List<PostCardBasic> list)) return;
            var tmpNode = orderDetailListView.FocusedNode;

            if (tmpNode == null) return;
            // ReSharper disable once InvertIf
            if (e.Button.Index == 1)
            {
                var customerProductSizeForm = new CustomerProductSizeForm();
                // ReSharper disable once InvertIf
                if (customerProductSizeForm.ShowDialog() == DialogResult.OK)
                {
                    // ReSharper disable once InvertIf
                    if (tmpNode.GetValue("Key") is EnvelopeInfo envelopeInfo)
                        envelopeInfo.ProductSize = customerProductSizeForm.ProductSize;
                    orderDetailListView.RefreshDataSource();
                }
            }
        }

        private void GridControl2_Click(object sender, EventArgs e)
        {
        }

        private void GridView9_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (envelopePrintInfoListView.GetFocusedRow() is EnvelopeInfo envelopeInfo)
                envelopePrintInfo2.EnvelopeInfo = envelopeInfo;
        }

        private void BarButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tmpFolderDialog = new FolderBrowserDialog();
            if (tmpFolderDialog.ShowDialog(this) != DialogResult.OK) return;
            if (!Directory.Exists(tmpFolderDialog.SelectedPath)) return;
            AddNewOrder(tmpFolderDialog.SelectedPath);
        }

        private void RepositoryItemButtonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            var tmpNode = orderDetailListView.FocusedNode;

            if (tmpNode == null) return;
            var nodeValue = tmpNode.GetValue("Key");
            if (!(nodeValue is PostCardInfo tmpPostCardInfo)) return;
            switch (e.Button.Index)
            {
                //更换图片按钮
                case 0:
                    var fileDialog = new OpenFileDialog();
                    var directoryInfo = (tmpPostCardInfo.DirectoryInfo as FileInfo)?.Directory;
                    if (directoryInfo != null) fileDialog.InitialDirectory = directoryInfo?.FullName;

                    if (fileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        tmpPostCardInfo.DirectoryInfo = new FileInfo(fileDialog.FileName);
                        tmpPostCardInfo.FileId = null;
                        tmpPostCardInfo.IsImage = ((FileInfo) tmpPostCardInfo.DirectoryInfo).IsImage();
                        _postCardUploadWorker.Upload(tmpPostCardInfo, success => { timer1.Start(); });
                    }

                    break;
                //删除按钮
                case 1:
                    if (XtraMessageBox.Show("是否删除当前文件？", "文件删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) !=
                        DialogResult.OK) return;
                    if (tmpPostCardInfo.Parent is EnvelopeInfo tmpEnvelopeInfo)
                    {
                        //tmpEnvelopeInfo.Status
                        orderDetailListView.FocusedNode = tmpNode.PrevNode;
                        orderDetailListView.Nodes.Remove(tmpNode);
                        tmpEnvelopeInfo.PostCards.Remove(tmpPostCardInfo);
                        XtraMessageBox.Show(tmpEnvelopeInfo.OrderInfo.EnvelopeInfoList.Contains(tmpPostCardInfo)
                            .ToString());
                        tmpEnvelopeInfo.OrderInfo.EnvelopeInfoList.Remove(tmpPostCardInfo);
                    }

                    break;
                default:
                    XtraMessageBox.Show("此按钮暂未实现");
                    break;
            }

            orderDetailListView.RefreshDataSource();
        }

        private void BarButtonItem13_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            var tmpNode = orderDetailListView.FocusedNode;
            if (tmpNode == null) return;

            var nodeValue = tmpNode.GetValue("Key");
            if (!(nodeValue is PostCardInfo tmpPostCardInfo)) return;

            if (XtraMessageBox.Show("是否删除当前文件？", "文件删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) !=
                DialogResult.OK) return;
            if (tmpPostCardInfo.Parent is EnvelopeInfo tmpEnvelopeInfo)
            {
                //tmpEnvelopeInfo.Status
                orderDetailListView.FocusedNode = tmpNode.NextNode ?? tmpNode.PrevNode;

                orderDetailListView.Nodes.Remove(tmpNode);
                tmpEnvelopeInfo.PostCards.Remove(tmpPostCardInfo);
                tmpEnvelopeInfo.OrderInfo.EnvelopeInfoList.Remove(tmpPostCardInfo);
            }
        }

        private void BarButtonItem14_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            var tmpEnvelopeInfo = CurrentEnvelopeInfo;
            if (tmpEnvelopeInfo == null) return;

            var fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                InitialDirectory = tmpEnvelopeInfo.DirectoryInfo.FullName
            };
            if (fileDialog.ShowDialog(this) == DialogResult.OK)
                fileDialog.FileNames.ForEach(
                    filePath =>
                    {
                        var tmpPostCardInfo = new PostCardInfo(new FileInfo(filePath), tmpEnvelopeInfo);
                        tmpEnvelopeInfo.PostCards.Add(tmpPostCardInfo);
                        tmpEnvelopeInfo.OrderInfo.EnvelopeInfoList.Add(tmpPostCardInfo);
                        _postCardUploadWorker.Upload(tmpPostCardInfo, success => { timer1.Start(); });
                        //添加完成后，刷新列表
                        orderDetailListView.RefreshDataSource();
                    }
                );
        }

        private void BarButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tmpNode = orderDetailListView.FocusedNode;
            if (tmpNode == null)
            {
                return ;
            }

            var nodeValue = tmpNode.GetValue("Key");
            if (!(nodeValue is PostCardInfo tmpPostCardInfo)) return;

            if (XtraMessageBox.Show("是否替换当前图片？", "文件替换", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) !=
                DialogResult.OK) return;

            var fileDialog = new OpenFileDialog();
            var directoryInfo = (tmpPostCardInfo.DirectoryInfo as FileInfo)?.Directory;
            if (directoryInfo != null) fileDialog.InitialDirectory = directoryInfo?.FullName;

            if (fileDialog.ShowDialog(this) == DialogResult.OK)
            {
                tmpPostCardInfo.DirectoryInfo = new FileInfo(fileDialog.FileName);
                tmpPostCardInfo.FileId = null;
                tmpPostCardInfo.IsImage = ((FileInfo) tmpPostCardInfo.DirectoryInfo).IsImage();
                _postCardUploadWorker.Upload(tmpPostCardInfo, success => { timer1.Start(); });
            }
        }


        private EnvelopeInfo CurrentEnvelopeInfo
        {
            get
            {
                var tmpNode = orderDetailListView.FocusedNode;
                var nodeValue = tmpNode.GetValue("Key");
                switch (nodeValue)
                {
                    case EnvelopeInfo tmpEnvelopeInfo:
                        return tmpEnvelopeInfo;
                    case PostCardInfo tmPostCardInfo:
                        return tmPostCardInfo.Parent as EnvelopeInfo;
                }

                return null;
            }
        }

        private void BarButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            //获取当前明信片集合
            var tmpEnvelopeInfo = CurrentEnvelopeInfo;
            if (tmpEnvelopeInfo == null) return;

            var postCardList = tmpEnvelopeInfo.PostCards;
            if (postCardList.Count == 0)
            {
                XtraMessageBox.Show("当前明信片集合中没有明信片，无法设置！");
                return;
            }

            var copySet = new PostCardCopySet();
            if (copySet.ShowDialog(this) != DialogResult.OK) return;

            if (copySet.IsCopy)
            {
                postCardList.ForEach(tmpPostCard => { tmpPostCard.Copy = copySet.Number; });
            }
            else
            {
                if (postCardList.Count > 0)
                {
                    var everyCopy = copySet.Number / postCardList.Count;
                    var surplus = copySet.Number - everyCopy * postCardList.Count;
                    postCardList.ForEach(tmpPostCard => { tmpPostCard.Copy = everyCopy + (surplus-- > 0 ? 1 : 0); });
                }
            }

            orderDetailListView.RefreshDataSource();
        }

        private void BarButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!(gridView2.GetFocusedRow() is OrderInfo orderInfo)) return;
            OrderInfos.Remove(orderInfo);

            gridView2.RefreshData();

            //如果焦点值不是订单信息，直接返回
            if (!(gridView2.GetFocusedRow() is OrderInfo tmpOrderInfo))
            {
                orderDetailListView.DataSource = null;
                gridControl2.DataSource = null;
                envelopePrintInfo2.EnvelopeInfo = null;
                envelopePrintInfo2.EnvelopeInfo = null;
                envelopeSettingControl2.EnvelopeInfo = null;
            }
            else
            {
                orderDetailListView.DataSource = tmpOrderInfo.EnvelopeInfoList;
                gridControl2.DataSource = tmpOrderInfo.SubmitEnvelopeList;
                orderPathTextEdit.Text = tmpOrderInfo.DirectoryInfo.FullName;
                if (orderDetailListView.FocusedNode?.GetValue("key") is EnvelopeInfo envelopeInfo)
                {
                    envelopePrintInfo2.EnvelopeInfo = envelopeInfo;
                    envelopeSettingControl2.EnvelopeInfo = envelopeInfo;
                }
            }
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            OrderInfos.FindAll(orderInfo => orderInfo.Status == BatchStatus.OrderAlready).ForEach(orderInfo =>
            {
                if (orderInfo.EnvelopeInfoList == null) return;
                var flag = false;
                orderInfo.EnvelopeInfoList.ForEach(postCardBasic =>
                {
                    if (flag) return;
                    if (postCardBasic is EnvelopeInfo envelopeInfo &&
                        envelopeInfo.BackStyle is CustomerBackStyleInfo)
                    {
                        var tmpBackStyleCropForm = new BackStyleCropForm(envelopeInfo);
                        if (tmpBackStyleCropForm.ShowDialog() != DialogResult.OK)
                        {
                            flag = true;
                        }
                    }
                });
                if (flag) return;

                var prepareSubmitRequest = orderInfo.PrepareSubmitRequest();
                prepareSubmitRequest.SelfProcess = checkEdit1.Checked;

                WebServiceInvoker.GetInstance().SubmitOrderList(prepareSubmitRequest, succ =>
                {
                    var fileInfo = new FileInfo(orderInfo.DirectoryInfo.FullName + "/hasSubmit.ini");
                    fileInfo.Create();
                    if (!(gridView2.DataSource is List<OrderInfo> orderInfos)) return;
                    orderInfos.Remove(orderInfo);
                    gridView2.RefreshData();

                    if (!(gridView2.GetFocusedRow() is OrderInfo tmpOrderInfo))
                    {
                        orderDetailListView.DataSource = null;
                        gridControl2.DataSource = null;
                        envelopePrintInfo2.EnvelopeInfo = null;
                        envelopeSettingControl2.EnvelopeInfo = null;
                    }
                    else
                    {
                        orderDetailListView.DataSource = tmpOrderInfo.EnvelopeInfoList;
                        gridControl2.DataSource = tmpOrderInfo.SubmitEnvelopeList;
                        orderPathTextEdit.Text = tmpOrderInfo.DirectoryInfo.FullName;
                        var tmpNode = orderDetailListView.FocusedNode;
                        var nodeValue = tmpNode.GetValue("Key");
                        if (nodeValue is EnvelopeInfo tmpEnvelopeInfo)
                        {
                            envelopePrintInfo2.EnvelopeInfo = null;
                            envelopePrintInfo2.EnvelopeInfo = tmpEnvelopeInfo;
                            envelopeSettingControl2.EnvelopeInfo = tmpEnvelopeInfo;
                        }
                    }
                });
            });
        }

        private void BarButtonItem8_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            var tmpEnvelopeInfo = CurrentEnvelopeInfo;
            if (tmpEnvelopeInfo == null) return;
            var postCardInfos = tmpEnvelopeInfo.PostCards.FindAll(postCard => !postCard.IsUpload);
            postCardInfos.ForEach(postCard =>
                _postCardUploadWorker.Upload(postCard, success => { timer1.Start(); })
            );
        }

        private void BarButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            orderDetailListView.RefreshDataSource();
        }

        private void BarButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            new SizeManageForm().ShowDialog(this);

            ProductSizeFactory.GetInstance().GetProductSizeListFromServer(success =>
            {
                repositoryItemComboBox4.Items.Clear();
                repositoryItemComboBox4.Items.AddRange(success);
            });
        }

        private void EnvelopeSettingControl1_envelopeChanged(EnvelopeInfo envelopeInfo)
        {
            orderDetailListView.Refresh();
            gridView2.RefreshData();
            if (envelopePrintInfoListView.FocusedValue is EnvelopeInfo tmpEnvelopeInfo)
            {
                envelopePrintInfo2.EnvelopeInfo = tmpEnvelopeInfo;
            }

            RefreshSubmitEnvelopeList();
        }

        private void EnvelopeSettingControl2_Load(object sender, EventArgs e)
        {
        }

        private void EnvelopePrintInfo2_EnvelopeChanged(EnvelopeInfo envelopeInfo)
        {
            orderDetailListView.Refresh();
            gridView2.RefreshData();
            envelopePrintInfoListView.RefreshData();
        }

        private void BarButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (envelopeSettingControl2.EnvelopeInfo?.DirectoryInfo is DirectoryInfo directoryInfo)
            {
                System.Diagnostics.Process.Start(directoryInfo.FullName);
            }
        }

        private void BarButtonItem24_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (envelopeSettingControl2.EnvelopeInfo is EnvelopeInfo envelopeInfo)
            {
                envelopeInfo.PostCards.ForEach(postCardInfo => { _postCardUploadWorker.Upload(postCardInfo, success => { timer1.Start(); }); });
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            orderDetailListView.RefreshDataSource();
        }
    }
}