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
            var directoryInfos = order.Directory.GetDirectories();

            for (var index = 0; index < directoryInfos.Length; index++)
            {
                var directoryInfo = order.Directory.GetDirectories()[index];
                if (order.Envelopes == null)
                {
                    order.Envelopes = new List<EnvelopeInfo>();
                } //如果此文件夹已经存在，则不再处理          
                if (order.Envelopes.Exists(orderEnvelope => orderEnvelope.Directory.FullName == directoryInfo.FullName))
                {
                    Console.WriteLine(@"此文件夹已经存在");
                    continue;
                }

                var envelopeInfoForm = new EnvelopeInfoForm(directoryInfo);
                //如果没有此目录下没有文件，跳到下一个循环
                if (envelopeInfoForm.envelope == null) continue;
                envelopeInfoForm.order = order;
                //如果弹出的窗口返回结果不为OK，跳到下一个训话
                if (envelopeInfoForm.ShowDialog() != DialogResult.OK) continue;
                //向此集合中添加明信片集合
                order.Envelopes.Add(envelopeInfoForm.envelope);
                //向明信片集合中链接此订单
                gridControl1.DataSource = order.Envelopes;
                gridControl1.RefreshDataSource();
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
            new EnvelopeInfoForm(order, envelope).ShowDialog(this);
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