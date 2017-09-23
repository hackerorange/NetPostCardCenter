using Newtonsoft.Json;
using System.Collections.Generic;

namespace postCardCenterSdk.response
{

    public class BodyResponse<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }    
}