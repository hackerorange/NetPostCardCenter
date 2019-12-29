using Newtonsoft.Json;

namespace Hacker.Inko.Net.Response
{
    public class LoginResponse
    {
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        /// <summary>
        ///     TOKEN
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        ///     用户真实姓名
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }
        /// <summary>
        ///     用户真实姓名
        /// </summary>
        [JsonProperty("realName")]
        public string RealName { get; set; }
    }
}