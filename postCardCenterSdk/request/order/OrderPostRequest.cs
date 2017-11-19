using System.Collections.Generic;
using Newtonsoft.Json;

namespace postCardCenterSdk.request.order
{
    /// <summary>
    ///     提交订单的对象
    /// </summary>
    public class OrderSubmitRequest
    {
        /// <summary>
        ///     此订单下的所有明信片集合
        /// </summary>
        [JsonProperty("envelopes")]
        public List<OrderSubmitEnvelope> EnvelopeList { get; set; }

        /// <summary>
        ///     此订单所属用户淘宝ID
        /// </summary>
        [JsonProperty("taobaoId")]
        public string TaobaoId { get; set; }

        /// <summary>
        ///     此订单是否加急
        /// </summary>
        [JsonProperty("urgent")]
        public int Urgent { get; set; }

        [JsonProperty("selfProcess")]
        public bool SelfProcess { get; set; }
    }

    /// <summary>
    ///     提交订单时，上传的明信片集合信息
    /// </summary>
    public class OrderSubmitEnvelope
    {
        public OrderSubmitEnvelope()
        {
            PostCards = new List<OrderSubmitPostCard>();
        }

        /// <summary>
        ///     纸张类型
        /// </summary>
        [JsonProperty("paperName")]
        public string PaperName { get; set; }

        /// <summary>
        ///     是否为双面
        /// </summary>
        [JsonProperty("doubleSide")]
        public bool DoubleSide { get; set; }

        /// <summary>
        ///     明信片板式ID
        /// </summary>
        [JsonProperty("frontStyle")]
        public string FrontStyle { get; set; }

        /// <summary>
        ///     明信片反面样式
        /// </summary>
        [JsonProperty("backStyle")]
        public string BackStyle { get; set; }

        /// <summary>
        ///     成品宽度
        /// </summary>
        [JsonProperty("productWidth")]
        public decimal ProductWidth { get; set; }

        /// <summary>
        ///     成品高度
        /// </summary>
        [JsonProperty("productHeight")]
        public decimal ProductHeight { get; set; }


        /// <summary>
        ///     纸张宽度
        /// </summary>
        [JsonProperty("paperWidth")]
        public decimal PaperWidth { get; set; }

        /// <summary>
        ///     纸张高度
        /// </summary>
        [JsonProperty("paperHeight")]
        public decimal PaperHeight { get; set; }

        /// <summary>
        ///     一张纸上排几列
        /// </summary>
        [JsonProperty("paperColumn")]
        public decimal PaperColumn { get; set; }

        /// <summary>
        ///     一张纸上排几行
        /// </summary>
        [JsonProperty("paperRow")]
        public decimal PaperRow { get; set; }

        /// <summary>
        ///     此明信片集合下的所有明信片信息
        /// </summary>
        [JsonProperty("postCards")]
        public List<OrderSubmitPostCard> PostCards { get; set; }
    }

    /// <summary>
    ///     提交订单时，上传的明信片信息
    /// </summary>
    public class OrderSubmitPostCard
    {
        /// <summary>
        ///     此文件在文件服务器的ID
        /// </summary>
        [JsonProperty("fileId")]
        public string FileId { get; set; }

        /// <summary>
        ///     此文件在文件服务器的ID
        /// </summary>
        [JsonProperty("thumbnailFileId")]
        public string ThumbnailFileId { get; set; }


        /// <summary>
        ///     此张明信片打印的份数
        /// </summary>
        [JsonProperty("copy")]
        public int Copy { get; set; }

        /// <summary>
        ///     此张明信片的正面样式
        /// </summary>
        [JsonProperty("frontStyle")]
        public string FrontStyle { get; set; }

        /// <summary>
        ///     此张明信片的反面样式
        /// </summary>
        [JsonProperty("backStyle")]
        public string BackStyle { get; set; }

        /// <summary>
        ///     明信片反面是否自定义
        /// </summary>
        [JsonProperty("customerBackStyle")]
        public bool CustomerBackStyle { get; set; }

        /// <summary>
        ///     明信片反面文件ID
        /// </summary>
        [JsonProperty("backFileId")]
        public string BackFileId { get; set; }

        /// <summary>
        ///     提交的明信片文件名称
        /// </summary>
        [JsonProperty("fileName")]
        public string FileName { get; set; }
    }
}