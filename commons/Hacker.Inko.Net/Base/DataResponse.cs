using Newtonsoft.Json;

namespace Hacker.Inko.Net.Base
{
    public class DataResponse<T>
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
    }
}