using postCardCenterSdk.request.postCard;
using soho.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.translator.request
{
    public static class CropInfoTranslator
    {

        public static CropSubmitRequest PrepareCropInfoRequest(this CropInfo cropInfo, string postCardId) => new CropSubmitRequest
        {
            CropHeight=cropInfo.HeightScale,
            CropTop=cropInfo.TopScale,
            CropLeft=cropInfo.LeftScale,
            CropWidth=cropInfo.WidthScale,
            Rotation=cropInfo.Rotation,
            PostCardId=postCardId
        };

    }
}
