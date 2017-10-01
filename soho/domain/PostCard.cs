using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;

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
        public string BackStyle { get; set; }

        /// <summary>
        /// 文件上传状态，分为：未上传，正在上传，已上传，
        /// </summary>
        public string FileUploadStat { get; set; }

        /// <summary>
        /// 文件是否是图片文件，根据Header请求获取文件信息
        /// </summary>
        public bool IsImage { get; set; }


        /// <summary>
        /// 明信片反面是否自定义
        /// </summary>
        public bool CustomerBackStyle { get; set; }

        /// <summary>
        /// 明信片反面文件信息
        /// </summary>
        public FileInfo BackFileInfo { get; set; }

        //private Image _image;

        //public Image ImageBefore
        //{
        //    get
        //    {
        //        if (_image == null)
        //        {
        //            _image = Image.FromFile(FileInfo.FullName);
        //        }
        //        return _image;
        //    }
        //}

        /// <summary>
        /// 明信片反面文件ID
        /// </summary>
        public string BackFileId { get; set; }

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
    }
}