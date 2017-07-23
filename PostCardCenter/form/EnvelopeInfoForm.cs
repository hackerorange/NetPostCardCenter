using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Common.Logging;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraLayout.Utils;
using PostCardCenter.Properties;
using soho.domain;
using soho.helper;
using soho.webservice;

namespace PostCardCenter.form
{
    public partial class EnvelopeInfoForm : XtraForm
    {
        private readonly DirectoryInfo _directoryInfo;
        public ILog Logger = LogManager.GetLogger("");


        public EnvelopeInfoForm()
        {
            InitializeComponent();
            envelopeSizeSelect.Properties.DataSource = SohoInvoker.getProductSizeTemplateList();
            PostCardFrontStyleGridLookUpEdit.DataSource = envelopeFrontStyle.Properties.DataSource =
                SohoInvoker.GetFrontStyleTemplateList();

            SohoInvoker.GetBackStyleTemplateList(
                success =>
                {
                    postCardBackStyleGridLookUpEdit.DataSource = envelopeBackStyle.Properties.DataSource = success;
                }, errorMessage => { XtraMessageBox.Show(errorMessage); });
        }

        public EnvelopeInfoForm(Order order, Envelope envelope) : this()
        {
            this.envelope = envelope;
            this.order = order;
            envelopeDetailGridView.DataSource = envelope.postCards;

            envelopePaperName.EditValue = envelope.paperName;
            envelopeFrontStyle.EditValue = envelope.frontStyle;
            envelopeFrontStyle.Text = envelope.frontStyle;
            orderTaobaoIdTextEdit.Text = order.taobaoId;
            orderUrgentCheckEdit.Checked = order.urgent;
            envelopeDoubleSideCheckBox.Checked = envelope.doubleSide;

            envelopeProductHeight.EditValue = envelope.productHeight;

            envelopeProductWidth.EditValue = envelope.productWidth;
        }

        public EnvelopeInfoForm(DirectoryInfo directoryInfo) : this()
        {
            _directoryInfo = directoryInfo;
        }

        public Envelope envelope { get; set; }
        public Order order { get; set; }

        private void EnvelopeInfoForm_Load(object sender, EventArgs e)
        {
            if (envelope == null && _directoryInfo == null)
            {
                DialogResult = DialogResult.Abort;
                Close();
            }

            dxValidationProvider1.SetValidationRule(envelopePaperName,
                new ConditionValidationRule
                {
                    ConditionOperator = ConditionOperator.IsNotBlank,
                    ErrorText = "请输入纸张名称"
                });
            dxValidationProvider1.SetValidationRule(orderTaobaoIdTextEdit,
                new ConditionValidationRule
                {
                    ConditionOperator = ConditionOperator.IsNotBlank,
                    ErrorText = "请输入客户淘宝ID"
                });
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            envelopeDetailGridView.RefreshDataSource();
        }

        private void envelopeProductWidth_EditValueChanged(object sender, EventArgs e)
        {
            envelope.productWidth = (int) envelopeProductWidth.Value;
        }

        private void envelopeProductHeight_EditValueChanged(object sender, EventArgs e)
        {
            envelope.productHeight = (int) envelopeProductHeight.Value;
        }

        private void envelopePaperName_EditValueChanged(object sender, EventArgs e)
        {
            envelope.paperName = envelopePaperName.Text;
        }

        private void envelopeSizeSelect_EditValueChanged(object sender, EventArgs e)
        {
            var size = envelopeSizeSelect.EditValue as ProductSize;
            if (size == null) return;
            envelopeProductHeight.EditValue = size.productHeight;
            envelopeProductWidth.EditValue = size.productWidth;
        }

        private void envelopeBackStyle_EditValueChanged(object sender, EventArgs e)
        {
            var backStyleInfo = envelopeBackStyle.EditValue as BackStyleInfo;
            if (backStyleInfo == null) return;
            //重置所有明信片的样式为选中的样式

            if (envelopeBackStyle.Focused)
            {
                envelope.backStyle = backStyleInfo.name;
                foreach (var envelopePostCard in envelope.postCards)
                {
                    envelopePostCard.backStyle = backStyleInfo.name;
                    envelopePostCard.backFileInfo = null;
                    envelopePostCard.backFileId = backStyleInfo.fileId;
                    envelopePostCard.customerBackStyle = false;
                }
            }
            gridView1.RefreshData();
        }

        private void envelopeFrontStyle_EditValueChanged(object sender, EventArgs e)
        {
            if (envelopeFrontStyle.EditValue == null) return;
            envelope.frontStyle = envelopeFrontStyle.EditValue as string;
            if (envelopeFrontStyle.Focused)
                envelope.postCards.ForEach(postCard => postCard.frontStyle = envelope.frontStyle);
        }

        private void envelopePostCardCopyEdit_EditValueChanged(object sender, EventArgs e)
        {
            envelope.postCards.ForEach(postCard => postCard.copy = (int) envelopePostCardCopyEdit.Value);
        }

        private void postCardCountEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (envelope.postCards.Exists(postCard => !postCard.isImage))
            {
                XtraMessageBox.Show("请删除明信片列表中的非图片条目，如果文件识别判断错误，请手工修改文件后，点击刷新按钮重新加载");
                return;
            }
            var count = (int) postCardCountEdit.Value;
            var copy = count / envelope.postCards.Count;
            var tmpCopy = count % envelope.postCards.Count;
            for (var i = 0; i < envelope.postCards.Count; i++)
            {
                if (i < tmpCopy)
                {
                    envelope.postCards[i].copy = copy + 1;
                    continue;
                }
                envelope.postCards[i].copy = copy;
            }
            envelopeDetailGridView.RefreshDataSource();
        }


