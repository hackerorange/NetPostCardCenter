using Newtonsoft.Json;

namespace postCardCenterSdk.request.file
{
    public class PdfGenerateRequest
    {
        [JsonProperty("cropInfo")] public PdfCropInfo CropInfo;
        [JsonProperty("fileId")] public string FileId;
        [JsonProperty("productSize")] public PdfSize ProductSize;

    }

    public class PdfCropInfo
    {
        [JsonProperty("left")] public double Left;
        [JsonProperty("top")] public double Top;
        [JsonProperty("width")] public double Width;
        [JsonProperty("height")] public double Height;
        [JsonProperty("rotation")] public int Rotation;
    }

    public class PdfSize
    {
        [JsonProperty("width")] public double Width;
        [JsonProperty("height")] public double Height;
    }
}