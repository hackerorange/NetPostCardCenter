using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using PostCardCenter.constant;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PostCardInfo
    {
        /// <summary>
        /// 明信片ID
        /// </summary>
        public string PostCardId { get; set; }

        /// <summary>
        /// 此文件在文件服务器的ID
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 明信片原始文件信息
        /// </summary>
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// 此张明信片打印的份数
        /// </summary>
        public int Copy { get; set; }

        /// <summary>
        /// 此张明信片的正面样式
        /// </summary>
        public string FrontStyle { get; set; }

        /// <summary>
        /// 此张明信片的反面样式
        /// </summary>
        public BackStyleInfo BackStyle { get; set; }

        /// <summary>
        /// 文件上传状态，分为：未上传，正在上传，已上传，
        /// </summary>
        public PostCardFileUploadStatusEnum FileUploadStat { get; set; }

        /// <summary>
        /// 文件是否是图片文件，根据Header请求获取文件信息
        /// </summary>
        public bool IsImage { get; set; }
                
        /// <summary>
        /// 明信片反面文件ID
        /// </summary>
        ///public string BackFileId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 裁切信息
        /// </summary>
        public CropInfo CropInfo { get; set; }

        /// <summary>
        /// 处理者名称
        /// </summary>
        public string ProcessorName { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public string ProcessStatus { get; set; }

        /// <summary>
        /// 处理状态码
        /// </summary>
        public int ProcessStatusCode { get; set; }


        public Image FrontImage {
            get {
                if (_frontImage == null)
                {
                    if (FileInfo != null && FileInfo.Exists)
                    {
                        try
                        {
                            _frontImage = Image.FromFile(FileInfo.FullName);
                        }
                        catch
                        {
                            return _frontImage = null;
                        }
                    }
                }
                return _frontImage;
            }
        }

        private Image _frontImage;

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