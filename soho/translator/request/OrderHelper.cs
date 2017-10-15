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
                            
                FrontStyle = envelope.FrontStyle,
                DoubleSide = envelope.DoubleSide,
                PaperName = envelope.PaperName,
                ProductHeight = envelope.ProductSize.Height,
                ProductWidth = envelope.ProductSize.Width,
                PaperColumn=envelope.ArrayColumn,
                PaperRow=envelope.ArrayRow,
                PaperHeight=envelope.PaperSize.Height,
                PaperWidth=envelope.PaperSize.Width
                
            };
            //如果反面样式不为null，返回将反面样式提交上去
            if (envelope.BackStyle != null)
            {
                orderSubmitEnvelope.BackStyle = envelope.BackStyle.Name;                
            }
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
                Copy = postCard.Copy,
                FileId = postCard.FileId,                
                FileName = postCard.FileName,
                FrontStyle = postCard.FrontStyle,                
            };
            if (postCard.BackStyle != null)
            {
                orderSubmitPostCard.BackFileId = postCard.BackStyle.FileId;
                orderSubmitPostCard.BackStyle = postCard.BackStyle.Name;
            }
            return orderSubmitPostCard;
        }

    }
}
