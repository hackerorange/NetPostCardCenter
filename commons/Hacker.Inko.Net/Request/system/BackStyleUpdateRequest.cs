using Newtonsoft.Json;

namespace Hacker.Inko.Net.Request.system
{
    public class BackStyleUpdateRequest
    {
        [JsonProperty("backStyleName")] public string Name { get; set; }
        [JsonProperty("fileId")] public string FileId { get; set; }
    }
}