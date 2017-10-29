using Newtonsoft.Json;

namespace postCardCenterSdk.request.postCard
{
    public class PostCardInfoPatchRequest
    {
        [JsonProperty("postCardId")]
        public string PostCardId { get; set; }

        [JsonProperty("frontStyle")]
        public string FrontStyle { get; set; }

        [JsonProperty("backStyle")]
        public string BackStyle { get; set; }

        [JsonProperty("fileId")]
        public string FileId { get; set; }

        [JsonProperty("backFileId")]
        public string BackFileId { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }
    }
}