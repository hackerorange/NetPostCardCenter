using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;

namespace soho.domain
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PostCard
    {
        /// <summary>
        /// 明信片ID
        /// </summary>
        public string postCardId { get; set; }

        /// <summary>
        /// 此文件在文件服务器的ID
        /// </summary>
        public string fileId { get; set; }

        /// <summary>
        /// 明信片原始文件信息
        /// </summary>
        [JsonIgnore]
        public FileInfo fileInfo { get; set; }

        /// <summary>
        /// 此张明信片打印的份数
        /// </summary>
        public int copy { get; set; }

        /// <summary>
        /// 此张明信片的正面样式
        /// </summary>
        public string frontStyle { get; set; }

        /// <summary>
        /// 此张明信片的反面样式
        /// </summary>
        public string backStyle { get; set; }

        /// <summary>
        /// 文件上传状态，分为：未上传，正在上传，已上传，
        /// </summary>
        public string fileUploadStat { get; set; }

        /// <summary>
        /// 文件是否是图片文件，根据Header请求获取文件信息
        /// </summary>
        public bool isImage { get; set; }


        /// <summary>
        /// 明信片反面是否自定义
        /// </summary>
        public bool customerBackStyle { get; set; }

        /// <summary>
        /// 明信片反面文件信息
        /// </summary>
        [JsonIgnore]
        public FileInfo backFileInfo { get; set; }

        [JsonIgnore]
        private Image _image;

        [JsonIgnore]
        public Image imageBefore { 
            get{
                if (_image == null)
                {
                    _image = Image.FromFile(fileInfo.FullName);
                }
                return _image;
            }
        }

        /// <summary>
        /// 明信片反面文件ID
        /// </summary>
        public string backFileId { get; set; }

        public string fileName { get; set; }

        public CropInfo cropInfo { get; set; }

        public string processorName { get; set; }

        public string processStatus { get; set; }

        public string processStatusCode { get; set; }

        
    }
}