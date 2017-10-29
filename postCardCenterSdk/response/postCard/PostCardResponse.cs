using Newtonsoft.Json;

namespace postCardCenterSdk.response.postCard
{
    public class PostCardResponse
    {
        /// <summary>
        ///     明信片ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///     此文件在文件服务器的ID
        /// </summary>
        [JsonProperty("fileId")]
        public string FileId { get; set; }

        /// <summary>
        ///     此文件在文件服务器的缩略图ID
        /// </summary>
        [JsonProperty("thumbnailFileId")]
        public string ThumbnailFileId { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

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
        ///     明信片反面文件ID
        /// </summary>
        [JsonProperty("backFileId")]
        public string BackFileId { get; set; }

        /// <summary>
        ///     明信片处理者姓名
        /// </summary>
        [JsonProperty("processorName")]
        public string ProcessorName { get; set; }

        /// <summary>
        ///     明信片处理状态
        /// </summary>
        [JsonProperty("processStatusText")]
        public string ProcessStatusText { get; set; }

        /// <summary>
        ///     处理状态码
        /// </summary>
        [JsonProperty("processStatus")]
        public int ProcessStatus { get; set; }

        /// <summary>
        ///     明信片裁切信息
        /// </summary>
        [JsonProperty("cropInfo")]
        public CropInfoResponse CropInfo { get; set; }
    }
}