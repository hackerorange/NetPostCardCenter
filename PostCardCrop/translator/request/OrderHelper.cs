using System.Collections.Generic;
using postCardCenterSdk.request.order;
using PostCardCrop.model;

namespace PostCardCrop.translator.request
{
    public static class OrderHelper
    {

        public static OrderSubmitEnvelope PrepareSubmitEnvelope(this EnvelopeInfo envelope)
        {
            var orderSubmitEnvelope = new OrderSubmitEnvelope
            {
                FrontStyle = envelope.FrontStyle,
                DoubleSide = envelope.DoubleSide,
                PaperName = envelope.PaperName,
                ProductHeight = envelope.ProductSize.Height,
                ProductWidth = envelope.ProductSize.Width,
                PaperColumn = envelope.ArrayColumn,
                PaperRow = envelope.ArrayRow,
                PaperHeight = envelope.PaperSize.Height,
                PaperWidth = envelope.PaperSize.Width
            };
            //如果反面样式不为null，返回将反面样式提交上去
            if (envelope.BackStyle != null)
            {
                orderSubmitEnvelope.BackStyle = envelope.BackStyle.Name;
            }
            //准备明信片信息
            envelope.PostCards.ForEach(postCard =>
            {
                orderSubmitEnvelope.PostCards.Add(postCard.PrepareSubmitPostCard());
            });
            return orderSubmitEnvelope;
        }


        public static OrderSubmitPostCard PrepareSubmitPostCard(this PostCardInfo postCard)
        {
            var orderSubmitPostCard = new OrderSubmitPostCard
            {
                Copy = postCard.Copy,
                FileId = postCard.FileId,
                
                FileName = postCard.FileName,
                FrontStyle = postCard.FrontStyle
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