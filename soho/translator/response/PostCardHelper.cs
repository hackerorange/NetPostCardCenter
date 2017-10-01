using postCardCenterSdk.response.postCard;
using soho;
using soho.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.translator.response
{
   public static class PostCardHelper
    {

        public static PostCardInfo TranlateToPostCard(this PostCardResponse postCardResponse)
        {
            var result = new PostCardInfo
            {
                FileId = postCardResponse.ThumbnailFileId,
                FileName = postCardResponse.FileName,
                BackFileId = postCardResponse.BackFileId,
                BackStyle = postCardResponse.BackStyle,
                Copy = postCardResponse.Copy,
                FrontStyle = postCardResponse.FrontStyle,
                PostCardId = postCardResponse.Id,
                ProcessorName = postCardResponse.ProcessorName,
                ProcessStatus = postCardResponse.ProcessStatusText,
                ProcessStatusCode = postCardResponse.ProcessStatus
            };
            if (postCardResponse.CropInfo != null)
            {
                result.CropInfo = postCardResponse.CropInfo.TranlateToCropInfo();
            };
            return result;
        }

        public static CropInfo TranlateToCropInfo(this CropInfoResponse postCardResponse) => new CropInfo
        {
            HeightScale= postCardResponse.HeightScale,
            WidthScale=postCardResponse.WidthScale,
            LeftScale=postCardResponse.LeftScale,
            Rotation=postCardResponse.Rotation,
            TopScale=postCardResponse.TopScale
        };

    }


}
