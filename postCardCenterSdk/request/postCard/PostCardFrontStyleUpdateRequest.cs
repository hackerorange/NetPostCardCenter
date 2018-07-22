using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace postCardCenterSdk.request.postCard
{
    public class PostCardFrontStyleUpdateRequest
    {
        [JsonProperty("postCardId")]
        public string PostCardId { get; set; }

        [JsonProperty("frontStyle")]
        public string FrontStyle { get; set; }
    }
}
