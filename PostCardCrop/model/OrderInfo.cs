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
            //BaseEnvelope = new EnvelopeInfo();
            //初始化明信片集合
            //Envelopes = new List<EnvelopeInfo>();
        }


        public string ProcessUserId { get; set; }

        /// <summary>
        ///     处理者名称
        /// </summary>
        public string ProcessorName { get; set; }

        /// <summary>
        ///     处理状态
        /// </summary>
        public string ProcessStatus { get; set; }


        /// <summary>
        ///     处理状态编码
        ///     2:已下载
        ///     4:已完成
        ///     8:未完成
        /// </summary>
        public string ProcessStatusCode { get; set; }


        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     纸张类型
        /// </summary>
        public string PaperType { get; set; }

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
    }
}