using System;
using System.Drawing;
using System.IO;
using SystemSetting.size.model;
using soho.domain;
using soho.helper;

namespace OrderBatchCreate.model
{
    public sealed class PostCardInfo : PostCardBasic
    {
        private Image _frontImage;

        //public string ThumbnailFileId { get; set; }


        public PostCardInfo(FileSystemInfo fileInfo)
        {
            DirectoryInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
            if (fileInfo is FileInfo tmpFileInfo)
            {
                IsImage = tmpFileInfo.CheckIsImage();
                if (!IsImage)
                    Status = BatchStatus.PostCardTypeError;
            }
        }

        public PostCardInfo(FileSystemInfo fileInfo, EnvelopeInfo envelopeInfo) : this(fileInfo)
        {
            Parent = envelopeInfo ?? throw new ArgumentNullException(nameof(envelopeInfo));
        }

        public override BatchStatus Status { get; set; } = BatchStatus.PostCardBeforeUpload;

        public override PostSize ProductSize => Parent.ProductSize;

        public override int ImageIndex { get; } = 1;

        /// <summary>
        ///     此文件在文件服务器的ID
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        ///     文件是否是图片文件，根据Header请求获取文件信息
        /// </summary>
        public bool IsImage { get; set; }

        /// <summary>
        ///     是否已经上传到服务器
        /// </summary>
        public bool IsUpload { get; set; } = false;

        public override bool DoubleSide => Parent.DoubleSide;

        public override string PaperName => Parent.PaperName;

        ~PostCardInfo()
        {
            if (_frontImage != null)
            {
                _frontImage.Dispose();
                _frontImage = null;
            }
        }
    }
}