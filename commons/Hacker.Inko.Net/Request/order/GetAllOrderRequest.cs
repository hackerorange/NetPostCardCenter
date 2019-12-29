using System;
using Newtonsoft.Json;

namespace Hacker.Inko.Net.Request.order
{
    class GetAllOrderRequest
    {

        [JsonProperty("startDate")]
        public DateTime StarDateTime { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDateTime { get; set; }
    }

}
