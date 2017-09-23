using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using PostCardCenter.helper;
using soho.domain;
using soho.webservice;
using postCardCenterSdk.sdk;
using postCardCenterSdk.request.order;
using soho.helper;

namespace PostCardCenter.form
{
    public partial class OrderBatchCreateForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public List<Order> orderList { get; set; }


        public OrderBatchCreateForm()
        {
            InitializeComponent();
        }

        private void CreateOrderFromDesktopButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            var directoryInfo =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            if (orderList == null)
            {
                orderList = new List<Order>();
            }
            var directoryInfos = directoryInfo.GetDirectories(@"*[订单]*");
            foreach (var directory in directoryInfo.GetDirectories(@"*[订单]*"))
            {
                //如果订单列表中已经存在此订单，则跳过
                if (orderList.Exists(order => order.directory.FullName.Equals(directory.FullName)))
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


                var tmpOrder = new Order
                {
                    directory = directory,
                    urgent = directory.FullName.Contains("[加急]"),
                    taobaoId = customerTaobaoId
                };
                foreach (var info in directory.GetDirectories())
                {
                    var envelopeInfoForm = new EnvelopeInfoForm(info)
                    {
                        order = tmpOrder
                    };
                    if (envelopeInfoForm.ShowDialog(this) != DialogResult.OK) continue;
                    if (tmpOrder.envelopes == null)
                    {
                        tmpOrder.envelopes = new List<Envelope>();
                    }
                    tmpOrder.envelopes.Add(envelopeInfoForm.envelope);
                }
                if (tmpOrder.hasEnvelope())
                {
                    orderList.Add(tmpOrder);
                }
                gridControl1.DataSource = orderList;
                gridControl1.RefreshDataSource();
            }
        }

        private void OrderBatchCreateForm_Load(object sender, EventArgs e)
        {
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            var a = gridView1.GetFocusedRow() as Order;
            if (a == null) return;
            if (gridView1.GetFocusedRow() == null) return;
            if (a.hasEnvelope())
            {
                if (a.envelopes.Count == 1)
                {
                    new EnvelopeInfoForm(a, a.envelopes[0]).ShowDialog(this);
                }
                else
                {
                    new OrderInfoForm(a).ShowDialog();
                }
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var order = gridView1.GetFocusedRow() as Order;
            if (order == null) return;
            orderList.Remove(order);
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
                if (orderList != null && orderList.Exists(order => order.directory.FullName.Equals(directory.FullName)))
                    continue;
                var match = new Regex(@"\[TID=.+]").Match(directoryInfo.FullName);
                var customerTaobaoId = "";
                if (match.Success)
                {
                    customerTaobaoId = match.Result("$1");
                }

                var tmpOrder = new Order
                {
                    directory = directory,
                    urgent = directory.FullName.Contains("[加急]"),
                    taobaoId = customerTaobaoId
                };
                foreach (var info in directory.GetDirectories())
                {
                    var envelopeInfoForm = new EnvelopeInfoForm(info)
                    {
                        order = tmpOrder
                    };
                    if (envelopeInfoForm.ShowDialog(this) != DialogResult.OK) continue;
                    if (tmpOrder.envelopes == null)
                    {
                        tmpOrder.envelopes = new List<Envelope>();
                    }
                    tmpOrder.envelopes.Add(envelopeInfoForm.envelope);
                }
                if (tmpOrder.hasEnvelope())
                {
                    if (orderList == null)
                    {
                        orderList = new List<Order>();
                    }
                    orderList.Add(tmpOrder);
                }
                gridControl1.DataSource = orderList;
                gridControl1.RefreshDataSource();
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            orderList.ForEach(order =>
            {
                WebServiceInvoker.SubmitPostCardList(order.PrepareSubmitRequest(), response =>
                {
                    //如果操作成功，移除此项目
                    orderList.Remove(order);
                    if (orderList.Count == 0)
                    {
                        DialogResult = DialogResult.OK;
                    }
                }, error => { XtraMessageBox.Show(error); });
            });
        }
    }
}