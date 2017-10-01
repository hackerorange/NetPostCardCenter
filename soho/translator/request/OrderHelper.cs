using postCardCenterSdk.request.order;
using soho.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.translator.request
{
    public static class OrderHelper
    {
        /// <summary>
        /// 将Order转化成提交订单请求对象
        /// </summary>
        /// <param name="order">Order对象</param>
        /// <returns>提交订单请求对象</returns>
        public static OrderSubmitRequest PrepareSubmitRequest(this OrderInfo order)
        {
            OrderSubmitRequest orderSubmit = new OrderSubmitRequest
            {
                TaobaoId = order.TaobaoId,
                Urgent = order.Urgent ? 1 : 0,
                EnvelopeList = new List<OrderSubmitEnvelope>()
            };
            order.Envelopes.ForEach(Envelope =>
            {
                orderSubmit.EnvelopeList.Add(Envelope.PrepareSubmitEnvelope());
            });
            return orderSubmit;
        }

        public static OrderSubmitEnvelope PrepareSubmitEnvelope(this EnvelopeInfo envelope)
        {
            OrderSubmitEnvelope orderSubmitEnvelope = new OrderSubmitEnvelope
            {
                BackStyle = envelope.BackStyle,
                FrontStyle = envelope.FrontStyle,
                DoubleSide = envelope.DoubleSide,
                PaperName = envelope.PaperName,
                ProductHeight = envelope.ProductSize.Height,
                ProductWidth = envelope.ProductSize.Width
            };
            envelope.PostCards.ForEach(postCard =>
            {
                orderSubmitEnvelope.PostCards.Add(postCard.PrepareSubmitPostCard());
            });
            return orderSubmitEnvelope;
        }


        public static OrderSubmitPostCard PrepareSubmitPostCard(this PostCardInfo postCard)
        {
            OrderSubmitPostCard orderSubmitPostCard = new OrderSubmitPostCard()
            {
                BackFileId = postCard.BackFileId,
                BackStyle = postCard.BackStyle,
                Copy = postCard.Copy,
                FileId = postCard.FileId,
                FileName = postCard.FileName,
                FrontStyle = postCard.FrontStyle,
                CustomerBackStyle = postCard.CustomerBackStyle
            };
            return orderSubmitPostCard;
        }

    }
}
