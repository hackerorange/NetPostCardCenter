using Newtonsoft.Json;

namespace Hacker.Inko.Net.Request.system
{
    public class BackStyleCreateRequest
    {
        [JsonProperty("backStyleName")] public string Name { get; set; }
        [JsonProperty("fileId")] public string FileId { get; set; }
    }
}