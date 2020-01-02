using Newtonsoft.Json;

namespace Hacker.Inko.Net.Response.system
{
    public class PostCardSizeResponse
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("width")] public int Width { get; set; }

        [JsonProperty("height")] public int Height { get; set; }
    }
}