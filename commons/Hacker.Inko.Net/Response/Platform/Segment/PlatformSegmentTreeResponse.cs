using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hacker.Inko.Net.Response.Platform.Segment
{
    public class PlatformSegmentTreeResponse
    {
        [JsonProperty("id")] public string Id { set; get; }
        [JsonProperty("parentId")] public string ParentId { set; get; }
        [JsonProperty("code")] public string Code { set; get; }
        [JsonProperty("name")] public string Name { set; get; }
        [JsonProperty("fullName")] public string FullName { set; get; }

        /// <summary>
        /// 流水段类型，5为流水段
        /// </summary>
        [JsonProperty("type")]
        public string Type { set; get; }

        public string ConstructionSectionId { set; get; }
        public string ConstructionSectionCode { set; get; }
        public string ConstructionSectionName { set; get; }
        [JsonProperty("children")] public List<PlatformSegmentTreeResponse> Children { set; get; }
    }
}