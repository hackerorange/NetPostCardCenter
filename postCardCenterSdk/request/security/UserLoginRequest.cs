using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace postCardCenterSdk.request.security
{
    public class UserLoginRequest
    {
        [JsonProperty("userName")] public string UserName { get; set; }

        [JsonProperty("password")] public string Password { get; set; }
    }
}