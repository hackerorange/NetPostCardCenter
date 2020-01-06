using System.Collections.Generic;
using System.IO;
using SystemSetting.backStyle.model;
using DevExpress.Mvvm.Native;
using DictionaryExtensions = DevExpress.Mvvm.Native.DictionaryExtensions;

namespace OrderBatchCreate.model
{
    public class OrderInfo
    {
        public OrderInfo(DirectoryInfo directoryInfo)
        {
            EnvelopeInfoList = new List<PostCardBasic>();
            DirectoryInfo = directoryInfo;
            TaobaoId = "=========";
        }

        public DirectoryInfo DirectoryInfo { get; }


        public List<PostCardBasic> EnvelopeInfoList { get; }

        public List<EnvelopeInfo> SubmitEnvelopeList
        {
            get
            {
                IDictionary<string, EnvelopeInfo> envelopeInfos = new Dictionary<string, EnvelopeInfo>();
                EnvelopeInfoList.ForEach(postCardBasic =>
                {
                    if (!(postCardBasic is EnvelopeInfo envelopeInfo) || envelopeInfo.Status == BatchStatus.EnvelopeEmptyPath)
                    {
                        return;
                    }

                    var key = envelopeInfo.DoubleSide.ToString() + envelopeInfo.ProductSize.Width.ToString() + envelopeInfo.ProductSize.Height + envelopeInfo.PaperName;
                    var find = envelopeInfos.GetOrAdd(key, () => new EnvelopeInfo
                    {
                        ProductSize = envelopeInfo.ProductSize,
                        PaperName = envelopeInfo.PaperName,
                        PaperSize = envelopeInfo.PaperSize,
                        DoubleSide = envelopeInfo.DoubleSide,
                        FrontStyle = envelopeInfo.FrontStyle,
                        BackStyle = envelopeInfo.BackStyle,
                        ArrayColumn = envelopeInfo.ArrayColumn,
                        ArrayRow = envelopeInfo.ArrayRow
                    });

                    if (find.FrontStyle != envelopeInfo.FrontStyle)
                    {
                        find.FrontStyle = "MIX";
                    }

                    if (find.BackStyle != null && envelopeInfo.BackStyle != null)
                    {
                        if (find.BackStyle.Name != envelopeInfo.BackStyle.Name)
                        {
                            find.BackStyle = new BackStyleInfo
                            {
                                Name = "MIX"
                            };
                        }
                    }

                    find.PostCards.AddRange(envelopeInfo.PostCards);
                });

                return new List<EnvelopeInfo>(envelopeInfos.Values);
            }
        }


        /// <summary>
        ///     用户淘宝ID
        /// </summary>
        public string TaobaoId { get; set; }

        /// <summary>
        ///     订单是否加急
        /// </summary>
        public bool Urgent { get; set; }

        public BatchStatus Status
        {
            get
            {
                var tmpEnvelopeList = SubmitEnvelopeList;
                if (tmpEnvelopeList.Exists(envelopeInfo =>
                    envelopeInfo.PostCardWaste != 0 || envelopeInfo.Status == BatchStatus.EnvelopeNotReady
                ))
                {
                    return BatchStatus.OrderNotReady;
                }

                ;
                if (string.IsNullOrEmpty(TaobaoId))
                {
                    return BatchStatus.OrderNotReady;
                }

                ;
                return tmpEnvelopeList.Exists(envelopeInfo => envelopeInfo.PostCards.Count > 0) ? BatchStatus.OrderAlready : BatchStatus.OrderEmpty;
            }
        }
    }
}