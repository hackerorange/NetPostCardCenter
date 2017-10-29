using Newtonsoft.Json;

namespace postCardCenterSdk.response.file
{
    public class FileUploadResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}