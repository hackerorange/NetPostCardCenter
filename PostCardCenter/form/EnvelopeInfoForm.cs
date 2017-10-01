using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Common.Logging;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraLayout.Utils;
using PostCardCenter.Properties;
using soho.domain;
using soho.translator;
using DevExpress.XtraGrid.Views.Grid;
using postCardCenterSdk.sdk;

namespace PostCardCenter.form
{
    public partial class EnvelopeInfoForm : XtraForm
    {
        private readonly DirectoryInfo _directoryInfo;
        public ILog Logger = LogManager.GetLogger("");


        public EnvelopeInfoForm()
        {
            InitializeComponent();
            //异步获取产品尺寸
            WebServiceInvoker.GetProductSizeTemplateList(success: response =>
            {
                List<PostSize> postSize = new List<PostSize>();


                response.ForEach(postCard =>
                {
                    postSize.Add(new PostSize()
                    {
                        Name = postCard.Name,
                        Height=postCard.Height,
                        Width=postCard.Width
                    });
                });

                gridControl1.DataSource = postSize;
            });
            //异步获取正面样式
            WebServiceInvoker.GetFrontStyleTemplateList(success: response =>
            {
                List<String> frontStyles = new List<String>();
                //获取正面集合，暂时只是字符串
                response.ForEach(frontStyle =>
                {
                    frontStyles.Add(frontStyle.Name);
                });
                PostCardFrontStyleGridLookUpEdit.DataSource = envelopeFrontStyle.Properties.DataSource = frontStyles;
            });
            //异步获取反面样式列表
            WebServiceInvoker.GetBackStyleTemplateList(success =>
            {
                List<BackStyleInfo> backStyleInfos = new List<BackStyleInfo>();
                success.ForEach(ba =>
                {
                    backStyleInfos.Add(new BackStyleInfo()
                    {
                        Name=ba.Name,
                        FileId=ba.FileId
                    });
                });
                postCardBackStyleGridLookUpEdit.DataSource = envelopeBackStyle.Properties.DataSource = backStyleInfos;
            }, errorMessage => 
            {
                XtraMessageBox.Show(errorMessage);
            });
        }

        public EnvelopeInfoForm(OrderInfo order, EnvelopeInfo envelope) : this()
        {
            this.envelope = envelope;
            this.order = order;
            envelopeDetailGridView.DataSource = envelope.PostCards;

            envelopePaperName.EditValue = envelope.PaperName;
            envelopeFrontStyle.EditValue = envelope.FrontStyle;
            envelopeFrontStyle.Text = envelope.FrontStyle;
            orderTaobaoIdTextEdit.Text = order.TaobaoId;
            orderUrgentCheckEdit.Checked = order.Urgent;
            envelopeDoubleSideCheckBox.Checked = envelope.DoubleSide;
            envelopeProductSize.EditValue = envelope.ProductSize.ToString();
            textEdit1.Text = envelope.Directory.FullName;
        }

        public EnvelopeInfoForm(DirectoryInfo directoryInfo) : this()
        {
            _directoryInfo = directoryInfo;
            textEdit1.Text = _directoryInfo.FullName;
        }


        public EnvelopeInfo envelope { get; set; }
        public OrderInfo order { get; set; }

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

        private void envelopePaperName_EditValueChanged(object sender, EventArgs e)
        {
            envelope.PaperName = envelopePaperName.Text;
        }

        private void envelopeBackStyle_EditValueChanged(object sender, EventArgs e)
        {
            var backStyleInfo = envelopeBackStyle.EditValue as BackStyleInfo;
            if (backStyleInfo == null) return;
            //重置所有明信片的样式为选中的样式

            if (envelopeBackStyle.Focused)
            {
                envelope.BackStyle = backStyleInfo.Name;
                foreach (var envelopePostCard in envelope.PostCards)
                {
                    envelopePostCard.BackStyle = backStyleInfo.Name;
                    envelopePostCard.BackFileInfo = null;
                    envelopePostCard.BackFileId = backStyleInfo.FileId;
                    envelopePostCard.CustomerBackStyle = false;
                }
            }
            gridView1.RefreshData();
        }

        private void envelopeFrontStyle_EditValueChanged(object sender, EventArgs e)
        {
            if (envelopeFrontStyle.EditValue == null) return;
            envelope.FrontStyle = envelopeFrontStyle.EditValue as string;
            if (envelopeFrontStyle.Focused)
                envelope.PostCards.ForEach(postCard => postCard.FrontStyle = envelope.FrontStyle);
        }

