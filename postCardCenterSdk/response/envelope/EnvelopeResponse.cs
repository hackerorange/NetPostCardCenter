using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postCardCenterSdk.response.envelope
{
    public class EnvelopeResponse
    {
        /// <summary>
        /// 此明信片所属订单ID
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId;

        /// <summary>
        /// 明信片集合ID
        /// </summary>
        [JsonProperty("envelopeId")]
        public string EnvelopeId { get; set; }

        /// <summary>
        /// 纸张类型
        /// </summary>
        [JsonProperty("paperName")]
        public string PaperName { get; set; }

        /// <summary>
        /// 是否为双面
        /// </summary>
        [JsonProperty("doubleSide")]
        public bool DoubleSide { get; set; }

        /// <summary>
        /// 明信片正面样式
        /// </summary>
        [JsonProperty("frontStyle")]
        public string FrontStyle { get; set; }

        /// <summary>
        /// 明信片反面样式
        /// </summary>
        [JsonProperty("backStyle")]
        public string BackStyle { get; set; }

        /// <summary>
        /// 成品宽度
        /// </summary>
        [JsonProperty("productWidth")]
        public int ProductWidth { get; set; }

        /// <summary>
        /// 成品高度
        /// </summary>
        [JsonProperty("productHeight")]
        public int ProductHeight { get; set; }

        /// <summary>
        /// 成品文件ID
        /// </summary>
        [JsonProperty("productFileId")]
        public string ProductFileId { get; set; }

    }
}
