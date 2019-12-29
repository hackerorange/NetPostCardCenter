using Newtonsoft.Json;

namespace Hacker.Inko.Net.Request.postCard
{
    public class PostCardFrontStyleUpdateRequest
    {
        [JsonProperty("postCardId")]
        public string PostCardId { get; set; }

        [JsonProperty("frontStyle")]
        public string FrontStyle { get; set; }
    }
}
