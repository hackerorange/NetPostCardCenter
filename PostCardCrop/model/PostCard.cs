using System.Diagnostics.CodeAnalysis;
using System.IO;
using SystemSetting.backStyle.model;
using Hacker.Inko.PostCard.Library;
using postCardCenterSdk.constant.postcard;

namespace PostCardCrop.model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PostCardInfo
    {
        public PostCardInfo()
        {
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
        ///     此文件在文件服务器的ID
        /// </summary>
        public string ProductFileId { get; set; }

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
        ///     明信片反面文件ID
        /// </summary>
        /// public string BackFileId { get; set; }
        /// <summary>
        ///     文件名称
        /// </summary>
        public string FileName { get; set; }


        /// <summary>
        ///     处理者名称
        /// </summary>
        public string ProcessorName { get; set; }

        /// <summary>
        ///     处理状态显示文本
        /// </summary>
        public string ProcessStatusText { get; set; }


        /// <summary>
        ///     成品尺寸（裁切页面使用）
        /// </summary>
        public PostSize ProductSize { get; set; }

        /// <summary>
        ///     当前处理状态枚举
        /// </summary>
        public PostCardProcessStatusEnum ProcessStatus { get; set; }
    }
}