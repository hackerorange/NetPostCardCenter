using postCardCenterSdk.Properties;
using postCardCenterSdk.request.order;
using postCardCenterSdk.response;
using postCardCenterSdk.response.system;
using Spring.Http;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Drawing;

namespace postCardCenterSdk.sdk
{
    public class WebServiceInvoker
    {

        public static String Token { get; set; }

        /// <summary>
        /// 请求失败
        /// </summary>
        /// <param name="message">失败返回的消息</param>
        public delegate  void Failure(string message);

        /// <summary>
        /// 请求成功
        /// </summary>
        /// <typeparam name="T">成功返回的结果类型</typeparam>
        /// <param name="backStyleInfos">成功返回的结果</param>
        public delegate void Success<T>(T backStyleInfos);
        
        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="postCards">要提交的订单集合</param>
        /// <param name="success">成功返回的结果</param>
        /// <param name="failure">失败处理逻辑</param>
        public static void SubmitPostCardList(OrderSubmitRequest postCards, Success<String> success, Failure failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            if (Token == null)
            {
                failure?.Invoke("此操作需要登录，当前用户没有登录");
                return;
            }
            //添加请求头Token
            var httpHeaders = new HttpHeaders { { "tokenId", Token } };
            //
            var headers = new HttpEntity(postCards, httpHeaders);

            restTemplate.PostForObjectAsync<BodyResponse<Boolean>>(Resources.postCardSubmitUrl, headers, res =>
            {
                if (res.Error != null)
                {
                    failure?.Invoke(res.Error.Message);
                    return;
                }
                if (res.Response.Code > 0)
                {
                    success?.Invoke(res.Response.Message);
                }
                else
                {
                    failure?.Invoke(res.Response.Message);
                }
            });           
        }
        
        public static void GetBackStyleTemplateList( Success<List<BackStyleResponse>> success, Failure failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            
            restTemplate.GetForObjectAsync<BodyResponse<Page<BackStyleResponse>>>(Resources.backStyleListUrl, response =>
            {
                if (response.Error != null)
                {
                    failure?.Invoke(response.Error.Message);
                }
                else
                {
                    if (response.Response.Code > 0)
                    {
                        success?.Invoke(response.Response.Data.Detail);
                    }
                    else
                    {
                        failure?.Invoke(response.Response.Message);
                    }
                }
            });            
        }

       

        /// <summary>
        /// 从服务器端获取明信片尺寸信息
        /// </summary>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void GetProductSizeTemplateList(Success<List<PostCardSizeResponse>> success,Failure failure=null)
        {
            var list = new List<PostCardSizeResponse>
            {
                new PostCardSizeResponse
                {
                    Name = "标准尺寸",
                    Height = 100,
                    Width = 148
                },
                new PostCardSizeResponse
                {
                    Name = "大尺寸",
                    Height = 105,
                    Width = 160
                }
            };
            success?.Invoke(list);            
        }

        /// <summary>
        /// 从服务器获取正面板式
        /// </summary>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        public static void GetFrontStyleTemplateList(Success<List<FrontStyleResponse>> success,Failure failure = null)
        {
            var a = new List<FrontStyleResponse>
            {
                new FrontStyleResponse() {Name="A" },
                new FrontStyleResponse() {Name="B" },
                new FrontStyleResponse() {Name="C" },                
            };
            success?.Invoke(a);
        }

