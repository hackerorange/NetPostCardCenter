using postCardCenterSdk.sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static postCardCenterSdk.sdk.WebServiceInvoker;

namespace soho.helper
{
    public static class FileHelper
    {


        /// <summary>
        /// 向服务器上传文件
        /// </summary>
        /// <param name="file">要上传到服务器的文件</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        /// <returns>服务器返回的文件ID</returns>
        public static void Upload(this FileInfo file, Success<bool> success, Failure failure = null)
        {
            var md5 = file.getMd5();
            IsFileExistInServer(md5, isFileExist =>
            {
                if (isFileExist)
                {
                    success(true);
                }
                else
                {
                    Upload(file, success, failure);                    
                }
            }, failure: result =>
            {
                Upload(file, success, failure);
            });
        }

    }
}
