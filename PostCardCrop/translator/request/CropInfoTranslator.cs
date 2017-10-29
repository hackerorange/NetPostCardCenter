using postCardCenterSdk.request.postCard;
using PostCardCrop.model;

namespace PostCardCrop.translator.request
{
    public static class CropInfoTranslator
    {
        public static CropSubmitRequest PrepareCropInfoRequest(this CropInfo cropInfo, string postCardId)
        {
            return new CropSubmitRequest
            {
                CropHeight = cropInfo.HeightScale,
                CropTop = cropInfo.TopScale,
                CropLeft = cropInfo.LeftScale,
                CropWidth = cropInfo.WidthScale,
                Rotation = cropInfo.Rotation,
                PostCardId = postCardId
            };
        }
    }
}