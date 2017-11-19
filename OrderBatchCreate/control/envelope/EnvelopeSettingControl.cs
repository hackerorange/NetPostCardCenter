using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemSetting.backStyle.model;
using SystemSetting.size.constant;
using SystemSetting.size.model;
using DevExpress.XtraEditors;
using OrderBatchCreate.form;
using OrderBatchCreate.model;

namespace OrderBatchCreate.control.envelope
{
    public partial class EnvelopeSettingControl : UserControl
    {
        public event EnvelopeEvent EnvelopeChanged;


        public EnvelopeSettingControl()
        {
            InitializeComponent();
            ProductSizeFactory.GetProductSizeListFromServer(success => success.ForEach(postSize => productSizeList.Items.Add(postSize)));
            BackStyleFactory.GetBackStyleFromServer(success => success.ForEach(backStyle => backStyleList.Items.Add(backStyle)));
        }

        private EnvelopeInfo _envelopeInfo;

        public EnvelopeInfo EnvelopeInfo
        {
            get => _envelopeInfo;
            set
            {
                _envelopeInfo = value;
                if (value == null)
                {
                    paperSettingGroup.Enabled = false;
                    productSettingGroup.Enabled = false;
                    productStyleGroup.Enabled = false;
                    envelopePath.EditValue = null;
                    layoutControlGroup3.Enabled = false;
                    return;
                }
                paperSettingGroup.Enabled = true;
                productSettingGroup.Enabled = true;
                productStyleGroup.Enabled = true;
                layoutControlGroup3.Enabled = true;
                envelopePath.EditValue = value.DirectoryInfo.FullName;
                //显示纸张名称
                if (string.IsNullOrEmpty(value.PaperName))
                {
                    paperNameList.SelectedIndex = -1;
                }
                else
                {
                    //设置纸张名称
                    for (var index = 0; index < paperNameList.Items.Count; index++)
                    {
                        var item = paperNameList.Items[index];
                        if (item.Equals(value.PaperName))
                        {
                            paperNameList.SelectedIndex = index;
                        }
                    }
                    paperName.EditValue = value.PaperName;
                }
                //显示成品尺寸
                if (value.ProductSize != null)
                {
                    envelopeProductSize.EditValue = value.ProductSize;
                    var flag = true;
                    for (var i = productSizeList.Items.Count - 1; i >= 0; i--)
                    {
                        if (productSizeList.Items[i] is PostSize tmpProductSize && (tmpProductSize.Width == value.ProductSize.Width && tmpProductSize.Height == _envelopeInfo.ProductSize.Height))
                        {
                            productSizeList.SelectedIndex = i;
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        productSizeList.SelectedIndex = -1;
                    }
                }
                frontStyle.EditValue = value.FrontStyle;
                //显示正面样式
                if (value.FrontStyle != null)
                {
                    if (string.IsNullOrEmpty(value.FrontStyle))
                    {
                        FrontStyleList.SelectedIndex = -1;
                    }
                    else
                    {
                        for (var i = FrontStyleList.Items.Count - 1; i >= 0; i--)
                        {
                            if (FrontStyleList.Items[i].Equals(value.FrontStyle))
                            {
                                FrontStyleList.SelectedIndex = i;
                            }
                        }
                    }
                }
                backStyleComboBox.EditValue = value.BackStyle;
                //反面样式非空
                if (value.BackStyle != null)
                {
                    var tmpFlag = true;
                    for (var i = backStyleList.Items.Count - 1; i >= 0; i--)
                    {
                        if (backStyleList.Items[i] is BackStyleInfo tmpBackStyleInfo && tmpBackStyleInfo.FileId == value.BackStyle.FileId)
                        {
                            backStyleList.SelectedIndex = i;
                            tmpFlag = false;
                            break;
                        }
                    }
                    if (tmpFlag)
                    {
                        backStyleList.SelectedIndex = -1;
                    }
                }
                //是否双面打印
                layoutControlGroup3.CustomHeaderButtons[0].Properties.Checked = value.DoubleSide;
                backStyleList.Enabled = value.DoubleSide;
            }
        }

        /// <summary>
        /// 选择的纸张名称变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaperNameList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_envelopeInfo == null) return;
            if (paperNameList.SelectedValue != null)
            {
                paperName.Text = paperNameList.SelectedValue.ToString();
                _envelopeInfo.PaperName = paperNameList.SelectedValue.ToString();
                _envelopeInfo.PostCards.ForEach(postCard => postCard.PaperName = _envelopeInfo.PaperName);
                EnvelopeChanged?.Invoke(_envelopeInfo);
            }
        }

