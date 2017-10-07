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
using soho.helper;

namespace PostCardCenter.form.envelope
{
    public partial class EnvelopeInfoForm : XtraForm
    {
        public ILog Logger = LogManager.GetLogger("");
        private IDictionary<string, FrontStyleInfo> _frontStyles = new Dictionary<string,FrontStyleInfo>();
        private IDictionary<string, BackStyleInfo> _backStyles = new Dictionary<string, BackStyleInfo>();

        /// <summary>
        /// 此订单的明信片信息
        /// </summary>
        private EnvelopeInfo _envelope;

        /// <summary>
        /// 初始化（构造函数私有化）
        /// </summary>
        private EnvelopeInfoForm()
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
                comboBoxEdit1.Properties.Items.Clear();
                //获取正面集合，暂时只是字符串
                response.ForEach(frontStyle =>
                {
                    
                    _frontStyles.Add(frontStyle.Name,new FrontStyleInfo {
                        Name = frontStyle.Name
                    });
                    comboBoxEdit1.Properties.Items.Add(frontStyle.Name);
                    repositoryItemComboBox1.Properties.Items.Add(frontStyle.Name);
                });
                //PostCardFrontStyleGridLookUpEdit.DataSource = envelopeFrontStyle.Properties.DataSource = _frontStyles;
            });
            //异步获取反面样式列表
            WebServiceInvoker.GetBackStyleTemplateList(success =>
            {
                List<BackStyleInfo> backStyleInfos = new List<BackStyleInfo>();
                success.ForEach(ba =>
                {
                    _backStyles.Add(ba.Name, new BackStyleInfo()
                    {
                        Name = ba.Name,
                        FileId = ba.FileId
                    });
                    comboBoxEdit2.Properties.Items.Add(ba.Name);
                });
            }, errorMessage => 
            {
                XtraMessageBox.Show(errorMessage);
            });
        }
        /// <summary>
        /// 获取明信片详细信息，如果明信片集合为空，并且存在文件夹信息，从文件夹中获取所有文件
        /// </summary>
        /// <param name="envelope"></param>
        public EnvelopeInfoForm(EnvelopeInfo envelope):this()
        {
            this._envelope = envelope;
            resetColumnRowArray();            
        }

       
        private void EnvelopeInfoForm_Load(object sender, EventArgs e)
        {
            if (_envelope == null )
            {
                DialogResult = DialogResult.Abort;
                Close();
                return;
            }
            var tmpStrings = _envelope.EnvelopeInfos;

            tmpStrings.ForEach(String => { orderTaobaoIdTextEdit.Properties.Items.Add(String); });

            //绑定一张纸上放几列
            arrayColumn.DataBindings.Add("EditValue", _envelope, "ArrayColumn" , false, DataSourceUpdateMode.OnPropertyChanged);
            //绑定一张纸上放几行
            arrayRow.DataBindings.Add("EditValue", _envelope, "ArrayRow", false, DataSourceUpdateMode.OnPropertyChanged);

            #region 数据绑定

            //绑定淘宝ID
            orderTaobaoIdTextEdit.DataBindings.Add("EditValue", _envelope.OrderInfo, "TaobaoId", false, DataSourceUpdateMode.OnPropertyChanged);
            //绑定纸张名称
            envelopePaperName.DataBindings.Add("EditValue", _envelope, "PaperName", false, DataSourceUpdateMode.OnPropertyChanged);
            //明信片总数量
            postCardCount.DataBindings.Add("EditValue", _envelope, "PostCardCount", false, DataSourceUpdateMode.OnPropertyChanged);
            //绑定横坐标
            horizontalWhite.DataBindings.Add("EditValue", _envelope, "HorizontalWhite", false, DataSourceUpdateMode.OnPropertyChanged);
            //绑定横坐标
            verticalWhite.DataBindings.Add("EditValue", _envelope, "VerticalWhite", false, DataSourceUpdateMode.OnPropertyChanged);
            //绑定浪费的张数
            PostCardWaste.DataBindings.Add("EditValue", _envelope, "PostCardWaste", false, DataSourceUpdateMode.OnPropertyChanged);
            //绑定需要打印的张数
            paperPrint.DataBindings.Add("EditValue", _envelope, "PaperNeedPrint", false, DataSourceUpdateMode.OnPropertyChanged);
           
            //绑定是否双面
            //envelopeDoubleSideCheckBox.DataBindings.Add("Checked", _envelope, "DoubleSide", false, DataSourceUpdateMode.OnPropertyChanged);
            //绑定订单是否加急
            orderUrgentCheckEdit.DataBindings.Add("Checked", _envelope.OrderInfo, "Urgent", false, DataSourceUpdateMode.OnPropertyChanged);
            //绑定打印份数
            //envelopePostCardCopyEdit.DataBindings.Add("EditValue", _envelope, "Copy", false, DataSourceUpdateMode.OnPropertyChanged);


            #endregion
            // 绑定数据源
            envelopeDetailGridView.DataSource = _envelope.PostCards;

            //绑定正面样式
            // envelopeFrontStyle.DataBindings.Add("EditValue", _envelope, "FrontStyle", false, DataSourceUpdateMode.OnPropertyChanged);

            //设置是否双面
            envelopeDoubleSide.Checked = _envelope.DoubleSide;



            envelopeProductSize.EditValue = _envelope.ProductSize.ToString();
            //显示名称
            textEdit1.Text = _envelope.Directory.FullName;
            //订单信息完整性校验
            dxValidationProvider1.SetValidationRule(envelopePaperName, new ConditionValidationRule
                {
                    ConditionOperator = ConditionOperator.IsNotBlank,
                    ErrorText = "请输入纸张名称"
                });
            dxValidationProvider1.SetValidationRule(orderTaobaoIdTextEdit, new ConditionValidationRule
                {
                    ConditionOperator = ConditionOperator.IsNotBlank,
                    ErrorText = "请输入客户淘宝ID"
                });
        }

        private void envelopePostCardCopyEdit_EditValueChanged(object sender, EventArgs e)
        {
            
            //将每一张明信片的份数统一设置
            _envelope.PostCards.ForEach(PostCardInfo => { PostCardInfo.Copy =(int)envelopePostCardCopyEdit.Value; });
            updatePrintInfo();
            gridView1.RefreshData();
            //envelope.PostCards.ForEach(postCard => postCard.Copy = (int)envelopePostCardCopyEdit.Value);
        }

        private void postCardCountEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (_envelope.PostCards.Exists(postCard => !postCard.IsImage))
            {
                XtraMessageBox.Show("请删除明信片列表中的非图片条目，如果文件识别判断错误，请手工修改文件后，点击刷新按钮重新加载");
                return;
            }
            var count = (int)postCardCountEdit.Value;
            var copy = count / _envelope.PostCards.Count;
            var tmpCopy = count % _envelope.PostCards.Count;
            _envelope.Copy = copy;

            for (var i = 0; i < _envelope.PostCards.Count; i++)
            {
                if (i < tmpCopy)
                {
                    _envelope.PostCards[i].Copy = copy + 1;
                    continue;
                }
                _envelope.PostCards[i].Copy = copy;
            }
        
            envelopeDetailGridView.RefreshDataSource();
            updatePrintInfo();            
        }

        private void envelopeDoubleSideCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (envelopeDoubleSide.Checked)
            {
                layoutControlItem17.Visibility = layoutControlItem37.Visibility = LayoutVisibility.Always;
                _envelope.BackStyle = null;
            }
            else
            {
                layoutControlItem17.Visibility = layoutControlItem37.Visibility = LayoutVisibility.Never;
            }
            postCardBackStyle.Visible = _envelope.DoubleSide = envelopeDoubleSide.Checked;




        }

        private void envelopeSubmit_Click(object sender, EventArgs e)
        {
            if (!dxValidationProvider1.Validate()) return;
            foreach (var envelopePostCard in _envelope.PostCards)
            {
                if (!envelopePostCard.IsImage)
                {
                    XtraMessageBox.Show("存在不是图片的明信片，请删除或手动");
                    return;
                }
                if (envelopePostCard.FrontStyle == null ||
                    _envelope.DoubleSide && envelopePostCard.BackStyle == null)
                {
                    XtraMessageBox.Show("存在没有设置正面或反面样式的明信片，请检查");
                    return;
                }
                if (envelopePostCard.Copy > 0) continue;
                XtraMessageBox.Show("存在打印张数不正确的明信片，请检查");
                return;
            }
            if (_envelope.PostCardWaste != 0)
            {
                if (XtraMessageBox.Show("存在空白明信片，是否跳过？", "是否继续", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void removeSelectedPostCardButton_Click(object sender,
            ButtonPressedEventArgs e)
        {
            var postCard = gridView1.GetFocusedRow() as PostCardInfo;
            if (postCard != null)
                _envelope.PostCards.Remove(postCard);
            gridView1.RefreshData();
            updatePrintInfo();
            
        }

        private void updatePrintInfo()
        {
            _envelope.NotifyPropertyChanged(() => _envelope.PaperNeedPrint);
            _envelope.NotifyPropertyChanged(() => _envelope.PostCardWaste);
            _envelope.NotifyPropertyChanged(() => _envelope.PostCardCount);
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
            backFileInfo.Upload("自定义反面样式", fileId =>
            {
                focusedValue.BackStyle = new BackStyleInfo
                {
                    Name = "自定义",
                    FileId = fileId
                };
                //focusedValue.BackFileInfo = backFileInfo;
                //focusedValue.BackFileId = fileId;
                //focusedValue.CustomerBackStyle = true;
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
            //focusedRow.CustomerBackStyle = false;            
            _envelope.BackStyle = styleInfo;
            //focusedRow.BackFileInfo = null;
            gridView1.RefreshData();
        }

        private void backStyleCustomerButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = _envelope.Directory.FullName
            }; //如果选择了文件
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var backFileInfo = new FileInfo(openFileDialog.FileName);
                //获取文件MD5
                // var md5 = backFileInfo.GetMd5();
                //上传文件
                backFileInfo.Upload("自定义反面样式", fileId =>
                {
                    _envelope.BackStyle =new BackStyleInfo
                    {
                        Name="自定义",
                        FileId= fileId
                    };
                    foreach (var envelopePostCard in _envelope.PostCards)
                    {
                        envelopePostCard.BackStyle = _envelope.BackStyle;
                    }
                    gridView1.RefreshData();
                    comboBoxEdit2.Text = _envelope.BackStyle.Name;
                }, error => { XtraMessageBox.Show(error); });
            }
        }

        private void popupContainerEdit1_EditValueChanged(object sender, EventArgs e)
        {
            var a = envelopeProductSize.EditValue.ToString().Split('×');
            var b = a.Length;
            if (b == 2)
            {
                if (_envelope != null)
                {
                    _envelope.ProductSize = new PostSize
                    {
                        Width = int.Parse(a[0]),
                        Height = int.Parse(a[1])
                    };
                    resetColumnRowArray();
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
            resetColumnRowArray();
        }

        private void resetColumnRowArray()
        {
            _envelope.CalculateArray();
            arrayColumn.Properties.MaxValue = arrayColumn.Value;
            arrayRow.Properties.MaxValue = arrayRow.Value;
        }

        private void buttonEdit1_EditValueChanged(object sender, EventArgs e)
        {
            var buttonEdit = (ButtonEdit)sender;
            if (buttonEdit == null)
            {
                return;
            }

            var values = buttonEdit.EditValue.ToString().Split('×');
            try
            {
                _envelope.PaperSize = new PostSize
                {
                    Width = int.Parse(values[0]),
                    Height = int.Parse(values[1])
                };
            }catch(Exception)
            {
                XtraMessageBox.Show("格式错误");
            }
        }

        private void envelopeDetailGridView_Click(object sender, EventArgs e)
        {

        }

        private void EnvelopeInfoForm_Shown(object sender, EventArgs e)
        {
            if (_envelope.PostCards.Count == 0 && _envelope.Directory != null)
            {
                var fileInfos = _envelope.Directory.GetFiles();
                layoutControlGroup2.Enabled = false;
                layoutControlGroup3.Enabled = false;
                layoutControlGroup5.Enabled = false;

                FileInfo tmpBackgroundFile = null;

                foreach (var fileInfo in fileInfos)
                {
                    if (Resources.notPostCardFileExtension.Contains(fileInfo.Extension.ToLower()))
                        continue;

                    if (fileInfo.Name.Contains("backgroundPicture"))
                    {
                        if (XtraMessageBox.Show("检测到反面文件，请确认？", "反面文件", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            tmpBackgroundFile = fileInfo;
                            continue;
                        }
                    }
                    _envelope.PostCards.Add(new PostCardInfo
                    {
                        Copy = 1,
                        FileInfo = fileInfo,
                        IsImage = fileInfo.CheckIsImage(),
                        FileUploadStat = constant.PostCardFileUploadStatusEnum.BEFOREL_UPLOAD
                    });
                    envelopeDetailGridView.RefreshDataSource();
                    updatePrintInfo();
                    Application.DoEvents();
                }
                if (tmpBackgroundFile != null)
                {
                    //上传反面样式
                    tmpBackgroundFile.Upload("自定义反面样式", fileId =>
                    {
                        //上传成功后，返回此文件ID,同时，将所有明信片的反面ID设置为此文件ID
                        _envelope.BackStyle = new BackStyleInfo
                        {
                            Name = "自定义",
                            FileId = fileId
                        };
                        foreach (var envelopePostCard in _envelope.PostCards)
                        {
                            envelopePostCard.BackStyle = _envelope.BackStyle;
                        }
                        gridView1.RefreshData();
                        comboBoxEdit2.Text = _envelope.BackStyle.Name;
                    }, error => { XtraMessageBox.Show(error); });
                }
                layoutControlGroup2.Enabled = true;
                layoutControlGroup3.Enabled = true;
                layoutControlGroup5.Enabled = true;
            }
        }

    
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            _envelope.PaperSize = new PostSize
            {
                Width = _envelope.PaperSize.Height,
                Height = _envelope.PaperSize.Width
            };
            resetColumnRowArray();
            buttonEdit1.EditValue = _envelope.PaperSize.ToString();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //如果没有明信片，无法补齐
            if (_envelope.PostCards.Count == 0) return;
            //如果浪费的张数小于总数
            int one= _envelope.PostCardWaste % _envelope.PostCards.Count;
            for(int i = 0; i < one; i++)
            {
                _envelope.PostCards[i].Copy++;
            }
            //如果浪费的数量比打印的数量多，则统一追加指定的数量
            int all = _envelope.PostCardWaste / _envelope.PostCards.Count;
            if (all > 0)
            {
                _envelope.PostCards.ForEach(PostCard => PostCard.Copy += all);
            }
            gridView1.RefreshData();
            //更新打印信息
            updatePrintInfo();            
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var com = sender as ComboBoxEdit;
            if (_frontStyles.ContainsKey(com.Text))
            {
                _envelope.FrontStyle = _frontStyles[com.Text].Name;
                _envelope.PostCards.ForEach(postCard => postCard.FrontStyle = _envelope.FrontStyle);
                gridView1.RefreshData();
            }
        }

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var com = sender as ComboBoxEdit;
            if (_backStyles.ContainsKey(com.Text))
            {
                _envelope.BackStyle = _backStyles[com.Text];
                _envelope.PostCards.ForEach(postCard => postCard.BackStyle = _envelope.BackStyle);
                gridView1.RefreshData();
            }
        }
    }
}