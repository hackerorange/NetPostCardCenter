using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static FileInfo DownloadFileByFileIdAsync(string fileId, FileInfo fileInfo, Action<FileInfo> success, Action<string> failure)
        {
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            NetGlobalInfo.RestTemplate.GetForObjectAsync<byte[]>("/file/{fileId}", new Dictionary<string, object>
            {
                {"fileId", fileId}
            }, result =>
            {
                if (result.Cancelled)
                {
                    failure?.Invoke("请求已取消！");
                    return;
                }

                if (result.Error != null)
                {
                    failure?.Invoke(result.Error.Message);
                }

                var fileStream = new BufferedStream(new FileStream(fileInfo.FullName, FileMode.CreateNew));
                fileStream.Write(result.Response, 0, result.Response.Length);
                fileStream.Flush();
                fileStream.Close();

                success?.Invoke(fileInfo);
            });

            return fileInfo;
        }

        public static FileInfo DownloadFileByFileId(string fileId, FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(new Uri(Properties.Settings.Default.Host + "/file/" + fileId), fileInfo.FullName);
                webClient.Dispose();
            }

            return fileInfo;
        }

        public static byte[] DownloadBytesByFileId(string fileId)
        {
            using (var webClient = new WebClient())
            {
                var downloadBytesByFileId = webClient.DownloadData(new Uri(Properties.Settings.Default.Host + "/file/" + fileId));
                webClient.Dispose();
                return downloadBytesByFileId;
            }
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


        public static FileUploadResponse UploadFile(this FileInfo file, string category)
        {
            var postForObject = NetGlobalInfo.RestTemplate.PostForObject<DataResponse<FileUploadResponse>>(
                "/file/upload",
                new Dictionary<string, object>
                {
                    {"file", new HttpEntity(file)},
                    {"category", Encoding.UTF8.GetBytes(category)}
                }
            );
            return postForObject.Data;
        }
    }
}