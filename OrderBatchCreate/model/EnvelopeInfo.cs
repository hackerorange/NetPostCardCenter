using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using SystemSetting.size.model;
using OrderBatchCreate.helper;
using soho.domain;

namespace OrderBatchCreate.model
{

    public delegate void EnvelopeEvent(EnvelopeInfo envelopeInfo);

    public sealed class EnvelopeInfo : PostCardBasic, INotifyPropertyChanged, ICloneable
    {
        private int _arrayColumn;
        private int _arrayRow;

//        private PostSize _productSize;

        public EnvelopeInfo()
        {
            PostCards = new List<PostCardInfo>();
            //纸张尺寸
            PaperSize = new PostSize
            {
                Width = 464,
                Height = 320
            };
            //成品尺寸
            ProductSize = new PostSize
            {
                Name = "标准尺寸",
                Width = 148,
                Height = 100
            };
            //默认为双面
            DoubleSide = true;
            //ResetRowAndColumn();
        }

        public EnvelopeInfo(PostCardBasic envelopeInfo) : this()
        {
            Parent = envelopeInfo;
        }


        public EnvelopeInfo(OrderInfo orderInfo, PostCardBasic envelopeInfo) : this(envelopeInfo)
        {
            OrderInfo = orderInfo;
        }

        public override BatchStatus Status => PostCardCount == 0 ? BatchStatus.EnvelopeEmptyPath : (this.Already() ? BatchStatus.EnvelopeAlready : BatchStatus.EnvelopeNotReady);


        public override int Copy
        {
            get
            {
                if (PostCards == null || PostCards.Count == 0)
                    return 0;
                return PostCards.Sum(postCard => postCard.Copy);
            }
        }


        /// <summary>
        ///     此明信片集合的父节点
        /// </summary>
        public EnvelopeInfo ParentEnvelope { get; set; }

        /// <summary>
        ///     此明信片集合所属的订单信息
        /// </summary>
        public OrderInfo OrderInfo { get; set; }

        /// <summary>
        ///     打印时使用的纸张尺寸
        /// </summary>
        public PostSize PaperSize { get; set; }

        /// <summary>
        ///     一张纸上排列的列数
        /// </summary>
        public int ArrayColumn
        {
            get => _arrayColumn;
            set
            {
                if (_arrayColumn == value) return;
                _arrayColumn = value;
                NotifyPropertyChanged(() => HorizontalWhite);
                NotifyPropertyChanged(() => ArrayColumn);
            }
        }


        /// <summary>
        ///     一张纸上排列的行数
        /// </summary>
        public int ArrayRow
        {
            get => _arrayRow;
            set
            {
                if (_arrayRow == value) return;
                _arrayRow = value;
                NotifyPropertyChanged(() => VerticalWhite);
                NotifyPropertyChanged(() => ArrayRow);
            }
        }

        /// <summary>
        ///     水平方向白边（只读属性）
        /// </summary>
        public int HorizontalWhite => PaperSize.Width - ProductSize.Width * _arrayColumn;

        /// <summary>
        ///     竖直方向白边(只读属性)
        /// </summary>
        public int VerticalWhite => PaperSize.Height - ProductSize.Height * _arrayRow;

        /// <summary>
        ///     此订单下的所有订单列表
        /// </summary>
        public List<PostCardInfo> PostCards { get; }

        /// <summary>
        ///     明信片总张数
        /// </summary>
        public int PostCardCount => Copy;

        /// <summary>
        ///     获取浪费的张数（只读）
        /// </summary>
        public int PostCardWaste
        {
            get
            {
                var aa = _arrayColumn * _arrayRow;
                if (aa <= 0) return 0;
                var waste = aa - PostCardCount % aa;
                return waste == aa ? 0 : waste;
            }
        }

        /// <summary>
        ///     获取需要打印的张数（只读）
        /// </summary>
        public int PaperNeedPrint
        {
            get
            {
                if (_arrayColumn * _arrayRow == 0)
                    return 0;
                return PostCardCount / (_arrayColumn * _arrayRow) + PostCardWaste == 0 ? 1 : 0;
            }
        }

        public object Clone()
        {
            return new EnvelopeInfo
            {
                ProductSize = ProductSize,
                PaperName = PaperName,
                PaperSize = PaperSize,
                DoubleSide = DoubleSide,
                FrontStyle = FrontStyle,
                BackStyle = BackStyle,
                ArrayColumn = ArrayColumn,
                ArrayRow = ArrayRow
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ResetRowAndColumn()
        {
            if (ProductSize == null) return;
            if (ProductSize.Width != 0)
            {
                _arrayColumn = PaperSize.Width / ProductSize.Width;
                NotifyPropertyChanged(() => ArrayColumn);
                NotifyPropertyChanged(() => HorizontalWhite);
            }
            if (ProductSize.Height != 0)
            {
                _arrayRow = PaperSize.Height / ProductSize.Height;
                NotifyPropertyChanged(() => ArrayRow);
                NotifyPropertyChanged(() => VerticalWhite);
            }
        }

        public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        {
            if (PropertyChanged == null)
                return;

            if (!(property.Body is MemberExpression memberExpression))
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }
    }
}