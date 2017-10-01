using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using PostCardCenter.helper;
using soho.domain;
using postCardCenterSdk.sdk;
using postCardCenterSdk.request.order;
using soho.translator;
using soho.translator.response;
using soho.translator.request;
using soho.helper;
using PostCardCenter.constant;

namespace PostCardCenter.form
{
    public partial class OrderBatchCreateForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public List<OrderInfo> OrderList { get; set; }


        public OrderBatchCreateForm()
        {
            InitializeComponent();
            OrderList = new List<OrderInfo>();
            gridControl1.DataSource = OrderList;
        }

        private void CreateOrderFromDesktopButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var directoryInfo =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            if (OrderList == null)
            {
                OrderList = new List<OrderInfo>();
            }
            var directoryInfos = directoryInfo.GetDirectories(@"*[订单]*");
            foreach (var directory in directoryInfo.GetDirectories(@"*[订单]*"))
            {
                //如果订单列表中已经存在此订单，则跳过
                if (OrderList.Exists(order => order.Directory.FullName.Equals(directory.FullName)))
                    continue;
                var match = new Regex("\\[TID=(.+)]|\\[tid=(.+)]").Match(directory.FullName);
                var customerTaobaoId = "";
                if (match.Success)
                {
                    customerTaobaoId = match.Result("$1");
                }
                var tmpOrder = new OrderInfo
                {
                    Directory = directory,
                    Urgent = directory.FullName.Contains("[加急]"),
                    TaobaoId = customerTaobaoId,
                    OrderStatus = "待设置"
                };
                OrderList.Add(tmpOrder);
                gridControl1.RefreshDataSource();
            }
        }

        private void OrderBatchCreateForm_Load(object sender, EventArgs e)
        {
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            var orderInfo = gridView1.GetFocusedRow() as OrderInfo;
            if (orderInfo == null) return;
            switch (orderInfo.OrderStatus)
            {
                case "待设置":

                    if (orderInfo.Directory.GetFiles().Length > 0)
                    {
                        var envelopeInfoForm = new EnvelopeInfoForm(orderInfo.Directory)
                        {
                            order = orderInfo
                        };
                        if (envelopeInfoForm.ShowDialog(this) == DialogResult.OK)
                        {
                            if (orderInfo.Envelopes == null)
                            {
                                orderInfo.Envelopes = new List<EnvelopeInfo>();
                            }
                            orderInfo.Envelopes.Add(envelopeInfoForm.envelope);
                        }
                    }
                    foreach (var info in orderInfo.Directory.GetDirectories())
                    {
                        var envelopeInfoForm = new EnvelopeInfoForm(info)
                        {
                            order = orderInfo
                        };
                        if (envelopeInfoForm.ShowDialog(this) != DialogResult.OK) continue;
                        if (orderInfo.Envelopes == null)
                        {
                            orderInfo.Envelopes = new List<EnvelopeInfo>();
                        }
                        orderInfo.Envelopes.Add(envelopeInfoForm.envelope);
                    }
                    if (orderInfo.hasEnvelope())
                    {
                        if (XtraMessageBox.Show("明信片是否已经设置完成", "设置完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            orderInfo.OrderStatus = "待上传";
                            //上传此明信片订单下的所有明信片图像
                            orderInfo.Envelopes.ForEach(EnvelopeInfo =>
                            {
                                if (EnvelopeInfo.PostCardCount > 0)
                                {
                                    EnvelopeInfo.PostCards.ForEach(PostCardInfo =>
                                    {
                                        //上传图像
                                        uploadPostCard(PostCardInfo, orderInfo);
                                    });
                                }
                            });
                        }
                    }
                    break;
                case "待提交":
                    if (orderInfo.hasEnvelope())
                    {
                        if (orderInfo.Envelopes.Count == 1)
                        {
                            new EnvelopeInfoForm(orderInfo, orderInfo.Envelopes[0]).ShowDialog(this);
                        }
                        else
                        {
                            new OrderInfoForm(orderInfo).ShowDialog();
                        }
                    }
                    break;
                default:
                    XtraMessageBox.Show("明信片集合设置错误");
                    break;
            }
        }


        private void uploadPostCard(PostCardInfo PostCardInfo,OrderInfo orderInfo) {


            //如果文件状态为非已上传
            if (PostCardInfo.FileUploadStat == PostCardFileUploadStatusEnum.BEFOREL_UPLOAD)
            {
                PostCardInfo.FileUploadStat = PostCardFileUploadStatusEnum.UPLOADING;
                PostCardInfo.FileInfo.Upload(
                success: result =>
                {
                    PostCardInfo.FileId = result;
                    PostCardInfo.FileName = PostCardInfo.FileInfo.Name;
                    PostCardInfo.FileUploadStat = PostCardFileUploadStatusEnum.AFTER_UPLOAD;
                                                //orderInfo.
                                                gridControl1.RefreshDataSource();
                    if (orderInfo.FileUploadPercent == 100)
                    {
                        orderInfo.OrderStatus = "待提交";
                    }
                    Application.DoEvents();
                },
                failure: message =>
                {
                    XtraMessageBox.Show("文件上传失败！" + message);
                    PostCardInfo.FileUploadStat = PostCardFileUploadStatusEnum.BEFOREL_UPLOAD;
                });
            }
        }



    


        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var order = gridView1.GetFocusedRow() as OrderInfo;
            if (order == null) return;
            OrderList.Remove(order);
            gridControl1.RefreshDataSource();
        }

        //private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    var folderBrowserDialog = new FolderBrowserDialog
        //    {
        //        RootFolder = Environment.SpecialFolder.Desktop,
        //        ShowNewFolderButton = false
        //    };


        //    if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

        //    var directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
        //    foreach (var directory in directoryInfo.GetDirectories(@"*[订单]*"))
        //    {
        //        //如果订单列表中已经存在此订单，则跳过
        //        if (OrderList != null && OrderList.Exists(order => order.Directory.FullName.Equals(directory.FullName)))
        //            continue;
        //        var match = new Regex(@"\[TID=.+]").Match(directoryInfo.FullName);
        //        var customerTaobaoId = "";
        //        if (match.Success)
        //        {
        //            customerTaobaoId = match.Result("$1");
        //        }

        //        var tmpOrder = new OrderInfo
        //        {
        //            Directory = directory,
        //            Urgent = directory.FullName.Contains("[加急]"),
        //            TaobaoId = customerTaobaoId
        //        };
        //        //如果此目录下存在文件，根据此目录下的文件，生成一个集合
        //        if (directory.GetFiles().Length > 0)
        //        {
        //            var envelopeInfoForm = new EnvelopeInfoForm(directory)
        //            {
        //                order = tmpOrder
        //            };
        //            if (envelopeInfoForm.ShowDialog(this) != DialogResult.OK) continue;
        //            if (tmpOrder.Envelopes == null)
        //            {
        //                tmpOrder.Envelopes = new List<EnvelopeInfo>();
        //            }
        //            tmpOrder.Envelopes.Add(envelopeInfoForm.envelope);
        //        }
        //        //遍历此目录下的子目录，生成一个集合
        //        foreach (var info in directory.GetDirectories())
        //        {
        //            var envelopeInfoForm = new EnvelopeInfoForm(info)
        //            {
        //                order = tmpOrder
        //            };
        //            if (envelopeInfoForm.ShowDialog(this) != DialogResult.OK) continue;
        //            if (tmpOrder.Envelopes == null)
        //            {
        //                tmpOrder.Envelopes = new List<EnvelopeInfo>();
        //            }
        //            tmpOrder.Envelopes.Add(envelopeInfoForm.envelope);
        //        }
        //        if (tmpOrder.hasEnvelope())
        //        {
        //            if (OrderList == null)
        //            {
        //                OrderList = new List<OrderInfo>();
        //            }
        //            OrderList.Add(tmpOrder);
        //        }
        //        gridControl1.DataSource = OrderList;
        //        gridControl1.RefreshDataSource();
        //    }
        //}

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderList.ForEach(order =>
            {
                if (order.OrderStatus != "待提交") return;
                if (order.IsAllPostCardUpload())
                {
                    order.OrderStatus = "正在提交";
                    gridControl1.RefreshDataSource();
                    Application.DoEvents();
                    WebServiceInvoker.SubmitPostCardList(order.PrepareSubmitRequest(), response =>
                    {
                        //如果操作成功，移除此项目
                        //OrderList.Remove(order);
                        order.OrderStatus = "订单已提交";
                        Application.DoEvents();
                        if (!OrderList.Exists(orderInfo => { return orderInfo.OrderStatus != "订单已提交"; }))
                        {
                            if (XtraMessageBox.Show("当前目录下的所有订单都已经提交完成，是否返回主界面查看订单详情？", "设置完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                DialogResult = DialogResult.OK;
                            }
                        };
                    }, error =>
                    {
                        XtraMessageBox.Show("订单提交失败");
                        order.OrderStatus = "待提交";
                        Application.DoEvents();
                    });
                }
                else
                {
                    XtraMessageBox.Show("订单" + order.TaobaoId + "存在没有上传的图片");
                }
            });
        }

        private void gridControl1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void gridControl1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
                foreach (String s in files)
                {
                    //如果存在此路径
                    if (Directory.Exists(s))
                    {
                        var OrderInfo = new OrderInfo
                        {
                            Directory = new DirectoryInfo(s),
                            OrderStatus = "待设置"
                        };
                        OrderList.Add(OrderInfo);
                        gridControl1.RefreshDataSource();
                    }
                }
            }

            gridControl1.RefreshDataSource();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //遍历订单
            OrderList.ForEach(OrderInfo =>
            {
                //如果订单状态为待上传
                if (OrderInfo.OrderStatus == "待上传")
                {
                    //遍历明信片集合
                    OrderInfo.Envelopes.ForEach(EnvelopeInfo =>
                    {
                        //遍历明信片集合中的所有明信片
                        EnvelopeInfo.PostCards.ForEach(PostCardInfo =>
                        {
                            //如果明信片为未提交状态
                            if (PostCardInfo.FileUploadStat == PostCardFileUploadStatusEnum.BEFOREL_UPLOAD)
                            {
                                //提交明信片
                                uploadPostCard(PostCardInfo, OrderInfo);
                            }
                        });
                    });
                }
            });
        }
    }
}