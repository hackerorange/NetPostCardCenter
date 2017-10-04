using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraPrinting.Native;
using PostCardCenter.helper;
using soho.domain;
using soho.translator;
using PostCardCenter.constant;

namespace PostCardCenter.form
{
    public partial class OrderInfoForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public OrderInfo order { get; set; }
        private IDictionary<string, string> iDictionay { get; set; }

        public OrderInfoForm()
        {
            InitializeComponent();
        }

        public OrderInfoForm(OrderInfo order) : this()
        {
            this.order = order;
            if (order.hasEnvelope())
            {
                gridControl1.DataSource = order.Envelopes;
            }
        }


        private void batchCreateFromDesktop_ItemClick(object sender, ItemClickEventArgs e)
        {
            getEnvelopeFromOrderDictionary();
            XtraMessageBox.Show("订单刷新完成");
        }

        private void getEnvelopeFromOrderDictionary()
        {
            
            if (order == null || order.Directory == null) return;

            List<DirectoryInfo> tmpDirectories = new List<DirectoryInfo>();
            var directoryInfos = order.Directory.GetDirectories();
            //将根目录添加到集合中
            if (order.Directory.GetFiles().Length > 0)
            {
                tmpDirectories.Add(order.Directory);
            }
            //将第一层子文件夹中的所有文件添加到集合中
            foreach (var info in order.Directory.GetDirectories())
            {
                tmpDirectories.Add(info);
            }
            //遍历目录
            foreach (var info in tmpDirectories)
            {
                EnvelopeInfo envelope = new EnvelopeInfo
                {
                    OrderInfo = order,
                    Directory = info
                };
                //显示此明信片集合详情页
                if (new EnvelopeInfoForm(envelope).ShowDialog(this) != DialogResult.OK) continue;
                order.Envelopes.Add(envelope);
            }
            if (order.hasEnvelope())
            {
                if (XtraMessageBox.Show("明信片是否已经设置完成", "设置完成", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    order.OrderStatus = "待上传";
                    //上传此明信片订单下的所有明信片图像
                    order.Envelopes.ForEach(EnvelopeInfo =>
                    {
                        if (EnvelopeInfo.PostCardCount > 0)
                        {
                            EnvelopeInfo.PostCards.ForEach(PostCardInfo =>
                            {
                                //上传图像
                                uploadPostCard(PostCardInfo, order);
                            });
                        }
                    });
                }
            }
        }

        private void uploadPostCard(PostCardInfo PostCardInfo, OrderInfo order)
        {
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
                    if (order.FileUploadPercent == 100)
                    {
                        order.OrderStatus = "待提交";
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

        private void OrderInfoForm_Load(object sender, EventArgs e)
        {
            if (order == null)
            {
                DialogResult = DialogResult.Abort;
                Close();
            }
            var notEmptyValidationRule = new ConditionValidationRule
            {
                ConditionOperator = ConditionOperator.IsNotBlank,
                ErrorText = "请输入客户淘宝ID"
            };
            orderDirectoryTextEdit.Text = order.Directory.FullName;
            //显示淘宝信息
            orderTaobaoIdTextEdit.Text = order.TaobaoId;
            //设置是否加急状态
            orderIsUrgentCheckEdit.Checked = order.Urgent;
            orderInfoValidationProvider.SetValidationRule(orderTaobaoIdTextEdit, notEmptyValidationRule);
            if (!order.hasEnvelope())
            {
                getEnvelopeFromOrderDictionary();
            }
        }


        private void orderTaobaoIdTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            order.TaobaoId = orderTaobaoIdTextEdit.EditValue.ToString();
        }

        private void orderIsUrgentCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            order.Urgent = orderIsUrgentCheckEdit.Checked;
        }

        private void removeEnvelopeButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            var envelope = advBandedGridView1.GetFocusedRow() as EnvelopeInfo;
            if (envelope == null) return;
            order.Envelopes.Remove(envelope);
            gridControl1.RefreshDataSource();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
        }

        private void advBandedGridView1_DoubleClick(object sender, EventArgs e)
        {
            var envelope = advBandedGridView1.GetFocusedRow() as EnvelopeInfo;
            if (envelope == null) return;
            new EnvelopeInfoForm(envelope).ShowDialog(this);
        }

        private void saveButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!orderInfoValidationProvider.Validate()) return;
            if (!order.hasEnvelope())
            {
                XtraMessageBox.Show("此订单下没有明信片集合，请确认");
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void closeButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }
    }
}