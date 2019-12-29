using System;
using Newtonsoft.Json;

namespace Hacker.Inko.Net.Response.order
{
    /// <summary>
    ///     订单中心响应对象
    /// </summary>
    internal class OrderCenterResponse
    {
        /// <summary>
        ///     订单编号
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     处理者姓名
        /// </summary>
        [JsonProperty("processorName")]
        public string ProcessorName { get; set; }

        /// <summary>
        ///     处理者状态
        /// </summary>
        [JsonProperty("processStatus")]
        public string ProcessStatus { get; set; }

        /// <summary>
        ///     订单创建时间
        /// </summary>
        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     纸张类型（此订单下包含的所有纸张类型）
        /// </summary>
        [JsonProperty("paperType")]
        public string PaperType { get; set; }

        /// <summary>
        ///     客户淘宝ID
        /// </summary>
        [JsonProperty("taobaoId")]
        public string TaobaoId { get; set; }

        /// <summary>
        ///     是否加急
        /// </summary>
        [JsonProperty("urgent")]
        public bool Urgent { get; set; }
    }
}