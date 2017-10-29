using Newtonsoft.Json;

namespace postCardCenterSdk.response.postCard
{
    public class CropInfoResponse
    {
        /// <summary>
        ///     裁切框X坐标相对于图像尺寸的比例
        /// </summary>
        [JsonProperty("leftScale")]
        public double LeftScale { get; set; }

        /// <summary>
        ///     裁切框Y坐标相对于图像尺寸的比例
        /// </summary>
        [JsonProperty("topScale")]
        public double TopScale { get; set; }

        /// <summary>
        ///     裁切框宽度相对于图像尺寸的比例
        /// </summary>
        [JsonProperty("widthScale")]
        public double WidthScale { get; set; }

        /// <summary>
        ///     裁切框高度相对于图像尺寸的比例
        /// </summary>
        [JsonProperty("heightScale")]
        public double HeightScale { get; set; }

        /// <summary>
        ///     旋转的角度
        /// </summary>
        [JsonProperty("rotation")]
        public int Rotation { get; set; }
    }
}