using SystemSetting.backStyle.model;
using Hacker.Inko.Net.Response.postCard;
using postCardCenterSdk.constant.postcard;
using PostCardCrop.model;


public static class PostCardHelper
{
    public static PostCardInfo TranlateToPostCard(this PostCardResponse postCardResponse)
    {
        var result = new PostCardInfo
        {
            ProductFileId = postCardResponse.ProductFileId,
            FileId = postCardResponse.FileId,
            FileName = postCardResponse.FileName,
            BackFileId = postCardResponse.BackFileId,
            BackProductFileId = postCardResponse.BackProductFileId,
            BackStyle = new BackStyleInfo
            {
                FileId = postCardResponse.BackFileId,
                Name = postCardResponse.BackStyle
            },
            Copy = postCardResponse.Copy,
            FrontStyle = postCardResponse.FrontStyle,
            PostCardId = postCardResponse.Id,
            ProcessorName = postCardResponse.ProcessorName,
            ProcessStatusText = postCardResponse.ProcessStatusText,
            ProcessStatus = (PostCardProcessStatusEnum) postCardResponse.ProcessStatus
        };
        return result;
    }

    public static CropInfo TranlateToCropInfo(this CropInfoResponse postCardResponse)
    {
        return new CropInfo
        {
            HeightScale = postCardResponse.HeightScale,
            WidthScale = postCardResponse.WidthScale,
            LeftScale = postCardResponse.LeftScale,
            Rotation = postCardResponse.Rotation,
            TopScale = postCardResponse.TopScale
        };
    }
}