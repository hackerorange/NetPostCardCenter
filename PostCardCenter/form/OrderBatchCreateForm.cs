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

                //                string pattern = "--(.+?)--";
                //                string replacement = "$1";
                //                string input = "He said--decisively--that the time--whatever time it was--had come.";
                //                foreach (Match match in Regex.Matches(input, pattern))
                //                {
                //                    string result = match.Result(replacement);
                //                    Console.WriteLine(result);
                //                }


                var tmpOrder = new OrderInfo
                {
                    Directory = directory,
                    Urgent = directory.FullName.Contains("[加急]"),
                    TaobaoId = customerTaobaoId,
                    OrderStatus = "待设置"
                };
                OrderList.Add(tmpOrder);


                //foreach (var info in directory.GetDirectories())
                //{
                //    var envelopeInfoForm = new EnvelopeInfoForm(info)
                //    {
                //        order = tmpOrder
                //    };
                //    if (envelopeInfoForm.ShowDialog(this) != DialogResult.OK) continue;
                //    if (tmpOrder.Envelopes == null)
                //    {
                //        tmpOrder.Envelopes = new List<EnvelopeInfo>();
                //    }
                //    tmpOrder.Envelopes.Add(envelopeInfoForm.envelope);
                //}
                //if (tmpOrder.hasEnvelope())
                //{
                //    orderList.Add(tmpOrder);
                //}
                gridControl1.DataSource = OrderList;
                gridControl1.RefreshDataSource();
            }
        }

        private void OrderBatchCreateForm_Load(object sender, EventArgs e)
        {
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            var a = gridView1.GetFocusedRow() as OrderInfo;
            if (a == null) return;
            switch (a.OrderStatus)
            {
                case "待设置":

                    foreach (var info in a.Directory.GetDirectories())
                    {
                        var envelopeInfoForm = new EnvelopeInfoForm(info)
                        {
                            order = a
                        };
                        if (envelopeInfoForm.ShowDialog(this) != DialogResult.OK) continue;
                        if (a.Envelopes == null)
                        {
                            a.Envelopes = new List<EnvelopeInfo>();
                        }
                        a.Envelopes.Add(envelopeInfoForm.envelope);
                    }
                    if (a.hasEnvelope())
                    {
                        if (XtraMessageBox.Show("明信片是否已经设置完成", "设置完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            a.OrderStatus = "待提交";
                            //上传此明信片订单下的所有明信片图像
                            a.Envelopes.ForEach(EnvelopeInfo =>
                            {
                                if (EnvelopeInfo.PostCardCount > 0)
                                {
                                    EnvelopeInfo.PostCards.ForEach(PostCardInfo =>
                                    {
                                        PostCardInfo.FileInfo.Upload(
                                            success: result =>
                                            {
                                                PostCardInfo.FileId = result;
                                                PostCardInfo.FileName = PostCardInfo.FileInfo.Name;
                                            }, 
                                            failure: message => 
                                            {
                                                XtraMessageBox.Show("文件上传失败！" + message);
                                            });
                                    });
                                }
                            });
                        }
                    }
                    break;
                case "待提交":
                    if (a.hasEnvelope())
                    {
                        if (a.Envelopes.Count == 1)
                        {
                            new EnvelopeInfoForm(a, a.Envelopes[0]).ShowDialog(this);
                        }
                        else
                        {
                            new OrderInfoForm(a).ShowDialog();
                        }
                    }
                    break;
                default:
                    XtraMessageBox.Show("明信片集合设置错误");
                    break;
            }
        }


        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var order = gridView1.GetFocusedRow() as OrderInfo;
            if (order == null) return;
            OrderList.Remove(order);
            gridControl1.RefreshDataSource();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.Desktop,
                ShowNewFolderButton = false
            };


            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

            var directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
            foreach (var directory in directoryInfo.GetDirectories(@"*[订单]*"))
            {
                //如果订单列表中已经存在此订单，则跳过
                if (OrderList != null && OrderList.Exists(order => order.Directory.FullName.Equals(directory.FullName)))
                    continue;
                var match = new Regex(@"\[TID=.+]").Match(directoryInfo.FullName);
                var customerTaobaoId = "";
                if (match.Success)
                {
                    customerTaobaoId = match.Result("$1");
                }

                var tmpOrder = new OrderInfo
                {
                    Directory = directory,
                    Urgent = directory.FullName.Contains("[加急]"),
                    TaobaoId = customerTaobaoId
                };
                foreach (var info in directory.GetDirectories())
                {
                    var envelopeInfoForm = new EnvelopeInfoForm(info)
                    {
                        order = tmpOrder
                    };
                    if (envelopeInfoForm.ShowDialog(this) != DialogResult.OK) continue;
                    if (tmpOrder.Envelopes == null)
                    {
                        tmpOrder.Envelopes = new List<EnvelopeInfo>();
                    }
                    tmpOrder.Envelopes.Add(envelopeInfoForm.envelope);
                }
                if (tmpOrder.hasEnvelope())
                {
                    if (OrderList == null)
                    {
                        OrderList = new List<OrderInfo>();
                    }
                    OrderList.Add(tmpOrder);
                }
                gridControl1.DataSource = OrderList;
                gridControl1.RefreshDataSource();
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderList.ForEach(order =>
            {
                if (order.OrderStatus != "待提交") return;
                if (order.IsAllPostCardUpload())
                {
                    order.OrderStatus = "正在提交";
                    WebServiceInvoker.SubmitPostCardList(order.PrepareSubmitRequest(), response =>
                    {
                        //如果操作成功，移除此项目
                        //OrderList.Remove(order);
                        order.OrderStatus = "订单已提交";
                        //if (OrderList.Count == 0)
                        //{
                        //    DialogResult = DialogResult.OK;
                        //}
                    }, error =>
                    {
                        XtraMessageBox.Show("订单提交失败");
                        order.OrderStatus = "待提交";
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
                            ProcessStatus = "待设置"
                        };
                        OrderList.Add(OrderInfo);
                    }
                    XtraMessageBox.Show(s);
                }
            }

            gridControl1.RefreshDataSource();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }
    }
}