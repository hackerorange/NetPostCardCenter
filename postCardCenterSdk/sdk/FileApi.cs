

using postCardCenterSdk.helper;
using postCardCenterSdk.Properties;
using postCardCenterSdk.response;
using postCardCenterSdk.web.response.file;
using Spring.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace postCardCenterSdk.web
{
    public class FileApi : BaseApi
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
        public void UploadAsync(FileInfo file, string category, Action<FileUploadResponse> success, Action<string> failure = null)
        {
            _restTemplate.PostForObjectAsync<BodyResponse<FileUploadResponse>>(Resources.fileUploadUrl, new Dictionary<string, object>
            {
                { "file", new HttpEntity(file) },
                { "category", Encoding.UTF8.GetBytes(category) }
            }, resp => resp.PrepareResult(success, failure));
        }

        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="file">要上传的文件</param>
        /// <param name="success">上传成功的回调函数</param>
        /// <param name="failure">上传失败的回调函数</param>
        public void UploadSynchronize(FileInfo file, string category, Action<FileUploadResponse> success, Action<string> failure = null)
        {
            try
            {
                var result = _restTemplate.PostForObject<BodyResponse<FileUploadResponse>>(Resources.fileUploadUrl, new Dictionary<string, object>
                {
                    { "file", new HttpEntity(file) },
                    { "category", Encoding.UTF8.GetBytes(category) }
                });

                if (result.Code < 0)
                {
                    failure?.Invoke(result.Message);
                }
                else
                {
                    success?.Invoke(result.Data);
                }
            }
            catch (Exception e)
            {
                failure?.Invoke(e.Message);
            }


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
        public void SubmitPostCardCropInfo(string fileId, double cropLeft, double cropTop, double cropWidth, double cropHeight, int rotation, int productWidth, int productHeight, string style, Action<string> success, Action<string> failure = null)
        {
            var request = new CropSubmitRequest
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

            _restTemplate.PostForObjectAsync<BodyResponse<string>>(Resources.cropInfoSubmitUrl, request, resp => { resp.PrepareResult(success, failure); });
        }

    }
}
