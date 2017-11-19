using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace soho.model
{
    public class ImageFileUploadModel
    {
        public string FileId { get; set; }

        public string ThumbnailFileId { get; set; }

        public bool ImageAvailable { get; set; }
    }
}