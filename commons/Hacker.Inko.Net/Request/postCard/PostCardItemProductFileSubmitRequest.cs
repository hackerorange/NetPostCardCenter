using Newtonsoft.Json;

namespace Hacker.Inko.Net.Request.postCard
{
    public class PostCardItemProductFileSubmitRequest
    {
        [JsonProperty("frontProductFileId")] public string FrontProductFileId { get; set; }
        [JsonProperty("backProductFileId")] public string BackProductFileId { get; set; }
    }
}