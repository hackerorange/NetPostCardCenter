using System;
using Newtonsoft.Json;

namespace postCardCenterSdk.response.postCard
{
    /// <summary>
    ///     订单相应结果
    /// </summary>
    public class OrderResponse
    {
        /// <summary>
        ///     订单ID
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     订单所属用户的淘宝ID
        /// </summary>
        [JsonProperty("taobaoId")]
        public string TaobaoId { get; set; }

        /// <summary>
        ///     订单的紧急程度，加急为True
        /// </summary>
        [JsonProperty("urgent")]
        public bool Urgent { get; set; }

        /// <summary>
        ///     订单处理人姓名
        /// </summary>
        [JsonProperty("processorName")]
        public string ProcessorName { get; set; }

        /// <summary>
        ///     订单处理状态文本
        /// </summary>
        [JsonProperty("processStatus")]
        public string ProcessStatus { get; set; }

        /// <summary>
        ///     订单创建时间
        /// </summary>
        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     订单纸张类型
        /// </summary>
        [JsonProperty("paperType")]
        public string PaperType { get; set; }
    }
}