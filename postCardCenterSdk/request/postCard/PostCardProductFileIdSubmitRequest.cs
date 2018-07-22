using Newtonsoft.Json;

namespace postCardCenterSdk.request.postCard
{
    public class PostCardProductFileIdSubmitRequest
    {
        [JsonProperty("postCardId")]
        public string PostCardId { get; set; }
        [JsonProperty("productFileId")]
        public string ProductFileId { get; set; }
    }
}