using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace postCardCenterSdk.request.order
{
    class GetAllOrderRequest
    {

        [JsonProperty("startDate")]
        public DateTime StarDateTime { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDateTime { get; set; }
    }

}
