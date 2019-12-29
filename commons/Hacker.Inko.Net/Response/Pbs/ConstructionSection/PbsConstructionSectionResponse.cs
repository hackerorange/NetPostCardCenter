using Newtonsoft.Json;

namespace Hacker.Inko.Net.Response.Pbs.ConstructionSection
{
    public class PbsConstructionSectionResponse
    {
        /// <summary>
        /// 施工段ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { set; get; }

        /// <summary>
        /// 施工段父节点ID
        /// </summary>
        [JsonProperty("pid")]
        public string Pid { set; get; }

        /// <summary>
        /// 施工段名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { set; get; }

        /// <summary>
        /// 施工段编码
        /// </summary>
        [JsonProperty("code")]
        public string Code { set; get; }

        /// <summary>
        /// 施工段类型
        /// </summary>
        [JsonProperty("constructionSectionType")]
        public string ConstructionSectionType { set; get; }
    }
}