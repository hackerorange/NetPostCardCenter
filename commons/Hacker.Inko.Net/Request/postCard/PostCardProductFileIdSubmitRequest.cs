using Newtonsoft.Json;

namespace Hacker.Inko.Net.Request.postCard
{
    public class PostCardProductFileIdSubmitRequest
    {
        [JsonProperty("postCardId")]
        public string PostCardId { get; set; }
        [JsonProperty("productFileId")]
        public string ProductFileId { get; set; }
    }
}