        private void envelopePostCardCopyEdit_EditValueChanged(object sender, EventArgs e)
        {
            envelope.PostCards.ForEach(postCard => postCard.Copy = (int)envelopePostCardCopyEdit.Value);
        }

        private void postCardCountEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (envelope.PostCards.Exists(postCard => !postCard.IsImage))
            {
                XtraMessageBox.Show("请删除明信片列表中的非图片条目，如果文件识别判断错误，请手工修改文件后，点击刷新按钮重新加载");
                return;
            }
            var count = (int)postCardCountEdit.Value;
            var copy = count / envelope.PostCards.Count;
            var tmpCopy = count % envelope.PostCards.Count;
            for (var i = 0; i < envelope.PostCards.Count; i++)
            {
                if (i < tmpCopy)
                {
                    envelope.PostCards[i].Copy = copy + 1;
                    continue;
                }
                envelope.PostCards[i].Copy = copy;
            }
            envelopeDetailGridView.RefreshDataSource();
        }


        private void gridView1_FocusedRowChanged(object sender,
            FocusedRowChangedEventArgs e)
        {
        }

        private void orderTaobaoIdTextEdit_EditValueChanged(object sender, EventArgs e)
        {
            order.TaobaoId = orderTaobaoIdTextEdit.Text;
        }

        private void orderUrgentCheckEdit_CheckedChanged(object sender, EventArgs e)
        {
            order.Urgent = orderUrgentCheckEdit.Checked;
        }

