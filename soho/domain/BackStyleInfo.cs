using System.Diagnostics.CodeAnalysis;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BackStyleInfo
    {
        public string Name { get; set; }
        public string FileId { get; set; }

        public override string ToString()
        {
            var displayText = Name;

            if (!string.IsNullOrEmpty(Name) && Name.Length > 1 && !string.IsNullOrEmpty(FileId))
                if (FileId.Length > 4)
                    displayText += "[" + FileId.Substring(0, 4) + "]";
                else
                    displayText += "[" + FileId + "]";
            return displayText;
        }
    }
}