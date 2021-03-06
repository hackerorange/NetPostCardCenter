﻿using Newtonsoft.Json;

namespace postCardCenterSdk.response.file
{
    public class FileUploadResponse
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("imageAvailable")] public bool ImageAvailable { get; set; }
    }
}