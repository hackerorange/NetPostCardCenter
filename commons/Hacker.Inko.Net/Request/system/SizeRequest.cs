using Newtonsoft.Json;

namespace Hacker.Inko.Net.Request.system
{
    public class SizeRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}