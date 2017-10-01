using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostCardCenter.constant
{
    public enum PostCardFileUploadStatusEnum
    {
        /// <summary>
        /// 文件还未上传
        /// </summary>
        BEFOREL_UPLOAD,
        /// <summary>
        /// 文件正在上传中
        /// </summary>
        UPLOADING,
        /// <summary>
        /// 文件已经上传
        /// </summary>
        AFTER_UPLOAD,
    }
}
