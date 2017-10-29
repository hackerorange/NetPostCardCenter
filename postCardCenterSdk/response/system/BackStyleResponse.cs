using Newtonsoft.Json;

namespace postCardCenterSdk.response.system
{
    public class BackStyleResponse
    {
        /// <summary>
        ///     反面样式名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///     反面样式文件ID
        /// </summary>
        [JsonProperty("fileId")]
        public string FileId { get; set; }
    }
}