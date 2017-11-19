using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using SystemSetting.backStyle.model;
using SystemSetting.size.model;
using soho.constant;
using soho.constant.postcard;
using soho.domain;

namespace PostCardCrop.model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PostCardInfo
    {
        private Image _frontImage;

        private int _thumbnailGenerateTime;


        public bool RetryGenerateThumbnail()
        {
            _thumbnailGenerateTime++;
            return _thumbnailGenerateTime <= 3;
        }


        public PostCardInfo()
        {
        }

        public PostCardInfo(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }


        /// <summary>
        ///     明信片ID
        /// </summary>
        public string PostCardId { get; set; }

        /// <summary>
        ///     此文件在文件服务器的ID
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        ///     此文件的缩略图文件ID
        /// </summary>
        public string ThumbnailFileId { get; set; }

        /// <summary>
        ///     明信片原始文件信息
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        ///     此张明信片打印的份数
        /// </summary>
        public int Copy { get; set; }

        /// <summary>
        ///     此张明信片的正面样式
        /// </summary>
        public string FrontStyle { get; set; }

        /// <summary>
        ///     此张明信片的反面样式
        /// </summary>
        public BackStyleInfo BackStyle { get; set; }

        /// <summary>
        ///     文件上传状态，分为：未上传，正在上传，已上传，
        /// </summary>
        public PostCardFileUploadStatusEnum FileUploadStat { get; set; }

        /// <summary>
        ///     文件是否是图片文件，根据Header请求获取文件信息
        /// </summary>
        public bool IsImage { get; set; }

        /// <summary>
        ///     明信片反面文件ID
        /// </summary>
        /// public string BackFileId { get; set; }
        /// <summary>
        ///     文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     裁切信息
        /// </summary>
        public CropInfo CropInfo { get; set; }

        /// <summary>
        ///     处理者名称
        /// </summary>
        public string ProcessorName { get; set; }

        /// <summary>
        ///     处理状态显示文本
        /// </summary>
        public string ProcessStatusText { get; set; }

        public Image FrontImage
        {
            get
            {
                if (_frontImage == null)
                    if (FileInfo != null && FileInfo.Exists)
                        try
                        {
                            _frontImage = Image.FromFile(FileInfo.FullName);
                        }
                        catch
                        {
                            return _frontImage = null;
                        }
                return _frontImage;
            }
        }

        /// <summary>
        ///     成品尺寸（裁切页面使用）
        /// </summary>
        public PostSize ProductSize { get; set; }

        /// <summary>
        ///     当前处理状态枚举
        /// </summary>
        public PostCardProcessStatusEnum ProcessStatus { get; set; }

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