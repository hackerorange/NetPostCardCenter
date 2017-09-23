using postCardCenterSdk.request.order;
using soho.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.helper
{
    public static class OrderHelper
    {
        /// <summary>
        /// 将Order转化成提交订单请求对象
        /// </summary>
        /// <param name="order">Order对象</param>
        /// <returns>提交订单请求对象</returns>
        public static OrderSubmitRequest PrepareSubmitRequest(this Order order)
        {
            OrderSubmitRequest orderSubmit = new OrderSubmitRequest
            {
                TaobaoId = order.taobaoId,
                Urgent = order.urgent ? 1 : 0,
                EnvelopeList = new List<OrderSubmitEnvelope>()
            };
            order.envelopes.ForEach(Envelope =>
            {
                orderSubmit.EnvelopeList.Add(Envelope.PrepareSubmitEnvelope());
            });
            return orderSubmit;
        }

        public static OrderSubmitEnvelope PrepareSubmitEnvelope(this Envelope envelope)
        {
            OrderSubmitEnvelope orderSubmitEnvelope = new OrderSubmitEnvelope
            {
                BackStyle = envelope.backStyle,
                FrontStyle = envelope.frontStyle,
                DoubleSide = envelope.doubleSide,
                PaperName = envelope.paperName,
                ProductHeight = envelope.productHeight,
                ProductWidth = envelope.productWidth
            };
            envelope.postCards.ForEach(postCard =>
            {
                orderSubmitEnvelope.PostCards.Add(postCard.PrepareSubmitPostCard());
            });
            return orderSubmitEnvelope;
        }


        public static OrderSubmitPostCard PrepareSubmitPostCard(this PostCard postCard)
        {
            OrderSubmitPostCard orderSubmitPostCard = new OrderSubmitPostCard()
            {
                BackFileId = postCard.backFileId,
                BackStyle = postCard.backStyle,
                Copy = postCard.copy,
                FileId = postCard.fileId,
                FileName = postCard.fileName,
                FrontStyle = postCard.frontStyle,
                CustomerBackStyle = postCard.customerBackStyle
            };
            return orderSubmitPostCard;
        }

    }
}
