using postCardCenterSdk.response.postCard;
using PostCardCrop.model;

namespace PostCardCrop.translator.response
{
    public static class OrderInfoTranslator
    {
        public static OrderInfo TranslateToOrderInfo(this OrderResponse orderResponse)
        {
            return new OrderInfo
            {
                Id = orderResponse.OrderId,
                CreateDate = orderResponse.CreateDate,
                PaperType = orderResponse.PaperType,
                ProcessorName = orderResponse.ProcessorName,
                ProcessStatus = string.IsNullOrEmpty(orderResponse.ProcessStatus) ? "未处理" : orderResponse.ProcessStatus,
                TaobaoId = orderResponse.TaobaoId,
                Urgent = orderResponse.Urgent
            };
        }
    }
}