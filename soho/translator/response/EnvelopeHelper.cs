using postCardCenterSdk.response.envelope;
using soho.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.translator.response
{
    public static class EnvelopeHelper
    {

        /// <summary>
        /// 将明信片请求结果转化为明信片对象
        /// </summary>
        /// <param name="envelopeResponse">明信片请求结果</param>
        /// <param name="orderId">订单ID</param>
        /// <returns>明信片对象</returns>
        public static EnvelopeInfo TranslateToEnvelope(this EnvelopeResponse envelopeResponse) => new EnvelopeInfo
        {
            Id = envelopeResponse.EnvelopeId,
            BackStyle = envelopeResponse.BackStyle,
            DoubleSide = envelopeResponse.DoubleSide,
            FrontStyle = envelopeResponse.FrontStyle,
            PaperName = envelopeResponse.PaperName,
            ProductSize = new PostSize
            {
                Height = envelopeResponse.ProductHeight,
                Width = envelopeResponse.ProductWidth,
                Name = envelopeResponse.ProductHeight.ToString() + "×" + envelopeResponse.ProductWidth.ToString()
            },
            ProductFileId = envelopeResponse.ProductFileId,
            PostCards = new List<PostCardInfo>(),
            OrderId= envelopeResponse.OrderId
        };
    }
}
