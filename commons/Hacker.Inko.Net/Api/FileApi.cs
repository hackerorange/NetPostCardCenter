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
        public static FileInfo DownloadFileByFileId(string fileId, FileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            var fileBytes = NetGlobalInfo.RestTemplate.GetForObject<byte[]>("/file/{fileId}", new Dictionary<string, object>
            {
                {"fileId", fileId}
            });

            var fileStream = new BufferedStream(new FileStream(fileInfo.FullName, FileMode.CreateNew));
            fileStream.Write(fileBytes, 0, fileBytes.Length);
            fileStream.Flush();
            fileStream.Close();
            return fileInfo;
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