using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace postCardCenterSdk.domain.orderCenter
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EnvelopeInfo
    {
        [JsonProperty("envelopeId")]
        public string EnvelopeId { get; set; }

        [JsonProperty("taobaoId")]
        public string TaobaoId { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("processorName")]
        public string ProcessorName { get; set; }

        [JsonProperty("processStatus")]
        public string ProcessStatus { get; set; }

        [JsonProperty("processStatusCode")]
        public string ProcessStatusCode { get; set; }

        [JsonProperty("productHeight")]
        public string ProductHeight { get; set; }

        [JsonProperty("productWidth")]
        public string ProductWidth { get; set; }

        [JsonProperty("urgent")]
        public bool Urgent { get; set; }

        [JsonProperty("paperType")]
        public string PaperType { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }
    }
}