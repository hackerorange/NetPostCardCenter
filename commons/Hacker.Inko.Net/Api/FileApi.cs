using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Hacker.Inko.Net.Base;
using Hacker.Inko.Net.Base.Helper;
using postCardCenterSdk.response.file;
using Spring.Http;

namespace Hacker.Inko.Net.Api
{
    public static class FileApi
    {
        /// <summary>
        ///     根据文件ID，下载到指定路径
        /// </summary>
        /// <param name="file">要下载的文件ID</param>
        /// <param name="path">下载文件后的目标文件夹路径信息</param>
        /// <param name="success">成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">失败响应结果</param>
        public static void DownLoadFileByFileId(string file, DirectoryInfo path, Action<FileInfo> success, Action<int> process = null, Action<string> failure = null)
        {
            DownLoadFileByFileId(file, new FileInfo(path.FullName + "/" + file), success, process, failure);
        }


        /// <summary>
        ///     根据文件ID，下载到指定文件路径
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileInfo">下载文件后的目标文件信息</param>
        /// <param name="success">成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">失败响应结果</param>
        public static void DownLoadFileByFileId(string fileId, FileInfo fileInfo, Action<FileInfo> success, Action<int> process = null, Action<string> failure = null)
        {
            DownLoadFile("/file/" + fileId, fileInfo, success, process, failure);
        }


        /// <summary>
        ///     异步下载文件
        /// </summary>
        /// <param name="url">文件所在路径URL</param>
        /// <param name="fileInfo">文件下载路径信息</param>
        /// <param name="success">下载成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">下载失败响应信息</param>
        public static void DownLoadFile(string url, FileInfo fileInfo, Action<FileInfo> success, Action<int> process, Action<string> failure = null)
        {
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
                fileInfo.Directory.Create();
            if (fileInfo.Exists)
            {
                Console.WriteLine(@"文件本地已经存在");
                success?.Invoke(fileInfo);
                return;
            }

            var webClient = new WebClient();
            webClient.Headers.Add("Authorization", "Bearer " + NetGlobalInfo.AccessToken);
            //下载完成
            webClient.DownloadFileCompleted += (sender, e) =>
            {
                if (e.Error != null)
                    failure?.Invoke(e.Error.Message);
                else
                    success?.Invoke(fileInfo);
            };

            // 进度条
            webClient.DownloadProgressChanged += (sender, e) => { process?.Invoke(e.ProgressPercentage); };
            // 异步下载文件
            webClient.DownloadFileAsync(new Uri(url), fileInfo.FullName, fileInfo.Name);
        }

        public static byte[] DownloadBytesByFileId(string fileId)
        {
            return NetGlobalInfo.RestTemplate.GetForObject<byte[]>("/file/{fileId}", new Dictionary<string, object>
            {
                {"fileId", fileId}
            });
        }

        public static void UploadAsync(this FileInfo file, string category, Action<FileUploadResponse> success, Action<string> failure = null)
        {
            NetGlobalInfo.RestTemplate.PostForMessageAsync<DataResponse<FileUploadResponse>>(
                "/file/upload",
                new Dictionary<string, object>
                {
                    {"file", new HttpEntity(file)},
                    {"category", Encoding.UTF8.GetBytes(category)}
                },
                result => result.PrepareResponse(success, failure)
            );
        }
    }
}