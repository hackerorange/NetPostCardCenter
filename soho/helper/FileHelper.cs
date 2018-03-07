using System;
using System.Drawing;
using System.IO;
using postCardCenterSdk;
using soho.model;
using WebServiceInvoker = postCardCenterSdk.sdk.WebServiceInvoker;

namespace soho.helper
{
    public static class FileHelper
    {
        public static WebServiceInvoker WebServiceInvoker = new WebServiceInvoker("http://localhost:8089");

        /// <summary>
        ///     向服务器上传文件
        /// </summary>
        /// <param name="file">要上传到服务器的文件</param>
        /// <param name="category">分类</param>
        /// <param name="synchronize">是否同步，同步为True</param>
        /// <param name="cropHeight"></param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        /// <param name="rotation"></param>
        /// <param name="cropLeft"></param>
        /// <param name="cropTop"></param>
        /// <param name="cropWidth"></param>
        /// <returns>服务器返回的文件ID</returns>
        public static void Upload(this FileInfo file, bool synchronize, string category, double rotation = 0, double cropLeft = 0, double cropTop = 0, double cropWidth = 1, double cropHeight = 1, Success<ImageFileUploadModel> success = null,
            Failure failure = null)
        {
            if (synchronize)
                try
                {
                    var fileUploadResponse = WebServiceInvoker.Upload(file, category, rotation, cropLeft, cropTop, cropWidth, cropHeight);

                    success?.Invoke(new ImageFileUploadModel
                    {
                        FileId = fileUploadResponse.Id,
                        ThumbnailFileId = fileUploadResponse.ThumbnailFileId,
                        ImageAvailable = fileUploadResponse.ImageAvailable
                    });
                }
                catch (Exception e)
                {
                    failure?.Invoke(e.Message);
                }
            else
                WebServiceInvoker.Upload(file, category, rotation, cropLeft, cropTop, cropWidth, cropHeight, succ =>
                {
                    success?.Invoke(new ImageFileUploadModel
                    {
                        FileId = succ.Id,
                        ThumbnailFileId = succ.ThumbnailFileId,
                        ImageAvailable = succ.ImageAvailable
                    });
                }, message => { failure?.Invoke(message); });
        }

        /// <summary>
        ///     检测文件是否为图像
        /// </summary>
        /// <param name="file">要检测的文件</param>
        /// <returns></returns>
        public static bool CheckIsImage(this FileInfo file)
        {
            //如果文件不存在，返回false;
            if (!file.Exists) return false;
            //创建文件流
            var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            try
            {
                //根据流创建图像
                var iamge = Image.FromStream(fileStream);
                //创建成功，返回true;
                return true;
            }
            catch
            {
                //创建失败，返回false;
                return false;
            }
            finally
            {
                //关闭流
                fileStream.Close();
            }
        }
    }
}