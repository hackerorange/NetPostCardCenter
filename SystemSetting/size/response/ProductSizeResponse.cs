using Newtonsoft.Json;

namespace SystemSetting.size.response
{
    public class ProductSizeResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}