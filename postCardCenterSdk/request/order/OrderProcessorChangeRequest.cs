using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace postCardCenterSdk.request.order
{
    public class OrderProcessorChangeRequest
    {
        [JsonProperty("orderId")] public string OrderId;
    }
}