        private void productSizeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_envelopeInfo == null) return;

            if (productSizeList.SelectedValue is PostSize postSize)
            {
                _envelopeInfo.ProductSize = new PostSize()
                {
                    Name = postSize.Name,
                    Width = postSize.Width,
                    Height = postSize.Height
                };
                _envelopeInfo.PostCards.ForEach(postCard => { postCard.ProductSize = _envelopeInfo.ProductSize; });
                envelopeProductSize.EditValue = _envelopeInfo.ProductSize;

                EnvelopeChanged?.Invoke(_envelopeInfo);
            }
        }

        private void FrontStyleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_envelopeInfo == null) return;
            if ((FrontStyleList.SelectedValue is string tmpFrontStyle))
            {
                _envelopeInfo.FrontStyle = tmpFrontStyle;
                _envelopeInfo.PostCards.ForEach(postCard => { postCard.FrontStyle = tmpFrontStyle; });
                frontStyle.EditValue = tmpFrontStyle;
            }
            EnvelopeChanged?.Invoke(_envelopeInfo);
        }


        private void BackStyleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_envelopeInfo == null) return;

            if (backStyleList.SelectedItem is BackStyleInfo backStyleInfo)
            {
                _envelopeInfo.BackStyle = backStyleInfo;
                _envelopeInfo.PostCards.ForEach(postCard => { postCard.BackStyle = backStyleInfo; });
                backStyleComboBox.EditValue = backStyleInfo;
                EnvelopeChanged?.Invoke(_envelopeInfo);
            }
        }

        private void EnvelopeProductSize_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //如果明信片信息为空，直接返回
            if (_envelopeInfo == null) return;
            var tmp = new CustomerProductSizeForm();
            if (tmp.ShowDialog(this) != DialogResult.OK) return;
            _envelopeInfo.ProductSize = tmp.ProductSize;
            envelopeProductSize.EditValue = tmp.ProductSize;
            EnvelopeChanged?.Invoke(_envelopeInfo);
        }

        private void LayoutControlGroup3_CustomButtonChecked(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            backStyleList.Enabled = e.Button.Properties.Checked;

            if (_envelopeInfo == null) return;
            _envelopeInfo.DoubleSide = true;
            backStyleComboBox.EditValue = _envelopeInfo.BackStyle;
            EnvelopeChanged?.Invoke(_envelopeInfo);
        }

        private void LayoutControlGroup3_CustomButtonUnchecked(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            backStyleList.Enabled = e.Button.Properties.Checked;

            if (_envelopeInfo == null) return;
            _envelopeInfo.DoubleSide = false;
            _envelopeInfo.BackStyle = null;
            _envelopeInfo.PostCards.ForEach(postCard => { postCard.BackStyle = null; });
            backStyleComboBox.EditValue = _envelopeInfo.BackStyle;
            EnvelopeChanged?.Invoke(_envelopeInfo);
        }

        private void BackStyleComboBox_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void BackStyleComboBox_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (_envelopeInfo == null || !_envelopeInfo.DoubleSide) return;
            var openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = _envelopeInfo.DirectoryInfo.FullName
            };
            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;
            var fileInfo = new FileInfo(openFileDialog.FileName);

            var tmpBackStyle = new CustomerBackStyleInfo(fileInfo);
            _envelopeInfo.BackStyle = tmpBackStyle;
            _envelopeInfo.PostCards.ForEach(postCardINfo => { postCardINfo.BackStyle = tmpBackStyle; });
            backStyleComboBox.EditValue = tmpBackStyle;
            EnvelopeChanged?.Invoke(_envelopeInfo);
        }

        private void EnvelopePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (_envelopeInfo?.DirectoryInfo is DirectoryInfo directoryInfo)
            {
                System.Diagnostics.Process.Start(directoryInfo.FullName);
            }
        }
    }
}