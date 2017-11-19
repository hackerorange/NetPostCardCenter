using System;
using System.Windows.Forms;
using SystemSetting.size.model;
using OrderBatchCreate.model;
using soho.domain;
using EnvelopeInfo = OrderBatchCreate.model.EnvelopeInfo;

namespace OrderBatchCreate.control.envelope
{
    public partial class EnvelopePrintInfo : UserControl
    {
        public event EnvelopeEvent EnvelopeChanged;

        private EnvelopeInfo _envelopeInfo;

        public EnvelopePrintInfo()
        {
            InitializeComponent();
        }

        public EnvelopeInfo EnvelopeInfo
        {
            get => _envelopeInfo;
            set
            {
                _envelopeInfo = value;
                if (value != null)
                {
                    layoutControl1.Enabled = true;
                    //绑定明信片列数
                    arrayColumn.DataBindings.Clear();
                    arrayColumn.DataBindings.Add("EditValue", _envelopeInfo, "ArrayColumn", false, DataSourceUpdateMode.OnPropertyChanged); //绑定一张纸上放几列
                    //绑定明信片行数                      
                    arrayRow.DataBindings.Clear();
                    arrayRow.DataBindings.Add("EditValue", _envelopeInfo, "ArrayRow", false, DataSourceUpdateMode.OnPropertyChanged); //绑定一张纸上放几列

                    //明信片总数量
                    postCardCount.DataBindings.Clear();
                    postCardCount.DataBindings.Add("EditValue", _envelopeInfo, "PostCardCount", false, DataSourceUpdateMode.OnPropertyChanged);

                    horizontalWhite.DataBindings.Clear();
                    horizontalWhite.DataBindings.Add("EditValue", _envelopeInfo, "HorizontalWhite", false, DataSourceUpdateMode.OnPropertyChanged);
                    //绑定横坐标
                    verticalWhite.DataBindings.Clear();
                    verticalWhite.DataBindings.Add("EditValue", _envelopeInfo, "VerticalWhite", false, DataSourceUpdateMode.OnPropertyChanged);
                    //绑定浪费的张数
                    PostCardWaste.DataBindings.Clear();
                    PostCardWaste.DataBindings.Add("EditValue", _envelopeInfo, "PostCardWaste", false, DataSourceUpdateMode.OnPropertyChanged);
                    //绑定需要打印的张数
                    paperPrint.DataBindings.Clear();
                    paperPrint.DataBindings.Add("EditValue", _envelopeInfo, "PaperNeedPrint", false, DataSourceUpdateMode.OnPropertyChanged);

                    paperHeight.EditValue = _envelopeInfo.PaperSize.Height;
                    paperWidth.EditValue = _envelopeInfo.PaperSize.Width;

//                    arrayRow.Properties.MinValue = 1;
//                    arrayRow.Properties.MaxValue = _envelopeInfo.ArrayRow;
//                    arrayColumn.Properties.MinValue = 1;
//                    arrayColumn.Properties.MaxValue = _envelopeInfo.ArrayColumn;
                }
                else
                {
                    layoutControl1.Enabled = false;
                }
            }
        }


        private void HorizontalWhite_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void SimpleButton5_Click(object sender, EventArgs e)
        {
            //如果没有明信片，无法补齐
            if (_envelopeInfo.PostCards.Count == 0) return;
            //如果浪费的张数小于总数
            var one = _envelopeInfo.PostCardWaste % _envelopeInfo.PostCards.Count;
            //如果浪费的数量比打印的数量多，则统一追加指定的数量
            var all = _envelopeInfo.PostCardWaste / _envelopeInfo.PostCards.Count;
            _envelopeInfo.PostCards.ForEach(postCard => postCard.Copy += all + (one-- > 0 ? 1 : 0));
            gridView1.RefreshData();

            _envelopeInfo.NotifyPropertyChanged(() => _envelopeInfo.PaperNeedPrint);
            _envelopeInfo.NotifyPropertyChanged(() => _envelopeInfo.PostCardWaste);
            _envelopeInfo.NotifyPropertyChanged(() => _envelopeInfo.PostCardCount);

            //更新打印信息
            EnvelopeChanged?.Invoke(_envelopeInfo);
        }

        /// <summary>
        /// 旋转纸张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaperSizeRotate_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            _envelopeInfo.PaperSize = new PostSize
            {
                Width = _envelopeInfo.PaperSize.Height,
                Height = _envelopeInfo.PaperSize.Width
            };
            paperWidth.EditValue = _envelopeInfo.PaperSize.Width;
            paperHeight.EditValue = _envelopeInfo.PaperSize.Height;

            _envelopeInfo.NotifyPropertyChanged(() => _envelopeInfo.PaperNeedPrint);
            _envelopeInfo.NotifyPropertyChanged(() => _envelopeInfo.PostCardWaste);
            _envelopeInfo.NotifyPropertyChanged(() => _envelopeInfo.PostCardCount);

//            arrayRow.Properties.MinValue = 1;
//            arrayRow.Properties.MaxValue = _envelopeInfo.ArrayRow;
//            arrayColumn.Properties.MinValue = 1;
//            arrayColumn.Properties.MaxValue = _envelopeInfo.ArrayColumn;

            EnvelopeChanged?.Invoke(_envelopeInfo);
        }

        private void ArrayColumn_EditValueChanged(object sender, EventArgs e)
        {
            EnvelopeChanged?.Invoke(_envelopeInfo);
        }

        private void ArrayRow_EditValueChanged(object sender, EventArgs e)
        {
            EnvelopeChanged?.Invoke(_envelopeInfo);
        }

        private void layoutControlGroup5_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            //重新刷新一下尺寸；
            _envelopeInfo.PaperSize = _envelopeInfo.PaperSize;

//            arrayRow.Properties.MinValue = 1;
//            arrayRow.Properties.MaxValue = _envelopeInfo.ArrayRow;
//            arrayColumn.Properties.MinValue = 1;
//            arrayColumn.Properties.MaxValue = _envelopeInfo.ArrayColumn;
        }
    }
}