        /// <summary>
        /// 异步下载文件
        /// </summary>
        /// <param name="url">文件所在路径URL</param>
        /// <param name="fileInfo">文件下载路径信息</param>        
        /// <param name="success">下载成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">下载失败响应信息</param>
        public static void DownLoadFile(string url, FileInfo fileInfo, Success<FileInfo> success,Success<int> process,Failure failure=null)
        {
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            };                        
            if (fileInfo.Exists)
            {
                Console.WriteLine(@"文件本地已经存在");
                success?.Invoke(fileInfo);
            }
            var webClient = new WebClient();
            //下载完成
            webClient.DownloadFileCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    failure?.Invoke(e.Error.Message);
                }
                else
                {
                    success?.Invoke(fileInfo);
                }
            };
            // 进度条
            webClient.DownloadProgressChanged += (sender, e) =>
            {
                process?.Invoke(e.ProgressPercentage);
            };
            // 异步下载文件
            webClient.DownloadFileAsync(new Uri(url), fileInfo.FullName, fileInfo.Name);
        }

        /// <summary>
        /// 根据文件ID，下载到指定路径
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="path">下载文件后的目标文件夹路径信息</param>
        /// <param name="success">成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">失败响应结果</param>
        public static void DownLoadFileByFileId(string file,DirectoryInfo path,Success<FileInfo> success, Success<int> process,Failure failure)
        {
            DownLoadFileByFileId(file, new FileInfo(path.FullName + "/" + file), success,process, failure);
        }

        /// <summary>
        /// 根据文件ID，下载到默认路径
        /// </summary>
        /// <param name="fileId"></param>        
        /// <param name="success">成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">失败响应结果</param>
        public static void DownLoadFileByFileId(string fileId, Success<FileInfo> success, Success<int> process=null, Failure failure=null)
        {
            DirectoryInfo path = new DirectoryInfo(Environment.CurrentDirectory + "/tmp");
            if (!path.Exists)
            {
                path.Create();
            }
            DownLoadFileByFileId(fileId, path, success, process, failure);
        }

        /// <summary>
        /// 根据文件ID，下载到指定文件路径
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileInfo">下载文件后的目标文件信息</param>
        /// <param name="success">成功响应结果</param>
        /// <param name="process">进度条</param>
        /// <param name="failure">失败响应结果</param>
        public static void DownLoadFileByFileId(string fileId, FileInfo fileInfo, Success<FileInfo> success, Success<int> process = null, Failure failure = null)
        {
            
            DownLoadFile(Resources.fileDownloadUrl + "/" + fileId, fileInfo, success, process, failure);            
        }
        
        /// <summary>
        /// 询问服务器，判断文件是否为图片文件
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static bool IsImageFile(string fileId)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            var headForHeaders = restTemplate.HeadForHeaders(Resources.fileInfoUrl + "/" + fileId);
            var singleValue = headForHeaders.GetSingleValue("isImage");
            return !string.IsNullOrWhiteSpace(singleValue) && singleValue.ToUpper().Equals("TRUE");
        }

        /// <summary>
        /// 询问服务器，文件是否存在
        /// </summary>
        /// <param name="fileId">要询问的文件信息</param>
        /// <param name="success">成功回调函数</param>
        /// <param name="failure">失败回调函数</param>
        /// <returns>文件是否已经存在</returns>
        public static void IsFileExistInServer(string fileId, Success<bool> success, Failure failure = null)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            restTemplate.HeadForHeadersAsync(Resources.fileInfoUrl + "/" + fileId, resp =>
            {
                if (resp.Error != null)
                {
                    failure?.Invoke(resp.Error.Message);
                }
                else
                {
                    var singleValue = resp.Response.GetSingleValue("isExist");
                    success?.Invoke(!string.IsNullOrWhiteSpace(singleValue) && singleValue.ToUpper().Equals("TRUE"));
                }
            });
        }


        public static void Upload(FileInfo file,Success<bool> success,Failure failure)
        {
            var restTemplate = new RestTemplate();
            restTemplate.MessageConverters.Add(new NJsonHttpMessageConverter());
            var dictionary = new Dictionary<string, object>();
            var entity = new HttpEntity(file);
            dictionary.Add("file", entity);
            restTemplate.PostForObjectAsync<BodyResponse<bool>>(Resources.fileUploadUrl, dictionary,
                resp =>
                {
                    if (resp.Error != null)
                    {
                        failure?.Invoke(resp.Error.Message);
                    }
                    else
                    {
                        success?.Invoke(resp.Response.Code > 0);
                    }
                });
        }

    }


}