        private void gridView1_FocusedRowChanged(object sender,
            FocusedRowChangedEventArgs e)
        {
        }

        private void orderTaobaoIdTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            order.taobaoId = orderTaobaoIdTextEdit.Text;
        }

        private void orderUrgentCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            order.urgent = orderUrgentCheckEdit.Checked;
        }

        private void envelopeDoubleSideCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem9.Visibility = layoutControlItem16.Visibility =
                envelopeDoubleSideCheckBox.Checked ? LayoutVisibility.Always : LayoutVisibility.Never;
            envelope.doubleSide = envelopeDoubleSideCheckBox.Checked;
        }

        private void envelopeSubmit_Click(object sender, EventArgs e)
        {
            if (!dxValidationProvider1.Validate()) return;
            foreach (var envelopePostCard in envelope.postCards)
            {
                if (!envelopePostCard.isImage)
                {
                    XtraMessageBox.Show("存在不是图片的明信片，请删除或手动");
                    return;
                }
                if (envelopePostCard.frontStyle == null ||
                    envelope.doubleSide && envelopePostCard.backStyle == null)
                {
                    XtraMessageBox.Show("存在没有设置正面或反面样式的明信片，请检查");
                    return;
                }
                if (envelopePostCard.copy > 0) continue;
                XtraMessageBox.Show("存在打印张数不正确的明信片，请检查");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void removeSelectedPostCardButton_Click(object sender,
            ButtonPressedEventArgs e)
        {
            var postCard = gridView1.GetFocusedRow() as PostCard;
            if (postCard != null)
                envelope.postCards.Remove(postCard);
        }

        private void repositoryItemButtonEdit3_ButtonClick(object sender,
            ButtonPressedEventArgs e)
        {
            var tmpSender = sender as ButtonEdit;

            var focusedValue = gridView1.GetFocusedRow() as PostCard;
            //如果当前没有选中的行,或者选中行的文件没有父目录（一般不会），直接返回
            if (focusedValue == null || focusedValue.fileInfo.Directory == null) return;

            //打开文件选择框
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = focusedValue.fileInfo.Directory.FullName
            };
            //如果取消选择文件，直接返回
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            //要上传的文件信息
            var backFileInfo = new FileInfo(openFileDialog.FileName);
            //获取文件MD5
            var md5 = backFileInfo.getMd5();
            //上传文件
            SohoInvoker.Upload(backFileInfo, res =>
            {
                focusedValue.backFileInfo = backFileInfo;
                focusedValue.backFileId = md5;
                focusedValue.customerBackStyle = true;
                gridView1.RefreshData();
                //问价
                if (tmpSender != null) tmpSender.Text = backFileInfo.Name;
            });
        }

        private void PostCardBackStyleGridLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            var b = sender as GridLookUpEdit;
            if (b == null) return;
            var styleInfo = b.GetSelectedDataRow() as BackStyleInfo;
            var focusedRow = gridView1.GetFocusedRow() as PostCard;
            if (focusedRow == null) return;
            focusedRow.customerBackStyle = false;
            if (styleInfo != null) focusedRow.backFileId = styleInfo.fileId;
            envelope.backStyle = focusedRow.backStyle;
            focusedRow.backFileInfo = null;
            gridView1.RefreshData();
        }

        private void backStyleCustomerButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = envelope.directory.FullName
            }; //如果选择了文件
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var backFileInfo = new FileInfo(openFileDialog.FileName);
                //获取文件MD5
                var md5 = backFileInfo.getMd5();
                //上传文件
                SohoInvoker.Upload(backFileInfo, resp =>
                {
                    envelope.backStyle = "自定义";
                    foreach (var envelopePostCard in envelope.postCards)
                    {
                        envelopePostCard.backStyle = "自定义";
                        envelopePostCard.backFileInfo = backFileInfo;
                        envelopePostCard.backFileId = md5;
                        envelopePostCard.customerBackStyle = true;
                    }
                    gridView1.RefreshData();
                }, error => { XtraMessageBox.Show(error); });
            }
        }

        private void EnvelopeInfoForm_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (_directoryInfo != null)
            {
                var fileInfos = _directoryInfo.GetFiles();
                if (fileInfos.Length == 0) return;
                envelope = new Envelope
                {
                    directory = _directoryInfo,
                    postCards = new List<PostCard>(),
                    productHeight = 100,
                    productWidth = 148,
                    doubleSide = !_directoryInfo.FullName.Contains("[单面]")
                };


                Text = envelope.directory.FullName;
                envelopeDetailGridView.DataSource = envelope.postCards;

                foreach (var fileInfo in fileInfos)
                {
                    if (Resources.notPostCardFileExtension.Contains(fileInfo.Extension.ToLower()))
                        continue;
                    var postCard = new PostCard
                    {
                        copy = 1,
                        fileInfo = fileInfo,
                        isImage = true
                    };
                    //向明信片集合中添加此文件

                    var md5 = postCard.fileInfo.getMd5();
                    envelope.postCards.Add(postCard);
                    //如果文件不存在服务器

                    //上传文件
                    SohoInvoker.Upload(postCard.fileInfo, standardResponse =>
                    {
                        if (standardResponse)
                        {
                            postCard.isImage = SohoInvoker.IsImageFile(md5);
                            postCard.fileId = md5;
                            postCard.fileName = postCard.fileInfo.Name;

                            envelopeDetailGridView.RefreshDataSource();
                            Application.DoEvents();
                        }
                        else
                        {
                            XtraMessageBox.Show("文件上传失败");
                        }
                    }, error => { XtraMessageBox.Show(error); });
                    Application.DoEvents();
                }
            }
        }
    }
}