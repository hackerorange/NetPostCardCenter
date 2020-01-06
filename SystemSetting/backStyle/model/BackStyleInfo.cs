using System.Diagnostics.CodeAnalysis;

namespace SystemSetting.backStyle.model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BackStyleInfo
    {
        public string Name { get; set; }
        public string FileId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}