        private void envelopeDoubleSideCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItem9.Visibility = layoutControlItem37.Visibility =
                envelopeDoubleSideCheckBox.Checked ? LayoutVisibility.Always : LayoutVisibility.Never;
            envelope.DoubleSide = envelopeDoubleSideCheckBox.Checked;
        }

        private void envelopeSubmit_Click(object sender, EventArgs e)
        {
            if (!dxValidationProvider1.Validate()) return;
            foreach (var envelopePostCard in envelope.PostCards)
            {
                if (!envelopePostCard.IsImage)
                {
                    XtraMessageBox.Show("存在不是图片的明信片，请删除或手动");
                    return;
                }
                if (envelopePostCard.FrontStyle == null ||
                    envelope.DoubleSide && envelopePostCard.BackStyle == null)
                {
                    XtraMessageBox.Show("存在没有设置正面或反面样式的明信片，请检查");
                    return;
                }
                if (envelopePostCard.Copy > 0) continue;
                XtraMessageBox.Show("存在打印张数不正确的明信片，请检查");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void removeSelectedPostCardButton_Click(object sender,
            ButtonPressedEventArgs e)
        {
            var postCard = gridView1.GetFocusedRow() as PostCardInfo;
            if (postCard != null)
                envelope.PostCards.Remove(postCard);
        }

        private void repositoryItemButtonEdit3_ButtonClick(object sender,
            ButtonPressedEventArgs e)
        {
            var tmpSender = sender as ButtonEdit;

            var focusedValue = gridView1.GetFocusedRow() as PostCardInfo;
            //如果当前没有选中的行,或者选中行的文件没有父目录（一般不会），直接返回
            if (focusedValue == null || focusedValue.FileInfo.Directory == null) return;

            //打开文件选择框
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = focusedValue.FileInfo.Directory.FullName
            };
            //如果取消选择文件，直接返回
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            //要上传的文件信息
            var backFileInfo = new FileInfo(openFileDialog.FileName);
            //获取文件MD5
            //var md5 = backFileInfo.GetMd5();
            //上传文件
            backFileInfo.Upload(fileId =>
            {
                focusedValue.BackFileInfo = backFileInfo;
                focusedValue.BackFileId = fileId;
                focusedValue.CustomerBackStyle = true;
                gridView1.RefreshData();
                //问价
                if (tmpSender != null) tmpSender.Text = backFileInfo.Name;
            }, message => { XtraMessageBox.Show(message); });
        }

        private void PostCardBackStyleGridLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            var b = sender as GridLookUpEdit;
            if (b == null) return;
            var styleInfo = b.GetSelectedDataRow() as BackStyleInfo;
            var focusedRow = gridView1.GetFocusedRow() as PostCardInfo;
            if (focusedRow == null) return;
            focusedRow.CustomerBackStyle = false;
            if (styleInfo != null) focusedRow.BackFileId = styleInfo.FileId;
            envelope.BackStyle = focusedRow.BackStyle;
            focusedRow.BackFileInfo = null;
            gridView1.RefreshData();
        }

        private void backStyleCustomerButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = envelope.Directory.FullName
            }; //如果选择了文件
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var backFileInfo = new FileInfo(openFileDialog.FileName);
                //获取文件MD5
                // var md5 = backFileInfo.GetMd5();
                //上传文件
                backFileInfo.Upload(fileId =>
                {
                    envelope.BackStyle = "自定义";
                    foreach (var envelopePostCard in envelope.PostCards)
                    {
                        envelopePostCard.BackStyle = "自定义";
                        envelopePostCard.BackFileInfo = backFileInfo;
                        envelopePostCard.BackFileId = fileId;
                        envelopePostCard.CustomerBackStyle = true;
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
                envelope = new EnvelopeInfo
                {
                    Directory = _directoryInfo,
                    PostCards = new List<PostCardInfo>(),
                    ProductSize=new PostSize
                    {
                        Width=148,
                        Height=100
                    },
                    DoubleSide = !_directoryInfo.FullName.Contains("[单面]")
                };
                Text = envelope.Directory.FullName;
                envelopeDetailGridView.DataSource = envelope.PostCards;
                var postCards = new Queue<PostCardInfo>();

                foreach (var fileInfo in fileInfos)
                {
                    if (Resources.notPostCardFileExtension.Contains(fileInfo.Extension.ToLower()))
                        continue;
                    if (fileInfo.CheckIsImage())
                    {
                        var postCard = new PostCardInfo
                        {
                            Copy = 1,
                            FileInfo = fileInfo,
                            IsImage = true
                        };
                        envelope.PostCards.Add(postCard);
                        Application.DoEvents();
                    }
                    else
                    {
                        XtraMessageBox.Show(fileInfo.FullName + "不是一个有效的图像，请确认！");
                    }
                }
            }
        }

        //public void tmpUpload(Queue<PostCardInfo> postCards)
        //{
        //    if (postCards.Count <= 0) return;
        //    Console.WriteLine(postCards.Count);
        //    var postCard = postCards.Dequeue();
        //    var md5 = postCard.FileInfo.GetMd5();

        //    postCard.FileUploadStat = "正在上传";
        //    envelopeDetailGridView.Update();
        //    gridView1.RefreshData();

        //    postCard.FileInfo.Upload(result =>
        //    {
        //        if (String.result)
        //        {
        //            postCard.IsImage = postCard.FileInfo.IsImage();
        //            postCard.FileId = md5;
        //            postCard.FileName = postCard.FileInfo.Name;
        //            postCard.FileUploadStat = "已上传";
        //            gridView1.RefreshData();
        //        }
        //        else
        //        {
        //            postCard.FileUploadStat = "上传失败";
        //        }
        //        envelopeDetailGridView.Update();
        //    }, failure: error =>
        //    {
        //        postCard.FileUploadStat = "上传失败";
        //        gridView1.RefreshData();
        //        envelopeDetailGridView.Update();
        //    });
        //}

        private void popupContainerEdit1_EditValueChanged(object sender, EventArgs e)
        {
            var a = envelopeProductSize.EditValue.ToString().Split('×');
            var b = a.Length;
            if (b == 2)
            {
                if (envelope != null)
                {
                    envelope.ProductSize = new PostSize
                    {
                        Width = int.Parse(a[0]),
                        Height = int.Parse(a[1])
                    };
                }
            }
        }

        private void gridView14_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            var b = (GridView)sender;
            PostSize size = (PostSize)b.GetFocusedRow();
            if (size == null)
            {
                return;
            }
            envelopeProductSize.EditValue = size.ToString();
            envelopeProductSize.ClosePopup();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }

        private void buttonEdit1_EditValueChanged(object sender, EventArgs e)
        {
            var buttonEdit = (ButtonEdit)sender;
            if (buttonEdit == null)
            {
                return;
            }

            var values = buttonEdit.EditValue.ToString().Split('×');
            envelope.PaperSize.Width = int.Parse(values[0]);
            envelope.PaperSize.Height = int.Parse(values[1]);
        }

        private void buttonEdit1_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 0)
            {
                var a = (ButtonEdit)sender;
                var arr = a.EditValue.ToString().Split('×');
                if (arr.Length == 2)
                {
                    a.EditValue = arr[1] + "×" + arr[0];
                }

            }
            Console.WriteLine(sender);
        }
    }
}