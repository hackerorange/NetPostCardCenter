using soho.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.helper
{
    public static class OrderInfoHelper
    {
        /// <summary>
        /// 判断是否所有的明信片都已经上传完成；
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public static bool IsAllPostCardUpload(this OrderInfo orderInfo)
        {
            bool flag = true;
            if (orderInfo.Envelopes != null)
            {
                flag = orderInfo.Envelopes.Exists(EnvelopeInfo =>
                {
                    if (EnvelopeInfo.PostCardCount > 0)
                    {
                        return !EnvelopeInfo.PostCards.Exists(PostCardInfo =>{return String.IsNullOrEmpty(PostCardInfo.FileId); });
                    }
                    return false;
                });
            }
            return flag;

        }

    }
}
