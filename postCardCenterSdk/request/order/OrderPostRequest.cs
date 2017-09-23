using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postCardCenterSdk.request.order
{
    class OrderPostRequest
    {
        /// <summary>
        /// 此订单下的所有明信片集合
        /// </summary>
        [JsonProperty("collection")]
        public List<Envelope> Collection { get; set; }
        /// <summary>
        /// 此订单所属用户淘宝ID
        /// </summary>
        [JsonProperty("taobaoId")]
        public string TaobaoId { get; set; }
        /// <summary>
        /// 此订单是否加急
        /// </summary>
        [JsonProperty("urgent")]
        public bool Urgent { get; set; }

       





    }

    class Envelope
    {
        private String


    }


}
