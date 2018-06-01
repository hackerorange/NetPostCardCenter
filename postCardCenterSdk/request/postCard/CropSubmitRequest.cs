using Newtonsoft.Json;

namespace postCardCenterSdk.request.postCard
{
    public class CropSubmitRequest
    {
        /// <summary>
        ///     明信片ID
        /// </summary>
        [JsonProperty("fileId")]
        public string FileId { get; set; }

        [JsonProperty("productSize")]
        public CropProductSize ProductSize { get; set; }

        [JsonProperty("cropInfo")]
        public CropInfoSubmitDto CropInfo { get; set; }

        [JsonProperty("frontStyle")]
        public string Style { get; set; }
    }

    public class CropInfoSubmitDto
    {
        /// <summary>
        ///     左侧裁切位置
        /// </summary>
        [JsonProperty("left")]
        public double CropLeft { get; set; }

        /// <summary>
        ///     顶部裁切位置
        /// </summary>
        [JsonProperty("top")]
        public double CropTop { get; set; }

        /// <summary>
        ///     裁切高度
        /// </summary>
        [JsonProperty("height")]
        public double CropHeight { get; set; }

        /// <summary>
        ///     裁切宽度
        /// </summary>
        [JsonProperty("width")]
        public double CropWidth { get; set; }

        /// <summary>
        ///     图像旋转角度
        /// </summary>
        [JsonProperty("rotation")]
        public int Rotation { get; set; }
    }

    public class CropProductSize
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}