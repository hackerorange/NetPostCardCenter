using Newtonsoft.Json;

namespace postCardCenterSdk.response.system
{
    public class PostCardSizeResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}