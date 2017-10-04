using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using postCardCenterSdk.response.envelope;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EnvelopeInfo : INotifyPropertyChanged
    {
        private int _copy;
        private BackStyleInfo _backStyle;
        private string _frontStyle;
        private int _arrayColumn;
        private int _arrayRow;
        private PostSize _paperSize;
        private PostSize _productSize;

        public EnvelopeInfo()
        {
            //纸张尺寸
            PaperSize = new PostSize
            {
                Width = 450,
                Height = 320
            };
            //成品尺寸
            ProductSize = new PostSize
            {
                Width = 148,
                Height = 100
            };
            //默认为双面
            DoubleSide = true;
            ResetRowAndColumn();
            PostCards = new List<PostCardInfo>();
        }

        /// <summary>
        /// 此明信片集合所属的订单信息
        /// </summary>
        public OrderInfo OrderInfo { get; set; }


        /// <summary>
        /// 明信片集合ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 纸张类型
        /// </summary>

        public string PaperName { get; set; }

        /// <summary>
        /// 统一的份数
        /// </summary>
        public int Copy {

            get { return _copy; }

            set {
                _copy = value;               
                NotifyPropertyChanged(() => Copy);
            }
        }

        /// <summary>
        /// 是否为双面
        /// </summary>
        public bool DoubleSide { get; set; }

        /// <summary>
        /// 明信片板式ID
        /// </summary>
        public string FrontStyle {

            get { return _frontStyle; }

            set {
                _frontStyle = value;
                //PostCards.ForEach(postCard => postCard.FrontStyle = value);
            }
        }

        /// <summary>
        /// 明信片反面样式
        /// </summary>
        public BackStyleInfo BackStyle {
            get { return _backStyle; }
            set {
                _backStyle = value;
                foreach (var envelopePostCard in PostCards)
                {
                    envelopePostCard.BackStyle = value.Name;
                    envelopePostCard.BackFileId = value.FileId;
                }
            }
        }
        /// <summary>
        /// 成品尺寸
        /// </summary>
        public PostSize ProductSize {
            get =>_productSize;
            set {
                if (_productSize == value) return;
                _productSize = value;
                ResetRowAndColumn();
            }
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

        /// <summary>
        /// 打印时使用的纸张尺寸
        /// </summary>
        public PostSize PaperSize {
            get => _paperSize;
            set {
                if (_paperSize == value) return;
                _paperSize = value;
                ResetRowAndColumn();
            }
        }

        /// <summary>
        /// 一张纸上排列的列数
        /// </summary>
        public int ArrayColumn {
            get => _arrayColumn;
            set {
                if (_arrayColumn == value) return;
                _arrayColumn = value;
                NotifyPropertyChanged(() => HorizontalWhite);
            }
        }


        /// <summary>
        /// 一张纸上排列的行数
        /// </summary>
        public int ArrayRow {

            get => _arrayRow;
            set {
                if (_arrayRow == value) return;
                _arrayRow = value;
                NotifyPropertyChanged(() => VerticalWhite);
            }
        }

        /// <summary>
        /// 水平方向白边（只读属性）
        /// </summary>
        public int HorizontalWhite => _paperSize.Width - _productSize.Width * _arrayColumn;

        /// <summary>
        /// 竖直方向白边(只读属性)
        /// </summary>
        public int VerticalWhite => _paperSize.Height - _productSize.Height * _arrayRow;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        {
            if (PropertyChanged == null)
                return;

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }

        /// <summary>
        /// 此明信片集合在本地的路径
        /// </summary>
        public DirectoryInfo Directory { get; set; }

        /// <summary>
        /// 此订单下的所有订单列表
        /// </summary>
        public List<PostCardInfo> PostCards { get; set; }

        /// <summary>
        /// 明信片总张数
        /// </summary>
        public int PostCardCount {
            get {
                if (PostCards == null || PostCards.Count == 0)
                {
                    return 0;
                }
                return PostCards.Sum(postCard => postCard.Copy);
            }
        }

        /// <summary>
        /// 成品文件ID
        /// </summary>
        public string ProductFileId { get; set; }
        /// <summary>
        /// 此明信片所属的订单ID
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 获取浪费的张数（只读）
        /// </summary>
        public int PostCardWaste {
            get {
                int aa = _arrayColumn * _arrayRow;
                if (aa > 0)
                {
                    int waste = aa - PostCardCount % aa;
                    return waste == aa ? 0 : waste;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 获取需要打印的张数（只读）
        /// </summary>
        public int PaperNeedPrint {
            get {
                if(_arrayColumn* _arrayRow == 0)
                {
                    return 0;
                }
                if (PostCardWaste == 0)
                {
                    return PostCardCount / (_arrayColumn * _arrayRow);
                }
                else
                {
                    return PostCardCount / (_arrayColumn * _arrayRow)+1;
                }
            }
        }

        public List<string> EnvelopeInfos {
            get {
                List<string> result = new List<string>();
                var match = new Regex("\\[(.+?)]+").Matches(Directory.FullName);
                
                for(int i = 0; i < match.Count; i++)
                {
                    result.Add(match[i].Result("$1"));
                }
                return result;
            }
        }
    }
}