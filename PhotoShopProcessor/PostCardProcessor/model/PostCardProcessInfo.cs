namespace PostCardProcessor.model
{
    public class PostCardProcessInfo
    {
        public string PostCardId { get; set; }
      
        public string PostCardType { get; set; }

        public double CropLeft { get; set; }
        public double CropTop { get; set; }
        public double CropHeight { get; set; }
        public double CropWidth { get; set; }
        public int Rotation { get; set; }

        public int ProductHeight { get; set; }

        public int ProductWidth { get; set; }
    }
}
