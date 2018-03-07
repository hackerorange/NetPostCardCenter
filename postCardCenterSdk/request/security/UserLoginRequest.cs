using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace postCardCenterSdk.request.security
{
    public class UserLoginRequest
    {
        [JsonProperty("userName")] public string UserName;
        [JsonProperty("password")] public string Password;
    }
}