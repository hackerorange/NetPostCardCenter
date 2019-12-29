using System.Collections.Generic;
using SystemSetting.backStyle.model;
using Hacker.Inko.Net.Response.envelope;
using Hacker.Inko.PostCard.Library;
using PostCardCrop.model;


namespace PostCardCrop.translator.response
{
    public static class EnvelopeHelper
    {
        /// <summary>
        ///     将明信片请求结果转化为明信片对象
        /// </summary>
        /// <param name="envelopeResponse">明信片请求结果</param>
        /// <param name="orderId">订单ID</param>
        /// <returns>明信片对象</returns>
        public static EnvelopeInfo TranslateToEnvelope(this EnvelopeResponse envelopeResponse)
        {
            return new EnvelopeInfo
            {
                Id = envelopeResponse.EnvelopeId,
                BackStyle = new BackStyleInfo
                {
                    Name = envelopeResponse.BackStyle
                },
                DoubleSide = envelopeResponse.DoubleSide,
                FrontStyle = envelopeResponse.FrontStyle,
                PaperName = envelopeResponse.PaperName,
                ProductSize = new PostSize
                {
                    Height = envelopeResponse.ProductHeight,
                    Width = envelopeResponse.ProductWidth,
                    Name = envelopeResponse.ProductHeight + "×" + envelopeResponse.ProductWidth
                },
                ProductFileId = envelopeResponse.ProductFileId,
                PostCards = new List<PostCardInfo>(),
                OrderId = envelopeResponse.OrderId
            };
        }
    }
}