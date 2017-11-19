using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using SystemSetting.backStyle.model;
using SystemSetting.size.model;
using soho.domain;

namespace PostCardCrop.model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EnvelopeInfo : INotifyPropertyChanged, IEquatable<EnvelopeInfo>
    {
        private decimal _arrayColumn;
        private decimal _arrayRow;
        private BackStyleInfo _backStyle;
        private int _copy;
        private PostSize _paperSize;
        private PostSize _productSize;

        public EnvelopeInfo()
        {
            //纸张尺寸
            PaperSize = new PostSize
            {
                Width = 464,
                Height = 320
            };
            //成品尺寸
            //ProductSize = new PostSize
            //{
            //    Width = 148,
            //    Height = 100
            //};
            //默认为双面
            DoubleSide = true;
            //ResetRowAndColumn();
            PostCards = new List<PostCardInfo>();
        }

        public EnvelopeInfo Key => this;

        public List<EnvelopeInfo> EnvelopeInfoList { get; set; }


        /// <summary>
        ///     此明信片集合所属的订单信息
        /// </summary>
        public OrderInfo OrderInfo { get; set; }


        /// <summary>
        ///     明信片集合ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     纸张类型
        /// </summary>

        public string PaperName { get; set; }

        /// <summary>
        ///     统一的份数
        /// </summary>
        public int Copy
        {
            get => _copy;

            set
            {
                _copy = value;
                NotifyPropertyChanged(() => Copy);
            }
        }

        /// <summary>
        ///     是否为双面
        /// </summary>
        public bool DoubleSide { get; set; }

        /// <summary>
        ///     明信片板式ID
        /// </summary>
        public string FrontStyle { get; set; }

        /// <summary>
        ///     明信片反面样式
        /// </summary>
        public BackStyleInfo BackStyle
        {
            get => _backStyle;
            set
            {
                _backStyle = value;

                foreach (var envelopePostCard in PostCards)
                    if (value != null)
                        envelopePostCard.BackStyle = value;
                    else
                        envelopePostCard.BackStyle = new BackStyleInfo();
            }
        }

        /// <summary>
        ///     成品尺寸
        /// </summary>
        public PostSize ProductSize
        {
            get => _productSize;
            set
            {
                if (_productSize == value) return;
                _productSize = value;
                ResetRowAndColumn();
            }
        }

        /// <summary>
        ///     打印时使用的纸张尺寸
        /// </summary>
        public PostSize PaperSize
        {
            get => _paperSize;
            set
            {
                if (_paperSize == value) return;
                _paperSize = value;
                ResetRowAndColumn();
            }
        }

        /// <summary>
        ///     一张纸上排列的列数
        /// </summary>
        public decimal ArrayColumn
        {
            get => _arrayColumn;
            set
            {
                if (_arrayColumn == value) return;
                _arrayColumn = value;
                NotifyPropertyChanged(() => HorizontalWhite);
            }
        }


        /// <summary>
        ///     一张纸上排列的行数
        /// </summary>
        public decimal ArrayRow
        {
            get => _arrayRow;
            set
            {
                if (_arrayRow == value) return;
                _arrayRow = value;
                NotifyPropertyChanged(() => VerticalWhite);
            }
        }

        /// <summary>
        ///     水平方向白边（只读属性）
        /// </summary>
        public decimal HorizontalWhite => _paperSize.Width - _productSize.Width * _arrayColumn;

        /// <summary>
        ///     竖直方向白边(只读属性)
        /// </summary>
        public decimal VerticalWhite => _paperSize.Height - _productSize.Height * _arrayRow;

        /// <summary>
        ///     此明信片集合在本地的路径
        /// </summary>
        /// <summary>
        ///     此订单下的所有订单列表
        /// </summary>
        public List<PostCardInfo> PostCards { get; set; }

        /// <summary>
        ///     明信片总张数
        /// </summary>
        public int PostCardCount
        {
            get
            {
                if (PostCards == null || PostCards.Count == 0)
                    return 0;
                return PostCards.Sum(postCard => postCard.Copy);
            }
        }

        /// <summary>
        ///     成品文件ID
        /// </summary>
        public string ProductFileId { get; set; }

        /// <summary>
        ///     此明信片所属的订单ID
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        ///     获取浪费的张数（只读）
        /// </summary>
        public decimal PostCardWaste
        {
            get
            {
                var aa = _arrayColumn * _arrayRow;
                if (aa > 0)
                {
                    var waste = aa - PostCardCount % aa;
                    return waste == aa ? 0 : waste;
                }
                return 0;
            }
        }

        /// <summary>
        ///     获取需要打印的张数（只读）
        /// </summary>
        public decimal PaperNeedPrint
        {
            get
            {
                if (_arrayColumn * _arrayRow == 0)
                    return 0;
                if (PostCardWaste == 0)
                    return PostCardCount / (_arrayColumn * _arrayRow);
                return PostCardCount / (_arrayColumn * _arrayRow) + 1;
            }
        }

        public bool Equals(EnvelopeInfo other)
        {
            ////如果成品尺寸不相同，返回false
            //if (!Equals(other.ProductSize,ProductSize)) return false;
            ////如果成品纸张名称不相同，返回false
            //if (!Equals(other.PaperName, PaperName)) return false;
            ////如果单双面打印不一致，返回false
            //if (!Equals(other.DoubleSide,DoubleSide)) return false;
            //返回相同
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddEnvelopeInfo(EnvelopeInfo envelope)
        {
            if (EnvelopeInfoList == null)
                EnvelopeInfoList = new List<EnvelopeInfo>();
            EnvelopeInfoList.Add(envelope);
        }

        private void ResetRowAndColumn()
        {
            if (_productSize == null) return;
            if (_productSize.Width != 0)
            {
                _arrayColumn = _paperSize.Width / ProductSize.Width;
                NotifyPropertyChanged(() => ArrayColumn);
                NotifyPropertyChanged(() => HorizontalWhite);
            }
            if (_productSize.Height != 0)
            {
                _arrayRow = _paperSize.Height / ProductSize.Height;
                NotifyPropertyChanged(() => ArrayRow);
                NotifyPropertyChanged(() => VerticalWhite);
            }
        }

        public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        {
            if (PropertyChanged == null)
                return;

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }

        //public List<string> EnvelopeInfos {
        //    get {
        //        List<string> result = new List<string>();
        //        var match = new Regex("\\[(.+?)]+").Matches(Directory.FullName);

        //        for(int i = 0; i < match.Count; i++)
        //        {
        //            result.Add(match[i].Result("$1"));
        //        }
        //        return result;
        //    }
        //}
    }
}