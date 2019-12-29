using Newtonsoft.Json;

namespace Hacker.Inko.Net.Response.CloudT
{
    public class CloudTLoginResponse
    {
        [JsonProperty("success")] public bool Success { get; set; } = true;
        [JsonProperty("errorMsg")] public string ErrorMsg { get; set; }
        [JsonProperty("errorCode")] public string ErrorCode { get; set; }
        [JsonProperty("cloudToken")] public string CloudToken { get; set; }
        [JsonProperty("realname")] public string RealName { get; set; }
        [JsonProperty("userId")] public string UserId { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
    }
}