using postCardCenterSdk.response.postCard;
using PostCardCrop.model;

namespace PostCardCrop.translator.response
{
    public static class OrderInfoTranslator
    {
        public static OrderInfo TranslateToOrderInfo(this OrderResponse orderResponse) => new OrderInfo
        {
            Id = orderResponse.OrderId,
            CreateDate = orderResponse.CreateDate,
            PaperType = orderResponse.PaperType,
            ProcessorName = orderResponse.ProcessorName,
            ProcessUserId = orderResponse.ProcessUserId,
            ProcessStatus = string.IsNullOrEmpty(orderResponse.ProcessStatus) ? "未处理" : orderResponse.ProcessStatus,
            ProcessStatusCode= orderResponse.ProcessStatusCode,
            TaobaoId = orderResponse.TaobaoId,
            Urgent = orderResponse.Urgent
        };
    }
}