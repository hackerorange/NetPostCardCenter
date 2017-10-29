using Newtonsoft.Json;

namespace postCardCenterSdk.request.postCard
{
    public class CropSubmitRequest
    {
        /// <summary>
        ///     明信片ID
        /// </summary>
        [JsonProperty("PostCardId")]
        public string PostCardId { get; set; }

        /// <summary>
        ///     左侧裁切位置
        /// </summary>
        [JsonProperty("cropLeft")]
        public double CropLeft { get; set; }

        /// <summary>
        ///     顶部裁切位置
        /// </summary>
        [JsonProperty("cropTop")]
        public double CropTop { get; set; }

        /// <summary>
        ///     裁切高度
        /// </summary>
        [JsonProperty("cropHeight")]
        public double CropHeight { get; set; }

        /// <summary>
        ///     裁切宽度
        /// </summary>
        [JsonProperty("cropWidth")]
        public double CropWidth { get; set; }

        /// <summary>
        ///     图像旋转角度
        /// </summary>
        [JsonProperty("rotation")]
        public int Rotation { get; set; }
    }
}