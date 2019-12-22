using Newtonsoft.Json;

namespace postCardCenterSdk.web.response.file
{
    public class CropSubmitRequest
    {
        /// <summary>
        ///     明信片ID
        /// </summary>
        [JsonProperty("fileId")]
        public string FileId { get; set; }

        [JsonProperty("productHeight")]
        public int ProductHeight { get; set; }

        [JsonProperty("productWidth")]
        public int ProductWidth { get; set; }

        [JsonProperty("frontStyle")]
        public string Style { get; set; }
        [JsonProperty("cropHeight")]
        public double CropHeight { get; set; }
        [JsonProperty("cropTop")]
        public double CropTop { get; set; }
        [JsonProperty("cropLeft")]
        public double CropLeft { get; set; }
        [JsonProperty("cropWidth")]
        public double CropWidth { get; set; }
        [JsonProperty("rotation")]
        public int Rotation { get; set; }
    }
}