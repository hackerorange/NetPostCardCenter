using postCardCenterSdk.response.postCard;
using soho.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.translator.response
{
    public static class OrderInfoTranslator
    {

        public static OrderInfo TranslateToOrderInfo(this OrderResponse orderResponse) => new OrderInfo
        {
            Id=orderResponse.OrderId,
            CreateDate=orderResponse.CreateDate,
            PaperType=orderResponse.PaperType,
            ProcessorName=orderResponse.ProcessorName,
            ProcessStatus=String.IsNullOrEmpty(orderResponse.ProcessStatus)?"未处理": orderResponse.ProcessStatus,
            TaobaoId =orderResponse.TaobaoId,
            Urgent=orderResponse.Urgent
        };

    }
}
