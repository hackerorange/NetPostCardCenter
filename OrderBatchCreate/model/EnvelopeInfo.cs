using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Hacker.Inko.PostCard.Library;
using OrderBatchCreate.helper;

namespace OrderBatchCreate.model
{

    public delegate void EnvelopeEvent(EnvelopeInfo envelopeInfo);

    public sealed class EnvelopeInfo : PostCardBasic, INotifyPropertyChanged, ICloneable
    {

        internal class LayoutInfo
        {
            public LayoutInfo(EnvelopeInfo envelopeInfo)
            {
                if (envelopeInfo.ProductSize.Width != 0)
                {
                    ColumnCount = envelopeInfo.PaperSize.Width / envelopeInfo.ProductSize.Width;
                }
                if (envelopeInfo.ProductSize.Height != 0)
                {
                    RowCount = envelopeInfo.PaperSize.Height / envelopeInfo.ProductSize.Height;
                }
            }

            public int ColumnCount { get; set; }
            public int RowCount { get; set; }
        }


        private static readonly IDictionary<string, LayoutInfo> _layoutInfos=new Dictionary<string, LayoutInfo>();

        private LayoutInfo GetLayoutInfo()
        {
            var key=GetLayoutKey();
            if (_layoutInfos.ContainsKey(key))
            {
                return _layoutInfos[key];
            }
            var tmp=new LayoutInfo(this);
            _layoutInfos.Add(key,tmp);
            return tmp;
        }

        private string GetLayoutKey()
        {
            return ProductSize.Width + ":" + ProductSize.Height + ":" + PaperSize.Width + ":" + PaperSize.Height;
        }
        
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
            get => GetLayoutInfo().ColumnCount;
            set
            {
                GetLayoutInfo().ColumnCount=value;
                if (GetLayoutInfo().ColumnCount == value) return;
                GetLayoutInfo().ColumnCount = value;
                NotifyPropertyChanged(() => HorizontalWhite);
                NotifyPropertyChanged(() => ArrayColumn);
            }
        }


        /// <summary>
        ///     一张纸上排列的行数
        /// </summary>
        public int ArrayRow
        {
            get => GetLayoutInfo().RowCount;
            set
            {
                if (GetLayoutInfo().RowCount == value) return;
                GetLayoutInfo().RowCount = value;
                NotifyPropertyChanged(() => VerticalWhite);
                NotifyPropertyChanged(() => ArrayRow);
            }
        }

        /// <summary>
        ///     水平方向白边（只读属性）
        /// </summary>
        public int HorizontalWhite => PaperSize.Width - ProductSize.Width * GetLayoutInfo().ColumnCount;

        /// <summary>
        ///     竖直方向白边(只读属性)
        /// </summary>
        public int VerticalWhite => PaperSize.Height - ProductSize.Height * GetLayoutInfo().RowCount;

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
                var aa = GetLayoutInfo().RowCount * GetLayoutInfo().ColumnCount;
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
                if (GetLayoutInfo().RowCount * GetLayoutInfo().ColumnCount == 0)
                    return 0;
                return PostCardCount / (GetLayoutInfo().ColumnCount * GetLayoutInfo().RowCount) + PostCardWaste == 0 ? 1 : 0;
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
            var key = GetLayoutKey();
            _layoutInfos.Remove(key);
            NotifyPropertyChanged(() => ArrayColumn);
            NotifyPropertyChanged(() => HorizontalWhite);
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