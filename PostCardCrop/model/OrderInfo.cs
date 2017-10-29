using System;
using System.Collections.Generic;
using System.Linq;

namespace PostCardCrop.model
{
    public class OrderInfo
    {
        public OrderInfo()
        {
            //订单根目录只有一个明信片集合
            BaseEnvelope = new EnvelopeInfo();
            //初始化明信片集合
            //Envelopes = new List<EnvelopeInfo>();
        }

        /// <summary>
        ///     处理者名称
        /// </summary>
        public string ProcessorName { get; set; }

        /// <summary>
        ///     处理状态
        /// </summary>
        public string ProcessStatus { get; set; }

        /// <summary>
        ///     订单状态
        /// </summary>
        public string OrderStatus { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     纸张类型
        /// </summary>
        public string PaperType { get; set; }

        /// <summary>
        ///     明信片集合
        /// </summary>
        public List<EnvelopeInfo> Envelopes => null;

        /// <summary>
        ///     基准路径明信片集合
        /// </summary>
        public EnvelopeInfo BaseEnvelope { get; set; }

        /// <summary>
        ///     订单ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     淘宝ID
        /// </summary>
        public string TaobaoId { get; set; }

        /// <summary>
        ///     是否加急
        /// </summary>
        public bool Urgent { get; set; }

        /// <summary>
        ///     此订单所处文件夹
        /// </summary>
        //  public DirectoryInfo Directory { get; set; }


        public double FileUploadPercent
        {
            get
            {
                var total = 0;
                var uploaded = 0;

                uploaded = Envelopes.Sum(EnvelopeInfo =>
                {
                    return EnvelopeInfo.PostCards.Sum(PostCardInfo =>
                    {
                        total++;
                        return string.IsNullOrEmpty(PostCardInfo.FileId) ? 0 : 1;
                    });
                });
                return 100 * uploaded / (double) total;
            }
        }
    }
}