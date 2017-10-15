using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postCardCenterSdk.request.postCard
{
    public class PostCardInfoPatchRequest
    {

        [JsonProperty("postCardId")]
        public String PostCardId { get; set; }

        [JsonProperty("frontStyle")]
        public String FrontStyle { get; set; }

        [JsonProperty("backStyle")]
        public String BackStyle { get; set; }

        [JsonProperty("fileId")]
        public String FileId { get; set; }

        [JsonProperty("backFileId")]
        public String BackFileId { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }
    }
}
