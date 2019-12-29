using System.IO;

namespace SystemSetting.backStyle.model
{
    public class CustomerBackStyleInfo : BackStyleInfo
    {
        public CustomerBackStyleInfo(FileInfo fileInfo)
        {
            Name = "自定义";
            FileInfo = fileInfo;
        }

        public FileInfo FileInfo { get; set; }
    }
}