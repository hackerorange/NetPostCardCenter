namespace PostCardQueueProcessor.model
{
    public class DoubleSidePostCardCropInfo
    {
        /// <summary>
        /// 明信片ID
        /// </summary>
        public string PostCardId { get; set; }

        /// <summary>
        /// 正面样式
        /// </summary>
        public string PostCardType { get; set; }

        /// <summary>
        /// 成品高度
        /// </summary>
        public int ProductHeight { get; set; }

        /// <summary>
        /// 成品宽度
        /// </summary>
        public int ProductWidth { get; set; }

        /// <summary>
        /// 正面裁切信息
        /// </summary>
        public PostCardProcessCropInfo FrontCropCropInfo { get; set; }

        /// <summary>
        /// 反面裁切信息
        /// </summary>
        public PostCardProcessCropInfo BackCropCropInfo { get; set; }
    }

    public class PostCardProcessCropInfo
    {
        public string FileId { get; set; }
        public long ImageWidth { get; set; }
        public long ImageHeight { get; set; }
        public double CropLeft { get; set; }
        public double CropTop { get; set; }
        public double CropHeight { get; set; }
        public double CropWidth { get; set; }
        public int Rotation { get; set; }
    }
}