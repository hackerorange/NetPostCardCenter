using Newtonsoft.Json;

namespace Hacker.Inko.Net.Request
{
    public class UserLoginRequest
    {
        [JsonProperty("userName")] public string UserName { get; set; }

        [JsonProperty("password")] public string Password { get; set; }
    }
}