using System.Collections.Generic;
using System.IO;
using SystemSetting.backStyle.model;
using SystemSetting.size.model;
using soho.domain;

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
                var envelopeList = new List<EnvelopeInfo>();
                EnvelopeInfoList.ForEach(postCardBasic =>
                {
                    if (!(postCardBasic is EnvelopeInfo envelopeInfo) || envelopeInfo.Status == BatchStatus.EnvelopeEmptyPath) return;

                    var find = envelopeList.Find(tmpEnvelope =>
                    {
                        if (tmpEnvelope.DoubleSide != envelopeInfo.DoubleSide) return false;
                        if (tmpEnvelope.ProductSize.Width != envelopeInfo.ProductSize.Width) return false;
                        if (tmpEnvelope.ProductSize.Height != envelopeInfo.ProductSize.Height) return false;
                        if (tmpEnvelope.PaperName != envelopeInfo.PaperName) return false;
                        return true;
                    });
                    if (find == null)
                    {
                        find = (EnvelopeInfo) envelopeInfo.Clone();
                        envelopeList.Add(find);
                    }
                    if (find.FrontStyle != envelopeInfo.FrontStyle)
                        find.FrontStyle = "MIX";
                    if (find.BackStyle != null && envelopeInfo.BackStyle != null)
                        if (find.BackStyle.Name != envelopeInfo.BackStyle.Name)
                            find.BackStyle = new BackStyleInfo
                            {
                                Name = "MIX"
                            };
                    find.PostCards.AddRange(envelopeInfo.PostCards);
                });
                return envelopeList;
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