using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using postCardCenterSdk;
using soho.Properties;
using soho.web.response.file;
using Spring.Http;
using FileUploadResponse = soho.web.response.file.FileUploadResponse;

namespace soho.web
{
    public class FileApi:BaseApi
    {
        private static FileApi _webServiceInvoker;

        public static FileApi GetInstance()
        {
            return _webServiceInvoker ?? (_webServiceInvoker = new FileApi());
        }

        private FileApi()
        {
            
        }


        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="file">要上传的文件</param>
        /// <param name="success">上传成功的回调函数</param>
        /// <param name="failure">上传失败的回调函数</param>
        public void Upload(FileInfo file, string category, Success<FileUploadResponse> success, Failure failure = null)
        {
            var dictionary = new Dictionary<string, object>();
            var entity = new HttpEntity(file);
            dictionary.Add("file", entity);
            dictionary.Add("category", Encoding.UTF8.GetBytes(category));
            PostForObjectAsync<FileUploadResponse>(Resources.fileUploadUrl, dictionary, result => { success?.Invoke(result); }, failure);
        }
        
        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="file">要上传的文件</param>
        public FileUploadResponse Upload(FileInfo file, string category)
        {
            var dictionary = new Dictionary<string, object>();
            var entity = new HttpEntity(file);
            dictionary.Add("file", entity);
            dictionary.Add("category", Encoding.UTF8.GetBytes(category));
            FileUploadResponse tmpResult = null;
            PostForObject<FileUploadResponse>(Resources.fileUploadUrl, dictionary, result => { tmpResult = result; }, message => throw new Exception(message));
            return tmpResult;
        }
       
        /// <summary>
        /// 提交裁切结果
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="cropLeft">左侧裁切</param>
        /// <param name="cropTop">上面裁切</param>
        /// <param name="cropWidth">裁切宽度</param>
        /// <param name="cropHeight">裁切高度</param>
        /// <param name="rotation">旋转角度值</param>
        /// <param name="productWidth">成品的宽度</param>
        /// <param name="productHeight">成品的高度</param>
        /// <param name="style">裁切样式（A,B,C,D）</param>
        /// <param name="success">成功后，返回文件裁切后的文件ID</param>
        /// <param name="failure">失败后的逻辑</param>
        public void SubmitPostCardCropInfo(string fileId, double cropLeft, double cropTop, double cropWidth, double cropHeight, int rotation, int productWidth, int productHeight, string style, Success<string> success, Failure failure = null)
        {var request = new CropSubmitRequest
            {
                ProductHeight = productHeight,
                ProductWidth = productWidth,
                CropHeight = cropHeight,
                CropTop = cropTop,
                CropLeft = cropLeft,
                CropWidth = cropWidth,
                Rotation = rotation,
                FileId = fileId,
                Style = style
            };

            PostForObjectAsync<FileUploadResponse>(Resources.cropInfoSubmitUrl, request, resp => { success?.Invoke(resp.Id); }, failure);
        }
        
    }
}
