using Newtonsoft.Json;

namespace Hacker.Inko.Net.Response.system
{
    public class BackStyleResponse
    {
        [JsonProperty("backStyleName")] public string Name { get; set; }
        [JsonProperty("fileId")] public string FileId { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
    }
}